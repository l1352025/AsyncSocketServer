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
    using ElectricPowerLib.Common;
    using XmlHelper;
    public partial class ClientUI : Form
    {
        private IOCPClient tcpClient;
        private Queue<byte[]> sendBufferQueue;
        private Semaphore sendSem;
        Thread clientThread;

        private const int MAX_RetryCnt = 10;
        private int retryCnt = 0;

        private const int MAX_Repeat = 1;
        int cnt = 0;
        public ClientUI()
        {
            InitializeComponent();

            sendBufferQueue = new Queue<byte[]>();
            sendSem = new Semaphore(0, 10);
        }

        #region TcpClient Process
        public void TcpClientHandle()
        {
            byte[] buffer;

            while (Thread.CurrentThread.IsAlive)
            {
                if (!tcpClient.IsConnected && retryCnt < MAX_RetryCnt)
                {
                    tcpClient.Connect();
                    retryCnt++;
                    Thread.Sleep(100);

                    while(sendBufferQueue.Count > 0)
                        sendBufferQueue.Dequeue();

                    continue;
                }

                if (sendBufferQueue.Count > 0)
                {
                    sendSem.WaitOne();
                    buffer = sendBufferQueue.Dequeue();
                    tcpClient.Send(buffer);
                    cnt++;

                    if (cnt < MAX_Repeat)
                    {
                        Thread.Sleep(10);
                        //btLogin_Click(null, null);
                    }
                }

                Thread.Sleep(10);
            }
        }
        #endregion

        #region Event connect/discnnt/send/recv
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
            sb.Clear();
            sb.Append(DateTime.Now.ToString("[HH:mm:ss.fff] ") + "  Tx: ");
            for (int i = 0; i < e.Length; i++)
            {
                sb.Append(e.Buffer[i].ToString("X2") + " ");
            }
            AddToCommLog(sb.ToString(), Color.Red);
            //AddToCommLog(e.Msg + " " + cnt, Color.Red);
        }

        StringBuilder sb = new StringBuilder();

        private void OnDataRecieved(object sender, AsyncSocketClientArgs e)
        {
            //string msg = e.Msg + Encoding.UTF8.GetString(e.Buffer);
            
            byte[] buf = e.Buffer;
            int index = 0;
            int len = e.Length;

            sb.Clear();
            sb.Append(DateTime.Now.ToString("[HH:mm:ss.fff] ") + "Rx  : ");
            for (int i = index; i < index + len; i++)
            {
                sb.Append(buf[i].ToString("X2") + " ");
            }

            AddToCommLog(sb.ToString(), Color.Blue);
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
            cnt = MAX_Repeat;

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

        private void btGetCode_ssc_Click(object sender, EventArgs e)
        {

        }

        #region Self-Command
        private void btSelfCmdSend_Click(object sender, EventArgs e)
        {
            byte[] bytes;
            if (chkHex.Checked)
            {
                bytes = Util.GetBytesFromStringHex(txtSelfCmd.Text, " ");
            }
            else
            {
                bytes = Encoding.UTF8.GetBytes(txtSelfCmd.Text);
            }
            sendBufferQueue.Enqueue(bytes);
            sendSem.Release();
        }
        #endregion

        private void btnCnnt_Click(object sender, EventArgs e)
        {
            if(btnCnnt.Text == "连接")
            {
                if (string.IsNullOrEmpty(txtIp.Text))
                {
                    txtIp.Text = "127.0.0.1";
                }

                try
                {
                    tcpClient = new IOCPClient(IPAddress.Parse(txtIp.Text), ushort.Parse(txtPort.Text));
                }
                catch (Exception) {
                    MessageBox.Show("Tcp Socket 创建失败！");
                    return; 
                }

                tcpClient.Connected += OnConnected;
                tcpClient.Disconnected += OnDisconnected;
                tcpClient.DataRecieved += OnDataRecieved;
                tcpClient.DataSent += OnDataSent;

                retryCnt = 0;
                clientThread = new Thread(TcpClientHandle);
                clientThread.IsBackground = true;
                clientThread.Start();

                btnCnnt.Text = "断开";
            }
            else
            {
                clientThread.Abort();
                if(tcpClient.IsConnected) tcpClient.Close();

                btnCnnt.Text = "连接";
            }
        }
    }
}
