using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;

namespace AsyncSocketClient
{
    using XmlHelper;
    public partial class ClientUI : Form
    {
        private IOCPClient tcpClient;
        private Queue<byte[]> sendBufferQueue;
        private Semaphore sendSem;

        private const int MAX_RetryCnt = 10;
        private int retryCnt = 0;

        private const int MAX_CNT = 1000;
        int cnt = 0;
        public ClientUI()
        {
            InitializeComponent();

            Thread clientThread = new Thread(TcpClientHandle);
            clientThread.IsBackground = true;
            clientThread.Start();

            sendBufferQueue = new Queue<byte[]>();
            sendSem = new Semaphore(0, 10);
        }

        #region TcpClient Process
        public void TcpClientHandle()
        {
            tcpClient = new IOCPClient(IPAddress.Parse("127.0.0.1"), 8088);
            tcpClient.Connected += OnConnected;
            tcpClient.Disconnected += OnDisconnected;
            tcpClient.DataRecieved += OnDataRecieved;
            tcpClient.DataSent += OnDataSent;

            byte[] buffer;

            while (Thread.CurrentThread.IsAlive)
            {
                sendSem.WaitOne();
                if (sendBufferQueue.Count > 0)
                {
                    if (!tcpClient.IsConnected && retryCnt < MAX_RetryCnt)
                    {
                        tcpClient.Connect();
                        retryCnt++;
                        Thread.Sleep(100);
                        sendSem.Release();
                        continue;
                    }

                    if (cnt < MAX_CNT)
                    {
                        buffer = sendBufferQueue.Dequeue();
                        tcpClient.Send(buffer);
                        cnt++;

                        Thread.Sleep(10);
                        btLogin_Click(null, null);
                    }
                }
            }
        }

        private void OnConnected(object sender, AsyncSocketClientArgs e)
        {
            AddToCommLog(e.Msg, Color.Red);
        }

        private void OnDisconnected(object sender, AsyncSocketClientArgs e)
        {
            AddToCommLog(e.Msg, Color.Red);
        }

        private void OnDataSent(object sender, AsyncSocketClientArgs e)
        {
            AddToCommLog(e.Msg + " " + cnt, Color.Red);
        }

        private void OnDataRecieved(object sender, AsyncSocketClientArgs e)
        {
            string msg = e.Msg + Encoding.UTF8.GetString(e.Buffer);
            AddToCommLog(msg, Color.Blue);
        }
        #endregion

        #region Communication Log

        private delegate void WriteRtbTxtCallback(string str, Color color);
        private void AddToCommLog(string msg, Color color)
        {
            if(this.InvokeRequired)
            {
                Invoke(new WriteRtbTxtCallback(AddToCommLog), new object[] { msg, color});
            }
            else
            {
                msg += "\r\n";

                if( rtbCommLog.TextLength > rtbCommLog.MaxLength / 2)
                {
                    rtbCommLog.Clear();
                }

                int start = rtbCommLog.TextLength;
                rtbCommLog.AppendText(msg);
                rtbCommLog.Select(start, msg.Length);
                rtbCommLog.SelectionColor = color;
                rtbCommLog.ScrollToCaret();
            }
        }

        #endregion

        #region Login / Register
        private void btLogin_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                cnt = 0;
            }

            string str = "user=" + txtUser.Text.Trim() + "&pswd=" + txtPswd.Text.Trim();
            sendBufferQueue.Enqueue(Encoding.UTF8.GetBytes(str));
            sendSem.Release();
        }

        private void btSignUp_Click(object sender, EventArgs e)
        {
            cnt = MAX_CNT;

            long ts = DateTime.Parse("2018-04-25 11:12:13.123").ToBinary();
            //long ts = DateTime.Now.ToBinary();
            txtSelfCmd.Text = ts.ToString("X") + "\r\n";
            ts = (ts & ~0xFFFFFF);

            byte[] bts = BitConverter.GetBytes(ts );
            foreach(byte b in bts)
            {
                txtSelfCmd.Text += b.ToString("X2");
            }
            txtSelfCmd.Text += "\r\n" + DateTime.FromBinary(BitConverter.ToInt64(bts, 0)).ToString("yyyyMMdd hh:mm:ss");
        }
        #endregion

        #region PK10 Plan Set
        private void btPlan_pks1_Click(object sender, EventArgs e)
        {
           txtSelfCmd.Text += XmlHelper.Read("userInfo", "item");
        }

        private void btPlan_pks2_Click(object sender, EventArgs e)
        {

        }

        private void btPlan_pks3_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region SSC Plan Set
        private void btPlan_ssc1_Click(object sender, EventArgs e)
        {

        }

        private void btPlan_ssc2_Click(object sender, EventArgs e)
        {

        }

        private void btPlan_ssc3_Click(object sender, EventArgs e)
        {

        }

        private void btGetCode_ssc_Click(object sender, EventArgs e)
        {

        }

        #endregion 

        #region Plan Order
        private void btOderPlan_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Self-Command
        private void btSelfCmdSend_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
