using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Concurrent;

namespace AsyncSocketServer
{
    public partial class UI : Form
    {
        AsyncSocketServer _server;
        Thread _threadUpdateUI;
        ConcurrentQueue<LogOutEventArgs> _logOutQueue;
        ManualResetEvent _logRecvEvt;
        public UI()
        {
            InitializeComponent();

            _server = new AsyncSocketServer(8088, 8000);

            _logOutQueue = new ConcurrentQueue<LogOutEventArgs>();
            _logRecvEvt = new ManualResetEvent(false);

            _threadUpdateUI = new Thread(UpdateUiTask);
            _threadUpdateUI.Start();

        }

        private void UI_Load(object sender, EventArgs e)
        {
            DataAccess da = new DataAccess();

            DataTable tb = new DataTable("testTb");
            tb.Columns.Add("Id");
            tb.Columns.Add("UserName");
            tb.Columns.Add("Password");
            tb.Columns.Add("Token");

            for (int i = 0; i < 1000; i++)
            {
                ///*
                UserInfo user = new UserInfo();
                
                user.Id = (i + 1).ToString();
                user.UserName = "user_" + (i + 1).ToString() ;
                user.Password = "123456";
                user.Token = "afasdaafsafadfaf";

                da.DbInsert<UserInfo>(user);
                //*/

                /*
                DataRow user = tb.NewRow();
                user["Id"] = (i + 1).ToString();
                user["UserName"] = "user_" + (i + 1).ToString();
                user["Password"] = "123456";
                user["Token"] = "afasdaafsafadfaf";

                //da.DbInsert<DataTable>(tb);
                tb.Rows.Add(user);
                 */

                user = null;

                //Thread.Sleep(5);
            }
            //da.DbInsert<DataTable>(tb);

        }

        private void UI_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_server.IsRunning)
            {
                _server.Stop();
                _server.LogOut -= OnLogOut;
            }
            _threadUpdateUI.Abort();
            _threadUpdateUI.Join();
        }

        private void btRunCtrl_Click(object sender, EventArgs e)
        {
            if(btRunCtrl.Text == "Start")
            {
                try
                {
                    //server = new AsyncSocketServer(Convert.ToUInt16(txtPort.Text), 8000);
                    _server.LogOut += OnLogOut;
                    _server.Start();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Server Port Error : " + ex.Message);
                    return;
                }

                btRunCtrl.BackColor = Color.Lime;
                btRunCtrl.Text = "Stop";
            }
            else
            {
                _server.Stop();
                _server.LogOut -= OnLogOut;

                btRunCtrl.BackColor = Color.Silver;
                btRunCtrl.Text = "Start";
            }
        }

        private void OnLogOut(object sender, LogOutEventArgs e)
        {
            //UpdateUI(sender, e);      // 直接更新可能影响服务器性能，先加入队列缓冲一下，让专门线程去更新

            _logOutQueue.Enqueue(e);
            _logRecvEvt.Set();
        }

        public void UpdateUiTask()
        {
            LogOutEventArgs logArgs;
            Thread.CurrentThread.IsBackground = true;

            while (Thread.CurrentThread.IsAlive)
            {
                _logRecvEvt.WaitOne();
                while (_logOutQueue.Count > 0 && _logOutQueue.TryDequeue(out logArgs))
                {
                    UpdateUI(this, logArgs);
                }
                _logRecvEvt.Reset();
            }
        }

        private void UpdateUI(object sender, LogOutEventArgs e)
        {
            if (this.InvokeRequired)
            {
                Invoke(new EventHandler<LogOutEventArgs>(UpdateUI), new object[] { sender, e });
            }
            else
            {
                txtClientCnt.Text = e.ClientCount.ToString();

                txtRecvBytes.Text = e.RecvBytes.ToString();

                if (txtLog.Text.Length > txtLog.MaxLength / 2)  txtLog.Clear();

                txtLog.AppendText(e.Msg + "\r\n");
                txtLog.ScrollToCaret();
            }
        }

    }
}
