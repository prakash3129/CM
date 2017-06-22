using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmFilterAllocation : DevComponents.DotNetBar.Office2007Form
    {
        public frmFilterAllocation()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 15);
        }
        string sFilterID;
        DataTable dtAllocation_Filter = new DataTable();
        DataTable dtAllocation_Filter_Condition = new DataTable();

        DataTable dtAllocation_Filter_Log = new DataTable();        

        //DataTable dtFilterView = new DataTable();
        DataTable dtCompanies = new DataTable();
        DataTable dtContacts = new DataTable();
        DataTable dtFieldMaster = new DataTable();
        //BAL_GlobalMfdySQL objBAL_Global = new BAL_GlobalMyfdSQL();
        DataTable dtFieldList = new DataTable();
        int iAllocationRowIndex;
        int iConditionRowIndex = -1;
        string sFieldName = string.Empty;
        string sTableName = string.Empty;
        string sUserTypeDateColumn = string.Empty;

        private void frmFilterAllocation_Load(object sender, EventArgs e)
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            if (GV.sAccessTo == "TR")
                sUserTypeDateColumn = "TR_DATECALLED";
            else if (GV.sAccessTo == "WR")
                sUserTypeDateColumn = "WR_DATE_OF_PROCESS";

            dateFrom.Value = dateTo.Value = GM.GetDateTime();             

            dgvAllocationFilter.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
            dgvConditions.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
            dtFieldMaster = GV.MSSQL1.BAL_ExecuteQuery("SELECT CASE WHEN TABLE_NAME = 'Master' THEN 'Company' WHEN TABLE_NAME = 'MasterContacts' THEN 'Contact' ELSE TABLE_NAME END AS 'TABLE_NAME',FIELD_NAME_TABLE FROM c_field_master WHERE PROJECT_ID='" + GV.sProjectID + "' AND SHOW_ON_CRITERIA='Y' ORDER BY TABLE_NAME");
            Load_AllocationGrid_and_Combo();//All Initial loads
            splitGridAndControls.Panel2Collapsed = true;

            if (dgvAllocationFilter.Rows.Count > 0)//Default first Row Selected
            {
                dgvAllocationFilter.CurrentCell = dgvAllocationFilter.Rows[0].Cells["FILTER_NAME"];
                dgvAllocationFilter_CellClick(dgvAllocationFilter, new DataGridViewCellEventArgs(2, 0));
            }
        }

        private void Load_AllocationGrid_and_Combo()
        {
            try
            {
                dtAllocation_Filter = GV.MSSQL1.BAL_FetchTable("C_ALLOCATION_FILTER", "PROJECT_ID = '" + GV.sProjectID + "' AND USERACCESS = '" + GV.sAccessTo + "'");
                dtAllocation_Filter_Log = dtAllocation_Filter.Copy();

                dtCompanies = GV.MSSQL1.BAL_FetchTable(GV.sCompanyTable, "1=0");//Company Table
                dtContacts = GV.MSSQL1.BAL_FetchTable(GV.sContactTable, "1=0");//Contact Table


                dgvAllocationFilter.Columns.Clear(); //Clear extra manual columns on reload
                dgvAllocationFilter.DataSource = dtAllocation_Filter;
                dgvAllocationFilter.Columns["FILTER_NAME"].FillWeight = 30;
                dgvAllocationFilter.Columns["FILTER_DESC"].FillWeight = 60;
                dgvAllocationFilter.Columns["Active"].FillWeight = 6;

                if (GV.sAccessTo == "WR")
                {
                    lblTimeZone.Visible = false;
                    swtchTimezone.Visible = false;
                }

                LoadComboxes(); //load combo box items

                ////Additional manual columns
                //AddGridColumn("Total Records", "RecordCount");//Total Count
                //AddGridColumn("Records Processed", "ProcessedCount");//Processed
                //AddGridColumn("Records Pending", "PendingCount");//Pending
                //if (GlobalVariables.sAccessTo == "TR")
                //    AddGridColumn("Can be Called Now (Time Zone)", "TimeZoneRecords");//Records can be called

                foreach (DataGridViewRow dgvr in dgvAllocationFilter.Rows) //Get counts for Total, Processed, Pending--(Extra column value populate)
                {
                    if (dgvr.Cells["Active"].Value.ToString() == "Y" && dgvr.Cells["SQLTEXT"].Value != null && dgvr.Cells["SQLTEXT"].Value.ToString().Length > 0)
                    {
                        //string sPrefix = "SELECT DISTINCT Company.* FROM " + GlobalVariables.sCompanyTable + " Company LEFT OUTER JOIN " + GlobalVariables.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID";

                        //string sCondition = sPrefix+" WHERE " + dgvr.Cells["SQLTEXT"].Value.ToString().Replace("AND (TIME(DATE_ADD(NOW(), INTERVAL HoursFromGMT HOUR)) BETWEEN '09:00' AND '16:00')", "");//Eliminate Timezone filter to calculate count
                        //sCondition = sCondition.Replace("AND " + sUserTypeDateColumn + " IS NULL", "");
                        //sCondition = sCondition.Replace("AND " + sUserTypeDateColumn + " < NOW()", "");
                        //DataTable dtTimeZoneRecords = GV.MYSdfQL.BAL_ExecuteQueryMydfSQL(sPrefix + " WHERE " + dgvr.Cells["SQLTEXT"].Value.ToString()); //Records can be called
                        //DataTable dtFilterViewTotalCount = GV.MYffdSQL.BAL_ExecuteQueryMySfdQL(sCondition);
                        //DataTable dtFilterViewProcessedCount = GV.MdfYSQL.BAL_ExecuteQueryMyfSQL(sCondition + " AND " + sUserTypeDateColumn + " is NOT NULL");
                        //DataTable dtFilterViewPendingCount = GV.MYSQdfL.BAL_ExecuteQueryMyfSQL(sCondition + " AND " + sUserTypeDateColumn + " is NULL");
                        //dgvr.Cells["ProcessedCount"].Value = dtFilterViewProcessedCount.Rows.Count;
                        //dgvr.Cells["PendingCount"].Value = dtFilterViewPendingCount.Rows.Count;
                        //dgvr.Cells["RecordCount"].Value = dtFilterViewTotalCount.Rows.Count;
                        //if (GlobalVariables.sAccessTo == "TR")
                        //    dgvr.Cells["TimeZoneRecords"].Value = dtTimeZoneRecords.Rows.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void LoadComboxes()
        {
            try
            {
                dtFieldList = new DataTable();
                dtFieldList.Columns.Add("FieldName", typeof(string));
                foreach (string s in GV.lstShowOnCriteriaMasterCompanies) //Get list of columns of Filter view into Combotree
                {
                    DataRow drFieldNameCompany = dtFieldList.NewRow();
                    drFieldNameCompany["FieldName"] = s;
                    dtFieldList.Rows.Add(drFieldNameCompany);
                }

                foreach (string s in GV.lstShowOnCriteriaMasterContacts) //Get list of columns of Filter view into Combotree
                {
                    DataRow drFieldNameContact = dtFieldList.NewRow();
                    drFieldNameContact["FieldName"] = s;
                    dtFieldList.Rows.Add(drFieldNameContact);
                }

                //txtFieldName.DataSource = dtFieldList; //List of columns
                //txtFieldName.DisplayMembers = "FieldName";
                //txtFieldName.ValueMember = "FieldName";

                cmbCondition.Items.Clear();
                cmbCondition.Items.Add("Equals"); //Conditions
                cmbCondition.Items.Add("Contains");
                cmbCondition.Items.Add("Not Equals");
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AddGridColumn(string sHeader, string sName)
        {
            try
            {
                DataGridViewColumn dgvCol = new DataGridViewColumn(); //Add columns to datagrid --(New columns are Disconnected from datasource)
                DataGridViewCell dgvCell = new DataGridViewTextBoxCell();
                dgvCol.CellTemplate = dgvCell;
                dgvCol.HeaderText = sHeader;
                dgvCol.Name = sName;
                dgvAllocationFilter.Columns.Add(dgvCol);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void DateToggle(string sTName, string sFName)
        {
            if ((sTName == "COMPANY" && dtCompanies.Columns[sFName].DataType.Name == "DateTime") || (sTName == "CONTACT" && dtContacts.Columns[sFName].DataType.Name == "DateTime"))
            {

                cmbCondition.Items.Clear();
                cmbCondition.Items.Add("Between"); //Conditions
                cmbCondition.Text = "Between";

                dateFrom.Visible = true;                
                dateTo.Visible = true;
                lblDateTo.Visible = true;

                //dateFrom.Value = dateTo.Value = GM.GetDateTime().AddDays(-1);
                //dateFrom.Value = dateTo.Value = GM.GetDateTime();

                txtValue.Visible = false;
            }
            else
            {
                //cmbCondition.Text = string.Empty;
                cmbCondition.Items.Clear();
                cmbCondition.Items.Add("Equals"); //Conditions
                cmbCondition.Items.Add("Contains");
                cmbCondition.Items.Add("Not Equals");
                cmbCondition.Text = "Equals";

                dateFrom.Visible = false;
                dateTo.Visible = false;
                lblDateTo.Visible = false;

                txtValue.Visible = true;
            }
        }

        private void txtFieldName_ButtonCustomClick(object sender, EventArgs e)
        {
            try
            {

                frmList_With_Header objListwithHeader = new frmList_With_Header();
                objListwithHeader.dtItems = dtFieldMaster;
                objListwithHeader.sHeaderColumn = "TABLE_NAME";
                objListwithHeader.sValueColumn = "FIELD_NAME_TABLE";
                objListwithHeader.TitleText = "Select Field Name";
                objListwithHeader.ShowDialog();
                if (objListwithHeader.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    txtTableName.Text = objListwithHeader.sReturnTable.ToUpper();
                    txtFieldName.Text = objListwithHeader.sReturnValue.ToUpper();                    
                }

                DateToggle(txtTableName.Text, txtFieldName.Text);

                if (dgvConditions.Rows.Count > 0 && iConditionRowIndex != -1)
                {
                    if (dateFrom.Visible)
                    {
                        dgvConditions.Rows[iConditionRowIndex].Cells["VALUE"].Value = dateFrom.Value.ToString("yyyy-MM-dd") + " AND " + dateTo.Value.ToString("yyyy-MM-dd");
                        dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                        dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
                    }
                    else
                    {
                        dgvConditions.Rows[iConditionRowIndex].Cells["VALUE"].Value = txtValue.Text;
                        dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                        dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
                    }
                }

                
                //frmComboList objFrmComboList = new frmComboList();//Custom combobox showing all filednames of FilterView                
                //objFrmComboList.TitleText = "Select Field Name";
                //objFrmComboList.dtItems = dtFieldList; //List of columns of Filterview Table
                //objFrmComboList.lstColumnsToDisplay.Add("FieldName");
                //objFrmComboList.sColumnToSearch = "FieldName";
                //objFrmComboList.IsSpellCheckEnabeld = false;
                //objFrmComboList.IsMultiSelect = false;
                //objFrmComboList.IsSingleWordSelection = true;
                //objFrmComboList.ShowDialog(this);
                //if (!string.IsNullOrEmpty(objFrmComboList.sReturn))
                //{
                //    txtFieldName.Text = objFrmComboList.sReturn;
                //    txtFieldName.Text = objFrmComboList.sReturn;
                //    txtValue.Text = string.Empty;
                //}
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
  
        private void cmbTreeValue_ButtonCustomClick_1(object sender, EventArgs e)//Custom combobox showing values of selected field name
        {
            try
            {
                if (sFieldName.Trim().Length > 0)
                {
                    frmComboList objFrmComboList = new frmComboList();
                    //objFrmComboList.ShowInTaskbar = false;
                    objFrmComboList.TitleText = "Select " + GM.ProperCaseLeaveCapital(sFieldName.Replace("_", " "));

                    //if (dtCompanies.Columns.Contains(sFieldName))
                    //    objFrmComboList.dtItems = dtCompanies.DefaultView.ToTable(true, sFieldName);//Distinct column value//Company
                    //else
                    //    objFrmComboList.dtItems = dtContacts.DefaultView.ToTable(true, sFieldName);//Distinct column value//Contacts

                    if (sTableName.ToUpper() == "COMPANY")
                        objFrmComboList.dtItems = GV.MSSQL1.BAL_ExecuteQuery("SELECT DISTINCT " + sFieldName + " FROM " + GV.sCompanyTable + ";");
                    else if (sTableName.ToUpper() == "CONTACT")
                        objFrmComboList.dtItems = GV.MSSQL1.BAL_ExecuteQuery("SELECT DISTINCT " + sFieldName + " FROM " + GV.sContactTable + ";");
                    else
                        objFrmComboList.dtItems = GV.MSSQL1.BAL_ExecuteQuery("SELECT DISTINCT " + sFieldName + " FROM " + GV.sProjectID + "_QC;");

                    //if (dtCompanies.Columns.Contains(sFieldName))
                    //    objFrmComboList.dtItems = GV.MasYSQL.BAL_ExecuteQueryMyfSQL("SELECT DISTINCT " + sFieldName + " FROM " + GV.sCompanyTable + ";");
                    //else
                    //    objFrmComboList.dtItems = GV.MYasSQL.BAL_ExecuteQueryMyfSQL("SELECT DISTINCT " + sFieldName + " FROM " + GV.sContactTable + ";");

                    objFrmComboList.lstColumnsToDisplay.Add(sFieldName);
                    objFrmComboList.sColumnToSearch = sFieldName;
                    objFrmComboList.IsSpellCheckEnabeld = true;
                    objFrmComboList.IsMultiSelect = true;
                    objFrmComboList.ShowDialog(this);
                    if (!string.IsNullOrEmpty(objFrmComboList.sReturn))
                    {
                        txtValue.Text = objFrmComboList.sReturn.Replace('|', ',');
                    }
                }
                else
                    ToastNotification.Show(this, "Field not selected.", eToastPosition.TopRight);

            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvFilterAllocation_RowEnter(object sender, DataGridViewCellEventArgs e)//populate controls based on selected rows.
        {
            //iAllocationRowIndex = e.RowIndex;
            //sFilterID = dgvAllocationFilter.Rows[e.RowIndex].Cells["FILTER_ID"].Value.ToString();
            //txtFilterName.Text = dgvAllocationFilter.Rows[e.RowIndex].Cells["FILTER_NAME"].Value.ToString();
            //txtFilterDesc.Text = dgvAllocationFilter.Rows[e.RowIndex].Cells["FILTER_DESC"].Value.ToString();
            //if (dgvAllocationFilter.Rows[e.RowIndex].Cells["Active"].Value.ToString() == "Y")
            //    swtchActive.Value = true;
            //else
            //    swtchActive.Value = false;

            //if (sFilterID.Length > 0)
            //{
            //    dtAllsocation_Filter_Condition = GV.MYsdSQL.BAL_FetchTable("ALLOCATIsON_FILTER_CONDITION", "PROJECT_ID = '" + GlobalVariables.sProjectID + "' AND FILTER_ID = " + sFilterID + "");
            //    dgvConditions.DataSource = dtAllocsation_Filter_Condition;
            //}
            //else//No conditions for selected filter(means create new Condition);
            //{
            //    dgvConditions.DataSource = null;
            //    cmbFieldName.SelectedIndex = -1;
            //    cmbCondition.Text = string.Empty;
            //    txtValue.Text = string.Empty;
            //    dtAllocastion_Filter_Condition = GV.MYdSQL.BAL_FetchTable("ALLOCAsTION_FILTER_CONDITION", "1=0");
            //}
        }

        private void NewFilter()
        {
            try
            {

                //Empty filter is inserted first to get the filter ID which should be used in Condition Table
                dtAllocation_Filter = new DataTable();
                dtAllocation_Filter = GV.MSSQL1.BAL_FetchTable("C_ALLOCATION_FILTER", "PROJECT_ID = '" + GV.sProjectID + "' AND USERACCESS = '" + GV.sAccessTo + "'");
                DataRow drAllocationFilterNewRow = dtAllocation_Filter.NewRow();
                string sNewFilterName;
                int i = 0;
                while (true)//Get name for New filter which does not exist in table(Usually like 'NewFilter1')
                {
                    i += 1;
                    sNewFilterName = "NewFilter" + i;
                    DataRow[] drNameDupeCheck = dtAllocation_Filter.Select("FILTER_NAME = '" + sNewFilterName.Replace("'","''") + "'");
                    if (drNameDupeCheck.Length == 0)
                        break;
                }

                //GlobalMethods.GetDateTime()

                drAllocationFilterNewRow["PROJECT_ID"] = GV.sProjectID;
                drAllocationFilterNewRow["FILTER_NAME"] = sNewFilterName;
                drAllocationFilterNewRow["FILTER_DESC"] = sNewFilterName;
                drAllocationFilterNewRow["Active"] = "N";
                drAllocationFilterNewRow["USERDEFINED"] = "Y";
                drAllocationFilterNewRow["RECHURN"] = "N";
                drAllocationFilterNewRow["TIMEZONE_ENABLED"] = "Y";
                drAllocationFilterNewRow["USERACCESS"] = GV.sAccessTo;
                drAllocationFilterNewRow["CREATED_DATE"] = GM.GetDateTime();
                drAllocationFilterNewRow["CREATED_BY"] = GV.sEmployeeName;
                dtAllocation_Filter.Rows.Add(drAllocationFilterNewRow);
                sFilterID = string.Empty;
                UpdateAllocationTableDB();//Update to Database
                dgvAllocationFilter.CurrentCell = dgvAllocationFilter.Rows[dgvAllocationFilter.Rows.Count - 1].Cells["FILTER_NAME"];
                dgvAllocationFilter_CellClick(dgvAllocationFilter, new DataGridViewCellEventArgs(2, dgvAllocationFilter.Rows.Count - 1));
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void UpdateAllocationTableDB()
        {
            try
            {
                this.BindingContext[dtAllocation_Filter].EndCurrentEdit();
                GM.Logging(dtAllocation_Filter, dtAllocation_Filter_Log, "C_ALLOCATION_FILTER", "FILTER_ID");
                bool IsDBAffected = false;
                if (dtAllocation_Filter.GetChanges(DataRowState.Added) != null)
                {
                    GV.MSSQL1.BAL_SaveToTable(dtAllocation_Filter.GetChanges(DataRowState.Added), "C_ALLOCATION_FILTER", "New", true);
                    IsDBAffected = true;
                }
                if (dtAllocation_Filter.GetChanges(DataRowState.Modified) != null)
                {
                    GV.MSSQL1.BAL_SaveToTable(dtAllocation_Filter.GetChanges(DataRowState.Modified), "C_ALLOCATION_FILTER", "Update", true);
                    IsDBAffected = true;
                }
                if (dtAllocation_Filter.GetChanges(DataRowState.Deleted) != null)
                {
                    GV.MSSQL1.BAL_SaveToTable(dtAllocation_Filter.GetChanges(DataRowState.Deleted), "C_ALLOCATION_FILTER", "Delete", true);
                    IsDBAffected = true;
                }
                if (IsDBAffected)
                    Load_AllocationGrid_and_Combo();//Reload after updations are done
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private string ValidateAllocationTable()
        {
            //DataTable dtConditions = new DataTable();
            //dtConditions = GV.MYSsdQL.BAL_FetchTable("ALLOCATION_FILTER_CONDITION", "PROJECT_ID = '"+GlobalVariables.sProjectID+"'");
            string sErrortText = string.Empty;

            try
            {
                if (dtAllocation_Filter.Rows[iAllocationRowIndex]["FILTER_NAME"].ToString().Trim().Length == 0 || dtAllocation_Filter.Rows[iAllocationRowIndex]["FILTER_DESC"].ToString().Trim().Length == 0)
                {
                    sErrortText += "Filter Name or Filter Description is Empty for one or more record(s)" + Environment.NewLine;
                }
                else
                {
                    DataRow[] dr = dtAllocation_Filter.Select("FILTER_NAME = '" + txtFilterName.Text.Trim() + "'");
                    if (dr.Length > 1)
                        sErrortText += "Duplicate filter name found" + Environment.NewLine;
                }

                //Check for existence of Conditions (Atlease one condition must exist to make allocation active)
                if (dtAllocation_Filter.Rows[iAllocationRowIndex]["Active"].ToString() == "Y")
                {
                    List<string> lstField = new List<string>();//Check for Duplicate(Both new and existing)
                    foreach (DataGridViewRow dgvr in dgvAllocationFilter.Rows)
                        lstField.Add(dgvr.Cells["FILTER_NAME"].Value.ToString());
                    if (lstField.Count != lstField.Distinct().Count())
                        sErrortText += "Duplicate Filter Name found" + Environment.NewLine;

                    if (dgvConditions.Rows.Count == 0)
                        sErrortText += "Active Filter must have condition(s)" + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
            return sErrortText;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtAllocation_Filter.Select("FILTER_NAME = '" + txtFilterName.Text.Trim() + "'").Length > 1)
                {
                    if (dtAllocation_Filter_Condition.Rows.Count > 0)
                    {
                        dgvConditions.CurrentCell = dgvConditions.Rows[0].Cells["FIELD"];
                        dgvConditions_CellClick(dgvConditions, new DataGridViewCellEventArgs(3, 0));
                    }
                    ToastNotification.Show(this, "Filter Name already exist.", eToastPosition.TopRight);
                }
                else
                {
                    iConditionRowIndex = -1;
                    string sConditionError = ValidateConditionTable().Trim();//Validate before Save
                    string sAllocationError = ValidateAllocationTable().Trim();//Validate before Save
                    if (sConditionError.Length == 0 && sAllocationError.Length == 0)
                    {
                        UpdateConditionTableDB();//Save or update Condition Table
                        dtAllocation_Filter.Rows[iAllocationRowIndex]["SQLTEXT"] = BuildQuery(sFilterID, false, swtchNewRecords.Value);
                        UpdateAllocationTableDB();//Save or update Allocation Table
                        dgvAllocationFilter.Enabled = true;
                        splitGridAndControls.Panel2Collapsed = true;
                    }
                    else
                    {
                        if (dtAllocation_Filter_Condition.Rows.Count > 0)
                        {
                            dgvConditions.CurrentCell = dgvConditions.Rows[0].Cells["FIELD"];
                            dgvConditions_CellClick(dgvConditions, new DataGridViewCellEventArgs(3, 0));
                        }
                        ToastNotification.Show(this, sAllocationError + Environment.NewLine + sConditionError, eToastPosition.TopRight);
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private string ValidateConditionTable()
        {
            string sErrorText = string.Empty;
            //string sFieldNameCondition = string.Empty;
            try
            {
                if (dtAllocation_Filter_Condition.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAllocation_Filter_Condition.Rows.Count; i++)//Mandatory check for Existing Condition
                    {
                        if (dtAllocation_Filter_Condition.Rows[i]["FIELD"].ToString().Length == 0 && dtAllocation_Filter_Condition.Rows[i]["CONDITION"].ToString().Length == 0 && dtAllocation_Filter_Condition.Rows[i]["VALUE"].ToString().Length == 0)
                        {
                            dtAllocation_Filter_Condition.Rows.Remove(dtAllocation_Filter_Condition.Rows[i]);
                            continue;
                        }
                        else
                        {
                            //sFieldNameCondition = dtAllocation_Filter_Condition.Rows[i]["FIELD"].ToString().Trim();
                            if (dtAllocation_Filter_Condition.Rows[i]["FIELD"].ToString().Trim().Length == 0)
                                sErrorText += "FieldName Empty" + Environment.NewLine;
                            if (dtAllocation_Filter_Condition.Rows[i]["CONDITION"].ToString().Length == 0)
                                sErrorText += "Condition Empty" + Environment.NewLine;
                            //if (dtAllocation_Filter_Condition.Rows[i]["VALUE"].ToString().Length == 0)
                            //    sErrorText += "Value Empty" + Environment.NewLine;
                        }
                    }

                    List<string> lstField = new List<string>();//Check for Duplicate(Both new and existing)
                    foreach (DataGridViewRow dgvr in dgvConditions.Rows)
                        lstField.Add(dgvr.Cells["FIELD"].Value.ToString());
                    if (lstField.Count != lstField.Distinct().Count())
                        sErrorText += "Duplicate FieldName" + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return sErrorText;
        }

        private void UpdateConditionTableDB()
        {
            try
            {
                if (dgvConditions.DataSource != null && dgvConditions.Rows.Count > 0)
                {
                    this.BindingContext[dtAllocation_Filter_Condition].EndCurrentEdit();
                    //Completly delete and insert conditions for current FilterID
                    GV.MSSQL1.BAL_DeleteFromTable("C_ALLOCATION_FILTER_CONDITION", "PROJECT_ID = '" + GV.sProjectID + "' AND FILTER_ID = " + sFilterID + "");
                    DataTable dtConditions = GV.MSSQL1.BAL_FetchTable("C_ALLOCATION_FILTER_CONDITION", "1=0");

                    foreach (DataGridViewRow dgvr in dgvConditions.Rows)
                    {
                        DataRow drNewRow = dtConditions.NewRow();
                        drNewRow["PROJECT_ID"] = dgvr.Cells["PROJECT_ID"].Value.ToString();
                        drNewRow["FILTER_ID"] = dgvr.Cells["FILTER_ID"].Value.ToString();
                        drNewRow["FIELD"] = dgvr.Cells["FIELD"].Value.ToString();
                        drNewRow["TABLE_NAME"] = dgvr.Cells["TABLE_NAME"].Value.ToString();
                        drNewRow["CONDITION"] = dgvr.Cells["CONDITION"].Value.ToString();
                        drNewRow["VALUE"] = dgvr.Cells["VALUE"].Value.ToString();
                        drNewRow["CREATED_BY"] = dgvr.Cells["CREATED_BY"].Value.ToString();
                        drNewRow["CREATED_DATE"] = dgvr.Cells["CREATED_DATE"].Value.ToString();
                        drNewRow["UPDATED_BY"] = dgvr.Cells["UPDATED_BY"].Value.ToString();
                        drNewRow["UPDATED_DATE"] = dgvr.Cells["UPDATED_DATE"].Value.ToString();
                        dtConditions.Rows.Add(drNewRow);
                    }
                    GV.MSSQL1.BAL_SaveToTable(dtConditions.GetChanges(DataRowState.Added), "C_ALLOCATION_FILTER_CONDITION", "New", true);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private string BuildQuery(string FilterID, bool ReturnSelect, bool IsNewRecords)//Build Qurery based on Conditions
        {
            string sSQLText = "";
            try
            {
                DataTable dtConditionQuery = null;

                if (FilterID.Length > 0)
                    dtConditionQuery = GV.MSSQL1.BAL_FetchTable("C_ALLOCATION_FILTER_CONDITION", "PROJECT_ID = '" + GV.sProjectID + "' AND FILTER_ID = " + FilterID);
                else
                {
                    dtConditionQuery = new DataTable();
                    dtConditionQuery.Columns.Add("CONDITION", typeof(string));
                    dtConditionQuery.Columns.Add("FIELD", typeof(string));
                    dtConditionQuery.Columns.Add("TABLE_NAME", typeof(string));
                    dtConditionQuery.Columns.Add("VALUE", typeof(string));

                    foreach (DataGridViewRow dgvr in dgvConditions.Rows)
                    {
                        if (dgvr.Cells["CONDITION"].Value.ToString().Length > 0 && dgvr.Cells["FIELD"].Value.ToString().Length > 0 && dgvr.Cells["TABLE_NAME"].Value.ToString().Length > 0)
                        {
                            DataRow dr = dtConditionQuery.NewRow();
                            dr["CONDITION"] = dgvr.Cells["CONDITION"].Value.ToString();
                            dr["FIELD"] = dgvr.Cells["FIELD"].Value.ToString();
                            dr["TABLE_NAME"] = dgvr.Cells["TABLE_NAME"].Value.ToString();
                            dr["VALUE"] = dgvr.Cells["VALUE"].Value.ToString().Replace("'", "''");
                            dtConditionQuery.Rows.Add(dr);
                        }
                    }
                }

                //DataTable dtCondition = GV.MadfYSQL.BAL_FetchTableMyfdSQL("ALLOCATION_FILTER_CONDITION", "PROJECT_ID = '" + GV.sProjectID + "' AND FILTER_ID = " + FilterID);

                if (dtConditionQuery != null && dtConditionQuery.Rows.Count > 0)
                {
                    foreach (DataRow drCondition in dtConditionQuery.Rows)//Loop through rows (Append 'AND' betwwen rows)
                    {
                        List<string> lstValue = new List<string>();
                        string sCondition = drCondition["CONDITION"].ToString();
                        string sTableName = drCondition["TABLE_NAME"].ToString();
                        string sFieldNameQuery = "ISNULL(" + sTableName + "." + drCondition["FIELD"] + ",'')";
                        string sInnerCondition = string.Empty;

                        lstValue = drCondition["VALUE"].ToString().Replace("'","''") .Split(',').ToList();
                        if (sSQLText.Length > 0)
                            sSQLText += " AND ";
                        sSQLText += "(";
                        
                        if (sCondition == "Between")
                        {
                            List<string> lstDate = drCondition["VALUE"].ToString().Split(new string[] { " AND " }, StringSplitOptions.None).ToList();
                            sSQLText += sFieldNameQuery + " BETWEEN '" + lstDate[0] + " 00:00:00' AND '" + lstDate[1] + " 23:59:59'";
                        }
                        else
                        {                            
                            foreach (string sValue in lstValue) //Loop through multiple values if exist(Multiple values are suprated by ','(Comma))
                            {                                   //Append 'OR' Between Values for 'CONTAINS'(Like) condition 
                                //Use IN function for values using 'EQUALS or NOT EQUALS'
                                if (sInnerCondition.Length > 0 && sCondition == "Contains")
                                    sInnerCondition += " OR ";
                                if (sCondition == "Equals")
                                {
                                    if (sInnerCondition.Length == 0)
                                        sInnerCondition += sFieldNameQuery + " IN ('" + sValue + "'";
                                    else
                                        sInnerCondition += ",'" + sValue + "'";
                                }
                                else if (sCondition == "Not Equals")
                                {
                                    if (sInnerCondition.Length == 0)
                                        sInnerCondition += sFieldNameQuery + " NOT IN ('" + sValue + "'";
                                    else
                                        sInnerCondition += ",'" + sValue + "'";
                                }                                
                                else//Like Condition
                                    sInnerCondition += sFieldNameQuery + " like '%" + sValue + "%'";
                            }
                        }

                        if (sCondition == "Contains" || sCondition == "Between")
                            sSQLText += sInnerCondition + ")";
                        else
                            sSQLText += sInnerCondition + "))";
                    }
                }

                if (sSQLText.Length > 0)
                {                                        
                    if (dtConditionQuery.Select("FIELD = 'EMAIL_VERIFIED'").Length > 0)
                        sSQLText += " AND CONTACT.EMAIL_VERIFIED = 'BOUNCED'";
                    else
                    {
                        //drDateCalledCheck = dtConditionQuery.Select("FIELD IN ('" + GV.sAccessTo + "_PRIMARY_DISPOSAL','" + GV.sAccessTo + "_SECONDARY_DISPOSAL')");
                        //if (drDateCalledCheck.Length > 0)
                        //    sSQLText += " AND " + sUserTypeDateColumn + " < CURDATE()";
                        //else
                        //    sSQLText += " AND " + sUserTypeDateColumn + " IS NULL";

                        if (IsNewRecords)
                            sSQLText += " AND " + sUserTypeDateColumn + " IS NULL";
                        else
                        {
                            if (dtConditionQuery.Select("Field = '" + GV.sAccessTo + "_PRIMARY_DISPOSAL" + "'").Length > 0)
                                sSQLText += " AND " + sUserTypeDateColumn + " < cast(GETDATE() as date) ";
                            else
                                sSQLText += " AND " + sUserTypeDateColumn + " < cast(GETDATE() as date) AND ISNULL(" + GV.sAccessTo + "_PRIMARY_DISPOSAL,'') NOT IN (SELECT Primary_Status FROM " + GV.sProjectID + "_recordstatus WHERE TABLE_NAME='COMPANY' AND Research_Type='" + GV.sAccessTo + "' AND Operation_Type LIKE '%Freeze%')";
                        }
                    }
                    sSQLText += " AND COMPANY.FLAG = '" + GV.sAccessTo + "'"; //Set flag

                    if (ReturnSelect)
                    {
                        string sSelect = "SELECT DISTINCT COMPANY.MASTER_ID FROM " + GV.sCompanyTable + " COMPANY ";
                        DataTable dtTable = dtConditionQuery .DefaultView.ToTable(true, "TABLE_NAME");
                        if (dtTable.Select("TABLE_NAME IN ('QC')").Length > 0)                        
                            sSelect += " INNER JOIN " + GV.sContactTable + " CONTACT ON COMPANY.MASTER_ID = CONTACT.MASTER_ID INNER JOIN " + GV.sProjectID + "_QC QC ON QC.RECORDID = CONTACT.CONTACT_ID_P AND QC.TABLENAME = 'CONTACT' AND QC.RESEARCHTYPE = '" + GV.sAccessTo + "'";
                        else if (dtTable.Select("TABLE_NAME = 'CONTACT'").Length > 0)
                            sSelect += " INNER JOIN " + GV.sContactTable + " CONTACT ON CONTACT.MASTER_ID = COMPANY.MASTER_ID";


                        sSQLText = sSelect + " WHERE " + sSQLText;
                    }

                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return sSQLText;
        }


        private string BuildQueryDynamic()//Build Qurery based on Conditions
        {
            string sSQLText = "";
            try
            {
                DataTable dtCondition =new DataTable();
                dtCondition.Columns.Add("CONDITION",typeof(string));
                dtCondition.Columns.Add("FIELD",typeof(string));
                dtCondition.Columns.Add("TABLE_NAME", typeof(string));
                dtCondition.Columns.Add("VALUE", typeof(string));

                foreach (DataGridViewRow dgvr in dgvConditions.Rows)
                {
                    DataRow dr = dtCondition.NewRow();
                    dr["CONDITION"] = dgvr.Cells["CONDITION"].Value.ToString();
                    dr["FIELD"] = dgvr.Cells["FIELD"].Value.ToString();
                    dr["TABLE_NAME"] = dgvr.Cells["TABLE_NAME"].Value.ToString();
                    dr["VALUE"] = dgvr.Cells["VALUE"].Value.ToString().Replace("'","''");
                    dtCondition.Rows.Add(dr);
                }

                if (dtCondition != null && dtCondition.Rows.Count > 0)
                {
                    foreach (DataRow drCondition in dtCondition.Rows)//Loop through rows (Append 'AND' betwwen rows)
                    {
                        List<string> lstValue = new List<string>();
                        string sCondition = drCondition["CONDITION"].ToString();
                        string sTableName = drCondition["TABLE_NAME"].ToString();
                        string sFieldNameQuery = "ISNULL(" + sTableName + "." + drCondition["FIELD"] + ",'')";
                        lstValue = drCondition["VALUE"].ToString().Split(',').ToList();
                        if (sSQLText.Length > 0)
                            sSQLText += " AND ";
                        sSQLText += "(";
                        string sInnerCondition = string.Empty;
                        foreach (string sValue in lstValue) //Loop through multiple values if exist(Multiple values are suprated by ','(Comma))
                        {                                   //Append 'OR' Between Values for 'CONTAINS'(Like) condition 
                            //Use IN function for values using 'EQUALS or NOT EQUALS'
                            if (sInnerCondition.Length > 0 && sCondition == "Contains")
                                sInnerCondition += " OR ";
                            if (sCondition == "Equals")
                            {
                                if (sInnerCondition.Length == 0)
                                    sInnerCondition += sTableName + "." + sFieldNameQuery + " IN ('" + sValue + "'";
                                else
                                    sInnerCondition += ",'" + sValue + "'";
                            }
                            else if (sCondition == "Not Equals")
                            {
                                if (sInnerCondition.Length == 0)
                                    sInnerCondition += sTableName + "." + sFieldNameQuery + " NOT IN ('" + sValue + "'";
                                else
                                    sInnerCondition += ",'" + sValue + "'";
                            }
                            else//Like Condition
                                sInnerCondition += sTableName + "." + sFieldNameQuery + " like '%" + sValue + "%'";
                        }
                        if (sCondition == "Contains")
                            sSQLText += sInnerCondition + ")";
                        else
                            sSQLText += sInnerCondition + "))";
                    }
                }
                if (sSQLText.Length > 0)
                {
                    DataRow[] drDateCalledCheck;
                    if (GV.sAccessTo == "TR" || GV.sAccessTo == "WR")
                    {
                        drDateCalledCheck = dtCondition.Select("FIELD IN ('" + GV.sAccessTo + "_PRIMARY_DISPOSAL','" + GV.sAccessTo + "_SECONDARY_DISPOSAL')");
                        if (drDateCalledCheck.Length > 0)
                            sSQLText += " AND " + sUserTypeDateColumn + " < '" + GM.GetDateTime() + "'";
                        else
                            sSQLText += " AND " + sUserTypeDateColumn + " IS NULL";

                        sSQLText += " AND FLAG = '" + GV.sAccessTo + "'"; //Set flag
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return sSQLText;
        }

        private void dgvAllocationFilter_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewColumn dgvc in dgvAllocationFilter.Columns)//Show only required Columns
                {
                    dgvc.Visible = false;
                    if (dgvc.Name == "FILTER_NAME" || dgvc.Name == "FILTER_DESC" || dgvc.Name == "Active" || dgvc.Name == "No_of_Records")
                    {
                        dgvc.HeaderText = GM.ProperCase_ProjectSpecific(dgvc.Name.Replace("_", " "));//Show row header in propercase
                        dgvc.Visible = true;
                        dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
                if (dgvAllocationFilter.DataSource != null && dgvAllocationFilter.Rows.Count > 0)
                    splitConditions.Panel2.Enabled = true;
                else
                    splitConditions.Panel2.Enabled = false;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvConditions_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewColumn dgvc in dgvConditions.Columns)//Show only required Columns
                {
                    dgvc.Visible = false;
                    if (dgvc.Name == "FIELD" || dgvc.Name == "CONDITION" || dgvc.Name == "VALUE")
                    {
                        dgvc.HeaderText = GM.ProperCaseLeaveCapital(dgvc.Name.Replace("_", " "));//Show row header in propercase
                        dgvc.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void newFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvAllocationFilter.Enabled)
            {
                NewFilter();
                EditFilter();
            }
        }

        private void deactivateFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAllocationFilter.Enabled)
                {
                    dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["Active"].Value = "N";
                    if (btnSave.Enabled == false)
                    {
                        btnSave.Enabled = true;
                        btnSave.PerformClick();
                        btnSave.Enabled = false;
                    }
                    else
                        btnSave.PerformClick();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void activateFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAllocationFilter.Enabled)
                {
                    dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["Active"].Value = "Y";
                    if (btnSave.Enabled == false)
                    {
                        btnSave.Enabled = true; //Cannot perform click in disabled control
                        btnSave.PerformClick();
                        btnSave.Enabled = false;
                    }
                    else
                        btnSave.PerformClick();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void previewFilterDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAllocationFilter.Enabled)
                {
                    if (dgvAllocationFilter.DataSource != null && dgvAllocationFilter.Rows.Count > 0)
                    {
                        if (dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["SQLText"].Value != null) //Preview the filter condition
                        {
                            frmPreviewFilter objPreview = new frmPreviewFilter();
                            string sQuery = dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["SQLText"].Value.ToString();
                            objPreview.sSQLText = sQuery;
                            objPreview.bTimeZoneEnabled = dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["TIMEZONE_ENABLED"].Value.ToString() == "Y";
                            objPreview.sFilterID = dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["FILTER_ID"].Value.ToString();
                            objPreview.IsNewRecord  = dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["RECHURN"].Value.ToString() == "N";
                            objPreview.bManualFilter = dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["USERDEFINED"].Value.ToString() == "N";
                            objPreview.ShowDialog(this);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void removeConditionToolStripMenuItem_Click(object sender, EventArgs e)//Remove the selected condition
        {
            try
            {
                if (dgvConditions != null && dgvConditions.Rows.Count > 0 && dgvConditions.CurrentCell != null)
                {
                    string sID = dgvConditions.Rows[dgvConditions.CurrentCell.RowIndex].Cells["ID"].Value.ToString();
                    if (sID.Length > 0)
                    {
                        if (DialogResult.Yes == MessageBoxEx.Show("Are you sure to delete this condition ?", "Alert", MessageBoxButtons.YesNo))
                        {
                            GV.MSSQL1.BAL_DeleteFromTable("C_ALLOCATION_FILTER_CONDITION", "ID = " + sID);
                            //Load_AllocationGrid_and_Combo();

                            if (sFilterID.Length > 0)
                            {
                                dtAllocation_Filter_Condition = GV.MSSQL1.BAL_FetchTable("C_ALLOCATION_FILTER_CONDITION", "PROJECT_ID = '" + GV.sProjectID + "' AND FILTER_ID = " + sFilterID + "");
                                dgvConditions.DataSource = dtAllocation_Filter_Condition;
                                if (dtAllocation_Filter_Condition.Rows.Count > 0)
                                {
                                    dgvConditions.CurrentCell = dgvConditions.Rows[dgvConditions.Rows.Count - 1].Cells["FIELD"];
                                    dgvConditions_CellClick(dgvConditions, new DataGridViewCellEventArgs(3, dgvConditions.Rows.Count - 1));
                                }
                                else
                                {
                                    dgvConditions.DataSource = null;
                                    txtFieldName.Text = string.Empty;
                                    txtTableName.Text = string.Empty;
                                    cmbCondition.Text = string.Empty;
                                    txtValue.Text = string.Empty;
                                    dtAllocation_Filter_Condition = GV.MSSQL1.BAL_FetchTable("C_ALLOCATION_FILTER_CONDITION", "1=0");
                                }
                            }
                            else//No conditions for selected filter(means create new Condition);
                            {
                                dgvConditions.DataSource = null;
                                txtFieldName.Text = string.Empty;
                                txtTableName.Text = string.Empty;
                                cmbCondition.Text = string.Empty;
                                txtValue.Text = string.Empty;
                                dtAllocation_Filter_Condition = GV.MSSQL1.BAL_FetchTable("C_ALLOCATION_FILTER_CONDITION", "1=0");
                            }
                        }
                    }
                    else
                        dgvConditions.Rows.Remove(dgvConditions.Rows[dgvConditions.CurrentCell.RowIndex]);

                    if (dtAllocation_Filter_Condition.Rows.Count > 0)
                    {
                        dgvConditions.CurrentCell = dgvConditions.Rows[0].Cells["FIELD"];
                        dgvConditions_CellClick(dgvConditions, new DataGridViewCellEventArgs(3, 0));
                    }
                    else
                        ShowConditionControls(false);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void editFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditFilter();
        }

        private void EditFilter()
        {
            try
            {
                if (dgvAllocationFilter.Enabled && dgvAllocationFilter.DataSource != null && dgvAllocationFilter.CurrentCell != null)
                {
                    if (dgvAllocationFilter.Rows[dgvAllocationFilter.CurrentCell.RowIndex].Cells["USERDEFINED"].Value.ToString() == "Y")
                    {
                        lblCount.Text = "Count";
                        foreach (DataGridViewRow dgvr in dgvAllocationFilter.Rows)
                        { dgvr.DefaultCellStyle.BackColor = Color.White; }
                        splitGridAndControls.Panel2Collapsed = false;
                        dgvAllocationFilter.Enabled = false;
                        dgvAllocationFilter.Rows[dgvAllocationFilter.CurrentCell.RowIndex].DefaultCellStyle.BackColor = Color.Pink;
                        if (dgvConditions.Rows.Count > 0)//Focus first row of condition by default
                        {
                            dgvConditions.CurrentCell = dgvConditions.Rows[0].Cells["FIELD"];
                            dgvConditions_CellClick(dgvConditions, new DataGridViewCellEventArgs(3, 0));
                        }
                        else
                        {
                            txtFieldName.Text = string.Empty;
                            txtTableName.Text = string.Empty;
                            cmbCondition.Text = string.Empty;
                            txtValue.Text = string.Empty;
                            ShowConditionControls(false);//if no conditions is zero controls ll be hidden.
                        }
                    }
                    else
                    {
                        ToastNotification.Show(this,"Manual Filter cannot be edited.",eToastPosition.TopRight);
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {
            Load_AllocationGrid_and_Combo();
            splitGridAndControls.Panel2Collapsed = true;
            dgvAllocationFilter.Enabled = true;
        }

        private void txtFieldName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtFieldName.Text.Length > 0)                
                    sFieldName = txtFieldName.Text;
                if (splitGridAndControls.Panel2Collapsed == false && dgvConditions.Rows.Count > 0 && iConditionRowIndex != -1 && txtFieldName.Text.Length > 0)
                {
                    dgvConditions.Rows[iConditionRowIndex].Cells["FIELD"].Value = txtFieldName.Text;                    
                    dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                    dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void txtTableName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtTableName.Text.Length > 0)                                    
                    sTableName = txtTableName.Text;
                if (splitGridAndControls.Panel2Collapsed == false && dgvConditions.Rows.Count > 0 && iConditionRowIndex != -1 && txtTableName.Text.Length > 0)
                {                    
                    dgvConditions.Rows[iConditionRowIndex].Cells["TABLE_NAME"].Value = txtTableName.Text;
                    dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                    dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        } 

        private void cmbCondition_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (splitGridAndControls.Panel2Collapsed == false && dgvConditions.Rows.Count > 0 && iConditionRowIndex != -1)
                {
                    dgvConditions.Rows[iConditionRowIndex].Cells["CONDITION"].Value = cmbCondition.Text;
                    dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                    dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (splitGridAndControls.Panel2Collapsed == false && dgvConditions.Rows.Count > 0 && iConditionRowIndex != -1)
                {
                    dgvConditions.Rows[iConditionRowIndex].Cells["VALUE"].Value = txtValue.Text;
                    dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                    dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmbCondition_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void addConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow drConditions = dtAllocation_Filter_Condition.NewRow();
                drConditions["PROJECT_ID"] = GV.sProjectID;
                drConditions["FILTER_ID"] = sFilterID;
                drConditions["CREATED_BY"] = GV.sEmployeeName;
                drConditions["CREATED_DATE"] = GM.GetDateTime();
                dtAllocation_Filter_Condition.Rows.Add(drConditions);
                //dgvConditions.Rows[dgvConditions.Rows.Count - 1].Selected = true;
                dgvConditions.DataSource = dtAllocation_Filter_Condition;
                dgvConditions.CurrentCell = dgvConditions.Rows[dgvConditions.Rows.Count - 1].Cells["FIELD"];
                dgvConditions_CellClick(dgvConditions, new DataGridViewCellEventArgs(3,dgvConditions.Rows.Count - 1));
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtFilterName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["FILTER_NAME"].Value = txtFilterName.Text;
                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtFilterDesc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["FILTER_DESC"].Value = txtFilterDesc.Text;
                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void swtchActive_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (swtchActive.Value)
                    dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["Active"].Value = "Y";
                else
                    dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["Active"].Value = "N";

                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void swtchRandom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (swtchRandom.Value)
                    dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["RANDOMIZE"].Value = "Y";
                else
                    dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["RANDOMIZE"].Value = "N";

                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void swtchTimezone_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (swtchTimezone.Value)
                    dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["TIMEZONE_ENABLED"].Value = "Y";
                else
                    dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["TIMEZONE_ENABLED"].Value = "N";

                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvConditions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {                    
                    iConditionRowIndex = e.RowIndex;
                    DateToggle(dgvConditions.Rows[e.RowIndex].Cells["TABLE_NAME"].Value.ToString(), dgvConditions.Rows[e.RowIndex].Cells["FIELD"].Value.ToString());
                    ShowConditionControls(true);
                    txtFieldName.Text = dgvConditions.Rows[e.RowIndex].Cells["FIELD"].Value.ToString();
                    txtTableName.Text = dgvConditions.Rows[e.RowIndex].Cells["TABLE_NAME"].Value.ToString();
                    cmbCondition.Text = dgvConditions.Rows[e.RowIndex].Cells["CONDITION"].Value.ToString();

                    if (dateFrom.Visible)
                    {
                        List<string> lstDate = dgvConditions.Rows[e.RowIndex].Cells["VALUE"].Value.ToString().Split(new string[] { " AND " }, StringSplitOptions.None).ToList();
                        if (lstDate.Count == 2)
                        {
                            dateFrom.Text = lstDate[0].Trim();
                            dateTo.Text = lstDate[1].Trim();
                        }
                        else
                            dateFrom.Value = dateTo.Value = GM.GetDateTime();
                    }
                    else                    
                        txtValue.Text = dgvConditions.Rows[e.RowIndex].Cells["VALUE"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void ShowConditionControls(bool Show)//if no conditions is zero controls ll be hidden.
        {
            //if (!cmbFieldName.Visible)
            {
                txtFieldName.Visible = Show;
                txtTableName.Visible = Show;
                cmbCondition.Visible = Show;                
                lblField.Visible = Show;
                lblCondition.Visible = Show;                
                lblValue.Visible = Show;

                //txtValue.Visible = Show;

                if (Show)
                {                     
                    txtValue.Visible = !dateFrom.Visible;
                }
                else
                {
                    dateFrom.Visible = false;
                    dateTo.Visible = false;
                    lblDateTo.Visible = false;
                    txtValue.Visible = false;
                }


                //dateFrom.Visible = DateShow;
                //dateTo.Visible = DateShow;
                //lblDateTo.Visible = DateShow;

                //txtValue.Visible = !DateShow;

                
            }
        }

        private void dgvAllocationFilter_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    iAllocationRowIndex = e.RowIndex;
                    sFilterID = dgvAllocationFilter.Rows[e.RowIndex].Cells["FILTER_ID"].Value.ToString();
                    txtFilterName.Text = dgvAllocationFilter.Rows[e.RowIndex].Cells["FILTER_NAME"].Value.ToString();
                    txtFilterDesc.Text = dgvAllocationFilter.Rows[e.RowIndex].Cells["FILTER_DESC"].Value.ToString(); 
                    swtchActive.Value = dgvAllocationFilter.Rows[e.RowIndex].Cells["Active"].Value.ToString() == "Y";
                    swtchRandom.Value = dgvAllocationFilter.Rows[e.RowIndex].Cells["RANDOMIZE"].Value.ToString() == "Y";
                    swtchNewRecords.Value = dgvAllocationFilter.Rows[e.RowIndex].Cells["RECHURN"].Value.ToString() == "N";
                    swtchTimezone.Value = dgvAllocationFilter.Rows[e.RowIndex].Cells["TIMEZONE_ENABLED"].Value.ToString() == "Y";

                    if (sFilterID.Length > 0)
                    {
                        dtAllocation_Filter_Condition = GV.MSSQL1.BAL_FetchTable("C_ALLOCATION_FILTER_CONDITION", "PROJECT_ID = '" + GV.sProjectID + "' AND FILTER_ID = " + sFilterID + "");
                        dgvConditions.DataSource = dtAllocation_Filter_Condition;
                    }
                    else//No conditions for selected filter(means create new Condition);
                    {
                        dgvConditions.DataSource = null;
                        txtFieldName.Text = string.Empty;
                        txtTableName.Text = string.Empty;
                        cmbCondition.Text = string.Empty;
                        txtValue.Text = string.Empty;
                        dtAllocation_Filter_Condition = GV.MSSQL1.BAL_FetchTable("C_ALLOCATION_FILTER_CONDITION", "1=0");
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvConditions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //string sSQLText = BuildQueryDynamic();

            //try
            //{
            //    DataRow[] dr = dtCompanies.Select(sSQLText);
            //    lblCount.Text = "Record(s) Found:"+ dr.Length;
            //}
            //catch (Exception ex)
            //{ lblCount.Text = string.Empty; }

        }

        private void btnCloseHidden_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReloadCount_Click(object sender, EventArgs e)
        {
            string sSQLText = BuildQuery(string.Empty, true, swtchNewRecords.Value);
            if (GV.HasAdminPermission)
            {
                txtQuery.Text = sSQLText;
                txtQuery.Visible = true;
            }
            if (sSQLText.Trim().Length > 0)
            {
                try
                {
                    DataTable dtCount = GV.MSSQL1.BAL_ExecuteQuery(sSQLText);
                    lblCount.Text = "Record(s) Found:" + dtCount.Rows.Count;
                }
                catch (Exception ex)
                { lblCount.Text = "Error Occured...!"; }
            }
            else
                lblCount.Text = "Count";
        }

        private void swtchNewRecords_ValueChanged(object sender, EventArgs e)
        {
            lblRechurn.Visible = !swtchNewRecords.Value;

            try
            {
                if (swtchNewRecords.Value)
                    dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["RECHURN"].Value = "N";
                else
                    dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["RECHURN"].Value = "Y";

                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                dgvAllocationFilter.Rows[iAllocationRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dateFrom_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (splitGridAndControls.Panel2Collapsed == false && dgvConditions.Rows.Count > 0 && iConditionRowIndex != -1)
                {
                    dgvConditions.Rows[iConditionRowIndex].Cells["VALUE"].Value = dateFrom.Value.ToString("yyyy-MM-dd") + " AND " + dateTo.Value.ToString("yyyy-MM-dd");
                    dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                    dgvConditions.Rows[iConditionRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
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
