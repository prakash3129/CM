using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Customization;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;

namespace GCC
{
    public partial class frmQC : DevComponents.DotNetBar.Office2007Form
    {
        public frmQC()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
            MessageBoxEx.EnableGlass = false;
            
        }

        private Form _ParantForm;

        //-----------------------------------------------------------------------------------------------------
        public Form ParantForm /////List of Columns to display on search window/////
        {
            get { return _ParantForm; }
            set { _ParantForm = value; }
        }

        GridControl gControl = new GridControl();
        LayoutView lView;
        DataTable dtQCTable;
        string sSelectedDate = string.Empty;
        string sProcessTable = string.Empty;
        string sProcessType = string.Empty;
        int iSamplePercent = 10;
        string sDateColumn = string.Empty;
        private void frmQC_Load(object sender, EventArgs e)
        {            
            
            ((frmMain)ParantForm).dateQCSampleDate.Value = GM.GetDateTime().AddDays(-1);
            Load_QC(GM.GetDateTime().AddDays(-1).ToString("yyyy-MM-dd"));
            
        }

        private void btnRandom_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            splitQC.Panel2Collapsed = true;
            dgvQCList.DataSource = null;
            if (lView.SelectedRowsCount > 0)
            {
                int[] selRows = lView.GetSelectedRows();
                DataRowView selRow = (DataRowView)lView.GetRow(selRows[0]);

                string sSelectedAgentName = selRow["AgentName"].ToString();
               

                string sValidStatus = string.Empty;
                if (GV.sAccessTo == "TR")
                    sValidStatus = GV.sTRContactstatusTobeValidated;
                else
                    sValidStatus = GV.sWRContactstatusTobeValidated;
                if (sSelectedAgentName.Length > 0)
                {
                    string sQuery = string.Empty;
                    if (sProcessTable == "Contact")
                    {
                        if (sProcessType == "Fresh")
                        {
                            sQuery = "SELECT TOP 1 A.Master_ID FROM " + GV.sContactTable + " A LEFT JOIN " + GV.sQCTable + " B ON A.CONTACT_ID_P = B.RecordID AND B.TableName='Contact' AND B.ResearchType='" + GV.sAccessTo + "'";
                            sQuery += " WHERE CAST(A." + GV.sAccessTo + "_UPDATED_DATE AS DATE) = '" + sSelectedDate + "' AND A." + GV.sAccessTo + "_AGENT_NAME='" + sSelectedAgentName + "' AND A." + GV.sAccessTo + "_CONTACT_STATUS IN ";
                            sQuery += " (" + sValidStatus + ")  AND B.QC_Sample_Status IS NULL Order By Rand();";
                        }
                        else
                        {
                            sQuery = "SELECT TOP 1 A.Master_ID FROM " + GV.sContactTable + " A INNER JOIN " + GV.sQCTable + " B ON A.CONTACT_ID_P = B.RecordID AND B.TableName='Contact' AND B.ResearchType='" + GV.sAccessTo + "'";
                            sQuery += " WHERE CAST(A." + GV.sAccessTo + "_UPDATED_DATE AS DATE) = '" + sSelectedDate + "' AND A." + GV.sAccessTo + "_AGENT_NAME='" + sSelectedAgentName + "' AND B.QC_Status ='Reprocessed'";
                            sQuery += " Order By Rand();";
                        }
                    }
                    else
                    {                        
                        if (sProcessType == "Fresh")
                        {
                            sQuery = "SELECT TOP 1 A.Master_ID FROM " + GV.sCompanyTable + " A LEFT JOIN " + GV.sQCTable + " B ON A.MASTER_ID = B.RecordID AND B.TableName='Company' AND B.ResearchType='" + GV.sAccessTo + "'";
                            sQuery += " WHERE CAST(A." + sDateColumn + " AS DATE) = '" + sSelectedDate + "' AND A." + GV.sAccessTo + "_AGENTNAME='" + sSelectedAgentName + "' ";
                            sQuery += " AND B.QC_Sample_Status IS NULL Order By Rand();";
                        }
                        else
                        {
                            sQuery = "SELECT TOP 1 A.Master_ID FROM " + GV.sCompanyTable + " A INNER JOIN " + GV.sQCTable + " B ON A.MASTER_ID = B.RecordID AND B.TableName='Company' AND B.ResearchType='" + GV.sAccessTo + "'";
                            sQuery += " WHERE CAST(A." + sDateColumn + " AS DATE) = '" + sSelectedDate + "' AND A." + GV.sAccessTo + "_AGENTNAME='" + sSelectedAgentName + "' AND B.QC_Status ='Reprocessed'";
                            sQuery += " Order By Rand();";
                        }
                    }

                    System.Data.DataTable dtSampleRecords = GV.MSSQL1.BAL_ExecuteQuery(sQuery);
                    if (dtSampleRecords.Rows.Count > 0)                                            
                        GM.OpenContactUpdate(dtSampleRecords.Rows[0]["MASTER_ID"].ToString(), false, true, this, null);                    
                }
            }
        }

        private void btnGetSample_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {            
            if (lView.SelectedRowsCount > 0)
            {
                int[] selRows = lView.GetSelectedRows();
                DataRowView selRow = (DataRowView)lView.GetRow(selRows[0]);                
                string sSelectedAgentName = selRow["AgentName"].ToString();
                string sValidStatus = string.Empty;
                if (GV.sAccessTo == "TR")
                    sValidStatus = GV.sTRContactstatusTobeValidated;
                else
                    sValidStatus = GV.sWRContactstatusTobeValidated;
                if (sSelectedAgentName.Length > 0)
                {
                    string sMainQuery = string.Empty;
                    if (sProcessTable == "Contact")
                    {
                        if (sProcessType == "Fresh")
                        {
                            sMainQuery = " FROM " + GV.sContactTable + " A LEFT JOIN " + GV.sQCTable + " B ON A.CONTACT_ID_P = B.RecordID AND B.TableName='Contact' AND B.ResearchType='" + GV.sAccessTo + "'";
                            sMainQuery += " WHERE CAST(A." + GV.sAccessTo + "_UPDATED_DATE as DATE) = '" + sSelectedDate + "' AND A." + GV.sAccessTo + "_AGENT_NAME='" + sSelectedAgentName + "' AND A." + GV.sAccessTo + "_CONTACT_STATUS IN ";
                            sMainQuery += " (" + sValidStatus + ") AND B.QC_Sample_Status IS NULL";
                        }
                        else
                        {
                            sMainQuery = " FROM " + GV.sContactTable + " A INNER JOIN " + GV.sQCTable + " B ON A.CONTACT_ID_P = B.RecordID AND B.TableName='Contact' AND B.ResearchType='" + GV.sAccessTo + "'";
                            sMainQuery += " WHERE CAST(A." + GV.sAccessTo + "_UPDATED_DATE as DATE) = '" + sSelectedDate + "' AND A." + GV.sAccessTo + "_AGENT_NAME='" + sSelectedAgentName + "' AND B.QC_Status ='Reprocessed'";                            
                        }
                    }
                    else
                    {
                        if (sProcessType == "Fresh")
                        {
                            sMainQuery = " FROM " + GV.sCompanyTable + " A LEFT JOIN " + GV.sQCTable + " B ON A.MASTER_ID = B.RecordID AND B.TableName='Company' AND B.ResearchType='" + GV.sAccessTo + "'";
                            sMainQuery += " WHERE CAST(A." + sDateColumn + " as DATE) = '" + sSelectedDate + "' AND A." + GV.sAccessTo + "_AGENTNAME='" + sSelectedAgentName + "' AND ";
                            sMainQuery += " B.QC_Sample_Status IS NULL";
                        }
                        else
                        {
                            sMainQuery = " FROM " + GV.sCompanyTable + " A INNER JOIN " + GV.sQCTable + " B ON A.MASTER_ID = B.RecordID AND B.TableName='Company' AND B.ResearchType='" + GV.sAccessTo + "'";
                            sMainQuery += " WHERE CAST(A." + sDateColumn + " as DATE) = '" + sSelectedDate + "' AND A." + GV.sAccessTo + "_AGENTNAME='" + sSelectedAgentName + "' AND B.QC_Status ='Reprocessed'";                            
                        }
                    }

                    string sQuery = "select ceiling(count(1) * " + iSamplePercent + " / 100.0) Count " + sMainQuery + ";";
                    DataTable dtCount = GV.MSSQL1.BAL_ExecuteQuery(sQuery);

                    if (dtCount.Rows.Count > 0 && dtCount.Rows[0][0].ToString().Length > 0)
                    {
                        
                        string sColumns = string.Empty;
                        DataTable dtColumns = GV.MSSQL1.BAL_ExecuteQuery("SELECT FIELD_NAME_CAPTION,FIELD_NAME_TABLE FROM c_field_master WHERE TABLE_NAME='" + (sProcessTable == "Contact" ? "MasterContacts" : "Master") + "' AND PROJECT_ID='" + GV.sProjectID + "' AND ACTIVE_COLUMN='Y' ORDER BY SEQUENCE_NO;");
                        if (dtColumns.Rows.Count > 0)
                        {
                            foreach (DataRow drColumns in dtColumns.Rows)
                            {
                                if (sColumns.Length > 0)
                                    sColumns += ",A."+drColumns["FIELD_NAME_TABLE"] + " AS " + "'" + drColumns["FIELD_NAME_CAPTION"] + "'";
                                else
                                    sColumns = "A."+drColumns["FIELD_NAME_TABLE"] + " AS " + "'" + drColumns["FIELD_NAME_CAPTION"] + "'";
                            }
                        }
                        else
                            sColumns = "A.*";
                        
                        sQuery = "SELECT TOP " + dtCount.Rows[0][0].ToString() + " " + (sProcessTable == "Contact" ? " A.Contact_ID_P, A.Master_ID, " : " A.Master_ID, ") + "" + sColumns + " " + sMainQuery + " Order By Rand();";

                        DataTable dtRandomRecord = GV.MSSQL1.BAL_ExecuteQuery(sQuery);
                        if (dtRandomRecord.Rows.Count > 0)
                        {
                            dgvQCList.DataSource = dtRandomRecord;
                            splitQC.Panel2Collapsed = false;
                        }
                    }
                }
            }
        }

        private void btnShowProcessed_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            splitQC.Panel2Collapsed = false;
            dgvQCList.DataSource = null;
            if (lView.SelectedRowsCount > 0)
            {
                int[] selRows = lView.GetSelectedRows();
                DataRowView selRow = (DataRowView)lView.GetRow(selRows[0]);


                string sSelectedAgentName = selRow["AgentName"].ToString();
                string sValidStatus = string.Empty;
                if (GV.sAccessTo == "TR")
                    sValidStatus = GV.sTRContactstatusTobeValidated;
                else
                    sValidStatus = GV.sWRContactstatusTobeValidated;
                if (sSelectedAgentName.Length > 0)
                {
                    

                    string sQuery = string.Empty;
                    string sColumns = string.Empty;
                    DataTable dtColumns = GV.MSSQL1.BAL_ExecuteQuery("SELECT FIELD_NAME_CAPTION,FIELD_NAME_TABLE FROM c_field_master WHERE TABLE_NAME='MasterContacts' AND PROJECT_ID='" + GV.sProjectID + "' AND ACTIVE_COLUMN='Y' ORDER BY SEQUENCE_NO;");
                    if (dtColumns.Rows.Count > 0)
                    {
                        foreach (DataRow drColumns in dtColumns.Rows)
                        {
                            if (sColumns.Length > 0)
                                sColumns += ",A." + drColumns["FIELD_NAME_TABLE"] + " AS " + "'" + drColumns["FIELD_NAME_CAPTION"] + "'";
                            else
                                sColumns = "A." + drColumns["FIELD_NAME_TABLE"] + " AS " + "'" + drColumns["FIELD_NAME_CAPTION"] + "'";
                        }
                    }
                    else
                        sColumns = "A.*";
                    sQuery = "SELECT A.Contact_ID_P, A.Master_ID," + sColumns + " FROM " + GV.sContactTable + " A INNER JOIN " + GV.sQCTable + " B ON A.CONTACT_ID_P = B.RecordID AND B.TableName='Contact' AND B.ResearchType='" + GV.sAccessTo + "'";
                    sQuery += " WHERE CONVERT(A." + GV.sAccessTo + "_UPDATED_DATE,DATE) = '" + sSelectedDate + "' AND A." + GV.sAccessTo + "_AGENT_NAME='" + sSelectedAgentName + "' AND A." + GV.sAccessTo + "_CONTACT_STATUS IN ";
                    sQuery += " (" + sValidStatus + ")  AND B.QC_Sample_Status IN (0,1) ORDER BY Master_ID";
                    System.Data.DataTable dtRandomRecord = GV.MSSQL1.BAL_ExecuteQuery(sQuery);
                    if (dtRandomRecord.Rows.Count > 0)
                    {
                        dgvQCList.DataSource = dtRandomRecord;
                        splitQC.Panel2Collapsed = false;
                    }                    
                }
            }
        }

        public void Load_QC(string sDate)
        {
            sSelectedDate = sDate;
            iSamplePercent = ((frmMain)ParantForm).txtQCSamplePecent.Value;
            if (GV.sAccessTo == "TR")
                sDateColumn = "TR_DATECALLED";
            else
                sDateColumn = "WR_DATE_OF_PROCESS";

            

            if (((frmMain)ParantForm).switchQCProcessTable.Value)
                sProcessTable = "Company";
            else
                sProcessTable = "Contact";

            

            if (((frmMain)ParantForm).switchQCProcessType.Value)
                sProcessType = "Reprocessed";
            else
                sProcessType = "Fresh";

            lblProcessDate.Text = "       Process Date :" + sDate;            
            lblProcessTable.Text = "       Process Table :" + sProcessTable;
            lblSamplePercent.Text = "       Sample Percent :" + iSamplePercent;
            lblProcessType.Text = "       Process Type :" + sProcessType;
            

            splitQC.Panel2Collapsed = true;
            if (GV.sUserType == "QC" || GV.sUserType == "Admin")
            {
                gControl.DataSource = null;
                string sQuery = string.Empty;
                string sValidContactStatus = string.Empty;

                if (GV.sAccessTo == "TR")
                    sValidContactStatus = GV.sTRContactstatusTobeValidated;
                else
                    sValidContactStatus = GV.sWRContactstatusTobeValidated;

                string sProcessed = string.Empty;
                if (sProcessType == "Reprocessed")
                    sProcessed = " WHERE (SendBack > 0 OR Reprocessed >0) ";    

                if (sProcessTable == "Contact")
                {
                    //sQuery = "SELECT T.AgentName,Processed, ROUND(((Pass + Fail)/Processed)*100,2,1) Sampled,ROUND((Pass/Processed)*100,2,1) Pass,ROUND((Fail/Processed)*100,2,1) Fail, SendBack,Reprocessed FROM (";
                    //sQuery += " SELECT " + GV.sAccessTo + "_AGENT_NAME AgentName,COUNT(*) Processed,";
                    //sQuery += " COUNT((SELECT 1 FROM " + GV.sQCTable + " QC WHERE QC.RecordID=CM.CONTACT_ID_P AND TABLENAME='Contact' AND QC.ResearchType='" + GV.sAccessTo + "' AND QC.QC_Sample_Status = 1)) Pass,";
                    //sQuery += " COUNT((SELECT 1 FROM " + GV.sQCTable + " QC WHERE QC.RecordID=CM.CONTACT_ID_P AND TABLENAME='Contact' AND QC.ResearchType='" + GV.sAccessTo + "'  AND QC.QC_Sample_Status = 0)) Fail,";
                    //sQuery += " COUNT((SELECT 1 FROM " + GV.sQCTable + " QC WHERE QC.RecordID=CM.CONTACT_ID_P AND TABLENAME='Contact' AND QC.ResearchType='" + GV.sAccessTo + "' AND QC.QC_STATUS='SendBack')) SendBack,";
                    //sQuery += " COUNT((SELECT 1 FROM " + GV.sQCTable + " QC WHERE QC.RecordID=CM.CONTACT_ID_P AND TABLENAME='Contact' AND QC.ResearchType='" + GV.sAccessTo + "'  AND QC.QC_STATUS='Reprocessed')) Reprocessed";
                    //sQuery += " FROM " + GV.sContactTable + "  CM WHERE cm." + GV.sAccessTo + "_CONTACT_STATUS IN (" + sValidContactStatus + ")";
                    //sQuery += " AND CAST(CM." + GV.sAccessTo + "_UPDATED_DATE AS DATE) = '" + sDate + "' GROUP BY " + GV.sAccessTo + "_AGENT_NAME)T " + sProcessed + " Order by T.Processed Desc;";

                    sQuery = "select T.AgentName,Processed, ROUND(((Pass + Fail) / Processed) * 100, 2, 1) Sampled,ROUND((Pass / Processed) * 100, 2, 1) Pass,ROUND((Fail / Processed) * 100, 2, 1) Fail, SendBack, Reprocessed from (";
                    sQuery += " select cm." + GV.sAccessTo + "_AGENT_NAME agentname, COUNT(*) Processed,";
                    sQuery += " count(case when  QC.QC_Sample_Status = 1 then cm.CONTACT_ID_P end) Pass,";
                    sQuery += " count(case when  QC.QC_Sample_Status = 0 then cm.CONTACT_ID_P end) Fail,";
                    sQuery += " count(case when  QC.QC_STATUS = 'SENDBACK' then cm.CONTACT_ID_P end) SendBack,";
                    sQuery += " count(case when  QC.QC_STATUS = 'Reprocessed' then cm.CONTACT_ID_P end) Reprocessed";
                    sQuery += " from " + GV.sContactTable + " CM left join (select * from " + GV.sQCTable + " where TableName = 'Contact'  AND ResearchType = '" + GV.sAccessTo + "') QC";
                    sQuery += " on QC.RecordID = CM.CONTACT_ID_P WHERE cm." + GV.sAccessTo + "_CONTACT_STATUS IN (" + sValidContactStatus + ")";
                    sQuery += " AND CAST(CM." + GV.sAccessTo + "_UPDATED_DATE AS DATE) = '" + sDate + "' GROUP BY cm." + GV.sAccessTo + "_AGENT_NAME )t Order by T.Processed Desc;";

                }
                else
                {                    
                    //sQuery = "SELECT T.AgentName,Processed, ROUND(((Pass + Fail)/Processed)*100,2,1) Sampled,ROUND((Pass/Processed)*100,2,1) Pass,ROUND((Fail/Processed)*100,2,1) Fail, SendBack,Reprocessed FROM (";
                    //sQuery += " SELECT " + GV.sAccessTo + "_AGENTNAME AgentName,COUNT(*) Processed,";
                    //sQuery += " COUNT((SELECT 1 FROM " + GV.sQCTable + " QC WHERE QC.RecordID=CM.MASTER_ID AND TABLENAME='Company' AND QC.ResearchType='" + GV.sAccessTo + "' AND QC.QC_Sample_Status = 1)) Pass,";
                    //sQuery += " COUNT((SELECT 1 FROM " + GV.sQCTable + " QC WHERE QC.RecordID=CM.MASTER_ID AND TABLENAME='Company' AND QC.ResearchType='" + GV.sAccessTo + "'  AND QC.QC_Sample_Status = 0)) Fail,";
                    //sQuery += " COUNT((SELECT 1 FROM " + GV.sQCTable + " QC WHERE QC.RecordID=CM.MASTER_ID AND TABLENAME='Company' AND QC.ResearchType='" + GV.sAccessTo + "' AND QC.QC_STATUS='SendBack')) SendBack,";
                    //sQuery += " COUNT((SELECT 1 FROM " + GV.sQCTable + " QC WHERE QC.RecordID=CM.MASTER_ID AND TABLENAME='Company' AND QC.ResearchType='" + GV.sAccessTo + "'  AND QC.QC_STATUS='Reprocessed')) Reprocessed";
                    //sQuery += " FROM " + GV.sCompanyTable + "  CM WHERE ";
                    //sQuery += " CAST(CM." + sDateColumn + " AS DATE) = '" + sDate + "' GROUP BY " + GV.sAccessTo + "_AGENTNAME)T " + sProcessed + " Order by T.Processed Desc;";



                    sQuery = "select T.AgentName,Processed, ROUND(((Pass + Fail) / Processed) * 100, 2, 1) Sampled,ROUND((Pass / Processed) * 100, 2, 1) Pass,ROUND((Fail / Processed) * 100, 2, 1) Fail, SendBack, Reprocessed from (";
                    sQuery += " select cm." + GV.sAccessTo + "_AGENT_NAME agentname, COUNT(*) Processed,";
                    sQuery += " count(case when  QC.QC_Sample_Status = 1 then cm.CONTACT_ID_P end) Pass,";
                    sQuery += " count(case when  QC.QC_Sample_Status = 0 then cm.CONTACT_ID_P end) Fail,";
                    sQuery += " count(case when  QC.QC_STATUS = 'SENDBACK' then cm.CONTACT_ID_P end) SendBack,";
                    sQuery += " count(case when  QC.QC_STATUS = 'Reprocessed' then cm.CONTACT_ID_P end) Reprocessed";
                    sQuery += " from " + GV.sContactTable + " CM left join (select * from " + GV.sQCTable + " where TableName = 'Contact'  AND ResearchType = '" + GV.sAccessTo + "') QC";
                    sQuery += " on QC.RecordID = CM.CONTACT_ID_P WHERE ";
                    sQuery += " CAST(CM." + sDateColumn + " AS DATE) = '" + sDate + "' GROUP BY cm." + GV.sAccessTo + "_AGENT_NAME )t Order by T.Processed Desc;";
                }




                

                dtQCTable = GV.MSSQL1.BAL_ExecuteQuery(sQuery);
                if (dtQCTable.Rows.Count > 0)
                {
                    DataColumn dcEmpImage = new DataColumn("EmpImage", typeof(System.Byte[]));
                    dtQCTable.Columns.Add("Random");
                    dtQCTable.Columns.Add("GetSample");
                    dtQCTable.Columns.Add("QCProcessed");
                    dtQCTable.Columns.Add(dcEmpImage);
                    
                    sQuery = "SELECT UserName , EmployeeImage FROM MVC..EmployeeImage A INNER JOIN Timesheet..Users B ON A.EmployeeID = B.EmployeeNo WHERE B.Active = 'Y' AND B.UserName IN ("+GM.ColumnToQString("AgentName",dtQCTable,"String")+");";
                    DataTable dtEmpImage = GV.MSSQL.BAL_ExecuteQuery(sQuery);

                    Byte[] bDummyImg = GM.imgToByte(Properties.Resources.Misc_User_icon__1_);

                    foreach (DataRow drImage in dtEmpImage.Rows)
                    {
                        string sEmpName = drImage["UserName"].ToString().ToUpper();
                        foreach (DataRow drQCTable in dtQCTable.Rows)
                        {
                            if (drQCTable["AgentName"].ToString().ToUpper() == sEmpName)
                            {
                                drQCTable["EmpImage"] = drImage["EmployeeImage"];
                                break;
                            }
                            if (drQCTable["EmpImage"] == DBNull.Value)
                                drQCTable["EmpImage"] = bDummyImg;
                        }
                    }

                    //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQC));
                    lView = new LayoutView(gControl);


                    gControl.LookAndFeel.SkinName = "Office 2010 Blue";
                    gControl.LookAndFeel.UseDefaultLookAndFeel = false;


                    lView.OptionsSelection.MultiSelect = false;

                    gControl.MainView = lView;

                    DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnItemRandom = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
                    btnItemRandom.Name = "btnRandom";
                    btnItemRandom.Buttons[0].Caption = "Random";
                    btnItemRandom.Buttons[0].IsLeft = false;
                    btnItemRandom.Buttons[0].Width = 30;                    
                    btnItemRandom.Buttons[0].Kind = ButtonPredefines.Glyph;
                    btnItemRandom.Buttons[0].Image = Properties.Resources.new_icon;
                    btnItemRandom.TextEditStyle = TextEditStyles.HideTextEditor;
                   // btnItemRandom.AutoHeight = true;
                    btnItemRandom.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                    btnItemRandom.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnRandom_ButtonClick);

                    DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnItemGetSample = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
                    btnItemGetSample.Name = "btnGetSample";
                    btnItemGetSample.Buttons[0].Caption = "Sampled";
                    btnItemGetSample.Buttons[0].IsLeft = false;
                    btnItemGetSample.Buttons[0].Width = 30;
                    btnItemGetSample.Buttons[0].Kind = ButtonPredefines.Glyph;
                    btnItemGetSample.Buttons[0].Image = Properties.Resources.contacts_3_icon;
                    btnItemGetSample.TextEditStyle = TextEditStyles.HideTextEditor;
                    //btnItemGetSample.AutoHeight = true;
                    btnItemGetSample.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                    btnItemGetSample.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnGetSample_ButtonClick);

                    DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnItemQCProcessed = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
                    btnItemQCProcessed.Name = "btnQCProcessed";
                    btnItemQCProcessed.Buttons[0].Caption = "QC Processed";
                    btnItemQCProcessed.Buttons[0].IsLeft = false;
                    btnItemQCProcessed.Buttons[0].Width = 30;
                    btnItemQCProcessed.Buttons[0].Kind = ButtonPredefines.Glyph;
                    btnItemQCProcessed.Buttons[0].Image = Properties.Resources.folder_icon__1_;
                    btnItemQCProcessed.TextEditStyle = TextEditStyles.HideTextEditor;
                    //btnItemQCProcessed.AutoHeight = true;
                    btnItemQCProcessed.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                    btnItemQCProcessed.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnShowProcessed_ButtonClick);


                    lView.OptionsBehavior.AutoPopulateColumns = false;
                    splitQC.Panel1.Controls.Add(gControl);
                    splitQC.Panel1.Controls.Add(panelQCInfo);
                    gControl.Dock = DockStyle.Fill;
                    panelQCInfo.Dock = DockStyle.Top;



                    LayoutViewColumn colAgentName = lView.Columns.AddField("AgentName");
                    LayoutViewColumn colProcessCount = lView.Columns.AddField("Processed");
                    LayoutViewColumn colSampled = lView.Columns.AddField("Sampled");                    
                    LayoutViewColumn colPass;
                    LayoutViewColumn colFail;
                    
                    if(sProcessType == "Fresh")
                    {
                        colPass= lView.Columns.AddField("Pass");
                        colFail = lView.Columns.AddField("Fail");
                    }
                    else
                    {
                        colPass= lView.Columns.AddField("SendBack");
                        colFail = lView.Columns.AddField("Reprocessed");
                    }
                                        
                    LayoutViewColumn colbtnRandom = lView.Columns.AddField("Random");
                    LayoutViewColumn colbtnGetSample = lView.Columns.AddField("GetSample");
                    LayoutViewColumn colbtnQCProcessed = lView.Columns.AddField("QCProcessed");
                    LayoutViewColumn colPhoto = lView.Columns.AddField("EmpImage");


                    //colbtnRandom.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;

                    LayoutViewField fieldAgentName = colAgentName.LayoutViewField;
                    LayoutViewField fieldProcessCount = colProcessCount.LayoutViewField;
                    LayoutViewField fieldSampled = colSampled.LayoutViewField;
                    LayoutViewField fieldPass = colPass.LayoutViewField;
                    LayoutViewField fieldFail = colFail.LayoutViewField;
                    LayoutViewField fieldbtnRandom = colbtnRandom.LayoutViewField;
                    LayoutViewField fieldbtnGetSample = colbtnGetSample.LayoutViewField;
                    LayoutViewField fieldbtnQCProcessed = colbtnQCProcessed.LayoutViewField;
                    LayoutViewField fieldEmpPhoto = colPhoto.LayoutViewField;
                    fieldEmpPhoto.SizeConstraintsType = SizeConstraintsType.Custom;
                    //fieldEmpPhoto.MaxSize = new System.Drawing.Size(20, 20);
                    fieldEmpPhoto.MinSize = new System.Drawing.Size(100, 100);
                    fieldEmpPhoto.TextSize = new System.Drawing.Size(0, 0);
                    fieldEmpPhoto.TextToControlDistance = 0;
                    fieldEmpPhoto.TextVisible = false;

                    fieldAgentName.MinSize = new System.Drawing.Size(100, 100);
                    fieldAgentName.TextSize = new System.Drawing.Size(0, 0);
                    fieldAgentName.TextToControlDistance = 0;
                    fieldAgentName.TextVisible = false;

                    colAgentName.AppearanceCell.Font = new System.Drawing.Font(colAgentName.AppearanceCell.Font.FontFamily,12,FontStyle.Bold);

                    colPhoto.Visible = true;

                    //RepositoryItemPictureEdit riPictureEdit = grid.RepositoryItems.Add("PictureEdit") as RepositoryItemPictureEdit;
                    
                    RepositoryItemPictureEdit riPictureEdit = new RepositoryItemPictureEdit();
                    riPictureEdit.SizeMode = PictureSizeMode.Zoom;
                    
                    colPhoto.ColumnEdit = riPictureEdit;

                    lView.OptionsView.ShowHeaderPanel = false;
                    lView.OptionsView.ViewMode = LayoutViewMode.MultiColumn;                                       

                    colAgentName.OptionsColumn.AllowFocus = false;
                    colProcessCount.OptionsColumn.AllowFocus = false;
                    colSampled.OptionsColumn.AllowFocus = false;
                    colPass.OptionsColumn.AllowFocus = false;
                    colFail.OptionsColumn.AllowFocus = false;
                    colPhoto.OptionsColumn.AllowFocus = false;

                    colAgentName.OptionsColumn.AllowEdit = false;
                    colProcessCount.OptionsColumn.AllowEdit = false;
                    colSampled.OptionsColumn.AllowEdit = false;
                    colPass.OptionsColumn.AllowEdit = false;
                    colFail.OptionsColumn.AllowEdit = false;
                    colPhoto.OptionsColumn.AllowEdit = false;

                    colAgentName.OptionsFilter.AllowFilter = false;
                    colProcessCount.OptionsFilter.AllowFilter = false;
                    colSampled.OptionsFilter.AllowFilter = false;
                    colPass.OptionsFilter.AllowFilter = false;
                    colFail.OptionsFilter.AllowFilter = false;
                    colbtnRandom.OptionsFilter.AllowFilter = false;
                    colbtnGetSample.OptionsFilter.AllowFilter = false;
                    colbtnQCProcessed.OptionsFilter.AllowFilter = false;
                    colPhoto.OptionsFilter.AllowFilter = false;

                    colAgentName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    colProcessCount.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    colSampled.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    colPass.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    colFail.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    colbtnRandom.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    colbtnGetSample.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    colbtnQCProcessed.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    colPhoto.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;

                    colAgentName.OptionsColumn.ReadOnly = true;
                    colProcessCount.OptionsColumn.ReadOnly = true;
                    colSampled.OptionsColumn.ReadOnly = true;
                    colPass.OptionsColumn.ReadOnly = true;
                    colFail.OptionsColumn.ReadOnly = true;
                    colbtnRandom.OptionsColumn.ReadOnly = true;
                    colbtnGetSample.OptionsColumn.ReadOnly = true;
                    colbtnQCProcessed.OptionsColumn.ReadOnly = true;
                    colPhoto.OptionsColumn.ReadOnly = true;

                    colbtnRandom.ColumnEdit = btnItemRandom;
                    colbtnGetSample.ColumnEdit = btnItemGetSample;
                    colbtnQCProcessed.ColumnEdit = btnItemQCProcessed;

                    DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();

                    styleFormatCondition1.Appearance.BackColor = Color.RosyBrown;
                    styleFormatCondition1.Appearance.Options.UseBackColor = true;
                    styleFormatCondition1.ApplyToRow = false;
                    styleFormatCondition1.Column = colProcessCount;
                    styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
                    styleFormatCondition1.Expression = "[Processed] > 20";
                    lView.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {styleFormatCondition1});


                    fieldAgentName.Move(new LayoutItemDragController(fieldAgentName, fieldEmpPhoto, InsertLocation.After, LayoutType.Horizontal));

                    LayoutControlGroup groupAgentInfo = lView.TemplateCard.AddGroup("Agent Info", fieldAgentName, InsertType.Bottom);
                    //groupAddress.Add(colPhoto.LayoutViewField);
                    //groupAddress.Add(colAgentName.LayoutViewField);
                    groupAgentInfo.Add(colProcessCount.LayoutViewField);
                    groupAgentInfo.Add(colSampled.LayoutViewField);
                    groupAgentInfo.Add(colPass.LayoutViewField);
                    groupAgentInfo.Add(colFail.LayoutViewField);
                    groupAgentInfo.Add(colbtnRandom.LayoutViewField);
                    groupAgentInfo.Add(colbtnGetSample.LayoutViewField);
                    groupAgentInfo.Add(colbtnQCProcessed.LayoutViewField);


                    //LayoutControlGroup groupRecords = lView.TemplateCard.AddGroup("Record", groupAgentInfo, InsertType.Bottom);
                    //groupRecords.Add(colbtnRandom.LayoutViewField);
                    //groupRecords.Add(colbtnGetSample.LayoutViewField);
                    //groupRecords.Add(colbtnQCProcessed.LayoutViewField);

                    colAgentName.Caption = "Agent Name";
                    colProcessCount.Caption = "Total Processed";
                    colSampled.Caption = "Sampled %";
                    if (sProcessType == "Fresh")
                    {
                        colPass.Caption = "Pass %";
                        colFail.Caption = "Fail %";
                    }
                    else
                    {
                        colPass.Caption = "Send Back";
                        colFail.Caption = "Reprocessed";
                    }
                    colbtnRandom.Caption = "Get a Random Record";
                    colbtnGetSample.Caption = "Get " + iSamplePercent + "% sample";
                    colbtnQCProcessed.Caption = "Show Sampled Records";

                    
                    //lView.Items[9].TextVisible = false;                    
                    lView.CardMinSize = new Size(60, 250);                    

                    gControl.DataSource = dtQCTable;
                }
                else
                    gControl.DataSource = null;

                dgvQCList.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;

            }
        }

        private void dgvQCList_BackgroundColorChanged(object sender, EventArgs e)
        {
            if (dgvQCList.BackgroundColor != GV.pnlGlobalColor.Style.BackColor2.Color)
                dgvQCList.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
        }

        private void dgvQCList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
                GM.OpenContactUpdate(dgvQCList.Rows[e.RowIndex].Cells["MASTER_ID"].Value.ToString(), false, true, this, null);
        }
    }
}
