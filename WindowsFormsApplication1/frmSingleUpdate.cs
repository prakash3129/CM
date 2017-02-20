using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;


namespace GCC
{
    public partial class frmSingleUpdate : Office2007Form
    {
        public frmSingleUpdate()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(Font.FontFamily, 22);
        }

        DataTable _dtContactFieldMaster;
        public DataTable dtContactFieldMaster
        {
            get { return _dtContactFieldMaster; }
            set { _dtContactFieldMaster = value; }
        }

        DataTable _dtContacts;
        public DataTable dtContacts
        {
            get { return _dtContacts; }
            set { _dtContacts = value; }
        }        

        bool _IsUpdated;
        public bool IsUpdated
        {
            get { return _IsUpdated; }
            set { _IsUpdated = value; }
        }

        private void frmSingleUpdate_Load(object sender, EventArgs e)
        {

        }
    }
}
