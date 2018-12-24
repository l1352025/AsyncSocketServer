using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncSocketServer
{
    public class LogOutEventArgs : EventArgs
    {
        private string _msg;
        private int _clientCount;
        private int _recvBytes;

        public string Msg { get { return _msg; } }
        public int ClientCount { get { return _clientCount; } }
        public int RecvBytes { get { return _recvBytes; } }

        public LogOutEventArgs(string msg, int clients)
            :this(msg, clients, 0)
        {
        }

        public LogOutEventArgs(int recvBytes, string msg)
            : this(msg, 0, recvBytes)
        {
        }
        public LogOutEventArgs(string msg, int clients, int recvBytes)
        {
            _msg = msg;
            _clientCount = clients;
            _recvBytes = recvBytes;
        }

        public void SetArgs(string msg, int clients, int recvBytes)
        {
            _msg = msg;
            _clientCount = clients;
            _recvBytes = recvBytes;
        }
    }
}
