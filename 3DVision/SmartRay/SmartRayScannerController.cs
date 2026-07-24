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

    // SmartRay ECCO 스캐너와의 통신을 담당하는 클래스.
    // 2단계-A: SDK 초기화, 장치 검색(Discovery), 연결/해제, 레이저/촬영 없는 장비 정보 조회.
    // 2단계-B: 레이저를 켜고 한 번 스냅샷을 찍어 포인트클라우드를 받아오는 단일 그랩(FreeRunning).
    // 3단계: ACS 축의 외부 트리거(Input1)에 맞춰 자동으로 프로파일을 쌓는 연속 스캔.
    public class SmartRayScannerController : IDisposable
    {
        private static bool _apiInitialized;

        private Sensor _sensor;
        private bool _baseConfigured;

        private ManualResetEventSlim _scanDone;
        private GrabResult _scanResult;
        private Sensor.OnPointCloudImageDelegate _scanHandler;

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
            _baseConfigured = false;
        }

        // 레이저/촬영과 무관한, 연결 확인용 장비 정보만 읽어온다.
        public (string ModelName, string SerialNumber, string FirmwareVersion) GetSensorInfo()
        {
            if (!IsConnected)
                throw new InvalidOperationException("스캐너에 연결되어 있지 않습니다.");

            return (_sensor.ModelName, _sensor.SerialNumber, _sensor.FirmwareVersion);
        }

        // 연결 후 한 번만 필요한 설정(획득 타입/캘리브레이션 로드)을 수행한다.
        // 캘리브레이션 로드는 네트워크 왕복이 있어 매번 반복하면 느려지므로, 연결이 끊기기 전까지는 한 번만 실행한다.
        // 트리거 모드는 용도(단발 촬영 vs 연속 스캔)에 따라 매번 달라지므로 여기 포함하지 않는다.
        private void EnsureBaseConfigured(Action<string> log)
        {
            if (_baseConfigured)
                return;

            log?.Invoke("SetImageAcquisitionType(PointCloud)");
            _sensor.SetImageAcquisitionType(ImageAcquisitionType.PointCloud);

            log?.Invoke("SetAcquisitionImageTypeFlags(PointCloudP)");
            _sensor.SetAcquisitionImageTypeFlags(AcquisitionImageTypeFlags.PointCloudP);

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

            _baseConfigured = true;
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
                EnsureBaseConfigured(log);

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

        // ACS 축의 외부 신호(Input1)에 맞춰 자동으로 프로파일을 찍는 연속 스캔을 무장(arm)한다.
        // 레이저를 켜고 StartAcquisition까지 호출한 뒤, 트리거를 기다리는 상태로 즉시 반환한다.
        // 반드시 이 호출이 끝난 "이후에" ACS 이동 명령을 내려야 스캔 시작 구간을 놓치지 않는다.
        public void ArmContinuousScan(int profileCount, double transportResolutionMm, Action<string> log = null)
        {
            if (!IsConnected)
                throw new InvalidOperationException("스캐너에 연결되어 있지 않습니다.");

            EnsureBaseConfigured(log);

            log?.Invoke("SetDataTriggerMode(External)");
            _sensor.SetDataTriggerMode(DataTriggerMode.External);

            log?.Invoke("SetDataTriggerExternalTriggerSource(Input1)");
            _sensor.SetDataTriggerExternalTriggerSource(DataTriggerSource.Input1);

            log?.Invoke("SetDataTriggerExternalTriggerParameters(divider=1)");
            _sensor.SetDataTriggerExternalTriggerParameters(1, 0, TriggerEdgeMode.Both);

            // SDK 내부 단위는 meter이므로 mm 입력값을 1000으로 나눈다.
            log?.Invoke(string.Format("SetTransportResolution({0}mm)", transportResolutionMm));
            _sensor.SetTransportResolution((float)(transportResolutionMm / 1000.0));

            log?.Invoke("SetAcquisitionMode(Snapshot)");
            _sensor.SetAcquisitionMode(AcquisitionMode.Snapshot);

            log?.Invoke("SetNumberOfProfilesToCapture");
            _sensor.SetNumberOfProfilesToCapture((uint)profileCount);

            _scanDone = new ManualResetEventSlim(false);
            _scanResult = null;

            _scanHandler = (sensor, dataType, width, height, points, intensity, reflectance, metaData) =>
            {
                _scanResult = new GrabResult { Points = points, Width = width, Height = height };
                _scanDone.Set();
            };
            _sensor.OnPointCloudImage += _scanHandler;

            log?.Invoke("SetLaserPower(true)");
            _sensor.SetLaserPower(true);

            log?.Invoke("StartAcquisition (외부 트리거 대기 시작)");
            _sensor.StartAcquisition();
        }

        // ArmContinuousScan 이후 호출. 요청한 프로파일 수만큼 데이터가 다 모일 때까지 대기한다.
        // 시간 안에 못 받으면 null을 반환한다 (예외를 던지지 않음 - 호출자가 StopContinuousScan을 반드시 부르게 하기 위함).
        public GrabResult WaitForContinuousScan(int timeoutMilliseconds)
        {
            if (_scanDone == null)
                throw new InvalidOperationException("ArmContinuousScan을 먼저 호출해야 합니다.");

            return _scanDone.Wait(timeoutMilliseconds) ? _scanResult : null;
        }

        // 레이저를 끄고 촬영을 정지한다. Arm 이후에는 성공/실패/타임아웃 여부와 관계없이 항상 호출해야 한다.
        public void StopContinuousScan()
        {
            if (_sensor == null)
                return;

            _sensor.SetLaserPower(false);
            _sensor.StopAcquisition();

            if (_scanHandler != null)
            {
                _sensor.OnPointCloudImage -= _scanHandler;
                _scanHandler = null;
            }
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
