using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace GCC
{
    public partial class frmEmailHunter : DevComponents.DotNetBar.Office2007Form
    {
        public frmEmailHunter()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new System.Drawing.Font(this.Font.FontFamily, 22);

            //cmbServerID.DataSource = GV.MYfSQL.BAL_ExecuteQueryMyfdSQL("SELECT SERVER_ID FROM c_email_server WHERE SERVER_TYPE='MANUAL' AND ACTIVE = 'Y' ORDER BY SERVER_ID;");
            //cmbServerID.DisplayMember = "SERVER_ID";
            //cmbServerID.ValueMember = "SERVER_ID";            
            dTimeMonthRange.Value = GM.GetDateTime();
            Reload();
        }

        DataTable dtBatch;

        
        void Reload()
        {
            string sQuery = @"select A.*,(select count(*) from c_email_hunter where batchid = a.ID and Status = 'Completed')'Completed',(select count(*) from c_email_hunter where batchid = a.ID)'Total' from c_email_hunter_batch A  where MONTH(A.LOADEDDATE) = '" + dTimeMonthRange.Value.Month + "' AND YEAR(A.LOADEDDATE) = '" + dTimeMonthRange.Value.Year + "' ORDER BY A.ID DESC;";

            

            dtBatch = GV.MSSQL1.BAL_ExecuteQuery(sQuery);
            sdgvManualChecks.PrimaryGrid.DataSource = dtBatch;

            DataTable dtServerLoad = GV.MSSQL1.BAL_ExecuteQuery(@"select count(*)'Load' from c_email_hunter where isnull(Status,'') ='';");

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
                        if(GV.MSSQL1.BAL_ExecuteQuery("SELECT 1 FROM c_email_hunter_batch WHERE BATCHNAME = '" + txtBatchName.Text.Trim().Replace("'", "''") + "';").Rows.Count > 0)
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

                        if (!dtEmailCSV.Columns.Contains("Domain"))
                        {
                            ToastNotification.Show(this, "Domain Column not found", eToastPosition.TopRight);
                            return;
                        }

                        if (!dtEmailCSV.Columns.Contains("FirstName"))
                        {
                            ToastNotification.Show(this, "FirstName Column not found", eToastPosition.TopRight);
                            return;
                        }

                        if (!dtEmailCSV.Columns.Contains("LastName"))
                        {
                            ToastNotification.Show(this, "LastName Column not found", eToastPosition.TopRight);
                            return;
                        }

                        string sInsertString = string.Empty;
                        if (dtEmailCSV.Select("LEN(ISNULL(FirstName,'')) > 0").Length > 0)
                        {
                            //if(dtEmailCSV.Select("LEN(ISNULL(EMAIL,'')) > 0").Length > 9999)
                            //{
                            //    ToastNotification.Show(this, "Cannot import more then 10000 Emails.", eToastPosition.TopRight);
                            //    return;
                            //}

                            //Insert for new source
                            string sBatchID = GV.MSSQL1.BAL_InsertAndGetIdentity("INSERT INTO c_email_hunter_batch (BATCHNAME, FILENAME, LOADEDBY, LOADEDDATE)VALUES ('" + txtBatchName.Text.Trim().Replace("'", "''") + "', '" + Path.GetFileNameWithoutExtension(txtFileImport.Text) + "', '" + GV.sEmployeeName + "', getdate());");
                            foreach (DataRow drImport in dtEmailCSV.Rows)
                            {
                                if (drImport["Domain"].ToString().Trim().Length > 0 && drImport["FirstName"].ToString().Trim().Length > 0 && drImport["LastName"].ToString().Trim().Length > 0)
                                    sInsertString += ",('"+ drImport["FirstName"].ToString().Trim().Replace("'", "''") + "','" + drImport["LastName"].ToString().Trim().Replace("'", "''") + "', '" + drImport["Domain"].ToString().Trim().Replace("'", "''") + "', GETDATE()," + sBatchID + ")";
                            }
                            
                            sInsertString = "INSERT INTO c_email_hunter (FirstName, LastName, DomainName,LoadedDate,BatchID) Values " + sInsertString.Substring(1);
                            GV.MSSQL1.BAL_ExecuteNonReturnQuery(sInsertString);

                            GV.MSSQL1.BAL_ExecuteNonReturnQuery(@"update t1 set t1.email=t2.email,t1.score=t2.score,t1.source=t2.source,t1.emailpattern=t2.emailpattern,t1.status='COMPLETED',t1.processeddate=getdate()  from c_email_hunter t1 join (select * from c_email_hunter where batchid <>'" + sBatchID + "' and status is not null)t2 on t1.firstname=t2.firstname and t1.lastname=t2.lastname and t1.domainname=t2.domainname where t1.status is null  and t1.BatchID='" + sBatchID + "'");

                            dtBatch = GV.MSSQL1.BAL_ExecuteQuery("SELECT * FROM c_email_hunter_batch");
                            sdgvManualChecks.PrimaryGrid.DataSource = dtBatch;


                            txtFileImport.Text = string.Empty;
                            txtBatchName.Text = string.Empty;
                            ToastNotification.Show(this, "Imported and scheduled successfully.", eToastPosition.TopRight);
                            
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
                using (SqlConnection conMSSQL = new SqlConnection(GV.sMSSQL1))
                {
                    string sQueryData = "SELECT " + sColumns + sQueryString;
                    string sQueryCount = "SELECT COUNT(*) " + sQueryString;
                    lblExportDisplayPercent.Invoke((MethodInvoker)delegate { lblExportDisplayPercent.Text = "Fetching data..."; });
                    DataTable dtRowCount = GV.MSSQL1.BAL_ExecuteQuery(sQueryCount);
                    double iTotalRowCount = Convert.ToDouble(dtRowCount.Rows[0][0]);
                    conMSSQL.Open();
                    //MydfSqlCommand cmdExport = new MydfSqlCommand("SET GLOBAL   net_read_timeout = 100;" + sQueryData, conMdfYSQL);
                    SqlCommand cmdExport = new SqlCommand(sQueryData, conMSSQL);
                    lblExportDisplayPercent.Invoke(
                        (MethodInvoker)delegate { lblExportDisplayPercent.Text = "Estimated Row count :" + iTotalRowCount; });
                    //if (GV.conMfdYSQL.State != ConnectionState.Open)
                    //    GV.conMYfdSQL.Open();
                    SqlDataReader rdrExport = cmdExport.ExecuteReader();

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
                        //GV.conMfdYSQL.Close();
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
                        ExportDocFormat_OpenXML(" FROM c_email_hunter WHERE BatchID='" + ((DevComponents.DotNetBar.SuperGrid.GridRow)e.GridRow).Cells["ID"].Value + "';", "ID,FirstName, LastName,DomainName,Email,CompanyName,Score,Source,Status,LoadedDate,ProcessedDate,BatchID", fileSavePath.FullName);
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

        private void frmEmailHunter_Load(object sender, EventArgs e)
        {

        }
    }
}
