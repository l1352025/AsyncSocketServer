using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncSocketServer
{
    /// <summary>  
    /// 异步SOCKET完成端口服务器  
    /// </summary>  
    public class AsyncSocketServer : IDisposable
    {
        const int opsToPreAlloc = 4;

        #region Fields
        private int _maxClient;                             // 服务器程序允许的最大客户端连接数
        private Socket _serverSock;                         // 监听Socket，用于接受客户端的连接请求
        private int _bufferSize = 2048;                     // 每个Socket操作的缓冲区大小
        private Semaphore _maxAcceptClientSem;              // 最大连接数信号量
        private Semaphore _stopWaitSem;                     // 服务器停止等待信号量, 与接受连接互斥
        private BufferManager _bufferManager;               // 缓冲区管理
        private AsyncSocketUserTokenPool _userTokenPool;    // 用户池 
        private AsyncEventArgsPool _eventArgsPool;

        private int _clientCount;                           // 当前的连接的客户端数
        private AsyncSocketUserTokenList _clientList;       // 当前的连接的客户端列表
        private int _totalBytesRead;                        // 当前接收的字节总数 

        private DaemonThread _daemonThread;                 // 守护进程

        private bool disposed = false;

        private Stack<LogOutEventArgs> _logOutEvtArgsPool;  // 日志输出事件参数池

        #endregion

        #region Properties

        /// <summary>  
        /// 服务器是否正在运行  
        /// </summary>  
        public bool IsRunning   { get; private set; }           // 服务器是否正在运行  
  
        public IPAddress Address    { get; private set; }       // 监听的IP地址  
        public int Port     { get; private set; }               // 监听的端口       
        public IPEndPoint LocalEndPoint { get; private set; }   // 服务器本地端点[ip : port]
        public Encoding Encoding    { get; private set; }       // 通信使用的编码     
        public int SocketTimeOutMS  { get; private set; }       // socket 最大超时时间，单位ms
        public BufferManager BufferMgr => _bufferManager;
        public int ClientCount => _clientCount;
        public int TotalRecvdBytes
        {
            get { return _totalBytesRead; }
            set { _totalBytesRead = value; }
        }

        public AsyncSocketUserTokenList ConnectedClients        // 获取客户端列表 
        {
            get { return _clientList; }
        }

        #endregion

        #region Constructors

        /// <summary>  
        /// 异步Socket SOCKET服务器  
        /// </summary>  
        /// <param name="listenPort">监听的端口</param>  
        /// <param name="maxClient">最大的客户端数量</param>  
        /// <param name="perBufSize">每个socket收/发缓存大小</param>  
        public AsyncSocketServer(int listenPort, int maxClient, int perBufSize)
            : this(IPAddress.Any, listenPort, maxClient, perBufSize)
        {
        }

        /// <summary>  
        /// 异步Socket TCP服务器  
        /// </summary>  
        /// <param name="localEP">监听的终结点</param>  
        /// <param name="maxClient">最大客户端数量</param>  
        /// <param name="perBufSize">每个socket收/发缓存大小</param>  
        public AsyncSocketServer(IPEndPoint localEP, int maxClient, int perBufSize)
            : this(localEP.Address, localEP.Port, maxClient, perBufSize)
        {
        }

        /// <summary>  
        /// 异步Socket TCP服务器  
        /// </summary>  
        /// <param name="localIPAddress">监听的IP地址</param>  
        /// <param name="listenPort">监听的端口</param>  
        /// <param name="maxClient">最大客户端数量</param>  
        /// <param name="perBufSize">每个socket收/发缓存大小</param>  
        public AsyncSocketServer(IPAddress localIPAddress, int listenPort, int maxClient, int perBufSize)
        {
            Address = localIPAddress;
            Port = listenPort;
            LocalEndPoint = new IPEndPoint(Address, Port);
            Encoding = Encoding.Default;

            _maxClient = maxClient;
            _bufferSize = perBufSize;

            SocketTimeOutMS = 5 * 60 * 1000;

        }

        #endregion

        #region Event
        public event EventHandler<AsyncEventArgs> OnRecieve;
        public event EventHandler<AsyncEventArgs> OnConnectChange;
        #endregion

        #region Initial

        /// <summary>  
        /// 初始化函数  
        /// </summary>  
        public void Init()
        {
            _totalBytesRead = 0;

            _clientCount = 0;

            _bufferManager = new BufferManager(_maxClient * opsToPreAlloc, _bufferSize);

            _userTokenPool = new AsyncSocketUserTokenPool(_maxClient);

            _eventArgsPool = new AsyncEventArgsPool(_maxClient * 2);

            _clientList = new AsyncSocketUserTokenList();

            _maxAcceptClientSem = new Semaphore(_maxClient, _maxClient);

            _stopWaitSem = new Semaphore(1, 1);

            _logOutEvtArgsPool = new Stack<LogOutEventArgs>(_maxClient);

            // preallocate pool of AsyncSocketUserToken and LogOutEventArgs
            AsyncSocketUserToken userToken;
            LogOutEventArgs logArgs;
            AsyncEventArgs eventArgs1;
            AsyncEventArgs eventArgs2;
            AsyncEventArgs eventArgs3;

            for (int i = 0; i < _maxClient; i++)
            {
                //Pre-allocate a set of reusable AsyncSocketUserToken  
                userToken = new AsyncSocketUserToken(_eventArgsPool);
                eventArgs1 = new AsyncEventArgs();
                eventArgs2 = new AsyncEventArgs();
                eventArgs3 = new AsyncEventArgs();
                userToken.RecvEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                eventArgs1.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                eventArgs2.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                eventArgs3.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);

                // assign a byte buffer from the buffer pool to the SocketAsyncEventArg object  
                _bufferManager.SetBuffer(userToken.RecvEventArgs);
                _bufferManager.SetBuffer(eventArgs1);
                _bufferManager.SetBuffer(eventArgs2);
                _bufferManager.SetBuffer(eventArgs3);

                _userTokenPool.Push(userToken);
                _eventArgsPool.Push(eventArgs1);
                _eventArgsPool.Push(eventArgs2);
                _eventArgsPool.Push(eventArgs3);

                // Pre-allocate a set of reusable LogOutEventArgs
                logArgs = new LogOutEventArgs("", 0, 0);
                _logOutEvtArgsPool.Push(logArgs);
            }

        }

        #endregion

        #region Start
        /// <summary>  
        /// 启动服务  
        /// </summary>  
        public void Start()
        {
            if (!IsRunning)
            {
                Init();

                IsRunning = true;

                // 创建监听socket  
                _serverSock = new Socket(LocalEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                if (LocalEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    // 配置监听socket为 dual-mode (IPv4 & IPv6)   
                    // 27 is equivalent to IPV6_V6ONLY socket option in the winsock snippet below,  
                    _serverSock.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
                    _serverSock.Bind(new IPEndPoint(IPAddress.IPv6Any, LocalEndPoint.Port));
                }
                else
                {
                    _serverSock.Bind(LocalEndPoint);
                }
                // 开始监听  
                _serverSock.Listen(this._maxClient);
                // 在监听Socket上投递一个接受请求。  
                StartAccept(null);

                _daemonThread = new DaemonThread(this);

                Log4Debug("服务器已启动....[" + LocalEndPoint.ToString() + "]");
            }
        }
        #endregion

        #region Stop

        /// <summary>  
        /// 停止服务  
        /// </summary>  
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;

                _stopWaitSem.WaitOne();

                _daemonThread.Close();
                //_serverSock.Close();

                //TODO 关闭对所有客户端的连接
                AsyncSocketUserToken[] users = null;
                _clientList.CopyList(ref users);
                foreach (AsyncSocketUserToken u in users)
                {
                    CloseClientSocket(u.RecvEventArgs);
                }
                users = null;

                _stopWaitSem.Release();

                _serverSock.Close();
                _clientList.Clear();
                _userTokenPool.Clear();

                _serverSock = null;
                _clientList = null;
                _userTokenPool = null;
                _bufferManager = null;
                _maxAcceptClientSem = null;

                Log4Debug("服务器已关闭....");
            }

            GC.Collect();
        }

        #endregion


        #region Accept

        /// <summary>  
        /// 从客户端开始接受一个连接操作  
        /// </summary>  
        private void StartAccept(AsyncEventArgs aAcceptArgs)
        {
            if(!IsRunning)
            {
                return;
            }

            if (aAcceptArgs == null)
            {
                aAcceptArgs = new AsyncEventArgs();
                aAcceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            }
            else
            {
                //socket must be cleared since the context object is being reused  
                aAcceptArgs.AcceptSocket = null;
            }
            _maxAcceptClientSem.WaitOne();
            if (!_serverSock.AcceptAsync(aAcceptArgs))
            {
                ProcessAccept(aAcceptArgs);
            }
        }

        /// <summary>  
        /// accept 操作完成时回调函数  
        /// </summary>  
        /// <param name="sender">Object who raised the event.</param>  
        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>  
        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept((AsyncEventArgs)e);
        }

        /// <summary>  
        /// 监听Socket接受处理  
        /// </summary>  
        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>  
        private void ProcessAccept(AsyncEventArgs e)
        {
            if (!IsRunning)
            {
                return;
            }

            if (e.SocketError == SocketError.Success)
            {
                Socket s = e.AcceptSocket;//和客户端关联的socket  
                if (s.Connected)
                {
                    try
                    {
                        _stopWaitSem.WaitOne();     // 服务器是否正在停止
                        if(!IsRunning)
                        {
                            _stopWaitSem.Release();
                            return;
                        }
                        Interlocked.Increment(ref _clientCount);//原子操作加1  
                        Log4Debug(String.Format("客户端 {0} 连入, 当前 {1} 个连接。", s.RemoteEndPoint.ToString(), _clientCount));

                        AsyncSocketUserToken userToken = _userTokenPool.Pop();
                        userToken.ConnectSocket = s;
                        userToken.RemoteEndPoint = userToken.ConnectSocket.RemoteEndPoint;
                        userToken.ConnectTime = DateTime.Now;
                        ConnectedClients.Add(userToken);

                        userToken.ConnectSocket.SendBufferSize = 1024000;
                        userToken.ConnectSocket.ReceiveBufferSize = 1024000;

                        e.UserToken = userToken;
                        OnConnectChange?.Invoke("cnnt", e);

                        _stopWaitSem.Release();

                        if (userToken.ConnectSocket != null)
                        {
                            if (!userToken.ConnectSocket.ReceiveAsync(userToken.RecvEventArgs))//投递接收请求  
                            {
                                ProcessReceive(userToken.RecvEventArgs);
                            }
                        }
                    }
                    catch (SocketException ex)
                    {
                        Log4Debug(DateTime.Now.ToString("yyyyMMdd hhMMss.fff") + " ProcessAccept Error:" + ex.Message + "\r\n" + ex.StackTrace);
                        Error2File("ProcessAccept Error:" + ex.Message + "\r\n" + ex.StackTrace);
                        //TODO 异常处理  
                    }

                    //投递下一个接受请求
                    StartAccept(e);
                }
            }
        }

        #endregion

        #region I/O Completed Callback

        /// <summary>  
        /// 当Socket上的发送或接收请求被完成时，调用此函数  
        /// </summary>  
        /// <param name="sender">激发事件的对象</param>  
        /// <param name="e">与发送或接收完成操作相关联的SocketAsyncEventArg对象</param>  
        private void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                // Determine which type of operation just completed and call the associated handler.  
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Send:
                        ProcessSend((AsyncEventArgs)e);
                        break;
                    case SocketAsyncOperation.Receive:
                        ProcessReceive((AsyncEventArgs)e);
                        break;
                    default:
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }
            }
            catch (Exception ex)
            {
                Log4Debug(DateTime.Now.ToString("yyyyMMdd hhMMss.fff") + " OnIOCompleted Error:" + ex.Message + "\r\n" + ex.StackTrace);
                Error2File("OnIOCompleted Error:" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }



        #endregion

        #region Recieve


        /// <summary>  
        ///接收完成时处理函数  
        /// </summary>  
        /// <param name="aRecvArgs">与接收完成操作相关联的SocketAsyncEventArg对象</param>  
        private void ProcessReceive(AsyncEventArgs aRecvArgs)
        {
            if (!IsRunning)
            {
                return;
            }

            AsyncSocketUserToken userToken = aRecvArgs.UserToken as AsyncSocketUserToken;
            Socket s = userToken.ConnectSocket;
            if (s == null)
            {
                return;
            }

            if (aRecvArgs.BytesTransferred > 0 && aRecvArgs.SocketError == SocketError.Success)  
            {
                userToken.ActiveTime = DateTime.Now;

                //判断所有需接收的数据是否已经完成  
                //if (userToken.ConnectSocket.Available == 0)
                if (aRecvArgs.BytesTransferred > 0)
                {
                    // 增加服务器接收的总字节数。
                    Interlocked.Add(ref _totalBytesRead, aRecvArgs.BytesTransferred);


                    OnRecieve?.Invoke("ProcessReceive", aRecvArgs);
                    if (!aRecvArgs.IsReuse)
                    {
                        //(aRecvArgs.UserToken as AsyncSocketUserToken).FreeArgs(aRecvArgs);
                    }
                    //Log4Debug("recv FreeArgs, poolCnt: " + userToken.ArgsPoolCnt);

                    //new Thread(obj =>
                    //{
                    //    var args = obj as AsyncEventArgs;
                    //    var token = (args.UserToken as AsyncSocketUserToken);
                    //    OnRecieve?.Invoke("ProcessReceive", args);
                    //    if (!args.IsReuse)
                    //        token.FreeArgs(args);

                    //    //Log4Debug(string.Format("recv FreeArgs, alloc pool: {0} ,  free pool: {1}",
                    //    //    token.AllocArgsPoolCnt, token.FreeArgsPoolCnt));

                    //}).Start(aRecvArgs);

                    // TODO 处理数据 
                    //string info = Encoding.Default.GetString(aRecvArgs.Buffer, aRecvArgs.Offset, aRecvArgs.BytesTransferred);
                    //Log4Debug(String.Format("已接收 {0} byte，当前接收[{1}]：{2}",_totalBytesRead, s.RemoteEndPoint.ToString(), info));


                    // 给客户端回显。
                    //byte[] buf = userToken.SendEventArgs.Buffer;
                    //int offset = userToken.SendEventArgs.Offset;
                    //Array.Copy(aRecvArgs.Buffer, aRecvArgs.Offset, buf, offset, aRecvArgs.BytesTransferred);
                    //Send(userToken.SendEventArgs, buf, offset, aRecvArgs.BytesTransferred);


                    //Send(serToken.SendEventArgs, aRecvArgs.Buffer, aRecvArgs.Offset, aRecvArgs.BytesTransferred);

                }

                bool repeat = false;

                do
                {
                    //var recvArgs = userToken.NewArgs();
                    var recvArgs = aRecvArgs;

                    if (!s.ReceiveAsync(recvArgs))//为接收下一段数据，投递接收请求
                    {
                        //同步接收时处理接收完成事件  
                        //var args = recvArgs;
                        
                        //Task.Factory.StartNew( obj => { 
                        //    ProcessReceive(obj as AsyncEventArgs);
                        //}, args);

                        ProcessReceive(recvArgs);

                        //repeat = true;
                    }
                    else
                    {
                        repeat = false;
                    }

                } while (repeat);


            }
            else
            {
                CloseClientSocket(aRecvArgs);
            }
        }

        #endregion

        #region Send

        /// <summary>  
        /// 异步的发送数据  
        /// </summary>  
        /// <param name="aSendArgs"></param>  
        /// <param name="buffer"></param>  
        public void Send(AsyncEventArgs aSendArgs, byte[] buffer)
        {
            Send(aSendArgs, buffer, 0, buffer.Length);
        }

        public void Send(AsyncEventArgs aSendArgs)
        {
            Send(aSendArgs, aSendArgs.Buffer, aSendArgs.Offset, aSendArgs.Count);
        }

        StringBuilder sb = new StringBuilder();

        public void Send(AsyncEventArgs aSendArgs, byte[] buffer, int offset, int count)
        {
            AsyncSocketUserToken userToken = aSendArgs.UserToken as AsyncSocketUserToken;
            if (userToken.ConnectSocket.Connected)
            {
                //Array.Copy(buffer, offset, aSendArgs.Buffer, 0, count);//设置发送数据  

                aSendArgs.SetBuffer(buffer, offset, count); //设置发送数据 

                if (!userToken.ConnectSocket.SendAsync(aSendArgs))//投递发送请求，这个函数有可能同步发送出去，这时返回false，并且不会引发AsyncEventArgs.Completed事件  
                {
                    // 同步发送时处理发送完成事件  
                    ProcessSend(aSendArgs);
                }

                sb.Clear();
                sb.Append(DateTime.Now.ToString("[HH:mm:ss.fff] ") + "  Tx: ");
                for (int i = offset; i < offset + count; i++)
                {
                    sb.Append(buffer[i].ToString("X2") + " ");
                }
                LogOut?.Invoke("server.Send", new LogOutEventArgs(0, sb.ToString()));
            }
        }

        /// <summary>  
        /// 同步的使用socket发送数据  
        /// </summary>  
        /// <param name="socket"></param>  
        /// <param name="buffer"></param>  
        /// <param name="offset"></param>  
        /// <param name="size"></param>  
        /// <param name="timeout"></param>  
        public void Send(Socket socket, byte[] buffer, int offset, int size, int timeout)
        {
            socket.SendTimeout = 0;
            int startTickCount = Environment.TickCount;
            int sent = 0; // how many bytes is already sent  
            do
            {
                if (Environment.TickCount > startTickCount + timeout)
                {
                    //throw new Exception("Timeout.");  
                }
                try
                {
                    sent += socket.Send(buffer, offset + sent, size - sent, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                    ex.SocketErrorCode == SocketError.IOPending ||
                    ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // socket buffer is probably full, wait and try again  
                        Thread.Sleep(30);
                    }
                    else
                    {
                        throw ex; // any serious error occurr  
                    }
                }
            } while (sent < size);
        }

        /// <summary>  
        /// 发送完成时处理函数  
        /// </summary>  
        /// <param name="e">与发送完成操作相关联的SocketAsyncEventArg对象</param>  
        private void ProcessSend(AsyncEventArgs aSendArgs)
        {
            if (!IsRunning)
            {
                return;
            }

            var userToken = aSendArgs.UserToken as AsyncSocketUserToken;

            if (aSendArgs.SocketError == SocketError.Success)
            {
                //TODO
                userToken.ActiveTime = DateTime.Now;
            }
            else
            {
                CloseClientSocket(aSendArgs);
            }

            userToken.FreeArgs(aSendArgs);

            //Log4Debug(string.Format("recv FreeArgs, alloc pool: {0} ,  free pool: {1}",
            //                userToken.AllocArgsPoolCnt, userToken.FreeArgsPoolCnt));

        }
        #endregion

        #region Close

        /// <summary>  
        /// 关闭socket连接  
        /// </summary>  
        /// <param name="s"></param>  
        /// <param name="e"></param>  
        public void CloseClientSocket(AsyncEventArgs e)
        {
            AsyncSocketUserToken userToken = e.UserToken as AsyncSocketUserToken;

            if (userToken.ConnectSocket == null)
            {
                return;
            }
            Interlocked.Decrement(ref _clientCount);
            Log4Debug(String.Format("客户端 {0} 断开连接, 当前 {1} 个连接。", userToken.RemoteEndPoint.ToString(), _clientCount));

            try
            {
                userToken.ConnectSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
                // Throw if client has closed, so it is not necessary to catch.  
            }
            finally
            {
                userToken.ConnectSocket.Close();
                userToken.ConnectSocket = null;
            }

            //Interlocked.Decrement(ref _clientCount);
            _maxAcceptClientSem.Release();
            _userTokenPool.Push(userToken);
            _clientList.Remove(userToken);

            OnConnectChange?.Invoke("discnnt", userToken.RecvEventArgs);

        }
        #endregion

        #region Dispose
        /// <summary>  
        /// Performs application-defined tasks associated with freeing,   
        /// releasing, or resetting unmanaged resources.  
        /// </summary>  
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>  
        /// Releases unmanaged and - optionally - managed resources  
        /// </summary>  
        /// <param name="disposing"><c>true</c> to release   
        /// both managed and unmanaged resources; <c>false</c>   
        /// to release only unmanaged resources.</param>  
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        Stop();
                        if (_serverSock != null)
                        {
                            _serverSock = null;
                        }
                    }
                    catch (SocketException ex)
                    {
                        //TODO 事件 
                        Error2File(ex.Message + "\r\n" + ex.StackTrace);
                    }
                }
                disposed = true;
            }
        }
        #endregion

        #region Log
        public event EventHandler<LogOutEventArgs> LogOut;
        private void Log4Debug(string msg)
        {
            //LogOutEventArgs logArgs = _logOutEvtArgsPool.Pop();
            //logArgs.SetArgs(msg, _clientCount, _totalBytesRead);
            LogOutEventArgs logArgs = new LogOutEventArgs(msg, _clientCount, _totalBytesRead);
            if (LogOut != null)
            {
                LogOut(this, logArgs);
                _logOutEvtArgsPool.Push(logArgs);
            }
            

            //Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss.fff") + "] " + msg);

            //Program.Logger.WriteLine(msg);

        }

        public void Error2File(string msg)
        {
            Log.Error(msg);
        }
        #endregion
    }
}
