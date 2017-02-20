using System;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmRejectReason : DevComponents.DotNetBar.Office2007Form
    {
        public frmRejectReason()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); //Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
            MessageBoxEx.EnableGlass = false;
        }

        string _sReason = string.Empty;
        public string sReason
        {
            get { return _sReason; }
            set { _sReason = value; }
        }

        Form _frmParant;
        public Form frmParant /////Parant form to display Notification
        {
            get { return _frmParant; }
            set { _frmParant = value; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (txtFeedback.Text.Trim().Length > 0)
            {
                sReason = txtFeedback.Text.Trim();
                btnReject.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
                ToastNotification.Show(frmParant, "Enter Reject Reason", eToastPosition.TopRight);
        }
    }
}
