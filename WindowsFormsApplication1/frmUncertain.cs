using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmUncertain : Office2007Form
    {

        DataTable dtPending;
        DataTable dtGridSource;
        DataTable dtUncertain_Fields;
       // DataTable dtField_Master;
        public frmUncertain()
        {
            InitializeComponent();
            
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);

            dtGridSource = new DataTable();
            dtGridSource.Columns.Add("FieldName");
            dtGridSource.Columns.Add("FieldValue");
            dtGridSource.Columns.Add("FieldValueCopy");
            dtGridSource.Columns.Add("Record Count");
            dtGridSource.Columns.Add("Agent Count");
            dtGridSource.Columns.Add("Date");
            dtGridSource.Columns.Add("AgentName");
            dtGridSource.Columns.Add("Status", typeof(bool));

            //dtField_Master = new DataTable();
            //dtField_Master.Columns.Add("FieldName");
            //dtField_Master.Columns.Add("FieldName_LinkColumn");
            //dtField_Master.Columns.Add("PickList_Category");
        }

        private DataTable _dtField_Master;

        //-----------------------------------------------------------------------------------------------------
        public DataTable dtField_Master /////
        {
            get { return _dtField_Master; }
            set { _dtField_Master = value; }
        }  

        
        
        
        private void frm_Uncertain_Load(object sender, EventArgs e)
        {
            //DataTable dtFields = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT FIELD_NAME_TABLE,UNCERTAIN_RAISABLE,UNCERTAIN_LINKED_COLUMN,PICKLIST_CATEGORY FROM c_field_master WHERE PROJECT_ID = '" + GV.sProjectID + "' AND TABLE_NAME='MasterContacts' AND (LENGTH(IFNULL(UNCERTAIN_LINKED_COLUMN,'')) > 0 OR UNCERTAIN_RAISABLE = 'Y');");
            //DataRow[] drrTemp = dtFields.Select("UNCERTAIN_RAISABLE = 'Y'");
            //foreach (DataRow drFieldRows in drrTemp)
            //{
            //    DataRow drNewRow = dtField_Master.NewRow();
            //    drNewRow["FieldName"] = drFieldRows["FIELD_NAME_TABLE"].ToString();
            //    drNewRow["PickList_Category"] = drFieldRows["PICKLIST_CATEGORY"].ToString();
            //    dtField_Master.Rows.Add(drNewRow);
            //}
            //drrTemp = dtFields.Select("LEN(UNCERTAIN_LINKED_COLUMN) > 0");
            //foreach (DataRow drFieldRows in drrTemp)
            //{
            //    if (dtField_Master.Select("FieldName = '" + drFieldRows["UNCERTAIN_LINKED_COLUMN"] + "'").Length > 0)
            //        dtField_Master.Select("FieldName = '" + drFieldRows["UNCERTAIN_LINKED_COLUMN"] + "'")[0]["FieldName_LinkColumn"] = drFieldRows["FIELD_NAME_TABLE"].ToString();
            //}


            ReLoad_Tables("OTHERS_JOBTITLE");
            
            //rCheckStatus_Reject.CheckStateChanged += new EventHandler( rCheckStatus_Reject_CheckStateChanged);
            //repositoryItemRadioGroup1.Items.Add(new RadioGroupItem("True", "True"));
            //repositoryItemRadioGroup1.Items.Add(new RadioGroupItem("False", "False"));

            
            
        }

        void ReLoad_Tables(string sFieldToSelect)
        {
            if (dtPending != null)            
                dtPending.Clear();
            if (dtUncertain_Fields != null)
                dtUncertain_Fields.Clear();
            
            cmbFieldNames.Items.Clear();
            dtPending = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM " + GV.sContactTable + " WHERE " + GV.sAccessTo + "_UNCERTAIN_STATUS = 1;");
            dtUncertain_Fields = dtPending.DefaultView.ToTable(true, GV.sAccessTo + "_UNCERTAIN_FIELD");
            foreach (DataRow drAddFields in dtUncertain_Fields.Rows)
                cmbFieldNames.Items.Add(drAddFields[GV.sAccessTo + "_UNCERTAIN_FIELD"].ToString());

            if (dtPending.Rows.Count > 0)
            {
                if (dtUncertain_Fields.Select(GV.sAccessTo + "_UNCERTAIN_FIELD = '" + sFieldToSelect + "'").Length > 0)
                {
                    cmbFieldNames.SelectedItem = sFieldToSelect;
                    Load_Grid(dtPending, sFieldToSelect);
                }
                else
                {
                    cmbFieldNames.SelectedItem = dtUncertain_Fields.Rows[0][0].ToString();
                    Load_Grid(dtPending, dtUncertain_Fields.Rows[0][0].ToString());
                }
            }
            else
            {
                xGridUncertain.DataSource = null;
                ToastNotification.Show(this, "No Uncertains raised.", eToastPosition.TopRight);
            }
        }

        void Load_Grid(DataTable dtPendingData, string sUncertain_Field)
        {
            xGridUncertain.DataSource = null;
            dtGridSource.Rows.Clear();

            DataTable dtContacts = dtPendingData.Select(GV.sAccessTo + "_UNCERTAIN_FIELD = '" + sUncertain_Field + "'").CopyToDataTable();

            foreach (DataRow drContacts in dtContacts.Rows)
            {
                if (dtGridSource.Select("FieldName = '" + drContacts[GV.sAccessTo + "_UNCERTAIN_FIELD"].ToString() + "' AND FieldValueCopy = '" + drContacts[sUncertain_Field].ToString().Replace("'", "''") + "'").Length > 0)
                    continue;

                DataRow drNew = dtGridSource.NewRow();
                drNew["FieldName"] = drContacts[GV.sAccessTo + "_UNCERTAIN_FIELD"].ToString();
                drNew["FieldValue"] = drContacts[sUncertain_Field].ToString();
                drNew["FieldValueCopy"] = drContacts[sUncertain_Field].ToString();
                drNew["Record Count"] = dtContacts.Select(sUncertain_Field + " = '" + drContacts[sUncertain_Field].ToString().Replace("'", "''") + "'").Length;
                DataTable dtAgents = dtContacts.Select(sUncertain_Field + " = '" + drContacts[sUncertain_Field].ToString().Replace("'", "''") + "'").CopyToDataTable().DefaultView.ToTable(true, GV.sAccessTo + "_AGENT_NAME");
                drNew["Agent Count"] = dtAgents.Rows.Count;
                string sAgentNames = string.Empty;

                foreach (DataRow drAgents in dtAgents.Rows)
                    sAgentNames += drAgents[GV.sAccessTo + "_AGENT_NAME"] + " (" + dtContacts.Select(sUncertain_Field + " = '" + drContacts[sUncertain_Field].ToString().Replace("'", "''") + "' AND " + GV.sAccessTo + "_AGENT_NAME = '" + drAgents[GV.sAccessTo + "_AGENT_NAME"] + "'").Length + ")" + Environment.NewLine;

                drNew["AgentName"] = sAgentNames;
                //drNew["Status"] = false;                
                dtGridSource.Rows.Add(drNew);
            }
            xGridUncertain.DataSource = dtGridSource;
            
            if (dtGridSource.Rows.Count > 0)
                lblSelectedField.Text = sUncertain_Field;
            else
                lblSelectedField.Text = string.Empty;
        }

        private void btnLoadGrid_Click(object sender, EventArgs e)
        {
            if(cmbFieldNames.SelectedItem != null && cmbFieldNames.SelectedItem.ToString().Length > 0)
                Load_Grid(dtPending, cmbFieldNames.SelectedItem.ToString());
        }        

        void Logging(DataTable dtNewTable, DataTable dtOldTable)
        {
            try
            {                
                DataTable dtChangedData = dtNewTable.GetChanges(DataRowState.Modified);
                DataTable dtLog = GV.MYSQL.BAL_FetchTableMySQL(GV.sProjectID + "_log", "1=0");

                if (dtChangedData != null)
                {
                    foreach (DataRow drChanged in dtChangedData.Rows)
                    {
                        string sContactID = drChanged["CONTACT_ID_P"].ToString();
                        DataRow drOld = dtOldTable.Select("CONTACT_ID_P = " + sContactID)[0];
                        foreach (DataColumn dcOld in drOld.Table.Columns)
                        {
                            if (drChanged[dcOld.ColumnName].ToString() != drOld[dcOld.ColumnName].ToString())
                            {
                                DataRow drLog = dtLog.NewRow();
                                drLog["RecordID"] = drChanged["CONTACT_ID_P"].ToString();
                                drLog["CompanySessionID"] = GV.sCompanySessionID;
                                drLog["TableName"] = "MasterContact_Uncertain";
                                drLog["Who"] = GV.sEmployeeName;
                                drLog["When"] = GM.GetDateTime();
                                drLog["SystemName"] = Environment.MachineName;
                                drLog["FieldName"] = dcOld.ColumnName;                                
                                drLog["OldValue"] = drOld[dcOld.ColumnName].ToString();
                                drLog["NewValue"] = drChanged[dcOld.ColumnName].ToString();
                                dtLog.Rows.Add(drLog);
                            }
                        }
                    }
                    GV.MYSQL.BAL_SaveToTableMySQL(dtLog, GV.sProjectID + "_log", "New", false);
                }
            }
            catch (Exception ex)
            {                
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
            }
        }

        void Save_Uncertains(DataTable dtChecked_GridSource)
        {
            string sQuery = string.Empty;            
            foreach (DataRow drField in dtUncertain_Fields.Rows)
            {
                if (
                    dtChecked_GridSource.Select("Status IN('True','False')  AND FieldName = '" + drField[0].ToString() + "' AND LEN(ISNULL(FieldValue,'')) = 0").Length > 0)//check if any field is completly deleted
                {
                    ToastNotification.Show(this, drField[0].ToString() + " cannot be empty.", eToastPosition.TopRight);
                    return;
                }

                if (dtChecked_GridSource.Select("Status IN('True','False')  AND FieldName = '" + drField[0].ToString() + "'").Length > 0)
                {
                    DataTable dtCheckRows = dtChecked_GridSource.Select("Status IN('True','False') AND FieldName = '" + drField[0].ToString() + "'").CopyToDataTable();
                    string sSelected_Fields = string.Empty;
                    foreach (DataRow drCheckedRow in dtCheckRows.Rows)
                    {
                        if (sSelected_Fields.Length > 0)
                            sSelected_Fields += ", '" + GM.RemoveEndBackSlash(drCheckedRow["FieldValueCopy"].ToString().Replace("'", "''")) + "'";
                        else
                            sSelected_Fields = "'" + GM.RemoveEndBackSlash(drCheckedRow["FieldValueCopy"].ToString().Replace("'", "''")) + "'";
                    }

                    if (sQuery.Length > 0)
                        sQuery += " OR " + drField[0].ToString() + " IN (" + sSelected_Fields + ")";
                    else
                        sQuery = drField[0].ToString() + " IN (" + sSelected_Fields + ")";
                }
            }


            if (sQuery.Length > 0)
            {
                sQuery = "SELECT * FROM " + GV.sContactTable + " WHERE (" + sQuery + ") AND " + GV.sAccessTo + "_UNCERTAIN_STATUS = 1";

                DataTable dtRealTime_ContactData = GV.MYSQL.BAL_ExecuteQueryMySQL(sQuery);
                DataTable dtRealTime_ContactData_Copy = dtRealTime_ContactData.Copy();
                string sPickList_Update = string.Empty;
                DataTable dtPickList_Delete = new DataTable();
                dtPickList_Delete.Columns.Add("CategoryName");
                dtPickList_Delete.Columns.Add("Values");
                foreach (DataRow drContactData in dtRealTime_ContactData.Rows)
                {
                    string sUncertainField = drContactData[GV.sAccessTo + "_UNCERTAIN_FIELD"].ToString().ToUpper();
                    string sUncertainValue = drContactData[sUncertainField].ToString().ToUpper().Trim();
                    string sUncertainValue_Edited = dtChecked_GridSource.Select("Status IN('True','False') AND FieldName = '" + sUncertainField + "' AND FieldValueCopy = '" + GM.RemoveEndBackSlash(sUncertainValue.Replace("'","''")) + "'")[0]["FieldValue"].ToString().Trim();
                    DataRow drTemp = dtField_Master.Select("FieldName = '" + sUncertainField + "' OR FieldName_LinkColumn = '" + sUncertainField + "'")[0];
                    string sFieldName = drTemp["FieldName"].ToString();
                    string sFieldName_LinkColumn = drTemp["FieldName_LinkColumn"].ToString();
                    string sPickLIstCatrgory = drTemp["Picklist_Category"].ToString();
                    bool IsAccpeted = dtChecked_GridSource.Select("Status IN('True','False') AND FieldName = '" + sUncertainField + "' AND FieldValueCopy = '" + GM.RemoveEndBackSlash(sUncertainValue.Replace("'", "''")) + "'")[0]["Status"].ToString() == "True";                    
                    if (IsAccpeted)
                    {
                        drContactData[sFieldName] = sUncertainValue_Edited;
                        //drContactData[sUncertainField] = DBNull.Value;
                        if (sFieldName_LinkColumn.Length > 0)
                            drContactData[sFieldName_LinkColumn] = DBNull.Value;

                        if (sPickLIstCatrgory.Length > 0)
                        {
                            if (dtPickList_Delete.Select("CategoryName = '" + sPickLIstCatrgory + "'").Length == 0)
                                dtPickList_Delete.Rows.Add(sPickLIstCatrgory, string.Empty);

                            if (sUncertainValue == sUncertainValue_Edited.ToUpper())
                            {
                                sPickList_Update += ",('" + sPickLIstCatrgory + "', '" + GM.RemoveEndBackSlash(sUncertainValue.Replace("'", "''")) + "', 'Accepted')";
                                dtPickList_Delete.Select("CategoryName = '" + sPickLIstCatrgory + "'")[0]["Values"] += ",'" + GM.RemoveEndBackSlash(sUncertainValue.Replace("'", "''")) + "'";
                            }
                            else
                            {
                                sPickList_Update += ",('" + sPickLIstCatrgory + "', '" + GM.RemoveEndBackSlash(sUncertainValue_Edited.Replace("'", "''")) + "', 'Accepted')";
                                sPickList_Update += ",('" + sPickLIstCatrgory + "', '" + GM.RemoveEndBackSlash(sUncertainValue.Replace("'", "''")) + "', 'Rejected')";

                                dtPickList_Delete.Select("CategoryName = '" + sPickLIstCatrgory + "'")[0]["Values"] += ",'" + GM.RemoveEndBackSlash(sUncertainValue.Replace("'", "''")) + "'";
                                dtPickList_Delete.Select("CategoryName = '" + sPickLIstCatrgory + "'")[0]["Values"] += ",'" + GM.RemoveEndBackSlash(sUncertainValue_Edited.Replace("'", "''")) + "'"; //May not exist
                            }
                        }
                        drContactData[GV.sAccessTo + "_CONTACT_STATUS"] = drContactData[GV.sAccessTo + "_CONTACT_STATUS"].ToString().Replace("_UNCERTAIN", string.Empty);
                        drContactData[GV.sAccessTo + "_UNCERTAIN_FIELD"] = DBNull.Value;
                        drContactData[GV.sAccessTo + "_UNCERTAIN_STATUS"] = 0;
                    }
                    else
                    {                        
                        drContactData[GV.sAccessTo + "_UNCERTAIN_STATUS"] = 2;
                        //drContactData[GV.sAccessTo + "_CONTACT_STATUS"] = "UNCERTAIN REJECTED";
                        drContactData["TR_CONTACT_STATUS"] = "UNCERTAIN REJECTED";
                        drContactData["WR_CONTACT_STATUS"] = "UNCERTAIN REJECTED";
                        if (sPickLIstCatrgory.Length > 0)
                        {
                            if (dtPickList_Delete.Select("CategoryName = '" + sPickLIstCatrgory + "'").Length == 0)
                                dtPickList_Delete.Rows.Add(sPickLIstCatrgory, string.Empty);

                            sPickList_Update += ",('" + sPickLIstCatrgory + "', '" + GM.RemoveEndBackSlash(sUncertainValue.Replace("'", "''")) + "', 'Rejected')";
                            dtPickList_Delete.Select("CategoryName = '" + sPickLIstCatrgory + "'")[0]["Values"] += ",'" + GM.RemoveEndBackSlash(sUncertainValue.Replace("'", "''")) + "'";
                        }
                    }
                    drContactData[GV.sAccessTo + "_UNCERTAIN_RESOLVED_BY"] = GV.sEmployeeName;
                    drContactData[GV.sAccessTo + "_UNCERTAIN_RESOLVED_DATE"] = GM.GetDateTime();
                }

                Logging(dtRealTime_ContactData, dtRealTime_ContactData_Copy);
                if (GV.MYSQL.BAL_SaveToTableMySQL(dtRealTime_ContactData.GetChanges(DataRowState.Modified), GV.sContactTable, "Update", true))
                {
                    if (sPickList_Update.Length > 0)
                    {
                        string sPicklist_Delete = string.Empty;
                        foreach (DataRow drDeletePending in dtPickList_Delete.Rows)
                        {
                            if (drDeletePending[1].ToString().Length > 0)
                                sPicklist_Delete += "DELETE FROM " + GV.sProjectID + "_picklists WHERE remarks ='Pending' AND PicklistCategory = '" + drDeletePending[0] + "' AND PicklistValue IN (" + GM.RemoveEndBackSlash(drDeletePending[1].ToString().Substring(1)) + ");";
                        }

                        GV.MYSQL.BAL_ExecuteNonReturnQueryMySQL("INSERT INTO " + GV.sProjectID + "_picklists (PicklistCategory, PicklistValue, remarks) VALUES " + sPickList_Update.Substring(1));

                        if (sPicklist_Delete.Trim().Length > 0)
                            GV.MYSQL.BAL_ExecuteNonReturnQueryMySQL(sPicklist_Delete);

                        GV.MYSQL.BAL_ExecuteNonReturnQueryMySQL("UPDATE c_project_settings set PICKLIST_LASTUPDATE = NOW() WHERE PROJECT_ID='" + GV.sProjectID + "';");
                        ToastNotification.Show(this, "Updated Successfully", eToastPosition.TopRight);
                        ReLoad_Tables("OTHERS_JOBTITLE");
                    }
                }
            }
            else            
                ToastNotification.Show(this, "No changes to update", eToastPosition.TopRight);            
        }


        private bool IsFontInstalled(string sFontName)
        {
            using (var testFont = new Font(sFontName, 8))
            {
                return 0 == string.Compare(sFontName, testFont.Name, StringComparison.InvariantCultureIgnoreCase);
            }
        }
        private void GetCaptcha()//Random captcha string
        {
            try
            {
                Random Rand = new Random();
                char[] chars = "a4sa1da3dwg7tehrt8jtyek9rtoit0itri2riwxwq4a2c4d6gbe5y2zqaioperdftghjuyzncmlp5z4a5q5ws5xh4f5".ToCharArray();
                string sCaptchaString = string.Empty;

                for (int i = 1; i < 6; i++)
                    sCaptchaString += chars[Rand.Next(0, 90)];
                List<string> lstFonts = new List<string>();

                lstFonts.Add("Arial");
                lstFonts.Add("Comic Sans MS");
                lstFonts.Add("Cooper Black");
                lstFonts.Add("Times New Roman");
                lstFonts.Add("Tahoma");
                string sFont = string.Empty;
                while (true)
                {
                    sFont = lstFonts[Rand.Next(0, 5)];
                    if (IsFontInstalled(sFont))
                        break;
                }
                FontFamily fFamily = new FontFamily(sFont);
                Font fFont = new Font(fFamily, 20);

                lblCaptcha.Font = fFont;
                lblCaptcha.ForeColor = Color.FromArgb(0xFF, Rand.Next(0, 255), Rand.Next(0, 255), Rand.Next(0, 255));

                string sCaptcha = string.Empty;
                foreach (char c in sCaptchaString)//Add space between text 
                    sCaptcha += c + " ";

                lblCaptcha.Text = sCaptcha;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnCaptchaGo_Click(object sender, EventArgs e)
        {
            if (lblCaptcha.Text.Replace(" ", string.Empty).ToUpper() == txtCaptchaText.Text.ToUpper())
            {
                Captcha(false);
                if (dtPending != null && dtPending.Rows.Count > 0 && xGridUncertain.DataSource != null)
                    Save_Uncertains((DataTable)xGridUncertain.DataSource);
            }
            else
            {
                ToastNotification.Show(this, "Invalid Text",eToastPosition.BottomRight);
                txtCaptchaText.Text = string.Empty;
                GetCaptcha();
            }
        }

        private void btnCaptchaCancel_Click(object sender, EventArgs e)
        {
            Captcha(false);
        }

        private void btnAccpetChecked_Click(object sender, EventArgs e)
        {
            
            Captcha(true);
            //if (dtPending != null && dtPending.Rows.Count > 0 && xGridUncertain.DataSource != null)
            //    Save_Uncertains((DataTable)xGridUncertain.DataSource);
        }

        void Captcha(bool ShowCaptcha)
        {
            if (ShowCaptcha)
            {
                lblCaptcha.Visible = txtCaptchaText.Visible = btnCaptchaGo.Visible = btnCaptchaCancel.Visible = true;
                txtCaptchaText.Text = string.Empty;
                btnAccpetChecked.Visible = false;                
                GetCaptcha();
                txtCaptchaText.Focus();
            }
            else
            {
                lblCaptcha.Visible = txtCaptchaText.Visible = btnCaptchaGo.Visible = btnCaptchaCancel.Visible = false;
                btnAccpetChecked.Visible = true;
            }
        }

        private void txtCaptchaText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtCaptchaText.Visible)
            {
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                    btnCaptchaGo.PerformClick();
                else if (e.KeyChar == Convert.ToChar(Keys.Escape))
                    btnCaptchaCancel.PerformClick();
            }
        }


    }
}

