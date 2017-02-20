using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;

namespace GCC
{
    public partial class frmList_With_Header : Office2007Form
    {
        public frmList_With_Header()
        {
            InitializeComponent();

            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
        }

        private bool _IsMultiSelect;
        public bool IsMultiSelect /////Is the list is multi selectable/////
        {
            get { return _IsMultiSelect; }
            set { _IsMultiSelect = value; }
        }

        private DataTable _dtItems;
        public DataTable dtItems /////Items in which the search is to be performed/////
        {
            get { return _dtItems; }
            set { _dtItems = value; }
        }

        string _sTitle = string.Empty;
        public string sTitle /////Title to be displayed/////
        {
            get { return _sTitle; }
            set { _sTitle = value; }
        }

        string _sHeaderColumn = string.Empty;
        public string sHeaderColumn /////Value to be searched/////
        {
            get { return _sHeaderColumn; }
            set { _sHeaderColumn = value; }
        }

        string _sValueColumn = string.Empty;
        public string sValueColumn /////Value to be searched/////
        {
            get { return _sValueColumn; }
            set { _sValueColumn = value; }
        }


        string _sReturnTable = string.Empty;
        public string sReturnTable /////Table to be searched/////
        {
            get { return _sReturnTable; }
            set { _sReturnTable = value; }
        }

        string _sReturnValue = string.Empty;
        public string sReturnValue /////Value to be Returned/////
        {
            get { return _sReturnValue; }
            set { _sReturnValue = value; }
        }


        private void frmList_With_Header_Load(object sender, EventArgs e)
        {
            //this.TitleText = sTitle;
            
            sdgvSearch.PrimaryGrid.EnterKeySelectsNextRow = false;                 
            GridPanel GP = sdgvSearch.PrimaryGrid;
            GP.RowDoubleClickBehavior = DevComponents.DotNetBar.SuperGrid.RowDoubleClickBehavior.ExpandCollapse;
            GP.ShowTreeButtons = true;
            dtItems = Sort(dtItems, sHeaderColumn, "ASC");

            GridColumn GCValues = new GridColumn();
            GridColumn GCHeader = new GridColumn();
            GCValues.EditorType = typeof(GridLabelXEditControl);
            GCHeader.EditorType = typeof(GridLabelXEditControl);
            GCValues.Name = "Values";
            GCHeader.Name = "Header";
            GCHeader.Visible = false;
            GP.Columns.Add(GCValues);
            GP.Columns.Add(GCHeader);           
            Search(dtItems);            
        }

        void Search(DataTable dtData)
        {
            string sHeader = string.Empty;
            sdgvSearch.PrimaryGrid.Rows.Clear();
            GridRow GR = null;
            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                if (sHeader == dtData.Rows[i][sHeaderColumn].ToString().ToUpper())
                {
                    GridRow GRSubRow = new GridRow(dtData.Rows[i][sValueColumn].ToString().ToUpper(), dtData.Rows[i][sHeaderColumn].ToString().ToUpper());
                    GR.Rows.Add(GRSubRow);
                }
                else
                {
                    sHeader = dtData.Rows[i][sHeaderColumn].ToString().ToUpper();
                    GR = new GridRow("<b>" + sHeader + "</b>");
                    GridRow GRSubRow = new GridRow(dtData.Rows[i][sValueColumn].ToString().ToUpper(), sHeader);
                    GR.Rows.Add(GRSubRow);
                    GR.Expanded = true;
                    sdgvSearch.PrimaryGrid.Rows.Add(GR);
                }
            }

            //sdgvSearch.VScrollBar.Refresh();
            //sdgvSearch.PerformLayout();
            //sdgvSearch.ResumeLayout();
            
            //if (sdgvSearch.VScrollBarVisible)
            //{
            //    //if (sdgvContacts.PrimaryGrid.Columns[0].Width != (sdgvContacts.Width - 18))
            //    sdgvSearch.PrimaryGrid.Columns[0].Width = sdgvSearch.Width - 18;
            //}
            //else
            //{
            //    //if (sdgvContacts.PrimaryGrid.Columns[0].Width != sdgvContacts.Width)
            //    sdgvSearch.PrimaryGrid.Columns[0].Width = sdgvSearch.Width;
            //}
        }

        DataTable Sort(DataTable dtSort, string sColName, string sDirection)
        {
            dtSort.DefaultView.Sort = sColName + " " + sDirection;
            dtSort = dtSort.DefaultView.ToTable();
            return dtSort;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (sReturnValue.Length > 0)            
                txtSelectedValue.Text = sReturnTable = sReturnValue = string.Empty;            

            DataRow[] drrItem = dtItems.Select(sValueColumn +" LIKE '%"+txtSearch.Text.Trim() + "%'",sHeaderColumn +" ASC");
            if (drrItem.Length > 0)            
                Search(drrItem.CopyToDataTable());            
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                                          
                //if (sReturn.Length == 0 && dgvList.CurrentCell != null)
                //    sReturn = dgvList.CurrentRow.Cells[sColumnToSearch].Value.ToString().Trim();


                if (sReturnValue.Length > 0)
                {//Eliminate if double space exist
                    sReturnValue = System.Text.RegularExpressions.Regex.Replace(sReturnValue, @"\s+", " ");
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }

                
                //return;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            sReturnValue = string.Empty;
            sReturnTable = string.Empty;            
        }

        private void sdgvSearch_RowActivated(object sender, GridRowActivatedEventArgs e)
        {
            
            GridRow GR = ((GridRow)e.NewActiveRow);                
            if (GR == null ||GR.Cells[0].Value.ToString().StartsWith("<b>"))
            {
                sReturnValue = string.Empty;
                sReturnTable = string.Empty;
                txtSelectedValue.Text = string.Empty;
            }
            else
            {
                sReturnValue = GR.Cells[0].Value.ToString().Trim();
                sReturnTable = GR.Cells[1].Value.ToString().Trim();
                txtSelectedValue.Text = sReturnValue;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            

            /////Key press listener for overall window/////
            if (keyData == Keys.Enter)/////Enter key for Submit/////
                btnSubmit.PerformClick();

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void sdgvSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)/////Enter key for Submit/////
                btnSubmit.PerformClick();

            if (e.KeyData == (Keys.Control | Keys.Up)) /////'Ctrl + Up' to focus Search Textbox/////
            {
                txtSearch.Focus();

                sReturnValue = string.Empty;
                sReturnTable = string.Empty;
                txtSelectedValue.Text = string.Empty;
                sdgvSearch.PrimaryGrid.ClearSelectedRows();
            }
        }

        private void sdgvSearch_RowDoubleClick(object sender, GridRowDoubleClickEventArgs e)
        {
            btnSubmit.PerformClick();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                /////Shortcuts on Search textbox/////
                if (e.KeyCode == Keys.Down && sdgvSearch.PrimaryGrid.Rows.Count > 0) /////Down arrow to focus search list/////
                {
                    sdgvSearch.Focus();
                }

                if (e.KeyCode == Keys.Up && sdgvSearch.PrimaryGrid.Rows.Count > 0) /////Up arrow to focus the last row the search list/////
                {
                    sdgvSearch.Focus();
                    //dgvList.CurrentCell = dgvList.Rows[dgvList.Rows.Count - 1].Cells[lstColumnsToDisplay[0]];

                    sdgvSearch.PrimaryGrid.ClearSelectedRows();
                    GridRow gr = sdgvSearch.PrimaryGrid.Rows[sdgvSearch.PrimaryGrid.Rows.Count - 1] as GridRow;
                    sdgvSearch.PrimaryGrid.SetActiveRow(gr);
                    sdgvSearch.PrimaryGrid.Select(gr);
                    gr.IsSelected = true;
                    gr.SetActive(true);
                    sdgvSearch.Refresh();
                    sdgvSearch.PrimaryGrid.SetSelectedRows(sdgvSearch.PrimaryGrid.Rows.Count - 1, 1, true);
                    sdgvSearch.Refresh();


                    if (sdgvSearch.VScrollBarVisible)
                        sdgvSearch.VScrollOffset = Convert.ToInt32((((sdgvSearch.PrimaryGrid.Rows.Count - 1 / Convert.ToDouble(sdgvSearch.PrimaryGrid.Rows.Count)) * 100) * sdgvSearch.VScrollMaximum) / 100) - 200;

                }               
                
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            sReturnValue = string.Empty;
            sReturnTable = string.Empty;
            txtSelectedValue.Text = string.Empty;
            sdgvSearch.PrimaryGrid.ClearSelectedRows();
        }
    }
}
