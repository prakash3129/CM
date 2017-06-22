using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DevComponents.DotNetBar;


namespace GCC
{
    public partial class frmAddNewCompany : DevComponents.DotNetBar.Office2007Form
    {
        public frmAddNewCompany()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
            MessageBoxEx.EnableGlass = false;
        }

        private string _sCompanyName;

        public string sCompanyName /////New Company Name
        {
            get { return _sCompanyName; }
            set { _sCompanyName = value; }
        }

        //BAL_GlobalMySfdQL objBALGlobalMyfdSQL = new BAL_GlobalMfdySQL();
        DataTable dtCompany = new DataTable();
        Regex rNumeric = new Regex(@"[^\d]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private void frmAddNewCompany_Load(object sender, EventArgs e)
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            cmbSearchin.Items.Add("COMPANY_NAME");
            cmbSearchin.Items.Add("ADDRESS_1");
            cmbSearchin.Items.Add("ADDRESS_2");
            cmbSearchin.Items.Add("SWITCHBOARD");
            cmbSearchin.SelectedIndex = 0;
            //dtCompany = GV.MYsdfSQL.BAL_ExecuteQueryMyfdSQL("SELECT MASTER_ID,COMPANY_NAME,ADDRESS_1,ADDRESS_2,CITY,COUNTRY,REPLACE(SWITCHBOARD,' ','') as SWITCHBOARD FROM " + GV.sCompanyTable + ";");
            
            //foreach (DataRow dr in dtCompany.Rows)//Remove empty in Telephone number.. Useful in searching
            //    dr["SWITCHBOARD"] = dr["SWITCHBOARD"].ToString().Replace(" ","");
            //dtCompany.AcceptChanges();
        }

        private void txtCompanyName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (txtCompanyName.Text.Length > 0)
                //{                    
                //    if (cmbSearchin.Text.Length > 0)
                //    {
                //        string sSearchText = txtCompanyName.Text.Replace("'", "''").Replace("[", string.Empty).Replace("]", string.Empty).Replace("%", string.Empty).Replace("*", string.Empty);
                //        if (cmbSearchin.Text == "SWITCHBOARD")
                //            sSearchText = sSearchText.Replace(" ", string.Empty);
                //        DataRow[] drrSearch = dtCompany.Select(cmbSearchin.Text + " LIKE '%" + sSearchText + "%'");
                //        if (drrSearch.Length > 0)
                //            dgvCompanyList.DataSource = drrSearch.CopyToDataTable();
                //        else
                //            dgvCompanyList.DataSource = null;
                //    }
                //    else
                //        ToastNotification.Show(this, "Select column to be searched", eToastPosition.TopRight);
                //}
                //else
                //    dgvCompanyList.DataSource = null;

                btnCreate.Text = "Search";

            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvCompanyList_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn dgvc in dgvCompanyList.Columns)
            {
                if (dgvc.Name == "MASTER_ID" || dgvc.Name == "COMPANY_NAME" || dgvc.Name == "ADDRESS_1" || dgvc.Name == "ADDRESS_2" || dgvc.Name == "CITY" || dgvc.Name == "COUNTRY" || dgvc.Name == "SWITCHBOARD")
                {
                    dgvc.HeaderText = GM.ProperCase_ProjectSpecific(dgvc.Name.Replace("_", " "));
                    dgvc.Visible = true;
                }
                else
                    dgvc.Visible = false;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string sButtonName = btnCreate.Text;
                if (sButtonName == "Create New Company")
                {
                    sCompanyName =
                        GM.RemoveEndBackSlash(
                            txtCompanyName.Text.Trim()
                                .Replace("'", "''")
                                .Replace("[", "")
                                .Replace("]", "")
                                .Replace("%", string.Empty));

                    if (sCompanyName.Length > 0)
                    {
                        if (dgvCompanyList.DataSource != null && dgvCompanyList.Rows.Count > 0)
                        {
                            if (
                                MessageBoxEx.Show("Duplicate entries found. Do you still wish to continue ?",
                                    "Campaign Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                                DialogResult.Yes)
                            {
                                DialogResult = DialogResult.OK;
                                Close();
                            }
                        }
                        else
                        {
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                    }
                    else
                        ToastNotification.Show(this, "Company Name cannot be empty", eToastPosition.TopRight);
                }
                else
                {
                    if (txtCompanyName.Text.Trim().Length > 0)
                    {
                        //txtCompanyName.Text = txtCompanyName.Text.Replace("'", "''").Replace("[", "").Replace("]", "");
                        if (cmbSearchin.Text.Length > 0)
                        {
                            string sSearchText = txtCompanyName.Text.Replace("'", "''").Replace("[", string.Empty).Replace("]", string.Empty).Replace("%", string.Empty).Replace("*", string.Empty);

                            if (cmbSearchin.Text == "SWITCHBOARD")
                            {
                                //string sSearchText = txtCompanyName.Text;
                                sSearchText = rNumeric.Replace(sSearchText, string.Empty);
                                if (sSearchText.Length > 7)                                
                                    sSearchText = sSearchText.Substring(sSearchText.Length - 8);                                                                    
                                else
                                {
                                    ToastNotification.Show(this, "Invalid Switchboard!", eToastPosition.TopRight);
                                    return;
                                }

                                if(dtCompany != null)//Clear Memory if exist
                                    dtCompany.Rows.Clear();
                                
                                dtCompany =
                                    GV.MSSQL1.BAL_ExecuteQuery(
                                        "SELECT MASTER_ID,COMPANY_NAME,ADDRESS_1,ADDRESS_2,CITY,COUNTRY,REPLACE(SWITCHBOARD,' ','') as SWITCHBOARD FROM " +
                                        GV.sCompanyTable + " WHERE SWITCHBOARD_TRIMMED = '" + sSearchText + "';");
                            }
                            else
                            {
                                if (dtCompany != null)
                                    dtCompany.Rows.Clear();

                                dtCompany =
                                    GV.MSSQL1.BAL_ExecuteQuery(
                                        "SELECT TOP 100 MASTER_ID,COMPANY_NAME,ADDRESS_1,ADDRESS_2,CITY,COUNTRY,REPLACE(SWITCHBOARD,' ','') as SWITCHBOARD FROM " +
                                        GV.sCompanyTable + " WHERE " + cmbSearchin.Text + " LIKE '%" + sSearchText + "%';");

                                if (dtCompany.Rows.Count == 100)
                                    ToastNotification.Show(this, "Showing only top 100 records.",
                                        eToastPosition.BottomCenter);
                            }

                            dgvCompanyList.DataSource = dtCompany;
                            btnCreate.Text = "Create New Company";
                        }
                        else
                            ToastNotification.Show(this, "Select column to be searched", eToastPosition.TopRight);
                    }
                    else
                        dgvCompanyList.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void cmbSearchin_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (txtCompanyName.Text.Trim().Length > 0)
                //{
                    if (cmbSearchin.Text.Length > 0)
                    {
                        //DataRow[] drrSearch = dtCompany.Select(cmbSearchin.Text + " LIKE '%" + txtCompanyName.Text.Replace("'", "''").Replace("[", string.Empty).Replace("]", string.Empty).Replace("%", string.Empty) + "%'");
                        //if (drrSearch.Length > 0)
                        //    dgvCompanyList.DataSource = drrSearch.CopyToDataTable();
                        //else
                        //    dgvCompanyList.DataSource = null;

                        btnCreate.Text = "Search";
                    }
                    else
                        ToastNotification.Show(this, "Select column to be searched", eToastPosition.TopRight);
                //}
                //else
                //    dgvCompanyList.DataSource = null;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
