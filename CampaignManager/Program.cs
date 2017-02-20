using System;
using System.Windows.Forms;
using System.Threading;

namespace GCC
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.Run(new frmMain());
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)//Manage Unhandeled Exceptions
        {
            GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  e.Exception, false,true);
           // MessageBoxEx.Show(e.Exception.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            
            GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  (e.ExceptionObject as Exception), false,true);
            //MessageBoxEx.Show((e.ExceptionObject as Exception).Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
