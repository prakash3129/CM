using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using i00SpellCheck;

namespace GCC
{
    public partial class frmComboList : DevComponents.DotNetBar.Office2007Form
    {
        public frmComboList()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);/////Gets the icon for current window////
        }

        private List<string> _lstColumnsToDisplay = new List<string>();
        private DataTable _dtItems;
        private bool _IsSpellCheckEnabeld;
        private bool _IsMultiSelect;
        private string _sReturn = string.Empty;
        private string _sColumnToSearch = string.Empty;
        private string _sColumnToReturn = string.Empty;
        private bool _IsSingleWordSelection = false;
        private string _sSearchValue = string.Empty;
        private string _sSortColumn = string.Empty;
        private bool _IncludeEmpty = false;

        public List<string> lstColumnsToDisplay /////List of Columns to display on search window/////
        {
            get { return _lstColumnsToDisplay; }
            set { _lstColumnsToDisplay = value; }
        }

        public string sReturn /////Final return value from this window(This will be the output)/////
        {
            get { return _sReturn; }
            set { _sReturn = value; }
        }

        public string sSortColumn /////Sort column
        {
            get { return _sSortColumn; }
            set { _sSortColumn = value; }
        }

        public string sSearchValue /////Value to be searched/////
        {
            get { return _sSearchValue; }
            set { _sSearchValue = value; }
        }

        public bool IncludeEmpty /////Value to be searched/////
        {
            get { return _IncludeEmpty; }
            set { _IncludeEmpty = value; }
        }

        public string sColumnToSearch /////Name of the column to perform search/////
        {
            get { return _sColumnToSearch; }
            set { _sColumnToSearch = value; }
        }

        public string sColumnToReturn /////Name of the column to Return/////
        {
            get { return _sColumnToReturn; }
            set { _sColumnToReturn = value; }
        }

        public bool IsMultiSelect /////Is the list is multi selectable/////
        {
            get { return _IsMultiSelect; }
            set { _IsMultiSelect = value; }
        }

        public bool IsSingleWordSelection /////Only single word can be selected from the list/////
        {
            get { return _IsSingleWordSelection; }
            set { _IsSingleWordSelection = value; }
        }

        public bool IsSpellCheckEnabeld /////Spell check////
        {
            get { return _IsSpellCheckEnabeld; }
            set { _IsSpellCheckEnabeld = value; }
        }

        public DataTable dtItems /////Items in which the search is to be performed/////
        {
            get { return _dtItems; }
            set { _dtItems = value; }
        }

        private void frmComboList_Load(object sender, EventArgs e)
        {
            try
            {
                this.MinimizeBox = false;
                this.MaximizeBox = false; /////To make the window to appear as tool window(Default tool window is eliminated on inheriting DevComponent Form)/////
                //this.ResizeRedraw = false;
                //dgvList.BackgroundColor = Color.FromArgb(255, 207, 221, 238); ////To make the grid to merge with the parant window/////
                //dgvList.BackgroundColor = this.BackColor; ////To make the grid to merge with the parant window/////
                //dgvList.BackgroundColor = Color.FromArgb(PanelEx.DefaultBackColor.A,PanelEx.DefaultBackColor.R,PanelEx.DefaultBackColor.G,PanelEx.DefaultBackColor.B);


                if(GV.pnlGlobalColor.Style.BackColor2.Color.Name != "0")
                    dgvList.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;

                


                if (IsSingleWordSelection) //Single word selection overrides Multiselect property
                {
                    IsMultiSelect = false;
                    txtSelected.ReadOnly = true;
                }

                if (IsSpellCheckEnabeld)
                {
                    txtSelected.EnableSpellCheck();
                    SpellCheckSettings s = new SpellCheckSettings();
                    s.AllowAdditions = false;
                    s.AllowChangeTo = true;
                    s.AllowF7 = false;
                    s.AllowIgnore = false;
                    s.AllowInMenuDefs = true;
                    s.AllowRemovals = false;
                    txtSelected.SpellCheck().Settings = s;

                    //NHunspellTextBoxExtender1.SetSpellCheckEnabled(txtSelected, true);
                }
                //splitCustomComboChild.Panel1Collapsed = true;

                if (IsMultiSelect)/////List Muli-Select Option/////
                {
                    dgvList.Anchor = (AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom);
                    dgvList.MultiSelect = true;
                    txtSelected.Visible = false; /////Single Select textbox hidden/////
                    lstSelected.Visible = true; /////Multi select listbox visible/////
                    //splitCustomComboChild.SplitterDistance = 100;
                    //lstSelected.Height = 70;
                    //splitCustomComboChild.Panel1Collapsed = false; // Show listbox
                    //dgvList.Location = new Point(dgvList.Location.X, dgvList.Location.Y + lstSelected.Height - 20); /////Relocate Grid based on Listbox Size/////
                    lstColumnsToDisplay.Add("Select"); ////To Display Checkbox Column/////

                    DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn(); /////CheckBox Column/////
                    chkColumn.Name = "Select"; //////Name of the Checkbox Column "SELECT"/////
                    chkColumn.HeaderText = "Select";
                    chkColumn.Width = 50;
                    chkColumn.ReadOnly = true;
                    chkColumn.FillWeight = 10;
                    dgvList.Columns.Add(chkColumn);
                }
                else/////List Single Select Option/////
                {
                    lstSelected.Visible = false; /////hide Multi-select listbox/////
                    mnuSelect.Dispose(); ////Eliminate "Check Selected" context menu on single select window/////
                    dgvList.ContextMenu = null;
                    dgvList.MultiSelect = false;
                    txtSelected.ReadOnly = true;
                    splitCustomComboChild.SplitterDistance = txtSearch.Height + txtSelected.Height;
                }

                if (IsSingleWordSelection == false && IsMultiSelect == false)
                    txtSelected.ReadOnly = false;

                //if (IncludeEmpty)
                //{
                //    if (dtItems == null)
                //    {
                //        DataTable dt = new DataTable();
                //        dt.Columns.Add(sColumnToSearch);
                //        DataRow drNewData = dt.NewRow();
                //        drNewData[0] = "(Empty)";
                //        dtItems.Rows.Add(drNewData);
                //    }                    
                //}
                if (dtItems != null)
                {
                    DataRow[] drrItems = dtItems.Select("LEN(TRIM(" + sColumnToSearch + ")) > 0");
                    if (drrItems.Length > 0)
                        dtItems = drrItems.CopyToDataTable();
                }

                dtItems = Sort(AddEmpty(dtItems));                
                if (dtItems != null && dtItems.Rows.Count > 0)
                {
                    dgvList.DataSource = dtItems;
                    //dgvList.DataSource = dtItems.DefaultView.ToTable(true, sColumnToSearch);
                    
                    //foreach (DataRow dr in dtItems.Rows)
                    //{ 

                    //}
                }
                else
                {
                    ToastNotification.Show(this.Owner, "List not available", eToastPosition.TopRight);
                    this.Close();
                }


                //if (sSearchValue.Trim().Length > 0)//Value to be searched
                //    txtSearch.Text = sSearchValue.Trim();

                if (dgvList.Rows.Count == 1)
                {
                    dgvList.Rows[0].Selected = true;
                    dgvList.CurrentCell = dgvList.Rows[0].Cells[sColumnToSearch];
                    //btnSubmit.PerformClick();
                }
                txtSearch.Focus();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        DataTable Sort(DataTable dtTable)
        {
            if (dtTable != null && sSortColumn.Length > 0 && dtTable.Columns.Contains(sSortColumn))
            {
                //dtTable.DefaultView.Sort = sSortColumn;
                //dtTable = dtTable.DefaultView.ToTable();
                //return dtTable;
                return dtTable.AsEnumerable().OrderBy(c => c.Field<int?>(sSortColumn) == null).ThenBy(c => c.Field<int?>(sSortColumn)).AsDataView().ToTable(); // this will ignore null on sort
            }
            else
                return dtTable;
        }

        DataTable AddEmpty(DataTable dtTable)
        {
            if (IncludeEmpty)
            {
                if (dtTable == null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(sColumnToSearch);
                    DataRow drNewData = dt.NewRow();
                    drNewData[0] = "(Empty)";
                    dt.Rows.Add(drNewData);
                    return dt;
                }
                else
                {
                    DataTable dt = dtTable.Copy();
                    DataRow drNewData = dt.NewRow();
                    drNewData[sColumnToSearch] = "(Empty)";
                    dt.Rows.Add(drNewData);
                    return dt;
                }
            }

            return dtTable;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtSearch.Text = txtSearch.Text.Replace("[", string.Empty).Replace("]", string.Empty).Replace("%",string.Empty);//do not allow [].. Will throw error on table.select
                txtSearch.SelectionStart = txtSearch.Text.Length + 1;
                /////Key press Listener for Search textbox/////
                /////Searches only the last word of the textbox/////
                if (txtSearch.Text.Length > 0)
                {
                    DataTable dtTemp = dtItems.Copy(); //////Datatable copy is made to retain the data on search(when search text is deleted then entire list has to be brought back)/////
                    
                    string sSearch = string.Empty;
                    if (IsSingleWordSelection)
                        sSearch = txtSearch.Text.Trim();
                    else
                    {
                        string[] sSplit = txtSearch.Text.Split(' ');/////Splits the text with space to search the last word/////
                        sSearch = sSplit[sSplit.Length - 1].Trim();
                    }

                    DataRow[] drTemp = dtTemp.Select(String.Format("{0} LIKE '%{1}%'", sColumnToSearch, sSearch.Replace("'","''").Replace("(*)",""))); /////Search Contains(Like %text%)/////
                    if (drTemp != null && drTemp.Length > 0)
                    {
                        dgvList.DataSource = Sort(AddEmpty(drTemp.CopyToDataTable())); /////Show the search result//////
                    }
                    else
                        dgvList.DataSource = AddEmpty(null); /////Null if the search returns nothing/////
                }
                else
                    dgvList.DataSource = Sort(AddEmpty(dtItems));
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void dgvList_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewColumn dgvc in dgvList.Columns)/////Hide list of unwanted columns/////
                {
                    dgvc.Visible = false;
                    foreach (string sColumns in lstColumnsToDisplay) /////list of columns to be visible/////
                    {
                        if (dgvc.Name.ToUpper() == sColumns.ToUpper())
                            dgvc.Visible = true;
                    }
                }
                dgvList.ClearSelection();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                /////Shortcuts on Search textbox/////
                if (e.KeyCode == Keys.Down && dgvList.Rows.Count > 0) /////Down arrow to focus search list/////
                {
                    dgvList.Focus();
                }

                if (e.KeyCode == Keys.Up && dgvList.Rows.Count > 0) /////Up arrow to focus the last row the search list/////
                {
                    dgvList.Focus();
                    dgvList.CurrentCell = dgvList.Rows[dgvList.Rows.Count - 1].Cells[lstColumnsToDisplay[0]];
                }

                if (e.KeyCode == Keys.Back && e.Modifiers == Keys.Shift) /////'Shift + BackSpace' to Delete entire search text/////
                {
                    txtSearch.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

       protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
       {
            /////Key press listener for overall window/////
            if (keyData == Keys.Enter)/////Enter key for Submit/////
                btnSubmit.PerformClick();            

            if (keyData == (Keys.Control | Keys.Up)) /////'Ctrl + Up' to focus Search Textbox/////
                txtSearch.Focus();

            if (keyData == (Keys.Control | Keys.L)) /////'Ctrl + L' to Clear entire selected text/////
                txtSelected.Text = string.Empty;

            if (keyData == (Keys.Control | Keys.W))/////'Ctrl + W' to delete the last word of the selected text/////
            {
                if (txtSelected.Text.Trim().Contains(" "))
                    txtSelected.Text = txtSelected.Text.Substring(0, txtSelected.Text.LastIndexOf(" ")).Trim();
                else
                    txtSelected.Text = string.Empty;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dgvList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                /////Key press listener for Datagrid//////
                if (e.KeyCode == Keys.Up && dgvList.CurrentRow.Index == 0) /////Up arrow (focus search textbox if the grid reaches the first row)//////
                {
                    txtSearch.Focus();
                }

                //if (e.KeyCode == Keys.Down && dgvList.CurrentRow.Index == dgvList.Rows.Count -1)
                //{
                //    dgvList.ClearSelection();
                //    txtSearch.Focus();
                //}

                if (dgvList.Rows.Count > 0 && IsMultiSelect && e.KeyCode == Keys.Space) /////Multiselect - 'Space key' to toggle between the checkbox checks/////
                {
                    //if (dgvList.CurrentRow.Cells["Select"].Value == null)
                    //    dgvList.CurrentRow.Cells["Select"].Value = true;
                    //else if (dgvList.CurrentRow.Cells["Select"].Value.ToString() == "False")
                    //    dgvList.CurrentRow.Cells["Select"].Value = true;
                    //else if (dgvList.CurrentRow.Cells["Select"].Value.ToString() == "True")
                    //    dgvList.CurrentRow.Cells["Select"].Value = false;
                }

                if (!IsMultiSelect && e.KeyCode == Keys.Space && dgvList.Rows.Count > 0) /////Single Select - 'Space key to select the highlighted value'/////
                {
                    if (IsSingleWordSelection)
                    {
                        txtSelected.Text = dgvList.CurrentCell.Value.ToString();
                    }
                    else
                    {
                        if (txtSelected.Text.Length == 0)
                        {
                            txtSelected.Text = dgvList.CurrentCell.Value.ToString();
                            txtSearch.Text = txtSearch.Text + " ";
                            txtSearch.SelectionStart = txtSearch.Text.Length + 1;
                            txtSearch.Focus();
                        }
                        else
                        {
                            txtSelected.Text = String.Format("{0} {1}", txtSelected.Text, dgvList.CurrentCell.Value);
                            txtSearch.Text = txtSearch.Text + " ";
                            txtSearch.SelectionStart = txtSearch.Text.Length + 1;
                            txtSearch.Focus();
                        }
                    }
                }

                if (e.KeyCode == Keys.Enter) /////Override Enter key listener of Grid/////
                {
                    e.Handled = true;
                    btnSubmit.PerformClick();
                }

                if (e.KeyCode == Keys.Up && e.Modifiers == Keys.Control)/////Override Ctrl + Up listener of Grid/////
                {
                    e.Handled = true;
                    txtSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Remove_Unselected_Items(); /////Remove Checked items from List if Selected words are deleted/////
                /////Cell Content Click to toggle between Checkbox checks/////
                if (IsMultiSelect)
                {
                    /////Checkbox Check Toggle/////
                    if (dgvList.Rows[e.RowIndex].Cells["Select"].Value == null)
                        dgvList.Rows[e.RowIndex].Cells["Select"].Value = true;
                    else if (dgvList.Rows[e.RowIndex].Cells["Select"].Value.ToString() == "False")
                        dgvList.Rows[e.RowIndex].Cells["Select"].Value = true;
                    else if (dgvList.Rows[e.RowIndex].Cells["Select"].Value.ToString() == "True")
                        dgvList.Rows[e.RowIndex].Cells["Select"].Value = false;

                    /////Add the list of checked list to "selected Text"/////
                    DataGridViewCheckBoxCell Dgvcbc;
                    txtSelected.Text = string.Empty;
                    //lstSelected.Items.Clear();
                    foreach (DataGridViewRow row in dgvList.Rows)
                    {
                        Dgvcbc = row.Cells["Select"] as DataGridViewCheckBoxCell;
                        bool IsChecked = (null != Dgvcbc && null != Dgvcbc.Value && true == (bool)Dgvcbc.Value); /////Gets the Checkbox value/////
                        if (IsChecked)
                        {
                            /////Add Checked Words to Selected List/////
                            if (!lstSelected.Items.Contains(row.Cells[sColumnToSearch].Value.ToString()))
                                lstSelected.Items.Add(row.Cells[sColumnToSearch].Value.ToString());
                        }
                        else
                        {
                            /////Remove from list box if unchecked/////
                            if (lstSelected.Items.Contains(row.Cells[sColumnToSearch].Value.ToString()))
                                lstSelected.Items.Remove(row.Cells[sColumnToSearch].Value.ToString());
                        }
                    }
                }
                else if (IsSingleWordSelection)
                {
                    txtSelected.Text = dgvList.Rows[e.RowIndex].Cells[sColumnToSearch].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Remove_Unselected_Items()
        {
            try
            {
                if (IsMultiSelect)
                {
                    /////Remove Checked items from List if Selected words are deleted/////
                    DataGridViewCheckBoxCell Dgvcbc;
                    foreach (DataGridViewRow row in dgvList.Rows)
                    {
                        Dgvcbc = row.Cells["Select"] as DataGridViewCheckBoxCell;
                        if (lstSelected.Items.Contains(row.Cells[sColumnToSearch].Value.ToString()))
                        {
                            Dgvcbc.Value = true;
                        }
                        else
                            Dgvcbc.Value = false;
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e) //************Few tweaks need to be done*******************
        {
            try
            {
                sReturn = string.Empty;
                if (IsMultiSelect && lstSelected.Items.Count > 0)
                {
                    foreach (string sItems in lstSelected.Items) /////Delimit Each selected items with ['/'] /////
                    {
                        if (sReturn.Length == 0)
                            sReturn = sItems.Trim();
                        else
                            sReturn += "|" + sItems.Trim();
                    }
                }
                else if (IsSingleWordSelection)
                {
                    if (dgvList.CurrentCell != null)
                        sReturn = dgvList.CurrentCell.Value.ToString().Trim();
                }
                else /////If single select just returen the selected Text/////
                {
                    sReturn = txtSelected.Text.Trim();
                    if (sReturn.Length == 0 && dgvList.CurrentCell != null)
                        sReturn = dgvList.CurrentRow.Cells[sColumnToSearch].Value.ToString().Trim();
                }

                if (sReturn.Length > 0)//Eliminate if double space exist
                    sReturn = System.Text.RegularExpressions.Regex.Replace(sReturn, @"\s+", " ");

                this.Close();
                return;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            /////Triggers on ESC or Close/////
            txtSelected.Text = string.Empty;
            this.Close();
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                /////Double-Click returns the selected item's text/////
                if (!IsMultiSelect)
                {
                    txtSelected.Text = dgvList.Rows[e.RowIndex].Cells[sColumnToSearch].Value.ToString();
                    btnSubmit.PerformClick();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void checkSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                /////Multi-Select Context Menu/////
                /////Check Selected items/////
                if (IsMultiSelect)
                {
                    foreach (DataGridViewRow Dgvr in dgvList.SelectedRows)
                    {
                        Dgvr.Cells["Select"].Value = true;
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lstSelected_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                /////Delete Selected items from Listbox/////
                if (e.KeyCode == Keys.Delete)
                {
                    if (lstSelected.Items.Count > 0)
                    {
                        foreach (string s in lstSelected.SelectedItems.OfType<string>().ToList())
                            lstSelected.Items.Remove(s);
                        Remove_Unselected_Items(); /////uncheck removed items in Grid/////
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
