using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmValidation_Step3 : Office2007Form
    {
        public frmValidation_Step3()
        {
            InitializeComponent();

            
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
        }

        DataTable _sValidation = new DataTable();
        public DataTable dtValidation 
        {
            get { return _sValidation; }
            set { _sValidation = value; }
        }

        private void frmValidation_Step3_Load(object sender, EventArgs e)
        {

        }
    }
}
