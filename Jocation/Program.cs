using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocationCleaned
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new frmMain());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"软件发生异常! 请将问题反馈给我!{ex.Message}");
#if !DEBUG
                Process.Start("https://www.cnblogs.com/jonneydong/p/9998324.html");
#endif
            }
        }
    }
}
