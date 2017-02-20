using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmValidation_Step1 : Office2007Form
    {
        public frmValidation_Step1()
        {
            InitializeComponent();

            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
        }

        DataTable dtValidation;

        private void Validation_Table_Load(object sender, EventArgs e)
        {
            dtValidation = GV.MYSQL.BAL_FetchTableMySQL(GV.sProjectID + "_validations", "VALIDATION_NAME IS NOT NULL");
            LoadTiles(dtValidation, "TR");
            LoadTiles(dtValidation, "WR");

            DevComponents.DotNetBar.Metro.MetroTileItem mtTR = new DevComponents.DotNetBar.Metro.MetroTileItem();
            DevComponents.DotNetBar.Metro.MetroTileItem mtWR = new DevComponents.DotNetBar.Metro.MetroTileItem();
            mtTR.Text = mtWR.Text = "Add New";
            itemContainerTRValidations.SubItems.Add(mtTR);
            itemContainerWRValidations.SubItems.Add(mtWR);
        }

        void LoadTiles(DataTable dtValidation, string sResearch_Type)
        {
            
            DataRow[] drrValidation = dtValidation.Select("Research_Type ='" + sResearch_Type + "'");
            if (drrValidation.Length > 0)
            {
                DataTable dtDistinct = drrValidation.CopyToDataTable().DefaultView.ToTable(true, "VALIDATION_NAME");
                //distinctTable = dt.DefaultView.ToTable(true, "FILENAME");

                if (sResearch_Type == "TR")
                {
                    foreach (DataRow drValidation in dtDistinct.Rows)
                    {
                        DevComponents.DotNetBar.Metro.MetroTileItem mt = new DevComponents.DotNetBar.Metro.MetroTileItem();
                        mt.Text = drValidation["VALIDATION_NAME"].ToString();
                        itemContainerTRValidations.SubItems.Add(mt);
                    }
                }
                else
                {
                    foreach (DataRow drValidation in dtDistinct.Rows)
                    {
                        DevComponents.DotNetBar.Metro.MetroTileItem mt = new DevComponents.DotNetBar.Metro.MetroTileItem();
                        mt.Text = drValidation["VALIDATION_NAME"].ToString();
                        itemContainerWRValidations.SubItems.Add(mt);
                    }
                }
            }            
        }
    }
}
