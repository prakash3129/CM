namespace GCC
{
    partial class frmEmailHunter
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
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn1 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn2 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn3 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn4 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn5 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn6 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn7 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.panelExManualChecks = new DevComponents.DotNetBar.PanelEx();
            this.lblServerLoad = new DevComponents.DotNetBar.LabelX();
            this.lstServerList = new System.Windows.Forms.ListBox();
            this.txtFileImport = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.circularProgressExport = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.lblExportDisplayPercent = new DevComponents.DotNetBar.LabelX();
            this.btnReload = new DevComponents.DotNetBar.ButtonX();
            this.dTimeMonthRange = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.cmbServerID = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnImport = new DevComponents.DotNetBar.ButtonX();
            this.txtBatchName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.sdgvManualChecks = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.openFileEmails = new System.Windows.Forms.OpenFileDialog();
            this.bWorkerExport = new System.ComponentModel.BackgroundWorker();
            this.saveFileDialogExportToExcel = new System.Windows.Forms.SaveFileDialog();
            this.panelExManualChecks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dTimeMonthRange)).BeginInit();
            this.SuspendLayout();
            // 
            // panelExManualChecks
            // 
            this.panelExManualChecks.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelExManualChecks.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelExManualChecks.Controls.Add(this.lblServerLoad);
            this.panelExManualChecks.Controls.Add(this.lstServerList);
            this.panelExManualChecks.Controls.Add(this.txtFileImport);
            this.panelExManualChecks.Controls.Add(this.labelX1);
            this.panelExManualChecks.Controls.Add(this.circularProgressExport);
            this.panelExManualChecks.Controls.Add(this.lblExportDisplayPercent);
            this.panelExManualChecks.Controls.Add(this.btnReload);
            this.panelExManualChecks.Controls.Add(this.dTimeMonthRange);
            this.panelExManualChecks.Controls.Add(this.cmbServerID);
            this.panelExManualChecks.Controls.Add(this.btnImport);
            this.panelExManualChecks.Controls.Add(this.txtBatchName);
            this.panelExManualChecks.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelExManualChecks.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExManualChecks.Location = new System.Drawing.Point(0, 0);
            this.panelExManualChecks.Name = "panelExManualChecks";
            this.panelExManualChecks.Size = new System.Drawing.Size(1019, 81);
            this.panelExManualChecks.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExManualChecks.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelExManualChecks.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExManualChecks.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelExManualChecks.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExManualChecks.Style.GradientAngle = 90;
            this.panelExManualChecks.TabIndex = 0;
            // 
            // lblServerLoad
            // 
            this.lblServerLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblServerLoad.AutoSize = true;
            // 
            // 
            // 
            this.lblServerLoad.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblServerLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblServerLoad.Location = new System.Drawing.Point(804, 2);
            this.lblServerLoad.Name = "lblServerLoad";
            this.lblServerLoad.Size = new System.Drawing.Size(60, 15);
            this.lblServerLoad.TabIndex = 567;
            this.lblServerLoad.Text = "Server Load";
            // 
            // lstServerList
            // 
            this.lstServerList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lstServerList.FormattingEnabled = true;
            this.lstServerList.Location = new System.Drawing.Point(804, 18);
            this.lstServerList.Name = "lstServerList";
            this.lstServerList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstServerList.Size = new System.Drawing.Size(85, 56);
            this.lstServerList.TabIndex = 566;
            this.lstServerList.UseTabStops = false;
            // 
            // txtFileImport
            // 
            // 
            // 
            // 
            this.txtFileImport.Border.Class = "TextBoxBorder";
            this.txtFileImport.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtFileImport.ButtonCustom.Visible = true;
            this.txtFileImport.Location = new System.Drawing.Point(13, 5);
            this.txtFileImport.Name = "txtFileImport";
            this.txtFileImport.PreventEnterBeep = true;
            this.txtFileImport.Size = new System.Drawing.Size(101, 20);
            this.txtFileImport.TabIndex = 0;
            this.txtFileImport.WatermarkBehavior = DevComponents.DotNetBar.eWatermarkBehavior.HideNonEmpty;
            this.txtFileImport.WatermarkText = "Import Emails";
            this.txtFileImport.ButtonCustomClick += new System.EventHandler(this.txtFileImport_ButtonCustomClick);
            // 
            // labelX1
            // 
            this.labelX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.labelX1.Location = new System.Drawing.Point(895, 64);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(115, 13);
            this.labelX1.TabIndex = 565;
            this.labelX1.Text = "Double-click rows to Export";
            // 
            // circularProgressExport
            // 
            // 
            // 
            // 
            this.circularProgressExport.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.circularProgressExport.Location = new System.Drawing.Point(181, 39);
            this.circularProgressExport.Name = "circularProgressExport";
            this.circularProgressExport.ProgressBarType = DevComponents.DotNetBar.eCircularProgressType.Donut;
            this.circularProgressExport.Size = new System.Drawing.Size(38, 23);
            this.circularProgressExport.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.circularProgressExport.TabIndex = 564;
            this.circularProgressExport.Visible = false;
            // 
            // lblExportDisplayPercent
            // 
            this.lblExportDisplayPercent.AutoSize = true;
            // 
            // 
            // 
            this.lblExportDisplayPercent.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblExportDisplayPercent.Location = new System.Drawing.Point(180, 12);
            this.lblExportDisplayPercent.Name = "lblExportDisplayPercent";
            this.lblExportDisplayPercent.Size = new System.Drawing.Size(39, 15);
            this.lblExportDisplayPercent.TabIndex = 563;
            this.lblExportDisplayPercent.Text = "labelX1";
            this.lblExportDisplayPercent.Visible = false;
            // 
            // btnReload
            // 
            this.btnReload.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReload.Location = new System.Drawing.Point(895, 41);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(81, 23);
            this.btnReload.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnReload.TabIndex = 5;
            this.btnReload.Text = "Reload";
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // dTimeMonthRange
            // 
            this.dTimeMonthRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.dTimeMonthRange.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dTimeMonthRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dTimeMonthRange.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dTimeMonthRange.CustomFormat = "MMM yyyy";
            this.dTimeMonthRange.Format = DevComponents.Editors.eDateTimePickerFormat.Custom;
            this.dTimeMonthRange.IsPopupCalendarOpen = false;
            this.dTimeMonthRange.Location = new System.Drawing.Point(895, 18);
            this.dTimeMonthRange.MinDate = new System.DateTime(2011, 1, 1, 0, 0, 0, 0);
            // 
            // 
            // 
            this.dTimeMonthRange.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dTimeMonthRange.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dTimeMonthRange.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this.dTimeMonthRange.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dTimeMonthRange.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dTimeMonthRange.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dTimeMonthRange.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dTimeMonthRange.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dTimeMonthRange.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dTimeMonthRange.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dTimeMonthRange.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dTimeMonthRange.MonthCalendar.DisplayMonth = new System.DateTime(2016, 5, 1, 0, 0, 0, 0);
            this.dTimeMonthRange.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday;
            this.dTimeMonthRange.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dTimeMonthRange.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dTimeMonthRange.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dTimeMonthRange.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dTimeMonthRange.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dTimeMonthRange.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dTimeMonthRange.MonthCalendar.TodayButtonVisible = true;
            this.dTimeMonthRange.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dTimeMonthRange.Name = "dTimeMonthRange";
            this.dTimeMonthRange.ShowUpDown = true;
            this.dTimeMonthRange.Size = new System.Drawing.Size(81, 20);
            this.dTimeMonthRange.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.dTimeMonthRange.TabIndex = 4;
            // 
            // cmbServerID
            // 
            this.cmbServerID.DisplayMember = "Text";
            this.cmbServerID.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbServerID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbServerID.FormattingEnabled = true;
            this.cmbServerID.ItemHeight = 14;
            this.cmbServerID.Location = new System.Drawing.Point(12, 52);
            this.cmbServerID.Name = "cmbServerID";
            this.cmbServerID.Size = new System.Drawing.Size(102, 20);
            this.cmbServerID.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbServerID.TabIndex = 2;
            this.cmbServerID.Visible = false;
            this.cmbServerID.WatermarkBehavior = DevComponents.DotNetBar.eWatermarkBehavior.HideNonEmpty;
            this.cmbServerID.WatermarkText = "Select server";
            // 
            // btnImport
            // 
            this.btnImport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnImport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnImport.Location = new System.Drawing.Point(120, 5);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(54, 67);
            this.btnImport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import\r\nEmails";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // txtBatchName
            // 
            this.txtBatchName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtBatchName.Border.Class = "TextBoxBorder";
            this.txtBatchName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtBatchName.DisabledBackColor = System.Drawing.Color.White;
            this.txtBatchName.ForeColor = System.Drawing.Color.Black;
            this.txtBatchName.Location = new System.Drawing.Point(12, 29);
            this.txtBatchName.Name = "txtBatchName";
            this.txtBatchName.PreventEnterBeep = true;
            this.txtBatchName.Size = new System.Drawing.Size(102, 20);
            this.txtBatchName.TabIndex = 1;
            this.txtBatchName.WatermarkBehavior = DevComponents.DotNetBar.eWatermarkBehavior.HideNonEmpty;
            this.txtBatchName.WatermarkText = "Enter batch name";
            // 
            // sdgvManualChecks
            // 
            this.sdgvManualChecks.BackColor = System.Drawing.Color.White;
            this.sdgvManualChecks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvManualChecks.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvManualChecks.ForeColor = System.Drawing.Color.Black;
            this.sdgvManualChecks.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvManualChecks.Location = new System.Drawing.Point(0, 81);
            this.sdgvManualChecks.Name = "sdgvManualChecks";
            this.sdgvManualChecks.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            gridColumn1.DataPropertyName = "ID";
            gridColumn1.HeaderText = "ID";
            gridColumn1.Name = "ID";
            gridColumn2.DataPropertyName = "BatchName";
            gridColumn2.HeaderText = "Batch";
            gridColumn2.Name = "BatchName";
            gridColumn3.DataPropertyName = "FileName";
            gridColumn3.HeaderText = "File Name";
            gridColumn3.Name = "FileName";
            gridColumn4.DataPropertyName = "LoadedDate";
            gridColumn4.HeaderText = "Loaded Date";
            gridColumn4.Name = "LoadedDate";
            gridColumn5.DataPropertyName = "LoadedBy";
            gridColumn5.HeaderText = "Loaded By";
            gridColumn5.Name = "LoadedBy";
            gridColumn6.DataPropertyName = "Completed";
            gridColumn6.HeaderText = "Completed";
            gridColumn6.Name = "Completed";
            gridColumn7.DataPropertyName = "Total";
            gridColumn7.HeaderText = "Total Loaded";
            gridColumn7.Name = "TotalLoaded";
            this.sdgvManualChecks.PrimaryGrid.Columns.Add(gridColumn1);
            this.sdgvManualChecks.PrimaryGrid.Columns.Add(gridColumn2);
            this.sdgvManualChecks.PrimaryGrid.Columns.Add(gridColumn3);
            this.sdgvManualChecks.PrimaryGrid.Columns.Add(gridColumn4);
            this.sdgvManualChecks.PrimaryGrid.Columns.Add(gridColumn5);
            this.sdgvManualChecks.PrimaryGrid.Columns.Add(gridColumn6);
            this.sdgvManualChecks.PrimaryGrid.Columns.Add(gridColumn7);
            this.sdgvManualChecks.PrimaryGrid.ReadOnly = true;
            this.sdgvManualChecks.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.sdgvManualChecks.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvManualChecks.Size = new System.Drawing.Size(1019, 513);
            this.sdgvManualChecks.TabIndex = 6;
            this.sdgvManualChecks.RowDoubleClick += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowDoubleClickEventArgs>(this.sdgvManualChecks_RowDoubleClick);
            // 
            // openFileEmails
            // 
            this.openFileEmails.Filter = "CSV files (*.csv)|*.csv";
            // 
            // bWorkerExport
            // 
            this.bWorkerExport.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bWorkerExport_DoWork);
            this.bWorkerExport.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bWorkerExport_RunWorkerCompleted);
            // 
            // saveFileDialogExportToExcel
            // 
            this.saveFileDialogExportToExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
            // 
            // frmEmailHunter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 594);
            this.Controls.Add(this.sdgvManualChecks);
            this.Controls.Add(this.panelExManualChecks);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.Name = "frmEmailHunter";
            this.Text = "Email Hunter";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmEmailHunter_Load);
            this.panelExManualChecks.ResumeLayout(false);
            this.panelExManualChecks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dTimeMonthRange)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelExManualChecks;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvManualChecks;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBatchName;
        private DevComponents.DotNetBar.ButtonX btnImport;
        private System.Windows.Forms.OpenFileDialog openFileEmails;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbServerID;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dTimeMonthRange;
        private DevComponents.DotNetBar.ButtonX btnReload;
        private System.ComponentModel.BackgroundWorker bWorkerExport;
        private DevComponents.DotNetBar.LabelX lblExportDisplayPercent;
        private System.Windows.Forms.SaveFileDialog saveFileDialogExportToExcel;
        private DevComponents.DotNetBar.Controls.CircularProgress circularProgressExport;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtFileImport;
        private System.Windows.Forms.ListBox lstServerList;
        private DevComponents.DotNetBar.LabelX lblServerLoad;
    }
}