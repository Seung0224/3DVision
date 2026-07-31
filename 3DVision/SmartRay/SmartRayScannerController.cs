using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using SmartRay.Api;

namespace _3DVision.SmartRay
{
    public class GrabResult
    {
        public Point3F[] Points;
        public uint Width;
        public uint Height;
    }

    // ZMap 방식으로 받은 원본 데이터. zData는 보정 전 raw 값(0 = 무효)이며, 실제 mm 단위 변환은 하지 않는다.
    // IntensityData는 같은 촬영에서 함께 나오는 밝기(카메라가 보는 반사광 세기) 채널이다.
    public class ZMapResult
    {
        public int Width;
        public int Height;
        public ushort[] ZData;
        public ushort[] IntensityData;
    }

    // Live Image 방식으로 받은 원본 카메라 이미지. 3D로 계산되기 전, 카메라가 실제로 보는 레이저 라인 그대로다.
    public class LiveImageFrame
    {
        public int Width;
        public int Height;
        public byte[] Data;
    }

    // SmartRay ECCO 스캐너와의 통신을 담당하는 클래스.
    // 2단계-A: SDK 초기화, 장치 검색(Discovery), 연결/해제, 레이저/촬영 없는 장비 정보 조회.
    // 2단계-B: 레이저를 켜고 단일 스냅샷을 촬영한다 (FreeRunning, 트리거 없이 즉시 촬영).
    //   - PointCloud 방식(GrabSingle)과 ZMap 방식(GrabZMap) 둘 다 지원한다.
    public class SmartRayScannerController : IDisposable
    {
        private static bool _apiInitialized;

        private Sensor _sensor;
        private bool _calibrationLoaded;

        private ManualResetEventSlim _zmapDone;
        private ZMapResult _zmapResult;
        private Sensor.OnZilImageDelegate _zmapHandler;
        private bool _zmapActive;

        private Sensor.OnLiveImageDelegate _liveImageHandler;
        private Action<LiveImageFrame> _liveImageCallback;
        private Stopwatch _liveFrameStopwatch;
        private bool _liveActive;

        public bool IsConnected => _sensor != null && _sensor.IsConnectionEstablished;

        private static void EnsureApiInitialized()
        {
            if (_apiInitialized)
                return;

            ApiManager.Initialize();
            _apiInitialized = true;
        }

        // 네트워크에서 응답하는 SmartRay 센서 목록을 검색한다. 연결/촬영은 하지 않는다.
        public DetectedSensor[] DiscoverSensors()
        {
            EnsureApiInitialized();

            DetectedSensorList list = Discovery.GetDetectedSensorList();
            var result = new DetectedSensor[list.Length];
            for (int i = 0; i < list.Length; i++)
                result[i] = list[i];

            return result;
        }

        public void Connect(string ipAddress, int port, int timeoutMilliseconds)
        {
            if (IsConnected)
                return;

            EnsureApiInitialized();

            var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            _sensor = new Sensor();
            _sensor.Connect(endPoint, TimeSpan.FromMilliseconds(timeoutMilliseconds));
        }

        public void Disconnect()
        {
            if (_sensor == null)
                return;

            _sensor.Disconnect();
            _sensor.Dispose();
            _sensor = null;
            _calibrationLoaded = false;
            _zmapActive = false;
            _liveActive = false;
        }

        // 레이저/촬영과 무관한, 연결 확인용 장비 정보만 읽어온다.
        public (string ModelName, string SerialNumber, string FirmwareVersion) GetSensorInfo()
        {
            if (!IsConnected)
                throw new InvalidOperationException("스캐너에 연결되어 있지 않습니다.");

            return (_sensor.ModelName, _sensor.SerialNumber, _sensor.FirmwareVersion);
        }

        // 캘리브레이션 로드는 네트워크 왕복이 있어 매번 반복하면 느려지므로, 연결이 끊기기 전까지는 한 번만 실행한다.
        // 촬영 타입(PointCloud/ZMap)은 그랩마다 다를 수 있어 각 그랩 메서드에서 매번 명시적으로 설정한다.
        private void EnsureCalibrationLoaded(Action<string> log)
        {
            if (_calibrationLoaded)
                return;

            log?.Invoke("IsCalibrationDataAvailableOnSensor");
            if (_sensor.IsCalibrationDataAvailableOnSensor())
            {
                log?.Invoke("LoadCalibrationDataFromSensor");
                _sensor.LoadCalibrationDataFromSensor();
            }
            else
            {
                log?.Invoke("경고: 센서에 캘리브레이션 데이터가 없습니다.");
            }

            _calibrationLoaded = true;
        }

        // 레이저를 켜고 단일 스냅샷을 촬영해 포인트클라우드를 받아온다. (모터 동기화 없이 소프트웨어 명령만으로 즉시 촬영)
        // 완료(성공/실패 모두) 후에는 반드시 레이저를 끈다.
        // log는 각 설정 단계를 실행 직전에 알려주는 콜백으로, 어느 단계에서 실패했는지 진단하는 용도다.
        public GrabResult GrabSingle(int profileCount, int timeoutMilliseconds, Action<string> log = null)
        {
            if (!IsConnected)
                throw new InvalidOperationException("스캐너에 연결되어 있지 않습니다.");

            GrabResult result = null;
            var done = new ManualResetEventSlim(false);

            Sensor.OnPointCloudImageDelegate handler = (sensor, dataType, width, height, points, intensity, reflectance, metaData) =>
            {
                result = new GrabResult { Points = points, Width = width, Height = height };
                done.Set();
            };

            _sensor.OnPointCloudImage += handler;
            try
            {
                log?.Invoke("SetImageAcquisitionType(PointCloud)");
                _sensor.SetImageAcquisitionType(ImageAcquisitionType.PointCloud);

                log?.Invoke("SetAcquisitionImageTypeFlags(PointCloudP)");
                _sensor.SetAcquisitionImageTypeFlags(AcquisitionImageTypeFlags.PointCloudP);

                EnsureCalibrationLoaded(log);

                log?.Invoke("SetStartTrigger(None)");
                _sensor.SetStartTrigger(StartTriggerSource.None, false, TriggerEdgeMode.RisingEdge);

                log?.Invoke("SetDataTriggerMode(FreeRunning)");
                _sensor.SetDataTriggerMode(DataTriggerMode.FreeRunning);

                log?.Invoke("SetAcquisitionMode(Snapshot)");
                _sensor.SetAcquisitionMode(AcquisitionMode.Snapshot);

                log?.Invoke("SetNumberOfProfilesToCapture");
                _sensor.SetNumberOfProfilesToCapture((uint)profileCount);

                log?.Invoke("SetLaserPower(true)");
                _sensor.SetLaserPower(true);
                try
                {
                    log?.Invoke("StartAcquisition");
                    _sensor.StartAcquisition();

                    log?.Invoke("이벤트 대기 중...");
                    if (!done.Wait(timeoutMilliseconds))
                        throw new TimeoutException("지정한 시간 안에 스캔 데이터를 받지 못했습니다.");
                }
                finally
                {
                    _sensor.SetLaserPower(false);
                    _sensor.StopAcquisition();
                }
            }
            finally
            {
                _sensor.OnPointCloudImage -= handler;
            }

            return result;
        }

        // 레이저를 켜고 ZMap 촬영 대기 상태로 만든 뒤 즉시 반환한다 (블로킹하지 않음).
        // 호출 직후 ACS 이동을 시작하면 "이동하는 내내 레이저가 켜져 있는" 상태로 촬영할 수 있다.
        // 반드시 WaitForZMapCapture로 결과를 받고, 성공/실패와 관계없이 StopZMapCapture를 호출해야 한다.
        public void ArmZMapCapture(int profileCount, Action<string> log = null)
        {
            if (!IsConnected)
                throw new InvalidOperationException("스캐너에 연결되어 있지 않습니다.");

            // 센서는 한 번에 하나의 촬영 모드만 가능하다. Live 화면이 켜진 채로 ZMap 모드로 전환을 시도하면
            // 센서가 설정 변경 명령(캘리브레이션 로드 등)을 처리하지 못하고 멈춰버리는 걸 실제로 확인했다.
            if (_liveActive)
                throw new InvalidOperationException("Live 화면이 켜져 있는 동안은 촬영을 시작할 수 없습니다. 먼저 Live를 정지하세요.");

            log?.Invoke("SetImageAcquisitionType(ZMapIntensityLaserLineThickness)");
            _sensor.SetImageAcquisitionType(ImageAcquisitionType.ZMapIntensityLaserLineThickness);

            EnsureCalibrationLoaded(log);

            // ZMap은 PointCloud와 달리 ROI가 좁게(또는 안 맞게) 남아있으면 데이터를 전혀 못 만든다.
            // Jastech도 Live/그랩 시작 전에 항상 센서 전체 범위로 ROI를 명시적으로 맞춘다.
            log?.Invoke(string.Format("SetROI(0, {0}, 0, {1})", _sensor.MaxDimensions.Width, _sensor.MaxDimensions.Height));
            _sensor.SetROI(0, (int)_sensor.MaxDimensions.Width, 0, (int)_sensor.MaxDimensions.Height);

            log?.Invoke("SetStartTrigger(None)");
            _sensor.SetStartTrigger(StartTriggerSource.None, false, TriggerEdgeMode.RisingEdge);

            log?.Invoke("SetDataTriggerMode(FreeRunning)");
            _sensor.SetDataTriggerMode(DataTriggerMode.FreeRunning);

            log?.Invoke("SetAcquisitionMode(Snapshot)");
            _sensor.SetAcquisitionMode(AcquisitionMode.Snapshot);

            log?.Invoke("SetNumberOfProfilesToCapture");
            _sensor.SetNumberOfProfilesToCapture((uint)profileCount);

            _zmapDone = new ManualResetEventSlim(false);
            _zmapResult = null;

            _zmapHandler = (sensor, dataType, height, width, verticalRes, horizontalRes, zData, intensityData, laserLineData, originY) =>
            {
                _zmapResult = new ZMapResult { Width = width, Height = height, ZData = zData, IntensityData = intensityData };
                _zmapDone.Set();
            };
            _sensor.OnZilImage += _zmapHandler;

            log?.Invoke("SetLaserPower(true)");
            _sensor.SetLaserPower(true);

            log?.Invoke("StartAcquisition (레이저 ON, 이동 중 계속 촬영)");
            _sensor.StartAcquisition();

            _zmapActive = true;
        }

        // ArmZMapCapture 이후 호출. 요청한 프로파일 수만큼 데이터가 다 모일 때까지 대기한다.
        // 시간 안에 못 받으면 null을 반환한다 (예외를 던지지 않음 - 호출자가 StopZMapCapture를 반드시 부르게 하기 위함).
        public ZMapResult WaitForZMapCapture(int timeoutMilliseconds)
        {
            if (_zmapDone == null)
                throw new InvalidOperationException("ArmZMapCapture를 먼저 호출해야 합니다.");

            return _zmapDone.Wait(timeoutMilliseconds) ? _zmapResult : null;
        }

        // 레이저를 끄고 촬영을 정지한다. Arm 이후에는 성공/실패/타임아웃 여부와 관계없이 항상 호출해야 한다.
        // ArmZMapCapture로 실제 시작한 적이 없는데(=_zmapActive가 false인데) 호출해도 안전하다 - 이 경우
        // StopAcquisition()을 부르지 않는다. 돌고 있지 않은 상태에서 정지시키려 하면 SmartRay SDK가
        // "-5 Function was not successful" 예외를 던지기 때문이다 (실제로 종료 시 이 문제를 겪었음).
        public void StopZMapCapture()
        {
            bool wasActive = _zmapActive;
            _zmapActive = false;

            if (_sensor == null)
                return;

            if (wasActive)
            {
                _sensor.SetLaserPower(false);
                _sensor.StopAcquisition();
            }

            if (_zmapHandler != null)
            {
                _sensor.OnZilImage -= _zmapHandler;
                _zmapHandler = null;
            }
        }

        // 레이저를 켜고, 3D 계산 전 카메라 원본 이미지(Live Image)를 계속 스트리밍 받는다 (블로킹하지 않음).
        // 프레임이 도착할 때마다 onFrame 콜백이 호출된다 (백그라운드 스레드에서 호출되므로, UI 갱신 시 Invoke 필요).
        // 데이터 갱신 주기가 너무 짧으면 UI 갱신 부하가 커지므로, 최소 80ms 간격으로만 콜백을 호출한다.
        // 반드시 StopLiveView로 정지해야 한다 (레이저를 끄기 위함). ZMap 촬영 중에는 켤 수 없다 (센서가 한 번에
        // 하나의 모드만 가능하기 때문 - ArmZMapCapture 주석 참고).
        public void StartLiveView(Action<LiveImageFrame> onFrame, Action<string> log = null)
        {
            if (!IsConnected)
                throw new InvalidOperationException("스캐너에 연결되어 있지 않습니다.");

            if (_zmapActive)
                throw new InvalidOperationException("촬영이 진행 중인 동안은 Live 화면을 켤 수 없습니다. 먼저 촬상을 정지하세요.");

            log?.Invoke("SetImageAcquisitionType(LiveImage)");
            _sensor.SetImageAcquisitionType(ImageAcquisitionType.LiveImage);

            log?.Invoke(string.Format("SetROI(0, {0}, 0, {1})", _sensor.MaxDimensions.Width, _sensor.MaxDimensions.Height));
            _sensor.SetROI(0, (int)_sensor.MaxDimensions.Width, 0, (int)_sensor.MaxDimensions.Height);

            log?.Invoke("SetStartTrigger(None)");
            _sensor.SetStartTrigger(StartTriggerSource.None, false, TriggerEdgeMode.RisingEdge);

            log?.Invoke("SetDataTriggerMode(FreeRunning)");
            _sensor.SetDataTriggerMode(DataTriggerMode.FreeRunning);

            _liveImageCallback = onFrame;
            _liveFrameStopwatch = Stopwatch.StartNew();

            _liveImageHandler = (sensor, originX, width, height, imageDataPtr) =>
            {
                if (_liveFrameStopwatch.ElapsedMilliseconds < 80)
                    return;
                _liveFrameStopwatch.Restart();

                var data = new byte[width * height];
                Marshal.Copy(imageDataPtr, data, 0, width * height);
                _liveImageCallback?.Invoke(new LiveImageFrame { Width = width, Height = height, Data = data });
            };
            _sensor.OnLiveImage += _liveImageHandler;

            log?.Invoke("SetLaserPower(true)");
            _sensor.SetLaserPower(true);

            log?.Invoke("StartAcquisition (Live)");
            _sensor.StartAcquisition();

            _liveActive = true;
        }

        // 레이저를 끄고 Live 스트리밍을 정지한다. StartLiveView 이후에는 반드시 호출해야 한다.
        // StartLiveView로 실제 시작한 적이 없는데(=_liveActive가 false인데) 호출해도 안전하다 - StopZMapCapture와
        // 같은 이유로, 돌고 있지 않으면 StopAcquisition()을 부르지 않는다.
        public void StopLiveView()
        {
            bool wasActive = _liveActive;
            _liveActive = false;

            if (_sensor == null)
                return;

            if (wasActive)
            {
                _sensor.SetLaserPower(false);
                _sensor.StopAcquisition();
            }

            if (_liveImageHandler != null)
            {
                _sensor.OnLiveImage -= _liveImageHandler;
                _liveImageHandler = null;
            }
            _liveImageCallback = null;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
