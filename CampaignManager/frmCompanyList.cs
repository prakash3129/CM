using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace GCC
{
    //-----------------------------------------------------------------------------------------------------
    public partial class frmCompanyList : DevComponents.DotNetBar.Office2007Form
    {
        //-----------------------------------------------------------------------------------------------------
        public frmCompanyList()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new System.Drawing.Font(this.Font.FontFamily, 22);
            MessageBoxEx.EnableGlass = false;
            expandablePanelExport.Expanded = false;
            GV.sSQLCECompanyTable = GV.sEmployeeName.Replace(" ", string.Empty) + "_" + GV.sCompanyTable;
            GV.sSQLCEContactTable = GV.sEmployeeName.Replace(" ", string.Empty) + "_" + GV.sContactTable;
            
            dgvCompanyList.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
            dgvCompanySearch.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
            rdoContactProcessed.ForeColor = GV.pnlGlobalColor.Style.ForeColor.Color;
            rdoCompaniesProcessed.ForeColor = GV.pnlGlobalColor.Style.ForeColor.Color;
            lblresearchType.ForeColor = GV.pnlGlobalColor.Style.ForeColor.Color;
            lblProcessedOn.ForeColor = GV.pnlGlobalColor.Style.ForeColor.Color;

            lstMovedToSource.ForeColor = GV.pnlGlobalColor.Style.ForeColor.Color;
            lstMovedToSource.BackColor = GV.pnlGlobalColor.Style.BackColor1.Color;

            lstMovedToSource.HeaderStyle = ColumnHeaderStyle.None;
            dtp.Format = DateTimePickerFormat.Short;
            cmbCriteria.Items.Add(string.Empty);
            cmbCriteria.Items.Add("Equals");
            cmbCriteria.Items.Add("Not Equals");
            cmbCriteria.Items.Add("Contains");
            splitCompanyList.Panel1Collapsed = true;
            RuntimeEvents();
        }        

        private Form _ParantForm;

        //-----------------------------------------------------------------------------------------------------
        public Form ParantForm /////List of Columns to display on search window/////
        {
            get { return _ParantForm; }
            set { _ParantForm = value; }
        }        
        
        DataTable dtSearch = new DataTable();
        ComboBox cmbCriteria = new ComboBox();
        DateTimePicker dtp = new DateTimePicker();
        DataTable dtMasterCompany = new DataTable();
        DataTable dtMasterContact = new DataTable();
        
        DataTable dtQC = new DataTable();
        DataTable dtSelectResult = new DataTable();
        DataTable dtQCTable = new DataTable();
        DataTable dtProjectAllColumns = null;
        DataTable dtProjectExportColumns = null;
        DataTable dtFieldMaster_CriteriaColumns = null;
        private DataTable dtMoveToSource = null;
        DataTable dtbg_EmailValidation = null;
        

        string sExcelExportQuery = string.Empty;
        string sExcelExportColumns = string.Empty;
        string sExcelExportPath = string.Empty;
        int iExcelExportStatus = 0;

        public DataTable dtFieldMasterAllColumns;
        public DataTable dtFieldMaster_Active;
        public DataTable dtFieldMasterCompany;
        public DataTable dtFieldMasterContact;
        public DataTable dtFieldMasterChromeCols;
        public DataTable dtUncertainFields;
        public DataTable dtPicklist;
        public DataTable dtValidations;
        public DataTable dtPreUpdate;
        public DataTable dtEmailSuggestion;
        public DataTable dtDialConfig;
        public DataTable dtRecordStatus;
        public DataTable dtSpellIgnore;
        public DataTable dtRecordStatusRevenue;
        public DataTable dtCountryInformation;
        public DataTable dtQCPicklist;
        public DataTable dtScrapperSettings;
        public DataTable dtBlock;        
        public byte[] EAF = null;
        public string sChromeColumnSettings = string.Empty;

        int iRowIndex = -1;

        //void InitilaizeSQLCE_Tables(string sTableName)
        //{
        //    try
        //    {
        //        string sSQLCETableName = GV.sEmployeeName.Replace(" ", string.Empty) + "_" + sTableName;
        //        bool IsTablePassed = true;
        //        DataTable dtSchema = GV.SQLCE.BAL_FetchTable("information_schema.tables", "TABLE_NAME ='" + sSQLCETableName + "'");
        //        DataTable dtCETableInfo = GV.SQLCE.BAL_FetchTable("TableInfo", "Table_Name = '" + sSQLCETableName + "'");
        //        DataTable dtSourceTableColumns = GV.MYSdQL.BAL_ExecuteQueryMydSQL("SELECT C.COLUMN_NAME,C.DATA_TYPE,C.CHARACTER_MAXIMUM_LENGTH,IFNULL(C.NUMERIC_PRECISION,'0') AS NUMERIC_PRECISION,IFNULL(C.NUMERIC_SCALE,'0') AS NUMERIC_SCALE FROM information_schema.COLUMNS C WHERE C.TABLE_SCHEMA='mvgc' AND C.TABLE_NAME='" + sTableName + "'");

        //        if (dtSchema.Rows.Count > 0)
        //        {
        //            if (dtCETableInfo.Rows.Count > 0 && dtSourceTableColumns.Rows.Count == (Convert.ToInt32(dtCETableInfo.Rows[0]["ColumnCount"])))
        //            {
        //                if (Convert.ToDateTime(dtCETableInfo.Rows[0]["Created_Date"]).ToShortDateString() == GM.GetDateTime().ToShortDateString())
        //                {
        //                    DataTable dtCETable = GV.SQLCE.BAL_FetchTable(sSQLCETableName, "1=0");
                            
        //                    foreach (DataRow drSourceColumns in dtSourceTableColumns.Rows)
        //                    {
        //                        if (!dtCETable.Columns.Contains(drSourceColumns["Column_Name"].ToString()))
        //                        {
        //                            IsTablePassed = false;
        //                            break;
        //                        }
        //                    }
        //                }
        //                else
        //                    IsTablePassed = false;
        //            }
        //            else
        //                IsTablePassed = false;
        //        }
        //        else
        //            IsTablePassed = false;

        //        if (!IsTablePassed)
        //        {
        //            if (dtSchema.Rows.Count > 0)
        //                GV.SQLCE.BAL_ExecuteQueryNonReturn("Drop Table " + sSQLCETableName);
        //            GV.SQLCE.BAL_ExecuteQueryNonReturn("delete from tableinfo where Table_Name='" + sSQLCETableName + "'");
        //            string sQueryTableCreation = BuildCreateQuery(dtSourceTableColumns, sSQLCETableName);
        //            GV.SQLCE.BAL_ExecuteQueryNonReturn(sQueryTableCreation);
        //            GV.SQLCE.BAL_ExecuteQueryNonReturn("insert into TableInfo(Table_Name,ColumnCount,Created_Date) Values('" + sSQLCETableName + "'," + dtSourceTableColumns.Rows.Count + ",'" + GM.GetDateTime().ToString("yyyy-MM-dd hh:mm:ss") + "')");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
        //    }
        //}

        //-----------------------------------------------------------------------------------------------------
        private string BuildCreateQuery(DataTable dtColumns, string sTableName)
        {
            try
            {
                string sQuery = string.Empty;
                foreach (DataRow drColumns in dtColumns.Rows)
                    sQuery += "," + drColumns["Column_Name"].ToString() + " " + DataType_Transformation(drColumns);

                if (sQuery.Replace(",", string.Empty).Length > 0)
                {
                    return "CREATE TABLE " + sTableName + "(SQLCEID INTEGER IDENTITY(1,1) PRIMARY KEY" + sQuery + ")";
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                return string.Empty;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private string DataType_Transformation(DataRow drColumn)
        {
            switch (drColumn["DATA_TYPE"].ToString().ToLower())
            {
                case "int":
                case "tinyint":
                    return "INT";

                case "varchar":
                    if (Convert.ToInt32(drColumn["CHARACTER_MAXIMUM_LENGTH"]) < 4000)
                        return "NVARCHAR(" + drColumn["CHARACTER_MAXIMUM_LENGTH"].ToString() + ")";
                    return "NTEXT";

                case "char":
                    if (Convert.ToInt32(drColumn["CHARACTER_MAXIMUM_LENGTH"]) < 4000)
                        return "NCHAR(" + drColumn["CHARACTER_MAXIMUM_LENGTH"].ToString() + ")";                    
                    return "NTEXT";

                case "timestamp":
                case "datetime":
                    return "DATETIME";

                case "decimal":
                case "double":
                    return "NUMERIC(" + drColumn["NUMERIC_PRECISION"].ToString() + "," + drColumn["NUMERIC_SCALE"].ToString() + ")";

                case "text":
                    return "NTEXT";

                case "blob":
                    return "IMAGE";

                case "date":
                    return "DATE";

                case "bigint":
                    return "BIGINT";

                default:
                    return "NTEXT";
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void frmCompanyList_Load(object sender, EventArgs e)
        {

            try
            {
                //if (GV.SQLCE.BAL_FetchTable("information_schema.tables", "TABLE_NAME ='TableInfo'").Rows.Count == 0)
                //    GV.SQLCE.BAL_ExecuteQuery("CREATE TABLE TableInfo(ID INTEGER IDENTITY (1, 1) PRIMARY KEY,Table_Name NVARCHAR(1000),ColumnCount INT,Created_Date DATETIME);");

                //InitilaizeSQLCE_Tables(GV.sCompanyTable);
                //InitilaizeSQLCE_Tables(GV.sContactTable);

                //dtFieldMasterAllColumns = new DataTable();


                //((frmMain)ParantForm).ribbonPanelProcess.Refresh();

                
                //splitContainerCount.BackColor = Color.Transparent;
                splitContainerCount.IsSplitterFixed = true;
                splitContainerCount.SplitterWidth = 1;

                expandablePanelCount.Expanded = false;
                expandablePanelRecordMovement.Expanded = false;
                ((frmMain)ParantForm).itemContainerProcessedAndNewCompany.Refresh();

                ((frmMain) ParantForm).progressBar.Value = 3;

                //if (GV.sEmployeeName == "THANGAPRAKASH" || GV.HasAdminPermission)
                //{
                //    ((frmMain)ParantForm).btnEmailCheck.Visible = true;
                //    //((frmMain)ParantForm).rbnBarScrapper.Visible = true;

                //}
                //else
                //{
                //    ((frmMain)ParantForm).btnEmailCheck.Visible = false;
                //    //((frmMain)ParantForm).rbnBarScrapper.Visible = false;
                //}

                //Load_Tables();
                                
                //Clear_ResultSet();                
                
                //RebuildSearchGrid();
                
                //Get_ProjectColumns(true,true);                                
                ((frmMain)ParantForm).ribbonPanelProcess.Enabled = false;
                ((frmMain)ParantForm).ribbonPanelLogin.Enabled = false;
                bWorkerLoadTables.RunWorkerAsync();

                //ColorizeDGV();
                //this.WindowState = FormWindowState.Maximized;
                
                

                ((frmMain)ParantForm).txtQuery.Visible = GV.HasAdminPermission;
                ((frmMain)ParantForm).txtQuery.Refresh();
                ((frmMain)ParantForm).ribbonMain.Refresh();                



            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        private void Load_Movement_Source()
        {
            if (GV.sUserType != "Agent")
            {
                //lstMovedToSource.View = View.Details;
                lstMovedToSource.Columns.Add("Existing Source", lstMovedToSource.Width - 4, HorizontalAlignment.Left);
                lstMovedToSource.Columns[0].Text = "Existing Source";
                
                lstMovedToSource.Items.Add("(New Source)");
                foreach (DataRow drMoveToSource in dtMoveToSource.Rows)
                {
                    if (drMoveToSource[0].ToString().Length > 0)
                        lstMovedToSource.Items.Add(drMoveToSource[0].ToString());
                }
            }
        }

        //private void lstColumnsToExport_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (lstColumnsToExport.SelectedItem == null) return;
        //    lstColumnsToExport.DoDragDrop(lstColumnsToExport.SelectedItem, DragDropEffects.Move);
        //}

        //private void lstColumnsToExport_DragOver(object sender, DragEventArgs e)
        //{
        //    e.Effect = DragDropEffects.Move;
        //}

        //private void lstColumnsToExport_DragDrop(object sender, DragEventArgs e)
        //{
        //    Point point = lstColumnsToExport.PointToClient(new Point(e.X, e.Y));
        //    int index = lstColumnsToExport.IndexFromPoint(point);
        //    if (index < 0) index = lstColumnsToExport.Items.Count - 1;
        //    object data = e.Data.GetData(typeof(string));
        //    lstColumnsToExport.Items.Remove(data);
        //    lstColumnsToExport.Items.Insert(index, data);
        //}
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
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return sPath + "\\" + sFileName;
        }

        private void Write_Blob_from_DB(bool IsScriptEnabled)
        {
            try
            {
                //if (sProjectIDBlob == "")
                //    return;
                DataTable dtBlob = new DataTable();
                byte[] byteBlob;
                string sPath = AppDomain.CurrentDomain.BaseDirectory, sFileName;

                string sBlobType = string.Empty;
                if (IsScriptEnabled)
                    sBlobType = "'CallScript','BOT','EAF'";                
                else
                    sBlobType = "'CallScript','EAF'";                

                //string sPath = AppDomain.CurrentDomain.BaseDirectory + "\\Campaign Manager", sFileName;
                string SQL = "SELECT * FROM PROJECT_FILES WHERE ProjectID IN('" + GV.sProjectID + "','ALL') AND FileType IN (" + sBlobType + ");";
                SqlConnection connection = new SqlConnection(GV.sMSSQL);
                SqlCommand command = new SqlCommand(SQL, connection);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                SqlDataReader sqldatar = command.ExecuteReader();
                dtBlob.Load(sqldatar);
                sqldatar.Close();
                connection.Close();

                GV.sCallScriptPath = string.Empty;
                //GV.sEAFLibararyPath = string.Empty;

                foreach (DataRow drBlob in dtBlob.Rows)
                {
                    byteBlob = (byte[])drBlob["Blob"];
                    sFileName = sPath + "\\" + drBlob["FileName"].ToString().Trim() + "." + drBlob["Extension"].ToString().Trim();

                    if (!Directory.Exists(sPath))
                        Directory.CreateDirectory(sPath);

                    if (drBlob["FileType"].ToString() == "CallScript")
                    {
                        File.WriteAllBytes(sFileName, byteBlob);
                        GV.sCallScriptPath = sFileName;
                    }
                    else if (drBlob["FileType"].ToString().ToUpper() == "BOT")
                    {
                        try
                        {
                            if (File.Exists(sFileName))
                            {
                                if (GM.FileInUse(sFileName))
                                {
                                    Process[] pName =
                                        Process.GetProcessesByName(Path.GetFileNameWithoutExtension(sFileName));
                                    if (pName.Length > 0)
                                    {
                                        pName[0].Kill(); // Kill if the bot is already running
                                        pName[0].WaitForExit();
                                    }
                                }
                                else
                                {
                                    Process[] pName1 =
                                        Process.GetProcessesByName(Path.GetFileNameWithoutExtension(sFileName));
                                    //Double check.. 
                                    if (pName1.Length == 0) //Still running ? do not rewrite the blob.
                                        File.WriteAllBytes(sFileName, byteBlob);
                                }
                            }
                            else
                                File.WriteAllBytes(sFileName, byteBlob);
                            GV.sBOTPath = sFileName;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else if (drBlob["FileType"].ToString().ToUpper() == "EAF")
                    {
                        EAF = byteBlob;
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBox.Show(ex.Message);
            }
        }

        private void Load_Tables()
        {

            GV.sAffFilePAth = WriteFile("en_US.aff", Properties.Resources.en_US);
            GV.sDicFilePath = WriteFile("en_US1.dic", Properties.Resources.en_US1);
            GV.sOSHandlerPath = WriteFile("Handle.exe", Properties.Resources.Handle);
            GV.sEmailCheckBinaryPath = WriteFile("Email_check.exe", Properties.Resources.Email_check);

            WriteFile("def.def", Properties.Resources.def);
            WriteFile("dic.dic", Properties.Resources.dic);
            WriteFile("syn.syn", Properties.Resources.syn);


            //if (JCS.OSVersionInfo.OSBits.ToString() == "Bit32")
            //{
            WriteFile("Perl522NH951.dll", Properties.Resources.Perl522NH951);
            WriteFile("Perl522RT951.dll", Properties.Resources.Perl522RT951);
            //}
            //else
            //{
            //WriteFile("Perl520NH940_64.dll", Properties.Resources.Perl520NH940);
            //WriteFile("Perl520RT940_64.dll", Properties.Resources.Perl520RT940);
            //}

            //GlobalVariables.sCallScriptPath = WriteDictioneryFile("CallScript.rtf", Properties.Resources.CallScript);
            GV.sSendKeyBinaryPath = WriteFile("SendKeys.exe", Properties.Resources.SendKeys);            


            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Text = "Loading Field Setttings"; });
            dtFieldMasterAllColumns = GV.MSSQL1.BAL_FetchTable("C_FIELD_MASTER", String.Format("PROJECT_ID = '{0}' ORDER BY SEQUENCE_NO", GV.sProjectID)); //All Fields in Master and Master Contacts
            dtFieldMasterAllColumns.TableName = "FieldMasterAllColumn";

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Value = 20; });
            
            dtFieldMaster_CriteriaColumns = dtFieldMasterAllColumns.Select("SHOW_ON_CRITERIA ='Y'", "TABLE_NAME ASC").CopyToDataTable();

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Value = 30; });
            
            dtFieldMaster_Active = dtFieldMasterAllColumns.Select("ACTIVE_COLUMN= 'Y' AND Visibility Like '%" + GV.sAccessTo + "%'", "SEQUENCE_NO ASC").CopyToDataTable();
            dtFieldMaster_Active.TableName = "FieldMaster";
            
            dtFieldMasterCompany = dtFieldMaster_Active.Select("TABLE_NAME = 'Master'", "SEQUENCE_NO ASC").CopyToDataTable(); //Master Table Fields
            dtFieldMasterCompany.TableName = "MasterFormat";
           
            dtFieldMasterContact = dtFieldMaster_Active.Select("TABLE_NAME = 'MasterContacts'", "SEQUENCE_NO ASC").CopyToDataTable(); //MasterContact Table Fields
            dtFieldMasterContact.TableName = "MasterContactFormat";


            DataRow[] drrChromeMenu = dtFieldMaster_Active.Select("LEN(ISNULL(CHROME_DISPLAYNAME,'')) > 0", "CHROME_DISPLAYORDER");
            sChromeColumnSettings = string.Empty;
            if (drrChromeMenu.Length > 0)
            {
                dtFieldMasterChromeCols = drrChromeMenu.CopyToDataTable();
                foreach (DataRow drChromeMenu in dtFieldMasterChromeCols.Rows)
                {
                    if (!sChromeColumnSettings.Contains("~" + drChromeMenu["CHROME_DISPLAYNAME"] + "~"))
                    {
                        if (sChromeColumnSettings.Length > 0)
                            sChromeColumnSettings += "|" + drChromeMenu["TABLE_NAME"] + "~" +
                                                     drChromeMenu["CHROME_DISPLAYNAME"] + "~" +
                                                     drChromeMenu["CHROME_CONTEXT"];
                        else
                            sChromeColumnSettings = drChromeMenu["TABLE_NAME"] + "~" +
                                                    drChromeMenu["CHROME_DISPLAYNAME"] + "~" +
                                                    drChromeMenu["CHROME_CONTEXT"];
                    }
                }
                sChromeColumnSettings = "Fields:{" + sChromeColumnSettings + "}";
            }

            dtUncertainFields = new DataTable();
            dtUncertainFields.Columns.Add("FieldName");
            dtUncertainFields.Columns.Add("FieldName_LinkColumn");
            dtUncertainFields.Columns.Add("PickList_Category");

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Text = "Loading Field Setttings.."; });
            DataRow[] drrTemp = dtFieldMasterContact.Select("UNCERTAIN_RAISABLE = 'Y'");
            foreach (DataRow drFieldRows in drrTemp)
            {
                DataRow drNewRow = dtUncertainFields.NewRow();
                drNewRow["FieldName"] = drFieldRows["FIELD_NAME_TABLE"].ToString();
                drNewRow["PickList_Category"] = drFieldRows["PICKLIST_CATEGORY"].ToString();
                dtUncertainFields.Rows.Add(drNewRow);
            }
            drrTemp = dtFieldMasterContact.Select("LEN(UNCERTAIN_LINKED_COLUMN) > 0");
            foreach (DataRow drFieldRows in drrTemp)
            {
                if (dtUncertainFields.Select("FieldName = '" + drFieldRows["UNCERTAIN_LINKED_COLUMN"] + "'").Length > 0)
                    dtUncertainFields.Select("FieldName = '" + drFieldRows["UNCERTAIN_LINKED_COLUMN"] + "'")[0]["FieldName_LinkColumn"] = drFieldRows["FIELD_NAME_TABLE"].ToString();
            }

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Value = 40; });

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Text = "Loading Validations"; });
            dtValidations = GV.MSSQL1.BAL_ExecuteQuery("SELECT * FROM " + GV.sProjectID + "_VALIDATIONS WHERE RESEARCH_TYPE='" + GV.sAccessTo + "'"); //Validation Table
            dtValidations.TableName = "Validations";

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Text = "Loading Validations.."; });
            if (dtValidations.Select("OPERATION_TYPE = 'PreUpdate'").Length > 0)
            {
                dtPreUpdate = dtValidations.Select("OPERATION_TYPE = 'PreUpdate'").CopyToDataTable();
                dtPreUpdate.TableName = "PreUpdate";
            }

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Value = 50; });

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Text = "Loading Selection Lists"; });
            dtPicklist = GV.MSSQL1.BAL_ExecuteQuery("SELECT * FROM c_picklists WHERE PicklistCategory NOT IN (SELECT DISTINCT PicklistCategory FROM " + GV.sProjectID + "_picklists) UNION SELECT * FROM " + GV.sProjectID + "_picklists ORDER BY PicklistCategory,PicklistValue;"); //Picklist Table                

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Text = "Loading Selection Lists.."; });
            dtEmailSuggestion = dtPicklist.Select("PicklistCategory = 'EmailSuggestion'", "Remarks ASC").CopyToDataTable();
            dtEmailSuggestion.TableName = "EmailSuggestion";

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Text = "Loading Selection Lists..."; });
            dtDialConfig = dtPicklist.Select("PicklistCategory like 'Dial_%'").CopyToDataTable();

            dtSpellIgnore = dtPicklist.Select("PicklistCategory = 'SpellCheckIgnore'").CopyToDataTable();

            dtbg_EmailValidation = dtPicklist.Select("PicklistCategory = 'EmailStatus'").CopyToDataTable();
                //GV.MYdfSQL.BAL_FetchTableMdySQL("c_picklists", "PicklistCategory = 'SpellCheckIgnore'");

            dtPicklist.TableName = "PickList";
            GV.PickList_LastUpdate = GM.GetDateTime();

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Value = 60; });

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Text = "Loading Disposal Lists"; });
            dtRecordStatus = GV.MSSQL1.BAL_ExecuteQuery("select Distinct Table_Name,Primary_Status,Secondary_Status,Operation_Type,Research_Type,sort from " + GV.sProjectID + "_recordstatus;"); //Contact Status Table
            dtRecordStatus.TableName = "RecordStatus";

            
            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Value = 70; });

            dtRecordStatusRevenue = GV.MSSQL1.BAL_ExecuteQuery("SELECT * FROM " + GV.sProjectID + "_RecordStatus"); //Contact Status Table
            dtRecordStatusRevenue.TableName = "RecordStatusRevenue";

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Text = "Loading Countries & Timezones"; });
            dtCountryInformation = GV.MSSQL1.BAL_ExecuteQuery("SELECT * FROM c_country");//Time Zone Datatable
            dtCountryInformation.TableName = "Country";

            RefreshBlockTable(true);

            if (GV.sUserType != "Agent")
            {
                dtQCPicklist = GV.MSSQL.BAL_ExecuteQuery("SELECT Field,Data FROM Timesheet..PickLists WHERE  ProjectType = 'C' AND Department = '" + GV.sAccessTo + "'");
                dtQCPicklist.TableName = "QCPickList";
            }

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Text = "Loading External Assembly"; });
            DataTable dtEAFBlob = GV.MSSQL.BAL_ExecuteQuery("select BLOB from project_files where filetype='EAF' and ProjectID='" + GV.sProjectID + "'");
            if (dtEAFBlob.Rows.Count > 0)
                EAF = (byte[])dtEAFBlob.Rows[0]["Blob"];

            dtScrapperSettings = GV.MSSQL1.BAL_ExecuteQuery("SELECT * FROM c_scrapper_settings where ProjectID = '" + GV.sProjectID + "' AND STATUS = 'ACTIVE';");
            
            Write_Blob_from_DB(dtScrapperSettings.Rows.Count > 0); //Initialize All Blobs

            GV.iAutoSave_Intervel = Convert.ToInt32(dtPicklist.Select("PicklistCategory = 'AUTOSAVE_INTERVEL'")[0]["PICKLISTVALUE"]);
            Chrome_Ext_Detection();

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Value = 80; });
        }

        void Chrome_Ext_Detection()
        {
            GV.sChromeExtensionID = dtPicklist.Select("PicklistCategory = 'CHROME_EXTID'")[0]["PICKLISTVALUE"].ToString();
            if (GV.sChromeExtensionID.Length > 0)
            {
                string sChromePath = string.Empty;
                if (JCS.OSVersionInfo.Name.ToLower() == "windows xp")                
                    sChromePath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), @"..\", @"Application Data\Google\Chrome\User Data\Default\Extensions\" + GV.sChromeExtensionID));                
                else
                    sChromePath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), @"..\", @"Google\Chrome\User Data\Default\Extensions\" + GV.sChromeExtensionID));

                if (Directory.Exists(sChromePath))
                    ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).btnChromeExt.Visible = false; });
                else
                    ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).btnChromeExt.Visible = true; });
            }
            else
                ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).btnChromeExt.Visible = false; });
        }


        public void RefreshBlockTable(bool RefreshbyDefault)
        {
            if(RefreshbyDefault)
            {
                dtBlock = GV.MSSQL1.BAL_ExecuteQuery("SELECT * FROM c_block WHERE BLOCK_EXPIRY > CAST(GETDATE() as date) AND PROJECT_ID IN ('ALL','" + GV.sProjectID + "') AND BLOCK_TO IN ('ALL','" + GV.sUserType + "');");
                GV.dBlockTableUpdateTime = Convert.ToDateTime(GV.MSSQL1.BAL_ExecuteQuery("SELECT TOP 1 UPDATED_DATE FROM  c_block ORDER BY UPDATED_DATE DESC;").Rows[0][0]);
                return;
            }

            DateTime? dLastUpdatedTime = GV.dBlockTableUpdateTime;            
            GV.dBlockTableUpdateTime = Convert.ToDateTime(GV.MSSQL1.BAL_ExecuteQuery("SELECT TOP 1 UPDATED_DATE FROM  c_block ORDER BY UPDATED_DATE DESC;").Rows[0][0]);
            if (GV.dBlockTableUpdateTime > dLastUpdatedTime)            
                dtBlock = GV.MSSQL1.BAL_ExecuteQuery("SELECT * FROM c_block WHERE BLOCK_EXPIRY > CAST(GETDATE() as date) AND PROJECT_ID IN ('ALL','" + GV.sProjectID + "') AND BLOCK_TO IN ('ALL','" + GV.sUserType + "');");
        }

        void Get_projectColumnsNSource()
        {
            if (GV.sUserType != "Agent")
            {
                ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Text = "Loading Columns to Export"; });
                dtProjectAllColumns =
                    GV.MSSQL1.BAL_ExecuteQuery(
                        "SELECT UPPER(TABLE_NAME)'TableName', COLUMN_NAME 'ColumnName',ORDINAL_POSITION 'Order' FROM information_schema.columns WHERE TABLE_CATALOG = db_name() AND TABLE_NAME IN ('" +
                        GV.sCompanyTable + "','" + GV.sContactTable +
                        "') AND Column_Name NOT IN ('MASTER_ID','CONTACT_ID_P','GROUP_ID') ORDER BY TABLE_NAME,ORDINAL_POSITION;");

                if (
                    GV.MSSQL1.BAL_ExecuteQuery("SELECT 'X' FROM " + GV.sProjectID +
                                                   "_Picklists where PicklistCategory = 'ExportTemplate'").Rows.Count >
                    0)
                    dtProjectExportColumns =
                        GV.MSSQL1.BAL_ExecuteQuery(
                            "SELECT PicklistCategory,PicklistField,PicklistValue,CAST(remarks AS INT)remarks FROM " +
                            GV.sProjectID +
                            "_picklists WHERE PicklistCategory='ExportTemplate' ORDER BY PicklistField, CAST(remarks AS INT)");
                else
                    dtProjectExportColumns =
                        GV.MSSQL1.BAL_ExecuteQuery(
                            "SELECT PicklistCategory,PicklistField,PicklistValue,CAST(remarks AS INT)remarks FROM c_picklists WHERE PicklistCategory='ExportTemplate' ORDER BY PicklistField, CAST(remarks AS INT)");

                ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Text = "Loading Export Sources"; });

                dtMoveToSource =
                    GV.MSSQL1.BAL_ExecuteQuery("SELECT DISTINCT MOVED_TO_SOURCE FROM " + GV.sCompanyTable +
                                                   " WHERE FLAG IN ('TR','WR');");
            }

            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Value = 90; });
        }

        private void Load_ProjectColumns_Export(bool LoadCompany, bool LoadContact)
        {
            if (GV.sUserType != "Agent")
            {

                //dtProjectAllColumns =
                //    GV.MsYSQL.BAL_ExecuteQueryMydSQL(
                //        "SELECT UPPER(TABLE_NAME)'TableName', COLUMN_NAME 'ColumnName',ORDINAL_POSITION 'Order' FROM information_schema.columns WHERE TABLE_SCHEMA='MVgC' AND TABLE_NAME IN ('" +
                //        GV.sCompanyTable + "','" + GV.sContactTable +
                //        "') AND Column_Name NOT IN ('MASTER_ID','CONTACT_ID_P','GROUP_ID') ORDER BY TABLE_NAME,ORDINAL_POSITION;");

                //if (GV.MsYSQL.BAL_ExecuteQueryMydSQL("SELECT 'X' FROM " + GV.sProjectID + "_Picklists where PicklistCategory = 'ExportTemplate'").Rows.Count > 0)
                //    dtProjectExportColumns = GV.MsYSQL.BAL_ExecuteQueryMydSQL("SELECT PicklistCategory,PicklistField,PicklistValue,CONVERT(remarks,SIGNED)remarks FROM " + GV.sProjectID + "_picklists WHERE PicklistCategory='ExportTemplate' ORDER BY PicklistField, CONVERT(remarks,SIGNED)");
                //else
                //    dtProjectExportColumns = GV.MYsSQL.BAL_ExecuteQueryMydSQL("SELECT PicklistCategory,PicklistField,PicklistValue,CONVERT(remarks,SIGNED)remarks FROM c_picklists WHERE PicklistCategory='ExportTemplate' ORDER BY PicklistField, CONVERT(remarks,SIGNED)");
                
                if (LoadCompany)
                {
                    lstAvaliableColumnsCompany.Items.Clear();
                    lstColumnsToExportCompany.Items.Clear();

                    lstAvaliableColumnsCompany.Columns.Clear();
                    lstColumnsToExportCompany.Columns.Clear();

                    //lstAvaliableColumnsCompany.View = View.Details;
                    lstAvaliableColumnsCompany.Columns.Add("Available Columns", lstColumnsToExportCompany.Width,HorizontalAlignment.Center);

                    //lstColumnsToExportCompany.View = View.Details;
                    lstColumnsToExportCompany.Columns.Add("Export Columns", lstColumnsToExportCompany.Width,HorizontalAlignment.Center);

                    DataRow[] drrProjectExportColumnsCompany =
                        dtProjectExportColumns.Select(
                            "PicklistCategory = 'ExportTemplate' AND PickListField='Company'", "Remarks");
                    DataRow[] drrProjectAllColumnsCompany =
                        dtProjectAllColumns.Select("TableName = '" + GV.sCompanyTable + "'");

                    foreach (DataRow drProjectExportColumnsCompany in drrProjectExportColumnsCompany)
                        lstColumnsToExportCompany.Items.Add(drProjectExportColumnsCompany["PickListValue"].ToString());

                    foreach (DataRow drProjectAllColumnsCompany in drrProjectAllColumnsCompany)
                    {
                        if (dtProjectExportColumns.Select("PickListValue = '" + drProjectAllColumnsCompany["ColumnName"] + "' AND PickListField = 'Company'").Length == 0)
                            lstAvaliableColumnsCompany.Items.Add(drProjectAllColumnsCompany["ColumnName"].ToString());
                    }
                }

                if (LoadContact)
                {
                    lstAvaliableColumnsContact.Items.Clear();
                    lstColumnsToExportContact.Items.Clear();

                    lstAvaliableColumnsContact.Columns.Clear();
                    lstColumnsToExportContact.Columns.Clear();

                    //lstAvaliableColumnsContact.View = View.Details;
                    lstAvaliableColumnsContact.Columns.Add("Available Columns", lstColumnsToExportCompany.Width,HorizontalAlignment.Center);

                    //lstColumnsToExportContact.View = View.Details;
                    lstColumnsToExportContact.Columns.Add("Export Columns", lstColumnsToExportCompany.Width,HorizontalAlignment.Center);

                    DataRow[] drrProjectExportColumnsContact =
                        dtProjectExportColumns.Select(
                            "PicklistCategory = 'ExportTemplate' AND PickListField='Contact'", "Remarks");
                    DataRow[] drrProjectAllColumnsContact =
                        dtProjectAllColumns.Select("TableName = '" + GV.sContactTable + "'");

                    foreach (DataRow drProjectExportColumnsContact in drrProjectExportColumnsContact)
                        lstColumnsToExportContact.Items.Add(drProjectExportColumnsContact["PickListValue"].ToString());

                    foreach (DataRow drProjectAllColumnsContact in drrProjectAllColumnsContact)
                    {
                        if (
                            dtProjectExportColumns.Select("PickListValue = '" +
                                                          drProjectAllColumnsContact["ColumnName"] +
                                                          "' AND PickListField = 'Contact'").Length == 0)
                            lstAvaliableColumnsContact.Items.Add(drProjectAllColumnsContact["ColumnName"].ToString());
                    }
                }
            }
        }

        private void btnExportColMovement_Click(object sender, EventArgs e)
        {
            ButtonX btn = sender as ButtonX;
            switch (btn.Name)
            {
                case "btnRightContact":
                    if (lstAvaliableColumnsContact.SelectedItems.Count > 0)
                    {
                        var itemsRightContact = lstAvaliableColumnsContact.SelectedItems;
                        foreach (ListViewItem item in itemsRightContact)
                        {
                            lstAvaliableColumnsContact.Items.Remove(item);
                            lstColumnsToExportContact.Items.Add(item);
                        }
                    }
                    break;
                case "btnRightAllContact":
                    var itemsRightAllContact = lstAvaliableColumnsContact.Items;
                    foreach (ListViewItem item in itemsRightAllContact)
                    {
                        lstAvaliableColumnsContact.Items.Remove(item);
                        lstColumnsToExportContact.Items.Add(item);
                    }
                    break;
                case "btnLeftContact":
                    if (lstColumnsToExportContact.SelectedItems.Count > 0)
                    {
                        var itemsbtnLeftContact = lstColumnsToExportContact.SelectedItems;
                        foreach (ListViewItem item in itemsbtnLeftContact)
                        {
                            lstColumnsToExportContact.Items.Remove(item);
                            lstAvaliableColumnsContact.Items.Add(item);
                        }
                    }
                    break;
                case "btnLeftAllContact":
                    var itemsLeftAllContact = lstColumnsToExportContact.Items;
                    foreach (ListViewItem item in itemsLeftAllContact)
                    {
                        lstColumnsToExportContact.Items.Remove(item);
                        lstAvaliableColumnsContact.Items.Add(item);
                    }
                    break;
                case "btnRightCompany":
                    if (lstAvaliableColumnsCompany.SelectedItems.Count > 0)
                    {
                        var itemsRightCompany = lstAvaliableColumnsCompany.SelectedItems;
                        foreach (ListViewItem item in itemsRightCompany)
                        {
                            lstAvaliableColumnsCompany.Items.Remove(item);
                            lstColumnsToExportCompany.Items.Add(item);
                        }
                    }
                    break;
                case "btnRightAllCompany":
                    var itemsRightAllCompany = lstAvaliableColumnsCompany.Items;
                    foreach (ListViewItem item in itemsRightAllCompany)
                    {
                        lstAvaliableColumnsCompany.Items.Remove(item);
                        lstColumnsToExportCompany.Items.Add(item);
                    }
                    break;
                case "btnLeftCompany":
                    if (lstColumnsToExportCompany.SelectedItems.Count > 0)
                    {
                        var itemsbtnLeftCompany = lstColumnsToExportCompany.SelectedItems;
                        foreach (ListViewItem item in itemsbtnLeftCompany)
                        {
                            lstColumnsToExportCompany.Items.Remove(item);
                            lstAvaliableColumnsCompany.Items.Add(item);
                        }
                    }
                    break;
                case "btnLeftAllCompany":
                    var itemsLeftAllCompany = lstColumnsToExportCompany.Items;
                    foreach (ListViewItem item in itemsLeftAllCompany)
                    {
                        lstColumnsToExportCompany.Items.Remove(item);
                        lstAvaliableColumnsCompany.Items.Add(item);
                    }
                    break;
            }
        }

        private void btnResetColumns_Click(object sender, EventArgs e)
        { 
            ButtonItem btn = sender as ButtonItem;
            if (btn.Name == "btnResetColumns")            
                Load_ProjectColumns_Export(true, true);            
            else if (btn.Name == "btnResetContactColumns")            
                Load_ProjectColumns_Export(false,true);            
            else if (btn.Name == "btnResetCompanyColumns")            
                Load_ProjectColumns_Export(true, false);            
        }

        

        //-----------------------------------------------------------------------------------------------------
        private void Count(DataTable dtCompanyCount, DataTable dtContactCount)
        {
            string sContactStatusToValidate = string.Empty;

            string sCompanyDateColumn = string.Empty;
            string sContactDateColumn = string.Empty;
            if (GV.sAccessTo == "TR")
            {
                sContactStatusToValidate = GV.sTRContactstatusTobeValidated;
                sCompanyDateColumn = "TR_DATECALLED";
                sContactDateColumn = "TR_UPDATED_DATE";
            }
            else if (GV.sAccessTo == "WR")
            {
                sContactStatusToValidate = GV.sWRContactstatusTobeValidated;
                sCompanyDateColumn = "WR_DATE_OF_PROCESS";
                sContactDateColumn = "WR_UPDATED_DATE";
            }

            DataRow[] drrTotalCompanyProcessedToday = null;
            DataRow[] drrTotalContactProcessedToday = null;
            drrTotalCompanyProcessedToday = dtCompanyCount.Select(String.Format(dtCompanyCount.Locale, sCompanyDateColumn + " >='{0:o}' AND " + sCompanyDateColumn + " < '{1:o}'", DateTime.Today, DateTime.Today.AddDays(1)));
            drrTotalContactProcessedToday = dtContactCount.Select(String.Format(dtContactCount.Locale, sContactDateColumn + " >='{0:o}' AND " + sContactDateColumn + " < '{1:o}'", DateTime.Today, DateTime.Today.AddDays(1)));

            //((frmMain)MdiParent).lblContactsComplets.Text = "Contacts Completes : "+iCount;

            //if (drrTotalCompanyProcessedToday.Length > 0)
            //{
            //    ((frmMain)MdiParent).lblTotalProcessed.Text = "Companies Processed : " + drrTotalCompanyProcessedToday.Length; //Total Processed Company
            //    ((frmMain)MdiParent).lblTotalComplets.Text = "Companies Completes : " + drrTotalCompanyProcessedToday.CopyToDataTable().Select(GlobalVariables.sAccessTo+"_PRIMARY_DISPOSAL = 'COMPLETE SURVEY'").Length;//Total Completed Company
            //    ((frmMain)MdiParent).lblPartialComplets.Text = "Partial Completes : " + drrTotalCompanyProcessedToday.CopyToDataTable().Select(GlobalVariables.sAccessTo + "_PRIMARY_DISPOSAL = 'PARTIAL COMPLETE'").Length;//Total Partial Company
            //    ((frmMain)MdiParent).labelItemCallBack.Text = "Call Back : " + drrTotalCompanyProcessedToday.CopyToDataTable().Select(GlobalVariables.sAccessTo + "_PRIMARY_DISPOSAL = 'CALL BACK'").Length;//Call Back Company
            //}
            //else
            //{
            //    ((frmMain)MdiParent).lblTotalProcessed.Text = "Companies Processed : 0";
            //    ((frmMain)MdiParent).lblTotalComplets.Text = "Companies Completes : 0";
            //    ((frmMain)MdiParent).lblPartialComplets.Text = "Partial Completes : 0";
            //    ((frmMain)MdiParent).labelItemCallBack.Text = "Call Back : 0";
            //}

            //if (drrTotalContactProcessedToday.Length > 0)
            //    ((frmMain)MdiParent).lblContactsComplets.Text = "Contacts Completes : " + drrTotalContactProcessedToday.CopyToDataTable().Select(GlobalVariables.sAccessTo + "_CONTACT_STATUS IN (" + sContactStatusToValidate + ")").Length;//Completed Contacts
            //else
            //    ((frmMain)MdiParent).lblContactsComplets.Text = "Contacts Completes : 0";
        }

        //-----------------------------------------------------------------------------------------------------
        public void Clear_ResultSet()
        {
            try
            {

                 //string sPrefix = "SELECT Company.*,Contact.* FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID";
                 dtMasterCompany.Rows.Clear();
                 dtMasterContact.Rows.Clear();
                dtQC.Rows.Clear();

                dgvCompanyList.DataSource = null;

               //  dtMasterCompany = GVd.MYdSQL.BAL_ExecuteQueryMySdQL(sPrefix + " where 1=0;");
                                  
               //  dtMasterContact = GV.MdYSQL.BAL_ExecuteQueryMySdQL("SELECT Contact.* FROM " + GV.sContactTable + " Contact WHERE 1=0");
                                  
               //  dtQC = GV.MdYSQL.BAL_ExecuteQueryMydSQL("SELECT QC.* FROM " + GV.sProjectID + "_QC QC WHERE 1=0");                                

                 
               ////dgvCompanyList.DataSource = dtMasterCompany;
               
               //foreach (DataColumn DC in dtMasterCompany.Columns)
               //{
               //    if(!dtSelectResult.Columns.Contains(DC.ColumnName))
               //         dtSelectResult.Columns.Add(DC.ColumnName);
               //}

               
               //foreach (DataColumn DC in dtMasterContact.Columns)
               //{
               //    if (!dtSelectResult.Columns.Contains(DC.ColumnName))
               //        dtSelectResult.Columns.Add(DC.ColumnName);
               //}
               

               
               //foreach (DataColumn DC in dtQC.Columns)
               //{
               //    if (!dtSelectResult.Columns.Contains(DC.ColumnName))
               //        dtSelectResult.Columns.Add(DC.ColumnName);
               //}
               
                

                ((frmMain)ParantForm).lblMessage.Visible = false;
                
                if(GV.sUserType == "Agent")
                    ((frmMain)ParantForm).btnSendBack.NotificationMarkText = ((frmMain)ParantForm).SendBackCount();                

                
                //ColorizeDGV();
                
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        public void RecordMovement()
        {
            if (dtMasterCompany != null && dtMasterCompany.Rows.Count > 0)
            {
                string sMasterIDs = string.Empty;
                string sMessageText = string.Empty;
                string sOppAcess = string.Empty;
                if (GV.sAccessTo == "WR")
                {
                    sOppAcess = "TR";
                    sMessageText = dtMasterCompany.Select("FLAG = 'WR' AND WR_DATE_OF_PROCESS IS NOT NULL").Length + " records to TR";
                }
                else
                {
                    sOppAcess = "WR";
                    sMessageText = dtMasterCompany.Select("FLAG = 'TR'").Length + " records to WR";
                }

                if (DialogResult.Yes == MessageBoxEx.Show("Are you sure to move " + sMessageText, "Campaign Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    foreach (DataRow dr in dtMasterCompany.Rows)
                    {
                        if ((GV.sAccessTo == "WR" && dr["FLAG"].ToString() == "WR" && dr["WR_DATE_OF_PROCESS"].ToString().Length > 0) || GV.sAccessTo == "TR" && dr["FLAG"].ToString() == "TR")// WHERE WR_DATE_OF_PROCESS IS NOT NULL or TR
                        {
                            dr["FLAG"] = sOppAcess;
                            dr["MOVED_TO_" + sOppAcess + "_DATE"] = GM.GetDateTime();
                            dr["MOVED_TO_" + sOppAcess + "_BY"] = GV.sEmployeeName;
                            if (sMasterIDs.Length > 0)
                                sMasterIDs += "," + dr["MASTER_ID"];
                            else
                                sMasterIDs = dr["MASTER_ID"].ToString();
                        }
                    }

                    //if ((sOppAcess == "WR" && GlobalVariables.sFreezeWRCompletedRecords == "Y") || (sOppAcess == "TR" && GlobalVariables.sFreezeTRCompletedRecords == "Y"))
                    //{
                    //    string sFreezedContactStatus = string.Empty;
                    //    foreach (string sFreeze in GlobalVariables.lstContactStatusToBeFreezed)
                    //    {
                    //        if(sFreeze != "WEBRESEARCHED" && sFreeze != "TELERESEARCHED")
                    //        {
                    //            if (sFreezedContactStatus.Length > 0)
                    //                sFreezedContactStatus += ",'" + sFreeze + "'";
                    //            else
                    //                sFreezedContactStatus = "'"+sFreeze+"'";
                    //        }
                    //    }

                    //    if (sMasterIDs.Length > 0 && sFreezedContactStatus.Length > 0)
                    //    {
                    //        string sQuery = string.Empty;
                    //        if(GlobalVariables.sAccessTo == "WR")
                    //            sQuery = "Update " + GlobalVariables.sContactTable + " SET TR_CONTACT_STATUS = 'WEBRESEARCHED' WHERE MASTER_ID IN(" + sMasterIDs + ") AND WR_CONTACT_STATUS IN(" + sFreezedContactStatus + ")";
                    //        else if (GlobalVariables.sAccessTo == "TR")
                    //            sQuery = "Update " + GlobalVariables.sContactTable + " SET WR_CONTACT_STATUS = 'TELERESEARCHED' WHERE MASTER_ID IN(" + sMasterIDs + ") AND TR_CONTACT_STATUS IN(" + sFreezedContactStatus + ")";

                    //        GV.MkYSQL.BAL_ExecuteNonReturnQueryMydSQL(sQuery);
                    //    }
                    //}
                    GV.MSSQL1.BAL_SaveToTable(dtMasterCompany.GetChanges(DataRowState.Modified), GV.sCompanyTable, "Update", true);
                    MessageBoxEx.Show("Records Moved", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ((frmMain)MdiParent).btnClearFilter.RaiseClick();
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------
        

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
        //public void OpenContactUpdate(string sID, bool IsNewCompany, bool IsOpenbyID)
        //{
        //    try
        //    {
        //        if (GV.sAccessTo == "TR" || GV.sAccessTo == "WR")
        //        {
        //            bool IsFormOpen = false;
        //            foreach (Form f in Application.OpenForms)
        //            {
        //                if (f.Name == "FrmContactsUpdate")
        //                {
        //                    IsFormOpen = true;
        //                    f.Focus();
        //                    if (DialogResult.Yes == MessageBoxEx.Show("Changes made in this screen will be lost." + Environment.NewLine + "Do you want to continue ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
        //                    {
        //                        f.Close();
        //                        if (f.IsDisposed)
        //                            IsFormOpen = false;
        //                        else
        //                            return;
        //                    }
        //                    else
        //                        return;//
        //                    break;
        //                }
        //            }

        //            if (GV.sUserType == "Agent")
        //            {
        //                DataTable dtRecordCheck = new DataTable();
        //                if (IsOpenbyID)
        //                {
        //                    dtRecordCheck = GV.MsaYSQL.BAL_FetchTableMydSQL(GV.sCompanyTable, GV.sAccessTo + "_AGENTNAME = 'Current_" + GV.sEmployeeName + "'");
        //                    if (dtRecordCheck.Rows.Count > 0)
        //                    {
        //                        if (dtRecordCheck.Rows[0]["Master_ID"].ToString() == sID && IsFormOpen == false)
        //                        {                                    
        //                            FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(null, this.MdiParent, "ListOpen", IsNewCompany);
        //                            objfrmContactsUpdate.Show();
        //                            return;                                    
        //                        }
        //                        else
        //                        {
        //                            MessageBoxEx.Show("Some contact(s) not saved properly. Opening them now.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //                            if (IsFormOpen == false)
        //                            {
        //                                FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(null, this.MdiParent, "ListOpen", IsNewCompany);
        //                                objfrmContactsUpdate.Show();
        //                                return;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        dtRecordCheck = GV.MYdSQL.BAL_FetchTableMydSQL(GV.sCompanyTable, "MASTER_ID = " + sID + " AND FLAG = '" + GV.sAccessTo + "'");
        //                        if (dtRecordCheck.Rows.Count > 0 && dtRecordCheck.Rows[0][GV.sAccessTo + "_AGENTNAME"].ToString().StartsWith("Current") && dtRecordCheck.Rows[0][GV.sAccessTo + "_AGENTNAME"].ToString().Replace("Current_", "").ToUpper() != GV.sEmployeeName.ToUpper())
        //                        {
        //                            MessageBoxEx.Show("This record is already in use by <font size = '10' color='OrangeRed'><b>" + GM.ProperCase_ProjectSpecific(dtRecordCheck.Rows[0][GV.sAccessTo + "_AGENTNAME"].ToString().Replace("Current_", string.Empty)) + "</b></font>", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //                            return;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    dtRecordCheck = GV.MYdSQL.BAL_FetchTableMydSQL(GV.sCompanyTable, GV.sAccessTo + "_AGENTNAME = 'Current_" + GV.sEmployeeName + "'");
        //                    if (dtRecordCheck.Rows.Count > 0)
        //                    {
        //                        MessageBoxEx.Show("Some contact(s) not saved properly. Opening them now.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //                        if (IsFormOpen == false)
        //                        {
        //                            FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(null, this.MdiParent, "ListOpen", IsNewCompany);                                    
        //                            objfrmContactsUpdate.Show();
        //                            return;//Show only records in stack(Current_AgentName)
        //                        }
        //                    }
        //                    else
        //                        dtRecordCheck = GV.MYdSQL.BAL_FetchTableMydSQL(GV.sCompanyTable, "MASTER_ID = " + sID + " AND " + GV.sAccessTo + "_AGENTNAME = '" + GV.sEmployeeName + "'");
        //                }

        //                if (dtRecordCheck.Rows.Count > 0)
        //                {
        //                    if (IsFormOpen == false && GV.sCompanyTable.Length > 0 && GV.sContactTable.Length > 0)
        //                    {
        //                        GV.MYdSQL.BAL_ExecuteQueryMydSQL("UPDATE " + GV.sCompanyTable + " SET " + GV.sAccessTo + "_AGENTNAME = 'Current_" + GV.sEmployeeName + "' WHERE GROUP_ID = " + dtRecordCheck.Rows[0]["GROUP_ID"] + ";");
        //                        FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(null, this.MdiParent, "ListOpen", IsNewCompany);
        //                        objfrmContactsUpdate.Show();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                DataTable dtAdminCheck = GV.MdYSQL.BAL_FetchTableMySdQL(GV.sCompanyTable, "MASTER_ID = " + sID + ";");
        //                if (dtAdminCheck.Rows.Count > 0)
        //                {
        //                    if (dtAdminCheck.Rows[0][GV.sAccessTo + "_AGENTNAME"].ToString().StartsWith("Current"))
        //                    {
        //                        MessageBoxEx.Show("This record is already in use by <font size = '10' color='OrangeRed'><b>" + GM.ProperCase_ProjectSpecific(dtAdminCheck.Rows[0][GV.sAccessTo + "_AGENTNAME"].ToString().Replace("Current_", string.Empty)) + "</b></font>", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //                        return;
        //                    }

        //                    FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(sID, this.MdiParent, "ListOpen", false);                            
        //                    objfrmContactsUpdate.Show();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
        //        //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    }
        //}

        //-----------------------------------------------------------------------------------------------------

        public int Clear()
        {
            foreach (DataRow dr in dtSearch.Rows)
            {
                if (dr["Primary"].ToString() == "False")
                {
                    dr["SearchFrom"] = string.Empty;
                    dr["SearchTo"] = string.Empty;
                }
            }
            dtSelectResult = dtMasterCompany.Copy();
            dgvCompanyList.DataSource = dtSelectResult;
            splitCompanyList.SplitterDistance = 202;
            return dtSelectResult.Rows.Count;
        }

        //-----------------------------------------------------------------------------------------------------
        public void RebuildSearchGrid()
        {
            try
            {
                dtSearch = new DataTable();
                dtSearch.Columns.Add("Primary", typeof(bool));
                dtSearch.Columns.Add("Table", typeof(string));
                dtSearch.Columns.Add("Search On", typeof(string));
                dtSearch.Columns.Add("Criteria", typeof(string));
                dtSearch.Columns.Add("SearchFrom", typeof(string));
                dtSearch.Columns.Add("SearchTo", typeof(string));
                dtSearch.Columns.Add("Datatype", typeof(string));
                dtSearch.Columns["Primary"].DefaultValue = false;
                dtSearch.Columns["Criteria"].DefaultValue = string.Empty;
                dtSearch.Columns["SearchFrom"].DefaultValue = string.Empty;
                dtSearch.Columns["SearchTo"].DefaultValue = string.Empty;

                //foreach (DataColumn dc in dtMasterCompany.Columns)
                //{
                //    if (GlobalVariables.lstShowOnCriteriaMasterCompanies.Contains(dc.ColumnName.ToUpper()))
                //    {
                //        DataRow drSearch = dtSearch.NewRow();
                //        drSearch["Search On"] = dc.ColumnName;
                //        drSearch["Datatype"] = dc.DataType.Name;
                //        dtSearch.Rows.Add(drSearch);
                //    }
                //}

                foreach (DataRow drField in dtFieldMaster_CriteriaColumns.Rows)
                {
                    string sFieldName = drField["FIELD_NAME_TABLE"].ToString().ToUpper();
                    if (drField["TABLE_NAME"].ToString() == "Master")// && dtMasterCompany.Columns.Contains(sFieldName))
                    {                        
                        DataRow drSearch = dtSearch.NewRow();
                        drSearch["Table"] = "Company";
                        drSearch["Search On"] = sFieldName;
                        drSearch["Datatype"] = drField["Datatype"].ToString();
                      
                        dtSearch.Rows.Add(drSearch);
                    }
                    else if (drField["TABLE_NAME"].ToString() == "MasterContacts")// && dtMasterContact.Columns.Contains(sFieldName))
                    {
                        DataRow drSearch = dtSearch.NewRow();
                        drSearch["Table"] = "Contact";
                        drSearch["Search On"] = sFieldName;
                        drSearch["Datatype"] = drField["Datatype"].ToString();                                                
                        dtSearch.Rows.Add(drSearch);
                    }
                    else if (drField["TABLE_NAME"].ToString() == "QC")// && dtQC.Columns.Contains(sFieldName))
                    {
                        DataRow drSearch = dtSearch.NewRow();
                        drSearch["Primary"] = true;
                        drSearch["Table"] = "QC";
                        drSearch["Search On"] = sFieldName;
                        drSearch["Datatype"] = drField["Datatype"].ToString();                        
                        dtSearch.Rows.Add(drSearch);
                    }
                }
                
                DataView dvSearch = dtSearch.DefaultView;
                dvSearch.Sort = "Table,Datatype ASC";
                dtSearch = dvSearch.ToTable();

                DataRow drCountRow = dtSearch.NewRow();
                drCountRow["Primary"] = true;
                drCountRow["Table"] = "Contact Count";                
                drCountRow["Search On"] = string.Empty;
                drCountRow["Datatype"] = "Number";
                dtSearch.Rows.Add(drCountRow);

                //dgvCompanySearch.Invoke((MethodInvoker)delegate { dgvCompanySearch.DataSource = dtSearch; });

                dgvCompanySearch.DataSource = dtSearch;
                
                dgvCompanySearch.Columns["Table"].FillWeight = 20;
                dgvCompanySearch.Columns["Table"].SortMode = DataGridViewColumnSortMode.NotSortable;

                dgvCompanySearch.Columns["Primary"].FillWeight = 20;                
                dgvCompanySearch.Columns["Primary"].HeaderText = "Main Filter";
                dgvCompanySearch.Columns["Primary"].SortMode = DataGridViewColumnSortMode.NotSortable;


                //dgvCompanySearch.Columns["Criteria"].Width = 70;
                dgvCompanySearch.Columns["Criteria"].FillWeight = 26;
                dgvCompanySearch.Columns["Criteria"].ReadOnly = true;
                dgvCompanySearch.Columns["Criteria"].SortMode = DataGridViewColumnSortMode.NotSortable;

                dgvCompanySearch.Columns["Datatype"].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvCompanySearch.Columns["Datatype"].ReadOnly = true;
               // dgvCompanySearch.Columns["Datatype"].Width = 50;

                dgvCompanySearch.Columns["Search On"].FillWeight = 35;
                dgvCompanySearch.Columns["Search On"].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvCompanySearch.Columns["Search On"].ReadOnly = true;
                dgvCompanySearch.Columns["SearchFrom"].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvCompanySearch.Columns["SearchTo"].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvCompanySearch.Columns["SearchFrom"].HeaderText = "Value / SearchFrom";



                                              

                //dgvCompanyList.DataSource = dtFilterView;
                //dgvCompanyList.BackgroundColor = Color.FromArgb(207, 221, 238);
                //dgvCompanySearch.BackgroundColor = Color.FromArgb(207, 221, 238);
                //ColorizeDGV();
                foreach (DataGridViewRow dgvr in dgvCompanySearch.Rows)
                {
                    if (dgvr.Cells["Datatype"].Value.ToString() == "Text" || dgvr.Cells["Datatype"].Value.ToString() == "Number")
                        dgvr.Cells["Criteria"].Value = "Equals";
                    else if (dgvr.Cells["Datatype"].Value.ToString() == "Date")
                    {
                        dgvr.Cells["Criteria"].Value = "Between";
                        if ((GV.sAccessTo == "TR" && dgvr.Cells["Search On"].Value.ToString().ToUpper() == "TR_DATECALLED") || (GV.sAccessTo == "WR" && dgvr.Cells["Search On"].Value.ToString().ToUpper() == "WR_DATE_OF_PROCESS"))
                        {
                            dgvr.Cells["Primary"].Value = true;
                            dgvr.Cells["SearchFrom"].Value = DateTime.Today.ToString("yyyy-MM-dd") + " 00:00:00";
                            dgvr.Cells["SearchTo"].Value = DateTime.Today.ToString("yyyy-MM-dd") + " 23:59:59";
                        }
                    }

                    if (GV.lstShowOnCriteriaMasterContacts.Contains(dgvr.Cells["Search On"].Value.ToString(), StringComparer.OrdinalIgnoreCase))
                        dgvr.Cells["Primary"].Value = true;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void ColorizeDGV()
        {
            if (dgvCompanyList != null && dgvCompanyList.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in dgvCompanyList.Rows)
                {
                    if (dgvr.Cells[GV.sAccessTo + "_PRIMARY_DISPOSAL"].Value.ToString().ToUpper() == "COMPLETE SURVEY")
                        dgvr.DefaultCellStyle.BackColor = System.Drawing.Color.PowderBlue;
                }
            }
            //dgvCompanyList.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
            //dgvCompanySearch.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
        }

        DataTable ExecuteSuggestionSQL()
        {
            try
            {
                string sSQLText = BuildQuery(FormatDatatableToBuildQuery(false));
                string sPrefix = string.Empty;
                if (sSQLText.ToUpper().Contains("ISNULL(CONTACT.QC_STATUS"))
                    //sPrefix = "SELECT * FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID INNER JOIN "+GV.sProjectID+"_QC QC ON Contact.CONTACT_ID_P = QC.RECORDID";
                    sPrefix = "SELECT * FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN  (SELECT * FROM " + GV.sContactTable + " C INNER JOIN " + GV.sProjectID + "_QC Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
                else
                    sPrefix = "SELECT * FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID ";

                DataTable dtCompanyContactData;
                if (sSQLText.Length > 0)
                {
                    if (GV.sUserType == "Agent")
                        dtCompanyContactData = GV.MSSQL1.BAL_ExecuteQuery(sPrefix + " where " + sSQLText + " AND " + GV.sAccessTo + "_AGENTNAME = '" + GV.sEmployeeName + "' AND FLAG = '" + GV.sAccessTo + "';");
                    else if (GV.sUserType == "QC" || GV.sUserType == "Admin")
                        dtCompanyContactData = GV.MSSQL1.BAL_ExecuteQuery(sPrefix + " where " + sSQLText + ";");
                    else
                        dtCompanyContactData = GV.MSSQL1.BAL_ExecuteQuery(sPrefix + " where " + sSQLText + " AND FLAG = '" + GV.sAccessTo + "';");

                    DataTable dtSecondarySearch = dtSearch.Clone();
                    DataRow[] drrCondition = dtSearch.Select("Primary = 'False' AND LEN(Criteria)> 0");
                    foreach (DataRow dr in drrCondition)
                    {
                        if (dr["Datatype"].ToString() == "Text" && dr["SearchFrom"].ToString().Length > 0)
                            dtSecondarySearch.ImportRow(dr);
                        else if (dr["Datatype"].ToString() == "Date" && dr["SearchFrom"].ToString().Length > 0 && dr["SearchTo"].ToString().Length > 0)
                            dtSecondarySearch.ImportRow(dr);
                        else if (dr["Datatype"].ToString() == "Number" && dr["Criteria"].ToString() == "Between" && dr["SearchFrom"].ToString().Length > 0 && dr["SearchTo"].ToString().Length > 0)
                            dtSecondarySearch.ImportRow(dr);
                        else if (dr["Datatype"].ToString() == "Number" && dr["Criteria"].ToString() == "Equals" && dr["SearchFrom"].ToString().Length > 0)
                            dtSecondarySearch.ImportRow(dr);
                        else if (dr["Datatype"].ToString() == "Number" && dr["Criteria"].ToString() == "Not Equals" && dr["SearchFrom"].ToString().Length > 0)
                            dtSecondarySearch.ImportRow(dr);
                    }

                    dtSecondarySearch.Columns.Remove("Primary");

                    if (dtCompanyContactData.Rows.Count > 0)
                    {
                        DataRow[] drrSelectResult = null;
                        if (dtSecondarySearch.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtSecondarySearch.Rows)
                            {
                                if (dr["Datatype"].ToString() == "Text")
                                {
                                    if (dr["Criteria"].ToString() == "Equals")
                                        drrSelectResult = dtCompanyContactData.Select(dr["Search On"] + " IN (" + GM.ListToQueryString(dr["SearchFrom"].ToString().Split(',').ToList(), "String") + ")");
                                    else if (dr["Criteria"].ToString() == "Not Equals")
                                        drrSelectResult = dtCompanyContactData.Select(dr["Search On"] + " NOT IN (" + GM.ListToQueryString(dr["SearchFrom"].ToString().Split(',').ToList(), "String") + ")");
                                    else if (dr["Criteria"].ToString() == "Contains")
                                        drrSelectResult = dtCompanyContactData.Select(dr["Search On"] + " LIKE '%" + dr["SearchFrom"] + "%'");
                                }
                                else if (dr["Datatype"].ToString() == "Date")
                                    drrSelectResult = dtCompanyContactData.Select(String.Format(dtCompanyContactData.Locale, dr["Search On"] + " >='{0:o}' AND " + dr["Search On"] + " < '{1:o}'", Convert.ToDateTime(dr["SearchFrom"]), Convert.ToDateTime(dr["SearchTo"])));
                                else if (dr["Datatype"].ToString() == "Number")
                                {
                                    if (dr["Criteria"].ToString() == "Equals")
                                        drrSelectResult = dtCompanyContactData.Select(dr["Search On"] + " IN (" + GM.ListToQueryString(dr["SearchFrom"].ToString().Split(',').ToList(), "String") + ")");
                                    else if (dr["Criteria"].ToString() == "Not Equals")
                                        drrSelectResult = dtCompanyContactData.Select(dr["Search On"] + " NOT IN (" + GM.ListToQueryString(dr["SearchFrom"].ToString().Split(',').ToList(), "String") + ")");
                                    else if (dr["Criteria"].ToString() == "Between")
                                        drrSelectResult = dtCompanyContactData.Select(dr["Search On"] + " >= " + dr["SearchFrom"] + " AND " + dr["Search On"] + " <= " + dr["SearchTo"]);
                                }

                                if (drrSelectResult.Length > 0)
                                    dtCompanyContactData = drrSelectResult.CopyToDataTable();
                                else
                                    return dtCompanyContactData;
                            }
                        }
                        if (drrSelectResult != null && drrSelectResult.Length > 0)
                            dtCompanyContactData = drrSelectResult.CopyToDataTable();
                        else
                            return dtCompanyContactData;
                    }
                    return dtCompanyContactData;
                }
                return null;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        public int ExecuteSQLText(out string sQuery)
        {
            sQuery = string.Empty;
            try
            {
                string sSQLText = BuildQuery(FormatDatatableToBuildQuery(false));                
                string sPrefix = string.Empty;                
                    
                if (sSQLText.Length > 0)
                {
                    if (sSQLText.ToUpper().Contains("ISNULL(CONTACT.QC_STATUS"))
                        //sPrefix = "SELECT * FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID INNER JOIN "+GV.sProjectID+"_QC QC ON Contact.CONTACT_ID_P = QC.RECORDID";
                        sPrefix = "SELECT DISTINCT Company.* FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN  (SELECT * FROM " + GV.sContactTable + " C INNER JOIN " + GV.sProjectID + "_QC Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
                    else
                        sPrefix = "SELECT DISTINCT Company.* FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID ";

                    string sExecute = string.Empty;
                    if (GV.sUserType == "Agent")
                        sExecute = sPrefix + " where " + sSQLText + " AND " + GV.sAccessTo + "_AGENTNAME = '" + GV.sEmployeeName + "' AND FLAG = '" + GV.sAccessTo + "';";
                    else if (GV.sUserType == "QC")
                        sExecute = sPrefix + " where " + sSQLText;
                    else
                        sExecute = sPrefix + " where " + sSQLText + " AND FLAG = '" + GV.sAccessTo + "';";
                    sQuery = sExecute;
                    dtMasterCompany = GV.MSSQL1.BAL_ExecuteQuery(sExecute);

                    DataTable dtSecondarySearch = dtSearch.Clone();
                    DataRow[] drrCondition = dtSearch.Select("Primary = 'False' AND LEN(Criteria)> 0");
                    foreach (DataRow dr in drrCondition)
                    {
                        if (dr["Datatype"].ToString() == "Text" && dr["SearchFrom"].ToString().Length > 0)
                            dtSecondarySearch.ImportRow(dr);
                        else if (dr["Datatype"].ToString() == "Date" && dr["SearchFrom"].ToString().Length > 0 && dr["SearchTo"].ToString().Length > 0)
                            dtSecondarySearch.ImportRow(dr);
                        else if (dr["Datatype"].ToString() == "Number" && dr["Criteria"].ToString() == "Between" && dr["SearchFrom"].ToString().Length > 0 && dr["SearchTo"].ToString().Length > 0)
                            dtSecondarySearch.ImportRow(dr);
                        else if (dr["Datatype"].ToString() == "Number" && dr["Criteria"].ToString() == "Equals" && dr["SearchFrom"].ToString().Length > 0)
                            dtSecondarySearch.ImportRow(dr);
                        else if (dr["Datatype"].ToString() == "Number" && dr["Criteria"].ToString() == "Not Equals" && dr["SearchFrom"].ToString().Length > 0)
                            dtSecondarySearch.ImportRow(dr);
                    }
                    dtSecondarySearch.Columns.Remove("Primary");

                    if (dtMasterCompany.Rows.Count > 0)
                    {
                        dtSelectResult = dtMasterCompany.Copy();
                        DataRow[] drrSelectResult = null;
                        if (dtSecondarySearch.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtSecondarySearch.Rows)
                            {
                                if (dr["Datatype"].ToString() == "Text")
                                {
                                    if (dr["Criteria"].ToString() == "Equals")
                                        drrSelectResult = dtSelectResult.Select(dr["Search On"] + " IN (" + GM.ListToQueryString(dr["SearchFrom"].ToString().Split(',').ToList(), "String") + ")");
                                    else if (dr["Criteria"].ToString() == "Not Equals")
                                        drrSelectResult = dtSelectResult.Select(dr["Search On"] + " NOT IN (" + GM.ListToQueryString(dr["SearchFrom"].ToString().Split(',').ToList(), "String") + ")");
                                    else if (dr["Criteria"].ToString() == "Contains")
                                        drrSelectResult = dtSelectResult.Select(dr["Search On"] + " LIKE '%" + dr["SearchFrom"] + "%'");
                                }
                                else if (dr["Datatype"].ToString() == "Date")
                                    drrSelectResult = dtSelectResult.Select(String.Format(dtSelectResult.Locale, dr["Search On"] + " >='{0:o}' AND " + dr["Search On"] + " < '{1:o}'", Convert.ToDateTime(dr["SearchFrom"]), Convert.ToDateTime(dr["SearchTo"])));
                                else if (dr["Datatype"].ToString() == "Number")
                                {
                                    if (dr["Criteria"].ToString() == "Equals")
                                        drrSelectResult = dtSelectResult.Select(dr["Search On"] + " IN (" + GM.ListToQueryString(dr["SearchFrom"].ToString().Split(',').ToList(), "String") + ")");
                                    else if (dr["Criteria"].ToString() == "Not Equals")
                                        drrSelectResult = dtSelectResult.Select(dr["Search On"] + " NOT IN (" + GM.ListToQueryString(dr["SearchFrom"].ToString().Split(',').ToList(), "String") + ")");
                                    else if (dr["Criteria"].ToString() == "Between")
                                        drrSelectResult = dtSelectResult.Select(dr["Search On"] + " >= " + dr["SearchFrom"] + " AND " + dr["Search On"] + " <= " + dr["SearchTo"]);
                                }

                                if (drrSelectResult.Length > 0)
                                    dtSelectResult = drrSelectResult.CopyToDataTable();
                                else
                                {
                                    dgvCompanyList.DataSource = null;
                                    return 0;
                                }
                            }
                        }
                        else
                        {
                            dgvCompanyList.DataSource = dtSelectResult;
                            splitCompanyList.SplitterDistance = 50;
                            return dtSelectResult.Rows.Count;
                        }

                        if (drrSelectResult.Length > 0)
                        {
                            dtSelectResult = drrSelectResult.CopyToDataTable();
                            dgvCompanyList.DataSource = dtSelectResult;
                            splitCompanyList.SplitterDistance = 50;
                        }
                        else
                        {
                            dgvCompanySearch.DataSource = null;
                            ToastNotification.Show(this, "Condition doesn't return data");
                        }

                        return drrSelectResult.Length;
                    }
                    else
                    {
                        dgvCompanyList.DataSource = null;
                        return 0;
                    }
                }
                else
                    ToastNotification.Show(this, "Filter doesn't return any row(s) or invalid filter conditions.", eToastPosition.TopRight);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return 0;
        }

        //-----------------------------------------------------------------------------------------------------
        private void dgvCompanySearch_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox)
            {
                TextBox tb = e.Control as TextBox;
                tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                int i = dgvCompanySearch.CurrentCell.RowIndex;
                if ((dgvCompanySearch.CurrentCell.OwningColumn.Name == "SearchFrom" || dgvCompanySearch.CurrentCell.OwningColumn.Name == "SearchTo") && (dgvCompanySearch.Rows[i].Cells["Datatype"].Value.ToString() == "Number"))
                {
                    if ((dgvCompanySearch.Rows[i].Cells["Criteria"].Value.ToString() == "Equals" || dgvCompanySearch.Rows[i].Cells["Criteria"].Value.ToString() == "Not Equals") && dgvCompanySearch.Rows[i].Cells["Table"].Value.ToString() != "Contact Count")
                    {
                        if (!(char.IsDigit(e.KeyChar)))
                        {
                            if (e.KeyChar == '\b' || e.KeyChar == ',') //allow the backspace and ','
                            { }
                            else
                                e.Handled = true;
                        }
                    }
                    else
                    {
                        if (!(char.IsDigit(e.KeyChar)))
                        {
                            if (e.KeyChar == '\b') //allow the backspace and ','
                            { }
                            else
                                e.Handled = true;
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

        //-----------------------------------------------------------------------------------------------------
        private void RuntimeEvents()
        {
            cmbCriteria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCriteria.DropDownClosed += new EventHandler(cmbCriteria_Leave);
            dtp.CloseUp += new EventHandler(dtp_CloseUp);
            dgvCompanySearch.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgvCompanySearch_EditingControlShowing);
        }

        //-----------------------------------------------------------------------------------------------------
        private void dtp_CloseUp(object sender, EventArgs e)
        {
            try
            {
                if (iRowIndex != -1)
                {
                    if (dgvCompanySearch.Rows[iRowIndex].Cells[dgvCompanySearch.CurrentCell.ColumnIndex].OwningColumn.Name == "SearchFrom")
                    {
                        dgvCompanySearch.Rows[iRowIndex].Cells[dgvCompanySearch.CurrentCell.ColumnIndex].Value = dtp.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                        if (dgvCompanySearch.Rows[iRowIndex].Cells["SearchTo"].Value.ToString().Length == 0)
                            dgvCompanySearch.Rows[iRowIndex].Cells["SearchTo"].Value = dtp.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                    }
                    else if (dgvCompanySearch.Rows[iRowIndex].Cells[dgvCompanySearch.CurrentCell.ColumnIndex].OwningColumn.Name == "SearchTo")
                        dgvCompanySearch.Rows[iRowIndex].Cells[dgvCompanySearch.CurrentCell.ColumnIndex].Value = dtp.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                    iRowIndex = -1;
                    dtp.Visible = false;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void cmbCriteria_Leave(object sender, EventArgs e)
        {
            try
            {
                if (iRowIndex != -1)
                {
                    if(cmbCriteria.Text.Length > 0)
                        dgvCompanySearch.Rows[iRowIndex].Cells["Criteria"].Value = cmbCriteria.Text;

                    iRowIndex = -1;
                    cmbCriteria.Visible = false;
                    dgvCompanySearch.Focus();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void dgvCompanySearch_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvCompanySearch.DataSource != null && dgvCompanySearch.Rows.Count > 0)
                {
                    
                    //foreach (DataGridViewColumn dgvc in dgvCompanySearch.Columns) //this part could be rewritten in better way(May be not in datasource change Event)
                    //{
                    //    if (dgvc.Name == "Datatype")
                    //        dgvc.Width = 50;
                    //    if (dgvc.Name == "SearchFrom")
                    //        dgvc.HeaderText = "Value / SearchFrom";
                    //    else if (dgvc.Name == "Criteria")
                    //    {
                    //        dgvc.Width = 70;
                    //        dgvc.ReadOnly = true;
                    //    }
                    //    else if (dgvc.Name == "Search On")
                    //        dgvc.ReadOnly = true;
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
        private void dgvCompanySearch_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {

                if (
                        GV.sUserType == "Agent"
                        && 
                        (
                            (GV.sAccessTo == "TR" && dgvCompanySearch.Rows[e.RowIndex].Cells["Search On"].Value.ToString().ToUpper() == "TR_DATECALLED") 
                            || 
                            (GV.sAccessTo == "WR" && dgvCompanySearch.Rows[e.RowIndex].Cells["Search On"].Value.ToString().ToUpper() == "WR_DATE_OF_PROCESS")
                        )
                   )
                    e.Cancel = true;//Agents cannot uncheck mainfilter from their Date columns

                if (e.ColumnIndex == 0 && (GV.lstShowOnCriteriaMasterContacts.Contains(dgvCompanySearch.Rows[e.RowIndex].Cells["Search On"].Value.ToString(), StringComparer.OrdinalIgnoreCase) || dgvCompanySearch.Rows[e.RowIndex].Cells["Table"].Value.ToString() == "Contact Count" || dgvCompanySearch.Rows[e.RowIndex].Cells["Table"].Value.ToString() == "QC"))
                    e.Cancel = true;

                //iRowIndex = e.RowIndex;
                if (dgvCompanySearch.Columns[e.ColumnIndex].Name == "SearchTo")
                {
                    if ((dgvCompanySearch.Rows[e.RowIndex].Cells["Datatype"].Value.ToString() == "Number" || dgvCompanySearch.Rows[e.RowIndex].Cells["Datatype"].Value.ToString() == "Date") && (dgvCompanySearch.Rows[e.RowIndex].Cells["Criteria"].Value.ToString() == "Between"))
                    { /*Do nothing*/}
                    else
                        e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void dgvCompanySearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F2)
                {
                    Simulate_Cell_Impact();
                }
                if ((e.KeyCode == Keys.Right && e.Modifiers == Keys.Control) || (e.KeyCode == Keys.Left && e.Modifiers == Keys.Control))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void Simulate_Cell_Impact()
        {
            try
            {
                if (dgvCompanySearch.CurrentCell != null)
                {
                    //if (cmbCriteria.Items.Contains("Between"))
                    //    cmbCriteria.Items.Remove("Between");

                    //if (!cmbCriteria.Items.Contains("Equals"))
                    //    cmbCriteria.Items.Add("Equals");

                    //if (!cmbCriteria.Items.Contains("Contains"))
                    //    cmbCriteria.Items.Add("Contains");

                    //if (!cmbCriteria.Items.Contains("Not Equals"))
                    //    cmbCriteria.Items.Add("Not Equals");

                    cmbCriteria.Items.Clear();


                    int iRow = dgvCompanySearch.CurrentCell.RowIndex;
                    int iCol = dgvCompanySearch.CurrentCell.ColumnIndex;

                    if (dgvCompanySearch.CurrentCell.OwningColumn.Name == "SearchFrom" || dgvCompanySearch.CurrentCell.OwningColumn.Name == "SearchTo")
                    {
                        frmComboList objfrmComboList = new frmComboList(); //Custom Designed Combobox replacement
                        
                        string sColumnName = dgvCompanySearch.Rows[iRow].Cells["Search On"].Value.ToString();
                        
                        if (dgvCompanySearch.CurrentCell.OwningColumn.Name == "SearchTo" && dgvCompanySearch.Rows[iRow].Cells["Datatype"].Value.ToString() == "Text")
                            return; //Readonly columns which are string

                        if (dgvCompanySearch.Rows[iRow].Cells["Datatype"].Value.ToString() == "Date")
                        {
                            dgvCompanySearch.Rows[iRow].Cells["Criteria"].Value = "Between";
                            if (dgvCompanySearch.CurrentCell.OwningColumn.Name == "SearchFrom")
                                Create_Runtime_DGVEditor(iRow, iCol, dtp);
                            else if (dgvCompanySearch.CurrentCell.OwningColumn.Name == "SearchTo")
                                Create_Runtime_DGVEditor(iRow, iCol, dtp);
                            iRowIndex = iRow;
                        }
                        else if (dgvCompanySearch.Rows[iRow].Cells["Datatype"].Value.ToString() == "Text")
                        {
                            objfrmComboList.TitleText = "Select " + sColumnName;
                            
                            if (dtSelectResult.Columns.Contains(sColumnName))
                            //if (dtSelectResult.Rows.Count > 0)                                
                            {
                                DataTable dtList = dtSelectResult.DefaultView.ToTable(true, sColumnName);
                                if (dtList.Rows.Count > 0)
                                    objfrmComboList.dtItems = dtList;
                                else
                                {
                                    if (dgvCompanySearch.Rows[iRow].Cells["Table"].Value.ToString() == "Contact")
                                        objfrmComboList.dtItems = GV.MSSQL1.BAL_ExecuteQuery("SELECT DISTINCT " + dgvCompanySearch.Rows[iRow].Cells["Search On"].Value.ToString() + " FROM " + GV.sContactTable + ";");
                                    else if (dgvCompanySearch.Rows[iRow].Cells["Table"].Value.ToString() == "Company")
                                        objfrmComboList.dtItems = GV.MSSQL1.BAL_ExecuteQuery("SELECT DISTINCT " + dgvCompanySearch.Rows[iRow].Cells["Search On"].Value.ToString() + " FROM " + GV.sCompanyTable + ";");
                                    else if (dgvCompanySearch.Rows[iRow].Cells["Table"].Value.ToString() == "QC")
                                        objfrmComboList.dtItems = GV.MSSQL1.BAL_ExecuteQuery("SELECT DISTINCT " + dgvCompanySearch.Rows[iRow].Cells["Search On"].Value.ToString() + " FROM " + GV.sProjectID + "_QC;");
                                    else
                                    {
                                        objfrmComboList.dtItems = null;
                                        ToastNotification.Show(this, "List not available", eToastPosition.TopRight);
                                        return;
                                    }

                                }
                            }
                            else
                            {
                                if (dgvCompanySearch.Rows[iRow].Cells["Table"].Value.ToString() == "Contact")
                                    objfrmComboList.dtItems = GV.MSSQL1.BAL_ExecuteQuery("SELECT DISTINCT " + dgvCompanySearch.Rows[iRow].Cells["Search On"].Value.ToString() + " FROM " + GV.sContactTable + ";");
                                else if (dgvCompanySearch.Rows[iRow].Cells["Table"].Value.ToString() == "Company")
                                    objfrmComboList.dtItems = GV.MSSQL1.BAL_ExecuteQuery("SELECT DISTINCT " + dgvCompanySearch.Rows[iRow].Cells["Search On"].Value.ToString() + " FROM " + GV.sCompanyTable + ";");
                                else if (dgvCompanySearch.Rows[iRow].Cells["Table"].Value.ToString() == "QC")
                                    objfrmComboList.dtItems = GV.MSSQL1.BAL_ExecuteQuery("SELECT DISTINCT " + dgvCompanySearch.Rows[iRow].Cells["Search On"].Value.ToString() + " FROM " + GV.sProjectID + "_QC;");
                                else
                                {
                                    objfrmComboList.dtItems = null;
                                    ToastNotification.Show(this, "List not available", eToastPosition.TopRight);
                                    return;
                                }

                            }
                            //else
                            //{
                            //    DataTable dtList = ExecuteSuggestionSQL();
                            //    if (dtList != null)
                            //        objfrmComboList.dtItems = dtList.DefaultView.ToTable(true, sColumnName);
                            //    else
                            //    {
                            //        objfrmComboList.dtItems = null;

                            //    }
                            //}

                            objfrmComboList.lstColumnsToDisplay.Add(sColumnName);
                            objfrmComboList.sColumnToSearch = sColumnName;
                            objfrmComboList.IsMultiSelect = true;
                            objfrmComboList.IncludeEmpty = true;
                            objfrmComboList.IsSingleWordSelection = false;
                            //objfrmComboList.IsMultiSelect = true;
                            objfrmComboList.IsSpellCheckEnabeld = false;
                            objfrmComboList.ShowDialog(this);
                            dgvCompanySearch.CurrentRow.Cells["SearchFrom"].Value = objfrmComboList.sReturn.Replace('|', ',');
                            //dgvCompanySearch.CurrentCell.Value = objfrmComboList.sReturn.Replace('|', ',');
                        }
                        else
                            iRowIndex = -1;
                    }
                    else if (dgvCompanySearch.CurrentCell.OwningColumn.Name == "Criteria")
                    {
                        if (dgvCompanySearch.Rows[iRow].Cells["Datatype"].Value.ToString() == "Number")
                        {
                            cmbCriteria.Items.Add("Between");
                            cmbCriteria.Items.Add("Equals");
                            cmbCriteria.Items.Add("Not Equals");
                            cmbCriteria.Items.Add("Greater than");
                            cmbCriteria.Items.Add("Less than");                            
                            cmbCriteria.Items.Add("Greater than Equals");
                            cmbCriteria.Items.Add("Less than Equals");

                            //cmbCriteria.Items.Add("Between");
                            //cmbCriteria.Items.Remove("Contains");
                        }
                        else if (dgvCompanySearch.Rows[iRow].Cells["Datatype"].Value.ToString() == "Date")
                        {
                            cmbCriteria.Items.Add("Between");

                            //cmbCriteria.Items.Add("Between");
                            //cmbCriteria.Items.Remove("Equals");
                            //cmbCriteria.Items.Remove("Contains");
                            //cmbCriteria.Items.Remove("Not Equals");
                        }
                        else if (dgvCompanySearch.Rows[iRow].Cells["Datatype"].Value.ToString() == "Text")
                        {
                            cmbCriteria.Items.Add("Equals");
                            cmbCriteria.Items.Add("Contains");
                            cmbCriteria.Items.Add("Not Equals");
                        }

                        Create_Runtime_DGVEditor(iRow, iCol, cmbCriteria);
                        cmbCriteria.DroppedDown = true;
                        iRowIndex = iRow;
                        cmbCriteria.Focus();
                    }
                    else if (dgvCompanySearch.CurrentCell.OwningColumn.Name == "Search On" && iRow == dgvCompanySearch.Rows.Count -1)
                    {
                        frmComboList objfrmComboList = new frmComboList(); //Custom Designed Combobox replacement
                        objfrmComboList.TitleText = "Select column to Group";
                        //objfrmComboList.ShowInTaskbar = false;

                        DataTable dtContactColumns = new DataTable();
                        dtContactColumns.Columns.Add("FieldName");

                        foreach (string sColName in GV.lstShowOnCriteriaMasterContacts)
                        {
                            DataRow drNewCol = dtContactColumns.NewRow();
                            drNewCol[0] = sColName;
                            dtContactColumns.Rows.Add(drNewCol);
                        }

                        objfrmComboList.dtItems = dtContactColumns;
                        objfrmComboList.lstColumnsToDisplay.Add("FieldName");
                        objfrmComboList.sColumnToSearch = "FieldName";
                        objfrmComboList.IsMultiSelect = false;
                        objfrmComboList.IsSingleWordSelection = false;
                        //objfrmComboList.IsMultiSelect = true;
                        objfrmComboList.IsSpellCheckEnabeld = false;
                        objfrmComboList.ShowDialog(this);
                        dgvCompanySearch.CurrentRow.Cells["Search On"].Value = objfrmComboList.sReturn.Replace('|', ',');
                        //dgvCompanySearch.CurrentCell.Value = objfrmComboList.sReturn.Replace('|', ',');
                    }
                    else
                    {
                        cmbCriteria.Visible = false;
                        iRowIndex = -1;
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
        private void Create_Runtime_DGVEditor(int iRow, int iCol, System.Windows.Forms.Control ctrl)
        {
            try
            {
                Rectangle RectClickPosition = dgvCompanySearch.GetCellDisplayRectangle(iCol, iRow, false);
                ctrl.Parent = dgvCompanySearch.Parent;
                ctrl.Left = RectClickPosition.Left + dgvCompanySearch.Left;
                ctrl.Top = RectClickPosition.Top + dgvCompanySearch.Top;
                ctrl.Height = RectClickPosition.Height + dgvCompanySearch.Height;
                ctrl.Width = dgvCompanySearch.Columns[iCol].Width;
                ctrl.Visible = true;
                ctrl.BringToFront();
                if (ctrl is DateTimePicker)
                {
                    ctrl.Focus();
                    SendKeys.Send("{F4}");//Opens dropdown automatically
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void dgvCompanySearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Simulate_Cell_Impact();
        }

        //-----------------------------------------------------------------------------------------------------
        private DataTable FormatDatatableToBuildQuery(bool Include_NonTicked_Condition)
        {
            try
            {
                if (dtSearch != null && dtSearch.Rows.Count > 0)
                {
                    dtSearch.AcceptChanges();
                    DataTable dtValiedSearch = dtSearch.Clone();
                    DataRow[] drrCondition = null;
                    string sErrorConditions = string.Empty;

                    if (Include_NonTicked_Condition)
                        drrCondition = dtSearch.Select("LEN(Criteria)> 0");
                    else
                    {
                        //if (IsZeroContactRecordMovement && GV.sAccessTo == "WR")
                        //{
                        //    drrCondition = dtSearch.Select("Primary = 'True' AND LEN(Criteria)> 0 AND TABLE <> 'QC'");
                        //    sErrorConditions += ", QC Status";
                        //}
                        //else
                        //    drrCondition = dtSearch.Select("Primary = 'True' AND LEN(Criteria)> 0");

                            drrCondition = dtSearch.Select("Primary = 'True' AND LEN(Criteria)> 0");
                    }

                    foreach (DataRow dr in drrCondition)
                    {
                        if (dr["Datatype"].ToString() == "Text" && dr["SearchFrom"].ToString().Length > 0)
                            dtValiedSearch.ImportRow(dr);
                        else if (dr["Datatype"].ToString() == "Date" && dr["SearchFrom"].ToString().Length > 0 && dr["SearchTo"].ToString().Length > 0)
                            dtValiedSearch.ImportRow(dr);
                        else if (dr["Datatype"].ToString() == "Number" && dr["Search On"].ToString().Length > 0)
                        {
                            if (dr["Criteria"].ToString() == "Between" && dr["SearchFrom"].ToString().Length > 0 && dr["SearchTo"].ToString().Length > 0)
                                dtValiedSearch.ImportRow(dr);
                            else if (dr["Criteria"].ToString() == "Equals" && dr["SearchFrom"].ToString().Length > 0)
                                dtValiedSearch.ImportRow(dr);
                            else if (dr["Criteria"].ToString() == "Not Equals" && dr["SearchFrom"].ToString().Length > 0)
                                dtValiedSearch.ImportRow(dr);
                            else if ((dr["Criteria"].ToString() == "Greater than" || dr["Criteria"].ToString() == "Less than" || dr["Criteria"].ToString() == "Greater than Equals" || dr["Criteria"].ToString() == "Less than Equals") && dr["SearchFrom"].ToString().Length > 0)
                            {
                                if (dr["SearchFrom"].ToString().Contains(","))
                                    sErrorConditions += ", " + dr["Search On"].ToString();
                                else
                                    dtValiedSearch.ImportRow(dr);
                            }                            
                        }
                    }

                    dtValiedSearch.Columns.Remove("Primary");
                    foreach (DataRow drSearch in dtValiedSearch.Rows)
                    {
                        if (drSearch["SearchFrom"].ToString().ToUpper() == "(EMPTY)")
                            drSearch["SearchFrom"] = string.Empty;

                        if (drSearch["SearchTo"].ToString().ToUpper() == "(EMPTY)")
                            drSearch["SearchTo"] = string.Empty;                        
                    }


                    if (dtValiedSearch.Rows.Count > 0)
                    {
                        if (sErrorConditions.Length > 0)
                        {
                            ToastNotification.ToastFont = new System.Drawing.Font(this.Font.FontFamily, 12);
                            ToastNotification.Show(this, "Error: Ignoring conditions " + sErrorConditions.Remove(0,2), eToastPosition.TopRight);
                            ToastNotification.ToastFont = new System.Drawing.Font(this.Font.FontFamily, 22);
                        }

                        return dtValiedSearch;
                    }
                    else
                    {
                        ToastNotification.Show(this, "Filter must contain condition.", eToastPosition.TopRight);
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private string BuildQuery(DataTable dtConditions)//Build Qurery based on Conditions
        {
            string sSQLText = string.Empty;
            try
            {
                if (dtConditions != null && dtConditions.Rows.Count > 0)
                {

                    DataRow[] drrContactCountExist = dtConditions.Select("Table = 'Contact Count'");
                    DataRow[] drrConditions = null;
                    DataRow[] drrContactGroup = null;
                    if (drrContactCountExist.Length > 0)
                    {
                        drrConditions = dtConditions.Select("[Search on] <> '" + drrContactCountExist[0]["Search On"] + "'");
                        drrContactGroup = dtConditions.Select("[Search on] = '" + drrContactCountExist[0]["Search On"] + "'");
                    }
                    else
                        drrConditions = dtConditions.Select("1=1");


                    foreach (DataRow drConditions in drrConditions)//Loop through rows (Append 'AND' betwwen rows)
                    {
                        List<string> lstValue = new List<string>();
                        string sCondition = drConditions["Criteria"].ToString();
                        string sFieldName =  drConditions["Search On"].ToString();                        

                        if (dtMasterCompany == null)
                            return string.Empty;

                        if (drConditions["Table"].ToString() == "QC") // Changing alias QC to Contact since contact and QC table are considered as same table in select Query. This will help with the problem faced with Left outer Join, Innser Join combination SELECT * FROM CRUCRU005_MASTERCOMPANIES Company LEFT OUTER JOIN (SELECT * FROM CRUCRU005_MASTERCONTACTS Contact INNER JOIN CRUCRU005_QC QC ON Contact.CONTACT_ID_P = QC.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID
                            drConditions["Table"] = "Contact";

                        sFieldName = "ISNULL(" + drConditions["Table"].ToString() + "." + sFieldName + ",'')";

                        if (sSQLText.Length > 0)
                            sSQLText += " AND ";

                        string sQueryCondition = FormCondition(sCondition, drConditions["SearchFrom"].ToString().Trim().Replace("'", "''"), drConditions["SearchTo"].ToString().Trim().Replace("'", "''"), drConditions["Datatype"].ToString(), sFieldName);

                        sSQLText += sQueryCondition;

                        #region Old

                        //sSQLText += "(";

                        //string sInnerCondition = string.Empty;
                        //if (sCondition == "Between")
                        //{
                        //    string sFromValue;
                        //    string sToValue;

                        //    sFromValue = drConditions["SearchFrom"].ToString().Trim();
                        //    sToValue = drConditions["SearchTo"].ToString().Trim();

                        //    if (GV.sUserType == "Agent")
                        //    {
                        //        if (drConditions["Datatype"].ToString() == "Date")
                        //        {
                        //            sFromValue = "'" + sFromValue + "'";
                        //            sToValue = "'" + sToValue + "'";
                        //        }
                        //        sInnerCondition += sFieldName + " >= " + sFromValue + " AND " + sFieldName + " <= " + sToValue;
                        //    }
                        //    else
                        //    {
                        //        if (drConditions["Datatype"].ToString() == "Date")
                        //        {
                        //            sFromValue = "'" + Convert.ToDateTime(sFromValue).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        //            sToValue = "'" + Convert.ToDateTime(sToValue).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        //        }
                        //        sInnerCondition += sFieldName + " >= " + sFromValue + " AND " + sFieldName + " <= " + sToValue;
                        //    }
                        //    //sSQLText += sInnerCondition;
                        //}
                        //else
                        //{
                        //    lstValue = drConditions["SearchFrom"].ToString().Replace(",,",",").Split(',').ToList();
                        //    foreach (string sValue in lstValue) //Loop through multiple values if exist(Multiple values are suprated by ','(Comma))
                        //    {                                   //Append 'OR' Between Values for 'CONTAINS'(Like) condition                                
                        //        //Use IN function for values using 'EQUALS or NOT EQUALS'
                        //        if (sInnerCondition.Length > 0 && sCondition == "Contains")
                        //            sInnerCondition += " OR ";

                        //        if (sCondition == "Equals")
                        //        {
                        //            if (sInnerCondition.Length == 0)
                        //                sInnerCondition += sFieldName + " IN ('" + sValue + "'";
                        //            else
                        //                sInnerCondition += ",'" + sValue + "'";
                        //        }
                        //        else if (sCondition == "Not Equals")
                        //        {
                        //            if (sInnerCondition.Length == 0)
                        //                sInnerCondition += sFieldName + " NOT IN ('" + sValue + "'";
                        //            else
                        //                sInnerCondition += ",'" + sValue + "'";
                        //        }
                        //        else//Like Condition
                        //            sInnerCondition += sFieldName + " like '%" + sValue + "%'";
                        //    }
                        //}

                        //if (sCondition == "Contains" || sCondition == "Between")
                        //    sSQLText += sInnerCondition + ")";
                        //else
                        //    sSQLText += sInnerCondition + "))"; 
                        #endregion
                    }

                    if (drrContactGroup != null)
                    {
                        //SELECT GROUP_CONCAT(MASTER_ID) FROM (SELECT MASTER_ID FROM crucru005_mastercontacts WHERE TR_CONTACT_STATUS IN ('asdasd','asdfsdaf') GROUP BY MASTER_ID HAVING COUNT(*) = 3)T;
                        int iGroupIndex;
                        int iConditionIndex;
                        string sQueryCondition = string.Empty;
                        if (drrContactGroup.Length == 2)
                        {
                            iGroupIndex = (drrContactGroup[0]["Table"].ToString() == "Contact Count") ? 0 : 1;
                            iConditionIndex = (drrContactGroup[0]["Table"].ToString() == "Contact") ? 0 : 1;
                            sQueryCondition = FormCondition(drrContactGroup[iConditionIndex]["Criteria"].ToString(), drrContactGroup[iConditionIndex]["SearchFrom"].ToString().Trim(), drrContactGroup[iConditionIndex]["SearchTo"].ToString().Trim(), drrContactGroup[iConditionIndex]["Datatype"].ToString(), "ISNULL(GRP." + drrContactGroup[iConditionIndex]["Search On"].ToString() + ",'')");
                        }
                        else                        
                            iGroupIndex = 0;

                        string sFromValue = drrContactGroup[iGroupIndex]["SearchFrom"].ToString().Replace("'", "''");
                        string sToValue = drrContactGroup[iGroupIndex]["SearchTo"].ToString().Replace("'", "''");                            

                        string sGroupBy = string.Empty;
                        switch (drrContactGroup[iGroupIndex]["Criteria"].ToString())
                        {
                            case "Between":
                                sGroupBy = " COUNT(*) BETWEEN " + sFromValue + " AND " + sToValue;
                                break;
                            case "Equals":
                                sGroupBy = " COUNT(*) = " + sFromValue;
                                break;
                            case "Not Equals":
                                sGroupBy = " COUNT(*) <> " + sFromValue;
                                break;
                            case "Greater than":
                                sGroupBy = " COUNT(*) > " + sFromValue;
                                break;
                            case "Greater than Equals":
                                sGroupBy = " COUNT(*) >= " + sFromValue;
                                break;
                            case "Less than":
                                sGroupBy = " COUNT(*) < " + sFromValue;
                                break;
                            case "Less than Equals":
                                sGroupBy = " COUNT(*) <= " + sFromValue;
                                break;
                        }

                        string sGroupQuery = string.Empty;

                        if(sQueryCondition.Trim().Length > 0)
                           sGroupQuery = "SELECT GRP.MASTER_ID FROM " + GV.sContactTable + " GRP WHERE " + sQueryCondition + " GROUP BY GRP.MASTER_ID HAVING " + sGroupBy + "";
                        else
                            sGroupQuery = "SELECT GRP.MASTER_ID FROM " + GV.sContactTable + " GRP GROUP BY GRP.MASTER_ID HAVING " + sGroupBy + "";

                        if (sSQLText.Length > 0)
                            sSQLText += " AND (Company.MASTER_ID IN (" + sGroupQuery + "))";
                        else
                            sSQLText += " (Company.MASTER_ID IN (" + sGroupQuery + "))";
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


        string FormCondition(string sCondition, string sFromValue, string sToValue, string sDatatype, string sFieldName)
        {
            string sSQLText = "(";
            string sInnerCondition = string.Empty;
            if (sCondition == "Between" || sDatatype == "Number")
            {                                
                //if (GV.sUserType == "Agent")
                //{
                    if (sDatatype == "Date")
                    {
                        sFromValue = "'" + sFromValue + "'";
                        sToValue = "'" + sToValue + "'";
                        sInnerCondition = sFieldName + " >= " + sFromValue + " AND " + sFieldName + " <= " + sToValue;
                    }
                    else if (sDatatype == "Number")
                    {                        
                        switch (sCondition)
                        {
                            case "Between":
                                sInnerCondition = sFieldName + " BETWEEN " + sFromValue + " AND " + sToValue;
                                break;
                            case "Equals":
                                sInnerCondition = sFieldName + " IN (" + sFromValue + ")";
                                break;
                            case "Not Equals":
                                sInnerCondition = sFieldName + " NOT IN (" + sFromValue + ")";
                                break;
                            case "Greater than":
                                sInnerCondition = sFieldName + " > " + sFromValue;
                                break;
                            case "Greater than Equals":
                                sInnerCondition = sFieldName + " >= " + sFromValue;
                                break;
                            case "Less than":
                                sInnerCondition = sFieldName + " < " + sFromValue;
                                break;
                            case "Less than Equals":
                                sInnerCondition = sFieldName + " <= " + sFromValue;
                                break;                                                                
                        }                        
                    }
                //}
                //else
                //{
                //    if (sDatatype == "Date")
                //    {
                //        sFromValue = "'" + Convert.ToDateTime(sFromValue).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                //        sToValue = "'" + Convert.ToDateTime(sToValue).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                //    }
                //    sInnerCondition += sFieldName + " >= " + sFromValue + " AND " + sFieldName + " <= " + sToValue;
                //}
            }
            else
            {
                List<string> lstValue = sFromValue.Replace(",,", ",").Split(',').ToList();
                foreach (string sValue in lstValue) //Loop through multiple values if exist(Multiple values are suprated by ','(Comma))
                {                                   //Append 'OR' Between Values for 'CONTAINS'(Like) condition                                
                    //Use IN function for values using 'EQUALS or NOT EQUALS'
                    if (sInnerCondition.Length > 0 && sCondition == "Contains")
                        sInnerCondition += " OR ";

                    if (sCondition == "Equals")
                    {
                        if (sInnerCondition.Length == 0)
                            sInnerCondition += sFieldName + " IN ('" + sValue.Trim() + "'";
                        else
                            sInnerCondition += ",'" + sValue.Trim() + "'";
                    }
                    else if (sCondition == "Not Equals")
                    {
                        if (sInnerCondition.Length == 0)
                            sInnerCondition += sFieldName + " NOT IN ('" + sValue.Trim() + "'";
                        else
                            sInnerCondition += ",'" + sValue.Trim() + "'";
                    }
                    else//Contains
                        sInnerCondition += sFieldName + " like '%" + sValue.Trim() + "%'";
                }
            }

            if (sCondition == "Contains" || sCondition == "Between" || sDatatype == "Number")
                sSQLText += sInnerCondition + ")";
            else
                sSQLText += sInnerCondition + "))";

            return sSQLText;
        }


        //-----------------------------------------------------------------------------------------------------
        private void dgvCompanyList_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn dgvc in dgvCompanyList.Columns)
            {
                if (GV.lstShowOnGridMasterCompanies.Contains(dgvc.Name, StringComparer.OrdinalIgnoreCase))
                    dgvc.HeaderText = GM.ProperCase_ProjectSpecific(dgvc.Name.Replace("_", " "));
                else
                    dgvc.Visible = false;
            }            
        }

        //-----------------------------------------------------------------------------------------------------
        private void dgvCompanySearch_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvCompanySearch.CommitEdit(DataGridViewDataErrorContexts.Commit); //Datagrid changes commit
            //immediate effect on textbox change to datagrid
        }

        //-----------------------------------------------------------------------------------------------------
        private void dgvCompanyList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
                GM.OpenContactUpdate(dgvCompanyList.Rows[e.RowIndex].Cells["MASTER_ID"].Value.ToString(), false, true,this,this);

            //if (GlobalVariables.sUserType == "Agent")
            //    OpenContactUpdate(dgvCompanyList.Rows[e.RowIndex].Cells["ID"].Value.ToString(), false, false);
            //else
            //    OpenContactUpdate(dgvCompanyList.Rows[e.RowIndex].Cells["MASTER_ID"].Value.ToString(), false, false);

            //Color changes// dntknow why.. Got to check it out
            //dgvCompanyList.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
            //dgvCompanySearch.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
        }

        //-----------------------------------------------------------------------------------------------------
        private void dgvCompanyList_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Right && e.Modifiers == Keys.Control) || (e.KeyCode == Keys.Left && e.Modifiers == Keys.Control))
                e.Handled = true;
            else if (e.KeyCode == Keys.Enter)//Open record on enter
            {
                if (dgvCompanyList.DataSource != null && dgvCompanyList.Rows.Count > 0 && dgvCompanyList.CurrentRow != null)
                    GM.OpenContactUpdate(dgvCompanyList.CurrentRow.Cells["Master_ID"].Value.ToString(), false, false,this,this);
                e.Handled = true;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            /////Key press listener for overall window/////

            //ToastNotification.Show(this, keyData.ToString(), eToastPosition.TopRight);

            if (keyData == (Keys.Control | Keys.Right))
                ((frmMain)MdiParent).tabItemShortcutHandle("Right");
            if (keyData == (Keys.Control | Keys.Left))
                ((frmMain)MdiParent).tabItemShortcutHandle("Left");

            if (keyData == Keys.F6)
            {
                if (!splitCompanyList.Panel1Collapsed)
                {
                    if (dgvCompanyList.Focused)
                        dgvCompanySearch.Focus();
                    else
                        dgvCompanyList.Focus();
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        //-----------------------------------------------------------------------------------------------------
        private void NavigateFilteredList()
        {
            GM.dtFilteredCompanyList = (DataTable)dgvCompanyList.DataSource;
        }        

        //-----------------------------------------------------------------------------------------------------
        public void ExportExcel()
        {
            try
            {
                sExcelExportQuery = string.Empty;
                sExcelExportColumns = string.Empty;
                sExcelExportPath = string.Empty;
                iExcelExportStatus = 0;
                if (DialogResult.OK == saveFileDialogExportToExcel.ShowDialog())
                {
                    System.IO.FileInfo fileSavePath = new System.IO.FileInfo(saveFileDialogExportToExcel.FileName);

                    try
                    {
                        if (fileSavePath.Exists)
                            fileSavePath.Delete();  // ensures we create a new workbook
                    }
                    catch (Exception ex1)
                    {
                        MessageBoxEx.Show("The Excel file is already opened.<br/>Please close the file or try different file name.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }                    
                                   
                    //sQueryFull = "SELECT * FROM inbinb008_mastercontacts im limit 100000";



                    if (saveFileDialogExportToExcel.FilterIndex == 1)// fileSavePath.Extension.ToUpper() == ".CSV")
                    {
                        #region Old Export Type
                        //if (dtToExport.Rows.Count > 1048576)
                        //{
                        //    MessageBoxEx.Show("Number of rows exceeds 1048576.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //    return;
                        //}
                        //else
                        //{
                        //    XLWorkbook wb = new XLWorkbook();//Closed XML
                        //    IXLWorksheet ws = wb.Worksheets.Add(sProjectName + "_" + GM.GetDateTime().ToString("yyyyMMdd"));
                        //    ws.Cell(1, 1).InsertTable(dtToExport.AsEnumerable());
                        //    try
                        //    {
                        //        wb.SaveAs(fileSavePath.FullName);


                        //    }
                        //    catch (Exception ex2)
                        //    {
                        //        string s = "\"" + fileSavePath.FullName + "\"";
                        //        ProcessStartInfo cmdInfo = new ProcessStartInfo(GV.sOSHandlerPath);
                        //        cmdInfo.Arguments = s;
                        //        cmdInfo.WorkingDirectory = fileSavePath.Directory.FullName;
                        //        cmdInfo.CreateNoWindow = true;
                        //        cmdInfo.RedirectStandardOutput = true;
                        //        cmdInfo.UseShellExecute = false;
                        //        Process cmd = new Process();
                        //        cmd.StartInfo = cmdInfo;
                        //        cmd.Start();
                        //        string sHandle = cmd.StandardOutput.ReadToEnd();
                        //        cmd.WaitForExit();
                        //        cmd.Close();
                        //        sHandle = sHandle.Replace(" ", "").Replace(Environment.NewLine, "").Replace(@"Handlev4.0Copyright(C)1997-2014MarkRussinovichSysinternals-www.sysinternals.com", string.Empty);
                        //        string sHandleID = Get_Betweenstr(sHandle, "type:File", ":");
                        //        string sPID = Get_Betweenstr(sHandle, "pid:", "type:File");
                        //        if (sHandleID.Length > 0 && sPID.Length > 0)
                        //        {
                        //            cmdInfo.Arguments = "-c " + sHandleID + " -p " + sPID + " /y";
                        //            cmd.StartInfo = cmdInfo;
                        //            cmd.Start();
                        //            cmd.WaitForExit();
                        //            cmd.Close();

                        //            try
                        //            {
                        //                if (File.Exists(fileSavePath.FullName))
                        //                    fileSavePath.Delete();  // ensures we create a new workbook

                        //                ExcelPackage eExcelPackage = new ExcelPackage(fileSavePath);//OpenedXML
                        //                ExcelWorksheet eExcelWorkSheet = eExcelPackage.Workbook.Worksheets.Add(sProjectName + "_" + GM.GetDateTime().ToString("yyyyMMdd"));// add a new worksheet to the empty workbook
                        //                eExcelWorkSheet.Cells["A1"].LoadFromDataTable(dtToExport, true);
                        //                eExcelPackage.Save();
                        //            }
                        //            catch (Exception ex1)
                        //            {
                        //                MessageBoxEx.Show("Export Failed. Try file type Excel File 2.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //                return;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            MessageBoxEx.Show("Export Failed. Try file type Excel File 2.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //            return;
                        //        }
                        //    }
                        //} 
                        #endregion


                        string sQueryFull = string.Empty;

                        string sCompanyColumns = string.Empty;
                        string sContactColumns = string.Empty;
                        string sColumns = string.Empty;

                        foreach (ListViewItem lstCompany in lstColumnsToExportCompany.Items)
                        {
                            if (sCompanyColumns.Length > 0)
                                sCompanyColumns += ", Company." + lstCompany.Text + " AS Company_" + lstCompany.Text.Replace("Company_", "");
                            else
                                sCompanyColumns = "Company." + lstCompany.Text + " AS Company_" + lstCompany.Text.Replace("Company_", "");
                        }

                        foreach (ListViewItem lstContact in lstColumnsToExportContact.Items)
                        {
                            if (sContactColumns.Length > 0)
                                sContactColumns += ", Contact." + lstContact.Text + " AS Contact_" + lstContact.Text.Replace("Contact_", "");
                            else
                                sContactColumns = "Contact." + lstContact.Text + " AS Contact_" + lstContact.Text.Replace("Contact_", "");
                        }

                        if (sCompanyColumns.Length > 0)
                            sColumns = "Company.Master_ID, " + sCompanyColumns;

                        if (sContactColumns.Length > 0)
                            sColumns = sColumns + ", Contact.Contact_ID_P, " + sContactColumns;

                        sColumns = sColumns + ", Contact.ID AS QC_ID, Contact.TableName AS QC_TableName, Contact.QC_Status, Contact.QC_Comments, Contact.QC_UpdatedDate, Contact.QC_UpdatedBy, Contact.QC_Sample_Status, Contact.ResearchType AS QC_ResearchType, Contact.Reprocessed_Date AS QC_Reprocessed_Date";

                        //string sPrefix = " FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
                        //sPrefix += " LEFT OUTER JOIN " + GV.sQCTable + " QC ON (Contact.CONTACT_ID_P = QC.RecordID AND QC.TableName='Contact' AND QC.ResearchType='" + GV.sAccessTo + "') ";

                        string sPrefix = " FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN  (SELECT * FROM " + GV.sContactTable + " C LEFT OUTER JOIN " + GV.sProjectID + "_QC Q ON C.CONTACT_ID_P = Q.RECORDID AND Q.TableName='Contact' AND Q.ResearchType='" + GV.sAccessTo + "') Contact ON Company.MASTER_ID = Contact.MASTER_ID ";

                        if (switchExportType.Value)
                        {
                            string sQueryToExport = BuildQuery(FormatDatatableToBuildQuery(true));
                            if (sQueryToExport.Trim().Length > 0)
                                sQueryFull = sPrefix + " WHERE " + sQueryToExport;
                            else
                            {
                                if (GV.sUserType != "Agent")
                                    sQueryFull = sPrefix + " WHERE 1=1";                                
                            }
                        }
                        else
                        {
                            string sAccess = (switchButtonExportResearch.Value ? "TR" : "WR");

                            if (dateInputExport.ToString().Length > 0)
                            {
                                if (rdoContactProcessed.Checked)
                                    sQueryFull = sPrefix + " WHERE CAST(" + sAccess + "_UPDATED_DATE As DATE) = '" + dateInputExport.Value.ToString("yyyyMMdd") + "'";
                                else
                                {
                                    string sDateColumn = string.Empty;
                                    if (sAccess == "WR")
                                        sDateColumn = "WR_DATE_OF_PROCESS";
                                    else
                                        sDateColumn = "TR_DATECALLED";

                                    sQueryFull = sPrefix + " WHERE CAST(" + sDateColumn + " As DATE) = '" + dateInputExport.Value.ToString("yyyyMMdd") + "'";
                                }
                            }
                            else
                                ToastNotification.Show(this, "Select processed date", eToastPosition.TopRight);
                        }

                        if (sQueryFull.Length > 0)
                        {
                            sExcelExportQuery = sQueryFull;
                            sExcelExportColumns = sColumns;
                            sExcelExportPath = fileSavePath.FullName;
                            btnExport.Enabled = false;
                            switchExportType.Enabled = false;
                            groupPanelQuickExport.Enabled = false;
                            circularProgressExport.Value = 0;
                            circularProgressExport.ProgressTextVisible = true;
                            bWorkerExport.RunWorkerAsync();
                        }


                    }
                    else
                        return;
                    //else if (saveFileDialogExportToExcel.FilterIndex == 1)// fileSavePath.Extension.ToUpper() == ".XLSX")
                    //{

                    //    string sSheetName = string.Empty;
                    //    if (GV.sProjectName.Length > 15)
                    //        sSheetName = GV.sProjectName.Substring(0, 15);
                    //    else
                    //        sSheetName = GV.sProjectName;

                    //    string sQueryToExport = BuildQuery(FormatDatatableToBuildQuery(true));
                    //    string sQueryFull = string.Empty;
                    //    string sPrefix = "SELECT * FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
                    //    sPrefix += " LEFT OUTER JOIN " + GV.sQCTable + " QC ON (Contact.CONTACT_ID_P = QC.RecordID AND QC.TableName='Contact' AND QC.ResearchType='" + GV.sAccessTo + "') ";

                    //    if (sQueryToExport.Trim().Length > 0 && dgvCompanyList.Rows.Count > 0)
                    //        sQueryFull = sPrefix + " WHERE " + sQueryToExport;
                    //    else if (GV.sUserType != "Agent")
                    //        sQueryFull = sPrefix;
                    //    else
                    //    {
                    //        ToastNotification.Show(this, "Filter doesn't return value(s)", eToastPosition.TopRight);
                    //        return;
                    //    }
                    //    DataTable dtToExport = GV.MaYSQL.BAL_ExecuteQueryMydSQL(sQueryFull);

                    //    //if (saveFileDialogExportToExcel.FilterIndex == 2)
                    //    //{
                    //        if (dtToExport.Rows.Count > 800000)
                    //        {
                    //            MessageBoxEx.Show("Number of rows exceeds 800000.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            ExcelPackage eExcelPackage = new ExcelPackage(fileSavePath);//OpenedXML
                    //            ExcelWorksheet eExcelWorkSheet = eExcelPackage.Workbook.Worksheets.Add(sSheetName + "_" + GM.GetDateTime().ToString("yyyyMMdd"));// add a new worksheet to the empty workbook
                    //            eExcelWorkSheet.Cells["A1"].LoadFromDataTable(dtToExport, true);
                    //            eExcelPackage.Save();
                    //        }
                    ////}
                    ////else                             

                    ////    dtToExport.WriteToCsvFile(fileSavePath.FullName);
                    //}                    
                    //MessageBoxEx.Show("Exported sucessfully", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        

        string Get_Betweenstr(string sText, string sStart, string sEnd)
        {
            if (sText.Contains(sStart) && sText.Contains(sEnd))
            {
                sText = sText.Split(new string[] { sStart }, StringSplitOptions.None).ToList()[1];
                if (sText.Contains(sEnd))
                {
                    if (sText.Contains(sEnd))
                    {
                        sText = sText.Split(new string[] { sEnd }, StringSplitOptions.None).ToList()[0];
                        return sText;
                    }
                    else
                        return string.Empty;
                }
                else
                    return string.Empty;
            }
            else
                return string.Empty;
        }

        //string Get_LostData_ID()
        //{
        //    DataTable dtCompanyLost = GV.SQLCE.BAL_FetchTable(GV.sSQLCECompanyTable, "1=1");
        //    DataTable dtContactLost = GV.SQLCE.BAL_FetchTable(GV.sSQLCEContactTable, "1=1");

        //    if (dtCompanyLost.Rows.Count > 0 || dtContactLost.Rows.Count > 0)
        //    {
        //        string sID = string.Empty;

        //        if (dtContactLost.Rows.Count > 0)
        //            sID = dtContactLost.Rows[0]["MASTER_ID"].ToString();
        //        else
        //            sID = dtCompanyLost.Rows[0]["MASTER_ID"].ToString();
        //        return sID;
        //    }
        //    else
        //        return string.Empty;
        //}

        //void recover()
        //{
        //    string sID = Get_LostData_ID();
        //    if (sID.Length > 0)
        //    {
        //        DataTable dtComp = GV.MfYSQL.BAL_FetchTableMydSQL(GV.sCompanyTable, "FLAG = '" + GV.sAccessTo + "' AND MASTER_ID =" + sID);
        //        DataTable dtContactLost = GV.SQLCE.BAL_FetchTable(GV.sSQLCEContactTable, "MASTER_ID = " + sID);
        //        if (dtComp.Rows.Count > 0)
        //        {
        //            if (MessageBoxEx.Show("Campaign Manager has detected a company : <b>" + dtComp.Rows[0]["COMPANY_NAME"].ToString() + "</b> with <b>" + dtContactLost.Rows.Count + "</b> contacts has not been saved.<br/>  • To save this company, Click Yes.<br/> • To discard this company, Click No.", "Campaign Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
        //            {
        //                while (true)
        //                {
        //                    if (dtComp.Rows[0][GV.sAccessTo + "_AGENTNAME"].ToString().ToUpper() == "CURRENT_" + GV.sEmployeeName.ToUpper())
        //                    {
        //                        FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(sID, this.MdiParent, "ListOpen", false, this);
        //                        objfrmContactsUpdate.Show();
        //                        return;//Show only records in stack(Current_AgentName)
        //                    }
        //                    else if (dtComp.Rows[0][GV.sAccessTo + "_AGENTNAME"].ToString().StartsWith("Currnet"))
        //                    {
        //                        if (MessageBoxEx.Show("The record already opened by <b>" + GM.ProperCase_ProjectSpecific(dtComp.Rows[0][GV.sAccessTo + "_AGENTNAME"].ToString().Replace("Current_", string.Empty)) + "</b>", "Campaign Manager", MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop) == System.Windows.Forms.DialogResult.Retry)
        //                            dtComp = GV.MYfSQL.BAL_FetchTableMydSQL(GV.sCompanyTable, "FLAG = '" + GV.sAccessTo + "' AND MASTER_ID =" + sID+ " AND MASTER_ID = GROUP_ID");
        //                        else
        //                        {
        //                            GV.SQLCE.BAL_DeleteFromTable(GV.sSQLCECompanyTable, "Master_ID = " + sID);
        //                            GV.SQLCE.BAL_DeleteFromTable(GV.sSQLCEContactTable, "Master_ID = " + sID);
        //                            return;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        GV.MYfdSQL.BAL_ExecuteQueryMydSQL("UPDATE " + GV.sCompanyTable + " SET " + GV.sAccessTo + "_AGENTNAME = 'Current_" + GV.sEmployeeName + "' WHERE MASTER_ID = " + sID + " AND MASTER_ID = GROUP_ID;");
        //                        FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(sID, this.MdiParent, "ListOpen", false, this);
        //                        objfrmContactsUpdate.Show();
        //                        return;
        //                    }

        //                }
        //            }
        //            else
        //            {
        //                if (sID.Length > 0)
        //                {
        //                    GV.SQLCE.BAL_DeleteFromTable(GV.sSQLCECompanyTable, "Master_ID = " + sID);
        //                    GV.SQLCE.BAL_DeleteFromTable(GV.sSQLCEContactTable, "Master_ID = " + sID);
        //                }
        //            }
        //        }
        //    }

        //    //DataTable dt = new DataTable();
        //    //if (IsOpenbyID)
        //    //{
        //    //    dt = GV.MYdfSQL.BAL_FetchTableMydSQL(GV.sCompanyTable, "MASTER_ID = " + sID + " AND FLAG = '" + GV.sAccessTo + "'");
        //    //    if (dt.Rows.Count > 0 && dt.Rows[0][GV.sAccessTo + "_AGENTNAME"].ToString().StartsWith("Current") && dt.Rows[0][GV.sAccessTo + "_AGENTNAME"].ToString().Replace("Current_", "").ToUpper() != GV.sEmployeeName.ToUpper())
        //    //    {
        //    //        MessageBoxEx.Show("This record is already in use by <font size = '10' color='OrangeRed'><b>" + GM.ProperCase(dt.Rows[0][GV.sAccessTo + "_AGENTNAME"].ToString().Replace("Current_", string.Empty)) + "</b></font>", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //    //        return;
        //    //    }
        //    //    else if (dt.Rows.Count > 0)
        //    //    {

        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    dt = GV.MYSfdQL.BAL_FetchTableMydSQL(GV.sCompanyTable, GV.sAccessTo + "_AGENTNAME = 'Current_" + GV.sEmployeeName + "'");
        //    //    if (dt.Rows.Count > 0)
        //    //    {
        //    //        MessageBoxEx.Show("Some contact(s) not saved properly. Opening them now.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    //        if (IsFormOpen == false && GV.sCompanyTable.Length > 0 && GV.sContactTable.Length > 0)
        //    //        {
        //    //            FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(null, this.MdiParent, this, "ListOpen", false);
        //    //            objfrmContactsUpdate.Show();
        //    //            return;//Show only records in stack(Current_AgentName)
        //    //        }
        //    //    }
        //    //    else
        //    //        dt = GV.MYfdSQL.BAL_FetchTableMydSQL(GV.sCompanyTable, "MASTER_ID = " + sID + " AND " + GV.sAccessTo + "_AGENTNAME = '" + GV.sEmployeeName + "'");
        //    //}

        //    //if (dt.Rows.Count > 0)
        //    //{
        //    //    if (IsFormOpen == false && GV.sCompanyTable.Length > 0 && GV.sContactTable.Length > 0)
        //    //    {
        //    //        GV.MYfdSQL.BAL_ExecuteQueryMdySQL("UPDATE " + GV.sCompanyTable + " SET " + GV.sAccessTo + "_AGENTNAME = 'Current_" + GV.sEmployeeName + "' WHERE MASTER_ID = " + sID + ";");
        //    //        FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(null, this.MdiParent, this, "ListOpen", false);
        //    //        objfrmContactsUpdate.Show();
        //    //    }
        //    //}
        //}

        string sContactID_QueryString(string sImportType, DataTable dtImport)
        {
            string sContactIDs = string.Empty;
            if (sImportType == "Bounce")
            {
                foreach (DataRow drImport in dtImport.Rows)
                {
                    if (drImport["CONTACT_ID_P"].ToString().Trim().Length > 0 && drImport["Bounce"].ToString().Trim().Length > 0)
                    {
                        if (sContactIDs.Length == 0)
                            sContactIDs = drImport["CONTACT_ID_P"].ToString();
                        else
                            sContactIDs += "," + drImport["CONTACT_ID_P"];
                    }
                }
            }
            else if (sImportType == "OK" || sImportType == "Upload Tag")
            {
                foreach (DataRow drImport in dtImport.Rows)
                {
                    if (drImport["CONTACT_ID_P"].ToString().Trim().Length > 0)
                    {
                        if (sContactIDs.Length == 0)
                            sContactIDs = drImport["CONTACT_ID_P"].ToString();
                        else
                            sContactIDs += "," + drImport["CONTACT_ID_P"];
                    }
                }
            }
            else
            {
                foreach (DataRow drImport in dtImport.Rows)
                {
                    if (drImport["CONTACT_ID_P"].ToString().Trim().Length > 0 && drImport["Rejection"].ToString().Trim().Length > 0)
                    {
                        if (sContactIDs.Length == 0)
                            sContactIDs = drImport["CONTACT_ID_P"].ToString();
                        else
                            sContactIDs += "," + drImport["CONTACT_ID_P"];
                    }
                }
            }

            return sContactIDs;
        }

       public DataTable Update_QCTable(string sContactID, DataTable dtMainTable, DataTable dtQCTable, string sTableName, string sStatus, string sQCComments)
        {
            if (dtQCTable.Select("RecordID = " + sContactID + " AND TableName = '" + sTableName + "' AND ResearchType = '" + GV.sAccessTo + "'").Length > 0)
            {
                foreach (DataRow drQCTable in dtQCTable.Rows)
                {
                    if (drQCTable["RecordID"].ToString() == sContactID)
                    {
                        drQCTable["QC_Status"] = sStatus;
                        drQCTable["TableName"] = sTableName;
                        drQCTable["RecordID"] = sContactID;
                        drQCTable["ResearchType"] = GV.sAccessTo;                        
                        drQCTable["QC_UpdatedDate"] = GM.GetDateTime();
                        drQCTable["QC_UpdatedBy"] = GV.sEmployeeName;
                        drQCTable["QC_Comments"] = sQCComments;
                        break;
                    }
                }
            }
            else
            {
                DataRow drNewQCRow = dtQCTable.NewRow();
                drNewQCRow["QC_Status"] = sStatus;
                drNewQCRow["TableName"] = sTableName;
                drNewQCRow["RecordID"] = sContactID;
                drNewQCRow["ResearchType"] = GV.sAccessTo;                
                drNewQCRow["QC_UpdatedDate"] = GM.GetDateTime();
                drNewQCRow["QC_UpdatedBy"] = GV.sEmployeeName;
                drNewQCRow["QC_Comments"] = sQCComments;
                dtQCTable.Rows.Add(drNewQCRow);
            }

            return dtQCTable;
        }

        //-----------------------------------------------------------------------------------------------------
        public void Bounce_Rejection_OK(DataTable dtImport, string sImportType, string sComments)
        {
            DataTable dtContacts = null;            
            try
            {
                string sContactIDsImported = string.Empty;
                string sContactIDsDB = string.Empty;
                string sMismatch = string.Empty;
                int iRecordCount = 0;
                DataTable dtQCTable = null;
                sContactIDsImported = sContactID_QueryString(sImportType, dtImport);
                if (sContactIDsImported.Length > 0)
                {
                    if (sImportType == "Bounce") //Bounced records
                    {
                        dtContacts = GV.MSSQL1.BAL_FetchTable(GV.sContactTable, "CONTACT_ID_P IN (" + sContactIDsImported + ") AND " + GV.sAccessTo + "_UNCERTAIN_STATUS IN (0,1);");
                        DataTable dtEmailStatus = GV.MSSQL1.BAL_FetchTable("c_picklists", "PicklistCategory='EmailStatus'");
                        iRecordCount = dtContacts.Rows.Count;
                        sContactIDsDB= GM.ColumnToQString("CONTACT_ID_P", dtContacts, "Int");
                        if (sContactIDsDB.Length > 0)
                        {
                            dtQCTable = GV.MSSQL1.BAL_FetchTable(GV.sQCTable, "RecordID IN (" + sContactIDsDB + ") AND TableName = 'Contact' AND ResearchType = '"+GV.sAccessTo+"'");
                            foreach (DataRow dr in dtContacts.Rows)
                            {
                                DataRow[] drrBounceImport = dtImport.Select("CONTACT_ID_P = '" + dr["CONTACT_ID_P"] + "'");
                                DataRow[] drrEmailStatus = dtEmailStatus.Select("PicklistField = '" + drrBounceImport[0]["Bounce"].ToString().Replace("'", "''") + "'");
                                if (drrEmailStatus.Length > 0)
                                {
                                    if (drrEmailStatus[0]["PicklistValue"].ToString() == "HARD")
                                    {
                                        string sContID = dr["Contact_ID_P"].ToString();
                                        dtQCTable = Update_QCTable(sContID, dtContacts, dtQCTable, "Contact", "EMAIL BOUNCES",string.Empty);
                                        dr["EMAIL_VERIFIED"] = "BOUNCED";                                        
                                    }
                                    else
                                        dr["EMAIL_VERIFIED"] = drrEmailStatus[0]["PicklistValue"].ToString();

                                    dr["BOUNCE_LOADED_DATE"] = GM.GetDateTime();
                                    dr["BOUNCE_LOADED_BY"] = GV.sEmployeeName;
                                    dr["BOUNCE_STATUS"] = drrBounceImport[0]["Bounce"].ToString();                                    
                                }
                                else
                                {
                                    sMismatch += drrBounceImport[0]["Bounce"].ToString() + Environment.NewLine;
                                    iRecordCount--;
                                }
                            }
                        }
                    }
                    else if (sImportType == "OK") // OK Records
                    {
                        dtContacts = GV.MSSQL1.BAL_FetchTable(GV.sContactTable, "CONTACT_ID_P IN (" + sContactIDsImported + ") AND " + GV.sAccessTo + "_UNCERTAIN_STATUS = 0;");
                        sContactIDsDB = GM.ColumnToQString("CONTACT_ID_P", dtContacts, "Int");
                        if (sContactIDsDB.Length > 0)
                        {
                            dtQCTable = GV.MSSQL1.BAL_FetchTable(GV.sQCTable, "RecordID IN (" + sContactIDsDB + ") AND TableName = 'Contact' AND ResearchType = '" + GV.sAccessTo + "'");
                            iRecordCount = dtContacts.Rows.Count;
                            foreach (DataRow dr in dtContacts.Rows)
                                dtQCTable = Update_QCTable(dr["Contact_ID_P"].ToString(), dtContacts, dtQCTable, "Contact", "OK", string.Empty);
                        }
                    }
                    else if(sImportType == "Upload Tag")
                    {
                        dtContacts = GV.MSSQL1.BAL_FetchTable(GV.sContactTable, "CONTACT_ID_P IN (" + sContactIDsImported + ") AND TAG_DATE IS NULL;");
                        //sContactIDsDB = GM.ColumnToQString("CONTACT_ID_P", dtContacts, "Int");
                        iRecordCount = dtContacts.Rows.Count;
                        if (iRecordCount > 0)
                        {
                            foreach (DataRow drContacts in dtContacts.Rows)
                            {
                                drContacts["TAG_DATE"] = GM.GetDateTime();
                                drContacts["TAG_COMMENT"] = sComments;
                                drContacts["TAG_BY"] = GV.sEmployeeName;
                            }
                        }
                    }
                    else//Rejected Records
                    {
                        dtContacts = GV.MSSQL1.BAL_FetchTable(GV.sContactTable, "CONTACT_ID_P IN (" + sContactIDsImported + ") AND " + GV.sAccessTo + "_UNCERTAIN_STATUS = 0;");
                        sContactIDsDB = GM.ColumnToQString("CONTACT_ID_P", dtContacts, "Int");
                        if (sContactIDsDB.Length > 0)
                        {
                            dtQCTable = GV.MSSQL1.BAL_FetchTable(GV.sQCTable, "RecordID IN (" + sContactIDsDB + ") AND TableName = 'Contact' AND ResearchType = '" + GV.sAccessTo + "'");
                            iRecordCount = dtContacts.Rows.Count;
                            foreach (DataRow dr in dtContacts.Rows)
                                dtQCTable = Update_QCTable(dr["Contact_ID_P"].ToString(), dtContacts, dtQCTable, "Contact", "Rejection - " + dtImport.Select("CONTACT_ID_P = '" + dr["CONTACT_ID_P"] + "'")[0]["Rejection"].ToString(), string.Empty);
                        }
                    }
                }

                if (sMismatch.Trim().Length > 0)
                {
                    string sLogPath = Environment.GetEnvironmentVariable("TEMP") + "\\CM_Logs_" + GM.GetDateTime().ToString("ddMMyyyyHHmmss") + ".txt";
                    StreamWriter sWrite = new StreamWriter(sLogPath, true);
                    sMismatch = "Campaign Manager" + Environment.NewLine + GM.GetDateTime().ToString("dd-MM-yyyy HH:mm:ss") + Environment.NewLine + Environment.NewLine + "Below are the mismatched bounce status." + Environment.NewLine + sMismatch;
                    sWrite.WriteLine(sMismatch);
                    sWrite.Close();
                    if (MessageBoxEx.Show(@"Unknown bounce status found. Some record(s) are not updated.<br/>Find Log under : " + sLogPath + ".<br/> Only " + iRecordCount + " record(s) will be updated.<br/> Do you still wish to continue ?", "Campaign Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (iRecordCount > 0)
                        {
                            if (GV.MSSQL1.BAL_SaveToTable(dtContacts, GV.sContactTable, "Update", true))
                            {
                                SaveToDB(dtQCTable, GV.sQCTable);
                                MessageBoxEx.Show(GM.ProperCase_ProjectSpecific(sImportType) + " record(s) updated", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dtContacts.Dispose();
                                dtImport.Dispose();
                            }
                            else
                                MessageBoxEx.Show("Error occured on connecting the server. Please try again.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                            MessageBoxEx.Show("No records to update.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    if (iRecordCount > 0)
                    {
                        if (MessageBoxEx.Show("Are you sure to update " + iRecordCount + " record(s)", "Campaign Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {                        
                            if (GV.MSSQL1.BAL_SaveToTable(dtContacts, GV.sContactTable, "Update", true))
                            {
                                if(dtQCTable != null)
                                    SaveToDB(dtQCTable, GV.sQCTable);

                                MessageBoxEx.Show(GM.ProperCase_ProjectSpecific(sImportType) + " record(s) updated", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dtContacts.Dispose();
                                dtImport.Dispose();
                            }
                            else
                                MessageBoxEx.Show("Error occured on connecting the server. Please try again.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);                        
                        }
                    }
                    else
                        MessageBoxEx.Show("No records to update.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }                
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
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
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                return false;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void frmCompanyList_Shown(object sender, EventArgs e)
        {
            ColorizeDGV();
            this.WindowState = FormWindowState.Maximized;
            //recover();

        }

        DataTable ContactCount()
        {
            try
            {
                DataTable dtContactCount;
                string sSQLTEXT = string.Empty;
                int iContactCompleteCount = 0;
                int iContactProcessed = 0;

                if (GV.sUserType == "Agent")
                    sSQLTEXT = "SELECT " + GV.sAccessTo + "_CONTACT_STATUS ContactCount, COUNT(*) Count FROM " + GV.sContactTable + " WHERE CAST(" + GV.sAccessTo + "_UPDATED_DATE AS DATE) = CAST(GETDATE() as date) AND " + GV.sAccessTo + "_AGENT_NAME = '" + GV.sEmployeeName + "' GROUP BY " + GV.sAccessTo + "_CONTACT_STATUS ORDER BY CASE WHEN ( " + GV.sAccessTo + "_CONTACT_STATUS LIKE '% Complete') THEN 1 ELSE 2 END;";
                else
                    sSQLTEXT = "SELECT " + GV.sAccessTo + "_CONTACT_STATUS ContactCount, COUNT(*) Count FROM " + GV.sContactTable + " WHERE CAST(" + GV.sAccessTo + "_UPDATED_DATE AS DATE) = CAST(GETDATE() as date) GROUP BY " + GV.sAccessTo + "_CONTACT_STATUS ORDER BY CASE WHEN (" + GV.sAccessTo + "_CONTACT_STATUS LIKE '% Complete') THEN 1 ELSE 2 END;";
                dtContactCount = GV.MSSQL1.BAL_ExecuteQuery(sSQLTEXT);

                if (dtContactCount != null)
                {
                    if (dtContactCount.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtContactCount.Rows)
                        {
                            iContactProcessed += Convert.ToInt32(dr["COUNT"]);
                            if ((GV.lstTRContactStatusToBeValidated.Contains(dr["ContactCount"].ToString(), StringComparer.OrdinalIgnoreCase) && GV.sAccessTo == "TR") || (GV.lstWRContactStatusToBeValidated.Contains(dr["ContactCount"].ToString(), StringComparer.OrdinalIgnoreCase) && GV.sAccessTo == "WR"))
                                iContactCompleteCount += Convert.ToInt32(dr["COUNT"]);
                        }
                    }

                    DataRow drNewContactCompleted = dtContactCount.NewRow();
                    drNewContactCompleted["ContactCount"] = "Total Contact Complete";
                    drNewContactCompleted["COUNT"] = iContactCompleteCount;
                    dtContactCount.Rows.Add(drNewContactCompleted);

                    DataRow drNewContactProcessed = dtContactCount.NewRow();
                    drNewContactProcessed["ContactCount"] = "Total Contact Processed";
                    drNewContactProcessed["COUNT"] = iContactProcessed;
                    dtContactCount.Rows.Add(drNewContactProcessed);
                }
                return dtContactCount;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return null;
        }

        DataTable CompanyCount()
        {
            try
            {
                DataTable dtCompanyCount;
                string sSQLTEXT = string.Empty;
                int iCompanyCompleteCount = 0;
                int iCompanyProcessed = 0;
                string sDateColumn = string.Empty;

                if (GV.sAccessTo == "TR")
                    sDateColumn = "TR_DATECALLED";
                else
                    sDateColumn = "WR_DATE_OF_PROCESS";

                if (GV.sUserType == "Agent")
                    sSQLTEXT = "SELECT " + GV.sAccessTo + "_PRIMARY_DISPOSAL CompanyCount, COUNT(*) Count FROM " + GV.sCompanyTable + " WHERE CAST(" + sDateColumn + " AS DATE) = CAST(GETDATE() as date) AND " + GV.sAccessTo + "_AGENTNAME = '" + GV.sEmployeeName + "' GROUP BY " + GV.sAccessTo + "_PRIMARY_DISPOSAL;";
                else
                    sSQLTEXT = "SELECT " + GV.sAccessTo + "_PRIMARY_DISPOSAL CompanyCount, COUNT(*) Count FROM " + GV.sCompanyTable + " WHERE CAST(" + sDateColumn + " AS DATE) = CAST(GETDATE() as date) GROUP BY " + GV.sAccessTo + "_PRIMARY_DISPOSAL;";
                dtCompanyCount = GV.MSSQL1.BAL_ExecuteQuery(sSQLTEXT);

                if (dtCompanyCount != null)
                {
                    if (dtCompanyCount.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCompanyCount.Rows)
                        {
                            iCompanyProcessed += Convert.ToInt32(dr["COUNT"]);
                            if (GV.TR_lstDisposalsToBeValidated.Contains(dr["CompanyCount"].ToString(), StringComparer.OrdinalIgnoreCase))
                                iCompanyCompleteCount += Convert.ToInt32(dr["COUNT"]);
                        }
                    }

                    DataRow drNewCompanyCompleted = dtCompanyCount.NewRow();
                    drNewCompanyCompleted["CompanyCount"] = "Total Company Complete";
                    drNewCompanyCompleted["COUNT"] = iCompanyCompleteCount;
                    dtCompanyCount.Rows.Add(drNewCompanyCompleted);

                    DataRow drNewCompanyProcessed = dtCompanyCount.NewRow();
                    drNewCompanyProcessed["CompanyCount"] = "Total Company Processed";
                    drNewCompanyProcessed["COUNT"] = iCompanyProcessed;
                    dtCompanyCount.Rows.Add(drNewCompanyProcessed);
                }
                return dtCompanyCount;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);

                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return null;
        }

        //-----------------------------------------------------------------------------------------------------
        private void expandablePanelCount_ExpandedChanging(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                DataTable dtContactCount = ContactCount();
                DataTable dtCompanyCount = CompanyCount();

                if (dtCompanyCount != null && dtContactCount != null)
                {
                    dgvContactCount.DataSource = dtContactCount;
                    dgvCompanyCount.DataSource = dtCompanyCount;

                    dgvContactCount.Columns["ContactCount"].FillWeight = 70;
                    dgvContactCount.Columns["Count"].FillWeight = 30;

                    dgvCompanyCount.Columns["CompanyCount"].FillWeight = 70;
                    dgvCompanyCount.Columns["Count"].FillWeight = 30;
                }
                dgvContactCount.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
                dgvCompanyCount.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;

                

            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void expandablePanelCount_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (!expandablePanelCount.Expanded)
            {
                expandablePanelCount.Visible = false;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void dgvCompanyList_BackgroundColorChanged(object sender, EventArgs e)
        {
            if (dgvCompanyList.BackgroundColor != GV.pnlGlobalColor.Style.BackColor2.Color)
            {
                if (GV.pnlGlobalColor.Style.BackColor2.Color.Name != "0")
                    dgvCompanyList.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void dgvCompanySearch_BackgroundColorChanged(object sender, EventArgs e)
        {
            
            if (dgvCompanySearch.BackgroundColor != GV.pnlGlobalColor.Style.BackColor2.Color)
            {
                if (GV.pnlGlobalColor.Style.BackColor2.Color.Name != "0")
                    dgvCompanySearch.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
            }
        }

        private void dgvContactCount_BackgroundColorChanged(object sender, EventArgs e)
        {
            if (dgvContactCount.BackgroundColor != GV.pnlGlobalColor.Style.BackColor2.Color)
            {
                if (GV.pnlGlobalColor.Style.BackColor2.Color.Name != "0")
                    dgvContactCount.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
            }
        }

        private void dgvCompanyCount_BackgroundColorChanged(object sender, EventArgs e)
        {
            if (dgvCompanyCount.BackgroundColor != GV.pnlGlobalColor.Style.BackColor2.Color)
            {
                if (GV.pnlGlobalColor.Style.BackColor2.Color.Name != "0")
                    dgvCompanyCount.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
            }
        }

        private void expandablePanelExport_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (expandablePanelExport.Expanded)
            {
                dateInputExport.Value = GM.GetDateTime().AddDays(-1);                
                switchButtonExportResearch.Value = (GV.sAccessTo == "TR");
            }
            else
                expandablePanelExport.Visible = false;
        }

        private void expandablePanelExport_Click(object sender, EventArgs e)
        {

        }

        //private void btnResetColumns_Click(object sender, EventArgs e)
        //{ 
        //    ButtonItem btn = sender as ButtonItem;
        //    if(btn.Name == "btnResetColumns")            
        //        Get_ProjectColumns(true, true);            
        //    else if(btn.Name == "btnResetContactColumns")
        //        Get_ProjectColumns(false, true);
        //    else if(btn.Name == "btnResetCompanyColumns")
        //        Get_ProjectColumns(true, false);
            
        //}

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!bWorkerExport.IsBusy)
                ExportExcel();
            else
                ToastNotification.Show(this,"Export in progress",eToastPosition.TopRight);
            
        }

        void ExportDocFormat_OpenXML (string sQueryString,string sColumns, string sPath)
        {
            double iRowCounter = 0;
            try
            {
                using (SqlConnection conMSSQL1 = new SqlConnection(GV.sMSSQL1))
                {
                    string sQueryData = "SELECT " + sColumns + sQueryString;
                    string sQueryCount = "SELECT COUNT(*) " + sQueryString;
                    lblExportRows.Invoke((MethodInvoker) delegate { lblExportRows.Text = "Fetching data..."; });
                    DataTable dtRowCount = GV.MSSQL1.BAL_ExecuteQuery(sQueryCount);
                    double iTotalRowCount = Convert.ToDouble(dtRowCount.Rows[0][0]);
                    conMSSQL1.Open();
                    //SqlCommand cmdExport = new SqlCommand("SET GLOBAL   net_read_timeout = 100;" + sQueryData, conMSSQL1);
                    SqlCommand cmdExport = new SqlCommand(sQueryData, conMSSQL1);
                    lblExportRows.Invoke(
                        (MethodInvoker) delegate { lblExportRows.Text = "Estimated Row count :" + iTotalRowCount; });
                    //if (GV.conMYdSQL.State != ConnectionState.Open)
                    //    GV.conMYSdQL.Open();
                    SqlDataReader rdrExport = cmdExport.ExecuteReader();

                    int iRowsPerSheetCounter = 0;
                    
                    DataTable dtSchema = rdrExport.GetSchemaTable();
                    DataTable dtExportData = new DataTable();

                    
                    int iRowPerSheet = 0;
                    if (dtSchema.Rows.Count <= 80)
                        iRowPerSheet = 100000;
                    else if (dtSchema.Rows.Count > 80 && dtSchema.Rows.Count < 150)
                        iRowPerSheet = 50000;
                    else if (dtSchema.Rows.Count >= 150)
                        iRowPerSheet = 25000;


                    var listCols = new List<DataColumn>();

                    if (iTotalRowCount > 0)
                    {
                        if (File.Exists(sPath))
                            File.Delete(sPath);

                        string sSheetName = string.Empty;
                        if ((GV.sProjectName).Length > 20)
                            sSheetName = (GV.sProjectName).Substring(0, 20);
                        else
                            sSheetName = GV.sProjectName;
                        uint sheetId = 1; //Start at the first sheet in the Excel workbook.
                        SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(sPath, SpreadsheetDocumentType.Workbook);
                        // Add a WorkbookPart to the document.
                        WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                        workbookpart.Workbook = new Workbook();

                        // Add a WorksheetPart to the WorkbookPart.
                        var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                        SheetData sheetData = new SheetData();
                        worksheetPart.Worksheet = new Worksheet(sheetData);

                        // Add Sheets to the Workbook.
                        Sheets sheets;
                        sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                        // Append a new worksheet and associate it with the workbook.
                        var sheet = new Sheet()
                        {
                            Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                            SheetId = sheetId,
                            Name = sSheetName + "_" + sheetId
                        };
                        sheets.Append(sheet);

                        //Stopwatch sWatch = new Stopwatch();
                        //sWatch.Start();
                        Row rowColumn = new Row();
                        foreach (DataRow drow in dtSchema.Rows)
                        {
                            string columnName = Convert.ToString(drow["ColumnName"]);
                            var column = new DataColumn(columnName, (Type) (drow["DataType"]));
                            column.Unique = (bool) drow["IsUnique"];
                            column.AllowDBNull = (bool) drow["AllowDBNull"];
                            column.AutoIncrement = (bool) drow["IsAutoIncrement"];
                            listCols.Add(column);
                            dtExportData.Columns.Add(column);
                            Cell cellColumn = new Cell
                            {
                                DataType = CellValues.String,
                                CellValue = new CellValue(GM.RemoveNonXMLChars(columnName))
                            };
                            rowColumn.AppendChild(cellColumn);                            
                        }
                        sheetData.AppendChild(rowColumn);

                        while (rdrExport.Read())
                        {
                            Row newRow = new Row();
                            for (int i = 0; i < listCols.Count; i++)
                            {
                                Cell cell = new Cell
                                {
                                    DataType = CellValues.String,
                                    CellValue = new CellValue(GM.RemoveNonXMLChars(rdrExport[i].ToString()))
                                };
                                newRow.AppendChild(cell);
                                cell = null;
                            }
                            sheetData.AppendChild(newRow);                            
                            iRowCounter++;
                            iRowsPerSheetCounter++;

                            //if (iRowCounter == 4194)
                            //{ }

                            lblExportRows.Invoke(
                                (MethodInvoker)
                                    delegate { lblExportRows.Text = "Writing " + iRowCounter + " / " + iTotalRowCount; });
                            circularProgressExport.Value = Convert.ToInt32((iRowCounter/iTotalRowCount)*100);

                            if (iRowsPerSheetCounter == iRowPerSheet)
                            {
                                lblExportRows.Invoke((MethodInvoker) delegate { lblExportRows.Text = "Splitting Worksheet..."; });
                                iRowsPerSheetCounter = 0;
                                workbookpart.Workbook.Save();
                                spreadsheetDocument.Close();                                                                
                                spreadsheetDocument.Dispose();
                                GC.Collect();

                                spreadsheetDocument = SpreadsheetDocument.Open(sPath, true);
                                workbookpart = spreadsheetDocument.WorkbookPart;

                                if (workbookpart.Workbook == null)
                                    workbookpart.Workbook = new Workbook();

                                lblExportRows.Invoke((MethodInvoker) delegate { lblExportRows.Text = "Opening Worksheet..."; });
                                worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                                sheetData = new SheetData();
                                worksheetPart.Worksheet = new Worksheet(sheetData);
                                sheets = spreadsheetDocument.WorkbookPart.Workbook.Sheets;

                                if (sheets.Elements<Sheet>().Any())
                                    sheetId = sheets.Elements<Sheet>().Max(s => s.SheetId.Value) + 1;
                                else
                                    sheetId = 1;

                                var sheeet = new Sheet()
                                {
                                    Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                                    SheetId = sheetId,
                                    Name = sSheetName + "_" + sheetId
                                };
                                sheets.Append(sheeet);

                                Row rowColumnNewSheet = new Row();
                                foreach (DataRow drow in dtSchema.Rows)
                                {
                                    string columnName = Convert.ToString(drow["ColumnName"]);
                                    Cell cellColumn = new Cell
                                    {
                                        DataType = CellValues.String,
                                        CellValue = new CellValue(GM.RemoveNonXMLChars(columnName))
                                    };
                                    rowColumnNewSheet.AppendChild(cellColumn);
                                    cellColumn = null;
                                }
                                sheetData.AppendChild(rowColumnNewSheet);
                                //rowColumnNewSheet = null;
                                //GC.Collect();
                                iExcelExportStatus = 4;
                            }
                        }

                        lblExportRows.Invoke((MethodInvoker) delegate { lblExportRows.Text = "Saving Worksheet..."; });
                        workbookpart.Workbook.Save();                        
                        spreadsheetDocument.Close();                        
                        spreadsheetDocument.Dispose();                        
                        //iExcelExportStatus = 3;
                        // Call Close when done reading.

                        lblExportRows.Invoke((MethodInvoker) delegate { lblExportRows.Text = "Closing Worksheet..."; });
                        rdrExport.Close();
                        rdrExport.Dispose();
                        //GV.conMYdSQL.Close();
                        GC.Collect();
                        lblExportRows.Invoke(
                            (MethodInvoker) delegate { lblExportRows.Text = "Worksheet saved sucessfully..."; });
                        //sWatch.Stop();
                        //MessageBoxEx.Show("Exported sucessfully", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        iExcelExportStatus = 2;
                    //else            
                    //    ToastNotification.Show(this, "Filter doesn't return any row(s) or invalid filter conditions.", eToastPosition.TopRight);            
                }
            }
            catch (Exception ex)
            {
                iExcelExportStatus = 1;
                if(GV.sUserType == "Admin")                                    
                    MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void bWorkerExport_DoWork(object sender, DoWorkEventArgs e)
        {            
            ExportDocFormat_OpenXML(sExcelExportQuery, sExcelExportColumns, sExcelExportPath);
        }

        private void bWorkerExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(iExcelExportStatus == 0)
                MessageBoxEx.Show("Exported sucessfully.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (iExcelExportStatus == 2)
                MessageBoxEx.Show("Filter doesn't return any records.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (iExcelExportStatus == 1)
                MessageBoxEx.Show("Error occured on export.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (iExcelExportStatus == 4)
                MessageBoxEx.Show("Exported sucessfully.<br/>Records splitted into multiple sheets.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnExport.Enabled = true;
            iExcelExportStatus = 0;
            circularProgressExport.ProgressTextVisible = false;
            circularProgressExport.Value = 0;
            switchExportType.Enabled = true;
            groupPanelQuickExport.Enabled = true;
            lblExportRows.Text = "";
        }

        
        private void chkMove_EmptyContacts_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMove_companies_with_zero_contact.Checked || chkMove_companies_with_unsucessfull_contacts.Checked)
            {
                //chkMove_QC_OK_records_only.Checked = chkMove_QC_OK_records_only.Enabled = chkInclude_QC_Send_Back_records.Checked = chkInclude_QC_Send_Back_records.Enabled = false;
                chkMove_all_processed_companies.Checked = chkMove_all_processed_companies.Enabled = false;
                ToastNotification.Show(this, "Contact based conditions will be ignored.", eToastPosition.TopRight);
            }
            else
            {
                chkMove_all_processed_companies.Enabled = true;
                //chkMove_QC_OK_records_only.Enabled = chkInclude_QC_Send_Back_records.Enabled = true;
            }
        }

        private void chkMove_all_processed_companies_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMove_all_processed_companies.Checked)
            {
                chkMove_companies_with_zero_contact.Checked = chkMove_companies_with_unsucessfull_contacts.Checked = chkNon_processed_records.Checked = chkMove_companies_with_zero_contact.Enabled = chkNon_processed_records.Enabled = chkMove_companies_with_unsucessfull_contacts.Enabled = false;
                ToastNotification.Show(this, "This will ignore detailed filter conditions.", eToastPosition.TopRight);
            }
            else
                chkMove_companies_with_zero_contact.Enabled = chkMove_companies_with_unsucessfull_contacts.Enabled = chkNon_processed_records.Enabled = true;
        }

        string Get_Processed_Query(string sPrefix, string sDateColumn, string sDateNull)
        {
            string sReturn = " AND " + sPrefix + ".FLAG = '" + GV.sAccessTo + "'";

            //if (chkNon_processed_records.Checked)
            //    sReturn += " AND " + sPrefix + "." + sDateColumn + " IS " + sDateNull + " NULL";
            //else
            //    sReturn += " AND " + sPrefix + "." + sDateColumn + " IS " + sDateNull + " NULL";

            if (chkNon_processed_records.Checked)
                sReturn += " AND " + sPrefix + "." + sDateColumn + " IS  NULL";
            else
                sReturn += " AND " + sPrefix + "." + sDateColumn + " IS NOT NULL";

            if (chkMove_only_valid_Switchboard.Checked)
                sReturn += " AND LEN(" + sPrefix + ".SWITCHBOARD_TRIMMED) >= 7";

            return sReturn;
        }

        private void btnMoverecords_Click(object sender, EventArgs e)
        {
            string sSQLText = string.Empty;
            string sPrefix = string.Empty;
            string sQCConditions = string.Empty;
            string sCompleteContactStatus = string.Empty;
            string sDateColumn = GV.sAccessTo == "WR" ? "WR_DATE_OF_PROCESS" : "TR_DATECALLED";
            DataTable dtConditionTable = null;
            string sMovedToSource = string.Empty;            
            try
            {

                if (txtNewMoveSource.Enabled)
                {
                    if (txtNewMoveSource.Text.Trim().Length > 0)
                        sMovedToSource = txtNewMoveSource.Text.Trim();
                    else
                    {
                        ToastNotification.Show(this, "Source not filled.", eToastPosition.TopRight);
                        return;
                    }
                }
                else if (lstMovedToSource.SelectedItems.Count > 0 && lstMovedToSource.SelectedItems[0].Text != "(New Source)")
                    sMovedToSource = lstMovedToSource.SelectedItems[0].Text;
                else
                {
                    ToastNotification.Show(this, "Source not filled.", eToastPosition.TopRight);
                    return;
                }


                //if (GV.sAccessTo == "WR")//For WR to TR only QC Passed records should be moved.
                //{
                //    sPrefix = "SELECT DBO.GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN  (SELECT * FROM " + GV.sContactTable + " C INNER JOIN " + GV.sProjectID + "_QC Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
                //    sQCConditions = " AND Contact.QC_STATUS = 'OK' AND Contact.ResearchType = 'WR' AND Contact.TableName = 'CONTACT'";
                //    sCompleteContactStatus = GV.sWRContactstatusTobeValidated;
                //}
                //else
                //{
                //    sPrefix = "SELECT DBO.GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
                //    sCompleteContactStatus = GV.sTRContactstatusTobeValidated;
                //}

                if (GV.sAccessTo == "WR")//For WR to TR only QC Passed records should be moved.                
                    sCompleteContactStatus = GV.sWRContactstatusTobeValidated;
                else                
                    sCompleteContactStatus = GV.sTRContactstatusTobeValidated;                
                

                if (chkMove_all_processed_companies.Checked)
                {
                    if (GV.sAccessTo == "TR")
                        sPrefix = "SELECT dbo.GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company ";
                    else
                    {
                        if (chkMove_AllowOnlyQCPassed.Checked)
                        {
                            sPrefix = "SELECT dbo.GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM (SELECT * FROM " +
                                      GV.sCompanyTable + " Com WHERE NOT EXISTS (SELECT 1 FROM " + GV.sContactTable +
                                      " C  INNER JOIN " + GV.sQCTable +
                                      " Q ON C.CONTACT_ID_P=Q.RecordID WHERE C.MASTER_ID = Com.master_id AND Q.QC_STATUS IN ('SendBack', 'Reprocessed'))) Company INNER JOIN  (SELECT * FROM " +
                                      GV.sContactTable + " C INNER JOIN " + GV.sQCTable +
                                      " Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
                        }
                        else
                        {
                            //sPrefix = "SELECT DBO.GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM (SELECT * FROM " + 
                            //          GV.sCompanyTable + " Com WHERE NOT EXISTS (SELECT 1 FROM " + GV.sContactTable +
                            //          " C  INNER JOIN " + GV.sQCTable + 
                            //          " Q ON C.CONTACT_ID_P=Q.RecordID WHERE C.MASTER_ID = Com.master_id AND Q.QC_STATUS IN ('SendBack', 'Reprocessed'))) Company INNER JOIN  (SELECT * FROM " +
                            //          GV.sContactTable + " C INNER JOIN " + GV.sQCTable + 
                            //          " Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";


                            sPrefix = "SELECT dbo.GROUP_CONCAT(DISTINCT COMPANY.MASTER_ID) FROM " + GV.sCompanyTable + " COMPANY LEFT JOIN " + GV.sCompanyTable + " COMPANY1 on COMPANY.MASTER_ID = COMPANY1.MASTER_ID ";
                        }
                    }

                    string sDefaultCondition = Get_Processed_Query("Company", sDateColumn, "NOT");
                    sSQLText = sDefaultCondition.Substring(4, sDefaultCondition.Length - 4);
                    
                    if (GV.sAccessTo == "WR" && chkMove_AllowOnlyQCPassed.Checked)//QC table conditions. QC table merged with Contact table
                        sSQLText += " AND Contact.ResearchType = 'WR' AND Contact.TableName = 'CONTACT'";                    

                    sSQLText = sPrefix + " WHERE " + sSQLText;
                }
                else
                {
                    dtConditionTable = FormatDatatableToBuildQuery(true);
                    DataTable dtList = null;

                    if (dtConditionTable != null && dtConditionTable.Rows.Count > 0)
                    {                       
                        dtList = dtConditionTable.DefaultView.ToTable(true, "TABLE");
                        if (chkMove_companies_with_zero_contact.Checked || chkMove_companies_with_unsucessfull_contacts.Checked || chkNon_processed_records.Checked)//Contact filter should be eliminated since we are picking companies with zero contacts
                        {
                            DataRow[] drrCondition = dtConditionTable.Select("TABLE NOT IN ('CONTACT','Contact Count')");
                            
                            if (drrCondition.Length > 0)
                                dtConditionTable = drrCondition.CopyToDataTable();
                            else
                                dtConditionTable = null;
                        }                        

                        if (dtConditionTable != null && dtConditionTable.Rows.Count > 0)
                        {
                            if (GV.sAccessTo == "WR")
                            {
                                DataRow[] drrCondition = dtConditionTable.Select("TABLE <> 'QC'");// Eliminate QC Conditions for WR
                                if (drrCondition.Length > 0)
                                    dtConditionTable = drrCondition.CopyToDataTable();
                                else
                                    dtConditionTable = null;
                            }

                            sSQLText = BuildQuery(dtConditionTable);
                            if (chkNon_processed_records.Checked || chkMove_companies_with_zero_contact.Checked || chkMove_companies_with_unsucessfull_contacts.Checked)
                            {
                                sPrefix = "SELECT dbo.GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company ";
                                sQCConditions = string.Empty;
                            }
                            else//No boxes checked
                            {                                
                                if (dtList.Select("TABLE IN ('COMPANY')").Length == 0 && chkMove_only_valid_Switchboard.Checked)
                                {
                                    DataRow drAddCompany = dtList.NewRow();
                                    drAddCompany[0] = "COMPANY";
                                    dtList.Rows.Add(drAddCompany);
                                }

                                if (GV.sAccessTo == "WR")
                                {
                                    if (chkMove_AllowOnlyQCPassed.Checked)
                                    {
                                        sPrefix =
                                            "SELECT dbo.GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM (SELECT * FROM " +
                                            GV.sCompanyTable + " Com WHERE NOT EXISTS (SELECT 1 FROM " +
                                            GV.sContactTable + " C  INNER JOIN " + GV.sQCTable +
                                            " Q ON C.CONTACT_ID_P=Q.RecordID WHERE C.MASTER_ID = Com.master_id AND Q.QC_STATUS IN ('SendBack', 'Reprocessed'))) Company INNER JOIN  (SELECT * FROM " +
                                            GV.sContactTable + " C INNER JOIN " + GV.sQCTable +
                                            " Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";

                                        sQCConditions = " AND Contact.ResearchType = 'WR' AND Contact.TableName = 'CONTACT'";
                                    }
                                    else
                                    {
                                        //sPrefix =
                                        // "SELECT GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM (SELECT * FROM " +
                                        // GV.sCompanyTable + " Com WHERE NOT EXISTS (SELECT 1 FROM " +
                                        // GV.sContactTable + " C  INNER JOIN " + GV.sQCTable +
                                        // " Q ON C.CONTACT_ID_P=Q.RecordID WHERE C.MASTER_ID = Com.master_id AND Q.QC_STATUS IN ('SendBack', 'Reprocessed'))) Company INNER JOIN  (SELECT * FROM " +
                                        // GV.sContactTable + " C INNER JOIN " + GV.sQCTable +
                                        // " Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";

                                        sPrefix = "SELECT dbo.GROUP_CONCAT(DISTINCT COMPANY.MASTER_ID) FROM " + GV.sCompanyTable + " COMPANY LEFT JOIN " + GV.sCompanyTable + " COMPANY1 on COMPANY.MASTER_ID = COMPANY1.MASTER_ID ";

                                        //sQCConditions = " AND Contact.ResearchType = 'WR' AND Contact.TableName = 'CONTACT'";
                                    }
                                }
                                else
                                {
                                    if (dtList.Select("TABLE IN ('COMPANY','CONTACT','QC')").Length == 3 || dtList.Select("TABLE IN ('COMPANY','QC')").Length == 2 || dtList.Select("TABLE IN ('CONTACT','QC')").Length == 2)
                                        sPrefix = "SELECT dbo.GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company INNER JOIN  (SELECT * FROM " + GV.sContactTable + " C INNER JOIN " + GV.sProjectID + "_QC Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
                                    //else if (dtList.Select("TABLE IN ('CONTACT','QC')").Length == 2)
                                    //    sPrefix = "SELECT GROUP_CONCAT(DISTINCT Contact.MASTER_ID) FROM " + GV.sContactTable + " Contact INNER JOIN " + GV.sProjectID + "_QC Q ON Contact.CONTACT_ID_P = Q.RECORDID ";
                                    else if (dtList.Select("TABLE IN ('COMPANY','CONTACT')").Length == 2 || dtList.Select("TABLE IN ('CONTACT')").Length == 1)
                                        sPrefix = "SELECT dbo.GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company INNER JOIN  " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
                                    //else if (dtList.Select("TABLE IN ('CONTACT')").Length == 1)
                                    //    sPrefix = "SELECT GROUP_CONCAT(DISTINCT Contact.MASTER_ID) FROM " + GV.sContactTable + " Contact ";
                                    else if (dtList.Select("TABLE IN ('COMPANY')").Length == 1)
                                        sPrefix = "SELECT dbo.GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company ";
                                    else if (dtList.Select("TABLE IN ('QC')").Length == 1)
                                        sPrefix = "SELECT dbo.GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company INNER JOIN  (SELECT * FROM " + GV.sContactTable + " C INNER JOIN " + GV.sProjectID + "_QC Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
                                        //sPrefix = "Select * from (SELECT * FROM " + GV.sContactTable + " C INNER JOIN " + GV.sProjectID + "_QC Q ON C.CONTACT_ID_P = Q.RECORDID) Contact";
                                }
                            }

                            if (sSQLText.Length > 0)
                            {
                                if (chkMove_companies_with_zero_contact.Checked || chkNon_processed_records.Checked || chkMove_companies_with_unsucessfull_contacts.Checked)
                                {/*Do Nothing*/ }
                                else
                                    sSQLText += sQCConditions;
                            }
                        }
                    }

                    string sUnsuccess = string.Empty;
                    if (chkMove_companies_with_zero_contact.Checked && chkMove_companies_with_unsucessfull_contacts.Checked)
                    {
                        sUnsuccess = @"SELECT DISTINCT M1.MASTER_ID FROM " + GV.sCompanyTable + " M1 ";
                        sUnsuccess += " LEFT OUTER JOIN (SELECT DISTINCT MASTER_ID FROM " + GV.sContactTable + " ";
                        sUnsuccess += " WHERE " + GV.sAccessTo + "_CONTACT_STATUS IN (" + sCompleteContactStatus + ")) M2 ";
                        sUnsuccess += " ON M1.MASTER_ID=M2.MASTER_ID WHERE M2.MASTER_ID IS NULL " + Get_Processed_Query("M1", sDateColumn, "NOT");
                    }
                    else if (chkMove_companies_with_zero_contact.Checked)
                    {
                        sUnsuccess = @"SELECT DISTINCT M1.MASTER_ID FROM " + GV.sCompanyTable + " M1 ";
                        sUnsuccess += " LEFT OUTER JOIN " + GV.sContactTable + " M2 ON M1.MASTER_ID=M2.MASTER_ID ";
                        sUnsuccess += " WHERE M2.MASTER_ID IS NULL " + Get_Processed_Query("M1", sDateColumn, "NOT");
                    }
                    else if (chkMove_companies_with_unsucessfull_contacts.Checked)
                    {
                        sUnsuccess = @"SELECT DISTINCT M1.MASTER_ID FROM " + GV.sCompanyTable + " M1 ";
                        sUnsuccess += " INNER JOIN " + GV.sContactTable + " M2 ON M2.MASTER_ID=M1.MASTER_ID ";
                        sUnsuccess += " WHERE M1.MASTER_ID NOT IN (SELECT DISTINCT MASTER_ID ";
                        sUnsuccess += " FROM " + GV.sContactTable + " WHERE " + GV.sAccessTo + "_CONTACT_STATUS IN (" + sCompleteContactStatus + ")) " + Get_Processed_Query("M1", sDateColumn, "NOT");
                    }

                    if (sUnsuccess.Length > 0)
                    {
                        if (sSQLText.Length > 0)
                            sSQLText += " AND Company.MASTER_ID IN (" + sUnsuccess + ") ";
                        else
                            sSQLText += " Company.MASTER_ID IN (" + sUnsuccess + ") ";
                    }
                    else
                    {
                        if (sSQLText.Length > 0)
                            sSQLText += Get_Processed_Query("Company", sDateColumn, "NOT");
                    }

                    if (chkNon_processed_records.Checked && sSQLText.Length == 0)
                    {
                        string sDefaultCondition = Get_Processed_Query("Company", sDateColumn, string.Empty);
                        sDefaultCondition = sDefaultCondition.Substring(4, sDefaultCondition.Length - 4);                                                
                        sSQLText = "(" + sDefaultCondition + ")"; 
                    }

                    if (sSQLText.Length > 0)
                    {
                        if (sPrefix.Length == 0)
                            sPrefix = "SELECT dbo.GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company ";

                        sSQLText = sPrefix + " WHERE " + sSQLText;
                    }
                }
                
                txtQuery.Text = sSQLText;

                if (sSQLText.Length > 0)
                {
                    DataTable dtCompanyIDsToMove = GV.MSSQL1.BAL_ExecuteQuery(sSQLText);
                    string sMasterIDsToMove = dtCompanyIDsToMove.Rows[0][0].ToString();

                    if (sMasterIDsToMove.Length > 0)
                    {
                        int RecordsCouont = sMasterIDsToMove.Split(',').Length;
                        if (DialogResult.Yes == MessageBoxEx.Show("Are you sure to move " + RecordsCouont + " companies to " + GV.sOppositAccess + " ?", "Campaign Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {
                            GV.MSSQL1.BAL_ExecuteQuery("UPDATE " + GV.sCompanyTable + " set FLAG='" + GV.sOppositAccess + "', MOVED_TO_" + GV.sOppositAccess + "_DATE = GETDATE(), MOVED_TO_" + GV.sOppositAccess + "_BY = '" + GV.sEmployeeName + "', MOVED_TO_SOURCE='" + sMovedToSource.Replace("'", "''") + "'  WHERE MASTER_ID IN (" + sMasterIDsToMove + ");");
                            GV.MSSQL1.BAL_ExecuteQuery("INSERT INTO " + GV.sProjectID + "_log (RecordID, TableName, FieldName, OldValue, NewValue, [When], [Who], SystemName) VALUES (0,'RecordsMoved_To_" + GV.sOppositAccess + "','Source','" + sSQLText.Replace("'", "''") + "','" + sMasterIDsToMove + "', GETDATE(),'" + GV.sEmployeeName + "','" + Environment.MachineName + "');");
                            MessageBoxEx.Show(RecordsCouont + " records moved to "+GV.sOppositAccess, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ToastNotification.Show(this, "Filter doesn't return any row(s) or invalid filter conditions.", eToastPosition.TopRight);
                }
                else
                    ToastNotification.Show(this, "Filter doesn't return any row(s) or invalid filter conditions.", eToastPosition.TopRight);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }


        #region MovetoBackup
        //private void btnMoverecords_Click(object sender, EventArgs e)
        //{
        //    string sSQLText = string.Empty;
        //    string sPrefix = string.Empty;
        //    string sQCConditions = string.Empty;
        //    string sCompleteContactStatus = string.Empty;
        //    string sDateColumn = GV.sAccessTo == "WR" ? "WR_DATE_OF_PROCESS" : "TR_DATECALLED";
        //    DataTable dtConditionTable = null;
        //    string sMovedToSource = string.Empty;
        //    try
        //    {

        //        if (txtNewMoveSource.Enabled)
        //        {
        //            if (txtNewMoveSource.Text.Trim().Length > 0)
        //                sMovedToSource = txtNewMoveSource.Text.Trim();
        //            else
        //            {
        //                ToastNotification.Show(this, "Source not filled.", eToastPosition.TopRight);
        //                return;
        //            }
        //        }
        //        else if (lstMovedToSource.SelectedItems.Count > 0 && lstMovedToSource.SelectedItems[0].Text != "(New Source)")
        //            sMovedToSource = lstMovedToSource.SelectedItems[0].Text;
        //        else
        //        {
        //            ToastNotification.Show(this, "Source not filled.", eToastPosition.TopRight);
        //            return;
        //        }


        //        if (GV.sAccessTo == "WR")//For WR to TR only QC Passed records should be moved.
        //        {
        //            sPrefix = "SELECT GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN  (SELECT * FROM " + GV.sContactTable + " C INNER JOIN " + GV.sProjectID + "_QC Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
        //            sQCConditions = " AND Contact.QC_STATUS = 'OK' AND Contact.ResearchType = 'WR' AND Contact.TableName = 'CONTACT'";
        //            sCompleteContactStatus = GV.sWRContactstatusTobeValidated;
        //        }
        //        else
        //        {
        //            sPrefix = "SELECT GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
        //            sCompleteContactStatus = GV.sTRContactstatusTobeValidated;
        //        }









        //        if (chkMove_all_processed_companies.Checked)
        //        {
        //            if (GV.sAccessTo == "TR")
        //                sPrefix = "SELECT GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company ";
        //            string sDefaultCondition = Get_Processed_Query("Company", sDateColumn, "NOT");
        //            sSQLText = sDefaultCondition.Substring(4, sDefaultCondition.Length - 4);
        //            sSQLText += sQCConditions;

        //            sSQLText = sPrefix + " WHERE " + sSQLText;
        //        }
        //        else
        //        {
        //            dtConditionTable = FormatDatatableToBuildQuery(true);

        //            if (dtConditionTable != null && dtConditionTable.Rows.Count > 0)
        //            {
        //                if (chkMove_companies_with_zero_contact.Checked || chkMove_companies_with_unsucessfull_contacts.Checked || chkNon_processed_records.Checked)//Contact filter should be eliminated since we are picking companies with zero contacts
        //                {
        //                    DataRow[] drrCondition = dtConditionTable.Select("TABLE NOT IN ('CONTACT','Contact Count')");

        //                    if (drrCondition.Length > 0)
        //                        dtConditionTable = drrCondition.CopyToDataTable();
        //                    else
        //                        dtConditionTable = null;
        //                }

        //                if (dtConditionTable != null && dtConditionTable.Rows.Count > 0)
        //                {
        //                    if (GV.sAccessTo == "WR")
        //                    {
        //                        DataRow[] drrCondition = dtConditionTable.Select("TABLE <> 'QC'");// Eliminate QC Conditions for WR
        //                        if (drrCondition.Length > 0)
        //                            sSQLText = BuildQuery(drrCondition.CopyToDataTable());

        //                        sPrefix = "SELECT GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN  (SELECT * FROM " + GV.sContactTable + " C INNER JOIN " + GV.sProjectID + "_QC Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
        //                        sQCConditions = " AND Contact.QC_STATUS = 'OK' AND Contact.ResearchType = 'WR' AND Contact.TableName = 'CONTACT'";
        //                    }
        //                    else if (GV.sAccessTo == "TR")
        //                    {
        //                        if (dtConditionTable.Select("TABLE = 'QC'").Length > 0)
        //                        {
        //                            sPrefix = "SELECT GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN  (SELECT * FROM " + GV.sContactTable + " C INNER JOIN " + GV.sProjectID + "_QC Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
        //                            sQCConditions = " AND Contact.ResearchType = 'TR' AND Contact.TableName = 'CONTACT'";
        //                        }
        //                        else
        //                        {
        //                            sPrefix = "SELECT GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company LEFT OUTER JOIN " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
        //                            sQCConditions = string.Empty;
        //                        }
        //                        sSQLText = BuildQuery(dtConditionTable);
        //                    }


        //                    if (chkNon_processed_records.Checked || chkMove_companies_with_zero_contact.Checked || chkMove_companies_with_unsucessfull_contacts.Checked)
        //                    {
        //                        sPrefix = "SELECT GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company ";
        //                        sQCConditions = string.Empty;
        //                    }
        //                    else//No boxes checked
        //                    {
        //                        DataTable dtList = dtConditionTable.DefaultView.ToTable(true, "TABLE");

        //                        if (GV.sAccessTo == "WR")
        //                        {
        //                            sPrefix = "SELECT GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company INNER JOIN  (SELECT * FROM " + GV.sContactTable + " C INNER JOIN " + GV.sProjectID + "_QC Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
        //                            sQCConditions = " AND Contact.QC_STATUS = 'OK' AND Contact.ResearchType = 'WR' AND Contact.TableName = 'CONTACT'";
        //                        }
        //                        else
        //                        {
        //                            if (dtList.Select("TABLE IN ('COMPANY','CONTACT','QC')").Length == 3 || dtList.Select("TABLE IN ('COMPANY','QC')").Length == 2)
        //                                sPrefix = "SELECT GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company INNER JOIN  (SELECT * FROM " + GV.sContactTable + " C INNER JOIN " + GV.sProjectID + "_QC Q ON C.CONTACT_ID_P = Q.RECORDID) Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
        //                            else if (dtList.Select("TABLE IN ('CONTACT','QC')").Length == 2)
        //                                sPrefix = "SELECT GROUP_CONCAT(DISTINCT Contact.MASTER_ID) FROM " + GV.sContactTable + " Contact INNER JOIN " + GV.sProjectID + "_QC Q ON Contact.CONTACT_ID_P = Q.RECORDID ";
        //                            else if (dtList.Select("TABLE IN ('COMPANY','CONTACT')").Length == 2)
        //                                sPrefix = "SELECT GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company INNER JOIN  " + GV.sContactTable + " Contact ON Company.MASTER_ID = Contact.MASTER_ID ";
        //                            else if (dtList.Select("TABLE IN ('CONTACT')").Length == 1)
        //                                sPrefix = "SELECT GROUP_CONCAT(DISTINCT Contact.MASTER_ID) FROM " + GV.sContactTable + " Contact ";
        //                            else if (dtList.Select("TABLE IN ('COMPANY')").Length == 1)
        //                                sPrefix = "SELECT GROUP_CONCAT(DISTINCT Company.MASTER_ID) FROM " + GV.sCompanyTable + " Company ";
        //                            else if (dtList.Select("TABLE IN ('QC')").Length == 1)
        //                                sPrefix = "Select * from (SELECT * FROM " + GV.sContactTable + " C INNER JOIN " + GV.sProjectID + "_QC Q ON C.CONTACT_ID_P = Q.RECORDID) Contact";
        //                        }
        //                    }







        //                    if (sSQLText.Length > 0)
        //                    {
        //                        if (chkMove_companies_with_zero_contact.Checked || chkNon_processed_records.Checked || chkMove_companies_with_unsucessfull_contacts.Checked)
        //                        { /*Do Nothing*/ }
        //                        else
        //                            sSQLText += sQCConditions;
        //                    }
        //                }
        //            }

        //            string sUnsuccess = string.Empty;
        //            if (chkMove_companies_with_zero_contact.Checked && chkMove_companies_with_unsucessfull_contacts.Checked)
        //            {
        //                sUnsuccess = @"SELECT DISTINCT M1.MASTER_ID FROM " + GV.sCompanyTable + " M1 ";
        //                sUnsuccess += " LEFT OUTER JOIN (SELECT DISTINCT MASTER_ID FROM " + GV.sContactTable + " ";
        //                sUnsuccess += " WHERE " + GV.sAccessTo + "_CONTACT_STATUS IN (" + sCompleteContactStatus + ")) M2 ";
        //                sUnsuccess += " ON M1.MASTER_ID=M2.MASTER_ID WHERE M2.MASTER_ID IS NULL " + Get_Processed_Query("M1", sDateColumn, "NOT");
        //            }
        //            else if (chkMove_companies_with_zero_contact.Checked)
        //            {
        //                sUnsuccess = @"SELECT DISTINCT M1.MASTER_ID FROM " + GV.sCompanyTable + " M1 ";
        //                sUnsuccess += " LEFT OUTER JOIN " + GV.sContactTable + " M2 ON M1.MASTER_ID=M2.MASTER_ID ";
        //                sUnsuccess += " WHERE M2.MASTER_ID IS NULL " + Get_Processed_Query("M1", sDateColumn, "NOT");
        //            }
        //            else if (chkMove_companies_with_unsucessfull_contacts.Checked)
        //            {
        //                sUnsuccess = @"SELECT DISTINCT M1.MASTER_ID FROM " + GV.sCompanyTable + " M1 ";
        //                sUnsuccess += " INNER JOIN " + GV.sContactTable + " M2 ON M2.MASTER_ID=M1.MASTER_ID ";
        //                sUnsuccess += " WHERE M1.MASTER_ID NOT IN (SELECT DISTINCT MASTER_ID ";
        //                sUnsuccess += " FROM " + GV.sContactTable + " WHERE " + GV.sAccessTo + "_CONTACT_STATUS IN (" + sCompleteContactStatus + ")) " + Get_Processed_Query("M1", sDateColumn, "NOT");
        //            }

        //            if (sUnsuccess.Length > 0)
        //            {
        //                if (sSQLText.Length > 0)
        //                    sSQLText += " AND Company.MASTER_ID IN (" + sUnsuccess + ") ";
        //                else
        //                    sSQLText += " Company.MASTER_ID IN (" + sUnsuccess + ") ";
        //            }
        //            else
        //            {
        //                if (sSQLText.Length > 0)
        //                    sSQLText += Get_Processed_Query("Company", sDateColumn, "NOT");
        //            }

        //            if (chkNon_processed_records.Checked && sSQLText.Length == 0)
        //            {
        //                string sDefaultCondition = Get_Processed_Query("Company", sDateColumn, string.Empty);
        //                sDefaultCondition = sDefaultCondition.Substring(4, sDefaultCondition.Length - 4);
        //                sSQLText = "(" + sDefaultCondition + ")";
        //            }

        //            if (sSQLText.Length > 0)
        //            {

        //                sSQLText = sPrefix + " WHERE " + sSQLText;
        //            }
        //        }

        //        txtQuery.Text = sSQLText;

        //        if (sSQLText.Length > 0)
        //        {
        //            DataTable dtCompanyIDsToMove = GV.MfYSQL.BAL_ExecuteQueryMySdQL(sSQLText);
        //            string sMasterIDsToMove = dtCompanyIDsToMove.Rows[0][0].ToString();

        //            if (sMasterIDsToMove.Length > 0)
        //            {
        //                int RecordsCouont = sMasterIDsToMove.Split(',').Length;
        //                if (DialogResult.Yes == MessageBoxEx.Show("Are you sure to move " + RecordsCouont + " companies to " + GV.sOppositAccess + " ?", "Campaign Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
        //                {
        //                    GV.MYSfdQL.BAL_ExecuteQueryMySdQL("UPDATE " + GV.sCompanyTable + " set FLAG='" + GV.sOppositAccess + "', MOVED_TO_WR_DATE = NOW(), MOVED_TO_TR_BY = '" + GV.sEmployeeName + "', MOVED_TO_SOURCE='" + sMovedToSource.Replace("'", "''") + "'  WHERE MASTER_ID IN (" + sMasterIDsToMove + ");");
        //                    GV.MYSfQL.BAL_ExecuteQueryMydSQL("INSERT INTO " + GV.sProjectID + "_log (RecordID, TableName, FieldName, OldValue, NewValue, `When`, Who, SystemName) VALUES (0,'RecordsMoved_To_" + GV.sOppositAccess + "','Source','" + sSQLText.Replace("'", "''") + "','" + sMasterIDsToMove + "', NOW(),'" + GV.sEmployeeName + "','" + Environment.MachineName + "');");
        //                    MessageBoxEx.Show(RecordsCouont + " records moved to " + GV.sOppositAccess, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //                }
        //            }
        //            else
        //                ToastNotification.Show(this, "Filter doesn't return any row(s) or invalid filter conditions.", eToastPosition.TopRight);
        //        }
        //        else
        //            ToastNotification.Show(this, "Filter doesn't return any row(s) or invalid filter conditions.", eToastPosition.TopRight);
        //    }
        //    catch (Exception ex)
        //    {
        //        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
        //    }
        //} 
        #endregion

        private void expandablePanelRecordMovement_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (expandablePanelRecordMovement.Expanded)
            {
                lblMovementInfo.Visible = false;
                chkMove_only_valid_Switchboard.Checked =  (GV.sAccessTo == "WR");
                chkMove_AllowOnlyQCPassed.Visible = chkMove_AllowOnlyQCPassed.Checked = (GV.sAccessTo == "WR");
                txtNewMoveSource.Text = string.Empty;                
                if (splitCompanyList.Panel1Collapsed)
                {
                    splitCompanyList.Panel1Collapsed = false;
                    splitCompanyList.SplitterDistance = 202;
                    dgvCompanySearch.Focus();
                    //btnShowHideFilter.Checked = true;
                }                
                txtQuery.Visible = (GV.sUserType == "Admin");
            }
            else
                expandablePanelRecordMovement.Visible = false;
        }

        private void lstMovedToSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMovedToSource.SelectedItems.Count > 0 && lstMovedToSource.SelectedItems[0].Text == "(New Source)")
            {
                txtNewMoveSource.Enabled = true;
                txtNewMoveSource.Focus();
            }
            else
            {
                txtNewMoveSource.Text = string.Empty;
                txtNewMoveSource.Enabled = false;
            }
        }

        private void chkInclude_non_processed_records_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNon_processed_records.Checked)
            {
                ToastNotification.Show(this, "Contact based conditions will be ignored.", eToastPosition.TopRight);
                foreach(DataGridViewRow dgvr in dgvCompanySearch.Rows)
                {
                    if ((GV.sAccessTo == "TR" && dgvr.Cells["Search On"].Value.ToString().ToUpper() == "TR_DATECALLED") || (GV.sAccessTo == "WR" && dgvr.Cells["Search On"].Value.ToString().ToUpper() == "WR_DATE_OF_PROCESS"))
                    {
                        dgvr.Cells["SearchFrom"].Value = string.Empty;
                        dgvr.Cells["SearchTo"].Value = string.Empty;
                    }                    
                }
                
                chkMove_all_processed_companies.Enabled = chkMove_all_processed_companies.Checked = false;
            }
            else
                chkMove_all_processed_companies.Enabled = true;
        }

        private void frmCompanyList_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == "FrmContactsUpdate")
                {                    
                    f.Focus();
                    ToastNotification.Show(f, "Close Contact Update screen.", eToastPosition.TopLeft);
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void bWorkerLoadTables_DoWork(object sender, DoWorkEventArgs e)
        {
            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Value = 5; });
            ((frmMain)ParantForm).Invoke((MethodInvoker)delegate { ((frmMain)ParantForm).progressBar.Text = "Loading Tables"; });
            Load_Tables();
            InitilizeGSpeech();
            Get_projectColumnsNSource();
        }

        void InitilizeGSpeech()
        {
            //return;// Until Google cloud is renewd

            //if(GV.IsWindowsXP)
            //{
            //    GV.GSpeech = true;
            //    return;
            //}

            if (!GV.GSpeech)
            {
                if (dtPicklist.Select("PicklistCategory='GoogleSpeechAuth'").Length > 0)
                {
                    try
                    {
                        string sJFileName = dtPicklist.Select("PicklistCategory='GoogleSpeechAuth'")[0]["PicklistField"].ToString();
                        string sJFileContent = dtPicklist.Select("PicklistCategory='GoogleSpeechAuth'")[0]["PicklistValue"].ToString();
                        string sEnvVariableName = dtPicklist.Select("PicklistCategory='GoogleSpeechAuth'")[0]["remarks"].ToString();
                        string sJpath = AppDomain.CurrentDomain.BaseDirectory + sJFileName;
                        StreamWriter sWrite = new StreamWriter(sJpath, false);
                        sWrite.WriteLine(sJFileContent);
                        sWrite.Close();
                        if (File.Exists(sJpath))
                        {
                            //string EnvObj = Environment.GetEnvironmentVariable(sEnvVariableName, EnvironmentVariableTarget.);
                            //if (EnvObj != null && EnvObj.ToLower() != sJpath.ToLower())
                            Environment.SetEnvironmentVariable(sEnvVariableName, sJpath, EnvironmentVariableTarget.Process);

                            if (GV.sAccessTo == "TR" && GV.AudioComments)
                            {
                                GV.GSpeech = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                        GV.GSpeech = false;
                    }
                }
            }
        }

        private void bWorkerLoadTables_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Clear_ResultSet();
            RebuildSearchGrid();
            Load_Movement_Source();
            Load_ProjectColumns_Export(true, true);
            ((frmMain)ParantForm).progressBar.Value = 100;
            
            if (dtScrapperSettings != null && dtScrapperSettings.Rows.Count > 0)
                ((frmMain)ParantForm).rbnBarScrapper.Visible = true;
            else
                ((frmMain)ParantForm).rbnBarScrapper.Visible = false;

            GM.Moniter();

            //ToastNotification.Show(this, "Completed");
            ((frmMain)ParantForm).ribbonPanelProcess.Enabled = true;
            ((frmMain)ParantForm).ribbonPanelLogin.Enabled = true;
            ((frmMain)ParantForm).ribbonPanelProcess.Refresh();
            ((frmMain) ParantForm).progressBar.Visible = false;
            ((frmMain)ParantForm).progressBar.Value = 0;


            if (!((frmMain)ParantForm).bWorkerEmailValidation.IsBusy)
                ((frmMain)ParantForm).bWorkerEmailValidation.RunWorkerAsync();

            if (!((frmMain)ParantForm).bWorkerRDP.IsBusy)
                ((frmMain)ParantForm).bWorkerRDP.RunWorkerAsync();
        }

        public void bg_EmailValidation()
        {
            GV.bbg_EmailValidation = (dtPicklist.Select("PicklistCategory = 'EmailValidation' AND PicklistField = 'Active' AND PicklistValue = 'Y'").Length > 0);            
            if (GV.bbg_EmailValidation)
            {
                GV.ibg_LoadCount = Convert.ToInt16(dtPicklist.Select("PicklistCategory = 'EmailValidation' AND PicklistField = 'LoadCount'")[0]["PicklistValue"]);
                GV.ibg_CheckTimeoutPerEmail = Convert.ToInt32(dtPicklist.Select("PicklistCategory = 'EmailValidation' AND PicklistField = 'Check_Timeout_Per_Email'")[0]["PicklistValue"]);
                GV.ibg_BatchExpiry = Convert.ToInt32(dtPicklist.Select("PicklistCategory = 'EmailValidation' AND PicklistField = 'Batch_Expiry_Sec'")[0]["PicklistValue"]);
                GV.sbg_API = dtPicklist.Select("PicklistCategory = 'EmailValidation' AND PicklistField = 'API'")[0]["PicklistValue"].ToString();
                GV.ibg_Interval = Convert.ToInt32(dtPicklist.Select("PicklistCategory = 'EmailValidation' AND PicklistField = 'Interval'")[0]["PicklistValue"]);
                ELV objELV = new ELV(dtbg_EmailValidation.Copy());
            }
        }

        public void RDP()
        {            
            GV.RDP = (dtPicklist.Select("PicklistCategory = 'RDP' AND PicklistField = 'Active' AND PicklistValue = 'Y'").Length > 0);
            if (GV.RDP)
            {                
                Server.Listen();
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------
    public static class DataTableExtensions
    {
        //-----------------------------------------------------------------------------------------------------
        public static bool WriteToCsvFile(this DataTable dataTable, string filePath)
        {
            try
            {
                StringBuilder fileContent = new StringBuilder();

                foreach (var col in dataTable.Columns)
                {
                    fileContent.Append(col.ToString() + ",");
                }

                fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);

                //dataTable.Rows[0][5] = "ab,cd" + Environment.NewLine + "efgh";

                foreach (DataRow dr in dataTable.Rows)
                {

                    foreach (var column in dr.ItemArray)
                    {
                        fileContent.Append("\"" + column.ToString().Replace(Environment.NewLine, " ").Replace("\"", "\"\"") + "\",");
                    }

                    fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);
                }

                System.IO.File.WriteAllText(filePath, fileContent.ToString(), Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}


