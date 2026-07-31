using System;
using System.Drawing;
using System.Drawing.Imaging;
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

        private void btnJogPlus_Click(object sender, EventArgs e)
        {
            JogMove(+1);
        }

        private void btnJogMinus_Click(object sender, EventArgs e)
        {
            JogMove(-1);
        }

        private void JogMove(int direction)
        {
            int axisIndex = (int)numMoveAxis.Value;

            if (!double.TryParse(txtJogStep.Text.Trim(), out double step))
            {
                Log("이동 단위 값이 올바르지 않습니다: " + txtJogStep.Text);
                return;
            }

            try
            {
                _acs.MoveRelative(axisIndex, step * direction);
                Log(string.Format("축 {0} 을(를) {1}만큼 이동 명령을 보냈습니다.", axisIndex, step * direction));
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
        private void ShowZMapImage(ZMapResult result)
        {
            var heightMapBitmap = BuildGrayscaleImage(result.Width, result.Height, result.ZData);

            Invoke(new Action(() =>
            {
                picHeightMap.Image?.Dispose();
                picHeightMap.Image = heightMapBitmap;
            }));
        }

        // ZMap과 같은 촬영에서 함께 나오는 Intensity(밝기) 채널을 표시한다.
        // 카메라가 보는 반사광 세기라서, 원본 Live 화면과 비슷한 느낌으로 형상을 확인할 수 있다.
        // 촬영이 끝난 시점(높이맵과 동시)에 갱신되며, 이동 중 실시간으로 갱신되지는 않는다.
        private void ShowIntensityImage(ZMapResult result)
        {
            var intensityBitmap = BuildGrayscaleImage(result.Width, result.Height, result.IntensityData);

            Invoke(new Action(() =>
            {
                picIntensity.Image?.Dispose();
                picIntensity.Image = intensityBitmap;
            }));
        }

        // 반복동작(아래 참고)은 ACS 이동만 하고 SmartRay 센서를 전혀 건드리지 않으므로, Live와 동시에 켜도 된다.
        // 그래서 여기서는 btnRepeat을 건드리지 않고, 센서를 같이 쓰는 btnScanStart(실제 촬상)만 막는다.
        private void btnLiveStart_Click(object sender, EventArgs e)
        {
            btnLiveStart.Enabled = false;
            btnScanStart.Enabled = false;
            try
            {
                Log("Live 화면을 시작합니다 (레이저 ON)...");
                _smartRay.StartLiveView(ShowLiveImage, step => LogSafe("  단계: " + step));
                btnLiveStop.Enabled = true;
            }
            catch (InvalidOperationException ex)
            {
                Log(ex.Message);
                btnLiveStart.Enabled = true;
                btnScanStart.Enabled = true;
            }
            catch (SmartRayApiException ex)
            {
                Log("Live 시작 실패 (SmartRayApiException): " + ex.Message);
                btnLiveStart.Enabled = true;
                btnScanStart.Enabled = true;
            }
        }

        private void btnLiveStop_Click(object sender, EventArgs e)
        {
            _smartRay.StopLiveView();
            Log("Live 화면을 정지했습니다 (레이저 OFF).");
            btnLiveStart.Enabled = true;
            btnScanStart.Enabled = true;
            btnLiveStop.Enabled = false;
        }

        // 백그라운드 스레드(SmartRay 이벤트)에서 호출된다. 비트맵을 만든 뒤 UI 스레드로 넘겨서 PictureBox에 표시한다.
        private void ShowLiveImage(LiveImageFrame frame)
        {
            var bitmap = BuildLiveImageBitmap(frame);

            if (!IsHandleCreated || IsDisposed)
                return;

            try
            {
                Invoke(new Action(() =>
                {
                    picLiveImage.Image?.Dispose();
                    picLiveImage.Image = bitmap;
                }));
            }
            catch (ObjectDisposedException)
            {
                // 폼이 닫히는 도중 마지막 프레임이 도착한 경우는 조용히 무시한다.
            }
            catch (InvalidOperationException)
            {
                // 핸들이 이미 파괴된 경우도 마찬가지.
            }
        }

        // Live Image는 3D 계산 전 원본 그레이스케일 이미지(1바이트/픽셀)라서, 8bpp Indexed + 흑백 팔레트로 그대로 옮긴다.
        private static Bitmap BuildLiveImageBitmap(LiveImageFrame frame)
        {
            int w = Math.Max(1, frame.Width);
            int h = Math.Max(1, frame.Height);
            var bitmap = new Bitmap(w, h, PixelFormat.Format8bppIndexed);

            var palette = bitmap.Palette;
            for (int i = 0; i < 256; i++)
                palette.Entries[i] = Color.FromArgb(i, i, i);
            bitmap.Palette = palette;

            if (frame.Data == null || frame.Data.Length < w * h)
                return bitmap;

            var bmpData = bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            try
            {
                for (int y = 0; y < h; y++)
                {
                    IntPtr rowPtr = IntPtr.Add(bmpData.Scan0, y * bmpData.Stride);
                    Marshal.Copy(frame.Data, y * w, rowPtr, w);
                }
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }

            return bitmap;
        }

        // ZMap/Intensity는 이미 Width x Height 격자로 정렬되어 있으므로, 흩뿌릴 필요 없이 그대로 그레이스케일로 매핑한다.
        private static Bitmap BuildGrayscaleImage(int width, int height, ushort[] data)
        {
            int w = Math.Max(1, width);
            int h = Math.Max(1, height);
            var bitmap = new Bitmap(w, h);

            using (var g = Graphics.FromImage(bitmap))
                g.Clear(Color.Black);

            if (data == null || data.Length < width * height)
                return bitmap;

            ushort minRaw = ushort.MaxValue, maxRaw = 0;
            foreach (var v in data)
            {
                if (v == 0)
                    continue;
                if (v < minRaw) minRaw = v;
                if (v > maxRaw) maxRaw = v;
            }

            if (maxRaw <= minRaw)
                return bitmap;

            float range = maxRaw - minRaw;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    ushort v = data[y * width + x];
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

            Task.Run(() => RunOneLeg(axisIndex, startPos, ScanEndPosition))
                .ContinueWith(t => Invoke(new Action(() => SetContinuousScanRunningState(false))));
        }

        private void btnScanStop_Click(object sender, EventArgs e)
        {
            // 이동 중일 수 있으므로 즉시 정지시키고, 레이저도 반드시 끈다.
            // 반복동작 진행 중이었다면 다음 왕복으로 넘어가지 않도록 플래그도 내린다.
            int axisIndex = (int)numMoveAxis.Value;
            _repeatRunning = false;
            _acs.Stop(axisIndex);
            _smartRay.StopZMapCapture();
            Log("[촬상] 비상 정지: 모터 정지 + 레이저 OFF");
        }

        private void SetContinuousScanRunningState(bool running)
        {
            btnScanStart.Enabled = !running;
            btnScanStop.Enabled = running;
            // 촬영 중에는 Live 화면/반복동작을 켤 수 없다 (센서가 한 번에 하나의 모드만 가능).
            btnLiveStart.Enabled = !running;
            btnRepeat.Enabled = !running;
        }

        // "반복동작" 버튼 하나로 시작/정지를 토글한다. 시작거리 ↔ 끝거리 입력창(txtRepeatStartPos/txtRepeatEndPos)을
        // 별도로 두어, 위쪽 "동작" 그룹의 시작위치/촬상과는 독립적으로 왕복 구간을 설정할 수 있다.
        private volatile bool _repeatRunning;

        private void btnRepeat_Click(object sender, EventArgs e)
        {
            if (_repeatRunning)
            {
                _repeatRunning = false;
                Log("[반복동작] 정지를 요청했습니다. 현재 진행 중인 왕복이 끝나는 대로 멈춥니다...");
                return;
            }

            if (!double.TryParse(txtRepeatStartPos.Text.Trim(), out double startPos))
            {
                Log("반복동작 시작거리 값이 올바르지 않습니다: " + txtRepeatStartPos.Text);
                return;
            }

            if (!double.TryParse(txtRepeatEndPos.Text.Trim(), out double endPos))
            {
                Log("반복동작 끝거리 값이 올바르지 않습니다: " + txtRepeatEndPos.Text);
                return;
            }

            if (startPos == endPos)
            {
                Log("반복동작 시작거리와 끝거리가 같습니다.");
                return;
            }

            int axisIndex = (int)numMoveAxis.Value;

            _repeatRunning = true;
            SetRepeatRunningState(true);

            Task.Run(() => RunRepeatMove(axisIndex, startPos, endPos))
                .ContinueWith(t => Invoke(new Action(() => SetRepeatRunningState(false))));
        }

        // 반복동작은 ACS 이동만 반복할 뿐 SmartRay 촬영은 하지 않으므로, Live 화면과는 동시에 켤 수 있다
        // (촬상만 축을 같이 쓰기 때문에 막는다).
        private void SetRepeatRunningState(bool running)
        {
            btnRepeat.Text = running ? "반복 동작 정지" : "반복 동작 시작";
            btnRepeat.BackColor = running ? Color.Firebrick : Color.MediumSeaGreen;
            btnScanStart.Enabled = !running;
            btnScanStop.Enabled = running;
        }

        // 시작거리 ↔ 끝거리 사이를 ACS 이동만으로 계속 왕복한다 (SmartRay 촬영 없음).
        // Live 화면을 켜놓은 상태에서 대상물이 지나가는 모습만 확인하고 싶을 때 쓰는 용도다.
        private void RunRepeatMove(int axisIndex, double posA, double posB)
        {
            LogSafe(string.Format("[반복동작] 시작: {0:0.###} ↔ {1:0.###} 왕복 (ACS 이동만, 촬영 없음)", posA, posB));

            bool goingToB = true;
            while (_repeatRunning)
            {
                double target = goingToB ? posB : posA;
                try
                {
                    LogSafe(string.Format("[반복동작] {0:0.###}(으)로 이동합니다...", target));
                    _acs.MoveAbsolute(axisIndex, target);
                    if (!_acs.WaitForInPosition(axisIndex, 180000))
                    {
                        LogSafe("[반복동작] 이동이 시간 안에 끝나지 않아 중단합니다.");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    LogSafe(string.Format("[반복동작] 오류 발생 ({0}): {1}", ex.GetType().Name, ex.Message));
                    break;
                }

                goingToB = !goingToB;
            }

            _repeatRunning = false;
            LogSafe("[반복동작] 정지되었습니다.");
        }

        // "촬상 시작" 버튼 전용: 편도 1회(시작위치 이동 → 레이저 ON → 촬영하며 끝위치까지 이동 → 결과 표시)를 수행한다.
        // 백그라운드 스레드에서 실행된다. UI 컨트롤은 절대 직접 건드리지 말고 LogSafe로만 로그를 남긴다.
        // 순서가 중요하다: 시작 위치로 이동(레이저 꺼짐) → SmartRay를 먼저 촬영 대기 상태로 만들어 레이저를 켠 뒤 →
        // 그 상태로 끝 위치까지 이동한다. 그래야 이동하는 내내 레이저가 켜진 채로 촬영된다.
        // 주의: SmartRay 센서는 한 번에 하나의 모드(ZMap/Live 등)만 돌릴 수 있어서, 별도 Live 스트리밍을 동시에
        // 켜면 ZMap 쪽 설정 명령이 멈춰버린다 (실제로 이 문제로 촬영이 안 되는 걸 확인함). 그래서 이동 중 화면은
        // 별도 Live 모드 대신, ZMap 촬영과 같이 나오는 Intensity(밝기) 채널을 촬영 완료 시점에 표시한다.
        // (반복동작은 촬영 없이 ACS 이동만 하므로 이 메서드를 쓰지 않는다 - RunRepeatMove 참고.)
        private bool RunOneLeg(int axisIndex, double startPos, double endPos)
        {
            try
            {
                LogSafe(string.Format("[촬상] 시작 위치({0:0.###})로 이동합니다...", startPos));
                _acs.MoveAbsolute(axisIndex, startPos);
                if (!_acs.WaitForInPosition(axisIndex, 30000))
                {
                    LogSafe("[촬상] 시작 위치 이동이 시간 안에 끝나지 않아 중단합니다.");
                    return false;
                }

                LogSafe("[촬상] SmartRay 촬영을 시작합니다 (레이저 ON)...");
                _smartRay.ArmZMapCapture(ScanGrabProfileCount, step => LogSafe("  단계: " + step));

                try
                {
                    LogSafe(string.Format("[촬상] 레이저를 켠 채로 끝 위치({0:0.###})까지 이동합니다...", endPos));
                    _acs.MoveAbsolute(axisIndex, endPos);

                    bool moveDone = _acs.WaitForInPosition(axisIndex, 180000);
                    double actualPos = _acs.GetAxisPosition(axisIndex);
                    if (!moveDone)
                    {
                        LogSafe(string.Format("[촬상] 경고: 이동이 제한 시간(180초) 안에 끝나지 않았습니다. 현재 위치: {0:0.###}", actualPos));
                        return false;
                    }

                    LogSafe(string.Format("[촬상] 이동 완료. 실제 위치: {0:0.###}. 남은 촬영 데이터를 대기합니다...", actualPos));

                    var result = _smartRay.WaitForZMapCapture(15000);
                    if (result == null)
                    {
                        LogSafe("[촬상] 촬영 데이터를 받지 못했습니다 (타임아웃).");
                        return false;
                    }

                    var s = AnalyzeZMap(result);
                    LogSafe(string.Format("[촬상] 완료: {0}x{1}, 전체 {2}점 중 유효 {3}점, raw Z:[{4}, {5}]",
                        result.Width, result.Height, s.total, s.valid, s.minRaw, s.maxRaw));

                    if (s.valid > 0)
                    {
                        ShowZMapImage(result);
                        ShowIntensityImage(result);
                    }

                    return true;
                }
                finally
                {
                    _smartRay.StopZMapCapture();
                }
            }
            catch (Exception ex)
            {
                LogSafe(string.Format("[촬상] 오류 발생 ({0}): {1}", ex.GetType().Name, ex.Message));
                return false;
            }
        }

        private void LogSafe(string message)
        {
            if (InvokeRequired)
                Invoke(new Action(() => Log(message)));
            else
                Log(message);
        }

        // 닫기(X) 버튼을 포함해 폼이 어떤 식으로 닫히든 항상 호출된다.
        // 반복동작/촬상이 진행 중이었을 수 있으니 먼저 확실히 멈춘 뒤, ACS/SmartRay 연결을 해제하고 종료한다.
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _repeatRunning = false;

            if (_acs.IsConnected)
            {
                try
                {
                    _acs.Stop((int)numMoveAxis.Value);
                }
                catch (Exception)
                {
                    // 종료 중이므로 정지 명령 실패는 무시하고 계속 진행한다.
                }
            }

            // 종료 경로라 여기서 예외가 나면 안 되므로, 혹시 모를 SDK 쪽 오류는 로그만 남기고 넘어간다.
            try
            {
                _smartRay.StopZMapCapture();
                _smartRay.StopLiveView();
            }
            catch (Exception ex)
            {
                Log(string.Format("종료 중 촬영/Live 정지 실패 ({0}): {1}", ex.GetType().Name, ex.Message));
            }

            try
            {
                _acs.Dispose();
                _smartRay.Dispose();
            }
            catch (Exception ex)
            {
                Log(string.Format("종료 중 연결 해제 실패 ({0}): {1}", ex.GetType().Name, ex.Message));
            }

            Log("ACS/SmartRay 연결을 해제하고 프로그램을 종료합니다.");
        }

        private void Log(string message)
        {
            txtLog.AppendText(string.Format("[{0:HH:mm:ss}] {1}{2}", DateTime.Now, message, Environment.NewLine));
        }
    }
}
