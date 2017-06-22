using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmTeleAllocation : DevComponents.DotNetBar.Office2007Form
    {
        public frmTeleAllocation()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
        }

        private DataTable _dtAllUsers = new DataTable();
        public DataTable dtAllUsers //All timelogger users to add new agents
        {
            get { return _dtAllUsers; }
            set {_dtAllUsers = value; }
        }

        DataTable dtFilterAssignment = new DataTable();
        DataTable dtAllocationFilter = new DataTable();
        DataTable dtFilterAssignment_Log = new DataTable();        
        ComboBox cmbFilters = new ComboBox();
        //BAL_GlobalMySfdQL objGlobalMySfdQL = new BAL_GlobalMySfdQL();
        int iCurrentRowIndex = -1;

        private void frmTeleAllocation_Load(object sender, EventArgs e)
        {
            this.Text = GV.sProjectName; //Project Name as Window title
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            dgvFilterAllocation.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;//Collor change to merge with  window
            LoadTables(); //Load datatables
            
            dgvFilterAllocation.ReadOnly = true;
            //if (dtAllocationFilter.Rows.Count > 0)
            //{
            //    DataRow[] drAllocationFilter = dtAllocationFilter.Select("Active = 'Y'"); //Gets only active filters
            //    if (drAllocationFilter.Length > 0)
            //    {
            //        cmbFilters.Items.Add(string.Empty);
            //        foreach (DataRow dr in drAllocationFilter)
            //            cmbFilters.Items.Add(dr["FILTER_NAME"].ToString()); //Add all filters to Filter Combo box
            //    }
            //}
            cmbFilters.DropDownClosed += new EventHandler(cmbFilters_DropDownClosed); //Runtime event(Combobox Close)
        }

        private void LoadTables()
        {
            try
            {
                dgvFilterAllocation.Columns.Clear();
                dtAllocationFilter = GV.MSSQL1.BAL_FetchTable("C_ALLOCATION_FILTER", "PROJECT_ID='" + GV.sProjectID + "' AND USERACCESS = '" + GV.sAccessTo + "'");//List of filters and their descriptions
                dtFilterAssignment = GV.MSSQL1.BAL_FetchTable("C_FILTER_ASSIGNMENT", "PROJECT_ID='" + GV.sProjectID + "' AND USERACCESS = '" + GV.sAccessTo + "'");//List of agents with filters assigned to them
                dtFilterAssignment_Log = dtFilterAssignment.Copy();
                dgvFilterAllocation.DataSource = dtFilterAssignment;

                DataGridViewColumn dgvColumn = new DataGridViewColumn();//Add Combobox column to show list of filters
                DataGridViewCell dgvCell = new DataGridViewTextBoxCell();
                dgvColumn.CellTemplate = dgvCell;
                dgvColumn.HeaderText = "Filter Name";
                dgvColumn.Name = "FILTER_NAME";
                dgvColumn.Visible = true;
                dgvColumn.Width = 60;
                dgvFilterAllocation.Columns.Add(dgvColumn);

                Populate_FilterNameColumn();//Show Already assigned filter Name
                Load_ComboBox();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Populate_FilterNameColumn()
        {
            try
            {
                foreach (DataGridViewRow dgvr in dgvFilterAllocation.Rows)//Show Already assigned filter Name from Filter ID
                {
                    if (dgvr.Cells["FILTER_ID"].Value.ToString().Length > 0)
                    {
                        DataRow[] drAllocation = dtAllocationFilter.Select("FILTER_ID = " + dgvr.Cells["FILTER_ID"].Value);
                        if (drAllocation.Length > 0)
                        {
                            dgvr.Cells["FILTER_NAME"].Value = drAllocation[0]["FILTER_NAME"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Load_ComboBox()
        {
            try
            {
                if (dtAllocationFilter.Rows.Count > 0)
                {
                    cmbFilters.Items.Clear();
                    DataRow[] drAllocationFilter = dtAllocationFilter.Select("Active = 'Y'"); //Gets only active filters
                    if (drAllocationFilter.Length > 0)
                    {
                        cmbFilters.Items.Add(string.Empty);
                        foreach (DataRow dr in drAllocationFilter)
                            cmbFilters.Items.Add(dr["FILTER_NAME"].ToString()); //Add all filters to Filter Combo box
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)//Close window on ESCAPE
        {
            /////Key press listener for overall window/////
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dgvUserFilterAllocation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete && dgvFilterAllocation != null && dgvFilterAllocation.Rows.Count > 0)//Delete DGV selected row
                    Dgv_DeleteSelectedRows();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvUserFilterAllocation_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            { }
            catch (Exception ex)
            {}
        }

        private void dgvFilterAllocation_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvFilterAllocation.DataSource != null)//Show only required columns
                {
                    foreach (DataGridViewColumn dgvc in dgvFilterAllocation.Columns)
                    {
                        dgvc.Visible = false;
                        if (dgvc.Name == "USERNAME" || dgvc.Name == "FILTER_NAME")
                        {
                            dgvc.Visible = true;
                            if (dgvc.Name == "USERNAME")
                                dgvc.HeaderText = "User Name";
                            else
                                dgvc.HeaderText = "Filter Name";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmbFilters_DropDownClosed(object sender, EventArgs e) //Runtime event
        {
            try
            {
                if (cmbFilters.SelectedItem == null || iCurrentRowIndex == -1)
                {
                    cmbFilters.Visible = false;//Hide combobox on dropdown close
                    return;
                }

                if (cmbFilters.SelectedItem.ToString().Trim().Length > 0)//Update selected filter to DB
                {
                    DataRow[] drAllocationFilter = dtAllocationFilter.Select("FILTER_NAME= '" + cmbFilters.SelectedItem.ToString() + "'");
                    if (drAllocationFilter.Length > 0)
                    {
                        dgvFilterAllocation.Rows[iCurrentRowIndex].Cells["FILTER_NAME"].Value = cmbFilters.SelectedItem.ToString();//Coulmn used only for user view(Not used anywhere else)
                        dgvFilterAllocation.Rows[iCurrentRowIndex].Cells["FILTER_ID"].Value = Convert.ToInt32(drAllocationFilter[0]["FILTER_ID"].ToString()); //Change filterID accordingly
                        dgvFilterAllocation.Rows[iCurrentRowIndex].Cells["UPDATED_DATE"].Value = GM.GetDateTime();
                        dgvFilterAllocation.Rows[iCurrentRowIndex].Cells["UPDATED_BY"].Value = GV.sEmployeeName;
                        DGV_Insert_Update_Delete(); //Updates the DB when filter changed
                        cmbFilters.Visible = false;
                    }
                }
                else//If empty selected then update it as NULL
                {
                    dgvFilterAllocation.Rows[iCurrentRowIndex].Cells["FILTER_ID"].Value = DBNull.Value;
                    DGV_Insert_Update_Delete(); //Updates the DB when filter changed
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void DGV_Insert_Update_Delete()//Save to DB
        {
            try
            {
                this.BindingContext[dtFilterAssignment].EndCurrentEdit();
                GM.Logging(dtFilterAssignment, dtFilterAssignment_Log, "C_FILTER_ASSIGNMENT", "ID");
                bool IsDBAffected = false;
                if (dtFilterAssignment.GetChanges(DataRowState.Added) != null)
                {
                    GV.MSSQL1.BAL_SaveToTable(dtFilterAssignment.GetChanges(DataRowState.Added), "C_FILTER_ASSIGNMENT", "New", true);
                    IsDBAffected = true;
                }
                if (dtFilterAssignment.GetChanges(DataRowState.Modified) != null)
                {
                    GV.MSSQL1.BAL_SaveToTable(dtFilterAssignment.GetChanges(DataRowState.Modified), "C_FILTER_ASSIGNMENT", "Update", true);
                    IsDBAffected = true;
                }
                if (dtFilterAssignment.GetChanges(DataRowState.Deleted) != null)
                {
                    GV.MSSQL1.BAL_SaveToTable(dtFilterAssignment.GetChanges(DataRowState.Deleted), "C_FILTER_ASSIGNMENT", "Delete", true);
                    IsDBAffected = true;
                }
                if (IsDBAffected)                
                    LoadTables();//Reload after updations are done
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvFilterAllocation_CellClick(object sender, DataGridViewCellEventArgs e)//Show dropdown when cell clicked
        {
            try
            {
                if (dgvFilterAllocation.Columns[e.ColumnIndex].Name == "FILTER_NAME" && e.RowIndex != -1)//Runtime Combobox when Filter column clicked
                {
                    Rectangle RectClickPosition = dgvFilterAllocation.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                    cmbFilters.Parent = dgvFilterAllocation.Parent;
                    cmbFilters.Left = RectClickPosition.Left + dgvFilterAllocation.Left;
                    cmbFilters.Top = RectClickPosition.Top + dgvFilterAllocation.Top;
                    cmbFilters.Width = dgvFilterAllocation.Columns[e.ColumnIndex].Width;
                    cmbFilters.Height = RectClickPosition.Height + dgvFilterAllocation.Height;
                    cmbFilters.Visible = true;
                    cmbFilters.DroppedDown = true;
                    iCurrentRowIndex = e.RowIndex; //Set store Row index to use on dropdown close event
                    //e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void mnushowFilters_Click(object sender, EventArgs e)//Filter Allocation
        {
            frmFilterAllocation objFilterAllocation = new frmFilterAllocation();
            objFilterAllocation.ShowDialog(this);
            LoadTables();
        }

        private void mnuaddNewAgents_Click(object sender, EventArgs e)//Add new agents from Timesheet..Users
        {
            try
            {
                frmComboList objFrmComboList = new frmComboList();
                string[] sNewAgentList;
                //objFrmComboList.ShowInTaskbar = false;
                objFrmComboList.TitleText = "Select Agent Name";
                objFrmComboList.dtItems = dtAllUsers;
                objFrmComboList.lstColumnsToDisplay.Add("UserName");
                objFrmComboList.sColumnToSearch = "UserName";
                objFrmComboList.IsSpellCheckEnabeld = false;
                objFrmComboList.IsMultiSelect = true;
                objFrmComboList.IsSingleWordSelection = false; //Single word selection overrides Multiselect property(Which only Single word can be selected from list)
                objFrmComboList.ShowDialog(this);
                sNewAgentList = objFrmComboList.sReturn.Split('|');
                if (sNewAgentList.Length > 0)
                {
                    foreach (string sName in sNewAgentList)
                    {
                        if (sName.Trim().Length > 0)
                        {
                            DataRow[] drAgentExistenceCheck = dtFilterAssignment.Select("USERNAME = '" + sName + "'");//Check if the agent already exist in current FilterAssignment Table
                            if (drAgentExistenceCheck.Length == 0)
                            {
                                DataRow dr = dtFilterAssignment.NewRow();
                                dr["PROJECT_ID"] = GV.sProjectID;
                                dr["USERNAME"] = sName;
                                dr["USERACCESS"] = GV.sAccessTo;
                                dr["CREATED_DATE"] = GM.GetDateTime();
                                dr["CREATED_BY"] = GV.sEmployeeName;
                                dtFilterAssignment.Rows.Add(dr);
                            }
                        }
                    }
                    DGV_Insert_Update_Delete();//Add new agents to DB
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void mnuDeleteSelectedAllocation_Click(object sender, EventArgs e)
        {
            Dgv_DeleteSelectedRows();
        }

        private void Dgv_DeleteSelectedRows()//Delete Selected Rows
        {
            try
            {
                if (DialogResult.Yes == MessageBoxEx.Show("Do you want delete this Allocation ?", "Alert", MessageBoxButtons.YesNo) && dgvFilterAllocation.CurrentRow != null && dgvFilterAllocation.CurrentRow.Index != -1)
                {
                    // dgvFilterAllocation.Rows.RemoveAt(dgvFilterAllocation.CurrentRow.Index);      
                    foreach (DataGridViewRow Dgvr in dgvFilterAllocation.SelectedRows)
                    {
                        dgvFilterAllocation.Rows.Remove(Dgvr);
                    }
                    DGV_Insert_Update_Delete(); //reflect Changes to DB
                    if (dgvFilterAllocation.Rows.Count > 0)
                    {
                        dgvFilterAllocation.CurrentCell = dgvFilterAllocation.Rows[0].Cells[3]; 
                        dgvFilterAllocation.Rows[0].Selected = true;
                        dgvFilterAllocation_CellClick(dgvFilterAllocation, new DataGridViewCellEventArgs(3, 0));
                    }
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
