using System;
using System.Net;
using SmartRay.Api;

namespace _3DVision.SmartRay
{
    // SmartRay ECCO 스캐너와의 통신을 담당하는 클래스.
    // 2단계-A 범위: SDK 초기화, 장치 검색(Discovery), 연결/해제와
    // 레이저/촬영 없이 확인 가능한 장비 정보 조회만 포함한다.
    public class SmartRayScannerController : IDisposable
    {
        private static bool _apiInitialized;

        private Sensor _sensor;

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
        }

        // 레이저/촬영과 무관한, 연결 확인용 장비 정보만 읽어온다.
        public (string ModelName, string SerialNumber, string FirmwareVersion) GetSensorInfo()
        {
            if (!IsConnected)
                throw new InvalidOperationException("스캐너에 연결되어 있지 않습니다.");

            return (_sensor.ModelName, _sensor.SerialNumber, _sensor.FirmwareVersion);
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
