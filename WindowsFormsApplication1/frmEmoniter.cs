using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmMoniter : MetroForm
    {
        DataTable dtEmoniter;
        int iTimer = 0;
        List<MetroTileItem> lstTileControls = new List<MetroTileItem>();
        DataTable dtEmonitorUsers;
        string sEmonitorUsersID = string.Empty;
        string sMoniterUsersEmpID = string.Empty;
        List<int> lstProjectNameShow = new List<int>();
        

        //void OnChange(object sender, SqlNotificationEventArgs e)
        //{
        //    MessageBox.Show(e.Source.ToString());
        //}
        public frmMoniter()
        {
            StyleManager.MetroColorGeneratorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(ColorScheme.GetColor("2F6496"), ColorScheme.GetColor("2F6496"));
            //StyleManager.MetroColorGeneratorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(ColorScheme.GetColor("0E6D38"), ColorScheme.GetColor("0E6D38"));



            InitializeComponent();
            lstProjectNameShow.Add(3);
            lstProjectNameShow.Add(4);
            lstProjectNameShow.Add(5);
            lstProjectNameShow.Add(8);
            this.WindowState = FormWindowState.Maximized;
            Load();
            pageSliderEmonitor.SelectedPage = page1Emonitor;

        }

        string Truncate(string value, int maxLength)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
            {
                return value.Substring(0, maxLength) + "...";
            }

            return value;
        }


        void Load()
        {
            itemContainerEmonitor.SubItems.Clear();
            dtEmonitorUsers = GV.MSSQL.BAL_ExecuteQuery("SELECT * FROM EMonitor_Users WHERE EMPLOYEID = '" + GV.sEmployeeNo + "';");
            lblNoUsers.Visible = false;
            if (dtEmonitorUsers.Rows.Count > 0)
            {
                sEmonitorUsersID = GM.ColumnToQString("EMONITORID", dtEmonitorUsers, "Int");                
                dtEmoniter = GV.MSSQL.BAL_ExecuteQuery("SELECT * FROM EMoniter WHERE ID IN (" + sEmonitorUsersID + ")");
                sMoniterUsersEmpID = GM.ColumnToQString("EMPLOYE_ID", dtEmoniter, "String");
                int iMoniterCount = dtEmoniter.Rows.Count;
                //            return;
                foreach (DataRow dr in dtEmoniter.Rows)
                {


                    MetroTileFrame frameNotLogged = new MetroTileFrame();                    
                    frameNotLogged.TileStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                    frameNotLogged.TileStyle.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(65)))), ((int)(((byte)(66)))));
                    frameNotLogged.TileStyle.CornerType = eCornerType.Square;
                    frameNotLogged.TileStyle.TextAlignment = eStyleTextAlignment.Center;
                    frameNotLogged.TitleTextAlignment = ContentAlignment.TopLeft;
                    //frameNotLogged.Text = sDisplsy;

                    MetroTileFrame frameLogged = new MetroTileFrame();                    
                    frameLogged.TileStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                    frameLogged.TileStyle.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(130)))), ((int)(((byte)(132)))));
                    frameLogged.TileStyle.CornerType = eCornerType.Square;
                    frameLogged.ImageIndent = new Point(4, -6);
                    frameLogged.ImageTextAlignment = ContentAlignment.BottomRight;
                    frameLogged.Image = global::GCC.Properties.Resources._129396005132x3220x20_new;
                    frameLogged.TileStyle.TextAlignment = eStyleTextAlignment.Center;
                    frameLogged.TitleTextAlignment = ContentAlignment.TopLeft;
                    //frameLogged.Text = sDisplsy;

                    MetroTileFrame frameProduction = new MetroTileFrame();
                    frameProduction.TileStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                    frameProduction.TileStyle.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(130)))), ((int)(((byte)(132)))));
                    frameProduction.TileStyle.CornerType = eCornerType.Square;
                    frameProduction.ImageIndent = new Point(4, -6);
                    frameProduction.ImageTextAlignment = ContentAlignment.BottomRight;
                    frameProduction.Image = global::GCC.Properties.Resources.Prod_new_dash;
                    frameProduction.TileStyle.TextAlignment = eStyleTextAlignment.Center;
                    frameProduction.TitleTextAlignment = ContentAlignment.TopLeft;
                   // frameProduction.Text = sDisplsy;

                    MetroTileFrame framePreCall = new MetroTileFrame();                    
                    framePreCall.TileStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(65)))), ((int)(((byte)(0)))));
                    framePreCall.TileStyle.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(130)))), ((int)(((byte)(0)))));                    
                    framePreCall.TileStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                    framePreCall.ImageIndent = new System.Drawing.Point(4, -6);
                    framePreCall.ImageTextAlignment = System.Drawing.ContentAlignment.BottomRight;
                    framePreCall.Image = global::GCC.Properties.Resources.icon256_24;
                    framePreCall.TileStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                    framePreCall.TitleTextAlignment = System.Drawing.ContentAlignment.TopLeft;
                    //framePreCall.Text = sDisplsy;

                    MetroTileFrame frameOnCall = new MetroTileFrame();
                    frameOnCall.TileStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(0)))));
                    frameOnCall.TileStyle.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(195)))), ((int)(((byte)(0)))));
                    frameOnCall.TileStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                    frameOnCall.ImageIndent = new System.Drawing.Point(4, -6);
                    frameOnCall.ImageTextAlignment = System.Drawing.ContentAlignment.BottomRight;
                    frameOnCall.Image = global::GCC.Properties.Resources.contact_center_badge_lg_24;
                    frameOnCall.TileStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                    frameOnCall.TitleTextAlignment = System.Drawing.ContentAlignment.TopLeft;
                   // frameOnCall.Text = sDisplsy;

                    MetroTileFrame framePostCall = new MetroTileFrame();                    
                    framePostCall.TileStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(65)))), ((int)(((byte)(0)))));
                    framePostCall.TileStyle.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(195)))), ((int)(((byte)(0)))));
                    framePostCall.TileStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                    framePostCall.ImageIndent = new System.Drawing.Point(4, -6);
                    framePostCall.ImageTextAlignment = System.Drawing.ContentAlignment.BottomRight;
                    framePostCall.Image = global::GCC.Properties.Resources.HangupICO_24;
                    framePostCall.TileStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                    framePostCall.TitleTextAlignment = System.Drawing.ContentAlignment.TopLeft;
                    //framePostCall.Text = sDisplsy;

                    MetroTileFrame frameTraining = new MetroTileFrame();                    
                    frameTraining.TileStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(66)))));
                    frameTraining.TileStyle.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(195)))), ((int)(((byte)(198)))));
                    frameTraining.TileStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                    frameTraining.ImageIndent = new System.Drawing.Point(4, -6);
                    frameTraining.ImageTextAlignment = System.Drawing.ContentAlignment.BottomRight;
                    frameTraining.Image = global::GCC.Properties.Resources.conference_icon;
                    frameTraining.TileStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                    frameTraining.TitleTextAlignment = System.Drawing.ContentAlignment.TopLeft;
                   // frameTraining.Text = sDisplsy;

                    MetroTileFrame frameBreak = new MetroTileFrame();                    
                    frameBreak.TileStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(0)))), ((int)(((byte)(66)))));
                    frameBreak.TileStyle.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(0)))), ((int)(((byte)(198)))));
                    frameBreak.TileStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                    frameBreak.ImageIndent = new System.Drawing.Point(4, -6);
                    frameBreak.ImageTextAlignment = System.Drawing.ContentAlignment.BottomRight;
                    frameBreak.Image = global::GCC.Properties.Resources.coffeeicon24x24;
                    frameBreak.TileStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                    frameBreak.TitleTextAlignment = System.Drawing.ContentAlignment.TopLeft;
                    //frameBreak.Text = sDisplsy;

                    MetroTileFrame frameDownTime = new MetroTileFrame();
                    frameDownTime.TileStyle.BackColor = System.Drawing.Color.DarkRed;
                    frameDownTime.TileStyle.BackColor2 = System.Drawing.Color.Brown;
                    frameDownTime.TileStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                    frameDownTime.ImageIndent = new System.Drawing.Point(4, -6);
                    frameDownTime.ImageTextAlignment = System.Drawing.ContentAlignment.BottomRight;
                    frameDownTime.Image = global::GCC.Properties.Resources.sign_warning_icon;
                    frameDownTime.TileStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                    frameDownTime.TitleTextAlignment = System.Drawing.ContentAlignment.TopLeft;
                    //frameBreak.Text = sDisplsy;

                    MetroTileItem mTile = new MetroTileItem();
                    //xx.Image = global::GCC.Properties.Resources.Yello;
                    //xx.ImageIndent = new System.Drawing.Point(0, -6);
                    //xx.ImageTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    mTile.Name = dr["ID"].ToString();
                    mTile.TileSize = new Size(200, 100);
                    mTile.Frames.Add(frameNotLogged);//1. Offline
                    mTile.Frames.Add(frameLogged);//2. Online
                    mTile.Frames.Add(framePreCall);//3. Precall
                    mTile.Frames.Add(frameOnCall);//4. Call in pgress
                    mTile.Frames.Add(framePostCall);//5. Postcall
                    mTile.Frames.Add(frameTraining);//6. Training
                    mTile.Frames.Add(frameBreak);//7. Break
                    mTile.Frames.Add(frameProduction);//8. Prodcution
                    mTile.Frames.Add(frameDownTime);//9. Downtime
                    mTile.CheckedChanged += new EventHandler(Tile_CheckedChanged);
                    //xx.SymbolColor = Color.Empty;
                    mTile.CurrentFrame = GetFrame(dr);                                        
                    mTile.Frames[mTile.CurrentFrame].Text = GetDisplayText(dr, mTile.CurrentFrame);
                    lstTileControls.Add(mTile);
                    itemContainerEmonitor.SubItems.AddRange(new BaseItem[] { mTile });
                    
                }
                timerRefresh.Enabled = true;
                lblInfo.Text = "Online";
                //metroTileItem1.Text = "<br/><br/><br/><br/><br/><font color = 'DarkSlateGray'><p align='right'>Not Logged In</p></font>";
            }
            else
            {
                timerRefresh.Enabled = false;
                lblNoUsers.Visible = true;
                lblInfo.Text = "Offline";
            }
            metroStatusBar1.Refresh();
            itemContainerEmonitor.Refresh();
            metroTilePanelEmonitor.Refresh();
        }

        string GetDisplayText(DataRow drMonitor, int iFrame)
        {
            string sDisplsy = string.Empty;
            sDisplsy += "<b><font size='+2'>" + Truncate(GM.ProperCase(drMonitor["EMPLOYE_NAME"].ToString()), 17) + "</font></b><br/>";

            if (lstProjectNameShow.Contains(iFrame))
                sDisplsy += "<i><font size='-2'>" + Truncate(drMonitor["PROJECT_NAME"].ToString(), 27) + "</font></i>";

            if (drMonitor["LAST_UPDATED_ON"].ToString().Length > 0)
            {
                if(GM.TimeAgo(Convert.ToDateTime(drMonitor["LAST_UPDATED_ON"])).Length == 0)
                { }
                sDisplsy += "<br/><br/><br/>Last Activity : " + GM.TimeAgo(Convert.ToDateTime(drMonitor["LAST_UPDATED_ON"]));
            }
            return sDisplsy;
        }


        int GetFrame(DataRow drMoniter)
        {
            if (drMoniter["LOGOUT_TIME"].ToString() == drMoniter["LAST_UPDATED_ON"].ToString())
                return 1;

            //if (drMoniter["LAST_UPDATED_ON"].ToString().Length > 0 && (GM.GetDateTime() - Convert.ToDateTime(drMoniter["LAST_UPDATED_ON"].ToString())).TotalHours >= 4)
            //    return 1;

            string sActivity = drMoniter["ACTIVITY"].ToString();
            string sCallStatus = drMoniter["CALL_STATUS"].ToString();

            if (sActivity.StartsWith("Meeting-"))
                return 6;
            if (sActivity.StartsWith("Other (") || sActivity == "Break")
                return 7;
            if (sActivity.StartsWith("Downtime"))
                return 9;
            if (sCallStatus == "Pre Call")
                return 3;
            if (sCallStatus == "Call in Progress")
                return 4;
            if (sCallStatus == "Post Call")
                return 5;
            if (sActivity.StartsWith("Production-"))
                return 8;
            if (sActivity.StartsWith("Administration W") || sCallStatus == "Idle" || sActivity.Length == 0)
                return 2;
            
            return 1;
        }

        private void metroTileItem3_Click(object sender, EventArgs e)
        {
            MetroTileItem xx = new MetroTileItem();
            xx.Image = global::GCC.Properties.Resources.Yello;
            xx.ImageIndent = new System.Drawing.Point(0, -6);
            xx.ImageTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            xx.Name = "metroTileItem2";
            xx.SymbolColor = System.Drawing.Color.Empty;
            xx.Text = "Store";
            xx.TileColor = DevComponents.DotNetBar.Metro.eMetroTileColor.RedOrange;
            xx.TileSize = new System.Drawing.Size(248, 120);
            // itemContainer1.SubItems.Add(xx);

            itemContainerEmonitor.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] { xx });
            itemContainerEmonitor.Refresh();
        }


        private void metroTileItem1_Click(object sender, EventArgs e)
        {

        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            iTimer++;

            if (iTimer % 5 == 0)
            {
                dtEmoniter = GV.MSSQL.BAL_ExecuteQuery("SELECT * FROM EMoniter WHERE ID IN ("+ sEmonitorUsersID + ")");
                foreach (MetroTileItem mTile in lstTileControls)
                {
                    DataRow[] drrMoniter = dtEmoniter.Select("ID = '" + mTile.Name + "'");
                    if (drrMoniter.Length > 0)
                    {
                        int iFrameIndex = GetFrame(drrMoniter[0]);
                        if (mTile.CurrentFrame != iFrameIndex)
                        {                            
                            mTile.Frames[iFrameIndex].Text = GetDisplayText(drrMoniter[0], iFrameIndex); ;
                            mTile.CurrentFrame = iFrameIndex;                            
                        }
                    }
                }
            }

            if(iTimer == 60)//60 sec refresh rate for updating project change and last updated time
            {
                iTimer = 0;
                foreach (MetroTileItem mTile in lstTileControls)
                {
                    DataRow[] drrMoniter = dtEmoniter.Select("ID = '" + mTile.Name + "'");
                    if (drrMoniter.Length > 0)
                        mTile.Frames[mTile.CurrentFrame].Text = GetDisplayText(drrMoniter[0], mTile.CurrentFrame);
                }
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            //StyleManager.MetroColorGeneratorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(ColorScheme.GetColor("0E6D80"), ColorScheme.GetColor("0E6D80"));
            frmComboList objFrmComboList = new frmComboList();
            DataTable dtAllUsers = GV.MSSQL.BAL_FetchTable("Timesheet..Users", "Active='Y'");//All active time logger users;
            objFrmComboList.FormBorderStyle = FormBorderStyle.FixedSingle;
            objFrmComboList.TitleText = "Select User";

            if (sMoniterUsersEmpID.Length > 0 && dtAllUsers.Select("EMPLOYEENO NOT IN (" + sMoniterUsersEmpID + ")").Length > 0)
                objFrmComboList.dtItems = dtAllUsers.Select("EMPLOYEENO NOT IN (" + sMoniterUsersEmpID + ")").CopyToDataTable();
            else if (sMoniterUsersEmpID.Length == 0)
                objFrmComboList.dtItems = dtAllUsers;
            else
                objFrmComboList.dtItems = null;

            objFrmComboList.lstColumnsToDisplay.Add("UserName");
            objFrmComboList.sColumnToSearch = "UserName";
            objFrmComboList.IsSpellCheckEnabeld = false;
            objFrmComboList.IsMultiSelect = true;
            objFrmComboList.IsSingleWordSelection = false;
            objFrmComboList.StartPosition = FormStartPosition.CenterScreen;
            //objFrmComboList.ShowInTaskbar = false;
            objFrmComboList.MinimizeBox = false;
            objFrmComboList.MaximizeBox = false;
            objFrmComboList.ShowDialog(this);
            if (objFrmComboList.sReturn != null && objFrmComboList.sReturn.Length > 0)
            {
                DataRow[] drrSelectedUsers = dtAllUsers.Select("USERNAME IN('" + objFrmComboList.sReturn.Replace("|", "','") + "')");
                if (drrSelectedUsers.Length > 0)
                {
                    if (DialogResult.Yes == MessageBoxEx.Show("Are you sure to add these selected users to watchlist ?", "e-Monitor", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        string sEMPs = GM.ColumnToQString("EMPLOYEENO", drrSelectedUsers.CopyToDataTable(), "String");
                        GV.MSSQL.BAL_ExecuteQuery("INSERT INTO EMoniter (EMPLOYE_NAME, EMPLOYE_ID) SELECT Fullname, EmployeeNo FROM Timesheet..Users WHERE Active = 'Y' AND EmployeeNo IN(" + sEMPs + ") AND EmployeeNo NOT in (SELECT EMPLOYE_ID from Emoniter);");
                        GV.MSSQL.BAL_ExecuteQuery("INSERT into EMonitor_Users (EMPLOYEID, EMONITORID) Select '" + GV.sEmployeeNo + "', ID from EMoniter WHERE EMPLOYE_ID IN(" + sEMPs + ") AND ID NOT IN(SELECT EMONITORID FROM EMonitor_Users WHERE EMPLOYEID = '" + GV.sEmployeeNo + "')");
                        ToastNotification.Show(this, "Users added to watch list.", eToastPosition.TopRight);
                        //DataTable dtDuplicateUsers = GV.MSSQL.BAL_ExecuteQuery("Select * from Emoniter where EMPLOYEID IN (" + sEMPs + ")");
                        //if(dtDuplicateUsers.Rows.Count > 0)
                        //{

                        //}
                        //else
                        //{

                        Load();
                        //}
                    }
                }
            }
        }


        private void Tile_CheckedChanged(object sender, EventArgs e)
        {
            foreach (MetroTileItem mTile in lstTileControls)
            {
                if (mTile.Checked)
                {
                    btnRemove.Visible = true;
                    return;
                }
            }
            btnRemove.Visible = false;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string sMoniterID = string.Empty;
            foreach (MetroTileItem mTile in lstTileControls)
            {
                if (mTile.Checked)
                {
                    if (sMoniterID.Length > 0)
                        sMoniterID += "," + mTile.Name;
                    else
                        sMoniterID = mTile.Name;
                }
            }

            if (sMoniterID.Length > 0)
            {
                if (DialogResult.Yes == MessageBoxEx.Show("Are you sure to remove these selected users from watchlist ?", "e-Monitor", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    GV.MSSQL.BAL_ExecuteQuery("DELETE FROM EMonitor_Users WHERE EMPLOYEID = '" + GV.sEmployeeNo + "' AND EMONITORID IN (" + sMoniterID + ");");
                    ToastNotification.Show(this, "Users removed from watch list.", eToastPosition.TopRight);
                    Load();
                }
            }
            btnRemove.Visible = false;
        }

        private void frmMoniter_FormClosing(object sender, FormClosingEventArgs e)
        {
            //StyleManager.ChangeStyle(eStyle.Office2010Blue, Color.FromName("0"));
            StyleManager.Style = StyleManager.PreviousStyle;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {

            

            if (pageSliderEmonitor.NextPageVisibleMargin == 230)
                pageSliderEmonitor.NextPageVisibleMargin = 0;
            else
                pageSliderEmonitor.NextPageVisibleMargin = 230;
            //if (pageSliderEmonitor.SelectedPage == page1Emonitor)
            //    pageSliderEmonitor.SelectedPage = page2Legend;
            //else
            //    pageSliderEmonitor.SelectedPage = page1Emonitor;
        }
    }
}