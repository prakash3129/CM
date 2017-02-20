using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GCC
{
    public partial class frmCallScript : DevComponents.DotNetBar.Office2007Form
    {
        public frmCallScript()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
        }

        private DataTable _dtCompany;
        public DataTable dtCompany
        {
            get { return _dtCompany; }
            set { _dtCompany = value; }
        }

        private DataTable _dtContact;
        public DataTable dtContact
        {
            get { return _dtContact; }
            set { _dtContact = value; }
        }

        private Form _frmMDI;
        public Form frmMDI
        {
            get { return _frmMDI; }
            set { _frmMDI = value; }
        }

        private bool _IsOutofTimeZone;
        public bool IsOutofTimeZone
        {
            get { return _IsOutofTimeZone; }
            set { _IsOutofTimeZone = value; }
        }

        private Form _frmContactUpdate;
        public Form frmContactUpdate
        {
            get { return _frmContactUpdate; }
            set { _frmContactUpdate = value; }
        }

        private int _iCurIndex;
        public int iCurIndex
        {
            get { return _iCurIndex; }
            set { _iCurIndex = value; }
        }

        int iMDIParentHeight;

        private void frmCallScript_Load(object sender, EventArgs e)
        {
            try
            {
                btnDialCallScript.Click += ((FrmContactsUpdate)frmContactUpdate).btnDial_Click;
                iMDIParentHeight = frmMDI.Height-10;
                rtxtCallScript.Rtf = GM.LoadRTF(dtContact, dtCompany, iCurIndex);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //public string LoadRTF(DataTable dtContact)
        //{
        //    string sRTF = string.Empty;
        //    RichTextBox txtrtf = new RichTextBox();
        //    txtrtf.LoadFile(GlobalVariables.sCallScriptPath);
        //    sRTF = txtrtf.Rtf;

        //    string sFirstName = string.Empty;
        //    string sLastName = string.Empty;
        //    string sJobTitle = string.Empty;
        //    string sEmail = string.Empty;

        //    int i = 0;
        //    while (true)
        //    {
        //        try
        //        {
        //            sFirstName = dtContact.Rows[i]["FIRST_NAME"].ToString();
        //            sLastName = dtContact.Rows[i]["LAST_NAME"].ToString();
        //            sJobTitle = dtContact.Rows[i]["JOB_TITLE"].ToString();
        //            sEmail = dtContact.Rows[i]["CONTACT_EMAIL"].ToString();
        //        }
        //        catch (Exception ex)
        //        {
        //            sFirstName = string.Empty;
        //            sLastName = string.Empty;
        //            sJobTitle = string.Empty;
        //            sEmail = string.Empty;
        //        }

        //        if (sFirstName.Length > 0 || sLastName.Length > 0)
        //            sRTF = sRTF.Replace("<Name" + i + ">", sFirstName + " " + sLastName);
        //        else
        //            sRTF = sRTF.Replace("Name: <Name" + i + ">", string.Empty);

        //        if (sJobTitle.Length > 0)
        //            sRTF = sRTF.Replace("<Jobtitle" + i + ">", sJobTitle);
        //        else
        //            sRTF = sRTF.Replace("Job Title: <Jobtitle" + i + ">", string.Empty);
        //        if (sEmail.Length > 0)
        //            sRTF = sRTF.Replace("<Email" + i + ">", sEmail);
        //        else
        //            sRTF = sRTF.Replace("Email: <Email" + i + ">", string.Empty);

        //        //sContact += "Name: " + dr["TITLE"]+" "+ dr["FIRST_NAME"] + " " + dr["LAST_NAME"] + Environment.NewLine;
        //        //sContact += "Job Title: " + dr["JOB_TITLE"] + Environment.NewLine;
        //        //sContact += "Email: " + dr["CONTACT_EMAIL"] + Environment.NewLine + Environment.NewLine;

        //        i++;
        //        if (i == 2)
        //            break;
        //    }


        //    sRTF = sRTF.Replace("<AgentName>", GlobalVariables.sEmployeeName);
        //    sRTF = sRTF.Replace("<CompanyName>", dtCompany.Rows[0]["COMPANY_NAME"].ToString());
        //    sRTF = sRTF.Replace("<Address1>", dtCompany.Rows[0]["ADDRESS_1"].ToString());
        //    sRTF = sRTF.Replace("<Address2>", dtCompany.Rows[0]["ADDRESS_2"].ToString());
        //    sRTF = sRTF.Replace("<Address3>", dtCompany.Rows[0]["ADDRESS_3"].ToString());
        //    sRTF = sRTF.Replace("<City>", dtCompany.Rows[0]["CITY"].ToString());
        //    return sRTF;
        //}

        private void btnResizeWindow_Click(object sender, EventArgs e)
        {
            try
            {
                int iScreenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                int iCallScriptScreenWidth = Screen.PrimaryScreen.WorkingArea.Width / 4;
                frmMDI.WindowState = FormWindowState.Normal;
                frmMDI.Height = iMDIParentHeight;// -10;
                frmMDI.Width = iScreenWidth - iCallScriptScreenWidth;
                this.Height = iMDIParentHeight;// -15;
                this.Width = iCallScriptScreenWidth;
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(frmMDI.Width, frmMDI.Location.Y);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void btnDialCallScript_Click(object sender, EventArgs e)
        //{

        //}
    }
}
