using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsyncSocketServer
{
    public class AsyncSocketUserToken
    {
        private Socket _socket;                     // 通信SOKET

        public EndPoint LocalEndPoint { get; set; }     // 本地地址：客户端终结点 
        public EndPoint RemoteEndPoint { get; set; }    // 远程地址：服务器终结点 
        public SocketAsyncEventArgs SendEventArgs { get; set; } // 通信SOKET异步发送事件参数 
        public SocketAsyncEventArgs RecvEventArgs { get; set; } // 通信SOKET异步接收事件参数 
        
        public DateTime ConnectTime { get; set; }       // 连接时间 
        public DateTime ActiveTime { get; set; }        // 活动时间 

        //public UserInfoModel UserInfo { get; set; }   // 所属用户信息 

        //public List<byte> Buffer { get; set; }          // 数据缓存区

        public Socket ConnectSocket                     // 通信SOKET
        {
            get
            {
                return _socket;
            }

            set
            {
                _socket = value;
                SendEventArgs.AcceptSocket = _socket;
                RecvEventArgs.AcceptSocket = _socket;
            }
        }


        public AsyncSocketUserToken()
        {
            SendEventArgs = new SocketAsyncEventArgs();
            SendEventArgs.UserToken = this;

            RecvEventArgs = new SocketAsyncEventArgs();
            RecvEventArgs.UserToken = this;
        }  
    }
}
