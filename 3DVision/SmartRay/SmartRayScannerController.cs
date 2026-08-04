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

        // OnZilImage는 한 번에 전체 스캔 데이터를 주지 않고, 촬영 도중 여러 번에 걸쳐 조각(chunk) 단위로 호출된다.
        // 그래서 콜백이 올 때마다 이어붙여서 하나의 큰 이미지로 누적해야 한다 (Jastech Framework의
        // ZMapImageData.AddImageData와 같은 방식 - 매번 처음 도착한 조각만 쓰면 화면에 얇은 띠만 남는다).
        private int _zmapAccumulatedWidth;
        private int _zmapAccumulatedRows;
        private int _zmapTargetRows;
        private ushort[] _zmapAccumulatedZData;
        private ushort[] _zmapAccumulatedIntensityData;
        // 마지막으로 조각(chunk)이 도착한 시점부터 경과 시간. 외부 트리거 촬영에서, 트리거가 한동안
        // 끊기면(=ACS가 이번 패스를 끝냈다고 추정) 그 시점까지 쌓인 데이터로 끝내는 데 사용한다.
        private Stopwatch _zmapLastChunkStopwatch;

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

        // ArmZMapCapture/ArmZMapCaptureExternalTrigger가 공통으로 쓰는 OnZilImage 핸들러를 만든다.
        // 매번 도착하는 조각을 요청한 profileCount 크기의 버퍼에 순서대로 이어붙이고, 버퍼가 다 채워지면
        // (또는 조각이 더 이상 오지 않아 타임아웃될 때까지) 누적된 전체 이미지를 결과로 확정한다.
        private Sensor.OnZilImageDelegate CreateAccumulatingZilHandler(int profileCount)
        {
            _zmapAccumulatedWidth = 0;
            _zmapAccumulatedRows = 0;
            _zmapTargetRows = profileCount;
            _zmapAccumulatedZData = null;
            _zmapAccumulatedIntensityData = null;
            _zmapLastChunkStopwatch = Stopwatch.StartNew();

            return (sensor, dataType, height, width, verticalRes, horizontalRes, zData, intensityData, laserLineData, originY) =>
            {
                _zmapLastChunkStopwatch.Restart();

                if (_zmapAccumulatedZData == null)
                {
                    _zmapAccumulatedWidth = width;
                    _zmapAccumulatedZData = new ushort[width * _zmapTargetRows];
                    _zmapAccumulatedIntensityData = new ushort[width * _zmapTargetRows];
                }

                int readRows = Math.Min(_zmapTargetRows - _zmapAccumulatedRows, height);
                if (readRows > 0)
                {
                    int destOffset = _zmapAccumulatedWidth * _zmapAccumulatedRows;
                    int count = _zmapAccumulatedWidth * readRows;
                    Array.Copy(zData, 0, _zmapAccumulatedZData, destOffset, Math.Min(count, zData.Length));
                    Array.Copy(intensityData, 0, _zmapAccumulatedIntensityData, destOffset, Math.Min(count, intensityData.Length));
                    _zmapAccumulatedRows += readRows;
                }

                if (_zmapAccumulatedRows >= _zmapTargetRows)
                {
                    _zmapResult = new ZMapResult
                    {
                        Width = _zmapAccumulatedWidth,
                        Height = _zmapAccumulatedRows,
                        ZData = _zmapAccumulatedZData,
                        IntensityData = _zmapAccumulatedIntensityData
                    };
                    _zmapDone.Set();
                }
            };
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

            _zmapHandler = CreateAccumulatingZilHandler(profileCount);
            _sensor.OnZilImage += _zmapHandler;

            log?.Invoke("SetLaserPower(true)");
            _sensor.SetLaserPower(true);

            log?.Invoke("StartAcquisition (레이저 ON, 이동 중 계속 촬영)");
            _sensor.StartAcquisition();

            _zmapActive = true;
        }

        // 실제 production Jastech.3DVision이 이 센서(Cancap Right)에 쓰는 값이다 (D:\Config\SmartRaySensorConfig.cfg의
        // Roi, D:\Config\SmartRay Setting\Cancap Right.json의 dataTrigger.externalTriggerParameters에서 그대로 가져옴).
        // 다른 센서/장비에 맞추려면 해당 설정 파일을 다시 확인해서 아래 값을 갱신해야 한다.
        private const int ProductionRoiOriginX = 320;
        private const int ProductionRoiWidth = 704;
        private const int ProductionRoiOriginY = 350;
        private const int ProductionRoiHeight = 304;
        private const int ProductionTriggerDivider = 2;
        private const int ProductionTriggerDelay = 0;

        // ArmZMapCapture와 동일하지만, 소프트웨어가 촬영 속도를 정하지 않고 Input1으로 들어오는 외부 트리거
        // 펄스 하나당 한 라인씩 촬영한다 (DataTriggerMode.External + DataTriggerSource.Input1).
        // ACS 등 외부 장비가 이동하며 계속 펄스를 보내는 동안 자동으로 라인이 쌓이고, 요청한 profileCount만큼
        // 다 모이면 OnZilImage가 발생한다. ACS의 왕복동작(반복동작)처럼 여러 패스에 걸쳐 계속 촬영하려면,
        // WaitForZMapCapture로 한 패스 결과를 받고 StopZMapCapture로 정리한 뒤 이 메서드를 다시 호출해 재무장하면 된다.
        public void ArmZMapCaptureExternalTrigger(int profileCount, Action<string> log = null)
        {
            if (!IsConnected)
                throw new InvalidOperationException("스캐너에 연결되어 있지 않습니다.");

            if (_liveActive)
                throw new InvalidOperationException("Live 화면이 켜져 있는 동안은 촬영을 시작할 수 없습니다. 먼저 Live를 정지하세요.");

            log?.Invoke("SetImageAcquisitionType(ZMapIntensityLaserLineThickness)");
            _sensor.SetImageAcquisitionType(ImageAcquisitionType.ZMapIntensityLaserLineThickness);

            EnsureCalibrationLoaded(log);

            log?.Invoke(string.Format("SetROI({0}, {1}, {2}, {3})", ProductionRoiOriginX, ProductionRoiWidth, ProductionRoiOriginY, ProductionRoiHeight));
            _sensor.SetROI(ProductionRoiOriginX, ProductionRoiWidth, ProductionRoiOriginY, ProductionRoiHeight);

            log?.Invoke("SetStartTrigger(None)");
            _sensor.SetStartTrigger(StartTriggerSource.None, false, TriggerEdgeMode.RisingEdge);

            log?.Invoke("SetDataTriggerMode(External)");
            _sensor.SetDataTriggerMode(DataTriggerMode.External);

            log?.Invoke("SetDataTriggerExternalTriggerSource(Input1)");
            _sensor.SetDataTriggerExternalTriggerSource(DataTriggerSource.Input1);

            log?.Invoke(string.Format("SetDataTriggerExternalTriggerParameters(divider={0}, delay={1}, edge=Both)", ProductionTriggerDivider, ProductionTriggerDelay));
            _sensor.SetDataTriggerExternalTriggerParameters(ProductionTriggerDivider, ProductionTriggerDelay, TriggerEdgeMode.Both);

            log?.Invoke("SetAcquisitionMode(Snapshot)");
            _sensor.SetAcquisitionMode(AcquisitionMode.Snapshot);

            log?.Invoke("SetNumberOfProfilesToCapture");
            _sensor.SetNumberOfProfilesToCapture((uint)profileCount);

            _zmapDone = new ManualResetEventSlim(false);
            _zmapResult = null;

            _zmapHandler = CreateAccumulatingZilHandler(profileCount);
            _sensor.OnZilImage += _zmapHandler;

            log?.Invoke("SetLaserPower(true)");
            _sensor.SetLaserPower(true);

            log?.Invoke("StartAcquisition (Input1 외부 트리거 대기)");
            _sensor.StartAcquisition();

            _zmapActive = true;
        }

        // ArmZMapCapture 이후 호출. 요청한 프로파일 수만큼 데이터가 다 모일 때까지 대기한다.
        // ArmZMapCapture(FreeRunning)는 "이동 시간보다 넉넉하게" profileCount를 크게 잡아두는 방식이라
        // 실제로는 다 채우지 못하고 시간이 끝나는 경우가 흔하다 - 이 경우 지금까지 쌓인 데이터만큼만 잘라서
        // 반환한다 (한 줄도 못 받았으면 null을 반환한다. 예외는 던지지 않는다 - 호출자가 StopZMapCapture를
        // 반드시 부르게 하기 위함).
        public ZMapResult WaitForZMapCapture(int timeoutMilliseconds)
        {
            if (_zmapDone == null)
                throw new InvalidOperationException("ArmZMapCapture를 먼저 호출해야 합니다.");

            if (_zmapDone.Wait(timeoutMilliseconds))
                return _zmapResult;

            return _zmapAccumulatedRows > 0 ? TrimAccumulatedResult() : null;
        }

        // 호출자가 (ACS 이동 완료 신호 등) 외부 근거로 "이번 패스는 여기까지"라고 이미 판단했을 때 쓴다.
        // 트리거 침묵을 기다리지 않고, 지금까지 쌓인 데이터를 그 즉시 잘라서 반환한다 (목표 라인 수를
        // 이미 다 채워서 _zmapDone이 set된 상태라면 그 결과를 그대로 반환한다).
        public ZMapResult FinishCaptureNow()
        {
            if (_zmapDone != null && _zmapDone.IsSet)
                return _zmapResult;

            return _zmapAccumulatedRows > 0 ? TrimAccumulatedResult() : null;
        }

        // ArmZMapCaptureExternalTrigger 전용 대기. 실제 한 패스에 트리거가 몇 번 들어올지는 미리 알 수 없으므로
        // (ArmZMapCapture처럼 profileCount를 넉넉하게 잡아도 정확히 다 채워진다는 보장이 없다), 요청한 라인 수를
        // 다 채우면 즉시 끝나고, 그렇지 않더라도 트리거가 idleTimeoutMilliseconds 동안 끊기면(=ACS가 이번
        // 패스를 끝내고 방향을 바꾸는 중이라고 추정) 그때까지 쌓인 데이터로 끝낸다. overallTimeoutMilliseconds는
        // 트리거가 한 번도 안 들어와 계속 대기만 하는 상황을 막는 최종 안전장치다.
        public ZMapResult WaitForZMapCaptureUntilIdle(int idleTimeoutMilliseconds, int overallTimeoutMilliseconds)
        {
            if (_zmapDone == null)
                throw new InvalidOperationException("ArmZMapCaptureExternalTrigger를 먼저 호출해야 합니다.");

            var overallStopwatch = Stopwatch.StartNew();
            while (overallStopwatch.ElapsedMilliseconds < overallTimeoutMilliseconds)
            {
                if (_zmapDone.Wait(200))
                    return _zmapResult;

                if (_zmapAccumulatedRows > 0 && _zmapLastChunkStopwatch.ElapsedMilliseconds >= idleTimeoutMilliseconds)
                    return TrimAccumulatedResult();
            }

            return _zmapAccumulatedRows > 0 ? TrimAccumulatedResult() : null;
        }

        // 요청한 라인 수를 다 채우지 못한 채 대기가 끝났을 때, 그때까지 누적된 부분만 잘라서 반환한다.
        private ZMapResult TrimAccumulatedResult()
        {
            int rows = _zmapAccumulatedRows;
            int width = _zmapAccumulatedWidth;

            var zData = new ushort[width * rows];
            var intensityData = new ushort[width * rows];
            Array.Copy(_zmapAccumulatedZData, zData, zData.Length);
            Array.Copy(_zmapAccumulatedIntensityData, intensityData, intensityData.Length);

            return new ZMapResult { Width = width, Height = rows, ZData = zData, IntensityData = intensityData };
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
