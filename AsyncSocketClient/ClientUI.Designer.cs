namespace AsyncSocketClient
{
    partial class ClientUI
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
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPswd = new System.Windows.Forms.TextBox();
            this.btLogin = new System.Windows.Forms.Button();
            this.rtbCommLog = new System.Windows.Forms.RichTextBox();
            this.btSignUp = new System.Windows.Forms.Button();
            this.grpSelfCmd = new System.Windows.Forms.GroupBox();
            this.txtSelfCmd = new System.Windows.Forms.TextBox();
            this.btSelfCmdSend = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCnnt = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkHex = new System.Windows.Forms.CheckBox();
            this.grpSelfCmd.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(311, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "UserName ";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(397, 17);
            this.txtUser.Margin = new System.Windows.Forms.Padding(4);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(131, 25);
            this.txtUser.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(549, 23);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "PassWord";
            // 
            // txtPswd
            // 
            this.txtPswd.Location = new System.Drawing.Point(628, 17);
            this.txtPswd.Margin = new System.Windows.Forms.Padding(4);
            this.txtPswd.Name = "txtPswd";
            this.txtPswd.Size = new System.Drawing.Size(120, 25);
            this.txtPswd.TabIndex = 1;
            this.txtPswd.UseSystemPasswordChar = true;
            // 
            // btLogin
            // 
            this.btLogin.Location = new System.Drawing.Point(779, 16);
            this.btLogin.Margin = new System.Windows.Forms.Padding(4);
            this.btLogin.Name = "btLogin";
            this.btLogin.Size = new System.Drawing.Size(80, 26);
            this.btLogin.TabIndex = 2;
            this.btLogin.Text = "Login";
            this.btLogin.UseVisualStyleBackColor = true;
            this.btLogin.Click += new System.EventHandler(this.btLogin_Click);
            // 
            // rtbCommLog
            // 
            this.rtbCommLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbCommLog.Location = new System.Drawing.Point(299, 50);
            this.rtbCommLog.Margin = new System.Windows.Forms.Padding(4);
            this.rtbCommLog.Name = "rtbCommLog";
            this.rtbCommLog.Size = new System.Drawing.Size(685, 524);
            this.rtbCommLog.TabIndex = 3;
            this.rtbCommLog.Text = "";
            // 
            // btSignUp
            // 
            this.btSignUp.Location = new System.Drawing.Point(888, 16);
            this.btSignUp.Margin = new System.Windows.Forms.Padding(4);
            this.btSignUp.Name = "btSignUp";
            this.btSignUp.Size = new System.Drawing.Size(80, 26);
            this.btSignUp.TabIndex = 2;
            this.btSignUp.Text = "SignUp";
            this.btSignUp.UseVisualStyleBackColor = true;
            this.btSignUp.Click += new System.EventHandler(this.btSignUp_Click);
            // 
            // grpSelfCmd
            // 
            this.grpSelfCmd.Controls.Add(this.chkHex);
            this.grpSelfCmd.Controls.Add(this.txtSelfCmd);
            this.grpSelfCmd.Controls.Add(this.btSelfCmdSend);
            this.grpSelfCmd.Location = new System.Drawing.Point(13, 137);
            this.grpSelfCmd.Margin = new System.Windows.Forms.Padding(4);
            this.grpSelfCmd.Name = "grpSelfCmd";
            this.grpSelfCmd.Padding = new System.Windows.Forms.Padding(4);
            this.grpSelfCmd.Size = new System.Drawing.Size(263, 375);
            this.grpSelfCmd.TabIndex = 4;
            this.grpSelfCmd.TabStop = false;
            this.grpSelfCmd.Text = "Self-Defined Command";
            // 
            // txtSelfCmd
            // 
            this.txtSelfCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSelfCmd.Location = new System.Drawing.Point(5, 64);
            this.txtSelfCmd.Margin = new System.Windows.Forms.Padding(4);
            this.txtSelfCmd.Multiline = true;
            this.txtSelfCmd.Name = "txtSelfCmd";
            this.txtSelfCmd.Size = new System.Drawing.Size(250, 303);
            this.txtSelfCmd.TabIndex = 1;
            // 
            // btSelfCmdSend
            // 
            this.btSelfCmdSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btSelfCmdSend.Location = new System.Drawing.Point(78, 24);
            this.btSelfCmdSend.Margin = new System.Windows.Forms.Padding(4);
            this.btSelfCmdSend.Name = "btSelfCmdSend";
            this.btSelfCmdSend.Size = new System.Drawing.Size(177, 30);
            this.btSelfCmdSend.TabIndex = 0;
            this.btSelfCmdSend.Text = "Send";
            this.btSelfCmdSend.UseVisualStyleBackColor = true;
            this.btSelfCmdSend.Click += new System.EventHandler(this.btSelfCmdSend_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCnnt);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtIp);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(13, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(263, 109);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connect";
            // 
            // btnCnnt
            // 
            this.btnCnnt.Location = new System.Drawing.Point(144, 62);
            this.btnCnnt.Name = "btnCnnt";
            this.btnCnnt.Size = new System.Drawing.Size(71, 29);
            this.btnCnnt.TabIndex = 4;
            this.btnCnnt.Text = "连接";
            this.btnCnnt.UseVisualStyleBackColor = true;
            this.btnCnnt.Click += new System.EventHandler(this.btnCnnt_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(78, 62);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(59, 25);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "8088";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 62);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Port :";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(78, 26);
            this.txtIp.Margin = new System.Windows.Forms.Padding(4);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(137, 25);
            this.txtIp.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 29);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "IP :";
            // 
            // chkHex
            // 
            this.chkHex.AutoSize = true;
            this.chkHex.Location = new System.Drawing.Point(7, 31);
            this.chkHex.Name = "chkHex";
            this.chkHex.Size = new System.Drawing.Size(53, 19);
            this.chkHex.TabIndex = 2;
            this.chkHex.Text = "HEX";
            this.chkHex.UseVisualStyleBackColor = true;
            // 
            // ClientUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 584);
            this.Controls.Add(this.grpSelfCmd);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rtbCommLog);
            this.Controls.Add(this.btSignUp);
            this.Controls.Add(this.btLogin);
            this.Controls.Add(this.txtPswd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ClientUI";
            this.Text = "ClientUI";
            this.grpSelfCmd.ResumeLayout(false);
            this.grpSelfCmd.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPswd;
        private System.Windows.Forms.Button btLogin;
        private System.Windows.Forms.RichTextBox rtbCommLog;
        private System.Windows.Forms.Button btSignUp;
        private System.Windows.Forms.GroupBox grpSelfCmd;
        private System.Windows.Forms.TextBox txtSelfCmd;
        private System.Windows.Forms.Button btSelfCmdSend;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCnnt;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkHex;
    }
}