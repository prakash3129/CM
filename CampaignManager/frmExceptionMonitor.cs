using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;

namespace GCC
{
    public partial class frmExceptionMonitor : DevComponents.DotNetBar.Office2007Form
    {
        public frmExceptionMonitor()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new System.Drawing.Font(this.Font.FontFamily, 22);
            MessageBoxEx.EnableGlass = false;   
        }

        DataTable dtException = null;
        DataTable dtException_Log = null;

        void Reload()
        {
            dtException = GV.MSSQL1.BAL_ExecuteQuery("Select * from c_Exceptions");
            dtException_Log = GV.MSSQL1.BAL_ExecuteQuery("select A.*,B.PROJECT_NAME from C_EXCEPTIONS_LOG A left join c_project_settings B on A.projectID = B.PROJECT_ID;");
            superGridErrorMain.PrimaryGrid.DataSource = dtException;
        }

        private void superGridErrorMain_RowActivated(object sender, DevComponents.DotNetBar.SuperGrid.GridRowActivatedEventArgs e)
        {
            GridCell grdCell = superGridErrorMain.GetCell(e.NewActiveRow.RowIndex, 1);

            DataRow[] drrException_Log = dtException_Log.Select("ExceptionID = '" + grdCell.Value + "'");
            if (drrException_Log.Length > 0)
                superGridErrorSub.PrimaryGrid.DataSource = drrException_Log.CopyToDataTable();
        }

        private void ExceptionMonitor_Load(object sender, EventArgs e)
        {
            Reload();

        }

        private void superGridErrorMain_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach(GridColumn GC in superGridErrorMain.PrimaryGrid.Columns)
            {
                if (GC.DataPropertyName.ToUpper() != "MESSAGE")
                    GC.Visible = false;
            }
        }
    }
}
