using System;
using System.Net;
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
    public class ZMapResult
    {
        public int Width;
        public int Height;
        public ushort[] ZData;
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
                _zmapResult = new ZMapResult { Width = width, Height = height, ZData = zData };
                _zmapDone.Set();
            };
            _sensor.OnZilImage += _zmapHandler;

            log?.Invoke("SetLaserPower(true)");
            _sensor.SetLaserPower(true);

            log?.Invoke("StartAcquisition (레이저 ON, 이동 중 계속 촬영)");
            _sensor.StartAcquisition();
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
        public void StopZMapCapture()
        {
            if (_sensor == null)
                return;

            _sensor.SetLaserPower(false);
            _sensor.StopAcquisition();

            if (_zmapHandler != null)
            {
                _sensor.OnZilImage -= _zmapHandler;
                _zmapHandler = null;
            }
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
