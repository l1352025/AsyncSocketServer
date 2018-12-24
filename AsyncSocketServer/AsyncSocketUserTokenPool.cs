using System;
using System.Collections.Generic;

namespace AsyncSocketServer
{
    class AsyncSocketUserTokenPool
    {
        private Stack<AsyncSocketUserToken> _pool;

        public int Count{   get { return _pool.Count; } }

        public AsyncSocketUserTokenPool(int capacity)
        {
            _pool = new Stack<AsyncSocketUserToken>(capacity);
        }

        public void Clear()
        {
            lock(_pool)
            {
                _pool.Clear();
            }
        }

        public void Push(AsyncSocketUserToken item)
        {
            if (item == null)
            {
                throw new ArgumentException("Items added to AsyncSocketUserTokenPool cannot be null");
            }

            lock (_pool)
            {
                _pool.Push(item);
            }
        }

        public AsyncSocketUserToken Pop()
        {
            lock (_pool)
            {
                return _pool.Pop();
            }
        }
    }
}
