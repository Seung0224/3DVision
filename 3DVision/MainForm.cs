using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
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
        private CancellationTokenSource _sweepCts;

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

        private void btnGrab_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtProfileCount.Text.Trim(), out int profileCount) || profileCount <= 0)
            {
                Log("프로파일 수 값이 올바르지 않습니다: " + txtProfileCount.Text);
                return;
            }

            btnGrab.Enabled = false;
            try
            {
                LogAxisPositionIfConnected();
                Log("촬영을 시작합니다 (레이저 ON)...");
                var result = _smartRay.GrabSingle(profileCount, 5000, step => Log("  단계: " + step));
                LogGrabResult(result);
            }
            catch (InvalidOperationException ex)
            {
                Log(ex.Message);
            }
            catch (TimeoutException ex)
            {
                Log("촬영 실패 (Timeout): " + ex.Message);
            }
            catch (SmartRayApiException ex)
            {
                Log("촬영 실패 (SmartRayApiException): " + ex.Message);
            }
            finally
            {
                btnGrab.Enabled = true;
            }
        }

        // 촬영 시점의 ACS 축 위치를 로그에 남긴다. SmartRay 포인트클라우드의 Y값은
        // FreeRunning 촬영에서는 실제 이동 거리와 무관하므로, 실제 위치 기준은 이 값을 써야 한다.
        private void LogAxisPositionIfConnected()
        {
            if (!_acs.IsConnected)
                return;

            int axisIndex = (int)numMoveAxis.Value;
            try
            {
                double position = _acs.GetAxisPosition(axisIndex);
                Log(string.Format("[ACS 위치] 축 {0}: {1:0.###}", axisIndex, position));
            }
            catch (COMException)
            {
                // 위치 조회 실패는 촬영 자체를 막을 이유가 아니므로 조용히 넘어간다.
            }
        }

        private void LogGrabResult(GrabResult result)
        {
            var s = AnalyzePoints(result.Points);

            Log(string.Format("촬영 완료: {0}x{1}, 전체 {2}점 중 유효 {3}점", result.Width, result.Height, s.total, s.valid));

            if (s.valid > 0)
            {
                Log(string.Format("범위 - X:[{0:0.###}, {1:0.###}] Y:[{2:0.###}, {3:0.###}] Z:[{4:0.###}, {5:0.###}]",
                    s.minX, s.maxX, s.minY, s.maxY, s.minZ, s.maxZ));
            }
        }

        // SmartRay는 무효 포인트를 NaN이 아니라 -999997~-999999 근처 특수값으로 표시한다.
        private static (int total, int valid, float minX, float maxX, float minY, float maxY, float minZ, float maxZ) AnalyzePoints(Point3F[] points)
        {
            const float InvalidThreshold = -999990f;

            int total = points.Length;
            int valid = 0;
            float minX = float.MaxValue, minY = float.MaxValue, minZ = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue, maxZ = float.MinValue;

            foreach (var p in points)
            {
                if (float.IsNaN(p.X) || float.IsNaN(p.Y) || float.IsNaN(p.Z))
                    continue;
                if (p.X < InvalidThreshold || p.Y < InvalidThreshold || p.Z < InvalidThreshold)
                    continue;

                valid++;
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.Z < minZ) minZ = p.Z;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
                if (p.Z > maxZ) maxZ = p.Z;
            }

            return (total, valid, minX, maxX, minY, maxY, minZ, maxZ);
        }

        // 백그라운드 스레드에서 호출된다. 비트맵을 만든 뒤 UI 스레드로 넘겨서 PictureBox에 표시한다.
        private void ShowHeightMap(Point3F[] points)
        {
            var bitmap = BuildHeightMapImage(points, picHeightMap.Width, picHeightMap.Height);

            Invoke(new Action(() =>
            {
                picHeightMap.Image?.Dispose();
                picHeightMap.Image = bitmap;
            }));
        }

        // 포인트들을 X(가로)/Y(세로) 범위에 맞춰 격자에 흩뿌리고, Z값을 그레이스케일로 매핑한 높이맵 이미지를 만든다.
        // SmartRay PointCloud는 SmartRay 자체적으로 고정된 사각 격자 구조가 아니라서, 값 범위 기준으로 직접 격자를 만든다.
        private static Bitmap BuildHeightMapImage(Point3F[] points, int imageWidth, int imageHeight)
        {
            const float InvalidThreshold = -999990f;

            var bitmap = new Bitmap(Math.Max(1, imageWidth), Math.Max(1, imageHeight));
            using (var g = Graphics.FromImage(bitmap))
                g.Clear(Color.Black);

            float minX = float.MaxValue, maxX = float.MinValue;
            float minY = float.MaxValue, maxY = float.MinValue;
            float minZ = float.MaxValue, maxZ = float.MinValue;

            foreach (var p in points)
            {
                if (float.IsNaN(p.X) || float.IsNaN(p.Y) || float.IsNaN(p.Z))
                    continue;
                if (p.X < InvalidThreshold || p.Y < InvalidThreshold || p.Z < InvalidThreshold)
                    continue;

                if (p.X < minX) minX = p.X;
                if (p.X > maxX) maxX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.Y > maxY) maxY = p.Y;
                if (p.Z < minZ) minZ = p.Z;
                if (p.Z > maxZ) maxZ = p.Z;
            }

            if (maxX <= minX || maxY <= minY)
                return bitmap;

            float zRange = maxZ - minZ;
            if (zRange <= 0)
                zRange = 1;

            int w = bitmap.Width;
            int h = bitmap.Height;
            var grid = new float?[w, h];

            foreach (var p in points)
            {
                if (float.IsNaN(p.X) || float.IsNaN(p.Y) || float.IsNaN(p.Z))
                    continue;
                if (p.X < InvalidThreshold || p.Y < InvalidThreshold || p.Z < InvalidThreshold)
                    continue;

                int px = (int)((p.X - minX) / (maxX - minX) * (w - 1));
                int py = (int)((p.Y - minY) / (maxY - minY) * (h - 1));
                py = h - 1 - py; // 이미지 좌표는 위가 0이므로 뒤집는다.

                grid[px, py] = p.Z;
            }

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (!grid[x, y].HasValue)
                        continue;

                    int gray = (int)((grid[x, y].Value - minZ) / zRange * 255);
                    gray = Math.Max(0, Math.Min(255, gray));
                    bitmap.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }

            return bitmap;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(txtStartPos.Text.Trim(), out double startPos))
            {
                Log("시작 위치 값이 올바르지 않습니다: " + txtStartPos.Text);
                return;
            }

            int axisIndex = (int)numMoveAxis.Value;

            try
            {
                Log(string.Format("[Home] 축 {0}을(를) 시작 위치({1:0.###})로 이동합니다...", axisIndex, startPos));
                _acs.MoveAbsolute(axisIndex, startPos);
            }
            catch (InvalidOperationException ex)
            {
                Log(ex.Message);
            }
            catch (COMException ex)
            {
                Log("[Home] 이동 실패 (COMException): " + ex.Message);
            }
            catch (ACSException ex)
            {
                Log("[Home] 이동 실패 (ACSException): " + ex.Message);
            }
        }

        // 자재 끝 위치는 실측으로 확인된 고정값이라 UI에서 입력받지 않는다.
        private const double ScanEndPosition = 130.221;

        private void btnScanStart_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(txtStartPos.Text.Trim(), out double startPos))
            {
                Log("시작 위치 값이 올바르지 않습니다: " + txtStartPos.Text);
                return;
            }

            if (!double.TryParse(txtTransportRes.Text.Trim(), out double transportResUm) || transportResUm <= 0)
            {
                Log("Transport Resolution 값이 올바르지 않습니다: " + txtTransportRes.Text);
                return;
            }

            if (startPos == ScanEndPosition)
            {
                Log("시작 위치와 끝 위치가 같습니다.");
                return;
            }

            int axisIndex = (int)numMoveAxis.Value;

            _sweepCts = new CancellationTokenSource();
            CancellationToken token = _sweepCts.Token;

            SetContinuousScanRunningState(true);

            Task.Run(() => RunContinuousScan(axisIndex, startPos, ScanEndPosition, transportResUm, token))
                .ContinueWith(t => Invoke(new Action(() => SetContinuousScanRunningState(false))));
        }

        private void btnScanStop_Click(object sender, EventArgs e)
        {
            _sweepCts?.Cancel();

            // 이동 중일 수 있으므로 즉시 정지시키고, 레이저도 반드시 끈다.
            int axisIndex = (int)numMoveAxis.Value;
            _acs.Stop(axisIndex);
            _smartRay.StopContinuousScan();
            Log("[연속 스캔] 비상 중지: 모터 정지 + 레이저 OFF");
        }

        private void SetContinuousScanRunningState(bool running)
        {
            btnScanStart.Enabled = !running;
            btnScanStop.Enabled = running;
            btnGrab.Enabled = !running;
            btnMovePlus.Enabled = !running;
            btnMoveMinus.Enabled = !running;
        }

        // 백그라운드 스레드에서 실행된다. UI 컨트롤은 절대 직접 건드리지 말고 LogSafe로만 로그를 남긴다.
        // 순서가 중요하다: SmartRay를 먼저 트리거 대기 상태로 만들고(ArmContinuousScan), 그 다음에 ACS를 끝 위치까지 이동시켜야
        // 이동 시작 구간의 데이터를 놓치지 않는다.
        private void RunContinuousScan(int axisIndex, double startPos, double endPos, double transportResUm, CancellationToken token)
        {
            double transportResMm = transportResUm / 1000.0;
            int profileCount = (int)Math.Ceiling(Math.Abs(endPos - startPos) / transportResMm) + 10; // 약간의 여유분

            LogSafe(string.Format("[연속 스캔] {0:0.###} → {1:0.###}, Transport Res {2}um, 예상 {3}프로파일",
                startPos, endPos, transportResUm, profileCount));

            try
            {
                LogSafe(string.Format("[연속 스캔] 시작 위치({0:0.###})로 이동합니다...", startPos));
                _acs.MoveAbsolute(axisIndex, startPos);
                if (!_acs.WaitForInPosition(axisIndex, 30000))
                {
                    LogSafe("[연속 스캔] 시작 위치 이동이 시간 안에 끝나지 않아 중단합니다.");
                    return;
                }

                if (token.IsCancellationRequested)
                {
                    LogSafe("[연속 스캔] 중지되었습니다.");
                    return;
                }

                LogSafe("[연속 스캔] SmartRay를 외부 트리거 대기 상태로 설정합니다 (레이저 ON)...");
                _smartRay.ArmContinuousScan(profileCount, transportResMm, step => LogSafe("  단계: " + step));

                try
                {
                    LogSafe(string.Format("[연속 스캔] ACS를 끝 위치({0:0.###})까지 이동합니다...", endPos));
                    _acs.MoveAbsolute(axisIndex, endPos);

                    bool moveDone = _acs.WaitForInPosition(axisIndex, 180000);
                    if (!moveDone)
                        LogSafe("[연속 스캔] 경고: 이동이 제한 시간(180초) 안에 끝나지 않았습니다.");
                    else
                        LogSafe("[연속 스캔] 이동 완료. 남은 데이터 수신을 대기합니다...");

                    var result = _smartRay.WaitForContinuousScan(10000);
                    if (result == null)
                    {
                        LogSafe("[연속 스캔] 데이터를 받지 못했습니다 (타임아웃).");
                        return;
                    }

                    var s = AnalyzePoints(result.Points);
                    LogSafe(string.Format("[연속 스캔] 완료: 전체 {0}점 중 유효 {1}점", s.total, s.valid));
                    if (s.valid > 0)
                    {
                        LogSafe(string.Format("[연속 스캔] 범위 - X:[{0:0.###}, {1:0.###}] Y:[{2:0.###}, {3:0.###}] Z:[{4:0.###}, {5:0.###}] (Y는 Input1 트리거 특성이 검증되지 않아 부정확할 수 있음)",
                            s.minX, s.maxX, s.minY, s.maxY, s.minZ, s.maxZ));

                        ShowHeightMap(result.Points);
                    }
                }
                finally
                {
                    _smartRay.StopContinuousScan();
                }
            }
            catch (Exception ex) when (ex is COMException || ex is ACSException || ex is SmartRayApiException || ex is InvalidOperationException)
            {
                LogSafe("[연속 스캔] 오류 발생: " + ex.Message);
            }
        }

        private void LogSafe(string message)
        {
            if (InvokeRequired)
                Invoke(new Action(() => Log(message)));
            else
                Log(message);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _sweepCts?.Cancel();
            _acs.Dispose();
            _smartRay.Dispose();
        }

        private void Log(string message)
        {
            txtLog.AppendText(string.Format("[{0:HH:mm:ss}] {1}{2}", DateTime.Now, message, Environment.NewLine));
        }
    }
}
