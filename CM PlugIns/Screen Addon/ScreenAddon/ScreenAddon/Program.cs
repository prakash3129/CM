using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ScreenAddon
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            
            //if (args.Length == 3)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);                
                Application.Run(new frmScreen(args));
            }
        }
    }
}
