using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ACS.SPiiPlusNET;
using _3DVision.Acs;

namespace _3DVision
{
    public partial class MainForm : Form
    {
        private readonly AcsMotionController _acs = new AcsMotionController();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                _acs.Connect(txtIp.Text.Trim());
                Log("연결 성공: " + txtIp.Text.Trim());
            }
            catch (COMException ex)
            {
                Log("연결 실패 (COMException): " + ex.Message);
            }
            catch (ACSException ex)
            {
                Log("연결 실패 (ACSException): " + ex.Message);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                _acs.Disconnect();
                Log("연결 해제했습니다.");
            }
            catch (COMException ex)
            {
                Log("연결 해제 실패: " + ex.Message);
            }
        }

        private void btnCheckStatus_Click(object sender, EventArgs e)
        {
            int axisIndex = (int)numAxis.Value;
            try
            {
                double position = _acs.GetAxisPosition(axisIndex);
                MotorStates state = _acs.GetAxisState(axisIndex);
                Log(string.Format("축 {0} - 위치: {1:0.###}, 상태: {2}", axisIndex, position, state));
            }
            catch (InvalidOperationException ex)
            {
                Log(ex.Message);
            }
            catch (COMException ex)
            {
                Log("상태 조회 실패 (COMException): " + ex.Message);
            }
            catch (ACSException ex)
            {
                Log("상태 조회 실패 (ACSException): " + ex.Message);
            }
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            int axisIndex = (int)numMoveAxis.Value;
            try
            {
                _acs.EnableAxis(axisIndex);
                Log(string.Format("축 {0} Enable 명령을 보냈습니다.", axisIndex));
            }
            catch (InvalidOperationException ex)
            {
                Log(ex.Message);
            }
            catch (COMException ex)
            {
                Log("Enable 실패 (COMException): " + ex.Message);
            }
            catch (ACSException ex)
            {
                Log("Enable 실패 (ACSException): " + ex.Message);
            }
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            int axisIndex = (int)numMoveAxis.Value;
            try
            {
                _acs.DisableAxis(axisIndex);
                Log(string.Format("축 {0} Disable 명령을 보냈습니다.", axisIndex));
            }
            catch (InvalidOperationException ex)
            {
                Log(ex.Message);
            }
            catch (COMException ex)
            {
                Log("Disable 실패 (COMException): " + ex.Message);
            }
            catch (ACSException ex)
            {
                Log("Disable 실패 (ACSException): " + ex.Message);
            }
        }

        private void btnMovePlus_Click(object sender, EventArgs e)
        {
            MoveRelative(+1);
        }

        private void btnMoveMinus_Click(object sender, EventArgs e)
        {
            MoveRelative(-1);
        }

        private void MoveRelative(int direction)
        {
            int axisIndex = (int)numMoveAxis.Value;

            if (!double.TryParse(txtDistance.Text.Trim(), out double distance))
            {
                Log("거리 값이 올바르지 않습니다: " + txtDistance.Text);
                return;
            }

            try
            {
                _acs.MoveRelative(axisIndex, distance * direction);
                Log(string.Format("축 {0} 을(를) {1}만큼 이동 명령을 보냈습니다.", axisIndex, distance * direction));
            }
            catch (InvalidOperationException ex)
            {
                Log(ex.Message);
            }
            catch (COMException ex)
            {
                Log("이동 실패 (COMException): " + ex.Message);
            }
            catch (ACSException ex)
            {
                Log("이동 실패 (ACSException): " + ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            int axisIndex = (int)numMoveAxis.Value;
            _acs.Stop(axisIndex);
            Log(string.Format("축 {0} 정지 명령을 보냈습니다.", axisIndex));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _acs.Dispose();
        }

        private void Log(string message)
        {
            txtLog.AppendText(string.Format("[{0:HH:mm:ss}] {1}{2}", DateTime.Now, message, Environment.NewLine));
        }
    }
}
