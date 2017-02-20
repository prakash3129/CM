namespace GCC
{
    partial class frmRDPMonitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn1 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn2 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn3 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.lblOnline = new DevComponents.DotNetBar.LabelItem();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.btnRefresh = new DevComponents.DotNetBar.ButtonItem();
            this.btnSearch = new DevComponents.DotNetBar.ButtonItem();
            this.txtSearch = new DevComponents.DotNetBar.TextBoxItem();
            this.lblLastUpdated = new DevComponents.DotNetBar.LabelItem();
            this.sdgvMonitor = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.timerLastUpdate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.SuspendLayout();
            // 
            // bar1
            // 
            this.bar1.AntiAlias = true;
            this.bar1.BarType = DevComponents.DotNetBar.eBarType.StatusBar;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bar1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.lblOnline,
            this.labelItem1,
            this.btnRefresh,
            this.btnSearch,
            this.txtSearch,
            this.lblLastUpdated});
            this.bar1.Location = new System.Drawing.Point(0, 775);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(1297, 26);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar1.TabIndex = 1;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // lblOnline
            // 
            this.lblOnline.BorderType = DevComponents.DotNetBar.eBorderType.Sunken;
            this.lblOnline.Image = global::GCC.Properties.Resources.Green2;
            this.lblOnline.Name = "lblOnline";
            this.lblOnline.Text = " Online (32)";
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Stretch = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::GCC.Properties.Resources.reload_icon__1_;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Image = global::GCC.Properties.Resources.search_icon__1_;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.TextBoxWidth = 120;
            this.txtSearch.Visible = false;
            this.txtSearch.WatermarkBehavior = DevComponents.DotNetBar.eWatermarkBehavior.HideNonEmpty;
            this.txtSearch.WatermarkColor = System.Drawing.SystemColors.GrayText;
            this.txtSearch.WatermarkText = "Search";
            this.txtSearch.LostFocus += new System.EventHandler(this.txtSearch_LostFocus);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblLastUpdated
            // 
            this.lblLastUpdated.BorderType = DevComponents.DotNetBar.eBorderType.Sunken;
            this.lblLastUpdated.Name = "lblLastUpdated";
            this.lblLastUpdated.Text = "Last Updated ";
            // 
            // sdgvMonitor
            // 
            this.sdgvMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvMonitor.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvMonitor.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvMonitor.Location = new System.Drawing.Point(0, 0);
            this.sdgvMonitor.Name = "sdgvMonitor";
            this.sdgvMonitor.PrimaryGrid.AllowEdit = false;
            this.sdgvMonitor.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvMonitor.PrimaryGrid.ColumnDragBehavior = DevComponents.DotNetBar.SuperGrid.ColumnDragBehavior.None;
            this.sdgvMonitor.PrimaryGrid.ColumnHeaderClickBehavior = DevComponents.DotNetBar.SuperGrid.ColumnHeaderClickBehavior.None;
            gridColumn1.AllowEdit = false;
            gridColumn1.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            gridColumn1.ColumnSortMode = DevComponents.DotNetBar.SuperGrid.ColumnSortMode.None;
            gridColumn1.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridImageEditControl);
            gridColumn1.FillWeight = 20;
            gridColumn1.HeaderText = " ";
            gridColumn1.Name = "ColumnPic";
            gridColumn1.ReadOnly = true;
            gridColumn1.RenderType = typeof(DevComponents.DotNetBar.SuperGrid.GridImageEditControl);
            gridColumn1.SortIndicator = DevComponents.DotNetBar.SuperGrid.SortIndicator.None;
            gridColumn1.Width = 30;
            gridColumn2.DataPropertyName = "MachineID";
            gridColumn2.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn2.Name = "MachineID";
            gridColumn2.Visible = false;
            gridColumn3.DataPropertyName = "LastUpdatedDate";
            gridColumn3.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn3.Name = "LastUpdatedDate";
            gridColumn3.Visible = false;
            this.sdgvMonitor.PrimaryGrid.Columns.Add(gridColumn1);
            this.sdgvMonitor.PrimaryGrid.Columns.Add(gridColumn2);
            this.sdgvMonitor.PrimaryGrid.Columns.Add(gridColumn3);
            this.sdgvMonitor.PrimaryGrid.DefaultVisualStyles.CellStyles.Default.Alignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.MiddleCenter;
            this.sdgvMonitor.PrimaryGrid.GridLines = DevComponents.DotNetBar.SuperGrid.GridLines.Horizontal;
            this.sdgvMonitor.PrimaryGrid.MultiSelect = false;
            this.sdgvMonitor.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.sdgvMonitor.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvMonitor.Size = new System.Drawing.Size(1297, 775);
            this.sdgvMonitor.TabIndex = 2;
            this.sdgvMonitor.RowDoubleClick += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowDoubleClickEventArgs>(this.sdgvMonitor_RowDoubleClick);
            // 
            // timerLastUpdate
            // 
            this.timerLastUpdate.Enabled = true;
            this.timerLastUpdate.Interval = 30000;
            this.timerLastUpdate.Tick += new System.EventHandler(this.timerLastUpdate_Tick);
            // 
            // frmRDPMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1297, 801);
            this.Controls.Add(this.sdgvMonitor);
            this.Controls.Add(this.bar1);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.Name = "frmRDPMonitor";
            this.ShowInTaskbar = false;
            this.Text = "Monitor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRDPMonitor_FormClosing);
            this.Load += new System.EventHandler(this.frmRDPMonitor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.LabelItem lblOnline;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.LabelItem lblLastUpdated;
        private DevComponents.DotNetBar.ButtonItem btnRefresh;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvMonitor;
        private System.Windows.Forms.Timer timerLastUpdate;
        private DevComponents.DotNetBar.ButtonItem btnSearch;
        private DevComponents.DotNetBar.TextBoxItem txtSearch;
    }
}