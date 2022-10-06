using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncSocketServer
{
    public class BufferItem
    {
        public byte[] Buffer;
        public int Offset;
        public int Count;
        public int DataLen;

        public void SetBuffer(byte[] buf, int offset, int len)
        {
            Buffer = buf;
            Offset = offset;
            Count = len;
            DataLen = 0;
        }
    }
}
