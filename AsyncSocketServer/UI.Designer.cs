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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btRunCtrl = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.cmenuLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.清空ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbClientCnt = new System.Windows.Forms.Label();
            this.lbRecvBytes = new System.Windows.Forms.Label();
            this.txtClientCnt = new System.Windows.Forms.TextBox();
            this.txtRecvBytes = new System.Windows.Forms.TextBox();
            this.lstvClient = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.txtOur = new System.Windows.Forms.TextBox();
            this.btnLose = new System.Windows.Forms.Button();
            this.btnWin = new System.Windows.Forms.Button();
            this.chkCtrl = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtZxDzMax = new System.Windows.Forms.TextBox();
            this.txtZxDzMin = new System.Windows.Forms.TextBox();
            this.txtZxHeMax = new System.Windows.Forms.TextBox();
            this.txtZxHeMin = new System.Windows.Forms.TextBox();
            this.txtZxMax = new System.Windows.Forms.TextBox();
            this.txtZxMin = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtLhHeMin = new System.Windows.Forms.TextBox();
            this.txtLhMin = new System.Windows.Forms.TextBox();
            this.txtLhHeMax = new System.Windows.Forms.TextBox();
            this.txtLhMax = new System.Windows.Forms.TextBox();
            this.btnSwitchVideo = new System.Windows.Forms.Button();
            this.btnQryRecordAll = new System.Windows.Forms.Button();
            this.btnResume = new System.Windows.Forms.Button();
            this.chkAutoLose = new System.Windows.Forms.CheckBox();
            this.chkAutoWin = new System.Windows.Forms.CheckBox();
            this.btnFoceOffline = new System.Windows.Forms.Button();
            this.txtDesk = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.lbIp = new System.Windows.Forms.Label();
            this.lbMac = new System.Windows.Forms.Label();
            this.chkCollect = new System.Windows.Forms.CheckBox();
            this.txtUserGw = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cmenuLog.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器端口";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(97, 8);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(62, 25);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "8088";
            // 
            // btRunCtrl
            // 
            this.btRunCtrl.BackColor = System.Drawing.Color.Silver;
            this.btRunCtrl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btRunCtrl.Location = new System.Drawing.Point(167, 6);
            this.btRunCtrl.Margin = new System.Windows.Forms.Padding(4);
            this.btRunCtrl.Name = "btRunCtrl";
            this.btRunCtrl.Size = new System.Drawing.Size(49, 30);
            this.btRunCtrl.TabIndex = 2;
            this.btRunCtrl.Text = "启动";
            this.btRunCtrl.UseVisualStyleBackColor = false;
            this.btRunCtrl.Click += new System.EventHandler(this.btRunCtrl_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.ContextMenuStrip = this.cmenuLog;
            this.txtLog.Location = new System.Drawing.Point(237, 172);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1010, 373);
            this.txtLog.TabIndex = 3;
            // 
            // cmenuLog
            // 
            this.cmenuLog.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmenuLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清空ToolStripMenuItem,
            this.保存ToolStripMenuItem});
            this.cmenuLog.Name = "cmenuLog";
            this.cmenuLog.Size = new System.Drawing.Size(109, 52);
            // 
            // 清空ToolStripMenuItem
            // 
            this.清空ToolStripMenuItem.Name = "清空ToolStripMenuItem";
            this.清空ToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.清空ToolStripMenuItem.Text = "清空";
            this.清空ToolStripMenuItem.Click += new System.EventHandler(this.btClear_Click);
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.保存ToolStripMenuItem.Text = "保存";
            this.保存ToolStripMenuItem.Click += new System.EventHandler(this.btSave_Click);
            // 
            // lbClientCnt
            // 
            this.lbClientCnt.AutoSize = true;
            this.lbClientCnt.Location = new System.Drawing.Point(13, 47);
            this.lbClientCnt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbClientCnt.Name = "lbClientCnt";
            this.lbClientCnt.Size = new System.Drawing.Size(82, 15);
            this.lbClientCnt.TabIndex = 4;
            this.lbClientCnt.Text = "当前连接数";
            // 
            // lbRecvBytes
            // 
            this.lbRecvBytes.AutoSize = true;
            this.lbRecvBytes.Location = new System.Drawing.Point(13, 81);
            this.lbRecvBytes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRecvBytes.Name = "lbRecvBytes";
            this.lbRecvBytes.Size = new System.Drawing.Size(82, 15);
            this.lbRecvBytes.TabIndex = 4;
            this.lbRecvBytes.Text = "接收字节数";
            // 
            // txtClientCnt
            // 
            this.txtClientCnt.Location = new System.Drawing.Point(97, 42);
            this.txtClientCnt.Margin = new System.Windows.Forms.Padding(4);
            this.txtClientCnt.Name = "txtClientCnt";
            this.txtClientCnt.Size = new System.Drawing.Size(62, 25);
            this.txtClientCnt.TabIndex = 5;
            // 
            // txtRecvBytes
            // 
            this.txtRecvBytes.Location = new System.Drawing.Point(97, 75);
            this.txtRecvBytes.Margin = new System.Windows.Forms.Padding(4);
            this.txtRecvBytes.Name = "txtRecvBytes";
            this.txtRecvBytes.Size = new System.Drawing.Size(119, 25);
            this.txtRecvBytes.TabIndex = 5;
            // 
            // lstvClient
            // 
            this.lstvClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstvClient.HideSelection = false;
            this.lstvClient.Location = new System.Drawing.Point(12, 172);
            this.lstvClient.Name = "lstvClient";
            this.lstvClient.Size = new System.Drawing.Size(218, 373);
            this.lstvClient.TabIndex = 6;
            this.lstvClient.UseCompatibleStateImageBehavior = false;
            this.lstvClient.View = System.Windows.Forms.View.List;
            this.lstvClient.SelectedIndexChanged += new System.EventHandler(this.lstvClient_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(133, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "官方的数据：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(133, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "我们的数据：";
            // 
            // txtOrigin
            // 
            this.txtOrigin.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtOrigin.Location = new System.Drawing.Point(237, 110);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.ReadOnly = true;
            this.txtOrigin.Size = new System.Drawing.Size(780, 25);
            this.txtOrigin.TabIndex = 9;
            // 
            // txtOur
            // 
            this.txtOur.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtOur.Location = new System.Drawing.Point(237, 140);
            this.txtOur.Name = "txtOur";
            this.txtOur.ReadOnly = true;
            this.txtOur.Size = new System.Drawing.Size(780, 25);
            this.txtOur.TabIndex = 9;
            // 
            // btnLose
            // 
            this.btnLose.BackColor = System.Drawing.Color.LightGreen;
            this.btnLose.Location = new System.Drawing.Point(1040, 138);
            this.btnLose.Name = "btnLose";
            this.btnLose.Size = new System.Drawing.Size(71, 26);
            this.btnLose.TabIndex = 10;
            this.btnLose.Text = "输";
            this.btnLose.UseVisualStyleBackColor = false;
            this.btnLose.Click += new System.EventHandler(this.btnLose_Click);
            // 
            // btnWin
            // 
            this.btnWin.BackColor = System.Drawing.Color.LightCoral;
            this.btnWin.Location = new System.Drawing.Point(1136, 140);
            this.btnWin.Name = "btnWin";
            this.btnWin.Size = new System.Drawing.Size(71, 26);
            this.btnWin.TabIndex = 10;
            this.btnWin.Text = "赢";
            this.btnWin.UseVisualStyleBackColor = false;
            this.btnWin.Click += new System.EventHandler(this.btnWin_Click);
            // 
            // chkCtrl
            // 
            this.chkCtrl.AutoSize = true;
            this.chkCtrl.Location = new System.Drawing.Point(787, 82);
            this.chkCtrl.Name = "chkCtrl";
            this.chkCtrl.Size = new System.Drawing.Size(89, 19);
            this.chkCtrl.TabIndex = 11;
            this.chkCtrl.Text = "管控账号";
            this.chkCtrl.UseVisualStyleBackColor = true;
            this.chkCtrl.CheckedChanged += new System.EventHandler(this.chkCtrl_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtZxDzMax);
            this.groupBox1.Controls.Add(this.txtZxDzMin);
            this.groupBox1.Controls.Add(this.txtZxHeMax);
            this.groupBox1.Controls.Add(this.txtZxHeMin);
            this.groupBox1.Controls.Add(this.txtZxMax);
            this.groupBox1.Controls.Add(this.txtZxMin);
            this.groupBox1.Location = new System.Drawing.Point(277, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(221, 100);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "百家乐-筹码限制";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "对子：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(124, 75);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(15, 15);
            this.label11.TabIndex = 8;
            this.label11.Text = "-";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(124, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(15, 15);
            this.label10.TabIndex = 8;
            this.label10.Text = "-";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(125, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 15);
            this.label7.TabIndex = 8;
            this.label7.Text = "-";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "和：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "庄/闲：";
            // 
            // txtZxDzMax
            // 
            this.txtZxDzMax.Location = new System.Drawing.Point(146, 72);
            this.txtZxDzMax.Margin = new System.Windows.Forms.Padding(4);
            this.txtZxDzMax.Name = "txtZxDzMax";
            this.txtZxDzMax.Size = new System.Drawing.Size(61, 25);
            this.txtZxDzMax.TabIndex = 5;
            this.txtZxDzMax.Text = "1000";
            // 
            // txtZxDzMin
            // 
            this.txtZxDzMin.Location = new System.Drawing.Point(77, 72);
            this.txtZxDzMin.Margin = new System.Windows.Forms.Padding(4);
            this.txtZxDzMin.Name = "txtZxDzMin";
            this.txtZxDzMin.Size = new System.Drawing.Size(41, 25);
            this.txtZxDzMin.TabIndex = 5;
            this.txtZxDzMin.Text = "50";
            // 
            // txtZxHeMax
            // 
            this.txtZxHeMax.Location = new System.Drawing.Point(146, 45);
            this.txtZxHeMax.Margin = new System.Windows.Forms.Padding(4);
            this.txtZxHeMax.Name = "txtZxHeMax";
            this.txtZxHeMax.Size = new System.Drawing.Size(61, 25);
            this.txtZxHeMax.TabIndex = 5;
            this.txtZxHeMax.Text = "1000";
            // 
            // txtZxHeMin
            // 
            this.txtZxHeMin.Location = new System.Drawing.Point(77, 45);
            this.txtZxHeMin.Margin = new System.Windows.Forms.Padding(4);
            this.txtZxHeMin.Name = "txtZxHeMin";
            this.txtZxHeMin.Size = new System.Drawing.Size(41, 25);
            this.txtZxHeMin.TabIndex = 5;
            this.txtZxHeMin.Text = "50";
            // 
            // txtZxMax
            // 
            this.txtZxMax.Location = new System.Drawing.Point(147, 18);
            this.txtZxMax.Margin = new System.Windows.Forms.Padding(4);
            this.txtZxMax.Name = "txtZxMax";
            this.txtZxMax.Size = new System.Drawing.Size(60, 25);
            this.txtZxMax.TabIndex = 5;
            this.txtZxMax.Text = "50000";
            // 
            // txtZxMin
            // 
            this.txtZxMin.Location = new System.Drawing.Point(77, 18);
            this.txtZxMin.Margin = new System.Windows.Forms.Padding(4);
            this.txtZxMin.Name = "txtZxMin";
            this.txtZxMin.Size = new System.Drawing.Size(41, 25);
            this.txtZxMin.TabIndex = 5;
            this.txtZxMin.Text = "50";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txtLhHeMin);
            this.groupBox2.Controls.Add(this.txtLhMin);
            this.groupBox2.Controls.Add(this.txtLhHeMax);
            this.groupBox2.Controls.Add(this.txtLhMax);
            this.groupBox2.Location = new System.Drawing.Point(513, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(224, 75);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "龙虎斗-筹码限制";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 15);
            this.label8.TabIndex = 8;
            this.label8.Text = "和：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 15);
            this.label9.TabIndex = 8;
            this.label9.Text = "龙/虎：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(125, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(15, 15);
            this.label13.TabIndex = 8;
            this.label13.Text = "-";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(126, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(15, 15);
            this.label12.TabIndex = 8;
            this.label12.Text = "-";
            // 
            // txtLhHeMin
            // 
            this.txtLhHeMin.Location = new System.Drawing.Point(77, 45);
            this.txtLhHeMin.Margin = new System.Windows.Forms.Padding(4);
            this.txtLhHeMin.Name = "txtLhHeMin";
            this.txtLhHeMin.Size = new System.Drawing.Size(41, 25);
            this.txtLhHeMin.TabIndex = 5;
            this.txtLhHeMin.Text = "20";
            // 
            // txtLhMin
            // 
            this.txtLhMin.Location = new System.Drawing.Point(77, 18);
            this.txtLhMin.Margin = new System.Windows.Forms.Padding(4);
            this.txtLhMin.Name = "txtLhMin";
            this.txtLhMin.Size = new System.Drawing.Size(41, 25);
            this.txtLhMin.TabIndex = 5;
            this.txtLhMin.Text = "20";
            // 
            // txtLhHeMax
            // 
            this.txtLhHeMax.Location = new System.Drawing.Point(148, 44);
            this.txtLhHeMax.Margin = new System.Windows.Forms.Padding(4);
            this.txtLhHeMax.Name = "txtLhHeMax";
            this.txtLhHeMax.Size = new System.Drawing.Size(60, 25);
            this.txtLhHeMax.TabIndex = 5;
            this.txtLhHeMax.Text = "200";
            // 
            // txtLhMax
            // 
            this.txtLhMax.Location = new System.Drawing.Point(148, 17);
            this.txtLhMax.Margin = new System.Windows.Forms.Padding(4);
            this.txtLhMax.Name = "txtLhMax";
            this.txtLhMax.Size = new System.Drawing.Size(60, 25);
            this.txtLhMax.TabIndex = 5;
            this.txtLhMax.Text = "2000";
            // 
            // btnSwitchVideo
            // 
            this.btnSwitchVideo.Location = new System.Drawing.Point(1040, 14);
            this.btnSwitchVideo.Name = "btnSwitchVideo";
            this.btnSwitchVideo.Size = new System.Drawing.Size(187, 26);
            this.btnSwitchVideo.TabIndex = 10;
            this.btnSwitchVideo.Text = "切换视频";
            this.btnSwitchVideo.UseVisualStyleBackColor = true;
            this.btnSwitchVideo.Click += new System.EventHandler(this.btnSwitchVideo_Click);
            // 
            // btnQryRecordAll
            // 
            this.btnQryRecordAll.Location = new System.Drawing.Point(1040, 46);
            this.btnQryRecordAll.Name = "btnQryRecordAll";
            this.btnQryRecordAll.Size = new System.Drawing.Size(187, 26);
            this.btnQryRecordAll.TabIndex = 10;
            this.btnQryRecordAll.Text = "查看注单";
            this.btnQryRecordAll.UseVisualStyleBackColor = true;
            this.btnQryRecordAll.Click += new System.EventHandler(this.btnQryRecordOur_Click);
            // 
            // btnResume
            // 
            this.btnResume.Location = new System.Drawing.Point(1040, 76);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(87, 26);
            this.btnResume.TabIndex = 10;
            this.btnResume.Text = "恢复路子";
            this.btnResume.UseVisualStyleBackColor = true;
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // chkAutoLose
            // 
            this.chkAutoLose.AutoSize = true;
            this.chkAutoLose.Font = new System.Drawing.Font("宋体", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkAutoLose.Location = new System.Drawing.Point(1040, 114);
            this.chkAutoLose.Name = "chkAutoLose";
            this.chkAutoLose.Size = new System.Drawing.Size(71, 18);
            this.chkAutoLose.TabIndex = 11;
            this.chkAutoLose.Text = "自动输";
            this.chkAutoLose.UseVisualStyleBackColor = true;
            this.chkAutoLose.CheckedChanged += new System.EventHandler(this.chkAutoLose_CheckedChanged);
            // 
            // chkAutoWin
            // 
            this.chkAutoWin.AutoSize = true;
            this.chkAutoWin.Font = new System.Drawing.Font("宋体", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkAutoWin.Location = new System.Drawing.Point(1136, 113);
            this.chkAutoWin.Name = "chkAutoWin";
            this.chkAutoWin.Size = new System.Drawing.Size(71, 18);
            this.chkAutoWin.TabIndex = 11;
            this.chkAutoWin.Text = "自动赢";
            this.chkAutoWin.UseVisualStyleBackColor = true;
            this.chkAutoWin.CheckedChanged += new System.EventHandler(this.chkAutoWin_CheckedChanged);
            // 
            // btnFoceOffline
            // 
            this.btnFoceOffline.Location = new System.Drawing.Point(1148, 76);
            this.btnFoceOffline.Name = "btnFoceOffline";
            this.btnFoceOffline.Size = new System.Drawing.Size(79, 26);
            this.btnFoceOffline.TabIndex = 10;
            this.btnFoceOffline.Text = "踢下线";
            this.btnFoceOffline.UseVisualStyleBackColor = true;
            this.btnFoceOffline.Click += new System.EventHandler(this.btnFoceOffline_Click);
            // 
            // txtDesk
            // 
            this.txtDesk.Location = new System.Drawing.Point(873, 48);
            this.txtDesk.Margin = new System.Windows.Forms.Padding(4);
            this.txtDesk.Name = "txtDesk";
            this.txtDesk.Size = new System.Drawing.Size(58, 25);
            this.txtDesk.TabIndex = 5;
            this.txtDesk.Text = "大厅";
            this.txtDesk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(784, 54);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(82, 15);
            this.label14.TabIndex = 8;
            this.label14.Text = "当前台桌：";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbIp
            // 
            this.lbIp.AutoSize = true;
            this.lbIp.Location = new System.Drawing.Point(789, 4);
            this.lbIp.Name = "lbIp";
            this.lbIp.Size = new System.Drawing.Size(38, 15);
            this.lbIp.TabIndex = 8;
            this.lbIp.Text = "Ip：";
            this.lbIp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbMac
            // 
            this.lbMac.AutoSize = true;
            this.lbMac.Location = new System.Drawing.Point(789, 25);
            this.lbMac.Name = "lbMac";
            this.lbMac.Size = new System.Drawing.Size(46, 15);
            this.lbMac.TabIndex = 8;
            this.lbMac.Text = "Mac：";
            this.lbMac.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkCollect
            // 
            this.chkCollect.AutoSize = true;
            this.chkCollect.Location = new System.Drawing.Point(897, 81);
            this.chkCollect.Name = "chkCollect";
            this.chkCollect.Size = new System.Drawing.Size(89, 19);
            this.chkCollect.TabIndex = 11;
            this.chkCollect.Text = "采集账号";
            this.chkCollect.UseVisualStyleBackColor = true;
            this.chkCollect.CheckedChanged += new System.EventHandler(this.chkCollect_CheckedChanged);
            // 
            // txtUserGw
            // 
            this.txtUserGw.Location = new System.Drawing.Point(47, 138);
            this.txtUserGw.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserGw.Name = "txtUserGw";
            this.txtUserGw.Size = new System.Drawing.Size(79, 25);
            this.txtUserGw.TabIndex = 5;
            this.txtUserGw.Text = "13584297";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(13, 114);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(82, 15);
            this.label15.TabIndex = 8;
            this.label15.Text = "绑定账号：";
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 553);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lbMac);
            this.Controls.Add(this.lbIp);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkAutoWin);
            this.Controls.Add(this.txtUserGw);
            this.Controls.Add(this.txtDesk);
            this.Controls.Add(this.chkAutoLose);
            this.Controls.Add(this.chkCollect);
            this.Controls.Add(this.chkCtrl);
            this.Controls.Add(this.btnWin);
            this.Controls.Add(this.btnQryRecordAll);
            this.Controls.Add(this.btnSwitchVideo);
            this.Controls.Add(this.btnFoceOffline);
            this.Controls.Add(this.btnResume);
            this.Controls.Add(this.btnLose);
            this.Controls.Add(this.txtOur);
            this.Controls.Add(this.txtOrigin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstvClient);
            this.Controls.Add(this.txtRecvBytes);
            this.Controls.Add(this.txtClientCnt);
            this.Controls.Add(this.lbRecvBytes);
            this.Controls.Add(this.lbClientCnt);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btRunCtrl);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UI";
            this.Text = "UI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UI_FormClosed);
            this.Load += new System.EventHandler(this.UI_Load);
            this.cmenuLog.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.ListView lstvClient;
        private System.Windows.Forms.ContextMenuStrip cmenuLog;
        private System.Windows.Forms.ToolStripMenuItem 清空ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.TextBox txtOur;
        private System.Windows.Forms.Button btnLose;
        private System.Windows.Forms.Button btnWin;
        private System.Windows.Forms.CheckBox chkCtrl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtZxDzMin;
        private System.Windows.Forms.TextBox txtZxHeMin;
        private System.Windows.Forms.TextBox txtZxMin;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtLhHeMin;
        private System.Windows.Forms.TextBox txtLhMin;
        private System.Windows.Forms.Button btnSwitchVideo;
        private System.Windows.Forms.Button btnQryRecordAll;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtZxDzMax;
        private System.Windows.Forms.TextBox txtZxHeMax;
        private System.Windows.Forms.TextBox txtZxMax;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtLhHeMax;
        private System.Windows.Forms.TextBox txtLhMax;
        private System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.CheckBox chkAutoLose;
        private System.Windows.Forms.CheckBox chkAutoWin;
        private System.Windows.Forms.Button btnFoceOffline;
        private System.Windows.Forms.TextBox txtDesk;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbIp;
        private System.Windows.Forms.Label lbMac;
        private System.Windows.Forms.CheckBox chkCollect;
        private System.Windows.Forms.TextBox txtUserGw;
        private System.Windows.Forms.Label label15;
    }
}