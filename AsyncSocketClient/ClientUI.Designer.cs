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
            this.grpCmd = new System.Windows.Forms.GroupBox();
            this.btPlan_pks1 = new System.Windows.Forms.Button();
            this.grpSelfCmd = new System.Windows.Forms.GroupBox();
            this.btSelfCmdSend = new System.Windows.Forms.Button();
            this.txtSelfCmd = new System.Windows.Forms.TextBox();
            this.btPlan_pks3 = new System.Windows.Forms.Button();
            this.btPlan_ssc1 = new System.Windows.Forms.Button();
            this.btPlan_pks2 = new System.Windows.Forms.Button();
            this.btPlan_ssc2 = new System.Windows.Forms.Button();
            this.btPlan_ssc3 = new System.Windows.Forms.Button();
            this.btGetCode_ssc = new System.Windows.Forms.Button();
            this.btOderPlan = new System.Windows.Forms.Button();
            this.grpCmd.SuspendLayout();
            this.grpSelfCmd.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "UserName ";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(73, 12);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(99, 21);
            this.txtUser.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "PassWord";
            // 
            // txtPswd
            // 
            this.txtPswd.Location = new System.Drawing.Point(246, 12);
            this.txtPswd.Name = "txtPswd";
            this.txtPswd.Size = new System.Drawing.Size(91, 21);
            this.txtPswd.TabIndex = 1;
            this.txtPswd.UseSystemPasswordChar = true;
            // 
            // btLogin
            // 
            this.btLogin.Location = new System.Drawing.Point(359, 11);
            this.btLogin.Name = "btLogin";
            this.btLogin.Size = new System.Drawing.Size(60, 21);
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
            this.rtbCommLog.Location = new System.Drawing.Point(224, 40);
            this.rtbCommLog.Name = "rtbCommLog";
            this.rtbCommLog.Size = new System.Drawing.Size(515, 420);
            this.rtbCommLog.TabIndex = 3;
            this.rtbCommLog.Text = "";
            // 
            // btSignUp
            // 
            this.btSignUp.Location = new System.Drawing.Point(441, 11);
            this.btSignUp.Name = "btSignUp";
            this.btSignUp.Size = new System.Drawing.Size(60, 21);
            this.btSignUp.TabIndex = 2;
            this.btSignUp.Text = "SignUp";
            this.btSignUp.UseVisualStyleBackColor = true;
            this.btSignUp.Click += new System.EventHandler(this.btSignUp_Click);
            // 
            // grpCmd
            // 
            this.grpCmd.Controls.Add(this.btOderPlan);
            this.grpCmd.Controls.Add(this.btGetCode_ssc);
            this.grpCmd.Controls.Add(this.btPlan_ssc3);
            this.grpCmd.Controls.Add(this.btPlan_ssc2);
            this.grpCmd.Controls.Add(this.btPlan_ssc1);
            this.grpCmd.Controls.Add(this.btPlan_pks3);
            this.grpCmd.Controls.Add(this.btPlan_pks2);
            this.grpCmd.Controls.Add(this.btPlan_pks1);
            this.grpCmd.Location = new System.Drawing.Point(10, 39);
            this.grpCmd.Name = "grpCmd";
            this.grpCmd.Size = new System.Drawing.Size(197, 258);
            this.grpCmd.TabIndex = 4;
            this.grpCmd.TabStop = false;
            this.grpCmd.Text = "Test Command";
            // 
            // btPlan_pks1
            // 
            this.btPlan_pks1.Location = new System.Drawing.Point(2, 20);
            this.btPlan_pks1.Name = "btPlan_pks1";
            this.btPlan_pks1.Size = new System.Drawing.Size(189, 21);
            this.btPlan_pks1.TabIndex = 0;
            this.btPlan_pks1.Text = "Set Plan: pk10-1-1-1-2-1";
            this.btPlan_pks1.UseVisualStyleBackColor = true;
            this.btPlan_pks1.Click += new System.EventHandler(this.btPlan_pks1_Click);
            // 
            // grpSelfCmd
            // 
            this.grpSelfCmd.Controls.Add(this.txtSelfCmd);
            this.grpSelfCmd.Controls.Add(this.btSelfCmdSend);
            this.grpSelfCmd.Location = new System.Drawing.Point(13, 326);
            this.grpSelfCmd.Name = "grpSelfCmd";
            this.grpSelfCmd.Size = new System.Drawing.Size(194, 134);
            this.grpSelfCmd.TabIndex = 4;
            this.grpSelfCmd.TabStop = false;
            this.grpSelfCmd.Text = "Self-Defined Command";
            // 
            // btSelfCmdSend
            // 
            this.btSelfCmdSend.Location = new System.Drawing.Point(4, 110);
            this.btSelfCmdSend.Name = "btSelfCmdSend";
            this.btSelfCmdSend.Size = new System.Drawing.Size(188, 24);
            this.btSelfCmdSend.TabIndex = 0;
            this.btSelfCmdSend.Text = "Send";
            this.btSelfCmdSend.UseVisualStyleBackColor = true;
            this.btSelfCmdSend.Click += new System.EventHandler(this.btSelfCmdSend_Click);
            // 
            // txtSelfCmd
            // 
            this.txtSelfCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSelfCmd.Location = new System.Drawing.Point(4, 20);
            this.txtSelfCmd.Multiline = true;
            this.txtSelfCmd.Name = "txtSelfCmd";
            this.txtSelfCmd.Size = new System.Drawing.Size(187, 85);
            this.txtSelfCmd.TabIndex = 1;
            // 
            // btPlan_pks3
            // 
            this.btPlan_pks3.Location = new System.Drawing.Point(2, 74);
            this.btPlan_pks3.Name = "btPlan_pks3";
            this.btPlan_pks3.Size = new System.Drawing.Size(189, 21);
            this.btPlan_pks3.TabIndex = 0;
            this.btPlan_pks3.Text = "Set Plan: pk10-1-3-8-1-1";
            this.btPlan_pks3.UseVisualStyleBackColor = true;
            this.btPlan_pks3.Click += new System.EventHandler(this.btPlan_pks3_Click);
            // 
            // btPlan_ssc1
            // 
            this.btPlan_ssc1.Location = new System.Drawing.Point(2, 111);
            this.btPlan_ssc1.Name = "btPlan_ssc1";
            this.btPlan_ssc1.Size = new System.Drawing.Size(189, 21);
            this.btPlan_ssc1.TabIndex = 0;
            this.btPlan_ssc1.Text = "Set Plan: ssc-2-1-1-2-1";
            this.btPlan_ssc1.UseVisualStyleBackColor = true;
            this.btPlan_ssc1.Click += new System.EventHandler(this.btPlan_ssc1_Click);
            // 
            // btPlan_pks2
            // 
            this.btPlan_pks2.Location = new System.Drawing.Point(2, 47);
            this.btPlan_pks2.Name = "btPlan_pks2";
            this.btPlan_pks2.Size = new System.Drawing.Size(189, 21);
            this.btPlan_pks2.TabIndex = 0;
            this.btPlan_pks2.Text = "Set Plan: pk10-1-2-1-2-1";
            this.btPlan_pks2.UseVisualStyleBackColor = true;
            this.btPlan_pks2.Click += new System.EventHandler(this.btPlan_pks2_Click);
            // 
            // btPlan_ssc2
            // 
            this.btPlan_ssc2.Location = new System.Drawing.Point(3, 138);
            this.btPlan_ssc2.Name = "btPlan_ssc2";
            this.btPlan_ssc2.Size = new System.Drawing.Size(189, 21);
            this.btPlan_ssc2.TabIndex = 0;
            this.btPlan_ssc2.Text = "Set Plan: ssc-2-2-1-2-1";
            this.btPlan_ssc2.UseVisualStyleBackColor = true;
            this.btPlan_ssc2.Click += new System.EventHandler(this.btPlan_ssc2_Click);
            // 
            // btPlan_ssc3
            // 
            this.btPlan_ssc3.Location = new System.Drawing.Point(2, 165);
            this.btPlan_ssc3.Name = "btPlan_ssc3";
            this.btPlan_ssc3.Size = new System.Drawing.Size(189, 21);
            this.btPlan_ssc3.TabIndex = 0;
            this.btPlan_ssc3.Text = "Set Plan: ssc-2-3-8-1-1";
            this.btPlan_ssc3.UseVisualStyleBackColor = true;
            this.btPlan_ssc3.Click += new System.EventHandler(this.btPlan_ssc3_Click);
            // 
            // btGetCode_ssc
            // 
            this.btGetCode_ssc.Location = new System.Drawing.Point(2, 204);
            this.btGetCode_ssc.Name = "btGetCode_ssc";
            this.btGetCode_ssc.Size = new System.Drawing.Size(189, 21);
            this.btGetCode_ssc.TabIndex = 0;
            this.btGetCode_ssc.Text = "Get OpenCode: ssc";
            this.btGetCode_ssc.UseVisualStyleBackColor = true;
            this.btGetCode_ssc.Click += new System.EventHandler(this.btGetCode_ssc_Click);
            // 
            // btOderPlan
            // 
            this.btOderPlan.Location = new System.Drawing.Point(2, 231);
            this.btOderPlan.Name = "btOderPlan";
            this.btOderPlan.Size = new System.Drawing.Size(189, 21);
            this.btOderPlan.TabIndex = 0;
            this.btOderPlan.Text = "Oder Plan: pk10-1-2-1-2-1";
            this.btOderPlan.UseVisualStyleBackColor = true;
            this.btOderPlan.Click += new System.EventHandler(this.btOderPlan_Click);
            // 
            // ClientUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 467);
            this.Controls.Add(this.grpSelfCmd);
            this.Controls.Add(this.grpCmd);
            this.Controls.Add(this.rtbCommLog);
            this.Controls.Add(this.btSignUp);
            this.Controls.Add(this.btLogin);
            this.Controls.Add(this.txtPswd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label1);
            this.Name = "ClientUI";
            this.Text = "ClientUI";
            this.grpCmd.ResumeLayout(false);
            this.grpSelfCmd.ResumeLayout(false);
            this.grpSelfCmd.PerformLayout();
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
        private System.Windows.Forms.GroupBox grpCmd;
        private System.Windows.Forms.Button btOderPlan;
        private System.Windows.Forms.Button btGetCode_ssc;
        private System.Windows.Forms.Button btPlan_ssc3;
        private System.Windows.Forms.Button btPlan_ssc2;
        private System.Windows.Forms.Button btPlan_ssc1;
        private System.Windows.Forms.Button btPlan_pks3;
        private System.Windows.Forms.Button btPlan_pks2;
        private System.Windows.Forms.Button btPlan_pks1;
        private System.Windows.Forms.GroupBox grpSelfCmd;
        private System.Windows.Forms.TextBox txtSelfCmd;
        private System.Windows.Forms.Button btSelfCmdSend;
    }
}