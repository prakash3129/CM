using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar;
using DevExpress.XtraCharts;
using Microsoft.VisualBasic.FileIO;

namespace GCC
{
    public partial class frmEmailChecks : Office2007Form
    {
        public frmEmailChecks()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); //Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(Font.FontFamily, 22);
            MessageBoxEx.EnableGlass = false;
            dtProcessDate.Value = GM.GetDateTime();
        }

        private DataTable dtEmails = new DataTable();
        private DataTable dtStatus = new DataTable();
        private DataTable dtImportSources = new DataTable();
        private void frmEmailChecks_Load(object sender, EventArgs e)
        {
            try
            {
                dtStatus = GV.MYSQL.BAL_FetchTableMySQL("c_picklists", "PicklistCategory = 'EmailStatus'");
                if (GV.sProjectID.ToUpper() == "CRUPRO001" && (GV.sUserType == "QC" || GV.sUserType == "Admin"))
                {
                    txtSourceORAgent.WatermarkText = "Select Source";
                    dtImportSources = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT PicklistCategory,PicklistValue FROM crupro001_picklists ORDER BY ID DESC LIMIT 100;");
                    panelLegends.Location = new Point(239, 7);
                }
                else
                {
                    txtImportPath.Visible = false;
                    txtImportSource.Visible = false;
                    btnImport.Visible = false;
                    txtSourceORAgent.WatermarkText = "Select Agent";
                    Load_Emails();
                }

                txtSourceORAgent.Visible = true; //If permission chenged from agent to manager in a same session
                if (GV.sUserType == "Agent")
                {

                    txtSourceORAgent.Visible = false;
                    //sdgvEmails.PrimaryGrid.Columns[0].Visible = true; //Status
                    //sdgvEmails.PrimaryGrid.Columns[1].Visible = true; //Company ID
                    //sdgvEmails.PrimaryGrid.Columns[2].Visible = true; //Contact ID
                    //sdgvEmails.PrimaryGrid.Columns[3].Visible = true; //Email
                    sdgvEmails.PrimaryGrid.Columns[4].Visible = false; //Email Source
                    sdgvEmails.PrimaryGrid.Columns[5].Visible = false; //Description
                    sdgvEmails.PrimaryGrid.Columns[6].Visible = false; //Detail
                    sdgvEmails.PrimaryGrid.Columns[7].Visible = false; //ServerID
                    sdgvEmails.PrimaryGrid.Columns[8].Visible = false; //Agent Name
                    sdgvEmails.PrimaryGrid.Columns[9].Visible = false; //Date
                    //sdgvEmails.PrimaryGrid.Columns[10].Visible = true; //FirstName
                    //sdgvEmails.PrimaryGrid.Columns[11].Visible = true; //LastName
                    sdgvEmails.PrimaryGrid.Columns[12].Visible = false; //bounce

                }
                else if (GV.sUserType == "Manager")
                {
                    //sdgvEmails.PrimaryGrid.Columns[0].Visible = true; //Status
                    //sdgvEmails.PrimaryGrid.Columns[1].Visible = true; //Company ID
                    //sdgvEmails.PrimaryGrid.Columns[2].Visible = true; //Contact ID
                    //sdgvEmails.PrimaryGrid.Columns[3].Visible = true; //Email
                    sdgvEmails.PrimaryGrid.Columns[4].Visible = false; //Email Source
                    sdgvEmails.PrimaryGrid.Columns[5].Visible = false; //Description
                    sdgvEmails.PrimaryGrid.Columns[6].Visible = false; //Detail
                    sdgvEmails.PrimaryGrid.Columns[7].Visible = false; //ServerID
                    sdgvEmails.PrimaryGrid.Columns[8].Visible = true; //Agent Name
                    sdgvEmails.PrimaryGrid.Columns[9].Visible = false; //Date
                    //sdgvEmails.PrimaryGrid.Columns[10].Visible = true; //FirstName
                    //sdgvEmails.PrimaryGrid.Columns[11].Visible = true; //LastName
                    sdgvEmails.PrimaryGrid.Columns[12].Visible = false; //bounce
                }
                else if (GV.sUserType == "QC")
                {
                    //sdgvEmails.PrimaryGrid.Columns[0].Visible = true; //Status
                    //sdgvEmails.PrimaryGrid.Columns[1].Visible = true; //Company ID
                    //sdgvEmails.PrimaryGrid.Columns[2].Visible = true; //Contact ID
                    //sdgvEmails.PrimaryGrid.Columns[3].Visible = true; //Email
                    sdgvEmails.PrimaryGrid.Columns[4].Visible = (GV.sProjectID.ToUpper() == "CRUPRO001"); //Email Source
                    sdgvEmails.PrimaryGrid.Columns[5].Visible = true; //Description
                    sdgvEmails.PrimaryGrid.Columns[6].Visible = true; //Detail
                    sdgvEmails.PrimaryGrid.Columns[7].Visible = false; //ServerID
                    sdgvEmails.PrimaryGrid.Columns[8].Visible = true; //Agent Name
                    sdgvEmails.PrimaryGrid.Columns[9].Visible = false; //Date
                    //sdgvEmails.PrimaryGrid.Columns[10].Visible = true; //FirstName
                    //sdgvEmails.PrimaryGrid.Columns[11].Visible = true; //LastName 
                    sdgvEmails.PrimaryGrid.Columns[12].Visible = false; //bounce
                }
                else if (GV.sUserType == "Admin")
                {
                    //sdgvEmails.PrimaryGrid.Columns[0].Visible = true; //Status
                    //sdgvEmails.PrimaryGrid.Columns[1].Visible = true; //Company ID
                    //sdgvEmails.PrimaryGrid.Columns[2].Visible = true; //Contact ID
                    //sdgvEmails.PrimaryGrid.Columns[3].Visible = true; //Email
                    sdgvEmails.PrimaryGrid.Columns[4].Visible = (GV.sProjectID.ToUpper() == "CRUPRO001"); //Email Source
                    sdgvEmails.PrimaryGrid.Columns[5].Visible = true; //Description
                    sdgvEmails.PrimaryGrid.Columns[6].Visible = true; //Detail
                    sdgvEmails.PrimaryGrid.Columns[7].Visible = true; //ServerID
                    sdgvEmails.PrimaryGrid.Columns[8].Visible = true; //Agent Name
                    sdgvEmails.PrimaryGrid.Columns[9].Visible = false; //Date
                    sdgvEmails.PrimaryGrid.Columns[10].Visible = false; //FirstName
                    sdgvEmails.PrimaryGrid.Columns[11].Visible = false; //LastName
                    sdgvEmails.PrimaryGrid.Columns[12].Visible = false; //bounce
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);                
            }
        }

        private void Load_Emails()
        {
            try
            {
                dtEmails.Rows.Clear();
                if (GV.sUserType == "Agent")
                {
                    dtEmails =
                        GV.MYSQL.BAL_ExecuteQueryMySQL(
                            "SELECT B.MASTER_ID,B.CONTACT_ID_P,B.First_Name,B.LAST_NAME,A.EMAIL,A.DESCRIPTION,A.DETAIL,A.CREATED_BY,A.PROCESSED_SERVER,A.CREATED_DATE,A.EMAIL_SOURCE FROM c_email_checks A INNER JOIN " +
                            GV.sProjectID +
                            "_mastercontacts B ON A.CONTACT_ID = B.CONTACT_ID_P WHERE date(A.CREATED_DATE) = '" +
                            dtProcessDate.Value.ToString("yyyy-MM-dd") + "' AND A.CREATED_BY = '" + GV.sEmployeeName +
                            "' AND A.PROJECT_ID = '" + GV.sProjectID + "' AND A.REPROCESSED = 0 ORDER BY B.MASTER_ID,B.CONTACT_ID_P;");
                }
                else if (GV.sProjectID.ToUpper() == "CRUPRO001" && (GV.sUserType == "QC" || GV.sUserType == "Admin"))
                {
                    if (txtSourceORAgent.Text.Trim().Length > 0)
                    {
                        dtEmails =
                            GV.MYSQL.BAL_ExecuteQueryMySQL(
                                "SELECT B.MASTER_ID,B.CONTACT_ID_P,B.First_Name,B.LAST_NAME,A.EMAIL,A.DESCRIPTION,A.DETAIL,A.CREATED_BY,A.PROCESSED_SERVER,A.CREATED_DATE,A.EMAIL_SOURCE FROM c_email_checks A INNER JOIN " +
                                GV.sProjectID +
                                "_mastercontacts B ON A.CONTACT_ID = B.CONTACT_ID_P WHERE date(A.CREATED_DATE) = '" +
                                dtProcessDate.Value.ToString("yyyy-MM-dd") + "' AND EMAIL_SOURCE = '" +
                                txtSourceORAgent.Text.Trim().Replace("'", "''") + "' AND A.PROJECT_ID = '" +
                                GV.sProjectID + "' ORDER BY B.MASTER_ID,B.CONTACT_ID_P;");
                    }
                    else
                    {
                        dtEmails =
                            GV.MYSQL.BAL_ExecuteQueryMySQL(
                                "SELECT B.MASTER_ID,B.CONTACT_ID_P,B.First_Name,B.LAST_NAME,A.EMAIL,A.DESCRIPTION,A.DETAIL,A.CREATED_BY,A.PROCESSED_SERVER,A.CREATED_DATE,A.EMAIL_SOURCE FROM c_email_checks A INNER JOIN " +
                                GV.sProjectID +
                                "_mastercontacts B ON A.CONTACT_ID = B.CONTACT_ID_P WHERE date(A.CREATED_DATE) = '" +
                                dtProcessDate.Value.ToString("yyyy-MM-dd") + "' AND A.PROJECT_ID = '" + GV.sProjectID +
                                "' ORDER BY B.MASTER_ID,B.CONTACT_ID_P;");
                    }
                }
                else if (GV.sUserType == "Manager" || GV.sUserType == "QC" || GV.sUserType == "Admin")
                {
                    if (txtSourceORAgent.Text.Trim().Length > 0)
                    {
                        dtEmails =
                            GV.MYSQL.BAL_ExecuteQueryMySQL(
                                "SELECT B.MASTER_ID,B.CONTACT_ID_P,B.First_Name,B.LAST_NAME,A.EMAIL,A.DESCRIPTION,A.DETAIL,A.CREATED_BY,A.PROCESSED_SERVER,A.CREATED_DATE,A.EMAIL_SOURCE FROM c_email_checks A INNER JOIN " +
                                GV.sProjectID +
                                "_mastercontacts B ON A.CONTACT_ID = B.CONTACT_ID_P WHERE date(A.CREATED_DATE) = '" +
                                dtProcessDate.Value.ToString("yyyy-MM-dd") + "' AND A.CREATED_BY = '" +
                                txtSourceORAgent.Text.Trim().Replace("'", "''") + "' AND A.PROJECT_ID = '" +
                                GV.sProjectID +
                                "' AND A.REPROCESSED = 0 ORDER BY B.MASTER_ID,B.CONTACT_ID_P;");
                    }
                    else
                    {
                        dtEmails =
                            GV.MYSQL.BAL_ExecuteQueryMySQL(
                                "SELECT B.MASTER_ID,B.CONTACT_ID_P,B.First_Name,B.LAST_NAME,A.EMAIL,A.DESCRIPTION,A.DETAIL,A.CREATED_BY,A.PROCESSED_SERVER,A.CREATED_DATE,A.EMAIL_SOURCE FROM c_email_checks A INNER JOIN " +
                                GV.sProjectID +
                                "_mastercontacts B ON A.CONTACT_ID = B.CONTACT_ID_P WHERE date(A.CREATED_DATE) = '" +
                                dtProcessDate.Value.ToString("yyyy-MM-dd") + "' AND A.PROJECT_ID = '" + GV.sProjectID +
                                "' AND A.REPROCESSED = 0 ORDER BY B.MASTER_ID,B.CONTACT_ID_P;");
                    }
                }
                //else if (GV.sUserType == "Admin")
                //{
                //    dtEmails =
                //        GV.MYSQL.BAL_ExecuteQueryMySQL(
                //            "SELECT B.MASTER_ID,B.First_Name,B.LAST_NAME,A.EMAIL,A.DESCRIPTION, A.CREATED_BY FROM c_email_checks A INNER JOIN " +
                //            GV.sProjectID +
                //            "_mastercontacts B ON A.CONTACT_ID = B.CONTACT_ID_P WHERE date(A.CREATED_DATE) = '" +
                //            dtProcessDate.Value.ToString("yyyy-MM-dd") + "' AND A.PROJECT_ID = '" + GV.sProjectID + "';");                
                //}

                dtEmails.Columns.Add("Status", typeof (byte[]));
                dtEmails.Columns.Add("Bounce");
                byte[] Pending, Complete, Hard, Soft;

                using (var memoryStream = new MemoryStream()) //Grey Icon
                {
                    Properties.Resources.Grey_Ball_icon__2_.Save(memoryStream, ImageFormat.Png);
                    Pending = memoryStream.ToArray();
                }

                using (var memoryStream = new MemoryStream()) //Red Icon
                {
                    Properties.Resources.Red_Ball_icon.Save(memoryStream, ImageFormat.Png);
                    Hard = memoryStream.ToArray();
                }

                using (var memoryStream = new MemoryStream()) //Green Icon
                {
                    Properties.Resources.Green_Ball_icon.Save(memoryStream, ImageFormat.Png);
                    Complete = memoryStream.ToArray();
                }

                using (var memoryStream = new MemoryStream()) //Blue Icon
                {
                    Properties.Resources.Blue_Ball_icon.Save(memoryStream, ImageFormat.Png);
                    Soft = memoryStream.ToArray();
                }


                foreach (DataRow dr in dtEmails.Rows)
                {
                    if (dr["DESCRIPTION"].ToString().Length > 0)
                    {
                        DataRow[] drrEmailStatus =
                            dtStatus.Select("PicklistField = '" + dr["DESCRIPTION"].ToString().Replace("'", "''") + "'");
                        if (drrEmailStatus.Length > 0)
                        {
                            if (drrEmailStatus[0]["PicklistValue"].ToString().ToUpper() == "HARD")
                            {
                                dr["Status"] = Hard;
                                dr["Bounce"] = "Hard";
                            }
                            else if (drrEmailStatus[0]["PicklistValue"].ToString().ToUpper() == "SOFT")
                            {
                                dr["Status"] = Soft;
                                dr["Bounce"] = "Soft";
                            }
                            else if (drrEmailStatus[0]["PicklistValue"].ToString().ToUpper() == "OK")
                            {
                                dr["Status"] = Complete;
                                dr["Bounce"] = "Passed";
                            }
                        }
                    }
                    else
                    {
                        dr["Status"] = Pending;
                        dr["Bounce"] = "Pending";
                    }
                }

                //sdgvEmails.PrimaryGrid.DataSource = dtEmails;
                LoadGrid(dtEmails);
                //  sdgvEmails.PrimaryGrid.Rows.Clear();            

                //foreach (DataRow dr in dtEmails.Rows)
                //{
                //    GridRow gridRow = new GridRow();
                //    GridCell GC1 = new GridCell();
                //    GridCell GC2 = new GridCell();
                //    GridCell GC3 = new GridCell();
                //    GridCell GC4 = new GridCell();
                //    GridCell GC5 = new GridCell();
                //    GridCell GC6 = new GridCell();
                //    GridCell GC7 = new GridCell();
                //    GridCell GC8 = new GridCell();
                //    GridCell GC9 = new GridCell();
                //    GridCell GC10 = new GridCell();                
                //}
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            Load_Emails();
        }

        void LoadChart(DataTable dtChartData)
        {

            try
            {
                chartBounce.Series[0].Points.Clear();

                chartBounce.Titles[1].Text = "Total Emails : " + dtChartData.Rows.Count;

                double Soft = dtChartData.Select("Bounce = 'Soft'").Length;
                double Hard = dtChartData.Select("Bounce = 'Hard'").Length;
                double Passed = dtChartData.Select("Bounce = 'Passed'").Length;
                double Notchecked = dtChartData.Select("Bounce = 'Pending'").Length;

                SeriesPoint seriesPoint1 = new SeriesPoint("Soft Bounce", new object[] {((object) (Soft))}, 0);
                SeriesPoint seriesPoint2 = new SeriesPoint("Hard Bounce", new object[] {((object) (Hard))}, 3);
                SeriesPoint seriesPoint3 = new SeriesPoint("Passed", new object[] {((object) (Passed))}, 4);
                SeriesPoint seriesPoint4 = new SeriesPoint("Not Checked", new object[] {((object) (Notchecked))}, 5);

                chartBounce.Series[0].Points.AddRange(new SeriesPoint[]
                {
                    seriesPoint1,
                    seriesPoint2,
                    seriesPoint3,
                    seriesPoint4
                });
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }

        }

        void LoadGrid(DataTable dtGridData)
        {
            try
            {
                List<string> lstBounceTypes = new List<string>();
                if (chkHard.Checked) lstBounceTypes.Add("Hard");
                if (chkSoft.Checked) lstBounceTypes.Add("Soft");
                if (chkPassed.Checked) lstBounceTypes.Add("Passed");
                if (chkNotChecked.Checked) lstBounceTypes.Add("Pending");

                if (lstBounceTypes.Count > 0)
                {
                    string sAgentFilter = string.Empty;
                    if (txtSourceORAgent.Visible && txtSourceORAgent.WatermarkText == "Select Agent" &&
                        txtSourceORAgent.Text.Length > 0)
                        sAgentFilter = "AND CREATED_BY = '" + txtSourceORAgent.Text + "'";
                    DataRow[] drrFiltered =
                        dtGridData.Select(
                            "Bounce IN(" + GM.ListToQueryString(lstBounceTypes, "String") + ") " + sAgentFilter,
                            "MASTER_ID ASC");
                    if (drrFiltered.Length > 0)
                        sdgvEmails.PrimaryGrid.DataSource = drrFiltered.CopyToDataTable();
                    else
                        sdgvEmails.PrimaryGrid.DataSource = null;

                    if (sAgentFilter.Length == 0)
                        LoadChart(dtGridData);
                    else
                        LoadChart(dtGridData.Select("CREATED_BY = '" + txtSourceORAgent.Text + "'").CopyToDataTable());
                }
                else
                    sdgvEmails.PrimaryGrid.DataSource = null;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }

        }

        private void txtImportPath_ButtonCustomClick(object sender, EventArgs e)
        {
            if (openFileEmails.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                txtImportPath.Text = openFileEmails.FileName;
        }        

        private void txtImportSource_ButtonCustomClick(object sender, EventArgs e)
        {
            frmComboList objfrmComboList = new frmComboList(); //Custom Designed Combobox replacement
            objfrmComboList.TitleText = "Select source";
            objfrmComboList.dtItems = dtImportSources;
            objfrmComboList.lstColumnsToDisplay.Add("PicklistValue");
            objfrmComboList.sColumnToSearch = "PicklistValue";
            objfrmComboList.IsMultiSelect = false;
            objfrmComboList.IsSingleWordSelection = false;
            objfrmComboList.IsSpellCheckEnabeld = false;
            objfrmComboList.ShowDialog(this);
            txtImportSource.Text = objfrmComboList.sReturn.Trim();
        }

        private void txtSourceORAgent_ButtonCustomClick(object sender, EventArgs e)
        {
            if (txtSourceORAgent.WatermarkText == "Select Source")
            {
                frmComboList objfrmComboList = new frmComboList(); //Custom Designed Combobox replacement
                objfrmComboList.TitleText = "Select source";
                objfrmComboList.dtItems = dtImportSources;
                objfrmComboList.lstColumnsToDisplay.Add("PicklistValue");
                objfrmComboList.sColumnToSearch = "PicklistValue";
                objfrmComboList.IsMultiSelect = false;
                objfrmComboList.IsSingleWordSelection = false;
                objfrmComboList.IsSpellCheckEnabeld = false;
                //objfrmComboList.ShowDialog(this);
                if (DialogResult.OK == objfrmComboList.ShowDialog(this))
                {
                    txtImportSource.Text = objfrmComboList.sReturn.Trim();
                }
            }
            else
            {

                frmComboList objfrmComboList = new frmComboList(); //Custom Designed Combobox replacement
                objfrmComboList.TitleText = "Select Agent Name";
                objfrmComboList.dtItems = dtEmails.DefaultView.ToTable(true, "CREATED_BY");;
                objfrmComboList.lstColumnsToDisplay.Add("CREATED_BY");
                objfrmComboList.sColumnToSearch = "CREATED_BY";
                objfrmComboList.IsMultiSelect = false;
                objfrmComboList.IsSingleWordSelection = false;
                objfrmComboList.IsSpellCheckEnabeld = false;
                if (DialogResult.OK == objfrmComboList.ShowDialog(this))
                {
                    txtSourceORAgent.Text = objfrmComboList.sReturn.Trim();
                    LoadGrid(dtEmails);
                }
                

            }            
        }

        private void txtSourceORAgent_ButtonCustom2Click(object sender, EventArgs e)
        {
            if (txtSourceORAgent.Text.Length > 0)
            {
                txtSourceORAgent.Text = string.Empty;
                LoadGrid(dtEmails);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtImportPath.Text))
            {
                try
                {
                    using (DataTable dtEmailCSV = new DataTable())
                    {
                        using (
                            TextFieldParser csvReader = new TextFieldParser(txtImportPath.Text,
                                System.Text.Encoding.Default))
                        {
                            csvReader.SetDelimiters(",");
                            csvReader.HasFieldsEnclosedInQuotes = true;

                            string[] colFields = csvReader.ReadFields();
                            foreach (string column in colFields)
                            {
                                DataColumn datecolumn = new DataColumn(column.ToUpper());
                                datecolumn.AllowDBNull = true;
                                dtEmailCSV.Columns.Add(datecolumn);
                            }

                            while (!csvReader.EndOfData)
                            {
                                string[] fieldData = csvReader.ReadFields();
                                //Making empty value as null
                                //for (int i = 0; i < fieldData.Length; i++)
                                //{
                                //    if (fieldData[i] == "")
                                //    {
                                //        fieldData[i] = null;
                                //    }
                                //}
                                dtEmailCSV.Rows.Add(fieldData);
                            }
                        }

                        if (!dtEmailCSV.Columns.Contains("EMAIL"))
                        {
                            ToastNotification.Show(this, "EMAIL Column not found", eToastPosition.TopRight);
                            return;
                        }

                        string sInsertString = string.Empty;
                        foreach (DataRow drImport in dtEmailCSV.Rows)
                        {
                            if (drImport["EMAIL"].ToString().Trim().Length > 0)
                                sInsertString += ",('" + GV.sProjectID + "',0,'" +
                                                 txtImportSource.Text.Trim().Replace("'", "''") + "', '" +
                                                 drImport["EMAIL"].ToString().Trim() + "','" + GV.sEmployeeName +
                                                 "', NOW())";
                        }

                        if (sInsertString.Trim().Length > 0)
                        {
                            sInsertString =
                                "INSERT INTO c_email_checks (PROJECT_ID, CONTACT_ID, EMAIL_SOURCE, EMAIL, CREATED_BY, CREATED_DATE) Values " +
                                sInsertString.Substring(1);
                            GV.MYSQL.BAL_ExecuteNonReturnQueryMySQL(sInsertString);

                            if (
                                GV.MYSQL.BAL_ExecuteQueryMySQL(
                                    "SELECT 1 FROM crupro001_picklists WHERE PicklistCategory = 'EmailSource' AND PicklistValue = '" +
                                    txtImportSource.Text.Trim().Replace("'", "''") + "';").Rows.Count == 0)
                            {
                                //Insert for new source
                                GV.MYSQL.BAL_ExecuteNonReturnQueryMySQL(
                                    "INSERT INTO crupro001_picklists (PicklistCategory, PicklistValue)VALUES ('EmailSource','" +
                                    txtImportSource.Text.Trim().Replace("'", "''") + "');");

                                //Refresh Source table
                                dtImportSources =
                                    GV.MYSQL.BAL_ExecuteQueryMySQL(
                                        "SELECT PicklistCategory,PicklistValue FROM crupro001_picklists ORDER BY ID DESC LIMIT 100;");
                            }

                            txtImportPath.Text = string.Empty;
                            txtImportSource.Text = string.Empty;
                            ToastNotification.Show(this, "Emails imported and scheduled successfully.",
                                eToastPosition.TopRight);
                        }
                        else
                            ToastNotification.Show(this, "Imported file doesn't have data.", eToastPosition.TopRight);
                    }
                }
                catch (Exception ex)
                {
                    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                }
            }
            else
            {
                ToastNotification.Show(this, "File does not exist.", eToastPosition.TopRight);
            }
        }

        private void sdgvEmails_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
           
            
        }

        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            LoadGrid(dtEmails);
        }

        private void sdgvEmails_RowDoubleClick(object sender, GridRowDoubleClickEventArgs e)
        {
            try
            {
                GridRow GRow = (GridRow) e.GridRow;
                string sID = GRow[1].Value.ToString();
                if (sID.Length > 0)
                    GM.OpenContactUpdate(sID, false, true, this, null);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        private void pictureBoxHard_Click(object sender, EventArgs e)
        {
            chkHard.Checked = !chkHard.Checked;
        }

        private void lblLegendHard_Click(object sender, EventArgs e)
        {
            chkHard.Checked = !chkHard.Checked;
        }

        private void pictureBoxSoft_Click(object sender, EventArgs e)
        {
            chkSoft.Checked = !chkSoft.Checked;
        }

        private void lblLegendSoft_Click(object sender, EventArgs e)
        {
            chkSoft.Checked = !chkSoft.Checked;
        }

        private void pictureBoxPassed_Click(object sender, EventArgs e)
        {
            chkPassed.Checked = !chkPassed.Checked;
        }

        private void lblLegendPassed_Click(object sender, EventArgs e)
        {
            chkPassed.Checked = !chkPassed.Checked;
        }

        private void pictureBoxNotChecked_Click(object sender, EventArgs e)
        {
            chkNotChecked.Checked = !chkNotChecked.Checked;
        }

        private void lblLegendNotChecked_Click(object sender, EventArgs e)
        {
            chkNotChecked.Checked = !chkNotChecked.Checked;
        }        
    }
}
