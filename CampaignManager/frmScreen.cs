using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmScreen : DevComponents.DotNetBar.Office2007Form
    {
        public frmScreen()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new System.Drawing.Font(this.Font.FontFamily, 22);
            MessageBoxEx.EnableGlass = false;
            
        }


        private string _IP;        
        public string IP 
        {
            get { return _IP; }
            set { _IP = value; }
        }

        private string _Agent;
        public string Agent
        {
            get { return _Agent; }
            set { _Agent = value; }
        }

        private string _Project;
        public string Project
        {
            get { return _Project; }
            set { _Project = value; }
        }

        private string _Port;
        public string Port
        {
            get { return _Port; }
            set { _Port = value; }
        }

        private void frmScreen_Load(object sender, EventArgs e)
        {
            //vncControl.AllowInput = true;
            //vncControl.AllowRemoteCursor = true;
            //vncControl.Capture = true;
            //vncControl.Cursor = Cursors.Default;

            this.Text = IP + ":" + Port + " | " + Agent + " | " + Project;

            StartSession();
        }

        public string StartSession()
        {
            //vncControl.AllowInput = true;
            vncControl.AllowRemoteCursor = true;
            //vncControl.Capture = true;
            //vncControl.Cursor = Cursors.Default;
            var options = new RemoteViewing.Vnc.VncClientConnectOptions();                        
            options.Password = "Pr@k@sH".ToCharArray();
            if (Port.Length > 0)
            { }
            int iPort = Convert.ToInt32(Port);

            try
            {
                try
                {
                  //  Cursor = Cursors.WaitCursor;
                    try
                    {
                        if (!this.IsHandleCreated)
                            this.CreateControl();

                        vncControl.Client.Connect(IP, iPort, options);
                        
                    }
                    finally {// Cursor = Cursors.Default;
                    }
                }
                catch (RemoteViewing.Vnc.VncException ex)
                {

                    return "Connection failed (" + ex.Reason.ToString() + ").";                    
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    return "Connection failed (" + ex.SocketErrorCode.ToString() + ").";
                 
                }
                vncControl.Focus();
            }
            finally
            {
                if (options.Password != null)
                {
                    Array.Clear(options.Password, 0, options.Password.Length);
                }
            }        
            return string.Empty;

            
        }

        private void frmScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            vncControl.Client.Close();
            vncControl.Dispose();
            vncControl = null;
        }

        private void frmScreen_Shown(object sender, EventArgs e)
        {
            StartSession();
        }
    }
}
