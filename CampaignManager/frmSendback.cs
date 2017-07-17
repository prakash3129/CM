using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Schedule;
using DevComponents.Schedule.Model;

namespace GCC
{
    public partial class frmSendback : Office2007Form
    {
        public frmSendback()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
        }

        private bool _IsLastMonth = false;
        public bool IsLastMonth 
        {
            get { return _IsLastMonth; }
            set { _IsLastMonth = value; }
        }


        DataTable dtSentBack;
        private void frmSendback_Load(object sender, EventArgs e)
        {
            try
            {

                string sDateCondition = string.Empty;

                if (IsLastMonth)
                {
                    if(GM.GetDateTime().Month == 1)
                        sDateCondition = " MONTH(cm." + GV.sAccessTo + "_UPDATED_DATE) = 12 AND YEAR(cm." + GV.sAccessTo + "_UPDATED_DATE) = YEAR(GETDATE())-1 ";
                    else
                        sDateCondition = " MONTH(cm." + GV.sAccessTo + "_UPDATED_DATE) = MONTH(getdate())-1 AND YEAR(cm." + GV.sAccessTo + "_UPDATED_DATE) = YEAR(getdate()) ";
                }
                else                
                    sDateCondition = " MONTH(cm." + GV.sAccessTo + "_UPDATED_DATE) = MONTH(getdate()) AND YEAR(cm." + GV.sAccessTo + "_UPDATED_DATE) = YEAR(getdate()) ";                

                string sQuery = @"SELECT * FROM ";


                sQuery += "(SELECT CAST(cm." + GV.sAccessTo + "_UPDATED_DATE as date) as Date,cm." + GV.sAccessTo + "_AGENT_NAME Agent ,COUNT(*) COUNT,'Processed' Status FROM " + GV.sContactTable + " cm LEFT JOIN " + GV.sProjectID + "_QC QC ON cm.Contact_ID_P = QC.RecordID AND QC.TABLENAME='CONTACT' AND QC.ResearchType ='" + GV.sAccessTo + "' WHERE  ";
                sQuery += sDateCondition + " AND cm." + GV.sAccessTo + "_UNCERTAIN_STATUS = 0 AND cm." + GV.sAccessTo + "_CONTACT_STATUS IN (" + (GV.sAccessTo == "TR" ? GV.sTRContactstatusTobeValidated : GV.sWRContactstatusTobeValidated) + ") ";
                sQuery += "AND QC.QC_STATUS IS NULL ";
                sQuery += "GROUP BY CAST(cm." + GV.sAccessTo + "_UPDATED_DATE as date) ,cm." + GV.sAccessTo + "_AGENT_NAME UNION ALL ";

                sQuery += "SELECT CAST(cm." + GV.sAccessTo + "_UPDATED_DATE as date) as Date,cm." + GV.sAccessTo + "_AGENT_NAME Agent,COUNT(*) COUNT,'ReProcessed' Status FROM " + GV.sContactTable + " cm  INNER JOIN " + GV.sProjectID + "_QC QC ON cm.Contact_ID_P = QC.RecordID AND QC.TABLENAME='CONTACT' AND QC.ResearchType ='" + GV.sAccessTo + "' WHERE ";
                sQuery += sDateCondition + " AND cm." + GV.sAccessTo + "_UNCERTAIN_STATUS = 0 AND cm." + GV.sAccessTo + "_CONTACT_STATUS IN (" + (GV.sAccessTo == "TR" ? GV.sTRContactstatusTobeValidated : GV.sWRContactstatusTobeValidated) + ") ";
                sQuery += "AND QC.QC_STATUS = 'Reprocessed' ";
                //sQuery += "AND LENGTH(IFNULL(cm." + GV.sAccessTo + "_QC_STATUS,'')) = 0 AND QC_UpdatedDate IS NOT NULL ";
                sQuery += "GROUP BY CAST(cm." + GV.sAccessTo + "_UPDATED_DATE as date) ,cm." + GV.sAccessTo + "_AGENT_NAME UNION ALL ";

                sQuery += "SELECT CAST(cm." + GV.sAccessTo + "_UPDATED_DATE as date) as Date,cm." + GV.sAccessTo + "_AGENT_NAME Agent,COUNT(*) COUNT,'SendBack' Status FROM " + GV.sContactTable + " cm INNER JOIN " + GV.sProjectID + "_QC QC ON cm.Contact_ID_P = QC.RecordID AND QC.TABLENAME='CONTACT' AND QC.ResearchType ='" + GV.sAccessTo + "' WHERE ";
                sQuery += sDateCondition;
                sQuery += "AND cm." + GV.sAccessTo + "_UNCERTAIN_STATUS = 0 AND QC.QC_STATUS ='SendBack' ";
                sQuery += "GROUP BY CAST(cm." + GV.sAccessTo + "_UPDATED_DATE as date) ,cm." + GV.sAccessTo + "_AGENT_NAME UNION ALL ";

                sQuery += "SELECT CAST(cm." + GV.sAccessTo + "_UPDATED_DATE as date) as Date,cm." + GV.sAccessTo + "_AGENT_NAME Agent,COUNT(*) COUNT,'OK' Status FROM " + GV.sContactTable + " cm INNER JOIN " + GV.sProjectID + "_QC QC ON cm.Contact_ID_P = QC.RecordID AND QC.TABLENAME='CONTACT' AND QC.ResearchType ='" + GV.sAccessTo + "' WHERE ";
                sQuery += sDateCondition + " AND cm." + GV.sAccessTo + "_UNCERTAIN_STATUS = 0 AND cm." + GV.sAccessTo + "_CONTACT_STATUS IN (" + (GV.sAccessTo == "TR" ? GV.sTRContactstatusTobeValidated : GV.sWRContactstatusTobeValidated) + ") ";
                sQuery += "AND QC.QC_STATUS ='OK' ";

                sQuery += "GROUP BY CAST(cm." + GV.sAccessTo + "_UPDATED_DATE as date) ,cm." + GV.sAccessTo + "_AGENT_NAME) AS T ";
                sQuery += "ORDER BY T.date ,T.Status,T.Agent;";

                //if (IsLastMonth)
                //{
                    
                //}
                //else
                //{
                //    sQuery += "(SELECT CONVERT(cm." + GV.sAccessTo + "_UPDATED_DATE,date) as Date,cm." + GV.sAccessTo + "_AGENT_NAME Agent ,COUNT(*) COUNT,'Processed' Status FROM " + GV.sContactTable + " cm LEFT JOIN "+GV.sProjectID+"_QC QC ON cm.Contact_ID_P = QC.RecordID AND QC.TABLENAME='CONTACT' AND QC.ResearchType ='"+GV.sAccessTo+"' WHERE  ";
                //    sQuery += "MONTH(cm." + GV.sAccessTo + "_UPDATED_DATE) = MONTH(CURDATE()) AND YEAR(cm." + GV.sAccessTo + "_UPDATED_DATE) = YEAR(CURDATE()) AND cm." + GV.sAccessTo + "_CONTACT_STATUS IN ("+(GV.sAccessTo == "TR" ? GV.sTRContactstatusTobeValidated : GV.sWRContactstatusTobeValidated)+") ";
                //    sQuery += "AND QC.QC_STATUS IS NULL ";
                //    sQuery += "GROUP BY CONVERT(cm." + GV.sAccessTo + "_UPDATED_DATE,date) ,cm." + GV.sAccessTo + "_AGENT_NAME UNION ALL ";

                //    sQuery += "SELECT CONVERT(cm." + GV.sAccessTo + "_UPDATED_DATE,date) as Date,cm." + GV.sAccessTo + "_AGENT_NAME Agent,COUNT(*) COUNT,'ReProcessed' Status FROM " + GV.sContactTable + " cm  INNER JOIN " + GV.sProjectID + "_QC QC ON cm.Contact_ID_P = QC.RecordID AND QC.TABLENAME='CONTACT' AND QC.ResearchType ='" + GV.sAccessTo + "' WHERE ";                     
                //    sQuery += "MONTH(cm." + GV.sAccessTo + "_UPDATED_DATE) = MONTH(CURDATE()) AND YEAR(cm." + GV.sAccessTo + "_UPDATED_DATE) = YEAR(CURDATE()) AND cm." + GV.sAccessTo + "_CONTACT_STATUS IN (" + (GV.sAccessTo == "TR" ? GV.sTRContactstatusTobeValidated : GV.sWRContactstatusTobeValidated) + ") ";
                //    sQuery += "AND QC.QC_STATUS = 'Reprocessed' ";
                //    //sQuery += "AND LENGTH(IFNULL(cm." + GV.sAccessTo + "_QC_STATUS,'')) = 0 AND QC_UpdatedDate IS NOT NULL ";
                //    sQuery += "GROUP BY CONVERT(cm." + GV.sAccessTo + "_UPDATED_DATE,date) ,cm." + GV.sAccessTo + "_AGENT_NAME UNION ALL ";

                //    sQuery += "SELECT CONVERT(cm." + GV.sAccessTo + "_UPDATED_DATE,date) as Date,cm." + GV.sAccessTo + "_AGENT_NAME Agent,COUNT(*) COUNT,'SendBack' Status FROM " + GV.sContactTable + " cm INNER JOIN " + GV.sProjectID + "_QC QC ON cm.Contact_ID_P = QC.RecordID AND QC.TABLENAME='CONTACT' AND QC.ResearchType ='" + GV.sAccessTo + "' WHERE ";
                //    sQuery += "MONTH(cm." + GV.sAccessTo + "_UPDATED_DATE) = MONTH(CURDATE()) AND YEAR(cm." + GV.sAccessTo + "_UPDATED_DATE) = YEAR(CURDATE()) ";
                //    sQuery += "AND QC.QC_STATUS ='SendBack' ";
                //    sQuery += "GROUP BY CONVERT(cm." + GV.sAccessTo + "_UPDATED_DATE,date) ,cm." + GV.sAccessTo + "_AGENT_NAME UNION ALL ";

                //    sQuery += "SELECT CONVERT(cm." + GV.sAccessTo + "_UPDATED_DATE,date) as Date,cm." + GV.sAccessTo + "_AGENT_NAME Agent,COUNT(*) COUNT,'OK' Status FROM " + GV.sContactTable + " cm INNER JOIN " + GV.sProjectID + "_QC QC ON cm.Contact_ID_P = QC.RecordID AND QC.TABLENAME='CONTACT' AND QC.ResearchType ='" + GV.sAccessTo + "' WHERE ";
                //    sQuery += "MONTH(cm." + GV.sAccessTo + "_UPDATED_DATE) = MONTH(CURDATE()) AND YEAR(cm." + GV.sAccessTo + "_UPDATED_DATE) = YEAR(CURDATE()) AND cm." + GV.sAccessTo + "_CONTACT_STATUS IN (" + (GV.sAccessTo == "TR" ? GV.sTRContactstatusTobeValidated : GV.sWRContactstatusTobeValidated) + ") ";
                //    sQuery += "AND QC.QC_STATUS ='OK' ";
                    
                //    sQuery += "GROUP BY CONVERT(cm." + GV.sAccessTo + "_UPDATED_DATE,date) ,cm." + GV.sAccessTo + "_AGENT_NAME) AS T ";
                //    sQuery += "ORDER BY T.date ,T.Status,T.Agent;";
                //}

                if (sQuery.Length > 0)
                {
                    dtSentBack = GV.MSSQL1.BAL_ExecuteQuery(sQuery);
                    CalViewSendBack.IsTimeRulerVisible = false;                    
                    backgroundLoader.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        void Load_Calnder_Data()
        {
            try
            {
                if (dtSentBack != null && dtSentBack.Rows.Count > 0)
                {
                    CalViewSendBack.CalendarModel.Appointments.Clear();                    
                    CalViewSendBack.SelectedView = eCalendarView.Month;

                    if (IsLastMonth)
                    {
                        DateTime dCalcDate = GM.GetDateTime().AddMonths(-1);
                        CalViewSendBack.MonthViewStartDate = new DateTime(dCalcDate.Year, dCalcDate.Month, 1);
                        CalViewSendBack.MonthViewEndDate = new DateTime(dCalcDate.Year, dCalcDate.Month, DateTime.DaysInMonth(dCalcDate.Year, dCalcDate.Month));
                    }
                    
                    List<string> lstQuery = new List<string>();
                    if (checkBoxProcessed.Checked)
                        lstQuery.Add("Processed");
                    if (checkBoxReProcessed.Checked)
                        lstQuery.Add("ReProcessed");
                    if (checkBoxSendBack.Checked)
                        lstQuery.Add("SendBack");
                    if (checkBoxOK.Checked)
                        lstQuery.Add("OK");

                    string sQuery = GM.ListToQueryString(lstQuery, "String");
                    if (sQuery.Length > 0)
                    {
                        DataRow[] drrCalItems = dtSentBack.Select("Status IN (" + sQuery + ")", "date,Status,Agent");
                        if (drrCalItems.Length > 0)
                        {
                            foreach (DataRow drSendBack in drrCalItems)
                            {
                                Appointment app = new Appointment();
                                app.Locked = true;
                                app.Subject = GM.ProperCase_ProjectSpecific(drSendBack["Agent"].ToString()) + "        <b>" + drSendBack["Count"].ToString() + "</b>";
                                app.Subject = GM.ProperCase_ProjectSpecific(drSendBack["Agent"].ToString()) + "        <b>" + drSendBack["Count"].ToString() + "</b>";
                                app.Description = GM.ProperCase_ProjectSpecific(drSendBack["Agent"].ToString()) + "        <b>" + drSendBack["Count"].ToString() + "</b>";
                                app.Tooltip = GM.ProperCase_ProjectSpecific(drSendBack["Agent"].ToString()) + "        <b>" + drSendBack["Count"].ToString() + "</b>";

                                switch (drSendBack["Status"].ToString())
                                {
                                    case "Processed":
                                        app.CategoryColor = Appointment.CategoryBlue;
                                        break;
                                    case "ReProcessed":
                                        app.CategoryColor = Appointment.CategoryYellow;
                                        break;
                                    case "SendBack":
                                        app.CategoryColor = Appointment.CategoryRed;
                                        break;
                                    case "OK":
                                        app.CategoryColor = Appointment.CategoryGreen;
                                        break;
                                    default:
                                        app.CategoryColor = Appointment.CategoryPurple;
                                        break;
                                }

                                app.StartTime = Convert.ToDateTime(drSendBack["Date"]);
                                app.EndTime = Convert.ToDateTime(drSendBack["Date"]).AddDays(1);
                                //btnDayViewBack.Visible = false;
                                CalViewSendBack.CalendarModel.Appointments.Add(app);                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }

        private void CalViewSendBack_ItemDoubleClick(object sender, MouseEventArgs e)
        {
            //DayView dv = sender as DayView;
            //CalViewSendBack.LabelTimeSlots = false;
            CalViewSendBack.FixedAllDayPanelHeight = CalViewSendBack.Height - 35;
            CalViewSendBack.SelectedView = eCalendarView.Day;
            //ToastNotification.Show(this, "Click");


            //DateTime startDate = CalViewSendBack.DateSelectionStart.GetValueOrDefault();
            //DateTime endDate = CalViewSendBack.DateSelectionEnd.GetValueOrDefault();

        }

        private void btnDayViewBack_Click(object sender, EventArgs e)
        {
            CalViewSendBack.SelectedView = eCalendarView.Month;
        }

        private void checkBoxProcessed_Click(object sender, EventArgs e)
        {
            backgroundLoader.RunWorkerAsync();
        }

        private void backgroundLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            ProgressLoading.Start();
            Load_Calnder_Data();
        }

        private void backgroundLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressLoading.Stop();
        }

        private void CalViewSendBack_SelectedViewChanged(object sender, SelectedViewEventArgs e)
        {
            if (e.NewValue == eCalendarView.Day)
                btnDayViewBack.Visible = true;
            else
                btnDayViewBack.Visible = false;
            barSendbackProperties.Refresh();
        }
    }
}
