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
            this.lblLog = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grpAcs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAxis)).BeginInit();
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
            // lblLog
            //
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(13, 142);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(24, 12);
            this.lblLog.TabIndex = 1;
            this.lblLog.Text = "로그";
            //
            // txtLog
            //
            this.txtLog.Location = new System.Drawing.Point(12, 158);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(460, 260);
            this.txtLog.TabIndex = 2;
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 431);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.grpAcs);
            this.Name = "Form1";
            this.Text = "3DVision - ACS 모터 테스트 (1단계-A)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.grpAcs.ResumeLayout(false);
            this.grpAcs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAxis)).EndInit();
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
    }
}
