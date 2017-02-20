using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmNewPickListCategory : DevComponents.DotNetBar.Office2007Form
    {
        public frmNewPickListCategory()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); //Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
        }

        private List<string> _lstCategory = new List<string>();
        public List<string> lstCategory
        {
            get { return _lstCategory; }
            set { _lstCategory = value; }
        }

        private void frmNewPickListCategory_Load(object sender, EventArgs e)
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtCategoryName.Text.Trim().Length > 0)
            {
                if (lstCategory.Contains(txtCategoryName.Text.ToUpper()))
                {
                    ToastNotification.Show(this.Owner, "Category already exist.");
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
