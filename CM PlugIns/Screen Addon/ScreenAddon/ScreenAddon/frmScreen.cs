using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace ScreenAddon
{
    public partial class frmScreen : Office2007Form
    {

        string sHost = string.Empty;
        int iPort = 1;
        string sPassword = string.Empty;
        public frmScreen(string[] args)
        {
            InitializeComponent();
            vncControl.AutoScroll = true;
            this.AutoScroll = true;

            this.HorizontalScroll.Visible = true;
            this.VerticalScroll.Visible = true;

            sHost = "172.27.143.254";
            iPort = 44567;
            sPassword = "Pr@k@sH";
            return;


            //sHost = args[0].Trim();
            //iPort = Convert.ToInt32(args[1].Trim());
            //sPassword = args[2];

            this.Text = sHost + "@" + iPort;
        }

        private void frmScreen_Load(object sender, EventArgs e)
        {



            //vncControl.AllowInput = true;
            vncControl.AllowRemoteCursor = false;
            vncControl.Capture = true;
            vncControl.Cursor = Cursors.Default;

            // return;

            var options = new RemoteViewing.Vnc.VncClientConnectOptions();
            if (sPassword != "") { options.Password = sPassword.ToCharArray(); }

            try
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    try
                    {                        
                        vncControl.Client.Connect(sHost, iPort, options);
                    }
                    finally { Cursor = Cursors.Default; }
                }
                catch (RemoteViewing.Vnc.VncException ex)
                {
                    MessageBoxEx.Show(this, "Connection failed (" + ex.Reason.ToString() + ").", "Connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (SocketException ex)
                {
                    MessageBoxEx.Show(this, "Connection failed (" + ex.SocketErrorCode.ToString() + ").", "Connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                vncControl.Focus();

                this.Height = vncControl.Client.Framebuffer.Height;
                this.Width = vncControl.Client.Framebuffer.Width;

                

            }
            finally
            {
                if (options.Password != null)
                {
                    Array.Clear(options.Password, 0, options.Password.Length);
                }
            }


            SeeThroughPanel x = new SeeThroughPanel();

            x.Parent = vncControl;

            x.Dock = DockStyle.Fill;







        }

        private void frmScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (vncControl.Client.IsConnected)
                    vncControl.Client.Close();
            }
            catch(Exception ex)
            { }
        }

    
    }



    internal class SeeThroughPanel : Panel
    {

        public SeeThroughPanel()
        {
        }

        protected void TickHandler(object sender, EventArgs e)
        {
            this.InvalidateEx();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT

                return cp;
            }
        }

        protected void InvalidateEx()
        {
            if (Parent == null)
            {
                return;
            }

            Rectangle rc = new Rectangle(this.Location, this.Size);

            Parent.Invalidate(rc, true);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {

        }

        private Random r = new Random();

        protected override void OnPaint(PaintEventArgs e)
        {

            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0, 0)), this.ClientRectangle);
        }
    }
}
