using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace AsyncSocketServer
{
    class Log
    {
        Mutex _logMutex = new Mutex();

        private string _logFilePath = ""; //文件路径
        public string LogFilePath         //文件路径读写
        {
            get { return _logFilePath; }
            set { _logFilePath = value; }
        }

        /// <summary>
        /// 不带参数的构造函数
        /// </summary>
        public Log()
            :this(DateTime.Now.ToString("yyyyMMdd") + ".txt")
        {
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="LogFilePath">文件路径</param>
        public Log(string logFilePath)
        {
            _logFilePath = logFilePath;           
        }

        #region 读日志
        public string ReadLine()
        {
            return ReadLine(_logFilePath);
        }

        public string ReadLine(string filePath)
        {
            string strRet = "";

            if (String.IsNullOrEmpty(filePath)) return strRet;

            _logMutex.WaitOne();
            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                strRet = sr.ReadLine();
            }
            _logMutex.ReleaseMutex();

            return strRet;
        }

        public string ReadAll(string filePath)
        {
            string strRet = "";

            _logMutex.WaitOne();
            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                strRet = sr.ReadToEnd();
            }
            _logMutex.ReleaseMutex();

            return strRet;
        }
        #endregion 


        #region 写日志

        public void Write(string text)
        {
            Write(_logFilePath, text);
        }
        public void WriteLine(string text)
        {
            text += "\r\n";
            Write(_logFilePath, text);
        }
       
        /// <summary>
        /// 追加一条16进制字节信息 如果文件不存在，则自动创建
        /// </summary>
        /// <param name="bstr">字节数组</param>
        /// <param name="Len">字节长度</param>
        public void Write(byte[] bstr, int Len)
        {
            StringBuilder strbuid = new StringBuilder();
            strbuid.Clear();

            for (int i = 0; i < Len; i++)
            {
                strbuid.Append(bstr[i].ToString("X2") + "  ");
            }

            Write(_logFilePath, strbuid.ToString());
        }

        /// <summary>
        /// 追加一行16进制字节信息 如果文件不存在，则自动创建
        /// </summary>
        /// <param name="bstr">字节数组</param>
        /// <param name="Len">字节长度</param>
        public void WriteLine(byte[] bstr, int Len)
        {
            StringBuilder strbuid = new StringBuilder();
            strbuid.Clear();          

            for (int i = 0; i < Len; i++)
            {
                strbuid.Append(bstr[i].ToString("X2") + "  ");
            }
            strbuid.Append("\r\n");

            Write(_logFilePath, strbuid.ToString());
        }

        public void Write(string filePath, string text)
        {
            _logMutex.WaitOne();
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff] ") + text);
                sw.Close();
            }
            _logMutex.ReleaseMutex();
        }
        /// <summary>
        /// 向指定文件追加一行错误信息 如果文件不存在，则自动创建
        /// </summary>
        public static void Error(string text)
        {
            Error(DateTime.Now.ToString("yyyyMMdd") + "_error.txt", text);
        }
        public static void Error(string LogFilePath, string text)
        {
            using (StreamWriter sw = new StreamWriter(LogFilePath, true, Encoding.UTF8))
            {
                sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff] ") + text);
                sw.Close();
            }
        }

        #endregion
    }
}
