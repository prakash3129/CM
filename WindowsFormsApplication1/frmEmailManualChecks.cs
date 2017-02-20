using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MySql.Data.MySqlClient;

namespace GCC
{
    public partial class frmEmailManualChecks : DevComponents.DotNetBar.Office2007Form
    {
        public frmEmailManualChecks()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new System.Drawing.Font(this.Font.FontFamily, 22);

            cmbServerID.DataSource = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT SERVER_ID FROM c_email_server WHERE SERVER_TYPE='MANUAL' AND ACTIVE = 'Y' ORDER BY SERVER_ID;");
            cmbServerID.DisplayMember = "SERVER_ID";
            cmbServerID.ValueMember = "SERVER_ID";            
            dTimeMonthRange.Value = GM.GetDateTime();
            Reload();
        }

        DataTable dtBatch;

        
        void Reload()
        {
            string sQuery = @"SELECT A.ID,A.PROCESS_SERVER_ID,A.BATCH_NAME,A.STARTED_DATE,A.COMPLETED_DATE,A.FILE_NAME,A.LOADED_DATE,A.LOADED_BY,A.EXPORTED_DATE,A.EXPORTED_BY, COUNT(*) `Loaded`,(SELECT COUNT(*) FROM c_email_checks C WHERE C.PROJECT_ID = '0' AND C.CONTACT_ID = 0 AND C.EMAIL_SOURCE = B.EMAIL_SOURCE AND C.DESCRIPTION IS NOT NULL) `Processed`
                            FROM c_email_batch_setting A INNER JOIN c_email_checks B ON A.ID = B.EMAIL_SOURCE WHERE A.ID > 0 AND B.PROJECT_ID='0' AND B.CONTACT_ID = 0 AND MONTH(LOADED_DATE) = '" + dTimeMonthRange.Value.Month +"' AND YEAR(LOADED_DATE) = '"+ dTimeMonthRange.Value.Year + "' GROUP BY  A.ID, A.PROCESS_SERVER_ID, A.BATCH_NAME, A.STARTED_DATE, A.COMPLETED_DATE, A.FILE_NAME, A.LOADED_DATE, A.LOADED_BY, A.EXPORTED_DATE, A.EXPORTED_BY ORDER BY IFNULL(A.STARTED_DATE,'2099-01-01 00:00:00') DESC;";

            dtBatch = GV.MYSQL.BAL_ExecuteQueryMySQL(sQuery);
            sdgvManualChecks.PrimaryGrid.DataSource = dtBatch;

            DataTable dtServerLoad = GV.MYSQL.BAL_ExecuteQueryMySQL(@"SELECT CONVERT(CONCAT('Server', PROCESS_SERVER_ID,' : ', SUM(PENDING)),char) AS `Load` FROM (                                                                    
                                                                    SELECT * FROM (SELECT C1.PROCESS_SERVER_ID, C1.ID,(SELECT COUNT(*) FROM c_email_checks C2 WHERE C2.PROJECT_ID='0' AND C2.CONTACT_ID = 0 AND C1.ID=C2.EMAIL_SOURCE AND C2.DESCRIPTION IS NULL) PENDING                                                                       
                                                                    FROM c_email_batch_setting C1 UNION SELECT '3' AS PROCESS_SERVER_ID, 'X' AS ID, COUNT(*) FROM c_email_checks C3 WHERE C3.DESCRIPTION IS NULL AND C3.EMAIL_SOURCE > 0) AS X 
                                                                    GROUP BY X.PROCESS_SERVER_ID,X.ID) T GROUP BY PROCESS_SERVER_ID;");

            lstServerList.ValueMember = "Load";
            lstServerList.DisplayMember = "Load";
            lstServerList.DataSource = dtServerLoad;
            
            

        }
        private void txtFileImport_ButtonCustomClick(object sender, EventArgs e)
        {
            if (openFileEmails.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                txtFileImport.Text = openFileEmails.FileName;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtFileImport.Text))
            {
                try
                {
                    if(txtBatchName.Text.Trim().Length > 0)
                    {
                        if(GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT 1 FROM c_email_batch_setting WHERE BATCH_NAME = '" + txtBatchName.Text.Trim().Replace("'", "''") + "';").Rows.Count > 0)
                        {
                            ToastNotification.Show(this, "Batch Name already exist", eToastPosition.TopRight);
                            return;
                        }
                    }
                    else
                    {
                        ToastNotification.Show(this, "Batch Name can't be empty", eToastPosition.TopRight);
                        return;
                    }


                    using (DataTable dtEmailCSV = new DataTable())
                    {
                        using (
                            TextFieldParser csvReader = new TextFieldParser(txtFileImport.Text,
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
                        if (dtEmailCSV.Select("LEN(ISNULL(EMAIL,'')) > 0").Length > 0)
                        {
                            if(dtEmailCSV.Select("LEN(ISNULL(EMAIL,'')) > 0").Length > 9999)
                            {
                                ToastNotification.Show(this, "Cannot import more then 10000 Emails.", eToastPosition.TopRight);
                                return;
                            }

                            //Insert for new source
                            string sBatchID = GV.MYSQL.BAL_InsertAndGetIdentityMySQL("INSERT INTO c_email_batch_setting (PROCESS_SERVER_ID, BATCH_NAME, FILE_NAME, LOADED_BY, LOADED_DATE)VALUES ('" + cmbServerID.Text + "','" + txtBatchName.Text.Trim().Replace("'", "''") + "', '" + Path.GetFileNameWithoutExtension(txtFileImport.Text) + "', '" + GV.sEmployeeName + "', NOW());");
                            foreach (DataRow drImport in dtEmailCSV.Rows)
                            {
                                if (drImport["EMAIL"].ToString().Trim().Length > 0 && GM.Email_Check(drImport["EMAIL"].ToString().Trim()))
                                    sInsertString += ",('0',0,'" + sBatchID + "', '" + drImport["EMAIL"].ToString().Trim().Replace("'", "''") + "','" + GV.sEmployeeName + "', NOW())";
                            }
                            
                            sInsertString = "INSERT INTO c_email_checks (PROJECT_ID, CONTACT_ID, EMAIL_SOURCE, EMAIL, CREATED_BY, CREATED_DATE) Values " + sInsertString.Substring(1);
                            GV.MYSQL.BAL_ExecuteNonReturnQueryMySQL(sInsertString);

                            dtBatch = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM c_email_batch_setting");
                            sdgvManualChecks.PrimaryGrid.DataSource = dtBatch;


                            txtFileImport.Text = string.Empty;
                            txtBatchName.Text = string.Empty;
                            ToastNotification.Show(this, "Emails imported and scheduled successfully.", eToastPosition.TopRight);
                            
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

        private void btnReload_Click(object sender, EventArgs e)
        {
            Reload();
        }

       


        void ExportDocFormat_OpenXML(string sQueryString, string sColumns, string sPath)
        {
            double iRowCounter = 0;
            try
            {
                using (MySqlConnection conMYSQL = new MySqlConnection(GV.sMySQL))
                {
                    string sQueryData = "SELECT " + sColumns + sQueryString;
                    string sQueryCount = "SELECT COUNT(*) " + sQueryString;
                    lblExportDisplayPercent.Invoke((MethodInvoker)delegate { lblExportDisplayPercent.Text = "Fetching data..."; });
                    DataTable dtRowCount = GV.MYSQL.BAL_ExecuteQueryMySQL(sQueryCount);
                    double iTotalRowCount = Convert.ToDouble(dtRowCount.Rows[0][0]);
                    conMYSQL.Open();
                    MySqlCommand cmdExport = new MySqlCommand("SET GLOBAL   net_read_timeout = 100;" + sQueryData, conMYSQL);
                    lblExportDisplayPercent.Invoke(
                        (MethodInvoker)delegate { lblExportDisplayPercent.Text = "Estimated Row count :" + iTotalRowCount; });
                    //if (GV.conMYSQL.State != ConnectionState.Open)
                    //    GV.conMYSQL.Open();
                    MySqlDataReader rdrExport = cmdExport.ExecuteReader();

                    int iRowsPerSheetCounter = 0;
                    int iRowPerSheet = 100000;
                    DataTable dtSchema = rdrExport.GetSchemaTable();
                    DataTable dtExportData = new DataTable();
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
                        SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(sPath,
                            SpreadsheetDocumentType.Workbook);
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
                            var column = new DataColumn(columnName, (Type)(drow["DataType"]));
                            column.Unique = (bool)drow["IsUnique"];
                            column.AllowDBNull = (bool)drow["AllowDBNull"];
                            column.AutoIncrement = (bool)drow["IsAutoIncrement"];
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
                            }

                            sheetData.AppendChild(newRow);
                            iRowCounter++;
                            iRowsPerSheetCounter++;

                            //if (iRowCounter == 4194)
                            //{ }

                            lblExportDisplayPercent.Invoke(
                                (MethodInvoker)
                                    delegate { lblExportDisplayPercent.Text = "Writing " + iRowCounter + " / " + iTotalRowCount; });
                            circularProgressExport.Value = Convert.ToInt32((iRowCounter / iTotalRowCount) * 100);

                            if (iRowsPerSheetCounter == iRowPerSheet)
                            {
                                lblExportDisplayPercent.Invoke(
                                    (MethodInvoker)delegate { lblExportDisplayPercent.Text = "Splitting Worksheet..."; });
                                iRowsPerSheetCounter = 0;
                                workbookpart.Workbook.Save();
                                spreadsheetDocument.Close();
                                spreadsheetDocument = SpreadsheetDocument.Open(sPath, true);
                                workbookpart = spreadsheetDocument.WorkbookPart;

                                if (workbookpart.Workbook == null)
                                    workbookpart.Workbook = new Workbook();

                                lblExportDisplayPercent.Invoke(
                                    (MethodInvoker)delegate { lblExportDisplayPercent.Text = "Opening Worksheet..."; });
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
                                }
                                sheetData.AppendChild(rowColumnNewSheet);                                
                            }
                        }

                        lblExportDisplayPercent.Invoke((MethodInvoker)delegate { lblExportDisplayPercent.Text = "Saving Worksheet..."; });
                        workbookpart.Workbook.Save();
                        spreadsheetDocument.Close();
                        //iExcelExportStatus = 3;
                        // Call Close when done reading.

                        lblExportDisplayPercent.Invoke((MethodInvoker)delegate { lblExportDisplayPercent.Text = "Closing Worksheet..."; });
                        rdrExport.Close();
                        //GV.conMYSQL.Close();
                        lblExportDisplayPercent.Invoke(
                            (MethodInvoker)delegate { lblExportDisplayPercent.Text = "Worksheet saved sucessfully..."; });
                        //sWatch.Stop();
                        //MessageBoxEx.Show("Exported sucessfully", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        ToastNotification.Show(this, "No data to export", eToastPosition.TopRight);
                    //else            
                    //    ToastNotification.Show(this, "Filter doesn't return any row(s) or invalid filter conditions.", eToastPosition.TopRight);            
                }
            }
            catch (Exception ex)
            {                
                if (GV.sUserType == "Admin")
                    MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void sdgvManualChecks_RowDoubleClick(object sender, DevComponents.DotNetBar.SuperGrid.GridRowDoubleClickEventArgs e)
        {
            if (!bWorkerExport.IsBusy)
            {
                if (((DevComponents.DotNetBar.SuperGrid.GridRow)e.GridRow).Cells["ID"].Value != null)
                {
                    if (DialogResult.OK == saveFileDialogExportToExcel.ShowDialog())
                    {
                        System.IO.FileInfo fileSavePath = new System.IO.FileInfo(saveFileDialogExportToExcel.FileName);
                        ExportDocFormat_OpenXML(" FROM c_email_checks WHERE EMAIL_SOURCE='"+ ((DevComponents.DotNetBar.SuperGrid.GridRow)e.GridRow).Cells["ID"].Value + "';", "ID,EMAIL_SOURCE AS BATCHID, EMAIL,DESCRIPTION,DETAIL,PROCESSED_SERVER,CREATED_BY,CREATED_DATE", fileSavePath.FullName);
                        ToastNotification.Show(this, "Export sucess", eToastPosition.TopRight);
                    }
                    else
                        ToastNotification.Show(this, "Export aborted", eToastPosition.TopRight);
                }
            }
            else
                ToastNotification.Show(this, "Export in progress", eToastPosition.TopRight);
        }

        private void bWorkerExport_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void bWorkerExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
    }
}
