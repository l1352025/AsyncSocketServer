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
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;
using ElectricPowerLib.Common;
using System.Threading.Tasks;

namespace AsyncSocketServer
{
    public partial class UI : Form
    {
        AsyncSocketServer _server;
        Thread _threadUpdateUI;
        Thread _threadVideoHandle;
        Thread _threadVideoSave;
        ConcurrentQueue<LogOutEventArgs> _logOutQueue;
        ConcurrentQueue<BufferItem> _videoQueue;
        ConcurrentQueue<BufferItem> _videoSaveQueue;
        ConcurrentQueue<BufferItem> _videoReplaceQueue;
        ManualResetEvent _logRecvEvt;
        AsyncSocketUserToken commToken;
        AsyncSocketUserToken videoToken;
        Packet pkt = new Packet();
        UserData usrInfo = new UserData();
        BufferManager _bufferMgr = new BufferManager(800, 250 * 1024, 2000, 6 * 1024);
        Log Log = new Log("C:\\Users\\wushu\\Desktop\\server.log");
        string _videoBaseDir = "D:\\video";


        public UI()
        {
            InitializeComponent();

            this.Text = "TcpServer";

            _server = new AsyncSocketServer(8088, 100, 1024 * 100);
            _server.OnRecieve += OnReceived;
            _server.OnConnectChange += OnCnntChange;

            _logOutQueue = new ConcurrentQueue<LogOutEventArgs>();
            _logRecvEvt = new ManualResetEvent(false);

            _threadUpdateUI = new Thread(UpdateUiTask) { IsBackground = true };
            _threadUpdateUI.Start();

            _videoQueue = new ConcurrentQueue<BufferItem>();
            _threadVideoHandle = new Thread(VideoHandleTask) { IsBackground = true };
            _threadVideoHandle.Start();

            _videoReplaceQueue = new ConcurrentQueue<BufferItem>();

            _videoSaveQueue = new ConcurrentQueue<BufferItem>();
            _threadVideoSave = new Thread(VideoSaveTask) { IsBackground = true };
            _threadVideoSave.Start();

            //for (int i = 0; i < pkt.Length; i++)
            //{
            //    pkt[i] = new Packet();
            //}

            //usrInfo.userBind = "27924571";  // 绑定账号
            //usrInfo.pswdBind = "27924571";
            //usrInfo.userBind = "31810750";  // 绑定账号
            //usrInfo.pswdBind = "31810750";

            usrInfo.userBind = "13584297";  // 绑定账号
            usrInfo.pswdBind = "13584297";

            usrInfo.user = "27924571";      // 平台账号
            usrInfo.pswd = "27924571";
            usrInfo.remain = 10000;         // 平台账号余额
            usrInfo.profit = 0;
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
                user.UserName = "user_" + (i + 1).ToString();
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
                _server.OnRecieve -= OnReceived;
            }
            _threadUpdateUI.Abort();
            _threadUpdateUI.Join();
        }

        private void btRunCtrl_Click(object sender, EventArgs e)
        {
            if (btRunCtrl.Text == "启动")
            {
                try
                {
                    //server = new AsyncSocketServer(Convert.ToUInt16(txtPort.Text), 8000);
                    _server.LogOut += OnLogOut;
                    _server.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Server Port Error : " + ex.Message);
                    return;
                }

                btRunCtrl.BackColor = Color.Lime;
                btRunCtrl.Text = "停止";

                usrInfo.userBind = txtUserGw.Text;
                txtUserGw.Enabled = false;
            }
            else
            {
                _server.Stop();
                _server.LogOut -= OnLogOut;

                btRunCtrl.BackColor = Color.Silver;
                btRunCtrl.Text = "启动";

                txtUserGw.Enabled = true;

            }
        }

        private void OnLogOut(object sender, LogOutEventArgs e)
        {
            //UpdateUI(sender, e);      // 直接更新可能影响服务器性能，先加入队列缓冲一下，让专门线程去更新

            _logOutQueue.Enqueue(e);
            _logRecvEvt.Set();
        }

        private void OnCnntChange(object sender, SocketAsyncEventArgs e)
        {
            List<string> list = new List<string>();

            lock (_server.ConnectedClients.List) 
            {
                foreach (var item in _server.ConnectedClients.List)
                {
                    list.Add(item.RemoteEndPoint.ToString());
                }
            }
            Invoke(new Action<List<string>>(lst => {
                lstvClient.Items.Clear();
                lstvClient.BeginUpdate();
                foreach (var item in list)
                {
                    lstvClient.Items.Add(item);
                }
                lstvClient.EndUpdate();
            }), list);

            if (list.Count == 1) commToken = (AsyncSocketUserToken)e.UserToken;
            if (list.Count == 2) videoToken = (AsyncSocketUserToken)e.UserToken;


            if ((string)sender == "discnnt")
            {
                commToken = null;
                videoToken = null;
            }
        }

        private void lstvClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstvClient.SelectedIndices.Count > 0)
            {
                //int index = lstvClient.SelectedIndices[0];
                //commToken = _server.ConnectedClients[index];
            }
        }

        #region message show / clear / save 

        public void UpdateUiTask()
        {
            LogOutEventArgs logArgs;

            while (Thread.CurrentThread.IsAlive)
            {
                _logRecvEvt.WaitOne();
                while (_logOutQueue.Count > 0 && _logOutQueue.TryDequeue(out logArgs))
                {
                    UpdateUI(this, logArgs);
                    //Log.WriteLine(logArgs.Msg);
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
                if (e.RecvBytes >= 0)
                {
                    txtRecvBytes.Text = e.RecvBytes.ToString();
                    txtClientCnt.Text = e.ClientCount.ToString();
                }

                if (txtLog.Text.Length > txtLog.MaxLength / 2) txtLog.Clear();

                txtLog.AppendText(e.Msg + "\r\n");
                txtLog.ScrollToCaret();
            }
        }

        private void ShowMsg(string msg, bool showTime = true)
        {
            //if (this.InvokeRequired)
            //{
            //    Invoke(new Action<string, Color, bool>(ShowMsg), new object[] { msg, fgColor, showTime });
            //    return;
            //}

            if (showTime)
            {
                msg = "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] " + msg;
            }

            var log = new LogOutEventArgs(msg, 0, -1);
            _logOutQueue.Enqueue(log);
            _logRecvEvt.Set();
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            _server.TotalRecvdBytes = 0;
            txtRecvBytes.Text = "";
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (txtLog.Text == "")
            {
                return;
            }


            string strFileName;
            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.Filter = "*.txt(文本文件)|*.txt";
            saveFileDlg.DefaultExt = "txt";
            saveFileDlg.FileName = "";
            if (DialogResult.OK != saveFileDlg.ShowDialog()) return;

            strFileName = saveFileDlg.FileName;

            using (StreamWriter sw = new StreamWriter(strFileName, false, Encoding.UTF8))
            {
                sw.WriteLine(txtLog.Text.Replace("\n", "\r\n"));
            }

            ShowMsg("保存成功 ！\r\n");
        }
        #endregion


        #region 接收解析
        StringBuilder sb = new StringBuilder();

        private void SendBack(AsyncEventArgs recvArgs, AsyncEventArgs sendArgs)
        {
            byte[] recvBuf = recvArgs.Buffer;
            int recvIdx = recvArgs.Offset;
            int recvLen = recvArgs.BytesTransferred;

            //var token = (recvArgs.UserToken as AsyncSocketUserToken);
            //token.FreeArgs(sendArgs);
            //recvArgs.IsReuse = true;
            //_server.Send(recvArgs, recvBuf, recvIdx, recvLen);

            byte[] sendBuf = sendArgs.Buffer;
            int sendIdx = sendArgs.Offset;
            int sendLen = recvLen;
            sendArgs.IsReuse = true;
            Array.Copy(recvBuf, recvIdx, sendBuf, sendIdx, sendLen);
            _server.Send(sendArgs, sendBuf, sendIdx, sendLen);
        }

        private void OnReceived(object sender, AsyncEventArgs aRecvArgs)
        {
            var userToken = aRecvArgs.UserToken as AsyncSocketUserToken;
            byte[] recvBuf = aRecvArgs.Buffer;
            int recvIdx = aRecvArgs.Offset;
            int recvLen = aRecvArgs.BytesTransferred;

            var sendArgs = userToken.NewArgs();
            byte[] sendBuf = sendArgs.Buffer;
            int sendIdx = sendArgs.Offset;
            int sendLen = 0;
            byte[] tmpBuf;

            if (recvLen > 10240)
            {
                Task.Factory.StartNew(() =>
                {

                    int free1 = 0;
                    int free2 = 0;
                    int cnt1 = _bufferMgr.CheckAllocatedBufCnt(ref free1);
                    int cnt2 = _bufferMgr.CheckAllocatedSmallBufCnt(ref free2);
                    Log.WriteLine("recv big data len = " + recvLen
                        + "\t big buf used -- reuse = " + cnt1 + "--" + free1
                        + "\t small buf used -- reuse = " + cnt2 + "--" + free2, true);

                    //var strHex = Util.GetStringHexFromBytes(recvBuf, recvIdx, recvLen);
                    //ShowMsg("video Big pkt: " + strHex);
                });
            }


            sb.Clear();
            sb.Append(DateTime.Now.ToString("[HH:mm:ss.fff] ") + "Rx  : ");
            int logLen = recvLen > 200 ? 12 : recvLen;
            for (int i = recvIdx; i < recvIdx + logLen; i++)
            {
                sb.Append(recvBuf[i].ToString("X2") + " ");
            }
            var log = new LogOutEventArgs(sb.ToString(), _server.ClientCount, _server.TotalRecvdBytes);
            _logOutQueue.Enqueue(log);
            _logRecvEvt.Set();


            byte type = recvBuf[recvIdx + 3];
            if (type != 0) type = 1;
            recvBuf[recvIdx + 3] = 0;

            byte desk = 0;
            if(recvLen > 8) desk = recvBuf[recvIdx + 8];
            
            int cmd = BitConverter.ToInt32(recvBuf, recvIdx);

            

            if (recvLen < 8) return;

            switch (cmd)
            {
                case 0x222A:    // 登录 请求/响应
                    if (recvLen == 48 && type == 1)
                    {
                        pkt.loginReq = Packet.BytesToStruct<stLoginReq>(recvBuf, recvIdx);

                        // 暂时不用验证登录
                        usrInfo.user = pkt.loginReq.user;

                        usrInfo.loginKey = (int)(pkt.loginReq.dwFixed ^ 0xFFFFFFFF);
                        usrInfo.commuKey = new Random().Next();
                        usrInfo.logRlt = 0;

                        usrInfo.isLogin = true;
                        usrInfo.loginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        pkt.loginRsp2.user = usrInfo.userBind;
                        pkt.loginRsp2.pswd = usrInfo.pswdBind;
                        pkt.loginRsp2.ctrl = (char)usrInfo.ctrl;
                        pkt.loginRsp2.logRlt = (char)usrInfo.logRlt;
                        pkt.loginRsp2.commuKey = usrInfo.commuKey;
                        sendLen = pkt.loginRsp2.len;
                        Packet.StructToBytes(pkt.loginRsp2, sendBuf, sendIdx, sendLen);
                        _server.Send(sendArgs, sendBuf, sendIdx, sendLen);
                    }
                    else
                    {
                        SendBack(aRecvArgs, sendArgs);
                    }
                    break;

                case 0x222C:    // 设备信息：ip 和 mac
                    if(recvLen  == 18)
                    {
                        pkt.devInfo = Packet.BytesToStruct<stDevInfo>(recvBuf, recvIdx);
                        SetDevInfo(pkt.devInfo.ip, pkt.devInfo.mac);
                    }
                    break;

                case 0xABCD:    // 客户端心跳

                    break;

                case 0xAB01:    // 视频注册
                    pkt.videoRegReq = Packet.BytesToStruct<stVideoRegReq>(recvBuf, recvIdx);
                    
                    // 用户校验：已登录 并且 未注册 则返回成功
                    if (pkt.videoRegReq.user == usrInfo.user 
                        && usrInfo.videoRegFlag == 0)
                    { 
                        pkt.videoRegRsp.result = 0;
                        //usrInfo.videoRegFlag = 1;
                    }
                    else
                    {
                        pkt.videoRegRsp.result = 1;
                    }
                    sendLen = pkt.videoRegRsp.len;
                    Packet.StructToBytes(pkt.videoRegRsp, sendBuf, sendIdx, sendLen);
                    _server.Send(sendArgs, sendBuf, sendIdx, sendLen);
                    break;

                case 0xAB02:    // 视频数据上传 请求 / 响应（无）
                    if (recvLen > 20)
                    {
                        int videoLen = recvLen - 20;
                        BufferItem videoDown = new BufferItem();
                        _bufferMgr.SetBuffer(videoDown, recvLen);
                        videoDown.DataLen = recvLen - 12;

                        tmpBuf = BitConverter.GetBytes(0xFFAB03);
                        Array.Copy(tmpBuf, 0, videoDown.Buffer, videoDown.Offset, 4);
                        tmpBuf = BitConverter.GetBytes(videoDown.DataLen);
                        Array.Copy(tmpBuf, 0, videoDown.Buffer, videoDown.Offset + 4, 4);
                        Array.Copy(recvBuf, recvIdx + 20, videoDown.Buffer, videoDown.Offset + 8, videoLen);

                        _videoQueue.Enqueue(videoDown);
                    }
                    break;

                case 0xAB03:    // 视频数据下发 请求 / 响应（无）
                    break;

                case 0x2730:    // 台桌路单 请求
                    break;

                case 0x2718:    // 台桌路单 通知
                case 0x2731:    // 台桌路单 响应
                    if(usrInfo.ctrl == 0)
                    {
                        SendBack(aRecvArgs, sendArgs);
                        break;
                    }
                    pkt.openListRsp = Packet.BytesToStruct<stOpenListRsp>(recvBuf, recvIdx, recvLen);
                    SetOpenList(pkt.openListRsp);
                    sendLen = pkt.openListRsp.len;
                    Packet.StructToBytes(pkt.openListRsp, sendBuf, sendIdx, sendLen);
                    _server.Send(sendArgs, sendBuf, sendIdx, sendLen);
                    break;

                case 0x2715:    // 切换台桌 请求/响应
                    pkt.deskChange = Packet.BytesToStruct<stChangeDesk>(recvBuf, recvIdx);
                    SetCurrentDesk(pkt.deskChange.deskLast, pkt.deskChange.deskCurr);
                    
                    if (pkt.deskChange.deskCurr == 0)
                    {
                        int free1 = 0;
                        int free2 = 0;
                        int cnt1 = _bufferMgr.CheckAllocatedBufCnt(ref free1);
                        int cnt2 = _bufferMgr.CheckAllocatedSmallBufCnt(ref free2);
                        Log.WriteLine("back to game hall "
                            + "\t big buf used -- reuse = " + cnt1 + "--" + free1
                            + "\t small buf used -- reuse = " + cnt2 + "--" + free2);
                    }

                    SendBack(aRecvArgs, sendArgs);
                    break;

                case 0x2713:    // 筹码设置
                    if (usrInfo.ctrl == 0)
                    {
                        SendBack(aRecvArgs, sendArgs);
                        break;
                    }
                    pkt.cmSet = Packet.BytesToStruct<stCmSetNtf>(recvBuf, recvIdx);
                    SetCmSet(pkt.cmSet.cmType);
                    sendLen = pkt.cmSet.len;
                    Packet.StructToBytes(pkt.cmSet, sendBuf, sendIdx, sendLen);
                    _server.Send(sendArgs, sendBuf, sendIdx, sendLen);
                    break;

                case 0x2719:    // 游戏状态
                    pkt.gameState = Packet.BytesToStruct<stGameState>(recvBuf, recvIdx);
                    SetDeskState(0, desk);
                    SetDeskState(1, desk);
                    SendBack(aRecvArgs, sendArgs); 
                    break;

                case 0x271A:    // 开奖通知
                    int result = recvBuf[recvIdx + 12];
                    SetOpenResult(0, desk, result);
                    SetOpenResult(1, desk, result);

                    if (usrInfo.ctrl == 0)
                    {
                        SendBack(aRecvArgs, sendArgs);
                        break;
                    }

                    sendLen = pkt.openResult.len;
                    Packet.StructToBytes(pkt.openResult, sendBuf, sendIdx, sendLen);
                    _server.Send(sendArgs, sendBuf, sendIdx, sendLen);

                    if (desk == usrInfo.desk)
                    {
                        UpdateOriginData(GetUserStr(0));
                        UpdateOurData(GetUserStr(1));
                        ShowMsg(GetBetInfoStr(0));
                        ShowMsg(GetBetInfoStr(1));
                    }

                    break;

                case 0x2712:    // 玩家数据
                    pkt.playerData = Packet.BytesToStruct<stPlayerData>(recvBuf, recvIdx);
                    
                    if (usrInfo.ctrl == 0)
                    {
                        SendBack(aRecvArgs, sendArgs);
                        break;
                    }

                    usrInfo.remainBind = pkt.playerData.remain;
                    usrInfo.profitBind = pkt.playerData.profit;
                    pkt.playerData.remain = usrInfo.remain;
                    pkt.playerData.profit = usrInfo.profit;

                    sendLen = pkt.playerData.len;
                    Packet.StructToBytes(pkt.playerData, sendBuf, sendIdx, sendLen);
                    _server.Send(sendArgs, sendBuf, sendIdx, sendLen);
                    break;

                case 0x2720:    // 游戏赔率
                    pkt.gameOdds = Packet.BytesToStruct<stGameOddsNtf>(recvBuf, recvIdx);
                    SetOdds(pkt.gameOdds);
                    SendBack(aRecvArgs, sendArgs);
                    break;

                case 0x271C:    // 投注信息 请求/响应/通知
                    pkt.betInfo = Packet.BytesToStruct<stBetInfo>(recvBuf, recvIdx);
                    SetBetInfo(type, pkt.betInfo.desk);

                    new Thread(() =>
                    {
                        if (type == 0)
                            UpdateOriginData(GetUserStr(type));
                        else
                            UpdateOurData(GetUserStr(type));
                        ShowMsg(GetBetInfoStr(type));
                    }).Start();

                    if (usrInfo.ctrl == 0)
                    {
                        SendBack(aRecvArgs, sendArgs);
                        break;
                    }

                    sendLen = pkt.betInfo.len;
                    Packet.StructToBytes(pkt.betInfo, sendBuf, sendIdx, sendLen);
                    _server.Send(sendArgs, sendBuf, sendIdx, sendLen);
                    break;

                case 0x271D:    // 撤销投注 请求/响应
                    SetBetCancel(type, desk);
                    _server.Send(sendArgs, recvBuf, recvIdx, recvLen);
                    break;

                case 0x2726:    // 投注总额 通知
                    if (usrInfo.ctrl == 0)
                    {
                        SendBack(aRecvArgs, sendArgs);
                        break;
                    }
                    SetBetTotal(1, desk);
                    sendLen = pkt.betTotal.len;
                    Packet.StructToBytes(pkt.betTotal, sendBuf, sendIdx, sendLen);
                    _server.Send(sendArgs, sendBuf, sendIdx, sendLen);
                    break;

                case 0x8000:    // 注单查询 请求/响应
                    if (usrInfo.ctrl == 0)
                    {
                        SendBack(aRecvArgs, sendArgs);
                        break;
                    }
                    pkt.betQryReq = Packet.BytesToStruct<stBetQryReq>(recvBuf, recvIdx);
                    pkt.betQryRsp.pageNo = pkt.betQryReq.pageNo;
                    SetBetQryRsp(sendBuf, sendIdx);

                    //int start = pkt.betQryRsp.pageNo * 20;
                    //int end = start + 20;
                    //if(end > usrInfo.betRecordList.Count)
                    //{
                    //    end = usrInfo.betRecordList.Count;
                    //}
                    //int cnt = end - start;

                    //sendLen = 0;
                    //pkt.betQryRsp.len = 12 + cnt * 60;
                    //Packet.StructToBytes(pkt.betQryRsp, sendBuf, sendIdx + sendLen, 12);
                    //sendLen += 12;

                    //for (int i = 0; i < cnt; i++)
                    //{
                    //    var rec = usrInfo.betRecordList[i + start];
                    //    Packet.StructToBytes(rec, sendBuf, sendIdx + sendLen, 60);
                    //    sendLen += 60;
                    //}
                    sendLen = pkt.betQryRsp.len;
                    _server.Send(sendArgs, sendBuf, sendIdx, sendLen);
                    break;
            }

            if (!sendArgs.IsReuse) userToken.FreeArgs(sendArgs);
        }

        #endregion

        #region 视频存取
        bool isReplace = false;

        private void VideoHandleTask()
        {
            BufferItem bufItem;
            BetRecordState curr;
            
            while (Thread.CurrentThread.IsAlive)
            {
                while (_videoQueue.Count > 0 && _videoQueue.TryDequeue(out bufItem))
                {
                    if (usrInfo.desk == 0) continue;

                    curr = usrInfo.betStates[usrInfo.desk - 1];

                    if (!isReplace)
                    {
                        if (curr.operateFlag == 0)
                        {
                            _server.Send(videoToken.ConnectSocket, bufItem.Buffer, bufItem.Offset, bufItem.DataLen, 2000);
                        }
                        else
                        {
                            //if (_videoReplaceQueue.TryDequeue(out BufferItem bufItem2))
                            //{
                            //    _server.Send(videoToken.ConnectSocket, bufItem2.Buffer, bufItem2.Offset, bufItem2.DataLen, 2000);
                            //    _bufferMgr.FreeBuffer(bufItem2);
                            //}

                            _server.Send(videoToken.ConnectSocket, bufItem.Buffer, bufItem.Offset, bufItem.DataLen, 2000);
                        }
                    }

                    //_bufferMgr.FreeBuffer(bufItem);
                    _videoSaveQueue.Enqueue(bufItem);
                }

                Thread.Sleep(5);
            }
        }
        private void VideoSaveTask()
        {
            BufferItem bufItem;
            int dataLen;
            BetRecordState curr;
            string fileName;
            string filePath = "";
            DateTime date = DateTime.Now.Date.AddDays(-1);
            
            while (Thread.CurrentThread.IsAlive)
            {
                while (_videoSaveQueue.Count > 0 && _videoSaveQueue.TryDequeue(out bufItem))
                {
                    if (usrInfo.desk > 0)
                    {
                        dataLen = bufItem.DataLen - 8;
                        curr = usrInfo.betStates[usrInfo.desk - 1];
                        
                        if (curr.state != 1)
                        {
                            if (DateTime.Now.Date > date)
                            {
                                date = DateTime.Now.Date;
                                filePath = _videoBaseDir + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                                Directory.CreateDirectory(filePath);
                            }
                            fileName = filePath + "\\" + curr.desk + "-" + curr.round + "-" + curr.roundTime + ".avi";

                            SaveVideo(fileName, bufItem.Buffer, bufItem.Offset + 8, dataLen);
                        }
                    }

                    _bufferMgr.FreeBuffer(bufItem);
                }

                Thread.Sleep(5);
            }
        }
        private void SaveVideo(string path, byte[] buf, int index, int len)
        {
            string indexFile = path + ".cfg";
            var fsIdx = new FileStream(indexFile, FileMode.Append, FileAccess.Write);
            var fs = new FileStream(path, FileMode.Append, FileAccess.Write);
            
            fs.Write(buf, index, len);
            fs.Close();
            fsIdx.Write(BitConverter.GetBytes(len), 0, sizeof(int));
            fsIdx.Close();

        }

        private void LoadVideo(string path)
        {
            string indexFile = path + ".cfg";
            var fsIdx = new FileStream(indexFile, FileMode.Open, FileAccess.Read);
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            byte[] bts = new byte[sizeof(int)];
            int len;
            int cnt = 0;
            int big = 0;
            int small = 0;
            byte[] tmpBuf;

            fsIdx.Seek(0, SeekOrigin.Begin);
            fs.Seek(0, SeekOrigin.Begin);

            while (fsIdx.Read(bts, 0, bts.Length) > 0)
            {
                len = BitConverter.ToInt32(bts, 0);
                var bufItem = new BufferItem();
                _bufferMgr.SetBuffer(bufItem, len + 20);

                tmpBuf = BitConverter.GetBytes(0xAB03);
                Array.Copy(tmpBuf, 0, bufItem.Buffer, bufItem.Offset, 4);
                bufItem.DataLen = len + 8;
                tmpBuf = BitConverter.GetBytes(bufItem.DataLen);
                Array.Copy(tmpBuf, 0, bufItem.Buffer, bufItem.Offset + 4, 4);

                fs.Read(bufItem.Buffer, bufItem.Offset + 8, len);

                _videoReplaceQueue.Enqueue(bufItem);

                if (len < 8192)
                    small++;
                else
                    big++;
                cnt++;
            }
            fsIdx.Close();
            fs.Close();

            ShowMsg("载入完成！总包数：" + cnt + "  大包：" + big + "  小包：" + small);
        }

        private void LoadImage(string dir)
        {
            string[] files = Directory.GetFiles(dir, "*.jpg");

            int len;
            int cnt = 0;
            int big = 0;
            int small = 1024*1024;
            byte[] tmpBuf;


            while (cnt < files.Length)
            {
                var fs = new FileStream(files[cnt], FileMode.Open, FileAccess.Read);
                len = (int)fs.Length;

                var bufItem = new BufferItem();
                _bufferMgr.SetBuffer(bufItem, len + 20);

                tmpBuf = BitConverter.GetBytes(0xAB03);
                Array.Copy(tmpBuf, 0, bufItem.Buffer, bufItem.Offset, 4);
                bufItem.DataLen = len + 8;
                tmpBuf = BitConverter.GetBytes(bufItem.DataLen);
                Array.Copy(tmpBuf, 0, bufItem.Buffer, bufItem.Offset + 4, 4);

                fs.Read(bufItem.Buffer, bufItem.Offset + 8, len);
                fs.Close();

                _videoReplaceQueue.Enqueue(bufItem);

                if (len < small) small = len;
                if (len > big) big = len;

                cnt++;
            }

            ShowMsg("载入完成！图片总数：" + cnt + "  最大：" + big / 1024 + " k" + "  最小：" + small / 1024 + " k");
        }
        #endregion

        #region 数据操作：计算、存取

        private void SetCurrentDesk(int deskOld, int desk)
        {
            usrInfo.desk = desk;

            Invoke(new Action<string>(s => txtDesk.Text = s), desk == 0 ? "大厅" : desk.ToString());

            if (deskOld == 0) return;

            var curr = usrInfo.betStates[deskOld - 1];

            if (curr.resumeFlag != 0)
            {
                var rstList = usrInfo.openResultList.FindAll(q => q.desk == curr.desk && q.round == curr.round);

                // 恢复该台桌 路单
                for (int i = 0; i < rstList.Count; i++)
                {
                    var rst = rstList[i];
                    rst.openRstLd = rst.openRstGw;
                }

                // 恢复该台桌 开奖结果 todo 若开奖结果恢复，投注记录的结算金额对不上
                //for (int i = 0; i < rstList.Count; i++)
                //{
                //    var rst = rstList[i];
                //    rst.openRst = rst.openRstGw;
                //}
                // 恢复该台桌 投注记录 todo 无法恢复   
            }
        }

        private void SetDeskState(int type, int desk)
        {
            if (desk == 0) return;

            BetRecordState curr;
            if (type == 1)
            {
                curr = usrInfo.betStates[desk - 1];
            }
            else
            {
                curr = usrInfo.betStatesGw[desk - 1];
            }

            curr.state = pkt.gameState.state;
            curr.round = pkt.gameState.round;
            curr.roundTime = pkt.gameState.roundTime;

            if (curr.state == 1)        // 游戏开始
            {
                curr.betTotal = 0;
                curr.winTotal = 0;
                curr.openRst = 0;

                curr.xianHu = 0;
                curr.he = 0;
                curr.zhuangLong = 0;
                curr.xianDui = 0;
                curr.zhuangDui = 0;
            }
            else
            {
                if(usrInfo.autoOperate > 0)
                {
                    FindOldResult(usrInfo.desk, usrInfo.autoOperate, out int round, out int roundTime, out int result);
                    if (result == 0)
                    {
                        return;
                    }
                    curr.operateFlag = usrInfo.autoOperate;
                    curr.roundOld = round;
                    curr.roundTimeOld = roundTime;
                    curr.openRstOpr = (byte)result;

                    new Thread(() => 
                    {
                        var currBet = usrInfo.betStates[usrInfo.desk - 1];
                        if (currBet.state != 1)
                        {
                            string fileName = _videoBaseDir + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\"
                                + currBet.desk + "-" + currBet.round + "-" + currBet.roundTime + ".avi";
                            LoadVideo(fileName);
                        }

                    }) { IsBackground = true }.Start();
                }
            }
        }

        private void SetCmSet(int cmType)
        {
            pkt.cmSet.cmType = cmType;

            if (pkt.cmSet.cmType == 0x0B60)
            {
                pkt.cmSet.min = int.Parse(txtZxMin.Text);
                pkt.cmSet.max = int.Parse(txtZxMax.Text);
                pkt.cmSet.heMin = int.Parse(txtZxHeMin.Text);
                pkt.cmSet.heMax = int.Parse(txtZxHeMax.Text);
                pkt.cmSet.dzMin = int.Parse(txtZxDzMin.Text);
                pkt.cmSet.dzMax = int.Parse(txtZxDzMax.Text);
            }
            else
            {
                pkt.cmSet.min = int.Parse(txtLhMin.Text);
                pkt.cmSet.max = int.Parse(txtLhMax.Text);
                pkt.cmSet.heMin = int.Parse(txtLhHeMin.Text);
                pkt.cmSet.heMax = int.Parse(txtLhHeMax.Text);
                pkt.cmSet.dzMin = 0;
                pkt.cmSet.dzMax = 0;
            }
        }

        private void SetOdds(stGameOddsNtf odds)
        {
            if (usrInfo.BjlXian != 0) return;

            usrInfo.BjlXian = odds.xianHu;
            usrInfo.BjlHe = odds.he;
            usrInfo.BjlZhuang = odds.zhuangLong;
            usrInfo.BjlXianDui = odds.xianDui;
            usrInfo.BjlZhuangDui = odds.zhuangDui;

            usrInfo.LhHu = 0.95f;       // odds.xianHu;
            usrInfo.LhHe = odds.he;
            usrInfo.LhLong = 0.95f;     // odds.zhuangLong;
        }

        private void SetBetInfo(int type, int desk)
        {
            BetRecordState curr;
            List<stBetRecord> recList;
            int total = GetBetSum(pkt.betInfo);

            if (type == 1)
            {
                curr = usrInfo.betStates[desk - 1];
                recList = usrInfo.betRecordList;
                usrInfo.remain -= total;
            }
            else
            {
                curr = usrInfo.betStatesGw[desk - 1];
                recList = usrInfo.betRecordListGw;
                usrInfo.remainBind -= total;
            }

            bool isNewRecord = curr.betTotal == 0;

            curr.betTotal += total;
            curr.xianHu += pkt.betInfo.xianHu;
            curr.he += pkt.betInfo.he;
            curr.zhuangLong += pkt.betInfo.zhuangLong;
            curr.xianDui += pkt.betInfo.xianDui;
            curr.zhuangDui += pkt.betInfo.zhuangDui;

            curr.betInfo = Packet.StructClone(pkt.betInfo);
            if (type == 0) pkt.betInfo = usrInfo.betStates[desk - 1].betInfo;

            // todo 注单时间：考虑是否用UTC时间 还是当地时间 ?
            curr.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            if (isNewRecord)
            {
                var time = DateTime.Now;
                curr.date = time.Hour < 12 && curr.round != 1 ? time.AddDays(-1).ToString("yyyy-MM-dd") : time.ToString("yyyy-MM-dd");

                var record = GenBetRecord(curr);
                recList.Add(record);
            }
            else
            {
                var idx = recList.FindIndex(q => q.desk == curr.desk && q.round == curr.round && q.roundTime == curr.roundTime);
                if (idx >= 0)
                {
                    var rec = recList[idx];
                    rec.time = curr.time;
                    rec.xianHu = curr.xianHu;
                    rec.he = curr.he;
                    rec.zhuangLong = curr.zhuangLong;
                    rec.xianDui = curr.xianDui;
                    rec.zhuangDui = curr.zhuangDui;
                    recList[idx] = rec;
                }
            }

        }

        private void SetBetTotal(int type, int desk)
        {
            var curr = usrInfo.betStates[desk - 1];
            
            pkt.betTotal.desk = desk;
            pkt.betTotal.xianHu += curr.xianHu;
            pkt.betTotal.he += curr.he;
            pkt.betTotal.zhuangLong += curr.zhuangLong;
            pkt.betTotal.xianDui += curr.xianDui;
            pkt.betTotal.zhuangDui += curr.zhuangDui;
        }

        private stBetRecord GenBetRecord(BetRecordState currBet)
        {
            stBetRecord rec = new stBetRecord();

            rec.time = currBet.time;
            rec.desk = currBet.desk;
            rec.round = currBet.round;
            rec.roundTime = currBet.roundTime;
            rec.xianHu = currBet.xianHu;
            rec.he = currBet.he;
            rec.zhuangLong = currBet.zhuangLong;
            rec.xianDui = currBet.xianDui;
            rec.zhuangDui = currBet.zhuangDui;
            rec.openRst = 0;
            rec.profit = 0;

            return rec;
        }

        private void SetBetCancel(int type, int desk)
        {
            BetRecordState curr;

            if (type == 1)
            {
                curr = usrInfo.betStates[desk - 1];
                usrInfo.remain += curr.betTotal;
                usrInfo.betRecordList.RemoveAll(q => q.desk == desk && q.round == curr.round && q.roundTime == curr.roundTime);
            }
            else
            {
                curr = usrInfo.betStatesGw[desk - 1];
                usrInfo.remainBind += curr.betTotal;
                usrInfo.betRecordListGw.RemoveAll(q => q.desk == desk && q.round == curr.round && q.roundTime == curr.roundTime);
            }

            curr.betTotal = 0;
            curr.xianHu = 0;
            curr.he = 0;
            curr.zhuangLong = 0;
            curr.xianDui = 0;
            curr.zhuangDui = 0;
        }

        private void SetOpenList(stOpenListRsp curr)
        {
            if (curr.roundTime == 0) return;

            var openList = usrInfo.openResultList.FindAll(q => q.desk == curr.desk && q.round == curr.round);

            if(openList.Count < curr.roundTime)
            {
                for (int i = openList.Count; i < curr.roundTime; i++)
                {
                    var openRst = new OpenResult();

                    openRst.desk = pkt.openListRsp.desk;
                    openRst.round = pkt.openListRsp.round;
                    openRst.roundTime = pkt.openListRsp.roundTime;

                    openRst.openRst = pkt.openListRsp.list[i];
                    openRst.openRstGw = openRst.openRst;
                    openRst.openRstLd = openRst.openRst;

                    usrInfo.openResultList.Add(openRst);
                }
            }


            for(int i = 0; i < openList.Count; i++)
            {
                pkt.openListRsp.list[i] = openList[i].openRstLd;
            }
        }

        private void SetOpenResult(int type, int desk, int result)
        {
            BetRecordState curr;
            List<stBetRecord> recList;

            if (type == 1)
            {
                curr = usrInfo.betStates[desk - 1];
                recList = usrInfo.betRecordList;

                if (curr.operateFlag > 0)
                {
                    curr.openRst = curr.openRstOpr;
                    curr.operateFlag = 0;
                }
                else
                {
                    curr.openRst = result;
                }
            }
            else
            {
                curr = usrInfo.betStatesGw[desk - 1];
                recList = usrInfo.betRecordListGw;
                curr.openRst = result;
            }

            curr.openRstGw = (byte)result;
            curr.winTotal = GetWinScore(curr);
            curr.profitTotal = curr.winTotal - curr.betTotal;

            var idx = recList.FindIndex(q => q.desk == curr.desk && q.round == curr.round && q.roundTime == curr.roundTime);
            if (idx >= 0)
            {
                var rec = recList[idx];
                rec.openRst = curr.openRst;
                rec.profit = curr.profitTotal;
                recList[idx] = rec;
            }

            if (type == 0)
            {
                usrInfo.remainBind += curr.winTotal;
                usrInfo.profitBind += curr.profitTotal;
            }
            else
            {
                usrInfo.remain += curr.winTotal;
                usrInfo.profit += curr.profitTotal;

                pkt.openResult.cmd = 0x271A;
                pkt.openResult.len = 36;
                pkt.openResult.desk = curr.desk;
                pkt.openResult.round = curr.round;
                pkt.openResult.roundTime = curr.roundTime;
                pkt.openResult.openRst = curr.openRst;
                pkt.openResult.remain = usrInfo.remain;
                pkt.openResult.profit = usrInfo.profit;
                pkt.openResult.win = curr.winTotal;

                var openRerult = new OpenResult();
                openRerult.date = curr.date;
                openRerult.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                openRerult.desk = curr.desk;
                openRerult.round = curr.round;
                openRerult.roundTime = curr.roundTime;
                openRerult.openRst = (byte)curr.openRst;
                openRerult.openRstGw = curr.openRstGw;
                openRerult.openRstLd = openRerult.openRst;

                usrInfo.openResultList.Add(openRerult);
            }
        }

        private void SetDevInfo(int ip, byte[] mac)
        {
            usrInfo.ip = ip;
            mac.CopyTo(usrInfo.mac, 0);

            byte[] ipbs = BitConverter.GetBytes(ip);
            string ipstr = string.Format("{0}.{1}.{2}.{3}", 
                ipbs[0], ipbs[1], ipbs[2], ipbs[3]);
            string macstr = string.Format("{0:X2}-{1:X2}-{2:X2}-{3:X2}-{4:X2}-{5:X2}",
                mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]);

            UpdateDevInfo(ipstr, macstr);
        }

        private void SetBetQryRsp(byte[] sendBuf, int sendIdx)
        {
            int sendLen = 0;

            int start = pkt.betQryRsp.pageNo * 20;
            int end = start + 20;
            if (end > usrInfo.betRecordList.Count)
            {
                end = usrInfo.betRecordList.Count;
            }
            int cnt = end - start;

            pkt.betQryRsp.len = 12 + cnt * 60;
            Packet.StructToBytes(pkt.betQryRsp, sendBuf, sendIdx + sendLen, 12);
            sendLen += 12;

            for (int i = 0; i < cnt; i++)
            {
                var rec = usrInfo.betRecordList[i + start];
                Packet.StructToBytes(rec, sendBuf, sendIdx + sendLen, 60);
                sendLen += 60;
            }
        }

        private void FindOldResult(int desk, int resultCtrl, out int roundOld, out int roundTimeOld, out int resultOld)
        {
            var curr = usrInfo.betStates[desk - 1];

            List<OpenResult> list = usrInfo.openResultList;
            int idx = list.Count - 5;   // 最近 10 ~5 局内寻找
            int cnt = 5;
            int oldIdx;
            int oldResult1;
            int oldResult2;
            int oldResult3;

            if(idx < 0 || (curr.xianHu == curr.zhuangLong && curr.xianHu > 0))
            {
                roundOld = 0;
                roundTimeOld = 0;
                resultOld = 0;  // 总局数小于5 或 两边下一样多， 不允许操作
                return;
            }

            if (desk > 4)
            {
                
                if (resultCtrl == 1)
                {
                    oldResult1 = curr.xianHu > curr.zhuangLong ? Result.Long : Result.Hu;
                }
                else if (resultCtrl == 2)
                {
                    oldResult1 = curr.xianHu > curr.zhuangLong ? Result.Hu : Result.Long;
                }
                else
                {
                    oldResult1 = Result.LhHe;
                }
                oldIdx = list.FindIndex(idx, cnt, q => q.openRstGw == oldResult1);
            }
            else
            {
                if (resultCtrl == 1)
                {
                    if (curr.xianHu > curr.zhuangLong)
                    {
                        oldResult1 = Result.Zhuang;
                        oldResult2 = Result.ZhuangXd;
                        oldResult3 = Result.ZhuangZd;
                    }
                    else
                    {
                        oldResult1 = Result.Xian;
                        oldResult2 = Result.XianXd;
                        oldResult3 = Result.XianZd;
                    }
                }
                else if (resultCtrl == 2)
                {
                    if (curr.xianHu > curr.zhuangLong)
                    {
                        oldResult1 = Result.Xian;
                        oldResult2 = Result.XianXd;
                        oldResult3 = Result.XianZd;
                    }
                    else
                    {
                        oldResult1 = Result.Zhuang;
                        oldResult2 = Result.ZhuangXd;
                        oldResult3 = Result.ZhuangZd;
                    }
                }
                else
                {
                    oldResult1 = Result.ZxHe;
                    oldResult2 = Result.ZxHeXd;
                    oldResult3 = Result.ZxHeZd;
                }

                if ((oldIdx = list.FindIndex(idx, cnt, q => q.openRstGw == oldResult1)) < 0)
                    if ((oldIdx = list.FindIndex(idx, cnt, q => q.openRstGw == oldResult2)) < 0)
                        oldIdx = list.FindIndex(idx, cnt, q => q.openRstGw == oldResult3);
            }

            if (oldIdx >= 0)
            {
                roundOld = list[oldIdx].round;
                roundTimeOld = list[oldIdx].roundTime;
                resultOld = list[oldIdx].openRstGw;
            }
            else
            {
                roundOld = 0;
                roundTimeOld = 0;
                resultOld = 0;      // 倒数 5 ~ 10 局内没牌
            }

        }

        private int GetBetSum(stBetInfo bet)
        {
            int sum = bet.xianHu + bet.he + bet.zhuangLong + bet.xianDui + bet.zhuangDui;
            return sum;
        }

        private int GetBetSum(stBetRecord bet)
        {
            int sum = bet.xianHu + bet.he + bet.zhuangLong + bet.xianDui + bet.zhuangDui;
            return sum;
        }

        private string GetResultStr(int result)
        {
            string str = "";

            switch (result)
            {
                case 1: str = "闲"; break;
                case 2: str = "闲赢闲对"; break;
                case 3: str = "闲赢庄对"; break;
                case 4: str = "闲赢闲对庄对"; break;
                case 5: str = "和"; break;
                case 6: str = "和赢闲对"; break;
                case 7: str = "和赢庄对"; break;
                case 8: str = "和赢闲对庄对"; break;
                case 9: str = "庄"; break;
                case 10: str = "庄赢闲对"; break;
                case 11: str = "庄赢庄对"; break;
                case 12: str = "庄赢闲对庄对"; break;
                case 13: str = "虎"; break;
                case 14: str = "和"; break;
                case 15: str = "龍"; break;
                case 0:
                default: str = "未开"; break;
            }

            return str;
        }

        private int GetWinScore(stBetRecord rec)
        {
            int score = 0;

            if(rec.desk > 4)
            {
                if (rec.openRst == Result.Hu) score += (int)(rec.xianHu * usrInfo.LhHu);
                if (rec.openRst == Result.LhHe) score += (int)(rec.he * usrInfo.LhHe) + rec.xianHu + rec.zhuangLong;
                if (rec.openRst == Result.Long) score += (int)(rec.zhuangLong * usrInfo.LhLong);
            }
            else
            {
                if (rec.openRst >= Result.Xian && rec.openRst <= Result.XianXdZd) score += (int)(rec.xianHu * usrInfo.BjlXian);
                if (rec.openRst >= Result.ZxHe && rec.openRst <= Result.ZxHeXdZd) 
                    score += (int)(rec.he * usrInfo.BjlHe) + rec.xianHu + rec.zhuangLong + rec.xianDui + rec.zhuangDui;
                if (rec.openRst >= Result.Zhuang && rec.openRst <= Result.ZhuangXdZd) score += (int)(rec.zhuangLong * usrInfo.BjlZhuang);

                switch (rec.openRst)
                {
                    case Result.XianXd:
                    case Result.ZxHeXd:
                    case Result.ZhuangXd:
                        score += (int)(rec.xianDui * usrInfo.BjlXianDui);
                        break;
                    case Result.XianZd:
                    case Result.ZxHeZd:
                    case Result.ZhuangZd:
                        score += (int)(rec.zhuangDui * usrInfo.BjlZhuangDui);
                        break;
                    case Result.XianXdZd:
                    case Result.ZxHeXdZd:
                    case Result.ZhuangXdZd:
                        score += (int)(rec.xianDui * usrInfo.BjlXianDui);
                        score += (int)(rec.zhuangDui * usrInfo.BjlZhuangDui);
                        break;
                }
            }

            return score;
        }
        private int GetWinScore(BetRecordState rec)
        {
            int score = 0;

            if (rec.desk > 4)
            {
                if (rec.openRst == Result.Hu) score += (int)(rec.xianHu * usrInfo.LhHu);
                if (rec.openRst == Result.LhHe) score += (int)(rec.he * usrInfo.LhHe) + rec.xianHu + rec.zhuangLong;
                if (rec.openRst == Result.Long) score += (int)(rec.zhuangLong * usrInfo.LhLong);
            }
            else
            {
                if (rec.openRst >= Result.Xian && rec.openRst <= Result.XianXdZd) score += (int)(rec.xianHu * usrInfo.BjlXian);
                if (rec.openRst >= Result.ZxHe && rec.openRst <= Result.ZxHeXdZd)
                    score += (int)(rec.he * usrInfo.BjlHe) + rec.xianHu + rec.zhuangLong + rec.xianDui + rec.zhuangDui;
                if (rec.openRst >= Result.Zhuang && rec.openRst <= Result.ZhuangXdZd) score += (int)(rec.zhuangLong * usrInfo.BjlZhuang);

                switch (rec.openRst)
                {
                    case Result.XianXd:
                    case Result.ZxHeXd:
                    case Result.ZhuangXd:
                        score += (int)(rec.xianDui * usrInfo.BjlXianDui);
                        break;
                    case Result.XianZd:
                    case Result.ZxHeZd:
                    case Result.ZhuangZd:
                        score += (int)(rec.zhuangDui * usrInfo.BjlZhuangDui);
                        break;
                    case Result.XianXdZd:
                    case Result.ZxHeXdZd:
                    case Result.ZhuangXdZd:
                        score += (int)(rec.xianDui * usrInfo.BjlXianDui);
                        score += (int)(rec.zhuangDui * usrInfo.BjlZhuangDui);
                        break;
                }
            }

            return score;
        }

        private string GetUserStr(int type)
        {
            BetRecordState curr;
            StringBuilder sb = new StringBuilder();

            if (type == 1)
            {
                curr = usrInfo.betStates[usrInfo.desk - 1];
                sb.Append("用户:" + usrInfo.user + ", ");
                sb.Append("余额:" + usrInfo.remain + ", ");
                sb.Append("输赢:" + usrInfo.profit + "   ");
            }
            else
            {
                curr = usrInfo.betStatesGw[usrInfo.desk - 1];
                sb.Append("用户:" + usrInfo.userBind + ", ");
                sb.Append("余额:" + usrInfo.remainBind + ", ");
                sb.Append("输赢:" + usrInfo.profitBind + "   ");
            }

            sb.Append("桌号:" + curr.desk + ", ");
            sb.Append("场次:" + curr.round + " - ");
            sb.Append(curr.roundTime + " 【 ");

            if(curr.xianHu > 0)
                sb.Append((usrInfo.desk < 5 ? "闲:" : "虎:") + curr.xianHu + " ");
            if (curr.he > 0)
                sb.Append("和:" + curr.he + " ");
            if (curr.zhuangLong > 0)
                sb.Append((usrInfo.desk < 5 ? "庄:" : "龙:") + curr.zhuangLong + " ");
            if (curr.xianDui > 0)
                sb.Append("闲对:" + curr.xianDui + " ");
            if (curr.zhuangDui > 0)
                sb.Append("庄对:" + curr.zhuangDui + " ");

            sb.Append("】开答:" + GetResultStr(curr.openRst) + ", ");
            sb.Append("输赢:" + curr.profitTotal + " ");

            return sb.ToString();
        }

        private string GetBetInfoStr(int type)
        {
            BetRecordState record;
            StringBuilder sb = new StringBuilder();

            if (type == 1)
            {
                record = usrInfo.betStates[usrInfo.desk - 1];
            }
            else
            {
                record = usrInfo.betStatesGw[usrInfo.desk - 1];
            }

            sb.Append(type == 0 ? "官方下注 " : "平台下注 ");
            sb.Append("时间:" + record.time + ", ");
            sb.Append("桌号:" + record.desk + ", ");
            sb.Append("靴次:" + record.round + ", ");
            sb.Append("口次:" + record.roundTime + ", ");
            sb.Append((usrInfo.desk < 5 ? "闲:" : "虎:") + record.betInfo.xianHu + ", ");
            sb.Append("和:" + record.betInfo.he + ", ");
            sb.Append((usrInfo.desk < 5 ? "庄:" : "龙:") + record.betInfo.zhuangLong + ", ");
            sb.Append("闲对:" + record.betInfo.xianDui + ", ");
            sb.Append("庄对:" + record.betInfo.zhuangDui + " ");

            return sb.ToString();
        }

        private string GetRecordListStr(int type)
        {
            List<stBetRecord> recList;
            StringBuilder sb = new StringBuilder();

            if (type == 1)
            {
                recList = usrInfo.betRecordList;
            }
            else
            {
                recList = usrInfo.betRecordListGw;
            }

            sb.AppendLine(type == 0 ? "官方的投注历史 " : "我们的投注历史 ");

            foreach (stBetRecord record in recList)
            {
                sb.Append("时间:" + record.time + ", ");
                sb.Append("桌号:" + record.desk + ", ");
                sb.Append("靴次:" + record.round + ", ");
                sb.Append("口次:" + record.roundTime + ", ");
                sb.Append("闲/虎:" + record.xianHu + ", ");
                sb.Append("和:" + record.he + ", ");
                sb.Append("庄/龙:" + record.zhuangLong + ", ");
                sb.Append("闲对:" + record.xianDui + ", ");
                sb.Append("庄对:" + record.zhuangDui + ", ");
                sb.Append("开答:" + record.openRst + ", ");
                sb.Append("输赢:" + record.profit + " \n");
            }

            return sb.ToString();
        }

        delegate void UpdateStrCallback(string str);
        private void UpdateOriginData(string str)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateStrCallback(UpdateOriginData), str);
                return;
            }

            txtOrigin.Text = str;
        }
        private void UpdateOurData(string str)
        {
            if (this.InvokeRequired)
            {
                Invoke(new UpdateStrCallback(UpdateOurData), str);
                return;
            }

            txtOur.Text = str;
        }
        private void UpdateDevInfo(string ip, string mac)
        {
            Invoke(new UpdateStrCallback(a => { lbIp.Text = "IP: " + a; }), ip);
            Invoke(new UpdateStrCallback(a => { lbMac.Text = "MAC: " + a; }), mac);
        }
        #endregion


        #region 界面事件

        private void chkCtrl_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCtrl.Checked)
            {
                chkCollect.Checked = false;
                usrInfo.ctrl = 1;
            }
            else
            {
                usrInfo.ctrl = 0;
            }

            if (commToken != null) commToken.ConnectSocket.Close();
            if (videoToken != null) videoToken.ConnectSocket.Close();
        }
        private void chkCollect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCollect.Checked)
            {
                chkCtrl.Checked = false;
                usrInfo.ctrl = 2;
            }
            else
            {
                usrInfo.ctrl = 0;
            }
            if (commToken != null) commToken.ConnectSocket.Close();
            if (videoToken != null) videoToken.ConnectSocket.Close();
        }

        private void chkAutoWin_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoWin.Checked)
            {
                usrInfo.autoOperate = 2;
                chkAutoLose.Checked = false;

                btnWin.Text = "撤赢";
                btnWin.BackColor = Color.LightGray;
                btnLose.Text = "输";
                btnLose.BackColor = Color.LightGreen;
                btnWin.Enabled = false;
                btnLose.Enabled = false;
            }
            else
            {
                usrInfo.autoOperate = 0;

                btnWin.Text = "赢";
                btnWin.BackColor = Color.LightCoral;
                btnWin.Enabled = true;
                btnLose.Enabled = true;
            }
        }

        private void chkAutoLose_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoLose.Checked)
            {
                usrInfo.autoOperate = 1;
                chkAutoWin.Checked = false;

                btnWin.Text = "赢";
                btnWin.BackColor = Color.LightCoral;
                btnLose.Text = "撤输";
                btnLose.BackColor = Color.LightGray;
                btnWin.Enabled = false;
                btnLose.Enabled = false;
            }
            else
            {
                usrInfo.autoOperate = 0;

                btnLose.Text = "输";
                btnLose.BackColor = Color.LightGreen;
                btnWin.Enabled = true;
                btnLose.Enabled = true;
            }
        }

        private void btnWin_Click(object sender, EventArgs e)
        {
            if(usrInfo.desk == 0 || btnLose.Text == "撤输") return;

            var curr = usrInfo.betStates[usrInfo.desk];

            if (btnWin.Text == "赢")
            {
                FindOldResult(usrInfo.desk, 2, out int round, out int roundTime, out int result);
                if(result == 0)
                {
                    MessageBox.Show("没牌 ！");
                    return;
                }
                curr.operateFlag = 2;
                curr.roundOld = round;
                curr.roundTimeOld = roundTime;
                curr.openRstOpr = (byte)result;

                btnWin.Text = "撤赢";
                btnWin.BackColor = Color.LightGray;
            }
            else
            {
                curr.operateFlag = 0;
                btnWin.Text = "赢";
                btnWin.BackColor = Color.LightCoral;
            }
        }

        private void btnLose_Click(object sender, EventArgs e)
        {
            if (usrInfo.desk == 0 || btnWin.Text == "撤赢") return;

            var curr = usrInfo.betStates[usrInfo.desk];

            if (btnWin.Text == "输")
            {
                FindOldResult(usrInfo.desk, 1, out int round, out int roundTime, out int result);
                if (result == 0)
                {
                    MessageBox.Show("没牌 ！");
                    return;
                }
                curr.operateFlag = 1;
                curr.roundOld = round;
                curr.roundTimeOld = roundTime;
                curr.openRstOpr = (byte)result;

                btnWin.Text = "撤输";
                btnWin.BackColor = Color.LightGray;
            }
            else
            {
                curr.operateFlag = 0;
                btnWin.Text = "输";
                btnWin.BackColor = Color.LightGreen;
            }
        }

        int fileIdx = 0;

        private void btnSwitchVideo_Click(object sender, EventArgs e)
        {
            //string filePath = _videoBaseDir + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
            //var files = Directory.GetFiles(filePath, "*.avi");

            //if (files.Length == 0) {
            //    ShowMsg("没有保存的视频！");
            //    return;
            //};
            //if (fileIdx > files.Length) fileIdx = 0;

            //ShowMsg(string.Format("切换视频 [{0}] -- {1}", fileIdx, files[fileIdx]));
            //LoadVideo(files[fileIdx]);

            string dir = "C:\\Users\\wushu\\Desktop\\视频数据";
            ShowMsg(string.Format("切换图片流 [{0}] ", dir));
            LoadImage(dir);

            new Thread(() => 
            {
                BufferItem bufItem;

                isReplace = true;

                while (_videoReplaceQueue.Count > 0 && _videoReplaceQueue.TryDequeue(out bufItem))
                {
                    if (videoToken != null)
                        _server.Send(videoToken.ConnectSocket, bufItem.Buffer, bufItem.Offset, bufItem.DataLen, 2000);

                    _bufferMgr.FreeBuffer(bufItem);

                    Thread.Sleep(2);
                }

                isReplace = false;

            }).Start();

        }

        private void btnQryRecordOur_Click(object sender, EventArgs e)
        {
            string str = GetRecordListStr(0);
            _logOutQueue.Enqueue(new LogOutEventArgs(0, str));

            str = GetRecordListStr(1);
            _logOutQueue.Enqueue(new LogOutEventArgs(0, str));
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            usrInfo.betStates[usrInfo.desk].resumeFlag = 1;
        }

        private void btnFoceOffline_Click(object sender, EventArgs e)
        {
            if (commToken != null) commToken.ConnectSocket.Close();
            if (videoToken != null) videoToken.ConnectSocket.Close();

        }

        #endregion

       
    }
}
