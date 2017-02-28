using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Net;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.SuperGrid.Style;
using System.Text;
using System.Management;
using i00SpellCheck;
using CustomText;
using NAudio.Wave;
using Google.Apis.CloudSpeechAPI.v1beta1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using DirectShowLib;

namespace GCC
{
    //-----------------------------------------------------------------------------------------------------
    public partial class FrmContactsUpdate : Office2007Form
    {
        //public FrmContactsUpdate()
        //{
        //    ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
        //    ToastNotification.DefaultTimeoutInterval = 2000;
        //    ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
        //    InitializeComponent();g

        //    this.MergeContacts.Parent = this;
        //    mLastState = this.WindowState;valid
        //    InitializeCustomComponent();
        //    //this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        //    //this.UpdateStyles();

        //}
        Form frmMDI;

        //-----------------------------------------------------------------------------------------------------
        public FrmContactsUpdate(string sID, Form frmMDIParant, string sOpenType, bool IsNewComp, frmCompanyList CompanyList)
        {
            GV.sPerformance += "Starting Component Initilize : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
            frmMDI = frmMDIParant;
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
            //this.MdiParent = frmMDIParant;
            this.Dock = DockStyle.Fill;
            this.sMaster_IDForAdminOpen = sID;
            //this.objfrmCompanyList = (frmCompanyList)frmCompany;
            this.sFormOpenType = sOpenType;
            this.IsNewCompany = IsNewComp;
            this.MdiParent = frmMDIParant;
            CL = CompanyList;            

            

            dtPicklist_Insert.Columns.Add("PicklistCategory");
            dtPicklist_Insert.Columns.Add("Value");

            dtPicklist_Delete.Columns.Add("PicklistCategory");
            dtPicklist_Delete.Columns.Add("Value");

            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
            MergeContacts.Parent = this;

            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;

            mLastState = this.WindowState;
            objNotifier.msgRefresh.Stop();

            //dtUncertainFields.Columns.Add("FieldName");
            //dtUncertainFields.Columns.Add("FieldName_LinkColumn");
            //dtUncertainFields.Columns.Add("PickList_Category");

            GV.sPerformance += "Ending Component Initilize : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

            InitializeCustomComponent();
            lblCallEvents.Text = string.Empty;
            txtTotalDials.TextBox.ShortcutsEnabled = false;
            switchTRWR.Value = (GV.sAccessTo == "TR");
            btnGSpeechRecord.Visible = (GV.sAccessTo == "TR" && GV.AudioComments);
        }

        void f_ResizeEnd(object sender, EventArgs e)
        {
            MdiClient mc = this.Controls.OfType<MdiClient>().First();
            Form f = sender as Form;
            if (f.Right >= mc.ClientSize.Width)
            {
                f.SetBounds(mc.ClientSize.Width / 2, 0,
                mc.ClientSize.Width / 2, mc.ClientSize.Height);
            }
        }

        //protected override CreateParams CreateParams//Visual Flikering
        //{
        //    get
        //    {
        //        var cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
        //        return cp;
        //    }
        //}



        private string _sFormOpenType;
        //-----------------------------------------------------------------------------------------------------
        public string sFormOpenType
        {
            get { return _sFormOpenType; }
            set { _sFormOpenType = value; }
        }

        private string _sMasterIdForAdminOpen;
        //-----------------------------------------------------------------------------------------------------
        public string sMaster_IDForAdminOpen
        {
            get { return _sMasterIdForAdminOpen; }
            set { _sMasterIdForAdminOpen = value; }
        }

        private TabStrip _tabStrip;
        //-----------------------------------------------------------------------------------------------------
        public TabStrip tabStrip
        {
            get { return _tabStrip; }
            set { _tabStrip = value; }
        }

        private bool _IsNewCompany;
        //-----------------------------------------------------------------------------------------------------
        public bool IsNewCompany
        {
            get { return _IsNewCompany; }
            set { _IsNewCompany = value; }
        }

        //private frmCompanyList _objfrmCompanyList;
        ////-----------------------------------------------------------------------------------------------------
        //public frmCompanyList objfrmCompanyList
        //{
        //    get { return _objfrmCompanyList; }
        //    set { _objfrmCompanyList = value; }
        //}

        bool IsShiftPressed = false;


        //WebReference.Service1 objWebServices = new WebReference.Service1();
        string sTab = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;•&nbsp;";
        private bool IsNewCompletesAdded = false;
        private FormWindowState mLastState;
        public static bool IsNoteFormOpened = false;
        DataTable dtFieldMaster_Active;
        DataTable dtFieldMasterAllColumns;
        DataTable dtFieldMasterChromeCol;
        DataTable dtFieldMasterCompany = new DataTable(); //Master Table Structure
        DataTable dtFieldMasterContact = new DataTable(); //Master Contacts Structure
                                                          // DataRow[] drrUncertain_Fields;
        DataTable dtUncertainFields = new DataTable();
        frmNotifier objNotifier = new frmNotifier();
        DataTable dtMasterCompanies = new DataTable(); //Master Companies Data
        public DataTable dtMasterContacts = new DataTable(); //Master Contacts Data

        //DataTable dtMasterCompaniesSQLCE = new DataTable();
        //DataTable dtMasterContactsSQLCE = new DataTable();

        DataTable dtMasterContactsCopy = new DataTable(); //Master Contacts Copy
        DataTable dtMasterCompaniesCopy = new DataTable(); //Master Companies Data
        DataTable dtPicklist = new DataTable(); // Picklist Table
        DataTable dtRecordStatus = new DataTable();
        DataTable dtRecordStatusRevenue = new DataTable();
        DataTable dtValidations = new DataTable(); // Validations Table

        DataTable dtProjectLog = new System.Data.DataTable();
        DataTable dtCompanyContact_Log = new DataTable();

        DataTable dtNameSayer = new DataTable();

        bool HistoryLoaded = false;

        DataTable dtCountryInformation = new DataTable();
        DataTable dtEmailSuggestion = new DataTable();
        DataTable dtPreUpdate = new DataTable();
        DataTable dtFilter = new DataTable();
        DataTable dtSpellIgnore = new DataTable();
        DataTable dtValidationResults;
        DataTable dtValidationResultsDynamic;
        DataTable dtQCPicklist;
        DataTable dtQCWeekIDs;
        DataTable dtQCTable;
        private DataTable dtEmailChecks;
        DataTable dtHRIMSQCDetails;
        DataTable dtDialLogger;
        DataTable dtDialConfig;
        DataTable dtBlock;

        List<int> lstFreezedContactIDs = new List<int>();
        List<int> lstFreezedMasterIDs = new List<int>();
        List<int> lstBouncedContactIDs = new List<int>();
        List<int> lstSendBackRecords = new List<int>();
        List<int> lstRejectedRecords = new List<int>();
        List<int> lstRecordsToUnlock = new List<int>();
        List<int> lstQCOKIDs = new List<int>();
        List<string> lstMasterIDs = new List<string>();
        List<string> lstApplicationError = new List<string>();
        List<string> lstSpellCheckIgnore = new List<string>();
        List<string> lstQCIDsToLog = new List<string>();
        List<DataRow> lstQCRowsToDelete = new List<DataRow>();

        string sFilterHeader = string.Empty;
        string sBrowserHTML = string.Empty;
        List<string> lstValidationTypeToIgnore = new List<string>() { "EQUALS", "NOTEQUALS", "STARTSWITH", "ENDSWITH", "CONTAINS", "NOTCONTAINS" };
        BackgroundWorker EmailWorker = new BackgroundWorker();
        //BackgroundWorker DataValidator = new BackgroundWorker();
        bool IsTimeZoneEnabled = true;
        bool IsSuperGridLoading = true;
        bool CallinProgress = false;
        string sDoFilter = string.Empty;
        Assembly EAF;
        object EAF_ClassInstance;
        Type EAF_MethodInstance;
        Stopwatch sWatch;
        bool CallLimitExceeded = false;
        public string sESAudioID = string.Empty;
        public string sESPhonetics = string.Empty;
        public string sCurrentESName = string.Empty;
        public string sCurrentAudioPath = string.Empty;
        public string sCommentText = string.Empty;
        public string sCommentError = string.Empty;

        int iRequestWaitTime = 0;

        string sRequestType = string.Empty;

        string sNAudioPath = string.Empty;

        string sCheckingEmail = string.Empty;
        //DataTable dtTelephoneFormat = new DataTable();
        DateTime tCurrentCountryTime;
        //DateTimePicker dtp = new DateTimePicker();
        //BAL_GlobalMySQL objBAL_Global = new BAL_GlobalMySQL();
        frmCallScript objfrmCallScript;
        System.Windows.Forms.Timer t;
        string sMaster_ID = string.Empty;
        string sGroup_ID = string.Empty;
        string sCurrentCallingNo = string.Empty;
        DataTable dtPicklist_Insert = new DataTable();
        DataTable dtPicklist_Delete = new DataTable();
        frmWindowedNotes objFrmWindowedNotes;
        private frmCompanyList CL;
        bool bNAudioStarted = false;


        public WaveIn waveSource;
        public WaveFileWriter waveFile = null;
        
        public List<TextBox> lstCompanyControls;
        public List<TextBox> lstContactControls;

        Timer tAutoSaveRecords;
        int iContactRowIndex = -1;
        int iCompanyRowIndex = -1;
        int iNameSayerRowIndex = -1;

        double iGMTHours = 0;

        bool IsLoading = false;

        //bool IsTableToTextContact = false;
        //bool IsTableToTextCompany = false;

        bool IsRecordFetchedFlag;
        bool IsRecordSaved = false;
        bool IsOutOfTimeZone = false;
        string sTimeLoggerMessage = string.Empty;
        string sRecordFetchMessage = string.Empty;
        string sSwitchboard_Trimmed = string.Empty;
        int iPreCall = 0;
        int iPostCall = 0;

        string sRecoveryPath = AppDomain.CurrentDomain.BaseDirectory + "\\Campaign Manager\\Logs\\Data\\" + GV.sEmployeeNo + "\\" + GV.sProjectID;

        bool IsPreCall = true;

        //-----------------------------------------------------------------------------------------------------
        private bool IsRecordFetched()
        {
            try
            {
                

                sTimeLoggerMessage = string.Empty;
                sRecordFetchMessage = string.Empty;
                IsRecordFetchedFlag = false;

                iCompanyRowIndex = 0;
                ReloadChart();//Loads or reloads the performance charts and Images                

                if (GV.sUserType == "Agent")
                {
                    #region Agent Open
                    sTimeLoggerMessage = GM.IsTimeLoggerHasProject();
                    if (sTimeLoggerMessage.Length > 0)
                    {
                        IsRecordFetchedFlag = false;
                        return false;
                    }

                    sMaster_ID = string.Empty;
                    string sUserTypeName = string.Empty;
                    string sEliminateCompanyCompletes = string.Empty;
                    bool IsRandom = false;
                    bool IsManualFilter = false;
                    string sDateColumn = string.Empty;
                    string sDisposalColumn = string.Empty;
                    string sFilterID = string.Empty;
                    string sSQLTEXTConditionOnly = string.Empty;

                    #region Eliminate Opposit Research Company completes
                    if (GV.sAccessTo == "TR")
                    {
                        sUserTypeName = "TR_AGENTNAME";
                        sDateColumn = "TR_DATECALLED";
                        if (GV.sFreezeWRCompanyCompletes == "Y")
                        {
                            sEliminateCompanyCompletes = " AND IFNULL(WR_PRIMARY_DISPOSAL,'') NOT IN (SELECT cp.Primary_Status FROM " + GV.sProjectID + "_RecordStatus cp WHERE cp.Table_Name = 'Company'  AND cp.Research_Type='WR' AND cp.Operation_Type LIKE '%Validate%')";
                            sDisposalColumn = "WR_PRIMARY_DISPOSAL,";
                        }
                    }
                    else if (GV.sAccessTo == "WR")
                    {
                        sUserTypeName = "WR_AGENTNAME";
                        sDateColumn = "WR_DATE_OF_PROCESS";
                        if (GV.sFreezeTRCompanyCompletes == "Y")
                        {
                            sEliminateCompanyCompletes = " AND IFNULL(TR_PRIMARY_DISPOSAL,'') NOT IN (SELECT cp.Primary_Status FROM " + GV.sProjectID + "_RecordStatus cp WHERE cp.Table_Name = 'Company' AND cp.Research_Type='TR' AND cp.Operation_Type LIKE '%Validate%')";
                            sDisposalColumn = "TR_PRIMARY_DISPOSAL,";
                        }
                    }
                    #endregion

                    #region Sendback
                    if (sFormOpenType == "SendBack")
                    {
                        string sQuery = "SELECT c.GROUP_ID FROM " + GV.sCompanyTable + " c";
                        sQuery += " INNER JOIN " + GV.sContactTable + " a ON c.MASTER_ID = a.MASTER_ID";
                        sQuery += " INNER JOIN " + GV.sQCTable + " b ON ((b.TableName = 'Contact' AND a.CONTACT_ID_P = b.RecordID) OR (b.TableName='Company' AND c.MASTER_ID = b.RecordID))";
                        sQuery += " WHERE ((b.TableName ='Company' AND c." + GV.sAccessTo + "_AGENTNAME ='" + GV.sEmployeeName + "') OR (b.TableName ='Contact' AND a." + GV.sAccessTo + "_AGENT_NAME ='" + GV.sEmployeeName + "'))  AND b.QC_Status = 'SendBack' AND b.ResearchType = '" + GV.sAccessTo + "' LIMIT 1;";

                        ((frmMain)frmMDI).txtQuery.Text = sQuery;
                        DataTable dtSendBackID = GV.MYSQL.BAL_ExecuteQueryMySQL(sQuery);

                        if (dtSendBackID.Rows.Count > 0)
                        {
                            sMaster_ID = dtSendBackID.Rows[0]["GROUP_ID"].ToString();


                            GV.MYSQL.BAL_ExecuteNonReturnQueryMySQL("UPDATE " + GV.sCompanyTable + " set " + sUserTypeName + " = 'Current_" + GV.sEmployeeName + "' WHERE GROUP_ID=" + sMaster_ID);


                            dtMasterCompanies = GV.MYSQL.BAL_FetchTableMySQL(GV.sCompanyTable, "GROUP_ID = " + sMaster_ID);
                            dtMasterCompanies.TableName = "MasterCompanies";

                            LoadMasterIds();
                            string sMasterIDs = GM.ColumnToQString("MASTER_ID", dtMasterCompanies, "Int");


                            dtMasterContacts = GV.MYSQL.BAL_FetchTableMySQL(GV.sContactTable, "MASTER_ID IN (" + sMasterIDs + ")"); //MasterContact Table
                            dtMasterContacts.TableName = "MasterContact";

                            string sContactIDs = GM.ColumnToQString("CONTACT_ID_P", dtMasterContacts, "Int");
                            if (sContactIDs.Length == 0)
                                sContactIDs = "0";


                            dtQCTable = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM " + GV.sQCTable + " WHERE (TableName='Contact' AND RecordID IN (" + sContactIDs + ")) OR (TableName='Company' AND RecordID IN (" + sMasterIDs + ")) AND ResearchType = '" + GV.sAccessTo + "';");
                            //dtSendBack = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM " + GV.sQCTable + " WHERE QC_Status ='SendBack' AND ResearchType = '" + GV.sAccessTo + "' AND (TableName = 'Company' AND RecordID IN (SELECT MASTER_ID FROM " + GV.sCompanyTable + " WHERE GROUP_ID=112)) OR (TableName = 'Contact' AND RecordID IN (SELECT CONTACT_ID_P FROM " + GV.sContactTable + " WHERE MASTER_ID IN (SELECT MASTER_ID FROM " + GV.sCompanyTable + " WHERE GROUP_ID=112)));");
                            dtQCTable.TableName = "QC";

                            dtEmailChecks = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM c_email_checks WHERE PROJECT_ID = '" + GV.sProjectID + "' AND CONTACT_ID IN (" + sContactIDs + ") AND EMAIL_SOURCE = 0;");
                            dtEmailChecks.TableName = "EMAIL_CHECKS";

                            GetDialerHistory();

                            IsRecordSaved = false;//New record possibly loaded which is not saved yet.
                            IsRecordFetchedFlag = true;


                            return true;
                        }
                        else
                        {

                            IsRecordFetchedFlag = false;
                            sRecordFetchMessage = "No Sendbacks found.";
                            return false;
                        }
                    }
                    #endregion

                    List<string> lstAllocation = CurrentFilter_Query();
                    if (IsNewCompany || sFormOpenType == "ListOpen" || (lstAllocation.Count > 0))
                    {
                        #region Get Filter Details and Add Timezone and Next Call date
                        DataTable dtColumnsUsed = null;
                        if (lstAllocation.Count > 0)
                        {
                            sFilterID = lstAllocation[0];
                            IsRandom = (lstAllocation[1] == "Y");
                            IsTimeZoneEnabled = (lstAllocation[2] == "Y");
                            sSQLTEXTConditionOnly = lstAllocation[3] + sEliminateCompanyCompletes + " AND Company.MASTER_ID = Company.GROUP_ID";
                            IsManualFilter = (lstAllocation[4] == "N");
                            if (!IsManualFilter)
                                dtColumnsUsed = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT TABLE_NAME, FIELD FROM mvc.allocation_filter_condition WHERE FILTER_ID = " + sFilterID + ";");

                            if (IsTimeZoneEnabled && GV.sAccessTo == "TR")
                            {
                                sSQLTEXTConditionOnly += " AND (TIME(DATE_ADD(NOW(), INTERVAL HoursFromGMT HOUR)) BETWEEN '08:00' AND '17:00') AND CALLS_ALLOWED_FROM <= CURDATE()"; //Filter Based on TimeZone
                            }
                        }
                        #endregion

                        while (true)
                        {
                            #region Check if record is opened                            
                            if (sMaster_ID.Length == 0)
                            {

                                dtMasterCompanies = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT GROUP_ID FROM " + GV.sCompanyTable + " WHERE " + sUserTypeName + " = 'Current_" + GV.sEmployeeName + "' LIMIT 1;");
                                if (dtMasterCompanies.Rows.Count > 0)
                                {
                                    sMaster_ID = dtMasterCompanies.Rows[0]["GROUP_ID"].ToString();
                                }
                            }

                            if (sMaster_ID.Length > 0)
                            {
                                dtMasterCompanies = GV.MYSQL.BAL_FetchTableMySQL(GV.sCompanyTable, "GROUP_ID = " + sMaster_ID);
                                dtMasterCompanies.TableName = "MasterCompanies";

                                LoadMasterIds();
                                string sMasterIDs = GM.ColumnToQString("MASTER_ID", dtMasterCompanies, "Int");


                                dtMasterContacts = GV.MYSQL.BAL_FetchTableMySQL(GV.sContactTable, "MASTER_ID IN (" + sMasterIDs + ")"); //MasterContact Table
                                dtMasterContacts.TableName = "MasterContact";


                                string sContactIDs = GM.ColumnToQString("CONTACT_ID_P", dtMasterContacts, "Int");


                                if (sContactIDs.Length == 0)
                                {

                                    sContactIDs = "0";
                                }


                                dtQCTable = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM " + GV.sQCTable + " WHERE (TableName='Contact' AND RecordID IN (" + sContactIDs + ")) OR (TableName='Company' AND RecordID IN (" + sMasterIDs + ")) AND ResearchType = '" + GV.sAccessTo + "';");
                                dtQCTable.TableName = "QC";

                                dtEmailChecks = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM c_email_checks WHERE PROJECT_ID = '" + GV.sProjectID + "' AND CONTACT_ID IN (" + sContactIDs + ") AND EMAIL_SOURCE = 0;");
                                dtEmailChecks.TableName = "EMAIL_CHECKS";

                                GetDialerHistory();

                                IsRecordFetchedFlag = true;
                                IsRecordSaved = false;//New record possibly loaded which is not saved yet.                                
                                return true;
                            }
                            #endregion

                            #region Open a New record
                            else // Search For New record
                            {
                                string sSQLUpdateQuery = string.Empty;
                                if (sUserTypeName.Length > 0 && sFilterID.Length > 0)
                                {
                                    string sPrefix = string.Empty;
                                    if (IsManualFilter)
                                    {
                                        sPrefix = "SELECT DISTINCT Company.Group_ID FROM " + GV.sCompanyTable + " Company ";
                                        if (sSQLTEXTConditionOnly.ToLower().Contains("qc."))
                                            sPrefix += " INNER JOIN " + GV.sContactTable + " CONTACT ON COMPANY.MASTER_ID = CONTACT.MASTER_ID INNER JOIN " + GV.sProjectID + "_QC QC ON QC.RECORDID = CONTACT.CONTACT_ID_P AND QC.TABLENAME = 'CONTACT' AND QC.RESEARCHTYPE = '" + GV.sAccessTo + "'";
                                        else if (sSQLTEXTConditionOnly.ToLower().Contains("contact."))
                                            sPrefix += " INNER JOIN " + GV.sContactTable + " CONTACT ON CONTACT.MASTER_ID = COMPANY.MASTER_ID";
                                    }
                                    else
                                    {
                                        if (IsRandom)
                                        {
                                            #region Random Record Query

                                            if (dtColumnsUsed != null && dtColumnsUsed.Rows.Count > 0)
                                            {
                                                string sColumnsUsed = string.Empty;
                                                List<string> lstHardCodedColumns = new List<string>();
                                                lstHardCodedColumns.Add(sDateColumn);
                                                lstHardCodedColumns.Add("FLAG");
                                                lstHardCodedColumns.Add(sUserTypeName);
                                                lstHardCodedColumns.Add(sDisposalColumn);
                                                lstHardCodedColumns.Add("MASTER_ID");
                                                lstHardCodedColumns.Add("HoursFromGMT");

                                                foreach (DataRow dr in dtColumnsUsed.Rows)
                                                {
                                                    if (!lstHardCodedColumns.Contains(dr["FIELD"].ToString(), StringComparer.OrdinalIgnoreCase))
                                                        sColumnsUsed += "," + dr["TABLE_NAME"] + "." + dr["FIELD"];
                                                }
                                                //if (sColumnsUsed.Trim().Length == 0)
                                                Random rnd = new Random();
                                                sPrefix = "SELECT DISTINCT Company.Group_ID  FROM (select Group_ID, MASTER_ID," + sDisposalColumn + "HoursFromGMT," + sDateColumn + "," + sUserTypeName + ",FLAG, REVERSE(RIGHT(UUID_SHORT()," + rnd.Next(2, 10) + "))+master_id as rn " + sColumnsUsed + " from " + GV.sCompanyTable + " order by rn)  Company LEFT OUTER JOIN " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID ";

                                                //else
                                                //    sPrefix = "SELECT DISTINCT Company.Master_ID  FROM (select " + sDisposalColumn + "HoursFromGMT," + sDateColumn + "," + sUserType + ",FLAG," + sDisposalColumn + ", REVERSE(RIGHT(UUID_SHORT(),10))+master_id as rn from " + GlobalVariables.sCompanyTable + " order by rn)  Company LEFT OUTER JOIN " + GlobalVariables.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            // sPrefix = "SELECT DISTINCT Company.Group_ID FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID";
                                            sPrefix = "SELECT DISTINCT Company.Group_ID FROM " + GV.sCompanyTable + " Company ";
                                            DataTable dtTable = dtColumnsUsed.DefaultView.ToTable(true, "TABLE_NAME");
                                            if (dtTable.Select("TABLE_NAME = 'QC'").Length > 0)
                                                sPrefix += " INNER JOIN " + GV.sContactTable + " CONTACT ON COMPANY.MASTER_ID = CONTACT.MASTER_ID INNER JOIN " + GV.sProjectID + "_QC QC ON QC.RECORDID = CONTACT.CONTACT_ID_P AND QC.TABLENAME = 'CONTACT' AND QC.RESEARCHTYPE = '" + GV.sAccessTo + "'";
                                            else if (dtTable.Select("TABLE_NAME = 'CONTACT'").Length > 0)
                                                sPrefix += " INNER JOIN " + GV.sContactTable + " CONTACT ON CONTACT.MASTER_ID = COMPANY.MASTER_ID";
                                        }
                                    }

                                    if (GV.Override_UserAccess)
                                        sSQLUpdateQuery = "UPDATE " + GV.sCompanyTable + " AS A INNER JOIN (" + sPrefix + " WHERE FLAG IN ('TR','WR') AND " + sSQLTEXTConditionOnly + " AND (IFNULL(TR_AGENTNAME,'') NOT LIKE 'Current_%' AND IFNULL(WR_AGENTNAME,'') NOT LIKE 'Current_%') LIMIT 1) AS B ON A.Group_ID = B.Group_ID SET A." + sUserTypeName + " = 'Current_" + GV.sEmployeeName + "' WHERE LENGTH(B.Group_ID) > 0; SELECT GROUP_ID FROM " + GV.sCompanyTable + " WHERE " + sUserTypeName + " = 'Current_" + GV.sEmployeeName + "';";
                                    else
                                        sSQLUpdateQuery = "UPDATE " + GV.sCompanyTable + " AS A INNER JOIN (" + sPrefix + " WHERE FLAG = '" + GV.sAccessTo + "' AND " + sSQLTEXTConditionOnly + " AND IFNULL(" + GV.sAccessTo + "_AGENTNAME,'') NOT LIKE 'Current_%' LIMIT 1) AS B ON A.Group_ID = B.Group_ID SET A." + sUserTypeName + " = 'Current_" + GV.sEmployeeName + "' WHERE LENGTH(B.Group_ID) > 0; SELECT GROUP_ID FROM " + GV.sCompanyTable + " WHERE " + sUserTypeName + " = 'Current_" + GV.sEmployeeName + "';";
                                    //string sPrefix = "SELECT DISTINCT Company.* FROM " + GlobalVariables.sCompanyTable + " Company ";// LEFT OUTER JOIN " + GlobalVariables.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID";
                                    //sSQLUpdateQuery = "DECLARE @OUTPUT TABLE(MASTERID BIGINT,AGENTNAME VARCHAR(100)) UPDATE " + GlobalVariables.sCompanyTable + " SET " + sUserType + " ='Current_" + GlobalVariables.sEmployeeName + "' OUTPUT DELETED.MASTER_ID,DELETED."+sUserType+" INTO @OUTPUT WHERE MASTER_ID IN(SELECT TOP 1 ID FROM " + GlobalVariables.sFilterViewName + " Where " + sSQLTEXTConditionOnly + " AND ISNULL("+sUserType+",'') NOT LIKE 'Current_%' ORDER BY TR_DATECALLED) AND LEN(MASTER_ID)>0  SELECT * FROM @OUTPUT";
                                    ((frmMain)frmMDI).txtQuery.Text = sSQLUpdateQuery;
                                    DataTable dtUpdateCheck = GV.MYSQL.BAL_ExecuteQueryMySQL(sSQLUpdateQuery);
                                    if (dtUpdateCheck.Rows.Count == 0)
                                    {
                                        sRecordFetchMessage = "No records available in current allocation.";
                                        return false;
                                    }
                                    else
                                        sMaster_ID = dtUpdateCheck.Rows[0]["GROUP_ID"].ToString();
                                    IsRecordFetchedFlag = false;
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        IsRecordFetchedFlag = false;
                        sRecordFetchMessage = "Records not assigned.";
                        return false;
                    } 
                    #endregion

                    #region Ignore
                    //else if (IsNewCompany || sFormOpenType == "ListOpen")
                    //{
                    //    GV.sRecordFetchError += ",58";
                    //    //DataTable dtCompTemp = GV.MYSQL.BAL_FetchTableMySQL(GV.sCompanyTable, sUserTypeName + " = 'Current_" + GV.sEmployeeName + "'");
                    //    DataTable dtCompTemp = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT GROUP_ID FROM " + GV.sCompanyTable + " WHERE " + sUserTypeName + " = 'Current_" + GV.sEmployeeName + "' LIMIT 1;");
                    //    GV.sRecordFetchError += ",59";
                    //    if (dtCompTemp.Rows.Count > 0)
                    //    {
                    //        GV.sRecordFetchError += ",60";
                    //        dtMasterCompanies = GV.MYSQL.BAL_FetchTableMySQL(GV.sCompanyTable, "GROUP_ID = " + dtCompTemp.Rows[0]["GROUP_ID"].ToString());
                    //        dtMasterCompanies.TableName = "MasterCompanies";

                    //        GV.sRecordFetchError += ",61";
                    //        sMaster_ID = dtMasterCompanies.Rows[0]["GROUP_ID"].ToString();

                    //        GV.sRecordFetchError += ",62";
                    //        string sMasterIDs = GM.ColumnToQString("MASTER_ID", dtMasterCompanies, "Int");

                    //        GV.sRecordFetchError += ",63";
                    //        dtMasterContacts = GV.MYSQL.BAL_FetchTableMySQL(GV.sContactTable, "MASTER_ID IN (" + sMasterIDs + ")"); //MasterContact Table
                    //        dtMasterContacts.TableName = "MasterContact";

                    //        GV.sRecordFetchError += ",64";
                    //        string sContactIDs = GM.ColumnToQString("CONTACT_ID_P", dtMasterContacts, "Int");

                    //        GV.sRecordFetchError += ",65";
                    //        if (sContactIDs.Length == 0)
                    //        {
                    //            GV.sRecordFetchError += ",66";
                    //            sContactIDs = "0";
                    //        }

                    //        GV.sRecordFetchError += ",67";
                    //        dtQCTable = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM " + GV.sQCTable + " WHERE (TableName='Contact' AND RecordID IN (" + sContactIDs + ")) OR (TableName='Company' AND RecordID IN (" + sMasterIDs + ")) AND ResearchType = '" + GV.sAccessTo + "';");
                    //        dtQCTable.TableName = "QC";

                    //        GV.sRecordFetchError += ",68";
                    //        IsRecordFetchedFlag = true;
                    //        return true;
                    //    }
                    //}                     
                    #endregion

                }
                else if (sMaster_IDForAdminOpen != null && sMaster_IDForAdminOpen.Length > 0)// for Admins and Managers
                {
                    #region Non-Agent Open

                    dtMasterCompanies = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT GROUP_ID,FLAG FROM " + GV.sCompanyTable + " WHERE MASTER_ID = " + sMaster_IDForAdminOpen);//Master Table                    

                    if (dtMasterCompanies.Rows.Count > 0)
                    {
                        if (GV.sUserType != "Admin")
                        {
                            if (dtMasterCompanies.Rows[0]["Flag"].ToString().Length > 0)
                            {
                                if (GV.sUserType == "QC" && !GV.Override_QCAccess)
                                {
                                    if (dtMasterCompanies.Rows[0]["Flag"].ToString() != GV.sAccessTo)
                                    {
                                        IsRecordFetchedFlag = false;
                                        sRecordFetchMessage = "This record is in " + GV.sOppositAccess + " bin.";
                                        return false;
                                    }
                                }

                                if (GV.sUserType == "Manager" && !GV.Override_ManagerAccess)
                                {
                                    if (dtMasterCompanies.Rows[0]["Flag"].ToString() != GV.sAccessTo)
                                    {
                                        IsRecordFetchedFlag = false;
                                        sRecordFetchMessage = "This record is in " + GV.sOppositAccess + " bin.";
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                IsRecordFetchedFlag = false;
                                sRecordFetchMessage = "This record is not your bin.";
                                return false;
                            }
                        }



                        sMaster_ID = dtMasterCompanies.Rows[0]["GROUP_ID"].ToString();
                        sGroup_ID = dtMasterCompanies.Rows[0]["GROUP_ID"].ToString();

                        GV.sPerformance += "Company Record : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                        dtMasterCompanies = GV.MYSQL.BAL_FetchTableMySQL(GV.sCompanyTable, "GROUP_ID = " + dtMasterCompanies.Rows[0]["GROUP_ID"]);
                        dtMasterCompanies.TableName = "MasterCompanies";

                        GV.sPerformance += "Company Record : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                        LoadMasterIds();
                        string sMasterIDs = GM.ColumnToQString("MASTER_ID", dtMasterCompanies, "Int");//Get all MasterIDs


                        GV.sPerformance += "Contact Record : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                        dtMasterContacts = GV.MYSQL.BAL_FetchTableMySQL(GV.sContactTable, "MASTER_ID IN (" + sMasterIDs + ")"); //MasterContact Table
                        dtMasterContacts.TableName = "MasterContact";
                        GV.sPerformance += "Contact Record : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                        string sContactIDs = GM.ColumnToQString("CONTACT_ID_P", dtMasterContacts, "Int");

                        if (sContactIDs.Length == 0)
                            sContactIDs = "0";

                        GV.sPerformance += "QC Record : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                        dtQCTable = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM " + GV.sQCTable + " WHERE (TableName='Contact' AND RecordID IN (" + sContactIDs + ")) OR (TableName='Company' AND RecordID IN (" + sMasterIDs + ")) AND ResearchType = '" + GV.sAccessTo + "';");
                        dtQCTable.TableName = "QC";

                        dtEmailChecks = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM c_email_checks WHERE PROJECT_ID = '" + GV.sProjectID + "' AND CONTACT_ID IN (" + sContactIDs + ") AND EMAIL_SOURCE = 0;");
                        dtEmailChecks.TableName = "EMAIL_CHECKS";

                        GV.sPerformance += "QC Record : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                        GetDialerHistory();

                        //sMaster_ID = sMaster_IDForAdminOpen;

                        IsRecordFetchedFlag = true;
                        return true;
                    }
                    else
                    {
                        sRecordFetchMessage = "ID does not exist.";
                        return false;
                    }
                    #endregion
                }
                else
                {
                    //ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
                    //ToastNotification.DefaultTimeoutInterval = 2000;
                    //ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
                    //ToastNotification.Show(((frmMain)MdiParent), "Admin cannot open New Record.", eToastPosition.TopRight);

                    IsRecordFetchedFlag = false;
                    sRecordFetchMessage = "Record connot be opened";
                    return false;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        List<string> CurrentFilter_Query()
        {
            DataTable dtFilterAssignment;
            DataTable dtAllocationFilter = null;
            List<string> lstReturn = new List<string>();
            string sFilterID = string.Empty;
            dtFilterAssignment = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT FILTER_ID FROM FILTER_ASSIGNMENT WHERE PROJECT_ID = '" + GV.sProjectID + "' AND USERNAME = '" + GV.sEmployeeName + "' AND USERACCESS = '" + GV.sAccessTo + "'");
            if (dtFilterAssignment != null && dtFilterAssignment.Rows.Count > 0 && dtFilterAssignment.Rows[0]["FILTER_ID"].ToString().Length > 0)
            {
                sFilterID = dtFilterAssignment.Rows[0]["FILTER_ID"].ToString();
                dtAllocationFilter = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT SQLTEXT,RANDOMIZE,TIMEZONE_ENABLED,USERDEFINED FROM ALLOCATION_FILTER WHERE FILTER_ID = " + sFilterID);
            }

            if ((dtAllocationFilter != null && dtAllocationFilter.Rows.Count > 0 && dtAllocationFilter.Rows[0]["SQLTEXT"].ToString().Length > 0))
            {
                lstReturn.Add(sFilterID);
                lstReturn.Add(dtAllocationFilter.Rows[0]["RANDOMIZE"].ToString());
                lstReturn.Add(dtAllocationFilter.Rows[0]["TIMEZONE_ENABLED"].ToString());
                lstReturn.Add(dtAllocationFilter.Rows[0]["SQLTEXT"].ToString());
                lstReturn.Add(dtAllocationFilter.Rows[0]["USERDEFINED"].ToString());
            }
            return lstReturn;
        }

        //-----------------------------------------------------------------------------------------------------
        private void InitializeCustomComponent()
        {
            try
            {
                GV.sPerformance += "Starting Record Fetch : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                if (!Directory.Exists(sRecoveryPath))
                    Directory.CreateDirectory(sRecoveryPath);                

                if (IsRecordFetched())
                {
                    GV.sPerformance += "Ending Record Fetch : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;                    

                    //tAutoSaveRecords = new Timer() { Interval = GV.iAutoSave_Intervel };
                    tAutoSaveRecords = new Timer(this.components) { Interval = GV.iAutoSave_Intervel };

                    GV.sCompanySessionID = GV.IP.Replace(".", string.Empty).Reverse() + GM.GetDateTime().ToString("yyMMddHHmmssff");
                    GM.Moniter("Pre Call");

                    //dtMasterCompaniesSQLCE = GV.SQLCE.BAL_FetchTable(GV.sSQLCECompanyTable, "Master_ID = '" + sMaster_ID + "'");
                    //dtMasterContactsSQLCE = GV.SQLCE.BAL_FetchTable(GV.sSQLCEContactTable, "Master_ID = '" + sMaster_ID + "'");
                    #region Initialize Error Table
                    dtValidationResults = new DataTable();
                    dtValidationResults.Columns.Add("TableName", typeof(string));
                    dtValidationResults.Columns.Add("RowIndex", typeof(int));
                    dtValidationResults.Columns.Add("CompanyID", typeof(int));
                    dtValidationResults.Columns.Add("ContactID", typeof(int));
                    dtValidationResults.Columns.Add("Message", typeof(string));
                    dtValidationResults.Columns.Add("Validation", typeof(string));
                    dtValidationResults.Columns.Add("TargetField", typeof(string));
                    dtValidationResults.Columns.Add("TargetValue", typeof(string));
                    dtValidationResults.Columns.Add("ConditionField", typeof(string));
                    dtValidationResults.Columns.Add("ConditionValue", typeof(string));
                    dtValidationResults.Columns.Add("Status", typeof(string));

                    dtValidationResultsDynamic = new DataTable();
                    dtValidationResultsDynamic.Columns.Add("TableName", typeof(string));
                    dtValidationResultsDynamic.Columns.Add("RowIndex", typeof(int));
                    dtValidationResultsDynamic.Columns.Add("CompanyID", typeof(int));
                    dtValidationResultsDynamic.Columns.Add("ContactID", typeof(int));
                    dtValidationResultsDynamic.Columns.Add("Message", typeof(string));
                    dtValidationResultsDynamic.Columns.Add("Validation", typeof(string));
                    dtValidationResultsDynamic.Columns.Add("TargetField", typeof(string));
                    dtValidationResultsDynamic.Columns.Add("TargetValue", typeof(string));
                    dtValidationResultsDynamic.Columns.Add("ConditionField", typeof(string));
                    dtValidationResultsDynamic.Columns.Add("ConditionValue", typeof(string));
                    dtValidationResultsDynamic.Columns.Add("Status", typeof(string));
                    #endregion


                    sMaster_ID = dtMasterCompanies.Rows[0]["MASTER_ID"].ToString(); // if group is not the first record
                    sGroup_ID = dtMasterCompanies.Rows[0]["GROUP_ID"].ToString();
                    GV.sCurrentCompanyID = sMaster_ID;
                    GV.sCurrentCompanyName = dtMasterCompanies.Rows[0]["COMPANY_NAME"].ToString();

                    if (dtMasterCompanies.Rows.Count > 1)
                    {
                        sTabPanelGroupCompany.Visible = true;
                        tabGroupCompany.Visible = true;
                        lblSpliter1.Visible = true;
                        lblGroupID.Visible = true;
                        txtGroupID.Visible = true;
                    }
                    else
                    {
                        sTabPanelGroupCompany.Visible = false;
                        tabGroupCompany.Visible = false;
                        lblSpliter1.Visible = false;
                        lblGroupID.Visible = false;
                        txtGroupID.Visible = false;
                    }

                    GV.sPerformance += "Open Log : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    Log_OpenClose("Opened");
                    GV.sPerformance += "Open Log : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    #region Initialize EAF
                    GV.sPerformance += "EAF : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    //#if debug
                    if (GV.sEmployeeName == "THANGAPRAKASH" && Environment.MachineName.ToUpper() == "MSSPLDET281")
                    {
                        string sEAFLibararyPath = @"D:\E\SVN\Campaign Manager\EAF\SHA256 Encryption\CM\bin\Debug\CM.dll";
                        EAF = Assembly.Load(File.ReadAllBytes(sEAFLibararyPath));
                        EAF_ClassInstance = EAF.CreateInstance("EAF.EAF");
                        EAF_MethodInstance = EAF.GetType("EAF.EAF");
                    }
                    else
                    {
                        //#endif
                        if (CL.EAF != null)
                        {
                            EAF = Assembly.Load(CL.EAF);
                            EAF_ClassInstance = EAF.CreateInstance("EAF.EAF");
                            EAF_MethodInstance = EAF.GetType("EAF.EAF");
                        }

                        //DataTable dtEAFBlob = GV.MSSQL.BAL_ExecuteQuery("select BLOB from project_files where filetype='EAF' and ProjectID='" + GV.sProjectID + "'");
                        //if (dtEAFBlob.Rows.Count > 0)
                        //{
                        //    //EAF = Assembly.Load(File.ReadAllBytes(GV.sEAFLibararyPath));
                        //    EAF = Assembly.Load((byte[])dtEAFBlob.Rows[0]["Blob"]);
                        //    EAF_ClassInstance = EAF.CreateInstance("EAF.EAF");
                        //    EAF_MethodInstance = EAF.GetType("EAF.EAF");
                        //}
                    }
                    #endregion
                    GV.sPerformance += "EAF : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;


                    Load_NameSayer();

                    GV.sPerformance += "LoadTables : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    dtFieldMasterAllColumns = CL.dtFieldMasterAllColumns;
                    dtFieldMaster_Active = CL.dtFieldMaster_Active;
                    dtFieldMasterCompany = CL.dtFieldMasterCompany;
                    dtFieldMasterContact = CL.dtFieldMasterContact;
                    dtFieldMasterChromeCol = CL.dtFieldMasterChromeCols;
                    dtUncertainFields = CL.dtUncertainFields;
                    dtValidations = CL.dtValidations;
                    dtPreUpdate = CL.dtPreUpdate;
                    dtPicklist = CL.dtPicklist;
                    dtEmailSuggestion = CL.dtEmailSuggestion;
                    dtDialConfig = CL.dtDialConfig;
                    dtRecordStatus = CL.dtRecordStatus;
                    dtSpellIgnore = CL.dtSpellIgnore;
                    dtRecordStatusRevenue = CL.dtRecordStatusRevenue;
                    dtCountryInformation = CL.dtCountryInformation;
                    dtQCPicklist = CL.dtQCPicklist;
                    dtBlock = CL.dtBlock;
                    GV.sPerformance += "LoadTables : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    //GV.sPerformance += "Starting Table Load : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    //#region Load Other Tables

                    //GV.sPerformance += "Field Master: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    //dtFieldMasterAllColumns = GV.MYSQL.BAL_FetchTableMySQL("C_FIELD_MASTER", String.Format("PROJECT_ID = '{0}' ORDER BY SEQUENCE_NO", GV.sProjectID)); //All Fields in Master and Master Contacts
                    //dtFieldMasterAllColumns.TableName = "FieldMasterAllColumn";

                    //dtFieldMaster_Active = dtFieldMasterAllColumns.Select("ACTIVE_COLUMN= 'Y' AND Visibility Like '%" + GV.sAccessTo + "%'", "SEQUENCE_NO ASC").CopyToDataTable();
                    //dtFieldMaster_Active.TableName = "FieldMaster";

                    //dtFieldMasterCompany = dtFieldMaster_Active.Select("TABLE_NAME = 'Master'", "SEQUENCE_NO ASC").CopyToDataTable(); //Master Table Fields
                    //dtFieldMasterCompany.TableName = "MasterFormat";

                    //dtFieldMasterContact = dtFieldMaster_Active.Select("TABLE_NAME = 'MasterContacts'", "SEQUENCE_NO ASC").CopyToDataTable(); //MasterContact Table Fields
                    //dtFieldMasterContact.TableName = "MasterContactFormat";

                    //drrUncertain_Fields = dtFieldMasterContact.Select("LEN(UNCERTAIN_LINKED_COLUMN) > 0 OR UNCERTAIN_RAISABLE = 'Y'");

                    //DataRow[] drrTemp = dtFieldMasterContact.Select("UNCERTAIN_RAISABLE = 'Y'");
                    //foreach (DataRow drFieldRows in drrTemp)
                    //{                        
                    //    DataRow drNewRow = dtUncertainFields.NewRow();
                    //    drNewRow["FieldName"] = drFieldRows["FIELD_NAME_TABLE"].ToString();
                    //    drNewRow["PickList_Category"] = drFieldRows["PICKLIST_CATEGORY"].ToString();
                    //    dtUncertainFields.Rows.Add(drNewRow);
                    //}
                    //drrTemp = dtFieldMasterContact.Select("LEN(UNCERTAIN_LINKED_COLUMN) > 0");
                    //foreach (DataRow drFieldRows in drrTemp)
                    //{
                    //    if (dtUncertainFields.Select("FieldName = '" + drFieldRows["UNCERTAIN_LINKED_COLUMN"] + "'").Length > 0)
                    //        dtUncertainFields.Select("FieldName = '" + drFieldRows["UNCERTAIN_LINKED_COLUMN"] + "'")[0]["FieldName_LinkColumn"] = drFieldRows["FIELD_NAME_TABLE"].ToString();
                    //}

                    //GV.sPerformance += "Field Master: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    //GV.sPerformance += "Validation: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    //dtValidations = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM " + GV.sProjectID + "_VALIDATIONS_new WHERE RESEARCH_TYPE='" + GV.sAccessTo + "'"); //Validation Table
                    //dtValidations.TableName = "Validations";

                    //if (dtValidations.Select("OPERATION_TYPE = 'PreUpdate'").Length > 0)
                    //{
                    //    dtPreUpdate = dtValidations.Select("OPERATION_TYPE = 'PreUpdate'").CopyToDataTable();
                    //    dtPreUpdate.TableName = "PreUpdate";
                    //}
                    //GV.sPerformance += "Validation: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    //GV.sPerformance += "Picklist: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    //dtPicklist = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM c_picklists WHERE PicklistCategory NOT IN (SELECT DISTINCT PicklistCategory FROM " + GV.sProjectID + "_picklists) UNION SELECT * FROM " + GV.sProjectID + "_picklists ORDER BY PicklistCategory,PicklistValue;"); //Picklist Table

                    ////dtPicklist = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM " + GV.sProjectID + "_picklists ORDER BY PicklistCategory,PicklistValue;"); //Picklist Table
                    ////dtPicklist = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM c_picklists");//WHERE PicklistCategory NOT IN (SELECT DISTINCT PicklistCategory FROM " + GV.sProjectID + "_picklists) UNION SELECT * FROM " + GV.sProjectID + "_picklists ORDER BY PicklistCategory,PicklistValue;"); //Picklist Table

                    //dtEmailSuggestion = dtPicklist.Select("PicklistCategory = 'EmailSuggestion'", "Remarks ASC").CopyToDataTable();
                    //dtEmailSuggestion.TableName = "EmailSuggestion";

                    //dtDialConfig = dtPicklist.Select("PicklistCategory like 'Dial_%'").CopyToDataTable();

                    //dtPicklist.TableName = "PickList";
                    //GV.PickList_LastUpdate = GM.GetDateTime();

                    //GV.sPerformance += "Picklist: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    //GV.sPerformance += "RecordStatus: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    //dtRecordStatus = GV.MYSQL.BAL_ExecuteQueryMySQL("select Distinct Table_Name,Primary_Status,Secondary_Status,Operation_Type,Research_Type,sort from " + GV.sProjectID + "_recordstatus;"); //Contact Status Table
                    //dtRecordStatus.TableName = "RecordStatus";                    

                    //dtSpellIgnore = GV.MYSQL.BAL_FetchTableMySQL("c_picklists", "PicklistCategory = 'SpellCheckIgnore'");
                    //lstSpellCheckIgnore = dtSpellIgnore.Rows.OfType<DataRow>().Select(dr => dr.Field<string>("PicklistValue")).ToList();

                    //dtRecordStatusRevenue = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM "+ GV.sProjectID + "_RecordStatus"); //Contact Status Table
                    //dtRecordStatusRevenue.TableName = "RecordStatusRevenue";

                    //GV.sPerformance += "RecordStatus: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    //GV.sPerformance += "Country: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    //dtCountryInformation = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM Country");//Time Zone Datatable
                    //dtCountryInformation.TableName = "Country";

                    //GV.sPerformance += "Country: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    //GV.sPerformance += "QC Picklist: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    //if (GV.sUserType != "Agent")
                    //{
                    //    dtQCPicklist = GV.MSSQL.BAL_ExecuteQuery("SELECT Field,Data FROM Timesheet..PickLists WHERE  ProjectType = 'C' AND Department = '" + GV.sAccessTo + "'");
                    //    dtQCPicklist.TableName = "QCPickList";
                    //} 
                    //#endregion
                    GV.sPerformance += "QC Picklist: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    lstSpellCheckIgnore = dtSpellIgnore.Rows.OfType<DataRow>().Select(dr => dr.Field<string>("PicklistValue")).ToList();


                    GV.sPerformance += "Freeze: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    ContactBounce_AND_CompanyContactFreeze();//Get bounce and freeze records
                    GV.sPerformance += "Freeze: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    Data_Recovery();

                    #region Layout Operation

                    GV.sPerformance += "Starting Control Design : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    this.SuspendLayout();

                    ((System.ComponentModel.ISupportInitialize)(this.SplitMain)).BeginInit();
                    SplitMain.Panel1.SuspendLayout();
                    SplitMain.Panel2.SuspendLayout();
                    SplitMain.SuspendLayout();

                    splitContactModernUI.Panel1.SuspendLayout();
                    splitContactModernUI.Panel2.SuspendLayout();
                    splitContactModernUI.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.splitContactModernUI)).BeginInit();

                    splitTRDisposalMainParant.Panel1.SuspendLayout();
                    splitTRDisposalMainParant.Panel2.SuspendLayout();
                    splitTRDisposalMainParant.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.splitTRDisposalMainParant)).BeginInit();

                    splitTRDisposals.Panel1.SuspendLayout();
                    splitTRDisposals.Panel2.SuspendLayout();
                    splitTRDisposals.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.splitTRDisposals)).BeginInit();

                    splitWRDisposalMainParant.Panel1.SuspendLayout();
                    splitWRDisposalMainParant.Panel2.SuspendLayout();
                    splitWRDisposalMainParant.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.splitWRDisposalMainParant)).BeginInit();

                    splitWRDisposals.Panel1.SuspendLayout();
                    splitWRDisposals.Panel2.SuspendLayout();
                    splitWRDisposals.SuspendLayout();
                    ((ISupportInitialize)(splitWRDisposals)).BeginInit();

                    sTabPanelCompanyInformation.SuspendLayout();
                    // ((System.ComponentModel.ISupportInitialize)(this.sTabPanelCompanyInformation)).BeginInit();

                    sdgvCompany.SuspendLayout();
                    sdgvContacts.SuspendLayout();

                    tabControlCompany.SuspendLayout();
                    ((ISupportInitialize)(tabControlCompany)).BeginInit();

                    tabControlContact.SuspendLayout();
                    ((ISupportInitialize)(tabControlContact)).BeginInit();
                    #endregion

                    GV.sPerformance += "Ending Control Design : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    

                    GV.sPerformance += "Start Loading Data to Control: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    GV.sPerformance += "MasterCompanies: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    Load_MasterCompany();//Loads the Controls for Master Company
                    GV.sPerformance += "MasterCompanies: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    splitContactModernUI.Panel1Collapsed = false;

                    btnPreviousContact.Visible = false;
                    btnNextContact.Visible = false;

                    GV.sPerformance += "Filter: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    InitializeFilter();
                    GV.sPerformance += "Filter: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    GV.sPerformance += "MasterContacts: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    Load_MasterContacts();//Loads the Controls for Master Contacts
                    GV.sPerformance += "MasterContacts: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

                    Assign_RunTime_Events();//Run Time Events

                    Miscellaneous(); //Various Assingings and Loadings to be done on form load

                    #region Layout Operation
                    tabControlContact.ResumeLayout(false);
                    tabControlContact.PerformLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.tabControlContact)).EndInit();

                    tabControlCompany.ResumeLayout(false);
                    tabControlCompany.PerformLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.tabControlCompany)).EndInit();

                    sdgvContacts.ResumeLayout(false);
                    sdgvContacts.PerformLayout();

                    sdgvCompany.ResumeLayout(false);
                    sdgvCompany.PerformLayout();
                    //((System.ComponentModel.ISupportInitialize)(this.sdgvCompany)).EndInit();

                    sTabPanelContacts.ResumeLayout(false);
                    sTabPanelContacts.PerformLayout();
                    // ((System.ComponentModel.ISupportInitialize)(this.sTabPanelContacts)).EndInit();

                    sTabPanelCompanyInformation.ResumeLayout(false);
                    sTabPanelCompanyInformation.PerformLayout();
                    //((System.ComponentModel.ISupportInitialize)(this.sTabPanelCompanyInformation)).EndInit();

                    splitWRDisposals.Panel1.ResumeLayout(false);
                    splitWRDisposals.Panel1.PerformLayout();
                    splitWRDisposals.Panel2.ResumeLayout(false);
                    splitWRDisposals.Panel2.PerformLayout();
                    splitWRDisposals.ResumeLayout(false);
                    splitWRDisposals.PerformLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.splitWRDisposals)).EndInit();

                    splitWRDisposalMainParant.Panel1.ResumeLayout(false);
                    splitWRDisposalMainParant.Panel1.PerformLayout();
                    splitWRDisposalMainParant.Panel2.ResumeLayout(false);
                    splitWRDisposalMainParant.Panel2.PerformLayout();
                    splitWRDisposalMainParant.ResumeLayout(false);
                    splitWRDisposalMainParant.PerformLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.splitWRDisposalMainParant)).EndInit();

                    splitTRDisposals.Panel1.ResumeLayout(false);
                    splitTRDisposals.Panel1.PerformLayout();
                    splitTRDisposals.Panel2.ResumeLayout(false);
                    splitTRDisposals.Panel2.PerformLayout();
                    splitTRDisposals.ResumeLayout(false);
                    splitTRDisposals.PerformLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.splitTRDisposals)).EndInit();

                    splitTRDisposalMainParant.Panel1.ResumeLayout(false);
                    splitTRDisposalMainParant.Panel1.PerformLayout();
                    splitTRDisposalMainParant.Panel2.ResumeLayout(false);
                    splitTRDisposalMainParant.Panel2.PerformLayout();
                    splitTRDisposalMainParant.ResumeLayout(false);
                    splitTRDisposalMainParant.PerformLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.splitTRDisposalMainParant)).EndInit();

                    SplitMain.Panel1.ResumeLayout(false);
                    SplitMain.Panel1.PerformLayout();
                    SplitMain.Panel2.ResumeLayout(false);
                    SplitMain.Panel2.PerformLayout();
                    SplitMain.ResumeLayout(false);
                    SplitMain.PerformLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.SplitMain)).EndInit();

                    splitContactModernUI.Panel1.ResumeLayout(false);
                    splitContactModernUI.Panel1.PerformLayout();
                    splitContactModernUI.Panel2.ResumeLayout(false);
                    splitContactModernUI.Panel2.PerformLayout();
                    splitContactModernUI.ResumeLayout(false);
                    splitContactModernUI.PerformLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.splitContactModernUI)).EndInit();

                    this.ResumeLayout(false);
                    this.PerformLayout();
                    #endregion

                    if (btnSave.Enabled && GV.IsCallScriptEnabled && GV.sAccessTo == "TR" && !IsNewCompany)//Display Call Script
                        CallScript();
                    //LoadSuperGrid();
                    tAutoSaveRecords.Start(); //Start timer

                    GV.sPerformance += "End Loading Data to Control: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                    //expandablePanelContactSearch.Parent = splitContactModernUI.Panel1;
                    //expandablePanelContactSearch.Dock = DockStyle.Top;

                    InitilizeCallTracker();
                }
                else
                {
                    if (sTimeLoggerMessage.Trim().Length > 0)
                    {
                        ToastNotification.DefaultToastGlowColor = DevComponents.DotNetBar.eToastGlowColor.Red;
                        ToastNotification.DefaultTimeoutInterval = 3000;
                        //ToastNotification.ToastBackColor = Color.Black;
                        ToastNotification.DefaultToastPosition = DevComponents.DotNetBar.eToastPosition.TopRight;
                        ToastNotification.Show(this.MdiParent, sTimeLoggerMessage, Properties.Resources._32x32);
                        ToastNotification.DefaultTimeoutInterval = 2000;
                        ToastNotification.DefaultToastGlowColor = DevComponents.DotNetBar.eToastGlowColor.None;
                    }
                    else
                        ToastNotification.Show(this.MdiParent, sRecordFetchMessage, eToastPosition.TopRight);
                    //this.Close(); //Form cannot be closed on load// Closed on Form.Shown() event based on IsRecordFetchedFlag
                    return;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }



        //-----------------------------------------------------------------------------------------------------
        private void frmContactsUpdate_Load(object sender, EventArgs e)
        {
            GV.sPerformance += "FormLoad: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
            if (!splitContactModernUI.Panel1Collapsed)
            {
                AlignContactGridColumn();
            }
            GV.sPerformance += "FormLoad: " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
            //Load_QC();
            // MessageBoxEx.Show("Telephone Auto formetting not fixed");

            //DataValidator.WorkerReportsProgress = false;
            //DataValidator.WorkerSupportsCancellation = false;
            //DataValidator.DoWork += new DoWorkEventHandler(Validator_DoWork);
            //DataValidator.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Validator_RunWorkerCompleted);
        }

        void Data_Recovery()
        {
            try
            {
                if
                (
                    (File.Exists(sRecoveryPath + "\\" + sMaster_ID + "_COM.XML") && File.GetCreationTime(sRecoveryPath + "\\" + sMaster_ID + "_COM.XML").Date == GM.GetDateTime().Date)
                        ||
                    (File.Exists(sRecoveryPath + "\\" + sMaster_ID + "_CON.XML") && File.GetCreationTime(sRecoveryPath + "\\" + sMaster_ID + "_CON.XML").Date == GM.GetDateTime().Date)
                )
                {
                    if(MessageBoxEx.Show("Campaign Manager recovered some unsaved data for this company.<br/>Would you like to restore them ?", "Campaign Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        DataSet dsRecovery_Com = new DataSet();
                        DataSet dsRecovery_Con = new DataSet();

                        dsRecovery_Com.ReadXml(sRecoveryPath + "\\" + sMaster_ID + "_COM.XML");
                        dsRecovery_Con.ReadXml(sRecoveryPath + "\\" + sMaster_ID + "_CON.XML");

                        if (dsRecovery_Con.Tables.Count > 0 && dsRecovery_Con.Tables[0] != null && dtMasterContacts.Rows.Count > dsRecovery_Con.Tables[0].Rows.Count)
                        {
                            MessageBoxEx.Show("Existing contacts are newer than the cached data. Aborting recovery.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Remove_Chached();
                            return;
                        }

                        if (dsRecovery_Com.Tables.Count > 0 && dsRecovery_Com.Tables[0] != null && dsRecovery_Com.Tables[0].Rows.Count == dtMasterCompanies.Rows.Count)
                            Recover_Company_Datatable(dsRecovery_Com.Tables[0], dtMasterCompanies);

                        if (dsRecovery_Con.Tables.Count > 0 && dsRecovery_Con.Tables[0] != null && dsRecovery_Con.Tables[0].Rows.Count == dtMasterContacts.Rows.Count)
                            Recover_Contact_Datatable(dsRecovery_Con.Tables[0], dtMasterContacts);

                        Remove_Chached();
                    }
                    else
                    {
                        Remove_Chached();
                        ToastNotification.Show(this, "Cached data discarded.", eToastPosition.TopRight);
                    }
                }                
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void Remove_Chached()
        {
            try
            {
                if (File.Exists(sRecoveryPath + "\\" + sMaster_ID + "_COM.XML"))
                    File.Delete(sRecoveryPath + "\\" + sMaster_ID + "_COM.XML");

                if (File.Exists(sRecoveryPath + "\\" + sMaster_ID + "_CON.XML"))
                    File.Delete(sRecoveryPath + "\\" + sMaster_ID + "_CON.XML");
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);                
            }
        }

        void Recover_Company_Datatable(DataTable dtSource, DataTable dtTarget)
        {
            try
            {
                foreach (DataRow drSource in dtSource.Rows)
                {
                    foreach (DataColumn drColumn in dtTarget.Columns)
                    {
                        if (dtSource.Columns.Contains(drColumn.ColumnName) && drSource[drColumn.ColumnName].ToString().Length > 0)
                        {
                            if (drColumn.DataType.Name == "Decimal" || drColumn.DataType.Name == "Int16" || drColumn.DataType.Name == "Int32" || drColumn.DataType.Name == "Int64" || drColumn.DataType.Name == "Double")
                                dtTarget.Rows[0][drColumn.ColumnName] = Convert.ToInt64(drSource[drColumn.ColumnName]);
                            else if (drColumn.DataType.Name == "DateTime")//Date
                            { /*Do Nothing*/}
                            else
                                dtTarget.Rows[0][drColumn.ColumnName] = drSource[drColumn.ColumnName].ToString();//Text
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

        void Recover_Contact_Datatable(DataTable dtSource, DataTable dtTarget)
        {
            List<string> lstColumnDate = new List<string>();
            lstColumnDate.Add(GV.sAccessTo + "_UPDATED_DATE");
            lstColumnDate.Add("CREATED_DATE");
            lstColumnDate.Add("UPDATED_DATE");

            try
            {
                foreach (DataRow drSource in dtSource.Rows)
                {
                    if (drSource["CONTACT_ID_P"].ToString().Length == 0)//Excisting Contact
                    {
                        DataRow drTarget = dtTarget.NewRow();
                        foreach (DataColumn drColumn in dtTarget.Columns)
                        {
                            if (dtSource.Columns.Contains(drColumn.ColumnName) && drSource[drColumn.ColumnName].ToString().Length > 0)
                            {
                                if (drColumn.DataType.Name == "DateTime")
                                {
                                    if (lstColumnDate.Contains(drColumn.ColumnName, StringComparer.OrdinalIgnoreCase))
                                        drTarget[drColumn.ColumnName] = GM.GetDateTime();
                                }
                                else if ((drColumn.DataType.Name == "Decimal" || drColumn.DataType.Name == "Int16" || drColumn.DataType.Name == "Int32" || drColumn.DataType.Name == "Int64" || drColumn.DataType.Name == "Double") && drTarget[drColumn.ColumnName].ToString() != drSource[drColumn.ColumnName].ToString())
                                    drTarget[drColumn.ColumnName] = Convert.ToInt64(drSource[drColumn.ColumnName]);
                                else if (drTarget[drColumn.ColumnName].ToString() != drSource[drColumn.ColumnName].ToString())
                                    drTarget[drColumn.ColumnName] = drSource[drColumn.ColumnName];
                            }
                        }
                        dtTarget.Rows.Add(drTarget);
                    }
                    else//New Contact
                    {
                        foreach (DataRow drTarget in dtTarget.Rows)
                        {
                            if (drTarget["CONTACT_ID_P"].ToString() == drSource["CONTACT_ID_P"].ToString())
                            {
                                foreach (DataColumn drColumn in drTarget.Table.Columns)
                                {
                                    if (dtSource.Columns.Contains(drColumn.ColumnName) && drSource[drColumn.ColumnName].ToString().Length > 0)
                                    {
                                        if ((drColumn.DataType.Name == "Decimal" || drColumn.DataType.Name == "Int16" || drColumn.DataType.Name == "Int32" || drColumn.DataType.Name == "Int64" || drColumn.DataType.Name == "Double") && drTarget[drColumn.ColumnName].ToString() != drSource[drColumn.ColumnName].ToString())
                                            drTarget[drColumn.ColumnName] = Convert.ToInt64(drSource[drColumn.ColumnName]);
                                        else if (drColumn.DataType.Name == "DateTime")
                                        { /*Do Nothing*/}
                                        else if (drTarget[drColumn.ColumnName].ToString() != drSource[drColumn.ColumnName].ToString())
                                            drTarget[drColumn.ColumnName] = drSource[drColumn.ColumnName].ToString();
                                    }
                                }
                                break;
                            }
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

        void Replicate_Datatable(DataTable dtSource, DataTable dtTarget)
        {
            try
            {
                foreach (DataRow drSource in dtSource.Rows)
                {
                    DataRow drTarget = dtTarget.NewRow();
                    foreach (DataColumn drColumn in drSource.Table.Columns)
                    {
                        if (drColumn.DataType.Name != "DateTime")
                            drTarget[drColumn.ColumnName] = drSource[drColumn.ColumnName];
                    }
                    dtTarget.Rows.Add(drTarget);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void Load_QC()
        {
            if (GV.sUserType == "QC" || GV.sUserType == "Admin")
            {
                if (GV.sAccessTo == "TR")
                {
                    DateTime dtMinDate = GM.GetDateTime();
                    DateTime dtMaxDate = GM.GetDateTime().AddDays(-7);

                    dtQCWeekIDs = new DataTable();
                    dtQCWeekIDs.Columns.Add("RowIndex", typeof(Int32));
                    dtQCWeekIDs.Columns.Add("TableName");
                    dtQCWeekIDs.Columns.Add("RecordID", typeof(Int32));
                    dtQCWeekIDs.Columns.Add("Date");
                    dtQCWeekIDs.Columns.Add("AgentName");
                    dtQCWeekIDs.Columns.Add("MISID");


                    for (int i = 0; i < dtMasterCompanies.Rows.Count; i++)
                    {
                        if (dtMasterCompanies.Rows[i]["TR_DATECALLED"].ToString().Length > 0 && dtMasterCompanies.Rows[i]["TR_AGENTNAME"].ToString().Length > 0)
                        {
                            DateTime dtCheck = Convert.ToDateTime(dtMasterCompanies.Rows[i]["TR_DATECALLED"]);
                            dtCheck = GetMonday(dtCheck, GV.sAccessTo);
                            DataRow drNewRow = dtQCWeekIDs.NewRow();
                            drNewRow["RowIndex"] = i;
                            drNewRow["TableName"] = "Company";
                            drNewRow["RecordID"] = dtMasterCompanies.Rows[i]["MASTER_ID"].ToString();
                            drNewRow["Date"] = dtCheck.ToString("yyyy-MM-dd");
                            drNewRow["AgentName"] = dtMasterCompanies.Rows[i]["TR_AGENTNAME"].ToString();
                            dtQCWeekIDs.Rows.Add(drNewRow);
                        }
                    }

                    for (int i = 0; i < dtMasterContacts.Rows.Count; i++)
                    {
                        if (dtMasterContacts.Rows[i]["TR_UPDATED_DATE"].ToString().Length > 0 && dtMasterContacts.Rows[i]["TR_AGENT_NAME"].ToString().Length > 0)
                        {
                            DateTime dtCheck = Convert.ToDateTime(dtMasterContacts.Rows[i]["TR_UPDATED_DATE"]);
                            dtCheck = GetMonday(dtCheck, GV.sAccessTo);
                            DataRow drNewRow = dtQCWeekIDs.NewRow();
                            drNewRow["RowIndex"] = i;
                            drNewRow["TableName"] = "Contact";
                            drNewRow["RecordID"] = dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString();
                            drNewRow["Date"] = dtCheck.ToString("yyyy-MM-dd");
                            drNewRow["AgentName"] = dtMasterContacts.Rows[i]["TR_AGENT_NAME"].ToString();
                            dtQCWeekIDs.Rows.Add(drNewRow);
                        }
                    }


                    List<string> lstFilter = new List<string>();
                    foreach (DataRow drMIS in dtQCWeekIDs.Rows)
                        lstFilter.Add("(AgentName = '" + drMIS["AgentName"] + "' AND CalledWeek = '" + drMIS["Date"] + "')");

                    lstFilter = lstFilter.Distinct().ToList();
                    string sFilter = string.Empty;
                    foreach (string sFilterStr in lstFilter)
                    {
                        if (sFilter.Length > 0)
                            sFilter += " OR " + sFilterStr;
                        else
                            sFilter = sFilterStr;
                    }

                    if (sFilter.Length > 0)
                    {
                        string sMISEntryQuery = string.Empty;
                        //sMISEntryQuery = "SELECT RecordID , AgentName , CalledWeek FROM Timesheet..MIS_QC WHERE AgentName IN (" + GM.ListToQueryString(lstAgentNames, "String") + ") AND CalledWeek BETWEEN '" + dtMinDate.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dtMaxDate.ToString("yyyy-MM-dd") + " 23:00:00';";
                        sMISEntryQuery = "SELECT RecordID , AgentName , replace(convert(VARCHAR(10), CalledWeek, 111), '/', '-') CalledWeek FROM Timesheet..MIS_QC WHERE " + sFilter;
                        DataTable dtMISEntryCheck = GV.MSSQL.BAL_ExecuteQuery(sMISEntryQuery);

                        foreach (DataRow drMISQC in dtQCWeekIDs.Rows)
                        {
                            DataRow[] drrMisEntry = dtMISEntryCheck.Select("AgentName = '" + drMISQC["AgentName"] + "' AND CalledWeek = '" + drMISQC["Date"] + "'");
                            if (drrMisEntry.Length > 0)
                                drMISQC["MISID"] = drrMisEntry[0]["RecordID"].ToString();
                        }
                        dtHRIMSQCDetails = GV.MSSQL.BAL_FetchTable("Timesheet..mis_QCDetails", "project_name = '" + GV.sProjectName + "' AND processRecordID IN (" + GM.ListToQueryString(lstMasterIDs, "String") + ")");
                    }
                    else
                        dtHRIMSQCDetails = GV.MSSQL.BAL_FetchTable("Timesheet..mis_QCDetails", "1=0");
                }
                else
                {

                }
            }
        }

        DateTime GetMonday(DateTime dtCheck, string sResearchType)
        {
            switch (dtCheck.DayOfWeek.ToString())
            {
                case "Sunday":
                    return (sResearchType == "TR" ? dtCheck.AddDays(1) : dtCheck.AddDays(-6));

                case "Monday":
                    return (sResearchType == "TR" ? dtCheck.AddDays(0) : dtCheck.AddDays(0));

                case "Tuesday":
                    return (sResearchType == "TR" ? dtCheck.AddDays(-1) : dtCheck.AddDays(-1));

                case "Wednesday":
                    return (sResearchType == "TR" ? dtCheck.AddDays(-2) : dtCheck.AddDays(-2));

                case "Thursday":
                    return (sResearchType == "TR" ? dtCheck.AddDays(-3) : dtCheck.AddDays(-3));

                case "Friday":
                    return (sResearchType == "TR" ? dtCheck.AddDays(3) : dtCheck.AddDays(-4));

                case "Saturday":
                    return (sResearchType == "TR" ? dtCheck.AddDays(2) : dtCheck.AddDays(-5));
                default:
                    return dtCheck;
            }
        }

        void LoadSuperGridCompany()
        {
            IsSuperGridLoading = true;
            sdgvCompany.PrimaryGrid.Rows.Clear();
            sdgvCompany.PrimaryGrid.DefaultRowHeight = 100;

            for (int i = 0; i < dtMasterCompanies.Rows.Count; i++)
            {
                string sCompanyName = string.Empty;
                sCompanyName = i + 1 + " <font color = 'Gray'>(" + dtMasterCompanies.Rows[i]["MASTER_ID"].ToString() + ")</font>. " + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[i]["COMPANY_NAME"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty));

                #region Form HTML to Display in Grid
                string sHTMLValue = string.Empty;
                sHTMLValue += "<div align='left' width ='0'><div align='left'><font size = '11'>" + sCompanyName + "</font></div><br/>";
                string sAddress = string.Empty;

                if (dtMasterCompanies.Rows[i]["ADDRESS_1"].ToString().Length > 0)
                    sAddress = GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[i]["ADDRESS_1"].ToString());

                if (dtMasterCompanies.Rows[i]["ADDRESS_2"].ToString().Length > 0)
                {
                    if (sAddress.Length > 0)
                        sAddress += ", " + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[i]["ADDRESS_2"].ToString());
                    else
                        sAddress = GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[i]["ADDRESS_2"].ToString());
                }

                if (dtMasterCompanies.Rows[i]["CITY"].ToString().Length > 0)
                {
                    if (sAddress.Length > 0)
                        sAddress += ", " + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[i]["CITY"].ToString());
                    else
                        sAddress = GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[i]["CITY"].ToString());
                }

                if (dtMasterCompanies.Rows[i]["COUNTRY"].ToString().Length > 0)
                {
                    if (sAddress.Length > 0)
                        sAddress += ", " + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[i]["COUNTRY"].ToString());
                    else
                        sAddress = GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[i]["COUNTRY"].ToString());
                }

                sHTMLValue += "<div align='Left'><font color = 'Gray' size = '10'>" + sAddress + "</font></div>";

                if (
                (
                GV.sAccessTo == "TR"
                &&
                (
                GV.TR_lstDisposalsToBeFreezed.Contains(dtMasterCompanies.Rows[i]["TR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase)
                ||
                GV.TR_lstDisposalsToBeValidated.Contains(dtMasterCompanies.Rows[i]["TR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase)
                )
                )
                ||
                (
                GV.sAccessTo == "WR"
                &&
                (
                GV.WR_lstDisposalsToBeFreezed.Contains(dtMasterCompanies.Rows[i]["WR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase)
                ||
                GV.WR_lstDisposalsToBeValidated.Contains(dtMasterCompanies.Rows[i]["WR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase)
                )
                )
                )
                {
                    sHTMLValue += "<div align = 'right'><font color = 'Green'>" + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[i][GV.sAccessTo + "_PRIMARY_DISPOSAL"].ToString()) + "</font></div>";
                    sHTMLValue += "<div align = 'right'><font color = 'Green'>" + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[i][GV.sAccessTo + "_SECONDARY_DISPOSAL"].ToString()) + "</font></div>";
                }
                else
                {
                    sHTMLValue += "<div align = 'right'><font color = 'Gray'>" + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[i][GV.sAccessTo + "_PRIMARY_DISPOSAL"].ToString()) + "</font></div>";
                    sHTMLValue += "<div align = 'right'><font color = 'Gray'>" + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[i][GV.sAccessTo + "_SECONDARY_DISPOSAL"].ToString()) + "</font></div>";
                }


                //if (GV.sUserType != "Agent")
                //{
                //    if (dtMasterContacts.Rows[i]["TR_Agent_Name"].ToString().Length > 0 && dtMasterContacts.Rows[i]["WR_Agent_Name"].ToString().Length > 0)
                //        sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Navy'>TR: " + GM.ProperCase(dtMasterContacts.Rows[i]["TR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[i]["TR_UPDATED_DATE"] + " |  WR: " + GM.ProperCase(dtMasterContacts.Rows[i]["WR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[i]["WR_UPDATED_DATE"] + "</font></div>";
                //    else if (dtMasterContacts.Rows[i]["TR_Agent_Name"].ToString().Length > 0)
                //        sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Navy'>TR: " + GM.ProperCase(dtMasterContacts.Rows[i]["TR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[i]["TR_UPDATED_DATE"] + "</font></div>";
                //    else if (dtMasterContacts.Rows[i]["WR_Agent_Name"].ToString().Length > 0)
                //        sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Navy'>WR: " + GM.ProperCase(dtMasterContacts.Rows[i]["WR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[i]["WR_UPDATED_DATE"] + "</font></div>";

                //    if (dtMasterContacts.Rows[i]["Rejection"].ToString().Length > 0 && dtMasterContacts.Rows[i]["Review_Tag"].ToString().Length > 0)
                //        sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'ForestGreen'>Rejection: " + GM.ProperCase(dtMasterContacts.Rows[i]["Rejection"].ToString()) + "</font> | <font size='7' face ='Microsoft Sans Serif' color = 'Crimson'>Review: " + GM.ProperCase(dtMasterContacts.Rows[i]["Review_Tag"].ToString()) + "</font></div>";
                //    else if (dtMasterContacts.Rows[i]["Rejection"].ToString().Length > 0)
                //        sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'ForestGreen'>Rejection: " + GM.ProperCase(dtMasterContacts.Rows[i]["Rejection"].ToString()) + "</font></div>";
                //    else if (dtMasterContacts.Rows[i]["Review_Tag"].ToString().Length > 0)
                //        sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Crimson'>Review: " + GM.ProperCase(dtMasterContacts.Rows[i]["Review_Tag"].ToString()) + "</font></div>";

                //}

                sHTMLValue += "</div>";

                #endregion

                #region Initilize Grid Row
                GridRow gridRow = new GridRow();
                GridCell gridCellHTMLContact = new GridCell();
                GridCell gridCellUniqueNumber = new GridCell();

                gridCellUniqueNumber.Value = i;//Row index of normal grid                
                gridCellHTMLContact.Value = sHTMLValue;
                gridRow.Cells.Add(gridCellHTMLContact);
                gridRow.Cells.Add(gridCellUniqueNumber);
                gridRow.Expanded = true;
                this.sdgvCompany.PrimaryGrid.Rows.Add(gridRow);
                #endregion
            }


            if (sdgvCompany.VScrollBar.Visible)//Alignment
                sdgvCompany.PrimaryGrid.Columns[0].Width = sdgvCompany.Width - 18;
            else
                sdgvCompany.PrimaryGrid.Columns[0].Width = sdgvCompany.Width;

            IsSuperGridLoading = false;
        }


        //-----------------------------------------------------------------------------------------------------
        private void LoadSuperGridContact()
        {
            expandablePanelContactSearch.Expanded = false;
            if (!splitContactModernUI.Panel1Collapsed)
            {
                IsSuperGridLoading = true;

                sdgvContacts.PrimaryGrid.Rows.Clear();
                sFilterHeader = string.Empty;
                //sdgvContacts.PrimaryGrid.Columns.Clear();
                //mnuSdgvSort.Items.Clear();
                ToolStripSort.DropDownItems.Clear();
                //btnSort.SubItems.Clear();


                if ((GV.sUserType != "Agent") || GV.sShowDetailedContact == "Y")
                    sdgvContacts.PrimaryGrid.DefaultRowHeight = 120;

                while (sdgvContacts.PrimaryGrid.Columns.Count != 2)//Clear all column except first two column(1.HTML content and 2.UniqueNumber)
                {
                    try
                    {
                        sdgvContacts.PrimaryGrid.Columns.Remove(sdgvContacts.PrimaryGrid.Columns[2]);
                    }
                    catch (Exception)
                    { break; }
                }

                //if (dtMasterContacts != null && dtMasterContacts.Rows.Count > 0)
                //{
                if (GV.lstSortableContactColumn != null)//Add Column to super grid //For sorting
                {
                    foreach (string sColumn in GV.lstSortableContactColumn)//Create dynamic columns and tool strip
                    {
                        GridColumn sdgvColumn = new GridColumn();
                        sdgvColumn.Name = sColumn;
                        sdgvColumn.DataType = Type.GetType("System.String");
                        sdgvColumn.Visible = false;
                        sdgvContacts.PrimaryGrid.Columns.Add(sdgvColumn);

                        //ButtonItem btnSortMenu = new DevComponents.DotNetBar.ButtonItem();
                        //btnSortMenu.Name = sColumn;
                        //btnSortMenu.Text = GM.ProperCase(sColumn.Replace("_", " "));
                        //btnSortMenu.Click += new EventHandler(sortToolStripMenuItem_Click);
                        //btnSort.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] { btnSortMenu });

                        ToolStripMenuItem toolstrip = new ToolStripMenuItem();
                        toolstrip.Name = sColumn;
                        toolstrip.Text = GM.ProperCase_ProjectSpecific(sColumn.Replace("_", " "));
                        toolstrip.Click += new EventHandler(sortToolStripMenuItem_Click);
                        ToolStripSort.DropDownItems.Add(toolstrip);

                    }

                    //ButtonItem btnNoSort = new DevComponents.DotNetBar.ButtonItem();
                    //btnNoSort.Name = "NoSort";
                    //btnNoSort.Text = "No Sort";
                    //btnNoSort.Click += new EventHandler(sortToolStripMenuItem_Click);
                    //btnSort.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] { btnNoSort });

                    ToolStripMenuItem toolstripNoSort = new ToolStripMenuItem();
                    toolstripNoSort.Name = "NoSort";
                    toolstripNoSort.Text = "No Sort";
                    toolstripNoSort.Click += new EventHandler(sortToolStripMenuItem_Click);
                    ToolStripSort.DropDownItems.Add(toolstripNoSort);
                }
                //}
                List<string> lstValiedContactStatus = new List<string>();

                if (GV.sAccessTo == "TR")
                    lstValiedContactStatus = GV.lstTRContactStatusToBeValidated;
                else
                    lstValiedContactStatus = GV.lstWRContactStatusToBeValidated;

                for (int i = 0; i < dtMasterContacts.Rows.Count; i++)
                {
                    if (sDoFilter.Length > 0 && (sDoFilter == sMaster_ID || sDoFilter == "All"))
                    {
                        bool IsRecordPassed = true;
                        foreach (DataRow drFilter in dtFilter.Rows)
                        {
                            if (drFilter["Value"].ToString().Trim().Length > 0)
                            {
                                if (drFilter["ValueType"].ToString() == "Date")
                                {
                                    if (dtMasterContacts.Rows[i][drFilter["FieldName"].ToString()].ToString().Length > 0 && Convert.ToDateTime(dtMasterContacts.Rows[i][drFilter["FieldName"].ToString()].ToString()).ToString("dd/MM/yyyy") != drFilter["Value"].ToString())
                                        IsRecordPassed = false;
                                }
                                else
                                {
                                    if (dtMasterContacts.Rows[i][drFilter["FieldName"].ToString()].ToString().ToUpper() != drFilter["Value"].ToString().ToUpper())
                                        IsRecordPassed = false;
                                }
                            }
                        }

                        if (!IsRecordPassed)
                            continue;
                    }


                    #region Form HTML to display in Grid
                    //Super Grid
                    string sTitle = string.Empty;

                    if ((GV.sUserType != "Agent") || GV.sShowDetailedContact == "Y")
                        sTitle = i + 1 + " <font color = 'Gray'>(" + dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString() + ")</font>. " + dtMasterContacts.Rows[i]["TITLE"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);
                    else
                        sTitle = i + 1 + ". " + dtMasterContacts.Rows[i]["TITLE"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);


                    string sJobTitle = dtMasterContacts.Rows[i]["JOB_TITLE"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);

                    //if (dtMasterContacts.Rows[i]["OTHERS_JOBTITLE"].ToString().Trim().Length > 0)
                    //    sJobTitle = sJobTitle + " / " + dtMasterContacts.Rows[i]["OTHERS_JOBTITLE"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);

                    string sFirstName = dtMasterContacts.Rows[i]["FIRST_NAME"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);
                    string sLastName = dtMasterContacts.Rows[i]["LAST_NAME"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);
                    string sContactEmail = dtMasterContacts.Rows[i]["CONTACT_EMAIL"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);
                    string sContactStatus = dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"].ToString();

                    string sPhonetics = string.Empty;

                    if (GV.NameSayer)
                    {
                        string sFirstName_Phonetics = NameSayer_Phonetics(sFirstName);
                        string sLast_Phonetics = NameSayer_Phonetics(sLastName);
                        if (sFirstName_Phonetics.Length > 0 && sLast_Phonetics.Length > 0)
                            sPhonetics = "<font color = 'Gray' Size = '-2'>" + sFirstName_Phonetics + " " + sLast_Phonetics + "</font>";                        
                    }


                    string sHTMLValue = string.Empty;
                    //if (dgvr.Cells["TITLE"].Value.ToString().Length > 0)
                    sHTMLValue += "<div align='left' width ='0'><div align='left'><font size = '11'>" + sTitle + " " + sFirstName + " " + sLastName + " " + sPhonetics + "</font></div><br/>";
                    //else
                    //    sHTMLValue += "<div align='left' width ='0'><div align='left'><font size = '11'>" + sFirstName + " " + sLastName + "</font></div><br/>";
                    if (dtMasterContacts.Rows[i]["JOB_TITLE"].ToString().Length > 0)
                        sHTMLValue += "<div align='Left'><font color = 'Gray' size = '10'>" + sJobTitle + "</font></div>";
                    sHTMLValue += "<div align='left'><font color = 'Gray'>" + sContactEmail + "</font></div>";
                    


                    if (GV.lstContactStatusToBeFreezed.Contains(sContactStatus, StringComparer.OrdinalIgnoreCase) || lstValiedContactStatus.Contains(sContactStatus, StringComparer.OrdinalIgnoreCase))
                    {
                        if (dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString().Length > 0)// && (lstRecordsToUnlock.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])) || lstQCOKIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"]))))
                        {
                            if (lstBouncedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])) && lstSendBackRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])))
                                sHTMLValue += "<div align = 'right'><font color = 'Red'>Bounced | SendBack</font> | <font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                            else if (lstBouncedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])))
                                sHTMLValue += "<div align = 'right'><font color = 'Red'>Bounced</font> | <font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                            else if (lstSendBackRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])))
                                sHTMLValue += "<div align = 'right'><font color = 'Red'>QC:SendBack</font> | <font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                            else if (lstQCOKIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])))
                                sHTMLValue += "<div align = 'right'><font color = 'Tomato'>QC:OK</font> | <font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                            else if (lstRejectedRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])))
                                sHTMLValue += "<div align = 'right'><font color = 'Red'>QC:Rejected</font> | <font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                            else
                                sHTMLValue += "<div align = 'right'><font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                        }
                        else
                            sHTMLValue += "<div align = 'right'><font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                    }
                    else if (dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString().Length > 0 && lstRecordsToUnlock.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])))
                    {
                        if (lstBouncedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])) && lstSendBackRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])))
                            sHTMLValue += "<div align = 'right'><font color = 'Red'>Bounced | SendBack</font> | <font color = 'Gray'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                        else if (lstBouncedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])))
                            sHTMLValue += "<div align = 'right'><font color = 'Red'>Bounced</font> | <font color = 'Gray'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                        else if (lstSendBackRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])))
                            sHTMLValue += "<div align = 'right'><font color = 'Red'>QC:SendBack</font> | <font color = 'Gray'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                    }
                    else
                        sHTMLValue += "<div align = 'right'><font color = 'Gray'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";

                    if ((GV.sUserType != "Agent") || GV.sShowDetailedContact == "Y")
                    {
                        if (dtMasterContacts.Rows[i]["TR_Agent_Name"].ToString().Length > 0 && dtMasterContacts.Rows[i]["WR_Agent_Name"].ToString().Length > 0)
                            sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Navy'>TR: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[i]["TR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[i]["TR_UPDATED_DATE"] + " |  WR: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[i]["WR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[i]["WR_UPDATED_DATE"] + "</font></div>";
                        else if (dtMasterContacts.Rows[i]["TR_Agent_Name"].ToString().Length > 0)
                            sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Navy'>TR: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[i]["TR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[i]["TR_UPDATED_DATE"] + "</font></div>";
                        else if (dtMasterContacts.Rows[i]["WR_Agent_Name"].ToString().Length > 0)
                            sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Navy'>WR: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[i]["WR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[i]["WR_UPDATED_DATE"] + "</font></div>";

                        //if (dtMasterContacts.Rows[i]["Rejection"].ToString().Length > 0 && dtMasterContacts.Rows[i]["Review_Tag"].ToString().Length > 0)
                        //    sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'ForestGreen'>Rejection: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[i]["Rejection"].ToString()) + "</font> | <font size='7' face ='Microsoft Sans Serif' color = 'Crimson'>Review: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[i]["Review_Tag"].ToString()) + "</font></div>";
                        //else if (dtMasterContacts.Rows[i]["Rejection"].ToString().Length > 0)
                        //    sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'ForestGreen'>Rejection: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[i]["Rejection"].ToString()) + "</font></div>";
                        //else if (dtMasterContacts.Rows[i]["Review_Tag"].ToString().Length > 0)
                        //    sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Crimson'>Review: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[i]["Review_Tag"].ToString()) + "</font></div>";
                        if (dtMasterContacts.Rows[i]["Review_Tag"].ToString().Length > 0)
                            sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Crimson'>Review: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[i]["Review_Tag"].ToString()) + "</font></div>";
                    }

                    sHTMLValue += "</div>";
                    #endregion

                    GridRow gridRow = new GridRow();
                    GridCell gridCellHTMLContact = new GridCell();
                    GridCell gridCellUniqueNumber = new GridCell();

                    if (GV.sUserType == "Agent")
                    {
                        if ((GV.sAccessTo == "TR" && GV.lstWR_DeleteStatus.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase)) || (GV.sAccessTo == "WR" && GV.lstTR_DeleteStatus.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase)))
                        {
                            Background b = new Background(Color.Gainsboro);
                            gridRow.CellStyles.Default.Background = b;
                            gridRow.CellStyles.Default.TextColor = Color.Gray;
                        }
                    }
                    else
                    {
                        if (GV.lstWR_DeleteStatus.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase) || GV.lstTR_DeleteStatus.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                        {
                            Background b = new Background(Color.Gainsboro);
                            gridRow.CellStyles.Default.Background = b;
                            gridRow.CellStyles.Default.TextColor = Color.Gray;
                        }
                    }

                    gridCellUniqueNumber.Value = i;//Row index of normal grid                    
                    gridCellHTMLContact.Value = sHTMLValue;
                    gridRow.Cells.Add(gridCellHTMLContact);
                    gridRow.Cells.Add(gridCellUniqueNumber);

                    if (dtMasterContacts.Rows[i]["MASTER_ID"].ToString() == sMaster_ID)
                        gridRow.Visible = true;
                    else
                        gridRow.Visible = false;

                    if (GV.lstSortableContactColumn != null)
                    {
                        for (int j = 0; j < GV.lstSortableContactColumn.Count; j++)
                        {
                            GridCell gridCell = new GridCell();
                            gridRow.Cells.Add(gridCell);
                        }
                    }

                    gridRow.Expanded = true;
                    this.sdgvContacts.PrimaryGrid.Rows.Add(gridRow);

                    if (GV.lstSortableContactColumn != null)//Populate value For sorting
                    {
                        foreach (string sColumn in GV.lstSortableContactColumn)
                        {
                            ((GridRow)sdgvContacts.PrimaryGrid.Rows[sdgvContacts.PrimaryGrid.Rows.Count - 1]).Cells[sColumn].Value = dtMasterContacts.Rows[i][sColumn].ToString();
                        }
                    }
                }

                if (sdgvContacts.PrimaryGrid.Rows.Count == dtMasterContacts.Rows.Count)
                    sdgvContacts.PrimaryGrid.Columns[0].HeaderText = string.Empty;
                else
                {
                    sdgvContacts.PrimaryGrid.Columns[0].HeaderText = sFilterHeader = "<div align = 'Right'>Filtered Contacts <b>" + sdgvContacts.PrimaryGrid.VisibleRowCount + "</b> of <b>" + dtMasterContacts.Select("MASTER_ID = " + sMaster_ID).Length + "</b></div>";
                    //sFilterHeader = "<div align = 'Right'>Filtered Contacts <b>" + sdgvContacts.PrimaryGrid.Rows.Count + "</b> of <b>" + dtMasterContacts.Rows.Count + "</b></div>";
                }



                if (dtMasterContacts.Select("MASTER_ID = " + sMaster_ID).Length > 0)
                {
                    for (int index = 0; index < dtMasterContacts.Rows.Count; index++)
                    {
                        if (dtMasterContacts.Rows[index]["MASTER_ID"].ToString() == sMaster_ID)
                        {
                            Impact_ContactTable(index);
                            break;
                        }
                    }
                }

                IsSuperGridLoading = false;

                AlignContactGridColumn();
                FreezeEmptyContact(); //If contact is empty then freeze contacts controls

            }
        }

        void ReloadChart()
        {
            //rbnBarEmpDetails.Visible = false;
            //rbnBarDashBoard.Visible = false;
            //rbnBarOverAllDashboard.Visible = false;

            string sStart = string.Empty;
            string sEndDate = string.Empty;
            if (GM.GetDateTime().Day < 25)
            {
                if (GM.GetDateTime().Month == 1)
                    sStart = (GM.GetDateTime().Year - 1) + "-12-25";
                else
                    sStart = GM.GetDateTime().Year + "-" + (GM.GetDateTime().Month - 1) + "-25";

                sEndDate = GM.GetDateTime().Year + "-" + GM.GetDateTime().Month + "-24";
            }
            else
            {
                sStart = GM.GetDateTime().Year + "-" + GM.GetDateTime().Month + "-25";

                if (GM.GetDateTime().Month == 12)
                    sEndDate = (GM.GetDateTime().Year + 1) + "-01-24";
                else
                    sEndDate = GM.GetDateTime().Year + "-" + (GM.GetDateTime().Month + 1) + "-24";
            }

            //25-03-2015
            //24-04-2015

            string sProjectTopperID = string.Empty;
            string sMTDTopperID = string.Empty;
            string sMTDRank = "DECLARE @FromDate DATE = (Select convert(DATE,'" + sStart + "')) ";

            sMTDRank += "DECLARE @ToDate DATE = (Select convert(DATE,'" + sEndDate + "')) ";

            sMTDRank += @";WITH CTE AS (
                                SELECT  D2.DATECALLED,D2.AGENTNAME, D3.Fullname AgentFullName,ISNULL(NO_OF_CONTACTS_VALIDATED - (ISNULL(EMAIL_REJECTION,0)+ISNULL(JOB_TITLE_REJECTION,0)+ISNULL(INCORRECT_DISPOSAL,0)+ISNULL(MATCHED_WITH_EXCLUSION,0)+ISNULL(MATCHED_WITH_PREVIOUS_SET,0)+ISNULL(OTHERS_REJECTION,0)) ,0) AchievedContacts,POINTS POINTS ,Tenure,D3.EmployeeNo
                                FROM DAILY_AGENT_PERFORMANCE_V1 D2
                                INNER JOIN Timesheet..Users D3
                                ON D3.UserName=D2.AGENTNAME
                                WHERE  DATECALLED BETWEEN @FromDate AND @ToDate
                                AND D3.ACTIVE='Y' AND d2.FLAG = '" + GV.sAccessTo + "'),CTECONSOLIDATE AS (SELECT AGENTNAME,SUM(POINTS) MTDPOINTS,SUM( AchievedContacts) AchievedContacts FROM CTE  GROUP BY AGENTNAME),";
            sMTDRank += @"CTE_FIN AS
                                (SELECT C2.AgentFullName,C2.AgentName,C1.AchievedContacts,C1.MTDPOINTS,C2.EmployeeNo,Tenure,RANK() OVER (ORDER BY MTDPOINTS DESC) RANKING
                                FROM CTECONSOLIDATE C1
                                INNER JOIN CTE C2 ON C1.AGENTNAME = C2.AGENTNAME
                                group by C2.AgentFullName,C1.AchievedContacts,C1.MTDPOINTS,C2.EmployeeNo,Tenure,C2.agentname)
                                select * from cte_fin where (agentname='" + GV.sEmployeeName + "' or ranking = 1);";

            string sProject = @"SELECT  D3.EmployeeNo ,
                                D3.Fullname AgentFullName ,
                                ISNULL(NO_OF_CONTACTS_VALIDATED - (ISNULL(EMAIL_REJECTION,0)+ISNULL(JOB_TITLE_REJECTION,0)+ISNULL(INCORRECT_DISPOSAL,0)+ISNULL(MATCHED_WITH_EXCLUSION,0)+ISNULL(MATCHED_WITH_PREVIOUS_SET,0)+ISNULL(OTHERS_REJECTION,0)) ,0) AchievedContacts ,
                                LastSeen LastSeen ,POINTS ,SELF_TARGET ,Project_rank ,Tenure,average
                                FROM    DASHBOARD D1 INNER JOIN DAILY_AGENT_PERFORMANCE_V1 D2 ON D2.DASHBOARD_ID = D1.ID
                                AND D2.DASHBOARD_ID = " + GV.sDashBoardID + " AND D2.DATECALLED = CONVERT(DATE, GETDATE()) AND D2.FLAG = '" + GV.sAccessTo + "'INNER JOIN Timesheet..Users D3 ON D3.UserName = D2.AGENTNAME order by PROJECT_RANK";

            ChartItemTarget.DataPoints.Clear();
            ChartItemTarget.DataPointTooltips.Clear();
            ChartItemTarget.PieChartStyle.SliceColors.Clear();
            ChartItemTarget.PieChartStyle.SliceColors.Add(Color.Orange);
            ChartItemTarget.PieChartStyle.SliceColors.Add(Color.Green);


            DataTable dtCurrentMTD = GV.MSSQL.BAL_ExecuteQuery(sMTDRank);
            DataTable dtProject = GV.MSSQL.BAL_ExecuteQuery(sProject);

            lblMTDRank.Text = "MTD Rank :";
            lblMTDTopper.Text = string.Empty;
            lblProjectTopper.Text = string.Empty;
            lblTarget.Text = "Target for the day :";
            lblAchived.Text = "Done for the day :";
            lblTeamAvg.Text = "Team Avg :";
            lblProjectRank.Text = "Project Rank :";
            pictureMTDTopper.Image = GCC.Properties.Resources.Misc_User_icon__1_;
            pictureProjectTopper.Image = GCC.Properties.Resources.Misc_User_icon__1_;



            if (dtCurrentMTD.Rows.Count > 0)
            {
                DataRow[] drrCurrentEmp = dtCurrentMTD.Select("AgentName ='" + GV.sEmployeeName + "'");
                if (drrCurrentEmp.Length > 0 && drrCurrentEmp[0]["Ranking"].ToString().Length > 0)
                    lblMTDRank.Text = "MTD Rank : <b>" + GM.NumberSuffix(drrCurrentEmp[0]["Ranking"].ToString()) + "</b>";
                DataRow[] drrTopper = dtCurrentMTD.Select("Ranking = '1'");
                if (drrTopper.Length > 0)
                {
                    lblMTDTopper.Text = ProperCaseHelper.NameANDJobTitleCasing(drrTopper[0]["AgentFullName"].ToString().ToLower(), "Name");
                    sMTDTopperID = drrTopper[0]["EmployeeNo"].ToString().Trim().ToLower();
                }
            }

            if (dtProject.Rows.Count > 0)
            {
                sProjectTopperID = dtProject.Rows[0]["EmployeeNo"].ToString().Trim().ToLower();
                lblProjectTopper.Text = ProperCaseHelper.NameANDJobTitleCasing(dtProject.Rows[0]["AgentFullName"].ToString().ToLower(), "Name");
                DataRow[] drrProjectPosition = dtProject.Select("EmployeeNo = '" + GV.sEmployeeNo + "'");
                if (drrProjectPosition.Length > 0)
                {
                    lblTarget.Text = "Target for the day : <b>" + drrProjectPosition[0]["SELF_TARGET"].ToString() + "</b>";
                    lblAchived.Text = "Done for the day : <b>" + drrProjectPosition[0]["AchievedContacts"].ToString() + "</b>";
                    lblTeamAvg.Text = "Team Avg : <b>" + drrProjectPosition[0]["Average"].ToString() + "</b>";
                    lblProjectRank.Text = "Project Rank : <b>" + GM.NumberSuffix(drrProjectPosition[0]["Project_Rank"].ToString()) + "</b>";

                    if (drrProjectPosition[0]["SELF_TARGET"].ToString().Length > 0)
                    {
                        ChartItemTarget.DataPoints.Add(Convert.ToInt32(drrProjectPosition[0]["SELF_TARGET"].ToString()));
                        ChartItemTarget.DataPointTooltips.Add("Target: " + drrProjectPosition[0]["SELF_TARGET"].ToString());
                        if (drrProjectPosition[0]["AchievedContacts"].ToString().Length > 0)
                        {
                            ChartItemTarget.DataPoints.Add(Convert.ToInt32(drrProjectPosition[0]["AchievedContacts"].ToString()));
                            ChartItemTarget.DataPointTooltips.Add("Target: " + drrProjectPosition[0]["AchievedContacts"].ToString());
                        }
                    }
                }
            }

            pictureCurrentEmp.SizeMode = PictureBoxSizeMode.Zoom;
            pictureCurrentEmp.Image = GV.imgEmployeeImage;

            DataTable dtImage = GV.MSSQL.BAL_FetchTable("EmployeeImage", "EmployeeID IN ('" + sProjectTopperID + "','" + sMTDTopperID + "')");
            if (dtImage.Rows.Count > 0)
            {
                foreach (DataRow dr in dtImage.Rows)
                {
                    byte[] bStream = (byte[])dr["EmployeeImage"];
                    MemoryStream ms = new MemoryStream(bStream);
                    if (dr["EmployeeID"].ToString().ToLower() == sProjectTopperID)
                        pictureProjectTopper.Image = Image.FromStream(ms);

                    if (dr["EmployeeID"].ToString().ToLower() == sMTDTopperID)
                        pictureMTDTopper.Image = Image.FromStream(ms);
                }
            }
        }



        //-----------------------------------------------------------------------------------------------------
        private DataTable EmailSuggetionsGeneration()
        {
            List<string> lstDomain = new List<string>();//List of existing email from previous contacts and General Email of company
            //List<string> lstEmailSuggestion = new List<string>();
            List<string> lstContactNameSplit = new List<string>();
            DataTable dtEmailSugg = dtPicklist.Clone();
            //DataTable dtEmail = new DataTable();
            try
            {
                // dtEmail.Columns.Add("PicklistValue", typeof(string));

                //string sFirstName = string.Empty;
                //string sMiddleName = string.Empty;
                //string sLastName = string.Empty;

                //string sF = string.Empty;//First Letter of firstname
                //string sMName = string.Empty;//First Letter of Middle Name
                //string sL = string.Empty;//First Letter of Lastname

                lstContactNameSplit = GetContactNameSplit();
                lstDomain = GetDomainList();

                //sFirstName = lstContactNameSplit[0];
                //sMiddleName = lstContactNameSplit[1];
                //sLastName = lstContactNameSplit[2];
                //sF = lstContactNameSplit[3];
                //sMName = lstContactNameSplit[4];
                //sL = lstContactNameSplit[5];


                //FirstName                                 LastName
                if ((lstContactNameSplit[0].Trim().Length > 0 || lstContactNameSplit[2].Trim().Length > 0) && lstDomain.Count > 0)
                {
                    foreach (string sDomainName in lstDomain)                                             //FirstName              LastName                FirstNameFirstLetter    LastNameFirstLetter     MiddleName              MiddleNameFirstLetter
                        dtEmailSugg = Load_EmailSuggestion_Loading(sDomainName, dtEmailSugg, lstContactNameSplit[0], lstContactNameSplit[2], lstContactNameSplit[3], lstContactNameSplit[5], lstContactNameSplit[1], lstContactNameSplit[4]);

                    //foreach (string sEmailSugg in lstEmailSuggestion)
                    //{
                    //    DataRow dr = dtEmail.NewRow();
                    //    dr["PicklistValue"] = sEmailSugg;
                    //    dtEmail.Rows.Add(dr);
                    //}
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return dtEmailSugg;
        }


        //-----------------------------------------------------------------------------------------------------
        private List<string> GetDomainList()
        {
            List<string> lstExistingDomain = new List<string>();//List of existing email from previous contacts and General Email of company
            string sWebSite = string.Empty;//Website of company
            string sEmailDomain = string.Empty;

            //foreach (TextBox txt in lstCompanyControls)//Get website from wbesite textbox
            //{
            //    if (txt.Name.ToUpper() == "WEB")
            //    {
            //        if (GM.Web_Check(txt.Text))
            //            sWebSite = txt.Text;
            //    }
            //}

            if (GM.Web_Check(dtMasterCompanies.Rows[iCompanyRowIndex]["WEB"].ToString()))
                sWebSite = dtMasterCompanies.Rows[iCompanyRowIndex]["WEB"].ToString();

            //Get General Email of Company
            if (dtMasterCompanies.Rows[iCompanyRowIndex]["GENERAL_EMAIL"].ToString().Length > 0 && GM.Email_Check(dtMasterCompanies.Rows[iCompanyRowIndex]["GENERAL_EMAIL"].ToString()))
                lstExistingDomain.Add(dtMasterCompanies.Rows[iCompanyRowIndex]["GENERAL_EMAIL"].ToString());

            foreach (DataRow dr in dtMasterContacts.Rows) //Get list of existing contact Email, so that different domain can be extracted
            {
                if (dr["MASTER_ID"].ToString() == sMaster_ID && dr["CONTACT_EMAIL"].ToString().Length > 0 && GM.Email_Check(dr["CONTACT_EMAIL"].ToString()))
                    lstExistingDomain.Add(dr["CONTACT_EMAIL"].ToString());
            }

            if (sWebSite.Length > 0)//Get Domin from website to form email
            {
                List<string> lstWebSplit = new List<string>();
                lstWebSplit = sWebSite.Split('/').ToList()[0].Split('.').ToList();

                for (int i = 1; i < lstWebSplit.Count; i++)
                {
                    if (sEmailDomain.Length > 0)
                        sEmailDomain += "." + lstWebSplit[i];
                    else
                        sEmailDomain += lstWebSplit[i];
                }
                sEmailDomain = "@" + sEmailDomain;
                lstExistingDomain.Add(sEmailDomain);
            }

            for (int i = 0; i < lstExistingDomain.Count; i++)
            {
                lstExistingDomain[i] = lstExistingDomain[i].Split('@').ToList()[1].Trim();
            }
            lstExistingDomain = lstExistingDomain.Distinct().ToList();//Contains List Domains

            return lstExistingDomain;
        }

        //-----------------------------------------------------------------------------------------------------
        private List<string> GetContactNameSplit()
        {
            List<string> lstContactNameSplit = new List<string>();

            string sFirstName = string.Empty;
            string sMiddleName = string.Empty;
            string sLastName = string.Empty;

            string sF = string.Empty;//First Letter of firstname
            string sMName = string.Empty;//First Letter of Middle Name
            string sL = string.Empty;//First Letter of Lastname

            foreach (TextBox txt in lstContactControls)//Get 'FirstName' and 'LastName' from mastercontact textboxes
            {
                if (txt.Name.ToUpper() == "FIRST_NAME")
                    sFirstName = txt.Text;
                else if (txt.Name.ToUpper() == "LAST_NAME")
                    sLastName = txt.Text;
            }


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

            lstContactNameSplit.Add(sFirstName);
            lstContactNameSplit.Add(sMiddleName);
            lstContactNameSplit.Add(sLastName);
            lstContactNameSplit.Add(sF);
            lstContactNameSplit.Add(sMName);
            lstContactNameSplit.Add(sL);
            return lstContactNameSplit;
        }

        //-----------------------------------------------------------------------------------------------------
        private string TelephoneFormat(string sTelephone, string sCompanyORContacts, int iIndex)
        {
            try
            {
                string sCountry = string.Empty;
                if (sCompanyORContacts == "Master")
                {
                    if (iIndex < 0)
                        iIndex = iCompanyRowIndex;
                    //foreach (TextBox txt in lstCompanyControls)//Get 'Country from Country TextBox (Master)
                    //{
                    //    if (txt.Name.ToUpper() == "COUNTRY")
                    //        sCountry = txt.Text;
                    //}

                    sCountry = dtMasterCompanies.Rows[iIndex]["COUNTRY"].ToString();
                }
                else if (sCompanyORContacts == "Contacts")
                {
                    if (iIndex < 0)
                        iIndex = iContactRowIndex;
                    //foreach (TextBox txt in lstContactControls)//Get 'Country from Country TextBox (Contacts)
                    //{
                    //    if (txt.Name.ToUpper() == "CONTACT_COUNTRY")
                    //        sCountry = txt.Text;
                    //}
                    sCountry = dtMasterContacts.Rows[iIndex]["CONTACT_COUNTRY"].ToString();
                    if (sCountry.Length == 0)//If contury not found then use company country
                    {
                        //foreach (TextBox txt in lstCompanyControls)//Get 'Country from Country TextBox (Master)
                        //{
                        //    if (txt.Name.ToUpper() == "COUNTRY")
                        //        sCountry = txt.Text;
                        //}
                        sCountry = dtMasterCompanies.Rows[iCompanyRowIndex]["COUNTRY"].ToString();
                    }
                }

                if (sCountry.Length > 0 && dtCountryInformation.Select(String.Format("CountryName = '{0}'", sCountry.Replace("'", "''"))).Length > 0)
                {
                    Regex rNumeric = new Regex(@"[a-zA-Z]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                    DataTable dtTelephoneFormatCountry = dtCountryInformation.Select("CountryName = '" + sCountry.Replace("'", "''") + "'").CopyToDataTable();
                    string sCountryCode = dtTelephoneFormatCountry.Rows[0]["DialCode"].ToString();
                    bool IsZeroAllowedCountry = (dtTelephoneFormatCountry.Rows[0]["CountryStartsWithZero"].ToString() == "Y");

                    if (sTelephone.Trim() == "+" + sCountryCode)//Clear telephone only if country in textbox(solves 'country code not deleting' problem)
                        return string.Empty;

                    sTelephone = rNumeric.Replace(sTelephone, string.Empty);//Removes all alpha
                    sTelephone = Remove_SplChars(sTelephone).Replace(" ", "");//Removes all spl chars including '+'

                    if (GV.sAllowTelephoneFormating == "Y" && sTelephone.Trim().Length > 0)
                    {
                        string TelStartsWithZero = string.Empty;

                        sTelephone = "+" + sTelephone;
                        if (sTelephone.StartsWith("+" + sCountryCode))//Eg:(+919790783282)
                        {
                            sTelephone = sTelephone.Replace("+" + sCountryCode, string.Empty);
                            TelStartsWithZero = sTelephone;
                            sTelephone = TelephoneDigitBasedFormat(sTelephone);
                        }
                        else if (sTelephone.StartsWith("+0" + sCountryCode))//Eg:(+0919790783282)
                        {
                            sTelephone = sTelephone.Replace("+0" + sCountryCode, string.Empty);
                            TelStartsWithZero = sTelephone;
                            sTelephone = TelephoneDigitBasedFormat(sTelephone);
                        }
                        else if (sTelephone.StartsWith("+00" + sCountryCode))//Eg:(+00919790783282)
                        {
                            sTelephone = sTelephone.Replace("+00" + sCountryCode, string.Empty);
                            TelStartsWithZero = sTelephone;
                            sTelephone = TelephoneDigitBasedFormat(sTelephone);
                        }
                        else//Eg:(9790783282) With out country code
                        {
                            if (sTelephone.Replace("+", string.Empty) == "0")
                                sTelephone = "0";
                            else
                            {
                                TelStartsWithZero = sTelephone.Replace("+", string.Empty);
                                sTelephone = TelephoneDigitBasedFormat(sTelephone.Replace("+", string.Empty));
                            }
                        }


                        if (sTelephone.Length > 0)
                        {
                            if (TelStartsWithZero.StartsWith("0") && IsZeroAllowedCountry && sTelephone.Replace(" ", string.Empty).Length < 13)
                                sTelephone = "+" + sCountryCode + " 0" + sTelephone.Replace("+", string.Empty);
                            else
                                sTelephone = "+" + sCountryCode + " " + sTelephone.Replace("+", string.Empty);

                            // sTelephone = "+" + sCountryCode + " " + sTelephone.Replace("+", string.Empty);
                        }
                    }
                }
                else if (sCountry.Length == 0)
                    ToastNotification.Show(this, "Country field is Empty.", eToastPosition.TopRight);
                else if (dtCountryInformation.Select(String.Format("CountryName = '{0}'", sCountry.Replace("'", "''"))).Length == 0)
                    ToastNotification.Show(this, "Country does not match.", eToastPosition.TopRight);
            }
            catch (Exception ex)
            {
                //Regex rNumeric = new Regex(@"[a-zA-Z]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                //sTelephone = rNumeric.Replace(sTelephone, string.Empty);
                //TelephoneFormat(sTelephone);
            }
            return sTelephone;
        }

        //-----------------------------------------------------------------------------------------------------
        private string TelephoneDigitBasedFormat(string sTelephone)
        {

            switch (sTelephone.Length)
            {
                case 1:
                    sTelephone = String.Format("{0:#}", double.Parse(sTelephone));
                    break;
                case 2:
                    sTelephone = String.Format("{0:##}", double.Parse(sTelephone));
                    break;
                case 3:
                    sTelephone = String.Format("{0:###}", double.Parse(sTelephone));
                    break;
                case 4:
                    sTelephone = String.Format("{0:### #}", double.Parse(sTelephone));
                    break;
                case 5:
                    sTelephone = String.Format("{0:### ##}", double.Parse(sTelephone));
                    break;
                case 6:
                    sTelephone = String.Format("{0:### ###}", double.Parse(sTelephone));
                    break;
                case 7:
                    sTelephone = String.Format("{0:### ####}", double.Parse(sTelephone));
                    break;
                case 8:
                    sTelephone = String.Format("{0:#### ####}", double.Parse(sTelephone));
                    break;
                case 9:
                    sTelephone = String.Format("{0:### ### ###}", double.Parse(sTelephone));
                    break;
                case 10:
                    sTelephone = String.Format("{0:## #### ####}", double.Parse(sTelephone));
                    break;
                case 11:
                    sTelephone = String.Format("{0:### #### ####}", double.Parse(sTelephone));
                    break;
                case 12:
                    sTelephone = String.Format("{0:#### #### ####}", double.Parse(sTelephone));
                    break;
            }
            return sTelephone;

        }

        //private void BulkEmailUpdate()
        //{
        //    List<string> lstEmailSyntax = new List<string>();
        //    foreach (DataGridViewRow dgvr in dgvContacts.Rows)
        //    {
        //        if (dgvr.Cells["CONTACT_EMAIL"].Value.ToString().Length > 0)
        //        {
        //            lstEmailSyntax.Add(DetermineEmailSyntax(dgvr.Cells["CONTACT_EMAIL"].Value.ToString()));
        //        }
        //    }
        //}



        //-----------------------------------------------------------------------------------------------------
        private DataTable Load_EmailSuggestion_Loading(string sDomainName, DataTable dtEmailsugg, string sFirstName, string sLastName, string sF, string sL, string sMiddleName, string sMName)
        {
            if (sDomainName.Contains("@"))
                sDomainName = "@" + sDomainName.Split('@')[1].Trim();
            else
                sDomainName = "@" + sDomainName;

            //DataTable dtEmailSuggestion = dtPicklist.Select("PicklistCategory = 'EmailSuggestion'", "Remarks ASC").CopyToDataTable();
            try
            {
                foreach (DataRow dr in dtEmailSuggestion.Rows)
                {
                    string sEmailFormation = string.Empty;
                    sEmailFormation = dr["PicklistValue"].ToString().Replace("FirstName", sFirstName);
                    sEmailFormation = sEmailFormation.Replace("LastName", sLastName);
                    sEmailFormation = sEmailFormation.Replace("FName", sF);
                    sEmailFormation = sEmailFormation.Replace("LName", sL);
                    sEmailFormation = sEmailFormation.Replace("MiddleName", sMiddleName);
                    sEmailFormation = sEmailFormation.Replace("MName", sMName);
                    sEmailFormation += sDomainName;

                    DataRow drEmailSugg = dtEmailsugg.NewRow();
                    drEmailSugg["PicklistValue"] = sEmailFormation;
                    drEmailSugg["PicklistCategory"] = dr["PicklistCategory"].ToString();
                    drEmailSugg["PicklistField"] = dr["PicklistField"].ToString();
                    drEmailSugg["remarks"] = dr["remarks"].ToString();
                    drEmailSugg["Sort"] = dr["Sort"];
                    dtEmailsugg.Rows.Add(drEmailSugg);
                    //lstEmail.Add(sEmailFormation);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return dtEmailsugg;
        }


        //-----------------------------------------------------------------------------------------------------
        private void Assign_RunTime_Events()
        {
            // To be called on form loading
            //dtp.KeyDown += new KeyEventHandler(dtp_KeyDown);
            //dtp.Leave += new EventHandler(dtp_Leave);
            tAutoSaveRecords.Tick += new EventHandler(TimerTick); //Auto save records
        }

        //-----------------------------------------------------------------------------------------------------
        private void Miscellaneous()
        {
            try
            {
                //sTabPanelGroupCompany.TabStop = false;
                //dgvContacts.BackgroundColor = splitGroupCompany.Panel2.BackColor; //Set BG for grid to stay with color of its parant
                //dgvContacts.BackgroundColor = spltErrorShow.BackColor;
                //dgvContacts.Font = new System.Drawing.Font(dgvContacts.Font.FontFamily, 10);

                //sliderNotesSize.Value = Convert.ToInt32(txtNotes.Font.Size); //Initial value for Slider
                //sliderResizeGrid.Value = Convert.ToInt32(dgvContacts.Font.Size);
                //sliderResizeGrid.Text = "Size :" + sliderResizeGrid.Value;

                if (dtMasterContacts.Rows.Count > 0)
                {
                    if (dtMasterContacts.Select("MASTER_ID = " + sMaster_ID).Length > 0)
                        rtxtCallScript.Rtf = GM.LoadRTF(dtMasterContacts.Select("MASTER_ID = " + sMaster_ID).CopyToDataTable(), dtMasterCompanies, iCompanyRowIndex);
                }

                tabWRDisposals.Visible = false;
                tabTRDisposals.Visible = false;
                tabQC.Visible = false;
                tabRecordHistory.Visible = false;
                btnAddNewContact.Visible = false;

                if (GV.sUserType == "Agent")
                {
                    btnAddNewContact.Visible = true;//Only Agent can add contact
                    if (GV.sAccessTo == "TR")
                        tabTRDisposals.Visible = true;
                    else if (GV.sAccessTo == "WR")
                        tabWRDisposals.Visible = true;
                }
                else if (GV.sUserType == "Manager")
                {
                    if (GV.sAccessTo == "TR")
                        tabTRDisposals.Visible = true;
                    else if (GV.sAccessTo == "WR")
                        tabWRDisposals.Visible = true;

                    tabRecordHistory.Visible = true;

                }
                else if (GV.sUserType == "Admin")
                {
                    btnAddNewContact.Visible = true;
                    tabWRDisposals.Visible = true;
                    tabTRDisposals.Visible = true;
                    tabQC.Visible = true;
                    tabRecordHistory.Visible = true;
                }
                else if (GV.sUserType == "QC")
                {
                    if (GV.sAccessTo == "TR")
                        tabTRDisposals.Visible = true;
                    else if (GV.sAccessTo == "WR")
                        tabWRDisposals.Visible = true;
                    tabQC.Visible = true;

                    tabRecordHistory.Visible = true;
                }



                txtDialerType.Text = GV.sDialerType;

                txtDialerType.TextBox.ReadOnly = true;
                txtDialExtension.TextBox.ReadOnly = true;
                txtTotalDials.TextBox.ReadOnly = true;
                txtCompanyID.TextBox.ReadOnly = true;
                txtGroupID.TextBox.ReadOnly = true;
                txtTimeZone.TextBox.ReadOnly = true;
                txtDialMain.TextBox.ReadOnly = false;

                txtDialerType.TextBox.ForeColor = Color.Gray;
                txtDialExtension.TextBox.ForeColor = Color.Gray;
                txtTotalDials.TextBox.ForeColor = Color.Gray;

                txtDialerType.TextBox.BackColor = Color.White;
                txtDialExtension.TextBox.BackColor = Color.White;
                txtTotalDials.TextBox.BackColor = Color.White;
                txtCompanyID.TextBox.BackColor = Color.White;
                txtGroupID.TextBox.BackColor = Color.White;
                txtDialMain.TextBox.BackColor = Color.White;

                //if(sFormOpenType == "ListOpen")
                //{
                //    if (GlobalVariables.sAccessTo == "TR")
                //        tabControlMain.SelectedPanel = sTabPanelTRDisposals;
                //    else if (GlobalVariables.sAccessTo == "WR")
                //        tabControlMain.SelectedPanel = sTabPanelWRDisposals;
                //}



                ////Remove Webresearched and Telesearched//
                //List<DataRow> lstDrTobeRemoved = new List<DataRow>();
                //foreach (DataRow dr in dtPicklist.Rows)
                //{
                //    if (dr["PicklistCategory"].ToString().ToUpper() == "TR_CONTACTSTATUS" && (dr["PicklistValue"].ToString().ToUpper() == "WEBRESEARCHED" || dr["PicklistValue"].ToString().ToUpper() == "TELERESEARCHED"))
                //        lstDrTobeRemoved.Add(dr);
                //}

                //foreach (DataRow dr in lstDrTobeRemoved)
                //    dr.Delete();
                //dtPicklist.AcceptChanges();


                //Listbox resizes automatically upon execution
                TR_PRIMARY_DISPOSAL.Height = splitTRDisposals.Panel1.Height - 20;
                TR_SECONDARY_DISPOSAL.Height = splitTRDisposals.Panel1.Height - 20;

                WR_PRIMARY_DISPOSAL.Height = splitWRDisposals.Height - 20;
                WR_SECONDARY_DISPOSAL.Height = splitWRDisposals.Height - 20;

                //TR_COMMENTS.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
                //TR_Comments_Browser.Anchor = AnchorStyles.Right | AnchorStyles.Left;

                //TR_COMMENTS.Height = 80;// (splitTRDisposalMainParant.Panel2.Height) / 4;                

                //TR_Comments_Browser.Height = (splitTRDisposalMainParant.Panel2.Height - TR_COMMENTS.Height) + 30;



                objNotifier.StartPosition = FormStartPosition.CenterScreen;
                objNotifier.btnCancel.Click += new EventHandler(btn_Cancle_Asynch);


            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void Load_MasterCompany()
        {
            try
            {

                //Populate Master fields
                if (dtFieldMasterCompany != null && dtFieldMasterCompany.Rows.Count > 0)
                {

                    dtMasterCompaniesCopy = dtMasterCompanies.Copy();//Copy of company(Disconnected)
                    dtMasterCompaniesCopy.TableName = "MasterCompanyCopy";

                    //Load Controls to listS
                    List<Control> ctrlsToLoad = new List<Control>();
                    ctrlsToLoad = Load_Controls(dtFieldMasterCompany); //Get list of controls to be added in a list
                    AlignControls(ctrlsToLoad, GetDisplaySize(ctrlsToLoad), sTabPanelCompanyInformation); //Adds and aligns the controls to its parant(Form or panel)
                    //lstMasterControls = GetAllControls(splitGroupCompany.Panel2); //Gets all loaded master controls to a list
                    lstCompanyControls = new List<TextBox>();
                    foreach (Control C in ctrlsToLoad)
                    {
                        if (C is TextBox)
                            lstCompanyControls.Add(C as TextBox);
                    }

                    //lstCompanyControls.Add(TR_COMMENTS as TextBox);
                    //lstCompanyControls.Add(WR_COMMENTS as TextBox);


                    IsLoading = true;

                    LoadMasterIds();

                    txtGroupID.Text = sGroup_ID;

                    if (GV.sAccessTo == "TR")
                    {
                        DataRow[] drRecordPRimaryStatusTR = dtRecordStatus.Select("Table_Name = 'Company' AND Research_Type='TR'");
                        if (drRecordPRimaryStatusTR.Length > 0)
                        {
                            DataTable dtRecordPRimaryStatusTR = drRecordPRimaryStatusTR.CopyToDataTable().DefaultView.ToTable(true, "Primary_Status");
                            dtRecordPRimaryStatusTR.DefaultView.Sort = "Primary_Status ASC";
                            TR_PRIMARY_DISPOSAL.DataSource = dtRecordPRimaryStatusTR;
                            TR_PRIMARY_DISPOSAL.DisplayMember = "Primary_Status";
                            TR_PRIMARY_DISPOSAL.ValueMember = "Primary_Status";

                        }
                    }
                    else if (GV.sAccessTo == "WR")
                    {
                        DataRow[] drRecordPRimaryStatusWR = dtRecordStatus.Select("Table_Name = 'Company' AND Research_Type='WR'");
                        if (drRecordPRimaryStatusWR.Length > 0)
                        {
                            DataTable dtRecordPRimaryStatusWR = drRecordPRimaryStatusWR.CopyToDataTable().DefaultView.ToTable(true, "Primary_Status");
                            dtRecordPRimaryStatusWR.DefaultView.Sort = "Primary_Status ASC";
                            WR_PRIMARY_DISPOSAL.DataSource = dtRecordPRimaryStatusWR;
                            WR_PRIMARY_DISPOSAL.DisplayMember = "Primary_Status";
                            WR_PRIMARY_DISPOSAL.ValueMember = "Primary_Status";

                        }
                    }
                    IsLoading = false;

                    PopulateMasterCompanyFields();//Populates data to Master Company fields
                    PopulateDisposals(); //Populate data on disposals (Static Fields)
                    LoadSuperGridCompany();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void LoadMasterIds()
        {
            lstMasterIDs.Clear();
            foreach (DataRow drCompanies in dtMasterCompanies.Rows)
                lstMasterIDs.Add(drCompanies["MASTER_ID"].ToString());
        }

        //-----------------------------------------------------------------------------------------------------
        private void PopulateDisposals()//Populate Disposals
        {
            try
            {
                if (!IsLoading)
                {
                    IsLoading = true;
                    if (GV.sAccessTo == "TR")
                    {
                        TR_PRIMARY_DISPOSAL.ClearSelected();
                        TR_SECONDARY_DISPOSAL.DataSource = null;

                        //txtTRDateOfCalling.Text = dtMasterCompanies.Rows[iCompanyRowIndex]["TR_DATECALLED"].ToString();
                        TR_COMMENTS.Text = dtMasterCompanies.Rows[iCompanyRowIndex]["TR_COMMENTS"].ToString();
                        TR_Comments_Browser.DocumentText = Interpret_Comments(dtMasterCompanies.Rows[iCompanyRowIndex]["TR_COMMENTS_LOG"].ToString());
                        txtNotes.Text = dtMasterCompanies.Rows[iCompanyRowIndex]["TR_AGENTNOTES"].ToString();
                        //txtTRNoOfTimesCalled.Text = dtMasterCompanies.Rows[iCompanyRowIndex]["TR_TIMESCALLED"].ToString();

                        if (sFormOpenType == "ListOpen" || sFormOpenType == "SendBack")
                        {
                            if (dtMasterCompanies.Rows[iCompanyRowIndex]["TR_PRIMARY_DISPOSAL"] != null)
                            {
                                TR_PRIMARY_DISPOSAL.SelectedValue = dtMasterCompanies.Rows[iCompanyRowIndex]["TR_PRIMARY_DISPOSAL"].ToString();
                                if (dtMasterCompanies.Rows[iCompanyRowIndex]["TR_SECONDARY_DISPOSAL"] != null)
                                {
                                    DataRow[] drRecordStatusTR = dtRecordStatus.Select("Research_Type = 'TR' AND Table_Name = 'Company' AND Primary_Status = '" + dtMasterCompanies.Rows[iCompanyRowIndex]["TR_PRIMARY_DISPOSAL"] + "'");
                                    if (drRecordStatusTR.Length > 0)
                                    {
                                        TR_SECONDARY_DISPOSAL.DataSource = drRecordStatusTR.CopyToDataTable();
                                        TR_SECONDARY_DISPOSAL.DisplayMember = "Secondary_Status";
                                        TR_SECONDARY_DISPOSAL.ValueMember = "Secondary_Status";
                                        TR_SECONDARY_DISPOSAL.SelectedValue = dtMasterCompanies.Rows[iCompanyRowIndex]["TR_SECONDARY_DISPOSAL"].ToString();
                                    }
                                }
                            }
                        }

                        TR_PRIMARY_DISPOSAL.Enabled = true;//If disabled.. Enable it for next record
                        TR_SECONDARY_DISPOSAL.Enabled = true;
                        btnSave.Enabled = true;
                        btnAddNewContact.Enabled = true;
                        TR_COMMENTS.Enabled = true;
                        txtNotes.Enabled = true;
                        btnWindowed.Enabled = true;

                        if (GV.sUserType == "Agent" && lstFreezedMasterIDs.Contains(Convert.ToInt32(dtMasterCompanies.Rows[iCompanyRowIndex]["MASTER_ID"])))
                        {
                            btnSave.Enabled = false;//Diable save for complete survay
                            TR_PRIMARY_DISPOSAL.Enabled = false;
                            TR_SECONDARY_DISPOSAL.Enabled = false;
                            btnAddNewContact.Enabled = false;
                            TR_COMMENTS.Enabled = false;
                            txtNotes.Enabled = false;
                            btnWindowed.Enabled = false;
                            ToastNotification.Show(this, "Completed Company cannot be edited.", eToastPosition.TopRight);
                        }
                    }
                    else if (GV.sAccessTo == "WR")
                    {
                        WR_PRIMARY_DISPOSAL.ClearSelected();
                        WR_SECONDARY_DISPOSAL.DataSource = null;

                        //txtWRDate.Text = dtMasterCompanies.Rows[iCompanyRowIndex]["WR_DATE_OF_PROCESS"].ToString();
                        WR_COMMENTS.Text = dtMasterCompanies.Rows[iCompanyRowIndex]["WR_COMMENTS"].ToString();
                        WR_Comments_Browser.DocumentText = Interpret_Comments(dtMasterCompanies.Rows[iCompanyRowIndex]["WR_COMMENTS_LOG"].ToString());
                        txtNotes.Text = dtMasterCompanies.Rows[iCompanyRowIndex]["WR_AGENTNOTES"].ToString();
                        if (sFormOpenType == "ListOpen" || sFormOpenType == "SendBack")
                        {
                            if (dtMasterCompanies.Rows[iCompanyRowIndex]["WR_PRIMARY_DISPOSAL"] != null)
                            {
                                WR_PRIMARY_DISPOSAL.SelectedValue = dtMasterCompanies.Rows[iCompanyRowIndex]["WR_PRIMARY_DISPOSAL"].ToString();
                                if (dtMasterCompanies.Rows[iCompanyRowIndex]["WR_SECONDARY_DISPOSAL"] != null)
                                {
                                    DataRow[] drRecordStatusWR = dtRecordStatus.Select("Research_Type = 'WR' AND Table_Name = 'Company' AND Primary_Status = '" + dtMasterCompanies.Rows[iCompanyRowIndex]["WR_PRIMARY_DISPOSAL"] + "'");
                                    if (drRecordStatusWR.Length > 0)
                                    {
                                        WR_SECONDARY_DISPOSAL.DataSource = drRecordStatusWR.CopyToDataTable();
                                        WR_SECONDARY_DISPOSAL.DisplayMember = "Secondary_Status";
                                        WR_SECONDARY_DISPOSAL.ValueMember = "Secondary_Status";
                                        WR_SECONDARY_DISPOSAL.SelectedValue = dtMasterCompanies.Rows[iCompanyRowIndex]["WR_SECONDARY_DISPOSAL"].ToString();
                                    }
                                }
                            }
                        }

                        btnSave.Enabled = true;
                        WR_PRIMARY_DISPOSAL.Enabled = true;
                        WR_SECONDARY_DISPOSAL.Enabled = true;
                        btnAddNewContact.Enabled = true;
                        WR_COMMENTS.Enabled = true;
                        txtNotes.Enabled = true;
                        btnWindowed.Enabled = true;

                        if (GV.sUserType == "Agent" && lstFreezedMasterIDs.Contains(Convert.ToInt32(dtMasterCompanies.Rows[iCompanyRowIndex]["MASTER_ID"])))
                        {
                            btnSave.Enabled = false;//Diable save for complete survay
                            WR_PRIMARY_DISPOSAL.Enabled = false;
                            WR_SECONDARY_DISPOSAL.Enabled = false;
                            btnAddNewContact.Enabled = false;
                            WR_COMMENTS.Enabled = false;
                            txtNotes.Enabled = false;
                            btnWindowed.Enabled = false;
                            ToastNotification.Show(this, "Completed Company cannot be edited.", eToastPosition.TopRight);
                        }
                    }
                    IsLoading = false;
                }

                //Freeze opposit research completes
                //if (GV.sUserType == "Agent" && (GV.sFreezeWRCompanyCompletes == "Y" && GV.sAccessTo == "TR") || (GV.sFreezeTRCompanyCompletes == "Y" && GV.sAccessTo == "WR"))//Freeze company completes
                //{
                //    string sOppositResearchType = string.Empty;
                //   string sFreezedDisposals = string.Empty;
                //    if (GV.sAccessTo == "TR")
                //        sOppositResearchType = "WR";
                //    else
                //        sOppositResearchType = "TR";

                //   DataRow[] drrDisposals = dtRecordStatus.Select("Research_Type = '"+sOppositResearchType+"'  AND Table_Name='Company' AND Operation_Type LIKE '%Validate%'");
                //   if(drrDisposals.Length > 0)
                //   {
                //       List<string> lstFreezedDisposals =  new List<string>();
                //       foreach(DataRow dr in drrDisposals)
                //           lstFreezedDisposals.Add(dr["Primary_Status"].ToString());
                //       string sSelectedDisposal = dtMasterCompanies.Rows[0][sOppositResearchType + "_PRIMARY_DISPOSAL"].ToString();

                //       if (lstFreezedDisposals.Contains(sSelectedDisposal, StringComparer.OrdinalIgnoreCase))
                //       {
                //           btnSave.Enabled = false;//Diable save for complete survay
                //           if (GV.sAccessTo == "TR")
                //           {
                //               lstTRPrimaryDisposal.Enabled = false;
                //               lstTRSecondaryDisposal.Enabled = false;
                //           }
                //           else if (GV.sAccessTo == "WR")
                //           {
                //               lstWRPrimaryDisposal.Enabled = false;
                //               lstWRSeconderyDisposal.Enabled = false;
                //           }
                //           ToastNotification.Show(this, sOppositResearchType+ " Complete Survey cannot be edited.", eToastPosition.TopRight);
                //       }
                //   }
                //}

            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        string Interpret_Comments(string sComment_Log)
        {
            string sReturn = string.Empty;
            string sStyle = string.Empty;
            if (sComment_Log.Length > 0)
            {
                sStyle = @"<style>
                            *{
                            font-family: 'Microsoft Sans Serif';                            
                            font-size: 12px;
                            color: Gray;
                            }                            

                            #Time {
                            font-size: x-small;  
                            color: RebeccaPurple;  
                            font-style: italic;      
                            font-family: 'Times New Roman';
                            letter-spacing: 0.5px;
                            }

                            #Comment {
                            font-size: x-small;                            
                            color: DarkSlateGray;                            
                            }

                            #AgentName {
                            color: Tomato;
                            font-size: x-small;                            
                            }
                            </style>";
                List<string> lstComments = sComment_Log.Split(new string[] { "]|[" }, StringSplitOptions.None).ToList();
                lstComments.Reverse();
                if (lstComments.Count > 0)
                {
                    lstComments[lstComments.Count - 1] = lstComments[lstComments.Count - 1].Substring(1);
                    lstComments[0] = lstComments[0].Remove(lstComments[0].Length - 1);
                    foreach (string sComment in lstComments)
                    {
                        List<string> lstCommentBreakup = sComment.Split(new string[] { "]:[" }, StringSplitOptions.None).ToList();

                        if (lstCommentBreakup.Count > 2)
                        {
                            sReturn += "commented by<span id = 'AgentName'> " + lstCommentBreakup[0] + " </span>on <span id = 'Time'>" + lstCommentBreakup[1] + "</span>, " + GM.TimeAgo(Convert.ToDateTime(lstCommentBreakup[1].Replace(".", string.Empty))) + " <br/><br/><span id = 'Comment'>" + lstCommentBreakup[2].Replace("\n", "<br/>") + "</span><hr/>";
                        }
                    }
                }
            }

            return sStyle + sReturn;
        }

        //-----------------------------------------------------------------------------------------------------
        private void lstTRPrimaryDisposal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!IsLoading)
                {
                    if (TR_PRIMARY_DISPOSAL.SelectedValue != null && TR_PRIMARY_DISPOSAL.SelectedValue.ToString() != "System.Data.DataRowView")
                    {

                        //{
                        //IsLoading = true;
                        dtMasterCompanies.Rows[iCompanyRowIndex]["TR_PRIMARY_DISPOSAL"] = TR_PRIMARY_DISPOSAL.SelectedValue;

                        //IsLoading = false;
                        //}

                        DataRow[] drRecordStatusTR = dtRecordStatus.Select("Research_Type = 'TR' AND Table_Name = 'Company' AND Primary_Status = '" + TR_PRIMARY_DISPOSAL.SelectedValue + "'");
                        if (drRecordStatusTR.Length > 0)
                        {
                            TR_SECONDARY_DISPOSAL.DataSource = drRecordStatusTR.CopyToDataTable();
                            TR_SECONDARY_DISPOSAL.DisplayMember = "Secondary_Status";
                            TR_SECONDARY_DISPOSAL.ValueMember = "Secondary_Status";
                            //lstTRSecondaryDisposal.Focus();
                        }

                        RefreshCompanyGrid(iCompanyRowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void lstWRPrimaryDisposal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!IsLoading)
                {
                    if (WR_PRIMARY_DISPOSAL.SelectedValue != null && WR_PRIMARY_DISPOSAL.SelectedValue.ToString() != "System.Data.DataRowView")
                    {

                        //{       //IsLoading = true;
                        dtMasterCompanies.Rows[iCompanyRowIndex]["WR_PRIMARY_DISPOSAL"] = WR_PRIMARY_DISPOSAL.SelectedValue;

                        //IsLoading = false;
                        //}

                        DataRow[] drRecordStatusWR = dtRecordStatus.Select("Research_Type = 'WR' AND Table_Name = 'Company' AND Primary_Status = '" + WR_PRIMARY_DISPOSAL.SelectedValue + "'");
                        if (drRecordStatusWR.Length > 0)
                        {
                            WR_SECONDARY_DISPOSAL.DataSource = drRecordStatusWR.CopyToDataTable();
                            WR_SECONDARY_DISPOSAL.DisplayMember = "Secondary_Status";
                            WR_SECONDARY_DISPOSAL.ValueMember = "Secondary_Status";
                            //lstWRPrimaryDisposal.Focus();
                        }

                        RefreshCompanyGrid(iCompanyRowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void PopulateMasterCompanyFields()//Populate MasterFields
        {
            try
            {
                if (lstCompanyControls != null && dtMasterCompanies.Rows.Count != 0)
                {
                    txtDialMain.Text = TelephoneFormat(dtMasterCompanies.Rows[iCompanyRowIndex]["SWITCHBOARD"].ToString(), "Master", iCompanyRowIndex);

                    if (GV.sDialerType == "Vortex")
                        txtDialExtension.Text = GV.sVortexExtension;
                    else
                        txtDialExtension.Text = GV.sExtensionNumber;

                    Impact_CompanyTable(iCompanyRowIndex);

                    //-----------------------------------------------------------------------------------------------------
                    #region Old
                    //-----------------------------------------------------------------------------------------------------
                    //IsLoading = true;
                    //foreach (TextBox txt in lstCompanyControls)
                    //{
                    //    if (dtMasterCompanies.Rows[iCompanyRowIndex][txt.Name].ToString().Length > 0)
                    //    {
                    //        txt.Text = dtMasterCompanies.Rows[iCompanyRowIndex][txt.Name].ToString();
                    //        DataRow[] drFormatting = dtFieldMaster.Select(String.Format("FIELD_NAME_TABLE = '{0}'", txt.Name));//Gets the field Name to which the textbox belongs to
                    //        if (drFormatting.Length > 0 && drFormatting[0]["FORMATTING"].ToString().Length > 0)
                    //        {
                    //            List<string> lstFormatting = drFormatting[0]["FORMATTING"].ToString().Split('|').ToList();//Perform validations based on the field of textBox
                    //            TextFormatting(txt, lstFormatting);
                    //        }
                    //    }

                    //    txt.SelectionStart = 0;   //Text goes inside textbox(some text gets hidden)
                    //    #region ChecklistBox
                    //    //else if (C is CheckedListBox)
                    //    //{
                    //    //    CheckedListBox chk = C as CheckedListBox;
                    //    //    if (dtMasterCompanies.Rows[0][C.Name] != null)
                    //    //    {
                    //    //        List<string> lstCheckedItems = dtMasterCompanies.Rows[0][C.Name].ToString().Split('|').ToList();
                    //    //        List<string> lstAllItems = new List<string>();
                    //    //        for (int i = 0; i <= chk.Items.Count - 1; i++)
                    //    //            lstAllItems.Add(chk.Items[i].ToString());
                    //    //        chk.Items.Clear();

                    //    //        foreach (string s in lstCheckedItems)
                    //    //        {
                    //    //            if(s.Trim().Length > 0)
                    //    //                chk.Items.Add(s, true);
                    //    //        }
                    //    //        foreach(string s in lstAllItems)
                    //    //        {
                    //    //            if (!lstCheckedItems.Contains(s) && s.Trim().Length > 0)
                    //    //                chk.Items.Add(s,false);
                    //    //        }
                    //    //    }
                    //    //}
                    //    #endregion
                    //}
                    //IsLoading = false;
                    //-----------------------------------------------------------------------------------------------------
                    #endregion
                    //-----------------------------------------------------------------------------------------------------
                    TimeZoneCalc(string.Empty);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void TimeZoneCalc(string sCountryName)
        {
            string sCountry = string.Empty;
            if (sCountryName.Length > 0)
                sCountry = sCountryName;
            else
                sCountry = dtMasterCompanies.Rows[iCompanyRowIndex]["COUNTRY"].ToString();
            //string sCountry = dtMasterCompanies.Rows[iCompanyRowIndex]["COUNTRY"].ToString();
            if (GV.sAccessTo == "TR" && sCountry.Length > 0)//Display Time zone of displayed company
            {
                txtTimeZone.Visible = true;
                lblTime.Visible = true;
                DataRow[] drSelectedTimeZone = null;
                if (sCountry.ToUpper() == "UNITED STATES" && dtMasterCompanies.Rows[iCompanyRowIndex]["COUNTY"].ToString().Length > 0)
                {
                    drSelectedTimeZone = dtCountryInformation.Select(String.Format("CountryName = 'UNITED STATES' AND State = '{0}'", dtMasterCompanies.Rows[iCompanyRowIndex]["COUNTY"].ToString().Replace("'", "''")));
                    if (drSelectedTimeZone.Length == 0)//If county not found then use genral country timezone
                        drSelectedTimeZone = dtCountryInformation.Select("CountryName = 'United States' AND State IS NULL");
                }
                else
                    drSelectedTimeZone = dtCountryInformation.Select(String.Format("CountryName = '{0}'", sCountry.Replace("'", "''")));

                if (drSelectedTimeZone.Length > 0)
                {
                    if (drSelectedTimeZone[0]["HoursFromGMT"].ToString().Length > 0)
                    {
                        iGMTHours = Convert.ToDouble(drSelectedTimeZone[0]["HoursFromGMT"].ToString());
                        tCurrentCountryTime = GM.GetDateTime().AddHours(iGMTHours);
                        //"Time in " + dtMasterCompanies.Rows[0]["COUNTRY"].ToString();
                        txtTimeZone.Text = tCurrentCountryTime.ToString("HH:mm");
                        //txtTimeZone.Tag = "Time: " + tCurrentCountryTime.ToString("HH:mm");
                        if (t != null)
                            t.Dispose();
                        t = new System.Windows.Forms.Timer();
                        if (Convert.ToDouble(tCurrentCountryTime.ToString("HH")) < 9 || Convert.ToDouble(tCurrentCountryTime.ToString("HH")) > 16)
                        {
                            t.Interval = 500;
                            t.Start();
                            t.Tick += new EventHandler(TimerLabelTick);
                            txtTimeZone.TextBox.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
                            txtTimeZone.TextBox.ForeColor = Color.White;
                            IsOutOfTimeZone = true;
                        }
                        else
                        {
                            txtTimeZone.TextBox.ForeColor = Control.DefaultForeColor;
                            txtTimeZone.TextBox.BackColor = Control.DefaultBackColor;
                            txtTimeZone.Text = tCurrentCountryTime.ToString("HH:mm");
                            IsOutOfTimeZone = false;
                        }
                    }
                    else
                    {
                        MessageBoxEx.Show("GMT Hours not found on TimeZone Table!!!");
                        //txtTimeZone.Text = string.Empty;
                    }
                }
                else if (dtMasterCompanies.Rows[iCompanyRowIndex]["NEW_OR_EXISTING"].ToString() != "New")
                {
                    MessageBoxEx.Show("Country doesn't match Timezone table!!!");
                    //txtTimeZone.Text = string.Empty;
                }
            }
            else
            {
                if (GV.sAccessTo == "TR")
                    ToastNotification.Show(this, "Country not found!!!", eToastPosition.TopRight);
                txtTimeZone.Visible = false;
                lblTime.Visible = false;
            }
            txtCompanyID.Text = sMaster_ID; //Display Company Id
        }

        //-----------------------------------------------------------------------------------------------------
        private void TimerLabelTick(object sender, EventArgs e)//Runtime event assigned to tErrorTextAutoClose object
        {
            if (txtTimeZone.TextBox.BackColor == Color.FromArgb(0xFF, 0x99, 0x99))
            {
                txtTimeZone.TextBox.ForeColor = Color.White;
                if (txtTimeZone.Text.Length > 0)
                    txtTimeZone.Text = string.Empty;
                else
                    txtTimeZone.Text = tCurrentCountryTime.ToString("HH:mm");
            }

        }

        //-----------------------------------------------------------------------------------------------------
        private void txt_Change(object sender, EventArgs e)//Run time Event
        {
            try
            {
                if (!IsLoading)
                {
                    IsLoading = true;
                    TextBox txtBox = sender as TextBox; //Gets the object of textbox that teriggers this event(This event is assigned to multiple textboxes)
                    DataRow[] drFormatting = dtFieldMaster_Active.Select(String.Format("FIELD_NAME_TABLE = '{0}'", txtBox.Name));//Gets the field Name to which the textbox belongs to                    
                    if (drFormatting.Length > 0 && drFormatting[0]["FORMATTING"].ToString().Length > 0)
                    {
                        List<string> lstFormatting = drFormatting[0]["FORMATTING"].ToString().Split('|').ToList();//Perform validations based on the field of textBox
                        Format_TextBox(txtBox, lstFormatting);
                    }

                    PreUpdate(txtBox);

                    if (txtBox.Tag.ToString() == "Contact")
                    {
                        //if (!IsTableToTextContact)
                        TableToTextContact(iContactRowIndex, "ControlsToGrid", true);
                    }
                    else
                    {
                        //if (!IsTableToTextCompany)
                        TableToTextCompany(iCompanyRowIndex, "ControlsToGrid", true);
                    }
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void Format_TextBox(TextBox txtBox, List<string> lstTextFormatting)
        {

            foreach (string sValidation in lstTextFormatting)
            {
                int i = txtBox.SelectionStart;
                switch (sValidation)
                {
                    case "PROPER_CASE":
                        if (IsShiftPressed || Control.IsKeyLocked(Keys.CapsLock))
                        { }
                        else
                        {
                            //txt.Text = ProperCaseHelper.ToProperCase(txt.Text, "Name");
                            txtBox.Text = GM.ProperCaseLeaveCapital(txtBox.Text);//Converts to propercases
                            //txt.SelectionStart = txt.Text.Length + 1;
                            txtBox.SelectionStart = i;
                        }
                        break;
                    case "NAME_CASING":
                        if (IsShiftPressed || Control.IsKeyLocked(Keys.CapsLock))
                        { }
                        else
                        {
                            txtBox.Text = ProperCaseHelper.NameANDJobTitleCasing(txtBox.Text, "Name");
                            //txt.Text = GlobalMethods.ProperCaseCustom(txt.Text);//Converts to propercases
                            //txt.SelectionStart = txt.Text.Length + 1;
                            txtBox.SelectionStart = i;
                        }
                        break;
                    case "JOBTITLE_CASING":
                        if (IsShiftPressed || Control.IsKeyLocked(Keys.CapsLock))
                        { }
                        else
                        {
                            txtBox.Text = ProperCaseHelper.NameANDJobTitleCasing(txtBox.Text, "JobTitle");
                            //txt.Text = GlobalMethods.ProperCaseCustom(txt.Text);//Converts to propercases
                            //txt.SelectionStart = txt.Text.Length + 1;
                            txtBox.SelectionStart = i;
                        }
                        break;
                    case "UPPER":
                        txtBox.Text = txtBox.Text.ToUpper();//converts to uppercase
                        //txt.SelectionStart = txt.Text.Length + 1;
                        txtBox.SelectionStart = i;
                        break;
                    case "LOWER":
                        txtBox.Text = txtBox.Text.ToLower();//converts to lower case
                        //txt.SelectionStart = txt.Text.Length + 1;
                        txtBox.SelectionStart = i;
                        break;

                    case "ONLYALPHA":// allow alphabts and spl chars
                        Regex rAlpha = new Regex(@"[0-9]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                        txtBox.Text = rAlpha.Replace(txtBox.Text, string.Empty);
                        txtBox.SelectionStart = i;
                        break;

                    case "ONLYNUMERIC":
                        //txtBox.Text = txtBox.Text.ToUpper();//Allow only numeric and spl chars
                        string[] sNum = txtBox.Text.Trim().Split('.');
                        string sOut = string.Empty;
                        Regex rNumeric = new Regex(@"[a-zA-Z\W_]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                        if (sNum.Length == 1)
                            sOut = rNumeric.Replace(sNum[0], string.Empty);
                        else if (sNum.Length == 2)
                            sOut = rNumeric.Replace(sNum[0], string.Empty) + "." + rNumeric.Replace(sNum[1], string.Empty);
                        else//unusual
                        {
                            for (int j = 0; j < sNum.Length; j++)
                            {
                                if (j > 0)
                                    sOut += rNumeric.Replace(sNum[j], string.Empty);
                                else
                                    sOut = rNumeric.Replace(sNum[0], string.Empty) + ".";
                            }
                        }

                        txtBox.Text = sOut;
                        txtBox.SelectionStart = i;

                        break;

                    //case "ONLYNUMERIC":
                    //    txtBox.Text = txtBox.Text.ToUpper();//Allow only numeric and spl chars
                    //    Regex rNumeric = new Regex(@"[\D]+\.*[\D]*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                    //    txtBox.Text = rNumeric.Replace(txtBox.Text, string.Empty);
                    //    txtBox.SelectionStart = i;
                    //    break;

                    case "EMAIL":
                        if (GM.Email_Check(txtBox.Text.Trim()))//email syntax validation
                            txtBox.BackColor = Color.White;
                        else
                            txtBox.BackColor = Color.FromArgb(0xFF, 0xFF, 0x99);

                        txtBox.Text = txtBox.Text.Replace(" ", string.Empty).Replace("\n", "").Replace("..",".");
                        txtBox.SelectionStart = i;
                        break;

                    case "WEBSITE":
                        //txt.TextChanged -= new EventHandler(txt_Change);
                        //IsLoading = true;
                        //txt.Text = WebFormating(txt.Text);

                        if (txtBox.Text.Length > 2)
                        {
                            txtBox.Text = txtBox.Text.ToLower().Trim();
                            txtBox.SelectionStart = txtBox.Text.Length + 1;
                            if (txtBox.Text.ToLower().Contains("https://"))
                            {
                                txtBox.Text = txtBox.Text.ToLower().Replace("https://", string.Empty);
                                txtBox.SelectionStart = txtBox.Text.Length + 1;
                            }
                            else if (txtBox.Text.ToLower().Contains("http://"))
                            {
                                txtBox.Text = txtBox.Text.ToLower().Replace("http://", string.Empty);
                                txtBox.SelectionStart = txtBox.Text.Length + 1;
                            }

                            if (!txtBox.Text.ToLower().StartsWith("www"))
                            {
                                txtBox.Text = "www." + txtBox.Text.ToLower();
                                txtBox.SelectionStart = txtBox.Text.Length + 1;
                            }
                        }
                        //IsLoading = false;
                        //txt.TextChanged += new EventHandler(txt_Change);

                        if (GM.Web_Check(txtBox.Text.Trim()))//Website syntax validation
                            txtBox.BackColor = Color.White;
                        else
                            txtBox.BackColor = Color.FromArgb(0xFF, 0xFF, 0xCC);
                        break;


                    case "REMOVE_SPECIAL_CHARACTERS"://removes spl chars
                        txtBox.Text = Remove_SplChars(txtBox.Text);
                        //txt.SelectionStart = txt.Text.Length + 1;
                        txtBox.SelectionStart = i;
                        break;

                    //case "MANDATORY":
                    //    TextBox_Indication(txt); // Set color indication for textbox if mandatory
                    //    break;

                    case "TELEPHONEMASTER":
                        //txt.TextChanged -= new EventHandler(txt_Change);
                        txtBox.Text = TelephoneFormat(txtBox.Text, "Master", -1);
                        //txt.TextChanged += new EventHandler(txt_Change);
                        txtBox.SelectionStart = txtBox.Text.Length + 1;
                        //txt.SelectionStart = i;
                        if (txtBox.Name.ToUpper() == "SWITCHBOARD")
                        {
                            txtDialMain.Text = txtBox.Text;
                            if (objfrmCallScript != null)
                                objfrmCallScript.txtDialCallScript.Text = txtBox.Text;
                        }
                        break;

                    case "TELEPHONECONTACTS":
                        //txt.TextChanged -= new EventHandler(txt_Change);
                        txtBox.Text = TelephoneFormat(txtBox.Text, "Contacts", -1);
                        //txt.TextChanged += new EventHandler(txt_Change);
                        txtBox.SelectionStart = txtBox.Text.Length + 1;
                        //txt.SelectionStart = i;
                        break;

                    case "COUNTRYMASTER"://Empty Telephone if Country Changed

                        DataRow[] drrFieldMasterCompany = dtFieldMasterCompany.Select("FORMATTING LIKE '%TELEPHONEMASTER%'");
                        if (drrFieldMasterCompany.Length > 0)
                        {
                            foreach (DataRow drFieldMasterCompany in drrFieldMasterCompany)
                            {
                                foreach (TextBox C in lstCompanyControls)
                                {
                                    if (C.Name.ToUpper() == drFieldMasterCompany["FIELD_NAME_TABLE"].ToString().ToUpper() && C.Text.Trim().Length > 0 && C.Text.Contains(" "))
                                    {
                                        //C.Text = TelephoneFormat(C.Text.Substring(C.Text.IndexOf(" ")).Trim(), "Master");
                                        C.Text = string.Empty; //Empty the telephone number when country changed
                                        break;
                                    }
                                }
                            }
                        }

                        TimeZoneCalc(txtBox.Text);
                        break;

                    case "COUNTRYCONTACT"://Empty Telephone if Country Changed
                        if (txtBox.Text.Trim().Length > 0)
                        {
                            DataRow[] drrFieldMasterContact = dtFieldMasterContact.Select("FORMATTING LIKE '%TELEPHONECONTACTS%'");
                            if (drrFieldMasterContact.Length > 0)
                            {
                                foreach (DataRow drFieldMasterContact in drrFieldMasterContact)
                                {
                                    foreach (TextBox C in lstContactControls)
                                    {
                                        if (C.Name.ToUpper() == drFieldMasterContact["FIELD_NAME_TABLE"].ToString().ToUpper() && C.Text.Trim().Length > 0 && C.Text.Contains(" "))
                                        {
                                            //C.Text = TelephoneFormat(C.Text.Substring(C.Text.IndexOf(" ")).Trim(), "Contacts");
                                            C.Text = string.Empty;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        string TextFormatting(string sTextToFormat, List<string> lstTextFormatting, int iIndex)
        {
            foreach (string sValidation in lstTextFormatting)
            {
                switch (sValidation)
                {
                    #region Warning-------This formattings can be overridden if Shift or Control Key is pressed. Example: if a agent changes a First name to 'DAVINCI'(Full caps), then this will turn into prper case auomatically.

                    //case "PROPER_CASE":
                    //    if (IsShiftPressed || Control.IsKeyLocked(Keys.CapsLock))
                    //    { }
                    //    else                        
                    //        sTextToFormat = GM.ProperCaseLeaveCapital(sTextToFormat);//Converts to propercases
                    //    break;
                    //case "NAME_CASING":
                    //    if (IsShiftPressed || Control.IsKeyLocked(Keys.CapsLock))
                    //    { }
                    //    else                        
                    //        sTextToFormat = ProperCaseHelper.NameANDJobTitleCasing(sTextToFormat, "Name");                                                    
                    //    break;
                    //case "JOBTITLE_CASING":
                    //    if (IsShiftPressed || Control.IsKeyLocked(Keys.CapsLock))
                    //    { }
                    //    else                        
                    //        sTextToFormat = ProperCaseHelper.NameANDJobTitleCasing(sTextToFormat, "JobTitle");                                                    
                    //    break; 
                    #endregion

                    case "UPPER":
                        sTextToFormat = sTextToFormat.ToUpper();//converts to uppercase                        
                        break;
                    case "LOWER":
                        sTextToFormat = sTextToFormat.ToLower();//converts to lower case                        
                        break;

                    case "ONLYALPHA":// allow alphabts and spl chars
                        Regex rAlpha = new Regex(@"[0-9]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                        sTextToFormat = rAlpha.Replace(sTextToFormat, string.Empty);
                        break;

                    case "ONLYNUMERIC":
                        //txtBox.Text = txtBox.Text.ToUpper();//Allow only numeric and spl chars
                        string[] sNum = sTextToFormat.Trim().Split('.');
                        string sOut = string.Empty;
                        Regex rNumeric = new Regex(@"[a-zA-Z\W_]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                        if (sNum.Length == 1)
                            sOut = rNumeric.Replace(sNum[0], string.Empty);
                        else if (sNum.Length == 2)
                            sOut = rNumeric.Replace(sNum[0], string.Empty) + "." + rNumeric.Replace(sNum[1], string.Empty);
                        else//unusual
                        {
                            for (int j = 0; j < sNum.Length; j++)
                            {
                                if (j > 0)
                                    sOut += rNumeric.Replace(sNum[j], string.Empty);
                                else
                                    sOut = rNumeric.Replace(sNum[0], string.Empty) + ".";
                            }
                        }

                        sTextToFormat = sOut;
                        break;

                    case "REMOVE_SPECIAL_CHARACTERS"://removes spl chars
                        sTextToFormat = Remove_SplChars(sTextToFormat);
                        break;

                    case "WEBSITE":
                        if (sTextToFormat.Length > 2)
                        {
                            sTextToFormat = sTextToFormat.ToLower().Trim();
                            if (sTextToFormat.Contains("https://"))
                                sTextToFormat = sTextToFormat.Replace("https://", string.Empty);
                            else if (sTextToFormat.Contains("http://"))
                                sTextToFormat = sTextToFormat.Replace("http://", string.Empty);

                            if (!sTextToFormat.StartsWith("www"))
                                sTextToFormat = "www." + sTextToFormat;
                        }
                        break;

                    case "TELEPHONEMASTER":
                        sTextToFormat = TelephoneFormat(sTextToFormat, "Master", iIndex);
                        break;

                    case "TELEPHONECONTACTS":
                        sTextToFormat = TelephoneFormat(sTextToFormat, "Contacts", iIndex);
                        break;
                }
            }
            return sTextToFormat;
        }

        void PreUpdate_Populate(string sValidationValue)
        {
            string sTarget = sValidationValue.Split('=')[0];
            string sSource = sValidationValue.Split('=')[1];

            if (sSource.StartsWith("C.") || sSource.StartsWith("M."))//Update control's text with another control's text
            {
                switch ((sSource[0].ToString() + sTarget[0].ToString()))
                {
                    case "CC":
                        CopyControlValues("C", sSource.Replace("C.", string.Empty), "C", sTarget.Replace("C.", string.Empty));
                        break;

                    case "MC":
                        CopyControlValues("M", sSource.Replace("M.", string.Empty), "C", sTarget.Replace("C.", string.Empty));
                        break;

                    case "CM":
                        CopyControlValues("C", sSource.Replace("C.", string.Empty), "M", sTarget.Replace("M.", string.Empty));
                        break;
                }
            }
            else//Update control's text with user's custom text
            {
                if (sTarget.StartsWith("C."))
                {
                    string sControlName = sTarget.Replace("C.", string.Empty).ToUpper().Trim();
                    GetSpecificControl("C", sControlName).Text = sSource;
                }
                else if (sTarget.StartsWith("M."))
                {
                    string sControlName = sTarget.Replace("M.", string.Empty).ToUpper().Trim();
                    GetSpecificControl("M", sControlName).Text = sSource;
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void PreUpdate(TextBox txt)
        {
            //-----------------------------------------------------------------------------------------------------
            #region Preupdate
            //-----------------------------------------------------------------------------------------------------
            //////////////////////////////////////////////////
            ///////////////////PreUpdate//////////////////////
            //////////////////////////////////////////////////

            if (dtPreUpdate != null && dtPreUpdate.Rows.Count > 0)//Run time data change
            {



                string sValidation_For = string.Empty;
                string sCondition = string.Empty;

                if (txt.Tag.ToString() == "Contact")
                    sValidation_For = "C." + txt.Name;
                else
                    sValidation_For = "M." + txt.Name;

                //-----------------------------------------------------------------------------------------------------
                #region Preupdate Text
                //-----------------------------------------------------------------------------------------------------
                DataRow[] drrPreUpdatePosibbleValidations = dtPreUpdate.Select("VALIDATION_TYPE = 'POPULATE' AND VALIDATION_FOR = '" + sValidation_For + "'");
                DataRow[] drrPreUpdate = null;

                foreach (DataRow drPreUpdatePosibbleValidations in drrPreUpdatePosibbleValidations)
                {
                    if (drPreUpdatePosibbleValidations["CONDITION"].ToString().StartsWith("C."))
                    {
                        drrPreUpdate = dtPreUpdate.Select("VALIDATION_TYPE = 'POPULATE' AND VALIDATION_FOR = '" + sValidation_For + "' AND CONDITION = '" + dtMasterContacts.Rows[iContactRowIndex][drPreUpdatePosibbleValidations["CONDITION"].ToString().Replace("C.", "")].ToString().Trim().Replace("'", "''") + "'");
                        if (txt.Text.Trim().ToUpper() == dtMasterContacts.Rows[iContactRowIndex][drPreUpdatePosibbleValidations["CONDITION"].ToString().Replace("C.", "")].ToString().Trim().ToUpper())
                        {
                            PreUpdate_Populate(drPreUpdatePosibbleValidations["VALIDATION_VALUE"].ToString());
                            break;
                        }
                    }
                    else if (drPreUpdatePosibbleValidations["CONDITION"].ToString().StartsWith("M."))
                    {
                        if (txt.Text.Trim().ToUpper() == dtMasterCompanies.Rows[iCompanyRowIndex][drPreUpdatePosibbleValidations["CONDITION"].ToString().Replace("M.", "")].ToString().Trim().ToUpper())
                        {
                            PreUpdate_Populate(drPreUpdatePosibbleValidations["VALIDATION_VALUE"].ToString());
                            break;
                        }
                    }
                    else
                    {
                        drrPreUpdate = dtPreUpdate.Select("VALIDATION_TYPE = 'POPULATE' AND VALIDATION_FOR = '" + sValidation_For + "' AND CONDITION = '" + txt.Text.Trim().Replace("'", "''") + "'");
                        break;
                    }
                }

                //if (dtPreUpdate.Select("VALIDATION_TYPE = 'POPULATE' AND VALIDATION_FOR = '" + sValidation_For + "' AND CONDITION = '" + txt.Text.Trim().Replace("'", "''") + "'").Length > 0)
                //    drrPreUpdate = dtPreUpdate.Select("VALIDATION_TYPE = 'POPULATE' AND VALIDATION_FOR = '" + sValidation_For + "' AND CONDITION = '" + txt.Text.Trim().Replace("'", "''") + "'");                


                if (drrPreUpdate != null && drrPreUpdate.Length > 0)//Text update
                {
                    foreach (DataRow drPreUpdate in drrPreUpdate)
                        PreUpdate_Populate(drPreUpdate["VALIDATION_VALUE"].ToString());
                }
                //-----------------------------------------------------------------------------------------------------
                #endregion
                //-----------------------------------------------------------------------------------------------------


                //-----------------------------------------------------------------------------------------------------
                #region If child of a tree is altered then clear parant
                //-----------------------------------------------------------------------------------------------------
                if (dtPreUpdate.Select("VALIDATION_TYPE = 'GETLIST'").Length > 0)//If child of a tree is altered then clear parant.
                {
                    DataRow[] drrPreUpdate_Getlist_ClearParant = dtPreUpdate.Select("VALIDATION_TYPE = 'GETLIST'");//To Check if value changed on Child field.. Clear the parant field if change occures
                    foreach (DataRow drPreUpdate_Getlist_ClearParant in drrPreUpdate_Getlist_ClearParant)
                    {
                        string sParantField = drPreUpdate_Getlist_ClearParant["VALIDATION_FOR"].ToString().ToUpper();
                        string sChildField = drPreUpdate_Getlist_ClearParant["VALIDATION_VALUE"].ToString().ToUpper();
                        string sParantTableName = string.Empty;
                        string sChildTableName = string.Empty;

                        if (sParantField.StartsWith("M."))
                        {
                            sParantTableName = "M";
                            sParantField = sParantField.Replace("M.", "");
                        }
                        else
                        {
                            sParantTableName = "C";
                            sParantField = sParantField.Replace("C.", "");
                        }

                        sChildField = sChildField.Split('=')[1];
                        sChildTableName = sChildField.Split('.')[0];

                        sChildField = sChildField.Split('.')[1];

                        if (txt.Name.ToUpper() == sChildField)
                        {
                            TextBox CControlChild = GetSpecificControl(sChildTableName, sChildField);
                            TextBox CControlParant = GetSpecificControl(sParantTableName, sParantField);
                            string sCategory = CControlChild.Text;

                            if (CControlParant != null)
                            {
                                if (sCategory.Trim().Length == 0)
                                    CControlParant.Text = string.Empty;
                                else if (dtPicklist.Select("PicklistField = '" + CControlChild.Text.Replace("'", "''") + "' AND PicklistValue = '" + CControlParant.Text.Replace("'", "''") + "'").Length == 0)
                                    CControlParant.Text = string.Empty;
                            }
                        }
                    }
                }
                //-----------------------------------------------------------------------------------------------------
                #endregion
                //-----------------------------------------------------------------------------------------------------


                //-----------------------------------------------------------------------------------------------------
                #region Clear controls based on conditions
                //-----------------------------------------------------------------------------------------------------
                //Clear controls based on conditions
                if (dtPreUpdate.Select("VALIDATION_TYPE = 'CLEAR' AND VALIDATION_FOR = '" + sValidation_For + "'").Length > 0)
                {
                    string sFieldsToClear = dtPreUpdate.Select("VALIDATION_TYPE = 'CLEAR' AND VALIDATION_FOR = '" + sValidation_For + "'")[0]["VALIDATION_VALUE"].ToString();

                    if (sFieldsToClear.Contains("|"))
                    {
                        List<string> lstFieldsToClear = sFieldsToClear.Split('|').ToList();
                        foreach (string sFields in lstFieldsToClear)
                            GetSpecificControl(sFields[0].ToString().ToUpper(), sFields.Split('.')[1].ToUpper().Trim()).Text = string.Empty;
                    }
                    else
                        GetSpecificControl(sFieldsToClear[0].ToString().ToUpper(), sFieldsToClear.Split('.')[1].ToUpper().Trim()).Text = string.Empty;
                }
                //-----------------------------------------------------------------------------------------------------
                #endregion
                //-----------------------------------------------------------------------------------------------------

            }
            //-----------------------------------------------------------------------------------------------------
            #endregion
            //-----------------------------------------------------------------------------------------------------
        }


        //-----------------------------------------------------------------------------------------------------
        //private void DynamicMandatoryNotification(DataRow drContact, int iRowNumber)
        //{
        //    //StringBuilder sValidation = new StringBuilder();
        //    try
        //    {
        //        string sContactStatus = drContact[GV.sAccessTo + "_CONTACT_STATUS"].ToString();

        //        if (sContactStatus.Length > 0)
        //        {
        //            DataRow[] drrValidation = dtValidations.Select("(TABLE_NAME = '" + GV.sContactTable + "' AND RESEARCH_TYPE = '" + GV.sAccessTo + "' AND Operation_Type ='Validation' AND Condition = '" + sContactStatus + "') OR VALIDATION_TYPE IN (" + GM.ListToQueryString(lstValidationTypeToIgnore, "String") + ")");
        //            DataRow[] drrValidation1 = dtValidations.Select("TABLE_NAME = '" + GV.sContactTable + "' AND RESEARCH_TYPE = '" + GV.sAccessTo + "' AND Operation_Type ='Validation' AND VALIDATION_FOR <> '" + GV.sAccessTo + "_CONTACT_STATUS'");
        //            DataTable dtValidateRuntime = new DataTable();

        //            if (drrValidation.Length > 0)
        //                dtValidateRuntime = drrValidation.CopyToDataTable();

        //            if (drrValidation1.Length > 0)
        //            {
        //                if (dtValidateRuntime.Rows.Count == 0)
        //                    dtValidateRuntime = drrValidation1.CopyToDataTable();
        //                else
        //                    dtValidateRuntime.Merge(drrValidation1.CopyToDataTable(), false, MissingSchemaAction.Ignore);
        //            }

        //            if (dtValidateRuntime.Rows.Count == 0)
        //                return;

        //            int iCompanyID = Convert.ToInt32(drContact["MASTER_ID"]);
        //            int iContactID = 0;
        //            string sTableName = "Contact";

        //            if (drContact["CONTACT_ID_P"].ToString().Length > 0)
        //                iContactID = Convert.ToInt32(drContact["CONTACT_ID_P"]);

        //            foreach (DataRow drValidation in dtValidateRuntime.Rows)
        //            {
        //                string sVALIDATION_TYPE = drValidation["VALIDATION_TYPE"].ToString();//Type of validation(Mandatory, Email check, Jobtitle check)
        //                string sFieldName = drValidation["VALIDATION_VALUE"].ToString();//Field to be validated(Title, FirstName, Telephone)

        //                if (drContact[drValidation["VALIDATION_FOR"].ToString()].ToString().ToUpper() == drValidation["CONDITION"].ToString().ToUpper() || (lstValidationTypeToIgnore.Contains(drValidation["VALIDATION_TYPE"].ToString(), StringComparer.OrdinalIgnoreCase) && ((GV.lstTRContactStatusToBeValidated.Contains(sContactStatus, StringComparer.OrdinalIgnoreCase) && GV.sAccessTo == "TR") || GV.lstWRContactStatusToBeValidated.Contains(sContactStatus, StringComparer.OrdinalIgnoreCase) && GV.sAccessTo == "WR")))
        //                {
        //                    TextBox txt = new TextBox();

        //                    foreach (TextBox C in lstContactControls)
        //                    {
        //                        if (C.Name.ToUpper() == sFieldName.ToUpper())
        //                        {
        //                            txt = C;
        //                            break;
        //                        }
        //                    }

        //                    if (drValidation["VALIDATION_FOR"].ToString().ToUpper() == GV.sAccessTo + "_CONTACT_STATUS")
        //                    {
        //                        foreach (TextBox C in lstContactControls)
        //                        {
        //                            if (C.Name.ToUpper() == drValidation["VALIDATION_FOR"].ToString().ToUpper())
        //                            {
        //                                C.BackColor = Color.White;
        //                                break;
        //                            }
        //                        }
        //                    }

        //                    string sValueToBeVerified = string.Empty;//Values to be validated(Thanga Prakash, prakash.cyberdyne@gmail.com, 9790783282)

        //                    switch (sVALIDATION_TYPE)
        //                    {
        //                        case "EMAILCOMPANYCHECK":
        //                            sValueToBeVerified = drContact[sFieldName].ToString();

        //                            //Check Email format Company Email
        //                            if (sValueToBeVerified.Trim().Length > 0)
        //                            {
        //                                if (GM.Email_Check(sValueToBeVerified))
        //                                {
        //                                    string sEmailContentCheck = CONTACT_EmailContentCheck(sValueToBeVerified, "COMPANYEMAIL").Trim();
        //                                    if (sEmailContentCheck.Length > 0)
        //                                    {
        //                                        AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "COMPANYEMAIL", sFieldName, sValueToBeVerified, true, -1);
        //                                        txt.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
        //                                        txt.Refresh();
        //                                    }
        //                                    else
        //                                    {
        //                                        txt.BackColor = Color.White;
        //                                        txt.Refresh();
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "INVALID", sFieldName, sValueToBeVerified, true, -1);
        //                                    txt.BackColor = Color.FromArgb(0xFF, 0xFF, 0xCC);
        //                                    txt.Refresh();
        //                                }
        //                            }
        //                            //else
        //                            //    sValidation += "<font color = 'DarkCyan'>" + GlobalMethods.ProperCase(sFieldName) + "</font> cannot be empty on condition <font color = 'Tomato'>" + GlobalMethods.ProperCase(drValidation["CONDITION"].ToString()) +"</font>"+ Environment.NewLine;

        //                            break;

        //                        case "EMAILPUBLICDOMAINCHECK":
        //                            sValueToBeVerified = drContact[sFieldName].ToString();

        //                            //Check Email format Public Domin Emails
        //                            if (sValueToBeVerified.Trim().Length > 0)
        //                            {
        //                                if (GM.Email_Check(sValueToBeVerified))
        //                                {
        //                                    string sEmailContentCheck = CONTACT_EmailContentCheck(sValueToBeVerified, "PUBLICDOMAIN").Trim();
        //                                    if (sEmailContentCheck.Length > 0)
        //                                    {
        //                                        //sValidation.AppendLine(sEmailContentCheck);
        //                                        AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "PUBLICDOMAIN", sFieldName, sValueToBeVerified, true, -1);
        //                                        txt.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
        //                                        txt.Refresh();
        //                                    }
        //                                    else
        //                                        txt.BackColor = Color.White;
        //                                }
        //                                else
        //                                {
        //                                    //sValidation.AppendLine("Invalid <font color = 'DarkCyan'>Email</font> Format: <font color = 'Tomato'>" + GM.ProperCase(sValueToBeVerified) + "</font>");
        //                                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "INVALID", sFieldName, sValueToBeVerified, true, -1);
        //                                    txt.BackColor = Color.FromArgb(0xFF, 0xFF, 0x99);
        //                                    txt.Refresh();
        //                                }
        //                            }
        //                            //else
        //                            //    sValidation += "<font color = 'DarkCyan'>" + GlobalMethods.ProperCase(sFieldName) + "</font> cannot be empty on condition <font color = 'Tomato'>" + GlobalMethods.ProperCase(drValidation["CONDITION"].ToString()) + "</font>" + Environment.NewLine;

        //                            break;

        //                        case "EMAILDUPECHECK":
        //                            sValueToBeVerified = drContact[sFieldName].ToString();
        //                            //Check Duplicate Email
        //                            if (GV.sAllowDuplicateEmail == "N" && sValueToBeVerified.Trim().Length > 0)
        //                            {
        //                                //Replace due to single quote(') in string
        //                                if (dtMasterContacts.Select(sFieldName + " = '" + sValueToBeVerified.Replace("'", "''") + "' AND (TR_CONTACT_STATUS IN (" + GV.sEmailCheckContactStatus + ") OR WR_CONTACT_STATUS IN (" + GV.sEmailCheckContactStatus + "))").Length > 1)// && (!(sValidation.Contains("Duplicate Email Found"))))
        //                                {
        //                                    string sIDs = string.Empty;
        //                                    for (int i = 0; i < dtMasterContacts.Rows.Count; i++)//Show row number of contact
        //                                    {
        //                                        if (
        //                                        dtMasterContacts.Rows[i]["MASTER_ID"].ToString() == iCompanyID.ToString()
        //                                        &&
        //                                        dtMasterContacts.Rows[i][sFieldName].ToString().Trim().ToLower() == sValueToBeVerified.Trim().ToLower()
        //                                        &&
        //                                        (
        //                                        (GV.lstEmailChackContactStatus.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
        //                                        ||
        //                                        (GV.lstEmailChackContactStatus.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
        //                                        )
        //                                        )
        //                                        {
        //                                            if (sIDs.Length > 0)
        //                                                sIDs += ", " + (i + 1).ToString();
        //                                            else
        //                                                sIDs = (i + 1).ToString();
        //                                        }
        //                                    }
        //                                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "EMAILDUPE", sFieldName, sValueToBeVerified, "", sIDs, true, -1);
        //                                    txt.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
        //                                    txt.Refresh();
        //                                }
        //                                else
        //                                {
        //                                    txt.BackColor = Color.White;
        //                                    txt.Refresh();
        //                                }
        //                            }
        //                            break;

        //                        case "JOBTITLESPELLCHECK":
        //                            sValueToBeVerified = drContact[sFieldName].ToString();

        //                            //Spell Check Job Title
        //                            if (GV.sSpellCheckJobTitle == "Y")
        //                            {
        //                                if (dtPicklist.Select("PicklistCategory='Jobtitle' AND PicklistValue = '" + sValueToBeVerified.Replace("'", "''") + "'").Length == 0)//If jobtitle exist in list then do not check
        //                                {
        //                                    if (SpellCheck(sValueToBeVerified))
        //                                    {
        //                                        txt.BackColor = Color.White;
        //                                        txt.Refresh();
        //                                    }
        //                                    else
        //                                    {
        //                                        AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "INVALID", sFieldName, sValueToBeVerified, true, -1);
        //                                        txt.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
        //                                        txt.Refresh();
        //                                    }
        //                                }
        //                            }
        //                            break;

        //                        case "EMAILFORMATCHECK":
        //                            sValueToBeVerified = drContact[sFieldName].ToString();

        //                            //Check Email format
        //                            if (sValueToBeVerified.Trim().Length > 0)
        //                            {
        //                                if (GM.Email_Check(sValueToBeVerified))
        //                                {
        //                                    txt.BackColor = Color.White;
        //                                    txt.Refresh();
        //                                }
        //                                else
        //                                {
        //                                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "INVALID", sFieldName, sValueToBeVerified, true, -1);
        //                                    txt.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
        //                                    txt.Refresh();
        //                                }
        //                            }
        //                            else
        //                                AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "EMPTY", sFieldName, sValueToBeVerified, true, -1);
        //                            break;

        //                        case "JOBTITLEDUPECHECK":
        //                            sValueToBeVerified = drContact[sFieldName].ToString();
        //                            //Check Duplicate Job Title
        //                            if (GV.sAllowDuplicateJobTitle == "N")
        //                            {
        //                                string sDupeIDs = string.Empty;
        //                                if (dtMasterContacts.Select(sFieldName + " = '" + sValueToBeVerified.Replace("'", "''") + "' AND (TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeValidated + ") OR WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeValidated + "))").Length > 1)// && (!(sValidation.Contains("Duplicate Email Found"))))
        //                                {
        //                                    for (int i = 0; i < dtMasterContacts.Rows.Count; i++)
        //                                    {
        //                                        if 
        //                                        (
        //                                            dtMasterContacts.Rows[i]["MASTER_ID"].ToString() == iCompanyID.ToString()
        //                                            &&
        //                                            dtMasterContacts.Rows[i][sFieldName].ToString().Trim().ToLower() == sValueToBeVerified.Trim().ToLower()
        //                                            &&
        //                                            (
        //                                                (GV.lstEmailChackContactStatus.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
        //                                                ||
        //                                                (GV.lstEmailChackContactStatus.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
        //                                            )
        //                                        )
        //                                        {
        //                                            if (sDupeIDs.Length > 0)
        //                                                sDupeIDs += ", " + i + 1;
        //                                            else
        //                                                sDupeIDs = (i + 1).ToString();
        //                                        }
        //                                    }
        //                                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "JOBTITLEDUPE", sFieldName, sValueToBeVerified, "", sDupeIDs, true, -1);
        //                                    txt.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
        //                                    txt.Refresh();
        //                                }
        //                                else
        //                                {
        //                                    txt.BackColor = Color.White;
        //                                    txt.Refresh();
        //                                }
        //                            }
        //                            break;

        //                        case "NAMEDUPECHECK":
        //                            //Check Dupe within contactname

        //                            if (dtMasterContacts.Select("FIRST_NAME = '" + drContact["FIRST_NAME"].ToString().Replace("'", "''") + "' AND LAST_NAME = '" + drContact["LAST_NAME"].ToString().Replace("'", "''") + "' AND (TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeValidated + ") OR WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeValidated + "))").Length > 1)// Name Dupe Check
        //                            {
        //                                sValueToBeVerified = drContact["FIRST_NAME"] + " " + drContact["LAST_NAME"];
        //                                string sIDs = string.Empty;
        //                                string sFName = drContact["FIRST_NAME"].ToString().Trim().ToLower();
        //                                string sLName = drContact["LAST_NAME"].ToString().Trim().ToLower();
        //                                for (int i = 0; i < dtMasterContacts.Rows.Count; i++)//Show row number of contact
        //                                {
        //                                    if 
        //                                    (
        //                                        (
        //                                            dtMasterContacts.Rows[i]["MASTER_ID"].ToString() == iCompanyID.ToString()
        //                                            &&
        //                                            dtMasterContacts.Rows[i]["FIRST_NAME"].ToString().Trim().ToLower() == sFName
        //                                            &&
        //                                            dtMasterContacts.Rows[i]["LAST_NAME"].ToString().Trim().ToLower() == sLName
        //                                        )
        //                                        &&
        //                                        (
        //                                            GV.lstTRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase)
        //                                            ||
        //                                            GV.lstWRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase)
        //                                        )
        //                                    )
        //                                    {
        //                                        if (sIDs.Length > 0)
        //                                            sIDs += ", " + (i + 1);
        //                                        else
        //                                            sIDs = (i + 1).ToString();
        //                                    }
        //                                }
        //                                AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "NAMEDUPE", sFieldName, sValueToBeVerified, "", sIDs, true, -1);
        //                            }
        //                            break;


        //                        case "MANDATORYFIELDS":
        //                            //Check Dupe within contactname


        //                            if (sFieldName.Contains("|"))//if validation field contains multiple column(Eg: TITLE|FIRSTNAME|LASTNAME)
        //                            {
        //                                List<string> lstValidationField = sFieldName.Split('|').ToList();
        //                                bool bAtleastOneFieldHasValue = false;
        //                                string sMultipleORfieldValidation = string.Empty;
        //                                foreach (string sFieldNameSplited in lstValidationField)
        //                                {
        //                                    if (drContact[sFieldNameSplited].ToString().Trim().Length > 0)
        //                                        bAtleastOneFieldHasValue = true;
        //                                    else
        //                                    {
        //                                        if (sMultipleORfieldValidation.Length > 0)
        //                                            sMultipleORfieldValidation += " <font color= 'black'>or</font> " + GM.ProperCase_ProjectSpecific(sFieldNameSplited);
        //                                        else
        //                                            sMultipleORfieldValidation += GM.ProperCase_ProjectSpecific(sFieldNameSplited);
        //                                    }
        //                                    //sMultipleORfieldValidation += "<font color = 'DarkCyan'> " + GlobalMethods.ProperCase(sFieldNameSplited) + " </font> cannot be empty if <font color= 'SeaGreen'>" + GlobalMethods.ProperCase(drValidation["VALIDATION_FOR"].ToString()) + "</font> is <font color = 'Tomato'>" + GlobalMethods.ProperCase(drValidation["CONDITION"].ToString()) + " </font>"+Environment.NewLine;
        //                                }
        //                                if (!bAtleastOneFieldHasValue)
        //                                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "EMPTY", sMultipleORfieldValidation, sValueToBeVerified, drValidation["VALIDATION_FOR"].ToString(), drValidation["CONDITION"].ToString(), true, -1);
        //                            }
        //                            else//single column(Eg: FIRSTNAME)
        //                            {
        //                                sValueToBeVerified = drContact[sFieldName].ToString();
        //                                if (sValueToBeVerified.Trim().Length == 0)
        //                                {
        //                                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "EMPTY", sFieldName, sValueToBeVerified, drValidation["VALIDATION_FOR"].ToString(), drValidation["CONDITION"].ToString(), true, -1);
        //                                    txt.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
        //                                    txt.Refresh();
        //                                }
        //                                else
        //                                    txt.BackColor = Color.White;
        //                            }
        //                            break;

        //                        case "EQUALS": // IF TR_CONTACT_STATUS - "EQUALS" - NEW AND COMPLETE - LAST_NAME=Prakash
        //                            if (drContact[drValidation["VALIDATION_FOR"].ToString()].ToString().Trim().ToUpper() == drValidation["CONDITION"].ToString().Trim().ToUpper())
        //                                Exceptional_ValidationType(drContact, drValidation, sFieldName, "Contact", iRowNumber, "IS", iCompanyID, iContactID, true, -1);
        //                            break;

        //                        case "NOTEQUALS": // IF TR_CONTACT_STATUS - "EQUALS" - NEW AND COMPLETE - LAST_NAME=Prakash

        //                            if (drContact[drValidation["VALIDATION_FOR"].ToString()].ToString().Trim().ToUpper() != drValidation["CONDITION"].ToString().Trim().ToUpper())
        //                                Exceptional_ValidationType(drContact, drValidation, sFieldName, "Contact", iRowNumber, "IS NOT", iCompanyID, iContactID, true, -1);
        //                            break;

        //                        case "STARTSWITH": // IF TR_CONTACT_STATUS - "EQUALS" - NEW AND COMPLETE - LAST_NAME=Prakash

        //                            if (drContact[drValidation["VALIDATION_FOR"].ToString()].ToString().Trim().ToUpper().StartsWith(drValidation["CONDITION"].ToString().Trim().ToUpper()))
        //                                Exceptional_ValidationType(drContact, drValidation, sFieldName, "Contact", iRowNumber, "STARTS WITH", iCompanyID, iContactID, true, -1);
        //                            break;

        //                        case "ENDSWITH": // IF TR_CONTACT_STATUS - "EQUALS" - NEW AND COMPLETE - LAST_NAME=Prakash

        //                            if (drContact[drValidation["VALIDATION_FOR"].ToString()].ToString().Trim().ToUpper().EndsWith(drValidation["CONDITION"].ToString().Trim().ToUpper()))
        //                                Exceptional_ValidationType(drContact, drValidation, sFieldName, "Contact", iRowNumber, "ENDS WITH", iCompanyID, iContactID, true, -1);
        //                            break;

        //                        case "CONTAINS": // IF TR_CONTACT_STATUS - "EQUALS" - NEW AND COMPLETE - LAST_NAME=Prakash

        //                            if (drContact[drValidation["VALIDATION_FOR"].ToString()].ToString().Trim().ToUpper().Contains(drValidation["CONDITION"].ToString().Trim().ToUpper()))
        //                                Exceptional_ValidationType(drContact, drValidation, sFieldName, "Contact", iRowNumber, "CONTAINS", iCompanyID, iContactID, true, -1);
        //                            break;

        //                        case "NOTCONTAINS": // IF TR_CONTACT_STATUS - "EQUALS" - NEW AND COMPLETE - LAST_NAME=Prakash

        //                            if (!drContact[drValidation["VALIDATION_FOR"].ToString()].ToString().Trim().ToUpper().Contains(drValidation["CONDITION"].ToString().Trim().ToUpper()))
        //                                Exceptional_ValidationType(drContact, drValidation, sFieldName, "Contact", iRowNumber, "DOES NOT CONTAINS", iCompanyID, iContactID, true, -1);
        //                            break;
        //                    }

        //                    //if (sValidation.ToString().Trim().Length > 0 && iRowNumber != 0)//Append Row number
        //                    //    sValidation = "<b>Row " + iRowNumber + "</b>: " + sValidation;
        //                }
        //            }

        //            if (dtValidationResultsDynamic.Select("RowIndex = " + iRowNumber).Length == 0)//if No Error then remove color for all fields
        //            {
        //                foreach (TextBox txt in lstContactControls)
        //                {
        //                    if (txt.BackColor != Color.White)
        //                    {
        //                        txt.BackColor = Color.White;
        //                        txt.Refresh();
        //                    }
        //                }
        //            }
        //        }
        //        else//if Contact status is empty then highilight Contact status field
        //        {
        //            foreach (TextBox txt in lstContactControls)
        //            {
        //                if (txt.Name.ToUpper() == GV.sAccessTo + "_CONTACT_STATUS")
        //                {
        //                    txt.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
        //                    txt.Refresh();
        //                }
        //                else
        //                {
        //                    txt.BackColor = Color.White;
        //                    txt.Refresh();
        //                }
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
        //    }
        //}


        TextBox GetSpecificControl(string sTableName, string sColumnName)
        {

            if (sTableName == "C" && lstContactControls != null)
            {
                foreach (TextBox txt in lstContactControls)
                {
                    if (txt.Name.ToUpper().Trim() == sColumnName)
                        return txt;
                }
            }
            else if (sTableName == "M" && lstCompanyControls != null)
            {
                foreach (TextBox txt in lstCompanyControls)
                {
                    if (txt.Name.ToUpper().Trim() == sColumnName)
                        return txt;
                }
            }

            return null;

        }

        void CopyControlValues(string sSourceTable, string sSourceColumn, string sTargetTable, string sTargetColumn)//Copy Controls on run time
        {
            try
            {
                TextBox cSource = new TextBox();
                TextBox cTarget = new TextBox();
                List<TextBox> lstSourceControl;
                List<TextBox> lstTargetControl;
                if (sSourceTable == "C")
                    lstSourceControl = lstContactControls;
                else
                    lstSourceControl = lstCompanyControls;

                if (sTargetTable == "C")
                    lstTargetControl = lstContactControls;
                else
                    lstTargetControl = lstCompanyControls;

                foreach (TextBox C in lstSourceControl)
                {
                    if (C.Name.ToUpper().Trim() == sSourceColumn.ToUpper().Trim())
                    {
                        cSource = C;
                        break;
                    }
                }

                foreach (TextBox C in lstTargetControl)
                {
                    if (C.Name.ToUpper().Trim() == sTargetColumn.ToUpper().Trim())
                    {
                        cTarget = C;
                        break;
                    }
                }

                cTarget.Text = cSource.Text.Trim();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //private string WebFormating(string sURL)
        //{
        //    if (sURL.Trim().Length > 0)
        //    {
        //        if (sURL.ToLower().StartsWith("http://"))
        //            sURL = sURL.ToLower().Replace("http://", string.Empty);
        //        else if (sURL.ToLower().StartsWith("https://"))
        //            sURL = sURL.ToLower().Replace("https://", string.Empty);

        //        if (!sURL.StartsWith("www"))
        //            sURL = "www." + sURL;
        //    }
        //    return sURL;
        //}



        //-----------------------------------------------------------------------------------------------------


        private string Remove_SplChars(string sText)
        {
            try
            {
                //Remove Special Chars
                Regex r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                return r.Replace(sText, "");
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return sText;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void TextBox_Indication(TextBox C)
        {
            //Toggle between colors to indicate mandetory
            if (C.Text.Length > 0)
            {
                C.BackColor = Color.White;
            }
            else
            {
                C.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private string CaseCorrection(string sText)
        {
            try
            {
                //Case correction for text:Proper Case
                CultureInfo properCase = System.Threading.Thread.CurrentThread.CurrentCulture;
                TextInfo currentInfo = properCase.TextInfo;
                sText = currentInfo.ToTitleCase(currentInfo.ToLower(sText));
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return sText;
        }

        //-----------------------------------------------------------------------------------------------------
        private List<Control> Load_Controls(DataTable dtFormat)//Creates controls based on datatable passed and adds them to a List..
        {
            var ctrl = new List<Control>();
            try
            {
                string sTableName = string.Empty;
                if (dtFormat.TableName == "MasterContactFormat")
                    sTableName = "Contact";
                else
                    sTableName = "Company";

                foreach (DataRow dr in dtFormat.Rows)
                {
                    switch (dr["CONTROL_TYPE"].ToString().ToUpper())
                    {
                        case "TEXTBOX":
                            ctrl = AddLabelControl(dr["FIELD_NAME_CAPTION"].ToString(), dr["FIELD_NAME_TABLE"].ToString(), ctrl);//Add label to Every Control
                            DevComponents.DotNetBar.Controls.TextBoxX txt = new DevComponents.DotNetBar.Controls.TextBoxX() { Name = dr["FIELD_NAME_TABLE"].ToString(), Tag = sTableName, TabIndex = Convert.ToInt32(dr["SEQUENCE_NO"].ToString()), Width = 200, Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top) };

                            //TextBox txt = new TextBox()                                                                   { Name = dr["FIELD_NAME_TABLE"].ToString(), Tag = dr["FIELD_NAME_TABLE"] + ";", TabIndex = Convert.ToInt32(dr["SEQUENCE_NO"].ToString()), Width = 200, Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top) };


                            txt.Font = new Font(txt.Font.FontFamily, 10);
                            //GM.SetDoubleBuffered(txt);
                            txt.Border.Class = "TextBoxBorder";
                            txt.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                            txt.BorderStyle = BorderStyle.FixedSingle;
                            //GM.SetDoubleBuffered(txt);

                            if (dr["READONLY"].ToString() == "Y")
                            {
                                txt.ReadOnly = true;
                            }

                            if (dr["FIELD_SIZE"].ToString().Length > 0)
                                txt.MaxLength = Convert.ToInt32(dr["FIELD_SIZE"].ToString());

                            txt.TextChanged += new EventHandler(txt_Change);

                            txt.MouseDoubleClick += new MouseEventHandler(txt_Copy);//Copy text on double click

                            //if (dr["FORMATTING"].ToString().Length > 0) //Common text Validations are defiend in 'txt_change'(Only display Validations)
                            //{
                            //    if (dr["FORMATTING"].ToString().Contains("MANDATORY")) //Assign txt_Change Event if the field is mandatory
                            //        txt.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
                            //}

                            txt.KeyDown += new KeyEventHandler(txt_KeyDown);

                            if (dr["SPELLCHECK"].ToString() == "Y")
                            {
                                txt.EnableSpellCheck();
                                SpellCheckSettings s = new SpellCheckSettings();
                                if (GV.sEmployeeName == "THANGAPRAKASH")
                                    s.AllowAdditions = true;
                                else
                                    s.AllowAdditions = false;

                                s.AllowChangeTo = true;
                                s.AllowF7 = false;
                                s.AllowIgnore = false;
                                s.AllowInMenuDefs = true;
                                s.AllowRemovals = false;
                                txt.SpellCheck().Settings = s;
                                //NHunspellTextBoxExtender1.EnableTextBoxBase(txt);
                            }


                            //If it is Master contacts control then data from control must synch with grid
                            if (dtFormat.TableName == "MasterContactFormat")
                            {
                                txt.Width = 250;
                                //txt.TextChanged += new EventHandler(TextChangeContact);//i.e. changes in textbox must affect the relevent grid cell
                            }
                            ctrl.Add(txt);
                            break;

                        case "CONTACTNAMETEXTBOX":
                            ctrl = AddLabelControl(dr["FIELD_NAME_CAPTION"].ToString(), dr["FIELD_NAME_TABLE"].ToString(), ctrl);//Add label to Every Control
                            DevComponents.DotNetBar.Controls.TextBoxX txtName = new DevComponents.DotNetBar.Controls.TextBoxX() { Name = dr["FIELD_NAME_TABLE"].ToString(), Tag = sTableName, TabIndex = Convert.ToInt32(dr["SEQUENCE_NO"].ToString()), Width = 200, Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top) };
                            txtName.Font = new Font(txtName.Font.FontFamily, 10);
                            //GM.SetDoubleBuffered(txtName);
                            //txt.Border.Class = "TextBoxBorder";
                            //txt.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                            //txt.BorderStyle = BorderStyle.FixedSingle;

                            if (dr["READONLY"].ToString() == "Y")
                            {
                                txtName.ReadOnly = true;
                            }

                            if (dr["FIELD_SIZE"].ToString().Length > 0)
                                txtName.MaxLength = Convert.ToInt32(dr["FIELD_SIZE"].ToString());

                            txtName.MouseDoubleClick += new MouseEventHandler(txt_Copy);//Copy text on double click

                            txtName.ButtonCustom.Visible = true;                            
                            txtName.ButtonCustomClick += new EventHandler(btn_NameSearch);
                            txtName.ButtonCustom.Image = GCC.Properties.Resources.search;
                            txtName.ButtonCustom.Shortcut = eShortcut.F2;

                            if (sTableName == "Contact" && GV.NameSayer)
                            {
                                txtName.ButtonCustom2.Visible = true;
                                txtName.ButtonCustom2Click += new EventHandler(btn_NameSayer);                                
                                txtName.ButtonCustom2.Image = GCC.Properties.Resources.audio16x16;
                                //txtName.ButtonCustom2.Image = GCC.Properties.Resources.ezgif_3008983895;
                                txtName.ButtonCustom2.Shortcut = eShortcut.F7;
                            }


                            txtName.Border.Class = "TextBoxBorder";
                            txtName.Border.CornerType = eCornerType.Square;
                            txtName.BackColor = Color.White;

                            txtName.TextChanged += new EventHandler(txt_Change);

                            //if (dr["FORMATTING"].ToString().Length > 0) //Common text Validations are defiend in 'txt_change'(Only display Validations)
                            //{
                            //    if (dr["FORMATTING"].ToString().Contains("MANDATORY")) //Assign txt_Change Event if the field is mandatory
                            //        txtName.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
                            //}

                            txtName.KeyDown += new KeyEventHandler(txt_KeyDown);

                            if (dr["SPELLCHECK"].ToString() == "Y")
                            {
                                txtName.EnableSpellCheck();
                                SpellCheckSettings s = new SpellCheckSettings();
                                s.AllowAdditions = false;
                                s.AllowChangeTo = true;
                                s.AllowF7 = false;
                                s.AllowIgnore = false;
                                s.AllowInMenuDefs = true;
                                s.AllowRemovals = false;
                                txtName.SpellCheck().Settings = s;
                                //NHunspellTextBoxExtender1.EnableTextBoxBase(txtName);
                            }


                            //If it is Master contacts control then data from control must synch with grid
                            if (dtFormat.TableName == "MasterContactFormat")
                            {
                                txtName.Width = 250;
                                //txtName.TextChanged += new EventHandler(TextChangeContact);//i.e. changes in textbox must affect the relevent grid cell
                            }
                            ctrl.Add(txtName);
                            break;

                        case "COMBOBOX":
                            ctrl = AddLabelControl(dr["FIELD_NAME_CAPTION"].ToString(), dr["FIELD_NAME_TABLE"].ToString(), ctrl);//Add label to Every Control
                            ComboBox cmb = new ComboBox() { Name = dr["FIELD_NAME_TABLE"].ToString(), Tag = sTableName, TabIndex = Convert.ToInt32(dr["SEQUENCE_NO"].ToString()), Width = 200, Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top) };
                            cmb.Font = new Font(cmb.Font.FontFamily, 10);
                            //txt.Border.Class = "TextBoxBorder";
                            //txt.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;

                            if (dr["READONLY"].ToString() == "Y")
                                cmb.Enabled = false;

                            //GM.SetDoubleBuffered(cmb);

                            //if (dr["FIELD_SIZE"].ToString().Length > 0)
                            //    cmb.MaxLength = Convert.ToInt32(dr["FIELD_SIZE"].ToString());

                            //if (dr["FORMATTING"].ToString().Length > 0) //Common text Validations are defiend in 'txt_change'(Only display Validations)
                            //{
                            //    //cmb.TextChanged += new EventHandler(txt_Change);
                            //    if (dr["FORMATTING"].ToString().Contains("MANDATORY")) //Assign txt_Change Event if the field is mandatory
                            //        cmb.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
                            //}

                            //If it is Master contacts control then data from control must synch with grid
                            if (dtFormat.TableName == "MasterContactFormat")
                            {
                                cmb.Width = 250;
                                // cmb.TextChanged += new EventHandler(TextChangeContact);//i.e. changes in textbox must affect the relevent grid cell
                            }
                            ctrl.Add(cmb);
                            break;
                        case "EXTENDEDCOMBO":
                            ctrl = AddLabelControl(dr["FIELD_NAME_CAPTION"].ToString(), dr["FIELD_NAME_TABLE"].ToString(), ctrl);//Add label to Every Control
                            //CustomList CList = new CustomList("...", 25, btn_Click, dr["FIELD_NAME_TABLE"].ToString())//Custom List box.. A textbox with button//
                            //Can be replaced with DevComponents Textbox
                            DevComponents.DotNetBar.Controls.TextBoxX txtCmbo = new DevComponents.DotNetBar.Controls.TextBoxX() //Textbox with option button
                            { Name = dr["FIELD_NAME_TABLE"].ToString(), Tag = sTableName, TabIndex = Convert.ToInt32(dr["SEQUENCE_NO"].ToString()), Width = 200, Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top) };

                            txtCmbo.Font = new Font(txtCmbo.Font.FontFamily, 10);



                            //GM.SetDoubleBuffered(txtCmbo);
                            if (dr["READONLY"].ToString() == "Y")
                            {
                                txtCmbo.ReadOnly = true;
                            }

                            txtCmbo.MouseDoubleClick += new MouseEventHandler(txt_Copy);//Copy text on double click

                            if (dr["FIELD_SIZE"].ToString().Length > 0)
                                txtCmbo.MaxLength = Convert.ToInt32(dr["FIELD_SIZE"].ToString());

                            txtCmbo.ButtonCustom.Visible = true;
                            txtCmbo.ButtonCustom.Text = "...";
                            txtCmbo.ButtonCustomClick += new EventHandler(btn_Click);
                            txtCmbo.ButtonCustom.Shortcut = eShortcut.F2;

                            txtCmbo.ButtonCustom2.Visible = true;
                            txtCmbo.ButtonCustom2.Image = GCC.Properties.Resources.eraseicon10x10;
                            txtCmbo.ButtonCustom2Click += new EventHandler(btnClear_Click);


                            txtCmbo.Border.Class = "TextBoxBorder";
                            txtCmbo.Border.CornerType = eCornerType.Square;
                            txtCmbo.BackColor = Color.White;
                            //txtCmbo.ReadOnly = true;

                            txtCmbo.TextChanged += new EventHandler(txt_Change);

                            //if (dr["FORMATTING"].ToString().Length > 0) //Common text Validations are defiend in 'txt_change'(Only display Validations)
                            //{
                            //    if (dr["FORMATTING"].ToString().Contains("MANDATORY")) //Assign txt_Change Event if the field is mandatory
                            //        txtCmbo.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
                            //}

                            if (dr["SPELLCHECK"].ToString() == "Y")
                            {
                                txtCmbo.EnableSpellCheck();
                                SpellCheckSettings s = new SpellCheckSettings();
                                s.AllowAdditions = false;
                                s.AllowChangeTo = true;
                                s.AllowF7 = false;
                                s.AllowIgnore = false;
                                s.AllowInMenuDefs = true;
                                s.AllowRemovals = false;
                                txtCmbo.SpellCheck().Settings = s;
                                //NHunspellTextBoxExtender1.EnableTextBoxBase(txtCmbo);
                            }

                            txtCmbo.KeyDown += new KeyEventHandler(txt_KeyDown);

                            //If it is Master contacts control then data from control must synch with grid
                            if (dtFormat.TableName == "MasterContactFormat")
                            {
                                txtCmbo.Width = 250;
                                //txtCmbo.TextChanged += new EventHandler(TextChangeContact);
                            }
                            ctrl.Add(txtCmbo);
                            break;

                        case "WEBSITE":
                            ctrl = AddLabelControl(dr["FIELD_NAME_CAPTION"].ToString(), dr["FIELD_NAME_TABLE"].ToString(), ctrl);//Add label to Every Control
                            //CustomList CList = new CustomList("...", 25, btn_Click, dr["FIELD_NAME_TABLE"].ToString())//Custom List box.. A textbox with button//
                            //Can be replaced with DevComponents Textbox
                            DevComponents.DotNetBar.Controls.TextBoxX txtWebSite = new DevComponents.DotNetBar.Controls.TextBoxX() //Textbox with option button
                            { Name = dr["FIELD_NAME_TABLE"].ToString(), Tag = sTableName, TabIndex = Convert.ToInt32(dr["SEQUENCE_NO"].ToString()), Width = 200, Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top) };

                            txtWebSite.Font = new Font(txtWebSite.Font.FontFamily, 10);
                            //GM.SetDoubleBuffered(txtWebSite);
                            if (dr["READONLY"].ToString() == "Y")
                                txtWebSite.ReadOnly = true;

                            if (dr["FIELD_SIZE"].ToString().Length > 0)
                                txtWebSite.MaxLength = Convert.ToInt32(dr["FIELD_SIZE"].ToString());

                            txtWebSite.ButtonCustom.Visible = true;
                            //txtWebSite.ButtonCustom.Text = "...";
                            txtWebSite.ButtonCustomClick += new EventHandler(btnWebsite_Click);
                            txtWebSite.ButtonCustom.Shortcut = eShortcut.F2;
                            txtWebSite.ButtonCustom.Image = GCC.Properties.Resources.Earth_icon;

                            txtWebSite.MouseDoubleClick += new MouseEventHandler(txt_Copy);//Copy text on double click

                            txtWebSite.Border.Class = "TextBoxBorder";
                            txtWebSite.Border.CornerType = eCornerType.Square;
                            txtWebSite.BackColor = Color.White;
                            //txtCmbo.ReadOnly = true;


                            txtWebSite.TextChanged += new EventHandler(txt_Change);

                            //if (dr["FORMATTING"].ToString().Length > 0) //Common text Validations are defiend in 'txt_change'(Only display Validations)
                            //{
                            //    if (dr["FORMATTING"].ToString().Contains("MANDATORY")) //Assign txt_Change Event if the field is mandatory
                            //        txtWebSite.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
                            //}

                            //if (dr["SPELLCHECK"].ToString() == "Y")
                            //    NHunspellTextBoxExtender1.EnableTextBoxBase(txtWebSite);

                            txtWebSite.KeyDown += new KeyEventHandler(txt_KeyDown);

                            //If it is Master contacts control then data from control must synch with grid
                            if (dtFormat.TableName == "MasterContactFormat")
                            {
                                txtWebSite.Width = 250;
                                //txtWebSite.TextChanged += new EventHandler(TextChangeContact);
                            }
                            ctrl.Add(txtWebSite);
                            break;

                        case "TELEPHONE":// Text box with dial button
                            ctrl = AddLabelControl(dr["FIELD_NAME_CAPTION"].ToString(), dr["FIELD_NAME_TABLE"].ToString(), ctrl);
                            //TextBox txtDial = new TextBox()
                            DevComponents.DotNetBar.Controls.TextBoxX txtDial = new DevComponents.DotNetBar.Controls.TextBoxX() //Textbox with dial button
                            { Name = dr["FIELD_NAME_TABLE"].ToString(), Tag = sTableName, TabIndex = Convert.ToInt32(dr["SEQUENCE_NO"].ToString()), Width = 200, Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top) };

                            txtDial.Font = new Font(txtDial.Font.FontFamily, 10);
                            //GM.SetDoubleBuffered(txtDial);
                            if (dr["READONLY"].ToString() == "Y")
                                txtDial.ReadOnly = true;
                            if (dr["FIELD_SIZE"].ToString().Length > 0)
                                txtDial.MaxLength = Convert.ToInt32(dr["FIELD_SIZE"].ToString());


                            if (GV.sAccessTo == "TR")
                            {
                                txtDial.ButtonCustom.Visible = true;
                                txtDial.ButtonCustom.Image = GCC.Properties.Resources.Phone_2_iconSmall;
                                txtDial.ButtonCustomClick += new EventHandler(btnDial_Click);

                                txtDial.ButtonCustom2.Visible = true;
                                txtDial.ButtonCustom2.Image = GCC.Properties.Resources.phone_folded_icon_Small;
                                txtDial.ButtonCustom2Click += new EventHandler(btnHangUp_Click);
                            }


                            txtDial.MouseDoubleClick += new MouseEventHandler(txt_Copy);//Copy text on double click

                            txtDial.Border.Class = "TextBoxBorder";
                            txtDial.Border.CornerType = eCornerType.Square;

                            txtDial.TextChanged += new EventHandler(txt_Change);

                            //if (dr["FORMATTING"].ToString().Length > 0) //Common text Validations are defiend in 'txt_change'(Only display Validations)
                            //{
                            //    if (dr["FORMATTING"].ToString().Contains("MANDATORY")) //Assign txt_Change Event if the field is mandatory
                            //        txtDial.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
                            //}

                            if (dtFormat.TableName == "MasterContactFormat")
                            {
                                txtDial.Width = 250;
                                //txtDial.TextChanged += new EventHandler(TextChangeContact);
                            }

                            ctrl.Add(txtDial);
                            //ctrl.Add(btnDial);
                            break;

                        case "DATETIMEPICKER":
                            ctrl = AddLabelControl(dr["FIELD_NAME_CAPTION"].ToString(), dr["FIELD_NAME_TABLE"].ToString(), ctrl);
                            DateTimePicker dtp = new DateTimePicker()
                            //{ Name = dr["FIELD_NAME_TABLE"].ToString(), Tag = dr["FIELD_NAME_TABLE"] + ";", Width = 200,TabIndex = Convert.ToInt32(dr["SEQUENCE_NO"].ToString()) };
                            { Name = dr["FIELD_NAME_TABLE"].ToString(), Tag = sTableName, TabIndex = Convert.ToInt32(dr["SEQUENCE_NO"].ToString()), Width = 200, Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top) };

                            dtp.Font = new Font(dtp.Font.FontFamily, 10);

                            if (dr["READONLY"].ToString() == "Y")
                                dtp.Enabled = false;

                            //GM.SetDoubleBuffered(dtp);


                            //If it is Master contacts control then data from control must synch with grid
                            if (dtFormat.TableName == "MasterContactFormat")
                            {
                                dtp.Width = 250;
                                // dtp.ValueChanged += new EventHandler(TextChangeContact);
                            }
                            ctrl.Add(dtp);
                            break;

                        case "CHECKEDLISTBOX":
                            ctrl = AddLabelControl(dr["FIELD_NAME_CAPTION"].ToString(), dr["FIELD_NAME_TABLE"].ToString(), ctrl);
                            CheckedListBox clb = new CheckedListBox() { Name = dr["FIELD_NAME_TABLE"].ToString(), Tag = sTableName, Width = 200, TabIndex = Convert.ToInt32(dr["SEQUENCE_NO"].ToString()), Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top), CheckOnClick = true };
                            clb.Font = new Font(clb.Font.FontFamily, 10);

                            if (dr["READONLY"].ToString() == "Y")
                                clb.Enabled = false;

                            //GM.SetDoubleBuffered(clb);

                            if (dr["PICKLIST_CATEGORY"].ToString().Length > 0)
                            {
                                List<string> lstItems = Load_PickList(dr["PICKLIST_CATEGORY"].ToString());
                                foreach (string sItems in lstItems)
                                {
                                    if (sItems.Trim().Length > 0)
                                        clb.Items.Add(sItems);
                                }
                            }
                            ctrl.Add(clb);
                            break;

                        case "EMAIL":
                            ctrl = AddLabelControl(dr["FIELD_NAME_CAPTION"].ToString(), dr["FIELD_NAME_TABLE"].ToString(), ctrl);//Add label to Every Control
                            //CustomList CList = new CustomList("...", 25, btn_Click, dr["FIELD_NAME_TABLE"].ToString())//Custom List box.. A textbox with button//
                            //Can be replaced with DevComponents Textbox
                            DevComponents.DotNetBar.Controls.TextBoxX txtEmail = new DevComponents.DotNetBar.Controls.TextBoxX() //Textbox with option button
                            { Name = dr["FIELD_NAME_TABLE"].ToString(), Tag = sTableName, TabIndex = Convert.ToInt32(dr["SEQUENCE_NO"].ToString()), Width = 200, Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top) };

                            txtEmail.Font = new Font(txtEmail.Font.FontFamily, 10);

                            //GM.SetDoubleBuffered(txtEmail);
                            if (dr["READONLY"].ToString() == "Y")
                            {
                                txtEmail.ReadOnly = true;
                            }

                            if (dr["FIELD_SIZE"].ToString().Length > 0)
                                txtEmail.MaxLength = Convert.ToInt32(dr["FIELD_SIZE"].ToString());

                            txtEmail.MouseDoubleClick += new MouseEventHandler(txt_Copy);//Copy text on double click




                            txtEmail.Border.Class = "TextBoxBorder";
                            txtEmail.Border.CornerType = eCornerType.Square;
                            txtEmail.BackColor = Color.White;

                            txtEmail.TextChanged += new EventHandler(txt_Change);

                            txtEmail.KeyDown += new KeyEventHandler(txt_KeyDown);

                            //If it is Master contacts control then data from control must synch with grid
                            if (dtFormat.TableName == "MasterContactFormat")
                            {
                                txtEmail.ButtonCustom.Visible = true;
                                txtEmail.ButtonCustom.Text = "...";
                                txtEmail.ButtonCustomClick += new EventHandler(btn_Click);
                                txtEmail.ButtonCustom.Shortcut = eShortcut.F2;

                                txtEmail.ButtonCustom2.Visible = true;
                                txtEmail.ButtonCustom2.Image = GCC.Properties.Resources.EmailUpdate1;
                                txtEmail.ButtonCustom2Click += new EventHandler(mnuBulkEmailUpdate_Click);

                                txtEmail.Width = 250;
                                // txtEmail.TextChanged += new EventHandler(TextChangeContact);
                            }
                            ctrl.Add(txtEmail);
                            break;


                        case "DUALTEXTBOX":
                            ctrl = AddLabelControl(dr["FIELD_NAME_CAPTION"].ToString(), dr["FIELD_NAME_TABLE"].ToString(), ctrl);//Add label to Every Control
                            //DevComponents.DotNetBar.Controls.TextBoxX txt = new DevComponents.DotNetBar.Controls.TextBoxX() { Name = dr["FIELD_NAME_TABLE"].ToString(), Tag = sTableName, TabIndex = Convert.ToInt32(dr["SEQUENCE_NO"].ToString()), Width = 200, Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top) };


                            DualText txtDual = new DualText { Name = dr["FIELD_NAME_TABLE"].ToString(), Tag = sTableName, TabIndex = Convert.ToInt32(dr["SEQUENCE_NO"].ToString()), Width = 200 };

                            //txtDual.TextBox1.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                            //txtDual.TextBox2.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);

                            txtDual.Anchor = (AnchorStyles.Left | AnchorStyles.Top);

                            //txtDual.TextBox1.Font = new Font(txtDual.Font.FontFamily, 10);
                            //txtDual.TextBox2.Font = new Font(txtDual.Font.FontFamily, 10);                            

                            //txtDual.TextBox1.Border.Class = "TextBoxBorder";
                            //txtDual.TextBox1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;

                            //txtDual.TextBox2.Border.Class = "TextBoxBorder";
                            //txtDual.TextBox2.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;

                            //txtDual.TextBox1.BorderStyle = BorderStyle.FixedSingle;
                            //txtDual.TextBox2.BorderStyle = BorderStyle.FixedSingle;

                            ////GM.SetDoubleBuffered(txt);

                            //if (dr["READONLY"].ToString() == "Y")
                            //{
                            //    txtDual.TextBox1.ReadOnly = true;
                            //    txtDual.TextBox2.ReadOnly = true;                                
                            //}

                            //if (dr["FIELD_SIZE"].ToString().Length > 0)
                            //{
                            //    txtDual.TextBox1.MaxLength = Convert.ToInt32(dr["FIELD_SIZE"].ToString());
                            //    txtDual.TextBox2.MaxLength = Convert.ToInt32(dr["FIELD_SIZE"].ToString());
                            //}

                            txtDual.TextChanged += new EventHandler(txt_Change);

                            txtDual.MouseDoubleClick += new MouseEventHandler(txt_Copy);//Copy text on double click                            

                            txtDual.KeyDown += new KeyEventHandler(txt_KeyDown);

                            if (dr["SPELLCHECK"].ToString() == "Y")
                            {
                                txtDual.EnableSpellCheck();
                                SpellCheckSettings s = new SpellCheckSettings();
                                if (GV.sEmployeeName == "THANGAPRAKASH")
                                    s.AllowAdditions = true;
                                else
                                    s.AllowAdditions = false;

                                s.AllowChangeTo = true;
                                s.AllowF7 = false;
                                s.AllowIgnore = false;
                                s.AllowInMenuDefs = true;
                                s.AllowRemovals = false;
                                txtDual.SpellCheck().Settings = s;
                                //NHunspellTextBoxExtender1.EnableTextBoxBase(txt);
                            }


                            //If it is Master contacts control then data from control must synch with grid
                            if (dtFormat.TableName == "MasterContactFormat")
                            {
                                txtDual.Width = 250;
                                //txt.TextChanged += new EventHandler(TextChangeContact);//i.e. changes in textbox must affect the relevent grid cell
                            }
                            ctrl.Add(txtDual);
                            break;

                        default:
                            MessageBoxEx.Show("Control Definition missing!!!");
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {

                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return ctrl;
        }

        //-----------------------------------------------------------------------------------------------------
        private void txt_Copy(object sender, MouseEventArgs e)
        {
            //TextBox txt = sender as TextBox;
            //if(txt.Text.Trim().Length > 0)
            //{
            //    System.Windows.Forms.Clipboard.SetText(txt.Text);
            //    txt.SelectAll();
            //    ToastNotification.ToastFont = new Font(this.Font.FontFamily, 16);
            //    ToastNotification.Show(this, "Text Copied", DevComponents.DotNetBar.eToastPosition.BottomCenter);
            //}
        }

        //-----------------------------------------------------------------------------------------------------
        private DataTable ContactStatus_Selection()
        {
            string sValidation = string.Empty;
            try
            {
                //if (GV.sUserType == "Agent")
                {
                    string sStatusString = string.Empty;
                    sStatusString += "Operation_Type Like '%Neutral%' OR ";

                    if (dtMasterContacts.Rows[iContactRowIndex]["CONTACT_ID_P"].ToString().Length > 0 && (lstFreezedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iContactRowIndex]["CONTACT_ID_P"])) || lstRecordsToUnlock.Contains(Convert.ToInt32(dtMasterContacts.Rows[iContactRowIndex]["CONTACT_ID_P"]))))
                    { /*Do Nothing*/}//Ignore freezed and Bounced records
                    else
                    {

                        if (dtMasterContacts.Rows[iContactRowIndex]["NEW_OR_EXISTING"].ToString().ToUpper() == "NEW")
                        {
                            if (dtMasterContacts.Rows[iContactRowIndex][GV.sOppositAccess + "_UPDATED_DATE"].ToString().Length > 0)
                            {

                                DataRow drMasterContactsCopy = dtMasterContactsCopy.Select("CONTACT_ID_P = " + dtMasterContacts.Rows[iContactRowIndex]["CONTACT_ID_P"])[0];
                                bool IsContactChanged = false;
                                foreach (DataRow drColumnName in dtFieldMasterContact.Rows)
                                {
                                    if (drColumnName["FIELD_NAME_TABLE"].ToString().ToUpper() == "TR_CONTACT_STATUS" || drColumnName["FIELD_NAME_TABLE"].ToString().ToUpper() == "WR_CONTACT_STATUS")
                                        continue;
                                    if (drMasterContactsCopy[drColumnName["FIELD_NAME_TABLE"].ToString()].ToString().Replace(" ", string.Empty).ToUpper() == dtMasterContacts.Rows[iContactRowIndex][drColumnName["FIELD_NAME_TABLE"].ToString()].ToString().Replace(" ", string.Empty).ToUpper())
                                    { /*Do Nothing*/}
                                    else
                                    {
                                        if (drColumnName["FORMATTING"].ToString().Contains("TELEPHONECONTACTS"))//Telephone contry code is automatically added if not already exist.. It is not user Updated data
                                        {
                                            if (TelephoneFormat(drMasterContactsCopy[drColumnName["FIELD_NAME_TABLE"].ToString()].ToString(), "Contacts", iContactRowIndex).Replace(" ", string.Empty) != dtMasterContacts.Rows[iContactRowIndex][drColumnName["FIELD_NAME_TABLE"].ToString()].ToString().Replace(" ", string.Empty))
                                            {
                                                IsContactChanged = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            IsContactChanged = true;
                                            break;
                                        }
                                    }
                                }

                                if (IsContactChanged)
                                    sStatusString += "Operation_Type Like '%Update%' OR ";
                                else
                                    sStatusString += "Operation_Type Like '%Verify%' OR ";

                            }
                            else if (dtMasterContacts.Rows[iContactRowIndex][GV.sAccessTo + "_UPDATED_DATE"].ToString().Length > 0 && dtMasterContacts.Rows[iContactRowIndex][GV.sOppositAccess + "_UPDATED_DATE"].ToString().Length == 0)
                            {
                                sStatusString += "Operation_Type Like '%New%' OR ";

                                if (GV.lstReplacementOptionRecordContactStatus.Count > 0 && dtMasterContacts.Select(GV.sAccessTo + "_CONTACT_STATUS IN(" + GM.ListToQueryString(GV.lstReplacementOptionRecordContactStatus, "String") + ")").Length > 0)
                                {
                                    sStatusString += "Operation_Type Like '%Replace%' OR ";
                                }
                            }
                        }
                        else
                        {
                            DataRow drMasterContactsCopy = dtMasterContactsCopy.Select("CONTACT_ID_P = " + dtMasterContacts.Rows[iContactRowIndex]["CONTACT_ID_P"])[0];
                            bool IsContactChanged = false;
                            foreach (DataRow drColumnName in dtFieldMasterContact.Rows)
                            {
                                if (drColumnName["FIELD_NAME_TABLE"].ToString().ToUpper() == "TR_CONTACT_STATUS" || drColumnName["FIELD_NAME_TABLE"].ToString().ToUpper() == "WR_CONTACT_STATUS")
                                    continue;
                                if (drMasterContactsCopy[drColumnName["FIELD_NAME_TABLE"].ToString()].ToString().Replace(" ", string.Empty).ToUpper() == dtMasterContacts.Rows[iContactRowIndex][drColumnName["FIELD_NAME_TABLE"].ToString()].ToString().Replace(" ", string.Empty).ToUpper())
                                { /*Do Nothing*/}
                                else
                                {
                                    if (drColumnName["FORMATTING"].ToString().Contains("TELEPHONECONTACTS"))//Telephone contry code is automatically added if not already exist.. It is not user Updated data
                                    {
                                        if (TelephoneFormat(drMasterContactsCopy[drColumnName["FIELD_NAME_TABLE"].ToString()].ToString(), "Contacts", iContactRowIndex).Replace(" ", string.Empty) != dtMasterContacts.Rows[iContactRowIndex][drColumnName["FIELD_NAME_TABLE"].ToString()].ToString().Replace(" ", string.Empty))
                                        {
                                            IsContactChanged = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        IsContactChanged = true;
                                        break;
                                    }
                                }
                            }

                            if (IsContactChanged)
                                sStatusString += "Operation_Type Like '%Update%' OR ";
                            else
                                sStatusString += "Operation_Type Like '%Verify%' OR ";
                        }
                    }

                    if (sStatusString.Length > 0)
                        sStatusString = sStatusString.Substring(0, sStatusString.Length - 4);

                    //Ignore for now
                    //DataRow[] drr = dtRecordStatus.Select("Research_Type = '" + GV.sAccessTo + "' AND Table_Name = 'Contact' AND "+ sStatusString);
                    DataRow[] drr = dtRecordStatus.Select("Research_Type = '" + GV.sAccessTo + "' AND Table_Name = 'Contact' AND Operation_Type NOT LIKE '%Hidden%'");

                    if (drr.Length > 0)
                    {
                        DataTable dtReturnStatus = drr.CopyToDataTable();
                        dtReturnStatus.Columns["Primary_Status"].ColumnName = "PicklistValue";
                        return dtReturnStatus.AsEnumerable().OrderBy(c => c.Field<int?>("SORT") == null).ThenBy(c => c.Field<int?>("SORT")).AsDataView().ToTable(); ;
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return null;
        }


        private void btn_NameSayer(object sender, EventArgs e)//Runtime event..
        {
            if (bWorkerNameSayer.IsBusy)
                this.Invoke((MethodInvoker)delegate { ToastNotification.Show(this, "Busy processing previous request.", eToastPosition.TopRight); });
            else
            {
                sRequestType = "ES";
                iRequestWaitTime = 40;
                iESPTimertick = 0;                
                timerESRequest.Start();
                bWorkerNameSayer.RunWorkerAsync();
            }
        }

        private void bWorkerNameSayer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (sRequestType == "ES")
            {
                if (iNameSayerRowIndex > -1)
                {
                    RefreshContactGrid(iNameSayerRowIndex);
                }                
            }
            else if (sRequestType == "GS")
            {
                btnGSpeechRecord.Invoke((MethodInvoker)delegate { btnGSpeechRecord.Visible = true; });
                circularProgressGSpeech.Invoke((MethodInvoker)delegate { circularProgressGSpeech.IsRunning = false; });
                circularProgressGSpeech.Invoke((MethodInvoker)delegate { circularProgressGSpeech.Visible = false; });
            }

            timerESRequest.Stop();
            iESPTimertick = 0;
        }

        private void bWorkerNameSayer_DoWork(object sender, DoWorkEventArgs e)
        {
            if(sRequestType == "ES")
                NameSayer();
            else if (sRequestType == "GS")
            {
                btnGSpeechRecord.Invoke((MethodInvoker)delegate { btnGSpeechRecord.Visible = false; });
                circularProgressGSpeech.Invoke((MethodInvoker)delegate { circularProgressGSpeech.Visible = true; });
                GSpeechTranscribe();
            }            
        }

        void Load_NameSayer()
        {
            if (GV.NameSayer)
            {
                List<string> lstName = new List<string>();
                foreach (DataRow drContacts in dtMasterContacts.Rows)
                {
                    string sFName = drContacts["FIRST_NAME"].ToString().Trim();
                    string sLName = drContacts["LAST_NAME"].ToString().Trim();
                    if (sFName.Length > 0)
                        lstName.AddRange(sFName.Split(' ').ToList());
                    if (sLName.Length > 0)
                        lstName.AddRange(sLName.Split(' ').ToList());
                }
                lstName = lstName.Distinct().ToList();
                string sQString = string.Empty;
                foreach (string sName in lstName)
                {
                    if (sQString.Length == 0)
                        sQString = "'" + GM.HandleBackSlash(sName.Replace("'", "''")) + "'";
                    else
                        sQString += ",'" + GM.HandleBackSlash(sName.Replace("'", "''")) + "'";
                }

                if (sQString.Length > 0)
                    dtNameSayer = GV.MSSQL.BAL_ExecuteQuery("SELECT AudioID,Name, Phonetics FROM NameSayer WHERE Name IN (" + sQString + ");");
                else
                {
                    dtNameSayer.Columns.Add("AudioID", typeof(int));
                    dtNameSayer.Columns.Add("Name", typeof(string));
                    dtNameSayer.Columns.Add("Phonetics", typeof(string));
                }
            }
        }

        void NameSayer()
        {
            try
            {
                iNameSayerRowIndex = iContactRowIndex;
                string sFName = dtMasterContacts.Rows[iContactRowIndex]["FIRST_NAME"].ToString().Trim();
                string sLname = dtMasterContacts.Rows[iContactRowIndex]["LAST_NAME"].ToString().Trim();
                List<string> lstWords = new List<string>();
                if (sFName.Length > 0)
                    lstWords.AddRange(sFName.Split(' ').ToList());

                if (sLname.Length > 0)
                    lstWords.AddRange(sLname.Split(' ').ToList());

                if (lstWords.Count > 0)
                {
                    foreach (string sWords in lstWords)
                    {
                        if (dtNameSayer.Select("Name = '" + sWords.Replace("'", "''") + "'").Length == 0)
                            dtNameSayer.Rows.Add(-1, sWords, "");
                    }

                    //string sQstring = GM.ListToQueryString(lstWords, "String");
                    string sQString = string.Empty;
                    foreach (string sName in lstWords)
                    {
                        if (sQString.Length == 0)
                            sQString = "'" + GM.HandleBackSlash(sName.Replace("'", "''")) + "'";
                        else
                            sQString += ",'" + GM.HandleBackSlash(sName.Replace("'", "''")) + "'";
                    }

                    DataTable dtExistingNameList = GV.MSSQL.BAL_ExecuteQuery("SELECT * FROM NameSayer WHERE Name IN (" + sQString + ");");

                    //dtNameSayer.Columns.Add("AudioID", typeof(string));
                    //dtNameSayer.Columns.Add("Name", typeof(string));
                    //dtNameSayer.Columns.Add("Phonetics", typeof(string));


                    foreach (string sName in lstWords)
                    {
                        DataRow[] drrExistingNameList = dtExistingNameList.Select("Name = '" + GM.RemoveEndBackSlash(sName.Replace("'", "''")) + "'");
                        DataRow drNameSayer = dtNameSayer.Select("Name = '" + sName.Replace("'", "''") + "'")[0];
                        if (drrExistingNameList.Length > 0)
                        {
                            drNameSayer["AudioID"] = drrExistingNameList[0]["AudioID"].ToString();
                            drNameSayer["Phonetics"] = drrExistingNameList[0]["Phonetics"].ToString();

                            if (File.Exists(@"\\172.27.137.182\Campaign Manager\NameSayer\" + drrExistingNameList[0]["AudioID"] + ".wav"))
                                GV.MSSQL.BAL_ExecuteQuery("UPDATE NameSayer SET HitCount = HitCount + 1, LastHitDate = GETDATE() WHERE AudioID = '" + drrExistingNameList[0]["AudioID"] + "'");
                            else
                            {
                                NameSayer_Download(drNameSayer, true);
                                //Notify Admin
                            }
                        }
                        else
                        {
                            // drNameSayer["FileExist"] = "N";
                            NameSayer_Download(drNameSayer, false);
                        }
                    }

                    foreach (string sName in lstWords)
                    {
                        if(dtNameSayer.Select("Name = '" + sName.Replace("'", "''") + "'").Length > 0)
                            NameSayer_Play(dtNameSayer.Select("Name = '" + sName.Replace("'", "''") + "'")[0]["AudioID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);
            }
        }

        List<string> GetMachine()
        {
            List<string> lstReturn = new List<string>();
            using (DataTable dtMachine = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT IP,ESPort FROM c_machines WHERE STATUS = 'Online' AND LENGTH(IFNULL(ESPort,'')) > 0 AND ESState = 0 AND CMVersion NOT IN ('Campaign Manager v3.0.1 Build 95','Campaign Manager v3.1.2 Build 23','Campaign Manager v3.1.2 Build 35') ORDER BY RAND() LIMIT 1"))
            {
                if(dtMachine.Rows.Count > 0)
                {                    
                    lstReturn.Add(dtMachine.Rows[0][0].ToString());
                    lstReturn.Add(dtMachine.Rows[0][1].ToString());                    
                }
            }
            return lstReturn;
        }

        int iESPTimertick = 0;
        private void timerESRequest_Tick(object sender, EventArgs e)
        {            
            iESPTimertick++;            
        }




        void NameSayer_Download(DataRow drNameSayer, bool IsExisting)
        {
            string sAdditionalInfo = string.Empty;
            try
            {
                if (GV.IsWindowsXP)
                {
                    if (GV.IP.StartsWith("172.27"))
                    {
                        string sDataToSend = "ESQuery[:|:]" + GV.IP + "[:|:]" + drNameSayer["Name"] + "[:|:]" + drNameSayer["AudioID"] + "[:|:]" + IsExisting + "[:|:]" + GV.ESPort;
                        Random Rand = new Random();
                        int iPort = Rand.Next(2000, 9000);//Max 65535
                        using (System.Net.Sockets.UdpClient sender1 = new System.Net.Sockets.UdpClient(iPort))
                        {
                            List<string> lstMachine = GetMachine();
                            if (lstMachine.Count > 0)
                            {
                                sESPhonetics = string.Empty;
                                sESAudioID = string.Empty;
                                sCurrentESName = drNameSayer["Name"].ToString();

                                sender1.Send(Encoding.ASCII.GetBytes(sDataToSend), sDataToSend.Length, lstMachine[0], Convert.ToInt32(lstMachine[1]));
                                int iStart = iESPTimertick;
                                while (sESAudioID.Length == 0 || sESPhonetics.Length == 0)//Wait till result arrives
                                {
                                    if ((iESPTimertick - iStart) > iRequestWaitTime)
                                    {
                                        sCurrentESName = string.Empty;
                                        return;
                                    }
                                }
                                if (sESAudioID.Length > 0 && sESAudioID != "Error")
                                {
                                    drNameSayer["Phonetics"] = sESPhonetics;
                                    drNameSayer["AudioID"] = sESAudioID;
                                }
                                //else
                                // Invoke((MethodInvoker)delegate { ToastNotification.Show(this, "Service machine not responding.", eToastPosition.TopRight); });
                                sCurrentESName = string.Empty;
                            }
                            //else                                
                            // Invoke((MethodInvoker)delegate {ToastNotification.Show(this, "Service machines not available.", eToastPosition.TopRight); });                            
                        }
                        // else
                        //   Invoke((MethodInvoker)delegate { ToastNotification.Show(this, "Unable to process request on this machine.", eToastPosition.TopRight); });
                    }
                }
                else
                {
                    string sAPI = "http://espeech.com/namesayer_servertest?name=" + drNameSayer["Name"] + "&user=meritapi&tts=1";
                    sAdditionalInfo = "API:" + sAPI + "|";
                    using (WebClient wClient = new WebClient())
                    {
                        string sResponse = wClient.DownloadString(sAPI);
                        sAdditionalInfo += "WebResponse:" + sResponse + "|";
                        string sInsertID = string.Empty;
                        if (sResponse.Contains(":ttsspeech") && sResponse.Contains(".wav"))
                        {
                            drNameSayer["Phonetics"] = sResponse.Replace("<html>", string.Empty).Replace("</html>", string.Empty).Replace("\n", string.Empty).Split(new string[] { ":ttsspeech" }, StringSplitOptions.None)[0];
                            if (IsExisting)
                            {
                                sInsertID = drNameSayer["AudioID"].ToString();
                                GV.MSSQL.BAL_ExecuteQuery("UPDATE NameSayer SET HitCount = HitCount + 1, LastHitDate = GETDATE() WHERE AudioID = '" + sInsertID + "'");
                            }
                            else
                            {
                                sInsertID = GV.MSSQL.BAL_InsertAndGetIdentity("INSERT INTO NameSayer (Name, Phonetics, HitCount, LastHitDate) VALUES ('" + GM.HandleBackSlash(drNameSayer["Name"].ToString().Replace("'", "''")) + "', '" + drNameSayer["Phonetics"].ToString().Replace("'", "''") + "', 1, GETDATE());");
                                drNameSayer["AudioID"] = sInsertID;
                            }
                            sAdditionalInfo += "AudioID:" + sInsertID + "|";
                            if (sInsertID.Length > 0)                                                            
                                wClient.DownloadFile("https://www.espeech.com/" + sResponse.Substring(sResponse.IndexOf("ttsspeech")).Replace("</html>", string.Empty).Replace("\n", string.Empty), @"\\172.27.137.182\Campaign Manager\NameSayer\" + sInsertID + ".wav");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false, sAdditionalInfo);
            }
        }

        void NameSayer_Play(string sAudioID)
        {
            try
            {
                if (sAudioID.Length > 0)
                {
                    if (File.Exists(@"\\172.27.137.182\Campaign Manager\NameSayer\" + sAudioID + ".wav"))
                    {
                        using (System.Media.SoundPlayer sPlayer = new System.Media.SoundPlayer(@"\\172.27.137.182\Campaign Manager\NameSayer\" + sAudioID + ".wav"))
                        {
                            sPlayer.PlaySync();
                        }
                    }
                }
                //else
                //{
                //    this.Invoke((MethodInvoker)delegate { ToastNotification.Show(this, "Speech engine busy.", eToastPosition.TopRight); });
                //}
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);
            }
        }

        string NameSayer_Phonetics(string sName)
        {
            try
            {
                if (sName.Length > 0)
                {
                    DataRow[] drrNameSayer = dtNameSayer.Select("Name = '" + GM.HandleBackSlash(sName.Replace("'", "''")) + "'");
                    if (drrNameSayer.Length > 0 && drrNameSayer[0]["Phonetics"].ToString().Length > 0)
                    {
                        return drrNameSayer[0]["Phonetics"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);
            }
            return string.Empty;
        }


        //-----------------------------------------------------------------------------------------------------
        private void btn_NameSearch(object sender, EventArgs e)//Runtime event..
        {
            // dtMasterContacts.Rows[iGridRowIndex]["First_Name"].ToString()
            // dtMasterContacts.Rows[iGridRowIndex]["Last_Name"].ToString()
            //string sSoundXQuery = string.Empty;
            //sSoundXQuery = "SELECT Company.Master_ID, Company.COMPANY_NAME, FIRST_NAME,LAST_NAME,CONTACT_EMAIL FROM " + GV.sContactTable + " Contact INNER JOIN " + GV.sCompanyTable + " Company ON Contact.MASTER_ID = Company.MASTER_ID WHERE Contact.First_Name_Soundx =  SOUNDEX('" + dtMasterContacts.Rows[iContactRowIndex]["First_Name"].ToString().Replace("'", "''").Replace("\\", "") + "') AND Contact.Last_Name_Soundx = SOUNDEX('" + dtMasterContacts.Rows[iContactRowIndex]["Last_Name"].ToString().Replace("'", "''").Replace("\\", "") + "') AND Company.MASTER_ID <> " + sMaster_ID + ";";
            //DataTable dtSoundxResult = GV.MYSQL.BAL_ExecuteQueryMySQL(sSoundXQuery);
            using (frmSearch objfrmSearch = new frmSearch())
            {
                TextBox txt = sender as TextBox;
                objfrmSearch.drCompany = dtMasterCompanies.Rows[iCompanyRowIndex];
                if (dtMasterContacts.Select("MASTER_ID = " + sMaster_ID).Length > 0)
                    objfrmSearch.drContact = dtMasterContacts.Rows[iContactRowIndex];
                else
                    objfrmSearch.drContact = null;
                objfrmSearch.StartPosition = FormStartPosition.CenterScreen;
                objfrmSearch.sMaster_ID = sMaster_ID;
                //if (dtMasterContacts.Columns.Contains(txt.Name))
                //    objfrmSearch.TableName = "Contact";
                //else
                //    objfrmSearch.TableName = "Company";

                objfrmSearch.TableToReturn = txt.Tag.ToString();
                objfrmSearch.SearchTriggeredFrom = txt.Tag.ToString();
                objfrmSearch.iCompanyRowIndex = iCompanyRowIndex;
                objfrmSearch.iContactRowIndex = iContactRowIndex;
                objfrmSearch.dtFieldMasterCompany = dtFieldMasterCompany;
                objfrmSearch.dtFieldMasterContact = dtFieldMasterContact;
                objfrmSearch.ShowDialog(this);
                DataRow drReturn = objfrmSearch.drReturnRow;
                if (drReturn != null)
                {
                    if (lstFreezedMasterIDs.Contains(Convert.ToInt32(dtMasterCompanies.Rows[iCompanyRowIndex]["MASTER_ID"])))// If company is freezed then do not update anything
                        return;

                    if (objfrmSearch.TableToReturn == "Contact")
                    {
                        if (iContactRowIndex == -1)
                            ToastNotification.Show(this, "Add new contact to populate values.", eToastPosition.TopRight);
                        else
                        {
                            if (dtMasterContacts.Rows[iContactRowIndex]["Contact_ID_P"].ToString().Length > 0 && lstFreezedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iContactRowIndex]["Contact_ID_P"])))//Freezed Contacts
                                return;

                            List<string> lstContactPopulateColumns = objfrmSearch.lstContactColumnsToPopulate;
                            foreach (string sColumn in lstContactPopulateColumns)
                            {
                                string sColumnToPoplulate = dtFieldMasterContact.Select("ALLOW_POPULATE_FROM_SEARCH = '" + sColumn + "'")[0]["FIELD_NAME_TABLE"].ToString();
                                foreach (Control C in lstContactControls)
                                {
                                    if (C.Name.ToUpper() == sColumnToPoplulate.ToUpper())
                                    {
                                        C.Text = drReturn[sColumn].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (objfrmSearch.TableToReturn == "Company")
                    {
                        List<string> lstCompanyPopulateColumns = objfrmSearch.lstCompanyColumnsToPopulate;

                        foreach (string sColumn in lstCompanyPopulateColumns)
                        {
                            string sColumnToPoplulate = dtFieldMasterCompany.Select("ALLOW_POPULATE_FROM_SEARCH = '" + sColumn + "'")[0]["FIELD_NAME_TABLE"].ToString();
                            foreach (Control C in lstCompanyControls)
                            {
                                if (C.Name.ToUpper() == sColumnToPoplulate.ToUpper())
                                {
                                    C.Text = drReturn[sColumn].ToString();
                                    break;
                                }
                            }
                        }
                    }
                    else if (objfrmSearch.TableToReturn == "PAF")
                    {
                        List<string> lstPAFPopulateColumns = objfrmSearch.lstPAFColumns;

                        if (objfrmSearch.SearchTriggeredFrom == "Company")
                        {
                            foreach (string sColumn in lstPAFPopulateColumns)
                            {
                                foreach (Control C in lstCompanyControls)
                                {
                                    if (C.Name.ToUpper() == sColumn.ToUpper())
                                    {
                                        C.Text = drReturn[sColumn].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (string sColumn in lstPAFPopulateColumns)
                            {
                                foreach (Control C in lstContactControls)
                                {
                                    if (C.Name.ToUpper() == sColumn.ToUpper())
                                    {
                                        C.Text = drReturn[sColumn].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void btn_Click(object sender, EventArgs e)//Runtime event.. //Button Click for 'Customlist' controls which opens the Custom designed combobox
        {
            try
            {
                TextBox txt = sender as TextBox;//Gets which Textbox button triggers the event
                if (txt != null)
                {
                    string sValidation_For = string.Empty;//Get relevent textbox field of button
                    string sTable_Name = string.Empty;
                    if (txt.Tag.ToString() == "Company")
                    {
                        sValidation_For = "M." + txt.Name;
                        sTable_Name = "Master";
                    }
                    else
                    {
                        sValidation_For = "C." + txt.Name;
                        sTable_Name = "MasterContacts";
                    }

                    string sCategoryValue = string.Empty;
                    DataRow[] drrFieldMaster = dtFieldMaster_Active.Select("FIELD_NAME_TABLE = '" + txt.Name + "' AND TABLE_NAME = '" + sTable_Name + "'");
                    DataTable dtPickListData = null;

                    if (dtPreUpdate != null && dtPreUpdate.Rows.Count > 0)
                    {
                        DataRow[] drrPreUpdate = drrPreUpdate = dtPreUpdate.Select("OPERATION_TYPE = 'PreUpdate' AND VALIDATION_TYPE = 'GETLIST' AND VALIDATION_FOR = '" + sValidation_For + "'");
                        if (drrPreUpdate.Length > 0)
                        {
                            string sPickList_ColumnName = drrPreUpdate[0]["VALIDATION_VALUE"].ToString().Split('=')[0].Trim();
                            string sControl_ColumnName = drrPreUpdate[0]["VALIDATION_VALUE"].ToString().Split('=')[1].Trim();

                            List<TextBox> lstControlsToSearch = new List<TextBox>();
                            if (sControl_ColumnName.StartsWith("C."))
                            {
                                sControl_ColumnName = sControl_ColumnName.Replace("C.", string.Empty).ToUpper();
                                lstControlsToSearch = lstContactControls;
                            }
                            else if (sControl_ColumnName.StartsWith("M."))
                            {
                                sControl_ColumnName = sControl_ColumnName.Replace("M.", string.Empty).ToUpper();
                                lstControlsToSearch = lstCompanyControls;
                            }

                            foreach (TextBox txtToSearch in lstControlsToSearch)
                            {
                                if (txtToSearch.Name.ToUpper() == sControl_ColumnName && txtToSearch.Text.Trim().Length > 0)
                                {
                                    if (dtPicklist.Select(sPickList_ColumnName + " = '" + txtToSearch.Text.Trim().Replace("'", "''") + "'").Length > 0)
                                    {
                                        dtPickListData = dtPicklist.Select(sPickList_ColumnName + " = '" + txtToSearch.Text.Trim().Replace("'", "''") + "'").CopyToDataTable();
                                        sCategoryValue = txtToSearch.Text.Trim();
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (drrFieldMaster.Length > 0 && (drrFieldMaster[0]["PICKLIST_CATEGORY"].ToString().Length > 0 || (dtPickListData != null && dtPickListData.Rows.Count > 0)))
                    {
                        if (drrFieldMaster[0]["UNCERTAIN_RAISABLE"].ToString().ToUpper() == "Y" || drrFieldMaster[0]["UNCERTAIN_LINKED_COLUMN"].ToString().Length > 0)
                        {
                            using (frmJobTitle_SelectionList objListwithHeader = new frmJobTitle_SelectionList())
                            {
                                Reload_PickList();
                                objListwithHeader.dtItems = dtPicklist.Select("picklistcategory = '" + drrFieldMaster[0]["PICKLIST_CATEGORY"].ToString() + "'").CopyToDataTable();
                                objListwithHeader.sHeaderColumn = "remarks";
                                objListwithHeader.sValueColumn = "picklistvalue";
                                objListwithHeader.TitleText = sCategoryValue;
                                objListwithHeader.ShowDialog();
                                if (objListwithHeader.DialogResult == System.Windows.Forms.DialogResult.OK)
                                {
                                    txt.Text = objListwithHeader.sReturnValue;
                                    txt.Text = objListwithHeader.sReturnValue;//Repeated to avoid triggering textbox change bringing grid value to textbox
                                }
                            }
                        }
                        else
                        {

                            using (frmComboList objFrmComboList = new frmComboList())//Custom Designed Combobox replacement
                            {
                                if (dtPickListData != null && dtPickListData.Rows.Count > 0)
                                {
                                    objFrmComboList.TitleText = sCategoryValue;
                                    objFrmComboList.dtItems = dtPickListData;
                                }
                                else
                                {
                                    objFrmComboList.TitleText = drrFieldMaster[0]["PICKLIST_CATEGORY"].ToString();
                                    objFrmComboList.dtItems = Load_PickList_Datatable(drrFieldMaster[0]["PICKLIST_CATEGORY"].ToString(), sValidation_For);
                                }
                                objFrmComboList.lstColumnsToDisplay.Add("PicklistValue");
                                objFrmComboList.sColumnToSearch = "PicklistValue";
                                objFrmComboList.sSearchValue = txt.Text;
                                objFrmComboList.IsSpellCheckEnabeld = (drrFieldMaster[0]["SPELLCHECK"].ToString() == "Y");
                                objFrmComboList.sSortColumn = drrFieldMaster[0]["PICKLIST_SORT"].ToString().Trim();
                                if (drrFieldMaster[0]["PICKLIST_SELECTION_TYPE"].ToString() == "MultiList")
                                {
                                    objFrmComboList.IsMultiSelect = true;
                                    objFrmComboList.IsSingleWordSelection = false;
                                }
                                else if (drrFieldMaster[0]["PICKLIST_SELECTION_TYPE"].ToString() == "SingleWord")//Single word selection overrides Multiselect property(Which only Single word can be selected from list)
                                {
                                    objFrmComboList.IsSingleWordSelection = true;
                                    objFrmComboList.IsMultiSelect = false;
                                }
                                else if (drrFieldMaster[0]["PICKLIST_SELECTION_TYPE"].ToString() == "MultiWord")
                                {
                                    objFrmComboList.IsSingleWordSelection = false;
                                    objFrmComboList.IsMultiSelect = false;
                                }

                                objFrmComboList.ShowDialog(this);

                                if (!string.IsNullOrEmpty(objFrmComboList.sReturn))
                                {
                                    txt.Text = objFrmComboList.sReturn;
                                    txt.Text = objFrmComboList.sReturn;//Repeated to avoid triggering textbox change bringing grid value to textbox
                                }
                            }
                        }

                    }
                    else
                        ToastNotification.Show(this, "Data Unavailable", eToastPosition.TopRight);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void Reload_PickList()
        {
            DateTime Picklist_DB_UpdateDate = Convert.ToDateTime(GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT PICKLIST_LASTUPDATE FROM c_project_settings WHERE PROJECT_ID='" + GV.sProjectID + "';").Rows[0][0]);
            if (Picklist_DB_UpdateDate > GV.PickList_LastUpdate)//Refresh Picklist data from DB based on Picklist last update date from ProjectSetting
            {
                DataRow[] drrPckListToRemove = dtPicklist.Select("remarks IN ('Accepted','Pending','Rejected')");

                foreach (DataRow drPckListToRemove in drrPckListToRemove)
                    drPckListToRemove.Delete();
                dtPicklist.AcceptChanges();
                DataTable dtPicklist_Updated = GV.MYSQL.BAL_FetchTableMySQL(GV.sProjectID + "_picklists", "remarks IN ('Accepted','Pending','Rejected')");
                dtPicklist.Merge(dtPicklist_Updated);
                GV.PickList_LastUpdate = GM.GetDateTime();
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnClear_Click(object sender, EventArgs e)//Runtime event.. //Button Clear Click for 'Customlist' controls
        {
            TextBox txt = sender as TextBox;//Gets which Textbox button triggers the event
            txt.Text = string.Empty;
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnUpdateEmailAllContacts_Click(object sender, EventArgs e)//Runtime event.. //Button Clear Click for 'Customlist' controls
        {
            if (EmailWorker.IsBusy)
            {
                ToastNotification.Show(this, "Previous verification in progress", eToastPosition.TopRight);
                return;
            }
            DevComponents.DotNetBar.Controls.TextBoxX txt = sender as DevComponents.DotNetBar.Controls.TextBoxX;
            if (txt.Text.Trim().Length > 0)
            {
                if (GM.Email_Check(txt.Text))
                {
                    EmailWorker.WorkerReportsProgress = false;
                    EmailWorker.WorkerSupportsCancellation = false;
                    sCheckingEmail = txt.Text;

                    EmailWorker.DoWork += delegate (object sender1, DoWorkEventArgs e1)
                    {
                        worker_DoWork(sender1, e1, txt);
                    };

                    EmailWorker.RunWorkerCompleted += delegate (object sender2, RunWorkerCompletedEventArgs e2)
                    {
                        worker_RunWorkerCompleted(sender2, e2, txt);
                    };
                    EmailWorker.RunWorkerAsync();



                    //string sPath = AppDomain.CurrentDomain.BaseDirectory + "\\Campaign Manager\\Email";
                    //if (!Directory.Exists(sPath))
                    //    Directory.CreateDirectory(sPath);

                    //DirectoryInfo Ddirectory = new DirectoryInfo(sPath);
                    //foreach (FileInfo fFile in Ddirectory.GetFiles()) fFile.Delete();

                    //////if (!(File.Exists(sPath + "\\" + sFileName)))
                    //////    File.WriteAllBytes(sPath + "\\" + sFileName, ByteFile);

                    //sPath += "\\EmailOut.txt";
                    //ProcessStartInfo pInfo = new ProcessStartInfo(@"D:\Email_check.exe", "\"" + txt.Text + "|" + sPath + "\"");
                    //pInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    //circularProgressEmail.Start();
                    //Process p = Process.Start(pInfo);
                    //p.WaitForExit();
                    //while (p.HasExited)
                    //{
                    //    ToastNotification.Show(this, System.IO.File.ReadAllText(sPath), eToastPosition.TopRight);
                    //    //circularProgressEmail.Stop();
                    //    break;
                    //}
                }
                else
                    ToastNotification.Show(this, "Incorrect Email format", eToastPosition.TopRight);
            }
            else
                ToastNotification.Show(this, "Email address empty", eToastPosition.TopRight);



            //mnuEmail.Show(Cursor.Position.X, Cursor.Position.Y);
            //TextBox txt = sender as TextBox;//Gets which Textbox button triggers the event
            //List<string> lstDomain = GetDomainList();
            //List<string> lstContactNameSplit = GetContactNameSplit();
            //string sEmail = string.Empty;
            //string sEmailNamePart = string.Empty;
            //string sEmailDomainPart = string.Empty;
            //string sEmailSyntax = string.Empty;
            //if (txt.Text.Length > 0)
            //{
            //    sEmail = txt.Text.Trim();
            //    sEmailNamePart = sEmail.Split('@').ToList()[0];
            //    sEmailDomainPart = sEmail.Split('@').ToList()[1];
            //}
            //        //FirstName                                 LastName
            //if ((lstContactNameSplit[0].Trim().Length > 0 || lstContactNameSplit[2].Trim().Length > 0) && lstDomain.Count > 0)
            //{
            //    //DataTable dtEmailSuggestion = dtPicklist.Select("PicklistCategory = 'EmailSuggestion'", "Remarks ASC").CopyToDataTable();
            //    foreach (DataRow dr in dtEmailSuggestion.Rows)
            //    {
            //        string sEmailFormation = string.Empty;
            //        sEmailFormation = dr["PicklistValue"].ToString().Replace("FirstName", lstContactNameSplit[0]);
            //        sEmailFormation = sEmailFormation.Replace("LastName", lstContactNameSplit[2]);
            //        sEmailFormation = sEmailFormation.Replace("FName", lstContactNameSplit[3]);
            //        sEmailFormation = sEmailFormation.Replace("LName", lstContactNameSplit[5]);
            //        sEmailFormation = sEmailFormation.Replace("MiddleName", lstContactNameSplit[1]);
            //        sEmailFormation = sEmailFormation.Replace("MName", lstContactNameSplit[4]);
            //        if (sEmailFormation.Trim().ToLower() == sEmailNamePart.ToLower())//Detect email format
            //        {
            //            sEmailSyntax = dr["PicklistValue"].ToString();
            //            break;
            //        }
            //    }

            //    frmBulkEmailUpdate objBulkEmailUpdate = new frmBulkEmailUpdate();
            //    objBulkEmailUpdate.lstContactNameSplit = lstContactNameSplit;
            //    objBulkEmailUpdate.lstDomain = lstDomain;
            //    objBulkEmailUpdate.sEmail = sEmail;
            //    objBulkEmailUpdate.sEmailSyntax = sEmailSyntax;
            //    objBulkEmailUpdate.dtEmailSugg = dtEmailSuggestion;
            //    objBulkEmailUpdate.dtContact = dtMasterContacts;
            //    objBulkEmailUpdate.lstFreezedContacts = lstFreezedContactIDs;
            //    objBulkEmailUpdate.ShowDialog();
            //    if (objBulkEmailUpdate.IsEmailUpdated)
            //    {
            //        dtMasterContacts = objBulkEmailUpdate.dtContact;
            //        txt.Text = dgvContacts.Rows[iGridRowIndex].Cells["CONTACT_EMAIL"].Value.ToString();
            //    }
            //}
            //else
            //    ToastNotification.Show(this, "No Name or Domain(s) found");
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnHangUp_Click(object sender, EventArgs e)//Runtime event.. //Button Dial Click for 'Customlist' controls
        {
            lblDialTimer.ForeColor = GV.pnlGlobalColor.Style.ForeColor.Color;
            if (Hangup())
                ToastNotification.Show(this, "Call Ended.", eToastPosition.TopRight);
            else
                ToastNotification.Show(this, "No Call found.", eToastPosition.TopRight);
        }

        void GetDialerHistory()
        {
            if (GV.sAccessTo == "TR")
            {
                //if (dtMasterCompanies.Rows[0]["SWITCHBOARD_TRIMMED"].ToString().Replace(" ", "").Length > 7)
                //{
                //    sSwitchboard_Trimmed = dtMasterCompanies.Rows[0]["SWITCHBOARD_TRIMMED"].ToString().Replace(" ", "");
                //    sSwitchboard_Trimmed = sSwitchboard_Trimmed.Substring(sSwitchboard_Trimmed.Length - 8);
                if (lstMasterIDs.Count > 1)

                    dtDialLogger = GV.MSSQL.BAL_ExecuteQuery("SELECT * FROM Call_Timesheet..AspectDialerLogger WHERE Company_ID IN (" + GM.ListToQueryString(lstMasterIDs, "Int") + ") AND ProjectName='" + GV.sProjectName + "';");
                else
                    dtDialLogger = GV.MSSQL.BAL_ExecuteQuery("SELECT * FROM Call_Timesheet..AspectDialerLogger WHERE Company_ID = " + sMaster_ID + " AND ProjectName='" + GV.sProjectName + "';");

                //dtDialLogger = GV.MSSQL.BAL_ExecuteQuery("SELECT * FROM Call_Timesheet..AspectDialerLogger WHERE Company_ID = " + sMaster_ID + " AND ProjectName='" + GV.sProjectName + "' AND convert(DATE,DateTimeStamp,112) BETWEEN convert(DATE,getdate()-3,112) AND convert(DATE,getdate(),112);");

                //dtDialLogger = GV.MSSQL.BAL_ExecuteQuery("SELECT * FROM  Call_Timesheet..AspectDialerLogger WHERE TelephoneNumber IN ('84920899270','849525115810','8493027907460','8496113480','8498923708080') ORDER BY DateTimeStamp");
                //dtDialLogger = GV.MSSQL.BAL_ExecuteQuery("SELECT top 10 * FROM  Call_Timesheet..AspectDialerLogger WHERE TelephoneNumber IN ('8442088592042') ORDER BY DateTimeStamp desc");
                Reload_DialerGrid();
                //  }
            }
            else
                rbnBarDial.Visible = false;
        }

        void Reload_DialerGrid()
        {
            sdgvCallLog.PrimaryGrid.Rows.Clear();
            if (dtDialLogger.Rows.Count > 0)
            {

                txtTotalDials.Invoke((MethodInvoker)delegate { txtTotalDials.Text = dtDialLogger.Select("RecordingID = 0").Length.ToString(); });
                //txtTotalDials.Text = dtDialLogger.Select("RecordingID = 0").Length.ToString();

                for (int i = dtDialLogger.Rows.Count - 1; i > 0; i--)
                {
                    if (dtDialLogger.Rows[i]["RecordingID"].ToString() == "1")
                    {
                        //GridRow GR = new GridRow("<font color = 'Gray'>" + GM.ProperCase_ProjectSpecific(dtDialLogger.Rows[i]["LoginID"].ToString()) + "</font>", "<font color = 'Gray'>"
                        //+ dtDialLogger.Rows[i]["Duration"].ToString().Substring(dtDialLogger.Rows[i]["Duration"].ToString().Length - 5) + "</font>", "<font color = 'Gray'>" + dtDialLogger.Rows[i]["DateTimeStamp"].ToString().Substring(0, 10).Replace("/","-") + "</font>");

                        //GridRow GR = new GridRow("<font color = 'Gray'><i>" + dtDialLogger.Rows[i]["Duration"].ToString().Substring(dtDialLogger.Rows[i]["Duration"].ToString().Length - 5) + "</i>&nbsp;&nbsp;&nbsp;" + dtDialLogger.Rows[i]["DateTimeStamp"].ToString().Substring(0, 10).Replace("/", "-") + "&nbsp;&nbsp;&nbsp;" + GM.ProperCase_ProjectSpecific(dtDialLogger.Rows[i]["LoginID"].ToString()) + "</font>");


                        //GridRow GR = new GridRow(Properties.Resources.Devices_audio_headset_icon, "<font color = 'Gray'><i>" + dtDialLogger.Rows[i]["Duration"].ToString().Substring(dtDialLogger.Rows[i]["Duration"].ToString().Length - 5) + "</i>&nbsp;&nbsp;&nbsp;" + dtDialLogger.Rows[i]["DateTimeStamp"].ToString().Substring(0, 10).Replace("/", "-") + "</font>");
                        GridRow GR = new GridRow(Properties.Resources.Devices_audio_headset_icon, "<font color = 'Gray'><i>" + dtDialLogger.Rows[i]["Duration"].ToString().Substring(dtDialLogger.Rows[i]["Duration"].ToString().Length - 5) + "</i>&nbsp;&nbsp;&nbsp;" + GM.TimeAgo(Convert.ToDateTime(dtDialLogger.Rows[i]["DateTimeStamp"])) + "</font>");
                        sdgvCallLog.PrimaryGrid.Rows.Add(GR);
                    }
                }
                controlContainerCallLog.Invoke((MethodInvoker)delegate { controlContainerCallLog.Visible = sdgvCallLog.PrimaryGrid.Rows.Count > 0; });
                //controlContainerCallLog.Visible = sdgvCallLog.PrimaryGrid.Rows.Count > 0;

            }
            else
            {
                rbnBarDial.Invoke((MethodInvoker)delegate { rbnBarDial.DialogLauncherVisible = false; });
                txtTotalDials.Invoke((MethodInvoker)delegate { txtTotalDials.Text = "0"; });
                controlContainerCallLog.Invoke((MethodInvoker)delegate { controlContainerCallLog.Visible = false; });

                //txtTotalDials.Text = "0";
                //controlContainerCallLog.Visible = false;
                //rbnBarDial.DialogLauncherVisible = false;
            }

            if (controlContainerCallLog.Visible)
            {
                rbnBarDial.Invoke((MethodInvoker)delegate { rbnBarDial.Width = 380; rbnBarDial.DialogLauncherVisible = true; });
                //rbnBarDial.Width = 380;
                //rbnBarDial.DialogLauncherVisible = true;
            }
            else
            {
                rbnBarDial.Invoke((MethodInvoker)delegate { rbnBarDial.Width = 135; rbnBarDial.DialogLauncherVisible = false; });
                //rbnBarDial.Width = 135;
                //rbnBarDial.DialogLauncherVisible = false;
            }
            rbnBarDial.Invoke((MethodInvoker)delegate { rbnBarDial.Refresh(); });
            //rbnBarDial.Refresh();            
        }

        bool Hangup()
        {
            if (CallinProgress)
            {
                if(GV.sDialerType == "Vortex" && txtTotalDials.Caption == "Input")
                {
                    txtTotalDials.Caption = "Dials.";
                }                
                GM.HangUp();
                //GV.MSSQL.BAL_ExecuteQuery("INSERT INTO Call_Timesheet..AspectDialerLogger (AgentName,LoginID,StationID,TelephoneNumber,RecordingID,Duration,DateTimeStamp,ClientName,ProjectName,CampaignID,Company_ID)VALUES('" + GV.sEmployeeName + " - " + GV.sEmployeeNo + "','" + GV.sEmployeeName + "','" + GV.sExtensionNumber + "','" + sCurrentCallingNo + "',1,'" + lblDialTimer.Text + "', GetDate(),'" + GV.sClientName + "','" + GV.sProjectName + "','GCC - 27'," + sMaster_ID + ");");
                Insert_DialerLogger(1);
                GV.MYSQL.BAL_ExecuteNonReturnQueryMySQL("INSERT INTO " + GV.sProjectID + "_log (RecordID, CompanySessionID, TableName, FieldName, OldValue, NewValue, `When`, Who, SystemName) VALUES (" + sMaster_ID + ",'" + GV.sCompanySessionID + "','CALL_HANGUP','" + GV.sDialerType + "', '" + GV.sExtensionNumber + "','" + sCurrentCallingNo + "',NOW(),'" + GV.sEmployeeName + "','" + Environment.MachineName + "');");
                lblDialTimer.Invoke((MethodInvoker)delegate { lblDialTimer.Text = "00:00:00"; });
                lblPrePostDuration.Invoke((MethodInvoker)delegate { lblPrePostDuration.Text = "00:00:00"; });
                //lblDialTimer.Text = "00:00:00";
                sCurrentCallingNo = string.Empty;

                pictureBoxDialImage.Invoke((MethodInvoker)delegate { pictureBoxDialImage.Enabled = false; });
                //pictureBoxDialImage.Enabled = false;

                tCall.Enabled = false;
                CallinProgress = false;
                controlContainerIPrePostCall.Visible = true;
                lblPrePostDuration.Visible = true;
                rbnBarDial.Refresh();
                return true;
            }
            return false;
        }

        bool Is_CallAllowed()
        {

            DataRow[] drrDialConfig = dtDialConfig.Select("PicklistCategory = 'Dial_Restrict_CallCount'");
            if (drrDialConfig.Length > 0)
            {
                int iCallDuration = Convert.ToInt32(dtDialConfig.Select("PicklistCategory = 'Dial_Restrict_CallDuration'")[0]["PicklistValue"]);
                foreach (DataRow drDialConfig in drrDialConfig)
                {
                    int iCallCount = Convert.ToInt32(drDialConfig["PicklistValue"]);
                    int iDayLimit = Convert.ToInt32(drDialConfig["PicklistField"]);

                    if (GM.GetDateTime().DayOfWeek.ToString() == "Monday")
                        iDayLimit += 2;

                    DataRow[] drrLastDialer = null;

                    if (lstMasterIDs.Count > 1)
                        drrLastDialer = dtDialLogger.Select("RecordingID = 1 AND TelephoneNumber = '" + sCurrentCallingNo + "' AND DATETIMESTAMP > '" + GM.GetDateTime().AddDays(-iDayLimit).ToString("dd/MM/yyyy") + " 23:59:59'");
                    else
                        drrLastDialer = dtDialLogger.Select("RecordingID = 1 AND DATETIMESTAMP > '" + GM.GetDateTime().AddDays(-iDayLimit).ToString("dd/MM/yyyy") + " 23:59:59'");

                    if (drrLastDialer.Length > 0)
                    {
                        int iDialCount = 0;
                        foreach (DataRow drCall in drrLastDialer)
                        {
                            if (TimeSpan.Parse(drCall["Duration"].ToString()).TotalSeconds > iCallDuration)
                                iDialCount++;
                        }

                        if (iDialCount >= iCallCount)
                            return false;
                    }
                }
            }

            return true;
        }

        string Block_Call(string sMatchValue)
        {
            CL.RefreshBlockTable(false);
            DataRow[] drrBlock = CL.dtBlock.Select("BLOCK_TYPE = 'CALL'");
            string sBlock_Message = string.Empty;
            foreach (DataRow drBlock in drrBlock)
            {
                string s = Block_Match(sMatchValue, drBlock);
                if (s.Length > 0)
                    sBlock_Message += s + "<br/>";
            }
            return sBlock_Message;
        }

        string Block_Match(string sMatchValue, DataRow drBlock)
        {
            switch (drBlock["MATCH_TYPE"].ToString().ToUpper())
            {
                case "EXACT":
                    if (sMatchValue.ToUpper() == drBlock["VALUE"].ToString().Trim().ToUpper())
                    {
                        if (drBlock["CUSTOM_MESSAGE"].ToString().Length > 0)
                            return drBlock["CUSTOM_MESSAGE"].ToString();
                        else
                            return drBlock["FIELD"].ToString().Replace("_", " ") + " : " + sMatchValue + " is blacklisted";
                    }
                    break;

                case "CONTAINS":
                    if (sMatchValue.ToUpper().Contains(drBlock["VALUE"].ToString().Trim().ToUpper()))
                    {
                        if (drBlock["CUSTOM_MESSAGE"].ToString().Length > 0)
                            return drBlock["CUSTOM_MESSAGE"].ToString();
                        else
                            return drBlock["FIELD"].ToString().Replace("_", " ") + " : " + sMatchValue + " is blacklisted";
                    }
                    break;

                case "STARTSWITH":
                    if (sMatchValue.ToUpper().StartsWith(drBlock["VALUE"].ToString().Trim().ToUpper()))
                    {
                        if (drBlock["CUSTOM_MESSAGE"].ToString().Length > 0)
                            return drBlock["CUSTOM_MESSAGE"].ToString();
                        else
                            return drBlock["FIELD"].ToString().Replace("_", " ") + " : " + sMatchValue + " is blacklisted";
                    }
                    break;

                case "ENDSWITH":
                    if (sMatchValue.ToUpper().EndsWith(drBlock["VALUE"].ToString().Trim().ToUpper()))
                    {
                        if (drBlock["CUSTOM_MESSAGE"].ToString().Length > 0)
                            return drBlock["CUSTOM_MESSAGE"].ToString();
                        else
                            return drBlock["FIELD"].ToString().Replace("_", " ") + " : " + sMatchValue + " is blacklisted.";
                    }
                    break;

                case "SWITCHBOARD":
                    sMatchValue = Regex.Replace(sMatchValue, @"\D*", string.Empty);
                    if (sMatchValue == drBlock["VALUE"].ToString().Trim())
                    {
                        if (drBlock["CUSTOM_MESSAGE"].ToString().Length > 0)
                            return drBlock["CUSTOM_MESSAGE"].ToString();
                        else
                            return drBlock["FIELD"].ToString().Replace("_", " ") + " : " + sMatchValue + " is blacklisted";
                    }
                    break;

                case "SWITCHBOARDSTARTS":
                    sMatchValue = Regex.Replace(sMatchValue, @"\D*", string.Empty);
                    if (sMatchValue.StartsWith(drBlock["VALUE"].ToString().Trim()))
                    {
                        if (drBlock["CUSTOM_MESSAGE"].ToString().Length > 0)
                            return drBlock["CUSTOM_MESSAGE"].ToString();
                        else
                            return drBlock["FIELD"].ToString().Replace("_", " ") + " : " + sMatchValue + " is blacklisted";
                    }
                    break;

                case "SWITCHBOARDENDS":
                    sMatchValue = Regex.Replace(sMatchValue, @"\D*", string.Empty);
                    if (sMatchValue.EndsWith(drBlock["VALUE"].ToString().Trim()))
                    {
                        if (drBlock["CUSTOM_MESSAGE"].ToString().Length > 0)
                            return drBlock["CUSTOM_MESSAGE"].ToString();
                        else
                            return drBlock["FIELD"].ToString().Replace("_", " ") + " : " + sMatchValue + " is blacklisted";
                    }
                    break;
            }
            return string.Empty;
        }

        string FormatTelephoneToDial(string sTelephone)
        {

            sTelephone = Regex.Replace(sTelephone, @"\D*", string.Empty);

            // Login is important and has to be done previous to login to iDialer.
            if (sTelephone.StartsWith("011"))
                sTelephone = sTelephone.Substring(3);
            if (sTelephone.StartsWith("00"))
                sTelephone = sTelephone.Substring(2);
            if (sTelephone.StartsWith("0"))
                sTelephone = sTelephone.Substring(1);
            if (!sTelephone.StartsWith("8"))
                sTelephone = "8" + sTelephone;
            // Append 8 before the number - assumption is that we have country code prefixed the number.                    


            return sTelephone;
        }

        //-----------------------------------------------------------------------------------------------------
        public void btnDial_Click(object sender, EventArgs e)//Runtime event.. //Button Dial Click for 'Customlist' controls
        {
            if (CallinProgress)
                ToastNotification.Show(this, "Call in progress.", eToastPosition.TopRight);
            else
            {
                lblDialTimer.ForeColor = GV.pnlGlobalColor.Style.ForeColor.Color;
                CallLimitExceeded = false;
                sCurrentCallingNo = string.Empty;
                string sDialNumber = string.Empty;
                lblDialTimer.Text = "00:00:00";

                bool TimeZoneFail = false;

                if (!IsTimeZoneEnabled && Convert.ToDouble(tCurrentCountryTime.ToString("HH")) < 8 || Convert.ToDouble(tCurrentCountryTime.ToString("HH")) > 17)
                {
                    if (GV.TimeEnabled)
                    {
                        ToastNotification.Show(this, "Out of Time Zone", eToastPosition.TopRight);
                        return;
                    }
                    else
                        TimeZoneFail = true;
                }

                if (((GV.sDialerType == "iSystem" || GV.sDialerType == "X-Lite") && Regex.Replace(GV.sExtensionNumber, @"\D*", string.Empty).Length >= 4) || ((GV.sDialerType == "Vortex") && Regex.Replace(GV.sVortexExtension, @"\D*", string.Empty).Length >= 4))
                {
                    TextBox txt = sender as TextBox;//Gets which Textbox button triggers the event                
                    if (txt == null)
                        sDialNumber = txtDialMain.Text.Trim();
                    else
                        sDialNumber = txt.Text.Trim();

                    string sBlockMessage = Block_Call(sDialNumber);
                    if (sBlockMessage.Length > 0)
                    {
                        ToastNotification.ToastForeColor = Color.Red;
                        ToastNotification.Show(this, sBlockMessage, eToastPosition.TopRight);
                        ToastNotification.ToastForeColor = Color.White;
                        return;
                    }

                    sCurrentCallingNo = FormatTelephoneToDial(sDialNumber);

                    if (sCurrentCallingNo.Length < 7 || sCurrentCallingNo.Length > 16)
                    {
                        ToastNotification.Show(this, "Invalid Number", eToastPosition.TopRight);
                        return;
                    }
                }
                else
                {
                    ToastNotification.Show(this, "Invalid Extension", eToastPosition.TopRight);
                    return;
                }


                if (!Is_CallAllowed())
                {
                    CallLimitExceeded = true;
                    //ToastNotification.Show(this, "Call not allowed." + Environment.NewLine + " No. of calls to this company exceeded.",5000, eToastPosition.TopRight);
                    ToastNotification.Show(this, "No. of calls to this company exceeded.", 5000, eToastPosition.BottomCenter);
                    //return;
                }

                if (GV.sDialerType == "iSystem")
                {
                    Process[] iSystemProcess = Process.GetProcessesByName("iSystems 3.0");
                    if (iSystemProcess.Length > 0)
                    {
                        //try
                        //{                            
                        //    White.Core.Application _application = White.Core.Application.Attach(Process.GetProcessesByName("iSystems 3.0")[0].Id);
                        //    White.Core.UIItems.WindowItems.Window  _mainWindow = _application.GetWindow(SearchCriteria.ByText("iSystem Panel"), InitializeOption.NoCache);
                        //    string sIsystemExt = ((White.Core.UIItems.ListBoxItems.Win32ComboBox)_mainWindow.Items[6]).SelectedItemText;

                        //    if (GV.sExtensionNumber.ToLower() != sIsystemExt.ToLower())
                        //    {
                        //        ToastNotification.Show(this, "iSystem extension not matching" + Environment.NewLine + "with Campaign Manager extension.", 5000, eToastPosition.TopRight);
                        //        return;
                        //    }

                        //}
                        //catch (Exception ex) { }

                    }
                    else
                    {
                        ToastNotification.Show(this, "iSystem not running." + Environment.NewLine + "Please start iSystem and redial.", 5000, eToastPosition.TopRight);
                        return;
                    }

                }
                else if (GV.sDialerType == "X-Lite")
                {
                    Process[] xLiteProcess = Process.GetProcessesByName("X-Lite");
                    if (xLiteProcess.Length == 0)
                    {
                        ToastNotification.Show(this, "X-Lite not running." + Environment.NewLine + "Starting X-Lite now. Please redial once X-Lite is initialized.", 5000, eToastPosition.TopRight);
                        if (File.Exists(@"C:\Program Files\CounterPath\X-Lite\x-lite.exe"))//32 and 64
                            Process.Start(@"C:\Program Files\CounterPath\X-Lite\x-lite.exe");
                        else if (File.Exists(@"C:\Program Files (x86)\CounterPath\X-Lite\x-lite.exe"))//64
                            Process.Start(@"C:\Program Files (x86)\CounterPath\X-Lite\x-lite.exe");
                        else
                            ToastNotification.Show(this, "X-Lite not installed.", eToastPosition.TopRight);
                        return;
                    }
                }
                else if (GV.sDialerType == "Vortex")
                {
                    if (DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice).Count() == 0)
                    {
                        ToastNotification.Show(this, "No Audio device found.", eToastPosition.TopRight);
                        return;
                    }

                    if (!GV.VorteX.IsConnected)
                    {
                        if (GV.VorteX.SoftPhoneState.Length == 0)
                        {
                            try
                            {
                                GV.VorteX.SupportVox = true;
                                GV.VorteX.SupportiDialer = false;
                                string sVortexStatus = GV.VorteX.Connect(GV.IP, GV.sSoftwareVersion, false);
                                if (sVortexStatus.Length == 0)
                                {
                                    ToastNotification.Show(this, "Initializing Vortex..", eToastPosition.TopRight);
                                    PhoneNotiier("InitVort");
                                }
                                else
                                {
                                    ToastNotification.Show(this, "Vortex Connection failed..", eToastPosition.TopRight);
                                    PhoneNotiier("Connection");
                                }
                                return;
                            }
                            catch (Exception ex)
                            {
                                ToastNotification.Show(this, "Dialer Connection failed.", eToastPosition.TopRight);
                                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);
                                return;
                            }
                        }
                        else
                        {
                            ToastNotification.Show(this, "Vortex Busy", eToastPosition.TopRight);
                            return;
                        }
                    }
                }

                if (txtTimeZone.TextBox.BackColor == Color.FromArgb(0xFF, 0x99, 0x99))
                {
                    if (GV.TimeEnabled)
                    {
                        if (DialogResult.Yes == MessageBoxEx.Show("This Country is out of Time Zone." + Environment.NewLine + "Do you still wish to call?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                            CallinProgress = GM.Dial(sCurrentCallingNo,sMaster_ID);
                        else
                            return;
                    }
                    else
                        CallinProgress = GM.Dial(sCurrentCallingNo, sMaster_ID);

                    //{                        
                    //    CallinProgress = GM.Dial(sCurrentCallingNo);
                    //}
                    //else
                    //    return;
                }
                else
                    CallinProgress = GM.Dial(sCurrentCallingNo, sMaster_ID);


                if (CallinProgress)
                {
                    //string sSQL = "INSERT INTO ASPECTDIALERLOGGER VALUES ('" + txtEmpName.Text + "', '" + EmployeeName + "', '" + Extension + "', '" + TelNo + "'," + Id.ToString() + " , '" + lblDuration.Text + "', GetDate(), '', '" + ClientName + "', '" + ProjectName + "', '" + cmbCampaignID.Text + "')";
                    //string sSQL = "INSERT INTO AspectDialerLogger (AgentName,LoginID,StationID,TelephoneNumber,RecordingID,Duration,DateTimeStamp,ClientName,ProjectName,CampaignID)VALUES('" + GV.sEmployeeName + " - " + GV.sEmployeeNo + "','" + GV.sEmployeeNo + "','" + GV.sExtensionNumber + "','" + sTeleno + "',0,'00:00:00','" + GM.GetDateTime() + "','Client','" + GV.sProjectName + "','GCC - 27');";
                    if (TimeZoneFail)
                        GV.MYSQL.BAL_ExecuteQueryMySQL("INSERT INTO timezone_override_calls (PROJECT_ID, COMPANY_ID, WHO, `WHEN`, TELEPHONE, ADDRESS, CITY, STATE, COUNTRY) VALUES ('" + GV.sProjectID + "'," + sMaster_ID + ",'" + GV.sEmployeeName + "', '" + GM.GetDateTime().ToString("yyyy-MM-dd hh:mm:ss") + "', '" + sCurrentCallingNo + "', '" + dtMasterCompanies.Rows[iCompanyRowIndex]["ADDRESS_1"].ToString().Replace("'", "''") + ", " + dtMasterCompanies.Rows[iCompanyRowIndex]["ADDRESS_2"].ToString().Replace("'", "''") + ", " + dtMasterCompanies.Rows[iCompanyRowIndex]["ADDRESS_3"].ToString().Replace("'", "''") + "' ,'" + dtMasterCompanies.Rows[iCompanyRowIndex]["CITY"].ToString().Replace("'", "''") + "','" + dtMasterCompanies.Rows[iCompanyRowIndex]["COUNTY"].ToString().Replace("'", "''") + "','" + dtMasterCompanies.Rows[iCompanyRowIndex]["COUNTRY"].ToString().Replace("'", "''") + "')");

                    GM.Moniter("Call in Progress");
                    Insert_DialerLogger(0);
                    GV.MYSQL.BAL_ExecuteNonReturnQueryMySQL("INSERT INTO " + GV.sProjectID + "_log (RecordID, CompanySessionID, TableName, FieldName, OldValue, NewValue, `When`, Who, SystemName) VALUES (" + sMaster_ID + ",'" + GV.sCompanySessionID + "','CALL_INITIATE','" + GV.sDialerType + "', '" + GV.sExtensionNumber + "','" + sCurrentCallingNo + "',NOW(),'" + GV.sEmployeeName + "','" + Environment.MachineName + "');");
                    pictureBoxDialImage.Enabled = true;
                    tCall.Enabled = true;
                    ToastNotification.Show(this, "Dialing " + sDialNumber, eToastPosition.TopRight);
                }
                else
                    ToastNotification.Show(this, "Call Failed." ,eToastPosition.TopRight);

            }
        }

        void Insert_DialerLogger(int iRecordingID)
        {

            GV.MSSQL.BAL_ExecuteQuery("INSERT INTO Call_Timesheet..AspectDialerLogger (AgentName,LoginID,StationID,TelephoneNumber,RecordingID,Duration,DateTimeStamp,ClientName,ProjectName,CampaignID,Company_ID) VALUES ('" + GV.sEmployeeName + " - " + GV.sEmployeeNo + "','" + GV.sEmployeeName + "','" + GV.sExtensionNumber + "','" + sCurrentCallingNo + "'," + iRecordingID + ",'" + lblDialTimer + "', GetDate(),'" + GV.sClientName + "','" + GV.sProjectName + "','GCC - 27'," + sMaster_ID + ");");
            DataRow drNewDial = dtDialLogger.NewRow();
            drNewDial["LoginID"] = GV.sEmployeeName;
            drNewDial["StationID"] = GV.sExtensionNumber;
            drNewDial["TelephoneNumber"] = sCurrentCallingNo;
            drNewDial["RecordingID"] = iRecordingID;
            drNewDial["Duration"] = lblDialTimer.Text;
            drNewDial["DateTimeStamp"] = GM.GetDateTime();
            dtDialLogger.Rows.Add(drNewDial);
            dtDialLogger.AcceptChanges();
            Reload_DialerGrid();
        }



        private void tCall_Tick(object sender, EventArgs e)
        {

            DateTime dDate1 = default(DateTime);
            dDate1 = Convert.ToDateTime(lblDialTimer.Text);
            dDate1 = dDate1.AddSeconds(1);
            //if (dDate1.Hour == 0)
            //    lblDialTimer.Text = dDate1.ToString("mm:ss");
            //else
            lblDialTimer.Text = dDate1.ToString("HH:mm:ss");

            if (CallLimitExceeded)
            {
                if (lblDialTimer.ForeColor == GV.pnlGlobalColor.Style.ForeColor.Color)
                    lblDialTimer.ForeColor = Color.Red;
                else
                    lblDialTimer.ForeColor = GV.pnlGlobalColor.Style.ForeColor.Color;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnWebsite_Click(object sender, EventArgs e)//Runtime event.. //Button Dial Click for 'Customlist' controls
        {
            TextBox txt = sender as TextBox;//Gets which Textbox button triggers the event
            if (txt.Text.Length > 0)
            {
                if (GM.Web_Check(txt.Text))
                    Process.Start(txt.Text.Trim());
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private string sSplitLenghtString(string sSplitString, int iContainerWidth, Font f)
        {
            string[] sWords = sSplitString.Split(' ');
            StringBuilder sb = new StringBuilder();
            iContainerWidth -= 50;
            string x = string.Empty;
            foreach (string sSplitWord in sWords)
            {
                x = x + " " + sSplitWord;
                if (iContainerWidth > TextRenderer.MeasureText(x, f).Width)
                {
                    sb.AppendFormat(" {0}", sSplitWord);
                }
                else
                {
                    sb.AppendFormat("{0}{1}", Environment.NewLine, sSplitWord);
                    x = sSplitWord;
                }
            }

            //int iCurrLength = 0;
            //foreach (string sSplitWord in sWords)
            //{
            //    if (iCurrLength + sSplitWord.Length + 1 < 45) // +1 accounts for adding a space
            //    {
            //        sb.AppendFormat(" {0}", sSplitWord);
            //        iCurrLength = (sb.Length % 45);
            //    }
            //    else
            //    {
            //        sb.AppendFormat("{0}{1}", Environment.NewLine, sSplitWord);
            //        iCurrLength = 0;
            //    }
            //}
            return sb.ToString();
        }

        //-----------------------------------------------------------------------------------------------------
        private List<Control> AddLabelControl(string sCaption, string sFieldName, List<Control> ctrl)//Adds Label to all Controls
        {
            if (sCaption.Length > 0)
            {
                Label lbl = new Label() { Name = "lbl" + sFieldName, Text = sCaption, AutoSize = true, BackColor = Color.Transparent };
                lbl.Font = new Font(lbl.Font.FontFamily, 10);
                lbl.SendToBack();
                //lbl.ForeColor = Color.FromArgb(0x3B, 0x3B, 0x5E);
                ctrl.Add(lbl);
            }
            return ctrl;
        }
        //private void Load_MasterContactsGrid(DataTable dtMasterContactsGrid, DataTable dtMasterContactsFormatGrid)//Load Master Contact datagrid
        //{
        //    for (int i = 0; i < dgvContacts.Columns.Count; i++)
        //    {
        //        DataRow[] drMasterContacts = dtMasterContactsFormatGrid.Select(String.Format("FIELD_NAME_TABLE = '{0}'", dgvContacts.Columns[i].Name));
        //        if (drMasterContacts.Length > 0)
        //        {
        //           dgvContacts.Columns[i].HeaderText = drMasterContacts[0]["FIELD_NAME_CAPTION"].ToString();
        //        }
        //        else
        //            dgvContacts.Columns[i].Visible = false;
        //    }
        //}


        //-----------------------------------------------------------------------------------------------------
        private List<string> Load_PickList(string sPickListCategory)
        {
            //Loads Picklist
            List<string> lstPickList = new List<string>();
            try
            {
                if (sPickListCategory == "EmailSuggestion")
                {
                    DataTable dtEmails = EmailSuggetionsGeneration();
                    if (dtEmails != null && dtEmails.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtEmails.Rows)
                        {
                            lstPickList.Add(dr["PicklistValue"].ToString().ToLower());
                        }
                    }
                }
                else
                {
                    //DataTable dtPicklist = objDALGeneral.ExecuteQuery(String.Format("SELECT * FROM dbo.{0}_PICKLISTS WHERE PicklistCategory = '{1}'", sProjectID, sPickListCategory));
                    DataRow[] drPicklist = dtPicklist.Select(String.Format("PicklistCategory = '{0}'", sPickListCategory.Replace("'", "''")));

                    foreach (DataRow drList in drPicklist)
                    {
                        lstPickList.Add(drList["PicklistValue"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return lstPickList;
        }

        //-----------------------------------------------------------------------------------------------------
        private DataTable Load_PickList_Datatable(string sPickListCategory, string sFieldName)
        {
            try
            {
                //Loads Picklist

                switch (sPickListCategory.ToUpper())
                {
                    case "EMAILSUGGESTION":
                        return EmailSuggetionsGeneration();

                    case "CONTROLS":
                        {
                            DataTable dt = dtUncertainFields.Copy();
                            dt.Columns["FieldName"].ColumnName = "Picklistvalue";
                            return dt;
                        }

                    case "EAF_GETLIST":
                        var objReturn = EAF_Call("GetList", sFieldName);
                        try
                        {
                            DataTable dtReturn = (DataTable)objReturn;
                            if (dtReturn.TableName == "Message")
                            {
                                if (dtReturn.Rows.Count > 0)
                                    MessageBoxEx.Show(dtReturn.Rows[0][1].ToString(), "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return null;
                            }
                            else
                                return dtReturn;
                        }
                        catch (Exception e)
                        { return null; }

                    case "EAF_GETLIST_C"://Includes Updated Company Data
                        COMPANY_CollectDataFromControl();
                        var objReturn_C = EAF_Call("GetList", sFieldName);
                        try
                        {

                            DataTable dtReturn_C = (DataTable)objReturn_C;
                            if (dtReturn_C.TableName == "Message")
                            {
                                if (dtReturn_C.Rows.Count > 0)
                                    MessageBoxEx.Show(dtReturn_C.Rows[0][1].ToString(), "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return null;
                            }
                            else
                                return dtReturn_C;
                        }
                        catch (Exception e)
                        { return null; }

                    case "TR_CONTACTSTATUS":
                    case "WR_CONTACTSTATUS":
                        return ContactStatus_Selection();



                    default:
                        DataRow[] drPicklist = dtPicklist.Select(String.Format("PicklistCategory = '{0}'", sPickListCategory.Replace("'", "''")));
                        if (drPicklist.Length > 0)
                            return drPicklist.CopyToDataTable();
                        else
                            return null;
                }

                //if (sPickListCategory == "EmailSuggestion")
                //{
                //    return EmailSuggetionsGeneration();
                //}
                //else if (sPickListCategory == "EAF_GetList")
                //{
                //    var objReturn =   EAF_Call("GetList", sFieldName);
                //    try
                //    {
                //        DataTable dtReturn = (DataTable)objReturn;
                //        return dtReturn;
                //    }
                //    catch (Exception e)
                //    { return null; }
                //}
                //else
                //{
                //    DataRow[] drPicklist = dtPicklist.Select(String.Format("PicklistCategory = '{0}'", sPickListCategory));
                //    if (drPicklist.Length > 0)
                //        return drPicklist.CopyToDataTable();
                //    else
                //        return null;
                //}

            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
        }

        void FreezeEmptyContact()
        {
            if (dtMasterContacts.Select("MASTER_ID = " + sMaster_ID).Length == 0)//Select first row by default
            {
                foreach (TextBox txt in lstContactControls)//if contact list empty then disable the controls related to contacts
                    txt.Enabled = false;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void Load_MasterContacts()
        {
            try
            {
                //Populate Mastercontacts fields
                if (dtFieldMasterContact != null && dtFieldMasterContact.Rows.Count > 0)
                {
                    //Load controls from datatable to list
                    List<Control> ctrlsToLoad = new List<Control>();
                    ctrlsToLoad = Load_Controls(dtFieldMasterContact);
                    AlignControls(ctrlsToLoad, GetDisplaySize(ctrlsToLoad), sTabPanelContacts);//Aligns the Controls
                    //lstMasterContactControls = GetAllControls(sTabPanelContacts); //Gets all loaded master Contacts controls to a list
                    lstContactControls = new List<TextBox>();

                    foreach (Control C in ctrlsToLoad)
                    {
                        if (C is TextBox)
                            lstContactControls.Add(C as TextBox);
                    }
                }

                //Load Mastar contact datagrid
                dtMasterContactsCopy = dtMasterContacts.Copy();//Disconnected datatable(No link with grid)
                dtMasterContactsCopy.TableName = "MasterContactCopy";

                // dgvContacts.DataSource = dtMasterContacts;

                LoadSuperGridContact();

            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        //-----------------------------------------------------------------------------------------------------
        private void ContactBounce_AND_CompanyContactFreeze()
        {
            try
            {
                lstFreezedContactIDs.Clear();
                lstBouncedContactIDs.Clear();
                lstRecordsToUnlock.Clear();
                lstSendBackRecords.Clear();
                lstFreezedMasterIDs.Clear();
                lstQCOKIDs.Clear();

                bool IsRecordsMoved = dtMasterCompanies.Rows[0]["MOVED_TO_TR_DATE"].ToString().Length > 0;

                //List<int> lstFreezeRecs = new List<int>();
                //foreach (DataRow drQC in dtQCTable.Rows)
                //{
                //    if (drQC["TABLENAME"].ToString() == "Contact" && drQC["QC_STATUS"].ToString().ToUpper() == "OK")
                //    {
                //        if (drQC["ResearchType"].ToString() == "TR" || (drQC["ResearchType"].ToString() == "WR" && (!IsRecordsMoved || GV.sFreezeWRCompletedRecords == "Y")))
                //            lstFreezeRecs.Add(Convert.ToInt32(drQC["RecordID"]));                        
                //    }
                //}

                foreach (DataRow drQC in dtQCTable.Rows)
                {
                    if (drQC["TABLENAME"].ToString().ToUpper() == "CONTACT")
                    {
                        if (drQC["QC_STATUS"].ToString().ToUpper() == "OK")
                        {
                            if (drQC["ResearchType"].ToString().ToUpper() == "TR" || (drQC["ResearchType"].ToString().ToUpper() == "WR" && (!IsRecordsMoved || GV.sFreezeWRCompletedRecords == "Y")))
                                lstQCOKIDs.Add(Convert.ToInt32(drQC["RECORDID"]));
                        }
                        else if (drQC["QC_STATUS"].ToString().ToUpper() == "SENDBACK")
                            lstSendBackRecords.Add(Convert.ToInt32(drQC["RecordID"]));//Records to be unlocked if locked
                        else if (drQC["QC_STATUS"].ToString().ToUpper().StartsWith("REJECTION"))
                            lstRejectedRecords.Add(Convert.ToInt32(drQC["RECORDID"]));
                    }
                }

                lstQCOKIDs = lstQCOKIDs.Distinct().ToList();

                DataRow[] drrBounced = dtMasterContacts.Select("EMAIL_VERIFIED IN('BOUNCED','NOT VERIFIED')");
                if (drrBounced.Length > 0)
                {
                    foreach (DataRow drBounced in drrBounced)
                        lstBouncedContactIDs.Add(Convert.ToInt32(drBounced["CONTACT_ID_P"]));//Records to be unlocked if locked
                }

                //DataRow[] drrSendBack = dtQCTable.Select("QC_Status = 'SendBack' AND TableName='Contact'");
                //if (drrSendBack.Length > 0)
                //{
                //    foreach (DataRow drSendBack in drrSendBack)
                //        lstSendBackRecords.Add(Convert.ToInt32(drSendBack["RecordID"]));//Records to be unlocked if locked
                //}


                if (GV.sUserType == "Agent" || GV.sUserType == "Manager")//Record only locked for agents
                {
                    if (GV.lstContactStatusToBeFreezed.Count > 0)
                    {
                        string sCStatusMerged = string.Empty;
                        foreach (string sCStatus in GV.lstContactStatusToBeFreezed)
                        {
                            if (sCStatusMerged.Length == 0)
                                sCStatusMerged = "'" + sCStatus + "'";
                            else
                                sCStatusMerged += "," + "'" + sCStatus + "'";
                        }

                        DataRow[] drrContactsToBeFreezed = dtMasterContacts.Select(GV.sAccessTo + "_CONTACT_STATUS IN (" + sCStatusMerged + ")");
                        if (drrContactsToBeFreezed.Length > 0)
                        {
                            foreach (DataRow drContactsToBeFreezed in drrContactsToBeFreezed)
                            {
                                int iContactID = Convert.ToInt32(drContactsToBeFreezed["CONTACT_ID_P"]);
                                //if (!lstBouncedContactIDs.Contains(iContactID))//Do not freeze Bounced Records
                                lstFreezedContactIDs.Add(iContactID);
                            }
                        }
                    }

                    if (GV.sFreezeTRCompletedRecords == "Y" && GV.sAccessTo == "WR" || GV.sFreezeWRCompletedRecords == "Y" && GV.sAccessTo == "TR")//WR completes connot be edited by TR and vice versa
                    {
                        //DataRow[] drrFreezeContactStatus = dtRecordStatus.Select("Research_Type = '" + sOppositResearchType + "' AND Table_Name ='Contact' AND (Operation_Type LIKE '%Validate%' OR Operation_Type LIKE '%Freeze%')");
                        DataRow[] drrFreezeContactStatus = dtRecordStatus.Select("Research_Type = '" + GV.sOppositAccess + "' AND Table_Name ='Contact' AND Operation_Type LIKE '%Freeze%'");
                        string sFreezedContactStatus = string.Empty;
                        if (drrFreezeContactStatus.Length > 0)
                        {
                            foreach (DataRow dr in drrFreezeContactStatus)
                            {
                                if (sFreezedContactStatus.Length == 0)
                                    sFreezedContactStatus = "'" + dr["Primary_Status"] + "'";
                                else
                                    sFreezedContactStatus += ",'" + dr["Primary_Status"] + "'";
                            }

                            if (sFreezedContactStatus.Length > 0)
                            {
                                DataRow[] drrFreezed = dtMasterContacts.Select(GV.sOppositAccess + "_CONTACT_STATUS IN (" + sFreezedContactStatus + ")");
                                if (drrFreezed.Length > 0)
                                {
                                    foreach (DataRow dr in drrFreezed)
                                    {
                                        int iContactID = Convert.ToInt32(dr["CONTACT_ID_P"]);
                                        if (!lstFreezedContactIDs.Contains(iContactID))
                                            lstFreezedContactIDs.Add(iContactID);
                                    }
                                }
                            }
                        }
                    }

                    lstRecordsToUnlock.AddRange(lstBouncedContactIDs);
                    lstRecordsToUnlock.AddRange(lstSendBackRecords);

                    lstRecordsToUnlock = lstRecordsToUnlock.Distinct().ToList();

                    foreach (int ID in lstRecordsToUnlock)//Do not freeze bounced & Sendback contacts
                    {
                        if (lstFreezedContactIDs.Contains(ID))
                            lstFreezedContactIDs.Remove(ID);
                    }


                    string sContactIDsToUnlock = GM.ListToQueryString(lstRecordsToUnlock, "Int");
                    DataRow[] drrDisposals = dtRecordStatus.Select("Research_Type = '" + GV.sOppositAccess + "'  AND Table_Name='Company' AND Operation_Type LIKE '%Validate%'");
                    List<string> lstOppoResearchFreezedDisposals = new List<string>();
                    foreach (DataRow dr in drrDisposals)
                        lstOppoResearchFreezedDisposals.Add(dr["Primary_Status"].ToString());

                    if (GV.sUserType == "Agent")
                    {
                        //For Company freeze
                        foreach (DataRow drCompany in dtMasterCompanies.Rows)
                        {

                            if (dtQCTable.Select("RecordID = " + drCompany["Master_ID"] + " AND TableName = 'Company' AND QC_Status = 'SendBack'").Length > 0)
                                continue;
                            if (
                            (
                            (GV.sAccessTo == "TR" && (GV.TR_lstDisposalsToBeFreezed.Contains(drCompany["TR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase)))
                            ||
                            (GV.sFreezeWRCompanyCompletes == "Y" && lstOppoResearchFreezedDisposals.Contains(drCompany["WR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase))
                            )
                            ||
                            (
                            (GV.sAccessTo == "WR" && (GV.WR_lstDisposalsToBeFreezed.Contains(drCompany["WR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase)))
                            ||
                            (GV.sFreezeTRCompanyCompletes == "Y" && lstOppoResearchFreezedDisposals.Contains(drCompany["TR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase))
                            )
                            )
                            {
                                if (lstRecordsToUnlock.Count > 0)
                                {
                                    if (dtMasterContacts.Select("MASTER_ID = " + drCompany["MASTER_ID"] + " AND CONTACT_ID_P IN (" + sContactIDsToUnlock + ")").Length == 0)
                                        lstFreezedMasterIDs.Add(Convert.ToInt32(drCompany["MASTER_ID"]));
                                }
                                else
                                    lstFreezedMasterIDs.Add(Convert.ToInt32(drCompany["MASTER_ID"]));
                            }
                        }
                    }

                    if (GV.sUserType == "Manager")//Lock only QC OK Records for Managers
                    {
                        if (lstQCOKIDs.Count > 0)
                        {
                            List<int> lstNonOKRecordsToBeRemoved = new List<int>();

                            foreach (int ID in lstFreezedContactIDs)
                            {
                                if (!lstQCOKIDs.Contains(ID))
                                    lstNonOKRecordsToBeRemoved.Add(ID);
                            }

                            foreach (int ID in lstNonOKRecordsToBeRemoved)
                            {
                                lstFreezedContactIDs.Remove(ID);
                            }
                            //foreach (int ID in lstQCOKIDs)
                            //{
                            //    if (!lstFreezedContactIDs.Contains(ID))o
                            //        lstFreezedContactIDs.Remove(ID);
                            //}
                        }
                        else
                            lstFreezedContactIDs.Clear();
                    }


                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void LineCounter(Label lbl)
        {
            //string[] sArr = null;
            //long lLines = 0;

            //sArr = String.Split(lbl.Text, "\n");
            //lLines = Information.UBound(sArr()) + 1;

            ////Or

            //long lLines = 0;
            //long lPos = 0;

            //lPos = 1;

            //do
            //{
            //    lPos = Strings.InStr(lPos, Label1.Caption, Constants.vbCrLf);
            //    if (lPos)
            //        lLines = lLines + 1;
            //} while (lPos > 0);
        }

        //-----------------------------------------------------------------------------------------------------
        private void AlignControls(List<Control> Control, int iDisplayWidth, Panel Container)
        {
            try
            {
                //Align Controls
                int X, Y = 30;
                int iContainerWidth = Container.Width;
                //if (Container.Name == "sTabPanelContacts")
                X = 5;
                //else
                //    X = 35;

                for (int i = 0; i < Control.Count; i++)
                {
                    //Usually the first control will be label in the control list, Skip the label and move to next control like textbox, combo etc. Now lets assume that
                    //current control is Textbox then strictly the previous control should be its label. So, control[i-1] = label and control[i] = textbox
                    //Now position the label and textbox in appropriate location

                    //Note: This method runs considering that every control has a label
                    if (Control[i] is TextBox || Control[i] is DualText)
                    {
                        if (Control[i - 1] is Label)
                        {
                            Control[i - 1].Location = new Point(X, Y); //Label
                            if (iDisplayWidth > 150)//If multiple line occurs ?
                            {
                                if (Control[i - 1].Text.Length > 30)
                                {
                                    Control[i - 1].Text = sSplitLenghtString(Control[i - 1].Text, iContainerWidth, Control[i - 1].Font).Trim();
                                    int iLineCount = Control[i - 1].Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList().Count;

                                    switch (iLineCount)
                                    {
                                        case 2:
                                            Y += 16;
                                            break;
                                        case 3:
                                            Y += 34;
                                            break;
                                        case 4:
                                            Y += 50;
                                            break;
                                        case 5:
                                            Y += 70;
                                            break;
                                        case 6:
                                            Y += 84;
                                            break;
                                        case 7:
                                            Y += 100;
                                            break;
                                        case 8:
                                            break;
                                    }
                                }
                                Control[i].Width = iContainerWidth - (Control[i - 1].Width - 40);//Fixed Control width

                                Container.Controls.Add(Control[i - 1]);
                                Container.Controls.Add(Control[i]);

                                Y = Y + Control[i - 1].Height + 2;
                                Control[i].Location = new Point(X + 2, Y); //Text Box


                            }
                            else
                            {

                                Control[i].Width = iContainerWidth - iDisplayWidth - 50;//Fixed Control width

                                Container.Controls.Add(Control[i - 1]);
                                Container.Controls.Add(Control[i]);

                                Control[i].Location = new Point(X + Convert.ToInt32(iDisplayWidth), Y); //Text Box

                            }

                            //Container.Controls.Add(Control[i - 1]);
                            //Container.Controls.Add(Control[i]);
                        }

                    }
                    Y = Y + Control[i].Height - 7;// Distance between controls(Vertical Height)

                    if (i == Control.Count - 1)//Add extra control at end to get some extra space
                    {
                        Label lblTemp = new Label();
                        lblTemp.Location = new Point(5, Y);
                        //lblTemp.Text = "Temp";
                        lblTemp.BackColor = Color.Transparent;
                        Container.Controls.Add(lblTemp);
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private int GetDisplaySize(List<Control> Controls)//Determines the maximum length between a label and control
        {
            float fWidthtxt = 0;
            int iTextWidth = 0;
            try
            {
                Graphics g = ((frmMain)frmMDI).CreateGraphics();//To Get grapical length
                foreach (Control C in Controls)
                {
                    if (C is Label)
                    {
                        if (C.Text.Length > iTextWidth)
                        {
                            //fWidthtxt = g.MeasureString(C.Text, C.Font).Width;//Gets Max display size of label
                            fWidthtxt = TextRenderer.MeasureText(C.Text, C.Font).Width;//Gets Max display size of label
                            iTextWidth = C.Text.Length;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return Convert.ToInt32(fWidthtxt) + 10;
        }



        //-----------------------------------------------------------------------------------------------------
        private void TextChangeContact(object sender, EventArgs e)//Runtime Event //Push values to grid
        {
            //if (!IsTableToTextContact)
            //    TableToTextContact(iGridRowIndex, "ControlsToGrid", true);
        }

        //-----------------------------------------------------------------------------------------------------
        private void TextChangeCompany(object sender, EventArgs e)//Runtime Event //Push values to grid
        {
            //if (!IsTableToTextCompany)
            //    TableToTextCompany("ControlsToGrid");
        }



        //-----------------------------------------------------------------------------------------------------
        private void TableToTextCompany(int iRowIndex, string sDirection, bool IsControlEnabled)//Pass data between text boxes to grid or grid to text boxes.
        {
            try
            {
                //GridToText(index,"TRUE") -- Transfer data from Data grid --> Controls
                //GridToText(index,"FALSE") -- Transfer data from Controls --> Data grid

                //if (!IsLoading)
                {
                    //IsTableToTextCompany = true;
                    IsLoading = true;
                    if (lstCompanyControls != null)
                    {
                        foreach (TextBox txt in lstCompanyControls)
                        {
                            if (dtMasterCompanies.Rows[iRowIndex][txt.Name] != null)
                            {
                                if (sDirection == "GridToControls")
                                {
                                    txt.Text = dtMasterCompanies.Rows[iRowIndex][txt.Name].ToString();
                                    txt.Enabled = IsControlEnabled;
                                }
                                else if (sDirection == "ControlsToGrid")
                                {
                                    dtMasterCompanies.Rows[iRowIndex][txt.Name] = txt.Text;
                                    //dtMasterCompaniesSQLCE.Rows[0][txt.Name] = txt.Text;
                                }
                            }
                        }
                    }

                    if (sDirection == "ControlsToGrid")
                        RefreshCompanyGrid(iRowIndex);

                    IsLoading = false;
                    //IsTableToTextCompany = false;
                }
            }
            catch (Exception ex)//here is the error 63
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        //-----------------------------------------------------------------------------------------------------
        private void TableToTextContact(int iRowIndex, string sDirection, bool IsControlEnabled)//Pass data between text boxes to grid or grid to text boxes.
        {
            try
            {
                //GridToText(index,"TRUE") -- Transfer data from Data grid --> Controls
                //GridToText(index,"FALSE") -- Transfer data from Controls --> Data grid

                //if(!IsLoading)
                {
                    //IsTableToTextContact = true;
                    IsLoading = true;
                    if (lstContactControls != null && iRowIndex >= 0)
                    {
                        iContactRowIndex = iRowIndex;
                        foreach (TextBox txt in lstContactControls)
                        {
                            if (dtMasterContacts.Rows[iRowIndex][txt.Name] != null)
                            {
                                if (sDirection == "GridToControls")
                                //Data grid -> TextBox
                                {
                                    //C.TextChanged -= new EventHandler(TextToGrid);

                                    txt.Text = dtMasterContacts.Rows[iRowIndex][txt.Name].ToString();
                                    txt.Enabled = IsControlEnabled;

                                    if (txt.Name.ToUpper() == "EMAIL_VERIFIED")//Control for bounce
                                    {
                                        //if (dtMasterContacts.Rows[iRowIndex]["EMAIL_VERIFIED"].ToString().Length > 0)
                                        if (dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"].ToString().Length > 0 && lstBouncedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                                            ShowHideControl(txt, true, sTabPanelContacts);
                                        else
                                            ShowHideControl(txt, false, sTabPanelContacts);
                                    }

                                    //This block is not relevent since the contacts are disabled by lstFreezed Contacts
                                    if (txt.Name.ToUpper() == GV.sAccessTo + "_CONTACT_STATUS" && dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"].ToString().Length > 0)//Freeeze contact status for bounced records
                                    {
                                        if (lstRecordsToUnlock.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])) && GV.lstContactStatusToBeFreezed.Contains(dtMasterContacts.Rows[iRowIndex][GV.sAccessTo + "_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                        {
                                            if (GV.sUserType == "Agent" || (GV.sUserType == "Manager" && lstQCOKIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])) && lstBouncedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"]))))
                                                txt.Enabled = false;
                                        }
                                    }
                                    // C.TextChanged += new EventHandler(TextToGrid);
                                }
                                else if (sDirection == "ControlsToGrid")
                                //Text box -> Data grid
                                {
                                    dtMasterContacts.Rows[iRowIndex][txt.Name] = txt.Text;
                                    // dtMasterContactsSQLCE.Rows[iRowIndex][txt.Name] = txt.Text;
                                }
                            }
                        }
                    }

                    if (sDirection == "ControlsToGrid")
                        RefreshContactGrid(iRowIndex);

                    IsLoading = false;


                    if ((!IsSuperGridLoading) && GV.DynamicValidator)
                    {
                        if (iContactRowIndex != -1)
                        {
                            webBrowserMessage.DocumentText = string.Empty;
                            dtValidationResultsDynamic.Rows.Clear();
                            //DynamicMandatoryNotification(dtMasterContacts.Rows[iRowIndex], iRowIndex);
                            ALL_ValidationTable_Check_Dynamic(iRowIndex);
                            if (dtValidationResultsDynamic.Rows.Count > 0)
                            {
                                if (!expandablePanelMessage.Expanded)
                                    expandablePanelMessage.Expanded = true;
                                webBrowserMessage.DocumentText = BuildError(string.Empty, true);
                            }
                            else
                            {
                                if (expandablePanelMessage.Expanded)
                                    expandablePanelMessage.Expanded = false;
                            }
                        }
                    }

                    //IsTableToTextContact = false;
                }

            }
            catch (Exception ex)//here is the error 63
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void ShowHideControl(TextBox txt, bool IsVisible, Panel pnl)
        {
            txt.Visible = IsVisible;//Hide / Show Control
            foreach (Control lbl in pnl.Controls)//Hide / Show Label for Control
            {
                if (lbl is Label)
                {
                    if (lbl.Name == "lbl" + txt.Name)
                    {
                        lbl.Visible = IsVisible;
                        break;
                    }
                }
            }
        }




        //-----------------------------------------------------------------------------------------------------
        private void dtp_Leave(object sender, EventArgs e)
        {
            //dgvContacts.CurrentCell.Value = dtp.Value.ToString();
            //dtp.Visible = false;
            //dgvContacts.Focus();
        }

        //-----------------------------------------------------------------------------------------------------
        private void dtp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    //dgvContacts.CurrentCell.Value = dtp.Value.ToString();
                    //dtp.Visible = false;
                    //dgvContacts.Focus();
                }
                catch (Exception ex)
                {
                    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }


        //-----------------------------------------------------------------------------------------------------
        public static List<Control> GetAllControls(Control Container)//Loads all the controls from  container or sub container
        {
            var controlList = new List<Control>();
            try
            {
                foreach (Control childControl in Container.Controls)
                {
                    // Recurse child controls.
                    controlList.AddRange(GetAllControls(childControl));
                    controlList.Add(childControl);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return controlList;
        }

        //-----------------------------------------------------------------------------------------------------
        public static List<Control> GetOnlyChildControls(Control Container)//Loads all the controls from  container or sub container
        {
            var controlList = new List<Control>();
            try
            {
                foreach (Control childControl in Container.Controls)
                {
                    // Recurse child controls.
                    //controlList.AddRange(GetAllControls(childControl));
                    controlList.Add(childControl);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return controlList;
        }


        //-----------------------------------------------------------------------------------------------------
        private DataTable RemoveDuplicateAndEmptyRows(DataTable dtContactsTable, string sKeyColumnName)
        {
            try
            {
                //remove Dupe from datatable
                string sSort = string.Empty;
                foreach (DataColumn scol in dtContactsTable.Columns)
                {
                    if (!string.IsNullOrEmpty(sSort))
                        sSort += ",";
                    sSort += scol.ColumnName;
                }
                dtContactsTable.DefaultView.Sort = sSort;
                int i = 0;
                bool bEquals = false;
                int c = 1;
                int ccount = dtContactsTable.Columns.Count;
                for (i = dtContactsTable.Rows.Count - 1; i >= 1; i += -1)
                {
                    bEquals = true;
                    for (c = 1; c <= ccount - 1; c++)
                    {
                        if (dtContactsTable.DefaultView[i][c].ToString() != dtContactsTable.DefaultView[i - 1][c].ToString())
                        {
                            bEquals = false;
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }

                    if (bEquals)
                    {
                        dtContactsTable.DefaultView[i].Delete();
                    }
                }
                //remove Empty rows if exist
                for (int j = 0; j <= dtContactsTable.Rows.Count - 1; j++)
                {
                    string valuesarr = string.Empty;
                    List<object> lst = new List<object>(dtContactsTable.Rows[j].ItemArray);
                    foreach (object s in lst)
                    {
                        valuesarr += s.ToString();
                    }
                    if (string.IsNullOrEmpty(valuesarr))
                    {
                        dtContactsTable.Rows[j].Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return dtContactsTable;
        }


        //-----------------------------------------------------------------------------------------------------
        private void All_PostUpdate(DoWorkEventArgs e)
        {
            //GV.sValidationMessage = "Running Automated data updations on contacts";
            try
            {
                if (RunASynch(e))
                {
                    GV.sValidationMessage = "Running Automated data updations";
                    DataRow[] drrDataPopulate = dtValidations.Select(String.Format("OPERATION_TYPE ='PostUpdate' AND RESEARCH_TYPE = '{0}'", GV.sAccessTo));
                    if (drrDataPopulate.Length > 0)
                    {
                        foreach (DataRow drDataPopulate in drrDataPopulate)
                        {
                            string sPopulateType = drDataPopulate["VALIDATION_TYPE"].ToString();
                            string sValidation_For = drDataPopulate["VALIDATION_FOR"].ToString();
                            string sTarget_Field = drDataPopulate["VALIDATION_VALUE"].ToString().Split('=').ToList()[0];
                            string sSource_Field = drDataPopulate["VALIDATION_VALUE"].ToString().Split('=').ToList()[1];
                            string sValidation_For_Values = string.Empty;
                            List<string> lstConditionColumns = drDataPopulate["CONDITION"].ToString().Split('|').ToList();
                            if (sValidation_For.StartsWith("C."))
                            {
                                sValidation_For = sValidation_For.Replace("C.", "");
                                foreach (DataRow drContacts in dtMasterContacts.Rows)
                                {
                                    if (drContacts["CONTACT_ID_P"].ToString().Length > 0 && lstFreezedContactIDs.Contains(Convert.ToInt32(drContacts["CONTACT_ID_P"])))
                                        continue;

                                    List<string> lstConditionValues = new List<string>();
                                    sValidation_For_Values = drContacts[sValidation_For].ToString().ToUpper();

                                    foreach (string s in lstConditionColumns)
                                    {
                                        if (s.StartsWith("C."))
                                            lstConditionValues.Add(drContacts[s.Replace("C.", "")].ToString().ToUpper());
                                        else if (s.StartsWith("M."))
                                            lstConditionValues.Add(dtMasterCompanies.Select("MASTER_ID = " + drContacts["MASTER_ID"])[0][s.Replace("M.", "")].ToString().ToUpper());
                                        else
                                            lstConditionValues.Add(s.ToUpper());
                                    }

                                    if (lstConditionValues.Contains(sValidation_For_Values))
                                    {
                                        if (sTarget_Field.StartsWith("C."))
                                            Postupdate_Variable_Population(sSource_Field, sTarget_Field.Replace("C.", ""), sPopulateType, drContacts, drContacts, dtMasterCompanies.Select("MASTER_ID = " + drContacts["MASTER_ID"])[0]);
                                        else
                                        {
                                            foreach (DataRow drCompanies in dtMasterCompanies.Rows)
                                            {
                                                if (drCompanies["MASTER_ID"].ToString() == drContacts["MASTER_ID"].ToString())
                                                {
                                                    Postupdate_Variable_Population(sSource_Field, sTarget_Field.Replace("M.", ""), sPopulateType, drCompanies, drContacts, drCompanies);
                                                    break;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            else
                            {
                                sValidation_For = sValidation_For.Replace("M.", "");
                                foreach (DataRow drCompany in dtMasterCompanies.Rows)
                                {
                                    if (drCompany["MASTER_ID"].ToString().Length > 0 && lstFreezedMasterIDs.Contains(Convert.ToInt32(drCompany["MASTER_ID"])))
                                        continue;

                                    List<string> lstConditionValues = new List<string>();
                                    sValidation_For_Values = drCompany[sValidation_For].ToString().ToUpper();

                                    foreach (string s in lstConditionColumns)
                                    {
                                        if (s.StartsWith("M."))
                                            lstConditionValues.Add(drCompany[s.Replace("M.", "")].ToString().ToUpper());
                                        else
                                            lstConditionValues.Add(s.ToUpper());
                                    }

                                    if (lstConditionValues.Contains(sValidation_For_Values))
                                    {
                                        if (sTarget_Field.StartsWith("M."))
                                            Postupdate_Variable_Population(sSource_Field, sTarget_Field.Replace("M.", ""), sPopulateType, drCompany, null, drCompany);
                                    }
                                }
                            }

                            //foreach (DataRow drContact in dtMasterContacts.Rows)
                            //{
                            //    if (drContact[drDataPopulate["VALIDATION_FOR"].ToString()].ToString() == drDataPopulate["CONDITION"].ToString())
                            //    {
                            //        switch (sPopulateType)
                            //        {
                            //            case "UPDATEONCE":
                            //                List<string> lstValueUPDATEONCE = drDataPopulate["VALIDATION_VALUE"].ToString().Split('=').ToList();
                            //                if (drContact[lstValueUPDATEONCE[0].Trim()].ToString().Length == 0)
                            //                {
                            //                    if (lstValueUPDATEONCE[1].Trim().ToUpper() == "GETDATE")
                            //                        drContact[lstValueUPDATEONCE[0].Trim()] = GM.GetDateTime();
                            //                    else
                            //                        drContact[lstValueUPDATEONCE[0].Trim()] = lstValueUPDATEONCE[1].Trim();
                            //                }
                            //                break;

                            //            case "UPDATEEVERYTIME":
                            //                List<string> lstValueUPDATEEVERYTIME = drDataPopulate["VALIDATION_VALUE"].ToString().Split('=').ToList();
                            //                if (lstValueUPDATEEVERYTIME[1].Trim().ToUpper() == "GETDATE")
                            //                    drContact[lstValueUPDATEEVERYTIME[0].Trim()] = GM.GetDateTime();
                            //                else
                            //                    drContact[lstValueUPDATEEVERYTIME[0].Trim()] = lstValueUPDATEEVERYTIME[1].Trim();
                            //                break;
                            //        }
                            //    }
                            //}
                        }
                    }
                    UpdateNotifier(0);
                }
            }
            catch (Exception ex)
            {
                lstApplicationError.Add("Contact DataPopulate : " + ex.Message);
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        void Postupdate_Variable_Population(string sSource, string sTarget, string sPopulate_Type, DataRow drTarget, DataRow drContactRow, DataRow drComapanyRow)
        {
            string sOut = string.Empty;
            if (sSource.StartsWith("C."))
                sOut = drContactRow[sSource.Replace("C.", "")].ToString();
            else if (sSource.StartsWith("M."))
                sOut = drComapanyRow[sSource.Replace("M.", "")].ToString();
            else
            {
                switch (sSource.ToUpper())
                {
                    case "GETDATE":
                        sOut = GM.GetDateTime().ToString();
                        break;

                    case "AGENTNAME":
                        sOut = GV.sEmployeeName;
                        break;

                    case "RESEARCHTYPE":
                        sOut = GV.sAccessTo;
                        break;

                    default:
                        sOut = sSource;
                        break;
                }
            }

            if (sPopulate_Type == "UPDATEONCE")
            {
                if (drTarget[sTarget].ToString().Length == 0)
                    drTarget[sTarget] = sOut;
            }
            else if (sPopulate_Type == "UPDATEEVERYTIME")
                drTarget[sTarget] = sOut;
        }

        //-----------------------------------------------------------------------------------------------------      


        //-----------------------------------------------------------------------------------------------------
        private void COMPANY_CollectDataFromControl()
        {
            try
            {
                bool bCompanyDataChanged = false;
                //Collectes data from Master Controls and updates it to master Datatable
                //Update Company Details from existing company controls
                foreach (TextBox txt in lstCompanyControls) //Loop through all Master controls of form
                {
                    if (txt.Name.Length > 0 && dtMasterCompanies.Columns.Contains(txt.Name))//Filter only MasterContacts Columns
                    {

                        if (dtMasterCompanies.Rows[0][txt.Name].ToString() != txt.Text)
                            bCompanyDataChanged = true;
                        dtMasterCompanies.Rows[0][txt.Name] = txt.Text.Trim();

                        //else if (txt is CheckedListBox)//Get all selected items in checklistbox
                        //{
                        //    CheckedListBox clb = (CheckedListBox)txt;
                        //    string sSelectedItems = string.Empty;
                        //    foreach (string sItems in clb.CheckedItems)
                        //    {
                        //        if (sSelectedItems.Length == 0)
                        //            sSelectedItems = sItems;
                        //        else
                        //            sSelectedItems = String.Format("{0}|{1}", sSelectedItems, sItems);
                        //    }


                        //    if (dtMasterCompanies.Rows[0][txt.Name].ToString() != sSelectedItems)
                        //        bCompanyDataChanged = true;
                        //    dtMasterCompanies.Rows[0][txt.Name] = sSelectedItems.Trim();
                        //}
                        //else if (txt is ListBox)
                        //{
                        //    ListBox lst = (ListBox)txt;
                        //    if (lst.SelectedValue != null)
                        //    {
                        //        if (dtMasterCompanies.Rows[0][txt.Name].ToString() != lst.SelectedValue.ToString())
                        //            bCompanyDataChanged = true;
                        //        dtMasterCompanies.Rows[0][txt.Name] = lst.SelectedValue.ToString();
                        //    }
                        //}

                    }
                }

                //if (bCompanyDataChanged)

            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void COMPANY_All_Validation(DoWorkEventArgs e)
        {
            GV.sValidationMessage = "Checking Comapny informations";
            if (RunASynch(e))
            {
                RecordTime("COMPANY_ValidateDisposals Start");
                COMPANY_ValidateDisposals(e);//Validate if disposals are selected or not
                RecordTime("COMPANY_ValidateDisposals Stop");

                RecordTime("COMPANY_CommonValidation Start");
                COMPANY_CommonValidation(e);
                RecordTime("COMPANY_CommonValidation Stop");

                RecordTime("COMPANY_CheckSwitchBoardDupe Start");
                COMPANY_CheckSwitchBoardDupe(e);
                RecordTime("COMPANY_CheckSwitchBoardDupe Stop");
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void COMPANY_CommonValidation(DoWorkEventArgs e)
        {
            GV.sValidationMessage = "Checking Company, Country, Switchboard empty";
            try
            {

                if (RunASynch(e))
                {

                    for (int i = 0; i < dtMasterCompanies.Rows.Count; i++)
                    {
                        if (dtRecordStatus.Select("TABLE_NAME = 'COMPANY' AND PRIMARY_STATUS = '" + dtMasterCompanies.Rows[i][GV.sAccessTo + "_PRIMARY_DISPOSAL"] + "' AND SECONDARY_STATUS = '" + dtMasterCompanies.Rows[i][GV.sAccessTo + "_SECONDARY_DISPOSAL"] + "' AND RESEARCH_TYPE = '" + GV.sAccessTo + "' AND  OPERATION_TYPE LIKE '%Delete%'").Length == 0)
                        {
                            if (!lstFreezedMasterIDs.Contains(Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"])))
                            {
                                if (dtMasterCompanies.Rows[i]["COMPANY_NAME"].ToString().Trim().Length == 0)//Check empty company field
                                    AddValidationResults("Company", i, Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"]), 0, "", "EMPTY", "COMPANY_NAME", "", false, 0);

                                if (dtMasterCompanies.Rows[i]["COUNTRY"].ToString().Trim().Length == 0)//Check valid country
                                    AddValidationResults("Company", i, Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"]), 0, "", "EMPTY", "COUNTRY", "", false, 0);
                                else
                                {
                                    if (dtCountryInformation.Select("CountryName = '" + dtMasterCompanies.Rows[i]["COUNTRY"].ToString().Trim().Replace("'", "''") + "'").Length == 0)
                                        AddValidationResults("Company", i, Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"]), 0, "", "INVALID", "COUNTRY", "", false, 0);
                                }

                                if (GV.sAccessTo == "TR")
                                {
                                    if (dtMasterCompanies.Rows[i]["SWITCHBOARD"].ToString().Trim().Length == 0)//Check valid Telephone
                                        AddValidationResults("Company", i, Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"]), 0, "", "EMPTY", "SWITCHBOARD", "", false, 0);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lstApplicationError.Add("Company CommonValidation : " + ex.Message);
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void CONTACT_All_Validation(DoWorkEventArgs e)
        {
            GV.sValidationMessage = "Checking all Contacts informations";
            if (RunASynch(e))
            {
                string sFreezedContatactIDs = string.Empty;
                foreach (int i in lstFreezedContactIDs)//Get freezed contactstatus(Validations will not be saved)
                {
                    if (sFreezedContatactIDs.Length == 0)
                        sFreezedContatactIDs = i.ToString();
                    else
                        sFreezedContatactIDs += "," + i;
                }

                RecordTime("CONTACT_RemoveEmptyRecords Start");
                CONTACT_RemoveEmptyRecords(e);//Remove Empty records or invalid Records (means empty first name,last name, email, job title and contact status)
                RecordTime("CONTACT_RemoveEmptyRecords Stop");

                RecordTime("CONTACT_Validate_ContactStatus Start");
                CONTACT_Validate_ContactStatus(e);
                RecordTime("CONTACT_Validate_ContactStatus Stop");

                RecordTime("CONTACT_CheckContactsCount Start");
                CONTACT_CheckContactsCount(e);
                RecordTime("CONTACT_CheckContactsCount Stop");

                RecordTime("CONTACT_CheckSwitchBoardinContacts Start");
                CONTACT_CheckSwitchBoardinContacts(sFreezedContatactIDs, e);
                RecordTime("CONTACT_CheckSwitchBoardinContacts Stop");

                RecordTime("CONTACT_CheckDuplicateEmail Start");
                CONTACT_CheckDuplicateEmail(sFreezedContatactIDs, e);
                RecordTime("CONTACT_CheckDuplicateEmail Stop");
            }
        }

        void Log_OpenClose(string sTransactionStatus)
        {
            DataTable dtLog = GV.MYSQL.BAL_FetchTableMySQL(GV.sProjectID + "_log", "1=0");

            string sInsertString = String.Empty;

            if (dtMasterCompanies.Rows.Count > 1)
            {
                foreach (DataRow dr in dtMasterCompanies.Rows)
                {
                    DataRow drNewRow = dtLog.NewRow();
                    drNewRow["RecordID"] = dr["MASTER_ID"].ToString();
                    drNewRow["CompanySessionID"] = GV.sCompanySessionID;
                    drNewRow["TableName"] = "RecordStatus";
                    drNewRow["FieldName"] = "Group " + sTransactionStatus;
                    drNewRow["OldValue"] = dtMasterContacts.Select("MASTER_ID = " + dr["MASTER_ID"]).Length;
                    drNewRow["NewValue"] = GV.sUserType;
                    drNewRow["When"] = GM.GetDateTime();
                    drNewRow["Who"] = GV.sEmployeeName;
                    drNewRow["SystemName"] = Environment.MachineName;
                    dtLog.Rows.Add(drNewRow);
                    //sInsertString +=  ",(" + sMaster_ID + ", '" + GV.sCompanySessionID + "','RecordStatus','" + sTransactionStatus + "','" + dtMasterContacts.Rows.Count + "','" + GV.sUserType + "','" + GM.GetDateTime() + "','" + GV.sEmployeeName + "','" + Environment.MachineName + "')";
                }

                //sInsertString =
                //    "INSERT crucru005_log (RecordID, CompanySessionID, TableName, FieldName, OldValue, NewValue, `When`, Who, SystemName) VALUES " +
                //    sInsertString.Substring(1);
            }
            else
            {
                sInsertString = "INSERT crucru005_log (RecordID, CompanySessionID, TableName, FieldName, OldValue, NewValue, `When`, Who, SystemName) VALUES " +
                                "(" + sMaster_ID + ", '" + GV.sCompanySessionID + "','RecordStatus','" + sTransactionStatus + "','" + dtMasterContacts.Rows.Count + "','" + GV.sUserType + "','" + GM.GetDateTime() + "','" + GV.sEmployeeName + "','" + Environment.MachineName + "')";
                DataRow drNewRow = dtLog.NewRow();
                drNewRow["RecordID"] = sMaster_ID;
                drNewRow["CompanySessionID"] = GV.sCompanySessionID;
                drNewRow["TableName"] = "RecordStatus";
                drNewRow["FieldName"] = sTransactionStatus;
                drNewRow["OldValue"] = dtMasterContacts.Rows.Count.ToString();
                drNewRow["NewValue"] = GV.sUserType;
                drNewRow["When"] = GM.GetDateTime();
                drNewRow["Who"] = GV.sEmployeeName;
                drNewRow["SystemName"] = Environment.MachineName;
                dtLog.Rows.Add(drNewRow);


            }
            //GV.MYSQL.BAL_ExecuteNonReturnQueryMySQL(sInsertString);
            GV.MYSQL.BAL_SaveToTableMySQL(dtLog, GV.sProjectID + "_log", "New", false);
        }

        void Logging(DataTable dtNewTable, DataTable dtOldTable)
        {
            try
            {
                string sIDColumn = string.Empty;
                string sTableName = dtNewTable.TableName;
                if (sTableName == "MasterContact")
                {
                    GV.sValidationMessage = "Logging Contact Informations";
                    sIDColumn = "CONTACT_ID_P";
                }
                else
                {
                    GV.sValidationMessage = "Logging Company Informations";
                    sIDColumn = "MASTER_ID";
                }

                DataTable dtChangedData = dtNewTable.GetChanges(DataRowState.Modified);
                DataTable dtLog = GV.MYSQL.BAL_FetchTableMySQL(GV.sProjectID + "_log", "1=0");

                if (dtChangedData != null)
                {
                    foreach (DataRow drChanged in dtChangedData.Rows)
                    {
                        string sContactID = drChanged[sIDColumn].ToString();
                        DataRow drOld = dtOldTable.Select(sIDColumn + " = " + sContactID)[0];
                        foreach (DataColumn dcOld in drOld.Table.Columns)
                        {
                            if (drChanged[dcOld.ColumnName].ToString() != drOld[dcOld.ColumnName].ToString())
                            {
                                DataRow drLog = dtLog.NewRow();
                                drLog["RecordID"] = drChanged[sIDColumn].ToString();
                                drLog["CompanySessionID"] = GV.sCompanySessionID;
                                drLog["TableName"] = sTableName;
                                drLog["Who"] = GV.sEmployeeName;
                                drLog["When"] = GM.GetDateTime();
                                drLog["SystemName"] = Environment.MachineName;
                                drLog["FieldName"] = dcOld.ColumnName;
                                if (dcOld.ColumnName == GV.sAccessTo + "_AGENTNAME" && sTableName == "MasterCompanies")
                                    drLog["OldValue"] = drOld[GV.sAccessTo + "_PREVIOUS_AGENTNAME"].ToString();
                                else
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
                GV.iNotifier[5] = 1;
                //CloseNotifier();
                lstApplicationError.Add("Logging : " + ex.Message);
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
            }
        }

        ////-----------------------------------------------------------------------------------------------------
        //private void CONTACT_UpdateRecordsContactStatus()
        //{
        //    try
        //    {
        //        if (GV.sAccessTo == "WR")
        //        {
        //            foreach (DataRow dr in dtMasterContacts.Rows)
        //            {
        //                //if (GV.sFreezeWRCompletedRecords == "Y" && GV.lstWRContactStatusToBeValidated.Contains(dr["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
        //                if (GV.lstWRContactStatusToBeValidated.Contains(dr["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
        //                {
        //                    if (dr["TR_CONTACT_STATUS"].ToString().Trim().Length == 0)//Do not update if already exist.
        //                        dr["TR_CONTACT_STATUS"] = "WEBRESEARCHED";
        //                }

        //                if (GV.lstWR_DeleteStatus.Contains(dr["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
        //                {
        //                    if (dr["TR_CONTACT_STATUS"].ToString().Trim().Length == 0)//Do not update if already exist.
        //                        dr["TR_CONTACT_STATUS"] = "Do Not Call";
        //                }
        //            }
        //        }
        //        else if (GV.sAccessTo == "TR")
        //        {
        //            foreach (DataRow dr in dtMasterContacts.Rows)
        //            {
        //                //if (GV.sFreezeTRCompletedRecords == "Y" && GV.lstTRContactStatusToBeValidated.Contains(dr["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
        //                if (GV.lstTRContactStatusToBeValidated.Contains(dr["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
        //                {
        //                    if (dr["WR_CONTACT_STATUS"].ToString().Trim().Length == 0)
        //                        dr["WR_CONTACT_STATUS"] = "TELERESEARCHED";
        //                }

        //                if (GV.lstTR_DeleteStatus.Contains(dr["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
        //                {
        //                    if (dr["WR_CONTACT_STATUS"].ToString().Trim().Length == 0)//Do not update if already exist.
        //                        dr["WR_CONTACT_STATUS"] = "Do Not Research";
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GV.iNotifier[4] = 1;
        //        //CloseNotifier();
        //        lstApplicationError.Add("Contact UpdateRecordsContactStatus : " + ex.Message);
        //        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
        //        return;
        //    }
        //}


        //-----------------------------------------------------------------------------------------------------

        private void ALL_ValidationTable_Check(DoWorkEventArgs e)
        {
            GV.sValidationMessage = "Checking Comapny Contact informations";
            if (RunASynch(e))
            {
                DataRow[] drrValidationTable = dtValidations.Select("OPERATION_TYPE='Validation'");
                string sValidation_Type = string.Empty;
                string sValidation_For = string.Empty;
                string sValidation_For_Values = string.Empty;
                string sCondition = string.Empty;
                List<string> lstConditionColumns;
                string sValidation_Value_Primary = string.Empty;
                string sValidation_Value_Sec = string.Empty;
                string sDelimiter = string.Empty;

                for (int i = 0; i < dtMasterContacts.Rows.Count; i++)
                {
                    if (GV.lstTRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase) || GV.lstWRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                    {
                        if (!(dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString().Length > 0 && lstFreezedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"]))))
                            CONTACT_Check_RejectList(i, false);
                    }
                }

                foreach (DataRow drValidation in drrValidationTable)
                {
                    sValidation_Type = drValidation["VALIDATION_TYPE"].ToString().ToUpper();
                    sValidation_For = drValidation["VALIDATION_FOR"].ToString().ToUpper();
                    sCondition = drValidation["CONDITION"].ToString().ToUpper();
                    lstConditionColumns = sCondition.Split('|').ToList();
                    sValidation_Value_Sec = string.Empty;
                    sValidation_For_Values = string.Empty;
                    sDelimiter = string.Empty;

                    if (Regex.IsMatch(drValidation["VALIDATION_VALUE"].ToString(), @"\=\=|\*\=|\=\*|\*\*|\#\#"))
                    {
                        if (drValidation["VALIDATION_VALUE"].ToString().Contains("=="))
                        {
                            sDelimiter = "==";
                            sValidation_Value_Primary = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[0].ToUpper();
                            sValidation_Value_Sec = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[1].ToUpper();
                        }
                        else if (drValidation["VALIDATION_VALUE"].ToString().Contains("*="))
                        {
                            sDelimiter = "*=";
                            sValidation_Value_Primary = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[0].ToUpper();
                            sValidation_Value_Sec = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[1].ToUpper();
                        }
                        else if (drValidation["VALIDATION_VALUE"].ToString().Contains("=*"))
                        {
                            sDelimiter = "=*";
                            sValidation_Value_Primary = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[0].ToUpper();
                            sValidation_Value_Sec = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[1].ToUpper();
                        }
                        else if (drValidation["VALIDATION_VALUE"].ToString().Contains("**"))
                        {
                            sDelimiter = "**";
                            sValidation_Value_Primary = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[0].ToUpper();
                            sValidation_Value_Sec = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[1].ToUpper();
                        }
                        else if (drValidation["VALIDATION_VALUE"].ToString().Contains("##"))
                        {
                            sDelimiter = "##";
                            sValidation_Value_Primary = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[0].ToUpper();
                            sValidation_Value_Sec = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[1].ToUpper();
                        }
                    }
                    else
                        sValidation_Value_Primary = drValidation["VALIDATION_VALUE"].ToString().ToUpper();

                    if (sValidation_For.StartsWith("M."))//If Validation is for company then the other fields has to deld only with company column
                    {
                        //sValidation_For = sValidation_For.Replace("M.", "");                        
                        for (int i = 0; i < dtMasterCompanies.Rows.Count; i++)
                        {
                            GV.sValidationMessage = "Checking " + (i + 1) + " of " + dtMasterCompanies.Rows.Count + " companie(s)";
                            if (!lstFreezedMasterIDs.Contains(Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"])))
                            {
                                List<string> lstConditionValues = new List<string>();
                                sValidation_For_Values = dtMasterCompanies.Rows[i][sValidation_For.Replace("M.", "")].ToString().ToUpper();
                                foreach (string s in lstConditionColumns)
                                {
                                    if (s.StartsWith("M."))
                                        lstConditionValues.Add(dtMasterCompanies.Rows[i][s.Replace("M.", "")].ToString().ToUpper());
                                    else
                                        lstConditionValues.Add(s.ToUpper());
                                }
                                if (lstConditionValues.Contains(sValidation_For_Values) || lstValidationTypeToIgnore.Contains(sValidation_Type, StringComparer.OrdinalIgnoreCase))
                                    Validation_Table(false, drValidation, "Company", sValidation_Type, sValidation_For, lstConditionValues, sValidation_Value_Primary, sValidation_Value_Sec, dtMasterCompanies.Rows[i], null, i, sDelimiter);
                            }
                        }
                    }
                    else if (drValidation["VALIDATION_FOR"].ToString().StartsWith("C."))
                    {
                        //sValidation_For = sValidation_For.Replace("C.", "");
                        for (int i = 0; i < dtMasterContacts.Rows.Count; i++)
                        {
                            GV.sValidationMessage = "Checking " + (i + 1) + " of " + dtMasterContacts.Rows.Count + " companie(s)";
                            if (dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString().Length > 0 && lstFreezedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])))
                                continue;
                            else
                            {
                                List<string> lstConditionValues = new List<string>();
                                sValidation_For_Values = dtMasterContacts.Rows[i][sValidation_For.Replace("C.", "")].ToString().ToUpper();
                                foreach (string s in lstConditionColumns)
                                {
                                    if (s.StartsWith("C."))
                                        lstConditionValues.Add(dtMasterContacts.Rows[i][s.Replace("C.", "")].ToString().ToUpper());
                                    else if (s.StartsWith("M."))
                                        lstConditionValues.Add(dtMasterCompanies.Select("MASTER_ID = " + dtMasterContacts.Rows[i]["MASTER_ID"])[0][s.Replace("M.", "")].ToString().ToUpper());
                                    else
                                        lstConditionValues.Add(s.ToUpper());
                                }

                                if (lstConditionValues.Contains(sValidation_For_Values) || lstValidationTypeToIgnore.Contains(drValidation["VALIDATION_TYPE"].ToString(), StringComparer.OrdinalIgnoreCase))
                                    Validation_Table(false, drValidation, "Contact", sValidation_Type, sValidation_For, lstConditionValues, sValidation_Value_Primary, sValidation_Value_Sec, dtMasterCompanies.Rows[0], dtMasterContacts.Rows[i], i, sDelimiter);
                            }
                        }
                    }
                }
                UpdateNotifier(1);
            }
        }

        private void ALL_ValidationTable_Check_Dynamic(int iRowIndex)
        {
            try
            {
                DataRow[] drrValidationTable = dtValidations.Select("OPERATION_TYPE='Validation'");
                string sValidation_Type = string.Empty;
                string sValidation_For = string.Empty;
                string sValidation_For_Values = string.Empty;
                string sCondition = string.Empty;
                List<string> lstConditionColumns;
                string sValidation_Value_Primary = string.Empty;
                string sValidation_Value_Sec = string.Empty;
                string sDelimiter = string.Empty;

                foreach (TextBox txt in lstContactControls)
                {
                    txt.BackColor = Color.White;
                    txt.Refresh();
                }

                if (!(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"].ToString().Length > 0 && lstFreezedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"]))))
                {
                    CONTACT_Check_RejectList(iRowIndex, true);
                }

                foreach (DataRow drValidation in drrValidationTable)
                {
                    sValidation_Type = drValidation["VALIDATION_TYPE"].ToString().ToUpper();
                    sValidation_For = drValidation["VALIDATION_FOR"].ToString().ToUpper();
                    sCondition = drValidation["CONDITION"].ToString().ToUpper();
                    lstConditionColumns = sCondition.Split('|').ToList();
                    sValidation_Value_Sec = string.Empty;
                    sValidation_For_Values = string.Empty;
                    sDelimiter = string.Empty;

                    if (Regex.IsMatch(drValidation["VALIDATION_VALUE"].ToString(), @"\=\=|\*\=|\=\*|\*\*|\#\#"))
                    {
                        if (drValidation["VALIDATION_VALUE"].ToString().Contains("=="))
                        {
                            sDelimiter = "==";
                            sValidation_Value_Primary = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[0].ToUpper();
                            sValidation_Value_Sec = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[1].ToUpper();
                        }
                        else if (drValidation["VALIDATION_VALUE"].ToString().Contains("*="))
                        {
                            sDelimiter = "*=";
                            sValidation_Value_Primary = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[0].ToUpper();
                            sValidation_Value_Sec = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[1].ToUpper();
                        }
                        else if (drValidation["VALIDATION_VALUE"].ToString().Contains("=*"))
                        {
                            sDelimiter = "=*";
                            sValidation_Value_Primary = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[0].ToUpper();
                            sValidation_Value_Sec = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[1].ToUpper();
                        }
                        else if (drValidation["VALIDATION_VALUE"].ToString().Contains("**"))
                        {
                            sDelimiter = "**";
                            sValidation_Value_Primary = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[0].ToUpper();
                            sValidation_Value_Sec = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[1].ToUpper();
                        }
                        else if (drValidation["VALIDATION_VALUE"].ToString().Contains("##"))
                        {
                            sDelimiter = "##";
                            sValidation_Value_Primary = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[0].ToUpper();
                            sValidation_Value_Sec = drValidation["VALIDATION_VALUE"].ToString().Split(new string[] { sDelimiter }, StringSplitOptions.None)[1].ToUpper();
                        }
                    }
                    else
                        sValidation_Value_Primary = drValidation["VALIDATION_VALUE"].ToString().ToUpper();

                    if (drValidation["VALIDATION_FOR"].ToString().StartsWith("C."))
                    {
                        //sValidation_For = sValidation_For.Replace("C.", "");
                        if (dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"].ToString().Length > 0 && lstFreezedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                            continue;
                        else
                        {
                            List<string> lstConditionValues = new List<string>();
                            sValidation_For_Values = dtMasterContacts.Rows[iRowIndex][sValidation_For.Replace("C.", "")].ToString().ToUpper();
                            foreach (string s in lstConditionColumns)
                            {
                                if (s.StartsWith("C."))
                                    lstConditionValues.Add(dtMasterContacts.Rows[iRowIndex][s.Replace("C.", "")].ToString().ToUpper());
                                else if (s.StartsWith("M."))
                                    lstConditionValues.Add(dtMasterCompanies.Select("MASTER_ID = " + dtMasterContacts.Rows[iRowIndex]["MASTER_ID"])[0][s.Replace("M.", "")].ToString().ToUpper());
                                else
                                    lstConditionValues.Add(s.ToUpper());
                            }

                            if (lstConditionValues.Contains(sValidation_For_Values) || lstValidationTypeToIgnore.Contains(drValidation["VALIDATION_TYPE"].ToString(), StringComparer.OrdinalIgnoreCase))
                                Validation_Table(true, drValidation, "Contact", sValidation_Type, sValidation_For, lstConditionValues, sValidation_Value_Primary, sValidation_Value_Sec, dtMasterCompanies.Rows[0], dtMasterContacts.Rows[iRowIndex], iRowIndex, sDelimiter);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
            }
        }

        void CONTACT_Check_RejectList(int iRowIndex, bool IsDynamic)
        {

            foreach (DataRow drUncertain_Field in dtUncertainFields.Rows)
            {
                string sPickListCategory = drUncertain_Field["Picklist_category"].ToString();
                string sFieldName = string.Empty;

                if (drUncertain_Field["FieldName_LinkColumn"].ToString().Length > 0)
                    sFieldName = "FieldName_LinkColumn";
                else
                    sFieldName = "FieldName";

                if (dtPicklist.Select("PicklistCategory = '" + sPickListCategory + "' AND remarks = 'REJECTED' AND PicklistValue = '" + dtMasterContacts.Rows[iRowIndex][drUncertain_Field[sFieldName].ToString()].ToString().Replace("'", "''") + "'").Length > 0)
                {
                    int iContactID = 0;
                    if (dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"].ToString().Length > 0)
                        iContactID = Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"]);

                    if (IsDynamic)
                    {
                        DevComponents.DotNetBar.Controls.TextBoxX txtBox = new DevComponents.DotNetBar.Controls.TextBoxX();
                        foreach (DevComponents.DotNetBar.Controls.TextBoxX txt in lstContactControls)
                        {
                            if (txt.Name.ToUpper() == drUncertain_Field[sFieldName].ToString().ToUpper())
                            {
                                txtBox = txt;
                                break;
                            }
                        }
                        AddValidationResults("Contact", iRowIndex, Convert.ToInt32(dtMasterCompanies.Rows[iCompanyRowIndex]["MASTER_ID"]), iContactID, "", "REJECTED", drUncertain_Field[sFieldName].ToString(), dtMasterContacts.Rows[iRowIndex][drUncertain_Field[sFieldName].ToString()].ToString(), IsDynamic, 2, txtBox);
                    }
                    else
                        AddValidationResults("Contact", iRowIndex, Convert.ToInt32(dtMasterCompanies.Rows[iCompanyRowIndex]["MASTER_ID"]), iContactID, "", "REJECTED", drUncertain_Field[sFieldName].ToString(), dtMasterContacts.Rows[iRowIndex][drUncertain_Field[sFieldName].ToString()].ToString(), IsDynamic, 2);
                }
            }
        }


        //-----------------------------------------------------------------------------------------------------
        private void COMPANY_ValidateDisposals(DoWorkEventArgs e)
        {
            GV.sValidationMessage = "Checking Disposal empty";
            try
            {
                if (RunASynch(e))
                {
                    for (int i = 0; i < dtMasterCompanies.Rows.Count; i++)
                    {
                        if (dtMasterCompanies.Rows[i][GV.sAccessTo + "_PRIMARY_DISPOSAL"].ToString().Trim().Length == 0)
                            AddValidationResults("Company", i, Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"]), 0, "", "EMPTY", GV.sAccessTo + "_PRIMARY_DISPOSAL", "", false, 0);

                        if (dtMasterCompanies.Rows[i][GV.sAccessTo + "_SECONDARY_DISPOSAL"].ToString().Trim().Length == 0)
                            AddValidationResults("Company", i, Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"]), 0, "", "EMPTY", GV.sAccessTo + "_SECONDARY_DISPOSAL", "", false, 0);

                        if (dtRecordStatus.Select("TABLE_NAME = 'COMPANY' AND PRIMARY_STATUS = '" + dtMasterCompanies.Rows[i][GV.sAccessTo + "_PRIMARY_DISPOSAL"] + "' AND SECONDARY_STATUS = '" + dtMasterCompanies.Rows[i][GV.sAccessTo + "_SECONDARY_DISPOSAL"] + "' AND RESEARCH_TYPE = '" + GV.sAccessTo + "' AND OPERATION_TYPE LIKE '%Delete%'").Length > 0
                            && dtMasterContacts.Select("TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeValidated + ") OR WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeValidated + ")").Length > 0)
                        {
                            AddValidationResults("Company", i, Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"]), 0, "", "INVALIDDISPOSAL", GV.sAccessTo + "_PRIMARY_DISPOSAL", dtMasterCompanies.Rows[i][GV.sAccessTo + "_PRIMARY_DISPOSAL"].ToString(), false, 0);
                            AddValidationResults("Company", i, Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"]), 0, "", "INVALIDDISPOSAL", GV.sAccessTo + "_SECONDARY_DISPOSAL", dtMasterCompanies.Rows[i][GV.sAccessTo + "_SECONDARY_DISPOSAL"].ToString(), false, 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lstApplicationError.Add("Company ValidateDisposals : " + ex.Message);
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void COMPANY_UpdateChangedRecords()
        {
            try
            {
                GV.sValidationMessage = "Tagging Company with Name and Date";
                Regex rNumeric = new Regex(@"[^\d]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                Regex rAlphaNumeric = new Regex(@"[^0-9A-Za-z]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                DataRow[] drrCompanyFormatFields = dtFieldMasterCompany.Select("LEN(FORMATTING) > 0");

                for (int i = 0; i < dtMasterCompanies.Rows.Count; i++)
                {
                    #region Tag Company with Agent Name and Date
                    ////////////Tag Company with Agent Name and Date/////////////////////////
                    if (!lstFreezedMasterIDs.Contains(Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"])))
                    {
                        if (GV.sAccessTo == "TR")
                        {
                            if (GV.sUserType == "Agent" || dtMasterCompanies.Rows[i]["TR_DATECALLED"].ToString().Trim().Length == 0)//Change only for Agent not for admin--- if admin opens the fresh record then fill the  admin name
                            {
                                //dtMasterCompanies.Rows[i]["TR_PREVIOUS_DATECALLED"] = dtMasterCompanies.Rows[i]["TR_DATECALLED"];
                                dtMasterCompanies.Rows[i]["TR_PREVIOUS_DATECALLED"] = GM.GetDateTime();
                                dtMasterCompanies.Rows[i]["TR_DATECALLED"] = GM.GetDateTime();


                                dtMasterCompanies.Rows[i]["TR_PREVIOUS_AGENTNAME"] = GV.sEmployeeName;
                                //else

                                dtMasterCompanies.Rows[i]["TR_AGENTNAME"] = GV.sEmployeeName;

                                //Calculate Times Called
                                if (dtMasterCompanies.Rows[i]["TR_TIMESCALLED"].ToString().Length > 0)
                                    dtMasterCompanies.Rows[i]["TR_TIMESCALLED"] = Convert.ToInt32(dtMasterCompanies.Rows[i]["TR_TIMESCALLED"]) + 1; //If already Called then increment by one
                                else
                                    dtMasterCompanies.Rows[i]["TR_TIMESCALLED"] = 1; //First time calling

                                //if (dtMasterCompanies.Rows[i]["Scrape_Status"].ToString() == "1")
                                //    dtMasterCompanies.Rows[i]["Scrape_Status"] = "3";
                            }
                        }
                        else if (GV.sAccessTo == "WR")
                        {
                            if (GV.sUserType == "Agent" || dtMasterCompanies.Rows[i]["WR_DATE_OF_PROCESS"].ToString().Trim().Length == 0)
                            {

                                dtMasterCompanies.Rows[i]["WR_PREVIOUS_DATE_OF_PROCESS"] = GM.GetDateTime();
                                dtMasterCompanies.Rows[i]["WR_DATE_OF_PROCESS"] = GM.GetDateTime();

                                // if (GlobalVariables.sPreviousAgentName.Length == 0)//First time record updates both WR_AgentName and WR_Previous_AgentName with Current AgentName
                                dtMasterCompanies.Rows[i]["WR_PREVIOUS_AGENTNAME"] = GV.sEmployeeName;
                                // else
                                //   dtMasterCompanies.Rows[0]["WR_PREVIOUS_AGENTNAME"] = GlobalVariables.sPreviousAgentName;

                                dtMasterCompanies.Rows[i]["WR_AGENTNAME"] = GV.sEmployeeName;

                                //if (dtMasterCompanies.Rows[i]["Scrape_Status"].ToString() == "1")
                                //    dtMasterCompanies.Rows[i]["Scrape_Status"] = "3";
                            }
                        }

                        if (dtMasterCompanies.Rows[i]["CREATED_DATE"].ToString().Trim().Length == 0)
                        {
                            dtMasterCompanies.Rows[i]["CREATED_BY"] = GV.sEmployeeName;
                            dtMasterCompanies.Rows[i]["CREATED_DATE"] = GM.GetDateTime();
                        }

                        dtMasterCompanies.Rows[i]["UPDATED_BY"] = GV.sEmployeeName;
                        dtMasterCompanies.Rows[i]["UPDATED_DATE"] = GM.GetDateTime();

                        if(GV.sUserType == "QC")
                        {
                            dtMasterCompanies.Rows[i]["QC_BY"] = GV.sEmployeeName;
                            dtMasterCompanies.Rows[i]["QC_DATE"] = GM.GetDateTime();
                        }


                    }
                    else
                    {
                        if (GV.sAccessTo == "TR")//Revert Current_Agent for freezed companies
                            dtMasterCompanies.Rows[i]["TR_AGENTNAME"] = dtMasterCompanies.Rows[i]["TR_PREVIOUS_AGENTNAME"].ToString();
                        else if (GV.sAccessTo == "WR")
                            dtMasterCompanies.Rows[i]["WR_AGENTNAME"] = dtMasterCompanies.Rows[i]["WR_PREVIOUS_AGENTNAME"].ToString();
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
                    #endregion

                    #region Update Region
                    ////////////////Update Region////////////////////////////////////
                    string sCountry = dtMasterCompanies.Rows[i]["Country"].ToString().Trim();
                    if (sCountry.Length > 0)
                    {
                        DataRow[] drrRegion = dtCountryInformation.Select("CountryName = '" + sCountry.Replace("'", "''") + "'");
                        if (drrRegion.Length > 0 && drrRegion[0]["Region"].ToString().Length > 0)
                            dtMasterCompanies.Rows[i]["Region"] = drrRegion[0]["Region"].ToString();
                        else
                            dtMasterCompanies.Rows[i]["Region"] = DBNull.Value;
                    }
                    else
                        dtMasterCompanies.Rows[i]["Region"] = DBNull.Value;
                    /////////////////////////////////////////////////////////////////// 
                    #endregion

                    #region Auto Format All data based on FieldMaster Text Format specification
                    ////////////////////////Auto Format All data based on FieldMaster Text Format specification//////////////////////////////////////
                    for (int j = 0; j < drrCompanyFormatFields.Length; j++)//Format all Data
                    {
                        string sFormatText = dtMasterCompanies.Rows[i][drrCompanyFormatFields[j]["FIELD_NAME_TABLE"].ToString()].ToString();
                        dtMasterCompanies.Rows[i][drrCompanyFormatFields[j]["FIELD_NAME_TABLE"].ToString()] = TextFormatting(sFormatText, drrCompanyFormatFields[j]["FORMATTING"].ToString().Split('|').ToList(), i);
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
                    #endregion

                    #region Update GMT Time for Newly created company
                    ////////////////////////////////Update GMT Time for Newly created company////////////////////////////////////////////////
                    //if (dtMasterCompanies.Rows[i]["NEW_OR_EXISTING"].ToString().ToLower() == "new" && dtMasterCompanies.Rows[i]["HoursFromGMT"].ToString().Trim().Length == 0)
                    if (dtMasterCompanies.Rows[i]["Country"].ToString().Length > 0 && dtMasterCompanies.Rows[i]["HoursFromGMT"].ToString().Trim().Length == 0)
                    {
                        string sGMTCountryName = dtMasterCompanies.Rows[i]["Country"].ToString();
                        DataRow[] drCountryInformation = dtCountryInformation.Select("CountryName = '" + sGMTCountryName.Replace("'", "''") + "'");
                        if (drCountryInformation.Length > 0)
                            dtMasterCompanies.Rows[i]["HoursFromGMT"] = drCountryInformation[0]["HoursFromGMT"].ToString();
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
                    #endregion

                    #region Update Switchboard Trimmed
                    ///////////////////////////Update Switchboard Trimmed/////////////////////////////////////////////////////
                    string sTelephone = dtMasterCompanies.Rows[i]["SWITCHBOARD"].ToString();
                    sTelephone = rNumeric.Replace(sTelephone, string.Empty);
                    if (sTelephone.Length > 7)
                    {
                        sTelephone = sTelephone.Substring(sTelephone.Length - 8);
                        dtMasterCompanies.Rows[i]["SWITCHBOARD_TRIMMED"] = Convert.ToInt32(sTelephone);
                    }
                    else
                        dtMasterCompanies.Rows[i]["SWITCHBOARD_TRIMMED"] = DBNull.Value;
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////// 
                    #endregion

                    #region Comments Log

                    //Log comments to display like chant window
                    if (dtMasterCompanies.Rows[i][GV.sAccessTo + "_COMMENTS"].ToString().Trim().ToLower() != dtMasterCompaniesCopy.Rows[i][GV.sAccessTo + "_COMMENTS"].ToString().Trim().ToLower())
                    {
                        if (dtMasterCompanies.Rows[i][GV.sAccessTo + "_COMMENTS_LOG"].ToString().Trim().Length == 0)
                            dtMasterCompanies.Rows[i][GV.sAccessTo + "_COMMENTS_LOG"] = "[" + GV.sEmployeeName + "]:[" + GM.GetDateTime().ToString("dd/MM/yyyy hh:mm:ss tt") + "]:[" + dtMasterCompanies.Rows[i][GV.sAccessTo + "_COMMENTS"].ToString().Replace("]:[", " ").Replace("]|[", " ") + "]";
                        else
                            dtMasterCompanies.Rows[i][GV.sAccessTo + "_COMMENTS_LOG"] += "|[" + GV.sEmployeeName + "]:[" + GM.GetDateTime().ToString("dd/MM/yyyy hh:mm:ss tt") + "]:[" + dtMasterCompanies.Rows[i][GV.sAccessTo + "_COMMENTS"].ToString().Replace("]:[", " ").Replace("]|[", " ") + "]";
                    }

                    #endregion

                    #region UpdateNextCallDate

                    if (GV.sAccessTo == "TR")
                    {
                        DataRow[] drrDialConfig = dtDialConfig.Select("PicklistCategory = 'Dial_Restrict_CallCount'", "PICKLISTFIELD ASC");
                        if (drrDialConfig.Length > 0)
                        {
                            int iCallDuration = Convert.ToInt32(dtDialConfig.Select("PicklistCategory = 'Dial_Restrict_CallDuration'")[0]["PicklistValue"]);
                            foreach (DataRow drDialConfig in drrDialConfig)
                            {
                                int iCallCount = Convert.ToInt32(drDialConfig["PicklistValue"]);
                                int iDayLimit = Convert.ToInt32(drDialConfig["PicklistField"]);

                                if (GM.GetDateTime().DayOfWeek.ToString() == "Monday")
                                    iDayLimit += 2;

                                // Start from the last date and go backwards
                                int iDialCount = 0;
                                for (int j = dtDialLogger.Rows.Count - 1; j > 0; j--)
                                {
                                    // If the duration of the call is more than 20 secs add it to the iDialCount
                                    if (dtDialLogger.Rows[j]["RecordingID"].ToString() == "1" && TimeSpan.Parse(dtDialLogger.Rows[j]["Duration"].ToString()).TotalSeconds > iCallDuration)
                                    {
                                        iDialCount++;
                                        // Check if the number of calls is more than the allowed calls
                                        if (iDialCount >= iCallCount)
                                        {
                                            // Add the number of days from the earlier date.  This will mark the next date when this record is available for calls.
                                            dtMasterCompanies.Rows[i]["CALLS_ALLOWED_FROM"] = Convert.ToDateTime(dtDialLogger.Rows[j]["DATETIMESTAMP"]).AddDays(iDayLimit);
                                            break;
                                        }
                                    }
                                }



                                //DataRow[] drrLastThreeDayDialer = null;

                                //if (lstMasterIDs.Count > 1)
                                //{
                                //    string sSwitchboard = FormatTelephoneToDial(dtMasterCompanies.Rows[i]["SWITCHBOARD"].ToString());
                                //    drrLastThreeDayDialer = dtDialLogger.Select("RecordingID = 1 AND TelephoneNumber = '" + sSwitchboard + "' AND DATETIMESTAMP > '" + GM.GetDateTime().AddDays(-iDayLimit).ToString("dd/MM/yyyy") + " 23:59:59'");
                                //}
                                //else
                                //    drrLastThreeDayDialer = dtDialLogger.Select("RecordingID = 1 AND DATETIMESTAMP > '" + GM.GetDateTime().AddDays(-iDayLimit).ToString("dd/MM/yyyy") + " 23:59:59'");

                                //if (drrLastThreeDayDialer.Length > 0)
                                //{

                                //    foreach (DataRow drCall in drrLastThreeDayDialer)
                                //    {
                                //        if (TimeSpan.Parse(drCall["Duration"].ToString()).TotalSeconds > iCallDuration)
                                //            iDialCount++;
                                //    }

                                //    if (iDialCount >= iCallCount)
                                //    {
                                //        dtMasterCompanies.Rows[i]["CALLS_ALLOWED_FROM"] = dtNextCallDate;
                                //    }
                                //}
                            }
                        }
                    }
                    #endregion

                    #region Update Company Name Alpha
                    dtMasterCompanies.Rows[i]["COMPANY_NAME_ALPHA"] = rAlphaNumeric.Replace(dtMasterCompanies.Rows[i]["COMPANY_NAME"].ToString(), string.Empty);
                    #endregion

                    #region Update Switchboard DND in Blocking Table

                    if (GV.sAccessTo == "TR")
                    {
                        string sDNDPrimaryDisposal = dtMasterCompanies.Rows[i]["TR_PRIMARY_DISPOSAL"].ToString();
                        string sDNDSecondaryDisposal = dtMasterCompanies.Rows[i]["TR_SECONDARY_DISPOSAL"].ToString();
                        if (dtRecordStatus.Select("Operation_Type LIKE '%DND%' AND Research_Type = 'TR' AND Primary_Status = '" + sDNDPrimaryDisposal + "' AND Secondary_Status = '" + sDNDSecondaryDisposal + "'").Length > 0)
                        {
                            string sDNDSwitchboard = Regex.Replace(dtMasterCompanies.Rows[i]["SWITCHBOARD"].ToString(), "[^0-9]", "");
                            if (sDNDSwitchboard.Length > 7)
                            {
                                if (dtBlock.Select("TABLE = 'COMPANY' AND FIELD = 'SWITCHBOARD' AND BLOCK_TYPE = 'CALL' AND MATCH_TYPE = 'SWITCHBOARD' AND VALUE = '" + sDNDSwitchboard + "'").Length == 0)
                                {
                                    if (GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT 1 FROM c_block WHERE `VALUE` = '" + sDNDSwitchboard + "' AND BLOCK_TYPE = 'CALL' AND MATCH_TYPE = 'SWITCHBOARD' AND BLOCK_REQUEST_FROM = 'Campaign Manager' AND BLOCK_EXPIRY > CURDATE();").Rows.Count == 0)
                                    {
                                        GV.MYSQL.BAL_ExecuteQueryMySQL(@"INSERT INTO c_block (PROJECT_ID, `TABLE`, FIELD, VALUE, BLOCK_TYPE, MATCH_TYPE, BLOCK_TO, BLOCK_EXPIRY, BLOCK_REQUEST_FROM, BLOCK_DESCRIPTION, UPDATED_DATE, UPDATED_BY)
                                                               VALUES('ALL', 'COMPANY', 'SWITCHBOARD', '" + sDNDSwitchboard + "', 'CALL', 'SWITCHBOARD', 'ALL', DATE_ADD(CURDATE(), INTERVAL 3 MONTH), 'Campaign Manager', 'Automated DND request raised from Campaign Manager. ProjectID:" + GV.sProjectID + "|CompanyID:" + dtMasterCompanies.Rows[i]["MASTER_ID"] + "', NOW(), '" + GV.sEmployeeName + "');");
                                    }
                                }
                            }
                        }
                    } 
                    #endregion
                    
                }


            }
            catch (Exception ex)
            {
                GV.iNotifier[4] = 1;
                //CloseNotifier();
                lstApplicationError.Add("Company UpdateChangedRecords : " + ex.Message);
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void CONTACT_RemoveEmptyRecords(DoWorkEventArgs e)
        {
            GV.sValidationMessage = "Removing Empty contacts";
            try
            {
                if (RunASynch(e))
                {
                    int iRowNumber = 0;
                    List<DataRow> lstdrTobeDeleted = new List<DataRow>();
                    for (int i = 0; i < dtMasterContacts.Rows.Count; i++)
                    {
                        iRowNumber++;

                        if ((dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString() + dtMasterContacts.Rows[i]["FIRST_NAME"] + dtMasterContacts.Rows[i]["LAST_NAME"] + dtMasterContacts.Rows[i]["JOB_TITLE"] + dtMasterContacts.Rows[i]["CONTACT_EMAIL"] + dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"]).Trim().Length == 0)
                        {
                            //dtMasterContacts.Rows[i].Delete();
                            lstdrTobeDeleted.Add(dtMasterContacts.Rows[i]);//Auto Remove Empty Contacts
                            iRowNumber--;
                            continue;//Nothing to do with deleted row
                        }
                    }

                    if (lstdrTobeDeleted.Count > 0)//Remove All Empty Row(s)
                    {
                        foreach (DataRow dr in lstdrTobeDeleted)
                            dr.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                //Cancel_Asynch("Error Occured...",e);
                lstApplicationError.Add("Contact RemoveEmptyRecords : " + ex.Message);
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void CONTACT_Validate_ContactStatus(DoWorkEventArgs e)
        {
            GV.sValidationMessage = "Checking Contact Status";

            try
            {
                if (RunASynch(e))
                {
                    if (GV.sUserType == "Agent")
                    {
                        string sNewrecordContactStatus = string.Empty;
                        foreach (string s in GV.lstNewRecordContactStatus)
                        {
                            if (sNewrecordContactStatus.Trim().Length == 0)
                                sNewrecordContactStatus = s;
                            else
                                sNewrecordContactStatus += ", " + s;
                        }

                        for (int i = 0; i < dtMasterContacts.Rows.Count; i++)
                        {
                            int iContact_ID = 0;
                            int iCompanyID = Convert.ToInt32(dtMasterContacts.Rows[i]["MASTER_ID"]);
                            if (dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString().Length > 0)
                                iContact_ID = Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"]);

                            if (dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"].ToString().Trim().Length == 0)
                            {
                                AddValidationResults("Contact", i, iCompanyID, iContact_ID, "", "EMPTY", GV.sAccessTo + "_CONTACT_STATUS", "", false, 2);
                                continue;
                            }
                            else if (GV.lstNeutralContactStatus.Contains(dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                continue;
                            else//Ignore dummy contact status like 'Not Verified'
                            {
                                DataRow[] drrPickListContactstatus = dtRecordStatus.Select("Research_Type = '" + GV.sAccessTo + "' AND Table_Name ='Contact' AND Primary_Status = '" + dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"].ToString() + "'");
                                if (drrPickListContactstatus.Length > 0)
                                {
                                    string sRemarks = drrPickListContactstatus[0]["Operation_Type"].ToString();
                                    if (sRemarks.Contains("Verify") || sRemarks.Contains("New") || sRemarks.Contains("Update") || sRemarks.Contains("Replace"))
                                    { /*Do Nothing*/}
                                    else
                                        continue;
                                }
                                else
                                    continue;
                            }

                            if (dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString().Length > 0 && (lstFreezedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])) || lstRecordsToUnlock.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"]))))
                            { /*Do Nothing*/}//Ignore freezed and Bounced records
                            else
                            {
                                if ((GV.sAccessTo == "TR" && GV.lstTRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString())) || (GV.sAccessTo == "WR" && GV.lstWRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString())))
                                {
                                    if (dtMasterContacts.Rows[i]["NEW_OR_EXISTING"].ToString().ToUpper() == "NEW")
                                    {
                                        if (dtMasterContacts.Rows[i][GV.sOppositAccess + "_UPDATED_DATE"].ToString().Length > 0)
                                        {
                                            if (GV.lstNewRecordContactStatus.Contains(dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                                AddValidationResults("Contact", i, iCompanyID, iContact_ID, "Existing contact cannot have <font color= 'DarkCyan'>Contact Status</font> New", false, 2);
                                        }
                                        else if (dtMasterContacts.Rows[i][GV.sAccessTo + "_UPDATED_DATE"].ToString().Length > 0 && dtMasterContacts.Rows[i][GV.sOppositAccess + "_UPDATED_DATE"].ToString().Length == 0)
                                        {
                                            if (!GV.lstNewRecordContactStatus.Contains(dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                            {
                                                if (GV.lstReplacementRecordContactStatus.Contains(dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                                {
                                                    if (GV.sReplacementOptionContactStatus.Length > 0 && dtMasterContacts.Select(GV.sAccessTo + "_CONTACT_STATUS IN (" + GV.sReplacementOptionContactStatus + ")").Length > 0)
                                                        continue;
                                                    else
                                                        AddValidationResults("Contact", i, iCompanyID, iContact_ID, "Contact Left not found in contact list. Replacement not possible", false, 2);
                                                }
                                                else
                                                    AddValidationResults("Contact", i, iCompanyID, iContact_ID, "New contact must have <font color= 'DarkCyan'>Contact Status</font> New", false, 2);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (GV.lstNewRecordContactStatus.Contains(dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                            AddValidationResults("Contact", i, iCompanyID, iContact_ID, "Existing contact cannot have <font color= 'DarkCyan'>Contact Status</font> New", false, 2);
                                        else
                                        {
                                            if (GV.lstReplacementRecordContactStatus.Contains(dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                            {
                                                if (GV.sReplacementOptionContactStatus.Length > 0 && dtMasterContacts.Select(GV.sAccessTo + "_CONTACT_STATUS IN (" + GV.sReplacementOptionContactStatus + ")").Length > 0)
                                                    continue;
                                                else
                                                    AddValidationResults("Contact", i, iCompanyID, iContact_ID, "Contact Left not found in contact list. Replacement not possible", false, 2);
                                            }
                                            else
                                            {
                                                DataRow drMasterContactsCopy = dtMasterContactsCopy.Select("CONTACT_ID_P = " + dtMasterContacts.Rows[i]["CONTACT_ID_P"])[0];
                                                bool IsContactChanged = false;
                                                foreach (DataRow drColumnName in dtFieldMasterContact.Rows)
                                                {
                                                    if (drColumnName["OPERATION_UPDATE_IGNORE"].ToString().ToUpper() == "Y" || drColumnName["FIELD_NAME_TABLE"].ToString().ToUpper() == "TR_CONTACT_STATUS" || drColumnName["FIELD_NAME_TABLE"].ToString().ToUpper() == "WR_CONTACT_STATUS")
                                                        continue;
                                                    if (drMasterContactsCopy[drColumnName["FIELD_NAME_TABLE"].ToString()].ToString().Replace(" ", string.Empty).ToUpper() == dtMasterContacts.Rows[i][drColumnName["FIELD_NAME_TABLE"].ToString()].ToString().Replace(" ", string.Empty).ToUpper())
                                                    { /*Do Nothing*/}
                                                    else
                                                    {
                                                        if (drColumnName["FORMATTING"].ToString().Contains("TELEPHONECONTACTS"))//Telephone contry code is automatically added if not already exist.. It is not user Updated data
                                                        {//Handle automatic telephone formatting. Do not mark is as change
                                                            if (TelephoneFormat(drMasterContactsCopy[drColumnName["FIELD_NAME_TABLE"].ToString()].ToString(), "Contacts", i).Replace(" ", string.Empty) != dtMasterContacts.Rows[i][drColumnName["FIELD_NAME_TABLE"].ToString()].ToString().Replace(" ", string.Empty))
                                                            {
                                                                IsContactChanged = true;
                                                                break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            IsContactChanged = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (IsContactChanged)
                                                {
                                                    if (!GV.lstChangedRecordContactStatus.Contains(dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                                        AddValidationResults("Contact", i, iCompanyID, iContact_ID, "Changes detected in contact. <font color= 'DarkCyan'>Contact Status</font> must be update", false, 2);
                                                }
                                                else
                                                {
                                                    if (!GV.lstUnchangedRecordContactStatus.Contains(dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                                        AddValidationResults("Contact", i, iCompanyID, iContact_ID, "Changes not found. <font color= 'DarkCyan'>Contact Status</font> must be verified", false, 2);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Cancel_Asynch("Error Occured...",e);
                lstApplicationError.Add("Validate ContactStatus : " + ex.Message);
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
            }
        }

        string Delimiter(string sFieldName, out string sDelimiter, out string sDefaultEquals)
        {
            if (sFieldName.Contains("=="))
            {
                sDelimiter = "==";
                sDefaultEquals = sFieldName.Split(new string[] { "==" }, StringSplitOptions.None)[1].Trim().ToUpper();
                return sFieldName.Split(new string[] { "==" }, StringSplitOptions.None)[0];
            }
            else if (sFieldName.Contains("##"))
            {
                sDelimiter = "##";
                sDefaultEquals = sFieldName.Split(new string[] { "##" }, StringSplitOptions.None)[1].Trim().ToUpper();
                return sFieldName.Split(new string[] { "##" }, StringSplitOptions.None)[0];
            }
            else if (sFieldName.Contains("*="))
            {
                sDelimiter = "*=";
                sDefaultEquals = sFieldName.Split(new string[] { "*=" }, StringSplitOptions.None)[1].Trim().ToUpper();
                return sFieldName.Split(new string[] { "*=" }, StringSplitOptions.None)[0];
            }
            else if (sFieldName.Contains("=*"))
            {
                sDelimiter = "=*";
                sDefaultEquals = sFieldName.Split(new string[] { "=*" }, StringSplitOptions.None)[1].Trim().ToUpper();
                return sFieldName.Split(new string[] { "=*" }, StringSplitOptions.None)[0];
            }
            else if (sFieldName.Contains("**"))
            {
                sDelimiter = "**";
                sDefaultEquals = sFieldName.Split(new string[] { "**" }, StringSplitOptions.None)[1].Trim().ToUpper();
                return sFieldName.Split(new string[] { "**" }, StringSplitOptions.None)[0];
            }
            else if (sFieldName.Contains("*#"))
            {
                sDelimiter = "*#";
                sDefaultEquals = sFieldName.Split(new string[] { "*#" }, StringSplitOptions.None)[1].Trim().ToUpper();
                return sFieldName.Split(new string[] { "*#" }, StringSplitOptions.None)[0];
            }
            else
            {
                sDelimiter = string.Empty;
                sDefaultEquals = string.Empty;
                return string.Empty;
            }
        }



        //private void Validation_Table(string sValidation_Type, string sValidation_For, List<string> lstCondition, List<string> lstValidation_Value_Primary, List<string> lstValidation_Value_Secoendary)
        private void Validation_Table(bool IsDynamic, DataRow drValidation, string sTableName, string saValidation_Type, string sValidation_For, List<string> lstConditionValue, string sValidation_Value_PrimaryCols, string sValidation_Value_SecCols, DataRow drCompany, DataRow drContact, int iRowNumber, string sDelimiter)
        {
            string sValidation = string.Empty;

            try
            {

                List<string> lstPrimaryCols = sValidation_Value_PrimaryCols.ToUpper().Split('|').ToList();
                List<string> lstSecoendryCols = sValidation_Value_SecCols.ToUpper().Split('|').ToList();

                List<string> lstPrimaryValues = new List<string>();
                List<string> lstSecoendryValues = new List<string>();


                string sValidation_For_Value = string.Empty;
                string sPrimaryCol = string.Empty;
                DataRow drRowToBeValidated;
                if (sTableName == "Company")
                {
                    drRowToBeValidated = drCompany;
                    drContact = dtMasterContactsCopy.NewRow();
                    sValidation_For_Value = drRowToBeValidated[sValidation_For.Replace("M.", "")].ToString().Trim().ToUpper();
                }
                else
                {
                    drRowToBeValidated = drContact;
                    sValidation_For_Value = drRowToBeValidated[sValidation_For.Replace("C.", "")].ToString().Trim().ToUpper();
                }

                int iCompanyID = Convert.ToInt32(drRowToBeValidated["Master_ID"]);
                int iContactID = 0;
                int iErrorArea = 0;
                if (sTableName == "Contact" && drRowToBeValidated["Contact_ID_P"].ToString().Length > 0)
                {
                    iErrorArea = 2;
                    iContactID = Convert.ToInt32(drRowToBeValidated["Contact_ID_P"]);
                }

                string sPrimaryVal = string.Empty;

                foreach (string sPrimaryCols in lstPrimaryCols)
                {
                    if (sPrimaryCols.StartsWith("M."))
                    {
                        if (drCompany[sPrimaryCols.Replace("M.", "")].ToString().Trim().Length > 0)
                        {
                            lstPrimaryValues.Add(drCompany[sPrimaryCols.Replace("M.", "")].ToString().Trim().ToUpper());
                            sPrimaryVal = drCompany[sPrimaryCols.Replace("M.", "")].ToString().Trim();
                        }
                    }
                    else if (sPrimaryCols.StartsWith("C."))
                    {
                        if (drContact[sPrimaryCols.Replace("C.", "")].ToString().Trim().Length > 0)
                        {
                            lstPrimaryValues.Add(drContact[sPrimaryCols.Replace("C.", "")].ToString().Trim().ToUpper());
                            sPrimaryVal = drContact[sPrimaryCols.Replace("C.", "")].ToString().Trim();
                        }
                    }
                    else if (sPrimaryCols.Length > 0)
                    {
                        lstPrimaryValues.Add(sPrimaryCols);
                        sPrimaryVal = sPrimaryCols;
                    }
                }

                if (lstPrimaryCols.Count == 1)
                    sPrimaryCol = lstPrimaryCols[0].Replace("C.", "").Replace("M.", "");

                foreach (string sSecoendryCols in lstSecoendryCols)
                {
                    if (sSecoendryCols.StartsWith("M."))
                    {
                        //if(drCompany[sSecoendryCols.Replace("M.", "")].ToString().Length > 0)
                        lstSecoendryValues.Add(drCompany[sSecoendryCols.Replace("M.", "")].ToString().Trim().ToUpper());
                    }
                    else if (sSecoendryCols.StartsWith("C."))
                    {
                        //if(drContact[sSecoendryCols.Replace("C.", "")].ToString().Length > 0)
                        lstSecoendryValues.Add(drContact[sSecoendryCols.Replace("C.", "")].ToString().Trim().ToUpper());
                    }
                    else //if (sSecoendryCols.Length > 0)
                        lstSecoendryValues.Add(sSecoendryCols);
                }


                TextBox txt = null;

                if (IsDynamic)
                {
                    txt = new TextBox();
                    foreach (TextBox C in lstContactControls)
                    {
                        if (C.Name.ToUpper() == sValidation_Value_PrimaryCols.Replace("C.", ""))
                        {
                            txt = C;
                            break;
                        }
                    }

                    //   if (sValidation_For == "C." + GV.sAccessTo + "_CONTACT_STATUS")
                    {
                        foreach (TextBox C in lstContactControls)
                        {
                            if (C.Name.ToUpper() == GV.sAccessTo + "_CONTACT_STATUS")
                            {
                                if (C.Text.Length == 0)
                                {
                                    C.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
                                    C.Refresh();
                                    return;
                                }
                            }
                        }
                    }
                }


                switch (saValidation_Type)
                {
                    case "MANDATORYFIELDS":
                        //Check Dupe within contactname
                        //if (lstPrimaryCols.Count > 1)//if validation field contains multiple column(Eg: TITLE|FIRSTNAME|LASTNAME)
                        //{
                        //    if (lstPrimaryValues.Count == 0)
                        //        AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "EMPTY", "", "", sValidation_For, lstConditionValue.Aggregate((x, y) => x + "," + y), false, iErrorArea);
                        //}
                        //else
                        //{                            
                        if (lstPrimaryValues.Count == 0)
                            AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "EMPTY", lstPrimaryCols.Aggregate((x, y) => x + "|" + y), "", sValidation_For, lstConditionValue.Aggregate((x, y) => x + "|" + y), IsDynamic, iErrorArea, txt);
                        //}
                        break;

                    case "EMAILCOMPANYCHECK":
                        //Check Email format Company Email
                        if (lstPrimaryValues.Count > 0)
                        {
                            if (GM.Email_Check(lstPrimaryValues[0]))
                            {
                                string sEmailContentCheck = CONTACT_EmailContentCheck(lstPrimaryValues[0], "COMPANYEMAIL").Trim();
                                if (sEmailContentCheck.Length > 0)
                                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "COMPANYEMAIL", sPrimaryCol, lstPrimaryValues[0], IsDynamic, iErrorArea, txt);
                            }
                            else
                                AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "INVALID", sPrimaryCol, lstPrimaryValues[0], IsDynamic, iErrorArea, txt);
                        }
                        break;

                    case "EMAILPUBLICDOMAINCHECK":

                        //Check Email format Public Domin Emails
                        if (lstPrimaryValues.Count > 0)
                        {
                            if (GM.Email_Check(lstPrimaryValues[0]))
                            {
                                string sEmailContentCheck = CONTACT_EmailContentCheck(lstPrimaryValues[0], "PUBLICDOMAIN").Trim();
                                if (sEmailContentCheck.Length > 0)
                                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "PUBLICDOMAIN", sPrimaryCol, lstPrimaryValues[0], IsDynamic, iErrorArea, txt);
                            }
                            else
                                AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "INVALID", sPrimaryCol, lstPrimaryValues[0], IsDynamic, iErrorArea, txt);
                        }
                        break;

                    case "EMAILFORMATCHECK":
                        //Check Email format
                        if (lstPrimaryValues.Count > 0)
                        {
                            if (!GM.Email_Check(lstPrimaryValues[0]))
                                AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "INVALID", sPrimaryCol, lstPrimaryValues[0], IsDynamic, iErrorArea, txt);
                        }
                        else
                            AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "EMPTY", sPrimaryCol, lstPrimaryValues[0], IsDynamic, iErrorArea, txt);
                        break;

                    case "EMAILDUPECHECK":
                        //Check Duplicate Email
                        if (GV.sAllowDuplicateEmail == "N" && lstPrimaryValues.Count > 0)
                        {
                            //Replace due to single quote(') in string
                            if (dtMasterContacts.Select(sPrimaryCol + " = '" + lstPrimaryValues[0].Replace("'", "''") + "' AND (TR_CONTACT_STATUS IN (" + GV.sEmailCheckContactStatus + ") OR WR_CONTACT_STATUS IN (" + GV.sEmailCheckContactStatus + "))").Length > 1)// && (!(sValidation.Contains("Duplicate Email Found"))))
                            {
                                string sIDs = string.Empty;
                                for (int i = 0; i < dtMasterContacts.Rows.Count; i++)//Show row number of contact
                                {
                                    if
                                    (
                                         dtMasterContacts.Rows[i]["MASTER_ID"].ToString() == iCompanyID.ToString()
                                         &&
                                         dtMasterContacts.Rows[i][sPrimaryCol].ToString().Trim().ToLower() == lstPrimaryValues[0].Trim().ToLower()
                                         &&
                                         (
                                             (GV.lstEmailChackContactStatus.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                             ||
                                             (GV.lstEmailChackContactStatus.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                         )
                                     )
                                    {
                                        if (sIDs.Length > 0)
                                            sIDs += ", " + (i + 1).ToString();
                                        else
                                            sIDs = (i + 1).ToString();
                                    }
                                }
                                AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "EMAILDUPE", sPrimaryCol, lstPrimaryValues[0], "", sIDs, IsDynamic, iErrorArea, txt);
                            }
                        }
                        break;

                    case "JOBTITLESPELLCHECK":
                        //Spell Check Job Title
                        if (lstPrimaryValues.Count > 0 && GV.sSpellCheckJobTitle == "Y")
                        {
                            if (dtPicklist.Select("PicklistCategory='Jobtitle' AND PicklistValue = '" + lstPrimaryValues[0].Replace("'", "''") + "'").Length == 0)//If jobtitle exist in list then do not check
                            {
                                if (!SpellCheck(sPrimaryVal))
                                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "INVALID", sPrimaryCol, lstPrimaryValues[0], IsDynamic, iErrorArea, txt);
                            }
                        }
                        break;

                    case "JOBTITLEDUPECHECK":
                        //Check Duplicate Job Title
                        if (GV.sAllowDuplicateJobTitle == "N")
                        {
                            string sDupeIDs = string.Empty;
                            if (dtMasterContacts.Select(sPrimaryCol + " = '" + lstPrimaryValues[0].Replace("'", "''") + "' AND (TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeValidated + ") OR WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeValidated + "))").Length > 1)// && (!(sValidation.Contains("Duplicate Email Found"))))
                            {
                                for (int i = 0; i < dtMasterContacts.Rows.Count; i++)
                                {
                                    if
                                    (
                                        dtMasterContacts.Rows[i]["MASTER_ID"].ToString() == iCompanyID.ToString()
                                        &&
                                        dtMasterContacts.Rows[i][sPrimaryCol].ToString().Trim().ToUpper() == lstPrimaryValues[0].Trim()
                                        &&
                                        (
                                            (GV.lstEmailChackContactStatus.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                            ||
                                            (GV.lstEmailChackContactStatus.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                        )
                                    )
                                    {
                                        if (sDupeIDs.Length > 0)
                                            sDupeIDs += ", " + i + 1;
                                        else
                                            sDupeIDs = (i + 1).ToString();
                                    }
                                }
                            }
                            AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "JOBTITLEDUPE", sPrimaryCol, lstPrimaryValues[0], "", sDupeIDs, IsDynamic, iErrorArea, txt);
                        }
                        break;

                    case "NAMEDUPECHECK":
                        //Check Dupe within contactname

                        if (dtMasterContacts.Select("FIRST_NAME = '" + drRowToBeValidated["FIRST_NAME"].ToString().Replace("'", "''") + "' AND LAST_NAME = '" + drRowToBeValidated["LAST_NAME"].ToString().Replace("'", "''") + "' AND (TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeValidated + ") OR WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeValidated + "))").Length > 1)// Name Dupe Check
                        {
                            string sValueToBeVerified = drRowToBeValidated["FIRST_NAME"] + " " + drRowToBeValidated["LAST_NAME"];
                            string sIDs = string.Empty;
                            string sFName = drRowToBeValidated["FIRST_NAME"].ToString().Trim().ToLower();
                            string sLName = drRowToBeValidated["LAST_NAME"].ToString().Trim().ToLower();
                            for (int i = 0; i < dtMasterContacts.Rows.Count; i++)//Show row number of contact
                            {
                                if
                                (
                                    (
                                        dtMasterContacts.Rows[i]["MASTER_ID"].ToString() == iCompanyID.ToString()
                                            &&
                                        dtMasterContacts.Rows[i]["FIRST_NAME"].ToString().Trim().ToLower() == sFName
                                            &&
                                        dtMasterContacts.Rows[i]["LAST_NAME"].ToString().Trim().ToLower() == sLName
                                    )
                                    &&
                                    (
                                        GV.lstTRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase)
                                            ||
                                        GV.lstWRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase)
                                    )
                                )
                                {
                                    if (sIDs.Length > 0)
                                        sIDs += ", " + (i + 1);
                                    else
                                        sIDs = (i + 1).ToString();
                                }
                            }
                            AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "NAMEDUPE", "FIRST_NAME", sValueToBeVerified, "", sIDs, IsDynamic, iErrorArea, txt);
                        }
                        break;

                    case "MANDATORYCONTACTSTATUSCONTAINS":

                        if (dtMasterContacts.Select(sPrimaryCol + " IN(" + GM.ListToQueryString(lstSecoendryValues, "String") + ")").Length == 0)
                            AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "MANDATORYCONTACTSTATUSCONTAINS", sPrimaryCol, lstSecoendryValues.Aggregate((x, y) => x + "|" + y), sValidation_For.Replace("M.", "").Replace("C.", ""), drValidation["CONDITION"].ToString(), false, iErrorArea);
                        break;

                    case "MANDATORYCONTACTSTATUSNOTCONTAINS":

                        if (dtMasterContacts.Select(sPrimaryCol + " IN(" + GM.ListToQueryString(lstSecoendryValues, "String") + ")").Length > 0)
                            AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, "", "MANDATORYCONTACTSTATUSNOTCONTAINS", sPrimaryCol, lstSecoendryValues.Aggregate((x, y) => x + "|" + y), sValidation_For.Replace("M.", "").Replace("C.", ""), lstConditionValue.Aggregate((x, y) => x + "|" + y), false, iErrorArea);
                        break;

                    case "EQUALS": // IF TR_CONTACT_STATUS - "EQUALS" - NEW AND COMPLETE - LAST_NAME=Prakash
                        foreach (string sConditionValue in lstConditionValue)
                        {
                            if (sValidation_For_Value == sConditionValue)
                            {
                                Exceptional_ValidationType(txt, sDelimiter, (lstPrimaryValues.Count > 0 ? lstPrimaryValues[0] : ""), lstPrimaryCols[0], lstSecoendryValues, lstConditionValue, sValidation_For, sTableName, iRowNumber, "IS", iCompanyID, iContactID, IsDynamic, iErrorArea);
                                return;
                            }
                        }
                        break;

                    case "NOTEQUALS": // IF TR_CONTACT_STATUS - "EQUALS" - NEW AND COMPLETE - LAST_NAME=Prakash
                        foreach (string sConditionValue in lstConditionValue)
                        {
                            if (sValidation_For_Value != sConditionValue)
                            {
                                Exceptional_ValidationType(txt, sDelimiter, (lstPrimaryValues.Count > 0 ? lstPrimaryValues[0] : ""), lstPrimaryCols[0], lstSecoendryValues, lstConditionValue, sValidation_For, sTableName, iRowNumber, "IS NOT", iCompanyID, iContactID, IsDynamic, iErrorArea);
                                return;
                            }
                        }
                        break;

                    case "STARTSWITH": // IF TR_CONTACT_STATUS - "EQUALS" - NEW AND COMPLETE - LAST_NAME=Prakash
                        foreach (string sConditionValue in lstConditionValue)
                        {
                            if (sValidation_For_Value.StartsWith(sConditionValue))
                            {
                                Exceptional_ValidationType(txt, sDelimiter, (lstPrimaryValues.Count > 0 ? lstPrimaryValues[0] : ""), lstPrimaryCols[0], lstSecoendryValues, lstConditionValue, sValidation_For, sTableName, iRowNumber, "STARTS WITH", iCompanyID, iContactID, IsDynamic, iErrorArea);
                                return;
                            }
                        }
                        break;

                    case "ENDSWITH": // IF TR_CONTACT_STATUS - "EQUALS" - NEW AND COMPLETE - LAST_NAME=Prakash
                        foreach (string sConditionValue in lstConditionValue)
                        {
                            if (sValidation_For_Value.EndsWith(sConditionValue))
                            {
                                Exceptional_ValidationType(txt, sDelimiter, (lstPrimaryValues.Count > 0 ? lstPrimaryValues[0] : ""), lstPrimaryCols[0], lstSecoendryValues, lstConditionValue, sValidation_For, sTableName, iRowNumber, "ENDS WITH", iCompanyID, iContactID, IsDynamic, iErrorArea);
                                return;
                            }
                        }
                        break;

                    case "CONTAINS": // IF TR_CONTACT_STATUS - "EQUALS" - NEW AND COMPLETE - LAST_NAME=Prakash
                        foreach (string sConditionValue in lstConditionValue)
                        {
                            if (sValidation_For_Value.Contains(sConditionValue))
                            {
                                Exceptional_ValidationType(txt, sDelimiter, (lstPrimaryValues.Count > 0 ? lstPrimaryValues[0] : ""), lstPrimaryCols[0], lstSecoendryValues, lstConditionValue, sValidation_For, sTableName, iRowNumber, "CONTAINS", iCompanyID, iContactID, IsDynamic, iErrorArea);
                                return;
                            }
                        }
                        break;

                    case "NOTCONTAINS": // IF TR_CONTACT_STATUS - "EQUALS" - NEW AND COMPLETE - LAST_NAME=Prakash
                        foreach (string sConditionValue in lstConditionValue)
                        {
                            if (!sValidation_For_Value.Contains(sConditionValue))
                            {
                                Exceptional_ValidationType(txt, sDelimiter, (lstPrimaryValues.Count > 0 ? lstPrimaryValues[0] : ""), lstPrimaryCols[0], lstSecoendryValues, lstConditionValue, sValidation_For, sTableName, iRowNumber, "DOES NOT CONTAINS", iCompanyID, iContactID, IsDynamic, iErrorArea);
                                return;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                //Cancel_Asynch("Error Occured...",e);
                lstApplicationError.Add(sTableName + " ValidationTable : " + ex.Message);
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void Exceptional_ValidationType(TextBox txt, string sDelimiter, string sPrimaryVals, string sPrimaryCols, List<string> lstSecoendryVals, List<string> lstConditionValue, string sValidation_For, string sTableName, int iRowNumber, string sMessage, int iCompanyID, int iContactID, bool IsDynamic, int iErrorArea)
        {
            switch (sDelimiter)
            {
                case "==":
                    bool IsEqual = false;

                    foreach (string sSecVals in lstSecoendryVals)
                    {
                        if (sPrimaryVals == sSecVals)
                        {
                            IsEqual = true;
                            break;
                        }
                    }
                    if (!IsEqual)
                        AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must be ", sMessage, sPrimaryCols, lstSecoendryVals.Aggregate((x, y) => x + "|" + y), sValidation_For, lstConditionValue.Aggregate((x, y) => x + "|" + y), IsDynamic, iErrorArea, txt);
                    break;

                case "*=":
                    bool StartsWith = false;
                    foreach (string sSecVals in lstSecoendryVals)
                    {
                        if (sPrimaryVals.StartsWith(sSecVals))
                        {
                            StartsWith = true;
                            break;
                        }
                    }
                    if (!StartsWith)
                        AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must start with ", sMessage, sPrimaryCols, lstSecoendryVals.Aggregate((x, y) => x + "|" + y), sValidation_For, lstConditionValue.Aggregate((x, y) => x + "|" + y), IsDynamic, iErrorArea, txt);
                    break;

                case "=*":
                    bool EndsWith = false;
                    foreach (string sSecVals in lstSecoendryVals)
                    {
                        if (sPrimaryVals.EndsWith(sSecVals))
                        {
                            EndsWith = true;
                            break;
                        }
                    }

                    if (!EndsWith)
                        AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must end with ", sMessage, sPrimaryCols, lstSecoendryVals.Aggregate((x, y) => x + "|" + y), sValidation_For, lstConditionValue.Aggregate((x, y) => x + "|" + y), IsDynamic, iErrorArea, txt);
                    break;

                case "**":
                    bool Contains = false;
                    foreach (string sSecVals in lstSecoendryVals)
                    {
                        if (sPrimaryVals.Contains(sSecVals))
                        {
                            Contains = true;
                            break;
                        }
                    }
                    if (!Contains)
                        AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must contain ", sMessage, sPrimaryCols, lstSecoendryVals.Aggregate((x, y) => x + "|" + y), sValidation_For, lstConditionValue.Aggregate((x, y) => x + "|" + y), IsDynamic, iErrorArea, txt);

                    break;
                case "##":
                    bool NotEquals = false;
                    foreach (string sSecVals in lstSecoendryVals)
                    {
                        if (sPrimaryVals == sSecVals)
                        {
                            NotEquals = true;
                            break;
                        }
                    }
                    if (NotEquals)
                        AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must not equal ", sMessage, sPrimaryCols, lstSecoendryVals.Aggregate((x, y) => x + "|" + y), sValidation_For, lstConditionValue.Aggregate((x, y) => x + "|" + y), IsDynamic, iErrorArea, txt);
                    break;

                case "*#":
                    bool NotContains = false;
                    foreach (string sSecVals in lstSecoendryVals)
                    {
                        if (sPrimaryVals.Contains(sSecVals))
                        {
                            NotContains = true;
                            break;
                        }
                    }
                    if (NotContains)
                        AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must not contain ", sMessage, sPrimaryCols, lstSecoendryVals.Aggregate((x, y) => x + "|" + y), sValidation_For, lstConditionValue.Aggregate((x, y) => x + "|" + y), IsDynamic, iErrorArea, txt);
                    break;
            }
        }


        //private void Exceptional_ValidationType_bak(string sPrimaryVals, List<string> lstSecoendryVals, DataRow drCompanyORContact, DataRow drValidation, string sFieldName, string sTableName, int iRowNumber, string sMessage, int iCompanyID, int iContactID, bool IsDynamic, int iErrorArea)
        //{
        //    string sDelimiter = string.Empty;
        //    string sDefaultEquals = string.Empty;
        //    sFieldName = Delimiter(sFieldName, out sDelimiter, out sDefaultEquals);

        //    string sValueToBeVerified = drCompanyORContact[sFieldName].ToString().Trim().ToUpper();
        //    List<string> lstDefaultSplit = new List<string>();
        //    string sDefaultEqualsCopy = string.Empty;
        //    if (sDefaultEquals.Length > 0)
        //        sDefaultEqualsCopy = sDefaultEquals;
        //    else
        //        sDefaultEqualsCopy = "EMPTY";

        //    switch (sDelimiter)
        //    {
        //        case "==":
        //            bool IsEqual = false;
        //            if (sDefaultEquals.Contains("|"))//For Multiple values
        //            {
        //                lstDefaultSplit = sDefaultEquals.Split('|').ToList();
        //                foreach (string sDefualt in lstDefaultSplit)
        //                {
        //                    if (sValueToBeVerified == sDefualt)
        //                    {
        //                        IsEqual = true;
        //                        break;
        //                    }
        //                }

        //                if (!IsEqual)
        //                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must be ", sMessage, sFieldName, sDefaultEquals, drValidation["VALIDATION_FOR"].ToString(), drValidation["Condition"].ToString(), IsDynamic, iErrorArea);
        //            }
        //            else
        //            {
        //                if (sValueToBeVerified != sDefaultEquals)
        //                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must be ", sMessage, sFieldName, sDefaultEquals, drValidation["VALIDATION_FOR"].ToString(), drValidation["Condition"].ToString(), IsDynamic, iErrorArea);
        //            }
        //            break;

        //        case "*=":
        //            bool StartsWith = false;
        //            if (sDefaultEquals.Contains("|"))//For Multiple values
        //            {
        //                lstDefaultSplit = sDefaultEquals.Split('|').ToList();
        //                foreach (string sDefualt in lstDefaultSplit)
        //                {
        //                    if (sValueToBeVerified.StartsWith(sDefualt))
        //                    {
        //                        StartsWith = true;
        //                        break;
        //                    }
        //                }

        //                if (!StartsWith)
        //                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must start with ", sMessage, sFieldName, sDefaultEquals, drValidation["VALIDATION_FOR"].ToString(), drValidation["Condition"].ToString(), IsDynamic, iErrorArea);
        //            }
        //            else
        //            {
        //                if (!sValueToBeVerified.StartsWith(sDefaultEquals))
        //                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must start with ", sMessage, sFieldName, sDefaultEquals, drValidation["VALIDATION_FOR"].ToString(), drValidation["Condition"].ToString(), IsDynamic, iErrorArea);
        //            }
        //            break;
        //        case "=*":
        //            bool EndsWith = false;
        //            if (sDefaultEquals.Contains("|"))//For Multiple values
        //            {
        //                lstDefaultSplit = sDefaultEquals.Split('|').ToList();
        //                foreach (string sDefualt in lstDefaultSplit)
        //                {
        //                    if (sValueToBeVerified.EndsWith(sDefualt))
        //                    {
        //                        EndsWith = true;
        //                        break;
        //                    }
        //                }

        //                if (!EndsWith)
        //                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must end with ", sMessage, sFieldName, sDefaultEquals, drValidation["VALIDATION_FOR"].ToString(), drValidation["Condition"].ToString(), IsDynamic, iErrorArea);
        //            }
        //            else
        //            {
        //                if (!sValueToBeVerified.EndsWith(sDefaultEquals))
        //                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must end with ", sMessage, sFieldName, sDefaultEquals, drValidation["VALIDATION_FOR"].ToString(), drValidation["Condition"].ToString(), IsDynamic, iErrorArea);
        //            }
        //            break;
        //        case "**":
        //            bool Contains = false;
        //            if (sDefaultEquals.Contains("|"))//For Multiple values
        //            {
        //                lstDefaultSplit = sDefaultEquals.Split('|').ToList();
        //                foreach (string sDefualt in lstDefaultSplit)
        //                {
        //                    if (sValueToBeVerified.Contains(sDefualt))
        //                    {
        //                        Contains = true;
        //                        break;
        //                    }
        //                }

        //                if (!Contains)
        //                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must contain ", sMessage, sFieldName, sDefaultEquals, drValidation["VALIDATION_FOR"].ToString(), drValidation["Condition"].ToString(), IsDynamic, iErrorArea);
        //            }
        //            else
        //            {
        //                if (!sValueToBeVerified.Contains(sDefaultEquals))
        //                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must contain ", sMessage, sFieldName, sDefaultEquals, drValidation["VALIDATION_FOR"].ToString(), drValidation["Condition"].ToString(), IsDynamic, iErrorArea);
        //            }
        //            break;
        //        case "##":
        //            bool NotEquals = false;
        //            if (sDefaultEquals.Contains("|"))//For Multiple values
        //            {
        //                lstDefaultSplit = sDefaultEquals.Split('|').ToList();
        //                foreach (string sDefualt in lstDefaultSplit)
        //                {
        //                    if (sValueToBeVerified == sDefualt)
        //                    {
        //                        NotEquals = true;
        //                        break;
        //                    }
        //                }

        //                if (NotEquals)
        //                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must not equal ", sMessage, sFieldName, sDefaultEquals, drValidation["VALIDATION_FOR"].ToString(), drValidation["Condition"].ToString(), IsDynamic, iErrorArea);
        //            }
        //            else
        //            {
        //                if (sValueToBeVerified == sDefaultEquals)
        //                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must not equal ", sMessage, sFieldName, sDefaultEquals, drValidation["VALIDATION_FOR"].ToString(), drValidation["Condition"].ToString(), IsDynamic, iErrorArea);
        //            }
        //            break;
        //        case "*#":
        //            bool NotContains = false;
        //            if (sDefaultEquals.Contains("|"))//For Multiple values
        //            {
        //                lstDefaultSplit = sDefaultEquals.Split('|').ToList();
        //                foreach (string sDefualt in lstDefaultSplit)
        //                {
        //                    if (sValueToBeVerified.Contains(sDefualt))
        //                    {
        //                        NotContains = true;
        //                        break;
        //                    }
        //                }

        //                if (NotContains)
        //                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must not contain ", sMessage, sFieldName, sDefaultEquals, drValidation["VALIDATION_FOR"].ToString(), drValidation["Condition"].ToString(), IsDynamic, iErrorArea);
        //            }
        //            else
        //            {
        //                if (sValueToBeVerified.Contains(sDefaultEquals))
        //                    AddValidationResults(sTableName, iRowNumber, iCompanyID, iContactID, " must not contain ", sMessage, sFieldName, sDefaultEquals, drValidation["VALIDATION_FOR"].ToString(), drValidation["Condition"].ToString(), IsDynamic, iErrorArea);
        //            }
        //            break;
        //    }
        //}

        //-----------------------------------------------------------------------------------------------------
        private void WriteTableToFile(DataTable dtToSave, string sTableName)
        {
            //    try
            //    {
            //        bool IsDataChanged = false;
            //        if (dtToSave.GetChanges(DataRowState.Added) != null)
            //        {
            //            IsDataChanged = true;
            //            GV..BAL_SaveToTable(dtToSave.GetChanges(DataRowState.Added), sTableName, "New");
            //        }
            //        if (dtToSave.GetChanges(DataRowState.Modified) != null)
            //        {
            //            IsDataChanged = true;
            //            GV.SQLCE.BAL_SaveToTable(dtToSave.GetChanges(DataRowState.Modified), sTableName, "Update");
            //        }
            //        if (dtToSave.GetChanges(DataRowState.Deleted) != null)
            //        {SQLCE
            //            IsDataChanged = true;
            //            GV.SQLCE.BAL_SaveToTable(dtToSave.GetChanges(DataRowState.Deleted), sTableName, "Delete");
            //        }

            //        if (IsDataChanged)
            //        {
            //            if (sTableName == GV.sSQLCECompanyTable)
            //                dtMasterCompaniesSQLCE = GV.SQLCE.BAL_FetchTable(GV.sSQLCECompanyTable, "Master_ID = '" + sMaster_ID + "'");
            //            else
            //                dtMasterContactsSQLCE = GV.SQLCE.BAL_FetchTable(GV.sSQLCEContactTable, "Master_ID = '" + sMaster_ID + "'");
            //        }

            //}
            //catch (Exception ex)
            //{
            //    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //}


            //string sPath = AppDomain.CurrentDomain.BaseDirectory + "\\Campaign Manager\\Backup\\"+GV.sProjectID;

            //    if (!Directory.Exists(sPath))
            //        Directory.CreateDirectory(sPath);
            //    //if (!(File.Exists(sPath + "\\" + sFileName)))
            //    //    File.WriteAllBytes(sPath + "\\" + sFileName, ByteFile);
            //    if (dtTableToSave.Rows.Count > 0)
            //    {
            //        sPath += "\\" + dtTableToSave.Rows[0]["Master_ID"] + "_" + sCompanyOrContact + ".txt";


            //        int i = 0;
            //        StreamWriter sw = null;
            //        sw = new StreamWriter(sPath, false);
            //        for (i = 0; i < dtTableToSave.Columns.Count - 1; i++)
            //        {
            //            sw.Write(dtTableToSave.Columns[i].ColumnName + ";");
            //        }
            //        sw.Write(dtTableToSave.Columns[i].ColumnName);
            //        sw.WriteLine();

            //        foreach (DataRow row in dtTableToSave.Rows)
            //        {
            //            object[] array = row.ItemArray;

            //            for (i = 0; i < array.Length - 1; i++)
            //            {
            //                sw.Write(array[i] + ";");
            //            }
            //            sw.Write(array[i].ToString());
            //            sw.WriteLine();

            //        }

            //        sw.Close();
            //    }
        }

        //-----------------------------------------------------------------------------------------------------
        private bool SpellCheck(string sWord)
        {
            try
            {


                //TextBox txt = new TextBox();
                //txt.EnableSpellCheck();
                //txt.Text = "";
                //txt.SpellCheck().CurrentDictionary.SpellCheckWord("as");

                List<string> lstWordSplit = sWord.Split(' ').ToList();
                foreach (string sWordSplit in lstWordSplit)
                {
                    //if (Regex.Replace(sWordSplit, @"[\W]", string.Empty).Length > 0)//Do not check special chars
                    if (Regex.Replace(sWordSplit, @"[\.\,\&\(\)\-]", string.Empty).Trim().Length == 0 || lstSpellCheckIgnore.Contains(sWordSplit.ToLower()))//Do not check special chars//Ignore chars . , & ( )-
                        continue;
                    else
                        if (Dictionary.DefaultDictionary.SpellCheckWord(Regex.Replace(sWordSplit, @"[\.\,\&\(\)\-]", string.Empty).Trim()) != Dictionary.SpellCheckWordError.OK)
                    {
                        if (Regex.IsMatch(sWordSplit, @"\W") || sWordSplit.ToUpper() != sWordSplit)//Eliminate CAPS
                            return false;
                    }
                    //if (!objHunspell.Spell(Regex.Replace(sWordSplit, @"[\.\,\&\(\)\-]", string.Empty)))//if check fails return false
                    //    return false;
                }
                return true; //if all check does not fail then return pass
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private string CONTACT_EmailContentCheck(string sEmail, string sCheckType)
        {
            List<string> lstEmailSplit = new List<string>();
            string sValidation = string.Empty;
            try
            {
                lstEmailSplit = sEmail.Split('@').ToList();
                string sEmailName = lstEmailSplit[0]; // Name part of the email
                string sDomainName = lstEmailSplit[1].Split('.').ToList()[0]; //Domain name of email

                if (GV.sAllowGeneralEmail == "N" && sCheckType == "COMPANYEMAIL")
                {
                    DataRow[] drGeneralEmail = dtPicklist.Select("PicklistCategory = 'CompanyEmails' AND '" + sEmailName.Replace("'", "''") + "' = PicklistValue"); //reverse Like operator(Avoids For loop)
                    if (drGeneralEmail.Length > 0)
                        sValidation += "<font color = 'Tomato'>" + sEmail + "</font> is a Company Email";
                }
                if (GV.sAllowPublicDomainEmails == "N" && sCheckType == "PUBLICDOMAIN")
                {
                    DataRow[] drPublicEmail = dtPicklist.Select("PicklistCategory = 'PublicDomainEmails' AND '" + sDomainName.Replace("'", "''") + "' = PicklistValue"); //reverse Like operator(Avoids For loop)
                    if (drPublicEmail.Length > 0)
                        sValidation += "<font color = 'Tomato'>" + sEmail + "</font> is a Public Domain Email";
                }
                return sValidation;
            }

            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return sValidation + ex.Message;
            }
        }

        void TrimCells(DataTable dt)
        {
            GV.sValidationMessage = "Trimming all data";
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dr[dc].ToString() != dr[dc].ToString().Trim())
                        dr[dc] = dr[dc].ToString().Trim();
                }
            }
        }

        void DeleteCrashRecovery()
        {
            try
            {

                // GV.SQLCE.BAL_DeleteFromTable(GV.sSQLCECompanyTable, "Master_ID=" + sMaster_ID + "");
                // GV.SQLCE.BAL_DeleteFromTable(GV.sSQLCEContactTable, "Master_ID=" + sMaster_ID + "");

                //string sPath = AppDomain.CurrentDomain.BaseDirectory + "\\Campaign Manager\\Backup\\" + GV.sProjectID;
                //if (Directory.Exists(sPath))
                //{
                //    if (File.Exists(sPath + "\\" + dtMasterCompanies.Rows[0]["Master_ID"] + "_Contacts.txt"))
                //        File.Delete(sPath + "\\" + dtMasterCompanies.Rows[0]["Master_ID"] + "_Contacts.txt");
                //    if (File.Exists(sPath + "\\" + dtMasterCompanies.Rows[0]["Master_ID"] + "_Company.txt"))
                //        File.Delete(sPath + "\\" + dtMasterCompanies.Rows[0]["Master_ID"] + "_Company.txt");
                //}
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void COMPANY_AutoPostUpdate()
        {
            try
            {
                Regex rNumeric = new Regex(@"[^\d]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                DataRow[] drrCompanyFormatFields = dtFieldMasterCompany.Select("LEN(FORMATTING) > 0");

                for (int i = 0; i < dtMasterCompanies.Rows.Count; i++)
                {
                    for (int j = 0; j < drrCompanyFormatFields.Length; j++)//Format all Data
                    {
                        string sFormatText = dtMasterCompanies.Rows[i][drrCompanyFormatFields[j]["FIELD_NAME_TABLE"].ToString()].ToString();
                        dtMasterCompanies.Rows[i][drrCompanyFormatFields[j]["FIELD_NAME_TABLE"].ToString()] = TextFormatting(sFormatText, drrCompanyFormatFields[j]["FORMATTING"].ToString().Split('|').ToList(), i);
                    }

                    if (dtMasterCompanies.Rows[i]["NEW_OR_EXISTING"].ToString().ToLower() == "new" && dtMasterCompanies.Rows[i]["HoursFromGMT"].ToString().Trim().Length == 0)
                    {
                        string sGMTCountryName = dtMasterCompanies.Rows[i]["Country"].ToString();
                        DataRow[] drCountryInformation = dtCountryInformation.Select("CountryName = '" + sGMTCountryName.Replace("'", "''") + "'");
                        if (drCountryInformation.Length > 0)
                            dtMasterCompanies.Rows[i]["HoursFromGMT"] = drCountryInformation[0]["HoursFromGMT"].ToString();
                    }

                    string sTelephone = dtMasterCompanies.Rows[i]["SWITCHBOARD"].ToString();
                    sTelephone = rNumeric.Replace(sTelephone, string.Empty);
                    if (sTelephone.Length > 7)
                    {
                        sTelephone = sTelephone.Substring(sTelephone.Length - 8);
                        dtMasterCompanies.Rows[i]["SWITCHBOARD_TRIMMED"] = Convert.ToInt32(sTelephone);
                    }
                    else
                        dtMasterCompanies.Rows[i]["SWITCHBOARD"] = DBNull.Value;

                }
            }
            catch (Exception ex)
            {
                GV.iNotifier[4] = 1;
                //CloseNotifier();
                lstApplicationError.Add("Company UpdateGMTforNewBuild : " + ex.Message);
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
            }
        }

        void COMPANY_CheckSwitchBoardDupe(DoWorkEventArgs e)//Check Switchboard Dupe
        {

            GV.sValidationMessage = "Checking Switchboard Duplicate";
            try
            {
                if (RunASynch(e))
                {
                    Regex rNumeric = new Regex(@"[^\d]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                    DataTable dtValidNumbers = new DataTable();
                    dtValidNumbers.Columns.Add("SWITCHBOARD");
                    dtValidNumbers.Columns.Add("MASTER_ID");
                    dtValidNumbers.Columns.Add("ROWINDEX");
                    for (int i = 0; i < dtMasterCompanies.Rows.Count; i++)
                    {
                        string sTelephone = dtMasterCompanies.Rows[i]["SWITCHBOARD"].ToString();
                        sTelephone = rNumeric.Replace(sTelephone, string.Empty);
                        if (sTelephone.Length > 7)
                        {
                            sTelephone = sTelephone.Substring(sTelephone.Length - 8);
                            DataRow drTele = dtValidNumbers.NewRow();
                            drTele["SWITCHBOARD"] = sTelephone;
                            drTele["MASTER_ID"] = dtMasterCompanies.Rows[i]["MASTER_ID"].ToString();
                            drTele["ROWINDEX"] = i;
                            dtValidNumbers.Rows.Add(drTele);
                        }
                        else
                        {
                            if (dtRecordStatus.Select("TABLE_NAME = 'COMPANY' AND PRIMARY_STATUS = '" + dtMasterCompanies.Rows[i][GV.sAccessTo + "_PRIMARY_DISPOSAL"] + "' AND SECONDARY_STATUS = '" + dtMasterCompanies.Rows[i][GV.sAccessTo + "_SECONDARY_DISPOSAL"] + "' AND RESEARCH_TYPE = '" + GV.sAccessTo + "' AND OPERATION_TYPE LIKE '%Delete%'").Length == 0)
                            {
                                if (sTelephone.Length > 0)
                                    AddValidationResults("Company", i, Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"]), 0, string.Empty, "INVALID", "SWITCHBOARD", dtMasterCompanies.Rows[i]["SWITCHBOARD"].ToString(), false, 1);
                                else
                                {
                                    if (GV.sAccessTo == "TR")
                                        AddValidationResults("Company", i, Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"]), 0, string.Empty, "INVALID", "SWITCHBOARD", dtMasterCompanies.Rows[i]["SWITCHBOARD"].ToString(), false, 1);
                                }
                            }
                        }
                    }

                    if (dtValidNumbers.Rows.Count > 0)
                    {
                        DataTable dtDistinctTele = dtValidNumbers.DefaultView.ToTable(true, "SWITCHBOARD");
                        string sTeleQuery = GM.ColumnToQString("SWITCHBOARD", dtDistinctTele, "Int");

                        RecordTime("Switchboard DB Operation Start");
                        DataTable dtCompanySwitchboard = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT Master_ID,Company_Name,COUNTRY,SWITCHBOARD FROM " + GV.sCompanyTable + "  WHERE SWITCHBOARD_TRIMMED IN (" + sTeleQuery + ") AND GROUP_ID <> " + sGroup_ID + ";");
                        //DataTable dtCompanySwitchboard = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT Master_ID,Company_Name,COUNTRY,SWITCHBOARD FROM " + GV.sCompanyTable + "  WHERE RIGHT(regex_replace('[^0-9]', '', SWITCHBOARD),8) IN (" + sTeleQuery + ") AND GROUP_ID <> " + sGroup_ID + ";");
                        RecordTime("Switchboard DB Operation Stop");
                        if (dtCompanySwitchboard.Rows.Count > 0)
                        {
                            //sValidation = "<font color = 'DarkCyan'>Switchboard</font> Number already found in another Company <font color = 'Tomato'> ID: " + dtCompanySwitchboard.Rows[0]["Master_ID"] + ", " + dtCompanySwitchboard.Rows[0]["Company_Name"] + ", " + dtCompanySwitchboard.Rows[0]["Country"] + "</font>";
                            foreach (DataRow drCompSwitchboard in dtCompanySwitchboard.Rows)
                            {
                                string sTele = rNumeric.Replace(drCompSwitchboard["SWITCHBOARD"].ToString(), string.Empty);
                                if (sTele.Length > 7)
                                {
                                    sTele = sTele.Substring(sTele.Length - 8);
                                    DataRow[] drrSwitchboardDupe = dtValidNumbers.Select("SWITCHBOARD = '" + sTele + "'");
                                    if (drrSwitchboardDupe.Length > 0)
                                    {
                                        foreach (DataRow drSwitchboardDupe in drrSwitchboardDupe)
                                            AddValidationResults("Company", Convert.ToInt32(drSwitchboardDupe["RowIndex"]), Convert.ToInt32(drSwitchboardDupe["MASTER_ID"]), 0, string.Empty, "SWITCHBOARDDUPE", "SWITCHBOARD", drCompSwitchboard["SWITCHBOARD"].ToString(), drCompSwitchboard["Master_ID"].ToString(), drCompSwitchboard["Company_Name"] + ", " + drCompSwitchboard["COUNTRY"], false, 1);
                                    }
                                }
                            }
                        }

                        if (GV.sAllowSwitchBoardDupeGroup == "N" && dtDistinctTele.Rows.Count != dtValidNumbers.Rows.Count)
                        {
                            foreach (DataRow drValidNumbers in dtValidNumbers.Rows)
                            {
                                DataRow[] drrGroupDupe = dtValidNumbers.Select("SWITCHBOARD = '" + drValidNumbers["SWITCHBOARD"] + "'");
                                if (drrGroupDupe.Length > 1)
                                {
                                    foreach (DataRow drGroupDupe in drrGroupDupe)
                                        AddValidationResults("Company", Convert.ToInt32(drGroupDupe["RowIndex"]), Convert.ToInt32(drGroupDupe["MASTER_ID"]), 0, string.Empty, "SWITCHBOARDDUPEGROUP", "SWITCHBOARD", dtMasterCompanies.Select("MASTER_ID = " + drGroupDupe["MASTER_ID"])[0]["SWITCHBOARD"].ToString(), drGroupDupe["MASTER_ID"].ToString(), string.Empty, false, 1);
                                }
                            }
                        }
                    }

                    UpdateNotifier(2);                  
                }
            }
            catch (Exception ex)
            {
                //Cancel_Asynch("Error Occured...",e);
                lstApplicationError.Add("CheckSwitchBoardDupe : " + ex.Message);
                
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void Revenue_Calc()
        {
            GV.sValidationMessage = "Updating Revenue for saved contacts";
            try
            {
                if (GV.sUserType == "Agent")
                {
                    DataTable dtRevenueContacts = GV.MYSQL.BAL_ExecuteQueryMySQL("select MASTER_ID, contact_id_p, " + GV.sAccessTo + "_Contact_Status, " +
                    GV.sAccessTo + "_agent_Name, Contact_Location, Contact_Country, Contact_Region," + GV.sAccessTo + "_Updated_Date from " + GV.sContactTable + " where Master_ID = " + sMaster_ID + " AND " +
                    GV.sAccessTo + "_contact_status IN (select Primary_Status from " + GV.sProjectID + "_RecordStatus where (Length(ifnull(Billing_revenue,'')) > 0 OR Length(ifnull(Agent_revenue,'')) > 0))");

                    if (dtRevenueContacts.Rows.Count > 0)
                    {
                        DataTable dtRevenue = GV.MYSQL.BAL_FetchTableMySQL(GV.sProjectID + "_REVENUE", "Master_ID IN (" + GM.ListToQueryString(lstMasterIDs, "Int") + ") AND User_Type = '" + GV.sAccessTo + "'");
                        foreach (DataRow drRevenueContacts in dtRevenueContacts.Rows)
                        {
                            DataRow[] drrRecordStatusRevenue;
                            if (drRevenueContacts["Contact_Location"].ToString().ToUpper() == "DIFFERENT")
                            {
                                drrRecordStatusRevenue = dtRecordStatusRevenue.Select("Table_Name = 'Contact' AND Billing_Location = '" + drRevenueContacts["Contact_Country"].ToString().Replace("'", "''") + "' AND Research_Type = '" +
                                GV.sAccessTo + "' AND Primary_Status = '" + drRevenueContacts[GV.sAccessTo + "_Contact_Status"] + "'");

                                if (drrRecordStatusRevenue.Length == 0)
                                    drrRecordStatusRevenue = dtRecordStatusRevenue.Select("Table_Name = 'Contact' AND Billing_Location = '" + drRevenueContacts["Contact_Region"].ToString().Replace("'", "''") + "' AND Research_Type = '" +
                                    GV.sAccessTo + "' AND Primary_Status = '" + drRevenueContacts[GV.sAccessTo + "_Contact_Status"] + "'");
                            }
                            else
                                drrRecordStatusRevenue = dtRecordStatusRevenue.Select("Table_Name = 'Contact' AND Billing_Location = '" + dtMasterCompanies.Select("MASTER_ID = " + drRevenueContacts["MASTER_ID"].ToString())[0]["Region"].ToString().Replace("'", "''") + "' AND Research_Type = '" + GV.sAccessTo + "' AND Primary_Status = '" + drRevenueContacts[GV.sAccessTo + "_Contact_Status"] + "'");

                            if (drrRecordStatusRevenue.Length > 0)
                            {
                                DataRow[] drrRevenue = dtRevenue.Select("Contact_ID = " + drRevenueContacts["Contact_ID_P"]);
                                double dBilling_Revenue = 0;
                                double dAgent_Revenue = 0;
                                if (drrRevenue.Length > 0)//Contact id already exist in Revenue Table
                                {
                                    foreach (DataRow drRevenue in drrRevenue)//Calculate total amount of billing and agent revenue
                                    {
                                        dBilling_Revenue += Convert.ToDouble(drRevenue["Billing_Revenue"]);
                                        dAgent_Revenue += Convert.ToDouble(drRevenue["Agent_Revenue"]);
                                    }

                                    if (Convert.ToDouble(drrRecordStatusRevenue[0]["Billing_Revenue"]) > dBilling_Revenue)
                                    {
                                        DataRow drNewRevenue = dtRevenue.NewRow();
                                        drNewRevenue["Master_ID"] = drRevenueContacts["MASTER_ID"].ToString();
                                        drNewRevenue["Contact_ID"] = drRevenueContacts["contact_id_p"].ToString();
                                        drNewRevenue["Agent_Name"] = drRevenueContacts[GV.sAccessTo + "_Agent_Name"].ToString();
                                        drNewRevenue["User_Type"] = GV.sAccessTo;
                                        drNewRevenue["Updated_Date"] = Convert.ToDateTime(drRevenueContacts[GV.sAccessTo + "_Updated_Date"]);
                                        drNewRevenue["Billing_Revenue"] = Convert.ToDouble(drrRecordStatusRevenue[0]["Billing_Revenue"]) - dBilling_Revenue;
                                        drNewRevenue["Agent_Revenue"] = Convert.ToDouble(drrRecordStatusRevenue[0]["Agent_Revenue"]) - dAgent_Revenue;
                                        dtRevenue.Rows.Add(drNewRevenue);
                                    }
                                }
                                else//ContactID new entry to Revenue table
                                {
                                    DataRow drNewRevenue = dtRevenue.NewRow();
                                    drNewRevenue["Master_ID"] = drRevenueContacts["MASTER_ID"].ToString();
                                    drNewRevenue["Contact_ID"] = drRevenueContacts["contact_id_p"].ToString();
                                    drNewRevenue["Agent_Name"] = drRevenueContacts[GV.sAccessTo + "_Agent_Name"].ToString();
                                    drNewRevenue["User_Type"] = GV.sAccessTo;
                                    drNewRevenue["Updated_Date"] = Convert.ToDateTime(drRevenueContacts[GV.sAccessTo + "_Updated_Date"]);
                                    drNewRevenue["Billing_Revenue"] = Convert.ToDouble(drrRecordStatusRevenue[0]["Billing_Revenue"]) - dBilling_Revenue;
                                    drNewRevenue["Agent_Revenue"] = Convert.ToDouble(drrRecordStatusRevenue[0]["Agent_Revenue"]) - dAgent_Revenue;
                                    dtRevenue.Rows.Add(drNewRevenue);
                                }
                            }
                        }

                        if (dtRevenue.GetChanges(DataRowState.Added) != null)
                            GV.MYSQL.BAL_SaveToTableMySQL(dtRevenue.GetChanges(DataRowState.Added), GV.sProjectID + "_Revenue", "New", true);
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //private void progressBarTimer_Tick(object sender, System.EventArgs e)
        //{

        //    //if (((frmMain)this.MdiParent).progressBar.Value >= ((frmMain)this.MdiParent).progressBar.Maximum)
        //    //    ((frmMain)this.MdiParent).progressBar.Value = 0;
        //    //else
        //    //    ((frmMain)this.MdiParent).progressBar.Value += 10;
        //}

        string FieldFormat(string sFieldName, string sTableName, string sSpanID)
        {
            string sReturn = string.Empty;

            List<string> lstFields = sFieldName.Split('|').ToList();


            foreach (string sFields in lstFields)
            {
                string sCaption = string.Empty;



                if (sFields.StartsWith("C.") && dtFieldMasterAllColumns.Select("TABLE_NAME = 'MasterContacts' AND FIELD_NAME_TABLE = '" + sFields.Replace("C.", "") + "' AND LEN(TRIM(FIELD_NAME_CAPTION)) > 0").Length > 0)
                    sCaption = dtFieldMasterAllColumns.Select("TABLE_NAME = 'MasterContacts' AND FIELD_NAME_TABLE = '" + sFields.Replace("C.", "") + "' AND LEN(TRIM(FIELD_NAME_CAPTION)) > 0")[0]["FIELD_NAME_CAPTION"].ToString();
                else if (sFields.StartsWith("M.") && dtFieldMasterAllColumns.Select("TABLE_NAME = 'Master' AND FIELD_NAME_TABLE = '" + sFields.Replace("M.", "") + "' AND LEN(TRIM(FIELD_NAME_CAPTION)) > 0").Length > 0)
                    sCaption = dtFieldMasterAllColumns.Select("TABLE_NAME = 'Master' AND FIELD_NAME_TABLE = '" + sFields.Replace("M.", "") + "' AND LEN(TRIM(FIELD_NAME_CAPTION)) > 0")[0]["FIELD_NAME_CAPTION"].ToString();
                else
                    sCaption = sFields.Replace("_", " ");


                //if (sFields.StartsWith("C.") && dtFieldMasterContact.Select("FIELD_NAME_TABLE = '" + sFields.Replace("C.", "") + "'").Length > 0)
                //    sCaption = dtFieldMasterContact.Select("FIELD_NAME_TABLE = '" + sFields.Replace("C.", "") + "'")[0]["FIELD_NAME_CAPTION"].ToString();
                //else if (sFields.StartsWith("M.") && dtFieldMasterCompany.Select("FIELD_NAME_TABLE = '" + sFields.Replace("M.", "") + "'").Length > 0)
                //    sCaption = dtFieldMasterCompany.Select("FIELD_NAME_TABLE = '" + sFields.Replace("M.", "") + "'")[0]["FIELD_NAME_CAPTION"].ToString();
                //else
                //    sCaption = sFields.Replace("_", " ");

                if (sReturn.Length > 0)
                    sReturn += ", <span id='" + sSpanID + "'>" + sCaption + "</span>";
                else
                    sReturn = "<span id='" + sSpanID + "'>" + sCaption + "</span>";
            }

            return sReturn;
        }

        StringBuilder GenerateError(DataRow drError, StringBuilder sMessage)
        {
            switch (drError["Validation"].ToString())
            {
                case "EMPTY":
                    if (drError["ConditionField"].ToString().Length == 0)
                        sMessage.AppendLine(sTab + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " is Empty.<br/>");
                    else
                        sMessage.AppendLine(sTab
                        + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " can't be empty if "
                        + FieldFormat(drError["ConditionField"].ToString(), drError["TableName"].ToString(), "2") + " is "
                        + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "3") + ".<br/>");
                    break;

                case "INVALID":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " is invalid.<br/>");
                    break;

                case "INVALIDDISPOSAL":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " cannot have successful contacts.<br/>");
                    break;

                case "REJECTED":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " is rejected.<br/>");
                    break;

                case "COMPANYEMAIL":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " is a Company's generic Email.<br/>");
                    break;

                case "PUBLICDOMAIN":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " is a general public domain Email.<br/>");
                    break;

                case "JOBTITLEDUPE":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " is already exist in row(s) "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "3") + ".<br/>");
                    break;

                case "NAMEDUPE":
                    sMessage.AppendLine(sTab
                    + FieldFormat("Contact Name : ", string.Empty, "1")
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "3") + " is already exist in row(s) "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "2") + ".<br/>");
                    break;

                case "EMAILDUPE":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " is already exist in row(s) "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "3") + ".<br/>");
                    break;

                case "MANDATORYCONTACTSTATUSCONTAINS":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " must be one of the following status "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " if "
                    + FieldFormat(drError["ConditionField"].ToString(), drError["TableName"].ToString(), "3") + " is "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "4") + ".<br/>");
                    break;

                case "MANDATORYCONTACTSTATUSNOTCONTAINS":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " must not be one of the following status "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " if "
                    + FieldFormat(drError["ConditionField"].ToString(), drError["TableName"].ToString(), "3") + " is "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "4") + ".<br/>");
                    break;

                case "IS":
                    string sTargetValIS = string.Empty;
                    if (drError["TargetValue"].ToString().Length > 0)
                        sTargetValIS = drError["TargetValue"].ToString();
                    else
                        sTargetValIS = "Empty";

                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " " + drError["Message"].ToString() + " "
                    + FieldFormat(sTargetValIS, string.Empty, "2") + " if "
                    + FieldFormat(drError["ConditionField"].ToString(), drError["TableName"].ToString(), "3") + " is "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "4") + ".<br/>");
                    break;

                case "IS NOT":
                    string sTargetValISNOT = string.Empty;
                    if (drError["TargetValue"].ToString().Length > 0)
                        sTargetValISNOT = drError["TargetValue"].ToString();
                    else
                        sTargetValISNOT = "Empty";

                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " " + drError["Message"].ToString() + " "
                    + FieldFormat(sTargetValISNOT, string.Empty, "2") + " if "
                    + FieldFormat(drError["ConditionField"].ToString(), drError["TableName"].ToString(), "3") + " is not "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "4") + ".<br/>");
                    break;

                case "STARTS WITH":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " " + drError["Message"].ToString() + " "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " if "
                    + FieldFormat(drError["ConditionField"].ToString(), drError["TableName"].ToString(), "3") + " starts with "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "4") + ".<br/>");
                    break;

                case "ENDS WITH":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " " + drError["Message"].ToString() + " "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " if "
                    + FieldFormat(drError["ConditionField"].ToString(), drError["TableName"].ToString(), "3") + " ends with "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "4") + ".<br/>");
                    break;

                case "CONTAINS":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " " + drError["Message"].ToString() + " "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " if "
                    + FieldFormat(drError["ConditionField"].ToString(), drError["TableName"].ToString(), "3") + " contains "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "4") + ".<br/>");
                    break;

                case "DOES NOT CONTAINS":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " " + drError["Message"].ToString() + " "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " if "
                    + FieldFormat(drError["ConditionField"].ToString(), drError["TableName"].ToString(), "3") + " does not contain "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "4") + ".<br/>");
                    break;

                case "SWITCHBOARDDUPE":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " duplicate found in ID:"
                    + FieldFormat(drError["ConditionField"].ToString(), drError["TableName"].ToString(), "3") + ". Company Name : "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "4") + ".<br/>");
                    break;

                case "SWITCHBOARDDUPEGROUP":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " duplicate found within Group Company : "
                    + FieldFormat(drError["ConditionField"].ToString(), string.Empty, "3") + ".<br/>");
                    break;

                case "EMAILDUPEOUTPROJECT":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " already exist in other company. ID : "
                    + FieldFormat(drError["ConditionField"].ToString(), string.Empty, "3") + ", Project Name : "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "4") + ".<br/>");
                    break;

                case "EMAILDUPEINPROJECT":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " already exist in other company. ID : "
                    + FieldFormat(drError["ConditionField"].ToString(), string.Empty, "3") + ".<br/>");
                    break;

                case "EMAILDUPEINCOMPANY":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " duplicate found within this company. Row : "
                    + FieldFormat((Convert.ToInt32(drError["ConditionField"]) + 1).ToString(), string.Empty, "3") + ".<br/>");
                    break;

                case "EMAILDUPEINCOMPANYGROUP":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " duplicate found within this Group companies. Row : "
                    + FieldFormat((Convert.ToInt32(drError["ConditionField"]) + 1).ToString(), string.Empty, "3") + ", ID : "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "4") + " <br/>");
                    break;

                case "EMAILDUPEOUTPROJECTMULTI":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " already exist in "
                    + FieldFormat(drError["Message"].ToString(), string.Empty, "3") + " other contacts on other projects. Sample ID : "
                    + FieldFormat(drError["ConditionField"].ToString(), string.Empty, "4") + ", Project Name : "
                    + FieldFormat(drError["ConditionValue"].ToString(), string.Empty, "1") + ".<br/>");
                    break;

                case "EMAILDUPEINPROJECTMULTI":
                    sMessage.AppendLine(sTab
                    + FieldFormat(drError["TargetField"].ToString(), drError["TableName"].ToString(), "1") + " : "
                    + FieldFormat(drError["TargetValue"].ToString(), string.Empty, "2") + " already exist in "
                    + FieldFormat(drError["Message"].ToString(), string.Empty, "3") + " other contacts. Sample ID : "
                    + FieldFormat(drError["ConditionField"].ToString(), string.Empty, "4") + ".<br/>");
                    break;

                default:
                    sMessage.AppendLine(sTab
                    + drError["Message"].ToString() + ".<br/>");
                    break;
            }
            return sMessage;
        }

        StringBuilder sStatistics = new StringBuilder();

        //void Validator_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    ((frmMain)this.MdiParent).progressBar.Visible = true;
        //    BeginInvoke((MethodInvoker)delegate
        //    {
        //        ValidateNSave();
        //    });

        //}
        //void Validator_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    ((frmMain)this.MdiParent).progressBar.Visible = false;
        //}

        //-----------------------------------------------------------------------------------------------------
        private void btnSave_Click(object sender, EventArgs e)//Save New Records for both master and master contacts
        {
            //if (!ApplicationDeployment.IsNetworkDeployed)
            //{
            //    foreach (DataRow dr in dtMasterContacts.Rows)
            //        dr["WR_CONTACT_STATUS"] = "NEW AND COMPLETE";
            //}

            if (IsNoteFormOpened)
                objFrmWindowedNotes.btnWindowed.PerformClick();

            if(bWorkerNameSayer.IsBusy)
            {
                ToastNotification.Show(this, "Application processing speech data.");
                return;
            }                            

            if (!bWorkerSave.IsBusy)
            {
                objNotifier.ReloadScreen();
                objNotifier.Show(this);
                this.Enabled = false;
                bWorkerSave.RunWorkerAsync();
            }
            else
                ToastNotification.Show(this, "Application Busy");


            //if (!DataValidator.IsBusy)
            //    DataValidator.RunWorkerAsync();
            //else
            //    ToastNotification.Show(this, "Working");
        }



        void SysInformation()
        {
            try
            {
                //ManagementObjectSearcher mosProcessor = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                //foreach (ManagementObject moProcessor in mosProcessor.Get())
                //{
                //    if (moProcessor["maxclockspeed"] != null)
                //        sStatistics += "maxclockspeed:|" + moProcessor["maxclockspeed"] + Environment.NewLine;
                //    if (moProcessor["datawidth"] != null)
                //        sStatistics += "datawidth:|" + moProcessor["datawidth"] + Environment.NewLine;
                //    if (moProcessor["name"] != null)
                //        sStatistics += "name:|" + moProcessor["name"] + Environment.NewLine;
                //    if (moProcessor["manufacturer"] != null)
                //        sStatistics += "manufacturer:|" + moProcessor["manufacturer"] + Environment.NewLine;


                // Page Level declaration
                PerformanceCounter cpuCounter;
                PerformanceCounter ramCounter;

                // Put into page load
                cpuCounter = new System.Diagnostics.PerformanceCounter();
                cpuCounter.CategoryName = "Processor";
                cpuCounter.CounterName = "% Processor Time";
                cpuCounter.InstanceName = "_Total";
                ramCounter = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes");
                sStatistics.AppendLine("CPUUsage:|" + cpuCounter.NextValue() + "%");
                sStatistics.AppendLine("RAM:|" + ramCounter.NextValue() + "MB");


                ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
                ManagementObjectCollection results = searcher.Get();

                foreach (ManagementObject result in results)
                {
                    sStatistics.AppendLine("Total Visible Memory: " + Convert.ToInt32(result["TotalVisibleMemorySize"]) / 1024 + " mb");
                    sStatistics.AppendLine("Free Physical Memory: " + Convert.ToInt32(result["FreePhysicalMemory"]) / 1024 + " mb");
                    sStatistics.AppendLine("Total Virtual Memory: " + Convert.ToInt32(result["TotalVirtualMemorySize"]) / 1024 + " mb");
                    sStatistics.AppendLine("Free Virtual Memory: " + Convert.ToInt32(result["FreeVirtualMemory"]) / 1024 + " mb");
                }

                //Process[] processes = Process.GetProcesses();
                //foreach (Process process in processes)
                //{
                //    sStatistics += process.ProcessName + " " +Convert.ToInt64(process.WorkingSet64)/ (1024*1024)+ Environment.NewLine;
                //}

                //}

                //ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_LogicalMemoryConfiguration");

                //ManagementObjectSearcher memor = new ManagementObjectSearcher(winQuery);
                //foreach (ManagementObject item in memor.Get())
                //{
                //    if (item["TotalPageFileSpace"] != null)
                //        sStatistics += "TotalPageFileSpace:|" + item["TotalPageFileSpace"] + Environment.NewLine;
                //    if (item["TotalPhysicalMemory"] != null)
                //        sStatistics += "TotalPhysicalMemory:|" + item["TotalPhysicalMemory"] + Environment.NewLine;
                //    if (item["TotalVirtualMemory"] != null)
                //        sStatistics += "TotalVirtualMemory:|" + item["TotalVirtualMemory"] + Environment.NewLine;
                //    if (item["AvailableVirtualMemory"] != null)
                //        sStatistics += "AvailableVirtualMemory:|" + item["AvailableVirtualMemory"] + Environment.NewLine;
                //}
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        //-----------------------------------------------------------------------------------------------------
        private void CONTACT_UpdateChangedRecords()
        {
            try
            {
                #region Empty contact's QC Status if agents opens and saves a Company
                ////////////////////////////Empty contact's QC Status if agents opens and saves a Company//
                //if (GV.sUserType == "Agent")
                {
                    foreach (DataRow dr in dtQCTable.Rows)
                    {
                        if (dr["QC_Status"].ToString().ToUpper() == "SENDBACK")
                        {
                            dr["QC_Status"] = "Reprocessed";
                            dr["Reprocessed_Date"] = GM.GetDateTime();
                            //if (dr["QC_Sample_Status"].ToString().Length > 0)
                            //{
                            //    if (dr["QC_Sample_Status"].ToString() == "0")
                            //        dr["QC_Sample_Status"] = "2";
                            //}
                        }
                    }
                }
                /////////////////////////////////////////////////////////////////////////////////////////                                                            
                #endregion

                #region Update Soundex for Contact Names
                /////////////////////////Update Soundex for Contact Names//////////////////////////
                GV.sValidationMessage = "Tagging Contacts with Name and Date";
                DataTable dtChangedContact = dtMasterContacts.GetChanges(DataRowState.Modified);
                DataTable dtChanged_and_New_Contact = dtMasterContacts.GetChanges(DataRowState.Modified | DataRowState.Added);
                if (dtChanged_and_New_Contact != null)
                {
                    string sSoundexQuery = string.Empty;
                    for (int i = 0; i < dtChanged_and_New_Contact.Rows.Count; i++)
                    {
                        if (sSoundexQuery.Length > 0)
                            sSoundexQuery += "UNION ALL SELECT '" + i + "' AS 'Index','" + GM.HandleBackSlash(dtChanged_and_New_Contact.Rows[i]["FIRST_NAME"].ToString().Replace("'", "''")) + "' AS'First_Name', SOUNDEX('" + GM.HandleBackSlash(dtChanged_and_New_Contact.Rows[i]["FIRST_NAME"].ToString().Replace("'", "''")) + "') AS 'First_Name_Soundx','" + GM.HandleBackSlash(dtChanged_and_New_Contact.Rows[i]["LAST_NAME"].ToString().Replace("'", "''")) + "' AS 'Last_Name', SOUNDEX('" + GM.HandleBackSlash(dtChanged_and_New_Contact.Rows[i]["LAST_NAME"].ToString().Replace("'", "''")) + "') AS 'Last_Name_Soundx'";
                        else
                            sSoundexQuery += "SELECT '" + i + "' AS 'Index','" + GM.HandleBackSlash(dtChanged_and_New_Contact.Rows[i]["FIRST_NAME"].ToString().Replace("'", "''")) + "' AS'First_Name', SOUNDEX('" + GM.HandleBackSlash(dtChanged_and_New_Contact.Rows[i]["FIRST_NAME"].ToString().Replace("'", "''")) + "') AS 'First_Name_Soundx','" + GM.HandleBackSlash(dtChanged_and_New_Contact.Rows[i]["LAST_NAME"].ToString().Replace("'", "''")) + "' AS 'Last_Name', SOUNDEX('" + GM.HandleBackSlash(dtChanged_and_New_Contact.Rows[i]["LAST_NAME"].ToString().Replace("'", "''")) + "') AS 'Last_Name_Soundx'";
                    }

                    DataTable dtSoundex = GV.MYSQL.BAL_ExecuteQueryMySQL(sSoundexQuery + ";");

                    for (int i = 0; i < dtMasterContacts.Rows.Count; i++)
                    {
                        DataRow[] drrSoundex = dtSoundex.Select("First_Name = '" + GM.HandleBackSlash(dtMasterContacts.Rows[i]["First_Name"].ToString().Replace("'", "''")) + "' AND Last_Name = '" + GM.HandleBackSlash(dtMasterContacts.Rows[i]["Last_Name"].ToString().Replace("'", "''")) + "'");
                        if (drrSoundex.Length > 0)
                        {
                            dtMasterContacts.Rows[i]["First_Name_Soundx"] = drrSoundex[0]["First_Name_Soundx"].ToString();
                            dtMasterContacts.Rows[i]["Last_Name_Soundx"] = drrSoundex[0]["Last_Name_Soundx"].ToString();
                        }
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
                #endregion

                GV.sValidationMessage = "Updating Gender for contacts";
                dtPicklist_Insert.Rows.Clear();
                dtPicklist_Delete.Rows.Clear();
                DataRow[] drrContactFormatFields = dtFieldMasterContact.Select("LEN(FORMATTING) > 0");
                for (int i = 0; i < dtMasterContacts.Rows.Count; i++)
                {
                    #region Update Modified records with Current date and Current Agent Name
                    /////////////////////////Update Modified records with Current date and Current Agent Name//////////////////////////
                    if (dtChangedContact != null)
                    {
                        for (int j = 0; j < dtChangedContact.Rows.Count; j++)
                        {
                            if (dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString() == dtChangedContact.Rows[j]["CONTACT_ID_P"].ToString())
                            {
                                if (lstFreezedContactIDs.Contains(Convert.ToInt32(dtChangedContact.Rows[j]["CONTACT_ID_P"].ToString())))//ignore freezed records
                                    continue;

                                #region Remove QC for changed old records
                                if (GV.lstTRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase) || GV.lstWRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                {
                                    if (!(GV.lstTRContactStatusToBeValidated.Contains(dtMasterContactsCopy.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase)
                                        ||
                                          GV.lstWRContactStatusToBeValidated.Contains(dtMasterContactsCopy.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase)))
                                    {
                                        if (dtQCTable.Select("TableName = 'Contact' AND RecordID = " + dtChangedContact.Rows[j]["CONTACT_ID_P"].ToString() + " AND QC_Status = 'OK' AND ResearchType = '" + GV.sAccessTo + "'").Length > 0)
                                            lstQCRowsToDelete.Add(dtQCTable.Select("TableName = 'Contact' AND RecordID = " + dtChangedContact.Rows[j]["CONTACT_ID_P"].ToString() + " AND QC_Status = 'OK' AND ResearchType = '" + GV.sAccessTo + "'")[0]);
                                    }
                                }
                                else
                                {
                                    if (dtQCTable.Select("TableName = 'Contact' AND RecordID = " + dtChangedContact.Rows[j]["CONTACT_ID_P"].ToString() + " AND QC_Status = 'OK' AND ResearchType = '" + GV.sAccessTo + "'").Length > 0)
                                        lstQCRowsToDelete.Add(dtQCTable.Select("TableName = 'Contact' AND RecordID = " + dtChangedContact.Rows[j]["CONTACT_ID_P"].ToString() + " AND QC_Status = 'OK' AND ResearchType = '" + GV.sAccessTo + "'")[0]);
                                }
                                #endregion

                                if (GV.sUserType == "Agent")//only for agents
                                {
                                    //if (lstSendBackRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])))//Send back processed so empty the qc status
                                    //    dtMasterContacts.Rows[i]["QC_Status"] = DBNull.Value;

                                    if (
                                            lstBouncedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"]))
                                            &&
                                            (
                                               (
                                                    GV.sAccessTo == "TR"
                                                    &&
                                                    GV.lstTRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase)
                                               )
                                               ||
                                               (
                                                    GV.sAccessTo == "WR"
                                                    &&
                                                    GV.lstWRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase)
                                               )
                                            )
                                       )//If record is bounced then do not update 'updateddate' and 'updated by'
                                    {
                                        dtMasterContacts.Rows[i]["EMAIL_VERIFIED_DATE"] = GM.GetDateTime();
                                        dtMasterContacts.Rows[i]["EMAIL_VERIFIED_AGENTNAME"] = GV.sEmployeeName;
                                        //dtMasterContacts.Rows[i]["QC_Status"] = "Email - " + dtMasterContacts.Rows[i]["EMAIL_VERIFIED"].ToString();
                                        if (dtMasterContacts.Rows[i]["CREATED_DATE"].ToString().Trim().Length == 0)
                                        {
                                            dtMasterContacts.Rows[i]["CREATED_BY"] = GV.sEmployeeName;
                                            dtMasterContacts.Rows[i]["CREATED_DATE"] = GM.GetDateTime();
                                        }
                                    }
                                    else
                                    {
                                        if (
                                            lstSendBackRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"]))
                                            &&
                                            (
                                                (GV.sAccessTo == "TR" && GV.lstTRContactStatusToBeValidated.Contains(dtMasterContactsCopy.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                                ||
                                                (GV.sAccessTo == "WR" && GV.lstWRContactStatusToBeValidated.Contains(dtMasterContactsCopy.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                                            )
                                           )
                                        {
                                            /*Do Nothing*/
                                        }
                                        else
                                        {
                                            dtMasterContacts.Rows[i][GV.sAccessTo + "_UPDATED_DATE"] = GM.GetDateTime();
                                            dtMasterContacts.Rows[i][GV.sAccessTo + "_AGENT_NAME"] = GV.sEmployeeName;

                                            if (dtMasterContacts.Rows[i]["CREATED_DATE"].ToString().Trim().Length == 0)
                                            {
                                                dtMasterContacts.Rows[i]["CREATED_BY"] = GV.sEmployeeName;
                                                dtMasterContacts.Rows[i]["CREATED_DATE"] = GM.GetDateTime();
                                            }
                                        }
                                    }
                                }

                                if (dtMasterContacts.Rows[i]["Scrape_status"].ToString() == "1")
                                    dtMasterContacts.Rows[i]["Scrape_status"] = "2";

                                //If Manager completes a record. Or Bounced record moved to opposit bin
                                if (dtMasterContacts.Rows[i][GV.sAccessTo + "_UPDATED_DATE"].ToString().Length == 0 || dtMasterContacts.Rows[i][GV.sAccessTo + "_AGENT_NAME"].ToString().Length == 0)
                                {
                                    dtMasterContacts.Rows[i][GV.sAccessTo + "_UPDATED_DATE"] = GM.GetDateTime();
                                    dtMasterContacts.Rows[i][GV.sAccessTo + "_AGENT_NAME"] = GV.sEmployeeName;

                                    if (dtMasterContacts.Rows[i]["CREATED_DATE"].ToString().Trim().Length == 0)
                                    {
                                        dtMasterContacts.Rows[i]["CREATED_BY"] = GV.sEmployeeName;
                                        dtMasterContacts.Rows[i]["CREATED_DATE"] = GM.GetDateTime();
                                    }
                                }

                                //For agents, admins etc etc--everyone
                                dtMasterContacts.Rows[i]["UPDATED_BY"] = GV.sEmployeeName;
                                dtMasterContacts.Rows[i]["UPDATED_DATE"] = GM.GetDateTime();

                                if (GV.sUserType == "QC")
                                {
                                    dtMasterContacts.Rows[i]["QC_BY"] = GV.sEmployeeName;
                                    dtMasterContacts.Rows[i]["QC_DATE"] = GM.GetDateTime();
                                }

                            }
                        }

                        //if (lstdr.Count > 0)
                        //    dtQCTable.Rows[0].Delete();                        


                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
                    #endregion

                    #region Update Opposit research contact status
                    /////////////////////////Update Opposit research contact status//////////////////////
                    if (GV.sAccessTo == "WR")
                    {
                        //if (GV.sFreezeWRCompletedRecords == "Y" && GV.lstWRContactStatusToBeValidated.Contains(dr["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                        if (GV.lstWRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                        {
                            if (dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString().Trim().Length == 0 || dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString().ToUpper().Trim() == "NOT VERIFIED")//Do not update if already exist.
                                dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"] = "WEBRESEARCHED";
                        }
                        else if (GV.lstWR_DeleteStatus.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                        {
                            if (dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString().Trim().Length == 0 || dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString().ToUpper().Trim() == "NOT VERIFIED")//Do not update if already exist.
                                dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"] = "Do Not Call";
                        }
                        else if (GV.Update_Blank_NotVerified)
                        {
                            if (dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString().Trim().Length == 0)
                                dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"] = "NOT VERIFIED";
                        }
                    }
                    else if (GV.sAccessTo == "TR")
                    {
                        //if (GV.sFreezeTRCompletedRecords == "Y" && GV.lstTRContactStatusToBeValidated.Contains(dr["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                        if (GV.lstTRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                        {
                            if (dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString().Trim().Length == 0 || dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString().ToUpper().Trim() == "NOT VERIFIED")
                                dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"] = "TELERESEARCHED";
                        }
                        else if (GV.lstTR_DeleteStatus.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                        {
                            if (dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString().Trim().Length == 0 || dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString().ToUpper().Trim() == "NOT VERIFIED")//Do not update if already exist.
                                dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"] = "Do Not Research";
                        }
                        else if (GV.Update_Blank_NotVerified)
                        {
                            if (dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString().Trim().Length == 0)
                                dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"] = "NOT VERIFIED";
                        }
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////// 
                    #endregion

                    #region Update Region
                    ////////////////Update Region////////////////////////////////////
                    string sCountry = dtMasterContacts.Rows[i]["Contact_Country"].ToString().Trim();
                    if (sCountry.Length > 0)
                    {
                        DataRow[] drrRegion = dtCountryInformation.Select("CountryName = '" + sCountry.Replace("'", "''") + "'");
                        if (drrRegion.Length > 0 && drrRegion[0]["Region"].ToString().Length > 0)
                            dtMasterContacts.Rows[i]["Contact_Region"] = drrRegion[0]["Region"].ToString();
                        else
                            dtMasterContacts.Rows[i]["Contact_Region"] = DBNull.Value;
                    }
                    else
                        dtMasterContacts.Rows[i]["Contact_Region"] = DBNull.Value;
                    /////////////////////////////////////////////////////////////////// 
                    #endregion

                    #region Update Gender
                    ////////////////////////////Update Gender////////////////////////////////////////////////
                    if (dtMasterContacts.Rows[i]["Title"].ToString().Length > 0)
                    {
                        DataRow[] drrZGender = dtPicklist.Select("PicklistCategory = 'Title' AND PicklistValue = '" + dtMasterContacts.Rows[i]["Title"].ToString().Replace("'", "''") + "'");
                        if (drrZGender.Length > 0 && drrZGender[0]["remarks"].ToString().Length > 0)
                            dtMasterContacts.Rows[i]["Gender"] = drrZGender[0]["remarks"].ToString();
                        else
                        {
                            if (dtPicklist.Select("PicklistCategory = 'Title' AND remarks = '" + dtMasterContacts.Rows[i]["Gender"].ToString().Replace("'", "''") + "'").Length > 0)
                                dtMasterContacts.Rows[i]["Gender"] = DBNull.Value;
                        }
                    }
                    else
                        dtMasterContacts.Rows[i]["Gender"] = DBNull.Value;
                    /////////////////////////////////////////////////////////////////////////////////////////

                    #endregion

                    #region Auto Format All data based on FieldMaster Text Format specification
                    ////////////////////////Auto Format All data based on FieldMaster Text Format specification//////////////////////////////////////
                    for (int j = 0; j < drrContactFormatFields.Length; j++)//Format all Data
                    {
                        string sFormatText = dtMasterContacts.Rows[i][drrContactFormatFields[j]["FIELD_NAME_TABLE"].ToString()].ToString();
                        if (sFormatText.Trim().Length > 0)
                            dtMasterContacts.Rows[i][drrContactFormatFields[j]["FIELD_NAME_TABLE"].ToString()] = TextFormatting(sFormatText, drrContactFormatFields[j]["FORMATTING"].ToString().Split('|').ToList(), i);
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
                    #endregion

                    #region Update Review Tag
                    ////////////////////////////Update Review Tag////////////////////////////////////////////
                    if (dtMasterContacts.Rows[i]["CONTACT_EMAIL"].ToString().Length > 0 && GM.Email_Check(dtMasterContacts.Rows[i]["CONTACT_EMAIL"].ToString()))
                    {
                        string sEmailNameDomainCheck = Email_NameAndDomain_MatchFlag(dtMasterContacts.Rows[i]["CONTACT_EMAIL"].ToString(), dtMasterContacts.Rows[i]["FIRST_NAME"].ToString(), dtMasterContacts.Rows[i]["LAST_NAME"].ToString(), i);
                        if (sEmailNameDomainCheck.Trim().Length > 0)
                        {
                            if (sEmailNameDomainCheck.Contains("EmailDomain") && sEmailNameDomainCheck.Contains("EmailName"))
                                dtMasterContacts.Rows[i]["REVIEW_TAG"] = "EmailName and EmailDomain";
                            else
                                dtMasterContacts.Rows[i]["REVIEW_TAG"] = sEmailNameDomainCheck;
                        }
                        else
                            dtMasterContacts.Rows[i]["REVIEW_TAG"] = DBNull.Value;
                    }
                    else if (dtMasterContacts.Rows[i]["CONTACT_EMAIL"].ToString().Trim().Length > 0)
                        dtMasterContacts.Rows[i]["REVIEW_TAG"] = "Invalid Email";
                    /////////////////////////////////////////////////////////////////////////////////////////

                    #endregion

                    #region Insert Date for New Records 
                    ////////////////////////////Insert Date for New Records///////////////////////////////////
                    if (dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString().Trim().Length == 0)
                    {
                        dtMasterContacts.Rows[i][GV.sAccessTo + "_UPDATED_DATE"] = GM.GetDateTime();

                        if (
                            GV.lstTRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(),
                                StringComparer.OrdinalIgnoreCase) ||
                            GV.lstWRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(),
                                StringComparer.OrdinalIgnoreCase))
                        {
                            IsNewCompletesAdded = true;
                        }
                    }




                    ///////////////////////////////////////////////////////////////////////////////////////// 
                    #endregion

                    #region Uncertain

                    if (dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString().Length > 0 && lstFreezedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString())))//ignore freezed records
                        continue;

                    if (GV.lstTRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase) || GV.lstWRContactStatusToBeValidated.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                    {

                        foreach (DataRow drUnCertain_Columns in dtUncertainFields.Rows)
                        {
                            string sPickListCategory = string.Empty;
                            sPickListCategory = drUnCertain_Columns["PICKLIST_CATEGORY"].ToString().Trim();

                            if (drUnCertain_Columns["FieldName_LinkColumn"].ToString().Length > 0 && dtMasterContacts.Rows[i][drUnCertain_Columns["FieldName_LinkColumn"].ToString()].ToString().Trim().Length > 0)
                            {
                                if (dtMasterContactsCopy.Rows.Count > i)//Check if uncertain is already raised before opening this record. (i.e. uncertain not raised by current agent)
                                {
                                    if (dtMasterContactsCopy.Rows[i][drUnCertain_Columns["FieldName_LinkColumn"].ToString()].ToString().ToUpper().Trim() != dtMasterContacts.Rows[i][drUnCertain_Columns["FieldName_LinkColumn"].ToString()].ToString().ToUpper().Trim())//If uncertain not matched with datatable copy then update uncertain with new.
                                    {
                                        if (dtMasterContacts.Rows[i][GV.sAccessTo + "_UNCERTAIN_FIELD"].ToString().ToUpper().Trim() == drUnCertain_Columns["FieldName_LinkColumn"].ToString().ToUpper().Trim())//Check if uncertain field is selected with some other column
                                        {//If Correct column is selected, then check if the uncertain is raised in current session or previous uncertain is opened.
                                            //if (dtMasterContactsCopy.Rows[i][GV.sAccessTo + "_UNCERTAIN_FIELD"].ToString().ToUpper() != dtMasterContacts.Rows[i][GV.sAccessTo + "_UNCERTAIN_FIELD"].ToString().ToUpper())//If uncertain not matched with datatable copy then update uncertain with new.
                                            {//Uncertain reaised in current session only
                                                UnCertain_Update_v1(1, drUnCertain_Columns["FieldName"].ToString(), drUnCertain_Columns["FieldName_LinkColumn"].ToString(), sPickListCategory, i);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            UnCertain_Update_v1(1, drUnCertain_Columns["FieldName"].ToString(), drUnCertain_Columns["FieldName_LinkColumn"].ToString(), sPickListCategory, i);
                                            break;
                                        }
                                    }
                                }
                                else//New contact with uncertain
                                {
                                    UnCertain_Update_v1(1, drUnCertain_Columns["FieldName"].ToString(), drUnCertain_Columns["FieldName_LinkColumn"].ToString(), sPickListCategory, i);
                                    break;
                                }

                            }
                            else
                            { dtMasterContacts.Rows[i][GV.sAccessTo + "_UNCERTAIN_STATUS"] = 0; }







                            //else if (dtMasterContacts.Rows[i][GV.sAccessTo + "_UNCERTAIN_FIELD"].ToString().Trim().Length > 0 && drUnCertain_Columns["FieldName_LinkColumn"].ToString().Length == 0) //Check if Uncertain Field is filled manually
                            //{





                            //    if (dtMasterContactsCopy.Rows.Count > i)//Check if uncertain is already raised before opening this record. (i.e. uncertain not raised by current agent)
                            //    {
                            //        if (dtMasterContactsCopy.Rows[i][GV.sAccessTo + "_UNCERTAIN_FIELD"].ToString().ToUpper().Trim() != dtMasterContacts.Rows[i][GV.sAccessTo + "_UNCERTAIN_FIELD"].ToString().ToUpper().Trim())//If uncertain not matched with datatable copy then update uncertain with new.
                            //        {
                            //            UnCertain_Update_v1(1, drUnCertain_Columns["FieldName"].ToString(), drUnCertain_Columns["FieldName_LinkColumn"].ToString(), sPickListCategory, i);
                            //            break;
                            //        }
                            //    }
                            //    else//Creat new uncertain values
                            //    {
                            //        UnCertain_Update_v1(1, drUnCertain_Columns["FieldName"].ToString(), drUnCertain_Columns["FieldName_LinkColumn"].ToString(), sPickListCategory, i);
                            //        break;
                            //    }






                            //}
                            //else
                            //{
                            //    //if (dtMasterContacts.Rows[i][GV.sAccessTo + "_UNCERTAIN_FIELD"].ToString().Length > 0)
                            //    //{
                            //    //    dtMasterContacts.Rows[i][GV.sAccessTo + "_UNCERTAIN_FIELD"] = DBNull.Value;
                            //    //    dtMasterContacts.Rows[i][GV.sAccessTo + "_UNCERTAIN_STATUS"] = 0;
                            //    //    dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"] = dtMasterContacts.Rows[i][GV.sAccessTo + "_CONTACT_STATUS"].ToString().Replace("_UNCERTAIN", string.Empty);
                            //    //    dtMasterContacts.Rows[i][GV.sAccessTo + "_UNCERTAIN_RESOLVED_BY"] = GV.sEmployeeName;
                            //    //    dtMasterContacts.Rows[i][GV.sAccessTo + "_UNCERTAIN_RESOLVED_DATE"] = GM.GetDateTime();
                            //    //}
                            //}
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                GV.iNotifier[4] = 1;
                //CloseNotifier();
                lstApplicationError.Add("Contact UpdateChangedRecords : " + ex.Message);
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
            }
        }

        void Email_Checks()
        {
            #region Email BounceCheck

            try
            {
                if (GV.sTRContactstatusTobeMailChecked.Length > 0 || GV.sWRContactstatusTobeMailChecked.Length > 0)
                {

                    DataRow[] drrContactsToCheck = null;
                    if (GV.sTRContactstatusTobeMailChecked.Length > 0 && GV.sWRContactstatusTobeMailChecked.Length > 0)
                        drrContactsToCheck = dtMasterContacts.Select("TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeMailChecked + ") OR WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeMailChecked + ")");
                    else if (GV.sTRContactstatusTobeMailChecked.Length > 0)
                        drrContactsToCheck = dtMasterContacts.Select("TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeMailChecked + ")");
                    else
                        drrContactsToCheck = dtMasterContacts.Select("WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeMailChecked + ")");

                    if (drrContactsToCheck.Length > 0)
                    {
                        DataTable dtEmail_Contact;
                        if (IsNewCompletesAdded)
                        {
                            if (GV.sTRContactstatusTobeMailChecked.Length > 0 && GV.sWRContactstatusTobeMailChecked.Length > 0)
                                dtEmail_Contact = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT CONTACT_ID_P, CONTACT_EMAIL, EMAIL_VERIFIED FROM " + GV.sContactTable + " WHERE MASTER_ID = " + sMaster_ID + " AND (TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeMailChecked + ") OR WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeMailChecked + "));");
                            else if (GV.sTRContactstatusTobeMailChecked.Length > 0)
                                dtEmail_Contact = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT CONTACT_ID_P, CONTACT_EMAIL, EMAIL_VERIFIED FROM " + GV.sContactTable + " WHERE MASTER_ID = " + sMaster_ID + " AND TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeMailChecked + ");");
                            else
                                dtEmail_Contact = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT CONTACT_ID_P, CONTACT_EMAIL, EMAIL_VERIFIED FROM " + GV.sContactTable + " WHERE MASTER_ID = " + sMaster_ID + " AND WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeMailChecked + ");");
                        }
                        else
                            dtEmail_Contact = drrContactsToCheck.CopyToDataTable();

                        foreach (DataRow drEmail_Contact in dtEmail_Contact.Rows)
                        {
                            if (drEmail_Contact["CONTACT_EMAIL"].ToString().Length > 0)
                            {
                                DataRow[] drrEmailChecks = dtEmailChecks.Select("CONTACT_ID = " + drEmail_Contact["CONTACT_ID_P"] + " AND ISNULL(DESCRIPTION,'') NOT LIKE 'Email Changed%'");
                                if (drrEmailChecks.Length > 0)
                                {
                                    if (string.Equals(drrEmailChecks[0]["EMAIL"].ToString(),
                                        drEmail_Contact["CONTACT_EMAIL"].ToString(),
                                        StringComparison.CurrentCultureIgnoreCase))
                                        continue;

                                    drrEmailChecks[0]["DESCRIPTION"] = "Email Changed - [" +
                                                                       drrEmailChecks[0]["DESCRIPTION"] + "]";
                                    drrEmailChecks[0]["DETAIL"] = "Email Changed - [" + drrEmailChecks[0]["DETAIL"] +
                                                                  "]";

                                    drrEmailChecks[0]["REPROCESSED"] = "1";

                                    if (drrEmailChecks[0]["APPENDED_DATE"].ToString().Length == 0)
                                        drrEmailChecks[0]["APPENDED_DATE"] = GM.GetDateTime();

                                    DataRow drNewEmail = dtEmailChecks.NewRow();
                                    drNewEmail["PROJECT_ID"] = GV.sProjectID;
                                    drNewEmail["CONTACT_ID"] = drEmail_Contact["CONTACT_ID_P"].ToString();
                                    drNewEmail["EMAIL"] = drEmail_Contact["CONTACT_EMAIL"].ToString();
                                    drNewEmail["EMAIL_SOURCE"] = "0";
                                    drNewEmail["REPROCESSED"] = "0";
                                    drNewEmail["CREATED_BY"] = GV.sEmployeeName;
                                    drNewEmail["CREATED_DATE"] = GM.GetDateTime();
                                    dtEmailChecks.Rows.Add(drNewEmail);
                                }
                                else if (drEmail_Contact["EMAIL_VERIFIED"].ToString().Trim() == string.Empty || drEmail_Contact["EMAIL_VERIFIED"].ToString().ToUpper() == "VERIFIED")
                                {
                                    DataRow drNewEmail = dtEmailChecks.NewRow();
                                    drNewEmail["PROJECT_ID"] = GV.sProjectID;
                                    drNewEmail["CONTACT_ID"] = drEmail_Contact["CONTACT_ID_P"].ToString();
                                    drNewEmail["EMAIL"] = drEmail_Contact["CONTACT_EMAIL"].ToString();
                                    drNewEmail["EMAIL_SOURCE"] = "0";
                                    drNewEmail["REPROCESSED"] = "0";
                                    drNewEmail["CREATED_BY"] = GV.sEmployeeName;
                                    drNewEmail["CREATED_DATE"] = GM.GetDateTime();
                                    dtEmailChecks.Rows.Add(drNewEmail);
                                }
                            }
                        }
                        SaveToDB(dtEmailChecks, "c_email_checks");
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }

            #endregion
        }


        void UnCertain_Update_v1(int sStatus, string sUncertainField, string sUncertain_Link_Field, string sPickListCategory, int iRowIndex)
        {
            DataRow drContact = dtMasterContacts.Rows[iRowIndex];
            if (sStatus == 1)
            {
                if (sPickListCategory.Length > 0 && sUncertain_Link_Field.Length > 0)
                {
                    if (dtPicklist.Select("PicklistCategory = '" + sPickListCategory + "' AND PicklistValue = '" + drContact[sUncertain_Link_Field].ToString().Replace("'", "''") + "' AND remarks = 'Accepted'").Length > 0)
                    {
                        drContact[sUncertainField] = drContact[sUncertain_Link_Field].ToString();
                        drContact[sUncertainField] = DBNull.Value;
                        return; // Do not update as uncertain. Just moving the value from 'Others_Jobtitle' to 'Jobtitle'
                    }

                    if (dtPicklist_Insert.Select("PickListCategory = '" + sPickListCategory + "'").Length == 0)
                        dtPicklist_Insert.Rows.Add(sPickListCategory, "('" + sPickListCategory + "','" + drContact[sUncertain_Link_Field].ToString().Trim().Replace("'", "''") + "', 'Pending')");
                    else
                        dtPicklist_Insert.Select("PickListCategory = '" + sPickListCategory + "'")[0]["Value"] += ",('" + sPickListCategory + "','" + drContact[sUncertain_Link_Field].ToString().Trim().Replace("'", "''") + "', 'Pending')";
                }

                drContact[GV.sAccessTo + "_CONTACT_STATUS"] = drContact[GV.sAccessTo + "_CONTACT_STATUS"].ToString().Replace("_UNCERTAIN", string.Empty) + "_UNCERTAIN";
                drContact[GV.sAccessTo + "_UNCERTAIN_FIELD"] = sUncertain_Link_Field.ToUpper();
                drContact[GV.sAccessTo + "_UNCERTAIN_RAISED_BY"] = GV.sEmployeeName;
                drContact[GV.sAccessTo + "_UNCERTAIN_RAISED_DATE"] = GM.GetDateTime();
                drContact[GV.sAccessTo + "_UNCERTAIN_STATUS"] = 1;
            }
            else if (sStatus == 0)
            {
                //drContact[GV.sAccessTo + "_UNCERTAIN_FIELD"] = string.Empty;
                //drContact[GV.sAccessTo + "_UNCERTAIN_RAISED_BY"] = string.Empty;
                //drContact[GV.sAccessTo + "_UNCERTAIN_RAISED_DATE"] = string.Empty;
                if (sPickListCategory.Length > 0)
                {
                    DataRow drContactCopy = dtMasterContactsCopy.Rows[iRowIndex];
                    if (dtPicklist_Delete.Select("PickListCategory = '" + sPickListCategory + "'").Length == 0)
                        dtPicklist_Delete.Rows.Add(sPickListCategory, "'" + drContactCopy[sUncertainField].ToString().Trim().Replace("'", "''") + "'");
                    else
                        dtPicklist_Delete.Select("PickListCategory = '" + sPickListCategory + "'")[0]["Value"] += ",'" + drContactCopy[sUncertainField].ToString().Trim().Replace("'", "''") + "'";

                    if (dtPicklist_Insert.Select("PickListCategory = '" + sPickListCategory + "'").Length == 0)
                        dtPicklist_Insert.Rows.Add(sPickListCategory, "('" + sPickListCategory + "','" + drContactCopy[sUncertainField].ToString().Trim().Replace("'", "''") + "','Accepted')");
                    else
                        dtPicklist_Insert.Select("PickListCategory = '" + sPickListCategory + "'")[0]["Value"] += ",('" + sPickListCategory + "','" + drContactCopy[sUncertainField].ToString().Trim().Replace("'", "''") + "','Accepted')";


                    //dtPicklist_Delete.Select("PickListCategory = '" + sPickListCategory + "'")[0]["Value"] += ",'" + drContactCopy[sUncertainField].ToString().Trim() + "'";
                    //dtPicklist_Insert.Select("PickListCategory = '" + sPickListCategory + "'")[0]["Value"] += ",('" + sPickListCategory + "','" + drContactCopy[sUncertainField].ToString().Trim() + "','Accepted')";
                }

                drContact[GV.sAccessTo + "_UNCERTAIN_STATUS"] = 0;
                drContact[GV.sAccessTo + "_CONTACT_STATUS"] = drContact[GV.sAccessTo + "_CONTACT_STATUS"].ToString().Replace("_UNCERTAIN", string.Empty);
                drContact[GV.sAccessTo + "_UNCERTAIN_RESOLVED_BY"] = GV.sEmployeeName;
                drContact[GV.sAccessTo + "_UNCERTAIN_RESOLVED_DATE"] = GM.GetDateTime();
            }
        }



        //-----------------------------------------------------------------------------------------------------
        private void CONTACT_CheckSwitchBoardinContacts(string sFreezedContatactIDs, DoWorkEventArgs e)
        {
            if (GV.sAllowSwitchBoardNumberinContacts == "N")
            {
                try
                {
                    if (RunASynch(e))
                    {
                        GV.sValidationMessage = "Checking Switchboard number on contacts";
                        for (int i = 0; i < dtMasterCompanies.Rows.Count; i++)
                        {
                            if (!lstFreezedMasterIDs.Contains(Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"])))
                            {
                                string sSwitchBoard = dtMasterCompanies.Rows[i]["SWITCHBOARD"].ToString().Replace(" ", string.Empty);

                                if (sSwitchBoard.Length > 0)
                                {
                                    DataRow[] drrMasterContacts = null;
                                    if (sFreezedContatactIDs.Length > 0)//Ignore freezed records. Nothing can be done on freezed records
                                        drrMasterContacts = dtMasterContacts.Select("MASTER_ID = " + dtMasterCompanies.Rows[i]["MASTER_ID"] + " AND (TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeValidated + ") OR WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeValidated + ")) AND CONTACT_ID_P NOT IN(" + sFreezedContatactIDs + ")");//Do not Check freezed contact
                                    else
                                        drrMasterContacts = dtMasterContacts.Select("MASTER_ID = " + dtMasterCompanies.Rows[i]["MASTER_ID"] + " AND (TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeValidated + ") OR WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeValidated + "))");

                                    if (drrMasterContacts.Length > 0)
                                    {
                                        foreach (DataRow dr in drrMasterContacts)
                                        {
                                            string sContactTele = string.Empty;
                                            sContactTele = dr["CONTACT_TELEPHONE"].ToString().Replace(" ", string.Empty);
                                            if (sContactTele.Length > 0 && sSwitchBoard == sContactTele)
                                                AddValidationResults("Company", i, Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"]), 0, "SwitchBoard Number " + sSwitchBoard + " found on Contacts", false, 0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lstApplicationError.Add("CheckSwitchBoardinContacts : " + ex.Message);
                    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                    return;
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void CONTACT_CheckContactsCount(DoWorkEventArgs e)
        {
            GV.sValidationMessage = "Checking number of valid contacts";
            try
            {
                if (RunASynch(e))
                {
                    for (int i = 0; i < dtMasterCompanies.Rows.Count; i++)
                    {
                        int iCompanyID = Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"]);
                        string sBounceCondition = string.Empty;
                        if (GV.ExcludeEmailBounceInCompleteContactCount && lstBouncedContactIDs.Count > 0)
                            sBounceCondition = " AND ISNULL(CONTACT_ID_P,'') NOT IN (" + GM.ListToQueryString(lstBouncedContactIDs, "String") + ")";
                        if (GV.iMaxValidatedContactCount > 0)
                        {
                            DataRow[] drrMaxCount = dtMasterContacts.Select("MASTER_ID = " + iCompanyID + " AND (TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeValidated + ") OR WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeValidated + ")) " + sBounceCondition);
                            if (drrMaxCount.Length > GV.iMaxValidatedContactCount)
                                AddValidationResults("Company", i, iCompanyID, 0, "Number of validated contacts exceeds " + GV.iMaxValidatedContactCount, false, 2);
                        }

                        if (!lstFreezedMasterIDs.Contains(Convert.ToInt32(dtMasterCompanies.Rows[i]["MASTER_ID"])))
                        {
                            if ((GV.TR_lstDisposalsToBeValidated.Contains(dtMasterCompanies.Rows[i]["TR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase) || GV.WR_lstDisposalsToBeValidated.Contains(dtMasterCompanies.Rows[i]["WR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase)) && GV.iMinValidatedContactCountComplets > 0)
                            {
                                DataRow[] drrMinCount = dtMasterContacts.Select("MASTER_ID = " + iCompanyID + " AND (TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeValidated + ") OR WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeValidated + ")) " + sBounceCondition);
                                if (drrMinCount.Length < GV.iMinValidatedContactCountComplets)
                                    AddValidationResults("Company", i, iCompanyID, 0, "Number of contacts must be at least " + GV.iMinValidatedContactCountComplets + " to have " + dtMasterCompanies.Rows[i][GV.sAccessTo + "_PRIMARY_DISPOSAL"], false, 2);
                            }

                            if (dtMasterCompanies.Rows[i][GV.sAccessTo + "_PRIMARY_DISPOSAL"].ToString().ToUpper() == "PARTIAL COMPLETE" && GV.iMinValidatedContactCountPartialComplets > 0)//Hard coded Partial Complete
                            {
                                DataRow[] drrMinCount = dtMasterContacts.Select("MASTER_ID = " + iCompanyID + " AND (TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeValidated + ") OR WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeValidated + ")) " + sBounceCondition);
                                if (drrMinCount.Length < GV.iMinValidatedContactCountPartialComplets)
                                    AddValidationResults("Company", i, iCompanyID, 0, "Number of contacts must be at least " + GV.iMinValidatedContactCountPartialComplets + " to have PARTIAL COMPLETE", false, 2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Cancel_Asynch("Error Occured...",e);
                lstApplicationError.Add("Contact CheckContactsCount : " + ex.Message);
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void CONTACT_CheckDuplicateEmail(string sFreezedContatactIDs, DoWorkEventArgs e)
        {
            try
            {
                GV.sValidationMessage = "Checking Contact Email Duplicates";
                //Check email duplicate with entire contacts of project
                //DataRow[] drrContactsToValidate = dtMasterContacts.Select("(TR_CONTACT_STATUS IN (" + GV.sEmailCheckContactStatus + ") OR WR_CONTACT_STATUS IN (" + GV.sEmailCheckContactStatus + "))");
                if (RunASynch(e))
                {
                    DataTable dtEmailsToValidate = new DataTable();
                    dtEmailsToValidate.Columns.Add("Master_ID");
                    dtEmailsToValidate.Columns.Add("Contact_ID");
                    dtEmailsToValidate.Columns.Add("RowIndex");
                    dtEmailsToValidate.Columns.Add("Email");
                    dtEmailsToValidate.Columns.Add("Status");
                    for (int i = 0; i < dtMasterContacts.Rows.Count; i++)
                    {
                        if (dtMasterContacts.Rows[i]["CONTACT_EMAIL"].ToString().Length > 0 && (GV.lstEmailChackContactStatus.Contains(dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase) || GV.lstEmailChackContactStatus.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase)))
                        {
                            DataRow drEmailsToValidate = dtEmailsToValidate.NewRow();

                            if (dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString().Length > 0 && lstFreezedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])))
                                drEmailsToValidate["Status"] = "FREEZED";
                            else
                                drEmailsToValidate["Status"] = "UNFREEZED";

                            drEmailsToValidate["Master_ID"] = dtMasterContacts.Rows[i]["MASTER_ID"].ToString();
                            if (dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString().Length > 0)
                                drEmailsToValidate["Contact_ID"] = dtMasterContacts.Rows[i]["CONTACT_ID_P"].ToString();
                            else
                                drEmailsToValidate["Contact_ID"] = 0;
                            drEmailsToValidate["RowIndex"] = i.ToString();
                            drEmailsToValidate["Email"] = dtMasterContacts.Rows[i]["CONTACT_EMAIL"].ToString().Replace("'", "''").Replace("\\", "").Trim();
                            dtEmailsToValidate.Rows.Add(drEmailsToValidate);
                        }
                    }

                    string sEmails = GM.ColumnToQString("Email", dtEmailsToValidate, "String");
                    DataTable dtContactEmailDupe = null;

                    if (sEmails.Length > 0)
                    {

                        if (GV.sAllowDuplicateEmail == "N")
                        {
                            string sDupeType = string.Empty;
                            if (dtMasterCompanies.Rows.Count > 1)
                                sDupeType = "EMAILDUPEINCOMPANYGROUP";
                            else
                                sDupeType = "EMAILDUPEINCOMPANY";


                            //RecordTime("Email linq Append Start");
                            //var lstDupeValues = (from row in dtEmailsToValidate.AsEnumerable()
                            //                       let Email = row.Field<string>("Email")
                            //                       group row by new {Email} into grp
                            //                       where grp.Count() > 1
                            //                       select new
                            //                       {
                            //                           Email = grp.Key.Email,
                            //                       }).ToList();

                            //if (lstDupeValues.Count > 0)
                            //{
                            //    for (int i = 0; i < lstDupeValues.Count; i++ )
                            //    {
                            //        DataRow[] drrEmail = dtEmailsToValidate.Select("Email = '" + lstDupeValues[i].Email.Replace("'", "''") + "'");
                            //        foreach (DataRow drEmail in drrEmail)
                            //            AddValidationResults("Contact", Convert.ToInt32(drEmail["RowIndex"]), Convert.ToInt32(drEmail["Master_ID"]), Convert.ToInt32(drEmail["Contact_ID"]), "", sValidationType, "CONTACT_EMAIL", drEmail["Email"].ToString(), drEmail["RowIndex"].ToString(), drEmail["Master_ID"].ToString(), false, 3);
                            //    }
                            //}
                            //RecordTime("Email linq Append Stop");


                            RecordTime("Email Dupe Within Company Start");
                            foreach (DataRow drEmailsToValidate in dtEmailsToValidate.Rows) //Check Dupe within company
                            {
                                if (drEmailsToValidate["Status"].ToString() == "UNFREEZED")
                                {
                                    DataRow[] drrEmail = dtEmailsToValidate.Select("Email = '" + drEmailsToValidate["Email"].ToString() + "' AND RowIndex <> '" + drEmailsToValidate["RowIndex"] + "'");
                                    if (drrEmail.Length > 0)
                                        AddValidationResults("Contact", Convert.ToInt32(drEmailsToValidate["RowIndex"]), Convert.ToInt32(drEmailsToValidate["Master_ID"]), Convert.ToInt32(drEmailsToValidate["Contact_ID"]), "", sDupeType, "CONTACT_EMAIL", drEmailsToValidate["Email"].ToString(), drrEmail[0]["RowIndex"].ToString(), drrEmail[0]["Master_ID"].ToString(), false, 3);
                                }
                            }
                            RecordTime("Email Dupe Within Company Stop");
                        }

                        if (GV.sAllowDuplicateEmailAcrossProject == "N")
                        {
                            //dtMasterContactsEmail = GlobalVariables.MYSQL.BAL_ExecuteQueryMySQL("SELECT DISTINCT Contact.MASTER_ID, Contact.CONTACT_EMAIL FROM " + GlobalVariables.sContactTable + " Contact WHERE Contact.CONTACT_EMAIL IN (" + sEmails + ") AND Contact.MASTER_ID <> " + sMaster_ID + ";");
                            RecordTime("EmailDupe DB Operation Start");
                            if (GV.sContact_View.Length > 0 && GV.bUseContactView_EmailDupe)
                                dtContactEmailDupe = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT MASTER_ID, CONTACT_EMAIL, PROJECT_NAME, PROJECT_ID FROM " + GV.sContact_View + " WHERE CONTACT_EMAIL IN (" + sEmails + ");");
                            else
                                dtContactEmailDupe = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT Contact.MASTER_ID, Contact.CONTACT_EMAIL FROM " + GV.sContactTable + " Contact WHERE Contact.CONTACT_EMAIL IN (" + sEmails + ") AND (TR_CONTACT_STATUS IN (" + GV.sEmailCheckContactStatus + ") OR WR_CONTACT_STATUS IN (" + GV.sEmailCheckContactStatus + "));");
                            RecordTime("EmailDupe DB Operation Stop");

                            if (dtContactEmailDupe == null)// This ll be null if error occured on connection
                                AddValidationResults("Contact", 0, Convert.ToInt32(sGroup_ID), 0, "Email duplicate Check:<font color = 'Red'> Error connecting server.</font>", false, 3);
                            else
                            {
                                if (dtContactEmailDupe.Rows.Count > 0)
                                {
                                    DataTable dtDistinctEmail = dtContactEmailDupe.DefaultView.ToTable(true, "CONTACT_EMAIL");

                                    List<string> lstReportedEmails = new List<string>();
                                    foreach (DataRow drContact_EmailDupe in dtContactEmailDupe.Rows)
                                    {
                                        string sEmail = drContact_EmailDupe["CONTACT_EMAIL"].ToString().Replace("'", "''").Trim();
                                        DataRow[] drrDupeOccurence = dtContactEmailDupe.Select("CONTACT_EMAIL = '" + sEmail + "'");
                                        bool IsSingleDupe = (drrDupeOccurence.Length == 1);
                                        bool IsOutSide = (GV.sContact_View.Length > 0 && GV.bUseContactView_EmailDupe && (drContact_EmailDupe["PROJECT_ID"].ToString().ToUpper() != GV.sProjectID.ToUpper()));

                                        if ((!IsOutSide && lstMasterIDs.Contains(drContact_EmailDupe["MASTER_ID"].ToString().Trim())) || lstReportedEmails.Contains(sEmail, StringComparer.OrdinalIgnoreCase))
                                            continue;//Ignore duplicates within company. It is handeled in different section

                                        DataRow[] drrContactDetail = dtEmailsToValidate.Select("Email = '" + sEmail + "' AND Status='UNFREEZED'");
                                        string sDupeType = string.Empty;
                                        if (IsSingleDupe && IsOutSide)
                                        {
                                            foreach (DataRow drCon in drrContactDetail)
                                                AddValidationResults("Contact", Convert.ToInt32(drCon["RowIndex"]), Convert.ToInt32(drCon["Master_ID"]), Convert.ToInt32(drCon["Contact_ID"]), "", "EMAILDUPEOUTPROJECT", "CONTACT_EMAIL", drCon["Email"].ToString(), drContact_EmailDupe["MASTER_ID"].ToString(), drContact_EmailDupe["PROJECT_NAME"].ToString(), false, 3);
                                        }
                                        else if (IsSingleDupe && !IsOutSide)
                                        {
                                            foreach (DataRow drCon in drrContactDetail)
                                                AddValidationResults("Contact", Convert.ToInt32(drCon["RowIndex"]), Convert.ToInt32(drCon["Master_ID"]), Convert.ToInt32(drCon["Contact_ID"]), "", "EMAILDUPEINPROJECT", "CONTACT_EMAIL", drCon["Email"].ToString(), drContact_EmailDupe["MASTER_ID"].ToString(), string.Empty, false, 3);
                                        }
                                        else if (!IsSingleDupe && IsOutSide)
                                        {
                                            foreach (DataRow drCon in drrContactDetail)
                                                AddValidationResults("Contact", Convert.ToInt32(drCon["RowIndex"]), Convert.ToInt32(drCon["Master_ID"]), Convert.ToInt32(drCon["Contact_ID"]), drrDupeOccurence.Length.ToString(), "EMAILDUPEOUTPROJECTMULTI", "CONTACT_EMAIL", drCon["Email"].ToString(), drrDupeOccurence[0]["MASTER_ID"].ToString(), drContact_EmailDupe["PROJECT_NAME"].ToString(), false, 3);
                                            lstReportedEmails.Add(sEmail);
                                        }
                                        else if (!IsSingleDupe && !IsOutSide)
                                        {
                                            foreach (DataRow drCon in drrContactDetail)
                                                AddValidationResults("Contact", Convert.ToInt32(drCon["RowIndex"]), Convert.ToInt32(drCon["Master_ID"]), Convert.ToInt32(drCon["Contact_ID"]), drrDupeOccurence.Length.ToString(), "EMAILDUPEINPROJECTMULTI", "CONTACT_EMAIL", drCon["Email"].ToString(), GM.ColumnToQString("Master_ID", drrDupeOccurence.CopyToDataTable(), "Int"), string.Empty, false, 3);
                                            lstReportedEmails.Add(sEmail);
                                        }
                                    }
                                    #region MyRegion
                                    //if (dtDistinctEmail.Rows.Count == dtContactEmailDupe.Rows.Count)
                                    //{
                                    //    if (GV.sContact_View.Length > 0 && GV.bUseContactView_EmailDupe)
                                    //    {
                                    //        foreach (DataRow drContactEmailDupe in dtContactEmailDupe.Rows)
                                    //        {
                                    //            if (drContactEmailDupe["PROJECT_ID"].ToString().ToUpper() == GV.sProjectID.ToUpper())
                                    //            {
                                    //                if (!lstMasterIDs.Contains(drContactEmailDupe["MASTER_ID"].ToString().Trim()))
                                    //                {
                                    //                    DataRow[] drrContactDetail = dtEmailsToValidate.Select("Email = '" + drContactEmailDupe["CONTACT_EMAIL"].ToString().Replace("'", "''") + "'");
                                    //                    AddValidationResults("Contact", Convert.ToInt32(drrContactDetail[0]["RowIndex"]), Convert.ToInt32(drrContactDetail[0]["Master_ID"]), Convert.ToInt32(drrContactDetail[0]["Contact_ID"]), "", "EMAILDUPEOUTPROJECT", "CONTACT_EMAIL", drContactEmailDupe["CONTACT_EMAIL"].ToString(), drContactEmailDupe["MASTER_ID"].ToString(), drContactEmailDupe["PROJECT_NAME"].ToString(), false, 3);
                                    //                }
                                    //            }
                                    //            else
                                    //            {
                                    //                DataRow[] drrContactDetail = dtEmailsToValidate.Select("Email = '" + drContactEmailDupe["CONTACT_EMAIL"].ToString().Replace("'", "''") + "'");
                                    //                AddValidationResults("Contact", Convert.ToInt32(drrContactDetail[0]["RowIndex"]), Convert.ToInt32(drrContactDetail[0]["Master_ID"]), Convert.ToInt32(drrContactDetail[0]["Contact_ID"]), "", "EMAILDUPEOUTPROJECT", "CONTACT_EMAIL", drContactEmailDupe["CONTACT_EMAIL"].ToString(), drContactEmailDupe["MASTER_ID"].ToString(), drContactEmailDupe["PROJECT_NAME"].ToString(), false, 3);
                                    //            }
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        foreach (DataRow dr in dtContactEmailDupe.Rows)
                                    //        {
                                    //            if (!lstMasterIDs.Contains(dr["MASTER_ID"].ToString().Trim()))
                                    //            {
                                    //                DataRow[] drrContactDetail = dtEmailsToValidate.Select("Email = '" + dr["CONTACT_EMAIL"].ToString().Replace("'", "''") + "'");
                                    //                if(drrContactDetail.Length > 0)
                                    //                    AddValidationResults("Contact", Convert.ToInt32(drrContactDetail[0]["RowIndex"]), Convert.ToInt32(drrContactDetail[0]["Master_ID"]), Convert.ToInt32(drrContactDetail[0]["Contact_ID"]), "", "EMAILDUPEINPROJECT", "CONTACT_EMAIL", dr["CONTACT_EMAIL"].ToString(), dr["MASTER_ID"].ToString(), string.Empty, false, 3);

                                    //                //sValidation += "<font color = 'DarkCyan'>Email : </font> <font color = 'Tomato'>" + dr["CONTACT_EMAIL"] + "</font> already exist in another company. ID : <font color = 'Tomato'>" + dr["MASTER_ID"] + "</font>" + Environment.NewLine;
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    foreach (DataRow dr in dtDistinctEmail.Rows)
                                    //    {
                                    //        string sEmail = dr["CONTACT_EMAIL"].ToString().Replace("'", "''");
                                    //        DataRow[] drrSelectedEmail = dtContactEmailDupe.Select("CONTACT_EMAIL = '" + sEmail + "'");                                            
                                    //        string sID = drrSelectedEmail[0]["MASTER_ID"].ToString();
                                    //        string sCount = drrSelectedEmail.Length.ToString();


                                    //        if (GV.sContact_View.Length > 0 && GV.bUseContactView_EmailDupe)
                                    //        {
                                    //            if (drrSelectedEmail.Length > 1)
                                    //            {
                                    //                if (drrSelectedEmail[0]["PROJECT_ID"].ToString().ToUpper() == GV.sProjectID.ToUpper())
                                    //                {
                                    //                    if (!lstMasterIDs.Contains(sID))
                                    //                    {
                                    //                        DataRow[] drrContactDetail = dtEmailsToValidate.Select("Email = '" + sEmail + "'");
                                    //                        foreach (DataRow drContactDetail in drrContactDetail)
                                    //                            AddValidationResults("Contact", Convert.ToInt32(drContactDetail["RowIndex"]), Convert.ToInt32(drContactDetail["Master_ID"]), Convert.ToInt32(drContactDetail["Contact_ID"]), drrSelectedEmail.Length.ToString(), "EMAILDUPEOUTPROJECTMULTI", "CONTACT_EMAIL", drContactDetail["Email"].ToString(), drContactDetail["MASTER_ID"].ToString(), drContactDetail["PROJECT_NAME"].ToString(), false, 3);
                                    //                    }
                                    //                }
                                    //                else
                                    //                {
                                    //                    DataRow[] drrContactDetail = dtEmailsToValidate.Select("Email = '" + sEmail + "'");
                                    //                    foreach (DataRow drContactDetail in drrContactDetail)
                                    //                        AddValidationResults("Contact", Convert.ToInt32(drContactDetail["RowIndex"]), Convert.ToInt32(drContactDetail["Master_ID"]), Convert.ToInt32(drContactDetail["Contact_ID"]), drrSelectedEmail.Length.ToString(), "EMAILDUPEOUTPROJECTMULTI", "CONTACT_EMAIL", drContactDetail["Email"].ToString(), drContactDetail["MASTER_ID"].ToString(), drContactDetail["PROJECT_NAME"].ToString(), false, 3);
                                    //                }
                                    //            }
                                    //            else
                                    //            {
                                    //                if (drrSelectedEmail[0]["PROJECT_ID"].ToString().ToUpper() == GV.sProjectID.ToUpper())
                                    //                {
                                    //                    if (!lstMasterIDs.Contains(sID))
                                    //                    {
                                    //                        DataRow[] drrContactDetail = dtEmailsToValidate.Select("Email = '" + sEmail + "'");
                                    //                        foreach (DataRow drContactDetail in drrContactDetail)
                                    //                            AddValidationResults("Contact", Convert.ToInt32(drContactDetail["RowIndex"]), Convert.ToInt32(drContactDetail["Master_ID"]), Convert.ToInt32(drContactDetail["Contact_ID"]), "", "EMAILDUPEOUTPROJECT", "CONTACT_EMAIL", drContactDetail["Email"].ToString(), drContactDetail["MASTER_ID"].ToString(), drContactDetail["PROJECT_NAME"].ToString(), false, 3);
                                    //                    }
                                    //                }
                                    //                else
                                    //                {
                                    //                    DataRow[] drrContactDetail = dtEmailsToValidate.Select("Email = '" + sEmail + "'");
                                    //                    foreach (DataRow drContactDetail in drrContactDetail)
                                    //                        AddValidationResults("Contact", Convert.ToInt32(drContactDetail["RowIndex"]), Convert.ToInt32(drContactDetail["Master_ID"]), Convert.ToInt32(drContactDetail["Contact_ID"]), "", "EMAILDUPEOUTPROJECT", "CONTACT_EMAIL", drContactDetail["CONTACT_EMAIL"].ToString(), drContactDetail["MASTER_ID"].ToString(), drContactDetail["PROJECT_NAME"].ToString(), false, 3);
                                    //                }
                                    //            }
                                    //        }
                                    //        else
                                    //        {

                                    //            if (!lstMasterIDs.Contains(sID))
                                    //            {
                                    //                if (drrSelectedEmail.Length > 1)
                                    //                {
                                    //                    DataRow[] drrContactDetail = dtEmailsToValidate.Select("Email = '" + sEmail + "'");
                                    //                    AddValidationResults("Contact", Convert.ToInt32(drrContactDetail[0]["RowIndex"]), Convert.ToInt32(drrContactDetail[0]["Master_ID"]), Convert.ToInt32(drrContactDetail[0]["Contact_ID"]), drrSelectedEmail.Length.ToString(), "EMAILDUPEINPROJECTMULTI", "CONTACT_EMAIL", dr["CONTACT_EMAIL"].ToString(), sID, string.Empty, false, 3);
                                    //                }
                                    //                else
                                    //                {
                                    //                    DataRow[] drrContactDetail = dtEmailsToValidate.Select("Email = '" + sEmail + "'");
                                    //                    AddValidationResults("Contact", Convert.ToInt32(drrContactDetail[0]["RowIndex"]), Convert.ToInt32(drrContactDetail[0]["Master_ID"]), Convert.ToInt32(drrContactDetail[0]["Contact_ID"]), "", "EMAILDUPEINPROJECT", "CONTACT_EMAIL", dr["CONTACT_EMAIL"].ToString(), sID, string.Empty, false, 3);
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //} 
                                    #endregion
                                }
                            }
                        }
                    }
                    UpdateNotifier(3);
                }
            }
            catch (Exception ex)
            {
                //Cancel_Asynch("Error Occured...",e);
                lstApplicationError.Add("CheckDuplicateEmail : " + ex.Message);
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private string Email_NameAndDomain_MatchFlag(string sContactEmail, string sContactFirstName, string sContactLastName, int iRow)
        {
            string sResult = string.Empty;
            string sDomainName = string.Empty;
            string sEmailName = string.Empty;
            string sWebSite = string.Empty;
            try
            {
                if (GM.Email_Check(sContactEmail))
                {
                    sDomainName = sContactEmail.Split('@')[1];
                    sEmailName = sContactEmail.Split('@')[0];
                    string sF = string.Empty;//First Letter of firstname
                    string sL = string.Empty;//First Letter of Lastname
                    string sMiddleName = string.Empty;
                    string sMName = string.Empty;//First Letter of Middle Name
                    string sEmailDomain = string.Empty;
                    List<string> lstEmailNames = new List<string>();

                    if (sContactFirstName.Length > 0)//Get First Letter's
                    {
                        sF = sContactFirstName[0].ToString().ToLower();
                        List<string> lstSplit = sContactFirstName.Split(' ').ToList();
                        if (lstSplit.Count > 1)
                        {
                            sContactFirstName = lstSplit[0];
                            sMiddleName = lstSplit[1].Replace(" ", string.Empty).ToLower();
                            if (sMiddleName.Length > 0)
                                sMName = sMiddleName[0].ToString().ToLower();
                        }
                    }

                    if (sContactLastName.Length > 0)//Get First Letter's
                        sL = sContactLastName[0].ToString().ToLower();

                    sContactFirstName = sContactFirstName.Replace(" ", string.Empty).ToLower();//if names contains any space remove it
                    sContactLastName = sContactLastName.Replace(" ", string.Empty).ToLower();

                    //DataTable dtEmailSuggestion = dtPicklist.Select("PicklistCategory = 'EmailSuggestion'", "Remarks ASC").CopyToDataTable();
                    foreach (DataRow drName in dtEmailSuggestion.Rows)
                    {
                        string sEmailFormation = string.Empty;
                        sEmailFormation = drName["PicklistValue"].ToString().Replace("FirstName", sContactFirstName);
                        sEmailFormation = sEmailFormation.Replace("LastName", sContactLastName);
                        sEmailFormation = sEmailFormation.Replace("FName", sF);
                        sEmailFormation = sEmailFormation.Replace("LName", sL);
                        sEmailFormation = sEmailFormation.Replace("MiddleName", sMiddleName);
                        sEmailFormation = sEmailFormation.Replace("MName", sMName);
                        lstEmailNames.Add(sEmailFormation);
                    }

                    if (!lstEmailNames.Contains(sEmailName, StringComparer.OrdinalIgnoreCase))
                    {
                        if (!sResult.Contains("EmailName"))
                            sResult += "EmailName";
                    }

                    //Get website of company
                    if (dtMasterCompanies.Select("MASTER_ID = " + dtMasterContacts.Rows[iRow]["MASTER_ID"])[0]["WEB"].ToString().Trim().Length > 0 && GM.Web_Check(dtMasterCompanies.Select("MASTER_ID = " + dtMasterContacts.Rows[iRow]["MASTER_ID"])[0]["WEB"].ToString()))
                    {
                        sWebSite = dtMasterCompanies.Select("MASTER_ID = " + dtMasterContacts.Rows[iRow]["MASTER_ID"])[0]["WEB"].ToString().Trim();
                        List<string> lstWebSplit = new List<string>();
                        lstWebSplit = sWebSite.Split('/').ToList()[0].Split('.').ToList();

                        for (int i = 1; i < lstWebSplit.Count; i++)
                        {
                            if (sEmailDomain.Length > 0)
                                sEmailDomain += "." + lstWebSplit[i];
                            else
                                sEmailDomain += lstWebSplit[i];
                        }

                        if (sEmailDomain.ToUpper() != sDomainName.ToUpper())
                        {
                            if (!sResult.Contains("EmailDomain"))
                                sResult += "EmailDomain";
                        }
                    }

                    //Check the email of Companies email
                    if (dtMasterCompanies.Select("MASTER_ID = " + dtMasterContacts.Rows[iRow]["MASTER_ID"])[0]["WEB"].ToString().Trim().Length > 0 && GM.Email_Check(dtMasterCompanies.Select("MASTER_ID = " + dtMasterContacts.Rows[iRow]["MASTER_ID"])[0]["WEB"].ToString()))
                    {
                        if (dtMasterCompanies.Select("MASTER_ID = " + dtMasterContacts.Rows[iRow]["MASTER_ID"])[0]["WEB"].ToString().Trim().Split('@')[1].ToUpper() != sDomainName.ToUpper())
                        {
                            if (!sResult.Contains("EmailDomain"))
                                sResult += "EmailDomain";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return sResult;
        }


        //-----------------------------------------------------------------------------------------------------
        private string Email_NameAndDomain_Check()
        {
            string sResult = string.Empty;
            string sContactEmail = string.Empty;
            string sDomainName = string.Empty;
            string sEmailName = string.Empty;
            string sWebSite = string.Empty;
            try
            {
                DataRow[] drrMasterContactsEmail = dtMasterContacts.Select("LEN(CONTACT_EMAIL) > 0");
                foreach (DataRow dr in drrMasterContactsEmail)
                {
                    if (GM.Email_Check(dr["CONTACT_EMAIL"].ToString()))
                    {
                        DataRow[] drValidation = dtValidations.Select(String.Format("CONDITION = '{0}' AND VALIDATION_VALUE = 'CONTACT_EMAIL' AND RESEARCH_TYPE = '{1}' AND VALIDATION_TYPE = 'EMAILDOMAINMATCHWARNING'", dr[GV.sAccessTo + "_CONTACT_STATUS"], GV.sAccessTo));
                        if (drValidation.Length > 0)
                        {
                            sContactEmail = drrMasterContactsEmail[0]["CONTACT_EMAIL"].ToString();
                            sDomainName = sContactEmail.Split('@')[1];
                            sEmailName = sContactEmail.Split('@')[0];

                            string sF = string.Empty;//First Letter of firstname
                            string sL = string.Empty;//First Letter of Lastname
                            string sMiddleName = string.Empty;
                            string sMName = string.Empty;//First Letter of Middle Name
                            string sEmailDomain = string.Empty;
                            string sContactFirstName = dr["FIRST_NAME"].ToString();
                            string sContactLastName = dr["LAST_NAME"].ToString();
                            List<string> lstEmailNames = new List<string>();

                            if (sContactFirstName.Length > 0)//Get First Letter's
                            {
                                sF = sContactFirstName[0].ToString().ToLower();
                                List<string> lstSplit = sContactFirstName.Split(' ').ToList();
                                if (lstSplit.Count > 1)
                                {
                                    sContactFirstName = lstSplit[0];
                                    sMiddleName = lstSplit[1].Replace(" ", string.Empty).ToLower();
                                    if (sMiddleName.Length > 0)
                                        sMName = sMiddleName[0].ToString().ToLower();
                                }
                            }

                            if (sContactLastName.Length > 0)//Get First Letter's
                                sL = sContactLastName[0].ToString().ToLower();

                            sContactFirstName = sContactFirstName.Replace(" ", string.Empty).ToLower();//if names contains any space remove it
                            sContactLastName = sContactLastName.Replace(" ", string.Empty).ToLower();

                            //DataTable dtEmailSuggestion = dtPicklist.Select("PicklistCategory = 'EmailSuggestion'", "Remarks ASC").CopyToDataTable();
                            foreach (DataRow drName in dtEmailSuggestion.Rows)
                            {
                                string sEmailFormation = string.Empty;
                                sEmailFormation = drName["PicklistValue"].ToString().Replace("FirstName", sContactFirstName);
                                sEmailFormation = sEmailFormation.Replace("LastName", sContactLastName);
                                sEmailFormation = sEmailFormation.Replace("FName", sF);
                                sEmailFormation = sEmailFormation.Replace("LName", sL);
                                sEmailFormation = sEmailFormation.Replace("MiddleName", sMiddleName);
                                sEmailFormation = sEmailFormation.Replace("MName", sMName);
                                lstEmailNames.Add(sEmailFormation);
                            }
                            if (!lstEmailNames.Contains(sEmailName, StringComparer.OrdinalIgnoreCase))
                            {
                                if (!sResult.Contains("EmailName"))
                                    sResult += "EmailName";
                            }


                            //Get website of company
                            if (dtMasterCompanies.Rows[0]["WEB"].ToString().Length > 0 && GM.Web_Check(dtMasterCompanies.Rows[0]["WEB"].ToString()))
                            {
                                sWebSite = dtMasterCompanies.Rows[0]["WEB"].ToString();
                                List<string> lstWebSplit = new List<string>();
                                lstWebSplit = sWebSite.Split('/').ToList()[0].Split('.').ToList();

                                for (int i = 1; i < lstWebSplit.Count; i++)
                                {
                                    if (sEmailDomain.Length > 0)
                                        sEmailDomain += "." + lstWebSplit[i];
                                    else
                                        sEmailDomain += lstWebSplit[i];
                                }

                                if (sEmailDomain.ToUpper() != sDomainName.ToUpper())
                                {
                                    if (!sResult.Contains("EmailDomain"))
                                        sResult += "EmailDomain";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return sResult;
        }

        void ClearControls(List<TextBox> lstControlsToClear, string sTableName)
        {
            IsLoading = true;
            foreach (TextBox txt in lstControlsToClear)//Empty all Controls to load next
            {
                //if (C.Name.Length > 0)
                //{
                //if (C is TextBox)
                //{
                if (txt.Text.Length > 0)
                    txt.Text = string.Empty;
                //}
                //else if (C is ComboBox)
                //{
                //    ComboBox cmb = (ComboBox)C;
                //    cmb.Items.Clear();
                //}
                //else if (C is CheckedListBox)
                //{
                //    CheckedListBox chk = (CheckedListBox)C;
                //    for (int i = 0; i <= chk.Items.Count - 1; i++)
                //        chk.SetItemChecked(i, false);
                //}
                // }
            }
            IsLoading = false;
        }

        //-----------------------------------------------------------------------------------------------------
        public void Load_Next_Record()
        {

            if (sFormOpenType == "ButtonOpen" || sFormOpenType == "SendBack")
            {
                if (GV.sUserType == "Agent")
                {
                    ClearControls(lstCompanyControls, "Company");
                    ClearControls(lstContactControls, "Contact");

                    //Load Next Record                    
                    ((frmMain)MdiParent).btnSendBack.NotificationMarkText = ((frmMain)MdiParent).SendBackCount(); // Refresh Sendback Count

                    dtMasterCompanies.Clear();
                    dtMasterContacts.Clear();
                    dtMasterCompaniesCopy.Clear();
                    dtMasterContactsCopy.Clear();

                    if (IsRecordFetched())
                    {
                        GV.sCompanySessionID = GV.IP.Replace(".", string.Empty).Reverse() + GM.GetDateTime().ToString("yyMMddHHmmssff");
                        GM.Moniter("Pre Call");
                        HistoryLoaded = false;
                        if (dtMasterCompanies.Rows.Count > 1)
                        {
                            sTabPanelGroupCompany.Visible = true;
                            tabGroupCompany.Visible = true;
                            lblSpliter1.Visible = true;
                            lblGroupID.Visible = true;
                            txtGroupID.Visible = true;
                        }
                        else
                        {
                            sTabPanelGroupCompany.Visible = false;
                            tabGroupCompany.Visible = false;
                            lblSpliter1.Visible = false;
                            lblGroupID.Visible = false;
                            txtGroupID.Visible = false;
                        }

                        Log_OpenClose("Opened");

                        dtMasterCompaniesCopy = dtMasterCompanies.Copy();
                        dtMasterContactsCopy = dtMasterContacts.Copy();//Disconnected datatable(No link with grid)

                        dtMasterCompaniesCopy.TableName = "MasterCompanyCopy";
                        dtMasterContactsCopy.TableName = "MasterContactCopy";

                        lstMasterIDs.Clear();
                        foreach (DataRow drCompanies in dtMasterCompanies.Rows)
                            lstMasterIDs.Add(drCompanies["MASTER_ID"].ToString());


                        sMaster_ID = dtMasterCompanies.Rows[0]["MASTER_ID"].ToString(); // if group is not the first record
                        sGroup_ID = dtMasterCompanies.Rows[0]["GROUP_ID"].ToString();
                        txtGroupID.Text = sGroup_ID;

                        PopulateMasterCompanyFields();

                        ContactBounce_AND_CompanyContactFreeze();

                        PopulateDisposals();

                        //dgvContacts.DataSource = dtMasterContacts;
                        LoadSuperGridCompany();
                        LoadSuperGridContact();


                        if (GV.sAccessTo == "TR")
                        {
                            tabControlContact.SelectedPanel = sTabPanelTRDisposals; //Select contact panel by default on new loading
                            if (GV.IsCallScriptEnabled)
                                CallScript();
                        }
                        else if (GV.sAccessTo == "WR")
                            tabControlContact.SelectedPanel = sTabPanelWRDisposals; //Select contact panel by default on new loading

                        InitilizeCallTracker();

                    }
                    else
                    {
                        ToastNotification.Show(this.MdiParent, sRecordFetchMessage, eToastPosition.TopRight);
                        this.Close();
                    }
                }
                else//If ADMIN then the record saves and close
                {
                    ToastNotification.Show(((frmMain)MdiParent), "Company Saved.", eToastPosition.TopRight);
                    this.Close();
                }
            }
            else
            {
                ToastNotification.Show(((frmMain)MdiParent), "Company Saved.", eToastPosition.TopRight);
                this.Close();
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private bool SaveToDB(DataTable dtSave, string sTableName)
        {
            GV.sValidationMessage = "Pushing data to database";
            try
            {
                bool IsSaveSucess = true;
                if (dtSave.GetChanges(DataRowState.Added) != null)
                    IsSaveSucess = GV.MYSQL.BAL_SaveToTableMySQL(dtSave.GetChanges(DataRowState.Added), sTableName, "New", false);
                if (dtSave.GetChanges(DataRowState.Modified) != null)
                    IsSaveSucess = GV.MYSQL.BAL_SaveToTableMySQL(dtSave.GetChanges(DataRowState.Modified), sTableName, "Update", false);
                if (dtSave.GetChanges(DataRowState.Deleted) != null)
                    IsSaveSucess = GV.MYSQL.BAL_SaveToTableMySQL(dtSave.GetChanges(DataRowState.Deleted), sTableName, "Delete", false);
                return IsSaveSucess;
            }
            catch (Exception ex)
            {
                //CloseNotifier();
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsRecordSaved = false;
            this.Close();
        }

        //-----------------------------------------------------------------------------------------------------
        private void sliderNotesSize_ValueChanged(object sender, EventArgs e)
        {
            //txtNotes.Font = new Font(txtNotes.Font.FontFamily, sliderNotesSize.Value);
            txtNotes.SelectAll();
            txtNotes.SelectionFont = new Font("Tahoma", sliderNotesSize.Value);
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnWindowUp_Click(object sender, EventArgs e)//Opens the agents notes on seperate window
        {
            try
            {
                if (IsNoteFormOpened)
                {
                    objFrmWindowedNotes.Activate();
                }
                else
                {
                    IsNoteFormOpened = true;
                    objFrmWindowedNotes = new frmWindowedNotes();
                    objFrmWindowedNotes.Owner = this;
                    objFrmWindowedNotes.FormClosed += new FormClosedEventHandler(frmNotes_Closed); // Runtime event for frmWindowedNotes form
                    objFrmWindowedNotes.sNoteText = txtNotes.Rtf;
                    objFrmWindowedNotes.iNoteTextSize = Convert.ToInt32(txtNotes.Font.Size);
                    objFrmWindowedNotes.TopMost = true;
                    objFrmWindowedNotes.WindowState = FormWindowState.Normal;
                    tabAgentsNotes.Visible = false;
                    tabControlContact.SelectedTab = tabContacts;
                    objFrmWindowedNotes.Show();

                    
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void frmNotes_Closed(object sender, FormClosedEventArgs e)//Triggers when frmWindowedNotes window closed
        {
            try
            {
                //Gets the text and settings back to main window
                txtNotes.Rtf = objFrmWindowedNotes.sNoteText;
                txtNotes.SelectionFont = new System.Drawing.Font("Tahoma", objFrmWindowedNotes.iNoteTextSize);
                sliderNotesSize.Value = objFrmWindowedNotes.iNoteTextSize;
                tabAgentsNotes.Visible = true;
                tabControlContact.SelectedTab = tabAgentsNotes;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        protected override void OnClientSizeChanged(EventArgs e)//To minimize frmWindowedNotes window with main window
        {
            if (this.WindowState != mLastState)
            {
                mLastState = this.WindowState;
                OnWindowStateChanged(e);
            }

            base.OnClientSizeChanged(e);
        }
        //-----------------------------------------------------------------------------------------------------
        protected void OnWindowStateChanged(EventArgs e)//To minimize frmWindowedNotes window with main window
        {
            if (IsNoteFormOpened)
            {

                if (this.MdiParent.WindowState == FormWindowState.Minimized)
                {
                    objFrmWindowedNotes.TopMost = false;
                    objFrmWindowedNotes.WindowState = FormWindowState.Minimized;
                }
                else if (this.MdiParent.WindowState == FormWindowState.Maximized || this.MdiParent.WindowState == FormWindowState.Normal)
                {
                    objFrmWindowedNotes.TopMost = true;
                    objFrmWindowedNotes.WindowState = FormWindowState.Normal;
                }
            }


        }

        //-----------------------------------------------------------------------------------------------------
        private void btnAddNewGridRow_Click(object sender, EventArgs e)
        {
            try
            {

                //dtMasterContacts.WriteXml(AppDomain.CurrentDomain.BaseDirectory + @"\xx.xml");

                //DataSet ds = new DataSet();
                //ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"\xx.xml");

                DataRow drMasterContacts = dtMasterContacts.NewRow();
                // DataRow drMasterContactsSQLCE = dtMasterContactsSQLCE.NewRow();

                foreach (DataColumn dc in dtMasterContacts.Columns)
                {
                    drMasterContacts[dc.ColumnName] = DBNull.Value;
                }

                drMasterContacts["MASTER_ID"] = sMaster_ID;
                //drMasterContactsSQLCE["MASTER_ID"] = sMaster_ID;

                drMasterContacts["CREATED_BY"] = GV.sEmployeeName;
                drMasterContacts["CREATED_DATE"] = GM.GetDateTime();

                drMasterContacts["NEW_OR_EXISTING"] = "NEW";
                //drMasterContactsSQLCE["NEW_OR_EXISTING"] = "NEW";

                if (GV.sAccessTo == "TR")
                {
                    drMasterContacts["TR_AGENT_NAME"] = GV.sEmployeeName;
                    //drMasterContactsSQLCE["TR_AGENT_NAME"] = GV.sEmployeeName;

                    drMasterContacts["TR_UPDATED_DATE"] = GM.GetDateTime();
                }
                else if (GV.sAccessTo == "WR")
                {
                    drMasterContacts["WR_AGENT_NAME"] = GV.sEmployeeName;
                    //drMasterContactsSQLCE["WR_AGENT_NAME"] = GV.sEmployeeName;

                    drMasterContacts["WR_UPDATED_DATE"] = GM.GetDateTime();
                }

                dtMasterContacts.Rows.Add(drMasterContacts);
                //dtMasterContactsSQLCE.Rows.Add(drMasterContactsSQLCE);

                //SplitContact.SplitterDistance = dgvContacts.Rows[0].Height * dgvContacts.Rows.Count + 20;
                //SplitContactExpanded.SplitterDistance = dgvContacts.Rows[0].Height * dgvContacts.Rows.Count + 20;


                //Super Grid
                GridCell gridCellContact = new GridCell();
                GridCell gridCellUniqueNumber = new GridCell();
                gridCellUniqueNumber.Value = dtMasterContacts.Rows.Count - 1;//Unique numbers (refers the dgvContacts 's Row index)

                GridRow gridRow = new GridRow();
                gridCellContact.Value = dtMasterContacts.Rows.Count + ".";
                gridRow.Cells.Add(gridCellContact);
                gridRow.Cells.Add(gridCellUniqueNumber);
                if (GV.lstSortableContactColumn != null)
                {
                    for (int i = 0; i < GV.lstSortableContactColumn.Count; i++)
                    {
                        GridCell gridCell = new GridCell();
                        gridRow.Cells.Add(gridCell);
                    }
                }
                gridRow.Expanded = true;
                sdgvContacts.PrimaryGrid.ClearSelectedRows();
                sdgvContacts.PrimaryGrid.Rows.Add(gridRow);

                sdgvContacts.Invoke((MethodInvoker)delegate { sdgvContacts.PrimaryGrid.SetActiveRow(gridRow); });

                //sdgvContacts.PrimaryGrid.SetActiveRow(gridRow);
                sdgvContacts.PrimaryGrid.Select(gridRow);
                sdgvContacts.PrimaryGrid.SetSelectedRows(dtMasterContacts.Rows.Count - 1, 1, true);
                gridRow.IsSelected = true;
                gridRow.SetActive(true);
                sdgvContacts.Invoke((MethodInvoker)delegate { sdgvContacts.Refresh(); });
                //sdgvContacts.Refresh();

                //sdgvContacts.PrimaryGrid.SetActiveRow

                //sdgvContacts.PrimaryGrid.AutoSelectNewBoundRows = true;
                //gridRow.IsSelected = true;
                //sdgvContacts.PrimaryGrid.

                if (!splitContactModernUI.Panel1Collapsed)
                {
                    if (sdgvContacts.VScrollBar.Visible)
                    {
                        sdgvContacts.Invoke((MethodInvoker)delegate { sdgvContacts.PrimaryGrid.Columns[0].Width = sdgvContacts.Width - 18; });
                        //sdgvContacts.PrimaryGrid.Columns[0].Width = sdgvContacts.Width - 18;
                        //sdgvContacts.VScrollOffset = sdgvContacts.VScrollMaximum;
                        sdgvContacts.Invoke((MethodInvoker)delegate { sdgvContacts.VScrollOffset = sdgvContacts.VScrollMaximum; });
                    }
                    else
                        sdgvContacts.Invoke((MethodInvoker)delegate { sdgvContacts.PrimaryGrid.Columns[0].Width = sdgvContacts.Width; });
                    //sdgvContacts.PrimaryGrid.Columns[0].Width = sdgvContacts.Width;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void TimerTick(object sender, EventArgs e)//Runtime event assigned to tErrorTextAutoClose object
        {

            dtMasterCompanies.WriteXml(sRecoveryPath + "\\" + sMaster_ID + "_COM.XML");
            dtMasterContacts.WriteXml(sRecoveryPath + "\\" + sMaster_ID + "_CON.XML");

            //WriteTableToFile(dtMasterContactsSQLCE, GV.sSQLCEContactTable);//Backup//Crash Recovery
            //WriteTableToFile(dtMasterCompaniesSQLCE, GV.sSQLCECompanyTable);

        }

        //-----------------------------------------------------------------------------------------------------
        private void frmContactsUpdate_Shown(object sender, EventArgs e)
        {
            //if (sFormOpenType == "ListOpen")//Select disposal by default if opend by list
            {
                if (GV.sAccessTo == "TR")
                    tabControlContact.SelectedPanel = sTabPanelTRDisposals;
                else if (GV.sAccessTo == "WR")
                    tabControlContact.SelectedPanel = sTabPanelWRDisposals;
            }
            if (!IsRecordFetchedFlag)
                this.Close();

            expandablePanelMessage.Expanded = false;

            //foreach (Control cMaster in lstMasterControls)
            //{
            //    if (cMaster is TextBox && ((TextBox)cMaster).ReadOnly == false)
            //    {
            //        cMaster.Focus();
            //        return;
            //    }
            //}

            if (!splitContactModernUI.Panel1Collapsed)
            {
                AlignContactGridColumn();
            }

            //while(Dictionary.DefaultDictionary == null)
            //{
            //    Application.DoEvents();
            //}

            //((i00SpellCheck.UserDictionaryBase)(Dictionary.DefaultDictionary)).UserWordList.Count < dtSpellIgnore.Rows.Count
            if (Dictionary.DefaultDictionary != null && Dictionary.DefaultDictionary.Count > 0)
            {
                //Exceptional Spellings
                foreach (DataRow drSpell in dtSpellIgnore.Rows)
                    Dictionary.DefaultDictionary.Ignore(drSpell["PicklistValue"].ToString().ToLower());
            }

            if (objfrmCallScript != null)
                objfrmCallScript.Activate();//Focus call script when on open

            if (GV.sUserType == "Agent" && GV.sAccessTo == "TR")
            {
                if (GV.iPreCallLimit > 0 || GV.iPostCallLimit > 0)
                {

                    tPrePostCall.Start();
                    itemContainerPrePostCall.Visible = true;
                }
                else
                {
                    itemContainerPrePostCall.Visible = false;
                    rbnBarDial.Refresh();
                }
            }
            else
                itemContainerPrePostCall.Visible = false;

            GV.sPerformance += "Complete : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;

        }

        //-----------------------------------------------------------------------------------------------------
        private void RestorePreviousAgentNameAndDeleteRecovery()
        {
            IsRecordSaved = false;
            dtMasterCompanies.RejectChanges();
            //dtMasterCompanies = GlobalVariables.MYSQL.BAL_FetchTable(GlobalVariables.sCompanyTable, "MASTER_ID = " + sRecordID);//Master Table
            for (int i = 0; i < dtMasterCompanies.Rows.Count; i++)
            {
                if (GV.sAccessTo == "TR")
                    dtMasterCompanies.Rows[i]["TR_AGENTNAME"] = dtMasterCompanies.Rows[i]["TR_PREVIOUS_AGENTNAME"];
                else if (GV.sAccessTo == "WR")
                    dtMasterCompanies.Rows[i]["WR_AGENTNAME"] = dtMasterCompanies.Rows[i]["WR_PREVIOUS_AGENTNAME"];
            }

            SaveToDB(dtMasterCompanies.GetChanges(DataRowState.Modified), GV.sCompanyTable);
            Remove_Chached();

        }

        //-----------------------------------------------------------------------------------------------------
        private void frmContactsUpdate_FormClosed(object sender, FormClosedEventArgs e)
        {
            //try
            //{
            //    //if (e.CloseReason == CloseReason.UserClosing)
            //    if (objfrmCallScript != null)
            //        objfrmCallScript.Close();
            //    if (!IsRecordSaved && dtMasterCompanies != null && dtMasterCompanies.Rows.Count > 0 && GlobalVariables.sUserType == "Agent")
            //    {
            //        dtMasterCompanies.RejectChanges();
            //        //dtMasterCompanies = GlobalVariables.MYSQL.BAL_FetchTable(GlobalVariables.sCompanyTable, "MASTER_ID = " + sRecordID);//Master Table
            //        if (GlobalVariables.sAccessTo == "TR")
            //            dtMasterCompanies.Rows[0]["TR_AGENTNAME"] = dtMasterCompanies.Rows[0]["TR_PREVIOUS_AGENTNAME"];
            //        else if (GlobalVariables.sAccessTo == "WR")
            //            dtMasterCompanies.Rows[0]["WR_AGENTNAME"] = dtMasterCompanies.Rows[0]["WR_PREVIOUS_AGENTNAME"];
            //        SaveToDB(dtMasterCompanies.GetChanges(DataRowState.Modified), GlobalVariables.sCompanyTable);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}                       
        }

        //-----------------------------------------------------------------------------------------------------
        private void frmContactsUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ///e.CloseReason = CloseReason.TaskManagerClosing

                if (objfrmCallScript != null)//Close call script before closing main window
                    objfrmCallScript.Close();


                if (!IsRecordFetchedFlag)
                    return;

                if (GV.sAccessTo == "TR") Hangup();

                if (IsRecordSaved)//if agent saved the record, no restorayetion needed
                {
                    Remove_Chached();
                    GV.sCompanySessionID = string.Empty;
                    GV.sCurrentCompanyName = string.Empty;
                    GV.sCurrentCompanyID = string.Empty;
                    GM.Moniter("");
                    return;
                }

                if (GV.sUserType == "Agent")// previous agent name has to restored if agent closes without saving
                {
                    if (btnSave.Enabled)// only prompts when save button is enabled
                    {
                        if (DialogResult.Yes == MessageBoxEx.Show(String.Format("Are you sure to close?{0}Changes will not be saved.", Environment.NewLine), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
                        {
                            RestorePreviousAgentNameAndDeleteRecovery();//Restores the previous agent name
                            Log_OpenClose("Closed without Saving");
                            GM.Moniter("");
                            objNotifier.Close();
                            GV.sCompanySessionID = string.Empty;
                            GV.sCurrentCompanyName = string.Empty;
                            GV.sCurrentCompanyID = string.Empty;




                            this.FormClosing -= new FormClosingEventHandler(frmContactsUpdate_FormClosing);
                            this.Close();//When closing from main window
                            this.FormClosing += new FormClosingEventHandler(frmContactsUpdate_FormClosing);
                        }
                        else
                            e.Cancel = true;
                    }
                    else
                    {
                        RestorePreviousAgentNameAndDeleteRecovery();
                        Log_OpenClose("Closed without Saving");
                        GM.Moniter("");
                        objNotifier.Close();
                        GV.sCompanySessionID = string.Empty;
                        GV.sCurrentCompanyName = string.Empty;
                        GV.sCurrentCompanyID = string.Empty;




                        this.FormClosing -= new FormClosingEventHandler(frmContactsUpdate_FormClosing);
                        this.Close();//When closing from main window
                        this.FormClosing += new FormClosingEventHandler(frmContactsUpdate_FormClosing);



                    }
                }//for admins// Will only warn before saving// No changes are done here
                else if (dtMasterCompanies.Rows.Count > 0 && DialogResult.Yes == MessageBoxEx.Show(String.Format("Are you sure to close?{0}Changes will not be saved.", Environment.NewLine), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
                {
                    Log_OpenClose("Closed without Saving");
                    GM.Moniter("");
                    objNotifier.Close();
                    Remove_Chached();
                    GV.sCompanySessionID = string.Empty;
                    GV.sCurrentCompanyName = string.Empty;
                    GV.sCurrentCompanyID = string.Empty;
                }
                else
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void Impact_ContactTable(int iTableRowIndex)
        {
            try
            {
                if (GV.sUserType == "Agent" || GV.sUserType == "Manager")
                {
                    string sContactID = dtMasterContacts.Rows[iTableRowIndex]["CONTACT_ID_P"].ToString();
                    if (sContactID.Length > 0 && (lstFreezedContactIDs.Contains(Convert.ToInt32(sContactID)) || lstFreezedMasterIDs.Contains(Convert.ToInt32(sMaster_ID))))
                    {
                        TableToTextContact(iTableRowIndex, "GridToControls", false); //Populate selected row data to master contacts controls//Disabled Controls
                        return;
                    }
                    TableToTextContact(iTableRowIndex, "GridToControls", true); //Populate selected row data to master contacts controls//Enabled controls for agent
                }
                else
                    TableToTextContact(iTableRowIndex, "GridToControls", true); //Populate selected row data to master contacts controls//For admin

            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void Impact_CompanyTable(int iTableRowIndex)
        {
            try
            {
                tabControlCompany.SelectedPanel = sTabPanelCompanyInformation;

                if (GV.sUserType == "Agent")
                {
                    if (lstFreezedMasterIDs.Contains(Convert.ToInt32(dtMasterCompanies.Rows[iTableRowIndex]["MASTER_ID"])))
                    {
                        TableToTextCompany(iTableRowIndex, "GridToControls", false); //Populate selected row data to master contacts controls//Disabled Controls
                        return;
                    }
                    TableToTextCompany(iTableRowIndex, "GridToControls", true); //Populate selected row data to master contacts controls//Enabled controls for agent
                }
                else
                    TableToTextCompany(iTableRowIndex, "GridToControls", true); //Populate selected row data to master contacts controls//For admin


            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            /////Key press listener for overall window/////

            //ToastNotification.Show(this, keyData.ToString(), eToastPosition.TopRight);

            //if (keyData == (Keys.Control | Keys.Up)) /////'
            //{
            //    tabControlMain.SelectPreviousTab();
            //}
            //else if (keyData == (Keys.Control | Keys.Down)) /////'
            //{
            //    tabControlMain.SelectNextTab();
            //}


            //if (keyData == (Keys.Control | Keys.Right))
            //    ((frmMain)MdiParent).tabItemShortcutHandle("Right");

            //if (keyData == (Keys.Control | Keys.Left))
            //    ((frmMain)MdiParent).tabItemShortcutHandle("Left");

            if (keyData == Keys.F1)
                GM.OpenHelp();

            try
            {
                if (tabControlContact.SelectedTab.Name == "tabContacts" && dtMasterContacts.Rows.Count > 0)
                {
                    if (keyData == Keys.Down)
                    {
                        if ((iContactRowIndex + 1) < dtMasterContacts.Rows.Count)
                        {

                            sdgvContacts.PrimaryGrid.ClearSelectedRows();
                            sdgvContacts.PrimaryGrid.SetSelectedRows(iContactRowIndex + 1, 0, true);
                            sdgvContacts.PrimaryGrid.GetCell(iContactRowIndex + 1, 0).SetActive(true);
                            // sdgvContacts.PrimaryGrid.GetCell(iContactRowIndex + 1, 0).IsSelected = true;
                            sdgvContacts.PrimaryGrid.SetActiveRow(sdgvContacts.PrimaryGrid.ActiveRow);
                            
                        }
                    }

                    if (keyData == Keys.Up)
                    {
                        if ((iContactRowIndex - 1) >= 0)
                        {
                            sdgvContacts.PrimaryGrid.ClearSelectedRows();
                            sdgvContacts.PrimaryGrid.SetSelectedRows(iContactRowIndex - 1, 0, true);
                            sdgvContacts.PrimaryGrid.GetCell(iContactRowIndex - 1, 0).SetActive(true);
                            // sdgvContacts.PrimaryGrid.GetCell(iContactRowIndex + 1, 0).IsSelected = true;
                            sdgvContacts.PrimaryGrid.SetActiveRow(sdgvContacts.PrimaryGrid.ActiveRow);
                        }
                    }

                    if (keyData == (Keys.Control | Keys.Home))
                    {
                        sdgvContacts.PrimaryGrid.ClearSelectedRows();
                        sdgvContacts.PrimaryGrid.SetSelectedRows(0, 0, true);
                        sdgvContacts.PrimaryGrid.GetCell(0, 0).SetActive(true);                        
                        sdgvContacts.PrimaryGrid.SetActiveRow(sdgvContacts.PrimaryGrid.ActiveRow);
                    }

                    if (keyData == (Keys.Control | Keys.End))
                    {
                        sdgvContacts.PrimaryGrid.ClearSelectedRows();
                        sdgvContacts.PrimaryGrid.SetSelectedRows(dtMasterContacts.Rows.Count - 1, 0, true);
                        sdgvContacts.PrimaryGrid.GetCell(dtMasterContacts.Rows.Count - 1, 0).SetActive(true);
                        sdgvContacts.PrimaryGrid.SetActiveRow(sdgvContacts.PrimaryGrid.ActiveRow);
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            if (keyData == (Keys.Alt | Keys.F6))
            {
                int iTabCount = 0;
                foreach (SuperTabItem sTab in tabControlContact.Tabs)
                {
                    if (sTab.Visible)
                        iTabCount++;
                }

                if (GV.sUserType != "Agent")//Temp fix
                    iTabCount--;

                if (tabControlContact.SelectedTabIndex == iTabCount)
                {
                    tabControlContact.SelectedPanel = sTabPanelContacts;
                }
                else
                    tabControlContact.SelectNextTab();
            }


            if (keyData == (Keys.Alt | Keys.Shift | Keys.F6))
                tabControlContact.SelectPreviousTab();

            if (keyData == Keys.F6)
                CycleFocus(false);

            if (keyData == (Keys.Shift | Keys.F6))
                CycleFocus(true);

            if (keyData == (Keys.Control | Keys.M))
            {
                if (expandablePanelMessage.Expanded)
                    expandablePanelMessage.Expanded = false;
                else
                    expandablePanelMessage.Expanded = true;

                if (GV.sEmployeeName == "THANGAPRAKASH")
                {


                    //if (sdgvContacts.VScrollBarVisible)
                    //{
                    //    sdgvContacts.VScrollOffset = Convert.ToInt32((((Convert.ToDouble(txtTRComments.Text) / sdgvContacts.PrimaryGrid.Rows.Count) * 100) * sdgvContacts.VScrollMaximum)/100) - 200;
                    //}
                    //    //for (int i = 0; i < 100; i++)
                    //    //    dtMasterContacts.Rows[i]["TR_CONTACT_STATUS"] = "NEW AND COMPLETE";

                    //    //foreach (DataRow dr in dtMasterContacts.Rows)
                    //    //{
                    //    //    dr["TR_CONTACT_STATUS"] = "NEW AND COMPLETE";
                    //    //    dr["TITLE"] = "Mr";
                    //    //    dr["FIRST_NAME"] = "Mani"+dr.Table.Rows.IndexOf(dr);
                    //    //    dr["LAST_NAME"] = "Prakash";
                    //    //    dr["CONTACT_LOCATION"] = "Same";
                    //    //    dr["JOB_TITLE"] = "Others";
                    //    //    dr["OTHERS_JOBTITLE"] = "Manager of informed from";
                    //    //}

                    //    //MessageBox.Show(GlobalMethods.GetDateTime().ToString());
                }
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.N))
            {
                //if (objFrmWindowedNotes == null)
                btnWindowed.PerformClick();
                //else if (!objFrmWindowedNotes.TopMost)
                //{
                //    objFrmWindowedNotes.WindowState = FormWindowState.Normal;
                //    objFrmWindowedNotes.Activate();
                //    objFrmWindowedNotes.TopMost = true;
                //    objFrmWindowedNotes.txtNotes.Focus();
                //}
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        //-----------------------------------------------------------------------------------------------------
        private void tabControlMain_SelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
        {
            //ToastNotification.Show(this, tabControlMain.SelectedPanel.Name, eToastPosition.TopRight);
            if (e.NewValue.Name == "tabContacts")
            {
                foreach (TextBox txt in lstContactControls)
                {
                    txt.Focus();
                    break;
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void CycleFocus(bool IsReverseFocus)
        {
            List<Control> lstPanelToFocus = new List<Control>();
            foreach (TextBox txt in lstCompanyControls)
            {
                if (txt.Focused)
                {
                    if (IsReverseFocus)
                    {
                        foreach (TextBox cMasterContact in lstContactControls)
                        {
                            cMasterContact.Focus();
                            return;
                        }
                    }
                    else
                        //if (splitContactModernUI.Panel1Collapsed)
                        //    dgvContacts.Focus();
                        //else
                        sdgvContacts.Focus();
                    return;
                }
            }

            if (sdgvContacts.Focused)
            {
                if (IsReverseFocus)
                {
                    foreach (TextBox cMaster in lstCompanyControls)
                    {
                        if (cMaster.ReadOnly == false)
                        {
                            cMaster.Focus();
                            return;
                        }
                    }
                }
                else
                {
                    tabControlContact.SelectedPanel = sTabPanelContacts;
                    foreach (TextBox cMasterContact in lstContactControls)
                    {
                        cMasterContact.Focus();
                        return;
                    }
                }
            }

            foreach (TextBox txt in lstContactControls)
            {
                if (txt.Focused)
                {
                    if (IsReverseFocus)
                    {
                        //if (splitContactModernUI.Panel1Collapsed)
                        //    dgvContacts.Focus();
                        //else
                        sdgvContacts.Focus();
                    }
                    else
                    {
                        foreach (TextBox cMaster in lstCompanyControls)
                        {
                            if (cMaster.ReadOnly == false)
                            {
                                cMaster.Focus();
                                return;
                            }
                        }
                    }
                }
            }

            if (splitContactModernUI.Panel1Collapsed)//If nothing focused default focus grid
                sdgvContacts.Focus();
        }



        //-----------------------------------------------------------------------------------------------------
        private void btnCallScript_Click(object sender, EventArgs e)
        {
            if (GV.sCallScriptPath.Length > 0)
                CallScript();
        }

        //public void Resize()
        //{
        //    foreach (Control C in lstControls)
        //    {
        //        if (C is TextBox && NHunspellTextBoxExtender1.GetSpellCheckEnabled(C))
        //        {
        //            lstSpellCheckControl.Add(C);
        //            TextBoxBase txt = (TextBox)C;
        //            NHunspellTextBoxExtender1.DisableTextBoxBase(ref txt);
        //        }
        //    }
        //}

        //-----------------------------------------------------------------------------------------------------
        private void CallScript()
        {
            try
            {
                if (GV.sCallScriptPath.Length > 0)
                {

                    bool IsFormOpen = false;
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Name == "frmCallScript")
                        {
                            IsFormOpen = true;
                            f.Focus();
                            break;
                        }
                    }

                    if (IsFormOpen == false && GV.sCompanyTable.Length > 0 && GV.sContactTable.Length > 0)
                    {
                        objfrmCallScript = new frmCallScript();
                        //foreach (TextBox txt in lstCompanyControls)
                        //{
                        //    if (txt.Name.ToUpper() == "SWITCHBOARD")
                        //        objfrmCallScript.txtDialCallScript.Text = txt.Text;
                        //}

                        objfrmCallScript.txtDialCallScript.Text = dtMasterCompanies.Rows[iCompanyRowIndex]["SWITCHBOARD"].ToString();

                        objfrmCallScript.IsOutofTimeZone = IsOutOfTimeZone;
                        objfrmCallScript.TopMost = true;
                        objfrmCallScript.frmMDI = this.MdiParent;
                        objfrmCallScript.StartPosition = FormStartPosition.CenterScreen;
                        objfrmCallScript.iCurIndex = iCompanyRowIndex;
                        objfrmCallScript.dtCompany = dtMasterCompanies;
                        objfrmCallScript.dtContact = dtMasterContacts;
                        objfrmCallScript.frmContactUpdate = this;
                        objfrmCallScript.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (e.Modifiers == Keys.Shift || (e.KeyCode == Keys.Space && txt.SelectionStart == txt.Text.Length))//Allow Free typing when Shift, Caps Lock, Space are pressed(Won't trigger Proper case event)
                IsShiftPressed = true;
            else
                IsShiftPressed = false;
        }

        //-----------------------------------------------------------------------------------------------------
        //private void btnDialButton_Click(object sender, EventArgs e)
        //{
        //    if (txtDialMain.Text.Replace(" ", string.Empty).Replace("+", string.Empty).Length >= 8)
        //    {
        //        if (IsOutOfTimeZone)
        //        {
        //            if (DialogResult.Yes == MessageBoxEx.Show(String.Format("This Country is out of Time Zone.{0}Do you still wish to call?", Environment.NewLine), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        //                GM.Dial(txtDialMain.Text);
        //            else
        //                return;
        //        }
        //        else
        //            GM.Dial(txtDialMain.Text);
        //    }
        //    else
        //        ToastNotification.Show(this, "Invalid Telephone Number", eToastPosition.TopRight);
        //}

        //-----------------------------------------------------------------------------------------------------
        private void RefreshCompanyGrid(int iRowIndex)
        {
            foreach (GridRow grd in sdgvCompany.PrimaryGrid.Rows)
            {
                if (grd.Cells[1].Value.ToString() == iRowIndex.ToString())
                {
                    string sCompanyName = string.Empty;
                    sCompanyName = iRowIndex + 1 + " <font color = 'Gray'>(" + dtMasterCompanies.Rows[iRowIndex]["MASTER_ID"].ToString() + ")</font>. " + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[iRowIndex]["COMPANY_NAME"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty));

                    string sHTMLValue = string.Empty;
                    sHTMLValue += "<div align='left' width ='0'><div align='left'><font size = '11'>" + sCompanyName + "</font></div><br/>";
                    string sAddress = string.Empty;

                    if (dtMasterCompanies.Rows[iRowIndex]["ADDRESS_1"].ToString().Length > 0)
                        sAddress = GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[iRowIndex]["ADDRESS_1"].ToString());

                    if (dtMasterCompanies.Rows[iRowIndex]["ADDRESS_2"].ToString().Length > 0)
                    {
                        if (sAddress.Length > 0)
                            sAddress += " ," + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[iRowIndex]["ADDRESS_2"].ToString());
                        else
                            sAddress = GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[iRowIndex]["ADDRESS_2"].ToString());
                    }

                    if (dtMasterCompanies.Rows[iRowIndex]["CITY"].ToString().Length > 0)
                    {
                        if (sAddress.Length > 0)
                            sAddress += " ," + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[iRowIndex]["CITY"].ToString());
                        else
                            sAddress = GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[iRowIndex]["CITY"].ToString());
                    }

                    if (dtMasterCompanies.Rows[iRowIndex]["COUNTRY"].ToString().Length > 0)
                    {
                        if (sAddress.Length > 0)
                            sAddress += " ," + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[iRowIndex]["COUNTRY"].ToString());
                        else
                            sAddress = GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[iRowIndex]["COUNTRY"].ToString());
                    }

                    sHTMLValue += "<div align='Left'><font color = 'Gray' size = '10'>" + sAddress + "</font></div>";

                    if (
                    (
                    GV.sAccessTo == "TR" &&
                    (
                    GV.TR_lstDisposalsToBeFreezed.Contains(dtMasterCompanies.Rows[iRowIndex]["TR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase)
                    ||
                    GV.TR_lstDisposalsToBeValidated.Contains(dtMasterCompanies.Rows[iRowIndex]["TR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase)
                    )
                    )
                    ||
                    (
                    GV.sAccessTo == "WR" &&
                    (
                    GV.WR_lstDisposalsToBeFreezed.Contains(dtMasterCompanies.Rows[iRowIndex]["WR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase)
                    ||
                    GV.WR_lstDisposalsToBeValidated.Contains(dtMasterCompanies.Rows[iRowIndex]["WR_PRIMARY_DISPOSAL"].ToString(), StringComparer.OrdinalIgnoreCase)
                    )
                    )
                    )
                    {
                        sHTMLValue += "<div align = 'right'><font color = 'Green'>" + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[iRowIndex][GV.sAccessTo + "_PRIMARY_DISPOSAL"].ToString()) + "</font></div>";
                        sHTMLValue += "<div align = 'right'><font color = 'Green'>" + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[iRowIndex][GV.sAccessTo + "_SECONDARY_DISPOSAL"].ToString()) + "</font></div>";
                    }
                    else
                    {
                        sHTMLValue += "<div align = 'right'><font color = 'Gray'>" + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[iRowIndex][GV.sAccessTo + "_PRIMARY_DISPOSAL"].ToString()) + "</font></div>";
                        sHTMLValue += "<div align = 'right'><font color = 'Gray'>" + GM.ProperCase_ProjectSpecific(dtMasterCompanies.Rows[iRowIndex][GV.sAccessTo + "_SECONDARY_DISPOSAL"].ToString()) + "</font></div>";
                    }

                    //-----------------------------------------------------------------------------------------------------
                    #region MyRegion
                    //-----------------------------------------------------------------------------------------------------

                    //if (GV.sUserType != "Agent")
                    //{
                    //    if (dtMasterContacts.Rows[i]["TR_Agent_Name"].ToString().Length > 0 && dtMasterContacts.Rows[i]["WR_Agent_Name"].ToString().Length > 0)
                    //        sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Navy'>TR: " + GM.ProperCase(dtMasterContacts.Rows[i]["TR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[i]["TR_UPDATED_DATE"] + " |  WR: " + GM.ProperCase(dtMasterContacts.Rows[i]["WR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[i]["WR_UPDATED_DATE"] + "</font></div>";
                    //    else if (dtMasterContacts.Rows[i]["TR_Agent_Name"].ToString().Length > 0)
                    //        sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Navy'>TR: " + GM.ProperCase(dtMasterContacts.Rows[i]["TR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[i]["TR_UPDATED_DATE"] + "</font></div>";
                    //    else if (dtMasterContacts.Rows[i]["WR_Agent_Name"].ToString().Length > 0)
                    //        sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Navy'>WR: " + GM.ProperCase(dtMasterContacts.Rows[i]["WR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[i]["WR_UPDATED_DATE"] + "</font></div>";

                    //    if (dtMasterContacts.Rows[i]["Rejection"].ToString().Length > 0 && dtMasterContacts.Rows[i]["Review_Tag"].ToString().Length > 0)
                    //        sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'ForestGreen'>Rejection: " + GM.ProperCase(dtMasterContacts.Rows[i]["Rejection"].ToString()) + "</font> | <font size='7' face ='Microsoft Sans Serif' color = 'Crimson'>Review: " + GM.ProperCase(dtMasterContacts.Rows[i]["Review_Tag"].ToString()) + "</font></div>";
                    //    else if (dtMasterContacts.Rows[i]["Rejection"].ToString().Length > 0)
                    //        sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'ForestGreen'>Rejection: " + GM.ProperCase(dtMasterContacts.Rows[i]["Rejection"].ToString()) + "</font></div>";
                    //    else if (dtMasterContacts.Rows[i]["Review_Tag"].ToString().Length > 0)
                    //        sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Crimson'>Review: " + GM.ProperCase(dtMasterContacts.Rows[i]["Review_Tag"].ToString()) + "</font></div>";

                    //}
                    //-----------------------------------------------------------------------------------------------------
                    #endregion
                    //-----------------------------------------------------------------------------------------------------

                    sHTMLValue += "</div>";
                    grd.Cells[0].Value = sHTMLValue;

                    if (sdgvCompany.VScrollBar.Visible)//Alignment
                        sdgvCompany.PrimaryGrid.Columns[0].Width = sdgvCompany.Width - 18;
                    else
                        sdgvCompany.PrimaryGrid.Columns[0].Width = sdgvCompany.Width;

                    IsSuperGridLoading = false;
                    //FreezeEmptyContact(); //If contact is empty then freeze contacts controls
                }
            }

        }




        //-----------------------------------------------------------------------------------------------------
        private void RefreshContactGrid(int iRowIndex)
        {
            if (!splitContactModernUI.Panel1Collapsed)
            {
                List<string> lstContactStatus = new List<string>();

                if (GV.sAccessTo == "TR")
                    lstContactStatus = GV.lstTRContactStatusToBeValidated;
                else
                    lstContactStatus = GV.lstWRContactStatusToBeValidated;

                foreach (GridRow grd in sdgvContacts.PrimaryGrid.Rows)
                {
                    if (grd.Cells[1].Value.ToString() == iRowIndex.ToString())
                    {
                        //grd.Cells[0].Value = "<div><b> as</b>df</div>";

                        string sTitle = string.Empty;
                        if ((GV.sUserType != "Agent") || GV.sShowDetailedContact == "Y")
                            sTitle = iRowIndex + 1 + " <font color = 'Gray'>(" + dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"].ToString() + ")</font>. " + dtMasterContacts.Rows[iRowIndex]["TITLE"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);
                        else
                            sTitle = iRowIndex + 1 + ". " + dtMasterContacts.Rows[iRowIndex]["TITLE"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);



                        //string sTitle = iRowIndex + 1 +". "+ dtMasterContacts.Rows[iRowIndex]["TITLE"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);
                        string sJobTitle = dtMasterContacts.Rows[iRowIndex]["JOB_TITLE"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);
                        if (dtMasterContacts.Rows[iRowIndex]["OTHERS_JOBTITLE"].ToString().Trim().Length > 0)
                            sJobTitle = sJobTitle + " / " + dtMasterContacts.Rows[iRowIndex]["OTHERS_JOBTITLE"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);
                        string sFirstName = dtMasterContacts.Rows[iRowIndex]["FIRST_NAME"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);
                        string sLastName = dtMasterContacts.Rows[iRowIndex]["LAST_NAME"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);
                        string sContactEmail = dtMasterContacts.Rows[iRowIndex]["CONTACT_EMAIL"].ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace("&", string.Empty);
                        string sContactStatus = dtMasterContacts.Rows[iRowIndex][GV.sAccessTo + "_CONTACT_STATUS"].ToString();

                        string sPhonetics = string.Empty;

                        if (GV.NameSayer)
                        {
                            string sFirstName_Phonetics = NameSayer_Phonetics(sFirstName);
                            string sLast_Phonetics = NameSayer_Phonetics(sLastName);
                            if (sFirstName_Phonetics.Length > 0 && sLast_Phonetics.Length > 0)                            
                                sPhonetics = "<font color = 'Gray' Size = '-2'>" + sFirstName_Phonetics + " " + sLast_Phonetics + "</font>";                            
                        }

                        string sHTMLValue = string.Empty;
                        //if (dgvr.Cells["TITLE"].Value.ToString().Length > 0)
                        sHTMLValue += "<div align='left' width ='0'><div align='left'><font size = '11'>" + sTitle + " " + sFirstName + " " + sLastName + " " + sPhonetics + "</font></div><br/>";
                        //else
                        //    sHTMLValue += "<div align='left' width ='0'><div align='left'><font size = '11'>" + sFirstName + " " + sLastName + "</font></div><br/>";
                        if (dtMasterContacts.Rows[iRowIndex]["JOB_TITLE"].ToString().Length > 0)
                            sHTMLValue += "<div align='Left'><font color = 'Gray' size = '10'>" + sJobTitle + "</font></div>";
                        sHTMLValue += "<div align='left'><font color = 'Gray'>" + sContactEmail + "</font></div>";

                        if (GV.lstContactStatusToBeFreezed.Contains(sContactStatus, StringComparer.OrdinalIgnoreCase) || lstContactStatus.Contains(sContactStatus, StringComparer.OrdinalIgnoreCase))
                        {
                            if (dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"].ToString().Length > 0)// && (lstRecordsToUnlock.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"])) || lstQCOKIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[i]["CONTACT_ID_P"]))))
                            {
                                if (lstBouncedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])) && lstSendBackRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                                    sHTMLValue += "<div align = 'right'><font color = 'Red'>Bounced | SendBack</font> | <font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                                else if (lstBouncedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                                    sHTMLValue += "<div align = 'right'><font color = 'Red'>Bounced</font> | <font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                                else if (lstSendBackRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                                    sHTMLValue += "<div align = 'right'><font color = 'Red'>QC:SendBack</font> | <font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                                else if (lstQCOKIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                                    sHTMLValue += "<div align = 'right'><font color = 'Tomato'>QC:OK</font> | <font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                                else if (lstRejectedRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                                    sHTMLValue += "<div align = 'right'><font color = 'Red'>QC:Rejected</font> | <font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                                else
                                    sHTMLValue += "<div align = 'right'><font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                            }
                            else
                                sHTMLValue += "<div align = 'right'><font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";

                            //if (dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"].ToString().Length > 0 && lstRecordsToUnlock.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                            //{
                            //    if (lstBouncedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])) && lstSendBackRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                            //        sHTMLValue += "<div align = 'right'><font color = 'Red'>Bounced | SendBack</font> | <font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                            //    else if (lstBouncedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                            //        sHTMLValue += "<div align = 'right'><font color = 'Red'>Bounced</font> | <font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                            //    else if (lstSendBackRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                            //        sHTMLValue += "<div align = 'right'><font color = 'Red'>SendBack</font> | <font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                            //}
                            //else
                            //    sHTMLValue += "<div align = 'right'><font color = 'Green'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                        }
                        else if (dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"].ToString().Length > 0 && lstRecordsToUnlock.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                        {
                            if (lstBouncedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])) && lstSendBackRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                                sHTMLValue += "<div align = 'right'><font color = 'Red'>Bounced | SendBack</font> | <font color = 'Gray'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                            else if (lstBouncedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                                sHTMLValue += "<div align = 'right'><font color = 'Red'>Bounced</font> | <font color = 'Gray'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                            else if (lstSendBackRecords.Contains(Convert.ToInt32(dtMasterContacts.Rows[iRowIndex]["CONTACT_ID_P"])))
                                sHTMLValue += "<div align = 'right'><font color = 'Red'>SendBack</font> | <font color = 'Gray'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";
                        }
                        else
                            sHTMLValue += "<div align = 'right'><font color = 'Gray'>" + GM.ProperCase_ProjectSpecific(sContactStatus) + "</font></div>";


                        if ((GV.sUserType != "Agent") || GV.sShowDetailedContact == "Y")
                        {
                            if (dtMasterContacts.Rows[iRowIndex]["TR_Agent_Name"].ToString().Length > 0 && dtMasterContacts.Rows[iRowIndex]["WR_Agent_Name"].ToString().Length > 0)
                                sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Navy'>TR: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[iRowIndex]["TR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[iRowIndex]["TR_UPDATED_DATE"] + " |  WR: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[iRowIndex]["WR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[iRowIndex]["WR_UPDATED_DATE"] + "</font></div>";
                            else if (dtMasterContacts.Rows[iRowIndex]["TR_Agent_Name"].ToString().Length > 0)
                                sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Navy'>TR: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[iRowIndex]["TR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[iRowIndex]["TR_UPDATED_DATE"] + "</font></div>";
                            else if (dtMasterContacts.Rows[iRowIndex]["WR_Agent_Name"].ToString().Length > 0)
                                sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Navy'>WR: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[iRowIndex]["WR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[iRowIndex]["WR_UPDATED_DATE"] + "</font></div>";

                            //if (dtMasterContacts.Rows[iRowIndex]["Rejection"].ToString().Length > 0 && dtMasterContacts.Rows[iRowIndex]["Review_Tag"].ToString().Length > 0)
                            //    sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'ForestGreen'>Rejection: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[iRowIndex]["Rejection"].ToString()) + "</font> | <font size='7' face ='Microsoft Sans Serif' color = 'Crimson'>Review: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[iRowIndex]["Review_Tag"].ToString()) + "</font></div>";
                            //else if (dtMasterContacts.Rows[iRowIndex]["Rejection"].ToString().Length > 0)
                            //    sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'ForestGreen'>Rejection: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[iRowIndex]["Rejection"].ToString()) + "</font></div>";
                            if (dtMasterContacts.Rows[iRowIndex]["Review_Tag"].ToString().Length > 0)
                                sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Crimson'>Review: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[iRowIndex]["Review_Tag"].ToString()) + "</font></div>";

                        }

                        sHTMLValue += "</div>";


                        grd.Cells[0].Value = sHTMLValue;

                        if (GV.lstSortableContactColumn != null)//Populate value For sorting
                        {
                            foreach (string sColumn in GV.lstSortableContactColumn)
                            {
                                ((GridRow)sdgvContacts.PrimaryGrid.Rows[grd.RowIndex]).Cells[sColumn].Value = dtMasterContacts.Rows[iRowIndex][sColumn].ToString();
                            }
                        }
                        break;//if row found then update and exit
                    }
                }

                AlignContactGridColumn();
            }
            //((GridRow)sdgvContacts.PrimaryGrid.Rows[0]).Cells[0].Value = "sdfsad";

        }

        //-----------------------------------------------------------------------------------------------------
        private void splitContactModernUI_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!splitContactModernUI.Panel1Collapsed)
            {
                AlignContactGridColumn();

                expandablePanelContactSearch.Width = splitContactModernUI.Panel1.Width;

                // ToastNotification.Show(this, splitContactModernUI.Panel1.Width.ToString(), eToastPosition.TopRight);
            }


        }

        //-----------------------------------------------------------------------------------------------------
        private void sdgvContacts_ColumnHeaderClick(object sender, GridColumnHeaderClickEventArgs e)
        {
            //expandablePanelContactSearch.Expanded = (!expandablePanelContactSearch.Expanded);
            mnuSdgvSort.Show(Cursor.Position.X, Cursor.Position.Y);
        }



        //-----------------------------------------------------------------------------------------------------
        private void sortToolStripMenuItem_Click(object sender, EventArgs e)//Run-time event for sorting super grid
        {
            ToolStripMenuItem tstrip = sender as ToolStripMenuItem;

            //foreach (ToolStripMenuItem t in mnuSdgvSort.Items)//Remove all Images
            foreach (ToolStripMenuItem t in ToolStripSort.DropDownItems)//Remove all Images
            {
                if (t.Name != tstrip.Name)
                {
                    t.Image = null;
                    t.Tag = null;
                }
            }

            if (tstrip.Name == "NoSort")
            {
                sdgvContacts.PrimaryGrid.Columns[0].HeaderText = string.Empty;
                sdgvContacts.PrimaryGrid.SetSort(sdgvContacts.PrimaryGrid.Columns["columnUniqueNumber"], SortDirection.Ascending);
                tstrip.Image = null;
                tstrip.Tag = string.Empty;
            }
            else
            {
                if (tstrip.Tag == null || tstrip.Tag.ToString() == string.Empty || tstrip.Tag.ToString() == "DESC")
                {
                    sdgvContacts.PrimaryGrid.SetSort(sdgvContacts.PrimaryGrid.Columns[tstrip.Name], SortDirection.Ascending);
                    sdgvContacts.PrimaryGrid.Columns[0].HeaderText = "<div align = 'Right'>Sort by <i>" + GM.ProperCase_ProjectSpecific(tstrip.Name.Replace("_", " ")) + "</i> - A to Z ↑</div>" + sFilterHeader;
                    //sFilterHeader = "<div align = 'Right'>Sort by <i>" + GM.ProperCase(tstrip.Name.Replace("_", " ")) + "</i> - A to Z ↑</div>";
                    //sdgvContacts.PrimaryGrid.Columns[0].HeaderText = "<div style='text-align:center;'><div style='border:1px solid #000; display:inline-block;align = Left'>Div 1</div><div style='border:1px solid red; display:inline-block;align = Right'>Sort by <i>" + GM.ProperCase(tstrip.Name.Replace("_", " ")) + "</i> - Z to A ↓</div></div>";
                    tstrip.Image = GCC.Properties.Resources.sort_ascending_icon;
                    tstrip.Tag = "ASC";
                }
                else if (tstrip.Tag.ToString() == "ASC")
                {
                    sdgvContacts.PrimaryGrid.SetSort(sdgvContacts.PrimaryGrid.Columns[tstrip.Name], SortDirection.Descending);
                    sdgvContacts.PrimaryGrid.Columns[0].HeaderText = "<div align = 'Right'>Sort by <i>" + GM.ProperCase_ProjectSpecific(tstrip.Name.Replace("_", " ")) + "</i> - Z to A ↓</div>" + sFilterHeader;
                    //sFilterHeader = "<div align = 'Right'>Sort by <i>" + GM.ProperCase(tstrip.Name.Replace("_", " ")) + "</i> - Z to A ↓</div>";
                    tstrip.Image = GCC.Properties.Resources.sort_descending_icon;
                    tstrip.Tag = "DESC";
                }
            }

            //tstrip.Image = null;
        }

        //-----------------------------------------------------------------------------------------------------
        private void SplitMain_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!splitContactModernUI.Panel1Collapsed)
            {
                AlignContactGridColumn();

                if (sdgvCompany.VScrollBar.Visible)//Alignment
                    sdgvCompany.PrimaryGrid.Columns[0].Width = sdgvCompany.Width - 18;
                else
                    sdgvCompany.PrimaryGrid.Columns[0].Width = sdgvCompany.Width;

                //if (expandablePanelMessage.Expanded)
                //    expandablePanelMessage.Width = SplitMain.Panel1.Width;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void FrmContactsUpdate_Activated(object sender, EventArgs e)
        {
            if (!splitContactModernUI.Panel1Collapsed)
            {
                AlignContactGridColumn();

                if (sdgvCompany.VScrollBar.Visible)//Alignment
                    sdgvCompany.PrimaryGrid.Columns[0].Width = sdgvCompany.Width - 18;
                else
                    sdgvCompany.PrimaryGrid.Columns[0].Width = sdgvCompany.Width;
            }
        }

        //private void mnuBulkEmailUpdate_Click(object sender, EventArgs e, DevComponents.DotNetBar.Controls.TextBoxX txt)
        //-----------------------------------------------------------------------------------------------------
        private void mnuBulkEmailUpdate_Click(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;//Gets which Textbox button triggers the event
            List<string> lstDomain = GetDomainList();
            List<string> lstContactNameSplit = GetContactNameSplit();
            string sEmail = string.Empty;
            string sEmailNamePart = string.Empty;
            string sEmailDomainPart = string.Empty;
            string sEmailSyntax = string.Empty;
            if (txt.Text.Length > 0)
            {
                sEmail = txt.Text.Trim();
                sEmailNamePart = sEmail.Split('@').ToList()[0];
                sEmailDomainPart = sEmail.Split('@').ToList()[1];
            }
            //FirstName                                  LastName
            if ((lstContactNameSplit[0].Trim().Length > 0 || lstContactNameSplit[2].Trim().Length > 0) && lstDomain.Count > 0)
            {
                //DataTable dtEmailSuggestion = dtPicklist.Select("PicklistCategory = 'EmailSuggestion'", "Remarks ASC").CopyToDataTable();
                foreach (DataRow dr in dtEmailSuggestion.Rows)
                {
                    string sEmailFormation = string.Empty;
                    sEmailFormation = dr["PicklistValue"].ToString().Replace("FirstName", lstContactNameSplit[0]);
                    sEmailFormation = sEmailFormation.Replace("LastName", lstContactNameSplit[2]);
                    sEmailFormation = sEmailFormation.Replace("FName", lstContactNameSplit[3]);
                    sEmailFormation = sEmailFormation.Replace("LName", lstContactNameSplit[5]);
                    sEmailFormation = sEmailFormation.Replace("MiddleName", lstContactNameSplit[1]);
                    sEmailFormation = sEmailFormation.Replace("MName", lstContactNameSplit[4]);
                    if (sEmailFormation.Trim().ToLower() == sEmailNamePart.ToLower())//Detect email format
                    {
                        sEmailSyntax = dr["PicklistValue"].ToString();
                        break;
                    }
                }

                frmBulkEmailUpdate objBulkEmailUpdate = new frmBulkEmailUpdate();
                objBulkEmailUpdate.lstContactNameSplit = lstContactNameSplit;
                objBulkEmailUpdate.lstDomain = lstDomain;
                objBulkEmailUpdate.sEmail = sEmail;
                objBulkEmailUpdate.sEmailSyntax = sEmailSyntax;
                objBulkEmailUpdate.dtEmailSugg = dtEmailSuggestion;
                objBulkEmailUpdate.dtContact = dtMasterContacts;
                objBulkEmailUpdate.lstFreezedContacts = lstFreezedContactIDs;
                objBulkEmailUpdate.ShowDialog(this);
                if (objBulkEmailUpdate.IsEmailUpdated)
                {
                    dtMasterContacts = objBulkEmailUpdate.dtContact;
                    txt.Text = dtMasterContacts.Rows[iContactRowIndex]["CONTACT_EMAIL"].ToString();
                    LoadSuperGridContact();
                }
            }
            else
                ToastNotification.Show(this, "No Name or Domain(s) found");
        }

        void worker_DoWork(object sender, DoWorkEventArgs e, DevComponents.DotNetBar.Controls.TextBoxX txt)
        {
            string sPath = AppDomain.CurrentDomain.BaseDirectory + "\\Campaign Manager\\Email";
            if (!Directory.Exists(sPath))
                Directory.CreateDirectory(sPath);

            DirectoryInfo Ddirectory = new DirectoryInfo(sPath);
            foreach (FileInfo fFile in Ddirectory.GetFiles()) fFile.Delete();

            ////if (!(File.Exists(sPath + "\\" + sFileName)))
            ////    File.WriteAllBytes(sPath + "\\" + sFileName, ByteFile);

            lblEmailStatus.Text = "Verifying email <b>" + sCheckingEmail + "</b>";
            lblEmailStatus.Refresh();
            itemContainerEmailVerifyResilt.Refresh();

            sPath += "\\EmailOut.txt";
            ProcessStartInfo pInfo = new ProcessStartInfo(GV.sEmailCheckBinaryPath, "\"" + sCheckingEmail + "|" + sPath + "\"");
            pInfo.WindowStyle = ProcessWindowStyle.Hidden;
            circularProgressEmail.Start();
            Process p = Process.Start(pInfo);
            p.WaitForExit();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e, DevComponents.DotNetBar.Controls.TextBoxX txt)
        {

            string sPath = AppDomain.CurrentDomain.BaseDirectory + "\\Campaign Manager\\Email\\EmailOut.txt";
            circularProgressEmail.Stop();
            ToastNotification.Show(this, System.IO.File.ReadAllText(sPath), eToastPosition.TopRight);
            string sStatus = File.ReadAllText(sPath);
            string sEmail = string.Empty;
            if (sStatus.Contains("Invalid mail domain"))
            {
                sEmail += sCheckingEmail.Split('@')[0] + "@";
                sEmail += "<font color = 'red'>" + sCheckingEmail.Split('@')[1] + "</font>";
            }
            else if (sStatus.Contains("E-mail address is valid"))
            {
                sEmail = "<font color = 'green'>" + sCheckingEmail + "</font>";
            }
            else if (sStatus.Contains("E-mail address does not exist on this server"))
            {
                sEmail += "<font color = 'red'>" + sCheckingEmail.Split('@')[0] + "</font>";
                sEmail += "<font color = 'green'>@" + sCheckingEmail.Split('@')[1] + "</font>";
            }
            else
            {
                sEmail = "<font color = 'orange'>" + sCheckingEmail + "</font>";
            }

            lblEmailStatus.Text = "Email: <b>" + sEmail + "</b><br />" + File.ReadAllText(sPath);
            lblEmailStatus.Refresh();
            rbnBarEmailCheck.Refresh();
        }

        private void btnQC_Click(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;

            frmComboList objFrmComboList = new frmComboList(); //Custom Designed Combobox replacement
            objFrmComboList.IsMultiSelect = true;
            objFrmComboList.lstColumnsToDisplay.Add("Data");
            objFrmComboList.sColumnToSearch = "Data";

            switch (txt.Name)
            {
                case "txtErrorIn":
                    objFrmComboList.dtItems = dtFieldMaster_Active; //.DefaultView.ToTable(true, "TABLE_NAME");
                    objFrmComboList.TitleText = "Select Error Field";
                    objFrmComboList.lstColumnsToDisplay.Clear();
                    objFrmComboList.lstColumnsToDisplay.Add("FIELD_NAME_TABLE");
                    objFrmComboList.sColumnToSearch = "FIELD_NAME_TABLE";
                    break;
                case "txtErrorReason":
                    objFrmComboList.dtItems = dtQCPicklist.Select("FIELD = 'ErrorReason'").CopyToDataTable();
                    objFrmComboList.TitleText = "Select Error reason";
                    break;
                case "txtCallHandlingRemarks":
                    objFrmComboList.dtItems = dtQCPicklist.Select("FIELD = 'CallHandlingRemarks'").CopyToDataTable();
                    objFrmComboList.TitleText = "Select Call Handling remarks";
                    break;
                case "txtCallGrade":
                    objFrmComboList.dtItems = dtQCPicklist.Select("FIELD = 'CallGrade'").CopyToDataTable();
                    objFrmComboList.TitleText = "Select Call Grade";
                    objFrmComboList.IsMultiSelect = false;
                    break;
            }


            objFrmComboList.IsSpellCheckEnabeld = false;

            objFrmComboList.IsSingleWordSelection = false;
            objFrmComboList.ShowDialog(this);
            if (objFrmComboList.sReturn != null && objFrmComboList.sReturn.Length > 0)
                txt.Text = objFrmComboList.sReturn.Replace("|", ", ");
        }



        //-----------------------------------------------------------------------------------------------------
        private void ToolStripEmailUpdate_Click(object sender, EventArgs e)
        {
            //DevComponents.DotNetBar.Controls.TextBoxX txt = new DevComponents.DotNetBar.Controls.TextBoxX();
            //mnuBulkEmailUpdate_Click(sender, e, txt);
            //ToolStripMenuItem tstrip = new ToolStripMenuItem();
            //tstrip.Name = "NoSort";
            //sortToolStripMenuItem_Click(tstrip, e);
        }

        //-----------------------------------------------------------------------------------------------------
        public static List<Control> GetInvisibleControls(Control Container)//Loads all the controls from  container or sub container
        {
            var controlList = new List<Control>();
            try
            {
                foreach (Control childControl in Container.Controls)
                {
                    // Recurse child controls.
                    controlList.AddRange(GetAllControls(childControl));
                    if (childControl.Visible == false)
                        controlList.Add(childControl);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return controlList;
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnNextContact_Click(object sender, EventArgs e)
        {
            List<Control> lstAllControlll = GetInvisibleControls(this);
            List<string> lstControlname = new List<string>();
            foreach (Control C in lstAllControlll)
            {
                lstControlname.Add(C.Name);
                C.Dispose();
            }
            //MessageBox.Show("aa");
        }

        //-----------------------------------------------------------------------------------------------------
        private void expandablePanelMessage_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                expandablePanelMessage.Width = SplitMain.Panel1.Width;
            }
        }

        void AlignContactGridColumn()
        {
            if (sdgvContacts.VScrollBar.Visible)
            {
                //if (sdgvContacts.PrimaryGrid.Columns[0].Width != (sdgvContacts.Width - 18))
                sdgvContacts.PrimaryGrid.Columns[0].Width = expandablePanelContactSearch.Width = sdgvContacts.Width - 18;
            }
            else
            {
                //if (sdgvContacts.PrimaryGrid.Columns[0].Width != sdgvContacts.Width)
                sdgvContacts.PrimaryGrid.Columns[0].Width = expandablePanelContactSearch.Width = sdgvContacts.Width;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void sdgvContacts_RowActivated(object sender, GridRowActivatedEventArgs e)
        {
            if (!IsSuperGridLoading)
            {

                AlignContactGridColumn();

                if (expandablePanelContactSearch.Expanded)
                    expandablePanelContactSearch.Expanded = false;

                GridCell grdCell = sdgvContacts.GetCell(e.NewActiveRow.RowIndex, 1);
                int iRowIndex = (int)grdCell.Value;

                //dgvContacts.CurrentCell = dgvContacts.Rows[iRowIndex].Cells[GlobalVariables.sAccessTo + "_CONTACT_STATUS"]; //TR_Contact status or WR_Contactstatus are selected because they are the standred static fields(other fileds can change in names)
                //dgvContacts.Rows[iRowIndex].Selected = true;
                //dgvContacts_CellClick(dgvContacts, new DataGridViewCellEventArgs(1, iRowIndex));
                Impact_ContactTable(iRowIndex);
            }
        }


        object EAF_Call(string sFunName, string sFieldName = "")
        {
            //if (GV.sEAFLibararyPath.Trim().Length > 0)
            if (EAF != null)
            {
                List<DataTable> lstTables = CollectData(sFieldName);
                var EAF_Return = EAF_MethodInstance.InvokeMember(sFunName, BindingFlags.Default | BindingFlags.InvokeMethod, null, EAF_ClassInstance, new Object[] { lstTables });
                return EAF_Return;
            }
            else
            {
                DataTable dtMessage = new DataTable("Message");
                dtMessage.Columns.Add("Type");
                dtMessage.Columns.Add("Message");
                return dtMessage;
            }
        }


        //-----------------------------------------------------------------------------------------------------
        private void sdgvContacts_CellClick(object sender, GridCellClickEventArgs e)
        {
            //if (!IsSuperGridLoading)
            //{
            //    GridCell grdCell = sdgvContacts.GetCell(e.GridCell.RowIndex, 1);
            //    int iRowIndex = (int)grdCell.Value;

            //    //dgvContacts.CurrentCell = dgvContacts.Rows[iRowIndex].Cells[GlobalVariables.sAccessTo + "_CONTACT_STATUS"]; //TR_Contact status or WR_Contactstatus are selected because they are the standred static fields(other fileds can change in names)
            //    //dgvContacts.Rows[iRowIndex].Selected = true;
            //    //dgvContacts_CellClick(dgvContacts, new DataGridViewCellEventArgs(1, iRowIndex));
            //    Impact_ContactTable(iRowIndex);
            //}


            foreach (TextBox C in lstContactControls)
            {
                C.Focus();
                break;

            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void sdgvContacts_Click(object sender, EventArgs e)
        {

            if (GV.sUserType == "QC")
            {
                if (tabControlContact.Tabs.Contains(tabContacts))
                {
                    tabControlContact.Tabs.Remove(tabContacts);
                    tabControlCompany.Tabs.Add(tabContacts);
                    tabControlContact.Controls.Remove(sTabPanelContacts);
                    tabControlCompany.Controls.Add(sTabPanelContacts);

                }

                tabControlCompany.SelectedPanel = sTabPanelContacts;
                tabControlContact.SelectedPanel = sTabPanelQC;
            }
            else
                tabControlContact.SelectedPanel = sTabPanelContacts;

            
        }


        List<DataTable> CollectData(string sFieldName = "")
        {
            //DataSet dsTables = new DataSet();
            List<DataTable> dsTables = new List<DataTable>();

            dsTables.Add(dtMasterCompanies);
            dsTables.Add(dtMasterContacts);
            dsTables.Add(dtMasterCompaniesCopy);
            dsTables.Add(dtMasterContactsCopy);
            dsTables.Add(dtFieldMaster_Active);
            dsTables.Add(dtPicklist);
            dsTables.Add(dtValidations);
            dsTables.Add(dtCountryInformation);
            dsTables.Add(dtRecordStatus);

            DataTable dtProjectInfo = new DataTable("ProjectInfo");
            dtProjectInfo.Columns.Add("Key");
            dtProjectInfo.Columns.Add("Value");
            dtProjectInfo.Columns.Add("Type");

            dtProjectInfo.Rows.Add("ProjectName", GV.sProjectName, "String");
            dtProjectInfo.Rows.Add("ProjectID", GV.sProjectID, "String");
            dtProjectInfo.Rows.Add("EmployeeName", GV.sEmployeeName, "String");
            dtProjectInfo.Rows.Add("EmployeeNo", GV.sEmployeeNo, "String");
            dtProjectInfo.Rows.Add("CompanyTable", GV.sCompanyTable, "String");
            dtProjectInfo.Rows.Add("ContactTable", GV.sContactTable, "String");
            dtProjectInfo.Rows.Add("UserType", GV.sUserType, "String");
            dtProjectInfo.Rows.Add("AccessTo", GV.sAccessTo, "String");
            dtProjectInfo.Rows.Add("AllowTelephoneFormating", GV.sAllowTelephoneFormating, "String");
            dtProjectInfo.Rows.Add("AllowGeneralEmail", GV.sAllowGeneralEmail, "String");
            dtProjectInfo.Rows.Add("AllowDuplicateEmail", GV.sAllowDuplicateEmail, "String");
            dtProjectInfo.Rows.Add("AllowDuplicateJobTitle", GV.sAllowDuplicateJobTitle, "String");
            dtProjectInfo.Rows.Add("AllowPublicDomainEmails", GV.sAllowPublicDomainEmails, "String");
            dtProjectInfo.Rows.Add("AllowDuplicateEmailAcrossProject", GV.sAllowDuplicateEmailAcrossProject, "String");
            dtProjectInfo.Rows.Add("AllowNewCompanyTR", GV.sAllowNewCompanyTR, "String");
            dtProjectInfo.Rows.Add("AllowNewCompanyWR", GV.sAllowNewCompanyWR, "String");
            dtProjectInfo.Rows.Add("EmailCheckBinaryPath", GV.sEmailCheckBinaryPath, "String");
            dtProjectInfo.Rows.Add("SendKeyBinaryPath", GV.sSendKeyBinaryPath, "String");
            dtProjectInfo.Rows.Add("SpellCheckJobTitle", GV.sSpellCheckJobTitle, "String");
            dtProjectInfo.Rows.Add("AllowSwitchBoardNumberinContacts", GV.sAllowSwitchBoardNumberinContacts, "String");
            dtProjectInfo.Rows.Add("FreezeWRCompletedRecords", GV.sFreezeWRCompletedRecords, "String");
            dtProjectInfo.Rows.Add("FreezeTRCompletedRecords", GV.sFreezeTRCompletedRecords, "String");
            dtProjectInfo.Rows.Add("FreezeWRCompanyCompletes", GV.sFreezeWRCompanyCompletes, "String");
            dtProjectInfo.Rows.Add("FreezeTRCompanyCompletes", GV.sFreezeTRCompanyCompletes, "String");
            dtProjectInfo.Rows.Add("MaxValidatedContactCount", GV.iMaxValidatedContactCount, "Int");
            dtProjectInfo.Rows.Add("MinValidatedContactCountComplets", GV.iMinValidatedContactCountComplets, "Int");
            dtProjectInfo.Rows.Add("MinValidatedContactCountPartialComplets", GV.iMinValidatedContactCountPartialComplets, "Int");
            dtProjectInfo.Rows.Add("MinValidatedContactCountPartialComplets", GV.iMinValidatedContactCountPartialComplets, "Int");
            dtProjectInfo.Rows.Add("ShowOnGridMasterCompanies", string.Join("~", GV.lstShowOnGridMasterCompanies.ToArray()), "List");
            dtProjectInfo.Rows.Add("ShowOnGridMasterContacts", string.Join("~", GV.lstShowOnGridMasterContacts.ToArray()), "List");
            dtProjectInfo.Rows.Add("ShowOnCriteriaMasterCompanies", string.Join("~", GV.lstShowOnCriteriaMasterCompanies.ToArray()), "List");
            dtProjectInfo.Rows.Add("ShowOnCriteriaMasterContacts", string.Join("~", GV.lstShowOnCriteriaMasterContacts.ToArray()), "List");
            dtProjectInfo.Rows.Add("ContactStatusToBeFreezed", string.Join("~", GV.lstContactStatusToBeFreezed.ToArray()), "List");
            dtProjectInfo.Rows.Add("TR_DisposalsToBeFreezed", string.Join("~", GV.TR_lstDisposalsToBeFreezed.ToArray()), "List");
            dtProjectInfo.Rows.Add("WR_DisposalsToBeFreezed", string.Join("~", GV.WR_lstDisposalsToBeFreezed.ToArray()), "List");
            dtProjectInfo.Rows.Add("TRContactStatusToBeValidated", string.Join("~", GV.lstTRContactStatusToBeValidated.ToArray()), "List");
            dtProjectInfo.Rows.Add("WRContactStatusToBeValidated", string.Join("~", GV.lstWRContactStatusToBeValidated.ToArray()), "List");
            dtProjectInfo.Rows.Add("TR_DeleteStatus", string.Join("~", GV.lstTR_DeleteStatus.ToArray()), "List");
            dtProjectInfo.Rows.Add("WR_DeleteStatus", string.Join("~", GV.lstWR_DeleteStatus.ToArray()), "List");
            dtProjectInfo.Rows.Add("TRDisposalsToBeValidated", string.Join("~", GV.TR_lstDisposalsToBeValidated.ToArray()), "List");
            dtProjectInfo.Rows.Add("WRDisposalsToBeValidated", string.Join("~", GV.WR_lstDisposalsToBeValidated.ToArray()), "List");
            dtProjectInfo.Rows.Add("NewRecordContactStatus", string.Join("~", GV.lstNewRecordContactStatus.ToArray()), "List");
            dtProjectInfo.Rows.Add("UnchangedRecordContactStatus", string.Join("~", GV.lstUnchangedRecordContactStatus.ToArray()), "List");
            dtProjectInfo.Rows.Add("ChangedRecordContactStatus", string.Join("~", GV.lstChangedRecordContactStatus.ToArray()), "List");
            dtProjectInfo.Rows.Add("ReplacementRecordContactStatus", string.Join("~", GV.lstReplacementRecordContactStatus.ToArray()), "List");
            dtProjectInfo.Rows.Add("ReplacementOptionRecordContactStatus", string.Join("~", GV.lstReplacementOptionRecordContactStatus.ToArray()), "List");
            dtProjectInfo.Rows.Add("NeutralContactStatus", string.Join("~", GV.lstNeutralContactStatus.ToArray()), "List");
            dtProjectInfo.Rows.Add("EmailCheckContactStatus", GV.sEmailCheckContactStatus, "String");
            dtProjectInfo.Rows.Add("ReplacementOptionContactStatus", GV.sReplacementOptionContactStatus, "String");
            dtProjectInfo.Rows.Add("TRContactstatusTobeValidated", GV.sTRContactstatusTobeValidated, "String");
            dtProjectInfo.Rows.Add("WRContactstatusTobeValidated", GV.sWRContactstatusTobeValidated, "String");
            dtProjectInfo.Rows.Add("SortableContactColumn", string.Join("~", GV.lstSortableContactColumn.ToArray()), "List");
            dtProjectInfo.Rows.Add("MSSQLConString", GV.sMSSQL, "String");
            dtProjectInfo.Rows.Add("MYSQLConString", GV.sMySQL, "String");
            //dtProjectInfo.Rows.Add("ContactRowIndex", iContactRowIndex, "Int");
            dtProjectInfo.Rows.Add("FieldName", sFieldName, "String");
            dtProjectInfo.Rows.Add("FreezedContactIDs", string.Join("~", lstFreezedContactIDs.ToArray()), "List");
            dtProjectInfo.Rows.Add("BouncedContactIDs", string.Join("~", lstBouncedContactIDs.ToArray()), "List");
            dtProjectInfo.Rows.Add("RecordsToUnlock", string.Join("~", lstRecordsToUnlock.ToArray()), "List");
            dtProjectInfo.Rows.Add("ContactRowIndex", iContactRowIndex, "Int");
            dtProjectInfo.Rows.Add("CompanyRowIndex", iCompanyRowIndex, "Int");


            dsTables.Add(dtProjectInfo);

            return dsTables;

        }

        //-----------------------------------------------------------------------------------------------------
        private void webBrowserMessage_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string sTarget = e.Url.ToString();
            if (sTarget != "about:blank")
            {
                e.Cancel = true;

                string sTable = string.Empty;
                int iRow;
                string sControlName = string.Empty;
                sTarget = sTarget.Replace("about:", string.Empty);

                List<string> lstContent = sTarget.Split('|').ToList();

                if (lstContent.Count == 3)
                {
                    sTable = lstContent[0];
                    iRow = Convert.ToInt32(lstContent[1]);
                    sControlName = lstContent[2].ToUpper().Replace("*", "_");

                    if (iRow == 0 && sTable == "Contact")
                    {
                        if (lstContactControls != null)
                        {
                            foreach (TextBox C in lstContactControls)
                            {
                                if (C.Name.ToUpper() == sControlName)
                                {
                                    C.Focus();
                                    break;
                                }
                            }
                        }

                        if (tabControlContact.SelectedPanel != sTabPanelContacts)//Select Main Tab if not already selected
                            tabControlContact.SelectedPanel = sTabPanelContacts;
                    }
                    else if (sTable == "Contact")
                    {
                        sdgvContacts.PrimaryGrid.ClearSelectedRows();
                        GridRow gr = sdgvContacts.PrimaryGrid.Rows[iRow - 1] as GridRow;
                        sdgvContacts.PrimaryGrid.SetActiveRow(gr);
                        sdgvContacts.PrimaryGrid.Select(gr);
                        gr.IsSelected = true;
                        gr.SetActive(true);
                        sdgvContacts.Refresh();
                        sdgvContacts.PrimaryGrid.SetSelectedRows(iRow - 1, 1, true);
                        sdgvContacts.Refresh();

                        if (lstContactControls != null)
                        {
                            foreach (TextBox C in lstContactControls)
                            {
                                if (C.Name.ToUpper() == sControlName)
                                {
                                    C.Focus();
                                    break;
                                }
                            }
                        }

                        if (tabControlContact.SelectedPanel != sTabPanelContacts)//Select Main Tab if not already selected
                            tabControlContact.SelectedPanel = sTabPanelContacts;

                        if (sdgvContacts.VScrollBarVisible)
                            sdgvContacts.VScrollOffset = Convert.ToInt32((((iRow / Convert.ToDouble(sdgvContacts.PrimaryGrid.Rows.Count)) * 100) * sdgvContacts.VScrollMaximum) / 100) - 200;
                    }
                    else if (sTable == "Company")
                    {

                    }
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void AlignFilterControlsControls(DataTable dtControl, Panel Container)
        {
            try
            {
                //Align Controls
                Container.Controls.Clear();
                int X = 10, Y = 10;
                int iContainerWidth = Container.Width;

                for (int i = 0; i < dtControl.Rows.Count; i++)
                {
                    LabelX lbl = new LabelX();
                    lbl.Text = dtControl.Rows[i]["LabelName"].ToString();
                    lbl.AutoSize = true;
                    DevComponents.DotNetBar.Controls.TextBoxX txt = new DevComponents.DotNetBar.Controls.TextBoxX();


                    txt.Width = Container.Width - 30;
                    //txt.Width = 470;

                    txt.ButtonCustom.Visible = true;
                    txt.ButtonCustomClick += new EventHandler(btn_OpenSelectables);
                    txt.ButtonCustom2.Visible = true;
                    txt.ButtonCustom2.Image = Properties.Resources.Button_Delete_icon2;
                    txt.ButtonCustom2Click += new EventHandler(btn_RemoveFilter);
                    txt.Name = "Filter_" + dtControl.Rows[i]["FieldName"].ToString();
                    //txt.Tag = dtControl.Rows[i]["FieldName"].ToString();
                    txt.Font = new Font(txt.Font.FontFamily, 10);
                    txt.Border.Class = "TextBoxBorder";
                    txt.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    txt.Text = dtControl.Rows[i]["Value"].ToString();
                    lbl.Location = new Point(X, Y); //Label
                    Y += 20;
                    txt.Location = new Point(X, Y); //Text Box
                    //dtControl[i].Width = iContainerWidth - iDisplayWidth - 50;//Fixed Control widt

                    //txt.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                    Container.Controls.Add(lbl);
                    Container.Controls.Add(txt);
                    Y = Y + txt.Height + 10;// Distance between controls(Vertical Height)

                    //Container.Dock = DockStyle.None;
                    //Container.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                    if (expandablePanelContactSearch.Height + 200 < splitContactModernUI.Panel1.Height)
                    {
                        //if (Container.Controls.Count > 2)
                        //    Container.Height += (txt.Height + lbl.Height + 17);
                        //else
                        Container.Height += (txt.Height + lbl.Height) + (30 - (i * 4));
                        expandablePanelContactSearch.Height = (panelExCustomFilter.Height + panelExDefaultFilter.Height + expandablePanelContactSearch.TitlePanel.Height);
                    }


                    //Container.Refresh();
                    //expandablePanelContactSearch.Refresh();

                    //if (i == dtControl.Rows.Count - 1)//Add extra control at end to get some extra space
                    //{
                    //    Label lblTemp = new Label();
                    //    lblTemp.Location = new Point(5, Y);
                    //    //lblTemp.Text = "Temp";
                    //    lblTemp.BackColor = Color.Transparent;
                    //    Container.Controls.Add(lblTemp);
                    //}
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void InitializeFilter()
        {
            //dtFilter = new DataTable();
            dtFilter.Rows.Clear();
            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add("FilterType");
                dtFilter.Columns.Add("FieldName");
                dtFilter.Columns.Add("Value");
                dtFilter.Columns.Add("ValueType");
                dtFilter.Columns.Add("LabelName");
            }

            dtFilter.Rows.Add("Default", "TR_CONTACT_STATUS", "", "String");
            dtFilter.Rows.Add("Default", "WR_CONTACT_STATUS", "", "String");
            dtFilter.Rows.Add("Default", "TR_AGENT_NAME", "", "String");
            dtFilter.Rows.Add("Default", "WR_AGENT_NAME", "", "String");
            dtFilter.Rows.Add("Default", "TR_UPDATED_DATE", "", "Date");
            dtFilter.Rows.Add("Default", "WR_UPDATED_DATE", "", "Date");

        }

        //-----------------------------------------------------------------------------------------------------
        private void btn_OpenSelectables(object sender, EventArgs e)
        {
            TextBox txtSelect = sender as TextBox;
            string sField = txtSelect.Name.Remove(0, 7);
            frmComboList objFrmComboList = new frmComboList(); //Custom Designed Combobox replacement
            objFrmComboList.TitleText = "Select Value";

            objFrmComboList.dtItems = dtMasterContacts.DefaultView.ToTable(true, sField);
            objFrmComboList.lstColumnsToDisplay.Add(sField);
            objFrmComboList.sColumnToSearch = sField;
            objFrmComboList.IsSpellCheckEnabeld = false;
            objFrmComboList.IsMultiSelect = false;
            objFrmComboList.IsSingleWordSelection = false;
            objFrmComboList.ShowDialog(this);
            if (!string.IsNullOrEmpty(objFrmComboList.sReturn))
                txtSelect.Text = objFrmComboList.sReturn;
        }

        //-----------------------------------------------------------------------------------------------------
        private void btn_RemoveFilter(object sender, EventArgs e)//Runtime event.. //Button Click for 'Customlist' controls which opens the Custom designed combobox
        {
            TextBox txtToRemove = sender as TextBox;
            for (int i = 0; i < dtFilter.Rows.Count; i++)
            {
                if ("FILTER_" + dtFilter.Rows[i]["FieldName"].ToString().ToUpper() == txtToRemove.Name.ToUpper())
                {
                    dtFilter.Rows.Remove(dtFilter.Rows[i]);
                    break;
                }
            }

            panelExCustomFilter.Height = 0;
            expandablePanelContactSearch.Height = panelExDefaultFilter.Height + expandablePanelContactSearch.TitleHeight;
            DataRow[] drrFilterControl = dtFilter.Select("FilterType = 'Custom'");

            if (drrFilterControl.Length > 0)
                AlignFilterControlsControls(drrFilterControl.CopyToDataTable(), panelExCustomFilter); //Adds and aligns the controls to its parant(Form or panel)
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnAddFilter_Click(object sender, EventArgs e)
        {
            if (dtFilter.Select("FilterType = 'Custom'").Length < 5)
            {

                frmComboList objFrmComboList = new frmComboList(); //Custom Designed Combobox replacement
                objFrmComboList.TitleText = "Select Field";
                //List<string> lstColumnToSelect = dtFieldMasterContact.AsEnumerable().Select(r => r.Field<string>("FIELD_NAME_TABLE")).ToList();
                DataTable dtColumnToSelect = dtFieldMasterContact.Copy();
                List<DataRow> lstDrToRemove = new List<DataRow>();

                foreach (DataRow drRowsToRemove in dtColumnToSelect.Rows)
                {
                    if (dtFilter.Select("FieldName = '" + drRowsToRemove["FIELD_NAME_TABLE"].ToString() + "'").Length > 0)
                        lstDrToRemove.Add(drRowsToRemove);
                }
                foreach (DataRow drRowsToRemove in lstDrToRemove)
                    dtColumnToSelect.Rows.Remove(drRowsToRemove);

                // dtColumnToSelect.Columns["FIELD_NAME_TABLE"].ColumnName = "PicklistValue";

                objFrmComboList.dtItems = dtColumnToSelect;
                objFrmComboList.lstColumnsToDisplay.Add("FIELD_NAME_CAPTION");
                objFrmComboList.sColumnToSearch = "FIELD_NAME_TABLE";
                objFrmComboList.IsSpellCheckEnabeld = false;
                objFrmComboList.IsMultiSelect = false;
                objFrmComboList.IsSingleWordSelection = false;
                objFrmComboList.ShowDialog(this);

                if (!string.IsNullOrEmpty(objFrmComboList.sReturn))
                {

                    dtFilter.Rows.Add("Custom", objFrmComboList.sReturn, "", "String", dtFieldMasterContact.Select("FIELD_NAME_TABLE = '" + objFrmComboList.sReturn + "'")[0]["FIELD_NAME_CAPTION"].ToString());
                    //txt.Text = objFrmComboList.sReturn;
                    //txt.Text = objFrmComboList.sReturn;//Repeated to avoid triggering textbox change bringing grid value to textbox
                }
                panelExCustomFilter.Height = 0;
                DataRow[] drrFilterControl = dtFilter.Select("FilterType = 'Custom'");
                expandablePanelContactSearch.Height = panelExDefaultFilter.Height + expandablePanelContactSearch.TitleHeight;
                if (drrFilterControl.Length > 0)
                    AlignFilterControlsControls(drrFilterControl.CopyToDataTable(), panelExCustomFilter); //Adds and aligns the controls to its parant(Form or panel)
            }
            else
                ToastNotification.Show(this, "Maximum filters exceeded.", eToastPosition.TopRight);
        }

        //-----------------------------------------------------------------------------------------------------
        private void CollectFilter()
        {
            foreach (DataRow drFilter in dtFilter.Rows)
            {
                //foreach (Control c in panelExCustomFilter.Controls)
                //{
                //    if(c is TextBox && c.)

                //}
                Control[] cControl = expandablePanelContactSearch.Controls.Find("Filter_" + drFilter["FieldName"].ToString(), true);
                if (cControl.Length > 0 && cControl[0].Text.Trim().Length > 0)
                    drFilter["Value"] = cControl[0].Text;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void switchTRWR_ValueChanged(object sender, EventArgs e)
        {
            if (switchTRWR.Value)
            {
                Filter_TR_UPDATED_DATE.Visible = true;
                Filter_TR_AGENT_NAME.Visible = true;
                Filter_TR_CONTACT_STATUS.Visible = true;

                lblFilterTRAgent.Visible = true;
                lblFilterTRDate.Visible = true;
                lblFilterTRStatus.Visible = true;

                Filter_WR_AGENT_NAME.Visible = false;
                Filter_WR_UPDATED_DATE.Visible = false;
                Filter_WR_CONTACT_STATUS.Visible = false;

                lblFilterWRAgent.Visible = false;
                lblFilterWRDate.Visible = false;
                lblFilterWRStatus.Visible = false;
            }
            else
            {
                Filter_TR_UPDATED_DATE.Visible = false;
                Filter_TR_AGENT_NAME.Visible = false;
                Filter_TR_CONTACT_STATUS.Visible = false;

                lblFilterTRAgent.Visible = false;
                lblFilterTRDate.Visible = false;
                lblFilterTRStatus.Visible = false;

                Filter_WR_AGENT_NAME.Visible = true;
                Filter_WR_UPDATED_DATE.Visible = true;
                Filter_WR_CONTACT_STATUS.Visible = true;

                lblFilterWRAgent.Visible = true;
                lblFilterWRDate.Visible = true;
                lblFilterWRStatus.Visible = true;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void expandablePanelContactSearch_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            if (expandablePanelContactSearch.Expanded)
            {
                if (dtMasterCompanies.Rows.Count > 0 || sFormOpenType == "ButtonOpen" || sFormOpenType == "SendBack")
                    btnFilterAllCompany.Enabled = true;
                else
                    btnFilterAllCompany.Enabled = false;

                //expandablePanelContactSearch.Width = splitContactModernUI.Panel1.Width;

                sdgvContacts.ColumnResized -= new EventHandler<GridColumnEventArgs>(sdgvContacts_ColumnResized);
                AlignContactGridColumn();
                sdgvContacts.ColumnResized += new EventHandler<GridColumnEventArgs>(sdgvContacts_ColumnResized);


                expandablePanelContactSearch.Location = sdgvContacts.Location;
                panelExCustomFilter.Height = 0;
                expandablePanelContactSearch.Height = panelExDefaultFilter.Height + expandablePanelContactSearch.TitleHeight;
                DataRow[] drrFilterControl = dtFilter.Select("FilterType = 'Custom'");

                if (drrFilterControl.Length > 0)
                    AlignFilterControlsControls(drrFilterControl.CopyToDataTable(), panelExCustomFilter); //Adds and aligns the controls to its parant(Form or panel)
            }
            else
                expandablePanelContactSearch.Visible = false;
        }

        //-----------------------------------------------------------------------------------------------------
        private void contactFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dtMasterContacts.Rows.Count > 5)
                expandablePanelContactSearch.Expanded = true;
            else
                ToastNotification.Show(this, "Filter not applicable on less contacts.", eToastPosition.TopRight);
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnFilterThisCompany_Click(object sender, EventArgs e)
        {
            btnFilterThisCompany.Checked = true;
            btnFilterAllCompany.Checked = false;
            sDoFilter = sMaster_ID;
            CollectFilter();
            LoadSuperGridContact();
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnFilterAllCompany_Click(object sender, EventArgs e)
        {
            btnFilterAllCompany.Checked = true;
            btnFilterThisCompany.Checked = false;
            sDoFilter = "All";
            CollectFilter();
            LoadSuperGridContact();
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            btnFilterAllCompany.Checked = false;
            btnFilterThisCompany.Checked = false;
            expandablePanelContactSearch.Expanded = false;

            Filter_WR_AGENT_NAME.Text = string.Empty;
            Filter_WR_UPDATED_DATE.Text = string.Empty;
            Filter_WR_CONTACT_STATUS.Text = string.Empty;

            Filter_TR_UPDATED_DATE.Text = string.Empty;
            Filter_TR_AGENT_NAME.Text = string.Empty;
            Filter_TR_CONTACT_STATUS.Text = string.Empty;

            sDoFilter = string.Empty;
            panelExCustomFilter.Controls.Clear();
            InitializeFilter();
            LoadSuperGridContact();
        }

        //-----------------------------------------------------------------------------------------------------
        private void sdgvContacts_ColumnResized(object sender, GridColumnEventArgs e)
        {
            //if (!splitContactModernUI.Panel1Collapsed)
            {
                AlignContactGridColumn();

                //expandablePanelContactSearch.Width = splitContactModernUI.Panel1.Width;

                //ToastNotification.Show(this, splitContactModernUI.Panel1.Width.ToString(), eToastPosition.TopRight);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void sdgvCompany_RowActivated(object sender, GridRowActivatedEventArgs e)
        {
            if (!IsSuperGridLoading)
            {
                if (sdgvCompany.VScrollBar.Visible)
                {
                    if (sdgvCompany.PrimaryGrid.Columns[0].Width != (sdgvCompany.Width - 18))
                        sdgvCompany.PrimaryGrid.Columns[0].Width = sdgvCompany.Width - 18;
                }
                else
                {
                    if (sdgvCompany.PrimaryGrid.Columns[0].Width != sdgvCompany.Width)
                        sdgvCompany.PrimaryGrid.Columns[0].Width = sdgvCompany.Width;
                }

                // if (sdgvCompany.PrimaryGrid.Rows.Count > 0)
                {
                    //int iNewRow = 0;
                    //if (e.NewActiveRow != null)
                    //    iNewRow = e.NewActiveRow.RowIndex;

                    GridCell grdCell = sdgvCompany.GetCell(e.NewActiveRow.RowIndex, 1);
                    int iRowIndex = (int)grdCell.Value;
                    iCompanyRowIndex = iRowIndex;



                    sMaster_ID = dtMasterCompanies.Rows[iCompanyRowIndex]["MASTER_ID"].ToString();

                    //dgvContacts.CurrentCell = dgvContacts.Rows[iRowIndex].Cells[GlobalVariables.sAccessTo + "_CONTACT_STATUS"]; //TR_Contact status or WR_Contactstatus are selected because they are the standred static fields(other fileds can change in names)
                    //dgvContacts.Rows[iRowIndex].Selected = true;
                    //dgvContacts_CellClick(dgvContacts, new DataGridViewCellEventArgs(1, iRowIndex));

                    PopulateMasterCompanyFields();//Populates data to Master Company fields
                    PopulateDisposals(); //Populate data on disposals (Static Fields)
                    LoadSuperGridContact();
                    //Impact_CompanyTable(iCompanyRowIndex);
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------
        //private void TextChanged_StaticControls(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!IsLoading)
        //        {
        //            IsLoading = true;
        //            TableToTextCompany(iCompanyRowIndex, "ControlsToGrid", true);
        //            IsLoading = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
        //        //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    }
        //}

        //-----------------------------------------------------------------------------------------------------
        private void TextChanged_StaticControls(object sender, EventArgs e)
        {
            try
            {
                if (!IsLoading)
                {
                    IsLoading = true;

                    TextBox txt = sender as TextBox;
                    string sTxtName = string.Empty;
                    if (txt == null)
                        sTxtName = "txtNotes";
                    else
                        sTxtName = txt.Name;
                    switch (sTxtName)
                    {
                        case "txtNotes":
                            dtMasterCompanies.Rows[iCompanyRowIndex][GV.sAccessTo + "_AGENTNOTES"] = txtNotes.Text;
                            break;
                        case "TR_COMMENTS":
                            dtMasterCompanies.Rows[iCompanyRowIndex]["TR_COMMENTS"] = txt.Text.Replace("]:[", " ").Replace("]|[", " ");


                            break;
                        case "WR_COMMENTS":

                            dtMasterCompanies.Rows[iCompanyRowIndex]["WR_COMMENTS"] = txt.Text.Replace("]:[", " ").Replace("]|[", " ");
                            //if (txt.Text.Trim().Length > 0)
                            //{
                            //    if (dtMasterCompanies.Rows[iCompanyRowIndex]["WR_COMMENTS"].ToString().Trim().Length == 0)
                            //        dtMasterCompanies.Rows[iCompanyRowIndex]["WR_COMMENTS"] = "[" + GV.sEmployeeName + "]:[" + GM.GetDateTime() + "]:[" + txt.Text.Replace("]:[", " ").Replace("]|[", " ") + "]";
                            //    else
                            //        dtMasterCompanies.Rows[iCompanyRowIndex]["WR_COMMENTS"] = "|[" + GV.sEmployeeName + "]:[" + GM.GetDateTime() + "]:[" + txt.Text.Replace("]:[", " ").Replace("]|[", " ") + "]";
                            //}
                            break;
                    }
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void TR_SECONDARY_DISPOSAL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
            {
                if (TR_SECONDARY_DISPOSAL.SelectedValue != null && TR_SECONDARY_DISPOSAL.SelectedValue.ToString() != "System.Data.DataRowView")
                {
                    //IsLoading = true;
                    dtMasterCompanies.Rows[iCompanyRowIndex]["TR_SECONDARY_DISPOSAL"] = TR_SECONDARY_DISPOSAL.SelectedValue;
                    RefreshCompanyGrid(iCompanyRowIndex);
                    //IsLoading = false;
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void WR_SECONDARY_DISPOSAL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
            {
                if (WR_SECONDARY_DISPOSAL.SelectedValue != null && WR_SECONDARY_DISPOSAL.SelectedValue.ToString() != "System.Data.DataRowView")
                {
                    //IsLoading = true;
                    dtMasterCompanies.Rows[iCompanyRowIndex]["WR_SECONDARY_DISPOSAL"] = WR_SECONDARY_DISPOSAL.SelectedValue;
                    RefreshCompanyGrid(iCompanyRowIndex);
                    //IsLoading = false;
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void tabControlCompany_SelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
        {

            //ToastNotification.Show(this, "Changing");
            //if (e.NewValue.Name == "tabGroupCompany")
            // LoadSuperGridCompany();            

        }

        //-----------------------------------------------------------------------------------------------------
        private void sdgvCompany_ColumnResized(object sender, GridColumnEventArgs e)
        {
            if (sdgvCompany.VScrollBar.Visible)
                sdgvCompany.PrimaryGrid.Columns[0].Width = sdgvCompany.Width - 18;
            else
                sdgvCompany.PrimaryGrid.Columns[0].Width = sdgvCompany.Width;
        }

        void AddValidationResults(string sTableName, int iIndex, int iValidationCompanyID, int iValidationContactID, string sMessage, string sValidationType, string sTargetField, string sTargetValue, string sConditionField, string sConditionValue, bool IsDynamic, int iErrorArea, TextBox txt = null)
        {
            DataTable dtValidateMessages;
            if (IsDynamic)
            {
                dtValidateMessages = dtValidationResultsDynamic;
                txt.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
                txt.Refresh();
            }
            else
            {
                dtValidateMessages = dtValidationResults;
                if (iErrorArea > -1)
                    GV.iNotifier[iErrorArea] = 1;
            }

            DataRow drValidationResults = dtValidateMessages.NewRow();
            drValidationResults["TableName"] = sTableName;
            if (iIndex >= 0)
                drValidationResults["RowIndex"] = iIndex;
            if (iValidationCompanyID > 0)
                drValidationResults["CompanyID"] = iValidationCompanyID;
            if (iValidationContactID > 0)
                drValidationResults["ContactID"] = iValidationContactID;

            drValidationResults["Message"] = sMessage;
            drValidationResults["Validation"] = sValidationType;
            drValidationResults["TargetField"] = sTargetField;
            drValidationResults["TargetValue"] = sTargetValue;
            drValidationResults["ConditionField"] = sConditionField;
            drValidationResults["ConditionValue"] = sConditionValue;
            dtValidateMessages.Rows.Add(drValidationResults);
        }

        void AddValidationResults(string sTableName, int iIndex, int iValidationCompanyID, int iValidationContactID, string sMessage, string sValidationType, string sTargetField, string sTargetValue, bool IsDynamic, int iErrorArea, TextBox txt = null)
        {
            DataTable dtValidateMessages;
            if (IsDynamic)
            {
                dtValidateMessages = dtValidationResultsDynamic;
                txt.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
                txt.Refresh();
            }
            else
            {
                dtValidateMessages = dtValidationResults;
                if (iErrorArea > -1)
                    GV.iNotifier[iErrorArea] = 1;
            }

            DataRow drValidationResults = dtValidateMessages.NewRow();
            drValidationResults["TableName"] = sTableName;
            if (iIndex >= 0)
                drValidationResults["RowIndex"] = iIndex;
            if (iValidationCompanyID > 0)
                drValidationResults["CompanyID"] = iValidationCompanyID;
            if (iValidationContactID > 0)
                drValidationResults["ContactID"] = iValidationContactID;

            drValidationResults["Message"] = sMessage;
            drValidationResults["Validation"] = sValidationType;
            drValidationResults["TargetField"] = sTargetField;
            drValidationResults["TargetValue"] = sTargetValue;
            dtValidateMessages.Rows.Add(drValidationResults);
        }

        void AddValidationResults(string sTableName, int iIndex, int iValidationCompanyID, int iValidationContactID, string sMessage, bool IsDynamic, int iErrorArea, TextBox txt = null)
        {
            DataTable dtValidateMessages;
            if (IsDynamic)
            {
                dtValidateMessages = dtValidationResultsDynamic;
                txt.BackColor = Color.FromArgb(0xFF, 0x99, 0x99);
                txt.Refresh();
            }
            else
            {
                dtValidateMessages = dtValidationResults;
                if (iErrorArea > -1)
                    GV.iNotifier[iErrorArea] = 1;
            }

            DataRow drValidationResults = dtValidateMessages.NewRow();
            drValidationResults["TableName"] = sTableName;
            if (iIndex >= 0)
                drValidationResults["RowIndex"] = iIndex;
            if (iValidationCompanyID > 0)
                drValidationResults["CompanyID"] = iValidationCompanyID;
            if (iValidationContactID > 0)
                drValidationResults["ContactID"] = iValidationContactID;
            drValidationResults["Message"] = sMessage;
            dtValidateMessages.Rows.Add(drValidationResults);
        }

        string BuildError(string sEAFMessage, bool IsDynamic)
        {
            StringBuilder sMessage = new StringBuilder();
            try
            {
                DataTable dtDistinctCompanyIDs = dtMasterCompanies.DefaultView.ToTable(true, "MASTER_ID");
                dtValidationResults.DefaultView.Sort = "CompanyID ASC";

                string sStyle = @"<style>
                            *{
                            font-family: 'Microsoft Sans Serif';
                            line-height: 150%;
                            font-size: 13px;
                            }

                            #Err {
                            color: Red;
                            }

                            #1 {
                            color: Tomato;
                            }

                            #2 {
                            color: DarkGoldenRod;
                            }

                            #3 {
                            color: SeaGreen;
                            }

                            #4 {
                            color: DodgerBlue;
                            }

                            #head1 {
                            font-size: x-small;
                            line-height: 200%;
                            }

                            #head2 {
                            font-size: x-small;
                            line-height: 170%;
                            color: DarkSlateGray;
                            letter-spacing: 1px;
                            }

                            #head3 {
                            color: RebeccaPurple;
                            font-size: small;
                            line-height: 170%;
                            letter-spacing: 1px;
                            }
                            </style>";

                if (IsDynamic)
                {
                    foreach (DataRow dr in dtValidationResultsDynamic.Rows)//Row index to Row Number
                    {
                        if (Convert.ToInt32(dr["RowIndex"]) >= 0)
                            dr["RowIndex"] = Convert.ToInt32(dr["RowIndex"]) + 1;
                    }
                    if (dtValidationResultsDynamic.Select("TableName = 'Contact'").Length > 0)
                    {
                        sMessage.AppendLine("<br/><span id='head2'>Contact Errors:</span>");
                        DataTable dtErrorContact = dtValidationResultsDynamic.Select("TableName = 'Contact'", "RowIndex ASC").CopyToDataTable();
                        //dtErrorContact.DefaultView.Sort = "RowIndex ASC";
                        string sRowID = dtErrorContact.Rows[0]["RowIndex"].ToString();
                        sMessage.AppendLine("<br/><span id='head1'>Contact : " + dtErrorContact.Rows[0]["RowIndex"] + "</span><br/>");
                        foreach (DataRow drError in dtErrorContact.Rows)
                        {
                            if (sRowID != drError["RowIndex"].ToString())
                            {
                                sMessage.AppendLine("<br/><span id='head1'>Contact : " + drError["RowIndex"] + "</span><br/>");
                                sRowID = drError["RowIndex"].ToString();
                            }
                            sMessage = GenerateError(drError, sMessage);
                        }
                    }

                    if (sMessage.ToString().Length > 0)
                    {
                        return sStyle + sMessage;
                    }
                    else
                        return string.Empty;
                }
                else
                {
                    GV.sValidationMessage = "Building Error Message";
                    List<string> lstEAFValidation = sEAFMessage.Split(Environment.NewLine.ToCharArray()).Distinct().ToList();
                    foreach (string sEAF in lstEAFValidation)
                    {
                        if (sEAF.Length > 0)
                            sMessage.AppendLine(sTab + sEAF + "<br/><br/>");
                    }

                    //if (lstApplicationError.Count > 0)
                    //{
                    //    sMessage.AppendLine("<span id='head2'>Application Error</span><br/>");
                    //    foreach (string sAppError in lstApplicationError)
                    //        sMessage.AppendLine(sTab + "<span id ='Err'>" + sAppError + "</span><br/>");
                    //}
                }

                foreach (DataRow dr in dtValidationResults.Rows)//Row index to Row Number
                {
                    if (Convert.ToInt32(dr["RowIndex"]) >= 0)
                        dr["RowIndex"] = Convert.ToInt32(dr["RowIndex"]) + 1;
                }

                foreach (DataRow drMaster_ID in dtDistinctCompanyIDs.Rows)
                {
                    if (dtValidationResults.Select("CompanyID = " + drMaster_ID["MASTER_ID"]).Length > 0)
                    {
                        DataTable dtError = dtValidationResults.Select("CompanyID = " + drMaster_ID["MASTER_ID"]).CopyToDataTable();
                        sMessage.AppendLine("<br/><span id='head3'>Company ID : " + drMaster_ID["MASTER_ID"] + "</span>");

                        if (dtError.Select("TableName = 'Company'").Length > 0)
                        {
                            DataTable dtErrorCompany = dtError.Select("TableName = 'Company'").CopyToDataTable();
                            sMessage.AppendLine("<br/><span id='head2'>Company Errors:</span><br/>");
                            foreach (DataRow drError in dtErrorCompany.Rows)
                                sMessage = GenerateError(drError, sMessage);
                        }

                        if (dtError.Select("TableName = 'Contact'").Length > 0)
                        {
                            sMessage.AppendLine("<br/><span id='head2'>Contact Errors:</span>");
                            DataTable dtErrorContact = dtError.Select("TableName = 'Contact'", "RowIndex ASC").CopyToDataTable();
                            //dtErrorContact.DefaultView.Sort = "RowIndex ASC";
                            string sRowID = dtErrorContact.Rows[0]["RowIndex"].ToString();
                            sMessage.AppendLine("<br/><span id='head1'>Contact : " + dtErrorContact.Rows[0]["RowIndex"] + "</span><br/>");
                            foreach (DataRow drError in dtErrorContact.Rows)
                            {
                                if (sRowID != drError["RowIndex"].ToString())
                                {
                                    sMessage.AppendLine("<br/><span id='head1'>Contact : " + drError["RowIndex"] + "</span><br/>");
                                    sRowID = drError["RowIndex"].ToString();
                                }
                                sMessage = GenerateError(drError, sMessage);
                            }
                        }
                        sMessage.AppendLine("<hr/>");
                    }
                }

                if (sMessage.ToString().Length > 0)
                {
                    return sStyle + sMessage;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                return sMessage.ToString();
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private string External_Library(DoWorkEventArgs e)
        {
            string sEAFMessage = string.Empty;
            if (RunASynch(e))
            {
                DataTable dtEAFValidation = (DataTable)EAF_Call("Validation");
                foreach (DataRow drEFAValidation in dtEAFValidation.Rows)
                {
                    if (drEFAValidation[0].ToString().ToUpper() == "ER" && drEFAValidation[1].ToString().Length > 0)
                        sEAFMessage = drEFAValidation[1].ToString();
                    else if (drEFAValidation[0].ToString().ToUpper() == "EX" && drEFAValidation[1].ToString().Length > 0)
                    {
                        Exception ex = (Exception)EAF_MethodInstance.InvokeMember("GetException", BindingFlags.Default | BindingFlags.InvokeMethod, null, EAF_ClassInstance, new Object[] { });
                        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                        lstApplicationError.Add("Library Error : " + ex.Message);
                        return "Error occured on External Library";
                    }
                }
                return sEAFMessage;
            }
            else return string.Empty;
        }

        void RecordTime(string s)
        {
            sStatistics.AppendLine(s + "\t" + sWatch.Elapsed.TotalSeconds);
        }

        //-----------------------------------------------------------------------------------------------------
        private void bWorkerSave_DoWork(object sender, DoWorkEventArgs e)
        {
            ValidateNSave(e);
        }

        void Block_Save(DataTable dtTableTockeck, string sTableName)
        {
            DataRow[] drrBlock = dtBlock.Select("TABLE = '" + sTableName + "' AND BLOCK_TYPE = 'SAVE'");
            if (drrBlock.Length > 0)
            {
                for (int i = 0; i < dtTableTockeck.Rows.Count; i++)
                {
                    foreach (DataRow drBlock in drrBlock)
                    {
                        string sField = drBlock["FIELD"].ToString();
                        if (dtTableTockeck.Columns.Contains(sField))
                        {
                            string sMatchValue = dtTableTockeck.Rows[i][sField].ToString().Trim();
                            string sBlock_Message = Block_Match(sMatchValue, drBlock);
                            if (sBlock_Message.Length > 0)
                            {
                                //if(sTableName.ToUpper() == "COMPANY")
                                //    AddValidationResults(sTableName, i, Convert.ToInt32(dtTableTockeck.Rows[i]["MASTER_ID"]), 0, sBlock_Message, false, 2);
                                //else
                                AddValidationResults(sTableName, i, Convert.ToInt32(dtTableTockeck.Rows[i]["MASTER_ID"]), 0, sBlock_Message, false, 2);
                            }
                        }
                    }
                }
            }
        }

        void ValidateNSave(DoWorkEventArgs e)
        {
            try
            {
                sWatch = Stopwatch.StartNew();
                IsNewCompletesAdded = false;
                string sStaticName = Environment.MachineName + "_" + GV.sProjectID + "_" + sMaster_ID + ".txt";

                sStatistics.AppendLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                sStatistics.AppendLine("Project Name: \t" + GV.sProjectName);
                sStatistics.AppendLine("Agent Name : \t" + GV.sEmployeeName);
                sStatistics.AppendLine("Date: \t" + GM.GetDateTime().ToString("dd-MM-yyyy HH:mm:ss"));
                sStatistics.AppendLine("Contact Count: \t" + dtMasterContacts.Rows.Count);
                sStatistics.AppendLine("Location: \t" + "ContactUpdate" + Environment.NewLine + Environment.NewLine + Environment.NewLine);

                RecordTime("Validation Start");

                sBrowserHTML = string.Empty;
                dtValidationResults.Rows.Clear();
                lstApplicationError.Clear();
                lstQCIDsToLog.Clear();
                lstQCRowsToDelete.Clear();

                //btnSave.Enabled = false;                

                this.BindingContext[dtMasterContacts].EndCurrentEdit();
                this.BindingContext[dtMasterCompanies].EndCurrentEdit();

                Reload_PickList();//Get fresh set of Picklist if Updated

                RecordTime("EAF Start");
                string sEAFValidation = External_Library(e);
                RecordTime("EAF Stop");

                RecordTime("DataPopulate Start");
                All_PostUpdate(e);
                RecordTime("DataPopulate Stop");

                RecordTime("Validation_Table Start");
                ALL_ValidationTable_Check(e);
                RecordTime("Validation_Table Stop");

                Block_Save(dtMasterCompanies, "Company");
                Block_Save(dtMasterContacts, "Contact");

                RecordTime("COMPANY_All_Validation Start");
                COMPANY_All_Validation(e);//Validates Company
                RecordTime("COMPANY_All_Validation Stop");

                RecordTime("CONTACT_All_Validation Start");
                CONTACT_All_Validation(e);//Validates Contacts
                RecordTime("CONTACT_All_Validation Stop");

                RecordTime("BuildError Start");
                string sMessage = BuildError(sEAFValidation, false);
                RecordTime("BuildError Stop");

                sBrowserHTML = sMessage.ToString();
                RecordTime("Validation Stop");

                if (RunASynch(e))
                {
                    if (lstApplicationError.Count == 0 && dtValidationResults.Rows.Count == 0 && sEAFValidation.Trim().Length == 0)
                    {
                        if (GV.sAccessTo == "TR") Hangup(); //Hang up call if not.

                        RecordTime("Save Start");
                        RecordTime("COMPANY_UpdateChangedRecords Start");
                        COMPANY_UpdateChangedRecords();//Save disposals to Master Datatable
                        RecordTime("COMPANY_UpdateChangedRecords Stop");

                        RecordTime("CONTACT_UpdateChangedRecords Start");
                        CONTACT_UpdateChangedRecords();//Update Modified records with Current date and Current Agent Name
                        RecordTime("CONTACT_UpdateChangedRecords Stop");

                        //-----------------------------------------------------------------------------------------------------
                        #region Warning
                        //-----------------------------------------------------------------------------------------------------
                        //string sEmailNameDominCheck = Email_NameAndDomin_Check();
                        //if (sEmailNameDominCheck.Contains("EmailName") && sEmailNameDominCheck.Contains("EmailDomain"))
                        //{
                        //    if (DialogResult.No == MessageBoxEx.Show("Contact Name and Domain Name doesn't match email." + Environment.NewLine + "Do you wish to continue ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                        //        return;
                        //}
                        //else if (sEmailNameDominCheck.Contains("EmailName"))
                        //{
                        //    if (DialogResult.No == MessageBoxEx.Show("Contact Name doesn't match email." + Environment.NewLine + "Do you wish to continue ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                        //        return;
                        //}
                        //else if (sEmailNameDominCheck.Contains("EmailDomain"))
                        //{
                        //    if (DialogResult.No == MessageBoxEx.Show("Website domain doesn't match email." + Environment.NewLine + "Do you wish to continue ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                        //        return;
                        //
                        //-----------------------------------------------------------------------------------------------------
                        #endregion
                        //-----------------------------------------------------------------------------------------------------

                        RecordTime("TrimCells Company Start");
                        TrimCells(dtMasterCompanies);//Trim All the fields
                        RecordTime("TrimCells Company Stop");

                        RecordTime("TrimCells Contact Start");
                        TrimCells(dtMasterContacts);
                        RecordTime("TrimCells Contact Stop");

                        if (lstApplicationError.Count == 0)
                        {
                            UpdateNotifier(4);
                            RecordTime("Logging Company Start");
                            Logging(dtMasterCompanies, dtMasterCompaniesCopy);//Log Changes in data.. New Data will not be logged
                            RecordTime("Logging Company Stop");

                            RecordTime("Logging Contact Start");
                            Logging(dtMasterContacts, dtMasterContactsCopy);
                            RecordTime("Logging Contact Stop");

                            this.BindingContext[dtMasterContacts].EndCurrentEdit();
                            this.BindingContext[dtMasterCompanies].EndCurrentEdit();

                            UpdateNotifier(5);

                            string sQCRemove_Log = QC_OK_Edit();

                            RecordTime("Save Company DB Start");
                            if (SaveToDB(dtMasterCompanies, GV.sCompanyTable))//Save MasterCompanies
                            {
                                //   GV.SQLCE.BAL_DeleteFromTable(GV.sSQLCECompanyTable, "Master_ID=" + sMaster_ID + "");
                            }
                            else
                            {
                                GV.iNotifier[6] = 1;
                                //sBrowserHTML = "<span id ='head2'>Database Error</span><br/>" + sTab + "<span id ='Err'>Error Pushing Company data</span><br/><br/>";
                                lstApplicationError.Add("Error Pushing Company data");

                                return;
                            }
                            RecordTime("Save Company DB Stop");

                            GV.sValidationMessage = "Saving Contact to Database";

                            RecordTime("Save Contact DB Start");

                            
                            if (SaveToDB(dtMasterContacts, GV.sContactTable))//Save MasterContacts
                            {
                                string sPickList_Insert = string.Empty;
                                string sPickList_Delete = string.Empty;

                                foreach (DataRow drPicklist_Insert in dtPicklist_Insert.Rows)
                                {
                                    if (drPicklist_Insert["Value"].ToString().Length > 0)
                                        sPickList_Insert += "INSERT INTO " + GV.sProjectID + "_picklists (PicklistCategory,PicklistValue, remarks) VALUES " + drPicklist_Insert["Value"] + ";";
                                }

                                foreach (DataRow drPicklist_Delete in dtPicklist_Delete.Rows)
                                {
                                    if (drPicklist_Delete["Value"].ToString().Length > 0)
                                        sPickList_Delete += "DELETE FROM " + GV.sProjectID + "_picklists WHERE remarks ='Pending' AND PicklistCategory = '" + drPicklist_Delete["PicklistCategory"] + "' AND PicklistValue IN (" + drPicklist_Delete["Value"] + ");";
                                }

                                if ((sPickList_Insert + sPickList_Delete).Trim().Length > 0)
                                    GV.MYSQL.BAL_ExecuteNonReturnQueryMySQL(sPickList_Insert + " " + sPickList_Delete + " UPDATE c_project_settings set PICKLIST_LASTUPDATE = NOW() WHERE PROJECT_ID='" + GV.sProjectID + "';");

                                //   GV.SQLCE.BAL_DeleteFromTable(GV.sSQLCEContactTable, "Master_ID=" + sMaster_ID + "");
                            }
                            else
                            {
                                GV.iNotifier[6] = 1;
                                //sBrowserHTML = "<span id ='head2'>Database Error</span><br/>" + sTab + "<span id ='Err'>Error Pushing Contact data</span><br/><br/>";
                                lstApplicationError.Add("Error Pushing Contact data");
                                return;
                            }
                            RecordTime("Save Contact DB Stop");

                            tAutoSaveRecords.Stop();

                            RecordTime("Save QC DB Start");
                            if (SaveToDB(dtQCTable, GV.sQCTable))//Save MasterContacts
                            {
                                if (sQCRemove_Log.Length > 0)
                                    GV.MYSQL.BAL_ExecuteQueryMySQL(sQCRemove_Log);
                                //   GV.SQLCE.BAL_DeleteFromTable(GV.sSQLCEContactTable, "Master_ID=" + sMaster_ID + "");
                            }
                            else
                            {
                                GV.iNotifier[6] = 1;
                                //sBrowserHTML = "<span id ='head2'>Database Error</span><br/>" + sTab + "<span id ='Err'>Error Pushing QC data</span><br/><br/>";
                                lstApplicationError.Add("Error Pushing QC data");
                                return;
                            }
                            RecordTime("Save QC DB Stop");

                            UpdateNotifier(6);

                            //RecordTime("Revenue_Calc Start");
                            //Revenue_Calc();
                            //RecordTime("Revenue_Calc Stop");



                            Email_Checks();

                            RecordTime("Save Stop");

                            WriteText(sStaticName);

                            CloseNotifier();
                            Log_OpenClose("Closed");
                            GM.Moniter(true);
                            IsRecordSaved = true;
                            Remove_Chached();//Remove all data backed for crash recovery

                            tAutoSaveRecords.Start();
                        }
                        else
                        {
                            string sError = string.Empty;
                            foreach (string sAppError in lstApplicationError)
                                sError += sTab + "<span id ='Err'>" + sAppError + "</span><br/><br/>";
                            sBrowserHTML = "<span id ='head2'>Application Error</span><br/>" + sError + sBrowserHTML;
                        }
                    }
                    else
                    {
                        RecordTime("Save Cancelled");
                        WriteText(sStaticName);
                        return;
                    }
                }
                else
                    return;

                //((frmMain)this.MdiParent).progressBar.Visible = false;
            }
            catch (Exception ex)
            {
                CloseNotifier();
                objNotifier.msgRefresh.Stop();

                this.Invoke((MethodInvoker)delegate { this.Enabled = true; });
                //this.Enabled = true;
                e.Cancel = true;

                if (!tAutoSaveRecords.Enabled)
                    tAutoSaveRecords.Start();

                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void WriteText(string sStaticName)
        {
            try//Do not trigger any error
            {
                if (sWatch.Elapsed.TotalSeconds > 1)
                {
                    StreamWriter sWrite = new StreamWriter(@"\\172.27.137.182\Campaign Manager\PerformanceLog\" + sStaticName, true);
                    sWrite.WriteLine(sStatistics);
                    sWrite.Close();
                    sStatistics.Clear();
                }
            }
            catch (Exception ex) { }
        }

        void UpdateNotifier(int i)
        {
            if (GV.iNotifier[i] == 0)
                GV.iNotifier[i] = 2;
            else if (GV.iNotifier[i] == 1)
                GV.iNotifier[i] = 3;
        }

        //-----------------------------------------------------------------------------------------------------
        private void bWorkerSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CloseNotifier();
            AlignContactGridColumn();

            if (IsRecordSaved)
            {
                ToastNotification.Show(this, "Company Saved.", eToastPosition.TopRight);
                webBrowserMessage.DocumentText = string.Empty;
                expandablePanelMessage.Expanded = false;
                Load_Next_Record();
            }
            else
            {
                Impact_CompanyTable(iCompanyRowIndex);
                LoadSuperGridContact();
                if (lstApplicationError.Count > 0)
                {
                    string sError = @"<style>
                            *{
                            font-family: 'Microsoft Sans Serif';
                            line-height: 150%;
                            font-size: 13px;
                            }

                            #Err {
                            color: Red;
                            }                            

                            #head2 {
                            font-size: x-small;
                            line-height: 170%;
                            color: DarkSlateGray;
                            letter-spacing: 1px;
                            }
                            </style>";

                    foreach (string sAppError in lstApplicationError)
                        sError += sTab + "<span id ='Err'>" + sAppError + "</span><br/>";

                    sBrowserHTML = "<span id='head2'>Application Error</span><br/>" + sError + sBrowserHTML;
                }
                webBrowserMessage.DocumentText = sBrowserHTML;
                if (sBrowserHTML.Length > 0)
                    expandablePanelMessage.Expanded = true;
            }
        }

        void Cancel_Asynch(string sMessage, DoWorkEventArgs e)
        {
            
            if (bWorkerSave.IsBusy)
            {
                this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate () { objNotifier.msgRefresh.Stop(); });
                this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate () { objNotifier.lblValidationMessage.Text = sMessage; });
                GV.sValidationMessage = sMessage;
                bWorkerSave.CancelAsync();
                if (e != null)
                    e.Cancel = true;
                while (bWorkerSave.IsBusy)//Wait till cancellation completes
                    Application.DoEvents();
                CloseNotifier();
                this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate () { ToastNotification.Show(this, "Save Cancelled!", eToastPosition.TopRight); });
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void btn_Cancle_Asynch(object sender, EventArgs e)
        {
            DoWorkEventArgs e1 = null;
            Cancel_Asynch("Cancelling...", e1);
        }

        void CloseNotifier()
        {
            this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate () { objNotifier.Hide(); });
            this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate () { objNotifier.msgRefresh.Stop(); });
            this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate () { this.Enabled = true; });
            this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate () { this.WindowState = FormWindowState.Maximized; });
            //this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate() { btnSave.Enabled = true; });
            //AlignContactGridColumn();
        }

        bool RunASynch(DoWorkEventArgs e)
        {
            if (bWorkerSave.CancellationPending)
            {
                e.Cancel = true;
                return false;
            }
            else
                return true;
        }

        private void tabControlContact_SelectedTabChanging(object sender, SuperTabStripSelectedTabChangingEventArgs e)
        {
            //if (e.NewValue.Name == "tabQC")
            //{
            //    DataRow[] drrQCWeekIDs = dtQCWeekIDs.Select("TableName = 'Contact' AND RowIndex = " + iContactRowIndex);
            //    if (drrQCWeekIDs.Length > 0)
            //    {
            //        if (drrQCWeekIDs[0]["MISID"].ToString().Length > 0)
            //        { 
            //            //if(dtHRIMSQCDetails.Select();
            //        }
            //        else
            //        {
            //            e.Cancel = true;
            //            ToastNotification.Show(this, "Week not created for " + drrQCWeekIDs[0]["AgentName"] + " - " + dtMasterContacts.Rows[iContactRowIndex]["TR_UPDATED_DATE"].ToString().Substring(0, 10), eToastPosition.TopRight);
            //        }
            //    }
            //    else
            //    {
            //        e.Cancel = true;
            //        ToastNotification.Show(this, "Agent Name or Date not found.", eToastPosition.TopRight);
            //    }
            //}
        }

        string QC_OK_Edit()
        {
            string sLog_Query = string.Empty;
            try
            {
                if (lstQCRowsToDelete.Count > 0)
                {
                    string sMachine = Environment.MachineName;
                    foreach (DataRow drToDelete in lstQCRowsToDelete)
                    {
                        string sOldData = string.Empty;
                        foreach (DataColumn DC in drToDelete.Table.Columns)
                            sOldData += "[" + DC.ColumnName + ":" + drToDelete[DC.ColumnName].ToString() + "]";

                        if (sLog_Query.Length > 0)
                            sLog_Query += "," + "(" + drToDelete["RecordID"].ToString() + ",'QC','DataRemoved','" + sOldData.Replace("'", "''") + "',NOW(),'" + GV.sEmployeeName + "','" + sMachine + "')";
                        else
                            sLog_Query = "(" + drToDelete["RecordID"].ToString() + ",'QC','DataRemoved','" + sOldData.Replace("'", "''") + "',NOW(),'" + GV.sEmployeeName + "','" + sMachine + "')";

                        drToDelete.Delete(); //Remove qc OK records that are deleted
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }

            if (sLog_Query.Length > 0)
                return "INSERT INTO " + GV.sProjectID + "_log (RecordID, TableName, FieldName, oldvalue, `When`, Who, SystemName) VALUES " + sLog_Query;
            else
                return string.Empty;
        }

        private void rbnBarDial_DialogLauncherMouseDown(object sender, MouseEventArgs e)
        {
            frmCallLog objfrmCallLog = new frmCallLog();
            objfrmCallLog.dtCallLog = dtDialLogger;
            objfrmCallLog.sCompany_Name = dtMasterCompanies.Rows[iCompanyRowIndex]["COMPANY_NAME"].ToString();
            objfrmCallLog.sSwitchboard = dtMasterCompanies.Rows[iCompanyRowIndex]["SWITCHBOARD"].ToString();
            objfrmCallLog.ShowDialog();
        }

        public string RespondChrome(string sChContent)
        {
            try
            {

                if (bWorkerSave.IsBusy)
                    return GM.ChromeResponse("SaveBusy");

                if (!btnSave.Enabled)
                    return GM.ChromeResponse("CompleteSurvey");


                //return GM.ChromeResponse("CompleteSurvey");

                string sChDisplayName = sChContent.Split(new string[] { "::{" }, StringSplitOptions.None)[0];
                string sValue = sChContent.Split(new string[] { "::{" }, StringSplitOptions.None)[1].Trim();
                string sTable = string.Empty;
                string sFieldName = string.Empty;

                if (sChDisplayName.StartsWith("chMnu_"))
                {
                    sChDisplayName = sChDisplayName.Replace("chMnu_", "");
                    if (sChDisplayName.ToLower().StartsWith("mastercontacts____"))
                    {
                        if (iContactRowIndex == -1) //if no Contacts, Add a Contact
                        {
                            tabControlContact.Invoke((MethodInvoker)delegate { tabControlContact.SelectedPanel = sTabPanelContacts; });
                            if (btnAddNewContact.Visible)
                            {
                                tabControlContact.Invoke(
                                    (MethodInvoker)delegate { tabControlContact.SelectedPanel = sTabPanelContacts; });
                                btnAddNewContact.PerformClick();
                            }
                            else
                                return GM.ChromeResponse("ContactNotAdded");
                        }
                        else
                        {
                            if (dtMasterContacts.Rows[iContactRowIndex]["CONTACT_ID_P"].ToString().Length > 0 && lstFreezedContactIDs.Contains(Convert.ToInt32(dtMasterContacts.Rows[iContactRowIndex]["CONTACT_ID_P"])))
                                return GM.ChromeResponse("Freeze");
                        }

                        sTable = "mastercontacts";
                        sChDisplayName = sChDisplayName.ToLower().Replace("mastercontacts____", "");
                    }
                    else if (sChDisplayName.ToLower().StartsWith("master____"))
                    {
                        sTable = "master";
                        sChDisplayName = sChDisplayName.ToLower().Replace("master____", "");
                    }
                    DataRow[] drrFieldMaster_Active =
                        dtFieldMaster_Active.Select("TABLE_NAME = '" + sTable + "' AND CHROME_DISPLAYNAME = '" +
                                                    sChDisplayName + "'");
                    if (drrFieldMaster_Active.Length > 0)
                    {
                        if (drrFieldMaster_Active[0]["CHROME_OUTSTRING_FUNCTIONALITY"].ToString().ToUpper() == "NAME")
                        {
                            DataTable dtActive_NameFields =
                                drrFieldMaster_Active.CopyToDataTable()
                                    .Select("CHROME_OUTSTRING_FUNCTIONALITY = 'NAME'")
                                    .CopyToDataTable();
                            string[] sNameSplit = sValue.Split(' ');
                            if (sNameSplit.Length == 1)
                            {
                                if (dtActive_NameFields.Select("CHROME_OUTSTRING_FIELD = 'cn_fn'").Length > 0)
                                    SetText(sTable,
                                        dtActive_NameFields.Select("CHROME_OUTSTRING_FIELD = 'cn_ln'")[0]["FIELD_NAME_TABLE"
                                            ].ToString().ToUpper(), sValue);

                                if (dtActive_NameFields.Select("CHROME_OUTSTRING_FIELD = 'cn_ln'").Length > 0)
                                    SetText(sTable,
                                        dtActive_NameFields.Select("CHROME_OUTSTRING_FIELD = 'cn_ln'")[0]["FIELD_NAME_TABLE"
                                            ].ToString().ToUpper(), string.Empty); //Empty last name
                            }
                            else if (sNameSplit.Length > 1)
                            {
                                string sFname = string.Empty;
                                string sLname = string.Empty;
                                bool isSpecialSplit = false;
                                for (int i = 0; i < sNameSplit.Length; i++)
                                {
                                    if (isSpecialSplit ||
                                        (i > 0 && GV.lstNameSplit.Contains(sNameSplit[i], StringComparer.OrdinalIgnoreCase)))
                                    {
                                        isSpecialSplit = true;
                                        if (sLname.Length > 0)
                                            sLname += " " + sNameSplit[i];
                                        else
                                            sLname = sNameSplit[i];
                                        continue;
                                    }

                                    if ((i + 1) == sNameSplit.Length)
                                        sLname = sNameSplit[i];
                                    else
                                    {
                                        if (sFname.Length > 0)
                                            sFname += " " + sNameSplit[i];
                                        else
                                            sFname = sNameSplit[i];
                                    }
                                }

                                if (dtActive_NameFields.Select("CHROME_OUTSTRING_FIELD = 'cn_fn'").Length > 0)
                                {
                                    sFieldName =
                                        dtActive_NameFields.Select("CHROME_OUTSTRING_FIELD = 'cn_fn'")[0]["FIELD_NAME_TABLE"
                                            ].ToString().ToUpper();
                                    SetText(sTable, sFieldName, sFname);
                                }

                                if (dtActive_NameFields.Select("CHROME_OUTSTRING_FIELD = 'cn_ln'").Length > 0)
                                {
                                    sFieldName =
                                        dtActive_NameFields.Select("CHROME_OUTSTRING_FIELD = 'cn_ln'")[0][
                                            "FIELD_NAME_TABLE"].ToString().ToUpper();
                                    SetText(sTable, sFieldName, sLname);
                                }
                            }
                        }
                        else
                        {
                            sFieldName = drrFieldMaster_Active[0]["FIELD_NAME_TABLE"].ToString().ToUpper();
                            SetText(sTable, sFieldName, sValue);
                        }
                    }
                    return GM.ChromeResponse("Received");
                }
                else
                {
                    if (sChDisplayName == "cmd_AddContact")
                    {
                        tabControlContact.Invoke((MethodInvoker)delegate { tabControlContact.SelectedPanel = sTabPanelContacts; });
                        btnAddNewContact.PerformClick();
                        return GM.ChromeResponse("ContactAdded");
                    }
                    else if (sChDisplayName == "cmd_lkdnData")
                    {
                        tabControlContact.Invoke((MethodInvoker)delegate { tabControlContact.SelectedPanel = sTabPanelContacts; });
                        if (btnAddNewContact.Visible)
                        {
                            //tabControlContact.Invoke((MethodInvoker)delegate { tabControlContact.SelectedPanel = sTabPanelContacts; });
                            btnAddNewContact.PerformClick();

                            List<string> lstValue = sValue.Split(new string[] { "|~|" }, StringSplitOptions.None).ToList();
                            foreach (string sFieldValues in lstValue)
                            {
                                string sLkdnFieldName = sFieldValues.Split(new string[] { ":~:" }, StringSplitOptions.None)[0];
                                string sLkdnValueValue = sFieldValues.Split(new string[] { ":~:" }, StringSplitOptions.None)[1];
                                DataRow[] drrFieldMaster_Active = dtFieldMaster_Active.Select("CHROME_OUTSTRING_FIELD = '" + sLkdnFieldName + "'");
                                if (drrFieldMaster_Active.Length > 0)
                                {
                                    sFieldName = drrFieldMaster_Active[0]["FIELD_NAME_TABLE"].ToString();
                                    sTable = drrFieldMaster_Active[0]["TABLE_NAME"].ToString().ToLower();
                                    SetText(sTable, sFieldName, sLkdnValueValue);
                                }
                            }
                            return GM.ChromeResponse("Received");
                        }
                        else
                            return GM.ChromeResponse("ContactNotAdded");
                    }
                    return GM.ChromeResponse("NotReceived");
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                return GM.ChromeResponse("CMException");
            }
        }

        void SetText(string sTable, string sFieldName, string sValue)
        {
            if (sFieldName.Length > 0 && sTable.Length > 0)
            {
                if (sTable == "mastercontacts")
                {
                    foreach (TextBox txt in lstContactControls)
                    {
                        if (txt.Name.ToUpper() == sFieldName.ToUpper())
                            txt.Invoke((MethodInvoker)delegate { txt.Text = sValue; });

                    }
                }
                else if (sTable == "master")
                {
                    foreach (TextBox txt in lstCompanyControls)
                    {
                        if (txt.Name.ToUpper() == sFieldName.ToUpper())
                            txt.Invoke((MethodInvoker)delegate { txt.Text = sValue; });
                    }
                }
            }
        }

        private void tPrePostCall_Tick(object sender, EventArgs e)
        {
            if (!bWorkerSave.IsBusy)
            {
                if (CallinProgress)
                {
                    IsPreCall = false;
                    circularProgressPrePostCall.ProgressColor = Color.Goldenrod;
                    if (controlContainerIPrePostCall.Visible)
                    {
                        controlContainerIPrePostCall.Visible = false;
                        lblPrePostDuration.Visible = false;
                        rbnBarDial.Refresh();
                    }
                    lblPrePostDuration.Text = "00:00:00";
                    circularProgressPrePostCall.Maximum = GV.iPostCallLimit;
                    circularProgressPrePostCall.ProgressText = "0";
                    circularProgressPrePostCall.Value = 0;
                    iPostCall = 0;
                }
                else
                {
                    if (!controlContainerIPrePostCall.Visible)
                    {
                        controlContainerIPrePostCall.Visible = true;
                        lblPrePostDuration.Visible = true;
                        rbnBarDial.Refresh();
                    }
                    int iPercent = 0;

                    DateTime dDate1 = default(DateTime);
                    dDate1 = Convert.ToDateTime(lblPrePostDuration.Text);
                    dDate1 = dDate1.AddSeconds(1);
                    lblPrePostDuration.Text = dDate1.ToString("HH:mm:ss");

                    if (IsPreCall)
                    {
                        iPreCall--;
                        iPercent = Convert.ToInt32((iPreCall / Convert.ToDouble(GV.iPreCallLimit)) * 100);

                        if (iPercent <= 0)
                        {
                            circularProgressPrePostCall.ProgressText = "0";
                            circularProgressPrePostCall.Value = GV.iPreCallLimit;

                            if (circularProgressPrePostCall.ProgressColor == Color.Red)
                                circularProgressPrePostCall.ProgressColor = Color.Transparent;
                            else
                                circularProgressPrePostCall.ProgressColor = Color.Red;
                            //controlContainerIPrePostCall.Visible = !controlContainerIPrePostCall.Visible;
                            //controlContainerIPrePostCall.Refresh();                    
                            //rbnBarDial.Refresh();
                        }
                        else if (iPercent < 25)
                            //circularProgressPrePostCall.ProgressColor = ColorTranslator.FromHtml("#FF0000");
                            circularProgressPrePostCall.ProgressColor = Color.Red;
                        else
                            circularProgressPrePostCall.ProgressColor = Color.Green;

                        if (iPercent > 0)
                        {
                            circularProgressPrePostCall.Value = iPreCall;
                            circularProgressPrePostCall.ProgressText = iPreCall.ToString();
                        }
                        //circularProgressPrePostCall.ProgressColor = ColorTranslator.FromHtml("#cccc00");                        
                    }
                    else
                    {
                        iPostCall++;
                        iPercent = Convert.ToInt32((iPostCall / Convert.ToDouble(GV.iPostCallLimit)) * 100);


                        if (iPercent >= 100)
                        {
                            circularProgressPrePostCall.ProgressText = GV.iPostCallLimit.ToString();
                            circularProgressPrePostCall.Value = GV.iPostCallLimit;

                            if (circularProgressPrePostCall.ProgressColor == Color.Red)
                                circularProgressPrePostCall.ProgressColor = Color.Transparent;
                            else
                                circularProgressPrePostCall.ProgressColor = Color.Red;
                            //controlContainerIPrePostCall.Visible = !controlContainerIPrePostCall.Visible;
                            //controlContainerIPrePostCall.Refresh();                    
                            //rbnBarDial.Refresh();
                        }
                        else if (iPercent > 75)
                            circularProgressPrePostCall.ProgressColor = Color.Red;
                        else
                            circularProgressPrePostCall.ProgressColor = Color.Goldenrod;

                        if (iPercent < 100)
                        {
                            circularProgressPrePostCall.Value = iPostCall;
                            circularProgressPrePostCall.ProgressText = iPostCall.ToString();
                        }


                    }


                    //if (iPercent < 25)                    
                    //    circularProgressPrePostCall.ProgressColor = ColorTranslator.FromHtml("#FF0000");                    
                    //else
                    //    circularProgressPrePostCall.ProgressColor = ColorTranslator.FromHtml("#009900"); 



                    //else if (iPercent <= 0)
                    //{
                    //    circularProgressPrePostCall.ProgressText = "0";
                    //    circularProgressPrePostCall.Value = iMaxvalue;

                    //    if (circularProgressPrePostCall.ProgressColor == Color.Transparent)
                    //        circularProgressPrePostCall.ProgressColor = ColorTranslator.FromHtml("#FF0000");
                    //    else
                    //        circularProgressPrePostCall.ProgressColor = Color.Transparent;

                    //    //controlContainerIPrePostCall.Visible = !controlContainerIPrePostCall.Visible;
                    //    //controlContainerIPrePostCall.Refresh();                    
                    //    //rbnBarDial.Refresh();
                    //}


                }
            }
        }

        void InitilizeCallTracker()
        {
            if (GV.sUserType == "Agent" && GV.sAccessTo == "TR")
            {
                if (GV.iPreCallLimit > 0 || GV.iPostCallLimit > 0)
                {

                    tPrePostCall.Start();
                    IsPreCall = true;
                    circularProgressPrePostCall.ProgressColor = ColorTranslator.FromHtml("#009900");
                    controlContainerIPrePostCall.Visible = true;
                    lblPrePostDuration.Visible = true;
                    rbnBarDial.Refresh();
                    circularProgressPrePostCall.Maximum = iPreCall = GV.iPreCallLimit;
                    iPostCall = GV.iPostCallLimit;
                    lblPrePostDuration.Text = "00:00:00";
                }
            }
            else
                itemContainerPrePostCall.Visible = false;
        }

        private void tabRecordHistory_DoubleClick(object sender, EventArgs e)
        {

            //List<string> lstCompanySessions = new List<string>();            
            //dtProjectLog = GV.MYSQL.BAL_ExecuteQueryMySQL(@"SELECT A.RecordID,A.CompanySessionID, A.Who, B.OldValue - A.OldValue AS 'NewContactsAdded',A.NewValue AS 'UserType',`A`.`When` AS 'OpeningTime',B.`When` AS 'ClosingTime',B.FieldName AS 'CloseType',A.SystemName FROM ((SELECT * FROM " + GV.sProjectID + "_log WHERE RecordID = " + sMaster_ID + " AND TableName = 'RecordStatus' AND FieldName = 'Opened') AS A LEFT JOIN(SELECT * FROM " + GV.sProjectID + "_log WHERE RecordID = " + sMaster_ID + " AND TableName = 'RecordStatus' AND FieldName IN('Closed', 'Closed without Saving')) AS B ON A.CompanySessionID = B.CompanySessionID) WHERE A.CompanySessionID IS NOT NULL ORDER BY OpeningTime;");
            //foreach (DataRow drProjectLog in dtProjectLog.Rows)                
            //    lstCompanySessions.Add(drProjectLog["CompanySessionID"].ToString());
                        
            //if (lstCompanySessions.Count > 0)
            //    dtCompanyContact_Log = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM " + GV.sProjectID + "_log WHERE  CompanySessionID IN (" + GM.ListToQueryString(lstCompanySessions, "String") + ");");
            //else
            //    dtCompanyContact_Log.Rows.Clear();

            //Load_History(dtProjectLog, dtCompanyContact_Log, string.Empty);
        }

        void Load_History(DataTable dtProjectLog, DataTable dtCompanyContact_Log, string sSearchText)
        {
            advTreeRecordHistoryAgent.Nodes.Clear();
            foreach (DataRow drProjectLog in dtProjectLog.Rows)
            {
                DevComponents.AdvTree.Node nNode = new DevComponents.AdvTree.Node();                
                nNode.Name = drProjectLog["CompanySessionID"].ToString();
                nNode.Text = GM.ProperCase(drProjectLog["Who"].ToString());
                DevComponents.AdvTree.Cell cellOpTime = new DevComponents.AdvTree.Cell();
                DevComponents.AdvTree.Cell cellCloTime = new DevComponents.AdvTree.Cell();
                DevComponents.AdvTree.Cell cellNewCont = new DevComponents.AdvTree.Cell();
                DevComponents.AdvTree.Cell cellCloseType = new DevComponents.AdvTree.Cell();
                DevComponents.AdvTree.Cell cellMachine = new DevComponents.AdvTree.Cell();
                DevComponents.AdvTree.Cell cellLevel = new DevComponents.AdvTree.Cell();

                cellOpTime.Text = drProjectLog["OpeningTime"].ToString();
                cellCloTime.Text = drProjectLog["ClosingTime"].ToString();
                cellCloseType.Text = drProjectLog["CloseType"].ToString();
                cellLevel.Text = drProjectLog["UserType"].ToString();
                cellMachine.Text = drProjectLog["SystemName"].ToString();
                cellNewCont.Text = drProjectLog["NewContactsAdded"].ToString();
                //nNode.Text = dr["Who"].ToString(); ;                
                nNode.Cells.Add(cellOpTime);
                nNode.Cells.Add(cellCloTime);
                nNode.Cells.Add(cellNewCont);
                nNode.Cells.Add(cellCloseType);
                nNode.Cells.Add(cellLevel);
                nNode.Cells.Add(cellMachine);
                advTreeRecordHistoryAgent.Nodes.AddRange(new DevComponents.AdvTree.Node[] { nNode });
                //advTreeRecordHistory.Nodes.Add(nNode);
            }

            if (dtCompanyContact_Log.Rows.Count > 0)
            {
                //string sCompanyColumnsToIgnore = string.Empty;
                //string sContactColumnsToIgnore = string.Empty;

                //DataRow[] drrComIgnore_Columns = dtPicklist.Select("PicklistCategory = 'COMPANY_LOG_IGNORE_COLUMNS'");
                //DataRow[] drrConIgnore_Columns = dtPicklist.Select("PicklistCategory = 'CONTACT_LOG_IGNORE_COLUMNS'");

                //if (drrComIgnore_Columns.Length > 0)
                //    sCompanyColumnsToIgnore = " AND FIELDNAME NOT IN(" + GM.ColumnToQString("PicklistValue", drrComIgnore_Columns.CopyToDataTable(), "String") + ")";

                //if (drrConIgnore_Columns.Length > 0)
                //    sContactColumnsToIgnore = " AND FIELDNAME NOT IN(" + GM.ColumnToQString("PicklistValue", drrConIgnore_Columns.CopyToDataTable(), "String") + ")";
                

                foreach (DevComponents.AdvTree.Node NodeLog in advTreeRecordHistoryAgent.Nodes)
                {
                    if (NodeLog.Name.Length > 0 && dtCompanyContact_Log.Select("CompanySessionID = '" + NodeLog.Name + "'").Length > 0)
                    {
                        DataRow[] drrLog_Companies = dtCompanyContact_Log.Select("CompanySessionID = '" + NodeLog.Name + "' AND TableName = 'MasterCompanies'");
                        DataRow[] drrLog_Contacts = dtCompanyContact_Log.Select("CompanySessionID = '" + NodeLog.Name + "' AND TableName = 'MasterContact'");

                        if (drrLog_Companies.Length > 0)
                            NodeLog.Nodes.AddRange(new DevComponents.AdvTree.Node[] { Add_CompanyContact_log("Company", drrLog_Companies, sSearchText) });

                        if (drrLog_Contacts.Length > 0)
                            NodeLog.Nodes.AddRange(new DevComponents.AdvTree.Node[] { Add_CompanyContact_log("Contact", drrLog_Contacts, sSearchText) });
                    }
                }
            }
        }

        DevComponents.AdvTree.Node Add_CompanyContact_log(string sTableName, DataRow[] drrLogData, string sSearchText)
        {
            DevComponents.AdvTree.Node nNode_Parant = new DevComponents.AdvTree.Node();            
            if (sTableName == "Company")
            {                
                nNode_Parant.Text = sTableName;
                DevComponents.AdvTree.Node nNode_Old = new DevComponents.AdvTree.Node();
                DevComponents.AdvTree.Node nNode_New = new DevComponents.AdvTree.Node();

                for (int i = 0; i < drrLogData.Length; i++)
                {

                    DevComponents.AdvTree.ColumnHeader ColHeader = new DevComponents.AdvTree.ColumnHeader();
                    ColHeader.SortingEnabled = false;
                    ColHeader.Editable = false;
                    ColHeader.Text = drrLogData[i]["FIELDNAME"].ToString().ToUpper();
                    ColHeader.Width.AutoSize = true;
                    ColHeader.Width.AutoSizeMinHeader = true;
                    nNode_Parant.NodesColumns.Add(ColHeader);
                    
                    //cell.Text = drLog_Companies["FIELDNAME"].ToString();
                    if (i > 0)
                    {
                        DevComponents.AdvTree.Cell cell_oldValue = new DevComponents.AdvTree.Cell();                        
                        cell_oldValue.Text = EmptyandHighlight(drrLogData[i]["OLDVALUE"].ToString(), sSearchText);
                        nNode_Old.Cells.Add(cell_oldValue);
                        DevComponents.AdvTree.Cell cell_NewValue = new DevComponents.AdvTree.Cell();
                        cell_NewValue.Text = EmptyandHighlight(drrLogData[i]["NEWVALUE"].ToString(), sSearchText);
                        nNode_New.Cells.Add(cell_NewValue);
                    }
                    else
                    {
                        nNode_Old.Text = EmptyandHighlight(drrLogData[i]["OLDVALUE"].ToString(), sSearchText);
                        nNode_New.Text = EmptyandHighlight(drrLogData[i]["NEWVALUE"].ToString(), sSearchText);
                    }
                    //    NodeLog.Nodes.Add(nNode_Companies);
                }
                nNode_Parant.Nodes.AddRange(new DevComponents.AdvTree.Node[] { nNode_Old, nNode_New });
            }
            else if (sTableName == "Contact")
            {
                nNode_Parant.Text = sTableName;
                

                List<int> lstLog_ContactIDs = new List<int>();
                foreach(DataRow drLogData in drrLogData)                
                    lstLog_ContactIDs.Add(Convert.ToInt32(drLogData["RECORDID"]));

                lstLog_ContactIDs = lstLog_ContactIDs.Distinct().ToList();
                lstLog_ContactIDs.Sort();
                DataTable dtLogData = drrLogData.CopyToDataTable();
                foreach (int ID in lstLog_ContactIDs)
                {
                    DevComponents.AdvTree.Node nNode_ContactID = new DevComponents.AdvTree.Node();
                    DevComponents.AdvTree.Node nNode_Old = new DevComponents.AdvTree.Node();
                    DevComponents.AdvTree.Node nNode_New = new DevComponents.AdvTree.Node();
                    nNode_ContactID.Text = ID.ToString();
                    DataRow[] drrContactLog = dtLogData.Select("RECORDID = '" + ID + "'");

                    for (int i = 0; i < drrContactLog.Length; i++)
                    {
                        DevComponents.AdvTree.ColumnHeader ColHeader = new DevComponents.AdvTree.ColumnHeader();
                        ColHeader.SortingEnabled = false;
                        ColHeader.Editable = false;
                        ColHeader.Text = drrContactLog[i]["FIELDNAME"].ToString().ToUpper();
                        ColHeader.Width.AutoSize = true;
                        ColHeader.Width.AutoSizeMinHeader = true;
                        nNode_ContactID.NodesColumns.Add(ColHeader);

                        //cell.Text = drLog_Companies["FIELDNAME"].ToString();
                        if (i > 0)
                        {
                            DevComponents.AdvTree.Cell cell_oldValue = new DevComponents.AdvTree.Cell();
                            cell_oldValue.Text = EmptyandHighlight(drrContactLog[i]["OLDVALUE"].ToString(), sSearchText);
                            nNode_Old.Cells.Add(cell_oldValue);

                            DevComponents.AdvTree.Cell cell_NewValue = new DevComponents.AdvTree.Cell();
                            cell_NewValue.Text = EmptyandHighlight(drrContactLog[i]["NEWVALUE"].ToString(), sSearchText);
                            nNode_New.Cells.Add(cell_NewValue);
                        }
                        else
                        {
                            nNode_Old.Text = EmptyandHighlight(drrContactLog[i]["OLDVALUE"].ToString(), sSearchText);
                            nNode_New.Text = EmptyandHighlight(drrContactLog[i]["NEWVALUE"].ToString(), sSearchText);
                        }
                        //    NodeLog.Nodes.Add(nNode_Companies);
                    }
                    nNode_ContactID.Nodes.AddRange(new DevComponents.AdvTree.Node[] { nNode_Old, nNode_New });
                    nNode_Parant.Nodes.AddRange(new DevComponents.AdvTree.Node[] { nNode_ContactID });
                }                
            }
            return nNode_Parant;
        }

        string EmptyandHighlight(string Value, string sSearchText)
        {
            if (Value.Trim().Length > 0)
            {
                if (sSearchText.Length > 0 && Value.ToLower().Contains(sSearchText.ToLower()))
                {
                    return Regex.Replace(Value, sSearchText, "<span bgcolor='#FFFF00'><b>" + Value.Substring(Value.IndexOf(sSearchText, StringComparison.OrdinalIgnoreCase), sSearchText.Length) + "</b></span>", RegexOptions.IgnoreCase);

                    //Value.Replace(sSearchText, "<span bgcolor='#FFFF00'>" + sSearchText + "</span>");

                    //Value.Substring(Value.IndexOf(sSearchText, StringComparison.OrdinalIgnoreCase), sSearchText.Length)
                }
                else
                    return Value.Trim();
            }
            else
                return "(empty)";
        }

        private void txtSearchHistory_TextChanged(object sender, EventArgs e)
        {
            if (txtSearchHistory.Text.Length > 0)
            {
                DataRow[] drrCompanyContact_Log = dtCompanyContact_Log.Select("OLDVALUE LIKE '%" + txtSearchHistory.Text.Replace("'","''") + "%' OR NEWVALUE LIKE '%" + txtSearchHistory.Text.Replace("'", "''") + "%'");
                if (drrCompanyContact_Log.Length > 0)
                    Load_History(dtProjectLog.Select("COMPANYSESSIONID IN (" + GM.ColumnToQString("COMPANYSESSIONID", drrCompanyContact_Log.CopyToDataTable(), "String") + ")").CopyToDataTable(), dtCompanyContact_Log.Select("RECORDID IN(" + GM.ColumnToQString("RECORDID", drrCompanyContact_Log.CopyToDataTable(), "String") + ")").CopyToDataTable(), txtSearchHistory.Text);
                else
                {
                    Load_History(dtProjectLog, dtCompanyContact_Log, string.Empty);
                    ToastNotification.Show(this, "Search did not return any records.", eToastPosition.TopRight);
                }
            }
            else
                Load_History(dtProjectLog, dtCompanyContact_Log, string.Empty);
        }

        private void tabRecordHistory_Click(object sender, EventArgs e)
        {
            if(!HistoryLoaded)
            {
                List<string> lstCompanySessions = new List<string>();
                dtProjectLog = GV.MYSQL.BAL_ExecuteQueryMySQL(@"SELECT A.RecordID,A.CompanySessionID, A.Who, B.OldValue - A.OldValue AS 'NewContactsAdded',A.NewValue AS 'UserType',`A`.`When` AS 'OpeningTime',B.`When` AS 'ClosingTime',CASE WHEN B.FieldName = 'Closed'  THEN 'Y' ELSE  'N' END  AS 'CloseType',A.SystemName FROM ((SELECT * FROM " + GV.sProjectID + "_log WHERE RecordID = " + sMaster_ID + " AND TableName = 'RecordStatus' AND FieldName = 'Opened') AS A LEFT JOIN(SELECT * FROM " + GV.sProjectID + "_log WHERE RecordID = " + sMaster_ID + " AND TableName = 'RecordStatus' AND FieldName IN('Closed', 'Closed without Saving')) AS B ON A.CompanySessionID = B.CompanySessionID) WHERE A.CompanySessionID IS NOT NULL ORDER BY OpeningTime;");
                foreach (DataRow drProjectLog in dtProjectLog.Rows)
                    lstCompanySessions.Add(drProjectLog["CompanySessionID"].ToString());

                if (lstCompanySessions.Count > 0)
                {
                    dtCompanyContact_Log = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM " + GV.sProjectID + "_log WHERE  CompanySessionID IN (" + GM.ListToQueryString(lstCompanySessions, "String") + ");");

                    string sCompanyColumnsToIgnore = string.Empty;
                    string sContactColumnsToIgnore = string.Empty;

                    DataRow[] drrComIgnore_Columns = dtPicklist.Select("PicklistCategory = 'COMPANY_LOG_IGNORE_COLUMNS'");
                    DataRow[] drrConIgnore_Columns = dtPicklist.Select("PicklistCategory = 'CONTACT_LOG_IGNORE_COLUMNS'");

                    if (drrComIgnore_Columns.Length > 0)
                        sCompanyColumnsToIgnore = " AND FIELDNAME NOT IN(" + GM.ColumnToQString("PicklistValue", drrComIgnore_Columns.CopyToDataTable(), "String") + ")";

                    if (drrConIgnore_Columns.Length > 0)
                        sContactColumnsToIgnore = " AND FIELDNAME NOT IN(" + GM.ColumnToQString("PicklistValue", drrConIgnore_Columns.CopyToDataTable(), "String") + ")";

                    DataRow[] drrCompanyContact_Log = dtCompanyContact_Log.Select("(TABLENAME = 'MasterCompanies'  " + sCompanyColumnsToIgnore + ") OR (TABLENAME = 'MasterContact' " + sContactColumnsToIgnore + ")");
                    if (drrCompanyContact_Log.Length > 0)
                        dtCompanyContact_Log = drrCompanyContact_Log.CopyToDataTable();
                    else
                        dtCompanyContact_Log.Rows.Clear();                    
                }
                else
                    dtCompanyContact_Log.Rows.Clear();

                Load_History(dtProjectLog, dtCompanyContact_Log, string.Empty);

                HistoryLoaded = true;
            }
        }

        int iPhoneNotifierItrate = 0;
        int iInputBoxTimeout = 0;

        public void PhoneNotiier(string sCallStatus)
        {
            tCallNotifier.Enabled = false;
            lblCallEvents.Text = string.Empty;
            iPhoneNotifierItrate = 0;
            switch (sCallStatus)
            {
                case "Connection":
                    lblCallEvents.Text = "Vortex connection failed..!";
                    break;
                case "Calling":
                    lblCallEvents.Text = "Calling";
                    tCallNotifier.Enabled = true;
                    break;
                case "Ringing":
                    lblCallEvents.Text = "Ringing";
                    tCallNotifier.Enabled = true;
                    break;
                case "Answered":
                    lblCallEvents.Text = "Call Answered";
                    tCallNotifier.Enabled = true;

                    txtDialerType.Visible = false;
                    btnMicMute.Visible = true;
                    btnMuteSpeaker.Visible = true;
                    itemContainerDialParant.Refresh();
                    break;
                case "Ended":
                    lblCallEvents.Text = "Call Ended";
                    tCallNotifier.Enabled = true;

                    txtDialerType.Visible = true;
                    btnMicMute.Visible = false;
                    btnMuteSpeaker.Visible = false;
                    itemContainerDialParant.Refresh();

                    if (CallinProgress)
                        btnHangUpButton.RaiseClick();
                    break;
                case "InitVort":
                    lblCallEvents.Text = "Initializing Vortex";
                    tCallNotifier.Enabled = true;
                    break;
                case "VortReady":
                    lblCallEvents.Text = "Vortex Ready...!";
                    break;
            }
        }

        string DotAnimator(string sInput)
        {
            if (sInput.EndsWith("...."))
                return sInput.Replace(".", string.Empty);
            else //if(sInput.EndsWith("...") || sInput.EndsWith("..") || sInput.EndsWith("."))
                return sInput + ".";            
        }

        private void tCallNotifier_Tick(object sender, EventArgs e)
        {
            string sCallStatus = lblCallEvents.Text;
            if (sCallStatus.Contains("Calling") || sCallStatus.Contains("Ringing") || sCallStatus.Contains("On Call") || sCallStatus.Contains("Initializing Vortex"))
            {
                lblCallEvents.Text = DotAnimator(sCallStatus);
                if (sCallStatus.Contains("On Call"))
                {
                    if (txtTotalDials.Enabled == false && txtTotalDials.Caption == "Input")
                    {
                        iInputBoxTimeout++;
                        if (iInputBoxTimeout > 1)
                        {
                            txtTotalDials.Enabled = true;
                            txtTotalDials.Focus();
                            iInputBoxTimeout = 0;
                        }
                    }
                }
            }
            else if (sCallStatus.Contains("Call Answered"))
            {
                iPhoneNotifierItrate++;
                if (iPhoneNotifierItrate > 15)
                {
                    lblCallEvents.Text = "On Call";
                    txtTotalDials.Caption = "Input";
                    txtTotalDials.Text = string.Empty;
                    txtTotalDials.TextBox.ReadOnly = false;
                    txtTotalDials.Refresh();
                    txtTotalDials.TextBox.Refresh();
                    lblCallEvents.Refresh();
                    itemContainerDialerType.Refresh();
                    itemContainerDialParant.Refresh();
                    //lblCallEvents.Parent.Refresh();
                }
            }
            else if (sCallStatus.Contains("Call Ended"))
            {
                txtTotalDials.Caption = "Dials.";
                txtTotalDials.Text = dtDialLogger.Select("RecordingID = 0").Length.ToString();
                txtTotalDials.TextBox.ReadOnly = true;
                txtTotalDials.Enabled = true;
                txtTotalDials.Refresh();
                itemContainerDialerType.Refresh();
                txtTotalDials.TextBox.Refresh();
                iPhoneNotifierItrate++;
                if (iPhoneNotifierItrate > 15)
                {
                    lblCallEvents.Text = string.Empty;
                    tCallNotifier.Enabled = false;
                }
            }
        }

        private void txtTotalDials_TextChanged(object sender, EventArgs e)
        {
            if(txtTotalDials.Caption == "Input" && lblCallEvents.Text.Contains("On Call"))
            {
                string sInput = txtTotalDials.Text;
                if(sInput.Trim().Length > 0)
                {
                    if (Char.IsDigit(sInput[sInput.Length - 1]) || sInput[sInput.Length - 1].ToString() == "#" || sInput[sInput.Length - 1].ToString() == "*")
                    {
                        GV.VorteX.SendTone(sInput[sInput.Length - 1]);
                        iInputBoxTimeout = 0;
                        txtTotalDials.Enabled = false;                            
                    }
                    else
                        txtTotalDials.Text = sInput.Trim().Substring(0, sInput.Length - 1);
                }                
            }
        }

        private void txtTotalDials_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Control)
                e.SuppressKeyPress = true;
        }

        private void btnMicMute_Click(object sender, EventArgs e)
        {
            if (btnMicMute.Tag.ToString() == "UnMute")
            {
                btnMicMute.Tag = "Mute";
                btnMicMute.Image = Properties.Resources.foundation_microphone_flat_circle_white_on_red_32x32;
                GV.VorteX.MuteMic(true);
            }
            else
            {
                btnMicMute.Tag = "UnMute";
                btnMicMute.Image = Properties.Resources.foundation_microphone_flat_circle_white_on_silver_32x32;                
                GV.VorteX.MuteMic(false);
            }
        }

        private void btnSpeakerVolume_Click(object sender, EventArgs e)
        {
            if (btnMuteSpeaker.Tag.ToString() == "UnMute")
            {
                btnMuteSpeaker.Tag = "Mute";
                btnMuteSpeaker.Image = Properties.Resources.foundation_microphone_flat_circle_white_on_silver_32x32___Copy;
                GV.VorteX.MuteSpeaker(true);
            }
            else
            {
                btnMuteSpeaker.Tag = "UnMute";
                btnMuteSpeaker.Image = Properties.Resources.foundation_microphone_flat_circle_white_on_silver_32x32___Copy__2_;                
                GV.VorteX.MuteSpeaker(false);
            }
        }

        private void btnGSpeechRecord_MouseDown(object sender, MouseEventArgs e)
        {
            bNAudioStarted = false;
            if (DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice).Count() > 0)
            {
                if (GV.GSpeech)
                {
                    if (bWorkerNameSayer.IsBusy)
                    {
                        ToastNotification.Show(this, "Busy processing previous speech.", eToastPosition.TopRight);
                        return;
                    }
                    try
                    {
                        sNAudioPath = string.Empty;
                        waveSource = new WaveIn();

                        waveSource.WaveFormat = new WaveFormat(16000, 1);
                        waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                        waveSource.RecordingStopped += new EventHandler<NAudio.Wave.StoppedEventArgs>(waveSource_RecordingStopped);
                        sNAudioPath = AppDomain.CurrentDomain.BaseDirectory + "GSpeech\\" + GV.sProjectID + "_" + GV.sEmployeeNo + "_" + sMaster_ID + "_" + GV.IP.Replace(".", string.Empty).Reverse().Replace("'", "").Replace("-", "") + GM.GetDateTime().ToString("yyMMddHHmmssff") + ".wav";
                        if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "GSpeech"))
                            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "GSpeech");
                        waveFile = new WaveFileWriter(sNAudioPath, waveSource.WaveFormat);
                        waveSource.StartRecording();
                        bNAudioStarted = true;
                    }
                    catch (Exception ex)
                    {
                        ToastNotification.Show(this, "Problem with recording device.", eToastPosition.TopRight);
                        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                    }
                }
                else
                    ToastNotification.Show(this, "Speech server not initialized.", eToastPosition.TopRight);
            }
            else
                ToastNotification.Show(this, "No Audio device found.", eToastPosition.TopRight);
        }

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }

        void waveSource_RecordingStopped(object sender, NAudio.Wave.StoppedEventArgs e)
        {
            try
            {
                if (bNAudioStarted)
                {
                    if (waveSource != null)
                    {
                        waveSource.Dispose();
                        waveSource = null;
                    }

                    if (waveFile != null)
                    {
                        waveFile.Dispose();
                        waveFile = null;
                    }

                    // System.Threading.Thread.Sleep(3000);

                    if (bWorkerNameSayer.IsBusy)
                        this.Invoke((MethodInvoker)delegate { ToastNotification.Show(this, "Busy processing previous request.", eToastPosition.TopRight); });
                    else
                    {
                        sRequestType = "GS";
                        iRequestWaitTime = 60;
                        iESPTimertick = 0;
                        timerESRequest.Start();
                        bWorkerNameSayer.RunWorkerAsync();
                    }

                        //bWorkerGSpeech.RunWorkerAsync();
                    //StartBtn.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
            }
        }

        void GSpeechTranscribe()
        {
            try
            {
                if (GV.IsWindowsXP)
                {
                    string sNAudioNetworkPath = string.Empty;
                    if (File.Exists(sNAudioPath))
                    {
                        sNAudioNetworkPath = @"\\172.27.137.182\Campaign Manager\GSpeech\" + Path.GetFileName(sNAudioPath);
                        File.Copy(sNAudioPath, sNAudioNetworkPath);                        
                    }
                    if (File.Exists(sNAudioNetworkPath))
                    {
                        string sDataToSend = "GSQuery[:|:]" + GV.IP + "[:|:]" + sNAudioNetworkPath + "[:|:]" + GV.ESPort;                        
                        Random Rand = new Random();
                        int iPort = Rand.Next(2000, 9000);//Max 65535
                        using (System.Net.Sockets.UdpClient sender1 = new System.Net.Sockets.UdpClient(iPort))
                        {
                            List<string> lstMachine = GetMachine();
                            if (lstMachine.Count > 0)
                            {
                                sCommentText = string.Empty;
                                sCommentError = string.Empty;
                                sCurrentAudioPath = sNAudioNetworkPath;
                                sender1.Send(Encoding.ASCII.GetBytes(sDataToSend), sDataToSend.Length, lstMachine[0], Convert.ToInt32(lstMachine[1]));
                               // MessageBox.Show("RequestSent");
                                int iStart = iESPTimertick;
                                while (sCommentError.Length == 0)//Wait till result arrives
                                {
                                    if ((iESPTimertick - iStart) > iRequestWaitTime)
                                    {
                                        //sCommentText = string.Empty;
                                        //sCommentError = string.Empty;
                                        sCurrentAudioPath = string.Empty;
                                        return;
                                    }
                                }
                               // MessageBox.Show(sCommentText);
                                if (sCommentText.Length > 0 && sCommentError != "Error")
                                {
                                   // MessageBox.Show("Error");
                                    TR_COMMENTS.Invoke((MethodInvoker)delegate { TR_COMMENTS.Text = sCommentText; });
                                }
                                //else
                                // Invoke((MethodInvoker)delegate { ToastNotification.Show(this, "Service machine not responding.", eToastPosition.TopRight); });
                                sCurrentAudioPath = string.Empty;
                            }
                        }
                    }
                }
                else
                {
                    var request = new Google.Apis.CloudSpeechAPI.v1beta1.Data.SyncRecognizeRequest()
                    {
                        Config = new Google.Apis.CloudSpeechAPI.v1beta1.Data.RecognitionConfig()
                        {
                            Encoding = "LINEAR16",
                            SampleRate = 16000,
                            LanguageCode = "en-IN"
                        },
                        Audio = new Google.Apis.CloudSpeechAPI.v1beta1.Data.RecognitionAudio()
                        {
                            Content = Convert.ToBase64String(File.ReadAllBytes(sNAudioPath))
                        }
                    };

                    

                    var response = GV.GSpeechCloudAPI.Speech.Syncrecognize(request).Execute();
                    string sOut = string.Empty;
                    foreach (var result in response.Results)
                    {
                        foreach (var alternative in result.Alternatives)
                            sOut += alternative.Transcript;
                    }


                    if (sOut.Length > 0)
                        TR_COMMENTS.Invoke((MethodInvoker)delegate { TR_COMMENTS.Text = sOut; });
                    try
                    {
                        GV.MYSQL.BAL_ExecuteNonReturnQueryMySQL("INSERT INTO c_gspeech_log (SessionID,CompanySessionID,ProjectID,AudioFileName,RecognitionText,Who,`When`) VALUES('" + GV.sSessionID + "','" + GV.sCompanySessionID + "','" + GV.sProjectID + "','" + Path.GetFileName(sNAudioPath) + "','" + sOut.Replace("'", "''") + "','" + GV.sEmployeeName.Replace("'", "''") + "',NOW());");                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Insert: " + ex.Message);
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Google : " + ex.Message);
                ToastNotification.Show(this, "Speech recognition error.", eToastPosition.TopRight);
            }
        }

        private void btnGSpeechRecord_MouseUp(object sender, MouseEventArgs e)
        {
            if (bNAudioStarted)
            {
                waveSource.StopRecording();
                circularProgressGSpeech.IsRunning = true;
            }
        }

        private void bWorkerGSpeech_DoWork(object sender, DoWorkEventArgs e)
        {            
            //btnGSpeechRecord.Invoke((MethodInvoker)delegate { btnGSpeechRecord.Visible = false; });            
            //circularProgressGSpeech.Invoke((MethodInvoker)delegate { circularProgressGSpeech.Visible = true; });
            //GSpeechTranscribe();
        }

        private void bWorkerGSpeech_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //btnGSpeechRecord.Invoke((MethodInvoker)delegate { btnGSpeechRecord.Visible = true; });
            //circularProgressGSpeech.Invoke((MethodInvoker)delegate { circularProgressGSpeech.IsRunning = false; });
            //circularProgressGSpeech.Invoke((MethodInvoker)delegate { circularProgressGSpeech.Visible = false; });
        }

        private void btnenIN_Click(object sender, EventArgs e)
        {
            btnenIN.Checked = true;
            btnenUK.Checked = false;
            btnenUS.Checked = false;
        }

        private void btnenUK_Click(object sender, EventArgs e)
        {
            btnenIN.Checked = false;
            btnenUK.Checked = true;
            btnenUS.Checked = false;
        }

        private void btnenUS_Click(object sender, EventArgs e)
        {
            btnenIN.Checked = false;
            btnenUK.Checked = false;
            btnenUS.Checked = true;
        }
    }

    static class StringExtensions
    {
        public static string Reverse(this string input)
        {
            return new string(input.ToCharArray().Reverse().ToArray());
        }
    }
}

