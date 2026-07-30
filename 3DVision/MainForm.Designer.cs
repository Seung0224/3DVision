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
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPageAcs = new System.Windows.Forms.TabPage();
            this.grpAcs = new System.Windows.Forms.GroupBox();
            this.lblAxis = new System.Windows.Forms.Label();
            this.numAxis = new System.Windows.Forms.NumericUpDown();
            this.btnCheckStatus = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.lblIp = new System.Windows.Forms.Label();
            this.grpMove = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnMovePlus = new System.Windows.Forms.Button();
            this.btnMoveMinus = new System.Windows.Forms.Button();
            this.txtDistance = new System.Windows.Forms.TextBox();
            this.lblDistance = new System.Windows.Forms.Label();
            this.btnDisable = new System.Windows.Forms.Button();
            this.btnEnable = new System.Windows.Forms.Button();
            this.numMoveAxis = new System.Windows.Forms.NumericUpDown();
            this.lblMoveAxis = new System.Windows.Forms.Label();
            this.tabPageSmartRay = new System.Windows.Forms.TabPage();
            this.grpSmartRay = new System.Windows.Forms.GroupBox();
            this.btnSrInfo = new System.Windows.Forms.Button();
            this.btnSrDisconnect = new System.Windows.Forms.Button();
            this.btnSrConnect = new System.Windows.Forms.Button();
            this.txtSrPort = new System.Windows.Forms.TextBox();
            this.lblSrPort = new System.Windows.Forms.Label();
            this.txtSrIp = new System.Windows.Forms.TextBox();
            this.lblSrIp = new System.Windows.Forms.Label();
            this.cmbSensors = new System.Windows.Forms.ComboBox();
            this.btnDiscover = new System.Windows.Forms.Button();
            this.grpGrab = new System.Windows.Forms.GroupBox();
            this.btnGrab = new System.Windows.Forms.Button();
            this.txtProfileCount = new System.Windows.Forms.TextBox();
            this.lblProfileCount = new System.Windows.Forms.Label();
            this.tabPageIntegration = new System.Windows.Forms.TabPage();
            this.grpContinuousScan = new System.Windows.Forms.GroupBox();
            this.btnScanStop = new System.Windows.Forms.Button();
            this.btnScanStart = new System.Windows.Forms.Button();
            this.txtStartPos = new System.Windows.Forms.TextBox();
            this.lblStartPos = new System.Windows.Forms.Label();
            this.btnHome = new System.Windows.Forms.Button();
            this.lblAcsStatus = new System.Windows.Forms.Label();
            this.lblSmartRayStatus = new System.Windows.Forms.Label();
            this.picHeightMap = new System.Windows.Forms.PictureBox();
            this.lblHeightMap = new System.Windows.Forms.Label();
            this.picPointCloud = new System.Windows.Forms.PictureBox();
            this.lblPointCloud = new System.Windows.Forms.Label();
            this.lblLog = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.tabMain.SuspendLayout();
            this.tabPageAcs.SuspendLayout();
            this.grpAcs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAxis)).BeginInit();
            this.grpMove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMoveAxis)).BeginInit();
            this.tabPageSmartRay.SuspendLayout();
            this.grpSmartRay.SuspendLayout();
            this.grpGrab.SuspendLayout();
            this.tabPageIntegration.SuspendLayout();
            this.grpContinuousScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHeightMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPointCloud)).BeginInit();
            this.SuspendLayout();
            //
            // tabMain
            //
            this.tabMain.Controls.Add(this.tabPageSmartRay);
            this.tabMain.Controls.Add(this.tabPageAcs);
            this.tabMain.Controls.Add(this.tabPageIntegration);
            this.tabMain.Location = new System.Drawing.Point(12, 12);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(976, 380);
            this.tabMain.TabIndex = 0;
            //
            // tabPageAcs
            //
            this.tabPageAcs.Controls.Add(this.grpAcs);
            this.tabPageAcs.Controls.Add(this.grpMove);
            this.tabPageAcs.Location = new System.Drawing.Point(4, 22);
            this.tabPageAcs.Name = "tabPageAcs";
            this.tabPageAcs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAcs.Size = new System.Drawing.Size(968, 354);
            this.tabPageAcs.TabIndex = 1;
            this.tabPageAcs.Text = "ACS 모터";
            this.tabPageAcs.UseVisualStyleBackColor = true;
            //
            // grpAcs
            //
            this.grpAcs.Controls.Add(this.lblAxis);
            this.grpAcs.Controls.Add(this.numAxis);
            this.grpAcs.Controls.Add(this.btnCheckStatus);
            this.grpAcs.Controls.Add(this.btnDisconnect);
            this.grpAcs.Controls.Add(this.btnConnect);
            this.grpAcs.Controls.Add(this.txtIp);
            this.grpAcs.Controls.Add(this.lblIp);
            this.grpAcs.Location = new System.Drawing.Point(6, 6);
            this.grpAcs.Name = "grpAcs";
            this.grpAcs.Size = new System.Drawing.Size(448, 120);
            this.grpAcs.TabIndex = 0;
            this.grpAcs.TabStop = false;
            this.grpAcs.Text = "1단계-A: 연결/상태 확인";
            //
            // lblAxis
            //
            this.lblAxis.AutoSize = true;
            this.lblAxis.Location = new System.Drawing.Point(240, 30);
            this.lblAxis.Name = "lblAxis";
            this.lblAxis.Size = new System.Drawing.Size(78, 12);
            this.lblAxis.TabIndex = 6;
            this.lblAxis.Text = "축 번호(0=X)";
            //
            // numAxis
            //
            this.numAxis.Location = new System.Drawing.Point(330, 27);
            this.numAxis.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            this.numAxis.Name = "numAxis";
            this.numAxis.Size = new System.Drawing.Size(60, 21);
            this.numAxis.TabIndex = 5;
            //
            // btnCheckStatus
            //
            this.btnCheckStatus.Location = new System.Drawing.Point(240, 65);
            this.btnCheckStatus.Name = "btnCheckStatus";
            this.btnCheckStatus.Size = new System.Drawing.Size(150, 30);
            this.btnCheckStatus.TabIndex = 4;
            this.btnCheckStatus.Text = "상태 확인 (읽기전용)";
            this.btnCheckStatus.UseVisualStyleBackColor = true;
            this.btnCheckStatus.Click += new System.EventHandler(this.btnCheckStatus_Click);
            //
            // btnDisconnect
            //
            this.btnDisconnect.Location = new System.Drawing.Point(120, 65);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(100, 30);
            this.btnDisconnect.TabIndex = 3;
            this.btnDisconnect.Text = "연결 해제";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            //
            // btnConnect
            //
            this.btnConnect.Location = new System.Drawing.Point(10, 65);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 30);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "연결";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            //
            // txtIp
            //
            this.txtIp.Location = new System.Drawing.Point(90, 27);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(130, 21);
            this.txtIp.TabIndex = 1;
            this.txtIp.Text = "10.0.0.100";
            //
            // lblIp
            //
            this.lblIp.AutoSize = true;
            this.lblIp.Location = new System.Drawing.Point(10, 30);
            this.lblIp.Name = "lblIp";
            this.lblIp.Size = new System.Drawing.Size(74, 12);
            this.lblIp.TabIndex = 0;
            this.lblIp.Text = "컨트롤러 IP";
            //
            // grpMove
            //
            this.grpMove.Controls.Add(this.btnStop);
            this.grpMove.Controls.Add(this.btnMovePlus);
            this.grpMove.Controls.Add(this.btnMoveMinus);
            this.grpMove.Controls.Add(this.txtDistance);
            this.grpMove.Controls.Add(this.lblDistance);
            this.grpMove.Controls.Add(this.btnDisable);
            this.grpMove.Controls.Add(this.btnEnable);
            this.grpMove.Controls.Add(this.numMoveAxis);
            this.grpMove.Controls.Add(this.lblMoveAxis);
            this.grpMove.Location = new System.Drawing.Point(6, 132);
            this.grpMove.Name = "grpMove";
            this.grpMove.Size = new System.Drawing.Size(448, 110);
            this.grpMove.TabIndex = 1;
            this.grpMove.TabStop = false;
            this.grpMove.Text = "1단계-B: 이동 테스트 (실제로 모터가 움직입니다. 주의)";
            //
            // btnStop
            //
            this.btnStop.BackColor = System.Drawing.Color.Firebrick;
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.Location = new System.Drawing.Point(290, 20);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(150, 65);
            this.btnStop.TabIndex = 8;
            this.btnStop.Text = "정지 (비상)";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            //
            // btnMovePlus
            //
            this.btnMovePlus.Location = new System.Drawing.Point(220, 55);
            this.btnMovePlus.Name = "btnMovePlus";
            this.btnMovePlus.Size = new System.Drawing.Size(60, 28);
            this.btnMovePlus.TabIndex = 7;
            this.btnMovePlus.Text = "이동+ ▶";
            this.btnMovePlus.UseVisualStyleBackColor = true;
            this.btnMovePlus.Click += new System.EventHandler(this.btnMovePlus_Click);
            //
            // btnMoveMinus
            //
            this.btnMoveMinus.Location = new System.Drawing.Point(150, 55);
            this.btnMoveMinus.Name = "btnMoveMinus";
            this.btnMoveMinus.Size = new System.Drawing.Size(60, 28);
            this.btnMoveMinus.TabIndex = 6;
            this.btnMoveMinus.Text = "◀ -이동";
            this.btnMoveMinus.UseVisualStyleBackColor = true;
            this.btnMoveMinus.Click += new System.EventHandler(this.btnMoveMinus_Click);
            //
            // txtDistance
            //
            this.txtDistance.Location = new System.Drawing.Point(80, 57);
            this.txtDistance.Name = "txtDistance";
            this.txtDistance.Size = new System.Drawing.Size(50, 21);
            this.txtDistance.TabIndex = 5;
            this.txtDistance.Text = "5";
            //
            // lblDistance
            //
            this.lblDistance.AutoSize = true;
            this.lblDistance.Location = new System.Drawing.Point(10, 60);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(58, 12);
            this.lblDistance.TabIndex = 4;
            this.lblDistance.Text = "거리(mm)";
            //
            // btnDisable
            //
            this.btnDisable.Location = new System.Drawing.Point(220, 20);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(75, 28);
            this.btnDisable.TabIndex = 3;
            this.btnDisable.Text = "Disable";
            this.btnDisable.UseVisualStyleBackColor = true;
            this.btnDisable.Click += new System.EventHandler(this.btnDisable_Click);
            //
            // btnEnable
            //
            this.btnEnable.Location = new System.Drawing.Point(140, 20);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(75, 28);
            this.btnEnable.TabIndex = 2;
            this.btnEnable.Text = "Enable";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            //
            // numMoveAxis
            //
            this.numMoveAxis.Location = new System.Drawing.Point(80, 22);
            this.numMoveAxis.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            this.numMoveAxis.Name = "numMoveAxis";
            this.numMoveAxis.Size = new System.Drawing.Size(50, 21);
            this.numMoveAxis.TabIndex = 1;
            //
            // lblMoveAxis
            //
            this.lblMoveAxis.AutoSize = true;
            this.lblMoveAxis.Location = new System.Drawing.Point(10, 25);
            this.lblMoveAxis.Name = "lblMoveAxis";
            this.lblMoveAxis.Size = new System.Drawing.Size(58, 12);
            this.lblMoveAxis.TabIndex = 0;
            this.lblMoveAxis.Text = "이동할 축";
            //
            // tabPageSmartRay
            //
            this.tabPageSmartRay.Controls.Add(this.grpGrab);
            this.tabPageSmartRay.Controls.Add(this.grpSmartRay);
            this.tabPageSmartRay.Location = new System.Drawing.Point(4, 22);
            this.tabPageSmartRay.Name = "tabPageSmartRay";
            this.tabPageSmartRay.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSmartRay.Size = new System.Drawing.Size(968, 354);
            this.tabPageSmartRay.TabIndex = 0;
            this.tabPageSmartRay.Text = "SmartRay 스캐너";
            this.tabPageSmartRay.UseVisualStyleBackColor = true;
            //
            // grpSmartRay
            //
            this.grpSmartRay.Controls.Add(this.btnSrInfo);
            this.grpSmartRay.Controls.Add(this.btnSrDisconnect);
            this.grpSmartRay.Controls.Add(this.btnSrConnect);
            this.grpSmartRay.Controls.Add(this.txtSrPort);
            this.grpSmartRay.Controls.Add(this.lblSrPort);
            this.grpSmartRay.Controls.Add(this.txtSrIp);
            this.grpSmartRay.Controls.Add(this.lblSrIp);
            this.grpSmartRay.Controls.Add(this.cmbSensors);
            this.grpSmartRay.Controls.Add(this.btnDiscover);
            this.grpSmartRay.Location = new System.Drawing.Point(6, 6);
            this.grpSmartRay.Name = "grpSmartRay";
            this.grpSmartRay.Size = new System.Drawing.Size(448, 150);
            this.grpSmartRay.TabIndex = 0;
            this.grpSmartRay.TabStop = false;
            this.grpSmartRay.Text = "2단계-A: 검색/연결/정보 확인";
            //
            // btnDiscover
            //
            this.btnDiscover.Location = new System.Drawing.Point(10, 20);
            this.btnDiscover.Name = "btnDiscover";
            this.btnDiscover.Size = new System.Drawing.Size(100, 28);
            this.btnDiscover.TabIndex = 0;
            this.btnDiscover.Text = "장치 검색";
            this.btnDiscover.UseVisualStyleBackColor = true;
            this.btnDiscover.Click += new System.EventHandler(this.btnDiscover_Click);
            //
            // cmbSensors
            //
            this.cmbSensors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSensors.FormattingEnabled = true;
            this.cmbSensors.Location = new System.Drawing.Point(120, 22);
            this.cmbSensors.Name = "cmbSensors";
            this.cmbSensors.Size = new System.Drawing.Size(318, 20);
            this.cmbSensors.TabIndex = 1;
            this.cmbSensors.SelectedIndexChanged += new System.EventHandler(this.cmbSensors_SelectedIndexChanged);
            //
            // lblSrIp
            //
            this.lblSrIp.AutoSize = true;
            this.lblSrIp.Location = new System.Drawing.Point(10, 60);
            this.lblSrIp.Name = "lblSrIp";
            this.lblSrIp.Size = new System.Drawing.Size(19, 12);
            this.lblSrIp.TabIndex = 2;
            this.lblSrIp.Text = "IP";
            //
            // txtSrIp
            //
            this.txtSrIp.Location = new System.Drawing.Point(35, 57);
            this.txtSrIp.Name = "txtSrIp";
            this.txtSrIp.Size = new System.Drawing.Size(120, 21);
            this.txtSrIp.TabIndex = 3;
            this.txtSrIp.Text = "192.168.111.200";
            //
            // lblSrPort
            //
            this.lblSrPort.AutoSize = true;
            this.lblSrPort.Location = new System.Drawing.Point(165, 60);
            this.lblSrPort.Name = "lblSrPort";
            this.lblSrPort.Size = new System.Drawing.Size(29, 12);
            this.lblSrPort.TabIndex = 4;
            this.lblSrPort.Text = "포트";
            //
            // txtSrPort
            //
            this.txtSrPort.Location = new System.Drawing.Point(200, 57);
            this.txtSrPort.Name = "txtSrPort";
            this.txtSrPort.Size = new System.Drawing.Size(50, 21);
            this.txtSrPort.TabIndex = 5;
            this.txtSrPort.Text = "40";
            //
            // btnSrConnect
            //
            this.btnSrConnect.Location = new System.Drawing.Point(260, 55);
            this.btnSrConnect.Name = "btnSrConnect";
            this.btnSrConnect.Size = new System.Drawing.Size(85, 26);
            this.btnSrConnect.TabIndex = 6;
            this.btnSrConnect.Text = "연결";
            this.btnSrConnect.UseVisualStyleBackColor = true;
            this.btnSrConnect.Click += new System.EventHandler(this.btnSrConnect_Click);
            //
            // btnSrDisconnect
            //
            this.btnSrDisconnect.Location = new System.Drawing.Point(350, 55);
            this.btnSrDisconnect.Name = "btnSrDisconnect";
            this.btnSrDisconnect.Size = new System.Drawing.Size(95, 26);
            this.btnSrDisconnect.TabIndex = 7;
            this.btnSrDisconnect.Text = "연결 해제";
            this.btnSrDisconnect.UseVisualStyleBackColor = true;
            this.btnSrDisconnect.Click += new System.EventHandler(this.btnSrDisconnect_Click);
            //
            // btnSrInfo
            //
            this.btnSrInfo.Location = new System.Drawing.Point(10, 95);
            this.btnSrInfo.Name = "btnSrInfo";
            this.btnSrInfo.Size = new System.Drawing.Size(200, 30);
            this.btnSrInfo.TabIndex = 8;
            this.btnSrInfo.Text = "정보 확인 (읽기전용)";
            this.btnSrInfo.UseVisualStyleBackColor = true;
            this.btnSrInfo.Click += new System.EventHandler(this.btnSrInfo_Click);
            //
            // grpGrab
            //
            this.grpGrab.Controls.Add(this.btnGrab);
            this.grpGrab.Controls.Add(this.txtProfileCount);
            this.grpGrab.Controls.Add(this.lblProfileCount);
            this.grpGrab.Location = new System.Drawing.Point(6, 162);
            this.grpGrab.Name = "grpGrab";
            this.grpGrab.Size = new System.Drawing.Size(448, 100);
            this.grpGrab.TabIndex = 1;
            this.grpGrab.TabStop = false;
            this.grpGrab.Text = "2단계-B: 촬영 테스트 (실제로 레이저가 켜집니다. 주의)";
            //
            // lblProfileCount
            //
            this.lblProfileCount.AutoSize = true;
            this.lblProfileCount.Location = new System.Drawing.Point(10, 30);
            this.lblProfileCount.Name = "lblProfileCount";
            this.lblProfileCount.Size = new System.Drawing.Size(65, 12);
            this.lblProfileCount.TabIndex = 0;
            this.lblProfileCount.Text = "프로파일 수";
            //
            // txtProfileCount
            //
            this.txtProfileCount.Location = new System.Drawing.Point(85, 27);
            this.txtProfileCount.Name = "txtProfileCount";
            this.txtProfileCount.Size = new System.Drawing.Size(60, 21);
            this.txtProfileCount.TabIndex = 1;
            this.txtProfileCount.Text = "100";
            //
            // btnGrab
            //
            this.btnGrab.BackColor = System.Drawing.Color.Goldenrod;
            this.btnGrab.Location = new System.Drawing.Point(10, 60);
            this.btnGrab.Name = "btnGrab";
            this.btnGrab.Size = new System.Drawing.Size(220, 30);
            this.btnGrab.TabIndex = 2;
            this.btnGrab.Text = "1회 촬영 (레이저 켜짐)";
            this.btnGrab.UseVisualStyleBackColor = false;
            this.btnGrab.Click += new System.EventHandler(this.btnGrab_Click);
            //
            // tabPageIntegration
            //
            this.tabPageIntegration.Controls.Add(this.picHeightMap);
            this.tabPageIntegration.Controls.Add(this.lblHeightMap);
            this.tabPageIntegration.Controls.Add(this.picPointCloud);
            this.tabPageIntegration.Controls.Add(this.lblPointCloud);
            this.tabPageIntegration.Controls.Add(this.grpContinuousScan);
            this.tabPageIntegration.Location = new System.Drawing.Point(4, 22);
            this.tabPageIntegration.Name = "tabPageIntegration";
            this.tabPageIntegration.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIntegration.Size = new System.Drawing.Size(968, 354);
            this.tabPageIntegration.TabIndex = 2;
            this.tabPageIntegration.Text = "연동 테스트";
            this.tabPageIntegration.UseVisualStyleBackColor = true;
            //
            // grpContinuousScan
            //
            this.grpContinuousScan.Controls.Add(this.btnScanStop);
            this.grpContinuousScan.Controls.Add(this.btnScanStart);
            this.grpContinuousScan.Controls.Add(this.txtStartPos);
            this.grpContinuousScan.Controls.Add(this.lblStartPos);
            this.grpContinuousScan.Controls.Add(this.btnHome);
            this.grpContinuousScan.Controls.Add(this.lblAcsStatus);
            this.grpContinuousScan.Controls.Add(this.lblSmartRayStatus);
            this.grpContinuousScan.Location = new System.Drawing.Point(6, 6);
            this.grpContinuousScan.Name = "grpContinuousScan";
            this.grpContinuousScan.Size = new System.Drawing.Size(448, 140);
            this.grpContinuousScan.TabIndex = 0;
            this.grpContinuousScan.TabStop = false;
            this.grpContinuousScan.Text = "3단계: 이동 + 촬영 (ACS가 시작~끝으로 이동한 뒤 FreeRunning으로 촬영, ZMap 이미지로 표시)";
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
            this.txtStartPos.Size = new System.Drawing.Size(80, 21);
            this.txtStartPos.TabIndex = 1;
            this.txtStartPos.Text = "-18.779";
            //
            // lblAcsStatus
            //
            this.lblAcsStatus.AutoSize = true;
            this.lblAcsStatus.ForeColor = System.Drawing.Color.Firebrick;
            this.lblAcsStatus.Location = new System.Drawing.Point(200, 28);
            this.lblAcsStatus.Name = "lblAcsStatus";
            this.lblAcsStatus.Size = new System.Drawing.Size(60, 12);
            this.lblAcsStatus.TabIndex = 2;
            this.lblAcsStatus.Text = "ACS: -";
            //
            // lblSmartRayStatus
            //
            this.lblSmartRayStatus.AutoSize = true;
            this.lblSmartRayStatus.ForeColor = System.Drawing.Color.Firebrick;
            this.lblSmartRayStatus.Location = new System.Drawing.Point(320, 28);
            this.lblSmartRayStatus.Name = "lblSmartRayStatus";
            this.lblSmartRayStatus.Size = new System.Drawing.Size(90, 12);
            this.lblSmartRayStatus.TabIndex = 3;
            this.lblSmartRayStatus.Text = "SmartRay: -";
            //
            // btnScanStart
            //
            this.btnScanStart.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnScanStart.Location = new System.Drawing.Point(10, 60);
            this.btnScanStart.Name = "btnScanStart";
            this.btnScanStart.Size = new System.Drawing.Size(150, 32);
            this.btnScanStart.TabIndex = 4;
            this.btnScanStart.Text = "이동 + 촬영 시작";
            this.btnScanStart.UseVisualStyleBackColor = false;
            this.btnScanStart.Click += new System.EventHandler(this.btnScanStart_Click);
            //
            // btnScanStop
            //
            this.btnScanStop.BackColor = System.Drawing.Color.Firebrick;
            this.btnScanStop.ForeColor = System.Drawing.Color.White;
            this.btnScanStop.Enabled = false;
            this.btnScanStop.Location = new System.Drawing.Point(170, 60);
            this.btnScanStop.Name = "btnScanStop";
            this.btnScanStop.Size = new System.Drawing.Size(100, 32);
            this.btnScanStop.TabIndex = 5;
            this.btnScanStop.Text = "중지";
            this.btnScanStop.UseVisualStyleBackColor = false;
            this.btnScanStop.Click += new System.EventHandler(this.btnScanStop_Click);
            //
            // btnHome
            //
            this.btnHome.Location = new System.Drawing.Point(280, 60);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(150, 32);
            this.btnHome.TabIndex = 6;
            this.btnHome.Text = "Home (시작 위치로)";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            //
            // lblHeightMap
            //
            this.lblHeightMap.AutoSize = true;
            this.lblHeightMap.Location = new System.Drawing.Point(6, 150);
            this.lblHeightMap.Name = "lblHeightMap";
            this.lblHeightMap.Size = new System.Drawing.Size(100, 12);
            this.lblHeightMap.TabIndex = 7;
            this.lblHeightMap.Text = "높이맵 (그레이스케일)";
            //
            // picHeightMap
            //
            this.picHeightMap.BackColor = System.Drawing.Color.Black;
            this.picHeightMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picHeightMap.Location = new System.Drawing.Point(6, 166);
            this.picHeightMap.Name = "picHeightMap";
            this.picHeightMap.Size = new System.Drawing.Size(468, 180);
            this.picHeightMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHeightMap.TabIndex = 1;
            this.picHeightMap.TabStop = false;
            //
            // lblPointCloud
            //
            this.lblPointCloud.AutoSize = true;
            this.lblPointCloud.Location = new System.Drawing.Point(482, 150);
            this.lblPointCloud.Name = "lblPointCloud";
            this.lblPointCloud.Size = new System.Drawing.Size(130, 12);
            this.lblPointCloud.TabIndex = 8;
            this.lblPointCloud.Text = "포인트클라우드 (등각 투영, 의사 3D)";
            //
            // picPointCloud
            //
            this.picPointCloud.BackColor = System.Drawing.Color.Black;
            this.picPointCloud.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPointCloud.Location = new System.Drawing.Point(482, 166);
            this.picPointCloud.Name = "picPointCloud";
            this.picPointCloud.Size = new System.Drawing.Size(468, 180);
            this.picPointCloud.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPointCloud.TabIndex = 9;
            this.picPointCloud.TabStop = false;
            //
            // lblLog
            //
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(13, 402);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(24, 12);
            this.lblLog.TabIndex = 1;
            this.lblLog.Text = "로그";
            //
            // txtLog
            //
            this.txtLog.Location = new System.Drawing.Point(12, 418);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(976, 220);
            this.txtLog.TabIndex = 2;
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.tabMain);
            this.Name = "MainForm";
            this.Text = "3DVision - ACS/SmartRay 연동 테스트";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabMain.ResumeLayout(false);
            this.tabPageAcs.ResumeLayout(false);
            this.grpAcs.ResumeLayout(false);
            this.grpAcs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAxis)).EndInit();
            this.grpMove.ResumeLayout(false);
            this.grpMove.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMoveAxis)).EndInit();
            this.tabPageSmartRay.ResumeLayout(false);
            this.grpSmartRay.ResumeLayout(false);
            this.grpSmartRay.PerformLayout();
            this.grpGrab.ResumeLayout(false);
            this.grpGrab.PerformLayout();
            this.tabPageIntegration.ResumeLayout(false);
            this.grpContinuousScan.ResumeLayout(false);
            this.grpContinuousScan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHeightMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPointCloud)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPageAcs;
        private System.Windows.Forms.GroupBox grpAcs;
        private System.Windows.Forms.Label lblIp;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnCheckStatus;
        private System.Windows.Forms.Label lblAxis;
        private System.Windows.Forms.NumericUpDown numAxis;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox grpMove;
        private System.Windows.Forms.Label lblMoveAxis;
        private System.Windows.Forms.NumericUpDown numMoveAxis;
        private System.Windows.Forms.Button btnEnable;
        private System.Windows.Forms.Button btnDisable;
        private System.Windows.Forms.Label lblDistance;
        private System.Windows.Forms.TextBox txtDistance;
        private System.Windows.Forms.Button btnMoveMinus;
        private System.Windows.Forms.Button btnMovePlus;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TabPage tabPageSmartRay;
        private System.Windows.Forms.GroupBox grpSmartRay;
        private System.Windows.Forms.Button btnDiscover;
        private System.Windows.Forms.ComboBox cmbSensors;
        private System.Windows.Forms.Label lblSrIp;
        private System.Windows.Forms.TextBox txtSrIp;
        private System.Windows.Forms.Label lblSrPort;
        private System.Windows.Forms.TextBox txtSrPort;
        private System.Windows.Forms.Button btnSrConnect;
        private System.Windows.Forms.Button btnSrDisconnect;
        private System.Windows.Forms.Button btnSrInfo;
        private System.Windows.Forms.GroupBox grpGrab;
        private System.Windows.Forms.Button btnGrab;
        private System.Windows.Forms.TextBox txtProfileCount;
        private System.Windows.Forms.Label lblProfileCount;
        private System.Windows.Forms.TabPage tabPageIntegration;
        private System.Windows.Forms.GroupBox grpContinuousScan;
        private System.Windows.Forms.Label lblStartPos;
        private System.Windows.Forms.TextBox txtStartPos;
        private System.Windows.Forms.Button btnScanStart;
        private System.Windows.Forms.Button btnScanStop;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Label lblAcsStatus;
        private System.Windows.Forms.Label lblSmartRayStatus;
        private System.Windows.Forms.PictureBox picHeightMap;
        private System.Windows.Forms.Label lblHeightMap;
        private System.Windows.Forms.PictureBox picPointCloud;
        private System.Windows.Forms.Label lblPointCloud;
    }
}
