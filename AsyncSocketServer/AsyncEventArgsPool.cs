using System;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace AsyncSocketServer
{
    public class AsyncEventArgs : SocketAsyncEventArgs
    {
        public bool IsReuse;
    }

    public class AsyncEventArgsPool
    {
        private ConcurrentQueue<AsyncEventArgs> _pool;

        public int Count{   get { return _pool.Count; } }

        public AsyncEventArgsPool(int capacity)
        {
            _pool = new ConcurrentQueue<AsyncEventArgs>();
        }

        public void Clear()
        {
            while (_pool.Count > 0)
            {
                _pool.TryDequeue(out _);
            }
        }

        public void Push(AsyncEventArgs item)
        {
            if (item == null)
            {
                throw new ArgumentException("Items added to AsyncEventArgsPool cannot be null");
            }

            _pool.Enqueue(item);
        }

        public AsyncEventArgs Pop()
        {
            AsyncEventArgs args;
            _pool.TryDequeue(out args);
            return args;
        }
    }
}
