using System;
using System.Drawing;
using System.Windows.Forms;
using System.Deployment.Application;
using System.Runtime.InteropServices;

namespace GCC
{
    public partial class frmSplash : Form
    {
        Timer tOpacity = new Timer();
        public frmSplash()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            //this.TransparencyKey = Color.White;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

        }

        bool Opening = true;
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
       (
           int nLeftRect, // x-coordinate of upper-left corner
           int nTopRect, // y-coordinate of upper-left corner
           int nRightRect, // x-coordinate of lower-right corner
           int nBottomRect, // y-coordinate of lower-right corner
           int nWidthEllipse, // height of ellipse
           int nHeightEllipse // width of ellipse
       );

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if ((int)m.Result == HTCLIENT)
                    {
                        m.Result = (IntPtr)HTCAPTION;
                    }
                    return;
            }

            base.WndProc(ref m);
        }

        private void TOpacity_Tick(object sender, EventArgs e)
        {
            if (Opening)
                this.Opacity += 0.07;
            else
            {
                if (this.Opacity > 0.01)
                    this.Opacity = this.Opacity - 0.07;
                else
                    this.Close();
            }
        }

        Timer t = new Timer();
        int i = 0;

        private void frmSplash_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.TopLevel = true;

            //tOpacity.Interval = 100;
            tOpacity.Tick += new EventHandler(TOpacity_Tick);
            tOpacity.Enabled = true;
            this.Opacity = 0.0;
            
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                System.Deployment.Application.ApplicationDeployment appDeployment = ApplicationDeployment.CurrentDeployment;

                string sVersion = "v" + appDeployment.CurrentVersion.Major + "." + appDeployment.CurrentVersion.Minor + "." + appDeployment.CurrentVersion.Build + " Build " + appDeployment.CurrentVersion.Revision;
                lblVersion.Text = sVersion;
            }
            //var thisApp = Assembly.GetExecutingAssembly();
            //AssemblyName name = new AssemblyName(thisApp.FullName);
            //string VersionNumber = "v" + name.Version;
            //lblVersion.Text = VersionNumber;

            t.Tick += new EventHandler(Timer_Tick);
            t.Interval = 1000;
            t.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            i++;
            if (i == 4)
            {
                t.Stop();
                this.Close();
            }
        }

        private void frmSplash_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;

            if (this.Opacity > 0.01f)
            {
                Opening = false;
                e.Cancel = true;
                //tOpacity.Interval = 50;
                tOpacity.Enabled = true;
            }
            else
                tOpacity.Enabled = false;
        }




    }
}
