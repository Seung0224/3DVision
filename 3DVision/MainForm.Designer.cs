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
            this.lblLog = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grpAcs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAxis)).BeginInit();
            this.grpMove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMoveAxis)).BeginInit();
            this.SuspendLayout();
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
            this.grpAcs.Location = new System.Drawing.Point(12, 12);
            this.grpAcs.Name = "grpAcs";
            this.grpAcs.Size = new System.Drawing.Size(460, 120);
            this.grpAcs.TabIndex = 0;
            this.grpAcs.TabStop = false;
            this.grpAcs.Text = "ACS 모터 (1단계-A: 연결/상태 확인)";
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
            this.grpMove.Location = new System.Drawing.Point(12, 138);
            this.grpMove.Name = "grpMove";
            this.grpMove.Size = new System.Drawing.Size(460, 110);
            this.grpMove.TabIndex = 3;
            this.grpMove.TabStop = false;
            this.grpMove.Text = "이동 테스트 (1단계-B: 실제로 모터가 움직입니다. 주의)";
            //
            // btnStop
            //
            this.btnStop.BackColor = System.Drawing.Color.Firebrick;
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.Location = new System.Drawing.Point(300, 20);
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
            this.btnMovePlus.Size = new System.Drawing.Size(75, 28);
            this.btnMovePlus.TabIndex = 7;
            this.btnMovePlus.Text = "이동+ ▶";
            this.btnMovePlus.UseVisualStyleBackColor = true;
            this.btnMovePlus.Click += new System.EventHandler(this.btnMovePlus_Click);
            //
            // btnMoveMinus
            //
            this.btnMoveMinus.Location = new System.Drawing.Point(140, 55);
            this.btnMoveMinus.Name = "btnMoveMinus";
            this.btnMoveMinus.Size = new System.Drawing.Size(75, 28);
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
            // lblLog
            //
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(13, 258);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(24, 12);
            this.lblLog.TabIndex = 1;
            this.lblLog.Text = "로그";
            //
            // txtLog
            //
            this.txtLog.Location = new System.Drawing.Point(12, 274);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(460, 200);
            this.txtLog.TabIndex = 2;
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 490);
            this.Controls.Add(this.grpMove);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.grpAcs);
            this.Name = "MainForm";
            this.Text = "3DVision - ACS 모터 테스트 (1단계-B)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.grpAcs.ResumeLayout(false);
            this.grpAcs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAxis)).EndInit();
            this.grpMove.ResumeLayout(false);
            this.grpMove.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMoveAxis)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
    }
}
