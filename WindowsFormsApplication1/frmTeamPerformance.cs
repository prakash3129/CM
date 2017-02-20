using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;

namespace GCC
{
    public partial class frmTeamPerformance : Office2007Form
    {
        public frmTeamPerformance()
        {
            InitializeComponent();
            this.AutoScroll = true;
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); //Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
            MessageBoxEx.EnableGlass = false;
            
        }
        //BAL_Global objBAL_Global = new BAL_Global();
        DataTable dtImages = new DataTable();
        DataTable dtProjectPerformence;
        DataTable dtOverallPerformence;
        DataTable dtAgentSummary;
        DataTable dtDaily_Agent_Perfoemance;
        private void TeamPerformance_Load(object sender, EventArgs e)
        {
            try
            {
                lblUpload.Parent = pictureDP;
                lblUpload.Dock = DockStyle.Bottom;
                pictureDP.Image = GV.imgEmployeeImage;
                lblUpload.Visible = false;
                //lblUpload.Width = pictureDP.Width;
                Load_Agent_Data();

                lblAgentName.Text = ProperCaseHelper.NameANDJobTitleCasing(GV.sEmployeeActualName, "Name");
                lblCurrentProject.Text = "Current Project: <b>" + GV.sProjectName.Replace("_", " ") + "</b>";
                lblEmpID.Text = "Employee ID: <b>" + GV.sEmployeeNo + "</b>";
                if (GV.sAccessTo == "TR")
                    lblResearchType.Text = "Research Type: <b>Voice</b>";
                else
                    lblResearchType.Text = "Research Type: <b>Web</b>";
                lblUserType.Text = "User Type: <b>" + GV.sUserType + "</b>";


                //if (GlobalVariables.sDashBoardID.Length > 0)
                {
                    //DataTable dtAgentWisePerformence = GV.MSSQL.BAL_ExecuteQuery("SELECT U.Fullname,SELF_TARGET,NO_OF_CONTACTS_VALIDATED,U.DateOfJoin, LastSeen FROM DAILY_AGENT_PERFORMANCE D INNER JOIN Timesheet.dbo.USERS U ON D.AGENTNAME = U.UserName WHERE D.DASHBOARD_ID=" + GlobalVariables.sDashBoardID + " AND U.Active = 'Y' AND DATECALLED='"+GlobalMethods.GetDateTime().ToString("yyyyMMdd")+"'");
                    //DataTable dtAgentWisePerformence = GV.MSSQL.BAL_ExecuteQuery("SELECT U.Fullname,SELF_TARGET,NO_OF_CONTACTS_VALIDATED,CASE WHEN DATEDIFF(YEAR, DateOfJoin,GETDATE())>1 THEN CONVERT(VARCHAR(100), DATEDIFF(YEAR, DateOfJoin,GETDATE())) +' Years' WHEN DATEDIFF(YEAR, DateOfJoin,GETDATE())=1 THEN CONVERT(VARCHAR(100), DATEDIFF(YEAR, DateOfJoin,GETDATE())) +' Year' WHEN (DATEDIFF(YEAR, DateOfJoin,GETDATE())<1 AND DATEDIFF(MONTH, DateOfJoin,GETDATE())>1)  THEN CONVERT(VARCHAR(100), DATEDIFF(MONTH, DateOfJoin,GETDATE())) +' Months' WHEN (DATEDIFF(YEAR, DateOfJoin,GETDATE())<1 AND DATEDIFF(MONTH, DateOfJoin,GETDATE())=1)  THEN CONVERT(VARCHAR(100), DATEDIFF(MONTH, DateOfJoin,GETDATE())) +' Month' WHEN (DATEDIFF(YEAR, DateOfJoin,GETDATE())<1 AND DATEDIFF(MONTH, DateOfJoin,GETDATE())<1)  THEN CONVERT(VARCHAR(100), DATEDIFF(DAY, DateOfJoin,GETDATE())) +' Days' END Tenure, LastSeen FROM DAILY_AGENT_PERFORMANCE D INNER JOIN Timesheet.dbo.USERS U ON D.AGENTNAME = U.UserName WHERE D.DASHBOARD_ID=174 AND U.Active = 'Y' AND DATECALLED='" + GlobalMethods.GetDateTime().ToString("yyyyMMdd") + "'");
                    //byte[] image = ReadImageFile(@"C:\Users\thangaprakashm\Desktop\Road\3759007_orig.jpg");
                    // Image img = Image.FromFile(@"C:\Users\thangaprakashm\Desktop\Road\3759007_orig.jpg");
                    dtImages = GV.MSSQL.BAL_ExecuteQuery("SELECT A.* FROM MVC..EmployeeImage A INNER JOIN Timesheet..Users B ON A.EmployeeID = B.EmployeeNo WHERE B.Active = 'Y'");
                    Load_ProjectPerformance();
                    Load_OverAllPerformance();
                    GridPanel panel1 = sdgvProjectPerformance.PrimaryGrid;
                    GridPanel panel2 = sdgvOverallPerformance.PrimaryGrid;

                    GridColumn column1 = panel1.Columns["Photo"];
                    GridColumn column2 = panel2.Columns["Photo"];

                    GridImageEditControl gc1 = column1.EditControl as GridImageEditControl;
                    GridImageEditControl gc2 = column1.RenderControl as GridImageEditControl;
                    gc1.ImageSizeMode = ImageSizeMode.Zoom;
                    gc2.ImageSizeMode = ImageSizeMode.Zoom;


                    GridImageEditControl gc11 = column2.EditControl as GridImageEditControl;
                    GridImageEditControl gc21 = column2.RenderControl as GridImageEditControl;
                    gc11.ImageSizeMode = ImageSizeMode.Zoom;
                    gc21.ImageSizeMode = ImageSizeMode.Zoom;

                    //superGridControl1.PrimaryGrid.Columns[1].CellStyles.Default.Font = new Font("Microsoft Sans Serif", 18, FontStyle.Bold);


                    splitContainerGrids.SplitterDistance = (splitContainerGrids.Size.Width / 2);
                    sdgvProjectPerformance.PrimaryGrid.Columns[0].FillWeight = 30;
                    sdgvProjectPerformance.PrimaryGrid.DefaultRowHeight = 140;

                    sdgvOverallPerformance.PrimaryGrid.Columns[0].FillWeight = 30;
                    sdgvOverallPerformance.PrimaryGrid.DefaultRowHeight = 140;

                    sdgvProjectPerformance.PrimaryGrid.Columns[1].Width = splitContainerGrids.Panel1.Width;// -135;

                    sdgvOverallPerformance.PrimaryGrid.Columns[1].Width = splitContainerGrids.Panel2.Width;// -135;


                    
                }

                //if ((Screen.PrimaryScreen.WorkingArea.Width - (sdgvProjectPerformance.PrimaryGrid.Columns[1].Width + sdgvOverallPerformance.PrimaryGrid.Columns[1].Width) - 100) > 600)
                //    panelInformation.Width = 600;
                //else if ((Screen.PrimaryScreen.WorkingArea.Width - (sdgvProjectPerformance.PrimaryGrid.Columns[1].Width + sdgvOverallPerformance.PrimaryGrid.Columns[1].Width) - 100) < 470)
                //{
                //    panelInformation.Width = Screen.PrimaryScreen.WorkingArea.Width - (sdgvProjectPerformance.PrimaryGrid.Columns[1].Width + sdgvOverallPerformance.PrimaryGrid.Columns[1].Width) - 100;
                //    panelInformation.Width = 490;
                    
                //}
                //else
                //    panelInformation.Width = Screen.PrimaryScreen.WorkingArea.Width - (sdgvProjectPerformance.PrimaryGrid.Columns[1].Width + sdgvOverallPerformance.PrimaryGrid.Columns[1].Width) - 100;

                //panelInformation.Height = this.Height- 80;
                ////panelInformation.Height = sdgvOverallPerformance.Height + 80;
                
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void Load_Agent_Data()
        {
            try
            {
                dtAgentSummary = GV.MSSQL.BAL_ExecuteQuery(GetQuery.AgentSummary(GV.sEmployeeName));
                dtDaily_Agent_Perfoemance = GV.MSSQL.BAL_FetchTable("DAILY_AGENT_PERFORMANCE_V1", "DASHBOARD_ID = " + GV.sDashBoardID + " AND FLAG='"+GV.sAccessTo+"' AND AGENTNAME = '" + GV.sEmployeeName + "' AND DATECALLED = '" + GM.GetDateTime().ToString("yyyyMMdd") + "'");

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
                    lblTargetMissed.Text = "Goal not achieved for <b>0</b> day(s) this month";
                }
                
                //if (dtAgentSummary.Rows.Count > 0)
                //{
                //    lblMonthlyPoints.Text = "Monthly Points : <b>" + dtAgentSummary.Rows[0]["MTDPOINTS"].ToString() + "</b>";
                //    //lblYesterDaysPoint.Text = dtAgentSummary.Rows[0]["YESTERDAYPOINTS"].ToString();
                //    lblTargetAchived.Text = "Target Achieved : <b>" + dtAgentSummary.Rows[0]["AchievedContacts"].ToString() + "</b>";
                //    lblTargetMissed.Text = "Target Missed : <b>" + dtAgentSummary.Rows[0]["TM_CNT"].ToString() + "</b>";
                //}
                //else
                //{
                //    lblMonthlyPoints.Text = "Monthly Points : <b>0</b>";
                //    lblTargetAchived.Text = "Target Achieved : <b>0</b>";
                //    lblTargetMissed.Text = "Target Missed : <b>0</b>";
                //}


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
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void Load_ProjectPerformance()
        {
            try
            {
                string sProjectWiseSQL = @";WITH CTE AS ( SELECT  D3.EmployeeNo ,D3.Fullname AgentFullName ,NO_OF_CONTACTS_VALIDATED AchievedContacts ,LastSeen LastSeen ,POINTS ,
                                        SELF_TARGET ,Project_rank , CASE  WHEN MVC.DBO.FullMonthsSeparation(DATEADD(MONTH,1, DateOfJoin) ,DATEADD(DD,0,GETDATE()))=0 THEN 'Below 1 Month'
                                        WHEN MVC.DBO.FullMonthsSeparation(DATEADD(MONTH,1, DateOfJoin) ,DATEADD(DD,0,GETDATE()))>=6 THEN '6 Months & Above'
                                        WHEN MVC.DBO.FullMonthsSeparation(DATEADD(MONTH,1, DateOfJoin) ,DATEADD(DD,0,GETDATE()))=1 THEN '1 Month'
                                        WHEN MVC.DBO.FullMonthsSeparation(DATEADD(MONTH,1, DateOfJoin) ,DATEADD(DD,0,GETDATE()))=2 THEN '2 Months'
                                        WHEN MVC.DBO.FullMonthsSeparation(DATEADD(MONTH,1, DateOfJoin) ,DATEADD(DD,0,GETDATE()))=3 THEN '3 Months'
                                        WHEN MVC.DBO.FullMonthsSeparation(DATEADD(MONTH,1, DateOfJoin) ,DATEADD(DD,0,GETDATE())) IN (4,5) THEN '4 & 5 Months'
                                        END TENURE FROM DASHBOARD D1 INNER JOIN DAILY_AGENT_PERFORMANCE_V1 D2 ON D2.DASHBOARD_ID = D1.ID                                       
                                        AND D2.DASHBOARD_ID = "+GV.sDashBoardID+" AND D2.DATECALLED = CONVERT(DATE, GETDATE()) AND D2.FLAG = '"+GV.sAccessTo+"'INNER JOIN Timesheet..Users D3 ON D3.UserName = D2.AGENTNAME )";
                sProjectWiseSQL += @"SELECT EmployeeNo,AgentFullName,AchievedContacts,LastSeen,POINTS,SELF_TARGET,PROJECT_RANK OVERALLRANKING,TENURE,ROW_NUMBER() OVER (PARTITION BY TENURE ORDER BY POINTS DESC) TenurebasedRanking
                                        FROM CTE ORDER BY PROJECT_RANK";

                dtProjectPerformence = GV.MSSQL.BAL_ExecuteQuery(sProjectWiseSQL);
                ProjectPerformance_FillGrid();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        void ProjectPerformance_FillGrid()
        {
            if (dtProjectPerformence != null && dtProjectPerformence.Rows.Count > 0)
            {
                DataRow[] drrTenure = dtProjectPerformence.Select("EmployeeNo = '" + GV.sEmployeeNo + "'");
                string sTenure = string.Empty;
                string sHeaderRank = string.Empty;
                string sShowType = btnShowAllProject.Text;
                DataRow[] drrProject;

                if (drrTenure.Length > 0)
                    sTenure = drrTenure[0]["Tenure"].ToString();

                if (sShowType == "Show Peer")
                {
                    if (drrTenure.Length > 0)
                        sHeaderRank = drrTenure[0]["OVERALLRANKING"].ToString();
                    else
                        sHeaderRank = "-";
                    drrProject = dtProjectPerformence.Select("1=1", "OVERALLRANKING ASC");
                }
                else
                {
                    if (drrTenure.Length > 0)
                    {
                        sHeaderRank = drrTenure[0]["TenurebasedRanking"].ToString();
                        drrProject = dtProjectPerformence.Select("Tenure='" + sTenure + "'", "TenurebasedRanking ASC");
                    }
                    else
                    {
                        drrProject = dtProjectPerformence.Select("1=1", "OVERALLRANKING ASC");
                        sHeaderRank = "-";
                        ToastNotification.Show(this, "You are not found in any of the bucket.");
                        btnShowAllProject.Text = "Show All";
                    }
                }

                if (drrTenure.Length > 0)
                {
                    sTenure = drrTenure[0]["Tenure"].ToString();
                    sdgvProjectPerformance.PrimaryGrid.Header.Text = "<div align = 'Right'><font size = '13'><b>Project Performance</b></font><br/> <font size = '11'>You are <b>" + sHeaderRank + "</b> out of <b>" + drrProject.Length + "</b></font></div>";
                    //lblProjectPostiton.Text = "You are <b>" + GM.NumberSuffix(drrTenure[0]["Ranking"].ToString()) + "</b> out of <b>" + dtAgentWisePerformence.Rows.Count + "</b>";
                }
                else
                    sdgvProjectPerformance.PrimaryGrid.Header.Text = "<div align = 'Right'><font size = '13'><b>Project Performance</b></font></div>";

                lblTenure.Text = "Tenure: <b>" + sTenure.Replace("&", "&amp;") + "</b>";

                if (sdgvProjectPerformance.PrimaryGrid != null && sdgvProjectPerformance.PrimaryGrid.Rows != null)
                    sdgvProjectPerformance.PrimaryGrid.Rows.Clear();


                foreach (DataRow dr in dtProjectPerformence.Rows)
                {
                    Image img;
                    DataRow[] drrImage = dtImages.Select("EmployeeID = '" + dr["EmployeeNo"] + "'");
                    if (drrImage.Length > 0)
                        img = GM.GetImageFromByte(((byte[])drrImage[0]["EmployeeImage"]));
                    else
                        img = GCC.Properties.Resources.Misc_User_icon__1_;

                    GridCell gr0 = new GridCell(img);
                    ///gr0.InfoText = "asjdgajsdgas";
                    gr0.EditorParams = new object[] { ImageSizeMode.Zoom };

                    string sHTMLValue = string.Empty;

                    sHTMLValue += "<font size = '10' face = 'Arial' ><div align = 'Left'> <div  align = 'left'><font size = '15' face = 'Arial' color ='CornflowerBlue'><b>" + ProperCaseHelper.NameANDJobTitleCasing(dr["AgentFullName"].ToString().ToLower(), "Name") + "</b></font></div>";
                    sHTMLValue += "<div align = 'right'>Points : <font size = '12' face = 'Arial' color ='IndianRed'><b>" + dr["Points"] + "</b></font></div>";
                    sHTMLValue += "<font size = '9'><div align = 'left'>Tenure &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: " + dr["Tenure"].ToString().Replace("&", "&amp;") + "</div>";
                    sHTMLValue += "<div align = 'left'>Target &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: " + dr["SELF_TARGET"] + "</div>";
                    sHTMLValue += "<div align = 'left'>Contacts Achieved &nbsp;&nbsp;: " + dr["AchievedContacts"] + "</div>";

                    if (sShowType == "Show Peer")
                        sHTMLValue += "<div align = 'left'>Peer Rank &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: <font face = 'Arial' color = 'SeaGreen'><b>" + dr["OVERALLRANKING"] + "</b></font></div>";
                    else
                        sHTMLValue += "<div align = 'left'>Project Rank &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: <font face = 'Arial' color = 'SeaGreen'><b>" + dr["OVERALLRANKING"] + "</b></font></div>";


                    if (sShowType == "Show Peer")
                        sHTMLValue += "<div align = 'right'>Project Rank &nbsp;: <font face = 'Arial' color = 'SeaGreen'><b>" + dr["OVERALLRANKING"] + "</b></font></div>";
                    else
                        sHTMLValue += "<div align = 'right'>Peer Rank &nbsp;: <font face = 'Arial' color = 'SeaGreen'><b>" + dr["TenurebasedRanking"] + "</b></font></div>";

                    sHTMLValue += "<div align = 'right'>Last Seen &nbsp;: " + dr["Lastseen"] + "</div></font> </div></font>";


                    GridCell gr1 = new GridCell();
                    //gr1.EditorType = typeof(GridTextBoxXEditControl);
                    //gr1.RenderType = typeof(GridTextBoxXEditControl);
                    gr1.Value = sHTMLValue;

                    //GridCell gr1 = new GridCell(ProperCaseHelper.NameANDJobTitleCasing(dr[0].ToString().ToLower(), "Name"));

                    GridRow gr = new GridRow();
                    gr.Cells.Add(gr0);
                    gr.Cells.Add(gr1);
                    sdgvProjectPerformance.PrimaryGrid.Rows.Add(gr);
                }
            }
            else
                sdgvProjectPerformance.PrimaryGrid.Header.Text = "<div align = 'Right'><font size = '13'><b>Project Performance</b></font></div>";
        }

        void Load_OverAllPerformance()
        {
            try
            {
                string sStart = string.Empty;
                string sEndDate = string.Empty;
                if (GM.GetDateTime().Day < 25)
                {
                    if (GM.GetDateTime().Month == 1)
                        sStart = (GM.GetDateTime().Year - 1) + "-12-25";
                    else
                        sStart = GM.GetDateTime().Year + "-" + (GM.GetDateTime().Month - 1) + "-25";

                    sEndDate = GM.GetDateTime().Year + "-" + GM.GetDateTime().Month + "-24";
                }
                else
                {
                    sStart = GM.GetDateTime().Year + "-" + GM.GetDateTime().Month + "-25";

                    if (GM.GetDateTime().Month == 12)
                        sEndDate = (GM.GetDateTime().Year + 1) + "-01-24";
                    else
                        sEndDate = GM.GetDateTime().Year + "-" + (GM.GetDateTime().Month + 1) + "-24";
                }



                string sOverallSQL = "DECLARE @FromDate DATE = (Select convert(DATE,'" + sStart + "')) DECLARE @ToDate DATE = (Select convert(DATE,'" + sEndDate + "')) ";                           
                sOverallSQL += @" DECLARE @TOTAL_WORKING_DAYS INT =
                (SELECT
                   (DATEDIFF(dd, @FromDate, @ToDate) + 1)
                  -(DATEDIFF(wk, @FromDate, @ToDate) * 2)
                  -(CASE WHEN DATENAME(dw, @FromDate) = 'Sunday' THEN 1 ELSE 0 END)
                  -(CASE WHEN DATENAME(dw, @ToDate) = 'Saturday' THEN 1 ELSE 0 END)
                  -(SELECT COUNT(*)
                FROM Timesheet..PickLists
                WHERE Field = 'Holidays_Voice'
                AND Data BETWEEN CONVERT(VARCHAR(10), @FromDate, 111) AND CONVERT(VARCHAR(10), @ToDate, 111)
                ));WITH CTE AS (
                SELECT AGENTNAME,TARGET,NO_OF_CONTACTS_VALIDATED,POINTS POINTS_AS_PER_AVG,NO_OF_CONTACTS_VALIDATED - (ISNULL(EMAIL_REJECTION,0)+ISNULL(JOB_TITLE_REJECTION,0)+ISNULL(INCORRECT_DISPOSAL,0)+ISNULL(MATCHED_WITH_EXCLUSION,0)+ISNULL(MATCHED_WITH_PREVIOUS_SET,0)+ISNULL(OTHERS_REJECTION,0)) CONT,
                CAST ( (NO_OF_CONTACTS_VALIDATED - (ISNULL(EMAIL_REJECTION,0)+ISNULL(JOB_TITLE_REJECTION,0)+ISNULL(INCORRECT_DISPOSAL,0)+ISNULL(MATCHED_WITH_EXCLUSION,0)+ISNULL(MATCHED_WITH_PREVIOUS_SET,0)+ISNULL(OTHERS_REJECTION,0)))/NULLIF(ISNULL(TARGET,0),0) AS NUMERIC(12,2)) POINTS
                FROM MVC..DAILY_AGENT_PERFORMANCE_V1
                WHERE  DATECALLED BETWEEN @FromDate AND @ToDate
                AND AGENTNAME<>'TOTAL' AND FLAG='" + GV.sAccessTo+"'),CTEAGENTPOINTS AS (SELECT AGENTNAME, SUM(CONT) CONTACT_ACHIEVED,SUM(POINTS) POINTS FROM CTE GROUP BY AGENTNAME),";
                sOverallSQL += @"CTEAGENTSUM AS (SELECT D1.*,d2.Fullname,d2.EmployeeNo,CASE 
                WHEN MVC.DBO.FullMonthsSeparation(DATEADD(MONTH,1, DateOfJoin) ,DATEADD(DD,0,GETDATE()))=0 THEN 'Below 1 Month'
                WHEN MVC.DBO.FullMonthsSeparation(DATEADD(MONTH,1, DateOfJoin) ,DATEADD(DD,0,GETDATE()))>=6 THEN '>= 6 MONTHS'
                WHEN MVC.DBO.FullMonthsSeparation(DATEADD(MONTH,1, DateOfJoin) ,DATEADD(DD,0,GETDATE()))=1 THEN '1 Month'
                WHEN MVC.DBO.FullMonthsSeparation(DATEADD(MONTH,1, DateOfJoin) ,DATEADD(DD,0,GETDATE()))=2 THEN '2 Months'
                WHEN MVC.DBO.FullMonthsSeparation(DATEADD(MONTH,1, DateOfJoin) ,DATEADD(DD,0,GETDATE()))=3 THEN '3 Months'
                WHEN MVC.DBO.FullMonthsSeparation(DATEADD(MONTH,1, DateOfJoin) ,DATEADD(DD,0,GETDATE())) IN (4,5) THEN '4 & 5 Months'
                END TENURE FROM CTEAGENTPOINTS D1 INNER JOIN Timesheet..Users D2 ON d1.AGENTNAME=D2.UserName WHERE Active='Y'), CTEFINAL AS (
                SELECT fullname AgentFullName,contact_achieved AchievedContacts,points MTDPoints,EmployeeNo,C1.Tenure,ROW_NUMBER() OVER ( PARTITION BY C1.TENURE ORDER BY POINTS DESC) Ranking
                ,CASE WHEN POINTS >= [80% TARGET] AND POINTS < [90% TARGET] THEN '80% Target'
                WHEN POINTS >= [90% TARGET] AND POINTS < [100% TARGET] THEN '90% Target'
                WHEN POINTS >= [100% TARGET] AND POINTS < [110% TARGET] THEN '100% Target'
                WHEN POINTS >= [110% TARGET] AND POINTS < [120% TARGET] THEN '110% Target'
                WHEN POINTS >= [120% TARGET] AND POINTS < [130% TARGET] THEN '120% Target'   
                WHEN POINTS >= [130% TARGET] AND POINTS < [140% TARGET] THEN '130% Target'   
                WHEN POINTS >= [140% TARGET] AND POINTS < [150% TARGET] THEN '140% Target'
                WHEN POINTS >= [150% TARGET]  THEN '150% Target'     
                ELSE 'Not Qualified' END BUCKET ,
                ROW_NUMBER() OVER ( ORDER BY POINTS  DESC) [OVERALL RANKING] FROM CTEAGENTSUM C1
                LEFT OUTER JOIN dbo.UDF_GET_INCENTIVE_PLAN(@TOTAL_WORKING_DAYS) C2 ON C2.TENURE=C1.TENURE)
                SELECT C1.AgentFullName,C1.AchievedContacts,C1.MTDPoints,C1.EmployeeNo,case when C1.TENURE ='>= 6 MONTHS' then '6 Months & Above' else C1.TENURE END Tenure,C1.Ranking [TenurebasedRanking],C1.[Overall Ranking] [OVERALLRANKING],ISNULL(C2.AMOUNT,0) IncentiveAmount
                FROM CTEFINAL C1 LEFT OUTER JOIN GCC_INCENTIVE_PLAN C2 ON C1.BUCKET=C2.BUCKET AND C2.RESEARCH_TYPE='" + GV.sAccessTo+"' order by [OVERALL RANKING],[TenurebasedRanking]";

                dtOverallPerformence = GV.MSSQL.BAL_ExecuteQuery(sOverallSQL);
                OverAllPerformance_FillGrid();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void OverAllPerformance_FillGrid()
        {
            if (dtOverallPerformence != null && dtOverallPerformence.Rows.Count > 0)
            {
                DataRow[] drrTenure = dtOverallPerformence.Select("EmployeeNo = '" + GV.sEmployeeNo + "'");
                string sTenure = string.Empty;
                string sHeaderRank = string.Empty;
                string sShowType = btnShowAllMTD.Text;
                DataRow[] drrOverall;
                if (drrTenure.Length > 0)
                    sTenure = drrTenure[0]["Tenure"].ToString();

                if (sShowType == "Show Peer")
                {
                    if (drrTenure.Length > 0)                    
                        sHeaderRank = drrTenure[0]["OVERALLRANKING"].ToString();
                    else
                        sHeaderRank = "-";

                    drrOverall = dtOverallPerformence.Select("1=1", "OVERALLRANKING ASC");
                }
                else
                {
                    if (drrTenure.Length > 0)
                    {
                        sHeaderRank = drrTenure[0]["TenurebasedRanking"].ToString();
                        drrOverall = dtOverallPerformence.Select("Tenure='" + sTenure + "'", "TenurebasedRanking ASC");
                    }
                    else
                    {
                        drrOverall = dtOverallPerformence.Select("1=1", "OVERALLRANKING ASC");
                        sHeaderRank = "-";
                        ToastNotification.Show(this, "You are not found in any Peer.");
                        btnShowAllMTD.Text = "Show All";
                    }
                }

                if (drrTenure.Length > 0)   
                    sdgvOverallPerformance.PrimaryGrid.Header.Text = "<div align = 'Right'><font size = '13'><b>Month to Date Performance</b></font><br/> <font size = '11'>You are <b>" + sHeaderRank + "</b> out of <b>" + drrOverall.Length + "</b></font></div>";
                else
                    sdgvOverallPerformance.PrimaryGrid.Header.Text = "<div align = 'Right'><font size = '13'><b>Month to Date Performance</b></font></div>";

                lblTenure.Text = "Tenure: <b>" + sTenure.Replace("&", "&amp;") + "</b>";

                if (sdgvOverallPerformance.PrimaryGrid != null && sdgvOverallPerformance.PrimaryGrid.Rows != null)
                    sdgvOverallPerformance.PrimaryGrid.Rows.Clear();

                foreach (DataRow dr in drrOverall)
                {
                    Image img;
                    DataRow[] drrImage = dtImages.Select("EmployeeID = '" + dr["EmployeeNo"] + "'");
                    if (drrImage.Length > 0)
                    {
                        byte[] bStream = (byte[])drrImage[0]["EmployeeImage"];
                        MemoryStream ms = new MemoryStream(bStream);
                        img = Image.FromStream(ms);
                    }
                    else
                        img = GCC.Properties.Resources.Misc_User_icon__1_;


                    GridCell gr0 = new GridCell(img);
                    gr0.EditorParams = new object[] { ImageSizeMode.Zoom };

                    string sHTMLValue = string.Empty;

                    sHTMLValue += "<font size = '10' face = 'Arial' ><div align = 'Left'> <div  align = 'left'><font size = '15' face = 'Arial' color ='CornflowerBlue'><b>" + ProperCaseHelper.NameANDJobTitleCasing(dr["AgentFullname"].ToString().ToLower(), "Name") + "</b></font></div>";
                    sHTMLValue += "<div align = 'right'>Points :<font size = '12' face = 'Arial' color ='IndianRed'><b>" + dr["MTDPOINTS"] + "</b></font></div>";
                    if (sShowType == "Show Peer")
                        sHTMLValue += "<div align = 'left'>Peer Rank &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: <font face = 'Arial' color = 'SeaGreen'><b>" + dr["TenurebasedRanking"] + "</b></font></div>";
                    else
                        sHTMLValue += "<div align = 'left'>MTD Rank &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: <font face = 'Arial' color = 'SeaGreen'><b>" + dr["OVERALLRANKING"] + "</b></font></div>";

                    sHTMLValue += "<font size = '9'><div align = 'left'>Tenure &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: " + dr["Tenure"].ToString().Replace("&", "&amp;") + "</div>";
                    sHTMLValue += "<div align = 'left'>Contacts Achieved &nbsp;&nbsp;: " + dr["AchievedContacts"] + "</div>";


                    //sHTMLValue += "<div>Company Processed : " + dr["NO_OF_CONTACTS_VALIDATED"] + "</div>";
                    if (sShowType == "Show Peer")
                        sHTMLValue += "<div align = 'right'>MTD Rank &nbsp;: <font face = 'Arial' color = 'SeaGreen'><b>" + dr["OVERALLRANKING"] + "</b></font></div></font> </div></font>";
                    else
                        sHTMLValue += "<div align = 'right'>Peer Rank &nbsp;: <font face = 'Arial' color = 'SeaGreen'><b>" + dr["TenurebasedRanking"] + "</b></font></div></font> </div></font>";



                    //sHTMLValue += "<font size = '10' face = 'Arial' ><div align = 'Left'> <div  align = 'left'><font size = '15' face = 'Arial' color ='CornflowerBlue'><b>" + ProperCaseHelper.NameANDJobTitleCasing(dr["AgentFullname"].ToString().ToLower(), "Name") + "</b></font></div>";
                    //sHTMLValue += "<div align = 'right'>Points :<font size = '12' face = 'Arial' color ='IndianRed'><b>" + dr["MTDPOINTS"] + "</b></font></div>";
                    //if (sShowType == "Show Peer")
                    //    sHTMLValue += "<div align = 'left'>Peer Rank &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: <font face = 'Arial' color = 'SeaGreen'><b>" + dr["TenurebasedRanking"] + "</b></font></div>";
                    //else
                    //    sHTMLValue += "<div align = 'left'>MTD Rank &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: <font face = 'Arial' color = 'SeaGreen'><b>" + dr["OVERALLRANKING"] + "</b></font></div>";

                    //sHTMLValue += "<font size = '9'><div align = 'left'>Tenure &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: " + dr["Tenure"].ToString().Replace("&", "&amp;") + "</div>";
                    //sHTMLValue += "<div align = 'left'>Contacts Achieved &nbsp;&nbsp;: " + dr["AchievedContacts"] + "</div>";

                    //sHTMLValue += "<table><tr><td>abcd</td><td>def</td></tr></table>";









                    GridCell gr1 = new GridCell();
                    //DevComponents.DotNetBar.SuperGrid.Style.Background b;
                    //switch (dr["Tenure"].ToString())
                    //{
                    //    case "1 Month":
                    //        b = new DevComponents.DotNetBar.SuperGrid.Style.Background(Color.LightCyan);
                    //        break;

                    //    case "2 Months":
                    //        b = new DevComponents.DotNetBar.SuperGrid.Style.Background(Color.BlanchedAlmond);
                    //        break;

                    //    case "3 Months":
                    //        b = new DevComponents.DotNetBar.SuperGrid.Style.Background(Color.FromArgb(255, 197, 242, 203));
                    //        break;

                    //    case "4 & 5 Months":
                    //        b = new DevComponents.DotNetBar.SuperGrid.Style.Background(Color.MistyRose);
                    //        break;

                    //    case "6 Months & Above":
                    //        b = new DevComponents.DotNetBar.SuperGrid.Style.Background(Color.Wheat);
                    //        break;

                    //    case "Not Qualified":
                    //        b = new DevComponents.DotNetBar.SuperGrid.Style.Background(Color.Tomato);
                    //        break;

                    //    default:
                    //        b = new DevComponents.DotNetBar.SuperGrid.Style.Background(Color.White);
                    //        break;
                    //}
                    //gr1.CellStyles.Default.Background = b;
                    //gr0.CellStyles.Default.Background = b;
                    //gr1.EditorType = typeof(GridTextBoxXEditControl);
                    //gr1.RenderType = typeof(GridTextBoxXEditControl);
                    gr1.Value = sHTMLValue;

                    //GridCell gr1 = new GridCell(ProperCaseHelper.NameANDJobTitleCasing(dr[0].ToString().ToLower(), "Name"));

                    GridRow gr = new GridRow();
                    gr.Cells.Add(gr0);
                    gr.Cells.Add(gr1);
                    sdgvOverallPerformance.PrimaryGrid.Rows.Add(gr);
                }
            }
            else
                sdgvOverallPerformance.PrimaryGrid.Header.Text = "<div align = 'Right'><font size = '13'><b>Month to Date Performance</b></font></div>";
        }

        public static byte[] ReadImageFile(string imageLocation)
        {
            try
            {
                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(imageLocation);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return imageData;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
        }

        private void pictureDP_MouseDown(object sender, MouseEventArgs e)
        {
            frmUploadPicture objfrmUploadPicture = new frmUploadPicture();
            objfrmUploadPicture.ShowDialog();
        }

        private void pictureDP_MouseEnter(object sender, EventArgs e)
        {
            lblUpload.Visible = true;
        }

        private void pictureDP_MouseLeave(object sender, EventArgs e)
        {
            lblUpload.Visible = false;
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTarget.Text.Length > 0 && txtTarget.Value > 1)
                {
                    if (dtDaily_Agent_Perfoemance.Rows.Count > 0)
                    {
                        dtDaily_Agent_Perfoemance.Rows[0]["SELF_TARGET"] = txtTarget.Value.ToString();
                        GV.MSSQL.BAL_SaveToTable(dtDaily_Agent_Perfoemance.GetChanges(DataRowState.Modified), "DAILY_AGENT_PERFORMANCE_V1", "Update", true);
                    }
                    else
                    {
                        DataRow drNewAgentWisePerformance = dtDaily_Agent_Perfoemance.NewRow();
                        drNewAgentWisePerformance["DASHBOARD_ID"] = GV.sDashBoardID;
                        drNewAgentWisePerformance["FLAG"] = GV.sAccessTo;
                        drNewAgentWisePerformance["DATECALLED"] = GM.GetDateTime();
                        drNewAgentWisePerformance["AGENTNAME"] = GV.sEmployeeName;
                        drNewAgentWisePerformance["SELF_TARGET"] = txtTarget.Value.ToString();
                        dtDaily_Agent_Perfoemance.Rows.Add(drNewAgentWisePerformance);
                        GV.MSSQL.BAL_SaveToTable(dtDaily_Agent_Perfoemance.GetChanges(DataRowState.Added), "DAILY_AGENT_PERFORMANCE_V1", "New", true);
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

        private void splitContainerGrids_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if(sdgvProjectPerformance.VScrollBar.Visible)
                sdgvProjectPerformance.PrimaryGrid.Columns[1].Width = splitContainerGrids.Panel1.Width -135;
            else
                sdgvProjectPerformance.PrimaryGrid.Columns[1].Width = splitContainerGrids.Panel1.Width - 117;

            if(sdgvOverallPerformance.VScrollBar.Visible)
                sdgvOverallPerformance.PrimaryGrid.Columns[1].Width = splitContainerGrids.Panel2.Width -135;
            else
                sdgvOverallPerformance.PrimaryGrid.Columns[1].Width = splitContainerGrids.Panel2.Width - 117;

            //btnShowAllProject.Location = new Point(splitContainerGrids.SplitterDistance - 100, btnShowAllProject.Location.Y);
            btnShowAllProject.Location = new Point(sdgvProjectPerformance.Location.X + 10, sdgvProjectPerformance.Location.Y + 25);
            //ToastNotification.Show(this, splitContainerGrids.Panel1.Width + " " + splitContainerGrids.Panel2.Width);
        }

        private void splitContainerInformation_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (sdgvProjectPerformance.VScrollBar.Visible)
                sdgvProjectPerformance.PrimaryGrid.Columns[1].Width = splitContainerGrids.Panel1.Width - 135;
            else
                sdgvProjectPerformance.PrimaryGrid.Columns[1].Width = splitContainerGrids.Panel1.Width - 117;

            if (sdgvOverallPerformance.VScrollBar.Visible)
                sdgvOverallPerformance.PrimaryGrid.Columns[1].Width = splitContainerGrids.Panel2.Width - 135;
            else
                sdgvOverallPerformance.PrimaryGrid.Columns[1].Width = splitContainerGrids.Panel2.Width - 117;

            //btnShowAllMTD.BringToFront();
            btnShowAllMTD.Location = new Point(sdgvOverallPerformance.Location.X + 10, sdgvOverallPerformance.Location.Y + 25);
        }

        private void btnShowAllProject_Click(object sender, EventArgs e)
        {
            btnShowAllProject.Checked = (!btnShowAllProject.Checked);

            if (btnShowAllProject.Text == "Show All")
                btnShowAllProject.Text = "Show Peer";
            else
                btnShowAllProject.Text = "Show All";

            ProjectPerformance_FillGrid();
        }

        private void btnShowAllMTD_Click(object sender, EventArgs e)
        {
            btnShowAllMTD.Checked = (!btnShowAllMTD.Checked);

            if (btnShowAllMTD.Text == "Show All")
                btnShowAllMTD.Text = "Show Peer";
            else
                btnShowAllMTD.Text = "Show All";


            OverAllPerformance_FillGrid();
        }
    }
}
