namespace AsyncSocketServer
{
    partial class UI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btRunCtrl = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lbClientCnt = new System.Windows.Forms.Label();
            this.lbRecvBytes = new System.Windows.Forms.Label();
            this.txtClientCnt = new System.Windows.Forms.TextBox();
            this.txtRecvBytes = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "ServerPort :";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(95, 6);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(67, 21);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "8088";
            // 
            // btRunCtrl
            // 
            this.btRunCtrl.BackColor = System.Drawing.Color.Silver;
            this.btRunCtrl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btRunCtrl.Location = new System.Drawing.Point(178, 6);
            this.btRunCtrl.Name = "btRunCtrl";
            this.btRunCtrl.Size = new System.Drawing.Size(64, 21);
            this.btRunCtrl.TabIndex = 2;
            this.btRunCtrl.Text = "Start";
            this.btRunCtrl.UseVisualStyleBackColor = false;
            this.btRunCtrl.Click += new System.EventHandler(this.btRunCtrl_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(12, 32);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(683, 354);
            this.txtLog.TabIndex = 3;
            // 
            // lbClientCnt
            // 
            this.lbClientCnt.AutoSize = true;
            this.lbClientCnt.Location = new System.Drawing.Point(341, 9);
            this.lbClientCnt.Name = "lbClientCnt";
            this.lbClientCnt.Size = new System.Drawing.Size(47, 12);
            this.lbClientCnt.TabIndex = 4;
            this.lbClientCnt.Text = "Clients";
            // 
            // lbRecvBytes
            // 
            this.lbRecvBytes.AutoSize = true;
            this.lbRecvBytes.Location = new System.Drawing.Point(506, 10);
            this.lbRecvBytes.Name = "lbRecvBytes";
            this.lbRecvBytes.Size = new System.Drawing.Size(71, 12);
            this.lbRecvBytes.TabIndex = 4;
            this.lbRecvBytes.Text = "Recvd Bytes";
            // 
            // txtClientCnt
            // 
            this.txtClientCnt.Location = new System.Drawing.Point(394, 5);
            this.txtClientCnt.Name = "txtClientCnt";
            this.txtClientCnt.Size = new System.Drawing.Size(68, 21);
            this.txtClientCnt.TabIndex = 5;
            // 
            // txtRecvBytes
            // 
            this.txtRecvBytes.Location = new System.Drawing.Point(583, 5);
            this.txtRecvBytes.Name = "txtRecvBytes";
            this.txtRecvBytes.Size = new System.Drawing.Size(112, 21);
            this.txtRecvBytes.TabIndex = 5;
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 392);
            this.Controls.Add(this.txtRecvBytes);
            this.Controls.Add(this.txtClientCnt);
            this.Controls.Add(this.lbRecvBytes);
            this.Controls.Add(this.lbClientCnt);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btRunCtrl);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label1);
            this.Name = "UI";
            this.Text = "UI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UI_FormClosed);
            this.Load += new System.EventHandler(this.UI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btRunCtrl;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lbClientCnt;
        private System.Windows.Forms.Label lbRecvBytes;
        private System.Windows.Forms.TextBox txtClientCnt;
        private System.Windows.Forms.TextBox txtRecvBytes;
    }
}