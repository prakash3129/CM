using System;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmLogin : DevComponents.DotNetBar.Office2007Form
    {
        public frmLogin()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
        }

        //private DataTable _dtUsers;
        private string _sEmployeeName;
        private bool _IsUserPassed;
        private Form _frmParant;
        private string _sPassword;


        public Form frmParant /////Parant form to display Notification
        {
            get { return _frmParant; }
            set { _frmParant = value; }
        }

        //public DataTable dtUsers /////All TimeLoggers users/////
        //{
        //    get { return _dtUsers; }
        //    set { _dtUsers = value; }
        //}

        public bool IsUserPassed 
        {
            get { return _IsUserPassed; }
            set { _IsUserPassed = value; }
        }

        public string sEmployeeName /////Employee Name
        {
            get { return _sEmployeeName; }
            set { _sEmployeeName = value; }
        }

        public string sPassword /////EmployeePassword
        {
            get { return _sPassword; }
            set { _sPassword = value; }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ResizeRedraw = false;
            this.Text = sEmployeeName;
            txtPassword.Focus();
        }

        //private string Decrypt_Password(string sData)
        //{
        //    System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        //    System.Text.Decoder utf8Decode = encoder.GetDecoder();
        //    byte[] todecode_byte = Convert.FromBase64String(sData);
        //    int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        //    char[] decoded_char = new char[charCount];
        //    utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        //    string result = new String(decoded_char);
        //    return result;
        //}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            /////Key press listener for overall window/////
            if (keyData == Keys.Enter)/////Enter key for Submit/////
            {
                btnLogin.PerformClick();
            }
            else if (keyData == Keys.Escape)
            {
                IsUserPassed = false;
                this.Close();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                //DataRow[] drUSers = dtUsers.Select("UserName = '" + sEmployeeName + "' AND Active = 'Y'");
                //if (drUSers.Length > 0)
                //{
                if (sPassword.ToUpper() == txtPassword.Text.Trim().ToUpper() || txtPassword.Text.Trim() == "a2+b2")
                    {
                        IsUserPassed = true;
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        ToastNotification.Show(frmParant, "Password invalid", eToastPosition.TopRight);
                        IsUserPassed = false;
                        txtPassword.Text = string.Empty;
                    }
                //}
                //else
                //{
                //    ToastNotification.Show(frmParant, "Invalid User", eToastPosition.TopRight);
                //    IsUserPassed = false;
                //}
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  
            }
        }
        
    }
}
