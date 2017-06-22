using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;

namespace GCC
{
    public partial class frmValidation : Office2007Form
    {
        public frmValidation()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
        }

        DataTable dtValidation;
        DataTable dtFieldMaster;
        private void frmValidation_Load(object sender, EventArgs e)
        {

            dtFieldMaster = GV.MSSQL1.BAL_ExecuteQuery("SELECT * from c_field_master where project_id='crucru005'");

            DataTable dtTable = dtFieldMaster.DefaultView.ToTable(true, "Table_Name");

            List<string> lst = new List<string>();
            lst.Add("asdsad");
            lst.Add("asdsadasd");
            lst.Add("asdsadasdwewe");
            lst.Add("asdsadasdwewewewe");
            lst.Add("asdsadasdwewewewewewe");
            lst.Add("asdasdr");
            lst.Add("rtrytrt");
            lst.Add("rtughjghj");
            lst.Add("wedsfsdfsd");
            lst.Add("dffdghfgh");
            lst.Add("sdkfdskg");
            tagListControl1.Tags = lst;
            
            //superGridControl1.PrimaryGrid.ShowTreeLines = true;

            

            //superGridControl1.Update();
            //superGridControl1.PrimaryGrid

            

            sdgvValidation.PrimaryGrid.DefaultRowHeight = 150;
            sdgvValidation.PrimaryGrid.Columns[0].Width = sdgvValidation.Width;
            sdgvValidation.PrimaryGrid.Columns[0].HeaderText = "Rules";
            dtValidation = GV.MSSQL1.BAL_FetchTable(GV.sProjectID + "_validations", "VALIDATION_NAME IS NOT NULL");


            for (int i = 0; i < dtValidation.Rows.Count; i++)
            {                

                //Super Grid
                string sHTMLValue = string.Empty;

                sHTMLValue += "<div align='left' width ='0'><div align='left'><font size = '11'>" + dtValidation.Rows[i]["VALIDATION_NAME"] + "</font></div><br/>";


                sHTMLValue += "<div align='Left'>If <font color = 'gray' size = '10'>" + GM.ProperCase_ProjectSpecific(dtValidation.Rows[i]["VALIDATION_FOR"].ToString()) + "</font> is <font color = 'gray' size = '10'>" + GM.ProperCase_ProjectSpecific(dtValidation.Rows[i]["Condition"].ToString()) + "</font></div>";

                //sHTMLValue += "<div align='left'><font color = 'Gray'>" + sContactEmail + "</font></div>";

                switch (dtValidation.Rows[i]["VALIDATION_TYPE"].ToString())
                {
                    case "MANDATORYFIELDS":
                        sHTMLValue += "<div align = 'left'><font color = 'gray'>" + GM.ProperCase_ProjectSpecific(dtValidation.Rows[i]["VALIDATION_VALUE"].ToString()) + "</font> is required</div>";
                        break;

                    case "EMAILCOMPANYCHECK":
                        sHTMLValue += "<div align = 'left'><font color = 'gray'>" + GM.ProperCase_ProjectSpecific(dtValidation.Rows[i]["VALIDATION_VALUE"].ToString()) + "</font> will be checked for Company email format.</div>";
                        break;

                    case "EMAILPUBLICDOMAINCHECK":
                        sHTMLValue += "<div align = 'left'><font color = 'gray'>" + GM.ProperCase_ProjectSpecific(dtValidation.Rows[i]["VALIDATION_VALUE"].ToString()) + "</font> will be checked for Public domain email format.</div>";
                        break;

                    case "NAMEDUPECHECK":
                        sHTMLValue += "<div align = 'left'><font color = 'gray'>Contact Names </font> will be dupe checked.</div>";
                        break;

                    case "JOBTITLESPELLCHECK":
                        sHTMLValue += "<div align = 'left'><font color = 'gray'>Job Title </font> will be spell checked.</div>";
                        break;
                                        
                }

                sHTMLValue += "<div align = 'right'><font color = 'Orange'>" + dtValidation.Rows[i]["OPERATION_TYPE"] + "</font></div>";
                
                //sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Navy'>WR: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[i]["WR_Agent_Name"].ToString()) + " - " + dtMasterContacts.Rows[i]["WR_UPDATED_DATE"] + "</font></div>";

                    
                //if (dtMasterContacts.Rows[i]["Review_Tag"].ToString().Length > 0)
                  //  sHTMLValue += "<div align = 'Left'><font size='7' face ='Microsoft Sans Serif' color = 'Crimson'>Review: " + GM.ProperCase_ProjectSpecific(dtMasterContacts.Rows[i]["Review_Tag"].ToString()) + "</font></div>";

                sHTMLValue += "</div>";

                GridRow gridRow = new GridRow();
                GridCell gridCellHTMLContact = new GridCell();                
                
                
                //if (GV.lstWR_DeleteStatus.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase) || GV.lstTR_DeleteStatus.Contains(dtMasterContacts.Rows[i]["WR_CONTACT_STATUS"].ToString(), StringComparer.OrdinalIgnoreCase))
                //{
                //    Background b = new Background(Color.Gainsboro);
                //    gridRow.CellStyles.Default.Background = b;
                //    gridRow.CellStyles.Default.TextColor = Color.Gray;
                //}
                                
                gridCellHTMLContact.Value = sHTMLValue;
                gridRow.Cells.Add(gridCellHTMLContact);
                gridRow.Expanded = true;
                this.sdgvValidation.PrimaryGrid.Rows.Add(gridRow);
                
            }

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            frmList_With_Header obj = new frmList_With_Header();
            obj.dtItems = dtFieldMaster;
            obj.sHeaderColumn = "TABLE_NAME";
            obj.sValueColumn = "FIELD_NAME_TABLE";
            obj.ShowDialog();
            if(obj.DialogResult == System.Windows.Forms.DialogResult.OK)
                txtWhenField.Text = obj.sReturnTable + "." + obj.sReturnValue;
        }

        private void txtWhenCondition_ButtonCustomClick(object sender, EventArgs e)
        {

        }

        private void sdgvValidation_RowActivated(object sender, GridRowActivatedEventArgs e)
        {
            List<string> lstEqual = new List<string>();
            lstEqual.Add("MANDATORYFIELDS");
            lstEqual.Add("JOBTITLESPELLCHECK");
            lstEqual.Add("EMAILCOMPANYCHECK");
            lstEqual.Add("EMAILDUPECHECK");
            lstEqual.Add("EMAILPUBLICDOMAINCHECK");
            lstEqual.Add("NAMEDUPECHECK");
            txtWhenField.Text = dtValidation.Rows[e.NewActiveRow.RowIndex]["VALIDATION_FOR"].ToString();
            
            if (lstEqual.Contains(dtValidation.Rows[e.NewActiveRow.RowIndex]["VALIDATION_TYPE"].ToString(), StringComparer.OrdinalIgnoreCase))
                txtWhenCondition.Text = "Equals";
            else
                txtWhenCondition.Text = dtValidation.Rows[e.NewActiveRow.RowIndex]["VALIDATION_TYPE"].ToString();

            tagListControl1.Tags.Clear();
            tagListControl1.Tags = dtValidation.Rows[e.NewActiveRow.RowIndex]["CONDITION"].ToString().Split('|').ToList();

        }
    }
}
