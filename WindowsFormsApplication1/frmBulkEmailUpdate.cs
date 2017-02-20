using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmBulkEmailUpdate : DevComponents.DotNetBar.Office2007Form
    {
        public frmBulkEmailUpdate()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 5000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 15);
        }

        private List<string> _lstDomain = new List<string>();
        public List<string> lstDomain /////List of Domains
        {
            get { return _lstDomain; }
            set { _lstDomain = value; }
        }

        private bool _IsEmailUpdated;
        public bool IsEmailUpdated 
        {
            get { return _IsEmailUpdated; }
            set { _IsEmailUpdated = value; }
        }

        private List<string> _lstContactNameSplit = new List<string>();
        public List<string> lstContactNameSplit /////Split of Name part
        {
            get { return _lstContactNameSplit; }
            set { _lstContactNameSplit = value; }
        }

        private string _sEmail = string.Empty;
        public string sEmail /////Email of selected contact
        {
            get { return _sEmail; }
            set { _sEmail = value; }
        }

        private List<int> _lstFreezedContacts = new List<int>();
        public List<int> lstFreezedContacts /////List of freezed contacts
        {
            get { return _lstFreezedContacts; }
            set { _lstFreezedContacts = value; }
        }

        private string _sEmailSyntax = string.Empty;
        public string sEmailSyntax /////Email of selected contact
        {
            get { return _sEmailSyntax; }
            set { _sEmailSyntax = value; }
        }

        private DataTable _dtContact = new DataTable();
        public DataTable dtContact //// Master Contact with db synch
        {
            get { return _dtContact; }
            set { _dtContact = value; }
        }

        private DataTable _dtEmailSugg = new DataTable();
        public DataTable dtEmailSugg /////Sugg for dropdown
        {
            get { return _dtEmailSugg; }
            set { _dtEmailSugg = value; }
        }

        CheckBox chkHeader; //Dgv header check box

        private void frmBulkEmailUpdate_Load(object sender, EventArgs e)
        {
            IsEmailUpdated = false; // By default
            dgvBulkEmailUpdate.DataSource = dtContact;
            dgvBulkEmailUpdate.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            CheckBoxColumn();
            Load_ComboBoxes();

            chkUpdateOnlyBlank.Checked = true; //Checkedstate event ll fire
            ShowOnlyEmptyEmail();

            txtEmail.Text = sEmail;
            txtFirstName.Text = lstContactNameSplit[0];
            txtMiddleName.Text = lstContactNameSplit[1];
            txtLastName.Text = lstContactNameSplit[2];
            txtFirstNameFirstLetter.Text = lstContactNameSplit[3];
            txtMiddleNameFirstLetter.Text = lstContactNameSplit[4];
            txtLastNameFirstLetter.Text = lstContactNameSplit[5];

            cmbEmailFormat.Text = sEmailSyntax;
            if (sEmailSyntax.Length == 0)
            {
                //ToastNotification.Show(this, "Email format cannot be detected. Please choose email format.",eToastPosition.BottomCenter);
                lblEmailFormat.Text = "Choose Format";
                this.TitleText = "Bulk Email Update <font color = 'red'>(Please choose email format.)</font>";
                cmbEmailFormat.Focus();
            }

            chkHeader.Checked = true;
            cmbDomainlist.SelectedIndex = 0;
        }

        private void Load_ComboBoxes()
        {
            try
            {
                foreach (string sDomain in lstDomain)//Load list of Domains found
                    cmbDomainlist.Items.Add(sDomain);

                DataTable dtNewEmailSugg = new DataTable();// to display combo box with example
                dtNewEmailSugg.Columns.Add("Format", typeof(string));
                dtNewEmailSugg.Columns.Add("Example", typeof(string));

                foreach (DataRow dr in dtEmailSugg.Rows)
                {
                    DataRow drNewRow = dtNewEmailSugg.NewRow();
                    string sEmailFormation = string.Empty;
                    sEmailFormation = dr["PicklistValue"].ToString().Replace("FirstName", lstContactNameSplit[0]);
                    sEmailFormation = sEmailFormation.Replace("LastName", lstContactNameSplit[2]);
                    sEmailFormation = sEmailFormation.Replace("FName", lstContactNameSplit[3]);
                    sEmailFormation = sEmailFormation.Replace("LName", lstContactNameSplit[5]);
                    sEmailFormation = sEmailFormation.Replace("MiddleName", lstContactNameSplit[1]);
                    sEmailFormation = sEmailFormation.Replace("MName", lstContactNameSplit[4]);

                    drNewRow["Format"] = dr["PicklistValue"];
                    drNewRow["Example"] = sEmailFormation;
                    dtNewEmailSugg.Rows.Add(drNewRow);
                }
                cmbEmailFormat.DataSource = dtNewEmailSugg;
                cmbEmailFormat.DisplayMember = "Format";
                cmbEmailFormat.ValueMember = "Example";
            }
            catch (Exception ex)
            {

                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void CheckBoxColumn()
        {
            try
            {
                DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn(); /////CheckBox Column/////
                chkColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                chkColumn.Name = "Select"; //////Name of the Checkbox Column "SELECT"/////
                chkColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                chkColumn.HeaderText = "  ";
                chkColumn.Width = 30;
                chkColumn.DisplayIndex = 0;
                chkColumn.ReadOnly = true;
                chkColumn.FillWeight = 30;
                dgvBulkEmailUpdate.Columns.Add(chkColumn);

                Rectangle rect = dgvBulkEmailUpdate.GetCellDisplayRectangle(dgvBulkEmailUpdate.Columns["Select"].Index, -1, true);
                rect.X = rect.Location.X + (rect.Width / 4) - 2;// set checkbox header to center of header cell. +1 pixel to positison correctly.
                chkHeader = new CheckBox();
                chkHeader.BackColor = Color.Transparent;
                chkHeader.Name = "checkboxHeader";
                chkHeader.Size = new Size(18, 18);
                chkHeader.Location = rect.Location;
                chkHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);
                dgvBulkEmailUpdate.Controls.Add(chkHeader);


                //// Creating checkbox without panel
                //CheckBox checkbox = new CheckBox();
                //checkbox.Size = new System.Drawing.Size(15, 15);
                //checkbox.BackColor = Color.Transparent;

                //// Reset properties
                //checkbox.Padding = new System.Windows.Forms.Padding(0);
                //checkbox.Margin = new System.Windows.Forms.Padding(0);
                //checkbox.Text = "";

                //// Add checkbox to datagrid cell
                //dgvBulkEmailUpdate.Controls.Add(checkbox);
                //DataGridViewHeaderCell header = dgvBulkEmailUpdate.Columns["Select"].HeaderCell;
                //checkbox.Location = new Point(
                //    header.ContentBounds.Left + (header.ContentBounds.Right - header.ContentBounds.Left + checkbox.Size.Width) / 2,
                //    header.ContentBounds.Top + (header.ContentBounds.Bottom - header.ContentBounds.Top + checkbox.Size.Height) / 2
                //);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = sender as CheckBox;
                foreach (DataGridViewRow dgvr in dgvBulkEmailUpdate.Rows)
                {
                    if (dgvr.Visible)
                    {
                        dgvr.Cells["Select"].Value = chk.Checked;
                    }
                }

                dgvCheckBoxHeadervalue();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvCheckBoxHeadervalue()
        {
            try
            {
                List<string> lst = new List<string>();
                foreach (DataGridViewRow dgvr in dgvBulkEmailUpdate.Rows)
                {
                    if (dgvr.Visible)
                    {
                        if (dgvr.Cells["Select"].Value == null)
                            lst.Add("False");
                        else
                            lst.Add(dgvr.Cells["Select"].Value.ToString());
                    }
                }

                chkHeader.CheckedChanged -= new EventHandler(checkboxHeader_CheckedChanged);

                if (lst.Distinct().Count() == 1)
                {
                    if (lst[0] == "True")
                        chkHeader.Checked = true;
                    else
                        chkHeader.Checked = false;
                }
                else if (lst.Distinct().Count() > 1)
                    chkHeader.CheckState = CheckState.Indeterminate;
                else
                    chkHeader.Checked = false;

                chkHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvBulkEmailUpdate_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvBulkEmailUpdate != null && dgvBulkEmailUpdate.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvBulkEmailUpdate.Columns.Count; i++)
                    {
                        if (dgvBulkEmailUpdate.Columns[i].Name.ToUpper() == "TITLE" || dgvBulkEmailUpdate.Columns[i].Name.ToUpper() == "FIRST_NAME" || dgvBulkEmailUpdate.Columns[i].Name.ToUpper() == "LAST_NAME" || dgvBulkEmailUpdate.Columns[i].Name.ToUpper() == "CONTACT_EMAIL")
                        {
                            dgvBulkEmailUpdate.Columns[i].Visible = true;
                            dgvBulkEmailUpdate.Columns[i].HeaderText = GM.ProperCase_ProjectSpecific(dgvBulkEmailUpdate.Columns[i].Name.Replace('_',' '));
                        }
                        else
                            dgvBulkEmailUpdate.Columns[i].Visible = false;

                        dgvBulkEmailUpdate.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }

                if (GV.sAccessTo == "TR")
                {
                    dgvBulkEmailUpdate.Columns["TR_CONTACT_STATUS"].Visible = true;
                    dgvBulkEmailUpdate.Columns["TR_CONTACT_STATUS"].HeaderText = "Contact Status";
                    dgvBulkEmailUpdate.Columns["WR_CONTACT_STATUS"].Visible = false;
                }
                else if (GV.sAccessTo == "WR")
                {
                    dgvBulkEmailUpdate.Columns["TR_CONTACT_STATUS"].Visible = false;
                    dgvBulkEmailUpdate.Columns["WR_CONTACT_STATUS"].Visible = true;
                    dgvBulkEmailUpdate.Columns["WR_CONTACT_STATUS"].HeaderText = "Contact Status";
                }
                dgvBulkEmailUpdate.Columns["CONTACT_EMAIL"].DisplayIndex = 0;

            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmbEmailFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEmailFormation = string.Empty;
            sEmailFormation = cmbEmailFormat.Text.Replace("FirstName", lstContactNameSplit[0]);
            sEmailFormation = sEmailFormation.Replace("LastName", lstContactNameSplit[2]);
            sEmailFormation = sEmailFormation.Replace("FName", lstContactNameSplit[3]);
            sEmailFormation = sEmailFormation.Replace("LName", lstContactNameSplit[5]);
            sEmailFormation = sEmailFormation.Replace("MiddleName", lstContactNameSplit[1]);
            sEmailFormation = sEmailFormation.Replace("MName", lstContactNameSplit[4]);
            sEmailFormation += "@"+cmbDomainlist.Text;
            lblEmailOut.Text = sEmailFormation;
        }

        private void lblEmailOut_TextChanged(object sender, EventArgs e)
        {
            if (!GM.Email_Check(lblEmailOut.Text))
                lblInvalid.Visible = true;
            else
                lblInvalid.Visible = false;
        }

        private void dgvBulkEmailUpdate_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ///////Checkbox Check Toggle/////
                if (e.RowIndex != -1 && dgvBulkEmailUpdate.Columns[e.ColumnIndex].Name == "Select")
                {
                    if (dgvBulkEmailUpdate.Rows[e.RowIndex].Cells["Select"].Value == null)
                        dgvBulkEmailUpdate.Rows[e.RowIndex].Cells["Select"].Value = true;
                    else if (dgvBulkEmailUpdate.Rows[e.RowIndex].Cells["Select"].Value.ToString() == "False")
                        dgvBulkEmailUpdate.Rows[e.RowIndex].Cells["Select"].Value = true;
                    else if (dgvBulkEmailUpdate.Rows[e.RowIndex].Cells["Select"].Value.ToString() == "True")
                        dgvBulkEmailUpdate.Rows[e.RowIndex].Cells["Select"].Value = false;

                    dgvCheckBoxHeadervalue();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvBulkEmailUpdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                dgvBulkEmailUpdate.Columns[1].Selected = true;
                dgvBulkEmailUpdate.CurrentCell = dgvBulkEmailUpdate.Rows[dgvBulkEmailUpdate.CurrentRow.Index].Cells["CONTACT_EMAIL"];

                if (e.KeyChar == 32)//Space
                {
                    ToastNotification.Show(this, "space", eToastPosition.BottomCenter);
                    if (dgvBulkEmailUpdate.Rows[dgvBulkEmailUpdate.CurrentRow.Index].Cells["Select"].Value == null)
                        dgvBulkEmailUpdate.Rows[dgvBulkEmailUpdate.CurrentRow.Index].Cells["Select"].Value = true;
                    else if (dgvBulkEmailUpdate.Rows[dgvBulkEmailUpdate.CurrentRow.Index].Cells["Select"].Value.ToString() == "False")
                        dgvBulkEmailUpdate.Rows[dgvBulkEmailUpdate.CurrentRow.Index].Cells["Select"].Value = true;
                    else if (dgvBulkEmailUpdate.Rows[dgvBulkEmailUpdate.CurrentRow.Index].Cells["Select"].Value.ToString() == "True")
                        dgvBulkEmailUpdate.Rows[dgvBulkEmailUpdate.CurrentRow.Index].Cells["Select"].Value = false;

                    dgvCheckBoxHeadervalue();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void chkUpdateOnlyBlank_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CurrencyManager cm = (CurrencyManager)BindingContext[this.dgvBulkEmailUpdate.DataSource];
                cm.SuspendBinding();
                foreach (DataGridViewRow dgvr in dgvBulkEmailUpdate.Rows)
                {
                    if (chkUpdateOnlyBlank.Checked)
                    {
                        if (dgvr.Cells["CONTACT_EMAIL"].Value.ToString().Length > 0)
                            dgvr.Visible = false;
                        else
                            dgvr.Visible = true;
                    }
                    else
                        dgvr.Visible = true;

                    if (lstFreezedContacts.Contains((int)dgvr.Cells["CONTACT_ID_P"].Value))//Hide Freezed Contacts
                        dgvr.Visible = false;
                }
                cm.ResumeBinding();

                dgvCheckBoxHeadervalue();
            }
            catch (Exception ex)
            {
               ////MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ShowOnlyEmptyEmail()
        {
            try
            {
                CurrencyManager cm = (CurrencyManager)BindingContext[this.dgvBulkEmailUpdate.DataSource];
                cm.SuspendBinding();
                foreach (DataGridViewRow dgvr in dgvBulkEmailUpdate.Rows)
                {
                    if (dgvr.Cells["CONTACT_EMAIL"].Value.ToString().Length > 0)
                        dgvr.Visible = false;
                    else
                        dgvr.Visible = true;

                    if (lstFreezedContacts.Contains((int)dgvr.Cells["CONTACT_ID_P"].Value))//Hide Freezed Contacts
                        dgvr.Visible = false;
                }
                cm.ResumeBinding();

                dgvCheckBoxHeadervalue();
            }
            catch (Exception ex)
            {
                ////MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private List<string> GetContactNameSplit(string sFirstName, string sLastName)
        {
            List<string> lstContactSplit = new List<string>();

            string sMiddleName = string.Empty;

            string sF = string.Empty;//First Letter of firstname
            string sMName = string.Empty;//First Letter of Middle Name
            string sL = string.Empty;//First Letter of Lastname            

            if (sFirstName.Length > 0)//Get First Letter's
            {
                sF = sFirstName[0].ToString().ToLower();
                List<string> lstSplit = sFirstName.Split(' ').ToList();
                if (lstSplit.Count > 1)
                {
                    sFirstName = lstSplit[0];
                    sMiddleName = lstSplit[1].Replace(" ", string.Empty).ToLower();
                    if (sMiddleName.Length > 0)
                        sMName = sMiddleName[0].ToString().ToLower();
                }
            }

            if (sLastName.Length > 0)//Get First Letter's
                sL = sLastName[0].ToString().ToLower();

            sFirstName = sFirstName.Replace(" ", string.Empty).ToLower();//if names contains any space remove it
            sLastName = sLastName.Replace(" ", string.Empty).ToLower();

            lstContactSplit.Add(sFirstName);
            lstContactSplit.Add(sMiddleName);
            lstContactSplit.Add(sLastName);
            lstContactSplit.Add(sF);
            lstContactSplit.Add(sMName);
            lstContactSplit.Add(sL);

            return lstContactSplit;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdateEmail_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show("Are you sure to update email(s) ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    IsEmailUpdated = true;
                    foreach (DataGridViewRow dgvr in dgvBulkEmailUpdate.Rows)
                    {
                        if (dgvr.Visible && dgvr.Cells["Select"].Value != null && dgvr.Cells["Select"].Value.ToString().ToUpper() == "TRUE" && dgvr.Cells["CONTACT_EMAIL"].Value.ToString().Length == 0)
                        {
                            List<string> lstContactSplit = GetContactNameSplit(dgvr.Cells["FIRST_NAME"].Value.ToString(), dgvr.Cells["LAST_NAME"].Value.ToString());
                            string sEmailFormation = string.Empty;
                            sEmailFormation = cmbEmailFormat.Text.Replace("FirstName", lstContactSplit[0]);
                            sEmailFormation = sEmailFormation.Replace("LastName", lstContactSplit[2]);
                            sEmailFormation = sEmailFormation.Replace("FName", lstContactSplit[3]);
                            sEmailFormation = sEmailFormation.Replace("LName", lstContactSplit[5]);
                            sEmailFormation = sEmailFormation.Replace("MiddleName", lstContactSplit[1]);
                            sEmailFormation = sEmailFormation.Replace("MName", lstContactSplit[4]);
                            sEmailFormation += "@" + cmbDomainlist.Text;
                            if (GM.Email_Check(sEmailFormation))
                            {
                                dgvr.Cells["CONTACT_EMAIL"].Value = sEmailFormation;
                                dgvr.Cells["EMAIL_EXTRAPOLATE"].Value = "YES";
                            }
                            else
                                ToastNotification.Show(this, "Not all emails are updated", eToastPosition.TopRight);
                        }

                        ShowOnlyEmptyEmail();
                        //if (dgvr.Cells["CONTACT_EMAIL"].Value.ToString().Length > 0)
                        //    dgvr.Visible = false;
                        //else
                        //    dgvr.Visible = true;

                    }
                }
                catch (Exception ex)
                {
                    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

    }
}
