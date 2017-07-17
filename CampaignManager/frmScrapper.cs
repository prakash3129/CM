using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using Microsoft.VisualBasic.FileIO;

namespace GCC
{
    public partial class frmScrapper : Office2007Form
    {
        public frmScrapper()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(Font.FontFamily, 22);

            ExpanelQueue.Parent = this;
            sdgvArchive.PrimaryGrid.Columns.Clear();
            GridColumn ColStatus = new GridColumn("Action");
            ColStatus.DataPropertyName = "Action";
       
            ColStatus.FillWeight = 20;            
            //ColStatus.EditorType = typeof(GridStateEditControl);
            sdgvArchive.PrimaryGrid.Columns.Add(ColStatus);
            lblCurrentCompany.Text = string.Empty;
            lblCount.Text = string.Empty;
        }

        private frmMain _ParantForm;

        //-----------------------------------------------------------------------------------------------------
        public frmMain ParantForm /////List of Columns to display on search window/////
        {
            get { return _ParantForm; }
            set { _ParantForm = value; }
        }  

        private DataTable _dtScrapperSettings;
        public DataTable dtScrapperSettings /////
        {
            get { return _dtScrapperSettings; }
            set { _dtScrapperSettings = value; }
        }

        private DataTable _dtCountry;
        public DataTable dtCountry /////
        {
            get { return _dtCountry; }
            set { _dtCountry = value; }
        }

        List<string> lstPendingCols  = new List<string>();
        List<string> lstPassedCols = new List<string>();
        List<string> lstInvalidCols = new List<string>();
        List<string> lstDuplicateCols = new List<string>();


        List<string> lstPendingStatus = new List<string>();
        List<string> lstPassedStatus = new List<string>();
        List<string> lstInvalidStatus = new List<string>();

        List<string> lstMadotoryImportColumns = new List<string>();

        private DataTable dtExistingCompaniesDupes = new DataTable();
        public DataTable dtImportedCompanies = new DataTable();
        public DataTable dtArchived = new DataTable();
        public DataTable dtQueue = new DataTable();
        string sArchiveSearchQuery = string.Empty;
        string sQueueSearchQuery = string.Empty;
        string sDateColumn = GV.sAccessTo == "TR" ? "TR_DATECALLED" : "WR_DATE_OF_PROCESS";
        private int iQueueCount = 0;

        private void frmCompanyImport_Load(object sender, EventArgs e)
        {
            //GridColumn column = sdgvPending.PrimaryGrid.Columns["Status"];
            //column.DataPropertyName = "Status";
            //column.EditorType = typeof(GridStateEditControl);

            lstPendingCols = Customize_sdgvColumns(sdgvPending.PrimaryGrid, dtScrapperSettings.Rows[0]["DisplayCols_Pending"].ToString());
            lstPassedCols = Customize_sdgvColumns(sdgvReady.PrimaryGrid, dtScrapperSettings.Rows[0]["DisplayCols_Passed"].ToString());
            lstInvalidCols = Customize_sdgvColumns(sdgvInvalid.PrimaryGrid, dtScrapperSettings.Rows[0]["DisplayCols_Invalid"].ToString());
            lstDuplicateCols = Customize_sdgvColumns(sdgvDuplicate.PrimaryGrid, dtScrapperSettings.Rows[0]["DisplayCols_Duplicate"].ToString());

            lstMadotoryImportColumns = dtScrapperSettings.Rows[0]["Mandatory_InColumns"].ToString().Split('|').ToList();

            if(!lstMadotoryImportColumns.Contains("COMPANY_NAME", StringComparer.OrdinalIgnoreCase))
                lstMadotoryImportColumns.Add("COMPANY_NAME");

            if (!lstMadotoryImportColumns.Contains("COUNTRY", StringComparer.OrdinalIgnoreCase))
                lstMadotoryImportColumns.Add("COUNTRY");

            dtExistingCompaniesDupes.Columns.Add("Action");
            dtExistingCompaniesDupes.Columns.Add("COMPANY_ID");;
            dtExistingCompaniesDupes.Columns.Add("UserStatus");            

            if (!lstDuplicateCols.Contains("MASTER_ID", StringComparer.OrdinalIgnoreCase))
                dtExistingCompaniesDupes.Columns.Add("MASTER_ID");
            
            foreach (string sColumnName in lstDuplicateCols)
                dtExistingCompaniesDupes.Columns.Add(sColumnName);
            

            lstPendingStatus = dtScrapperSettings.Rows[0]["StatusList_Pending"].ToString().Split('|').ToList();
            lstPassedStatus = dtScrapperSettings.Rows[0]["StatusList_Passed"].ToString().Split('|').ToList();
            lstInvalidStatus = dtScrapperSettings.Rows[0]["StatusList_Invalid"].ToString().Split('|').ToList();            
            
            //GridRow row = new GridRow("2,4");
            //sdgvPending.PrimaryGrid.Rows.Add(row);


            ReloadTables();

            ReloadGrid();

            

            //sdgvPending.PrimaryGrid.DataSource = dt;
            //sdgvPending.PrimaryGrid.Columns["Status"].Width = 20;
            //sdgvPending.PrimaryGrid.Columns["Status"].FillWeight = 20;

        }

        public void ReloadTables()
        {
            try
            {
                dtExistingCompaniesDupes.Rows.Clear();
                dtImportedCompanies.Rows.Clear();
                dtImportedCompanies.Columns.Clear();
                dtArchived.Rows.Clear();
                dtArchived.Columns.Clear();
                dtQueue.Rows.Clear();
                dtQueue.Columns.Clear();


                dtImportedCompanies =
                    GV.MSSQL1.BAL_ExecuteQuery("SELECT * FROM c_mastercompanies WHERE CREATED_BY = '" +
                                                   GV.sEmployeeName +
                                                   "' AND PROJECTID = '" + GV.sProjectID +
                                                   "' AND (RECORD_STATUS='IMPORTPROGRESS' OR (RECORD_STATUS = 'REJECTED' AND CAST(CREATED_DATE AS DATE) = cast(GETDATE() as date)));");
                dtImportedCompanies.Columns.Add("Action");
                dtImportedCompanies.Columns.Add("UserStatus");

                dtArchived =
                    GV.MSSQL1.BAL_ExecuteQuery(
                        "SELECT COMPANY_ID,COMPANY_NAME,ADDRESS_1,ADDRESS_2 FROM c_mastercompanies WHERE CREATED_BY = '" +
                        GV.sEmployeeName +
                        "' AND PROJECTID = '" + GV.sProjectID +
                        "' AND RECORD_STATUS = 'ARCHIVED'");
                dtArchived.Columns.Add("Action");
                foreach (DataRow drArchive in dtArchived.Rows)
                    drArchive["Action"] = "5";

                dtQueue =
                    GV.MSSQL1.BAL_ExecuteQuery(
                        "SELECT MASTER_ID, COMPANY_NAME,ADDRESS_1,ADDRESS_2,city,POST_CODE,country FROM " +
                        GV.sProjectID +
                        "_mastercompanies WHERE Scrape_Status=1 AND " + GV.sAccessTo + "_AGENTNAME = '" +
                        GV.sEmployeeName +
                        "';");

                foreach (DataRow drImportedCompanies in dtImportedCompanies.Rows)
                {
                    string sDupeCompanies = drImportedCompanies["DUPE_COMPANIES"].ToString().Trim();
                    if (sDupeCompanies.Length > 0)
                    {
                        List<string> lstCompanies = sDupeCompanies.Split('|').ToList();
                        foreach (string sCompanies in lstCompanies)
                        {
                            DataRow drNewRow = dtExistingCompaniesDupes.NewRow();
                            drNewRow["Action"] = "2";
                            drNewRow["COMPANY_ID"] = drImportedCompanies["COMPANY_ID"].ToString();
                            drNewRow["UserStatus"] = "";
                            //drNewRow["COMPANY_STATUS"] = drImportedCompanies["COMPANY_STATUS"].ToString();
                            List<string> lstColValues = sCompanies.Split('~').ToList();
                            for (int i = 0; i < lstDuplicateCols.Count; i++)
                                drNewRow[lstDuplicateCols[i]] = lstColValues[i];

                            dtExistingCompaniesDupes.Rows.Add(drNewRow);
                        }
                    }
                }


                sArchiveSearchQuery = FullTextQuery(dtArchived);
                sQueueSearchQuery = FullTextQuery(dtQueue);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        string FullTextQuery(DataTable dtSearchTable)
        {
            int colCount = dtSearchTable.Columns.Count;
            string sSearchQuery = string.Empty;
            string likeStatement = "Like '%{0}%'";
            for (int i = 0; i < colCount; i++)
            {
                string colName = dtSearchTable.Columns[i].ColumnName;
                sSearchQuery += string.Concat("Convert(", colName, ", 'System.String')", likeStatement);

                if (i != colCount - 1)
                    sSearchQuery += " OR ";
            }
            return sSearchQuery;
        }

        public void ReloadGrid()
        {
            try
            {
                sdgvArchive.PrimaryGrid.DataSource = dtArchived;
                sdgvPending.PrimaryGrid.Rows.Clear();
                sdgvDuplicate.PrimaryGrid.Rows.Clear();
                sdgvInvalid.PrimaryGrid.Rows.Clear();
                sdgvReady.PrimaryGrid.Rows.Clear();
                sdgvQueue.PrimaryGrid.Rows.Clear();

                sdgvQueue.PrimaryGrid.DataSource = dtQueue;
                sdgvQueue.PrimaryGrid.Caption.Text = "Total companies in queue :" + dtQueue.Rows.Count;

                ExpanelQueue.TitleText = "Queue ("+ dtQueue.Rows.Count +")";
                foreach (DataRow drImportedCompanies in dtImportedCompanies.Rows)
                {
                    if (drImportedCompanies["UserStatus"].ToString().Length > 0)
                    {
                        if (drImportedCompanies["UserStatus"].ToString() == "Pending")
                        {
                            drImportedCompanies["Action"] = "2,4";
                            AddGridRow(sdgvPending.PrimaryGrid, drImportedCompanies);
                        }
                        else if (drImportedCompanies["UserStatus"].ToString() == "Passed")
                        {
                            drImportedCompanies["Action"] = "3,4";
                            AddGridRow(sdgvReady.PrimaryGrid, drImportedCompanies);
                        }
                        else if (drImportedCompanies["UserStatus"].ToString() == "Invalid")
                        {
                            drImportedCompanies["Action"] = "2,3";
                            AddGridRow(sdgvInvalid.PrimaryGrid, drImportedCompanies);
                        }
                    }
                    else
                    {
                        if (lstPendingStatus.Contains(drImportedCompanies["COMPANY_STATUS"].ToString(),
                            StringComparer.OrdinalIgnoreCase))
                        {
                            drImportedCompanies["Action"] = "2,4";
                            AddGridRow(sdgvPending.PrimaryGrid, drImportedCompanies);
                        }
                        else if (lstPassedStatus.Contains(drImportedCompanies["COMPANY_STATUS"].ToString(),
                            StringComparer.OrdinalIgnoreCase))
                        {
                            drImportedCompanies["Action"] = "3,4";
                            AddGridRow(sdgvReady.PrimaryGrid, drImportedCompanies);
                        }
                        else if (lstInvalidStatus.Contains(drImportedCompanies["COMPANY_STATUS"].ToString(),
                            StringComparer.OrdinalIgnoreCase))
                        {
                            drImportedCompanies["Action"] = null;
                            AddGridRow(sdgvInvalid.PrimaryGrid, drImportedCompanies);
                        }
                    }
                }

                if (sdgvPending.PrimaryGrid.Rows.Count > 0)
                {
                    GridRow GR = sdgvPending.PrimaryGrid.Rows[0] as GridRow;
                    sdgvPending.PrimaryGrid.ClearSelectedRows();
                    //sdgvPending.PrimaryGrid.Rows.Add(GR);
                    sdgvPending.PrimaryGrid.SetActiveRow(GR);
                    sdgvPending.PrimaryGrid.Select(GR);
                    sdgvPending.PrimaryGrid.SetSelectedRows(0, 1, true);
                    GR.IsSelected = true;
                    GR.SetActive(true);
                }

                DataRow[] drrExisting = dtExistingCompaniesDupes.Select("UserStatus = 'ExistingPassed'");
                if (drrExisting.Length > 0)
                {
                    foreach (DataRow drExisting in drrExisting)
                    {
                        drExisting["Action"] = "4";
                        AddGridRow(sdgvReady.PrimaryGrid, drExisting);
                    }
                }

                sdgvPending.PrimaryGrid.Caption.Text = "Citation Needed (" + sdgvPending.PrimaryGrid.Rows.Count + ")";
                sdgvReady.PrimaryGrid.Caption.Text = "Ready to go (" + sdgvReady.PrimaryGrid.Rows.Count + ")";
                sdgvInvalid.PrimaryGrid.Caption.Text = "Not fit (" + sdgvInvalid.PrimaryGrid.Rows.Count + ")";
                sdgvArchive.PrimaryGrid.Caption.Text = "Archived Companies (" + dtArchived.Rows.Count + ") Selected : " +
                                                       dtArchived.Select("Action = '1'").Length;

                ExpanelArchive.TitleText = "Archive (" + dtArchived.Rows.Count + ")";

            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        public static void Action(GridPanel GP, int iActionIndex, int iRowIndex)
        {
            try
            {
                frmScrapper CurrentForm = GetCurrentCompaniesTable();
                if (CurrentForm != null && GP.Rows.Count > 0)
                {
                    GridRow GR = GP.Rows[iRowIndex] as GridRow;
                    if (GR != null && GR.Cells["COMPANY_ID"].Value != null)
                    {
                        string ID = GR.Cells["COMPANY_ID"].Value.ToString();
                        DataTable dtCurrentImportedCompanies = CurrentForm.dtImportedCompanies;
                        DataTable dtCurrentDupeCompanies = CurrentForm.dtExistingCompaniesDupes;
                        if (GP.SuperGrid.Name == "sdgvDuplicate")
                        {
                            string sExisting_MasterID = GR.Cells["MASTER_ID"].Value.ToString();
                            if (iActionIndex == 2)
                            {
                                DataRow drCurrentDupeCompanies =
                                    dtCurrentDupeCompanies.Select("COMPANY_ID = '" + ID + "' AND MASTER_ID = '" +
                                                                  sExisting_MasterID + "'")[0];
                                drCurrentDupeCompanies["UserStatus"] = "ExistingPassed";
                                drCurrentDupeCompanies["Action"] = "4";

                                //dtCurrentDupeCompanies.Select("COMPANY_ID = '" + ID + "' AND MASTER_ID = '" +
                                //                              sExisting_MasterID + "'")[0]["UserStatus"] = "ExistingPassed";
                                dtCurrentImportedCompanies.Select("COMPANY_ID = '" + ID + "'")[0]["UserStatus"] =
                                    "Invalid";
                            }
                            else if (iActionIndex == 4)
                            {
                                dtCurrentDupeCompanies.Select("COMPANY_ID = '" + ID + "' AND MASTER_ID = '" +
                                                              sExisting_MasterID + "'")[0]["UserStatus"] = "Invalid";
                            }
                            CurrentForm.ReloadGrid();
                        }
                        else if (GP.SuperGrid.Name == "sdgvArchive")
                        {
                            if (dtCurrentImportedCompanies.Select("RECORD_STATUS = 'IMPORTPROGRESS'").Length > 0)
                            {
                                ToastNotification.Show(CurrentForm,
                                    "Records still pending to process.<br/>These records has to be processed or cleared to import new.",
                                    eToastPosition.TopRight);
                                return;
                            }

                            DataTable dtCurrentArchive = CurrentForm.dtArchived;
                            if (GR.Cells["Action"].Value.ToString() == "5" &&
                                dtCurrentArchive.Select("Action = '1'").Length >= 30)
                            {
                                ToastNotification.Show(CurrentForm, "Maximum selection reached.",
                                    eToastPosition.TopRight);
                                return;
                            }

                            GR.Cells["Action"].Value = GR.Cells["Action"].Value.ToString() == "5" ? "1" : "5";
                            dtCurrentArchive.Select("COMPANY_ID = '" + ID + "'")[0]["Action"] =
                                GR.Cells["Action"].Value.ToString();

                            GP.Caption.Text = "Archived Companies (" + CurrentForm.dtArchived.Rows.Count +
                                              ") Selected : " + CurrentForm.dtArchived.Select("Action = '1'").Length;
                        }
                        else
                        {
                            if (GR.Cells["UserStatus"].Value.ToString() == "ExistingPassed")
                            {
                                string sExisting_MasterID = GR.Cells["MASTER_ID"].Value.ToString();

                                //DataRow[] drrCurrentDupeCompanies = dtCurrentDupeCompanies.Select("MASTER_ID = '" + sExisting_MasterID + "'");
                                //foreach (DataRow drCurrentDupeCompanies in drrCurrentDupeCompanies)
                                //{
                                //    drCurrentDupeCompanies["UserStatus"] = "Invalid";
                                //    drCurrentDupeCompanies["Action"] = "2";
                                //}

                                DataRow drExisting =
                                    dtCurrentDupeCompanies.Select("COMPANY_ID = '" + ID + "' AND MASTER_ID = '" +
                                                                  sExisting_MasterID + "'")[0];
                                drExisting["UserStatus"] = "Invalid";
                                drExisting["Action"] = "2";
                            }
                            else
                            {
                                string sUserStatus = string.Empty;
                                if (iActionIndex == 2)
                                    sUserStatus = "Passed";
                                else if (iActionIndex == 4)
                                    sUserStatus = "Invalid";
                                else if (iActionIndex == 3)
                                    sUserStatus = "Pending";
                                dtCurrentImportedCompanies.Select("COMPANY_ID = '" + ID + "'")[0]["UserStatus"] =
                                    sUserStatus;
                            }
                            CurrentForm.ReloadGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        static frmScrapper  GetCurrentCompaniesTable()
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == "frmScrapper")
                {
                    return (frmScrapper)f;
                }
            }
            return null;
        }

        void AddGridRow(GridPanel GP, DataRow dr)
        {

            try
            {
                List<string> Val = new List<string>();
                foreach (GridColumn GC in GP.Columns)
                {
                    if (dr.Table.Columns.Contains(GC.Name))
                        Val.Add(dr[GC.Name].ToString());
                    else
                        Val.Add(string.Empty);
                }
                GridRow GR = new GridRow(Val);

                //string sValue = string.Empty;
                //foreach (DataColumn DC in dr.Table.Columns)
                //{
                //    if (lstPassedCols.Contains(DC.ColumnName))
                //    {
                //        GR.Cells[DC.ColumnName].Value = dr[DC.ColumnName].ToString();
                //        //GridCell GCell = new GridCell();
                //        //GCell.GridColumn.DataPropertyName = DC.ColumnName;
                //        //GCell.Value = dr[DC.ColumnName].ToString();
                //        //GR.Cells.Add(GCell);

                //        sValue += "\"" + dr[DC.ColumnName] + "\"";
                //    }
                //}
                GP.Rows.Add(GR);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }

        }

        List<string> Customize_sdgvColumns(GridPanel GP, string sColumns)
        {
            List<string> ReturnColList = new List<string>();
            try
            {
                GP.Columns.Clear();
                GridColumn ColStatus = new GridColumn("Action");
                ColStatus.DataPropertyName = "Action";
                ColStatus.FillWeight = 30;
                ColStatus.MinimumWidth = 50;
                //ColStatus.EditorType = typeof (GridStateEditControl);
                GP.Columns.Add(ColStatus);
                GridColumn ColID = new GridColumn("COMPANY_ID");
                ColID.DataPropertyName = "COMPANY_ID";
                ColID.ReadOnly = true;
                
                ColID.Visible = false;
                
                GP.Columns.Add(ColID);

                GridColumn ColUserStatus = new GridColumn("UserStatus");
                ColUserStatus.DataPropertyName = "UserStatus";
                ColUserStatus.ReadOnly = true;
                ColUserStatus.Visible = false;
                ColUserStatus.FillWeight = 150;
                GP.Columns.Add(ColUserStatus);

                //if (GP.SuperGrid.Name == "sdgvDuplicate")
                {
                    GridColumn ColExistingID = new GridColumn("MASTER_ID");
                    ColExistingID.DataPropertyName = "MASTER_ID";
                    ColExistingID.ReadOnly = true;
                    ColExistingID.Visible = false;
                    GP.Columns.Add(ColExistingID);
                }

                
                List<string> lstCols = sColumns.Split('|').ToList();
                foreach (string sCols in lstCols)
                {
                    string sColumnName = sCols.Split('~')[0].Trim();
                    if (sColumnName.Length > 0)
                    {
                        ReturnColList.Add(sColumnName);
                        GridColumn Column = new GridColumn(sColumnName);
                        Column.DataPropertyName = sColumnName;
                        if (sCols.Contains("~") && sCols.Split('~')[1].Trim().Length > 0)
                            Column.HeaderText = sCols.Split('~')[1].Trim();
                        else
                            Column.Visible = false;
                        Column.EditorType = typeof (GridLabelEditControl);
                        GP.Columns.Add(Column);
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
            return ReturnColList;
        }       

        private void sdgvPending_RowActivated(object sender, GridRowActivatedEventArgs e)
        {
            try
            {
                sdgvDuplicate.PrimaryGrid.Rows.Clear();
                GridRow GR = e.NewActiveRow as GridRow;
                if (GR != null)
                {
                    string sCompanyID = GR.Cells["COMPANY_ID"].Value.ToString();

                    DataRow[] drrExistingCompanies =
                        dtExistingCompaniesDupes.Select("COMPANY_ID = '" + sCompanyID +
                                                        "' AND UserStatus <> 'ExistingPassed'");
                    if (drrExistingCompanies.Length > 0)
                        sdgvDuplicate.PrimaryGrid.DataSource = drrExistingCompanies.CopyToDataTable();

                    //string sDupeCompanies =
                    //    dtImportedCompanies.Select("COMPANY_ID = " + sCompanyID)[0]["DUPE_COMPANIES"].ToString();
                    //List<string> lstDupeCompanies = sDupeCompanies.Split('|').ToList();
                    //foreach (string sDupeComs in lstDupeCompanies)
                    //{
                    //    List<string> lstColValues = new List<string>();
                    //    lstColValues.Add("2");
                    //    lstColValues.Add(sCompanyID);
                    //    lstColValues.Add("");
                    //    lstColValues.Add("");
                    //    lstColValues.AddRange(sDupeComs.Split('~').ToList());
                    //    GridRow GRNewRow = new GridRow(lstColValues);
                    //    sdgvDuplicate.PrimaryGrid.Rows.Add(GRNewRow);
                    //}
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        private void txtImportFile_ButtonCustomClick(object sender, EventArgs e)
        {
            
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> lstExistingIDsToScrape = new List<int>();
                bool RecordAvailableForScrapping = false;
                bool IsInvalidRecordsExist = false;
                DataRow[] drrExistingRecords = dtExistingCompaniesDupes.Select("UserStatus = 'ExistingPassed'");
                if (drrExistingRecords.Length > 0)
                {
                    foreach (DataRow drExistingRecords in drrExistingRecords)
                        lstExistingIDsToScrape.Add(Convert.ToInt32(drExistingRecords["MASTER_ID"]));

                    string sMasterIDs = GM.ListToQueryString(lstExistingIDsToScrape, "Int");

                    GV.MSSQL1.BAL_ExecuteNonReturnQuery("UPDATE " + GV.sProjectID +
                                                            "_mastercompanies set Scrape_Status = 1, UPDATED_BY = '" +
                                                            GV.sEmployeeName +
                                                            "', UPDATED_DATE = GETDATE() WHERE MASTER_ID IN (" + sMasterIDs + ") AND FLAG IN ('TR','WR');");
                    RecordAvailableForScrapping = true;
                }

                DataRow[] drrNewRecords =
                    dtImportedCompanies.Select("ISNULL(COMPANY_STATUS,'') IN (" +
                                               GM.ListToQueryString(lstPassedStatus, "String") +
                                               ") OR UserStatus = 'Passed'");

                if (drrNewRecords.Length > 0)
                {
                    string sUpdateScrapeStatus = GM.ColumnToQString("COMPANY_ID", drrNewRecords.CopyToDataTable(),
                        "String");
                    string sInsertValues = string.Empty;
                    foreach (DataRow drImport in drrNewRecords)
                    {
                        sInsertValues += ",(";
                        foreach (string sCols in lstMadotoryImportColumns)
                        {
                            sInsertValues += "'" + drImport[sCols].ToString().Replace("'", "''") + "',";
                        }
                        sInsertValues += " '" + drImport["COMPANY_NAME_ALPHA"] + "'," + (drImport["SWITCHBOARD_TRIMMED"].ToString().Length > 0 ? "'" + drImport["SWITCHBOARD_TRIMMED"].ToString() + "'" : "NULL") + ", GETDATE(), '" + GV.sEmployeeName + "' ,'" + GV.sEmployeeName +
                                         "', '" + GV.sAccessTo + "', GETDATE(), '" + GV.sEmployeeName + "', 1,'NEW'," +
                                         dtCountry.Select("CountryName = '" + drImport["COUNTRY"] + "'")[0][
                                             "HoursFromGMT"] + " , 'L_COMPANY','L_COMPANY','L_COMPANY','L_COMPANY')";
                    }
                    sInsertValues = sInsertValues.Substring(1);
                    string sInsertColumns = string.Empty;
                    foreach (string sCols in lstMadotoryImportColumns)
                    {
                        sInsertColumns += sCols + ",";
                    }
                    GV.MSSQL1.BAL_ExecuteNonReturnQuery("INSERT INTO " + GV.sProjectID + "_mastercompanies (" +
                                                            sInsertColumns + " COMPANY_NAME_ALPHA,SWITCHBOARD_TRIMMED, " +
                                                            sDateColumn + "," + GV.sAccessTo + "_AGENTNAME," +
                                                            GV.sAccessTo +
                                                            "_PREVIOUS_AGENTNAME, FLAG,CREATED_DATE,CREATED_BY, Scrape_Status,NEW_OR_EXISTING,HoursFromGMT,TR_PRIMARY_DISPOSAL,TR_SECONDARY_DISPOSAL,WR_PRIMARY_DISPOSAL,WR_SECONDARY_DISPOSAL) VALUES " +
                                                            sInsertValues);
                    GV.MSSQL1.BAL_ExecuteNonReturnQuery("UPDATE " + GV.sProjectID +
                                                            "_mastercompanies set GROUP_ID = MASTER_ID WHERE GROUP_ID IS NULL AND CREATED_BY = '" +
                                                            GV.sEmployeeName + "';");
                    GV.MSSQL1.BAL_ExecuteQuery(
                        "UPDATE c_mastercompanies set RECORD_STATUS = 'IMPORTED' WHERE COMPANY_ID IN (" +
                        sUpdateScrapeStatus + ") AND PROJECTID = '" + GV.sProjectID + "' AND CREATED_BY = '" +
                        GV.sEmployeeName + "';");
                    RecordAvailableForScrapping = true;
                }

                DataTable dtCheckRecordExsitForScraping =
                    GV.MSSQL1.BAL_ExecuteQuery("SELECT 1 FROM " + GV.sProjectID + "_mastercompanies WHERE " +
                                                   GV.sAccessTo + "_AGENTNAME = '" + GV.sEmployeeName +
                                                   "' AND Scrape_Status = 1");

                DataRow[] drrInvalidRecords =
                    dtImportedCompanies.Select("ISNULL(COMPANY_STATUS,'') IN (" +
                                               GM.ListToQueryString(lstInvalidStatus, "String") +
                                               ") OR UserStatus = 'Invalid'");
                if (drrInvalidRecords.Length > 0)
                {
                    IsInvalidRecordsExist = true;
                    string sUpdateScrapeStatus = GM.ColumnToQString("COMPANY_ID", drrInvalidRecords.CopyToDataTable(),
                        "String");
                    GV.MSSQL1.BAL_ExecuteQuery(
                        "UPDATE c_mastercompanies set RECORD_STATUS = 'REJECTED',COMPANY_STATUS = 'REJECTED' WHERE COMPANY_ID IN (" +
                        sUpdateScrapeStatus + ") AND PROJECTID = '" + GV.sProjectID + "' AND CREATED_BY = '" +
                        GV.sEmployeeName + "';");

                    

                    if (!bProcess.IsBusy && !RecordAvailableForScrapping && dtCheckRecordExsitForScraping.Rows.Count > 0)//Mine existing records in the DB
                    {
                        if (DialogResult.Yes !=
                            MessageBoxEx.Show(
                                "Current records are cleared.<br/>Still some records are in queue for mining. Would you like to mine them now ?",
                                "Campaign Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {
                            return;
                        }
                    }
                }

                if (RecordAvailableForScrapping || IsInvalidRecordsExist)
                {
                    ReloadTables();
                    ReloadGrid();
                }
                if (RecordAvailableForScrapping || dtCheckRecordExsitForScraping.Rows.Count > 0)
                {
                    if (bProcess.IsBusy)
                    {
                        if (RecordAvailableForScrapping)
                        {
                            ParantForm.Invoke(
                                (MethodInvoker)
                                    delegate
                                    {
                                        ToastNotification.Show(this, "Records added to mining queue.",
                                            eToastPosition.TopRight);
                                    });
                        }
                        else
                        {
                            ParantForm.Invoke(
                                (MethodInvoker)
                                    delegate
                                    {
                                        ToastNotification.Show(this, "No records to added.",
                                            eToastPosition.TopRight);
                                    });
                        }
                    }
                    else
                    {
                        if ((txtLinkedInUserName.Text.Trim().Length > 0 && txtLinkedInPassword.Text.Trim().Length > 0))
                        {
                            btnProcess.Text = "Add to mining Queue";
                            txtLinkedInUserName.Enabled = false;
                            txtLinkedInPassword.Enabled = false;
                            picProcess.Visible = true;
                            ToastNotification.Show(this, "Mining Started.", eToastPosition.TopRight);
                            timerStatusReader.Enabled = true;
                            timerStatusReader.Start();
                            iQueueCount = dtQueue.Rows.Count;
                            ParantForm.Invoke((MethodInvoker) delegate { ParantForm.progressBar.Visible = true; });
                            ParantForm.Invoke((MethodInvoker) delegate { ParantForm.progressBar.Value = 0; });
                            bProcess.RunWorkerAsync();
                        }
                        else if (txtLinkedInUserName.Text.Trim().Length == 0 ||
                                 txtLinkedInPassword.Text.Trim().Length == 0)
                            ToastNotification.Show(this,
                                "Username or Password empty.<br/>Records queued for mining.",
                                eToastPosition.TopRight);
                    }
                }
                else
                    ToastNotification.Show(this, "No records to mine.", eToastPosition.TopRight);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        private void bProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            if (File.Exists(GV.sBOTPath))
            {
                try
                {
                        Process[] pName = Process.GetProcessesByName("LinkedIN");
                        if (pName.Length > 0)
                            pName[0].Kill(); // Make sure the scrapper is not running


                    //File.WriteAllText(fullPath + "Failure_details.txt", string.Empty);
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Update_Status.ini", string.Empty);
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Bridge.txt", string.Empty);
                    
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = Path.GetFileName(GV.sBOTPath);
                    psi.WorkingDirectory = Path.GetDirectoryName(GV.sBOTPath);

                    psi.UseShellExecute = false;
                    //psi.RedirectStandardError = true;
                    //psi.RedirectStandardInput = true;
                    //psi.RedirectStandardOutput = true;
                    psi.CreateNoWindow = true;
                    psi.WindowStyle = ProcessWindowStyle.Hidden;

                    List<string> lst = GV.sMSSQL.Split(';').ToList();
                    string sServerIP = string.Empty;
                    string sServerUID = string.Empty;
                    string sServerPwd = string.Empty;
                    foreach (string s in lst)
                    {
                        if (s.StartsWith("Server="))
                            sServerIP = s.Split('=')[1];
                        else if (s.StartsWith("Uid="))
                            sServerUID = s.Split('=')[1];
                        else if (s.StartsWith("Pwd="))
                            sServerPwd = s.Split('=')[1];
                    }

                    string sCompleteStatus = " AND (TR_CONTACT_STATUS IN(" +
                                             GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") +
                                             ") OR WR_CONTACT_STATUS IN(" +
                                             GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + "))";

                    psi.Arguments = "\"" + sServerIP + "\" \"" + sServerUID + "\" \"" + sServerPwd + "\" \"" + GV.sProjectID + "\" \"" + GV.sEmployeeName + "\" \"" + txtLinkedInUserName.Text + "\" \"" +
                                    txtLinkedInPassword.Text + "\" \"" + sCompleteStatus + "\" \"" + GV.sAccessTo + "\"";
                    Process ps = new Process();
                    ps.StartInfo = psi;
                    ps.Start();
                    ParantForm.Invoke((MethodInvoker)delegate { ParantForm.progressBar.Text = "Miner started"; });
                    ps.WaitForExit();


                }
                catch (Exception ex)
                {
                    //GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex,true,true);
                    this.Invoke((MethodInvoker)delegate { MessageBoxEx.Show(ex.Message); });
                }
            }
            else
            {
                this.Invoke((MethodInvoker)delegate { MessageBoxEx.Show("Miner does not exist. Restart Campaign Manager.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop); });                
            }
        }

        private void timerStatusReader_Tick(object sender, EventArgs e)
        {
            try
            {
                string sUpdateStatus = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Update_Status.ini");
                if (sUpdateStatus.Trim() == "1")
                {
                    DataTable dtScrapper_Log =
                        GV.MSSQL1.BAL_ExecuteQuery(
                            "SELECT * FROM c_scrapper_log WHERE  CAST(START_TIME as DATE) = cast(GETDATE() as date) AND PROJECTID='" +
                            GV.sProjectID + "' AND AGENTNAME = '" + GV.sEmployeeName + "' AND RESEARCH_TYPE='" +
                            GV.sAccessTo + "' AND (STATUS IS NULL OR STATUS = 'Researched');");

                    DataTable dtScrappedContact = new DataTable();
                    if (dtScrapper_Log.Select("STATUS = 'Researched'").Length > 0)
                    {
                        string sCompanyIDs = GM.ColumnToQString("COMPANYID",
                            dtScrapper_Log.Select("STATUS = 'Researched'").CopyToDataTable(), "Int");
                        if (sCompanyIDs.Length > 0)
                        {
                            dtScrappedContact =
                                GV.MSSQL1.BAL_ExecuteQuery("SELECT * FROM " + GV.sProjectID +
                                                               "_mastercontacts WHERE MASTER_ID IN (" + sCompanyIDs +
                                                               ");");

                            foreach (DataRow drScrappedContacts in dtScrappedContact.Rows)
                            {
                                if (drScrappedContacts["CONTACT_EMAIL"].ToString().Length == 0)
                                {
                                    string sFirstName = drScrappedContacts["FIRST_NAME"].ToString();
                                    string sLastName = drScrappedContacts["LAST_NAME"].ToString();
                                    string sDomain = drScrappedContacts["Contact_Domain"].ToString();
                                    if (sFirstName.Length > 0 && sLastName.Length > 0 && sDomain.Length > 0)
                                    {
                                        string sEmail = sFirstName + "." + sLastName + "@" + sDomain;
                                        sEmail =
                                            sEmail.Replace(" ", string.Empty)
                                                .Replace("'", string.Empty)
                                                .Replace("/", string.Empty);
                                        if (GM.Email_Check(sEmail))
                                        {
                                            drScrappedContacts["CONTACT_EMAIL"] = sEmail.ToLower();
                                            drScrappedContacts["EMAIL_EXTRAPOLATE"] = "Yes";
                                        }
                                    }
                                }
                            }
                        }
                    }

                    foreach (DataRow drScrapperLog in dtScrapper_Log.Rows)
                    {
                        if (drScrapperLog["STATUS"].ToString() == "Researched")
                        {
                            drScrapperLog["STATUS"] = "Processed";
                            if (dtQueue.Select("MASTER_ID = '" + drScrapperLog["COMPANYID"] + "'").Length > 0)
                                dtQueue.Select("MASTER_ID = '" + drScrapperLog["COMPANYID"] + "'")[0].Delete();
                            dtQueue.AcceptChanges();

                            ParantForm.Invoke(
                                (MethodInvoker)
                                    delegate
                                    {
                                        ExpanelQueue.TitleText = "Queue ("+ dtQueue.Rows.Count +")";
                                    });

                            ParantForm.Invoke(
                                (MethodInvoker)
                                    delegate
                                    {
                                        ParantForm.progressBar.Value =
                                            Convert.ToInt32(((iQueueCount - dtQueue.Rows.Count)/
                                                             Convert.ToDouble(iQueueCount))*100);
                                    });
                        }
                        else if (drScrapperLog["STATUS"].ToString() == string.Empty)
                        {
                            //   if (drScrapperLog["END_TIME"].ToString() == string.Empty)
                            {
                                DataRow[] drrCompName =
                                    dtQueue.Select("MASTER_ID = '" + drScrapperLog["COMPANYID"] + "'");
                                if (drrCompName.Length > 0)
                                {
                                    lblCurrentCompany.Text = "Current Company : " + drrCompName[0]["MASTER_ID"] + " : " +
                                                             drrCompName[0]["COMPANY_NAME"];

                                    ParantForm.Invoke(
                                        (MethodInvoker)
                                            delegate
                                            {
                                                ParantForm.progressBar.Text = drrCompName[0]["COMPANY_NAME"].ToString();
                                            });
                                }
                            }
                        }
                    }

                    if (GV.MSSQL1.BAL_SaveToTable(dtScrapper_Log, "c_scrapper_log", "Update", true))
                    {
                        if (dtScrappedContact.Rows.Count > 0)
                        {
                            GV.MSSQL1.BAL_SaveToTable(dtScrappedContact, GV.sProjectID + "_mastercontacts", "Update",
                                true);
                        }
                    }
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Update_Status.ini", string.Empty);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnImport.Text == "Import")
                {

                    int inc = 0;
                    if (
                        GV.MSSQL1.BAL_ExecuteQuery(
                            "SELECT 1 FROM c_mastercompanies WHERE RECORD_STATUS = 'IMPORTPROGRESS' AND PROJECTID = '" +
                            GV.sProjectID + "' AND CREATED_BY = '" + GV.sEmployeeName + "'").Rows.Count > 0)
                    {
                        if (DialogResult.Yes ==
                            MessageBoxEx.Show(
                                "Some companies are still pending to process.<br/>So, would you like to import companies to archive ?",
                                "Campaign Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {
                            //ClearALL();
                            //ReloadTables();
                            // ReloadGrid();
                            inc = 40; //Exceed the 30 limit intentionly
                        }
                        else
                        {
                            ToastNotification.Show(this, "Operation canceled.", eToastPosition.TopRight);
                            return;
                        }
                    }

                    if (openFile.ShowDialog(this) == DialogResult.OK)
                    {
                        if (File.Exists(openFile.FileName))
                        {
                            using (DataTable dtCSV = new DataTable())
                            {
                                using (
                                    TextFieldParser csvReader = new TextFieldParser(openFile.FileName,
                                        System.Text.Encoding.Default))
                                {
                                    csvReader.SetDelimiters(",");
                                    csvReader.HasFieldsEnclosedInQuotes = true;

                                    string[] colFields = csvReader.ReadFields();
                                    foreach (string column in colFields)
                                    {
                                        DataColumn datecolumn = new DataColumn(column.ToUpper());
                                        datecolumn.AllowDBNull = true;
                                        dtCSV.Columns.Add(datecolumn);
                                    }

                                    while (!csvReader.EndOfData)
                                    {
                                        string[] fieldData = csvReader.ReadFields();
                                        //Making empty value as null
                                        for (int i = 0; i < fieldData.Length; i++)
                                        {
                                            fieldData[i] = fieldData[i].Replace("  ", " ").Trim();
                                            //if (fieldData[i] == "")
                                            //{
                                            //    fieldData[i] = null;
                                            //}
                                        }
                                        dtCSV.Rows.Add(fieldData);
                                    }
                                }

                                string sMissingCols = string.Empty;
                                foreach (string sColumns in lstMadotoryImportColumns)
                                {
                                    if (!dtCSV.Columns.Contains(sColumns))
                                    {
                                        sMissingCols += sColumns + ", ";
                                    }
                                }

                                if (sMissingCols.Length > 0)
                                {
                                    ToastNotification.Show(this, "Missing columns : " + sMissingCols.Trim().Substring(1),
                                        eToastPosition.TopRight);
                                    return;
                                }

                                bool IsArchiveEnabled = true;
                                if (dtCSV.Rows.Count > 0)
                                {
                                    if (dtCSV.Rows.Count >= 30 && inc == 0)
                                    {
                                        DialogResult dRes =
                                            MessageBoxEx.Show(
                                                "Imported data exceeds the limit.<br/>Would you like the archive the remaining ?",
                                                "Campaign Manager", MessageBoxButtons.YesNoCancel,
                                                MessageBoxIcon.Question);
                                        if (dRes == DialogResult.Yes)
                                            IsArchiveEnabled = true;
                                        else if (dRes == DialogResult.No)
                                            IsArchiveEnabled = false;
                                        else
                                        {
                                            ToastNotification.Show(this, "Import cancelled.", eToastPosition.TopRight);
                                            return;
                                        }
                                    }



                                    string sInsertValues = string.Empty;
                                    //int inc = 0;

                                    Regex rAlphaNumeric = new Regex(@"[^0-9A-Za-z]+",
                                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                                    Regex rNumeric = new Regex(@"[^\d]",
                                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                                    string sRecordStatus = "IMPORTPROGRESS";
                                    foreach (DataRow drImport in dtCSV.Rows)
                                    {
                                        string sCompanyStatus = string.Empty;
                                        string sCompanyAlphaNumeric = string.Empty;
                                        string sSwitchBoardTrim = string.Empty;
                                        if (drImport["COMPANY_NAME"].ToString().Length > 0 &&
                                            dtCountry.Select("CountryName = '" + drImport["COUNTRY"] +
                                                             "' AND LEN(ISNULL(Short,'')) > 0").Length > 0)
                                        {
                                            inc++;
                                            sCompanyAlphaNumeric = rAlphaNumeric.Replace(
                                                drImport["COMPANY_NAME"].ToString(), string.Empty);

                                            if (
                                                lstMadotoryImportColumns.Contains("SWITCHBOARD",
                                                    StringComparer.OrdinalIgnoreCase) &&
                                                dtCSV.Columns.Contains("SWITCHBOARD"))
                                            {
                                                sSwitchBoardTrim = rNumeric.Replace(drImport["SWITCHBOARD"].ToString(),
                                                    string.Empty);
                                                if (sSwitchBoardTrim.Length > 7)
                                                    sSwitchBoardTrim =
                                                        sSwitchBoardTrim.Substring(sSwitchBoardTrim.Length - 8);
                                                else sSwitchBoardTrim = "NULL";
                                            }
                                            else sSwitchBoardTrim = "NULL";
                                        }
                                        else if (drImport["COMPANY_NAME"].ToString().Length == 0)
                                            sCompanyStatus = "Company Empty";
                                        else
                                            sCompanyStatus = "Invalid Country";

                                        sInsertValues += ",(";
                                        foreach (string sCols in lstMadotoryImportColumns)
                                        {
                                            sInsertValues += "'" + drImport[sCols].ToString().Replace("'", "''") + "',";
                                        }

                                        if (sCompanyStatus.Length > 0)
                                            sInsertValues += "'" + GV.sProjectID + "', NULL , NULL,'REJECTED', '" +
                                                             sCompanyStatus + "', GETDATE(), GETDATE(), '" + GV.sEmployeeName +
                                                             "')";
                                        else
                                        {
                                            if (inc >= 30) //Import only 30 Rows.. Atleast for now
                                            {
                                                if (IsArchiveEnabled)
                                                    sInsertValues += "'" + GV.sProjectID + "', '" + sCompanyAlphaNumeric +
                                                                     "','" + sSwitchBoardTrim + "','ARCHIVED', NULL , GETDATE(), GETDATE(), '" +
                                                                     GV.sEmployeeName + "')";
                                                else
                                                {
                                                    sInsertValues += "'" + GV.sProjectID + "', '" + sCompanyAlphaNumeric +
                                                                 "','" + sSwitchBoardTrim + "','IMPORTPROGRESS', NULL , GETDATE(), GETDATE(), '" +
                                                                 GV.sEmployeeName + "')";
                                                    break;
                                                }
                                            }
                                            else
                                                sInsertValues += "'" + GV.sProjectID + "', '" + sCompanyAlphaNumeric +
                                                                 "','" +sSwitchBoardTrim + "','IMPORTPROGRESS', NULL , GETDATE(), GETDATE(), '" +
                                                                 GV.sEmployeeName + "')";
                                        }

                                        //if (inc >= 30) //Import only 30 Rows.. Atleast for now
                                        //{
                                        //    sRecordStatus
                                        //    break;
                                        //}
                                    }
                                    sInsertValues = sInsertValues.Substring(1);

                                    string sInsertColumns = string.Empty;
                                    foreach (string sCols in lstMadotoryImportColumns)
                                    {
                                        sInsertColumns += sCols + ",";
                                    }

                                    GV.MSSQL1.BAL_ExecuteNonReturnQuery("INSERT INTO c_mastercompanies (" +
                                                                            sInsertColumns +
                                                                            " PROJECTID,COMPANY_NAME_ALPHA,SWITCHBOARD_TRIMMED, RECORD_STATUS,COMPANY_STATUS,IMPORT_DATE,CREATED_DATE,CREATED_BY) VALUES " +
                                                                            sInsertValues);
                                    GV.MSSQL1.BAL_Procedure("C_Dupes_Check", GV.sProjectID, GV.sEmployeeName);

                                    ReloadTables();

                                    ReloadGrid();

                                    ToastNotification.Show(this, "Records imported.", eToastPosition.TopRight);

                                }
                                else
                                {
                                    ToastNotification.Show(this, "No data found.", eToastPosition.TopRight);
                                }

                                //objMYfSQL.Procedure("L_DUPE_CHECK", sProjectID, sAgentName, dateLoad_Research.Value.ToString("yyyy-MM-dd"));
                                //panelInfo.Enabled = false;
                                //picImport.Visible = true;

                                //btnReject.Enabled = false;
                                //btnAccept.Enabled = false;
                                //btnLoad_Research.Enabled = false;
                                //btnImportDomains.Enabled = false;
                                //bWorker_Import.RunWorkerAsync();
                            }
                        }
                        else
                        {
                            ToastNotification.Show(this, "File does not exist.", eToastPosition.TopRight);
                        }
                    }
                }
                else if (btnImport.Text == "Import Selected")
                {
                    DataRow[] drrSelectedArchive = dtArchived.Select("Action = '1'");
                    int iCount = drrSelectedArchive.Length;
                    if (iCount > 0)
                    {
                        if (DialogResult.Yes ==
                            MessageBoxEx.Show("Are you sure to load these " + iCount + " records for processing ?",
                                "Campaign Manager",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question))
                        {
                            string sIDString = GM.ColumnToQString("COMPANY_ID", drrSelectedArchive.CopyToDataTable(),
                                "Int");
                            GV.MSSQL1.BAL_ExecuteNonReturnQuery(
                                "UPDATE c_mastercompanies set RECORD_STATUS='IMPORTPROGRESS', IMPORT_DATE = GETDATE() WHERE COMPANY_ID IN (" +
                                sIDString + ") AND PROJECTID = '" + GV.sProjectID + "' AND CREATED_BY = '" +
                                GV.sEmployeeName +
                                "' AND RECORD_STATUS='ARCHIVED';");
                            GV.MSSQL1.BAL_Procedure("C_Dupes_Check", GV.sProjectID, GV.sEmployeeName);
                            ReloadTables();
                            ReloadGrid();
                            ToastNotification.Show(this, "Records loaded for processing.", eToastPosition.TopRight);
                            ExpanelArchive.Expanded = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        private void bProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerStatusReader.Stop();

            ParantForm.Invoke((MethodInvoker)delegate { ParantForm.progressBar.Text = ""; });
            ParantForm.Invoke((MethodInvoker)delegate { ParantForm.progressBar.Value = 0; });
            ParantForm.Invoke((MethodInvoker)delegate { ParantForm.progressBar.Visible = false; });

            string sEnvironmentStatus = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Bridge.txt");
            if (sEnvironmentStatus.Length == 0)
                this.Invoke((MethodInvoker)delegate { MessageBoxEx.Show("Mining completed.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information); });
            else if (sEnvironmentStatus.Contains("No Internet"))
                this.Invoke((MethodInvoker)delegate { MessageBoxEx.Show("Internet not connected.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error); });
            else if (sEnvironmentStatus.Contains("Invalid login"))
                this.Invoke((MethodInvoker)delegate { MessageBoxEx.Show("Invalid Username or Password.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error); });
            else if (sEnvironmentStatus.Contains("No Premium"))
                this.Invoke((MethodInvoker)delegate { MessageBoxEx.Show("Please provide premium account for mining.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error); });
            else if (sEnvironmentStatus.Contains("LinkedIn Blocked"))
                this.Invoke((MethodInvoker)delegate { MessageBoxEx.Show("LinkedIn account blocked.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error); });
            else//Incase if above all fails
                this.Invoke((MethodInvoker)delegate { MessageBoxEx.Show("Mining completed.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information); });            

            btnProcess.Text = "Start";            
            txtLinkedInUserName.Enabled = true;
            txtLinkedInPassword.Enabled = true;
            picProcess.Visible = false;            
            timerStatusReader.Enabled = false;
            lblCurrentCompany.Text = string.Empty;
            lblCount.Text = string.Empty;
        }

        void ClearALL()
        {
            try
            {
                GV.MSSQL1.BAL_ExecuteNonReturnQuery(
                    "UPDATE c_mastercompanies set RECORD_STATUS = 'CLEARED' WHERE RECORD_STATUS = 'IMPORTPROGRESS' AND PROJECTID = '" +
                    GV.sProjectID + "' AND CREATED_BY = '" + GV.sEmployeeName + "'");
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (btnClear.Text == "Clear All")
            {
                if (DialogResult.Yes ==
                    MessageBoxEx.Show("Are you sure to clear all bins ?", "Campaign Manager",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question))
                {
                    ClearALL();
                    ReloadTables();
                    ReloadGrid();
                    ToastNotification.Show(this, "Companies cleared.", eToastPosition.TopRight);
                }
            }
            else if (btnClear.Text == "Clear Archive")
            {
                if (DialogResult.Yes ==
                    MessageBoxEx.Show("Are you sure to clear the archive ?", "Campaign Manager",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question))
                {
                    GV.MSSQL1.BAL_ExecuteNonReturnQuery("DELETE FROM c_mastercompanies WHERE RECORD_STATUS = 'ARCHIVED' AND PROJECTID = '" + GV.sProjectID + "' AND CREATED_BY = '" + GV.sEmployeeName + "'");
                    dtArchived.Rows.Clear();
                    sdgvArchive.PrimaryGrid.DataSource = dtArchived;
                    ToastNotification.Show(this, "Archive cleared.", eToastPosition.TopRight);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtArchiveSearch.Text.Trim().Length > 0)
                {
                    string currFilter = string.Format(sArchiveSearchQuery, txtArchiveSearch.Text);
                    DataRow[] drrSearchRows = dtArchived.Select(currFilter);
                    if (drrSearchRows.Length > 0)
                    {
                        sdgvArchive.PrimaryGrid.DataSource = drrSearchRows.CopyToDataTable();
                        sdgvArchive.PrimaryGrid.Caption.Text = "<div align='center'>Archived Companies (<i>Filtered " +
                                                               drrSearchRows.Length + " out of " + dtArchived.Rows.Count +
                                                               "</i>) Selected : " +
                                                               dtArchived.Select("Action = '1'").Length + "</div>";
                    }
                    else
                        sdgvArchive.PrimaryGrid.DataSource = null;

                    //sdgvArchive.PrimaryGrid.Caption.Text = "<div align='center'>Archived Companies (<i>Filtered</i>)</div>";

                }
                else
                {
                    sdgvArchive.PrimaryGrid.DataSource = dtArchived;
                    sdgvArchive.PrimaryGrid.Caption.Text = "Archived Companies (" + dtArchived.Rows.Count +
                                                           ") Selected : " + dtArchived.Select("Action = '1'").Length;
                }


                //if (txtSearch.Text.Trim().Length > 0 && dtArchived.Rows.Cast<DataRow>().Where(r => r.ItemArray.Any(c => c.ToString().IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) > 0)).Count() > 0)
                //{
                //    sdgvArchive.PrimaryGrid.DataSource = dtArchived.Rows.Cast<DataRow>().Where(r => r.ItemArray.Any(c => c.ToString().IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) > 0)).CopyToDataTable();                
                //}
                //else
                //    sdgvArchive.PrimaryGrid.DataSource = dtArchived;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        private void ExpanelArchive_ExpandedChanging(object sender, ExpandedChangeEventArgs e)
        {            
            splitHorizontal.Visible = !e.NewExpandedValue;            
        }

        private void ExpanelArchive_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                ExpanelArchive.Width = panelInfo.Width - 30;
                btnClear.Text = "Clear Archive";
                btnImport.Text = "Import Selected";

                panelLegends.Visible = false;
                panelProcess.Visible = false;
                panelArchiveLegend.Visible = true;

                if (ExpanelQueue.Expanded)
                    ExpanelQueue.Expanded = false;
            }
            else
            {                
                btnClear.Text = "Clear All";
                btnImport.Text = "Import";

                panelArchiveLegend.Visible = false;
                panelLegends.Visible = true;
                panelProcess.Visible = true;                
            }
        }

        private void ExpanelQueue_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                ExpanelQueue.Width = panelInfo.Width - 30;
                //ExpanelQueue.Width = this.Width;

                if (ExpanelArchive.Expanded)
                    ExpanelArchive.Expanded = false;

                panelLegends.Visible = false;
                panelArchiveLegend.Visible = false;

                btnImport.Visible = false;
                btnClear.Visible = false;
                btnImportInfo.Visible = false;
            }
            else
            {
                panelLegends.Visible = true;

                btnImport.Visible = true;
                btnClear.Visible = true;
                btnImportInfo.Visible = true;
            }
            
        }

        private void ExpanelQueue_ExpandedChanging(object sender, ExpandedChangeEventArgs e)
        {
            splitHorizontal.Visible = !e.NewExpandedValue;
        }

        private void txtQueueSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtQueueSearch.Text.Trim().Length > 0)
                {
                    string currFilter = string.Format(sQueueSearchQuery, txtQueueSearch.Text);
                    DataRow[] drrSearchRows = dtQueue.Select(currFilter);
                    if (drrSearchRows.Length > 0)
                    {
                        sdgvQueue.PrimaryGrid.DataSource = drrSearchRows.CopyToDataTable();
                        sdgvQueue.PrimaryGrid.Caption.Text = "<div align='center'>Total companies in queue : " +
                                                             dtQueue.Rows.Count + " (<i>Filtered : " +
                                                             drrSearchRows.Length + "</i>)</div>";
                    }
                    else
                    {
                        sdgvQueue.PrimaryGrid.Caption.Text = "<div align='center'>Total companies in queue : " + dtQueue.Rows.Count + " (<i>Filtered : 0</i>)</div>";
                        //sdgvQueue.PrimaryGrid.Caption.Text = "Total companies in queue :";
                    }

                }
                else
                {
                    sdgvQueue.PrimaryGrid.DataSource = dtQueue;
                    sdgvQueue.PrimaryGrid.Caption.Text = "Total companies in queue :" + dtQueue.Rows.Count;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        private void frmCompanyImport_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bProcess.IsBusy)
            {
                e.Cancel = true;
                ToastNotification.Show(this, "Research in progress.", eToastPosition.TopRight);
            }
        }

        private void btnMarked_Click(object sender, EventArgs e)
        {
            try
            {
                if (sdgvArchive.PrimaryGrid.SelectedRowCount > 0)
                {
                    if (dtImportedCompanies.Select("RECORD_STATUS = 'IMPORTPROGRESS'").Length > 0)
                    {
                        ToastNotification.Show(this,
                            "Records still pending to process.<br/>These records has to be processed or cleared to import new.",
                            eToastPosition.TopRight);
                        return;
                    }


                    if (sdgvArchive.PrimaryGrid.SelectedRowCount > 30 ||
                        (dtArchived.Select("Action = '1'").Length + sdgvArchive.PrimaryGrid.SelectedRowCount) > 30)
                        ToastNotification.Show(this, "More then 30 rows selected.", eToastPosition.TopRight);
                    else
                    {
                        //foreach (DataRow drArchive in dtArchived.Rows)                    
                        //    drArchive["Action"] = "5";                    

                        IEnumerable SelectedRows = sdgvArchive.PrimaryGrid.SelectedRows;
                        foreach (var item in SelectedRows)
                        {
                            GridRow GR = item as GridRow;
                            GR.Cells["Action"].Value = "1";
                        }
                    }
                    sdgvArchive.PrimaryGrid.Caption.Text = "Archived Companies (" + dtArchived.Rows.Count +
                                                           ") Selected : " + dtArchived.Select("Action = '1'").Length;
                }
                else
                    ToastNotification.Show(this, "No rows selected.", eToastPosition.TopRight);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }        

        private void btnTopSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtImportedCompanies.Select("RECORD_STATUS = 'IMPORTPROGRESS'").Length > 0)
                {
                    ToastNotification.Show(this,
                        "Records still pending to process.<br/>These records has to be processed or cleared to import new.",
                        eToastPosition.TopRight);
                    return;
                }

                for (int i = 0; i < dtArchived.Rows.Count; i++)
                {
                    if (i < 30)
                        dtArchived.Rows[i]["Action"] = "1";
                    else
                        dtArchived.Rows[i]["Action"] = "5";
                }

                sdgvArchive.PrimaryGrid.Caption.Text = "Archived Companies (" + dtArchived.Rows.Count + ") Selected : " +
                                                       dtArchived.Select("Action = '1'").Length;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        private void btnClearSelection_Click(object sender, EventArgs e)
        {
            try
            {

                //if (dtImportedCompanies.Select("RECORD_STATUS = 'IMPORTPROGRESS'").Length > 0)
                //{
                //    ToastNotification.Show(this, "Records still pending to process.<br/>These records has to be processed or cleared to import new.", eToastPosition.TopRight);
                //    return;
                //}

                for (int i = 0; i < dtArchived.Rows.Count; i++)
                    dtArchived.Rows[i]["Action"] = "5";

                sdgvArchive.PrimaryGrid.Caption.Text = "Archived Companies (" + dtArchived.Rows.Count + ") Selected : " +
                                                       dtArchived.Select("Action = '1'").Length;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        private void sdgvArchive_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridColumn GC in sdgvArchive.PrimaryGrid.Columns)
            {
                if (GC.Name.ToUpper() == "ACTION")
                {
                    GC.ReadOnly = false;
                    GC.FillWeight = 10;
                    GC.MinimumWidth = 50;                    
                }
                GC.Visible = (GC.Name.ToUpper() != "COMPANY_ID");

                GC.HeaderText = GC.Name.Replace("_", " ");
            }
        }        
    }
}

