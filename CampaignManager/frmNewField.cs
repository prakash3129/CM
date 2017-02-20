using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmNewField : DevComponents.DotNetBar.Office2007Form
    {
        public frmNewField()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); //Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
        }

        private List<string> _lstFields = new List<string>();
        public List<string> lstFields 
        {
            get { return _lstFields; }
            set { _lstFields = value; }
        }

        private void frmNewField_Load(object sender, EventArgs e)
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            cmbFieldType.Items.Add("Text");
            cmbFieldType.Items.Add("Number");
            cmbFieldType.Items.Add("Decimal Number");
            cmbFieldType.Items.Add("Date and Time");
        }

        private void txtFieldName_TextChanged(object sender, EventArgs e)
        {
            if (lstFields.Contains(txtFieldName.Text.Replace(" ",string.Empty).ToUpper()) || lstFields.Contains(txtFieldName.Text.Replace(" ","_").ToUpper()))
            {
                ToastNotification.Show(this.Owner, "Column already Exist."+Environment.NewLine+"Try different Name.");
            }
        }

        private void cmbFieldType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtFieldSize.Enabled = cmbFieldType.Text == "Text";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(cmbFieldType.Text.Trim().Length > 0 && txtFieldName.Text.Trim().Length > 0)
            {
                if (lstFields.Contains(txtFieldName.Text.Replace(" ", string.Empty).ToUpper()) || lstFields.Contains(txtFieldName.Text.Replace(" ", "_").ToUpper()))
                    ToastNotification.Show(this.Owner, "Column already Exist." + Environment.NewLine + "Try different Name.");
                else
                {
                    if (cmbFieldType.Text == "Text")
                    {
                        //if (txtFieldSize.Value < 1)
                        //{
                        //    ToastNotification.Show(this.Owner, "Field Size cannot be emnpty");
                        //    return;
                        //}
                    }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Close();                    
                }
            }
        }

        private void frmNewField_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (lstFields.Contains(txtFieldName.Text.Replace(" ", string.Empty)) || lstFields.Contains(txtFieldName.Text.Replace(" ", "_")))
            //{
            //    ToastNotification.Show(this.Parent, "Column already Exist." + Environment.NewLine + "Try different Name.");
            //    e.Cancel = true;
            //}
        }
    }
}
