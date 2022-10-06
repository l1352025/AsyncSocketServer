using System;
using System.Net;
using System.Net.Sockets;

namespace AsyncSocketServer
{
    public class AsyncSocketUserToken
    {
        private Socket _socket;                     // 通信SOKET
        private AsyncEventArgsPool _eventArgsPool;

        public EndPoint LocalEndPoint { get; set; }     // 本地地址：客户端终结点 
        public EndPoint RemoteEndPoint { get; set; }    // 远程地址：服务器终结点 
        //public SocketAsyncEventArgs SendEventArgs { get; set; }     // 通信SOKET异步发送事件参数 
        public AsyncEventArgs RecvEventArgs { get; set; }     // 通信SOKET异步接收事件参数 
        
        public DateTime ConnectTime { get; set; }       // 连接时间 
        public DateTime ActiveTime { get; set; }        // 活动时间 

        //public UserInfoModel UserInfo { get; set; }   // 所属用户信息 

        public int PoolCnt => _eventArgsPool.Count;

        public Socket ConnectSocket                     // 通信SOKET
        {
            get
            {
                return _socket;
            }

            set
            {
                _socket = value;
                RecvEventArgs.AcceptSocket = _socket;
            }
        }


        public AsyncSocketUserToken(AsyncEventArgsPool eventArgsPool)
        {
            _eventArgsPool = eventArgsPool;

            RecvEventArgs = new AsyncEventArgs();
            RecvEventArgs.UserToken = this;
        }
        
        public AsyncEventArgs NewArgs()
        {
            var args = _eventArgsPool.Pop();
            if(args != null) args.UserToken = this;
            args.SetBuffer(args.Buffer, args.Offset, 1024 * 100);

            return args;
        }

        public void FreeArgs(AsyncEventArgs args)
        {
            args.IsReuse = false;
            _eventArgsPool.Push(args);
        }
    }
}
