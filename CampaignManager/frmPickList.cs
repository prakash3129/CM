using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmPickList : DevComponents.DotNetBar.Office2007Form
    {
        public frmPickList()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); //Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
        }

        //BAL.BAL_GlobalMySdfQL objBAL_GlobalMySfdQL = new BAL.BAL_GlobalMyfdSQL();
        DataTable dtPickList = new DataTable();
        bool IsLoading = false;

        private void frmPickList_Load(object sender, EventArgs e)
        {
            InitialLoad();
        }

        private void InitialLoad()
        {
            IsLoading = true;
            dtPickList = GV.MSSQL1.BAL_FetchTable(GV.sProjectID + "_picklists", "1=1");
            dgvPickListValues.DataSource = dtPickList;
            dgvPicklistCategory.DataSource = dtPickList.DefaultView.ToTable(true, "PicklistCategory");
            dgvPicklistCategory.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
            dgvPickListValues.BackgroundColor = GV.pnlGlobalColor.Style.BackColor2.Color;
            IsLoading = false;
        }

        private void dgvPicklistCategory_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            foreach(DataGridViewRow dgvr in dgvPickListValues.Rows)
            {
                CurrencyManager CM = (CurrencyManager)BindingContext[dgvPickListValues.DataSource];
                CM.SuspendBinding();
                if (dgvr.Cells["PicklistCategory"].Value != null)
                {
                    if (dgvr.Cells["PicklistCategory"].Value.ToString() == dgvPicklistCategory.Rows[e.RowIndex].Cells["PicklistCategory"].Value.ToString())
                        dgvr.Visible = true;
                    else
                        dgvr.Visible = false;
                }
                CM.ResumeBinding();
            }
        }

        private void dgvPickListValues_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn dgvc in dgvPickListValues.Columns)
            {
                if (dgvc.Name == "ID")// || dgvc.Name == "PicklistCategory")
                    dgvc.Visible = false;
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            List<string> lstPickListCategory = new List<string>();
            foreach (DataGridViewRow dgvr in dgvPicklistCategory.Rows)
                lstPickListCategory.Add(dgvr.Cells["PicklistCategory"].Value.ToString().ToUpper());            

            frmNewPickListCategory objfrmNewPickListCategory = new frmNewPickListCategory();
            objfrmNewPickListCategory.lstCategory = lstPickListCategory;
            if (objfrmNewPickListCategory.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                DataRow drNewRow = dtPickList.NewRow();
                drNewRow["PicklistCategory"] = objfrmNewPickListCategory.txtCategoryName.Text.Trim();
                drNewRow["PicklistField"] = string.Empty;
                drNewRow["PicklistValue"] = string.Empty;
                drNewRow["remarks"] = string.Empty;
                dtPickList.Rows.Add(drNewRow);
                Save();
                InitialLoad();
            }
        }

        private void Save()
        {
            if (dtPickList.GetChanges(DataRowState.Added) != null)
                GV.MSSQL1.BAL_SaveToTable(dtPickList.GetChanges(DataRowState.Added), GV.sProjectID + "_picklists", "New",  true);
            if (dtPickList.GetChanges(DataRowState.Modified) != null)
                GV.MSSQL1.BAL_SaveToTable(dtPickList.GetChanges(DataRowState.Modified), GV.sProjectID + "_picklists", "Update", true);
            if (dtPickList.GetChanges(DataRowState.Deleted) != null)
                GV.MSSQL1.BAL_SaveToTable(dtPickList.GetChanges(DataRowState.Deleted), GV.sProjectID + "_picklists", "Delete", true);
        }

        private void dgvPickListValues_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (!IsLoading)
            {
                ToastNotification.Show(this, "new");
                dgvPickListValues.Rows[e.RowIndex -1].Cells["PicklistCategory"].Value = dgvPicklistCategory.CurrentRow.Cells["PicklistCategory"].Value.ToString();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Save();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
