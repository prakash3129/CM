namespace GCC
{
    partial class frmEmailChecks
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
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn27 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn28 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn29 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn30 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn31 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn32 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn33 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn34 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn35 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn36 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn37 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn38 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn39 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PieSeriesLabel pieSeriesLabel5 = new DevExpress.XtraCharts.PieSeriesLabel();
            DevExpress.XtraCharts.PiePointOptions piePointOptions3 = new DevExpress.XtraCharts.PiePointOptions();
            DevExpress.XtraCharts.PieSeriesView pieSeriesView5 = new DevExpress.XtraCharts.PieSeriesView();
            DevExpress.XtraCharts.PieSeriesLabel pieSeriesLabel6 = new DevExpress.XtraCharts.PieSeriesLabel();
            DevExpress.XtraCharts.PieSeriesView pieSeriesView6 = new DevExpress.XtraCharts.PieSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle5 = new DevExpress.XtraCharts.ChartTitle();
            DevExpress.XtraCharts.ChartTitle chartTitle6 = new DevExpress.XtraCharts.ChartTitle();
            this.sdgvEmails = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.panelLoad = new DevComponents.DotNetBar.PanelEx();
            this.panelLegends = new System.Windows.Forms.Panel();
            this.chkSoft = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkPassed = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkNotChecked = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkHard = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.lblLegendNotChecked = new DevComponents.DotNetBar.LabelX();
            this.pictureBoxNotChecked = new System.Windows.Forms.PictureBox();
            this.pictureBoxHard = new System.Windows.Forms.PictureBox();
            this.pictureBoxSoft = new System.Windows.Forms.PictureBox();
            this.lblLegendHard = new DevComponents.DotNetBar.LabelX();
            this.pictureBoxPassed = new System.Windows.Forms.PictureBox();
            this.lblLegendPassed = new DevComponents.DotNetBar.LabelX();
            this.lblLegendSoft = new DevComponents.DotNetBar.LabelX();
            this.txtImportPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnLoad = new DevComponents.DotNetBar.ButtonX();
            this.dtProcessDate = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.txtSourceORAgent = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnImport = new DevComponents.DotNetBar.ButtonX();
            this.txtImportSource = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.openFileEmails = new System.Windows.Forms.OpenFileDialog();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.chartBounce = new DevExpress.XtraCharts.ChartControl();
            this.panelLoad.SuspendLayout();
            this.panelLegends.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNotChecked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSoft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPassed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtProcessDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBounce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView6)).BeginInit();
            this.SuspendLayout();
            // 
            // sdgvEmails
            // 
            this.sdgvEmails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvEmails.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvEmails.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvEmails.Location = new System.Drawing.Point(0, 0);
            this.sdgvEmails.Name = "sdgvEmails";
            this.sdgvEmails.PrimaryGrid.AllowEdit = false;
            this.sdgvEmails.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvEmails.PrimaryGrid.ColumnHeaderClickBehavior = DevComponents.DotNetBar.SuperGrid.ColumnHeaderClickBehavior.Select;
            gridColumn27.DataPropertyName = "Status";
            gridColumn27.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridImageEditControl);
            gridColumn27.FillWeight = 30;
            gridColumn27.HeaderText = "Status";
            gridColumn27.MinimumWidth = 20;
            gridColumn27.Name = "colStatus";
            gridColumn27.RenderType = typeof(DevComponents.DotNetBar.SuperGrid.GridImageEditControl);
            gridColumn28.CellStyles.Default.TextColor = System.Drawing.Color.Black;
            gridColumn28.DataPropertyName = "MASTER_ID";
            gridColumn28.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn28.FillWeight = 30;
            gridColumn28.HeaderText = "ComID";
            gridColumn28.MinimumWidth = 10;
            gridColumn28.Name = "colMasterID";
            gridColumn29.CellStyles.Default.TextColor = System.Drawing.Color.Black;
            gridColumn29.DataPropertyName = "CONTACT_ID_P";
            gridColumn29.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn29.FillWeight = 30;
            gridColumn29.HeaderText = "ConID";
            gridColumn29.MinimumWidth = 10;
            gridColumn29.Name = "colContactID";
            gridColumn30.CellStyles.Default.TextColor = System.Drawing.Color.Black;
            gridColumn30.DataPropertyName = "EMAIL";
            gridColumn30.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn30.FillWeight = 130;
            gridColumn30.HeaderText = "Email";
            gridColumn30.MinimumWidth = 100;
            gridColumn30.Name = "colEmail";
            gridColumn31.CellStyles.Default.TextColor = System.Drawing.Color.Black;
            gridColumn31.DataPropertyName = "EMAIL_SOURCE";
            gridColumn31.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn31.HeaderText = "Email Source";
            gridColumn31.Name = "colSource";
            gridColumn32.CellStyles.Default.TextColor = System.Drawing.Color.Black;
            gridColumn32.DataPropertyName = "DESCRIPTION";
            gridColumn32.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn32.FillWeight = 200;
            gridColumn32.HeaderText = "Description";
            gridColumn32.Name = "colDescription";
            gridColumn33.CellStyles.Default.TextColor = System.Drawing.Color.Black;
            gridColumn33.DataPropertyName = "DETAIL";
            gridColumn33.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn33.FillWeight = 200;
            gridColumn33.HeaderText = "Transcription";
            gridColumn33.Name = "colDetails";
            gridColumn34.CellStyles.Default.TextColor = System.Drawing.Color.Black;
            gridColumn34.DataPropertyName = "PROCESSED_SERVER";
            gridColumn34.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn34.HeaderText = "Server ID";
            gridColumn34.Name = "colServer";
            gridColumn35.CellStyles.Default.TextColor = System.Drawing.Color.Black;
            gridColumn35.DataPropertyName = "CREATED_BY";
            gridColumn35.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn35.HeaderText = "Agent";
            gridColumn35.Name = "colCreatedBy";
            gridColumn36.CellStyles.Default.TextColor = System.Drawing.Color.Black;
            gridColumn36.DataPropertyName = "CREATED_DATE";
            gridColumn36.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn36.HeaderText = "Date";
            gridColumn36.Name = "colCreatedDate";
            gridColumn37.CellStyles.Default.TextColor = System.Drawing.Color.Black;
            gridColumn37.DataPropertyName = "First_Name";
            gridColumn37.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn37.HeaderText = "First Name";
            gridColumn37.Name = "colFirstName";
            gridColumn38.CellStyles.Default.TextColor = System.Drawing.Color.Black;
            gridColumn38.DataPropertyName = "LAST_NAME";
            gridColumn38.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn38.HeaderText = "Last Name";
            gridColumn38.Name = "colLastName";
            gridColumn39.DataPropertyName = "Bounce";
            gridColumn39.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn39.HeaderText = "Bounce";
            gridColumn39.Name = "colBounce";
            this.sdgvEmails.PrimaryGrid.Columns.Add(gridColumn27);
            this.sdgvEmails.PrimaryGrid.Columns.Add(gridColumn28);
            this.sdgvEmails.PrimaryGrid.Columns.Add(gridColumn29);
            this.sdgvEmails.PrimaryGrid.Columns.Add(gridColumn30);
            this.sdgvEmails.PrimaryGrid.Columns.Add(gridColumn31);
            this.sdgvEmails.PrimaryGrid.Columns.Add(gridColumn32);
            this.sdgvEmails.PrimaryGrid.Columns.Add(gridColumn33);
            this.sdgvEmails.PrimaryGrid.Columns.Add(gridColumn34);
            this.sdgvEmails.PrimaryGrid.Columns.Add(gridColumn35);
            this.sdgvEmails.PrimaryGrid.Columns.Add(gridColumn36);
            this.sdgvEmails.PrimaryGrid.Columns.Add(gridColumn37);
            this.sdgvEmails.PrimaryGrid.Columns.Add(gridColumn38);
            this.sdgvEmails.PrimaryGrid.Columns.Add(gridColumn39);
            this.sdgvEmails.PrimaryGrid.FocusCuesEnabled = false;
            this.sdgvEmails.PrimaryGrid.MultiSelect = false;
            this.sdgvEmails.PrimaryGrid.ReadOnly = true;
            this.sdgvEmails.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.sdgvEmails.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvEmails.Size = new System.Drawing.Size(763, 677);
            this.sdgvEmails.TabIndex = 30;
            this.sdgvEmails.DataBindingComplete += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs>(this.sdgvEmails_DataBindingComplete);
            this.sdgvEmails.RowDoubleClick += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowDoubleClickEventArgs>(this.sdgvEmails_RowDoubleClick);
            // 
            // panelLoad
            // 
            this.panelLoad.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelLoad.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelLoad.Controls.Add(this.panelLegends);
            this.panelLoad.Controls.Add(this.txtImportPath);
            this.panelLoad.Controls.Add(this.btnLoad);
            this.panelLoad.Controls.Add(this.dtProcessDate);
            this.panelLoad.Controls.Add(this.txtSourceORAgent);
            this.panelLoad.Controls.Add(this.btnImport);
            this.panelLoad.Controls.Add(this.txtImportSource);
            this.panelLoad.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelLoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLoad.Location = new System.Drawing.Point(0, 0);
            this.panelLoad.Name = "panelLoad";
            this.panelLoad.Size = new System.Drawing.Size(1276, 60);
            this.panelLoad.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelLoad.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelLoad.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelLoad.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelLoad.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelLoad.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelLoad.Style.GradientAngle = 90;
            this.panelLoad.TabIndex = 2;
            // 
            // panelLegends
            // 
            this.panelLegends.Controls.Add(this.chkSoft);
            this.panelLegends.Controls.Add(this.chkPassed);
            this.panelLegends.Controls.Add(this.chkNotChecked);
            this.panelLegends.Controls.Add(this.chkHard);
            this.panelLegends.Controls.Add(this.lblLegendNotChecked);
            this.panelLegends.Controls.Add(this.pictureBoxNotChecked);
            this.panelLegends.Controls.Add(this.pictureBoxHard);
            this.panelLegends.Controls.Add(this.pictureBoxSoft);
            this.panelLegends.Controls.Add(this.lblLegendHard);
            this.panelLegends.Controls.Add(this.pictureBoxPassed);
            this.panelLegends.Controls.Add(this.lblLegendPassed);
            this.panelLegends.Controls.Add(this.lblLegendSoft);
            this.panelLegends.Location = new System.Drawing.Point(9, 8);
            this.panelLegends.Name = "panelLegends";
            this.panelLegends.Size = new System.Drawing.Size(239, 44);
            this.panelLegends.TabIndex = 20;
            // 
            // chkSoft
            // 
            // 
            // 
            // 
            this.chkSoft.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkSoft.Checked = true;
            this.chkSoft.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSoft.CheckValue = "Y";
            this.chkSoft.Location = new System.Drawing.Point(3, 22);
            this.chkSoft.Name = "chkSoft";
            this.chkSoft.Size = new System.Drawing.Size(15, 15);
            this.chkSoft.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkSoft.TabIndex = 1;
            this.chkSoft.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            // 
            // chkPassed
            // 
            // 
            // 
            // 
            this.chkPassed.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkPassed.Location = new System.Drawing.Point(119, 2);
            this.chkPassed.Name = "chkPassed";
            this.chkPassed.Size = new System.Drawing.Size(15, 15);
            this.chkPassed.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkPassed.TabIndex = 2;
            this.chkPassed.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            // 
            // chkNotChecked
            // 
            // 
            // 
            // 
            this.chkNotChecked.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkNotChecked.Location = new System.Drawing.Point(119, 21);
            this.chkNotChecked.Name = "chkNotChecked";
            this.chkNotChecked.Size = new System.Drawing.Size(15, 15);
            this.chkNotChecked.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkNotChecked.TabIndex = 3;
            this.chkNotChecked.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            // 
            // chkHard
            // 
            // 
            // 
            // 
            this.chkHard.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkHard.Checked = true;
            this.chkHard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHard.CheckValue = "Y";
            this.chkHard.Location = new System.Drawing.Point(3, 2);
            this.chkHard.Name = "chkHard";
            this.chkHard.Size = new System.Drawing.Size(15, 15);
            this.chkHard.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkHard.TabIndex = 0;
            this.chkHard.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            // 
            // lblLegendNotChecked
            // 
            this.lblLegendNotChecked.AutoSize = true;
            // 
            // 
            // 
            this.lblLegendNotChecked.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblLegendNotChecked.Location = new System.Drawing.Point(154, 22);
            this.lblLegendNotChecked.Name = "lblLegendNotChecked";
            this.lblLegendNotChecked.Size = new System.Drawing.Size(64, 15);
            this.lblLegendNotChecked.TabIndex = 18;
            this.lblLegendNotChecked.Text = "Not checked";
            this.lblLegendNotChecked.Click += new System.EventHandler(this.lblLegendNotChecked_Click);
            // 
            // pictureBoxNotChecked
            // 
            this.pictureBoxNotChecked.Image = global::GCC.Properties.Resources.Grey_Ball_icon__2_;
            this.pictureBoxNotChecked.Location = new System.Drawing.Point(134, 20);
            this.pictureBoxNotChecked.Name = "pictureBoxNotChecked";
            this.pictureBoxNotChecked.Size = new System.Drawing.Size(19, 18);
            this.pictureBoxNotChecked.TabIndex = 17;
            this.pictureBoxNotChecked.TabStop = false;
            this.pictureBoxNotChecked.Click += new System.EventHandler(this.pictureBoxNotChecked_Click);
            // 
            // pictureBoxHard
            // 
            this.pictureBoxHard.Image = global::GCC.Properties.Resources.Red_Ball_icon;
            this.pictureBoxHard.Location = new System.Drawing.Point(18, 1);
            this.pictureBoxHard.Name = "pictureBoxHard";
            this.pictureBoxHard.Size = new System.Drawing.Size(19, 18);
            this.pictureBoxHard.TabIndex = 9;
            this.pictureBoxHard.TabStop = false;
            this.pictureBoxHard.Click += new System.EventHandler(this.pictureBoxHard_Click);
            // 
            // pictureBoxSoft
            // 
            this.pictureBoxSoft.Image = global::GCC.Properties.Resources.Blue_Ball_icon;
            this.pictureBoxSoft.Location = new System.Drawing.Point(18, 21);
            this.pictureBoxSoft.Name = "pictureBoxSoft";
            this.pictureBoxSoft.Size = new System.Drawing.Size(19, 18);
            this.pictureBoxSoft.TabIndex = 10;
            this.pictureBoxSoft.TabStop = false;
            this.pictureBoxSoft.Click += new System.EventHandler(this.pictureBoxSoft_Click);
            // 
            // lblLegendHard
            // 
            this.lblLegendHard.AutoSize = true;
            // 
            // 
            // 
            this.lblLegendHard.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblLegendHard.Location = new System.Drawing.Point(38, 1);
            this.lblLegendHard.Name = "lblLegendHard";
            this.lblLegendHard.Size = new System.Drawing.Size(67, 15);
            this.lblLegendHard.TabIndex = 16;
            this.lblLegendHard.Text = "Hard Bounce";
            this.lblLegendHard.Click += new System.EventHandler(this.lblLegendHard_Click);
            // 
            // pictureBoxPassed
            // 
            this.pictureBoxPassed.Image = global::GCC.Properties.Resources.Green_Ball_icon;
            this.pictureBoxPassed.Location = new System.Drawing.Point(134, 1);
            this.pictureBoxPassed.Name = "pictureBoxPassed";
            this.pictureBoxPassed.Size = new System.Drawing.Size(19, 18);
            this.pictureBoxPassed.TabIndex = 12;
            this.pictureBoxPassed.TabStop = false;
            this.pictureBoxPassed.Click += new System.EventHandler(this.pictureBoxPassed_Click);
            // 
            // lblLegendPassed
            // 
            this.lblLegendPassed.AutoSize = true;
            // 
            // 
            // 
            this.lblLegendPassed.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblLegendPassed.Location = new System.Drawing.Point(154, 3);
            this.lblLegendPassed.Name = "lblLegendPassed";
            this.lblLegendPassed.Size = new System.Drawing.Size(39, 15);
            this.lblLegendPassed.TabIndex = 15;
            this.lblLegendPassed.Text = "Passed";
            this.lblLegendPassed.Click += new System.EventHandler(this.lblLegendPassed_Click);
            // 
            // lblLegendSoft
            // 
            this.lblLegendSoft.AutoSize = true;
            // 
            // 
            // 
            this.lblLegendSoft.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblLegendSoft.Location = new System.Drawing.Point(38, 22);
            this.lblLegendSoft.Name = "lblLegendSoft";
            this.lblLegendSoft.Size = new System.Drawing.Size(62, 15);
            this.lblLegendSoft.TabIndex = 14;
            this.lblLegendSoft.Text = "Soft Bounce";
            this.lblLegendSoft.Click += new System.EventHandler(this.lblLegendSoft_Click);
            // 
            // txtImportPath
            // 
            // 
            // 
            // 
            this.txtImportPath.Border.Class = "TextBoxBorder";
            this.txtImportPath.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtImportPath.ButtonCustom.Visible = true;
            this.txtImportPath.Location = new System.Drawing.Point(12, 7);
            this.txtImportPath.Name = "txtImportPath";
            this.txtImportPath.PreventEnterBeep = true;
            this.txtImportPath.Size = new System.Drawing.Size(137, 20);
            this.txtImportPath.TabIndex = 8;
            this.txtImportPath.WatermarkBehavior = DevComponents.DotNetBar.eWatermarkBehavior.HideNonEmpty;
            this.txtImportPath.WatermarkText = "Select Emails to Import";
            this.txtImportPath.ButtonCustomClick += new System.EventHandler(this.txtImportPath_ButtonCustomClick);
            // 
            // btnLoad
            // 
            this.btnLoad.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLoad.Location = new System.Drawing.Point(1191, 7);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 46);
            this.btnLoad.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "Load";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // dtProcessDate
            // 
            this.dtProcessDate.AllowEmptyState = false;
            this.dtProcessDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.dtProcessDate.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dtProcessDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dtProcessDate.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dtProcessDate.ButtonDropDown.Visible = true;
            this.dtProcessDate.IsPopupCalendarOpen = false;
            this.dtProcessDate.Location = new System.Drawing.Point(1060, 7);
            // 
            // 
            // 
            this.dtProcessDate.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dtProcessDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dtProcessDate.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this.dtProcessDate.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dtProcessDate.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dtProcessDate.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dtProcessDate.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dtProcessDate.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dtProcessDate.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dtProcessDate.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dtProcessDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dtProcessDate.MonthCalendar.DisplayMonth = new System.DateTime(2015, 10, 1, 0, 0, 0, 0);
            this.dtProcessDate.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday;
            this.dtProcessDate.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dtProcessDate.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dtProcessDate.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dtProcessDate.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dtProcessDate.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dtProcessDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dtProcessDate.MonthCalendar.TodayButtonVisible = true;
            this.dtProcessDate.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dtProcessDate.Name = "dtProcessDate";
            this.dtProcessDate.Size = new System.Drawing.Size(122, 20);
            this.dtProcessDate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.dtProcessDate.TabIndex = 4;
            // 
            // txtSourceORAgent
            // 
            this.txtSourceORAgent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceORAgent.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtSourceORAgent.Border.Class = "TextBoxBorder";
            this.txtSourceORAgent.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSourceORAgent.ButtonCustom.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtSourceORAgent.ButtonCustom.Visible = true;
            this.txtSourceORAgent.ButtonCustom2.Image = global::GCC.Properties.Resources.Actions_application_exit_icon;
            this.txtSourceORAgent.ButtonCustom2.Visible = true;
            this.txtSourceORAgent.Location = new System.Drawing.Point(1060, 32);
            this.txtSourceORAgent.Name = "txtSourceORAgent";
            this.txtSourceORAgent.PreventEnterBeep = true;
            this.txtSourceORAgent.ReadOnly = true;
            this.txtSourceORAgent.Size = new System.Drawing.Size(122, 22);
            this.txtSourceORAgent.TabIndex = 5;
            this.txtSourceORAgent.WatermarkBehavior = DevComponents.DotNetBar.eWatermarkBehavior.HideNonEmpty;
            this.txtSourceORAgent.WatermarkText = "Select Source";
            this.txtSourceORAgent.ButtonCustomClick += new System.EventHandler(this.txtSourceORAgent_ButtonCustomClick);
            this.txtSourceORAgent.ButtonCustom2Click += new System.EventHandler(this.txtSourceORAgent_ButtonCustom2Click);
            // 
            // btnImport
            // 
            this.btnImport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnImport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnImport.Location = new System.Drawing.Point(158, 7);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 46);
            this.btnImport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnImport.TabIndex = 10;
            this.btnImport.Text = "Import\r\nEmails";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // txtImportSource
            // 
            // 
            // 
            // 
            this.txtImportSource.Border.Class = "TextBoxBorder";
            this.txtImportSource.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtImportSource.ButtonCustom.Visible = true;
            this.txtImportSource.Location = new System.Drawing.Point(12, 32);
            this.txtImportSource.Name = "txtImportSource";
            this.txtImportSource.PreventEnterBeep = true;
            this.txtImportSource.Size = new System.Drawing.Size(137, 20);
            this.txtImportSource.TabIndex = 9;
            this.txtImportSource.WatermarkBehavior = DevComponents.DotNetBar.eWatermarkBehavior.HideNonEmpty;
            this.txtImportSource.WatermarkText = "Enter or Select Source";
            this.txtImportSource.ButtonCustomClick += new System.EventHandler(this.txtImportSource_ButtonCustomClick);
            // 
            // openFileEmails
            // 
            this.openFileEmails.Filter = "CSV file (*.csv)|*.csv";
            this.openFileEmails.Title = "Select CSV file with column EMAIL";
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 60);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.sdgvEmails);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.chartBounce);
            this.splitMain.Size = new System.Drawing.Size(1276, 677);
            this.splitMain.SplitterDistance = 763;
            this.splitMain.TabIndex = 4;
            // 
            // chartBounce
            // 
            this.chartBounce.BackColor = System.Drawing.Color.Transparent;
            this.chartBounce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartBounce.Legend.Visible = false;
            this.chartBounce.Location = new System.Drawing.Point(0, 0);
            this.chartBounce.LookAndFeel.SkinName = "Office 2010 Blue";
            this.chartBounce.LookAndFeel.UseDefaultLookAndFeel = false;
            this.chartBounce.Name = "chartBounce";
            this.chartBounce.PaletteName = "Bounce";
            this.chartBounce.PaletteRepository.Add("Bounce", new DevExpress.XtraCharts.Palette("Bounce", DevExpress.XtraCharts.PaletteScaleMode.Repeat, new DevExpress.XtraCharts.PaletteEntry[] {
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(129)))), ((int)(((byte)(189))))), System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(90)))), ((int)(((byte)(136)))))),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(80)))), ((int)(((byte)(77))))), System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(52)))), ((int)(((byte)(49)))))),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(187)))), ((int)(((byte)(89))))), System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(137)))), ((int)(((byte)(56)))))),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(100)))), ((int)(((byte)(162))))), System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(69)))), ((int)(((byte)(115)))))),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.Silver, System.Drawing.Color.Silver)}));
            pieSeriesLabel5.LineVisible = true;
            piePointOptions3.PercentOptions.ValueAsPercent = false;
            piePointOptions3.PointView = DevExpress.XtraCharts.PointView.ArgumentAndValues;
            pieSeriesLabel5.PointOptions = piePointOptions3;
            series3.Label = pieSeriesLabel5;
            series3.Name = "Series";
            pieSeriesView5.RuntimeExploding = false;
            series3.View = pieSeriesView5;
            this.chartBounce.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series3};
            pieSeriesLabel6.LineVisible = true;
            this.chartBounce.SeriesTemplate.Label = pieSeriesLabel6;
            pieSeriesView6.RuntimeExploding = false;
            this.chartBounce.SeriesTemplate.View = pieSeriesView6;
            this.chartBounce.Size = new System.Drawing.Size(509, 677);
            this.chartBounce.TabIndex = 5;
            this.chartBounce.TabStop = false;
            chartTitle5.Text = "Bounce Status";
            chartTitle6.Alignment = System.Drawing.StringAlignment.Near;
            chartTitle6.Font = new System.Drawing.Font("Tahoma", 9F);
            chartTitle6.Text = "Total Emails : 0";
            this.chartBounce.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle5,
            chartTitle6});
            // 
            // frmEmailChecks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 737);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.panelLoad);
            this.Name = "frmEmailChecks";
            this.Text = "Email Check";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmEmailChecks_Load);
            this.panelLoad.ResumeLayout(false);
            this.panelLegends.ResumeLayout(false);
            this.panelLegends.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNotChecked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSoft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPassed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtProcessDate)).EndInit();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartBounce)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvEmails;
        private DevComponents.DotNetBar.PanelEx panelLoad;
        private DevComponents.DotNetBar.Controls.TextBoxX txtImportPath;
        private DevComponents.DotNetBar.ButtonX btnLoad;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dtProcessDate;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSourceORAgent;
        private DevComponents.DotNetBar.ButtonX btnImport;
        private DevComponents.DotNetBar.Controls.TextBoxX txtImportSource;
        private System.Windows.Forms.OpenFileDialog openFileEmails;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel panelLegends;
        private System.Windows.Forms.PictureBox pictureBoxHard;
        private System.Windows.Forms.PictureBox pictureBoxSoft;
        private DevComponents.DotNetBar.LabelX lblLegendHard;
        private System.Windows.Forms.PictureBox pictureBoxPassed;
        private DevComponents.DotNetBar.LabelX lblLegendPassed;
        private DevComponents.DotNetBar.LabelX lblLegendSoft;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkSoft;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkPassed;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkNotChecked;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkHard;
        private DevComponents.DotNetBar.LabelX lblLegendNotChecked;
        private System.Windows.Forms.PictureBox pictureBoxNotChecked;
        private DevExpress.XtraCharts.ChartControl chartBounce;
    }
}