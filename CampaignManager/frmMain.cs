using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using DevComponents.DotNetBar;
using System.Data.OleDb;
using System.Deployment.Application;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
//using System.ServiceProcess;
using System.Text.RegularExpressions;
using CustomUIControls;
using DataTable = System.Data.DataTable;
using Size = System.Drawing.Size;

//using Microsoft.Expression.Encoder.ScreenCapture;

namespace GCC
{
    //-----------------------------------------------------------------------------------------------------
    public partial class frmMain : Office2007Form
    {
        //BAL.BAL_Global objBAL_Global = new BAL.BAL_Global();
        //BAL.BAL_GlobalMydSQL objBAL_GlobalMydSQL = new BAL.BAL_GlobalMdySQL();

        RegistryKey reg = default(RegistryKey);

        DataTable dtProjectSettings;
        DataTable dtAllUsers;
        DataTable dtGCCUsers;
        DataTable dtProjectMaster;
        DataTable dtFieldMaster;
        DataTable dtRecordStatus;
        DataTable dtSession;
        public ChromeConnect cConnect;


        public string sAppTitle;
        public string sProcessName = string.Empty;

        public DateTime dApplicationFocusTime;
        public TimeSpan tApplicationFocusIntervel;

        string sPreviousAppTitle = string.Empty;
        string sPreviousProcessName = string.Empty;
        string sPreviousCompanySessionID = string.Empty;
        int iTimer = 0;

        frmCompanyList objFrmCompanyList;
        FrmContactsUpdate objfrmContactsUpdate;
        frmTarget objfrmTarget;
        frmTeamPerformance objfrmTeamPerformance;
        frmQC objfrmQC;
        private bool m_ColorSelected = false;
        private eStyle m_BaseStyle = eStyle.Office2010Silver;
        bool CanClose = false;
        bool IsLoading = false;
        TaskbarNotifier tNotifier;

        //ScreenCaptureJob sCaptureJob;

        //private int tempHeight = 0, tempWidth = 0;
        //private int FixHeight = 1024, FixWidth = 768;
        //private System.ComponentModel.Container components = null;

        void OnChange(object sender, SqlNotificationEventArgs e)
        {
            MessageBox.Show(e.Source.ToString());
        }




        // unhook static eventhandler when application terminates!

        //Handle event
        static void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            GM.Log(e.Reason.ToString(), "", "", "", "", "", "SystemEvents", "", "", "", "", "");

            if (GV.sMachineID.Length > 0)
                GV.MSSQL1.BAL_ExecuteNonReturnQuery("UPDATE c_machines SET SystemState = '" + e.Reason.ToString() + "' WHERE MachineID = '" + GV.sMachineID + "';");
        }

        //-----------------------------------------------------------------------------------------------------
        public frmMain()
        {

            dtSession = new DataTable();
            dtSession.Columns.Add("SessionID");
            dtSession.Columns.Add("AppTitle");
            dtSession.Columns.Add("AppProcess");
            dtSession.Columns.Add("CompanySessionID");
            dtSession.Columns.Add("TimeTaken");

            tNotifier = new TaskbarNotifier();
            tNotifier.CloseClickable = true;
            tNotifier.TitleClickable = true;
            tNotifier.ContentClickable = false;
            tNotifier.EnableSelectionRectangle = false;
            tNotifier.KeepVisibleOnMousOver = false;    // Added Rev 002
            tNotifier.ReShowOnMouseOver = false;


            //if (Screen.PrimaryScreen.Bounds.Height == 768 && Screen.PrimaryScreen.Bounds.Width == 1024)
            //{
            //    //MessageBox.Show("Resolution is going to change to " + FixHeight.ToString() + " X " + FixWidth.ToString());
            //    try
            //    {
            //        GCC.CResolution ChangeRes = new GCC.CResolution(1152, 864);
            //    }
            //    catch (Exception ex)
            //    {
            //        GCC.CResolution ChangeRes = new GCC.CResolution(1280, 720);
            //    }
            //}



            if (ApplicationDeployment.IsNetworkDeployed)
            {
                // Get the installation path of the current application.
                string sAppDomain = System.AppDomain.CurrentDomain.ApplicationIdentity.FullName;
                Uri uriDeployment = new Uri(sAppDomain.Substring(0, sAppDomain.IndexOf("#")));
                string sDeploymentPath = uriDeployment.LocalPath;
                GV.IsApplicationinBeta = sDeploymentPath.ToLower().Contains("campaign manager beta");
                GV.IsApplicationinAlpha = sDeploymentPath.ToLower().Contains("campaign manager alpha");
                ApplicationDeployment appDeployment = ApplicationDeployment.CurrentDeployment;
                GV.sSoftwareVersion = Application.ProductName + " v" + appDeployment.CurrentVersion.Major + "." + appDeployment.CurrentVersion.Minor + "." + appDeployment.CurrentVersion.Build + " Build " + appDeployment.CurrentVersion.Revision;
            }
            else if (Process.GetCurrentProcess().ProcessName.ToLower().EndsWith(".vshost"))            
                GV.sSoftwareVersion = "Campaign Manager Dev";
            else
            {
                MessageBoxEx.EnableGlass = false;
                ribbonMain.Enabled = false;
                MessageBoxEx.Show("Campaign Manager cannot connect to update server.<br/>Please re-install Campaign Manager.<br/>If you encounter errors in installation, then uninstall all the versions of Campaign Manager first.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Process.Start(@"\\172.27.137.180\Merit Software Deployment\Voice\Campaign Manager\Campaign Manager.application");
                Process.GetCurrentProcess().Kill();
                
                return;
            }


            GV.dtErrorMap = GV.MSSQL1.BAL_FetchTable("c_picklists", "PicklistCategory = 'ErrorMap'"); //initilize error handling
            GV.IsWindowsXP = JCS.OSVersionInfo.VersionString.StartsWith("5.1");

           

           

            using (DataTable dtIP = GV.MSSQL1.BAL_ExecuteQuery("SELECT MachineID FROM c_machines WHERE IP = '" + GV.IP + "'"))
            {
                if (dtIP.Rows.Count > 0)
                {
                    GV.sMachineID = dtIP.Rows[0][0].ToString();
                    GV.MSSQL1.BAL_ExecuteNonReturnQuery("UPDATE c_machines SET SystemState = '',ESPort = '', ESState = -1 WHERE MachineID = '" + GV.sMachineID + "'");
                }
                else
                    GV.sMachineID = GV.MSSQL1.BAL_InsertAndGetIdentity("INSERT INTO c_machines (HostName,IP,ESState,ESPort) VALUES('" + Environment.MachineName.Replace("'", "''") + "','" + GV.IP + "',-1,'');");
            }
            GV.sScreenAddonPath = WriteFile("ScreenAddon.exe", Properties.Resources.ScreenAddon);


            //SqlDependency.Stop("user id=USerUD;password=DummyPWD;data source=Server;initial catalog=MkVC;Application Name=Campaign Manager;");
            //SqlDependency.Start("user id=USerUD;password=DummyPWD;data source=Server;initial catalog=MVkC;Application Name=Campaign Manager;");
            //SqlConnection con = new SqlConnection("user id=USerUD;password=DummyPWD;data source=Server;initial catalog=MVkC;Application Name=Campaign Manager;");
            //SqlCommand cmd = new SqlCommand("SELECT * FROM EMoniter", con);
            //cmd.Notification = null;
            //con.Open();
            //SqlDependency dependency = new SqlDependency(cmd);
            //dependency.OnChange += OnChange;
            //return;

            if (Environment.MachineName.ToUpper() != "MSSPLDET281")
                Splash();

            InitializeComponent();
            GM.MailSettings("UserName");//Just initialize the mail settings table
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); //Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
            MessageBoxEx.EnableGlass = false;
            swhType.Visible = false;

            //MessageBoxEx.Show("Outside Impersonation :" + WindowsIdentity.GetCurrent().Name);
            //using (new Impersonation("meritgroup", "satheeshk", "Bommiii989898"))
            //{
            //    MessageBoxEx.Show("Inside Impersonation :" + WindowsIdentity.GetCurrent().Name);
            //}

            GV.pnlGlobalColor.CanvasColor = System.Drawing.SystemColors.Control;
            GV.pnlGlobalColor.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            GV.pnlGlobalColor.DisabledBackColor = System.Drawing.Color.Empty;
            GV.pnlGlobalColor.Size = new System.Drawing.Size(200, 100);
            GV.pnlGlobalColor.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            GV.pnlGlobalColor.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            GV.pnlGlobalColor.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            GV.pnlGlobalColor.Style.GradientAngle = 90;


        

            try
            {
                // MessageBoxEx.Show("Current user: " + WindowsIdentity.GetCurrent().Name);
                //  WrapperImpersonationContext context = new WrapperImpersonationContext("meritgroup", "software", "s)lu7!on123");
                //context.Enter();

                //TellExpressionEncoderWhereItIs();

                //MessageBoxEx.Show("Current user: " + WindowsIdentity.GetCurrent().Name);
                //context.Leave();
                // MessageBoxEx.Show("Current user: " + WindowsIdentity.GetCurrent().Name);


                //Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.wmv").ToList().ForEach(x => File.Delete(x));

                //sCaptureJob = new ScreenCaptureJob();
                ////Rectangle _screenRectangle = Screen.PrimaryScreen.Bounds;
                //sCaptureJob.CaptureRectangle = Screen.PrimaryScreen.Bounds;
                //sCaptureJob.ShowFlashingBoundary = true;
                //sCaptureJob.ScreenCaptureVideoProfile.FrameRate = 20;
                //sCaptureJob.CaptureMouseCursor = true;
                //sCaptureJob.ScreenCaptureVideoProfile.Quality = 50;
                ////sCaptureJob.ScreenCaptureVideoProfile.Bitrate = new Microsoft.Expression.Encoder.Profiles.ConstantBitrate(2000);// VariableUnconstrainedBitrate(1000); 
            }
            catch (Exception exx)
            { }

            if (GV.IsApplicationinBeta)
                toolStripStatuslblVersion.Text = GV.sSoftwareVersion + " <font color ='red'>Beta</font>";
            else if (GV.IsApplicationinAlpha)
                toolStripStatuslblVersion.Text = GV.sSoftwareVersion + " <font color ='red'>Alpha</font>";
            else
                toolStripStatuslblVersion.Text = GV.sSoftwareVersion;
            //toolStripStatuslblVersion.Text = Application.ProductName;

        }

        public void Splash()
        {
            Application.Run(new frmSplash());
        }

        static string RemoveDiacritics(string text)
        {
            return string.Concat(text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                              UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
        }

        //-----------------------------------------------------------------------------------------------------
        private void frmMain_Load(object sender, EventArgs e)
        {

            //frmCompanyImport obj = new frmCompanyImport();
            //obj.WindowState = FormWindowState.Maximized;
            //obj.MdiParent = this;
            //obj.Show();
            //JSTest obj = new JSTest();
            //obj.ShowDialog();
            //return;


            //DataTable dt = GV.MSSQL.BAL_ExecuteQuery("SELECT * FROM mgvc..gender_info_2;");

            //foreach (DataRow dr in dt.Rows)
            //{
            //    dr["name"] = RemoveDiacritics(dr["name"].ToString());
            //}

            //GV.MSSQL.BAL_SaveToTable(dt, "gender_info_2", "Update", true);


            //swhType.Visible = false;


            //string sArchitecture = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            // txtPath.Text = AppDomain.CurrentDomain.BaseDirectory;
            //List<string> vars = new List<string>();
            //foreach (System.Collections.DictionaryEntry de in Environment.GetEnvironmentVariables())
            //    vars.Add(de.Key.ToString()+" : "+ de.Value);

            // MessageBoxEx.Show(v.);
            //Load_ImportType();

            // MessageBoxEx.Show(@"This is testing version of Campaign Manager.<br/>Please install stable version from <br/>\\172.27.137.180\Merit Software Deployment\Voice\Campaign Manager", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);




            if (GV.IsApplicationinBeta)
                this.TitleText = "Campaign Manager - <b><font color = 'Red'>Beta</font></b>";

            //switchButtonRejectionType.Visible = false;
            dateRejectionImport.Visible = false;
            controlContainerDate.Visible = false;

            //MessageBoxEx.Show(Environment.StackTrace);


            //con.Open();
            //MydSqlCommand cmd = new MydSqlCommand("Select * from city", con);
            ////SqlCommand cmd = new SqlCommand("Select * from city", con);
            //MydSqlDataAdapter da = new MydSqlDataAdapter(cmd);
            ////SqlDataAdapter da = new SqlDataAdapter(cmd);

            //DataTable dt = new DataTable();
            //dt = obj.BAL_FetchTableMydSQL("ALLOCATION_FILTER", "1=1");

            //obj.BAL_SaveToTableMydSQL(dt.GetChanges(DataRowState.Modified), "city", "Update");



           //GV.IsWindowsXP = true;
            if (GV.IsWindowsXP)
            {
                this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); //Gets the icon for current window
                ToastNotification.DefaultToastGlowColor = eToastGlowColor.Blue;
                ToastNotification.DefaultTimeoutInterval = 100000;
                //ToastNotification.ToastBackColor = Color.DarkSlateGray;
                this.TitleText = "Campaign Manager - <b><font color = 'Red'>Campaign Manager is no more compatible with Windows XP. Please contact System Administrator.</font></b>";
                ribbonMain.Enabled = false;
                ToastNotification.ToastFont = new Font(this.Font.FontFamily, 25);
                MessageBoxEx.EnableGlass = false;
                CanClose = true;
                ToastNotification.Show(this, "Campaign Manager is no more compatible with Windows XP." + Environment.NewLine + "Please contact System Administrator.", eToastPosition.MiddleCenter);
                return;
            }
            

            Process[] runningProcesses = Process.GetProcesses();
            var currentSessionID = Process.GetCurrentProcess().SessionId;
            Process[] pName = (from c in runningProcesses where c.ProcessName.ToLower() == "campaign manager" && c.SessionId == currentSessionID select c).ToArray();

            // Process[] pName = Process.GetProcessesByName("Campaign Manager");
            if (pName.Length > 1)
            {
                //MessageBoxEx.Show("Campaign Manager already opened.");
                MessageBoxEx.Show("Campaign Manager already opened.", "Campaign Manager", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                CanClose = true;
                //#if DEBUG
                //CanClose = false;
                //#endif
                Application.Exit();
            }
            else
            {
                Process[] pLinkedIn = Process.GetProcessesByName("LinkedIN");
                if (pLinkedIn.Length > 0)
                    pLinkedIn[0].Kill(); // Kill if the bot is already running

                Single_Load();
                Refresh_LoadAll(sender, e);

                GV.VorteX.PhoneEventRecieved += VortexEvents;

            }

            ESpeechServer objESpeechServer = new ESpeechServer();

            try
            {
                cConnect = new ChromeConnect();
                cConnect.Start("http://localhost:60024/");
                cConnect.MessageReceived += new ChromeConnect.MessageReceivedHandler(MessageReceived);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);
            }
            // cConnect.ExceptionRaised += new ChromeConnect.ExceptionRaisedHandler(ExceptionRaised);            
        }

        //private void ExceptionRaised(object sender, ExceptionRaisedEventArgs e)
        //{
        //    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  e.Ex,true,true);
        //    //MessageBox.Show(e.Ex.Message);
        //}

        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            MessageBox.Show(e.Text);
        }

        void ResetVariable(string sResetType)
        {
            if (sResetType == "User")
            {
                GV.sEmployeeNo = string.Empty;
                GV.sEmployeeName = string.Empty;
                GV.sEmployeePassword = string.Empty;
                GV.sProjectID = string.Empty;
                GV.sClientName = string.Empty;
                GV.sProjectName = string.Empty;
                GV.sAccessTo = string.Empty;
                GV.sOppositAccess = string.Empty;
                GV.sUserType = string.Empty;
            }
        }

        private void Single_Load()
        {
            //dtAllUsers = GV.MSSQL.BAL_FetchTable("Timesheet..Users", "Active='Y'");//All active time logger users
            //dtProjectSettings = GV.MYsaSQL.BAL_FetchTableMydSQL("C_PROJECT_SETTINGS", "STATUS='ACTIVE'");//Settings of All GCC projects
            //string sActiveProjects = GM.ColumnToQString("PROJECT_ID", dtProjectSettings, "String");
            //dtProjectMaster = GV.MSSQL.BAL_FetchTable("Timesheet..ProjectMaster", "Active='Y' AND PROJECTID IN (" + sActiveProjects + ")");//All active merit projects..Should be filtered
            //dtGCCUsers = GV.MYSasQL.BAL_FetchTableMydSQL("USERS", "1=1");//Admins, Managers, QC and some exceptional agents
        }

        //-----------------------------------------------------------------------------------------------------
        private void Refresh_LoadAll(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    //Create for local machine
                    RegistryKey regShortCut = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\cm.exe");//Run command Shortcut
                    if (regShortCut == null)
                    {
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\cm.exe");
                        RegistryKey regCreateShortCut = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\cm.exe", true);
                        regCreateShortCut.SetValue("", @"\\172.27.137.180\Merit Software Deployment\Voice\Campaign Manager\Campaign Manager.application");
                        regCreateShortCut.SetValue("Path", @"\\172.27.137.180\Merit Software Deployment\Voice\Campaign Manager");
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        //Create for Current user
                        RegistryKey regShortCut1 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\cm.exe");//Run command Shortcut
                        if (regShortCut1 == null)
                        {
                            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\cm.exe");
                            RegistryKey regCreateShortCut = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\cm.exe", true);
                            regCreateShortCut.SetValue("", @"\\172.27.137.180\Merit Software Deployment\Voice\Campaign Manager\Campaign Manager.application");
                            regCreateShortCut.SetValue("Path", @"\\172.27.137.180\Merit Software Deployment\Voice\Campaign Manager");
                        }
                    }
                    catch (Exception ex1) { }
                }

                cmbDialerType.SelectedIndex = 0;

                ribbonMain.SelectedRibbonTabItem = rbnTabLogin;


                Process[] runningProcesses = Process.GetProcesses();
                var currentSessionID = Process.GetCurrentProcess().SessionId;
                Process[] pName = (from c in runningProcesses where c.ProcessName.ToLower() == "timesheetonline" && c.SessionId == currentSessionID select c).ToArray();

                //Process[] pName = Process.GetProcessesByName("TimeSheetOnline");
                reg = Registry.CurrentUser.OpenSubKey("Software\\VB and VBA Program Settings\\Merit\\Time Logger", true); //Get Current Registry Data//
                if (pName.Length == 0)
                {
                    ToastNotification.Show(this, "Please Login your Time Logger", eToastPosition.TopRight);
                    cmbSelectProject.DataSource = null;
                    dtProjectMaster = new DataTable();
                    dtProjectSettings = new DataTable();
                    dtAllUsers = new DataTable();
                    dtGCCUsers = new DataTable();
                    ResetVariable("User");
                    cmbUserType.Text = string.Empty;
                    statuslblAgentName.Text = "Login : Please Login";//Display in Status bar
                    statuslblProject.Text = "Project : Select Project"; //Status bar display
                    statuslblUserType.Text = "User Type : Please Login";
                    statuslblAccessTo.Text = "User Access : Please Login";
                    rbnTabProcess.Visible = false;
                    rbnTabQC.Visible = false;
                    return;
                }

                rbnTabProcess.Visible = false;//Initially before project and agent selection Process Tab is Hidden
                rbnTabQC.Visible = false;

                dtAllUsers = GV.MSSQL.BAL_FetchTable("Timesheet..Users", "Active='Y'");//All active time logger users
                dtProjectSettings = GV.MSSQL1.BAL_FetchTable("C_PROJECT_SETTINGS", "STATUS='ACTIVE'");//Settings of All GCC projects
                string sActiveProjects = GM.ColumnToQString("PROJECT_ID", dtProjectSettings, "String");
                dtProjectMaster = GV.MSSQL.BAL_FetchTable("Timesheet..ProjectMaster", "Active='Y' AND PROJECTID IN (" + sActiveProjects + ")");//All active merit projects..Should be filtered
                dtGCCUsers = GV.MSSQL1.BAL_FetchTable("C_USERS", "1=1");//Admins, Managers, QC and some exceptional agents

                GV.sVortexExtension = string.Empty;
                using (DataTable dtVortex = GV.MSSQL.BAL_ExecuteQuery("SELECT Extension FROM Call_Timesheet..Extensions WHERE IP = '" + GV.IP.Replace("'", "''") + "';"))
                {
                    if (dtVortex != null && dtVortex.Rows.Count > 0 && dtVortex.Rows[0][0].ToString().Trim().Length > 0)
                        GV.sVortexExtension = dtVortex.Rows[0][0].ToString().Trim();
                    else
                        GV.sVortexExtension = "N/A";
                }
                //dtProjectMaster = GV.MSSQL.BAL_FetchTable("Timesheet..ProjectMaster", "Active='Y' AND Department IN ('FTE','Data','GCC')");//All active merit projects..Should be filtered                

                //Project Selection Combobox
                cmbSelectProject.TextChanged -= new EventHandler(cmbSelectProject_TextChanged);
                cmbSelectProject.DataSource = dtProjectMaster;
                cmbSelectProject.TextChanged += new EventHandler(cmbSelectProject_TextChanged);
                cmbSelectProject.DisplayMembers = "ProjectName"; //Column to be visible
                cmbSelectProject.ValueMember = "ProjectName";

                if (reg != null && reg.GetValue("EmployeeNo") != null && reg.GetValue("Agentname") != null && reg.GetValue("ProjectName") != null)
                {
                    GV.sEmployeeNo = reg.GetValue("EmployeeNo").ToString();
                    GV.sEmployeeName = reg.GetValue("Agentname").ToString();
                    GV.sProjectName = reg.GetValue("ProjectName").ToString();
                    if (reg.GetValue("Extension").ToString().Length > 0)
                        GV.sExtensionNumber = reg.GetValue("Extension").ToString();
                    else
                        GV.sExtensionNumber = "N/A";
                    txtExtension.Text = GV.sExtensionNumber;

                    if (GV.sEmployeeNo.Length == 0 || GV.sEmployeeName.Length == 0)
                        btnLogin_Click(sender, e);//Get Proper employee name again
                    else
                    {
                        //Get_Employee_Details(sender, e);
                        Get_Employee_Permission(sender, e);// Gets the Employee Informations
                        if (GV.sProjectName.Length == 0)
                            cmbSelectProject_ButtonCustomClick(sender, e);//Gets the project Name if not avail
                        else
                            Get_Project_Tables(sender, e);// Gets the Project Informations

                        //Get_Project_Details(sender, e); 
                    }
                    ToastNotification.Show(this, "Welcome " + GV.sEmployeeActualName, eToastPosition.TopRight);

                    //labelItem.Width = this.Width - (statuslblAgentName.Width + statuslblProject.Width + labelItem.Width + toolStripStatuslblVersion.Width + 120);
                    cmbSelectProject.Focus();
                }
                else
                {
                    btnLogin_Click(sender, e);
                    cmbSelectProject_ButtonCustomClick(sender, e);
                }




                //GV.sAffFilePAth = WriteFile("en_US.aff", Properties.Resources.en_US);
                //GV.sDicFilePath = WriteFile("en_US1.dic", Properties.Resources.en_US1);
                //GV.sOSHandlerPath = WriteFile("Handle.exe", Properties.Resources.Handle);
                //GV.sEmailCheckBinaryPath = WriteFile("Email_check.exe", Properties.Resources.Email_check);

                //WriteFile("def.def", Properties.Resources.def);
                //WriteFile("dic.dic", Properties.Resources.dic);
                //WriteFile("syn.syn", Properties.Resources.syn);


                ////if (JCS.OSVersionInfo.OSBits.ToString() == "Bit32")
                ////{
                //    WriteFile("Perl520NH940.dll", Properties.Resources.Perl520NH940);
                //    WriteFile("Perl520RT940.dll", Properties.Resources.Perl520RT940);
                ////}
                ////else
                ////{
                //    WriteFile("Perl520NH940_64.dll", Properties.Resources.Perl520NH940);
                //    WriteFile("Perl520RT940_64.dll", Properties.Resources.Perl520RT940);
                ////}

                ////GlobalVariables.sCallScriptPath = WriteDictioneryFile("CallScript.rtf", Properties.Resources.CallScript);
                //GV.sSendKeyBinaryPath = WriteFile("SendKeys.exe", Properties.Resources.SendKeys);


                if (GV.sEmployeeName == "THANGAPRAKASH")
                {

                    //rbnTabDashboard.Visible = true;
                    //cmbSelectProject.SelectedValue = "CRU_Copper";
                    frmProjectUpdates x = new frmProjectUpdates();
                    x.dtProjectUpdates = GV.MSSQL1.BAL_ExecuteQuery("SELECT * FROM c_project_instructions WHERE PROJECTID IN ('ALL','CRUCRU005');");
                    //x.dtProjectUpdates = GV.MYSQsaL.BAL_ExecuteQueryMydSQL("SELECT * FROM mlvc.c_project_instructions WHERE DATE(CREATED_DATE) > ADDDATE(CURDATE(),INTERVAL -31 DAY) AND PROJECTID IN ('ALL','CRUCRU005');");
                    x.MdiParent = this;
                    x.Show();

                }

                SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);

            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        //-----------------------------------------------------------------------------------------------------
        private string WriteFile(string sFileName, Byte[] ByteFile)
        {
            string sPath = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                if (!Directory.Exists(sPath))
                    Directory.CreateDirectory(sPath);
                if (!(File.Exists(sPath + "\\" + sFileName)))
                    File.WriteAllBytes(sPath + "\\" + sFileName, ByteFile);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return sPath + "\\" + sFileName;
        }

        //-----------------------------------------------------------------------------------------------------
        //private void Get_Employee_Details(object sender, EventArgs e)// Gets the Employee Informations
        //{
        //    try
        //    {
        //        GV.sEmployeeNo = reg.GetValue("EmployeeNo").ToString();
        //        GV.sEmployeeName = reg.GetValue("Agentname").ToString();

        //        if (GV.sEmployeeNo.Length == 0 || GV.sEmployeeName.Length == 0)
        //            btnLogin_Click(sender, e);//Get Proper employee name again
        //        else
        //            Get_Employee_Permission(sender, e);//Get employee permission such as Admin, Maager, QC and attribs WR or TR
        //    }
        //    catch (Exception ex)
        //    {
        //        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
        //        ////MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    }
        //}

        //-----------------------------------------------------------------------------------------------------
        private void Get_Employee_Permission(object sender, EventArgs e)
        {
            try
            {
                DataRow[] drrAllUsers = null;
                DataRow[] drrGCCUsers;
                GV.sUserType = string.Empty; //To make sure user type resets when User changed

                IsLoading = true;

                if (dtAllUsers == null)
                    return;
                drrAllUsers = dtAllUsers.Select(String.Format("UserName = '{0}'", GV.sEmployeeName));

                // drrAllUsers = null;

                if (drrAllUsers.Length > 0)
                {
                    GV.sEmployeeActualName = GM.ProperCase(drrAllUsers[0]["FullName"].ToString());
                    GV.sEmployeeNo = drrAllUsers[0]["EmployeeNo"].ToString();//Get Employee Number
                    statuslblAgentName.Text = "Login : " + GV.sEmployeeActualName;//Display in Status bar
                    GV.sEmployeePassword = GM.Decrypt_Password(drrAllUsers[0]["UserPassword"].ToString());

                    DataTable dtImage = GV.MSSQL.BAL_FetchTable("EmployeeImage", "EmployeeID = '" + GV.sEmployeeNo + "'");
                    if (dtImage.Rows.Count > 0)
                        GV.imgEmployeeImage = GM.GetImageFromByte(((byte[])dtImage.Rows[0]["EmployeeImage"]));
                    else
                        GV.imgEmployeeImage = GCC.Properties.Resources.Misc_User_icon__1_;

                    cmbUserType.Items.Clear();
                    cmbUserAccess.Items.Clear();
                    cmbUserType.Items.Add("Agent");
                    cmbUserAccess.Items.Add("Tele Researcher");
                    cmbUserAccess.Items.Add("Web Researcher");

                    if (drrAllUsers[0]["Department"].ToString() == "Data") //Pre Select the Usertype combobox based on the acquired data
                    {
                        GV.sUserType = "Agent";
                        GV.sAccessTo = "WR";
                        GV.sOppositAccess = "TR";

                        cmbUserType.SelectedItem = "Agent";
                        cmbUserAccess.SelectedItem = "Web Researcher";
                    }
                    else if (drrAllUsers[0]["Department"].ToString().ToUpper() == "GCC" || drrAllUsers[0]["Department"].ToString().ToUpper() == "VOICE")
                    {
                        GV.sUserType = "Agent";
                        GV.sAccessTo = "TR";
                        GV.sOppositAccess = "WR";

                        cmbUserType.SelectedItem = "Agent";
                        cmbUserAccess.SelectedItem = "Tele Researcher";
                    }
                    else
                    {
                        cmbUserType.SelectedIndex = -1;
                        cmbUserAccess.SelectedIndex = -1;
                        GV.sUserType = string.Empty;
                        GV.sAccessTo = string.Empty;
                        GV.sOppositAccess = string.Empty;
                    }

                    cmbSelectProject.Enabled = false;
                    cmbUserType.Enabled = false;



                    //////////GCC User details overrides the Timelogger User details/////////////                    
                    drrGCCUsers = dtGCCUsers.Select(String.Format("USERNAME = '{0}'", GV.sEmployeeName));
                    if (drrGCCUsers.Length > 0)
                    {
                        //rbnBarUser.AutoSize = true;
                        GV.sAccessTo = drrGCCUsers[0]["USERACCESS"].ToString();//Get Employee Access
                        if (GV.sAccessTo == "TR")
                            GV.sOppositAccess = "WR";
                        else
                            GV.sOppositAccess = "TR";

                        btnProjectControl.Visible = false;

                        //In here, the user should be Admin or QC.. Or some exceptional agents
                        if (drrGCCUsers[0]["USERTYPE"].ToString().ToUpper() == "ADMIN")//All option visible for admin
                        {
                            GV.sUserType = "Admin";
                            btnProjectControl.Visible = btnProjectControl.Enabled = true;
                            cmbUserType.Items.Add("Manager");
                            cmbUserType.Items.Add("Admin");
                            cmbUserType.Items.Add("QC");
                            cmbUserType.SelectedItem = "Admin";

                            if (drrGCCUsers[0]["USERACCESS"].ToString().ToUpper() == "TR")
                                cmbUserAccess.SelectedItem = "Tele Researcher";
                            else
                                cmbUserAccess.SelectedItem = "Web Researcher";

                            btnUserMaster.Visible = true;
                            btnAllocation.Visible = true;
                            cmbSelectProject.Enabled = true;
                            cmbUserType.Enabled = true;
                            btnSendBackInfo.Visible = true;
                            txtExtension.Enabled = true;

                            pictDialer.Visible = true;
                            cmbDialerType.Visible = true;
                            txtExtension.Visible = true;
                            rbnBarUser.Width = 150;

                        }
                        else if (drrGCCUsers[0]["USERTYPE"].ToString().ToUpper() == "MANAGER")
                        {
                            GV.sUserType = "Manager";
                            cmbUserType.Items.Add("Manager");
                            cmbUserType.SelectedItem = "Manager";

                            if (drrGCCUsers[0]["USERACCESS"].ToString().ToUpper() == "TR")
                                cmbUserAccess.SelectedItem = "Tele Researcher";
                            else
                                cmbUserAccess.SelectedItem = "Web Researcher";

                            btnUserMaster.Visible = true;
                            btnAllocation.Visible = true;
                            cmbSelectProject.Enabled = true;
                            cmbUserType.Enabled = true;
                            txtExtension.Enabled = true;
                            btnSendBackInfo.Visible = true;
                            rbnBarUser.Width = 150;
                        }
                        else if (drrGCCUsers[0]["USERTYPE"].ToString().ToUpper() == "QC")
                        {
                            GV.sUserType = "QC";

                            cmbUserType.Items.Add("QC");
                            cmbUserType.SelectedItem = "QC";

                            if (drrGCCUsers[0]["USERACCESS"].ToString().ToUpper() == "TR")
                                cmbUserAccess.SelectedItem = "Tele Researcher";
                            else
                                cmbUserAccess.SelectedItem = "Web Researcher";

                            btnUserMaster.Visible = false;
                            btnAllocation.Visible = false;
                            cmbSelectProject.Enabled = true;
                            cmbUserType.Enabled = true;
                            btnSendBackInfo.Visible = true;

                            rbnBarUser.Width = 50;

                        }
                        else if (drrGCCUsers[0]["USERTYPE"].ToString().ToUpper() == "AGENT")
                        {
                            //The user is Agent                            
                            cmbUserType.SelectedItem = "Agent";
                            GV.sUserType = "Agent";

                            if (drrGCCUsers[0]["USERACCESS"].ToString().ToUpper() == "TR")
                                cmbUserAccess.SelectedItem = "Tele Researcher";
                            else
                                cmbUserAccess.SelectedItem = "Web Researcher";

                            btnUserMaster.Visible = false;
                            btnAllocation.Visible = false;
                            btnSendBackInfo.Visible = false;
                            rbnBarUser.Width = 50;
                        }

                        //statuslblUserType.Text = "User Type : " + GV.sUserType;
                        //statuslblAccessTo.Text = "User Access : "+GV.sAccessTo;
                    }
                    else
                    {
                        //The user is Agent
                        cmbUserType.TextChanged -= new EventHandler(cmbUserType_TextChanged);
                        cmbUserType.SelectedItem = "Agent";
                        cmbUserType.TextChanged += new EventHandler(cmbUserType_TextChanged);
                        GV.sUserType = "Agent";

                        btnUserMaster.Visible = false;
                        btnAllocation.Visible = false;
                        btnSendBackInfo.Visible = false;

                        rbnBarUser.Width = 50;
                    }

                    statuslblUserType.Text = "User Type : " + GV.sUserType;
                    statuslblAccessTo.Text = "User Access : " + GV.sAccessTo;

                    ShowDialer();
                }
                else
                {
                    ToastNotification.Show(this, "User Not found", eToastPosition.TopRight);
                    ResetVariable("User");
                    statuslblAgentName.Text = "Please Login";
                }

                IsLoading = false;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
            }
        }

        void ShowDialer()
        {
            if (GV.sAccessTo == "TR")
            {
                pictDialer.Visible = true;
                cmbDialerType.Visible = true;
                txtExtension.Visible = true;

                cmbSelectProject.SuspendLayout();
                cmbUserType.SuspendLayout();
                cmbUserAccess.SuspendLayout();
                pictProjectSelection.SuspendLayout();
                pictUserType.SuspendLayout();
                pictUserAccess.SuspendLayout();

                cmbSelectProject.Location = new Point(29, 5);
                cmbUserType.Location = new Point(29, 34);
                cmbUserAccess.Location = new Point(29, 60);
                pictProjectSelection.Location = new Point(2, 2);
                pictUserType.Location = new Point(2, 28);
                pictUserAccess.Location = new Point(2, 56);


                cmbSelectProject.ResumeLayout();
                cmbUserType.ResumeLayout();
                cmbUserAccess.ResumeLayout();
                pictProjectSelection.ResumeLayout();
                pictUserType.ResumeLayout();
                pictUserAccess.ResumeLayout();
            }
            else
            {
                pictDialer.Visible = false;
                cmbDialerType.Visible = false;
                txtExtension.Visible = false;

                cmbSelectProject.SuspendLayout();
                cmbUserType.SuspendLayout();
                cmbUserAccess.SuspendLayout();
                pictProjectSelection.SuspendLayout();
                pictUserType.SuspendLayout();
                pictUserAccess.SuspendLayout();

                cmbSelectProject.Location = new Point(29, 25);
                cmbUserType.Location = new Point(29, 54);
                cmbUserAccess.Location = new Point(29, 80);
                pictProjectSelection.Location = new Point(2, 22);
                pictUserType.Location = new Point(2, 48);
                pictUserAccess.Location = new Point(2, 76);

                cmbSelectProject.ResumeLayout();
                cmbUserType.ResumeLayout();
                cmbUserAccess.ResumeLayout();
                pictProjectSelection.ResumeLayout();
                pictUserType.ResumeLayout();
                pictUserAccess.ResumeLayout();


            }

            if (GV.sUserType == "Agent")
                txtExtension.Enabled = false;
            else if (GV.sUserType == "Admin")
            {
                txtExtension.Enabled = true;
                txtExtension.ReadOnly = false;
                pictDialer.Visible = true;
                cmbDialerType.Visible = true;
                txtExtension.Visible = true;

                cmbSelectProject.SuspendLayout();
                cmbUserType.SuspendLayout();
                cmbUserAccess.SuspendLayout();
                pictProjectSelection.SuspendLayout();
                pictUserType.SuspendLayout();
                pictUserAccess.SuspendLayout();

                cmbSelectProject.Location = new Point(29, 5);
                cmbUserType.Location = new Point(29, 34);
                cmbUserAccess.Location = new Point(29, 60);
                pictProjectSelection.Location = new Point(2, 2);
                pictUserType.Location = new Point(2, 28);
                pictUserAccess.Location = new Point(2, 56);


                cmbSelectProject.ResumeLayout();
                cmbUserType.ResumeLayout();
                cmbUserAccess.ResumeLayout();
                pictProjectSelection.ResumeLayout();
                pictUserType.ResumeLayout();
                pictUserAccess.ResumeLayout();
            }
            else if (GV.sUserType == "Manager")
                txtExtension.ReadOnly = true;
            else
                txtExtension.ReadOnly = false;
        }

        ////-----------------------------------------------------------------------------------------------------
        //private void Get_Project_Details(object sender, EventArgs e)// Gets the Project Informations
        //{
        //    try
        //    {
        //        GV.sProjectName = reg.GetValue("ProjectName").ToString();
        //        //GlobalVariables.sProjectName = null;
        //        if (GV.sProjectName.Length == 0)
        //            cmbSelectProject_ButtonCustomClick(sender, e);//Gets the project Name if not avail
        //        else
        //            Get_Project_Tables(sender, e);
        //    }
        //    catch (Exception ex)
        //    {
        //        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
        //        //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    }
        //}

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

        //-----------------------------------------------------------------------------------------------------
        private void btnContactUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                OpenCompanyList();
                if (GV.sUserType == "Agent")
                {
                    if (IsTargetSet())
                    {
                        if (!GM.IsFormExist("FrmContactsUpdate") && GV.sCompanyTable.Length > 0 && GV.sContactTable.Length > 0)
                        {
                            string sOpenType = string.Empty;
                            if (GM.IsPendingRecordsExist().Rows.Count > 0)
                            {
                                MessageBoxEx.Show("Some contact(s) not saved properly. Opening them now.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                sOpenType = "ListOpen";//Next Record will not come automaticaly
                            }
                            else
                            {
                                ButtonItem btn = sender as ButtonItem;
                                sOpenType = btn.Tag.ToString();//Next record will come automatically                            
                            }

                            objfrmContactsUpdate = new GCC.FrmContactsUpdate(null, this, sOpenType, false, objFrmCompanyList);
                            //objfrmContactsUpdate.MdiParent = this;
                            objfrmContactsUpdate.tabStrip = tabStrip;
                            //objfrmContactsUpdate.Dock = DockStyle.Fill;
                            objfrmContactsUpdate.ResizeEnd += f_ResizeEnd;
                            //objfrmContactsUpdate.IsNewCompany = false;
                            //objfrmContactsUpdate.objfrmCompanyList = objFrmCompanyList;

                            objfrmContactsUpdate.Show();
                        }
                        else
                            txtErrors.Text += "Unexpected Error on fetching Project Table informations";
                    }
                    else
                    {
                        ToastNotification.Show(this, "Today's Target not set.", eToastPosition.TopRight);
                        if (!GM.IsFormExist("frmTarget"))
                        {
                            objfrmTarget = new frmTarget();
                            objfrmTarget.MdiParent = this;
                            objfrmTarget.Show();
                        }
                    }
                }
                else
                    ToastNotification.Show(this, "Non-Agents can't open new record.", eToastPosition.TopRight);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtAllUsers != null && dtAllUsers.Rows.Count > 0)
                {
                    frmComboList objFrmComboList = new frmComboList();
                    objFrmComboList.TitleText = "Select User";
                    objFrmComboList.dtItems = dtAllUsers;
                    objFrmComboList.lstColumnsToDisplay.Add("UserName");
                    objFrmComboList.sColumnToSearch = "UserName";
                    objFrmComboList.IsSpellCheckEnabeld = false;
                    objFrmComboList.IsMultiSelect = false;
                    objFrmComboList.IsSingleWordSelection = true;
                    objFrmComboList.StartPosition = FormStartPosition.CenterScreen;
                    //objFrmComboList.ShowInTaskbar = false;
                    objFrmComboList.MinimizeBox = false;
                    objFrmComboList.MaximizeBox = false;
                    objFrmComboList.ShowDialog(this);
                    if (objFrmComboList.sReturn != null && objFrmComboList.sReturn.Length > 0)
                    {
                        DataRow[] drrUSers = dtAllUsers.Select("UserName = '" + objFrmComboList.sReturn + "' AND Active = 'Y'");
                        frmLogin objLogin = new frmLogin();
                        objLogin.sEmployeeName = objFrmComboList.sReturn;
                        objLogin.sPassword = GM.Decrypt_Password(drrUSers[0]["UserPassword"].ToString());
                        //objLogin.dtUsers = dtAllUsers;
                        objLogin.frmParant = this;
                        objLogin.ShowDialog(this);
                        if (objLogin.IsUserPassed && objFrmComboList.sReturn.Length > 0)
                        {
                            GV.sEmployeeName = objFrmComboList.sReturn;
                            Get_Employee_Permission(sender, e);//Get employee permission such as Admin, Maager, QC and attribs WR or TR                            
                            if (GV.sProjectName.Length == 0)
                                cmbSelectProject_ButtonCustomClick(sender, e);//Gets the project Name if not avail
                            else
                                Get_Project_Tables(sender, e);// Gets the Project Informations

                            ToastNotification.Show(this, "Welcome " + GV.sEmployeeName, eToastPosition.TopRight);
                        }
                        else
                            ToastNotification.Show(this, "Invalid user", eToastPosition.TopRight);
                    }
                    else
                        ToastNotification.Show(this, "Invalid user", eToastPosition.TopRight);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void Get_Project_Tables(object sender, EventArgs e)
        {
            try
            {
                DataRow[] drProjectMaster = null;
                if (dtProjectMaster == null)
                    return;
                if (GV.sProjectName == "(New Project)")
                {
                    //ToastNotification.Show(this, "Open Project Control to Create New Project", eToastPosition.TopRight);
                    GV.sProjectName = string.Empty;
                    GV.sProjectID = string.Empty;
                    GV.sClientName = string.Empty;
                    btnProjectControl.RaiseClick();
                    return;
                }
                drProjectMaster = dtProjectMaster.Select(String.Format("ProjectName = '{0}'", GV.sProjectName));

                if (drProjectMaster.Length > 0)
                {
                    GV.sProjectID = drProjectMaster[0]["ProjectID"].ToString().Replace(" ", string.Empty);
                    GV.sClientName = drProjectMaster[0]["Clientname"].ToString().Trim();
                    statuslblProject.Text = "Project : " + GV.sProjectName; //Status bar display
                    cmbSelectProject.SelectedValue = GV.sProjectName; //Pre Select Project Name
                    DataTable dtDashBoard = GV.MSSQL.BAL_ExecuteQuery("SELECT ID FROM dbo.DASHBOARD WHERE PROJECT_ID='" + GV.sProjectID + "'");
                    if (dtDashBoard.Rows.Count > 0)
                        GV.sDashBoardID = dtDashBoard.Rows[0]["ID"].ToString();
                    else
                        GV.sDashBoardID = string.Empty;
                    //else
                    //{
                    //    btnTarget.Enabled = false;
                    //    btnPerformance.Enabled = false;
                    //}
                }
                //else
                //cmbSelectProject_ButtonCustomClick(sender, e);

                DataRow[] drProjectSettings = dtProjectSettings.Select(String.Format("PROJECT_ID = '{0}'", GV.sProjectID));
                if (drProjectSettings.Length > 0)
                {
                    dtFieldMaster = GV.MSSQL1.BAL_FetchTable("C_FIELD_MASTER", "PROJECT_ID = '" + GV.sProjectID + "'");//Gets all list of columns used(master and master contacts)
                    ProperCaseHelper.dtProperCase = GV.MSSQL1.BAL_ExecuteQuery("SELECT * FROM c_picklists WHERE PicklistCategory = 'Correction'");
                    dtRecordStatus = GV.MSSQL1.BAL_FetchTable(GV.sProjectID + "_RecordStatus", "1=1");//Load Contact status and Disposals
                    GV.sContactTable = drProjectSettings[0]["CONTACTS_TABLE"].ToString().ToUpper();//Mastercontact Table Name
                    GV.sCompanyTable = drProjectSettings[0]["COMPANY_TABLE"].ToString().ToUpper();//Master Table Name
                    GV.sQCTable = GV.sProjectID + "_QC";
                    //GlobalVariables.sFilterViewName = drProjectSettings[0]["FILTER_VIEW"].ToString();//Master Table Name
                    GV.sAllowGeneralEmail = drProjectSettings[0]["ALLOW_GENERAL_EMAIL"].ToString();
                    GV.sAllowDuplicateEmail = drProjectSettings[0]["ALLOW_DUPLICATE_EMAIL"].ToString();
                    GV.sAllowDuplicateJobTitle = drProjectSettings[0]["ALLOW_DUPLICATE_JOBTITLE"].ToString();

                    GV.sAllowPublicDomainEmails = drProjectSettings[0]["ALLOW_PUBLIC_DOMAIN_EMAIL"].ToString();
                    GV.sAllowTelephoneFormating = drProjectSettings[0]["TELEPHONE_FORMATTING"].ToString();
                    GV.sSpellCheckJobTitle = drProjectSettings[0]["SPELLCHECK_JOBTITLE"].ToString();
                    GV.sAllowNewCompanyTR = drProjectSettings[0]["ADD_BUTTON_TR"].ToString();
                    GV.sAllowNewCompanyWR = drProjectSettings[0]["ADD_BUTTON_WR"].ToString();
                    GV.sAllowDuplicateEmailAcrossProject = drProjectSettings[0]["ALLOW_DUPLICATE_EMAIL_ACROSSPROJECT"].ToString();
                    GV.sAllowSwitchBoardNumberinContacts = drProjectSettings[0]["ALLOW_SWITCHBOARD_NUMBER_IN_CONTACT"].ToString();
                    GV.sAllowSwitchBoardDupeGroup = drProjectSettings[0]["ALLOW_SWITCHBOARD_DUPE_GROUP"].ToString();
                    GV.sShowDetailedContact = drProjectSettings[0]["SHOW_DETAILED_CONTACT"].ToString();

                    GV.Contact_AllowPopulateFromSearch = drProjectSettings[0]["CONTACT_ALLOW_POPULATE_FROM_SEARCH"].ToString() == "Y";
                    GV.Company_AllowPopulateFromSearch = drProjectSettings[0]["COMPANY_ALLOW_POPULATE_FROM_SEARCH"].ToString() == "Y";
                    GV.AllowPopulateFromPAFSearch = drProjectSettings[0]["ALLOW_POPULATE_FROM_PAF_SEARCH"].ToString() == "Y";

                    GV.sFreezeWRCompletedRecords = drProjectSettings[0]["FREEZE_WR_COMPLETED_RECORDS"].ToString();
                    GV.sFreezeTRCompletedRecords = drProjectSettings[0]["FREEZE_TR_COMPLETED_RECORDS"].ToString();
                    GV.sFreezeWRCompanyCompletes = drProjectSettings[0]["FREEZE_WR_COMPANY_COMPLETES"].ToString();
                    GV.sFreezeTRCompanyCompletes = drProjectSettings[0]["FREEZE_TR_COMPANY_COMPLETES"].ToString();

                    GV.sCompany_View = drProjectSettings[0]["COMPANY_VIEW"].ToString();
                    GV.sContact_View = drProjectSettings[0]["CONTACT_VIEW"].ToString();
                    GV.bUseContactView_EmailDupe = (drProjectSettings[0]["USE_CONTACT_VIEW_EMAILDUPLICATE"].ToString() == "Y");

                    GV.iMaxValidatedContactCount = Convert.ToInt32(drProjectSettings[0]["MAX_CONTACT_COUNT"]);
                    GV.iMinValidatedContactCountComplets = Convert.ToInt32(drProjectSettings[0]["MIN_CONTACTS_COMPLETE"]);
                    GV.iMinValidatedContactCountPartialComplets = Convert.ToInt32(drProjectSettings[0]["MIN_CONTACTS_PARTIAL_COMPLETE"]);

                    GV.Override_UserAccess = drProjectSettings[0]["OVERRIDE_RECORD_USER_ACCESS"].ToString() == "Y";
                    GV.Override_QCAccess = drProjectSettings[0]["OVERRIDE_RECORD_QC_ACCESS"].ToString() == "Y";
                    GV.Override_ManagerAccess = drProjectSettings[0]["OVERRIDE_RECORD_MANAGER_ACCESS"].ToString() == "Y";
                    GV.Allow_External_Search = drProjectSettings[0]["ALLOW_EXTERNAL_SEARCH"].ToString() == "Y";
                    GV.Update_Blank_NotVerified = drProjectSettings[0]["UPDATE_BLANK_WITH_NOTVERIFIED"].ToString() == "Y";
                    GV.AudioComments = drProjectSettings[0]["AUDIO_COMMENTS"].ToString() == "Y";

                    GV.iPreCallLimit = Convert.ToInt32(drProjectSettings[0]["PRE_CALL_LIMIT"]);
                    GV.iPostCallLimit = Convert.ToInt32(drProjectSettings[0]["POST_CALL_LIMIT"]);


                    GV.lstShowOnGridMasterCompanies = Get_ProjectColumns("Master", "SHOW_ON_GRID");
                    GV.lstShowOnGridMasterContacts = Get_ProjectColumns("MasterContacts", "SHOW_ON_GRID");

                    GV.lstShowOnCriteriaMasterCompanies = Get_ProjectColumns("Master", "SHOW_ON_CRITERIA");
                    GV.lstShowOnCriteriaMasterContacts = Get_ProjectColumns("MasterContacts", "SHOW_ON_CRITERIA");

                    GV.lstSortableContactColumn = Get_ProjectColumns("MasterContacts", "SORTABLE_COLUMN");

                    GV.lstShowOnGridMasterCompanies.Sort();
                    GV.lstShowOnGridMasterContacts.Sort();
                    GV.lstShowOnCriteriaMasterCompanies.Sort();
                    GV.lstShowOnCriteriaMasterContacts.Sort();

                    GV.IsIDOpen_TR = (drProjectSettings[0]["OpenByID_TR"].ToString() == "Y");
                    GV.IsIDOpen_WR = (drProjectSettings[0]["OpenByID_WR"].ToString() == "Y");
                    GV.ExcludeEmailBounceInCompleteContactCount = (drProjectSettings[0]["EXCLUDE_EMAILBOUNCE_IN_COMPLETECONTACT_COUNT"].ToString() == "Y");

                    GV.NameSayer = (GV.sAccessTo == "TR" && drProjectSettings[0]["NAME_SAYER"].ToString() == "Y");
                    GV.TollFreeBlock = (drProjectSettings[0]["ALLOW_TOLLFREE"].ToString() == "N");

                    //GV.Unfreeze_SameDay = (drProjectSettings[0]["SAME_DAY_UNFREEZE_"+GV.sAccessTo].ToString() == "Y");

                    Get_MandatoryList();
                    //if (dtPicklist.Select("PicklistCategory = 'Correction'").Length > 0)
                    //    ProperCaseHelper.dtProperCase = dtPicklist.Select("PicklistCategory = 'Correction'").CopyToDataTable();

                }
                else//If project not exist in GCC
                {
                    MessageBoxEx.EnableGlass = false;
                    //MessageBoxEx.Show("Could not find Project Settings","Caption",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);

                    ToastNotification.Show(this, "Could not find Project Settings", eToastPosition.BottomRight);
                    GV.sContactTable = string.Empty;
                    GV.sCompanyTable = string.Empty;
                    GV.sQCTable = string.Empty;
                    //GlobalVariables.sProjectID = string.Empty;
                    GV.sProjectName = string.Empty;
                    GV.sAllowGeneralEmail = string.Empty;
                    GV.sAllowDuplicateEmail = string.Empty;
                    GV.sAllowDuplicateJobTitle = string.Empty;
                    GV.sAllowPublicDomainEmails = string.Empty;
                    GV.sAllowTelephoneFormating = string.Empty;
                    GV.Contact_AllowPopulateFromSearch = false;
                    GV.Company_AllowPopulateFromSearch = false;
                    GV.TollFreeBlock = true;
                    GV.AllowPopulateFromPAFSearch = false;
                    GV.sSpellCheckJobTitle = string.Empty;
                    GV.sAllowNewCompanyTR = string.Empty;
                    GV.sAllowNewCompanyWR = string.Empty;
                    GV.sSQLCECompanyTable = string.Empty;
                    GV.sSQLCEContactTable = string.Empty;
                    GV.sAllowSwitchBoardDupeGroup = string.Empty;
                    GV.sShowDetailedContact = string.Empty;
                    GV.sAllowSwitchBoardNumberinContacts = string.Empty;
                    GV.sAllowDuplicateEmailAcrossProject = string.Empty;
                    GV.sDashBoardID = string.Empty;
                    cmbSelectProject.SelectedIndex = -1;
                    GV.IsIDOpen_TR = false;
                    GV.IsIDOpen_WR = false;
                    GV.Unfreeze_SameDay = false;
                    GV.sCompany_View = string.Empty;
                    GV.sContact_View = string.Empty;
                    GV.Override_UserAccess = false;
                    GV.Override_QCAccess = false;
                    GV.Override_ManagerAccess = false;
                    GV.Allow_External_Search = false;
                    GV.bUseContactView_EmailDupe = false;
                    GV.iPreCallLimit = 0;
                    GV.iPostCallLimit = 0;
                    GV.ExcludeEmailBounceInCompleteContactCount = false;
                    GV.AudioComments = false;

                    GV.sTRContactstatusTobeValidated = string.Empty;
                    GV.sWRContactstatusTobeValidated = string.Empty;

                    GV.sTRContactstatusTobeMailChecked = string.Empty;
                    GV.sWRContactstatusTobeMailChecked = string.Empty;
                    
                    //cmbSelectProject_ButtonCustomClick(sender, e);


                }

                //double d = 1 / Convert.ToInt16(0);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //string ListToQueryString(List<string> lst, string sDateType)
        //{
        //    string sOut = string.Empty;
        //    foreach (string s in lst)
        //    {
        //        if (sDateType == "String")
        //        {
        //            if (sOut.Length == 0)
        //                sOut = "'" + s + "'";
        //            else
        //                sOut += ",'" + s + "'";
        //        }
        //        else
        //        {
        //            if (sOut.Length == 0)
        //                sOut = s;
        //            else
        //                sOut += "," + s;
        //        }
        //    }
        //    return sOut;
        //}

        //-----------------------------------------------------------------------------------------------------
        private void Get_MandatoryList()
        {
            GV.lstContactStatusToBeFreezed = Get_MandatoryColumns(GV.sAccessTo, "Contact", "Freeze");

            GV.TR_lstDisposalsToBeFreezed = Get_MandatoryColumns("TR", "Company", "Freeze");
            GV.WR_lstDisposalsToBeFreezed = Get_MandatoryColumns("WR", "Company", "Freeze");

            GV.lstTRContactStatusToBeValidated = Get_MandatoryColumns("TR", "Contact", "Validate");
            GV.lstWRContactStatusToBeValidated = Get_MandatoryColumns("WR", "Contact", "Validate");

            GV.lstTR_DeleteStatus = Get_MandatoryColumns("TR", "Contact", "Delete");
            GV.lstWR_DeleteStatus = Get_MandatoryColumns("WR", "Contact", "Delete");

            GV.lstEmailChackContactStatus.Clear();
            GV.lstEmailChackContactStatus.AddRange(Get_MandatoryColumns("TR", "Contact", "Email"));
            GV.lstEmailChackContactStatus.AddRange(Get_MandatoryColumns("WR", "Contact", "Email"));
            //lstEmailChackContactStatus.AddRange(Get_MandatoryColumns("TR","Contact", "Freeze"));
            //lstEmailChackContactStatus.AddRange(Get_MandatoryColumns("WR","Contact", "Freeze"));
            GV.lstEmailChackContactStatus = GV.lstEmailChackContactStatus.Distinct().ToList();

            GV.sEmailCheckContactStatus = GM.ListToQueryString(GV.lstEmailChackContactStatus, "String");

            GV.sTRContactstatusTobeValidated = GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String");
            GV.sWRContactstatusTobeValidated = GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String");
            
            GV.lstTRContactstatusTobeMailChecked = Get_MandatoryColumns("TR", "Contact", "MailCheck");
            GV.lstWRContactstatusTobeMailChecked = Get_MandatoryColumns("WR", "Contact", "MailCheck");

            GV.sTRContactstatusTobeMailChecked = GM.ListToQueryString(Get_MandatoryColumns("TR", "Contact", "MailCheck"), "String");
            GV.sWRContactstatusTobeMailChecked = GM.ListToQueryString(Get_MandatoryColumns("WR", "Contact", "MailCheck"), "String");

            GV.TR_lstDisposalsToBeValidated = Get_MandatoryColumns(GV.sAccessTo, "Company", "Validate");

            GV.lstNewRecordContactStatus = Get_MandatoryColumns(GV.sAccessTo, "Contact", "New");

            GV.lstUnchangedRecordContactStatus = Get_MandatoryColumns(GV.sAccessTo, "Contact", "Verify");

            GV.lstChangedRecordContactStatus = Get_MandatoryColumns(GV.sAccessTo, "Contact", "Update");

            GV.lstReplacementRecordContactStatus = Get_MandatoryColumns(GV.sAccessTo, "Contact", "Replacement");

            GV.lstReplacementOptionRecordContactStatus = Get_MandatoryColumns(GV.sAccessTo, "Contact", "ReplaceLeft");

            GV.sReplacementOptionContactStatus = GM.ListToQueryString(GV.lstReplacementOptionRecordContactStatus, "String");

            GV.lstNeutralContactStatus = Get_MandatoryColumns(GV.sAccessTo, "Contact", "Neutral");
        }

        //-----------------------------------------------------------------------------------------------------
        private List<string> Get_MandatoryColumns(string sFlag, string sTableName, string sOperationType)
        {
            List<string> lst = new List<string>();
            string sCondition = string.Empty;
            if (sOperationType.Contains("|"))
            {
                List<string> lstRemarks = sOperationType.Split('|').ToList();
                foreach (string s in lstRemarks)
                {
                    if (sCondition.Length == 0)
                        sCondition = "Operation_Type LIKE '%" + s + "%'";
                    else
                        sCondition += " OR Operation_Type LIKE '%" + s + "%'";
                }
            }
            else
                sCondition = "Operation_Type LIKE '%" + sOperationType + "%'";

            sCondition = "(" + sCondition + ")";

            if (dtRecordStatus != null && dtRecordStatus.Rows.Count > 0)
            {
                DataRow[] drMandatoryStatus = dtRecordStatus.Select("Table_Name = '" + sTableName + "' AND Research_Type = '" + sFlag + "' AND " + sCondition);
                foreach (DataRow dr in drMandatoryStatus)
                    lst.Add(dr["Primary_Status"].ToString());
            }
            return lst.Distinct().ToList();
        }

        //-----------------------------------------------------------------------------------------------------
        private List<string> Get_ProjectColumns(string sTableName, string sColumnName)
        {
            try
            {
                DataRow[] drSelected = dtFieldMaster.Select("TABLE_NAME = '" + sTableName + "' AND " + sColumnName + " = 'Y'");
                if (drSelected.Length > 0)
                {
                    List<string> lst = new List<string>();
                    foreach (DataRow dr in drSelected)
                    {
                        lst.Add(dr["FIELD_NAME_TABLE"].ToString().ToUpper());
                    }
                    //return drSelected.CopyToDataTable().AsEnumerable().Select(x => x["FIELD_NAME_TABLE"].ToString()).ToList();
                    return lst;
                }
                return new List<string>();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnUserMaster_Click(object sender, EventArgs e)//Add,Edit or remove GCCUsers
        {
            try
            {
                frmUserMaster objfrmUserMaster = new frmUserMaster();
                objfrmUserMaster.dtAllUsers = dtAllUsers;
                objfrmUserMaster.dtGCCUsers = dtGCCUsers;
                objfrmUserMaster.StartPosition = FormStartPosition.CenterScreen;
                objfrmUserMaster.MinimizeBox = false;
                objfrmUserMaster.MaximizeBox = false;
                //objfrmUserMaster.ShowInTaskbar = false;
                objfrmUserMaster.ShowDialog(this);
                dtGCCUsers = objfrmUserMaster.dtGCCUsers;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void cmbSelectProject_ButtonCustomClick(object sender, EventArgs e)//Select Project manually
        {
            try
            {
                if (dtProjectMaster != null && dtProjectMaster.Rows.Count > 0)
                {
                    frmComboList objFrmComboList = new frmComboList();
                    objFrmComboList.TitleText = "Select Project";

                    if (GV.sUserType == "Admin")//To create new project
                    {
                        DataTable dtProjectMasterCopy = dtProjectMaster.Copy();
                        DataRow drNewRow = dtProjectMasterCopy.NewRow();
                        drNewRow["ProjectName"] = "(New Project)";
                        dtProjectMasterCopy.Rows.Add(drNewRow);
                        cmbSelectProject.DataSource = dtProjectMasterCopy;
                        objFrmComboList.dtItems = dtProjectMasterCopy;
                    }
                    else
                        objFrmComboList.dtItems = dtProjectMaster;

                    objFrmComboList.lstColumnsToDisplay.Add("ProjectName");
                    objFrmComboList.sColumnToSearch = "ProjectName";
                    objFrmComboList.IsSpellCheckEnabeld = false;
                    objFrmComboList.IsMultiSelect = false;
                    objFrmComboList.IsSingleWordSelection = true;
                    objFrmComboList.StartPosition = FormStartPosition.CenterScreen;
                    objFrmComboList.MinimizeBox = false;
                    objFrmComboList.MaximizeBox = false;
                    //objFrmComboList.ShowInTaskbar = false;
                    objFrmComboList.ShowDialog(this);

                    if (objFrmComboList.sReturn != null && objFrmComboList.sReturn.Length > 0)
                    {
                        GV.sProjectName = objFrmComboList.sReturn;
                        cmbSelectProject.TextChanged -= new EventHandler(cmbSelectProject_TextChanged);
                        cmbSelectProject.SelectedValue = GV.sProjectName;
                        cmbSelectProject.TextChanged -= new EventHandler(cmbSelectProject_TextChanged);

                        Get_Project_Tables(sender, e);
                    }
                    //else
                    //cmbSelectProject_ButtonCustomClick(sender, e);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void cmbUserType_TextChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
            {

                if (cmbUserType.Text == "Agent")
                {
                    GV.sUserType = "Agent";
                    GV.sAccessTo = GetUserAccess();
                }
                else if (cmbUserType.Text == "QC")
                {
                    GV.sUserType = "QC";
                    GV.sAccessTo = GetUserAccess();
                }
                else if (cmbUserType.Text == "Manager")
                {
                    GV.sUserType = "Manager";
                    GV.sAccessTo = GetUserAccess();
                }
                else if (cmbUserType.Text == "Admin")
                {
                    GV.sUserType = "Admin";
                    GV.sAccessTo = GetUserAccess();
                }

                if (GV.sAccessTo.Length > 0)
                {
                    IsLoading = true;
                    if (GV.sAccessTo == "TR")
                    {
                        GV.sOppositAccess = "WR";
                        cmbUserAccess.SelectedItem = "Tele Researcher";
                    }
                    else if (GV.sAccessTo == "WR")
                    {
                        GV.sOppositAccess = "TR";
                        cmbUserAccess.SelectedItem = "Web Researcher";
                    }
                    IsLoading = false;

                    Get_Project_Tables(sender, e);
                }

                ShowDialer();
                statuslblUserType.Text = "User Type : " + cmbUserType.Text;
                statuslblAccessTo.Text = "User Access : " + GV.sAccessTo;
            }
        }

        private void cmbUserAccess_TextChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
            {
                if (cmbUserAccess.Text == "Tele Researcher")
                {
                    GV.sAccessTo = "TR";
                    GV.sOppositAccess = "WR";
                }
                else if (cmbUserAccess.Text == "Web Researcher")
                {
                    GV.sAccessTo = "WR";
                    GV.sOppositAccess = "TR";
                }
                ShowDialer();
                Get_Project_Tables(sender, e);
                statuslblAccessTo.Text = "User Access : " + GV.sAccessTo;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private string GetUserAccess()
        {
            if (dtGCCUsers.Select("USERNAME = '" + GV.sEmployeeName + "'").Length > 0)
                return dtGCCUsers.Select("USERNAME = '" + GV.sEmployeeName + "'")[0]["USERACCESS"].ToString();
            if (dtAllUsers.Select(String.Format("UserName = '{0}'", GV.sEmployeeName)).Length > 0)
            {
                DataRow[] drrAllUser = dtAllUsers.Select(String.Format("UserName = '{0}'", GV.sEmployeeName));
                if (drrAllUser[0]["Department"].ToString() == "Data") //Pre Select the Usertype combobox based on the acquired data                                    
                    return "WR";
                if (drrAllUser[0]["Department"].ToString() == "GCC")
                    return "TR";
                return string.Empty;
            }
            return string.Empty;
        }

        //-----------------------------------------------------------------------------------------------------
        private void cmbSelectProject_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSelectProject.DisplayMembers == "ProjectName" && cmbSelectProject.ValueMember == "ProjectName") // To avoid initial loading text changes
                {
                    GV.sProjectName = cmbSelectProject.Text;
                    Get_Project_Tables(sender, e);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private string ProjectReadyCheck()//Final validation to launch the Process
        {
            string sError = string.Empty;
            if (GV.sCompanyTable.Trim().Length == 0 || GV.sContactTable.Trim().Length == 0)
                sError += "Project Setting not found" + Environment.NewLine;
            if (GV.sEmployeeName.Trim().Length == 0 || GV.sEmployeeNo.Trim().Length == 0)
                sError += "User data not found" + Environment.NewLine;
            if (GV.sProjectName.Trim().Length == 0 || GV.sProjectID.Trim().Length == 0)
                sError += "Project data not found" + Environment.NewLine;
            if (GV.sUserType.Trim().Length == 0)
                sError += "Can't find User Type";
            if (GV.sAccessTo.Trim().Length == 0)
                sError += "Can't find User Access";
            if (GV.sDashBoardID.Trim().Length == 0)
                sError += "Dashboard not found";
            if (GV.sDialerType.Trim().Length == 0)
                sError += "Dialer not selected";
            if (GV.sProjectID.ToUpper() == "CRUPRO001" && GV.sUserType == "Agent")
                sError += "This project is not accessible.";
            if (GV.sUserType == "Agent" && GV.sAccessTo == "TR")
            {
                if ((GV.sDialerType == "iSystem" || GV.sDialerType == "X-Lite") && (GV.sExtensionNumber.Trim().Length == 0 || GV.sExtensionNumber == "N/A"))
                    sError += "Invalid Extension";

                if ((GV.sDialerType == "Vortex") && (GV.sVortexExtension.Trim().Length == 0 || GV.sVortexExtension == "N/A"))
                    sError += "Invalid Extension";
            }

            if (GV.HasAdminPermission || (GV.IsApplicationinAlpha && GV.MSSQL1.BAL_ExecuteQuery("select 1 from c_picklists where PicklistCategory IN ('AlphaAllowed_ProjectID','AlphaAllowed_EmployeeID') and PicklistValue IN ('" + GV.sProjectID + "','" + GV.sEmployeeNo + "')").Rows.Count >= 2))
            {/* Do Nothing */ } 
            else
            {
                sError += "Access denied to use Alpha edition.";
            }

            //if(GV.IsApplicationinAlpha && !GV.HasAdminPermission)
            //{
            //    sError += "Access denied to use Alpha edition.";
            //}

            return sError;
            /////////////Mode validations to be added//////////////////////////////////////
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnLaunchProject_Click(object sender, EventArgs e)//Launch and Close Project
        {
            LaunchCloseProject();
        }

        //-----------------------------------------------------------------------------------------------------
        private void LaunchCloseProject()
        {
            try
            {
                if (btnLaunchProject.Text == "<div align='center'>Launch<br/>Project</div>")
                {
                    GV.HasAdminPermission = (GV.sEmployeeName.Length > 1 && dtGCCUsers.Select("USERTYPE = 'Admin' AND USERNAME = '" + GV.sEmployeeName + "'").Length > 0);
                    string sError = ProjectReadyCheck().Trim();
                    if (sError.Length == 0)
                    {
                        GV.sSessionID = GV.IP.Replace(".", string.Empty).Reverse() + GM.GetDateTime().ToString("yyMMddHHmmssff");                        
                        GM.Log("Project LoggedIn");
                        progressBar.Visible = true;
                        progressBar.Value = 0;
                        progressBar.Text = string.Empty;
                        ProcessTimer.Enabled = true;

                        btnLaunchProject.Text = "<div align='center'>Close<br/>Project</div>";
                        rbnBarSwitchProject.Enabled = false;
                        rbnBarUser.Enabled = false;
                        btnRefresh.Enabled = false;
                        btnProjectControl.Enabled = false;

                        if (GV.sProjectID.ToUpper() == "CRUPRO001")
                        {
                            if (!GM.IsFormExist("frmEmailManualChecks"))
                            {
                                frmEmailManualChecks objfrmEmailChecks = new frmEmailManualChecks();
                                objfrmEmailChecks.MdiParent = this;
                                objfrmEmailChecks.Show();
                            }
                            return;
                        }



                        txtPath.TextBox.ReadOnly = true;
                        txtPath.TextBox.BackColor = Color.White;
                        rbnTabProcess.Visible = true; //Show process Tab and disable all initial setting controls
                        rbnTabQC.Visible = true;
                        ribbonMain.SelectedRibbonTabItem = rbnTabProcess;

                        OpenCompanyList();


                        if (GV.sUserType == "QC" || GV.sUserType == "Admin")
                        {
                            rbnTabQC.Visible = true;
                            //OpenQC();
                        }
                        else
                            rbnTabQC.Visible = false;

                        if ((GV.sAccessTo == "TR" && GV.sAllowNewCompanyTR == "Y") || (GV.sAccessTo == "WR" && GV.sAllowNewCompanyWR == "Y"))
                            btnAddNewCompany.Enabled = true;
                        else
                            btnAddNewCompany.Enabled = false;

                        if (GV.sAccessTo == "WR")
                            switchButtonEnableDiableCallscript.Visible = switchButtonTimeZone.Visible = false;
                        else
                            switchButtonEnableDiableCallscript.Visible = switchButtonTimeZone.Visible = true;

                        timerNotification.Start();
                    }
                    else
                    {
                        //MessageBoxEx.Show(sError);
                        GM.Log("Project Launch Error", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, sError.Replace("'", "''"));
                        ToastNotification.Show(this, sError, eToastPosition.TopRight);
                        return;
                    }
                }
                else if (btnLaunchProject.Text == "<div align='center'>Close<br/>Project</div>")
                {

                    foreach (Form f in Application.OpenForms)//Close Contact update screen first
                    {
                        if (f.Name == "FrmContactsUpdate")
                        {
                            ((Form)f).Activate();
                            ToastNotification.Show(f, "Close Contact Update Screen.", eToastPosition.TopRight);
                            return;
                        }

                        if (f.Name == "frmSearch")
                        {
                            ((Form)f).Activate();
                            ToastNotification.Show(f, "Close Search Screen.", eToastPosition.TopRight);
                            return;
                        }

                        if (f.Name == "frmScrapper")
                        {
                            ((Form)f).Activate();
                            ToastNotification.Show(f, "Close Scrapper.", eToastPosition.TopRight);
                            return;
                        }

                        timerNotification.Stop();

                    }

                    while (Application.OpenForms.Count != 1)
                    {
                        try
                        {
                            for (int i = 0; i < Application.OpenForms.Count; i++)
                            {
                                if (Application.OpenForms[i] != this)
                                    Application.OpenForms[i].Close();
                            }
                        }
                        catch (Exception ex) { }
                    }

                    btnLaunchProject.Text = "<div align='center'>Launch<br/>Project</div>";

                    GM.Log("Project LoggedOut");
                    //On project close enable all initial setting controls and hide process tab
                    GM.Moniter(false, "");
                    rbnTabProcess.Visible = false;
                    rbnTabQC.Visible = false;
                    rbnBarSwitchProject.Enabled = true;
                    rbnBarUser.Enabled = true;
                    btnRefresh.Enabled = true;
                    btnProjectControl.Visible = true;

                    Session(dtSession);
                    ProcessTimer.Enabled = false;
                    dtSession.Rows.Clear();

                }
                btnShowHideFilter.Checked = false;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void switchRbnExpandCollapse_ValueChanged(object sender, EventArgs e)//  hide/show ribbon
        {
            try
            {
                if (switchRbnExpandCollapse.Value)
                    ribbonMain.Expanded = true;
                else
                    ribbonMain.Expanded = false;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnAllocation_Click(object sender, EventArgs e)
        {
            try
            {

                //txtErrors.Text = Application.ExecutablePath + Environment.NewLine + Application.StartupPath + Environment.NewLine + Assembly.GetExecutingAssembly().Location;

                string sError = ProjectReadyCheck().Trim();
                if (sError.Length == 0)
                {
                    GV.HasAdminPermission = (GV.sEmployeeName.Length > 1 && dtGCCUsers.Select("USERTYPE = 'Admin' AND USERNAME = '" + GV.sEmployeeName + "'").Length > 0);
                    frmTeleAllocation objfrmTeleAllocation = new frmTeleAllocation();
                    objfrmTeleAllocation.dtAllUsers = dtAllUsers;
                    objfrmTeleAllocation.StartPosition = FormStartPosition.CenterScreen;
                    //objfrmTeleAllocation.ShowInTaskbar = false;
                    //objfrmTeleAllocation.MinimizeBox = false;
                    //objfrmTeleAllocation.MaximizeBox = false;
                    objfrmTeleAllocation.ShowDialog(this);
                }
                else
                {
                    //MessageBoxEx.Show(sError);
                    ToastNotification.Show(this, sError, eToastPosition.TopRight);
                    return;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnCompanyList_Click(object sender, EventArgs e)
        {
            OpenCompanyList();
        }

        //-----------------------------------------------------------------------------------------------------
        public bool IsTargetSet()
        {
            if (GV.HasAdminPermission)
                return true;

            if (GV.sUserType == "Agent")
            {
                DataTable dtDaily_Agent_Perfoemance = GV.MSSQL.BAL_ExecuteQuery("select iSNULL(SELF_TARGET,'')SELF_TARGET from DAILY_AGENT_PERFORMANCE_V1 where DASHBOARD_ID = " + GV.sDashBoardID + " AND FLAG = '" + GV.sAccessTo + "' AND AGENTNAME = '" + GV.sEmployeeName + "' AND DATECALLED = '" + GM.GetDateTime().ToString("yyyy-MM-dd") + "'");// + GM.GetDateTime().ToString("yyyyMMdd") + "'");
                if (dtDaily_Agent_Perfoemance.Rows.Count > 0 && dtDaily_Agent_Perfoemance.Rows[0]["SELF_TARGET"].ToString().Length > 0 && dtDaily_Agent_Perfoemance.Rows[0]["SELF_TARGET"].ToString() != "0")
                    return true;
                return false;
            }
            return true;
        }

        public void OpenQC()
        {
            if (GV.sEmployeeName == "THANGAPRAKASH")
            {
                if (!GM.IsFormExist("frmQC"))
                {
                    objfrmQC = new frmQC();
                    //objfrmQC.FormClosed += new FormClosedEventHandler(objFrmCompanyList_Closed);
                    //objfrmQC.Activated += new EventHandler(objFrmCompanyList_Activated);
                    objfrmQC.MdiParent = this;
                    objfrmQC.ParantForm = this;
                    objfrmQC.Show();

                    //frmUncertain obj = new frmUncertain();
                    //obj.MdiParent = this;
                    //obj.Show();
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------
        public void OpenCompanyList()
        {
            try
            {
                if (IsTargetSet())
                {

                    if (!GM.IsFormExist("frmCompanyList"))
                    {
                        objFrmCompanyList = new frmCompanyList();
                        objFrmCompanyList.FormClosed += new FormClosedEventHandler(objFrmCompanyList_Closed);
                        objFrmCompanyList.Activated += new EventHandler(objFrmCompanyList_Activated);
                        objFrmCompanyList.MdiParent = this;
                        objFrmCompanyList.ParantForm = this;

                        objFrmCompanyList.Show();



                        if (GV.HasAdminPermission)
                            rbnBarQuery.Visible = true;
                        else
                            rbnBarQuery.Visible = false;

                        rbnBarSearch.Enabled = true;
                        rbnBarAction.Enabled = true;
                        rbnBarDashboard.Enabled = true;
                        rbnBarBounce.Enabled = true;
                        btnExportToExcel.Enabled = true;
                        btnMoveTo.Enabled = true;
                        btnExportToExcel.Enabled = true;
                        btnCount.Enabled = true;

                        btnSearch.Visible = GV.Allow_External_Search;
                        rbnBarContact.MinimumSize = GV.Allow_External_Search ? new Size(282, 110) : new Size(220, 110);
                        txtID.Text = string.Empty;

                        if (GV.sUserType == "Agent")
                        {
                            switchButtonEnableDiableCallscript.Value = true;
                            switchButtonTimeZone.Value = true;
                            btnMoveTo.Visible = false;
                            rbnBarBounce.Visible = false;
                            btnExportToExcel.Visible = false;
                            rbnBarSearch.Text = "Filter";
                            rbnBarUncertain.Visible = false;
                            btnSendBackInfo.Visible = false;
                            btnEmonitor.Visible = false;
                            txtID.Enabled = GV.sAccessTo == "TR" ? GV.IsIDOpen_TR : GV.IsIDOpen_WR;

                            //btnSendBack.NotificationMarkText = SendBackCount();
                        }
                        else
                        {
                            btnSendBackInfo.Visible = true;
                            switchButtonEnableDiableCallscript.Value = false;
                            btnEmonitor.Visible = true;
                            btnMoveTo.Visible = true;
                            rbnBarBounce.Visible = true;
                            btnExportToExcel.Visible = true;
                            rbnBarSearch.Text = "Detailed Filter";
                            rbnBarUncertain.Visible = true;
                        }

                        if (GV.sAccessTo == "TR")
                            btnMoveTo.Text = "Move to WR";
                        else if (GV.sAccessTo == "WR")
                            btnMoveTo.Text = "Move to TR";

                    }
                }
                else
                {
                    ToastNotification.Show(this, "Today's Target not set.", eToastPosition.TopRight);
                    if (!GM.IsFormExist("frmTarget"))
                    {
                        objfrmTarget = new frmTarget();
                        objfrmTarget.MdiParent = this;
                        objfrmTarget.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        //-----------------------------------------------------------------------------------------------------
        private void objFrmCompanyList_Activated(object sender, EventArgs e)
        {
            ribbonMain.SelectedRibbonTabItem = rbnTabProcess;
        }

        //-----------------------------------------------------------------------------------------------------
        private void objFrmCompanyList_Closed(object sender, FormClosedEventArgs e)//Triggers when objFrmCompanyList window closed
        {
            rbnBarSearch.Enabled = false;
            rbnBarAction.Enabled = false;
            rbnBarBounce.Enabled = false;
            rbnBarDashboard.Enabled = false;
            btnExportToExcel.Enabled = false;
            btnCount.Enabled = false;
            btnMoveTo.Enabled = false;
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnShowHideFilter_Click(object sender, EventArgs e)
        {
            if (objFrmCompanyList.splitCompanyList.Panel1Collapsed)
            {
                objFrmCompanyList.splitCompanyList.Panel1Collapsed = false;
                objFrmCompanyList.splitCompanyList.SplitterDistance = 202;
                objFrmCompanyList.dgvCompanySearch.Focus();
                btnShowHideFilter.Checked = true;
            }
            else
            {
                objFrmCompanyList.splitCompanyList.Panel1Collapsed = true;
                objFrmCompanyList.Clear_ResultSet();
                objFrmCompanyList.RebuildSearchGrid();
                objFrmCompanyList.dgvCompanyList.Focus();
                btnShowHideFilter.Checked = false;
                lblMessage.Visible = false;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            if (!objFrmCompanyList.splitCompanyList.Panel1Collapsed)
            {
                lblMessage.Visible = true;
                int iFilteredRowCount = objFrmCompanyList.Clear();
                lblMessage.Text = "Filtered Rows: " + iFilteredRowCount;
                lblMessage.BorderType = eBorderType.Sunken;
                barStatus.Refresh();
                lblMessage.Refresh();
            }

            //objFrmCompanyList.Reload();
            //objFrmCompanyList.RebuildSearchGrid();
            //objFrmCompanyList.splitCompanyList.SplitterDistance = 202;
            //lblMessage.Visible = false;
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnReload_Click(object sender, EventArgs e)
        {
            objFrmCompanyList.Clear_ResultSet();
            objFrmCompanyList.RebuildSearchGrid();
            if (!objFrmCompanyList.splitCompanyList.Panel1Collapsed)
                objFrmCompanyList.splitCompanyList.Panel1Collapsed = true;
            btnShowHideFilter.Checked = false;
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnExecuteFilter_Click(object sender, EventArgs e)
        {
            ExecuteFilter();
        }

        //-----------------------------------------------------------------------------------------------------
        public void ExecuteFilter()
        {
            if (!objFrmCompanyList.splitCompanyList.Panel1Collapsed)
            {
                lblMessage.Visible = true;
                string sQuery = string.Empty;
                int iFilteredRowCount = objFrmCompanyList.ExecuteSQLText(out sQuery);
                txtQuery.Text = sQuery;
                lblMessage.Text = "Filtered Rows: " + iFilteredRowCount;
                lblMessage.BorderType = eBorderType.Sunken;
                barStatus.Refresh();
                lblMessage.Refresh();
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnGo_Click(object sender, EventArgs e)
        {
            GM.StringAppend("Start");
            if (txtID.Text.Trim().Length > 0)
                GM.OpenContactUpdate(txtID.Text, false, true, objFrmCompanyList, objFrmCompanyList);
            txtID.Value = 0;
            txtID.Text = string.Empty;
            GM.StringAppend("Stop");
        }

        //-----------------------------------------------------------------------------------------------------
        private void txtID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtID.Text.Length > 0)
            {
                GM.OpenContactUpdate(txtID.Text, false, true, objFrmCompanyList, objFrmCompanyList);
                txtID.Value = 0;
                txtID.Text = string.Empty;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            /////Key press listener for overall window/////
            if (keyData == Keys.Enter && ribbonMain.SelectedRibbonTabItem.Name == "rbnTabLogin")/////Enter key for Submit/////
                LaunchCloseProject();
            if (keyData == Keys.Enter && ribbonMain.SelectedRibbonTabItem.Name == "rbnTabProcess")/////
            {

            }

            if (keyData == (Keys.Control | Keys.I) && ribbonMain.SelectedRibbonTabItem.Name == "rbnTabProcess")/////
                txtID.Focus();

            if (keyData == Keys.F1)
                GM.OpenHelp();

            if (keyData == (Keys.Control | Keys.Shift | Keys.L) && ribbonMain.SelectedRibbonTabItem.Name == "rbnTabProcess")/////
            {
                if (objFrmCompanyList.splitCompanyList.Panel1Collapsed)
                    btnShowHideFilter.RaiseClick();
                else
                {
                    btnShowHideFilter.RaiseClick();
                    btnClearFilter.RaiseClick();
                }
            }

            if (ribbonMain.SelectedRibbonTabItem.Name == "rbnTabProcess")
            {
                if (keyData == Keys.F5)
                    btnReload.RaiseClick();

                if (keyData == Keys.F9)
                    btnExecuteFilter.RaiseClick();

                if (keyData == Keys.F10)
                    btnClearFilter.RaiseClick();

                if (keyData == (Keys.Control | Keys.P))
                    btnCompanyList.RaiseClick();

                if (keyData == (Keys.Control | Keys.N) && btnAddNewCompany.Enabled)
                    btnAddNewCompany.RaiseClick();

                if (keyData == (Keys.Control | Keys.U))
                    btnContactUpdate.RaiseClick();

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        //-----------------------------------------------------------------------------------------------------
        public void tabItemShortcutHandle(string sDirection)
        {
            if (tabStrip != null && tabStrip.Tabs.Count > 0)
            {
                //if (sDirection == "Right" && !(tabStrip.SelectedTabIndex + 1 > tabStrip.Tabs.Count-1))
                //    tabStrip.SelectedTabIndex = tabStrip.SelectedTabIndex + 1;
                //else if (sDirection == "Left" && !(tabStrip.SelectedTabIndex - 1 < 0))
                //    tabStrip.SelectedTabIndex = tabStrip.SelectedTabIndex - 1;

                if (sDirection == "Right")
                    tabStrip.SelectNextTab();
                else if (sDirection == "Left")
                    tabStrip.SelectPreviousTab();
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CanClose)
            { }
            else
            {
                //while (Application.OpenForms.Count != 1)
                //{
                //    try
                //    {
                //        for (int i = 0; i < Application.OpenForms.Count; i++)
                //        {
                //            if (Application.OpenForms[i] != this)
                //                Application.OpenForms[i].Close();
                //        }
                //    }
                //    catch (Exception ex) { }
                //}

                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "FrmContactsUpdate")
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                if (DialogResult.Yes != MessageBoxEx.Show("Are you sure you want to close Campaign Manager", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
                    e.Cancel = true;
                else
                    GV.MSSQL1.BAL_ExecuteNonReturnQuery("UPDATE c_machines set STATUS = 'Offline', RDPPort='',ESPort = '' WHERE MachineID='" + GV.sMachineID + "';");
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void frmMain_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            frmHelp objfrmHelp = new frmHelp();
            objfrmHelp.ShowDialog(this);
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnAddNewCompany_Click(object sender, EventArgs e)
        {
            try
            {
                OpenCompanyList();
                string sTimeLogger = GM.IsTimeLoggerHasProject();
                if (sTimeLogger.Trim().Length > 0)
                {
                    ToastNotification.Show(this, sTimeLogger);
                    return;
                }

                if (IsTargetSet())
                {

                    bool IsFormClosed = true;
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Name == "FrmContactsUpdate")
                        {
                            IsFormClosed = false;
                            f.Focus();
                            if (DialogResult.Yes == MessageBoxEx.Show("Creating new company will close the existing screen." + Environment.NewLine + "Changes will not be saved." + Environment.NewLine + "Do you want to continue ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                            {
                                f.Close();
                                IsFormClosed = true;
                            }
                            else
                                IsFormClosed = false;
                            break;
                        }
                    }

                    if (IsFormClosed)
                    {
                        frmAddNewCompany objfrmAddNewCompany = new frmAddNewCompany();
                        string sNewCompanyName = string.Empty;
                        if (objfrmAddNewCompany.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && objfrmAddNewCompany.sCompanyName.Length > 0)
                        {
                            sNewCompanyName = objfrmAddNewCompany.sCompanyName;
                            if (DialogResult.Yes == MessageBoxEx.Show("Are you sure to create New Company?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                            {
                                Regex rAlphaNumeric = new Regex(@"[^0-9A-Za-z]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                                string sCompanyNameAlpha = rAlphaNumeric.Replace(sNewCompanyName, string.Empty);
                                string sInsertSQL = "INSERT INTO " + GV.sCompanyTable + "(COMPANY_NAME, COMPANY_NAME_ALPHA , " + GV.sAccessTo + "_AGENTNAME, " + GV.sAccessTo + "_PREVIOUS_AGENTNAME, NEW_OR_EXISTING ,COUNTRY, FLAG ,CREATED_BY ,CREATED_DATE)VALUES('" + sNewCompanyName + "', '" + sCompanyNameAlpha + "', '" + GV.sEmployeeName + "','" + GV.sEmployeeName + "','New','Country','" + GV.sAccessTo + "','" + GV.sEmployeeName + "',GETDATE());";
                                sInsertSQL += " UPDATE " + GV.sCompanyTable + " set GROUP_ID = MASTER_ID WHERE MASTER_ID = @@IDENTITY;";
                                string sMasterID = GV.MSSQL1.BAL_InsertAndGetIdentity(sInsertSQL);
                                GM.OpenContactUpdate(sMasterID, true, true, objFrmCompanyList, objFrmCompanyList);
                            }
                        }
                    }
                }
                else
                {
                    ToastNotification.Show(this, "Today's Target not set.", eToastPosition.TopRight);
                    if (!GM.IsFormExist("frmTarget"))
                    {
                        objfrmTarget = new frmTarget();
                        objfrmTarget.MdiParent = this;
                        objfrmTarget.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        //-----------------------------------------------------------------------------------------------------
        private void rbnTabProcess_Click(object sender, EventArgs e)
        {

            OpenCompanyList();
            //foreach (Form f in Application.OpenForms)
            //{
            //    if (f.Name == "frmCompanyList")
            //    {
            //        f.Focus();
            //        break;
            //    }
            //}
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnRefersh_Click(object sender, EventArgs e)
        {
            Refresh_LoadAll(sender, e);
            //ToastNotification.Show(this, "GMT"+GlobalMethods.GetDateTime().ToString(), eToastPosition.BottomRight);
            //ToastNotification.Show(this, "Current Time"+GlobalMethods.GetDateTime().ToString(), eToastPosition.BottomLeft);
        }

        //-----------------------------------------------------------------------------------------------------
        //private void Get_Blob_Path(string sProjectIDBlob)
        //{
        //    try
        //    {
        //        if (sProjectIDBlob == "")
        //            return;
        //        DataTable dtBlob = new DataTable();
        //        byte[] byteBlob;
        //        string sPath = AppDomain.CurrentDomain.BaseDirectory, sFileName;
        //        //string sPath = AppDomain.CurrentDomain.BaseDirectory + "\\Campaign Manager", sFileName;
        //        string SQL = "SELECT * FROM PROJECT_FILES WHERE ProjectID='" + sProjectIDBlob + "' AND FileType IN ('CallScript','BOT');";
        //        SqlConnection connection = new SqlConnection(GV.sMSSQL);
        //        SqlCommand command = new SqlCommand(SQL, connection);
        //        if (connection.State != ConnectionState.Open)
        //            connection.Open();
        //        SqlDataReader sqldatar = command.ExecuteReader();
        //        dtBlob.Load(sqldatar);
        //        sqldatar.Close();
        //        connection.Close();

        //        GV.sCallScriptPath = string.Empty;
        //        //GV.sEAFLibararyPath = string.Empty;

        //        foreach (DataRow drBlob in dtBlob.Rows)
        //        {
        //            byteBlob = (byte[])drBlob["Blob"];
        //            sFileName = sPath + "\\" + drBlob["FileName"].ToString().Trim() + "." + drBlob["Extension"].ToString().Trim();

        //            if (!Directory.Exists(sPath))
        //                Directory.CreateDirectory(sPath);

        //            if (drBlob["FileType"].ToString() == "CallScript")
        //            {
        //                File.WriteAllBytes(sFileName, byteBlob);
        //                GV.sCallScriptPath = sFileName;
        //            }
        //            else if (drBlob["FileType"].ToString().ToUpper() == "BOT")
        //            {
        //                File.WriteAllBytes(sFileName, byteBlob);
        //                GV.sBOTPath = sFileName;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
        //        //MessageBox.Show(ex.Message);
        //    }
        //}

        //-----------------------------------------------------------------------------------------------------
        private void switchButtonEnableDiableCallscript_ValueChanged(object sender, EventArgs e)
        {
            GV.IsCallScriptEnabled = switchButtonEnableDiableCallscript.Value;

            //if (switchButtonEnableDiableCallscript.Value)
            //    rbnBarCallScript.Text = "Call Script is Disabled";
            //else
            //    rbnBarCallScript.Text = "Call Script is Enabled";

        }

        //-----------------------------------------------------------------------------------------------------
        private void switchButtonEnableDiableValidator_ValueChanged(object sender, EventArgs e)
        {
            GV.DynamicValidator = switchButtonEnableDiableValidator.Value;

            if (!switchButtonEnableDiableValidator.Value)
            {
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "FrmContactsUpdate")
                    {
                        List<TextBox> lst = ((FrmContactsUpdate)f).lstContactControls;
                        foreach (Control C in lst)
                            C.BackColor = Color.White;

                        ((FrmContactsUpdate)f).expandablePanelMessage.Expanded = false;
                        ((FrmContactsUpdate)f).webBrowserMessage.DocumentText = string.Empty;
                        break;
                    }
                }
            }
        }

        private void switchButtonTimeZone_ValueChanged(object sender, EventArgs e)
        {
            GV.TimeEnabled = switchButtonTimeZone.Value;
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnMoveTo_Click(object sender, EventArgs e)
        {
            //frmLogin objLogin = new frmLogin();
            //objLogin.sEmployeeName = GV.sEmployeeName;
            //objLogin.sPassword = GV.sEmployeePassword;            
            //objLogin.frmParant = this;
            //if (objLogin.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && objLogin.IsUserPassed)
            //    objFrmCompanyList.RecordMovement();



            objFrmCompanyList.expandablePanelRecordMovement.Parent = objFrmCompanyList;
            objFrmCompanyList.expandablePanelRecordMovement.Dock = DockStyle.Right;
            objFrmCompanyList.expandablePanelExport.Expanded = false;
            objFrmCompanyList.expandablePanelExport.Visible = false;
            objFrmCompanyList.expandablePanelRecordMovement.Expanded = !objFrmCompanyList.expandablePanelRecordMovement.Expanded;


        }

        //-----------------------------------------------------------------------------------------------------
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            objFrmCompanyList.expandablePanelExport.Parent = objFrmCompanyList;
            objFrmCompanyList.expandablePanelExport.Dock = DockStyle.Right;
            objFrmCompanyList.expandablePanelRecordMovement.Expanded = false;
            objFrmCompanyList.expandablePanelRecordMovement.Visible = false;
            objFrmCompanyList.expandablePanelExport.Expanded = !objFrmCompanyList.expandablePanelExport.Expanded;
            //objFrmCompanyList.ExportExcel();
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnProjectControl_Click(object sender, EventArgs e)
        {
            if (GV.sEmployeeName == "THANGAPRAKASHx")
            {
                frmValidation obj = new frmValidation();
                obj.ShowDialog();
                return;
            }

            GM.Log("Project Setting Launched");
            frmProjectControl objfrmProjectControl = new frmProjectControl();
            objfrmProjectControl.WindowState = FormWindowState.Maximized;
            objfrmProjectControl.IsNewProject = (dtProjectSettings.Select("PROJECT_ID = '" + GV.sProjectID + "'").Length == 0);
            objfrmProjectControl.ShowDialog(this);
            //LoadAll(sender,e);
        }

        //-----------------------------------------------------------------------------------------------------
        private void textBoxItemBounce_ButtonCustomClick(object sender, EventArgs e)
        {
            if (dateRejectionImport.Visible && ((swhType.Value && swhType.Visible) || btnImportType.Text == "SendBack"))
            {
                
                if (dateRejectionImport.Text.Length > 0)
                {

                    DataTable dtContactReject = GV.MSSQL1.BAL_ExecuteQuery("SELECT DISTINCT " + GV.sAccessTo + "_AGENT_NAME FROM " + GV.sContactTable + " WHERE " + GV.sAccessTo + "_UPDATED_DATE BETWEEN '" + dateRejectionImport.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dateRejectionImport.Value.ToString("yyyy-MM-dd") + " 23:59:59';");

                    if (dtContactReject.Rows.Count > 0)
                    {
                        frmComboList objFrmComboList = new frmComboList();
                        objFrmComboList.TitleText = "Select Agent";
                        objFrmComboList.dtItems = dtContactReject;
                        objFrmComboList.lstColumnsToDisplay.Add(GV.sAccessTo + "_AGENT_NAME");
                        objFrmComboList.sColumnToSearch = GV.sAccessTo + "_AGENT_NAME";
                        objFrmComboList.IsSpellCheckEnabeld = false;
                        objFrmComboList.IsMultiSelect = false;
                        objFrmComboList.IsSingleWordSelection = true;
                        objFrmComboList.StartPosition = FormStartPosition.CenterScreen;
                        //objFrmComboList.ShowInTaskbar = false;
                        objFrmComboList.MinimizeBox = false;
                        objFrmComboList.MaximizeBox = false;
                        objFrmComboList.ShowDialog(this);
                        txtPath.Text = objFrmComboList.sReturn;
                    }
                    else
                        ToastNotification.Show(this, "No contacts processed in selected date.", eToastPosition.TopRight);

                }
                else
                    ToastNotification.Show(this, "Please select the date to reject.", eToastPosition.TopRight);
            }
            else
            {
                if (openFileBounce.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    txtPath.Text = openFileBounce.FileName;
                }
            }
        }





        DataTable ImportExcelXLS(string sFileName)
        {
            try
            {

                return GM.ImportExcel(sFileName);

                string sConn = string.Empty;
                if (sFileName.Substring(sFileName.LastIndexOf('.')).ToLower() == ".xlsx")
                    sConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sFileName + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=0\"";//off 2007 and above
                else if (sFileName.Substring(sFileName.LastIndexOf('.')).ToLower() == ".xls")
                    sConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFileName + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=0\"";//off 2003
                else
                {
                    ToastNotification.Show(this, "Invalid file", eToastPosition.TopRight);
                    return null;
                }
                DataTable dtImportData;
                using (OleDbConnection conn = new OleDbConnection(sConn))
                {
                    conn.Open();
                    DataTable dtSchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    foreach (DataRow schemaRow in dtSchemaTable.Rows)
                    {
                        string sSheetName = schemaRow["TABLE_NAME"].ToString();
                        if (!sSheetName.EndsWith("_"))
                        {
                            try
                            {
                                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sSheetName + "]", conn);
                                cmd.CommandType = CommandType.Text;
                                dtImportData = new DataTable(sSheetName);
                                new OleDbDataAdapter(cmd).Fill(dtImportData);
                                return dtImportData;//Only first sheet is returned
                            }
                            catch (Exception ex)
                            {
                                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                                throw new Exception(ex.Message + string.Format("Sheet:{0}.File:F{1}", sSheetName, sFileName), ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("not registered on the local machine."))
                    MessageBoxEx.Show("OLEDB Engine not installed. Import aborted.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            return null;//if error occured

        }

        #region Commented Code
        //-----------------------------------------------------------------------------------------------------
        //private void btnImportBounce_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //string Qc_Status = "";
        //        if (btnImportType.Text == "SendBack" || (swhType.Visible && swhType.Value))
        //        {
        //            if (txtPath.Text.Trim().Length > 0 && dateRejectionImport.Text.Length > 0)
        //            {
        //                DataTable dtContactReject;
        //                if (btnImportType.Text == "OK")
        //                    dtContactReject = GV.MYSsdfQL.BAL_ExecuteQueryMdySQL("SELECT b.* FROM " + GV.sContactTable + " a INNER JOIN " + GV.sQCTable + " b ON a.CONTACT_ID_P = b.RecordID WHERE a." + GV.sAccessTo + "_UPDATED_DATE BETWEEN '" + dateRejectionImport.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dateRejectionImport.Value.ToString("yyyy-MM-dd") + " 23:59:59' AND b.TableName = 'Contact' AND b.ResearchType = '" + GV.sAccessTo + "' AND (Qc_Status is null OR Qc_Status='Processed');");
        //                else
        //                    dtContactReject = GV.MYdsSQL.BAL_ExecuteQueryMydSQL("SELECT b.* FROM " + GV.sContactTable + " a INNER JOIN " + GV.sQCTable + " b ON a.CONTACT_ID_P = b.RecordID WHERE a." + GV.sAccessTo + "_UPDATED_DATE BETWEEN '" + dateRejectionImport.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dateRejectionImport.Value.ToString("yyyy-MM-dd") + " 23:59:59' AND b.TableName = 'Contact' AND b.ResearchType = '" + GV.sAccessTo + "';");

        //                if (dtContactReject.Rows.Count > 0)
        //                {
        //                    //frmRejectReason objfrmRejectReason = new frmRejectReason();
        //                    //objfrmRejectReason.frmParant = this;
        //                    //objfrmRejectReason.ShowDialog();

        //                    //if (objfrmRejectReason.sReason.Trim().Length > 0)
        //                    //{
        //                    //string sReason = objfrmRejectReason.sReason;                            

        //                    //if (switchButtonSendBackType.Value)
        //                    //    Qc_Status = "SendBack";
        //                    //else
        //                    //    Qc_Status = "Rejection";

        //                    foreach (DataRow dr in dtContactReject.Rows)
        //                    {                                
        //                        dr[GV.sAccessTo + "_QC_Status"] = btnImportType.Text;
        //                        dr[GV.sAccessTo + "_QC_UpdatedDate"] = GM.GetDateTime();
        //                        dr[GV.sAccessTo + "_QC_UpdatedBy"] = GV.sEmployeeName;
        //                        dr[GV.sAccessTo + "_QC_Comments"] = txtQcComments.Text;
        //                    }
        //                    //foreach (DataRow dr in dtContactReject.Rows)
        //                    //{
        //                    //dr["Rejection"] = sReason;
        //                    //dr["Rejection"] = "Others";
        //                    //dr["Rejection_UpdatedDate"] = GM.GetDateTime();
        //                    //dr["Rejection_UpdatedBy"] = GV.sEmployeeName;
        //                    //}

        //                    if (GV.MYsaSQL.BAL_SaveToTableMySdsQL(dtContactReject.GetChanges(DataRowState.Modified), GV.sContactTable, "Update"))
        //                        MessageBoxEx.Show(dtContactReject.Rows.Count + " Contacts marked " + btnImportType.Text + " for " + txtPath.Text.Trim(), "Campaign Manager");
        //                    // }
        //                }
        //            }
        //            else
        //                ToastNotification.Show(this, "Select date and Agent.", eToastPosition.TopRight);
        //        }
        //        else
        //        {
        //            string sImportType = string.Empty;
        //            //if (switchButtonImportType.Value)
        //            //    sImportType = "Rejection";
        //            //else
        //            //    sImportType = "Bounced";

        //            sImportType = btnImport .Text;
        //            if (txtPath.Text.Length > 0)
        //            {
        //                if (File.Exists(txtPath.Text))
        //                {
        //                    DataTable dtImport = ImportExcelXLS(txtPath.Text);
        //                    if (dtImport != null && dtImport.Rows.Count > 0)
        //                    {
        //                        if (dtImport.Columns.Contains("CONTACT_ID_P") && (dtImport.Columns.Contains(sImportType) || btnImportType.Text == "OK"))
        //                        {
        //                            frmLogin objLogin = new frmLogin();
        //                            objLogin.sEmployeeName = GV.sEmployeeName;
        //                            objLogin.sPassword = GV.sEmployeePassword;
        //                            //objLogin.dtUsers = dtAllUsers;
        //                            objLogin.frmParant = this;
        //                            if (objLogin.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && objLogin.IsUserPassed)
        //                            {
        //                                objFrmCompanyList.Bounce_Rejection_OK(dtImport, sImportType);
        //                                txtPath.Text = string.Empty;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ToastNotification.Show(this, "File does not contain 'CONTACT_ID_P' or '" + sImportType + "' columns.", eToastPosition.TopRight);
        //                            txtPath.Text = string.Empty;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ToastNotification.Show(this, "Invalid file", eToastPosition.TopRight);
        //                        txtPath.Text = string.Empty;
        //                    }
        //                }
        //                else
        //                {
        //                    ToastNotification.Show(this, "Invalid path or File does not exist", eToastPosition.TopRight);
        //                    txtPath.Text = string.Empty;
        //                }
        //            }
        //            else
        //                ToastNotification.Show(this, "Select file", eToastPosition.TopRight);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
        //        //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    }

        //    //try
        //    //{
        //    //    //string Qc_Status = "";
        //    //    if (btnImportType.Text == "SendBack")
        //    //    {
        //    //        if (txtPath.Text.Trim().Length > 0 && dateRejectionImport.Text.Length > 0)
        //    //        {
        //    //            DataTable dtContactReject = GV.MdsYSQL.BAL_ExecuteQueryMySdQL("SELECT * FROM " + GV.sContactTable + " WHERE " + GV.sAccessTo + "_UPDATED_DATE BETWEEN '" + dateRejectionImport.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dateRejectionImport.Value.ToString("yyyy-MM-dd") + " 23:59:59';");
        //    //            if (dtContactReject.Rows.Count > 0)
        //    //            {
        //    //                //frmRejectReason objfrmRejectReason = new frmRejectReason();
        //    //                //objfrmRejectReason.frmParant = this;
        //    //                //objfrmRejectReason.ShowDialog();

        //    //                //if (objfrmRejectReason.sReason.Trim().Length > 0)
        //    //                //{
        //    //                    //string sReason = objfrmRejectReason.sReason;

        //    //                /**********************************************
        //    //                 * Updated By: Mohammed Niyas
        //    //                 * Updated On: 29/09/2014
        //    //                 * Description: For update Rejection and Sendback Status
        //    //                 * *********************************************/

        //    //                //if (switchButtonSendBackType.Value)
        //    //                //    Qc_Status = "SendBack";
        //    //                //else
        //    //                //    Qc_Status = "Rejection";

        //    //                foreach (DataRow dr in dtContactReject.Rows)
        //    //                {
        //    //                    //dr["Rejection"] = Qc_Status;
        //    //                    //dr["Rejection_UpdatedDate"] = GM.GetDateTime();
        //    //                    //dr["Rejection_UpdatedBy"] = GV.sEmployeeName;

        //    //                    dr["QC_Status"] = btnImportType.Text;
        //    //                    dr["Qc_UpdatedDate"] = GM.GetDateTime();
        //    //                    dr["Qc_UpdatedBy"] = GV.sEmployeeName;
        //    //                    dr["Qc_Comments"] = txtQcComments.Text;
        //    //                }
        //    //                    //foreach (DataRow dr in dtContactReject.Rows)
        //    //                    //{
        //    //                        //dr["Rejection"] = sReason;
        //    //                        //dr["Rejection"] = "Others";
        //    //                        //dr["Rejection_UpdatedDate"] = GM.GetDateTime();
        //    //                        //dr["Rejection_UpdatedBy"] = GV.sEmployeeName;
        //    //                    //}

        //    //                    if (GV.MYSfdQL.BAL_SaveToTableMydSQL(dtContactReject.GetChanges(DataRowState.Modified), GV.sContactTable, "Update"))
        //    //                        MessageBoxEx.Show(dtContactReject.Rows.Count + " Contacts rejected for " + txtPath.Text.Trim(), "Campaign Manager");

        //    //               // }
        //    //            }
        //    //        }
        //    //        else
        //    //            ToastNotification.Show(this, "Select date and Agent.", eToastPosition.TopRight);
        //    //    }
        //    //    else
        //    //    {
        //    //        string sImportType = string.Empty;
        //    //        //if (switchButtonImportType.Value)
        //    //        //    sImportType = "Rejection";
        //    //        //else
        //    //        //    sImportType = "Bounced";

        //    //        sImportType = btnImportType.Text;

        //    //        if (txtPath.Text.Length > 0)
        //    //        {
        //    //            if (File.Exists(txtPath.Text))
        //    //            {
        //    //                DataTable dtImport = ImportExcelXLS(txtPath.Text);
        //    //                if (dtImport != null && dtImport.Rows.Count > 0)
        //    //                {
        //    //                    if (dtImport.Columns.Contains("CONTACT_ID_P") && dtImport.Columns.Contains(sImportType))
        //    //                    {
        //    //                        frmLogin objLogin = new frmLogin();
        //    //                        objLogin.sEmployeeName = GV.sEmployeeName;
        //    //                        objLogin.sPassword = GV.sEmployeePassword;
        //    //                        //objLogin.dtUsers = dtAllUsers;
        //    //                        objLogin.frmParant = this;
        //    //                        if (objLogin.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && objLogin.IsUserPassed)
        //    //                        {
        //    //                            objFrmCompanyList.Bounce_Rejection_Import(dtImport, sImportType);
        //    //                            txtPath.Text = string.Empty;
        //    //                        }
        //    //                    }
        //    //                    else
        //    //                    {
        //    //                        ToastNotification.Show(this, "File does not contain 'CONTACT_ID_P' or '" + sImportType + "' columns.", eToastPosition.TopRight);
        //    //                        txtPath.Text = string.Empty;
        //    //                    }
        //    //                }
        //    //                else
        //    //                {
        //    //                    ToastNotification.Show(this, "Invalid file", eToastPosition.TopRight);
        //    //                    txtPath.Text = string.Empty;
        //    //                }
        //    //            }
        //    //            else
        //    //            {
        //    //                ToastNotification.Show(this, "Invalid path or File does not exist", eToastPosition.TopRight);
        //    //                txtPath.Text = string.Empty;
        //    //            }
        //    //        }
        //    //        else
        //    //            ToastNotification.Show(this, "Select file", eToastPosition.TopRight);
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
        //    //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    //}
        //} 
        #endregion 

        //-----------------------------------------------------------------------------------------------------
        private void btnCount_Click(object sender, EventArgs e)
        {
            objFrmCompanyList.expandablePanelCount.Expanded = !objFrmCompanyList.expandablePanelCount.Expanded;
        }

        #region Commented Code 2
        //-----------------------------------------------------------------------------------------------------
        //private void switchButtonImportType_ValueChanged(object sender, EventArgs e)
        //{
        //    if (switchButtonImportType.Value)
        //    {
        //        btnImport.Text = "Import Rejection";
        //        switchButtonRejectionType.Visible = true;
        //        switchButtonRejectionType.Value = false; //by default
        //        switchButtonSendBackType.Visible = true;
        //        switchButtonSendBackType.Value = false;

        //        dateRejectionImport.Visible = false;
        //        controlContainerDate.Visible = false;
        //        btnImport.Text = "Import Rejection";
        //    }
        //    else
        //    {
        //        btnImport.Text = "Import Bounce";
        //        switchButtonRejectionType.Visible = false;
        //        switchButtonSendBackType.Visible = false;
        //        dateRejectionImport.Visible = false;
        //        controlContainerDate.Visible = false;
        //    }

        //    switchButtonRejectionType.Visible = false;
        //    txtPath.Text = string.Empty;
        //    dateRejectionImport.Text = string.Empty;
        //    btnImport.Refresh();
        //    itemContainerImport.Refresh();
        //    rbnBarBounce.Refresh();
        //    itemContainerComments.Refresh();

        //}

        //-----------------------------------------------------------------------------------------------------
        //private void switchButtonRejectionType_ValueChanged(object sender, EventArgs e)
        //{
        //    if (switchButtonRejectionType.Value)
        //    {
        //        dateRejectionImport.Visible = true;
        //        controlContainerDate.Visible = true;
        //        btnImport.Text = "Reject";
        //    }
        //    else
        //    {
        //        dateRejectionImport.Visible = false;
        //        controlContainerDate.Visible = false;
        //        btnImport.Text = "Import Rejection";
        //    }

        //    switchButtonRejectionType.Visible = false;
        //    txtPath.Text = string.Empty;
        //    dateRejectionImport.Text = string.Empty;
        //    controlContainerDate.Refresh();
        //    itemContainerImport.Refresh();
        //    rbnBarBounce.Refresh();

        //} 
        #endregion

        //-----------------------------------------------------------------------------------------------------
        private void btnTarget_Click_1(object sender, EventArgs e)
        {
            if (!GM.IsFormExist("frmTarget"))
            {
                objfrmTarget = new frmTarget();
                objfrmTarget.MdiParent = this;
                objfrmTarget.Show();
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void btnPerformance_Click(object sender, EventArgs e)
        {
            if (!GM.IsFormExist("frmTeamPerformance"))
            {
                objfrmTeamPerformance = new frmTeamPerformance();
                objfrmTeamPerformance.MdiParent = this;
                objfrmTeamPerformance.Show();
            }
        }

        private void statuslblProject_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (GV.sProjectID != null && GV.sProjectID.Length > 0)
                    System.Windows.Forms.Clipboard.SetText(GV.sProjectID);
            }
            catch (Exception ex)
            { }
        }

        //private void switchButtonSendBackType_ValueChanged(object sender, EventArgs e)
        //{
        //    if (switchButtonSendBackType.Value)
        //    {
        //        btnImport.Text = "SendBack";
        //        dateRejectionImport.Visible = true;
        //        controlContainerDate.Visible = true;
        //        //switchButtonRejectionType.Visible = true;
        //        //switchButtonRejectionType.Value = false; //by default
        //        //switchButtonSendBackType.Visible = true;
        //        //switchButtonSendBackType.Value = false;
        //    }
        //    else
        //    {
        //        btnImport.Text = "Import Rejection";
        //        dateRejectionImport.Visible = false;
        //        controlContainerDate.Visible = false;

        //        //switchButtonRejectionType.Visible = false;
        //        //switchButtonSendBackType.Visible = false;
        //        //dateRejectionImport.Visible = false;
        //        //controlContainerDate.Visible = false;
        //    }

        //    txtPath.Text = string.Empty;
        //    dateRejectionImport.Text = string.Empty;
        //    btnImport.Refresh();
        //    itemContainerImport.Refresh();
        //    itemContainerComments.Refresh();
        //    rbnBarBounce.Refresh();
        //}


        //private void cmbImportType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (btnImportType.Text == "Rejection" || btnImportType.Text == "Bounce")
        //    {
        //        txtPath.Text = "";
        //        dateRejectionImport.Visible = false;
        //        controlContainerDate.Visible = false;
        //        swhType.Visible = false;
        //    }
        //    else if (btnImportType.Text == "SendBack")
        //    {
        //        txtPath.Text = "";
        //        dateRejectionImport.Visible = true;
        //        controlContainerDate.Visible = true; 
        //        swhType.Visible = false;
        //    }
        //    else if (btnImportType.Text == "OK")
        //    {
        //        txtPath.Text = "";
        //        dateRejectionImport.Visible = false;
        //        controlContainerDate.Visible = false;
        //        swhType.Visible = true;
        //    }
        //    rbnBarBounce.Refresh();
        //    itemContainerImport.Refresh();
        //}

        private void swhType_ValueChanged(object sender, EventArgs e)
        {
            if (swhType.Value)
            {
                dateRejectionImport.Visible = true;
                controlContainerDate.Visible = true;
            }
            else
            {
                dateRejectionImport.Visible = false;
                controlContainerDate.Visible = false;
            }
            txtPath.Text = string.Empty;
            dateRejectionImport.Text = "";
            itemContainerPathandDate.Refresh();
        }

        private void swhType_VisibleChanged(object sender, EventArgs e)
        {
            if (swhType.Visible)
            {
                if (swhType.Value)
                {
                    dateRejectionImport.Visible = true;
                    controlContainerDate.Visible = true;
                }
                else
                {
                    dateRejectionImport.Visible = false;
                    controlContainerDate.Visible = false;
                }
                txtPath.Text = string.Empty;
                dateRejectionImport.Text = "";
                itemContainerPathandDate.Refresh();
            }
        }

        private void btnItemBounce_Click(object sender, EventArgs e)
        {
            btnImportType.Text = "Bounce";
            btnImportType.Image = Properties.Resources.email_alert_iconBig;
            btnItemBounce.Checked = true;
            btnItemSendBack.Checked = false;
            btnItemRejection.Checked = false;
            btnItemOK.Checked = false;
            btnUploadTag.Checked = false;

            swhType.Visible = false;
            dateRejectionImport.Visible = false;
            controlContainerDate.Visible = false;
            itemContainerPathandDate.Refresh();
        }

        private void btnItemSendBack_Click(object sender, EventArgs e)
        {
            btnImportType.Text = "SendBack";
            btnImportType.Image = Properties.Resources.send_iconBig;
            btnItemBounce.Checked = false;
            btnItemSendBack.Checked = true;
            btnItemRejection.Checked = false;
            btnItemOK.Checked = false;
            btnUploadTag.Checked = false;

            swhType.Visible = true;
        }

        private void btnItemRejection_Click(object sender, EventArgs e)
        {
            btnImportType.Text = "Rejection";
            btnImportType.Image = Properties.Resources.Close_iconBig;
            btnItemBounce.Checked = false;
            btnItemSendBack.Checked = false;
            btnItemRejection.Checked = true;
            btnItemOK.Checked = false;
            btnUploadTag.Checked = false;
        }

        private void btnItemOK_Click(object sender, EventArgs e)
        {
            btnImportType.Text = "OK";
            btnImportType.Image = Properties.Resources.ok_icon_Big;
            swhType.Value = false;
            btnItemBounce.Checked = false;
            btnItemSendBack.Checked = false;
            btnItemRejection.Checked = false;
            btnItemOK.Checked = true;
            btnUploadTag.Checked = false;
        }

        private void btnUploadTag_Click(object sender, EventArgs e)
        {
            btnImportType.Text = "Upload Tag";
            btnImportType.Image = Properties.Resources.tag_alt_icon;
            btnItemBounce.Checked = false;
            btnItemSendBack.Checked = false;
            btnItemRejection.Checked = false;
            btnItemOK.Checked = false;
            btnUploadTag.Checked = true;


            swhType.Visible = false;
            dateRejectionImport.Visible = false;
            controlContainerDate.Visible = false;
            itemContainerPathandDate.Refresh();
        }

        private void btnImportType_TextChanged(object sender, EventArgs e)
        {
            if (btnImportType.Text == "Rejection" || btnImportType.Text == "Bounce")
            {
                txtPath.Text = "";
                dateRejectionImport.Visible = false;
                dateRejectionImport.Text = "";
                controlContainerDate.Visible = false;
                swhType.Visible = false;
            }
            else if (btnImportType.Text == "SendBack")
            {
                txtPath.Text = "";
                dateRejectionImport.Text = "";
                dateRejectionImport.Visible = true;
                controlContainerDate.Visible = true;
                swhType.Visible = false;
            }
            else if (btnImportType.Text == "OK")
            {
                txtPath.Text = "";
                dateRejectionImport.Text = "";
                dateRejectionImport.Visible = false;
                controlContainerDate.Visible = false;
                swhType.Visible = true;
            }
            rbnBarBounce.Refresh();
            itemContainerPathandDate.Refresh();
        }

        public string SendBackCount()
        {
            try
            {
                //string sQuery = "SELECT COUNT(DISTINCT c.MASTER_ID) FROM " + GV.sCompanyTable + " c";
                //sQuery += " INNER JOIN " + GV.sContactTable + " a ON c.MASTER_ID = a.MASTER_ID";
                //sQuery += " INNER JOIN " + GV.sQCTable + " b ON ((b.TableName = 'Contact' AND a.CONTACT_ID_P = b.RecordID) OR (b.TableName='Company' AND c.MASTER_ID = b.RecordID))";
                //sQuery += " WHERE ((b.TableName ='Company' AND c." + GV.sAccessTo + "_AGENTNAME ='" + GV.sEmployeeName + "') OR (b.TableName ='Contact' AND a." + GV.sAccessTo + "_AGENT_NAME ='" + GV.sEmployeeName + "'))  AND b.QC_Status = 'SendBack' AND b.ResearchType = '" + GV.sAccessTo + "';";

                if (GV.sUserType == "Agent")
                {
                    string sQuery = "SELECT COUNT(DISTINCT A.CONTACT_ID_P) FROM ";
                    sQuery += GV.sContactTable + " a";
                    sQuery += " INNER JOIN " + GV.sQCTable +
                              " b ON (b.TableName = 'Contact' AND a.CONTACT_ID_P = b.RecordID) ";
                    sQuery += " WHERE (b.TableName ='Contact' AND a." + GV.sAccessTo + "_AGENT_NAME ='" +
                              GV.sEmployeeName + "')  AND b.QC_Status = 'SendBack' AND b.ResearchType = '" +
                              GV.sAccessTo + "';";

                    DataTable dtSendBackCount = GV.MSSQL1.BAL_ExecuteQuery(sQuery);
                    //DataTable dtSendBackCount = GV.MYdsSQL.BAL_ExecuteQueryMydSQL("SELECT COUNT(*) FROM " + GV.sContactTable + " WHERE " + GV.sAccessTo + "_QC_Status = 'SendBack' AND " + GV.sAccessTo + "_AGENT_NAME = '" + GV.sEmployeeName + "';");
                    if (dtSendBackCount != null && dtSendBackCount.Rows.Count > 0)
                    {
                        if (dtSendBackCount.Rows[0][0].ToString() == "0")
                            return string.Empty;
                        return dtSendBackCount.Rows[0][0].ToString();
                    }
                    return "R";
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return "R";
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnImportType.Text == "SendBack" || (swhType.Visible && swhType.Value))
                {//only Sendback and OK
                    if (txtPath.Text.Trim().Length > 0 && dateRejectionImport.Text.Length > 0)
                    {
                        DataTable dtContactReject;
                        //string sQuery = string.Empty;
                        //sQuery = "SELECT a.CONTACT_ID_P FROM " + GV.sContactTable + " a LEFT OUTER JOIN " + GV.sQCTable + " b ON a.CONTACT_ID_P = b.RecordID AND b.TableName = 'Contact' AND b.ResearchType = '" + GV.sAccessTo + "'";
                        //sQuery += " WHERE a." + GV.sAccessTo + "_UPDATED_DATE BETWEEN '" + dateRejectionImport.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dateRejectionImport.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                        //sQuery += " AND a." + GV.sAccessTo + "_AGENT_NAME = '" + txtPath.Text + "' AND a." + GV.sAccessTo + "_CONTACT_STATUS IN (SELECT DISTINCT Primary_Status FROM " + GV.sProjectID + "_recordstatus WHERE Research_Type = '" + GV.sAccessTo + "' AND Operation_Type LIKE '%Validate%')";

                        string sQuery = string.Empty;
                        sQuery = "SELECT a.CONTACT_ID_P FROM " + GV.sContactTable + " a LEFT OUTER JOIN " + GV.sQCTable + " b ON a.CONTACT_ID_P = b.RecordID AND b.TableName = 'Contact' AND b.ResearchType = '" + GV.sAccessTo + "'";
                        sQuery += " WHERE a." + GV.sAccessTo + "_UPDATED_DATE BETWEEN '" + dateRejectionImport.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND '" + dateRejectionImport.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                        sQuery += " AND a." + GV.sAccessTo + "_UNCERTAIN_STATUS = 0 AND a." + GV.sAccessTo + "_AGENT_NAME = '" + txtPath.Text + "'";

                        
                        //Prakash::24-04-2017::As per request from QC, they can now OK any records.
                        //if (btnImportType.Text == "OK")
                        //    sQuery += " AND (b.Qc_Status is null OR b.Qc_Status='Reprocessed')";

                        dtContactReject = GV.MSSQL1.BAL_ExecuteQuery(sQuery);
                        if (dtContactReject.Rows.Count > 0)
                        {
                            string sContactIDs = GM.ColumnToQString("CONTACT_ID_P", dtContactReject, "Int");
                            DataTable dtQCTable = GV.MSSQL1.BAL_FetchTable(GV.sQCTable, "RecordID IN (" + sContactIDs + ") AND TableName = 'Contact' AND ResearchType = '" + GV.sAccessTo + "'");

                            foreach (DataRow dr in dtContactReject.Rows)
                            {
                                string sContID = dr["Contact_ID_P"].ToString();
                                dtQCTable = objFrmCompanyList.Update_QCTable(sContID, dtContactReject, dtQCTable, "Contact", btnImportType.Text, txtQcComments.Text);
                            }

                            if (SaveToDB(dtQCTable, GV.sQCTable))
                                MessageBoxEx.Show("<b>" + dtContactReject.Rows.Count + "</b> Contacts marked <b>" + btnImportType.Text + "</b> for <b>" + txtPath.Text.Trim() + "</b>", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                                MessageBoxEx.Show("Error occured on connecting the server. Please try again.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                        ToastNotification.Show(this, "Select Date and Agent.", eToastPosition.TopRight);
                }
                else // Bounce and Rejection
                {
                    string sImportType = btnImportType.Text;
                    if (txtPath.Text.Length > 0)
                    {
                        if (File.Exists(txtPath.Text))
                        {
                            DataTable dtImport = GM.ImportExcel(txtPath.Text);
                            if (dtImport != null && dtImport.Rows.Count > 0)
                            {
                                if (dtImport.Columns.Contains("CONTACT_ID_P") && (dtImport.Columns.Contains(sImportType) || btnImportType.Text == "OK" || btnImportType.Text == "Upload Tag"))
                                {
                                    frmLogin objLogin = new frmLogin();
                                    objLogin.sEmployeeName = GV.sEmployeeName;
                                    objLogin.sPassword = GV.sEmployeePassword;
                                    //objLogin.dtUsers = dtAllUsers;
                                    objLogin.frmParant = this;
                                    if (objLogin.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && objLogin.IsUserPassed)
                                    {
                                        objFrmCompanyList.Bounce_Rejection_OK(dtImport, sImportType, txtQcComments.Text);
                                        txtPath.Text = string.Empty;
                                    }
                                }
                                else
                                {
                                    ToastNotification.Show(this, "File does not contain 'CONTACT_ID_P' or '" + sImportType + "' columns.", eToastPosition.TopRight);
                                    txtPath.Text = string.Empty;
                                }
                            }
                            else
                            {
                                ToastNotification.Show(this, "Invalid file", eToastPosition.TopRight);
                                txtPath.Text = string.Empty;
                            }
                        }
                        else
                        {
                            ToastNotification.Show(this, "Invalid path or File does not exist", eToastPosition.TopRight);
                            txtPath.Text = string.Empty;
                        }
                    }
                    else
                        ToastNotification.Show(this, "Select file", eToastPosition.TopRight);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private bool SaveToDB(DataTable dtSave, string sTableName)
        {
            try
            {
                bool IsSaveSucess = true;
                if (dtSave.GetChanges(DataRowState.Added) != null)
                    IsSaveSucess = GV.MSSQL1.BAL_SaveToTable(dtSave.GetChanges(DataRowState.Added), sTableName, "New", true);
                if (dtSave.GetChanges(DataRowState.Modified) != null)
                    IsSaveSucess = GV.MSSQL1.BAL_SaveToTable(dtSave.GetChanges(DataRowState.Modified), sTableName, "Update", true);
                if (dtSave.GetChanges(DataRowState.Deleted) != null)
                    IsSaveSucess = GV.MSSQL1.BAL_SaveToTable(dtSave.GetChanges(DataRowState.Deleted), sTableName, "Delete", true);
                return IsSaveSucess;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                return false;
            }
        }

        private void AppCommandTheme_Executed(object sender, EventArgs e)
        {
            ICommandSource source = sender as ICommandSource;
            if (source.CommandParameter is string)
            {
                eStyle style = (eStyle)Enum.Parse(typeof(eStyle), source.CommandParameter.ToString());
                // Using StyleManager change the style and color tinting
                if (StyleManager.IsMetro(style))
                {
                    // More customization is needed for Metro
                    // Capitalize App Button and tab
                    btnStart.Text = btnStart.Text.ToUpper();

                    GV.pnlGlobalColor.Style.BackColor2.Color = Color.White;

                    //foreach (BaseItem item in RibbonControl.Items)
                    //{
                    //    // Ribbon Control may contain items other than tabs so that needs to be taken in account
                    //    RibbonTabItem tab = item as RibbonTabItem;
                    //    if (tab != null)
                    //        tab.Text = tab.Text.ToUpper();
                    //}

                    btnStart.BackstageTabEnabled = true; // Use Backstage for Metro

                    ribbonMain.RibbonStripFont = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    if (style == eStyle.Metro)
                        StyleManager.MetroColorGeneratorParameters = DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters.DarkBlue;

                    // Adjust size of switch button to match Metro styling
                    switchRbnExpandCollapse.SwitchWidth = 12;
                    switchRbnExpandCollapse.ButtonWidth = 42;
                    switchRbnExpandCollapse.ButtonHeight = 19;

                    // Adjust tab strip style
                    tabStrip.Style = eTabStripStyle.Metro;

                    StyleManager.Style = style; // BOOM
                }
                else
                {
                    GV.pnlGlobalColor.CanvasColor = System.Drawing.SystemColors.Control;
                    GV.pnlGlobalColor.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
                    GV.pnlGlobalColor.DisabledBackColor = System.Drawing.Color.Empty;
                    GV.pnlGlobalColor.Size = new System.Drawing.Size(200, 100);
                    GV.pnlGlobalColor.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
                    GV.pnlGlobalColor.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
                    GV.pnlGlobalColor.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
                    GV.pnlGlobalColor.Style.GradientAngle = 90;

                    // If previous style was Metro we need to update other properties as well
                    if (StyleManager.IsMetro(StyleManager.Style))
                    {
                        ribbonMain.RibbonStripFont = null;
                        // Fix capitalization App Button and tab
                        btnStart.Text = GM.ProperCase_ProjectSpecific(btnStart.Text);
                        //foreach (BaseItem item in RibbonControl.Items)
                        //{
                        //    // Ribbon Control may contain items other than tabs so that needs to be taken in account
                        //    RibbonTabItem tab = item as RibbonTabItem;
                        //    if (tab != null)
                        //        tab.Text = GM.ProperCase(tab.Text);
                        //}
                        // Adjust size of switch button to match Office styling
                        switchRbnExpandCollapse.SwitchWidth = 28;
                        switchRbnExpandCollapse.ButtonWidth = 62;
                        switchRbnExpandCollapse.ButtonHeight = 20;
                    }

                    // Adjust tab strip style
                    tabStrip.Style = eTabStripStyle.Office2007Document;
                    StyleManager.ChangeStyle(style, Color.Empty);
                    if (style == eStyle.Office2007Black || style == eStyle.Office2007Blue || style == eStyle.Office2007Silver || style == eStyle.Office2007VistaGlass)
                        btnStart.BackstageTabEnabled = false;
                    else
                        btnStart.BackstageTabEnabled = true;
                }
            }
            else if (source.CommandParameter is Color)
            {
                if (StyleManager.IsMetro(StyleManager.Style))
                    StyleManager.MetroColorGeneratorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(Color.White, (Color)source.CommandParameter);
                else
                    StyleManager.ColorTint = (Color)source.CommandParameter;
            }
        }

        private void buttonStyleCustom_ColorPreview(object sender, ColorPreviewEventArgs e)
        {
            if (StyleManager.IsMetro(StyleManager.Style))
            {
                Color baseColor = e.Color;
                StyleManager.MetroColorGeneratorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(Color.White, baseColor);
            }
            else
                StyleManager.ColorTint = e.Color;
        }

        private void buttonStyleCustom_EnabledChanged(object sender, EventArgs e)
        {
            if (buttonStyleCustom.Expanded)
            {
                // Remember the starting color scheme to apply if no color is selected during live-preview
                m_ColorSelected = false;
                m_BaseStyle = StyleManager.Style;
            }
            else
            {
                if (!m_ColorSelected)
                {
                    if (StyleManager.IsMetro(StyleManager.Style))
                        StyleManager.MetroColorGeneratorParameters = DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters.Default;
                    else
                        StyleManager.ChangeStyle(m_BaseStyle, Color.Empty);
                }
            }
        }

        private void buttonStyleCustom_SelectedColorChanged(object sender, EventArgs e)
        {
            m_ColorSelected = true; // Indicate that color was selected for buttonStyleCustom_ExpandChange method
            buttonStyleCustom.CommandParameter = buttonStyleCustom.SelectedColor;
        }

        private void btnSendBackInfo_Click(object sender, EventArgs e)
        {

            if (GV.sUserType != "Agent")
            {
                if (!GM.IsFormExist("frmSendback"))
                {

                    frmSendback objfrmSendback = new frmSendback();
                    objfrmSendback.MdiParent = this;
                    objfrmSendback.IsLastMonth = false;
                    objfrmSendback.Show();
                }
            }
        }

        private void btnSamplingCalc_Click(object sender, EventArgs e)
        {
            // GM.HRIMSWeekID(txtQCSampleAgent.Text, dateQCSampleDate.Value);
            objfrmQC.Load_QC(dateQCSampleDate.Value.ToString("yyyy-MM-dd"));
        }

        private void btnSendBackPrevious_Click(object sender, EventArgs e)
        {
            if (GV.sUserType != "Agent")
            {
                if (!GM.IsFormExist("frmSendback"))
                {

                    frmSendback objfrmSendback = new frmSendback();
                    objfrmSendback.MdiParent = this;
                    objfrmSendback.IsLastMonth = true;
                    objfrmSendback.Show();

                }
            }
        }

        private void rbnTabQC_Click(object sender, EventArgs e)
        {
            OpenQC();
        }

        private void tabStrip_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
        {
            if (e.NewTab != null)
            {
                if (e.NewTab.AttachedControl.Name == "frmQC" && ribbonMain.SelectedRibbonTabItem != rbnTabQC)
                {
                    ribbonMain.SelectedRibbonTabItem = rbnTabQC;
                    return;
                }

                if (e.NewTab.AttachedControl.Name == "frmCompanyList" &&
                    ribbonMain.SelectedRibbonTabItem != rbnTabProcess)
                {
                    ribbonMain.SelectedRibbonTabItem = rbnTabProcess;
                    return;
                }

                if (e.NewTab.AttachedControl.Name == "frmSearch" && ribbonMain.SelectedRibbonTabItem != rbnTabProcess)
                {
                    ribbonMain.SelectedRibbonTabItem = rbnTabProcess;
                    return;
                }

                if (e.NewTab.AttachedControl.Name == "frmUncertain" && ribbonMain.SelectedRibbonTabItem != rbnTabProcess)
                {
                    ribbonMain.SelectedRibbonTabItem = rbnTabProcess;
                }

                if (e.NewTab.AttachedControl.Name == "frmScrapper" && ribbonMain.SelectedRibbonTabItem != rbnTabProcess)
                {
                    ribbonMain.SelectedRibbonTabItem = rbnTabProcess;
                    return;
                }

                if (e.NewTab.AttachedControl.Name == "frmScrapedContacts" &&
                    ribbonMain.SelectedRibbonTabItem != rbnTabProcess)
                {
                    ribbonMain.SelectedRibbonTabItem = rbnTabProcess;
                    return;
                }

                if (e.NewTab.AttachedControl.Name == "frmEmailChecks" &&
                    ribbonMain.SelectedRibbonTabItem != rbnTabProcess)
                {
                    ribbonMain.SelectedRibbonTabItem = rbnTabProcess;
                    return;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!GM.IsFormExist("frmSearch"))
            {
                //using (frmSearch objfrmSearch = new frmSearch())
                {
                    frmSearch objfrmSearch = new frmSearch();
                    objfrmSearch.WindowState = FormWindowState.Maximized;
                    objfrmSearch.drCompany = null;
                    objfrmSearch.drContact = null;
                    objfrmSearch.StartPosition = FormStartPosition.CenterScreen;
                    objfrmSearch.sMaster_ID = string.Empty;
                    objfrmSearch.TableToReturn = "Contact";
                    objfrmSearch.SearchTriggeredFrom = "External";
                    objfrmSearch.iCompanyRowIndex = -1;
                    objfrmSearch.iContactRowIndex = -1;
                    objfrmSearch.dtFieldMasterCompany = dtFieldMaster.Select("TABLE_NAME = 'Master' AND ACTIVE_COLUMN= 'Y' AND Visibility Like '%" + GV.sAccessTo + "%'").CopyToDataTable();
                    objfrmSearch.dtFieldMasterContact = dtFieldMaster.Select("TABLE_NAME = 'MasterContacts' AND ACTIVE_COLUMN= 'Y' AND Visibility Like '%" + GV.sAccessTo + "%' ").CopyToDataTable(); ;
                    objfrmSearch.MdiParent = this;
                    objfrmSearch.Show();
                }
            }

            //dtFieldMasterCompany = dtFieldMaster.Select("TABLE_NAME = 'Master'").CopyToDataTable(); //Master Table Fields            
            //dtFieldMasterContact = dtFieldMaster.Select("TABLE_NAME = 'MasterContacts'").CopyToDataTable(); //MasterContact Table Fields            
        }

        private void cmbDialerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GV.sDialerType = cmbDialerType.SelectedItem.ToString();

            if (GV.sDialerType == "iSystem" || GV.sDialerType == "X-Lite")
            {
                txtExtension.Text = GV.sExtensionNumber;
                //txtExtension.Visible = true;
                //btnDialerLogin.Visible = false;
            }
            else if (GV.sDialerType == "Vortex")
            {
                txtExtension.Text = GV.sVortexExtension;
                //txtExtension.Visible = false;
                //btnDialerLogin.Visible = true;
                //if(GV.mDialer.IsConnected)                
                //    btnDialerLogin.Text = "Logged";
                //else
                //    btnDialerLogin.Text = "Login";

            }


        }

        private void ProcessTimer_Tick(object sender, EventArgs e)
        {

            bool IsNewApplicationDetected = false;
            IntPtr hwnd = ProcessWatch.getforegroundWindow();
            Int32 pid = ProcessWatch.GetWindowProcessID(hwnd);
            Process p = Process.GetProcessById(pid);
            sProcessName = p.ProcessName;
            sAppTitle = Encoding.UTF8.GetString(Encoding.Default.GetBytes(ProcessWatch.ActiveApplTitle().Trim().Replace("\0", "")));
            try
            {
                if (dtSession.Select("AppTitle = '" + sAppTitle.Replace("'", "''") + "' AND AppProcess = '" + sProcessName.Replace("'", "''") + "' AND CompanySessionID = '" + GV.sCompanySessionID + "'").Length == 0)
                {
                    DataRow drNewRow = dtSession.NewRow();
                    drNewRow["AppTitle"] = sAppTitle;
                    drNewRow["AppProcess"] = sProcessName;
                    drNewRow["SessionID"] = GV.sSessionID;
                    drNewRow["CompanySessionID"] = GV.sCompanySessionID;
                    drNewRow["TimeTaken"] = "0";
                    dtSession.Rows.Add(drNewRow);
                    IsNewApplicationDetected = true;
                }

                if ((sPreviousAppTitle + sPreviousProcessName + sPreviousCompanySessionID) != (sAppTitle + sProcessName + GV.sCompanySessionID))
                {
                    tApplicationFocusIntervel = DateTime.Now.Subtract(dApplicationFocusTime);
                    foreach (DataRow dr in dtSession.Rows)
                    {
                        if (dr["AppTitle"].ToString() == sPreviousAppTitle && dr["AppProcess"].ToString() == sPreviousProcessName && dr["CompanySessionID"].ToString() == sPreviousCompanySessionID)
                        {
                            double prevseconds = Convert.ToDouble(dr["TimeTaken"]);
                            dr["TimeTaken"] = (tApplicationFocusIntervel.TotalSeconds + prevseconds);
                            break;
                        }
                    }

                    sPreviousAppTitle = sAppTitle;
                    sPreviousProcessName = sProcessName;
                    sPreviousCompanySessionID = GV.sCompanySessionID;
                    dApplicationFocusTime = DateTime.Now;
                }

                if (IsNewApplicationDetected)
                    dApplicationFocusTime = DateTime.Now;

                iTimer++;

                if (iTimer > 599)
                {
                    Session(dtSession);
                    iTimer = 0;
                }


                //if (sCaptureJob != null && sCaptureJob.Status != RecordStatus.Running)
                //{
                //    string sBasePath = AppDomain.CurrentDomain.BaseDirectory;

                //    if (File.Exists(string.Format(sBasePath + "\\" + GV.sCurrentCaptureName + ".wmv")))
                //    {
                //        if (GV.sCurrentCaptureName == GV.sErrorCaptureName)
                //            File.Move(string.Format(sBasePath + "\\" + GV.sCurrentCaptureName + ".wmv"), string.Format(@"\\172.27.137.182\Campaign Manager\Exceptions\Capture\" + GV.sCurrentCaptureName + ".wmv"));
                //        else
                //            File.Delete(string.Format(sBasePath + "\\" + GV.sCurrentCaptureName + ".wmv"));
                //    }

                //    GV.sErrorCaptureName = string.Empty;
                //    sCaptureJob.Duration = TimeSpan.FromSeconds(60);                    
                //    GV.sCurrentCaptureName = GV.sEmployeeName + "_" + GV.sProjectID + "_" + GM.IP().Replace(".", string.Empty).Reverse() + GM.GetDateTime().ToString("yyMMddHHmmssff");
                //    sCaptureJob.OutputScreenCaptureFileName = string.Format(sBasePath + "\\" + GV.sCurrentCaptureName + ".wmv");
                //    sCaptureJob.Start();
                //}

            }
            catch (Exception ex)
            {
                //StreamWriter sWrite = new StreamWriter(@"\\172.27.137.182\Campaign Manager\Exceptions\Sessions.txt", true);
                //sWrite.WriteLine("AppTitle : " + sAppTitle + Environment.NewLine + "ProcessName : " + sProcessName);
                //sWrite.Close();
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);

            }
            finally
            {
                p.Dispose();
                p = null;
            }
        }

        void Session(DataTable dtSes)
        {
            if (dtSes.Rows.Count > 0)
            {
                string sInsertValues = string.Empty;
                foreach (DataRow drSessions in dtSes.Rows)
                {
                    if (sInsertValues.Length > 0)
                        sInsertValues += ",('" + drSessions["SessionID"] + "', '" + GM.RemoveEndBackSlash(drSessions["AppTitle"].ToString().Replace("'", "''")) + "' , '" + GM.RemoveEndBackSlash(drSessions["AppProcess"].ToString().Replace("'", "''")) + "', '" + drSessions["CompanySessionID"] + "' , " + drSessions["TimeTaken"] + ")";
                    else
                        sInsertValues = "('" + drSessions["SessionID"] + "', '" + GM.RemoveEndBackSlash(drSessions["AppTitle"].ToString().Replace("'", "''")) + "' , '" + GM.RemoveEndBackSlash(drSessions["AppProcess"].ToString().Replace("'", "''")) + "', '" + drSessions["CompanySessionID"] + "' , " + drSessions["TimeTaken"] + ")";
                }

                try
                {
                    //GV.MYaSQL.BAL_ExecuteNonReturnQueryMydSQL_ExclusiveCon("DELETE FROM c_sessions WHERE SessionID = '" + GV.sSessionID + "';");
                    GV.MSSQL1.BAL_ExecuteNonReturnQuery_ExclusiveCon("DELETE FROM c_sessions WHERE SessionID = '" + GV.sSessionID + "'; INSERT INTO c_sessions (SessionID, AppTitle, AppProcess, CompanySessionID, TimeTaken) Values " + sInsertValues + ";");
                }
                catch (Exception ex)
                {/*Do Nothing - Supress errror for Sessions */ }

            }
        }

        private void TellExpressionEncoderWhereItIs()
        {
            /*
             * Because expression encoder expects it's going to be installed through the standard installer, and not through our surreptitious means,
             * it expects a regestry key. If it doesn't find the key it breaks. So we will put the key in before it has a chance to check.
             */

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\en"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\en");

            if (!(File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\en\\Microsoft.Expression.Encoder.resources.dll")))
                File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "\\en\\Microsoft.Expression.Encoder.resources.dll", Properties.Resources.Microsoft_Expression_Encoder_resources);

            if (!(File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Microsoft.Expression.Encoder.EEScreen.dll")))
                File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "\\Microsoft.Expression.Encoder.EEScreen.dll", Properties.Resources.Microsoft_Expression_Encoder_EEScreen);


            var key = "SOFTWARE\\Microsoft\\Expression\\Encoder\\4.0";


            using (var registryKey = Registry.LocalMachine.OpenSubKey(key))
            {
                if (registryKey == null)
                {
                    using (var newKey = Registry.LocalMachine.CreateSubKey(key))
                    {
                        CheckInstallKey(newKey);
                    }
                }
                else
                {
                    CheckInstallKey(registryKey);
                }
            }
        }

        private void CheckInstallKey(RegistryKey registryKey)
        {
            var path = Environment.CurrentDirectory;
            var installKey = "InstallPath";

            if (registryKey != null)
            {
                string text = registryKey.GetValue(installKey) as string;
                if (string.IsNullOrEmpty(text))
                {
                    registryKey.SetValue(installKey, path);
                }
            }
        }

        private void btnUncertain_Click(object sender, EventArgs e)
        {
            if (!GM.IsFormExist("frmUncertain") && GV.sUserType != "Agent")
            {
                frmUncertain objfrmUncertain = new frmUncertain();
                objfrmUncertain.MdiParent = this;
                objfrmUncertain.dtField_Master = objFrmCompanyList.dtUncertainFields;
                objfrmUncertain.Show();
            }
        }

        private void btnEmailCheck_Click(object sender, EventArgs e)
        {
            if (IsTargetSet())
            {
                if (!GM.IsFormExist("frmEmailChecks"))
                {
                    frmEmailChecks objfrmEmailChecks = new frmEmailChecks();
                    objfrmEmailChecks.MdiParent = this;
                    objfrmEmailChecks.Show();
                }

                //if (!GM.IsFormExist("frmCompanyImport"))
                //{
                //    frmCompanyImport obj = new frmCompanyImport();
                //    obj.MdiParent = this;
                //    obj.dtScrapperSettings = objFrmCompanyList.dtScrapperSettings;
                //    obj.dtCountry = objFrmCompanyList.dtCountryInformation;
                //    obj.Show();
                //}
            }
            else
            {
                ToastNotification.Show(this, "Today's Target not set.", eToastPosition.TopRight);
                if (!GM.IsFormExist("frmTarget"))
                {
                    objfrmTarget = new frmTarget();
                    objfrmTarget.MdiParent = this;
                    objfrmTarget.Show();
                }
            }
        }

        private void btnScrapper_Click(object sender, EventArgs e)
        {
            if (!GM.IsFormExist("frmScrapper"))
            {
                frmScrapper objCompanyImport = new frmScrapper();
                objCompanyImport.ParantForm = this;
                objCompanyImport.MdiParent = this;
                objCompanyImport.dtScrapperSettings = objFrmCompanyList.dtScrapperSettings;
                objCompanyImport.dtCountry = objFrmCompanyList.dtCountryInformation;
                objCompanyImport.Show();
            }
        }

        private void btnScrapperProcessed_Click(object sender, EventArgs e)
        {
            if (!GM.IsFormExist("frmScrapedContacts"))
            {
                frmScrapedContacts objScrapedContacts = new frmScrapedContacts();
                objScrapedContacts.MdiParent = this;
                objScrapedContacts.Show();
            }
        }

        private void btnEmonitor_Click(object sender, EventArgs e)
        {
            frmMoniter fEmonitor = new frmMoniter();
            fEmonitor.ShowDialog();
        }

        private void timerNotification_Tick(object sender, EventArgs e)
        {
            //using (DataTable dtNotification = GV.MYdSQL.BAL_ExecuteQueryMydSQL("SELECT CREATED_BY,SUBJECT FROM c_project_instructions WHERE PROJECTID IN ('" + GV.sProjectID + "', 'ALL') AND USER_LEVEL IN ('" + GV.sUserType.ToUpper() + "','ALL') AND RESEARCH_TYPE IN ('" + GV.sAccessTo + "','ALL') AND NOTIFY_USER = 'Y' AND IFNULL(USER_READ,'') NOT LIKE '%|" + GV.sEmployeeNo + "~%' ORDER BY RAND() LIMIT 1;"))
            //{
            //    if (dtNotification.Rows.Count > 0)                
            //        tNotifier.Show(dtNotification.Rows[0]["CREATED_BY"].ToString(), dtNotification.Rows[0]["Subject"].ToString(), 500, 3000, 500);                
            //}
        }

        private void btnChromeExt_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("chrome.exe", "https://chrome.google.com/webstore/detail/campaign-manager/edennfbknmkoomkfinpciofgdcamangh");
            }
            catch (Exception ex)
            {
                //GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);
                MessageBoxEx.Show("Looks like problem with Chrome.<br/>Make sure Chrome is installed.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }

        void VortexEvents(object sender, VortexDial.Vortex.PhoneEventArgs e)
        {
            String sMessage = e.Data;

           // MessageBox.Show(e.Data);
            if (sMessage.Trim().Length > 0)
            {
                List<string> lstMessage = sMessage.Split('|').ToList();
                switch (lstMessage[0].Trim())
                {
                    case "Initialized":
                    case "InitializeError":
                    case "Registration":
                    case "RegistrationFailed":
                    case "UnRegister":
                        if(lstMessage[1].ToUpper().Contains("FAILURE") || lstMessage[1].ToUpper().Contains("FAILED"))
                        {
                            string sWriteError = "Vortex Error : " + GM.GetDateTime() + " | "+ lstMessage[1] + Environment.NewLine + Environment.NewLine;
                            GM.WriteLog(@"\\172.27.137.182\Campaign Manager\Exceptions\" + GV.sEmployeeName + ".txt", sWriteError, true);
                            ((FrmContactsUpdate)GM.GetForm("FrmContactsUpdate")).PhoneNotiier("Connection");
                        }

                        if (lstMessage[1].Contains("Registration success"))                        
                            ((FrmContactsUpdate)GM.GetForm("FrmContactsUpdate")).PhoneNotiier("VortReady");                        
                        break;

                    case "OnCall":
                        if (lstMessage[2] == "Trying" || lstMessage[2] == "Session Progress")
                            ((FrmContactsUpdate)GM.GetForm("FrmContactsUpdate")).PhoneNotiier("Calling");
                        else if (lstMessage[2] == "Ringing")
                            ((FrmContactsUpdate)GM.GetForm("FrmContactsUpdate")).PhoneNotiier("Ringing");
                        break;                    
                    case "CallAnswered":
                        ((FrmContactsUpdate)GM.GetForm("FrmContactsUpdate")).PhoneNotiier("Answered");
                        break;
                    case "CallHangup":
                        ((FrmContactsUpdate)GM.GetForm("FrmContactsUpdate")).PhoneNotiier("Ended");
                        break;
                }
            }
        }

        private void btnProjectSettings_Click(object sender, EventArgs e)
        {

        }

        private void btnFieldSettings_Click(object sender, EventArgs e)
        {

        }

        private void btnValidation_Click(object sender, EventArgs e)
        {

        }

        private void bWorkerEmailValidation_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            objFrmCompanyList.bg_EmailValidation();
        }

        private void bWorkerRDP_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            objFrmCompanyList.RDP();
        }

        private void btnRemote_Click(object sender, EventArgs e)
        {
            frmRDPMonitor objfrmRDPMonitor = new frmRDPMonitor();
            objfrmRDPMonitor.MdiParent = this;
            objfrmRDPMonitor.Show();
        }

        public class WrapperImpersonationContext
        {
            [DllImport("advapi32.dll", SetLastError = true)]
            public static extern bool LogonUser(String lpszUsername, String lpszDomain,
            String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public extern static bool CloseHandle(IntPtr handle);

            private const int LOGON32_PROVIDER_DEFAULT = 0;
            private const int LOGON32_LOGON_INTERACTIVE = 2;

            private string m_Domain;
            private string m_Password;
            private string m_Username;
            private IntPtr m_Token;

            private WindowsImpersonationContext m_Context = null;


            protected bool IsInContext
            {
                get { return m_Context != null; }
            }

            public WrapperImpersonationContext(string domain, string username, string password)
            {
                m_Domain = domain;
                m_Username = username;
                m_Password = password;
            }

            [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
            public void Enter()
            {
                if (this.IsInContext) return;
                m_Token = new IntPtr(0);
                try
                {
                    m_Token = IntPtr.Zero;
                    bool logonSuccessfull = LogonUser(
                       m_Username,
                       m_Domain,
                       m_Password,
                       LOGON32_LOGON_INTERACTIVE,
                       LOGON32_PROVIDER_DEFAULT,
                       ref m_Token);
                    if (logonSuccessfull == false)
                    {
                        int error = Marshal.GetLastWin32Error();
                        //throw new Win32Exception(error);
                        throw new ApplicationException(string.Format("Could not impersonate the elevated user.  LogonUser returned error code {0}.", error));
                    }
                    WindowsIdentity identity = new WindowsIdentity(m_Token);
                    m_Context = identity.Impersonate();
                }
                catch (Exception exception)
                {
                    // Catch exceptions here
                }
            }


            [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
            public void Leave()
            {
                if (this.IsInContext == false) return;
                m_Context.Undo();

                if (m_Token != IntPtr.Zero) CloseHandle(m_Token);
                m_Context = null;
            }
        }

        private void btnHunter_Click(object sender, EventArgs e)
        {
            using (DataTable dtHunterUser = GV.MSSQL1.BAL_ExecuteQuery("select 1 from c_picklists where PicklistCategory='HunterUser' and PicklistValue = '" + GV.sEmployeeName + "';"))
            {
                if (dtHunterUser.Rows.Count > 0)
                {
                    if (!GM.IsFormExist("frmEmailHunter"))
                    {
                        frmEmailHunter objfrmEmailHunter = new frmEmailHunter();
                        objfrmEmailHunter.MdiParent = this;
                        objfrmEmailHunter.Show();
                    }
                }
                else
                    ToastNotification.Show(this, "Access Denied", eToastPosition.TopRight);
            }
        }
    }
}




