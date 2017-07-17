using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using i00SpellCheck;
using VortexDial;

namespace GCC
{
    public class GV //Global Veriables
    {
        public static Stopwatch PerformanceWatch = new Stopwatch();
        public static string sPerformance = string.Empty;
        public static string sProjectName = string.Empty;
        public static string sClientName = string.Empty;
        public static string sProjectID = string.Empty;
        public static string sDashBoardID = string.Empty;
        public static string sEmployeeName = string.Empty;
        public static string sEmployeePassword = string.Empty;  
        public static string sEmployeeNo = string.Empty;
        public static string sCompanyTable = string.Empty;
        public static string sContactTable = string.Empty;
        public static string sQCTable = string.Empty;
        public static string sUserType = string.Empty;
        public static string sAccessTo = string.Empty;
        public static string sOppositAccess = string.Empty;
        public static string sAllowTelephoneFormating = string.Empty;
        public static string sAllowGeneralEmail = string.Empty;
        public static string sAllowDuplicateEmail = string.Empty;
        public static string sAllowDuplicateJobTitle = string.Empty;
        public static string sAllowPublicDomainEmails = string.Empty;
        public static string sAllowDuplicateEmailAcrossProject = string.Empty;
        public static string sEmployeeActualName = string.Empty;
        public static string sAllowNewCompanyTR = string.Empty;
        public static string sAllowNewCompanyWR = string.Empty;
        public static string sAffFilePAth = string.Empty;
        public static string sDicFilePath = string.Empty;
        public static string sCallScriptPath = string.Empty;
        public static string sBOTPath = string.Empty;
        public static string sEmailCheckBinaryPath = string.Empty;
        public static string sOSHandlerPath = string.Empty;
        public static string sSendKeyBinaryPath = string.Empty;
        public static string sSpellCheckJobTitle = string.Empty;
        public static string sAllowSwitchBoardNumberinContacts = string.Empty;
        public static string sAllowSwitchBoardDupeGroup = string.Empty;
        public static string sExtensionNumber = string.Empty;
        public static string sDialerType = string.Empty;
        public static string sVortexExtension = string.Empty;
        public static bool AllowPopulateFromPAFSearch = false;
        public static string sShowDetailedContact = string.Empty;
        public static string sFreezeWRCompletedRecords = string.Empty;
        public static string sFreezeTRCompletedRecords = string.Empty;
        public static string sFreezeWRCompanyCompletes = string.Empty;
        public static string sFreezeTRCompanyCompletes = string.Empty;
        public static int iMaxValidatedContactCount = 0;
        public static int iMinValidatedContactCountComplets = 0;
        public static int iMinValidatedContactCountPartialComplets = 0;
        public static bool IsCallScriptEnabled = true;
        public static bool DynamicValidator = true;
        public static bool TimeEnabled = true;

        public static bool AudioComments = false;

        public static string sPreviousLoggedProjectID = string.Empty;

        public static string TR_Switchboard_Mandate = string.Empty;
        public static int iPreCallLimit = 0;
        public static int iPostCallLimit = 0;

        public static Vortex VorteX = new Vortex();

        public static List<string> lstShowOnGridMasterCompanies = new List<string>();
        public static List<string> lstShowOnGridMasterContacts = new List<string>();
        public static List<string> lstShowOnCriteriaMasterCompanies = new List<string>();
        public static List<string> lstShowOnCriteriaMasterContacts = new List<string>();

        public static bool Contact_AllowPopulateFromSearch = false;
        public static bool Company_AllowPopulateFromSearch = false;

        public static bool TollFreeBlock = true;

        public static bool NameSayer = false;

        public static Dictionary Dic = Dictionary.DefaultDictionary;
        

        public static DevComponents.DotNetBar.PanelEx pnlGlobalColor = new DevComponents.DotNetBar.PanelEx();

        public static string sChromeExtensionID = string.Empty;

        public static int iAutoSave_Intervel = 60000;

        public static List<string> lstContactStatusToBeFreezed = new List<string>();
        
        
        public static List<string> TR_lstDisposalsToBeFreezed = new List<string>();
        public static List<string> WR_lstDisposalsToBeFreezed = new List<string>();
        
        public static List<string> TR_lstDisposalsToBeValidated = new List<string>();
        public static List<string> WR_lstDisposalsToBeValidated = new List<string>();



        public static List<string> lstEmailChackContactStatus = new List<string>();

        public static List<string> lstTRContactStatusToBeValidated = new List<string>();
        public static List<string> lstWRContactStatusToBeValidated = new List<string>();

        public static List<string> lstTR_DeleteStatus = new List<string>();
        public static List<string> lstWR_DeleteStatus = new List<string>();

        

        public static List<string> lstNewRecordContactStatus = new List<string>();
        public static List<string> lstUnchangedRecordContactStatus = new List<string>();
        public static List<string> lstChangedRecordContactStatus = new List<string>();
        public static List<string> lstReplacementRecordContactStatus = new List<string>();
        public static List<string> lstReplacementOptionRecordContactStatus = new List<string>();

        public static System.Data.DataTable dtMailConfig = new System.Data.DataTable();
        
        public static List<string> lstNeutralContactStatus = new List<string>();

        public static string sEmailCheckContactStatus = string.Empty;
        public static string sReplacementOptionContactStatus = string.Empty;
        public static string sTRContactstatusTobeValidated = string.Empty;
        public static string sWRContactstatusTobeValidated = string.Empty;

        public static string sTRContactstatusTobeMailChecked = string.Empty;
        public static string sWRContactstatusTobeMailChecked = string.Empty;

        public static List<string> lstTRContactstatusTobeMailChecked = new List<string>();
        public static List<string> lstWRContactstatusTobeMailChecked = new List<string>();

        public static string sSQLCECompanyTable = string.Empty;
        public static string sSQLCEContactTable = string.Empty;

        public static bool Unfreeze_SameDay = false;

        public static string sMachineID = string.Empty;
        //public static string sEAFLibararyPath = string.Empty;

        public static List<string> lstSortableContactColumn = new List<string>();
        public static string sDialerExt = string.Empty;
               
//      public static string sMySQhjL = Connection.Connection.Getstring("CM_MYjhSQL");
        public static string sMySQL = "";


       // public static string sVFD = Connection.Connection.Getstring("VFD_Audit");

        public static string sMSSQL1 = Connection.Connection.Getstring("CM1");
        public static string sMSSQL = Connection.Connection.Getstring("CM2");
        public static string sMSSQL_RM = Connection.Connection.Getstring("CM3");
        //public static string sMSSQL1 = "user id=sa;password=Merit123;data source=CH1020BDSM02;initial catalog=MVC_latest;Application Name=Campaign Manager;";


        public static SqlConnection conMSSQL = new SqlConnection(sMSSQL);

        public static SqlConnection conMSSQL1 = new SqlConnection(sMSSQL1);

        public static SqlConnection conMSSQL_RM = new SqlConnection(sMSSQL_RM);


        // public static MyShqlConnection conMYjSQL = new MhjySqlConnection(Connection.Connection.Getstring("CM_MYhjSQL"));

        //public static MyShjqlConnection conMYhjSQLReader = new MyhjSqlConnection(Connection.Connection.Getstring("CM_MYShjQL"));


        //public static SqlCeConnection conSQLCE = new SqlCeConnection("Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\Backup.sdf;Persist Security Info=False;");

        public static BAL.BAL_GlobalMSSQL MSSQL = new BAL.BAL_GlobalMSSQL();
        public static BAL.BAL_GlobalMSSQL1 MSSQL1 = new BAL.BAL_GlobalMSSQL1();
        public static BAL.BAL_GlobalMSSQL_RM MSSQL_RM = new BAL.BAL_GlobalMSSQL_RM();
        //public static BAL.BAL_GlobalMyfgSQL MYfgSQL = new BAL.BAL_GlobalMyfgSQL();
        //public static BAL.BAL_GobalSQLCE SQLCE = new BAL.BAL_GobalSQLCE();

        public static Image imgEmployeeImage = null;
        public static Image imgOverAllTopper = null;
        public static Image imgProjectTopper = null;
        public static bool IsIDOpen_TR = false;
        public static bool IsIDOpen_WR = false;
        public static DataTable dtErrorMap;


        public static bool bbg_EmailValidation = false;
        public static int ibg_LoadCount = 5;
        public static Int32 ibg_CheckTimeoutPerEmail = 20000;
        public static string sbg_API = string.Empty;
        public static Int32 ibg_BatchExpiry = 200000;
        public static Int32 ibg_Interval = 60000;

        public static bool IsWindowsXP = false;

        public static string ESPort = string.Empty;

        public static string sCompany_View = string.Empty;
        
        public static string sContact_View = string.Empty;
        public static bool bUseContactView_EmailDupe = false;
        public static bool Override_UserAccess = false;
        public static bool Override_QCAccess = false;
        public static bool Override_ManagerAccess = false;
        public static bool Allow_External_Search = false;
        public static bool Update_Blank_NotVerified = false;
        public static string sValidationMessage = string.Empty;

        public static List<string> lstNameSplit = new List<string>() { "van", "von", "de", "du", "di", "le", "da" };

        public static string sScreenAddonPath = string.Empty;

        public static string IP = GM.IP();

        public static bool RDP = false;

        public static bool GSpeech = false;        

        public static string sSoftwareVersion = string.Empty;

        public static int[] iNotifier = new int[7];

        public static bool HasAdminPermission = false;

        public static bool IsApplicationinBeta = false;
        public static bool IsApplicationinAlpha = false;

        public static bool ExcludeEmailBounceInCompleteContactCount = false;


        public static string sSessionID = string.Empty;
        public static string sCompanySessionID = string.Empty;

        public static string sCurrentCaptureName = string.Empty;
        public static string sErrorCaptureName = string.Empty;

        public static bool SearchOnlyCompletedContacts = false;

        public static string sCurrentCompanyName = string.Empty;
        public static string sCurrentCompanyID = string.Empty;

        public static DateTime? dBlockTableUpdateTime = null;
        
        public static DateTime PickList_LastUpdate = DateTime.Now.AddDays(-1); //Initialize with lastDate

    }
}
