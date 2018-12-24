using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AsyncSocketServer
{
    class DaemonThread : Object
    {
        private Thread _thread;
        private AsyncSocketServer _asyncSocketServer;

        public DaemonThread(AsyncSocketServer asyncSocketServer)
        {
            _asyncSocketServer = asyncSocketServer;
            _thread = new Thread(DaemonThreadStart);
            _thread.Start();
        }

        public void DaemonThreadStart()
        {
            while (_thread.IsAlive)
            {
                AsyncSocketUserToken[] userTokenArray = null;
                _asyncSocketServer.ConnectedClients.CopyList(ref userTokenArray);
                for (int i = 0; i < userTokenArray.Length; i++)
                {
                    if (!_thread.IsAlive)
                        break;
                    try
                    {
                        if ((DateTime.Now - userTokenArray[i].ActiveTime).Milliseconds > _asyncSocketServer.SocketTimeOutMS) //超时Socket断开
                        {
                            lock (userTokenArray[i])
                            {
                                _asyncSocketServer.CloseClientSocket(userTokenArray[i].RecvEventArgs);
                            }
                        }
                    }
                    catch (Exception E)
                    {
                        Program.Logger.WriteLine(string.Format("Daemon thread check timeout socket error, message: {0}", E.Message));
                        Program.Logger.WriteLine(E.StackTrace);
                    }
                }

                for (int i = 0; i < 60 * 1000 / 10; i++) //每分钟检测一次
                {
                    if (!_thread.IsAlive)
                        break;
                    Thread.Sleep(10);
                }
            }
        }

        public void Close()
        {
            _thread.Abort();
            _thread.Join();
        }
    }
}
