using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace ElectricPowerLib.Common
{
    /// <summary>
    /// 日志输出辅助类
    /// </summary>
    public class LogHelper : IDisposable
    {
        private static ConcurrentQueue<string> _logQueue;
        private static Thread _logOutputTask;
        private static Semaphore _logSem;
        private static string _logFileName;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// LogHelper类实例化，并使用log队列缓存，然后批量写入到文件
        /// </summary>
        /// <param name="logName">日志文件名</param>
        public LogHelper(string logName)
        {
            _logFileName = logName;
            _logQueue = new ConcurrentQueue<string>();
            _logSem = new Semaphore(0, 1000);

            _logOutputTask = new Thread(new ThreadStart(delegate 
            {
                string strLog;
                StreamWriter sw;
                while(_logOutputTask.IsAlive)
                {
                    _logSem.WaitOne();
                    sw = new StreamWriter(_logFileName, true, Encoding.UTF8);
                    while(_logQueue.Count > 0 && _logQueue.TryDequeue(out strLog))
                    {
                        sw.Write(strLog);
                    }
                    sw.Close();
                }
            }));

            _logOutputTask.IsBackground = true;
            _logOutputTask.Start();
        }

        /// <summary>
        /// 关闭时处理
        /// </summary>
        public void Close()
        {
            if(_logOutputTask != null)
            {
                _logOutputTask.Abort();
                _logOutputTask = null;
            }

            if(_logQueue != null)
            {
                _logQueue = null;
            }

            if(_logSem != null)
            {
                _logSem.Close();
                _logSem = null;
            }
        }

        /// <summary>
        /// 写log，不追加换行符
        /// </summary>
        /// <param name="str">log字符串</param>
        public void Write(string str)
        {
            Write(str, false);
        }

        /// <summary>
        /// 写log，不追加换行符
        /// </summary>
        /// <param name="str">log字符串</param>
        /// <param name="isShowTime">是否在log前面显示当前时间： "[yyyy-MM-dd HH:mm:ss.fff]  "</param>
        public void Write(string str, bool isShowTime)
        {
            Thread t = new Thread(new ThreadStart(delegate 
            {
                if (isShowTime)
                {
                    str = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff]  ") + str;
                }
                _logQueue.Enqueue(str);
                _logSem.Release(1);
            }));

            t.Start();
        }

        /// <summary>
        /// 写log，追加换行符
        /// </summary>
        /// <param name="str">log字符串</param>
        public void WriteLine(string str)
        {
            WriteLine(str, false);
        }

        /// <summary>
        /// 写log，追加换行符
        /// </summary>
        /// <param name="str">log字符串</param>
        /// <param name="isShowTime">是否在log前面显示当前时间： "[yyyy-MM-dd HH:mm:ss.fff]  "</param>
        public void WriteLine(string str, bool isShowTime)
        {
            Thread t = new Thread(new ThreadStart(delegate
            {
                if (isShowTime)
                {
                    str = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff]  ") + str;
                }
                _logQueue.Enqueue(str + "\r\n");
                _logSem.Release(1);
            }));

            t.Start();
        }

        // 静态函数

        /// <summary>
        /// 写log到文件，不追加换行符
        /// </summary>
        /// <param name="path">文件名</param>
        /// <param name="str">log字符串</param>
        public static void Write(string path, string str)
        {
            Write(path, str, false);
        }

        /// <summary>
        /// 写log到文件，不追加换行符
        /// </summary>
        /// <param name="path">文件名</param>
        /// <param name="str">log字符串</param>
        /// <param name="isShowTime">是否在log前面显示当前时间： "[yyyy-MM-dd HH:mm:ss.fff]  "</param>
        public static void Write(string path, string str, bool isShowTime)
        {
            Thread t = new Thread(new ThreadStart(delegate
            {
                if (isShowTime)
                {
                    str = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff]  ") + str;
                }

                using(StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
                {
                    sw.Write(str);
                }
            }));

            t.Start();
        }

        /// <summary>
        /// 写log到文件，追加换行符
        /// </summary>
        /// <param name="path">文件名</param>
        /// <param name="str">log字符串</param>
        public static void WriteLine(string path, string str)
        {
            WriteLine(path, str, false);
        }

        /// <summary>
        /// 写log到文件，追加换行符
        /// </summary>
        /// <param name="path">文件名</param>
        /// <param name="str">log字符串</param>
        /// <param name="isShowTime">是否在log前面显示当前时间： "[yyyy-MM-dd HH:mm:ss.fff]  "</param>
        public static void WriteLine(string path, string str, bool isShowTime)
        {
            Thread t = new Thread(new ThreadStart(delegate
            {
                if (isShowTime)
                {
                    str = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff]  ") + str;
                }

                using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
                {
                    sw.WriteLine(str);
                }
            }));

            t.Start();
        }
    }
}
