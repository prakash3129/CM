using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GCC
{
    public partial class frmPreviewFilter : DevComponents.DotNetBar.Office2007Form
    {
        public frmPreviewFilter()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
        }

        public string sSQLText //Query
        {
            get { return _sSQLText; }
            set { _sSQLText = value; }
        }

        private string _sFilterID = string.Empty;
        public string sFilterID 
        {
            get { return _sFilterID; }
            set { _sFilterID = value; }
        }


        private bool _IsNewRecord = true;
        public bool IsNewRecord
        {
            get { return _IsNewRecord; }
            set { _IsNewRecord = value; }
        }

        private bool _bTimeZoneEnabled =true;
        public bool bTimeZoneEnabled
        {
            get { return _bTimeZoneEnabled; }
            set { _bTimeZoneEnabled = value; }
        }

        private bool _bManualFilter = false;
        public bool bManualFilter
        {
            get { return _bManualFilter; }
            set { _bManualFilter = value; }
        }

        private string _sSQLText = string.Empty;
        string sTimeZone = "";
        //BAL_GlobalMyfdSQL objBAL_Global = new BAL_GlobalMySdfQL();
        private void PreviewFilter_Load(object sender, EventArgs e)
        {

            //DataTable dtTimeZoneRecords = new DataTable();
            //DataTable dtFilterViewTotalRecords = new DataTable();
            //DataTable dtFilterViewProcessedRecords = new DataTable();
            //DataTable dtFilterViewPendingRecords = new DataTable();
            if (sSQLText.Length > 0)
            {
                if (bManualFilter)
                {                    
                    this.Text = "Filter Details - (Manual Filter)";
                    
                    string sPrefix = "SELECT 'Count' AS 'Details', COUNT(DISTINCT Company.MASTER_ID) AS 'Records' FROM " + GV.sCompanyTable + " COMPANY ";

                    if (sSQLText.ToUpper().Contains("QC."))
                        sPrefix += " INNER JOIN " + GV.sContactTable + " CONTACT ON COMPANY.MASTER_ID = CONTACT.MASTER_ID INNER JOIN " + GV.sProjectID + "_QC QC ON QC.RECORDID = CONTACT.CONTACT_ID_P AND QC.TABLENAME = 'CONTACT' AND QC.RESEARCHTYPE = '" + GV.sAccessTo + "'";
                    else if (sSQLText.ToUpper().Contains("CONTACT."))
                        sPrefix += " INNER JOIN " + GV.sContactTable + " CONTACT ON CONTACT.MASTER_ID = COMPANY.MASTER_ID";


                    string sQuery = sPrefix + " WHERE " + sSQLText;
                    //string sQuery = "SELECT 'Count' AS 'Details', COUNT(DISTINCT Company.MASTER_ID) AS 'Records' FROM " + GV.sCompanyTable + " COMPANY INNER JOIN " + GV.sContactTable + " CONTACT ON COMPANY.MASTER_ID = CONTACT.MASTER_ID INNER JOIN " + GV.sProjectID + "_QC QC ON QC.RECORDID = CONTACT.CONTACT_ID_P AND QC.TABLENAME = 'CONTACT' AND QC.RESEARCHTYPE = '" + GV.sAccessTo + "' WHERE " + sSQLText;
                    DataTable dtCount = GV.MSSQL1.BAL_ExecuteQuery(sQuery);
                    dgvCount.DataSource = dtCount;

                    if (GV.HasAdminPermission)
                    {
                        txtQuery.Text = sQuery;
                        txtQuery.Visible = true;
                    }
                }
                else
                {                
                    DataTable dtContidion = GV.MSSQL1.BAL_FetchTable("c_allocation_filter_condition", "FILTER_ID=" + sFilterID);

                    //if (GlobalVariables.sAccessTo == "TR" && sSQLText.Contains("(TIME(DATE_ADD(NOW(), INTERVAL HoursFromGMT HOUR)) BETWEEN"))
                    //{
                    //    sTimeZone = sSQLText.Substring(sSQLText.IndexOf("(TIME(DATE_ADD(NOW(), INTERVAL HoursFromGMT HOUR)) BETWEEN") + "(TIME(DATE_ADD(NOW(), INTERVAL HoursFromGMT HOUR)) BETWEEN".Length, 20);
                    //}
                    string sUserTypeDateColumn = string.Empty;
                    if (GV.sAccessTo == "TR")
                        sUserTypeDateColumn = "TR_DATECALLED";
                    else if (GV.sAccessTo == "WR")
                        sUserTypeDateColumn = "WR_DATE_OF_PROCESS";

                    if (IsNewRecord)
                        this.Text = "Filter Details";
                    else
                        this.Text = "Filter Details - (Processed)";


                    // string sPrefix = "SELECT 'Details', COUNT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
                    string sPrefix = "SELECT 'Details' AS 'Details', COUNT(DISTINCT Company.MASTER_ID) AS 'Records' FROM " + GV.sCompanyTable + " COMPANY ";
                    DataTable dtTable = dtContidion.DefaultView.ToTable(true, "TABLE_NAME");
                    if (dtTable.Select("TABLE_NAME = 'QC'").Length > 0)
                        sPrefix += " INNER JOIN " + GV.sContactTable + " CONTACT ON COMPANY.MASTER_ID = CONTACT.MASTER_ID INNER JOIN " + GV.sProjectID + "_QC QC ON QC.RECORDID = CONTACT.CONTACT_ID_P AND QC.TABLENAME = 'CONTACT' AND QC.RESEARCHTYPE = '" + GV.sAccessTo + "'";
                    else if (dtTable.Select("TABLE_NAME = 'CONTACT'").Length > 0)
                        sPrefix += " INNER JOIN " + GV.sContactTable + " CONTACT ON CONTACT.MASTER_ID = COMPANY.MASTER_ID";


                    string sTotal = string.Empty;
                    string sProcessed = string.Empty;
                    string sPending = string.Empty;
                    string sCall = string.Empty;

                    // if (IsNewRecord)
                    {
                        sTotal = BuildQuery(dtContidion, "Total");
                        sProcessed = BuildQuery(dtContidion, "Processed");
                        sPending = BuildQuery(dtContidion, "Pending");
                        sCall = BuildQuery(dtContidion, "Call");
                    }
                    //else                
                    //{
                    //    this.Text = "Filter Details - (Processed)";
                    //    sTotal = BuildQuery(dtContidion, "Total");                                        
                    //    sCall = BuildQuery(dtContidion, "Call");
                    //}


                    string sQuery = string.Empty;
                    if (GV.sAccessTo == "TR")
                        sQuery = sPrefix.Replace("'Details' AS 'Details'", "'Can be Called Now' AS 'Details'") + " Where " + sCall;
                    else
                        sQuery = sPrefix.Replace("'Details' AS 'Details'", "'Can be Processed Now' AS 'Details'") + " Where " + sCall;

                    if (sPending.Length > 0)
                        sQuery += " UNION ALL " + sPrefix.Replace("'Details' AS 'Details'", "'Pending' AS 'Details'") + " Where " + sPending;
                    if (sProcessed.Length > 0)
                        sQuery += " UNION ALL " + sPrefix.Replace("'Details' AS 'Details'", "'Processed' AS 'Details'") + " Where " + sProcessed;
                    sQuery += " UNION ALL " + sPrefix.Replace("'Details' AS 'Details'", "'Total' AS 'Details'") + " Where " + sTotal;

                    if (sTotal.Length > 0)
                    {
                        DataTable dtCount = GV.MSSQL1.BAL_ExecuteQuery(sQuery);
                        if (GV.HasAdminPermission)
                        {
                            txtQuery.Text = sQuery;
                            txtQuery.Visible = true;
                        }
                        dgvCount.DataSource = dtCount;
                    }
                }                
            }
        }

        private string BuildQuery(DataTable dtCondition,string sQueryType)//Build Qurery based on Conditions
        {
            string sSQLQuery = "";
            try
            {
                if (dtCondition != null && dtCondition.Rows.Count > 0)
                {
                    foreach (DataRow drCondition in dtCondition.Rows)//Loop through rows (Append 'AND' betwwen rows)
                    {
                        List<string> lstValue = new List<string>();
                        string sCondition = drCondition["CONDITION"].ToString();
                        string sFieldNameQuery = "ISNULL(" + drCondition["TABLE_NAME"].ToString() + "." + drCondition["FIELD"] + ",'')";
                        string sInnerCondition = string.Empty;

                        lstValue = drCondition["VALUE"].ToString().Replace("'", "''").Split(',').ToList();
                        if (sSQLQuery.Length > 0)
                            sSQLQuery += " AND ";
                        sSQLQuery += "(";


                        if (sCondition == "Between")
                        {
                            List<string> lstDate = drCondition["VALUE"].ToString().Split(new string[] { " AND " }, StringSplitOptions.None).ToList();
                            sSQLQuery += sFieldNameQuery + " BETWEEN '" + lstDate[0] + " 00:00:00' AND '" + lstDate[1] + " 23:59:59'";
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
                            sSQLQuery += sInnerCondition + ")";
                        else
                            sSQLQuery += sInnerCondition + "))";
                    }
                }

                if (sSQLQuery.Length > 0)
                {
                    string sUserTypeDateColumn = "";
                    if (GV.sAccessTo == "TR")
                        sUserTypeDateColumn = "TR_DATECALLED";
                    else
                        sUserTypeDateColumn = "WR_DATE_OF_PROCESS";

                    switch (sQueryType)
                    {
                        case "Total":

                            if (!IsNewRecord)
                            {
                                //if (dtCondition.Select("Field = '" + GV.sAccessTo + "_PRIMARY_DISPOSAL" + "'").Length > 0)
                                    sSQLQuery += " AND cast(" + sUserTypeDateColumn + " AS DATE) <= cast(GETDATE() as date) ";
                                //else
                                //    sSQLQuery += " AND DATE(" + sUserTypeDateColumn + ") <= CURDATE() AND IFNULL(" + GV.sAccessTo + "_PRIMARY_DISPOSAL,'') NOT IN (SELECT Primary_Status FROM " + GV.sProjectID + "_recordstatus WHERE TABLE_NAME='COMPANY' AND Research_Type='" + GV.sAccessTo + "' AND Operation_Type LIKE '%Freeze%')";
                            }

                        break;

                        case "Pending":
                            if (dtCondition.Select("FIELD = 'EMAIL_VERIFIED'").Length > 0)
                                sSQLQuery += " AND EMAIL_VERIFIED = 'BOUNCED'";//This may be ambigious in query but it is must.. There are chances that query can be formed like Email_Verified = 'Not Verified' which is wrong
                            else
                            {
                                //if (dtCondition.Select("FIELD IN ('" + GV.sAccessTo + "_PRIMARY_DISPOSAL','" + GV.sAccessTo + "_SECONDARY_DISPOSAL')").Length > 0)
                                //    sSQLQuery += " AND " + sUserTypeDateColumn + " < CURDATE()";
                                //else
                                //    sSQLQuery += " AND " + sUserTypeDateColumn + " IS NULL";


                                if (IsNewRecord)
                                    sSQLQuery += " AND " + sUserTypeDateColumn + " IS NULL";
                                else
                                    if (dtCondition.Select("Field = '" + GV.sAccessTo + "_PRIMARY_DISPOSAL" + "'").Length > 0)
                                        sSQLQuery += " AND " + sUserTypeDateColumn + " < CAST(GETDATE() as date) ";
                                    else
                                        sSQLQuery += " AND " + sUserTypeDateColumn + " < CAST(GETDATE() as date) AND ISNULL(" + GV.sAccessTo + "_PRIMARY_DISPOSAL,'') NOT IN (SELECT Primary_Status FROM " + GV.sProjectID + "_recordstatus WHERE TABLE_NAME='COMPANY' AND Research_Type='" + GV.sAccessTo + "' AND Operation_Type LIKE '%Freeze%')";
                            }
                        break;

                        case "Processed":
                        //if (dtCondition.Select("FIELD = 'EMAIL_VERIFIED'").Length > 0)
                        //    sSQLQuery += " AND EMAIL_VERIFIED = 'BOUNCED' AND " + sUserTypeDateColumn + " IS NOT NULL";//This may be ambigious in query but it is must.. There are chances that query can be formed like Email_Verified = 'Not Verified' which is wrong
                        //else
                            

                            if (IsNewRecord)
                                sSQLQuery += " AND " + sUserTypeDateColumn + " IS NOT NULL";
                            else
                            {
                                //if (dtCondition.Select("Field = '" + GV.sAccessTo + "_PRIMARY_DISPOSAL" + "'").Length > 0)
                                    sSQLQuery += " AND CAST(" + sUserTypeDateColumn + " AS DATE) = CAST(GETDATE() as date) ";
                                //else
                                //    sSQLQuery += " AND " + sUserTypeDateColumn + " < CURDATE() AND IFNULL(" + GV.sAccessTo + "_PRIMARY_DISPOSAL,'') NOT IN (SELECT Primary_Status FROM " + GV.sProjectID + "_recordstatus WHERE TABLE_NAME='COMPANY' AND Research_Type='" + GV.sAccessTo + "' AND Operation_Type LIKE '%Freeze%')";
                            }
                        break;

                        case "Call":
                            if (dtCondition.Select("FIELD = 'EMAIL_VERIFIED'").Length > 0)
                                sSQLQuery += " AND EMAIL_VERIFIED = 'BOUNCED'";//This may be ambigious in query but it is must.. There are chances that query can be formed like Email_Verified = 'Not Verified' which is wrong
                            else
                            {
                                if (IsNewRecord)
                                    sSQLQuery += " AND " + sUserTypeDateColumn + " IS NULL";
                                else
                                    if (dtCondition.Select("Field = '" + GV.sAccessTo + "_PRIMARY_DISPOSAL" + "'").Length > 0)
                                        sSQLQuery += " AND " + sUserTypeDateColumn + " < CAST(GETDATE() as date) ";
                                    else
                                        sSQLQuery += " AND " + sUserTypeDateColumn + " < CAST(GETDATE() as date) AND ISNULL(" + GV.sAccessTo + "_PRIMARY_DISPOSAL,'') NOT IN (SELECT Primary_Status FROM " + GV.sProjectID + "_recordstatus WHERE TABLE_NAME='COMPANY' AND Research_Type='" + GV.sAccessTo + "' AND Operation_Type LIKE '%Freeze%')";

                                //if (dtCondition.Select("FIELD IN ('" + GV.sAccessTo + "_PRIMARY_DISPOSAL','" + GV.sAccessTo + "_SECONDARY_DISPOSAL')").Length > 0)
                                //    sSQLQuery += " AND " + sUserTypeDateColumn + " < CURDATE()";
                                //else
                                //    sSQLQuery += " AND " + sUserTypeDateColumn + " IS NULL";
                            }

                            if (GV.sAccessTo == "TR" && bTimeZoneEnabled)
                                sSQLQuery += "  AND (CAST(dateadd(HOUR, HoursFromGMT, getdate()) AS TIME) BETWEEN '08:00' AND '17:00')"; //Filter Based on TimeZone
                            //sSQLQuery += "  AND (TIME(DATE_ADD(NOW(), INTERVAL HoursFromGMT HOUR)) BETWEEN '08:00' AND '17:00')"; //Filter Based on TimeZone

                            break;
                    }
                    sSQLQuery += " AND FLAG = '" + GV.sAccessTo + "'"; //Set flag
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return sSQLQuery;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            /////Key press listener for overall window/////
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dgvPreview_DataSourceChanged(object sender, EventArgs e)
        {
            DataGridView dgvGenrel = sender as DataGridView;
            foreach (DataGridViewColumn dgvc in dgvGenrel.Columns)//Show only required Columns
            {
                dgvc.Visible = false;
                //if(GlobalVariables.lstShowOnGridMasterCompanies.Contains(dgvc.Name.ToUpper()))
                if (dgvc.Name == "COMPANY_NAME" || dgvc.Name == "COUNTRY" || dgvc.Name == "SWITCHBOARD"  || dgvc.Name == "MASTER_ID"  || dgvc.Name == "TR_PRIMARY_DISPOSAL" || dgvc.Name == "WR_PRIMARY_DISPOSAL" || dgvc.Name == "TR_DATECALLED" || dgvc.Name == "WR_DATE_OF_PROCESS")
                {
                    dgvc.HeaderText = GM.ProperCaseLeaveCapital(dgvc.Name.Replace("_", " "));
                    dgvc.Visible = true;
                }
            }
        }

        //private void Get_CountryTime()
        //{
        //    //lblTime.Text = ""

        //    DataTable dtTimeZone = new DataTable();
        //    string query = "select remarks from PickLists where picklistcategory='Country_List' and PickListField='" + cmbCountry.Text.Replace("'", "''") + "' and LEN(remarks)>0 ";
        //    dtTimeZone.Clear();
        //    Execute_Query(query, dtTimeZone, sResult);
        //    if (dtTimeZone.Rows.Count > 0)
        //    {
        //        lblTime.Text = GlobalMethods.GetDateTime().ToUniversalTime.AddHours(Convert.ToDecimal(dtTimeZone.Rows(0).Item(0))).ToString("HH:mm:ss");
        //    }
        //    else
        //    {
        //        lblTime.Text = "";
        //    }
        //    if (sResult.Length > 0)
        //    {
        //        MessageBox.Show(sResult, this.Text);
        //        return;
        //    }
        //}

    }
}
