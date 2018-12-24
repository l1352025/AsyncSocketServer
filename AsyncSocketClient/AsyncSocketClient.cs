using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace AsyncSocketClient
{
    public class IOCPClient
    {
        private Socket _clientSock;             // 连接服务器的socket 
        private IPEndPoint _remoteEndPoint;     // 服务器监听端点  
        private Boolean _connected = false;     // Socket连接标志 

        private SocketAsyncEventArgs _cnntEvtArgs;
        private SocketAsyncEventArgs _recvEvtArgs;
        private SocketAsyncEventArgs _sendEvtArgs;
        private byte[] _recvBuffer;

        private static Semaphore acceptSem = new Semaphore(1, 1);
        private const int FLAG_Recv = 1, FLAG_Send = 0;
        private static AutoResetEvent[] autoSendReceiveEvents = new AutoResetEvent[]  
        {  
            new AutoResetEvent(true),  
            new AutoResetEvent(true)  
        };

        public Boolean IsConnected { get { return _connected; } }
        public event EventHandler<AsyncSocketClientArgs> Connected;
        public event EventHandler<AsyncSocketClientArgs> Disconnected;
        public event EventHandler<AsyncSocketClientArgs> DataRecieved;
        public event EventHandler<AsyncSocketClientArgs> DataSent;

        public IOCPClient(IPAddress ip, int port)
            : this(new IPEndPoint(ip, port))
        {
        }
        public IOCPClient(IPEndPoint remote)
        {
            _clientSock = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _remoteEndPoint = remote;

            _cnntEvtArgs = new SocketAsyncEventArgs();
            _cnntEvtArgs.UserToken = _clientSock;
            _cnntEvtArgs.RemoteEndPoint = _remoteEndPoint;
            _cnntEvtArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnected);

            _recvBuffer = new byte[2048];
            _recvEvtArgs = new SocketAsyncEventArgs();
            _recvEvtArgs.UserToken = _clientSock;
            _recvEvtArgs.RemoteEndPoint = _remoteEndPoint;
            _recvEvtArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceiveComplete);
            _recvEvtArgs.SetBuffer(_recvBuffer, 0, _recvBuffer.Length);

            _sendEvtArgs = new SocketAsyncEventArgs();
            _sendEvtArgs.UserToken = _clientSock;
            _sendEvtArgs.RemoteEndPoint = _remoteEndPoint;
            _sendEvtArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendComplete);
            
        }

        #region 连接服务器

        /// <summary>  
        /// 连接远程服务器  
        /// </summary>  
        public void Connect()
        {
            if (!_connected)
            {
                acceptSem.WaitOne();
                if (!_clientSock.ConnectAsync(_cnntEvtArgs))//异步连接  
                {
                    ProcessConnected(_cnntEvtArgs);
                }
            }
        }
        /// <summary>  
        /// 连接上的事件  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        void OnConnected(object sender, SocketAsyncEventArgs e)
        {
            ProcessConnected(e);
        }
        /// <summary>  
        /// 处理连接服务器  
        /// </summary>  
        /// <param name="e"></param>  
        private void ProcessConnected(SocketAsyncEventArgs e)
        {
            acceptSem.Release();

            //TODO  
            if (e.SocketError == SocketError.Success)
            {
                //设置Socket已连接标志。   
                _connected = true;

                string msg = "Connected, Local " + e.ConnectSocket.LocalEndPoint + " ==> Remote " + e.RemoteEndPoint;
                if (Connected != null)
                {
                    Connected(this, new AsyncSocketClientArgs(msg));
                }

                StartRecive();
            }
            else
            {
                if (Connected != null)
                {
                    Connected(this, new AsyncSocketClientArgs("Connect Failed .."));
                }
            }
        }

        #endregion

        #region 发送消息
        /// <summary>  
        /// 向服务器发送消息  
        /// </summary>  
        /// <param name="data"></param>  
        public void Send(byte[] data)
        {
            autoSendReceiveEvents[FLAG_Send].WaitOne();

            _sendEvtArgs.SetBuffer(data, 0, data.Length);

            if(!_clientSock.Connected)
            {
                autoSendReceiveEvents[FLAG_Send].Set();
                return;
            }
            
            if (!_clientSock.SendAsync(_sendEvtArgs))//投递发送请求
            { 
                // 若不需要异步  
                ProcessSend(_sendEvtArgs);
            }
        }

        /// <summary>  
        /// 发送操作的回调方法  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        private void OnSendComplete(object sender, SocketAsyncEventArgs e)
        {
            //发出发送完成信号。   
            autoSendReceiveEvents[FLAG_Send].Set();
            ProcessSend(e);
        }

        /// <summary>  
        /// 发送完成时处理函数  
        /// </summary>  
        /// <param name="e">与发送完成操作相关联的SocketAsyncEventArg对象</param>  
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            //TODO  
            if(e.SocketError == SocketError.Success)
            {
                if (DataSent != null)
                {
                    DataSent(this, new AsyncSocketClientArgs("Send Success"));
                }
            }
            else
            {
                Close();
            }
        }
        #endregion

        #region 接收消息
        /// <summary>  
        /// 开始监听服务端数据  
        /// </summary>  
        /// <param name="e"></param>  
        public void StartRecive()
        {
            //准备接收。   
            autoSendReceiveEvents[FLAG_Recv].WaitOne();

            if (!_clientSock.Connected)
            {
                autoSendReceiveEvents[FLAG_Recv].Set();
                return;
            }

            if (!_clientSock.ReceiveAsync(_recvEvtArgs))
            {
                ProcessReceive(_recvEvtArgs);
            }
        }

        /// <summary>  
        /// 接收操作的回调方法  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        private void OnReceiveComplete(object sender, SocketAsyncEventArgs e)
        {
            //发出接收完成信号。   
            autoSendReceiveEvents[FLAG_Recv].Set();
            ProcessReceive(e);
        }

        /// <summary>  
        ///接收完成时处理函数  
        /// </summary>  
        /// <param name="e">与接收完成操作相关联的SocketAsyncEventArg对象</param>  
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                // 检查远程主机是否关闭连接  
                if (e.BytesTransferred > 0)
                {
                    Socket s = (Socket)e.UserToken;
                    //判断所有需接收的数据是否已经完成  
                    if (s.Available == 0)
                    {
                        byte[] data = new byte[e.BytesTransferred];
                        Array.Copy(e.Buffer, e.Offset, data, 0, data.Length);//从e.Buffer块中复制数据出来，保证它可重用  

                        //TODO 处理数据
                        if (DataRecieved != null)
                        {
                            DataRecieved(this, new AsyncSocketClientArgs("Recieved : ", data));
                        }
  
                    }

                    if (!s.ReceiveAsync(e))//为接收下一段数据，投递接收请求，这个函数有可能同步完成，这时返回false，并且不会引发SocketAsyncEventArgs.Completed事件  
                    {
                        //同步接收时处理接收完成事件  
                        ProcessReceive(e);
                    }
                }
            }
            else
            {
                Close();
            }
        }

        #endregion


        public void Close()
        {
            _clientSock.Disconnect(true);
            _connected = false;

            if (Disconnected != null)
            {
                Disconnected(this, new AsyncSocketClientArgs("Disconnected . ."));
            }
        }

        /// <summary>  
        /// 失败时关闭Socket，根据SocketError抛出异常。  
        /// </summary>  
        /// <param name="e"></param>  

        private void ProcessError(SocketAsyncEventArgs e)
        {
            Socket s = e.UserToken as Socket;
            if (s.Connected)
            {
                //关闭与客户端关联的Socket  
                try
                {
                    s.Shutdown(SocketShutdown.Both);
                }
                catch (Exception)
                {
                    //如果客户端处理已经关闭，抛出异常   
                }
                finally
                {
                    if (s.Connected)
                    {
                        s.Close();
                    }
                }
            }
            //抛出SocketException   
            throw new SocketException((Int32)e.SocketError);
        }


        /// <summary>  
        /// 释放SocketClient实例  
        /// </summary>  
        public void Dispose()
        {
            autoSendReceiveEvents[FLAG_Send].Close();
            autoSendReceiveEvents[FLAG_Recv].Close();
            if (_clientSock.Connected)
            {
                _clientSock.Close();
            }
        }

    }  
}
