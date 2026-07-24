using System;
using ACS.SPiiPlusNET;

namespace _3DVision.Acs
{
    // ACS SPiiPlus 컨트롤러와의 통신을 담당하는 클래스.
    // 1단계-A 범위: 연결/해제와, 모터를 움직이지 않는 읽기 전용 상태 조회만 포함한다.
    public class AcsMotionController : IDisposable
    {
        private Api _api;

        public bool IsConnected { get; private set; }

        public void Connect(string ipAddress)
        {
            if (IsConnected)
                return;

            _api = new Api();
            _api.OpenCommEthernet(ipAddress, (int)EthernetCommOption.ACSC_SOCKET_DGRAM_PORT);
            IsConnected = true;
        }

        public void Disconnect()
        {
            if (!IsConnected)
                return;

            _api.CloseComm();
            IsConnected = false;
        }

        // 축을 움직이지 않고, 현재 피드백 위치만 읽어와 통신이 실제로 살아있는지 확인한다.
        public double GetAxisPosition(int axisIndex)
        {
            if (!IsConnected)
                throw new InvalidOperationException("컨트롤러에 연결되어 있지 않습니다.");

            return _api.GetFPosition((Axis)axisIndex);
        }

        public MotorStates GetAxisState(int axisIndex)
        {
            if (!IsConnected)
                throw new InvalidOperationException("컨트롤러에 연결되어 있지 않습니다.");

            return _api.GetMotorState((Axis)axisIndex);
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
