using System;
using System.Collections.Generic;
//using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace AbtoSoftPhone
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.Run(new Form1());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)//Manage Unhandeled Exceptions
        {
            //GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), e.Exception, false, true);
             MessageBox.Show(e.Exception.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            //GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), (e.ExceptionObject as Exception), false, true);
            MessageBox.Show((e.ExceptionObject as Exception).Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
