namespace _3DVision
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpAcs = new System.Windows.Forms.GroupBox();
            this.btnJogPlus = new System.Windows.Forms.Button();
            this.btnJogMinus = new System.Windows.Forms.Button();
            this.txtJogStep = new System.Windows.Forms.TextBox();
            this.lblJogStep = new System.Windows.Forms.Label();
            this.lblAcsStatus = new System.Windows.Forms.Label();
            this.btnSetupPeg = new System.Windows.Forms.Button();
            this.btnHomeAxis = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.lblIp = new System.Windows.Forms.Label();
            this.grpSmartRay = new System.Windows.Forms.GroupBox();
            this.lblSmartRayStatus = new System.Windows.Forms.Label();
            this.btnSrDisconnect = new System.Windows.Forms.Button();
            this.btnSrConnect = new System.Windows.Forms.Button();
            this.txtSrPort = new System.Windows.Forms.TextBox();
            this.lblSrPort = new System.Windows.Forms.Label();
            this.txtSrIp = new System.Windows.Forms.TextBox();
            this.lblSrIp = new System.Windows.Forms.Label();
            this.grpControl = new System.Windows.Forms.GroupBox();
            this.btnScanStop = new System.Windows.Forms.Button();
            this.btnScanStart = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.numMoveAxis = new System.Windows.Forms.NumericUpDown();
            this.lblMoveAxis = new System.Windows.Forms.Label();
            this.txtStartPos = new System.Windows.Forms.TextBox();
            this.lblStartPos = new System.Windows.Forms.Label();
            this.lblHeightMap = new System.Windows.Forms.Label();
            this.picHeightMap = new System.Windows.Forms.PictureBox();
            this.lblIntensity = new System.Windows.Forms.Label();
            this.picIntensity = new System.Windows.Forms.PictureBox();
            this.lblLive = new System.Windows.Forms.Label();
            this.btnLiveStart = new System.Windows.Forms.Button();
            this.btnLiveStop = new System.Windows.Forms.Button();
            this.lblRepeatStart = new System.Windows.Forms.Label();
            this.txtRepeatStartPos = new System.Windows.Forms.TextBox();
            this.lblRepeatEnd = new System.Windows.Forms.Label();
            this.txtRepeatEndPos = new System.Windows.Forms.TextBox();
            this.btnRepeat = new System.Windows.Forms.Button();
            this.btnTriggerGrab = new System.Windows.Forms.Button();
            this.picLiveImage = new System.Windows.Forms.PictureBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grpAcs.SuspendLayout();
            this.grpSmartRay.SuspendLayout();
            this.grpControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMoveAxis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHeightMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLiveImage)).BeginInit();
            this.SuspendLayout();
            //
            // grpAcs
            //
            this.grpAcs.Controls.Add(this.btnHomeAxis);
            this.grpAcs.Controls.Add(this.btnSetupPeg);
            this.grpAcs.Controls.Add(this.btnJogPlus);
            this.grpAcs.Controls.Add(this.btnJogMinus);
            this.grpAcs.Controls.Add(this.txtJogStep);
            this.grpAcs.Controls.Add(this.lblJogStep);
            this.grpAcs.Controls.Add(this.lblAcsStatus);
            this.grpAcs.Controls.Add(this.btnDisconnect);
            this.grpAcs.Controls.Add(this.btnConnect);
            this.grpAcs.Controls.Add(this.txtIp);
            this.grpAcs.Controls.Add(this.lblIp);
            this.grpAcs.Location = new System.Drawing.Point(12, 12);
            this.grpAcs.Name = "grpAcs";
            this.grpAcs.Size = new System.Drawing.Size(340, 230);
            this.grpAcs.TabIndex = 0;
            this.grpAcs.TabStop = false;
            this.grpAcs.Text = "ACS 연결";
            //
            // lblIp
            //
            this.lblIp.AutoSize = true;
            this.lblIp.Location = new System.Drawing.Point(10, 28);
            this.lblIp.Name = "lblIp";
            this.lblIp.Size = new System.Drawing.Size(74, 12);
            this.lblIp.TabIndex = 0;
            this.lblIp.Text = "컨트롤러 IP";
            //
            // txtIp
            //
            this.txtIp.Location = new System.Drawing.Point(95, 25);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(150, 21);
            this.txtIp.TabIndex = 1;
            this.txtIp.Text = "10.0.0.100";
            //
            // btnConnect
            //
            this.btnConnect.Location = new System.Drawing.Point(10, 55);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 30);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "연결";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            //
            // btnDisconnect
            //
            this.btnDisconnect.Location = new System.Drawing.Point(120, 55);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(100, 30);
            this.btnDisconnect.TabIndex = 3;
            this.btnDisconnect.Text = "연결 해제";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            //
            // lblJogStep
            //
            this.lblJogStep.AutoSize = true;
            this.lblJogStep.Location = new System.Drawing.Point(10, 98);
            this.lblJogStep.Name = "lblJogStep";
            this.lblJogStep.Size = new System.Drawing.Size(76, 12);
            this.lblJogStep.TabIndex = 4;
            this.lblJogStep.Text = "이동 단위(mm)";
            //
            // txtJogStep
            //
            this.txtJogStep.Location = new System.Drawing.Point(100, 95);
            this.txtJogStep.Name = "txtJogStep";
            this.txtJogStep.Size = new System.Drawing.Size(60, 21);
            this.txtJogStep.TabIndex = 5;
            this.txtJogStep.Text = "5";
            //
            // btnJogMinus
            //
            this.btnJogMinus.Location = new System.Drawing.Point(170, 93);
            this.btnJogMinus.Name = "btnJogMinus";
            this.btnJogMinus.Size = new System.Drawing.Size(65, 26);
            this.btnJogMinus.TabIndex = 6;
            this.btnJogMinus.Text = "◀ -";
            this.btnJogMinus.UseVisualStyleBackColor = true;
            this.btnJogMinus.Click += new System.EventHandler(this.btnJogMinus_Click);
            //
            // btnJogPlus
            //
            this.btnJogPlus.Location = new System.Drawing.Point(240, 93);
            this.btnJogPlus.Name = "btnJogPlus";
            this.btnJogPlus.Size = new System.Drawing.Size(65, 26);
            this.btnJogPlus.TabIndex = 7;
            this.btnJogPlus.Text = "+ ▶";
            this.btnJogPlus.UseVisualStyleBackColor = true;
            this.btnJogPlus.Click += new System.EventHandler(this.btnJogPlus_Click);
            //
            // lblAcsStatus
            //
            this.lblAcsStatus.AutoSize = true;
            this.lblAcsStatus.ForeColor = System.Drawing.Color.Firebrick;
            this.lblAcsStatus.Location = new System.Drawing.Point(10, 198);
            this.lblAcsStatus.Name = "lblAcsStatus";
            this.lblAcsStatus.Size = new System.Drawing.Size(60, 12);
            this.lblAcsStatus.TabIndex = 8;
            this.lblAcsStatus.Text = "ACS: -";
            //
            // btnSetupPeg
            //
            this.btnSetupPeg.Location = new System.Drawing.Point(10, 125);
            this.btnSetupPeg.Name = "btnSetupPeg";
            this.btnSetupPeg.Size = new System.Drawing.Size(320, 30);
            this.btnSetupPeg.TabIndex = 9;
            this.btnSetupPeg.Text = "PEG 트리거 설정 (axis 0 → axis 4, Input1)";
            this.btnSetupPeg.UseVisualStyleBackColor = true;
            this.btnSetupPeg.Click += new System.EventHandler(this.btnSetupPeg_Click);
            //
            // btnHomeAxis
            //
            this.btnHomeAxis.Location = new System.Drawing.Point(10, 161);
            this.btnHomeAxis.Name = "btnHomeAxis";
            this.btnHomeAxis.Size = new System.Drawing.Size(320, 30);
            this.btnHomeAxis.TabIndex = 10;
            this.btnHomeAxis.Text = "축 0 홈잡기 (Buffer 0 실행)";
            this.btnHomeAxis.UseVisualStyleBackColor = true;
            this.btnHomeAxis.Click += new System.EventHandler(this.btnHomeAxis_Click);
            //
            // grpSmartRay
            //
            this.grpSmartRay.Controls.Add(this.lblSmartRayStatus);
            this.grpSmartRay.Controls.Add(this.btnSrDisconnect);
            this.grpSmartRay.Controls.Add(this.btnSrConnect);
            this.grpSmartRay.Controls.Add(this.txtSrPort);
            this.grpSmartRay.Controls.Add(this.lblSrPort);
            this.grpSmartRay.Controls.Add(this.txtSrIp);
            this.grpSmartRay.Controls.Add(this.lblSrIp);
            this.grpSmartRay.Location = new System.Drawing.Point(12, 252);
            this.grpSmartRay.Name = "grpSmartRay";
            this.grpSmartRay.Size = new System.Drawing.Size(340, 115);
            this.grpSmartRay.TabIndex = 1;
            this.grpSmartRay.TabStop = false;
            this.grpSmartRay.Text = "SmartRay 연결";
            //
            // lblSrIp
            //
            this.lblSrIp.AutoSize = true;
            this.lblSrIp.Location = new System.Drawing.Point(10, 28);
            this.lblSrIp.Name = "lblSrIp";
            this.lblSrIp.Size = new System.Drawing.Size(19, 12);
            this.lblSrIp.TabIndex = 0;
            this.lblSrIp.Text = "IP";
            //
            // txtSrIp
            //
            this.txtSrIp.Location = new System.Drawing.Point(35, 25);
            this.txtSrIp.Name = "txtSrIp";
            this.txtSrIp.Size = new System.Drawing.Size(140, 21);
            this.txtSrIp.TabIndex = 1;
            this.txtSrIp.Text = "192.168.111.200";
            //
            // lblSrPort
            //
            this.lblSrPort.AutoSize = true;
            this.lblSrPort.Location = new System.Drawing.Point(185, 28);
            this.lblSrPort.Name = "lblSrPort";
            this.lblSrPort.Size = new System.Drawing.Size(29, 12);
            this.lblSrPort.TabIndex = 2;
            this.lblSrPort.Text = "포트";
            //
            // txtSrPort
            //
            this.txtSrPort.Location = new System.Drawing.Point(220, 25);
            this.txtSrPort.Name = "txtSrPort";
            this.txtSrPort.Size = new System.Drawing.Size(50, 21);
            this.txtSrPort.TabIndex = 3;
            this.txtSrPort.Text = "40";
            //
            // btnSrConnect
            //
            this.btnSrConnect.Location = new System.Drawing.Point(10, 55);
            this.btnSrConnect.Name = "btnSrConnect";
            this.btnSrConnect.Size = new System.Drawing.Size(100, 30);
            this.btnSrConnect.TabIndex = 4;
            this.btnSrConnect.Text = "연결";
            this.btnSrConnect.UseVisualStyleBackColor = true;
            this.btnSrConnect.Click += new System.EventHandler(this.btnSrConnect_Click);
            //
            // btnSrDisconnect
            //
            this.btnSrDisconnect.Location = new System.Drawing.Point(120, 55);
            this.btnSrDisconnect.Name = "btnSrDisconnect";
            this.btnSrDisconnect.Size = new System.Drawing.Size(100, 30);
            this.btnSrDisconnect.TabIndex = 5;
            this.btnSrDisconnect.Text = "연결 해제";
            this.btnSrDisconnect.UseVisualStyleBackColor = true;
            this.btnSrDisconnect.Click += new System.EventHandler(this.btnSrDisconnect_Click);
            //
            // lblSmartRayStatus
            //
            this.lblSmartRayStatus.AutoSize = true;
            this.lblSmartRayStatus.ForeColor = System.Drawing.Color.Firebrick;
            this.lblSmartRayStatus.Location = new System.Drawing.Point(10, 92);
            this.lblSmartRayStatus.Name = "lblSmartRayStatus";
            this.lblSmartRayStatus.Size = new System.Drawing.Size(90, 12);
            this.lblSmartRayStatus.TabIndex = 6;
            this.lblSmartRayStatus.Text = "SmartRay: -";
            //
            // grpControl
            //
            this.grpControl.Controls.Add(this.btnScanStop);
            this.grpControl.Controls.Add(this.btnScanStart);
            this.grpControl.Controls.Add(this.btnHome);
            this.grpControl.Controls.Add(this.numMoveAxis);
            this.grpControl.Controls.Add(this.lblMoveAxis);
            this.grpControl.Controls.Add(this.txtStartPos);
            this.grpControl.Controls.Add(this.lblStartPos);
            this.grpControl.Location = new System.Drawing.Point(12, 377);
            this.grpControl.Name = "grpControl";
            this.grpControl.Size = new System.Drawing.Size(340, 180);
            this.grpControl.TabIndex = 2;
            this.grpControl.TabStop = false;
            this.grpControl.Text = "동작 (시작위치 / Home / 촬상 / 중지)";
            //
            // lblStartPos
            //
            this.lblStartPos.AutoSize = true;
            this.lblStartPos.Location = new System.Drawing.Point(10, 28);
            this.lblStartPos.Name = "lblStartPos";
            this.lblStartPos.Size = new System.Drawing.Size(70, 12);
            this.lblStartPos.TabIndex = 0;
            this.lblStartPos.Text = "시작 위치(mm)";
            //
            // txtStartPos
            //
            this.txtStartPos.Location = new System.Drawing.Point(100, 25);
            this.txtStartPos.Name = "txtStartPos";
            this.txtStartPos.Size = new System.Drawing.Size(90, 21);
            this.txtStartPos.TabIndex = 1;
            this.txtStartPos.Text = "-18.779";
            //
            // lblMoveAxis
            //
            this.lblMoveAxis.AutoSize = true;
            this.lblMoveAxis.Location = new System.Drawing.Point(10, 58);
            this.lblMoveAxis.Name = "lblMoveAxis";
            this.lblMoveAxis.Size = new System.Drawing.Size(58, 12);
            this.lblMoveAxis.TabIndex = 2;
            this.lblMoveAxis.Text = "이동할 축";
            //
            // numMoveAxis
            //
            this.numMoveAxis.Location = new System.Drawing.Point(100, 55);
            this.numMoveAxis.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            this.numMoveAxis.Name = "numMoveAxis";
            this.numMoveAxis.Size = new System.Drawing.Size(60, 21);
            this.numMoveAxis.TabIndex = 3;
            //
            // btnHome
            //
            this.btnHome.Location = new System.Drawing.Point(10, 90);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(320, 34);
            this.btnHome.TabIndex = 4;
            this.btnHome.Text = "Home (시작 위치로)";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            //
            // btnScanStart
            //
            this.btnScanStart.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnScanStart.Location = new System.Drawing.Point(10, 132);
            this.btnScanStart.Name = "btnScanStart";
            this.btnScanStart.Size = new System.Drawing.Size(155, 36);
            this.btnScanStart.TabIndex = 5;
            this.btnScanStart.Text = "촬상 시작";
            this.btnScanStart.UseVisualStyleBackColor = false;
            this.btnScanStart.Click += new System.EventHandler(this.btnScanStart_Click);
            //
            // btnScanStop
            //
            this.btnScanStop.BackColor = System.Drawing.Color.Firebrick;
            this.btnScanStop.ForeColor = System.Drawing.Color.White;
            this.btnScanStop.Enabled = false;
            this.btnScanStop.Location = new System.Drawing.Point(175, 132);
            this.btnScanStop.Name = "btnScanStop";
            this.btnScanStop.Size = new System.Drawing.Size(155, 36);
            this.btnScanStop.TabIndex = 6;
            this.btnScanStop.Text = "중지";
            this.btnScanStop.UseVisualStyleBackColor = false;
            this.btnScanStop.Click += new System.EventHandler(this.btnScanStop_Click);
            //
            // lblHeightMap
            //
            this.lblHeightMap.AutoSize = true;
            this.lblHeightMap.Location = new System.Drawing.Point(372, 12);
            this.lblHeightMap.Name = "lblHeightMap";
            this.lblHeightMap.Size = new System.Drawing.Size(100, 12);
            this.lblHeightMap.TabIndex = 3;
            this.lblHeightMap.Text = "높이맵 (그레이스케일)";
            //
            // picHeightMap
            //
            this.picHeightMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.picHeightMap.BackColor = System.Drawing.Color.Black;
            this.picHeightMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picHeightMap.Location = new System.Drawing.Point(372, 28);
            this.picHeightMap.Name = "picHeightMap";
            this.picHeightMap.Size = new System.Drawing.Size(440, 600);
            this.picHeightMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHeightMap.TabIndex = 4;
            this.picHeightMap.TabStop = false;
            //
            // lblIntensity
            //
            this.lblIntensity.AutoSize = true;
            this.lblIntensity.Location = new System.Drawing.Point(832, 12);
            this.lblIntensity.Name = "lblIntensity";
            this.lblIntensity.Size = new System.Drawing.Size(150, 12);
            this.lblIntensity.TabIndex = 5;
            this.lblIntensity.Text = "Intensity 화면 (촬상 완료 시 갱신)";
            //
            // picIntensity
            //
            this.picIntensity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.picIntensity.BackColor = System.Drawing.Color.Black;
            this.picIntensity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picIntensity.Location = new System.Drawing.Point(832, 28);
            this.picIntensity.Name = "picIntensity";
            this.picIntensity.Size = new System.Drawing.Size(440, 600);
            this.picIntensity.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIntensity.TabIndex = 6;
            this.picIntensity.TabStop = false;
            //
            // lblLive
            //
            this.lblLive.AutoSize = true;
            this.lblLive.Location = new System.Drawing.Point(1292, 12);
            this.lblLive.Name = "lblLive";
            this.lblLive.Size = new System.Drawing.Size(150, 12);
            this.lblLive.TabIndex = 7;
            this.lblLive.Text = "Live 화면 (원본 카메라, 촬상 중엔 사용 불가)";
            //
            // btnLiveStart
            //
            this.btnLiveStart.BackColor = System.Drawing.Color.Goldenrod;
            this.btnLiveStart.Location = new System.Drawing.Point(1292, 28);
            this.btnLiveStart.Name = "btnLiveStart";
            this.btnLiveStart.Size = new System.Drawing.Size(110, 26);
            this.btnLiveStart.TabIndex = 8;
            this.btnLiveStart.Text = "Live 시작";
            this.btnLiveStart.UseVisualStyleBackColor = false;
            this.btnLiveStart.Click += new System.EventHandler(this.btnLiveStart_Click);
            //
            // btnLiveStop
            //
            this.btnLiveStop.Enabled = false;
            this.btnLiveStop.Location = new System.Drawing.Point(1408, 28);
            this.btnLiveStop.Name = "btnLiveStop";
            this.btnLiveStop.Size = new System.Drawing.Size(90, 26);
            this.btnLiveStop.TabIndex = 9;
            this.btnLiveStop.Text = "Live 정지";
            this.btnLiveStop.UseVisualStyleBackColor = true;
            this.btnLiveStop.Click += new System.EventHandler(this.btnLiveStop_Click);
            //
            // lblRepeatStart
            //
            this.lblRepeatStart.AutoSize = true;
            this.lblRepeatStart.Location = new System.Drawing.Point(1292, 61);
            this.lblRepeatStart.Name = "lblRepeatStart";
            this.lblRepeatStart.Size = new System.Drawing.Size(46, 12);
            this.lblRepeatStart.TabIndex = 10;
            this.lblRepeatStart.Text = "시작거리";
            //
            // txtRepeatStartPos
            //
            this.txtRepeatStartPos.Location = new System.Drawing.Point(1350, 58);
            this.txtRepeatStartPos.Name = "txtRepeatStartPos";
            this.txtRepeatStartPos.Size = new System.Drawing.Size(75, 21);
            this.txtRepeatStartPos.TabIndex = 11;
            this.txtRepeatStartPos.Text = "-18.779";
            //
            // lblRepeatEnd
            //
            this.lblRepeatEnd.AutoSize = true;
            this.lblRepeatEnd.Location = new System.Drawing.Point(1435, 61);
            this.lblRepeatEnd.Name = "lblRepeatEnd";
            this.lblRepeatEnd.Size = new System.Drawing.Size(40, 12);
            this.lblRepeatEnd.TabIndex = 12;
            this.lblRepeatEnd.Text = "끝거리";
            //
            // txtRepeatEndPos
            //
            this.txtRepeatEndPos.Location = new System.Drawing.Point(1487, 58);
            this.txtRepeatEndPos.Name = "txtRepeatEndPos";
            this.txtRepeatEndPos.Size = new System.Drawing.Size(75, 21);
            this.txtRepeatEndPos.TabIndex = 13;
            this.txtRepeatEndPos.Text = "130.221";
            //
            // btnRepeat
            //
            this.btnRepeat.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnRepeat.Location = new System.Drawing.Point(1292, 86);
            this.btnRepeat.Name = "btnRepeat";
            this.btnRepeat.Size = new System.Drawing.Size(440, 30);
            this.btnRepeat.TabIndex = 14;
            this.btnRepeat.Text = "반복 동작 시작";
            this.btnRepeat.UseVisualStyleBackColor = false;
            this.btnRepeat.Click += new System.EventHandler(this.btnRepeat_Click);
            //
            // btnTriggerGrab
            //
            this.btnTriggerGrab.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnTriggerGrab.Location = new System.Drawing.Point(1292, 120);
            this.btnTriggerGrab.Name = "btnTriggerGrab";
            this.btnTriggerGrab.Size = new System.Drawing.Size(440, 30);
            this.btnTriggerGrab.TabIndex = 15;
            this.btnTriggerGrab.Text = "트리거 그랩 시작 (Input1)";
            this.btnTriggerGrab.UseVisualStyleBackColor = false;
            this.btnTriggerGrab.Click += new System.EventHandler(this.btnTriggerGrab_Click);
            //
            // picLiveImage
            //
            this.picLiveImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picLiveImage.BackColor = System.Drawing.Color.Black;
            this.picLiveImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLiveImage.Location = new System.Drawing.Point(1292, 156);
            this.picLiveImage.Name = "picLiveImage";
            this.picLiveImage.Size = new System.Drawing.Size(440, 472);
            this.picLiveImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLiveImage.TabIndex = 16;
            this.picLiveImage.TabStop = false;
            //
            // lblLog
            //
            this.lblLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(13, 650);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(24, 12);
            this.lblLog.TabIndex = 11;
            this.lblLog.Text = "로그";
            //
            // txtLog
            //
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(12, 666);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1720, 210);
            this.txtLog.TabIndex = 12;
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1752, 890);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.picLiveImage);
            this.Controls.Add(this.btnTriggerGrab);
            this.Controls.Add(this.btnRepeat);
            this.Controls.Add(this.txtRepeatEndPos);
            this.Controls.Add(this.lblRepeatEnd);
            this.Controls.Add(this.txtRepeatStartPos);
            this.Controls.Add(this.lblRepeatStart);
            this.Controls.Add(this.btnLiveStop);
            this.Controls.Add(this.btnLiveStart);
            this.Controls.Add(this.lblLive);
            this.Controls.Add(this.picIntensity);
            this.Controls.Add(this.lblIntensity);
            this.Controls.Add(this.picHeightMap);
            this.Controls.Add(this.lblHeightMap);
            this.Controls.Add(this.grpControl);
            this.Controls.Add(this.grpSmartRay);
            this.Controls.Add(this.grpAcs);
            this.Name = "MainForm";
            this.Text = "3DVision - ACS/SmartRay 연동 테스트";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpAcs.ResumeLayout(false);
            this.grpAcs.PerformLayout();
            this.grpSmartRay.ResumeLayout(false);
            this.grpSmartRay.PerformLayout();
            this.grpControl.ResumeLayout(false);
            this.grpControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMoveAxis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHeightMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLiveImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpAcs;
        private System.Windows.Forms.Label lblIp;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lblJogStep;
        private System.Windows.Forms.TextBox txtJogStep;
        private System.Windows.Forms.Button btnJogMinus;
        private System.Windows.Forms.Button btnJogPlus;
        private System.Windows.Forms.Label lblAcsStatus;
        private System.Windows.Forms.Button btnSetupPeg;
        private System.Windows.Forms.Button btnHomeAxis;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox grpSmartRay;
        private System.Windows.Forms.Label lblSrIp;
        private System.Windows.Forms.TextBox txtSrIp;
        private System.Windows.Forms.Label lblSrPort;
        private System.Windows.Forms.TextBox txtSrPort;
        private System.Windows.Forms.Button btnSrConnect;
        private System.Windows.Forms.Button btnSrDisconnect;
        private System.Windows.Forms.Label lblSmartRayStatus;
        private System.Windows.Forms.GroupBox grpControl;
        private System.Windows.Forms.Label lblStartPos;
        private System.Windows.Forms.TextBox txtStartPos;
        private System.Windows.Forms.Label lblMoveAxis;
        private System.Windows.Forms.NumericUpDown numMoveAxis;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnScanStart;
        private System.Windows.Forms.Button btnScanStop;
        private System.Windows.Forms.Label lblHeightMap;
        private System.Windows.Forms.PictureBox picHeightMap;
        private System.Windows.Forms.Label lblIntensity;
        private System.Windows.Forms.PictureBox picIntensity;
        private System.Windows.Forms.Label lblLive;
        private System.Windows.Forms.Button btnLiveStart;
        private System.Windows.Forms.Button btnLiveStop;
        private System.Windows.Forms.Label lblRepeatStart;
        private System.Windows.Forms.TextBox txtRepeatStartPos;
        private System.Windows.Forms.Label lblRepeatEnd;
        private System.Windows.Forms.TextBox txtRepeatEndPos;
        private System.Windows.Forms.Button btnRepeat;
        private System.Windows.Forms.Button btnTriggerGrab;
        private System.Windows.Forms.PictureBox picLiveImage;
    }
}
