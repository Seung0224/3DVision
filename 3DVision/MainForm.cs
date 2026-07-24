using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ACS.SPiiPlusNET;
using SmartRay.Api;
using _3DVision.Acs;
using _3DVision.SmartRay;

namespace _3DVision
{
    public partial class MainForm : Form
    {
        private readonly AcsMotionController _acs = new AcsMotionController();
        private readonly SmartRayScannerController _smartRay = new SmartRayScannerController();
        private DetectedSensor[] _discoveredSensors = new DetectedSensor[0];

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

        private void btnDiscover_Click(object sender, EventArgs e)
        {
            try
            {
                Log("SmartRay 장치를 검색합니다...");
                _discoveredSensors = _smartRay.DiscoverSensors();

                cmbSensors.Items.Clear();
                foreach (var sensor in _discoveredSensors)
                    cmbSensors.Items.Add(string.Format("{0} / S/N {1} - {2}", sensor.ModelName, sensor.SerialNumber, sensor.IPAddress));

                Log(string.Format("검색 완료: {0}개 장치를 찾았습니다.", _discoveredSensors.Length));

                if (cmbSensors.Items.Count > 0)
                    cmbSensors.SelectedIndex = 0;
            }
            catch (SmartRayApiException ex)
            {
                Log("검색 실패 (SmartRayApiException): " + ex.Message);
            }
        }

        private void cmbSensors_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbSensors.SelectedIndex;
            if (index < 0 || index >= _discoveredSensors.Length)
                return;

            var sensor = _discoveredSensors[index];
            txtSrIp.Text = sensor.IPAddress.ToString();
            txtSrPort.Text = sensor.Port.ToString();
        }

        private void btnSrConnect_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSrPort.Text.Trim(), out int port))
            {
                Log("포트 값이 올바르지 않습니다: " + txtSrPort.Text);
                return;
            }

            try
            {
                _smartRay.Connect(txtSrIp.Text.Trim(), port, 3000);
                Log("SmartRay 연결 성공: " + txtSrIp.Text.Trim());
            }
            catch (SmartRayApiException ex)
            {
                Log("SmartRay 연결 실패 (SmartRayApiException): " + ex.Message);
            }
            catch (FormatException)
            {
                Log("IP 주소 형식이 올바르지 않습니다: " + txtSrIp.Text);
            }
        }

        private void btnSrDisconnect_Click(object sender, EventArgs e)
        {
            _smartRay.Disconnect();
            Log("SmartRay 연결을 해제했습니다.");
        }

        private void btnSrInfo_Click(object sender, EventArgs e)
        {
            try
            {
                var info = _smartRay.GetSensorInfo();
                Log(string.Format("모델: {0}, S/N: {1}, F/W: {2}", info.ModelName, info.SerialNumber, info.FirmwareVersion));
            }
            catch (InvalidOperationException ex)
            {
                Log(ex.Message);
            }
            catch (SmartRayApiException ex)
            {
                Log("정보 조회 실패 (SmartRayApiException): " + ex.Message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _acs.Dispose();
            _smartRay.Dispose();
        }

        private void Log(string message)
        {
            txtLog.AppendText(string.Format("[{0:HH:mm:ss}] {1}{2}", DateTime.Now, message, Environment.NewLine));
        }
    }
}
