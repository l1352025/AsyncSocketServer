using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AsyncSocketServer
{
    class Program
    {
        public static ElectricPowerLib.Common.LogHelper Log;
        [STAThread]
        static void Main(string[] args)
        {
            Log = new ElectricPowerLib.Common.LogHelper("error.log");

            //test code start
            //using (StreamWriter sw = new StreamWriter(".\\test.txt", false, Encoding.UTF8))
            //{
            //    for (int i = 0; i < 256; i++) sw.Write("X");

            //    sw.Close();
            //}
            // test code end

            /* // no ui
            AsyncSocketServer server = new AsyncSocketServer(8088, 8000);
            server.Start();

            //Thread.CurrentThread.IsBackground = false;
            System.Console.ReadLine();
            */

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UI());
        }
    }
}
