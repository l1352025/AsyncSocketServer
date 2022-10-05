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
        public int Offset { get; private set; }
        public int Length { get; private set; }

        public AsyncSocketClientArgs(string msg) :this(msg, null, 0, 0)
        {
        }
        public AsyncSocketClientArgs(string msg, byte[] buf, int offset, int len)
        {
            _msg = msg;
            _buf = buf;
            Offset = offset;
            Length = len;
        }
    }
}
