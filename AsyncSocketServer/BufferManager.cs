using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace AsyncSocketServer
{
    public class BufferManager
    {
        int m_numBytes;                 // the total number of bytes controlled by the buffer pool
        byte[] m_buffer;                // the underlying byte array maintained by the Buffer Manager
        Queue<int> m_freeIndexPool;     
        int m_currentIndex;
        int m_bufferSize;

        int m_numBytesSmall;                
        byte[] m_bufferSmall;
        Queue<int> m_freeIndexPoolSmall;
        int m_currentIndexSmall;
        int m_bufferSizeSmall;

        public BufferManager(int bufferCnt, int bufferSize)
            :this(bufferCnt, bufferSize, 0, 0)
        {
        }

        public BufferManager(int bigBufCnt, int bigBufSize, int smallBufCnt, int smallBufSize)
        {
            m_numBytes = bigBufCnt * bigBufSize;
            m_currentIndex = 0;
            m_bufferSize = bigBufSize;
            m_buffer = new byte[m_numBytes];
            m_freeIndexPool = new Queue<int>();

            m_numBytesSmall = smallBufCnt * smallBufSize;
            m_currentIndexSmall = 0;
            m_bufferSizeSmall = smallBufSize;
            m_bufferSmall = new byte[m_numBytesSmall];
            m_freeIndexPoolSmall = new Queue<int>();
        }

        // Assigns a buffer from the buffer pool to the 
        // specified SocketAsyncEventArgs object
        //
        // <returns>true if the buffer was successfully set, else false</returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {

            if (m_freeIndexPool.Count > 0)
            {
                args.SetBuffer(m_buffer, m_freeIndexPool.Dequeue(), m_bufferSize);
            }
            else
            {
                if ((m_numBytes - m_bufferSize) < m_currentIndex)
                {
                    return false;
                }
                args.SetBuffer(m_buffer, m_currentIndex, m_bufferSize);
                m_currentIndex += m_bufferSize;
            }
            return true;
        }

        public bool SetBuffer(BufferItem bufItem, int maxLen)
        {
            if (m_bufferSizeSmall > 0 && maxLen < m_bufferSizeSmall)
            {
                if (m_freeIndexPoolSmall.Count > 0)
                {
                    bufItem.SetBuffer(m_bufferSmall, m_freeIndexPoolSmall.Dequeue(), m_bufferSizeSmall);
                }
                else
                {
                    if ((m_numBytesSmall - m_bufferSizeSmall) < m_currentIndexSmall)
                    {
                        return false;
                    }
                    bufItem.SetBuffer(m_bufferSmall, m_currentIndexSmall, m_bufferSizeSmall);
                    m_currentIndexSmall += m_bufferSizeSmall;
                }
            }
            else
            {
                if (m_freeIndexPool.Count > 0)
                {
                    bufItem.SetBuffer(m_buffer, m_freeIndexPool.Dequeue(), m_bufferSize);
                }
                else
                {
                    if ((m_numBytes - m_bufferSize) < m_currentIndex)
                    {
                        return false;
                    }
                    bufItem.SetBuffer(m_buffer, m_currentIndex, m_bufferSize);
                    m_currentIndex += m_bufferSize;
                }
            }
            return true;
        }

        // Removes the buffer from a SocketAsyncEventArg object.  
        // This frees the buffer back to the buffer pool
        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            m_freeIndexPool.Enqueue(args.Offset);
            args.SetBuffer(null, 0, 0);
        }

        public void FreeBuffer(BufferItem bufItem)
        {
            if (m_bufferSizeSmall > 0 && bufItem.Count <= m_bufferSizeSmall)
            {
                m_freeIndexPoolSmall.Enqueue(bufItem.Offset);
                bufItem.SetBuffer(null, 0, 0);
            }
            else
            {
                m_freeIndexPool.Enqueue(bufItem.Offset);
                bufItem.SetBuffer(null, 0, 0);
            }
        }


        public int CheckAllocatedBufCnt(ref int freePoolCnt)
        {
            freePoolCnt = m_freeIndexPool.Count;

            return m_currentIndex / m_bufferSize;
        }

        public int CheckAllocatedSmallBufCnt(ref int freePoolCnt)
        {
            if (m_bufferSizeSmall == 0) return 0;

            freePoolCnt = m_freeIndexPoolSmall.Count;

            return m_currentIndexSmall / m_bufferSizeSmall;
        }
    }
}
