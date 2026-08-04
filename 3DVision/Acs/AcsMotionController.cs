using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
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

        public void EnableAxis(int axisIndex)
        {
            if (!IsConnected)
                throw new InvalidOperationException("컨트롤러에 연결되어 있지 않습니다.");

            _api.EnableAsync((Axis)axisIndex);
        }

        public void DisableAxis(int axisIndex)
        {
            if (!IsConnected)
                throw new InvalidOperationException("컨트롤러에 연결되어 있지 않습니다.");

            _api.DisableAsync((Axis)axisIndex);
        }

        // 현재 위치 기준 상대 이동(PTP). distance가 양수면 +방향, 음수면 -방향으로 이동한다.
        public void MoveRelative(int axisIndex, double distance)
        {
            if (!IsConnected)
                throw new InvalidOperationException("컨트롤러에 연결되어 있지 않습니다.");

            _api.ToPointAsync(MotionFlags.ACSC_AMF_RELATIVE, (Axis)axisIndex, distance);
        }

        // 절대좌표 이동(PTP). position은 축의 원점(홈) 기준 목표 위치.
        public void MoveAbsolute(int axisIndex, double position)
        {
            if (!IsConnected)
                throw new InvalidOperationException("컨트롤러에 연결되어 있지 않습니다.");

            _api.ToPointAsync(MotionFlags.ACSC_NONE, (Axis)axisIndex, position);
        }

        // 이동 명령 이후 해당 축이 정지 상태(INPOS)가 될 때까지 대기한다.
        // 그랩(촬영) 전에 반드시 이동이 끝났는지 확인하기 위한 용도.
        public bool WaitForInPosition(int axisIndex, int timeoutMilliseconds)
        {
            if (!IsConnected)
                throw new InvalidOperationException("컨트롤러에 연결되어 있지 않습니다.");

            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < timeoutMilliseconds)
            {
                MotorStates state = GetAxisState(axisIndex);
                bool inPosition = (state & MotorStates.ACSC_MST_INPOS) != 0;
                bool moving = (state & MotorStates.ACSC_MST_MOVE) != 0;

                if (inPosition && !moving)
                    return true;

                Thread.Sleep(20);
            }

            return false;
        }

        // 비상 정지: 해당 축의 모션을 즉시 취소한다.
        public void Stop(int axisIndex)
        {
            if (!IsConnected)
                return;

            _api.KillAsync((Axis)axisIndex);
        }

        // 컨트롤러의 ACSPL+ Buffer 2("PEG 4 AXIS FACTOR SETTING")를 그대로 재현한다. SmartRay Input1로 나가는
        // 트리거 펄스는 ACS SDK의 정식 PEG 함수(PegI 등)가 아니라, 실제 이동축(moveAxis)의 위치에 전자
        // 기어링(CONNECT)으로 연동된 가상 펄스 출력 축(pegAxis)에서 나온다 - moveAxis가 stepFactor(mm)만큼
        // 움직일 때마다 pegAxis가 펄스를 하나씩 낸다. 기본값은 Buffer 2에 저장된 실제 값 그대로다
        // (pegAxis=4, moveAxis=0, stepFactor=0.025mm는 SmartRay 설정의 zMap.lateralResolution과 일치).
        // 컨트롤러가 이미 이 설정으로 부팅되어 있다면(Buffer 2가 AUTOEXEC로 자동 실행됨) 다시 호출할 필요는
        // 없고, 컨트롤러가 리셋되어 설정이 날아갔을 때를 위한 것이다.
        public void SetupPegTrigger(int pegAxis = 4, int moveAxis = 0, double stepFactor = 0.025, double stepWidth = 0.05, double delaySeconds = 0.000314)
        {
            if (!IsConnected)
                throw new InvalidOperationException("컨트롤러에 연결되어 있지 않습니다.");

            _api.Command(string.Format(CultureInfo.InvariantCulture, "DISABLE({0})", pegAxis));
            _api.Command(string.Format(CultureInfo.InvariantCulture, "STEPF({0}) = {1}", pegAxis, stepFactor));
            _api.Command(string.Format(CultureInfo.InvariantCulture, "STEPW({0}) = {1}", pegAxis, stepWidth));
            _api.Command(string.Format(CultureInfo.InvariantCulture, "MFLAGS({0}).#DEFCON = 1", pegAxis));
            _api.Command(string.Format(CultureInfo.InvariantCulture, "MFLAGS({0}).#DEFCON = 0", pegAxis));
            _api.Command(string.Format(CultureInfo.InvariantCulture,
                "CONNECT RPOS({0}) = APOS({1}) - {2} * RVEL({1})", pegAxis, moveAxis, delaySeconds));
            _api.Command(string.Format(CultureInfo.InvariantCulture, "ENABLE({0})", pegAxis));
        }

        // 컨트롤러의 ACSPL+ Buffer 0(axis 0 홈잡기)을 원격으로 실행한다. Buffer 0은 IF/GOTO/TILL 같은 제어
        // 흐름이 있고 실제로 리밋 스위치를 찾아 축을 움직이므로, PEG 설정과 달리 로직을 C#으로 다시 구현하지
        // 않는다 - 이미 검증된 그 버퍼를 그대로 실행시키고 끝날 때까지 기다리는 방식이 훨씬 안전하다.
        // 완료 후 D-Buffer의 HomeFlag(0)을 읽어 성공 여부를 반환한다 (Buffer 0은 성공 시에만 1로 세팅하고,
        // Time_Out 경로로 빠지면 세팅하지 않는다).
        public bool RunHomingBuffer(int timeoutMilliseconds, Action<string> log = null)
        {
            if (!IsConnected)
                throw new InvalidOperationException("컨트롤러에 연결되어 있지 않습니다.");

            log?.Invoke("RunBuffer(ACSC_BUFFER_0)");
            _api.RunBuffer(ProgramBuffer.ACSC_BUFFER_0, null);

            log?.Invoke("완료 대기 중 (GetProgramState 폴링)");
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < timeoutMilliseconds)
            {
                ProgramStates state = _api.GetProgramState(ProgramBuffer.ACSC_BUFFER_0);
                if ((state & ProgramStates.ACSC_PST_RUN) == 0)
                    break;

                Thread.Sleep(100);
            }

            log?.Invoke("HomeFlag(0) 읽는 중");
            // HomeFlag는 1차원 배열(vector)이라, ind2는 반드시 Api.ACSC_NONE으로 둬야 한다 - 0을 넣으면
            // SDK가 2차원 행렬 접근("HomeFlag(0,0)")으로 잘못 만들어서 컨트롤러가 "Extra characters after
            // the command" 오류를 낸다.
            object homeFlag = _api.ReadVariableAsScalar("HomeFlag", ProgramBuffer.ACSC_NONE, 0, Api.ACSC_NONE);
            return Convert.ToInt32(homeFlag) == 1;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
