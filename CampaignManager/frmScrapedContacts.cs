﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.SuperGrid.Style;


namespace GCC
{
    public partial class frmScrapedContacts : Office2007Form
    {
        public frmScrapedContacts()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 1000;
            ToastNotification.ToastFont = new Font(Font.FontFamily, 15);
            
            GridColumn ColStatus = new GridColumn("Action");
            ColStatus.DataPropertyName = "Action";
            ColStatus.ReadOnly = false;
            ColStatus.EditorType = typeof(GridStateEditControl);
            sdgvScrapedContacts.PrimaryGrid.Columns.Add(ColStatus);

            dateLoad.Value = DateTime.Today;
            
            //GridColumn ColID = new GridColumn("MASTER_ID");
            //ColStatus.DataPropertyName = "MASTER_ID";            
            //sdgvScrapedContacts.PrimaryGrid.Columns.Add(ColID);

        }

        DataTable dtScraped;        

        private void frmScrapedContacts_Load(object sender, EventArgs e)
        {            
            Load_Tables();            
        }

        void Load_Tables()
        {
            try
            {
                dtScraped =
                    GV.MYSQL.BAL_ExecuteQueryMySQL(
                        "SELECT B.MASTER_ID, B.COMPANY_NAME, A.CONTACT_ID_P, A.DM_CompanyName, A.FIRST_NAME, A.LAST_NAME,A.TITLE,A.JOB_TITLE, A.OTHERS_JOBTITLE,A.CONTACT_EMAIL,A.CONTACT_LINK,A.CONTACT_CITY,A.CONTACT_STATE,A.CONTACT_COUNTRY,A.CREATED_BY,A.CREATED_DATE,A.Scrape_Date,A.Contact_Industry FROM " +
                        GV.sProjectID + "_mastercontacts A INNER JOIN " + GV.sProjectID +
                        "_mastercompanies B ON A.MASTER_ID = B.MASTER_ID WHERE A.SCRAPE_STATUS = 1 AND A.CREATED_BY = '" +
                        GV.sEmployeeName + "' AND DATE(A.CREATED_DATE) = '" + dateLoad.Value.ToString("yyyy-MM-dd") + "';");

                dtScraped.Columns.Add("Action");
                dtScraped.Columns.Add("Color");
                dtScraped.Columns.Add("CONLINK");
               
                string sPreviousMasterID = string.Empty;
                string sPreviousColor = "";
                foreach (DataRow drColor in dtScraped.Rows)
                {
                    string ID = drColor["MASTER_ID"].ToString();
                    if (sPreviousMasterID == ID)
                        drColor["Color"] = sPreviousColor;
                    else
                    {
                        sPreviousColor = GetColor(sPreviousColor);
                        drColor["Color"] = sPreviousColor;
                        sPreviousMasterID = ID;
                    }

                    drColor["Action"] = "2";
                    if (drColor["CONTACT_LINK"].ToString().Length > 0)
                        drColor["CONLINK"] = "<a href = '" + drColor["CONTACT_LINK"] + "' >" + drColor["CONTACT_LINK"] + "</a>";
                }
                sdgvScrapedContacts.PrimaryGrid.DataSource = dtScraped;

                sdgvScrapedContacts.PrimaryGrid.Caption.Text = "Researched Contacts (" + dtScraped.Rows.Count + ")";

                //if (dtScraped.Rows.Count > 0)
                //    ToastNotification.Show(this, "Contacts loaded for selected date.", eToastPosition.TopRight);
                //else
                //    ToastNotification.Show(this, "No contacts found on selected date.", eToastPosition.TopRight);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);                
            }
        }
        
        string GetColor(string sCurrentColor)
        {
            if (sCurrentColor == string.Empty || sCurrentColor == "White")
                return "Beige";
            if (sCurrentColor == "Beige")
                return "White";

            //if (sCurrentColor == string.Empty)
            //    return "Azure";
            //if (sCurrentColor == "Azure")
            //    return "MistyRose";
            //if (sCurrentColor == "MistyRose")
            //    return "GhostWhite";
            //if (sCurrentColor == "GhostWhite")
            //    return "LightGoldenRodYellow";
            //if (sCurrentColor == "LightGoldenRodYellow")
            //    return "HoneyDew";
            //if (sCurrentColor == "HoneyDew")
            //    return "LavenderBlush";
            //if (sCurrentColor == "LavenderBlush")
            //    return "Azure";

            return "White";

        }
        

        private void btnLoad_Click(object sender, EventArgs e)
        {
            Load_Tables();            
        }

        private void btnDeleteContacts_Click(object sender, EventArgs e)
        {
            DataRow[] drrScraped = dtScraped.Select("Action = '4'");
            if (drrScraped.Length > 0)
            {
                if (DialogResult.Yes == MessageBoxEx.Show("Are you sure to delete the selected " + drrScraped.Length + " contact(s) ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    try
                    {
                        List<int> lstDeleteContactStatus = new List<int>();
                        DataTable dtRejectedContacts = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM c_RejectedContacts WHERE 1 = 0;");
                        foreach (DataRow drScraped in drrScraped)
                        {
                            lstDeleteContactStatus.Add(Convert.ToInt32(drScraped["CONTACT_ID_P"]));
                            DataRow drNewRow = dtRejectedContacts.NewRow();
                            //foreach (DataColumn dc in dtRejectedContacts.Columns)
                            //{
                            //    if(dc.ColumnName == "ID")
                            //        continue;
                            //    if (drrScraped.CopyToDataTable().Columns.Contains(dc.ColumnName))
                            //        drNewRow[dc.ColumnName] = drScraped[dc.ColumnName].ToString();
                            //}
                            drNewRow["CONTACT_ID_P"] = drScraped["CONTACT_ID_P"].ToString();
                            drNewRow["MASTER_ID"] = drScraped["MASTER_ID"].ToString();
                            drNewRow["TITLE"] = drScraped["TITLE"].ToString();
                            drNewRow["FIRST_NAME"] = drScraped["FIRST_NAME"].ToString();
                            drNewRow["LAST_NAME"] = drScraped["LAST_NAME"].ToString();
                            drNewRow["JOB_TITLE"] = drScraped["JOB_TITLE"].ToString();
                            drNewRow["OTHERS_JOBTITLE"] = drScraped["OTHERS_JOBTITLE"].ToString();
                            drNewRow["CONTACT_EMAIL"] = drScraped["CONTACT_EMAIL"].ToString();
                            drNewRow["CONTACT_LINK"] = drScraped["CONTACT_LINK"].ToString();
                            drNewRow["CONTACT_CITY"] = drScraped["CONTACT_CITY"].ToString();
                            drNewRow["CONTACT_STATE"] = drScraped["CONTACT_STATE"].ToString();
                            drNewRow["CONTACT_COUNTRY"] = drScraped["CONTACT_COUNTRY"].ToString();
                            drNewRow["CREATED_BY"] = drScraped["CREATED_BY"].ToString();
                            if (drScraped["CREATED_DATE"].ToString().Length > 0) drNewRow["CREATED_DATE"] = drScraped["CREATED_DATE"].ToString();
                            if(drScraped["Scrape_Date"].ToString().Length > 0) drNewRow["Scrape_Date"] = drScraped["Scrape_Date"].ToString();
                            drNewRow["Contact_CompanyName"] = drScraped["DM_CompanyName"].ToString();
                            drNewRow["Contact_Industry"] = drScraped["Contact_Industry"].ToString();
                            drNewRow["REJECTED_BY"] = GV.sEmployeeName;
                            drNewRow["REJECTED_DATE"] = GM.GetDateTime();
                            drNewRow["PROJECTID"] = GV.sProjectID;
                            dtRejectedContacts.Rows.Add(drNewRow);
                        }

                        if (GV.MYSQL.BAL_SaveToTableMySQL(dtRejectedContacts, "c_RejectedContacts", "New", true))
                        {
                            string sDeleteString = GM.ListToQueryString(lstDeleteContactStatus, "Int");
                            if (sDeleteString.Length > 0)
                            {
                                GV.MYSQL.BAL_ExecuteQueryMySQL("DELETE FROM " + GV.sProjectID + "_mastercontacts WHERE CONTACT_ID_P IN (" + sDeleteString + ") AND Scrape_status = 1;");
                                ToastNotification.Show(this, "Contacts removed successfully.", eToastPosition.TopRight);
                                Load_Tables();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                    }
                }
            }
            else
                ToastNotification.Show(this, "No contacts marked for delete.", eToastPosition.TopRight);
        }

        private void sdgvScrapedContacts_PreRenderRow(object sender, GridPreRenderRowEventArgs e)
        {
            Background b = new Background(Color.FromName(((GridRow)e.GridRow)["Color"].Value.ToString()));
            e.GridRow.CellStyles.Default.Background = b;
        }

        private void sdgvScrapedContacts_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridColumn DC in sdgvScrapedContacts.PrimaryGrid.Columns)
            {
                if (DC.Name == "Action")
                {
                    //DC.Width = 25;
                    DC.FillWeight = 7;
                    DC.DisplayIndex = 0;
                }
                else
                {
                    DC.HeaderText = GM.ProperCase(DC.Name.Replace("_", " "));
                    DC.ReadOnly = true;

                    if (DC.Name.ToUpper() == "MASTER_ID")
                    {
                        DC.DisplayIndex = 1;
                        DC.HeaderText = "ID";
                        DC.FillWeight = 10;
                    }
                    else if (DC.Name.ToUpper() == "COMPANY_NAME")
                    {
                        DC.DisplayIndex = 2;
                        DC.HeaderText = "Input Company";
                        DC.FillWeight = 40;
                    }
                    else if (DC.Name.ToUpper() == "DM_COMPANYNAME")
                    {
                        DC.DisplayIndex = 3;
                        DC.HeaderText = "Output Company";
                        DC.FillWeight = 40;
                    }
                    else if (DC.Name.ToUpper() == "OTHERS_JOBTITLE")
                    {
                        DC.DisplayIndex = 4;
                        DC.HeaderText = "Jobtitle";
                        DC.FillWeight = 50;
                    }
                    else if (DC.Name.ToUpper() == "FIRST_NAME")
                    {
                        DC.DisplayIndex = 5;
                        DC.FillWeight = 20;
                    }
                    else if (DC.Name.ToUpper() == "LAST_NAME")
                    {
                        DC.DisplayIndex = 6;
                        DC.FillWeight = 20;
                    }
                    else if (DC.Name.ToUpper() == "CONTACT_LINK")
                    {
                        DC.DisplayIndex = 7;
                        DC.FillWeight = 70;

                        Font X = new Font(sdgvScrapedContacts.Font.FontFamily, sdgvScrapedContacts.Font.Size, FontStyle.Underline);
                        DC.CellStyles.Default.Font = X;
                        DC.CellStyles.Default.TextColor = Color.Blue;
                    }
                    else
                    {
                        DC.Visible = false;
                    }                    
                }
            }
        }

        private void sdgvScrapedContacts_CellMouseEnter(object sender, GridCellEventArgs e)
        {
            if(e.GridCell.GridColumn.Name.ToUpper() == "CONTACT_LINK" && e.GridCell.Value.ToString().Length > 0)
                sdgvScrapedContacts.Cursor = Cursors.Hand;
            else
                sdgvScrapedContacts.Cursor = Cursors.Default;
            


        }

        private void sdgvScrapedContacts_CellClick(object sender, GridCellClickEventArgs e)
        {
            if (e.GridCell.GridColumn.Name.ToUpper() == "CONTACT_LINK" && e.GridCell.Value.ToString().Length > 0)
            {
                Process.Start(e.GridCell.Value.ToString());
            }
        }        
    }
}
