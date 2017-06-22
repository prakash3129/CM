using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;

namespace GCC
{
    public partial class frmRDPMonitor : DevComponents.DotNetBar.Office2007Form
    {
        public frmRDPMonitor()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new System.Drawing.Font(this.Font.FontFamily, 22);
            MessageBoxEx.EnableGlass = false;
        }

        byte[] imgGreen;
        byte[] imgGrey;
        byte[] imgRed;
        DateTime dLastUpdated = default(DateTime);
        DataTable dtMonitor = new DataTable();
        private void frmRDPMonitor_Load(object sender, EventArgs e)
        {
            using (var memoryStream = new System.IO.MemoryStream()) //Grey Icon
            {
                Properties.Resources.Green_Ball_icon.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                imgGreen = memoryStream.ToArray();
            }
            using (var memoryStream = new System.IO.MemoryStream()) //Grey Icon
            {
                Properties.Resources.Grey_Ball_icon__2_.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                imgGrey = memoryStream.ToArray();
            }
            using (var memoryStream = new System.IO.MemoryStream()) //Grey Icon
            {
                Properties.Resources.Red_Ball_icon.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                imgRed = memoryStream.ToArray();
            }

            LoadTable();
        }

        void LoadTable()
        {
            dtMonitor = GV.MSSQL1.BAL_ExecuteQuery("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT M.MachineID, L.WHO AS [User],M.HostName AS Host,M.IP, M.Status,M.SystemState,RDPPort,S.PROJECT_NAME AS 'Project Name',L.PROJECTID AS 'Project ID', M.LastUpdatedDate, L.USERTYPE AS Level, L.RESEARCHTYPE AS 'Access To', M.CMVersion AS Version FROM c_machines AS M  INNER JOIN (SELECT USERTYPE,RESEARCHTYPE,SESSIONID,PROJECTID,WHO FROM c_log WHERE ACTION = 'Project LoggedIn' AND Cast([WHEN] as date) IN (Cast(getdate()-1 as date))) as L ON M.LastSession = L.SESSIONID INNER JOIN c_project_settings S ON M.LASTLOGGEDPROJECTID = S.PROJECT_ID ORDER BY M.Status DESC;");
            dtMonitor.Columns.Add("LastUpdated");
            foreach (DataRow drMonitor in dtMonitor.Rows)
            {
                drMonitor["LastUpdated"] = GM.TimeAgo(Convert.ToDateTime(drMonitor["LastUpdatedDate"]));
                drMonitor["Version"] = drMonitor["Version"].ToString().Replace("Campaign Manager ", string.Empty);
            }
            
            LoadGrid(dtMonitor);
            lblOnline.Text = " Online (" + dtMonitor.Select("Status = 'Online'").Length + ")";
            dLastUpdated = GM.GetDateTime();
            lblLastUpdated.Text = "Last Updated : Now";
        }

        void LoadGrid(DataTable dtMonitorGrid)
        {
            try
            {
                if(!dtMonitorGrid.Columns.Contains("ColumnPic"))
                    dtMonitorGrid.Columns.Add("ColumnPic", typeof(byte[]));

                foreach (DataRow drMonitor in dtMonitorGrid.Rows)
                {
                    if (drMonitor["Status"].ToString() == "Online")
                        drMonitor["ColumnPic"] = imgGreen;
                    else if (drMonitor["Status"].ToString() == "Offline")
                        drMonitor["ColumnPic"] = imgGrey;
                    else
                        drMonitor["ColumnPic"] = imgRed;
                }

                sdgvMonitor.PrimaryGrid.DataSource = dtMonitorGrid;
                sdgvMonitor.PrimaryGrid.Columns["MachineID"].Visible = false;                               
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void timerLastUpdate_Tick(object sender, EventArgs e)
        {
            lblLastUpdated.Text = "Last Updated : " + GM.TimeAgo(dLastUpdated);
        }

        private void sdgvMonitor_RowDoubleClick(object sender, GridRowDoubleClickEventArgs e)
        {
            try
            {
                GridRow gRow = ((GridRow)e.GridRow);
                DataTable dtMachine = GV.MSSQL1.BAL_ExecuteQuery("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT M.MachineID, L.WHO AS [User],M.HostName AS Host,M.IP,M.Status, M.SystemState,RDPPort,S.PROJECT_NAME AS 'Project Name',L.PROJECTID AS 'Project ID', M.LastUpdatedDate, L.USERTYPE AS Level, L.RESEARCHTYPE AS 'Access To',L.SESSIONID AS Session, M.CMVersion AS Version FROM c_machines AS M INNER JOIN c_log L ON M.LastSession = L.SESSIONID INNER JOIN c_project_settings S ON M.LASTLOGGEDPROJECTID = S.PROJECT_ID WHERE L.ACTION='Project LoggedIn' AND M.MachineID='" + gRow.Cells["MachineID"].Value + "';");
                if (dtMachine.Rows[0]["Status"].ToString() == "Online")
                {
                    if (!GM.IsFormExist("frmScreen"))
                    {
                        using (frmScreen objfrmScreen = new frmScreen())
                        {
                            if (dtMachine.Rows[0]["SystemState"].ToString() == "" || dtMachine.Rows[0]["SystemState"].ToString() == "SessionUnlock")
                            {

                                using (System.Diagnostics.Process p = new System.Diagnostics.Process())
                                {
                                    p.StartInfo.FileName = GV.sScreenAddonPath;
                                    p.StartInfo.Arguments = dtMachine.Rows[0]["IP"].ToString() + " " + dtMachine.Rows[0]["RDPPort"].ToString() + " Pr@k@sH";
                                    p.StartInfo.UseShellExecute = false;
                                    p.StartInfo.CreateNoWindow = true;
                                    p.Start();
                                }
                                    //System.Diagnostics.Process.Start(GV.sScreenAddonPath + " " + dtMachine.Rows[0]["IP"].ToString() + " " + dtMachine.Rows[0]["RDPPort"].ToString() + " Pr@k@sH");

                                //objfrmScreen.IP = dtMachine.Rows[0]["IP"].ToString();
                                //objfrmScreen.Port = dtMachine.Rows[0]["RDPPort"].ToString();
                                //objfrmScreen.Agent = gRow.Cells["User"].Value.ToString();
                                //objfrmScreen.Project = gRow.Cells["Project Name"].Value.ToString();
                                //objfrmScreen.ShowDialog();
                            }
                            else
                                ToastNotification.Show(this, "System not active. State : " + dtMachine.Rows[0]["SystemState"].ToString(), eToastPosition.TopRight);

                            //string sMessage = objfrmScreen.StartSession();
                            //if (sMessage.Length > 0)
                            //{
                            //    objfrmScreen.Close();
                            //    Reload();
                            //    ToastNotification.Show(this, "Error connecting Machine", eToastPosition.TopRight);
                            //}
                        }
                    }
                    
                }
                else
                {
                    LoadTable();
                    ToastNotification.Show(this, "Machine is not online", eToastPosition.TopRight);
                }
            }
            catch(Exception ex)
            {                
                    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Visible = true;
            btnSearch.Visible = false;
            bar1.Refresh();
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {            
            string sSearch = txtSearch.Text.Replace("'", "''").Replace("  ", " ");
            DataRow[] drrMonitor = dtMonitor.Select("User LIKE '%"+ sSearch + "%' OR Host LIKE '%" + sSearch + "%' OR IP LIKE '%" + sSearch + "%' OR [Project Name] LIKE '%" + sSearch + "%' OR Version LIKE '%" + sSearch + "%'");
            if (drrMonitor.Length > 0)
            {
                using (DataTable dtMon = drrMonitor.CopyToDataTable())
                {
                    dtMon.DefaultView.Sort = "Status DESC";
                    LoadGrid(dtMon);
                }
            }
            else
                sdgvMonitor.PrimaryGrid.DataSource = null;
        }

        private void txtSearch_LostFocus(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim().Length == 0)
            {
                txtSearch.Visible = false;
                btnSearch.Visible = true;
                bar1.Refresh();
            }
        }

        private void frmRDPMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
           System.Diagnostics.Process[] pLinkedIn = System.Diagnostics.Process.GetProcessesByName("ScreenAddon");
            if (pLinkedIn.Length > 0)
                pLinkedIn[0].Kill(); // Kill if the bot is already running
        }
    }
}
