using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    static class Program
    {
        public static double dSensorK = 0.8;
        public static double dSensorB = 100;


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            HardwareOpt.InitCard();
            Application.Run(new FrmMain());
        }
    }
}
