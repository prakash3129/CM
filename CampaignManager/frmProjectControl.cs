using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using System.IO;

using DevComponents.DotNetBar.SuperGrid.Style;

namespace GCC
{
    public partial class frmProjectControl : DevComponents.DotNetBar.Office2007Form
    {
        public frmProjectControl()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); //Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
            MessageBoxEx.EnableGlass = false;
        }

        //BAL.BAL_GlobalMghySQL objBAL_GlobalMyhSQL = new BAL.BAL_GlobalMyhgSQL();
        //BAL.BAL_Global objBAL_Global = new BAL.BAL_Global();

        DataTable dtProjectSettings = new DataTable();
        DataTable dtFieldCompany = new DataTable();
        DataTable dtFieldContact = new DataTable();
        DataTable dtFieldMasterAllProjects = new DataTable();
        DataTable dtExistingProjects = new DataTable();
        DataTable dtClonableProjects = new DataTable();
        
        DataTable dtValidationsCompany = new DataTable();
        DataTable dtValidationsContact = new DataTable();

        DataTable dtPickList = new DataTable();
        DataTable dtCM_FieldConfig = new DataTable();
        DataTable dtProjectMaster = new DataTable();
        DataTable dtDashboard = new DataTable();
        List<Control> lstProjectControls = new List<Control>();

        private bool _IsNewProject;
        public bool IsNewProject /////Is the project new ?
        {
            get { return _IsNewProject; }
            set { _IsNewProject = value; }
        }

        private void frmProjectControl_Load(object sender, EventArgs e)
        {
            
            ProjectEditProgressSteps.Width = this.Width;

            step0ProjectCreation.MinimumSize = new Size((this.Width / 4)+4, 30);
            step1ProjectSettings.MinimumSize = new Size((this.Width / 4)+4, 30);
            step2FieldSettings.MinimumSize = new Size((this.Width / 4)+4, 30);
            step3Validations.MinimumSize = new Size((this.Width / 4)+4, 30);
            //step4DataImport.MinimumSize = new Size((this.Width / 4)+4, 30);

            step0ProjectCreation.Enabled = true;
            step1ProjectSettings.Enabled = false;
            step2FieldSettings.Enabled = false;
            step3Validations.Enabled = false;
            //step4DataImport.Enabled = false;

            panelstep0ProjectCreation.Visible = true;
            panelstep1ProjectSettings.Visible = false;
            panelstep2FieldSettings.Visible = false;
            panelstep3Validations.Visible = false;

            step0ProjectCreation.SymbolColor = Color.Green;
            step1ProjectSettings.SymbolColor = Color.Gray;
            step2FieldSettings.SymbolColor = Color.Gray;
            step3Validations.SymbolColor = Color.Gray;
            //step4DataImport.SymbolColor = Color.Gray;

            panelstep0ProjectCreation.Parent = this;
            panelstep0ProjectCreation.Dock = DockStyle.Top;
            step0ProjectCreation.Value = 100;
            step1ProjectSettings.Value = 0;
            

            //panelstep1ProjectSettings.Parent = this;
            //panelstep1ProjectSettings.Dock = DockStyle.Top;
            Initial_Load();
            btnPrevious.Enabled = false;
            
            
            
        }

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


        void Initial_Load()
        {
            if (GV.sProjectID.Length == 0)
            {
                btnNext.Text = "&Create Project";
            }
            else
            {
                btnNext.Text = "&Update Project";
                dtProjectMaster = GV.MSSQL.BAL_FetchTable("Timesheet.dbo.ProjectMaster", "PROJECTID = '" + GV.sProjectID + "'");

                dtDashboard = GV.MSSQL_RM.BAL_FetchTable("DASHBOARD", "PROJECT_ID = '" + GV.sProjectID + "'");
                if (dtDashboard.Rows.Count == 0)//(Can occure if project is created under Time Logger Admin)
                {
                    DataRow drNewRow = dtDashboard.NewRow();
                    drNewRow["PROJECT_NAME"] = dtProjectMaster.Rows[0]["ProjectName"];
                    drNewRow["PROJECT_ID"] = dtProjectMaster.Rows[0]["ProjectID"];
                    drNewRow["ACTIVE"] = "Y";
                    dtDashboard.Rows.Add(drNewRow);
                    GV.MSSQL_RM.BAL_SaveToTable(dtDashboard, "DASHBOARD", "New", true);
                    dtDashboard = GV.MSSQL_RM.BAL_FetchTable("DASHBOARD", "PROJECT_ID = '" + GV.sProjectID + "'");
                }

                lstProjectControls = GetAllControls(panelstep0ProjectCreation);
                foreach (Control ctrl in lstProjectControls)
                {
                    if (ctrl.Tag != null && dtDashboard.Columns.Contains(ctrl.Tag.ToString()))
                        ctrl.Text = dtDashboard.Rows[0][ctrl.Tag.ToString()].ToString();
                }

                txtProjectName.Text = dtProjectMaster.Rows[0]["ProjectName"].ToString();
                txtClientname.Text = dtProjectMaster.Rows[0]["ClientName"].ToString();
                txtClientname.ButtonCustom.Visible = false;//freeze if existing project
                txtClientname.ReadOnly = true;
                txtProjectName.ReadOnly = true;
                txtProject_For.ReadOnly = true;
                txtProject_For.ButtonCustom.Visible = false;

                if (txtProject_StartDate.Text.Trim().Length > 0)//Enter start date if empty.. (Can occure if project is created under Time Logger Admin)
                {
                    txtProject_StartDate.ButtonDropDown.Visible = false;
                    txtProject_StartDate.IsInputReadOnly = true;
                }
            }
        }

        void Project_Control_Load()
        {
            panelProjectClone.Visible = IsNewProject;
            panelSetting.Visible = !IsNewProject;
            if (IsNewProject)
                Load_ClonableProjects();//Load clone projects combo and project settings
            else
            {
                step1ProjectSettings.Value = 100;
                ProjectDetailsLoad();
                Populate_Settings(false);
                Populate_Fields(sdgvCompany.PrimaryGrid, dtFieldCompany);
                Populate_Fields(sdgvContact.PrimaryGrid, dtFieldContact);
                Populate_Validations();
            }
        }

        void Populate_Validations()
        {
            dgvValidationCompany.DataSource = dtValidationsCompany;
            dgvValidationContact.DataSource = dtValidationsContact;
        }

        private void ProjectDetailsLoad()
        {
            dtProjectSettings = GV.MSSQL1.BAL_FetchTable("c_project_settings", "PROJECT_ID = '" + GV.sProjectID + "'");
            GV.sCompanyTable = dtProjectSettings.Rows[0]["COMPANY_TABLE"].ToString();
            GV.sContactTable = dtProjectSettings.Rows[0]["CONTACTS_TABLE"].ToString();
            //if (IsNewProject)
            //{
            //    dtFieldCompany = GV.gfMYSjhQL.BAL_FetchTableMyjhSQL("c_field_master", "PROJECT_ID = 'Template' AND TABLE_NAME = 'Master' AND FIELD_TYPE = 'UserDefined'");
            //    dtFieldContact = GV.MYSgfQL.BAL_FetchTableMhjySQL("c_field_master", "PROJECT_ID = 'Template' AND TABLE_NAME = 'MasterContacts' AND FIELD_TYPE = 'UserDefined'");
            //}
            //else
            //{
                dtFieldCompany = GV.MSSQL1.BAL_FetchTable("c_field_master", "PROJECT_ID = '" + GV.sProjectID + "' AND TABLE_NAME = 'Master' AND FIELD_TYPE = 'UserDefined' ORDER BY FieldID");
                dtFieldContact = GV.MSSQL1.BAL_FetchTable("c_field_master", "PROJECT_ID = '" + GV.sProjectID + "' AND TABLE_NAME = 'MasterContacts' AND FIELD_TYPE = 'UserDefined' ORDER BY FieldID");
            //}
            //dtValidations = 
            dtPickList = GV.MSSQL1.BAL_FetchTable(GV.sProjectID+"_picklists", "1=1");
            dtProjectMaster = GV.MSSQL.BAL_ExecuteQuery("SELECT * FROM Timesheet..ProjectMaster WHERE Active = 'Y'");
            dtCM_FieldConfig = GV.MSSQL1.BAL_FetchTable("c_fieldconfig", "1=1");
            dtFieldMasterAllProjects = GV.MSSQL1.BAL_FetchTable("c_field_master", "FIELD_TYPE = 'UserDefined' ORDER BY FieldID");
            dtValidationsCompany = GV.MSSQL1.BAL_FetchTable(GV.sProjectID + "_validations", "Table_Name='" + GV.sProjectID + "_Mastercompanies'");
            dtValidationsContact = GV.MSSQL1.BAL_FetchTable(GV.sProjectID + "_validations", "Table_Name='" + GV.sProjectID + "_Mastercontacts'");
          
        }

        void Load_ClonableProjects()
        {
            dtProjectMaster = GV.MSSQL.BAL_ExecuteQuery("SELECT * FROM Timesheet..ProjectMaster WHERE Active = 'Y'");
            dtExistingProjects = GV.MSSQL1.BAL_ExecuteQuery("SELECT T2.* FROM information_schema.TABLES AS T1 INNER JOIN c_project_settings AS T2 ON T1.TABLE_NAME =T2.COMPANY_TABLE WHERE T1.TABLE_SCHEMA = DB_NAME() AND T2.STATUS = 'ACTIVE' AND T1.TABLE_TYPE = 'BASE TABLE'");
            dtClonableProjects.Columns.Add("ProjectID", typeof(string));
            dtClonableProjects.Columns.Add("ProjectName", typeof(string));

            foreach (DataRow dr in dtExistingProjects.Rows)
            {
                if (dtProjectMaster.Select("ProjectID = '" + dr["Project_ID"].ToString() + "'").Length > 0)
                {
                    DataRow drProj = dtProjectMaster.Select("ProjectID = '" + dr["Project_ID"].ToString() + "'")[0];
                    DataRow drNewRow = dtClonableProjects.NewRow();
                    drNewRow["ProjectID"] = drProj["ProjectID"].ToString();
                    drNewRow["ProjectName"] = drProj["ProjectName"].ToString();
                    dtClonableProjects.Rows.Add(drNewRow);
                }
            }

            //cmbProjects.DataSource = dtClonableProjects;
            //cmbProjects.ValueMember = "ProjectID";
        }


        private void Populate_Fields(GridPanel GPanel, DataTable dtCompanyContact)
        {
            if (dtCompanyContact != null && dtCompanyContact.Rows.Count > 0)
            {
                //dgvFieldsCompany.DataSource = dtFieldCompany;
                //dgvFieldsContacts.DataSource = dtFieldContact;

                if (dtCompanyContact != null && dtCompanyContact.Rows.Count > 0)
                {
                    //GridPanel pCompanyPanel = sdgvCompany.PrimaryGrid;
                    foreach (DataRow dr in dtCompanyContact.Rows)
                    {
                        GridRow gRow = new GridRow
                        (
                            dr["ACTIVE_COLUMN"].ToString() == "Y",
                            dr["CONTROL_TYPE"].ToString(),
                            dr["FIELD_NAME_CAPTION"].ToString(),
                            dr["SEQUENCE_NO"].ToString(),
                            dr["FIELD_SIZE"].ToString(),
                            dr["PICKLIST_CATEGORY"].ToString(),
                            dr["PICKLIST_SELECTION_TYPE"].ToString(),
                            dr["SORTABLE_COLUMN"].ToString() == "Y",
                            dr["READONLY"].ToString() == "Y",
                            dr["SPELLCHECK"].ToString() == "Y",
                            dr["SHOW_ON_GRID"].ToString() == "Y",
                            dr["SHOW_ON_CRITERIA"].ToString() == "Y",
                            dr["Visibility"].ToString(),
                            dr["FORMATTING"].ToString(),
                            "Existing", //ColumnStatus = Existing
                            dr["FieldID"].ToString(),
                            dr["FIELD_NAME_TABLE"].ToString()
                        );
                        GPanel.Rows.Add(gRow);
                    }

                    //pCompanyPanel.Columns["CONTROL_TYPE"].EditorType = typeof(FragrantComboBox);
                    //pCompanyPanel.Columns["CONTROL_TYPE"].EditorParams = new object[] { GetComboItems(dtCM_FieldConfig, "CONTROLS") };

                    //pCompanyPanel.Columns["PICKLIST_CATEGORY"].EditorType = typeof(FragrantComboBox);
                    //pCompanyPanel.Columns["PICKLIST_CATEGORY"].EditorParams = new object[] { GetComboItems(dtPickList, "PicklistCategory") };

                    //pCompanyPanel.Columns["PICKLIST_SELECTION_TYPE"].EditorType = typeof(FragrantComboBox);
                    //pCompanyPanel.Columns["PICKLIST_SELECTION_TYPE"].EditorParams = new object[] { GetComboItems(dtFieldMasterAllProjects, "PICKLIST_SELECTION_TYPE") };

                    //pCompanyPanel.Columns["Visibility"].EditorType = typeof(FragrantComboBox);
                    //pCompanyPanel.Columns["Visibility"].EditorParams = new object[] { GetComboItems(dtFieldMasterAllProjects, "Visibility") };                    
                    

                    foreach (GridRow gRow in GPanel.Rows)
                    {
                        if (Convert.ToBoolean(gRow["ACTIVE_COLUMN"].Value))
                        {
                            if (dtCM_FieldConfig.Select("CONTROLS = '" + gRow["CONTROL_TYPE"].Value.ToString() + "'").Length > 0)//Read only cells which are not required
                            {
                                DataRow drCM_FieldConfig = dtCM_FieldConfig.Select("CONTROLS = '" + gRow["CONTROL_TYPE"].Value.ToString() + "'")[0];

                                foreach (GridCell gc in gRow.Cells)
                                {
                                    if (drCM_FieldConfig.Table.Columns.Contains(gc.GridColumn.Name))
                                    {
                                        if (drCM_FieldConfig[gc.GridColumn.Name].ToString() == "Y")
                                            gc.AllowEdit = true;
                                        else//Revert options if not applicable for that control(like 'Telephone' doesnt need Spell check)
                                        {
                                            gc.AllowEdit = false;
                                            if (gc.GridColumn.EditorType.Name == "GridSwitchButtonEditControl")
                                                gc.Value = false;
                                            else if(gc.GridColumn.Name.ToUpper() != "ID")
                                                gc.Value = string.Empty;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (GridCell gc in gRow.Cells)
                            {
                                if (gc.GridColumn.Name != "ACTIVE_COLUMN")
                                {
                                    gc.ReadOnly = true;
                                }
                            }
                            //gRow["CONTROL_TYPE"].ReadOnly = true;
                            //gRow["FIELD_NAME_CAPTION"].ReadOnly = true;
                            //gRow["SEQUENCE_NO"].ReadOnly = true;
                            //gRow["FIELD_SIZE"].ReadOnly = true;
                            //gRow["PICKLIST_CATEGORY"].ReadOnly = true;
                            //gRow["PICKLIST_SELECTION_TYPE"].ReadOnly = true;
                            //gRow["SORTABLE_COLUMN"].ReadOnly = true;
                            //gRow["READONLY"].ReadOnly = true;
                            //gRow["SPELLCHECK"].ReadOnly = true;
                            //gRow["SHOW_ON_GRID"].ReadOnly = true;
                            //gRow["SHOW_ON_CRITERIA"].ReadOnly = true;
                            //gRow["Visibility"].ReadOnly = true;
                            //gRow["FORMATTING"].ReadOnly = true;
                        }
                    }  
                }
            }
        }

        private string Synch_GridnTable(GridPanel GP, DataTable dtCompanyContact, string sTableName)//Update datatable manually from grid
        {
            string sNewColumnsQuery = string.Empty;
            foreach (GridRow gr in GP.Rows)
            {
                if (gr["ID"].Value.ToString() !=  "New")
                {
                    foreach (DataRow dr in dtCompanyContact.Rows)
                    {
                        if (dr["FieldID"].ToString() == gr["ID"].Value.ToString())
                        {
                            foreach (DataColumn dc in dr.Table.Columns)
                            {
                                if (GP.Columns.Contains(dc.ColumnName))
                                {
                                    if ((dc.DataType.Name == "Decimal" || dc.DataType.Name == "Int32") & gr[dc.ColumnName].Value.ToString().Length == 0)
                                        dr[dc.ColumnName] = 0;
                                    else
                                    {
                                        if (gr[dc.ColumnName].Value.ToString() == "True")
                                            dr[dc.ColumnName] = "Y";
                                        else if (gr[dc.ColumnName].Value.ToString() == "False")
                                            dr[dc.ColumnName] = "N";
                                        else
                                            dr[dc.ColumnName] = gr[dc.ColumnName].Value.ToString();
                                    }
                                    //dc.DataType
                                }
                            }
                        }
                        dr["PROJECT_ID"] = GV.sProjectID;
                    }
                }
                else //New Row
                {
                    DataRow drNewRow = dtCompanyContact.NewRow();
                    foreach (DataColumn dc in drNewRow.Table.Columns)
                    {
                        if (GP.Columns.Contains(dc.ColumnName))
                        {
                            if ((dc.DataType.Name == "Decimal" || dc.DataType.Name == "Int32") & gr[dc.ColumnName].Value.ToString().Length == 0)
                                drNewRow[dc.ColumnName] = 0;
                            else
                            {
                                if (gr[dc.ColumnName].Value.ToString() == "True")
                                    drNewRow[dc.ColumnName] = "Y";
                                else if (gr[dc.ColumnName].Value.ToString() == "False")
                                    drNewRow[dc.ColumnName] = "N";
                                else
                                    drNewRow[dc.ColumnName] = gr[dc.ColumnName].Value.ToString();
                            }
                        }
                    }


                    //Build Column add Query
                    if (gr["ColumnStatus"].Value.ToString() == "Text")
                    {
                        if (gr["FIELD_SIZE"].Value.ToString().Length > 0 && Convert.ToInt32(gr["FIELD_SIZE"].Value) > 0)
                        {
                            if (sNewColumnsQuery.Trim().Length == 0)
                                sNewColumnsQuery += "ADD " + gr["FIELD_NAME_TABLE"].Value + " varchar(" + Convert.ToInt32(gr["FIELD_SIZE"].Value) + ")";
                            else if (!sNewColumnsQuery.Contains("ADD " + gr["FIELD_NAME_TABLE"].Value + " varchar(" + Convert.ToInt32(gr["FIELD_SIZE"].Value) + ")"))
                                sNewColumnsQuery += ", ADD " + gr["FIELD_NAME_TABLE"].Value + " varchar(" + Convert.ToInt32(gr["FIELD_SIZE"].Value) + ")";
                        }
                        else
                        {
                            if (sNewColumnsQuery.Trim().Length == 0)
                                sNewColumnsQuery += "ADD " + gr["FIELD_NAME_TABLE"].Value + " varchar(4000)";
                            else if (!sNewColumnsQuery.Contains("ADD " + gr["FIELD_NAME_TABLE"].Value + " varchar(4000)"))
                                sNewColumnsQuery += ", ADD " + gr["FIELD_NAME_TABLE"].Value + " varchar(4000)";
                        }
                    }
                    else if (gr["ColumnStatus"].Value.ToString() == "Number")
                    {
                        if (sNewColumnsQuery.Trim().Length == 0)
                            sNewColumnsQuery += "ADD " + gr["FIELD_NAME_TABLE"].Value + " int";
                        else if (!sNewColumnsQuery.Contains("ADD " + gr["FIELD_NAME_TABLE"].Value + " int"))
                            sNewColumnsQuery += ", ADD " + gr["FIELD_NAME_TABLE"].Value + " int";
                    }
                    else if (gr["ColumnStatus"].Value.ToString() == "Decimal Number")
                    {
                        if (sNewColumnsQuery.Trim().Length == 0)
                            sNewColumnsQuery += "ADD " + gr["FIELD_NAME_TABLE"].Value + " decimal(18,3)";
                        else if (!sNewColumnsQuery.Contains("ADD " + gr["FIELD_NAME_TABLE"].Value + " decimal(18,3)"))
                            sNewColumnsQuery += ", ADD " + gr["FIELD_NAME_TABLE"].Value + " decimal(18,3)";
                    }
                    else if (gr["ColumnStatus"].Value.ToString() == "Date and Time")
                    {
                        if (sNewColumnsQuery.Trim().Length == 0)
                            sNewColumnsQuery += "ADD " + gr["FIELD_NAME_TABLE"].Value + " datetime";
                        else if (!sNewColumnsQuery.Contains("ADD " + gr["FIELD_NAME_TABLE"].Value + " datetime"))
                            sNewColumnsQuery += ", ADD " + gr["FIELD_NAME_TABLE"].Value + " datetime";
                    }

                    drNewRow["PROJECT_ID"] = GV.sProjectID;
                    drNewRow["TABLE_NAME"] = sTableName;
                    drNewRow["FIELD_TYPE"] = "UserDefined";
                    dtCompanyContact.Rows.Add(drNewRow);
                }
            }

            if (sNewColumnsQuery.Trim().Length > 0)
            {
                if (sTableName == "Master")
                    sNewColumnsQuery = "ALTER TABLE " + GV.sCompanyTable + " " + sNewColumnsQuery;
                else if (sTableName == "MasterContacts")
                    sNewColumnsQuery = "ALTER TABLE " + GV.sContactTable + " " + sNewColumnsQuery;                
            }
            return sNewColumnsQuery;
        }

        bool SynchGrids()
        {
            string sCompanyQuery = Synch_GridnTable(sdgvCompany.PrimaryGrid, dtFieldCompany, "Master");
            string sContactQuery = Synch_GridnTable(sdgvContact.PrimaryGrid, dtFieldContact, "MasterContacts");

            if ((sCompanyQuery + sContactQuery).Length > 0)
            {
                DataTable dCompanyCurrent = GV.MSSQL1.BAL_FetchTable(GV.sCompanyTable, "TR_AGENTNAME LIKE 'CURRENT%' OR WR_AGENTNAME LIKE 'CURRENT%'");                

                if (dCompanyCurrent.Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    if (sCompanyQuery.Trim().Length > 0)
                        GV.MSSQL1.BAL_ExecuteQuery(sCompanyQuery);
                    if (sContactQuery.Trim().Length > 0)
                        GV.MSSQL1.BAL_ExecuteQuery(sContactQuery);
                    return true;
                }
            }
            return true;
        }

        private void sdgvCompany_CellValueChanged(object sender, GridCellValueChangedEventArgs e)
        {
            if (e.GridCell.GridColumn.Name == "ACTIVE_COLUMN")//Enable or disable row
            {
                //e.GridCell.GridRow["CONTROL_TYPE"].ReadOnly = !Convert.ToBoolean(e.GridCell.Value);
                e.GridCell.GridRow["FIELD_NAME_CAPTION"].ReadOnly = !Convert.ToBoolean(e.GridCell.Value);
                e.GridCell.GridRow["SEQUENCE_NO"].ReadOnly = !Convert.ToBoolean(e.GridCell.Value);
                e.GridCell.GridRow["FIELD_SIZE"].ReadOnly = !Convert.ToBoolean(e.GridCell.Value);
                //e.GridCell.GridRow["PICKLIST_CATEGORY"].ReadOnly = !Convert.ToBoolean(e.GridCell.Value);
                //e.GridCell.GridRow["PICKLIST_SELECTION_TYPE"].ReadOnly = !Convert.ToBoolean(e.GridCell.Value);
                e.GridCell.GridRow["SORTABLE_COLUMN"].ReadOnly = !Convert.ToBoolean(e.GridCell.Value);
                e.GridCell.GridRow["READONLY"].ReadOnly = !Convert.ToBoolean(e.GridCell.Value);
                e.GridCell.GridRow["SPELLCHECK"].ReadOnly = !Convert.ToBoolean(e.GridCell.Value);
                e.GridCell.GridRow["SHOW_ON_GRID"].ReadOnly = !Convert.ToBoolean(e.GridCell.Value);
                e.GridCell.GridRow["SHOW_ON_CRITERIA"].ReadOnly = !Convert.ToBoolean(e.GridCell.Value);
                //e.GridCell.GridRow["Visibility"].ReadOnly = !Convert.ToBoolean(e.GridCell.Value);
                //e.GridCell.GridRow["FORMATTING"].ReadOnly = !Convert.ToBoolean(e.GridCell.Value);
            }
            //else if (e.GridCell.GridColumn.Name == "CONTROL_TYPE")
            //{
                if (dtCM_FieldConfig.Select("CONTROLS = '" + e.GridCell.GridRow["CONTROL_TYPE"].Value.ToString() + "'").Length > 0)
                {
                    DataRow drCM_FieldConfig = dtCM_FieldConfig.Select("CONTROLS = '" + e.GridCell.GridRow["CONTROL_TYPE"].Value.ToString() + "'")[0];

                    foreach (GridCell gc in e.GridCell.GridRow.Cells)
                    {
                        if(drCM_FieldConfig.Table.Columns.Contains(gc.GridColumn.Name))
                        {
                            if (drCM_FieldConfig[gc.GridColumn.Name].ToString() == "Y")
                                gc.AllowEdit = true;
                            else//Revert options if not applicable for that control(like 'Telephone' doesnt need Spell check)
                            {
                                gc.AllowEdit = false;
                                if (gc.GridColumn.EditorType.Name == "GridSwitchButtonEditControl")
                                    gc.Value = false;
                                else if(gc.GridColumn.Name.ToUpper() != "ID")
                                    gc.Value = string.Empty;
                            }

                            if (e.GridCell.GridColumn.Name == "CONTROL_TYPE")
                                e.GridCell.GridRow["FORMATTING"].Value = string.Empty;
                        }
                    }
                }
            //}
        }

        
        private void sdgvCompany_CellDoubleClick(object sender, GridCellDoubleClickEventArgs e)
        {
            Cell_Impact(e.GridCell);
        }

        private void Cell_Impact(GridCell GC)
        {
            string sControlType = GC.GridRow["CONTROL_TYPE"].Value.ToString();
            if (GC.AllowEdit && Convert.ToBoolean(GC.GridRow["ACTIVE_COLUMN"].Value))
            {
                switch (GC.GridColumn.Name.ToUpper())
                {
                    case "FORMATTING":
                        if (sControlType.Trim().Length > 0 && dtCM_FieldConfig.Select("CONTROLS = '" + sControlType + "'").Length > 0)
                        {
                            string sFormatting = dtCM_FieldConfig.Select("CONTROLS = '" + sControlType + "'")[0]["FORMATTING_VALUES"].ToString();
                            List<string> lstFormatting = sFormatting.Split('~').ToList();
                            DataTable dtFormatting = new DataTable();
                            dtFormatting.Columns.Add("Formatting", typeof(string));
                            foreach (string s in lstFormatting)
                            {
                                DataRow dr = dtFormatting.NewRow();
                                dr["Formatting"] = s;
                                dtFormatting.Rows.Add(dr);
                            }
                                
                            frmComboList objFrmComboListFORMATTING = new frmComboList();
                            objFrmComboListFORMATTING.TitleText = "Formatting";
                            objFrmComboListFORMATTING.dtItems = dtFormatting;
                            objFrmComboListFORMATTING.lstColumnsToDisplay.Add("Formatting");
                            objFrmComboListFORMATTING.sColumnToSearch = "Formatting";
                            objFrmComboListFORMATTING.IsSpellCheckEnabeld = false;
                            objFrmComboListFORMATTING.IsMultiSelect = true;
                            objFrmComboListFORMATTING.IsSingleWordSelection = false;
                            if (objFrmComboListFORMATTING.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                                GC.Value = objFrmComboListFORMATTING.sReturn;
                        }
                        break;

                    case "CONTROL_TYPE":
                        frmComboList objFrmComboListCONTROL_TYPE = new frmComboList();
                        objFrmComboListCONTROL_TYPE.TitleText = "Control Type";
                        objFrmComboListCONTROL_TYPE.dtItems = dtFieldMasterAllProjects.DefaultView.ToTable(true, "CONTROL_TYPE");
                        objFrmComboListCONTROL_TYPE.lstColumnsToDisplay.Add("CONTROL_TYPE");
                        objFrmComboListCONTROL_TYPE.sColumnToSearch = "CONTROL_TYPE";
                        objFrmComboListCONTROL_TYPE.IsSpellCheckEnabeld = false;
                        objFrmComboListCONTROL_TYPE.IsMultiSelect = false;
                        objFrmComboListCONTROL_TYPE.IsSingleWordSelection = true;
                        if (objFrmComboListCONTROL_TYPE.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            //if(objFrmComboListCONTROL_TYPE.sReturn.Trim().Length > 0)
                            GC.Value = objFrmComboListCONTROL_TYPE.sReturn;
                        break;

                    case "PICKLIST_CATEGORY":
                        frmComboList objFrmComboListPICKLIST_CATEGORY = new frmComboList();
                        objFrmComboListPICKLIST_CATEGORY.TitleText = "Selection Values";
                        objFrmComboListPICKLIST_CATEGORY.dtItems = dtPickList.DefaultView.ToTable(true, "PicklistCategory");
                        objFrmComboListPICKLIST_CATEGORY.lstColumnsToDisplay.Add("PicklistCategory");
                        objFrmComboListPICKLIST_CATEGORY.sColumnToSearch = "PicklistCategory";
                        objFrmComboListPICKLIST_CATEGORY.IsSpellCheckEnabeld = false;
                        objFrmComboListPICKLIST_CATEGORY.IsMultiSelect = false;
                        objFrmComboListPICKLIST_CATEGORY.IsSingleWordSelection = true;
                        if (objFrmComboListPICKLIST_CATEGORY.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            //if(objFrmComboListCONTROL_TYPE.sReturn.Trim().Length > 0)
                            GC.Value = objFrmComboListPICKLIST_CATEGORY.sReturn;
                        break;

                    case "PICKLIST_SELECTION_TYPE":
                        frmComboList objFrmComboListPICKLIST_SELECTION_TYPE = new frmComboList();
                        objFrmComboListPICKLIST_SELECTION_TYPE.TitleText = "Selection Type";
                        objFrmComboListPICKLIST_SELECTION_TYPE.dtItems = dtFieldMasterAllProjects.DefaultView.ToTable(true, "PICKLIST_SELECTION_TYPE");
                        objFrmComboListPICKLIST_SELECTION_TYPE.lstColumnsToDisplay.Add("PICKLIST_SELECTION_TYPE");
                        objFrmComboListPICKLIST_SELECTION_TYPE.sColumnToSearch = "PICKLIST_SELECTION_TYPE";
                        objFrmComboListPICKLIST_SELECTION_TYPE.IsSpellCheckEnabeld = false;
                        objFrmComboListPICKLIST_SELECTION_TYPE.IsMultiSelect = false;
                        objFrmComboListPICKLIST_SELECTION_TYPE.IsSingleWordSelection = true;
                        if (objFrmComboListPICKLIST_SELECTION_TYPE.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            //if(objFrmComboListCONTROL_TYPE.sReturn.Trim().Length > 0)
                            GC.Value = objFrmComboListPICKLIST_SELECTION_TYPE.sReturn;
                        break;

                    case "VISIBILITY":
                        frmComboList objFrmComboListVISIBILITY = new frmComboList();
                        objFrmComboListVISIBILITY.TitleText = "Visibility";
                        objFrmComboListVISIBILITY.dtItems = dtFieldMasterAllProjects.DefaultView.ToTable(true, "VISIBILITY");
                        objFrmComboListVISIBILITY.lstColumnsToDisplay.Add("Visibility");
                        objFrmComboListVISIBILITY.sColumnToSearch = "Visibility";
                        objFrmComboListVISIBILITY.IsSpellCheckEnabeld = false;
                        objFrmComboListVISIBILITY.IsMultiSelect = false;
                        objFrmComboListVISIBILITY.IsSingleWordSelection = true;
                        if (objFrmComboListVISIBILITY.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            //if(objFrmComboListCONTROL_TYPE.sReturn.Trim().Length > 0)
                            GC.Value = objFrmComboListVISIBILITY.sReturn;
                        break;
                }
            }
        }

        private void Populate_Settings(bool IsSave)
        {
            foreach (DataColumn dc in dtProjectSettings.Columns)//Get or put data to controls in settings panel
            {
                Control[] ctrlArr = panelSetting.Controls.Find("c" + dc.ColumnName, true);
                if (ctrlArr.Length > 0 && "c" + dc.ColumnName == ctrlArr[0].Name)
                {
                    if (IsSave)//Save data from controls to tables
                    {
                        if (ctrlArr[0] is DevComponents.DotNetBar.Controls.SwitchButton)
                        {
                            if (((DevComponents.DotNetBar.Controls.SwitchButton)ctrlArr[0]).Value)
                                dtProjectSettings.Rows[0][dc.ColumnName] = "Y";
                            else
                                dtProjectSettings.Rows[0][dc.ColumnName] = "N";
                        }
                        else if (ctrlArr[0] is DevComponents.Editors.IntegerInput)
                        {
                            dtProjectSettings.Rows[0][dc.ColumnName] = ((DevComponents.Editors.IntegerInput)ctrlArr[0]).Value;
                        }
                    }
                    else//Populate data to controls
                    {
                        if (ctrlArr[0] is DevComponents.DotNetBar.Controls.SwitchButton)
                        {
                            if (dtProjectSettings.Rows[0][dc.ColumnName].ToString() == "N")
                                ((DevComponents.DotNetBar.Controls.SwitchButton)ctrlArr[0]).Value = false;
                            else
                                ((DevComponents.DotNetBar.Controls.SwitchButton)ctrlArr[0]).Value = true;
                        }
                        else if (ctrlArr[0] is DevComponents.Editors.IntegerInput)
                        {
                            if (ctrlArr.Length > 0 && "c" + dc.ColumnName == ctrlArr[0].Name)
                                ((DevComponents.Editors.IntegerInput)ctrlArr[0]).Value = Convert.ToInt32(dtProjectSettings.Rows[0][dc.ColumnName]);
                        }
                    }
                }
            }
        }

        string SaveProjectDetails()
        {
            string sErr = string.Empty;

            try
            {
                if (btnNext.Text == "&Create Project")
                {
                    txtClientname.Text = txtClientname.Text.Trim().Replace("  ", " ").Replace("—", "-").Replace("–", "-").Replace("'", "");
                    txtProjectName.Text = txtProjectName.Text.Trim().Replace("  ", " ").Replace("—", "-").Replace("–", "-").Replace("'", "");

                    string sProjectID = string.Empty;
                    string sProjectNameStr = string.Empty;
                    string sRandom = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    int incr = 0;

                    if (txtProjectName.Text.Replace(" ", "").Replace("-", "").Length < 3 || txtClientname.Text.Replace(" ", "").Replace("-", "").Length < 3)
                        return "Client Name and Project Name must have atleast 3 Characters.";

                    sProjectNameStr = (txtClientname.Text.Replace(" ", "").Replace("-", "")).Substring(0, 3) + (txtProjectName.Text.Replace(" ", "").Replace("-", "")).Substring(0, 3);

                    while (true)
                    {
                        DataTable dtMaxID = GV.MSSQL.BAL_ExecuteQuery("SELECT TOP 1 CONVERT(INT,REPLACE(ProjectID,'" + sProjectNameStr + "',''))+1 As ID FROM Timesheet.dbo.ProjectMaster WHERE ProjectID LIKE '" + sProjectNameStr + "___' ORDER BY 1 DESC");
                        if (dtMaxID.Rows.Count > 0)
                        {
                            int iMaxID = Convert.ToInt32(dtMaxID.Rows[0][0]);
                            if (iMaxID <= 9)
                                sProjectID = sProjectNameStr + "00" + iMaxID;
                            else if (iMaxID > 9 && iMaxID <= 99)
                                sProjectID = sProjectNameStr + "0" + iMaxID;
                            else if (iMaxID > 99)
                                sProjectID = sProjectNameStr + iMaxID;
                        }
                        else
                            sProjectID = sProjectNameStr + "001";

                        DataTable dtDupeCheck = GV.MSSQL.BAL_ExecuteQuery("SELECT ProjectID FROM Timesheet.dbo.ProjectMaster WHERE ProjectID = '" + sProjectID + "'");
                        if (dtDupeCheck.Rows.Count == 0)
                        {
                            dtProjectMaster = GV.MSSQL.BAL_FetchTable("Timesheet.dbo.ProjectMaster", "1=0");
                            DataTable dtProjectFor = GV.MSSQL.BAL_ExecuteQuery("SELECT Data,Pvalues FROM Timesheet.dbo.PickLists WHERE Field='Project_For' AND ProjectType='C'");
                            //objFrmComboList.dtItems = GV.MSSQL.BAL_ExecuteQuery("SELECT Data FROM Timesheet.dbo.PickLists WHERE Field='" + txt.Tag + "' AND ProjectType='C'");

                            DataRow drNewProject = dtProjectMaster.NewRow();
                            drNewProject["ProjectID"] = sProjectID;
                            drNewProject["Clientname"] = txtClientname.Text;
                            drNewProject["ProjectName"] = txtProjectName.Text;
                            drNewProject["Department"] = "GCC";

                            drNewProject["Main_Division"] = dtProjectFor.Select("Data = '" + txtProject_For.Text.Replace("'", "''") + "'")[0]["Pvalues"].ToString();

                            //if (txtProject_For.Text == "Voice")
                            //    drNewProject["Main_Division"] = "VOICE BUREAU";
                            //else if (txtProject_For.Text == "Web")
                            //    drNewProject["Main_Division"] = "DATA BUREAU";
                            //else
                            //    drNewProject["Main_Division"] = "VOICE BUREAU";

                            drNewProject["Active"] = "Y";
                            drNewProject["LastModifiedBy"] = GV.sEmployeeName;
                            dtProjectMaster.Rows.Add(drNewProject);


                            dtDashboard = GV.MSSQL_RM.BAL_FetchTable("DASHBOARD", "PROJECT_ID = '" + sProjectID + "'");
                            if (dtDashboard.Rows.Count == 0)
                            {
                                DataRow drNewRow = dtDashboard.NewRow();
                                drNewRow["PROJECT_NAME"] = dtProjectMaster.Rows[0]["ProjectName"];
                                drNewRow["PROJECT_ID"] = dtProjectMaster.Rows[0]["ProjectID"];
                                drNewRow["ACTIVE"] = "Y";
                                drNewRow["CREATED_BY"] = GV.sEmployeeName;
                                drNewRow["CREATED_DATE"] = GM.GetDateTime();
                                dtDashboard.Rows.Add(drNewRow);
                            }
                            sErr = MandCheck();

                            if (sErr.Length == 0)
                            {
                                lstProjectControls = GetAllControls(panelstep0ProjectCreation);
                                foreach (Control ctrl in lstProjectControls)//Retrive Data from controls
                                {
                                    if (ctrl.Tag != null && dtDashboard.Columns.Contains(ctrl.Tag.ToString()))
                                    {
                                        if (dtDashboard.Columns[ctrl.Tag.ToString()].DataType.Name == "Int16" || dtDashboard.Columns[ctrl.Tag.ToString()].DataType.Name == "Int32" || dtDashboard.Columns[ctrl.Tag.ToString()].DataType.Name == "Int64" || dtDashboard.Columns[ctrl.Tag.ToString()].DataType.Name == "Decimal" || dtDashboard.Columns[ctrl.Tag.ToString()].DataType.Name == "DateTime")
                                        {
                                            if (ctrl.Text.Length > 0)
                                                dtDashboard.Rows[0][ctrl.Tag.ToString()] = ctrl.Text;
                                            else
                                                dtDashboard.Rows[0][ctrl.Tag.ToString()] = DBNull.Value;
                                        }
                                        else
                                            dtDashboard.Rows[0][ctrl.Tag.ToString()] = ctrl.Text;
                                    }
                                }

                                GV.MSSQL.BAL_SaveToTable(dtProjectMaster, "Timesheet.dbo.ProjectMaster", "New", true);
                                dtProjectMaster = GV.MSSQL.BAL_FetchTable("Timesheet.dbo.ProjectMaster", "ProjectID = '" + sProjectID + "'");


                                if(dtDashboard.GetChanges(DataRowState.Modified) != null)
                                    GV.MSSQL_RM.BAL_SaveToTable(dtDashboard, "DASHBOARD", "Update", true);
                                else if(dtDashboard.GetChanges(DataRowState.Added) != null)
                                    GV.MSSQL_RM.BAL_SaveToTable(dtDashboard, "DASHBOARD", "New", true);

                                dtDashboard = GV.MSSQL_RM.BAL_FetchTable("DASHBOARD", "PROJECT_ID = '" + GV.sProjectID + "'");

                                GV.sProjectID = sProjectID;
                                GV.sProjectName = txtProjectName.Text;

                                ToastNotification.Show(this, "New Project Created", eToastPosition.TopRight);

                                break;
                            }
                            else
                                return sErr;
                        }
                        else//Extream Situation // (Maybe never)
                            if (incr < 26)
                                sProjectNameStr = sProjectNameStr.Substring(0, sProjectNameStr.Length - 1) + sRandom[incr];
                            else
                                sProjectNameStr = sProjectNameStr.Substring(0, sProjectNameStr.Length - 2) + sRandom[incr] + sRandom[incr];
                    }
                }
                else if (btnNext.Text == "&Update Project")
                {
                    sErr = MandCheck();//Check if mandatory fields are avail

                    if (sErr.Length == 0)
                    {
                        foreach (Control ctrl in lstProjectControls)
                        {
                            if (ctrl.Tag != null && dtDashboard.Columns.Contains(ctrl.Tag.ToString()))
                            {
                                if (dtDashboard.Columns[ctrl.Tag.ToString()].DataType.Name == "Int16" || dtDashboard.Columns[ctrl.Tag.ToString()].DataType.Name == "Int32" || dtDashboard.Columns[ctrl.Tag.ToString()].DataType.Name == "Int64" || dtDashboard.Columns[ctrl.Tag.ToString()].DataType.Name == "Decimal")
                                {
                                    if (ctrl.Text.Length > 0)
                                        dtDashboard.Rows[0][ctrl.Tag.ToString()] = ctrl.Text;
                                    else
                                        dtDashboard.Rows[0][ctrl.Tag.ToString()] = DBNull.Value;
                                }
                                else if (dtDashboard.Columns[ctrl.Tag.ToString()].DataType.Name == "DateTime")
                                {
                                    if(ctrl.Text.Length > 0)
                                        dtDashboard.Rows[0][ctrl.Tag.ToString()] = Convert.ToDateTime(ctrl.Text);
                                }
                                else
                                    dtDashboard.Rows[0][ctrl.Tag.ToString()] = ctrl.Text;
                            }
                        }

                        if (dtDashboard.GetChanges(DataRowState.Modified) != null)
                        {

                            dtDashboard.Rows[0]["UPDATED_BY"] = GV.sEmployeeName;
                            dtDashboard.Rows[0]["UPDATED_DATE"] = GM.GetDateTime();

                            GV.MSSQL_RM.BAL_SaveToTable(dtDashboard.GetChanges(DataRowState.Modified), "DASHBOARD", "Update", true);
                            ToastNotification.Show(this, "Project Updated", eToastPosition.TopRight);
                        }
                    }
                    else
                        return sErr;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                sErr = ex.Message;
            }
            return sErr;
        }

        string MandCheck()
        {
            string sError = string.Empty;
            List<Control> lstMandControls = new List<Control>();
            lstMandControls.Add(txtClientname);
            lstMandControls.Add(txtProjectName);
            lstMandControls.Add(txtProject_For);
            lstMandControls.Add(txtProject_Type);
            lstMandControls.Add(txtProject_StartDate);
            lstMandControls.Add(txtProject_EndDate);
            lstMandControls.Add(txtGeography);
            lstMandControls.Add(txtVoulmns_Due_To_Client);

            foreach (Control C in lstMandControls)
            {
                if (C.Text.Trim().Length == 0)
                    sError += "<b>" + C.Tag.ToString().Replace("_", " ") + "</b> cannot be Empty<br/>";
            }

            return sError;
        }

        private void Next()
        {
            btnPrevious.Enabled = true;
            if(step0ProjectCreation.Enabled)
            {
                string sErr = SaveProjectDetails();
                if (sErr.Length == 0)
                {
                    btnNext.Text = "Next >";
                    step1ProjectSettings.Value = 100;
                    step1ProjectSettings.Enabled = true;
                    panelstep1ProjectSettings.Visible = true;
                    panelstep1ProjectSettings.Dock = DockStyle.Fill;

                    Project_Control_Load();

                    step0ProjectCreation.Value = 0;
                    step0ProjectCreation.Enabled = false;
                    panelstep0ProjectCreation.Visible = false;
                    panelstep0ProjectCreation.Dock = DockStyle.None;


                    step2FieldSettings.Value = 0;
                    step2FieldSettings.Enabled = false;
                    panelstep2FieldSettings.Visible = false;
                    panelstep2FieldSettings.Dock = DockStyle.None;

                    step3Validations.Value = 0;
                    step3Validations.Enabled = false;
                    panelstep3Validations.Visible = false;
                    panelstep3Validations.Dock = DockStyle.None;
                }
                else
                {
                    ToastNotification.DefaultToastGlowColor = eToastGlowColor.Red;
                    ToastNotification.DefaultTimeoutInterval = 5000;
                    ToastNotification.ToastFont = new Font(this.Font.FontFamily, 10);
                    
                    ToastNotification.Show(this, sErr, eToastPosition.TopRight);

                    ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
                    ToastNotification.DefaultTimeoutInterval = 2000;
                    ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
                }

            }
            else if (step1ProjectSettings.Enabled)//Settings to Fields
            {
                step2FieldSettings.Value = 100;
                step2FieldSettings.Enabled = true;
                panelstep2FieldSettings.Visible = true;
                panelstep2FieldSettings.Dock = DockStyle.Fill;

                step0ProjectCreation.Value = 0;
                step0ProjectCreation.Enabled = false;
                panelstep0ProjectCreation.Visible = false;
                panelstep0ProjectCreation.Dock = DockStyle.None;

                step1ProjectSettings.Value = 0;
                step1ProjectSettings.Enabled = false;
                panelstep1ProjectSettings.Visible = false;
                panelstep1ProjectSettings.Dock = DockStyle.None;

                step3Validations.Value = 0;
                step3Validations.Enabled = false;
                panelstep3Validations.Visible = false;
                panelstep3Validations.Dock = DockStyle.None;

                //step4DataImport.Value = 0;
                //step4DataImport.Enabled = false;

                //Populate_Settings(true);//Gets value from control to datatable
            }
            else if (step2FieldSettings.Enabled)//Fields to Validations
            {
                step3Validations.Value = 100;
                step3Validations.Enabled = true;
                panelstep3Validations.Visible = true;
                panelstep3Validations.Dock = DockStyle.Fill;

                step1ProjectSettings.Value = 0;
                step1ProjectSettings.Enabled = false;
                panelstep1ProjectSettings.Visible = false;
                panelstep1ProjectSettings.Dock = DockStyle.None;

                step2FieldSettings.Value = 0;
                step2FieldSettings.Enabled = false;
                panelstep2FieldSettings.Visible = false;
                panelstep2FieldSettings.Dock = DockStyle.None;

                step0ProjectCreation.Value = 0;
                step0ProjectCreation.Enabled = false;
                panelstep0ProjectCreation.Visible = false;
                panelstep0ProjectCreation.Dock = DockStyle.None;

                btnNext.Text = "Finish";

                //step4DataImport.Value = 0;
                //step4DataImport.Enabled = false;
                                
                
                //Populate_Settings(true);
            }
            else if (step3Validations.Enabled)//Validations to Finish
            {
                if (MessageBoxEx.Show("Are your to continue ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Populate_Settings(true);

                    if (dtProjectSettings.GetChanges(DataRowState.Modified) != null)
                        GV.MSSQL1.BAL_SaveToTable(dtProjectSettings.GetChanges(DataRowState.Modified), "c_project_settings", "Update", true);

                    string sCompanyQuery = Synch_GridnTable(sdgvCompany.PrimaryGrid, dtFieldCompany, "Master");
                    string sContactQuery = Synch_GridnTable(sdgvContact.PrimaryGrid, dtFieldContact, "MasterContacts");
                    bool IsAlterPassed = false;
                    
                    DataTable dCompanyCurrent = GV.MSSQL1.BAL_FetchTable(GV.sCompanyTable, "TR_AGENTNAME LIKE 'CURRENT%' OR WR_AGENTNAME LIKE 'CURRENT%'");
                    if (dCompanyCurrent.Rows.Count > 0)
                            ToastNotification.Show(this, "Cannot alter. Table still in use.", eToastPosition.TopRight);
                    else
                    {
                        #region Add New Field
                        try
                        {
                            if (sCompanyQuery.Trim().Length > 0)
                                GV.MSSQL1.BAL_ExecuteQuery(sCompanyQuery);

                            if (sContactQuery.Trim().Length > 0)
                                GV.MSSQL1.BAL_ExecuteQuery(sContactQuery);

                            IsAlterPassed = true;
                        }
                        catch (Exception e)
                        {
                            ToastNotification.Show(this, "Error occrued on altering Table..!", eToastPosition.TopRight);
                            IsAlterPassed = false;
                            return;
                        }                            
                        #endregion

                        #region Update Field Master
                        if (IsAlterPassed)
                        {
                            if (dtFieldCompany.GetChanges(DataRowState.Modified) != null)
                                GV.MSSQL1.BAL_SaveToTable(dtFieldCompany.GetChanges(DataRowState.Modified), "c_field_master", "Update", true);

                            if (dtFieldCompany.GetChanges(DataRowState.Added) != null)
                                GV.MSSQL1.BAL_SaveToTable(dtFieldCompany.GetChanges(DataRowState.Added), "c_field_master", "New", true);

                            if (dtFieldCompany.GetChanges(DataRowState.Deleted) != null)
                                GV.MSSQL1.BAL_SaveToTable(dtFieldCompany.GetChanges(DataRowState.Deleted), "c_field_master", "Delete", true);


                            if (dtFieldContact.GetChanges(DataRowState.Modified) != null)
                                GV.MSSQL1.BAL_SaveToTable(dtFieldContact.GetChanges(DataRowState.Modified), "c_field_master", "Update", true);

                            if (dtFieldContact.GetChanges(DataRowState.Added) != null)
                                GV.MSSQL1.BAL_SaveToTable(dtFieldContact.GetChanges(DataRowState.Added), "c_field_master", "New", true);

                            if (dtFieldContact.GetChanges(DataRowState.Deleted) != null)
                                GV.MSSQL1.BAL_SaveToTable(dtFieldContact.GetChanges(DataRowState.Deleted), "c_field_master", "Delete", true);

                            ToastNotification.Show(this, "Updated Sucessfully", eToastPosition.TopRight);
                            this.Close();
                        }  
                        #endregion                           
                    }                                      
                }                
            }
        }

        private void Previous()
        {
            btnNext.Text = "Next >";
            if (step0ProjectCreation.Enabled)
            {
                //possible navigation not available
            }
            else if (step1ProjectSettings.Enabled)//Settings to Fields
            {
                btnNext.Text = "Update Project";
                btnPrevious.Enabled = false;
                step0ProjectCreation.Value = 100;
                step0ProjectCreation.Enabled = true;
                panelstep0ProjectCreation.Visible = true;
                panelstep0ProjectCreation.Dock = DockStyle.Fill;

                step1ProjectSettings.Value = 0;
                step1ProjectSettings.Enabled = false;
                panelstep1ProjectSettings.Visible = false;
                panelstep1ProjectSettings.Dock = DockStyle.None;

                step2FieldSettings.Value = 0;
                step2FieldSettings.Enabled = false;
                panelstep2FieldSettings.Visible = false;
                panelstep2FieldSettings.Dock = DockStyle.None;

                step3Validations.Value = 0;
                step3Validations.Enabled = false;
                panelstep3Validations.Visible = false;
                panelstep3Validations.Dock = DockStyle.None;

                //step4DataImport.Value = 0;
                //step4DataImport.Enabled = false;

                Populate_Settings(true);//Gets value from control to datatable
            }
            else if (step2FieldSettings.Enabled)//Fields to Validations
            {
                step3Validations.Value = 0;
                step3Validations.Enabled = false;
                panelstep3Validations.Visible = false;
                panelstep3Validations.Dock = DockStyle.None;

                step1ProjectSettings.Value = 100;
                step1ProjectSettings.Enabled = true;
                panelstep1ProjectSettings.Visible = true;
                panelstep1ProjectSettings.Dock = DockStyle.Fill;

                step2FieldSettings.Value = 0;
                step2FieldSettings.Enabled = false;
                panelstep2FieldSettings.Visible = false;
                panelstep2FieldSettings.Dock = DockStyle.None;

                //step4DataImport.Value = 0;
                //step4DataImport.Enabled = false;


                //Populate_Settings(true);
            }
            else if (step3Validations.Enabled)//Validations to Data import
            {
                //step4DataImport.Value = 0;
                //step4DataImport.Enabled = false;

                step1ProjectSettings.Value = 0;
                step1ProjectSettings.Enabled = false;
                panelstep1ProjectSettings.Visible = false;
                panelstep1ProjectSettings.Dock = DockStyle.None;

                step2FieldSettings.Value = 100;
                step2FieldSettings.Enabled = true;
                panelstep2FieldSettings.Visible = true;
                panelstep2FieldSettings.Dock = DockStyle.Fill;

                step3Validations.Value = 0;
                step3Validations.Enabled = false;
                panelstep3Validations.Visible = false;
                panelstep3Validations.Dock = DockStyle.None;

                //btnNext.Text = "Finish";

                //Populate_Settings(true);
            }
            //else if (step4DataImport.Enabled)//Finish
            //{
            //    step3Validations.Value = 100;
            //    step3Validations.Enabled = true;
            //    panelstep3Validations.Visible = true;
            //    panelstep3Validations.Dock = DockStyle.Fill;

            //    step4DataImport.Value = 0;
            //    step4DataImport.Enabled = false;
            //}
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            Previous();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Next();
        }

        private string[] GetComboItems(DataTable dt, string sColumn)
        {
            DataTable dtControls = dt.DefaultView.ToTable(true, sColumn);
            string[] arrControls = new string[dtControls.Rows.Count];
            for (int i = 0; i < dtControls.Rows.Count; i++)
                arrControls[i] = dtControls.Rows[i][sColumn].ToString();
            return arrControls;   
        }


        private void sdgvCompany_KeyDown(object sender, KeyEventArgs e)
        {
            //ToastNotification.Show(this, e.KeyData.ToString());
            if (e.KeyData.ToString() == "F2")
            { 
                //sdgvCompany.PrimaryGrid
                GridPanel gp = sdgvCompany.PrimaryGrid;
                //gp.ActiveRow.Index;
                GridCell gc = (GridCell)gp.GetSelectedCells()[0];
                //gc.Value = "Prakash";
                Cell_Impact(gc);       
            }

            if (e.KeyData.ToString() == "F6")
            { 

            }
        }

        private void sdgvContact_KeyDown(object sender, KeyEventArgs e)
        {
            //ToastNotification.Show(this, e.KeyData.ToString());
            if (e.KeyData.ToString() == "F2")
            {
                //sdgvCompany.PrimaryGrid
                GridPanel gp = sdgvContact.PrimaryGrid;
                //gp.ActiveRow.Index;
                GridCell gc = (GridCell)gp.GetSelectedCells()[0];
                //gc.Value = "Prakash";
                Cell_Impact(gc);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show("Are you sure ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                this.Close();
        }

        private void NewGridRow(GridPanel GPanel)
        {
            List<string> lstFields = new List<string>();
            foreach (GridRow gr in GPanel.Rows)
                lstFields.Add(gr["FIELD_NAME_TABLE"].Value.ToString().ToUpper());

            frmNewField objfrmNewField = new frmNewField();
            objfrmNewField.lstFields = lstFields;
            if (objfrmNewField.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                GridRow gRow = new GridRow
                    (
                        true,//Active
                        string.Empty,//ControlType
                        objfrmNewField.txtFieldName.Text,//Caption
                        string.Empty,//Sequence
                        "",//Max Text Length
                        string.Empty,//Picklist category
                        string.Empty,//SelectionType
                        false,//Sortable
                        false,//Read-only
                        false,//SpellCheck
                        false,//Show on Grid
                        false,//Show on condition
                        "TR|WR",//Visibility
                        string.Empty,//Formatting
                        objfrmNewField.cmbFieldType.Text, //ColumnStatus = Datatype of new column--Used on creating columns
                        "New",//ID
                        objfrmNewField.txtFieldName.Text.Replace(" ","_")//Field Name
                        );
                GPanel.Rows.Add(gRow);   
            }
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            if(TabTables.SelectedPanel.Name == "tabPanelCompany")
                NewGridRow(sdgvCompany.PrimaryGrid);
            else if (TabTables.SelectedPanel.Name == "tabPanelContact")
                NewGridRow(sdgvContact.PrimaryGrid);
        }

        private void btnPickList_Click(object sender, EventArgs e)
        {
            frmPickList objfrmPickList = new frmPickList();
            if (objfrmPickList.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                dtPickList = GV.MSSQL1.BAL_FetchTable(GV.sProjectID + "_picklists", "1=1");
                ToastNotification.Show(this, "Selection List Updated");
            }
        }

        private void frmProjectControl_Resize(object sender, EventArgs e)
        {
            ProjectEditProgressSteps.Width = this.Width;
            step1ProjectSettings.MinimumSize = new Size((this.Width / 3) + 4, 30);
            step2FieldSettings.MinimumSize = new Size((this.Width / 3) + 4, 30);
            step3Validations.MinimumSize = new Size((this.Width / 3) + 4, 30);
           // step4DataImport.MinimumSize = new Size((this.Width / 4) + 4, 30);
        }

        private void btnClone_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBoxEx.Show("Are you sure to clone this project ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (cmbProjectsSelect.Text.Length > 0 && GV.sProjectID.Length > 0 &&
                        dtClonableProjects.Select("ProjectName = '" + cmbProjectsSelect.Text.Replace("'", "") + "'")
                            .Length > 0)
                    {
                        SqlConnection connection = new SqlConnection(GV.sMSSQL1);
                        //MyjSqlDataAdapter da = new MyhSqlDataAdapter();
                        SqlCommand cmd = new SqlCommand("USP_Project_Tables_Creations", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("PROJECT_ID", SqlDbType.VarChar).Value = GV.sProjectID;
                        cmd.Parameters.AddWithValue("DEFAULTPROJECTID", SqlDbType.VarChar).Value =
                            dtClonableProjects.Select("ProjectName = '" + cmbProjectsSelect.Text.Replace("'", "") + "'")
                                [0]["ProjectID"].ToString(); // cmbProjectsSelect.Text;
                        cmd.Parameters.AddWithValue("NEW_PROJECTNAME", SqlDbType.VarChar).Value = GV.sProjectName;
                        if (connection.State != ConnectionState.Open)
                            connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        ToastNotification.Show(this, "Project cloned sucessfully", eToastPosition.TopRight);
                        IsNewProject = false;
                        Project_Control_Load();
                    }
                    else
                        ToastNotification.Show(this, "Invalid project selection.");
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCreateNewPronect_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBoxEx.Show("Are you sure to create new project ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (cmbProjectsSelect.Text.Length > 0 && GV.sProjectID.Length > 0)
                    {
                        SqlConnection connection = new SqlConnection(GV.sMSSQL1);
                        //MyjhSqlDataAdapter da = new MySjhqlDataAdapter();
                        SqlCommand cmd = new SqlCommand("USP_Project_Tables_Creations", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("PROJECT_ID", SqlDbType.VarChar).Value = GV.sProjectID;
                        cmd.Parameters.AddWithValue("DEFAULTPROJECTID", SqlDbType.VarChar).Value = "CRUCRU005";
                        cmd.Parameters.AddWithValue("NEW_PROJECTNAME", SqlDbType.VarChar).Value = GV.sProjectName;
                        if (connection.State != ConnectionState.Open)
                            connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        ToastNotification.Show(this, "New Project created sucessfully", eToastPosition.TopRight);
                        IsNewProject = false;
                        Project_Control_Load();
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btn_Click(object sender, EventArgs e)//Runtime event.. //Button Click for 'Customlist' controls which opens the Custom designed combobox
        { 
            TextBox txt = sender as TextBox;//Gets which Textbox button triggers the event
            if (txt != null && txt.Tag != null)
            {
                frmComboList objFrmComboList = new frmComboList(); //Custom Designed Combobox replacement
                objFrmComboList.TitleText = txt.Tag.ToString().Replace("_"," ");
                objFrmComboList.IsMultiSelect = false;
                objFrmComboList.IsSingleWordSelection = false;

                switch (txt.Tag.ToString())
                { 
                    case "Clientname":
                        objFrmComboList.lstColumnsToDisplay.Add("Clientname");
                        objFrmComboList.sColumnToSearch = "Clientname";
                        objFrmComboList.dtItems = GV.MSSQL.BAL_ExecuteQuery("SELECT DISTINCT Clientname FROM Timesheet.dbo.ProjectMaster WHERE Active ='Y' AND Department IN ('FTE','Data','GCC')");
                        break;

                    case "Project_For":
                    case "Project_Type":
                    case "Project_Status":
                    case "Team_Type":
                    case "Client_Brief":
                    case "Source_Data":
                    case "Sales_Owner":
                    case "Probability":
                        objFrmComboList.lstColumnsToDisplay.Add("Data");
                        objFrmComboList.sColumnToSearch = "Data";
                        objFrmComboList.dtItems = GV.MSSQL.BAL_ExecuteQuery("SELECT Data FROM Timesheet.dbo.PickLists WHERE Field='" + txt.Tag + "' AND ProjectType='C'");
                        break;

                    case "Geography":
                        objFrmComboList.lstColumnsToDisplay.Add("Region");
                        objFrmComboList.sColumnToSearch = "Region";
                        objFrmComboList.dtItems = GV.MSSQL1.BAL_ExecuteQuery("SELECT 'Global' AS Region UNION (SELECT TOP 1000 DISTINCT Region FROM c_country WHERE Region IS NOT NULL ORDER BY Region) UNION (SELECT DISTINCT CountryName FROM c_country);");
                        objFrmComboList.IsMultiSelect = true;
                        break;

                    case "WR_Owner":
                    case "TR_Owner":
                        objFrmComboList.lstColumnsToDisplay.Add("TeamName");
                        objFrmComboList.sColumnToSearch = "TeamName";
                        objFrmComboList.dtItems = GV.MSSQL.BAL_ExecuteQuery("SELECT DISTINCT TeamName FROM Timesheet.dbo.Users WHERE Department IN ('FTE','GCC','Data')AND Active='Y'");
                        objFrmComboList.IsMultiSelect = true;
                        break;

                        case "BST_Owner":
                        objFrmComboList.lstColumnsToDisplay.Add("UserName");
                        objFrmComboList.sColumnToSearch = "UserName";
                        objFrmComboList.dtItems = GV.MSSQL.BAL_ExecuteQuery("SELECT UserName FROM Timesheet.dbo.users WHERE Division = 'BUSINESS DEVELOPMENT' AND Active='Y'");
                        objFrmComboList.IsMultiSelect = true;
                        break;
                }

                objFrmComboList.ShowDialog(this);

                if (!string.IsNullOrEmpty(objFrmComboList.sReturn))
                {
                    if (txt.Tag.ToString() == "WR_Owner" || txt.Tag.ToString() == "TR_Owner")
                        txt.Text = objFrmComboList.sReturn.Replace("|"," / ");
                    else
                        txt.Text = objFrmComboList.sReturn;
                }

            }
        }

        private void frmProjectControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (DialogResult.Yes != MessageBoxEx.Show("Are you sure to close this window ?" + Environment.NewLine + "Changes will not be saved!", "Campaign Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    e.Cancel = true;
            }
        }

        private void txtCallScriptPath_ButtonCustomClick(object sender, EventArgs e)
        {
            openDialog.Filter = "Rich Text Format|*.rtf";
            if (DialogResult.OK == openDialog.ShowDialog())
            {
                txtCallScriptPath.Text = openDialog.FileName;
                BlobAcess(openDialog.FileName, "CallScript");
            }
        }

        private void txtEAFLibraryPath_ButtonCustomClick(object sender, EventArgs e)
        {
            openDialog.Filter = "EAF Library|*.dll";
            if (DialogResult.OK == openDialog.ShowDialog())
            {
                txtEAFLibraryPath.Text = openDialog.FileName;
                BlobAcess(openDialog.FileName, "EAF");
            }
        }


        private void BlobAcess(string sPath, string sFileType)
        {
            try
            {
                if (File.Exists(sPath) && GV.sProjectID.Length > 0)
                {
                    DataTable dtBlob = GV.MSSQL_RM.BAL_ExecuteQuery("SELECT * FROM RM..PROJECT_FILES WHERE ProjectID = '"+GV.sProjectID+"' AND FileType ='"+sFileType+"'");
                    string sSQLText = string.Empty;
                    byte[] bStream = File.ReadAllBytes(sPath);
                    string sExtension = Path.GetExtension(sPath).Replace(".", string.Empty);
                    string sFileName = Path.GetFileNameWithoutExtension(sPath);

                    if ((sExtension.ToUpper() == "DLL" && sFileType == "EAF") || (sExtension.ToUpper() == "RTF" && sFileType == "CallScript") || (sExtension.ToUpper() == "EXE" && sFileType == "BOT"))
                    {
                        if (bStream.Length > 0)
                        {
                            if (dtBlob.Rows.Count > 0)
                                sSQLText = "UPDATE PROJECT_FILES SET Blob = @Binary WHERE ProjectID='" + GV.sProjectID + "' AND FileType='" + sFileType + "'";
                            else
                                sSQLText = "INSERT INTO PROJECT_FILES( ProjectID, FileName, FileType,Extension,Blob ) VALUES( '" + GV.sProjectID + "','" + sFileName + "','" + sFileType + "','" + sExtension + "',@Binary )";

                            if (DialogResult.Yes == MessageBoxEx.Show("Are you sure to update " + sFileType + " ?", "Campaign Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                            {
                                SqlConnection connection = new SqlConnection(GV.sMSSQL_RM);
                                SqlCommand command = new SqlCommand(sSQLText, connection);
                                if (connection.State != ConnectionState.Open)
                                    connection.Open();
                                command.Parameters.AddWithValue("@Binary", bStream);
                                command.ExecuteNonQuery();
                                connection.Close();
                                ToastNotification.Show(this, sFileType + " updated sucessfully.", eToastPosition.TopRight);
                            }
                        }
                        else
                            ToastNotification.Show(this, "Invalid File!!!", eToastPosition.TopRight);
                    }
                    else
                        ToastNotification.Show(this, "Invalid File!!!", eToastPosition.TopRight);
                }
                else                
                    ToastNotification.Show(this,"File not Found!!!",eToastPosition.TopRight);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtScrapePath_ButtonCustomClick(object sender, EventArgs e)
        {
            openDialog.Filter = "Scrapper|*.exe";
            if (DialogResult.OK == openDialog.ShowDialog())
            {
                txtScrapePath.Text = openDialog.FileName;
                BlobAcess(openDialog.FileName, "BOT");
            }
        }

        private void cmbProjectsSelect_ButtonCustomClick(object sender, EventArgs e)
        {
            frmComboList objFrmComboList = new frmComboList();
                    objFrmComboList.TitleText = "Select Project to Clone";
                    objFrmComboList.dtItems = dtClonableProjects;
                    objFrmComboList.lstColumnsToDisplay.Add("ProjectName");
                    objFrmComboList.sColumnToSearch = "ProjectName";
                    objFrmComboList.IsSpellCheckEnabeld = false;
                    objFrmComboList.IsMultiSelect = false;
                    objFrmComboList.IsSingleWordSelection = true;
                    objFrmComboList.StartPosition = FormStartPosition.CenterScreen;
                    //objFrmComboList.sColumnToReturn = "";
                    objFrmComboList.MinimizeBox = false;
                    objFrmComboList.MaximizeBox = false;
                    objFrmComboList.ShowDialog(this);
            if (objFrmComboList.sReturn != null && objFrmComboList.sReturn.Length > 0)
            {
                cmbProjectsSelect.Text = objFrmComboList.sReturn.Trim();
            }
            else
                cmbProjectsSelect.Text = string.Empty;
        }

       
    }

    //pCompanyPanel.Columns["CONTROL_TYPE"].EditorType = typeof(FragrantComboBox);

    internal class FragrantComboBox : GridComboBoxExEditControl//Combobox on Grid
    {
        public FragrantComboBox(IEnumerable orderArray)
        {
            DataSource = orderArray;
        }
    }    
}
