using System;
using System.Collections.Generic;

namespace AsyncSocketServer
{
    public class AsyncSocketUserTokenList : Object
    {
        private List<AsyncSocketUserToken> _list;

        public AsyncSocketUserTokenList()
        {
            _list = new List<AsyncSocketUserToken>();
        }

        public void Clear()
        {
            lock (_list)
            {
                _list.Clear();
            }
        }

        public void Add(AsyncSocketUserToken userToken)
        {
            lock (_list)
            {
                _list.Add(userToken);
            }
        }

        public void Remove(AsyncSocketUserToken userToken)
        {
            lock (_list)
            {
                _list.Remove(userToken);
            }
        }

        public void CopyList(ref AsyncSocketUserToken[] array)
        {
            lock (_list)
            {
                array = new AsyncSocketUserToken[_list.Count];
                _list.CopyTo(array);
            }
        }
    }
}
