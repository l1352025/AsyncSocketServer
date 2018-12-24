using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncSocketClient
{
    public class AsyncSocketClientArgs : EventArgs
    {
        private string _msg;
        private byte[] _buf;

        public string Msg { get { return _msg; } }
        public byte[] Buffer { get { return _buf; } }

        public AsyncSocketClientArgs(string msg, byte[] buf = null)
        {
            _msg = msg;
            _buf = buf;
        }
    }
}
