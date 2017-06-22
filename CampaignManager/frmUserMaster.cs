using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmUserMaster : DevComponents.DotNetBar.Office2007Form
    {
        private DataTable _dtGCCUsers;
        private DataTable _dtAllUsers;
        DataTable dtGCCUsers_log;
        public DataTable dtGCCUsers
        {
            get { return _dtGCCUsers; }
            set { _dtGCCUsers = value; }
        }
        public DataTable dtAllUsers
        {
            get { return _dtAllUsers; }
            set { _dtAllUsers = value; }
        }

        public frmUserMaster()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
        }
        //BAL.BAL_GlobalMydSQL objBal_GlobalMyfdSQL = new BAL.BAL_GlobalMySfdQL();

        private void frmUserMaster_Load(object sender, EventArgs e) // Text box hidden for close
        {
            try
            {
                dtGCCUsers_log = dtGCCUsers.Copy();
                dgvUser.DataSource = dtGCCUsers;
                dgvUser.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
                qCToolStripMenuItem.Visible = !(GV.sUserType == "Manager" || GV.sUserType == "Agent"); //What if manager selects agent and opens user edit window
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
         }

        private void dgvUser_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewColumn dgvc in dgvUser.Columns) //Hide irrelavent columns
                {
                    dgvc.Visible = false;
                    if (dgvc.Name == "USERNAME")
                    { dgvc.Visible = true; dgvc.HeaderText = "Name"; }
                    else if (dgvc.Name == "USERTYPE")
                    { dgvc.Visible = true; dgvc.HeaderText = "Permission"; }
                    else if (dgvc.Name == "USERACCESS")
                    { dgvc.Visible = true; dgvc.HeaderText = "Access to"; }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
             //DataRow[]drSearch = dtGCCUsers.Select("UserName LIKE '%" + txtSearch.Text + "%'");
             //if (drSearch.Length > 0)
             //    dgvUser.DataSource = drSearch.CopyToDataTable();
             //else
             //    dgvUser.DataSource = null;
        }

       

        private void managerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dgvr in dgvUser.SelectedRows)
                {
                    dgvr.Cells["USERTYPE"].Value = "Manager";
                    dgvr.Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                    dgvr.Cells["UPDATED_DATE"].Value = GM.GetDateTime();                    
                }
                UpdateTable();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void agentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dgvr in dgvUser.SelectedRows)
                {
                    dgvr.Cells["USERTYPE"].Value = "Agent";
                    dgvr.Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                    dgvr.Cells["UPDATED_DATE"].Value = GM.GetDateTime();                    
                }
                UpdateTable();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void qCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dgvr in dgvUser.SelectedRows)
                {
                    dgvr.Cells["USERTYPE"].Value = "QC";
                    dgvr.Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                    dgvr.Cells["UPDATED_DATE"].Value = GM.GetDateTime();                    
                }
                UpdateTable();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void webRearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dgvr in dgvUser.SelectedRows)
                {
                    dgvr.Cells["USERACCESS"].Value = "WR";
                    dgvr.Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                    dgvr.Cells["UPDATED_DATE"].Value = GM.GetDateTime();                    
                }
                UpdateTable();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void teleResearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dgvr in dgvUser.SelectedRows)
                {
                    dgvr.Cells["USERACCESS"].Value = "TR";
                    dgvr.Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                    dgvr.Cells["UPDATED_DATE"].Value = GM.GetDateTime();                    
                }
                UpdateTable();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void UpdateTable()
        {
            try
            {
                this.BindingContext[dtGCCUsers].EndCurrentEdit();

                GM.Logging(dtGCCUsers, dtGCCUsers_log, "C_USERS", "ID");

                bool IsDBAffected = false;
                if (dtGCCUsers.GetChanges(DataRowState.Added) != null)
                {
                    GV.MSSQL1.BAL_SaveToTable(dtGCCUsers.GetChanges(DataRowState.Added), "C_USERS", "New", true);
                    IsDBAffected = true;
                }
                if (dtGCCUsers.GetChanges(DataRowState.Modified) != null)
                {
                    GV.MSSQL1.BAL_SaveToTable(dtGCCUsers.GetChanges(DataRowState.Modified), "C_USERS", "Update", true);
                    IsDBAffected = true;
                }
                if (dtGCCUsers.GetChanges(DataRowState.Deleted) != null)
                {
                    GV.MSSQL1.BAL_SaveToTable(dtGCCUsers.GetChanges(DataRowState.Deleted), "C_USERS", "Delete", true);
                    IsDBAffected = true;
                }
                if (IsDBAffected)
                {
                    dtGCCUsers = GV.MSSQL1.BAL_FetchTable("C_USERS", "1=1");
                    dtGCCUsers_log = dtGCCUsers.Copy();
                    dgvUser.DataSource = dtGCCUsers;
                }                
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBoxEx.Show("Are you sure to remove Selected Agent(s) ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                {
                    foreach (DataGridViewRow dgvr in dgvUser.SelectedRows)                    
                        dgvUser.Rows.Remove(dgvUser.CurrentRow);                        
                    
                    UpdateTable();
                    if (dgvUser.Rows.Count > 0)
                    {
                        dgvUser.CurrentCell = dgvUser.Rows[0].Cells[3];
                        dgvUser.Rows[0].Selected = true;
                        //dgvUser_CellClick(dgvUser, new DataGridViewCellEventArgs(3, 0));
                    }
                }
            }
            catch (Exception ex)
            {

                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmComboList objFrmComboList = new frmComboList();
                objFrmComboList.TitleText = "Select User";
                objFrmComboList.dtItems = dtAllUsers;
                objFrmComboList.lstColumnsToDisplay.Add("UserName");
                objFrmComboList.sColumnToSearch = "UserName";
                objFrmComboList.IsSpellCheckEnabeld = false;
                objFrmComboList.IsMultiSelect = true;
                objFrmComboList.ShowDialog(this);

                if (objFrmComboList.sReturn != null && objFrmComboList.sReturn.Length > 0)
                {
                    List<string> lstUsers = new List<string>();
                    lstUsers = objFrmComboList.sReturn.Split('|').ToList();
                    foreach (string sUserName in lstUsers)
                    {
                        if (dtGCCUsers.Select(String.Format("UserName = '{0}'", sUserName)).Length == 0)
                        {
                            DataRow dr = dtGCCUsers.NewRow();
                            dr["UserName"] = sUserName;
                            dr["USERTYPE"] = "Agent";
                            dr["USERACCESS"] = GV.sAccessTo;
                            dr["CREATED_BY"] = GV.sEmployeeName;
                            dr["CREATED_DATE"] = GM.GetDateTime();
                            dtGCCUsers.Rows.Add(dr);                            
                        }
                    }
                    dgvUser.DataSource = dtGCCUsers;
                    UpdateTable();                    
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }   
        }

       

    }
}

