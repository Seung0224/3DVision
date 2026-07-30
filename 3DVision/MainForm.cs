using System;
using System.Drawing;
using System.Runtime.InteropServices;
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

        public MainForm()
        {
            InitializeComponent();
        }

        // 프로그램을 켜면 ACS와 SmartRay에 자동으로 연결을 시도한다. 둘 다 네트워크 연결이라 약간 시간이
        // 걸릴 수 있어 백그라운드에서 실행하고, 끝나면 상태 텍스트를 갱신한다.
        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateConnectionStatusLabels();

            Task.Run(() =>
            {
                try
                {
                    _acs.Connect(txtIp.Text.Trim());
                    LogSafe("[자동연결] ACS 연결 성공: " + txtIp.Text.Trim());
                }
                catch (Exception ex)
                {
                    LogSafe(string.Format("[자동연결] ACS 연결 실패 ({0}): {1}", ex.GetType().Name, ex.Message));
                }

                try
                {
                    if (!int.TryParse(txtSrPort.Text.Trim(), out int port))
                        throw new FormatException("포트 값이 올바르지 않습니다: " + txtSrPort.Text);

                    _smartRay.Connect(txtSrIp.Text.Trim(), port, 3000);
                    LogSafe("[자동연결] SmartRay 연결 성공: " + txtSrIp.Text.Trim());
                }
                catch (Exception ex)
                {
                    LogSafe(string.Format("[자동연결] SmartRay 연결 실패 ({0}): {1}", ex.GetType().Name, ex.Message));
                }

                Invoke(new Action(UpdateConnectionStatusLabels));
            });
        }

        // "연동 테스트" 탭에서 두 장비가 지금 연결되어 있는지 한눈에 보이도록 텍스트로 표시한다.
        private void UpdateConnectionStatusLabels()
        {
            lblAcsStatus.Text = "ACS: " + (_acs.IsConnected ? "연결됨" : "연결 안됨");
            lblAcsStatus.ForeColor = _acs.IsConnected ? Color.SeaGreen : Color.Firebrick;

            lblSmartRayStatus.Text = "SmartRay: " + (_smartRay.IsConnected ? "연결됨" : "연결 안됨");
            lblSmartRayStatus.ForeColor = _smartRay.IsConnected ? Color.SeaGreen : Color.Firebrick;
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

            UpdateConnectionStatusLabels();
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

            UpdateConnectionStatusLabels();
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

            UpdateConnectionStatusLabels();
        }

        private void btnSrDisconnect_Click(object sender, EventArgs e)
        {
            _smartRay.Disconnect();
            Log("SmartRay 연결을 해제했습니다.");
            UpdateConnectionStatusLabels();
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

        // ZMap의 raw 값(UInt16)을 분석한다. 0은 "측정 안 됨"으로 취급한다 (실제 mm 단위 변환 전 raw 값 기준).
        private static (int total, int valid, int minRaw, int maxRaw) AnalyzeZMap(ZMapResult result)
        {
            int total = result.ZData?.Length ?? 0;
            int valid = 0;
            int minRaw = int.MaxValue, maxRaw = int.MinValue;

            if (result.ZData != null)
            {
                foreach (var v in result.ZData)
                {
                    if (v == 0)
                        continue;

                    valid++;
                    if (v < minRaw) minRaw = v;
                    if (v > maxRaw) maxRaw = v;
                }
            }

            if (valid == 0)
            {
                minRaw = 0;
                maxRaw = 0;
            }

            return (total, valid, minRaw, maxRaw);
        }

        // 백그라운드 스레드에서 호출된다. 비트맵을 만든 뒤 UI 스레드로 넘겨서 PictureBox에 표시한다.
        // 높이맵(위에서 본 그레이스케일)과 포인트클라우드(등각 투영, 의사 3D)를 같이 갱신한다.
        private void ShowZMapImage(ZMapResult result)
        {
            var heightMapBitmap = BuildZMapImage(result);
            var pointCloudBitmap = BuildPointCloudImage(result, picPointCloud.Width, picPointCloud.Height);

            Invoke(new Action(() =>
            {
                picHeightMap.Image?.Dispose();
                picHeightMap.Image = heightMapBitmap;

                picPointCloud.Image?.Dispose();
                picPointCloud.Image = pointCloudBitmap;
            }));
        }

        // ZMap 격자를 열(col)/행(row)/높이(z) 기준으로 간단한 등각(isometric) 투영해 흩뿌린다.
        // 진짜 3D 뷰어(회전/줌)는 아니지만, 위에서만 보는 높이맵과 달리 입체감을 볼 수 있다.
        // 참고: row(행) 방향은 실제 mm 거리가 아니라 촬영 순서 기준이라 세로 비율은 정확하지 않다.
        private static Bitmap BuildPointCloudImage(ZMapResult result, int imageWidth, int imageHeight)
        {
            var bitmap = new Bitmap(Math.Max(1, imageWidth), Math.Max(1, imageHeight));
            using (var g = Graphics.FromImage(bitmap))
                g.Clear(Color.Black);

            if (result.ZData == null || result.ZData.Length < result.Width * result.Height)
                return bitmap;

            ushort minRaw = ushort.MaxValue, maxRaw = 0;
            foreach (var v in result.ZData)
            {
                if (v == 0)
                    continue;
                if (v < minRaw) minRaw = v;
                if (v > maxRaw) maxRaw = v;
            }

            if (maxRaw <= minRaw)
                return bitmap;

            float zRange = maxRaw - minRaw;
            int w = result.Width;
            int h = result.Height;

            const double AngleDeg = 30.0;
            double cos = Math.Cos(AngleDeg * Math.PI / 180.0);
            double sin = Math.Sin(AngleDeg * Math.PI / 180.0);

            double spanX = (w + h) * cos;
            double scaleXY = spanX > 0 ? (bitmap.Width * 0.9) / spanX : 1.0;
            double heightScale = bitmap.Height * 0.5;
            double originX = bitmap.Width / 2.0;
            double originY = bitmap.Height * 0.8;

            for (int row = 0; row < h; row++)
            {
                for (int col = 0; col < w; col++)
                {
                    ushort v = result.ZData[row * w + col];
                    if (v == 0)
                        continue;

                    float zNorm = (v - minRaw) / zRange;

                    double sx = originX + (col - row) * cos * scaleXY;
                    double sy = originY - (col + row) * sin * scaleXY * 0.5 - zNorm * heightScale;

                    int px = (int)sx;
                    int py = (int)sy;
                    if (px < 0 || px >= bitmap.Width || py < 0 || py >= bitmap.Height)
                        continue;

                    int gray = (int)(60 + zNorm * 195);
                    gray = Math.Max(0, Math.Min(255, gray));
                    bitmap.SetPixel(px, py, Color.FromArgb(gray, gray, 255));
                }
            }

            return bitmap;
        }

        // ZMap은 이미 Width x Height 격자로 정렬되어 있으므로, 흩뿌릴 필요 없이 그대로 그레이스케일로 매핑한다.
        private static Bitmap BuildZMapImage(ZMapResult result)
        {
            int w = Math.Max(1, result.Width);
            int h = Math.Max(1, result.Height);
            var bitmap = new Bitmap(w, h);

            using (var g = Graphics.FromImage(bitmap))
                g.Clear(Color.Black);

            if (result.ZData == null || result.ZData.Length < result.Width * result.Height)
                return bitmap;

            ushort minRaw = ushort.MaxValue, maxRaw = 0;
            foreach (var v in result.ZData)
            {
                if (v == 0)
                    continue;
                if (v < minRaw) minRaw = v;
                if (v > maxRaw) maxRaw = v;
            }

            if (maxRaw <= minRaw)
                return bitmap;

            float range = maxRaw - minRaw;

            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    ushort v = result.ZData[y * result.Width + x];
                    if (v == 0)
                        continue;

                    int gray = (int)((v - minRaw) / range * 255);
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

        // 이동하는 내내 레이저가 켜져 있도록, 이동 시간보다 넉넉하게 걸릴 만큼 큰 프로파일 수를 요청한다.
        private const int ScanGrabProfileCount = 5000;

        // 자재 끝 위치는 실측으로 확인된 고정값이라 UI에서 입력받지 않는다.
        private const double ScanEndPosition = 130.221;

        private void btnScanStart_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(txtStartPos.Text.Trim(), out double startPos))
            {
                Log("시작 위치 값이 올바르지 않습니다: " + txtStartPos.Text);
                return;
            }

            if (startPos == ScanEndPosition)
            {
                Log("시작 위치와 끝 위치가 같습니다.");
                return;
            }

            int axisIndex = (int)numMoveAxis.Value;

            SetContinuousScanRunningState(true);

            Task.Run(() => RunMoveAndGrab(axisIndex, startPos, ScanEndPosition))
                .ContinueWith(t => Invoke(new Action(() => SetContinuousScanRunningState(false))));
        }

        private void btnScanStop_Click(object sender, EventArgs e)
        {
            // 이동 중일 수 있으므로 즉시 정지시키고, 레이저도 반드시 끈다.
            int axisIndex = (int)numMoveAxis.Value;
            _acs.Stop(axisIndex);
            _smartRay.StopZMapCapture();
            Log("[이동+촬영] 비상 정지: 모터 정지 + 레이저 OFF");
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
        // 순서가 중요하다: 시작 위치로 이동(레이저 꺼짐) → SmartRay를 먼저 촬영 대기 상태로 만들어 레이저를 켠 뒤 →
        // 그 상태로 끝 위치까지 이동한다. 그래야 이동하는 내내 레이저가 켜진 채로 촬영된다.
        private void RunMoveAndGrab(int axisIndex, double startPos, double endPos)
        {
            try
            {
                LogSafe(string.Format("[이동+촬영] 시작 위치({0:0.###})로 이동합니다...", startPos));
                _acs.MoveAbsolute(axisIndex, startPos);
                if (!_acs.WaitForInPosition(axisIndex, 30000))
                {
                    LogSafe("[이동+촬영] 시작 위치 이동이 시간 안에 끝나지 않아 중단합니다.");
                    return;
                }

                LogSafe("[이동+촬영] SmartRay 촬영을 시작합니다 (레이저 ON)...");
                _smartRay.ArmZMapCapture(ScanGrabProfileCount, step => LogSafe("  단계: " + step));

                try
                {
                    LogSafe(string.Format("[이동+촬영] 레이저를 켠 채로 끝 위치({0:0.###})까지 이동합니다...", endPos));
                    _acs.MoveAbsolute(axisIndex, endPos);

                    bool moveDone = _acs.WaitForInPosition(axisIndex, 180000);
                    double actualPos = _acs.GetAxisPosition(axisIndex);
                    if (!moveDone)
                    {
                        LogSafe(string.Format("[이동+촬영] 경고: 이동이 제한 시간(180초) 안에 끝나지 않았습니다. 현재 위치: {0:0.###}", actualPos));
                        return;
                    }

                    LogSafe(string.Format("[이동+촬영] 이동 완료. 실제 위치: {0:0.###}. 남은 촬영 데이터를 대기합니다...", actualPos));

                    var result = _smartRay.WaitForZMapCapture(15000);
                    if (result == null)
                    {
                        LogSafe("[이동+촬영] 촬영 데이터를 받지 못했습니다 (타임아웃).");
                        return;
                    }

                    var s = AnalyzeZMap(result);
                    LogSafe(string.Format("[이동+촬영] 완료: {0}x{1}, 전체 {2}점 중 유효 {3}점, raw Z:[{4}, {5}]",
                        result.Width, result.Height, s.total, s.valid, s.minRaw, s.maxRaw));

                    if (s.valid > 0)
                        ShowZMapImage(result);
                }
                finally
                {
                    _smartRay.StopZMapCapture();
                }
            }
            catch (Exception ex)
            {
                LogSafe(string.Format("[이동+촬영] 오류 발생 ({0}): {1}", ex.GetType().Name, ex.Message));
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
            _acs.Dispose();
            _smartRay.Dispose();
        }

        private void Log(string message)
        {
            txtLog.AppendText(string.Format("[{0:HH:mm:ss}] {1}{2}", DateTime.Now, message, Environment.NewLine));
        }
    }
}
