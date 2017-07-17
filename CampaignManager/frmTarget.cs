using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevExpress.XtraCharts;

namespace GCC
{
    public partial class frmTarget : DevComponents.DotNetBar.Office2007Form
    {
        public frmTarget()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); //Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
            MessageBoxEx.EnableGlass = false;
        }

        //BAL.BAL_Global objBAL_Global = new BAL.BAL_Global();
        DataTable dtDaily_Agent_Perfoemance;
        DataTable dtAgentSummary;
        private void frmTarget_Load(object sender, EventArgs e)
        {
            Load_Chart();
            Load_Agent_Data();
        }

        void Load_Agent_Data()
        {
            try
            {
                lblPanelHeader.Text = "Overall Statistics <br/>(" + GM.GetDateTime().ToString("MMMM") + ")";
                dtAgentSummary = GV.MSSQL1.BAL_ExecuteQuery(GetQuery.AgentSummary(GV.sEmployeeName));
                dtDaily_Agent_Perfoemance = GV.MSSQL1.BAL_FetchTable("RM..DAILY_AGENT_PERFORMANCE_V1", "DASHBOARD_ID = " + GV.sDashBoardID + " AND FLAG='"+GV.sAccessTo+"' AND AGENTNAME = '" + GV.sEmployeeName + "' AND DATECALLED = '" + GM.GetDateTime().ToString("yyyyMMdd") + "'");
                if (dtAgentSummary.Rows.Count > 0)
                {
                    lblMonthlyPoints.Text = "<b>" + dtAgentSummary.Rows[0]["MTDPOINTS"].ToString() + "</b> point(s) accumulated this month.";
                    //lblYesterDaysPoint.Text = dtAgentSummary.Rows[0]["YESTERDAYPOINTS"].ToString();
                    lblTargetAchived.Text = "Goal accomplished for <b>" + dtAgentSummary.Rows[0]["TA_CNT"] + "</b> day(s) this month.";
                    lblTargetMissed.Text = "Goal not achieved for <b>" + dtAgentSummary.Rows[0]["TM_CNT"] + "</b> day(s) this month.";
                }
                else
                {
                    lblMonthlyPoints.Text = "<b>0.00</b> point(s) accumulated this month.";
                    lblTargetAchived.Text = "Goal accomplished for <b>0</b> day(s) this month.";
                    lblTargetMissed.Text = "Goal not achieved for <b>0</b> day(s) this month.";
                }
            
                if (dtDaily_Agent_Perfoemance.Rows.Count > 0 && dtDaily_Agent_Perfoemance.Rows[0]["SELF_TARGET"].ToString().Length > 0)
                {
                    txtTarget.Value = Convert.ToInt32(dtDaily_Agent_Perfoemance.Rows[0]["SELF_TARGET"].ToString());
                    txtTarget.Enabled = false;
                    btnTarget.Visible = false;
                }
                else//By default empty.
                    txtTarget.Text = string.Empty;

                if (GV.sUserType != "Agent")
                    btnTarget.Enabled = false;

                pictureDP.Image = GV.imgEmployeeImage;
                lblEmpName.Text = ProperCaseHelper.NameANDJobTitleCasing(GV.sEmployeeActualName.ToLower(), "Name");
                lblUpload.Visible = false;
                lblUpload.Parent = pictureDP;
                lblUpload.Dock = DockStyle.Bottom;
                
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void Load_Chart()
        {
            try
            {
                DataTable dtChartData = GV.MSSQL1.BAL_ExecuteQuery("SELECT TOP 15 DATECALLED,DATENAME(dw,DATECALLED)AS [Day] ,ISNULL(NO_OF_CONTACTS_VALIDATED,0)NO_OF_CONTACTS_VALIDATED,ISNULL(SELF_TARGET,0)SELF_TARGET,ISNULL(AVERAGE,0) AS Team FROM RM..DAILY_AGENT_PERFORMANCE_V1 WHERE DASHBOARD_ID=" + GV.sDashBoardID + " AND FLAG='"+GV.sAccessTo+"' AND AGENTNAME='" + GV.sEmployeeName + "' ORDER BY DATECALLED DESC");
                foreach (DataRow dr in dtChartData.Rows)
                {
                    SeriesPoint x1;
                    SeriesPoint x2;
                    SeriesPoint x3;
                    string sXVal = ((DateTime)dr["DATECALLED"]).ToString("dd-MMM") + " / " + dr["Day"].ToString();
                    x1 = new SeriesPoint(sXVal, Convert.ToInt32(dr["SELF_TARGET"]));
                    x2 = new SeriesPoint(sXVal, Convert.ToInt32(dr["Team"]));
                    x3 = new SeriesPoint(sXVal, Convert.ToInt32(dr["NO_OF_CONTACTS_VALIDATED"]));
                    chartBarTarget.Series[0].Points.Add(x1);
                    chartBarTarget.Series[1].Points.Add(x2);
                    chartBarTarget.Series[2].Points.Add(x3);

                    chartBarTarget.Series[0].Points.Add(x1);
                    chartBarTarget.Series[1].Points.Add(x2);
                    chartBarTarget.Series[2].Points.Add(x3);

                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTarget.Text.Length > 0 && txtTarget.Text != "0")
                {
                    if (dtDaily_Agent_Perfoemance.Rows.Count > 0)
                    {
                        dtDaily_Agent_Perfoemance.Rows[0]["SELF_TARGET"] = txtTarget.Value.ToString();
                        GV.MSSQL_RM.BAL_SaveToTable(dtDaily_Agent_Perfoemance.GetChanges(DataRowState.Modified), "RM..DAILY_AGENT_PERFORMANCE_V1", "Update", true);
                    }
                    else
                    {
                        DataRow drNewAgentWisePerformance = dtDaily_Agent_Perfoemance.NewRow();
                        drNewAgentWisePerformance["DASHBOARD_ID"] = GV.sDashBoardID;
                        drNewAgentWisePerformance["DATECALLED"] = GM.GetDateTime();
                        drNewAgentWisePerformance["FLAG"] = GV.sAccessTo;
                        drNewAgentWisePerformance["AGENTNAME"] = GV.sEmployeeName;
                        drNewAgentWisePerformance["SELF_TARGET"] = txtTarget.Value.ToString();
                        dtDaily_Agent_Perfoemance.Rows.Add(drNewAgentWisePerformance);
                        GV.MSSQL_RM.BAL_SaveToTable(dtDaily_Agent_Perfoemance.GetChanges(DataRowState.Added), "RM..DAILY_AGENT_PERFORMANCE_V1", "New", true);
                    }
                    ToastNotification.Show(this, "Target Updated Sucessfully", eToastPosition.TopRight);
                    txtTarget.Enabled = false;
                    btnTarget.Visible = false;
                    ((frmMain)this.MdiParent).OpenCompanyList();
                    this.Close();
                }
                else
                    ToastNotification.Show(this, "Target not set", eToastPosition.TopRight);
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void pictureDP_MouseEnter(object sender, EventArgs e)
        {
            lblUpload.Visible = true;
        }

        private void pictureDP_MouseLeave(object sender, EventArgs e)
        {
            lblUpload.Visible = false;
        }

        private void pictureDP_MouseDown(object sender, MouseEventArgs e)
        {
            frmUploadPicture objfrmUploadPicture = new frmUploadPicture();
            objfrmUploadPicture.ShowDialog();
        }
    }
}
