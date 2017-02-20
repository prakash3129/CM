namespace GCC
{
    partial class frmScrapedContacts
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
            DevComponents.DotNetBar.SuperGrid.Style.Background background1 = new DevComponents.DotNetBar.SuperGrid.Style.Background();
            DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend backColorBlend1 = new DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend();
            this.panelInfo = new DevComponents.DotNetBar.PanelEx();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnDeleteContacts = new DevComponents.DotNetBar.ButtonX();
            this.btnLoad = new DevComponents.DotNetBar.ButtonX();
            this.dateLoad = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.sdgvScrapedContacts = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.sdgvDeletedContacts = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateLoad)).BeginInit();
            this.SuspendLayout();
            // 
            // panelInfo
            // 
            this.panelInfo.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelInfo.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInfo.Location = new System.Drawing.Point(0, 0);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(1206, 49);
            this.panelInfo.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelInfo.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelInfo.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelInfo.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelInfo.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelInfo.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelInfo.Style.GradientAngle = 90;
            this.panelInfo.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnDeleteContacts);
            this.splitContainer1.Panel1.Controls.Add(this.btnLoad);
            this.splitContainer1.Panel1.Controls.Add(this.dateLoad);
            this.splitContainer1.Panel1.Controls.Add(this.sdgvScrapedContacts);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sdgvDeletedContacts);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(1206, 619);
            this.splitContainer1.SplitterDistance = 548;
            this.splitContainer1.TabIndex = 1;
            // 
            // btnDeleteContacts
            // 
            this.btnDeleteContacts.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDeleteContacts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteContacts.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDeleteContacts.Location = new System.Drawing.Point(1095, 5);
            this.btnDeleteContacts.Name = "btnDeleteContacts";
            this.btnDeleteContacts.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteContacts.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnDeleteContacts.TabIndex = 3;
            this.btnDeleteContacts.Text = "Delete";
            this.btnDeleteContacts.Click += new System.EventHandler(this.btnDeleteContacts_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLoad.Location = new System.Drawing.Point(1014, 5);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // dateLoad
            // 
            this.dateLoad.AllowEmptyState = false;
            this.dateLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.dateLoad.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dateLoad.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateLoad.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dateLoad.ButtonDropDown.Visible = true;
            this.dateLoad.IsPopupCalendarOpen = false;
            this.dateLoad.Location = new System.Drawing.Point(919, 7);
            // 
            // 
            // 
            this.dateLoad.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateLoad.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateLoad.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this.dateLoad.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dateLoad.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dateLoad.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dateLoad.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dateLoad.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dateLoad.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dateLoad.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dateLoad.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateLoad.MonthCalendar.DisplayMonth = new System.DateTime(2015, 12, 1, 0, 0, 0, 0);
            this.dateLoad.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday;
            this.dateLoad.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dateLoad.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateLoad.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dateLoad.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dateLoad.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dateLoad.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateLoad.MonthCalendar.TodayButtonVisible = true;
            this.dateLoad.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dateLoad.Name = "dateLoad";
            this.dateLoad.Size = new System.Drawing.Size(89, 20);
            this.dateLoad.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.dateLoad.TabIndex = 0;
            this.dateLoad.Value = new System.DateTime(2015, 12, 29, 18, 7, 12, 0);
            // 
            // sdgvScrapedContacts
            // 
            this.sdgvScrapedContacts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvScrapedContacts.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvScrapedContacts.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvScrapedContacts.Location = new System.Drawing.Point(0, 0);
            this.sdgvScrapedContacts.Name = "sdgvScrapedContacts";
            this.sdgvScrapedContacts.PrimaryGrid.Caption.BackgroundImage = global::GCC.Properties.Resources.Search_2_icon__1_;
            this.sdgvScrapedContacts.PrimaryGrid.Caption.BackgroundImageLayout = DevComponents.DotNetBar.SuperGrid.GridBackgroundImageLayout.TopLeft;
            this.sdgvScrapedContacts.PrimaryGrid.Caption.RowHeight = 35;
            this.sdgvScrapedContacts.PrimaryGrid.Caption.Text = "Researched contacts";
            this.sdgvScrapedContacts.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvScrapedContacts.PrimaryGrid.ColumnDragBehavior = DevComponents.DotNetBar.SuperGrid.ColumnDragBehavior.None;
            this.sdgvScrapedContacts.PrimaryGrid.ColumnHeaderClickBehavior = DevComponents.DotNetBar.SuperGrid.ColumnHeaderClickBehavior.None;
            this.sdgvScrapedContacts.PrimaryGrid.Header.Text = "";
            this.sdgvScrapedContacts.PrimaryGrid.MultiSelect = false;
            this.sdgvScrapedContacts.PrimaryGrid.RowDragBehavior = DevComponents.DotNetBar.SuperGrid.RowDragBehavior.None;
            this.sdgvScrapedContacts.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.sdgvScrapedContacts.PrimaryGrid.ShowCellInfo = false;
            this.sdgvScrapedContacts.PrimaryGrid.ShowRowDirtyMarker = false;
            this.sdgvScrapedContacts.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvScrapedContacts.Size = new System.Drawing.Size(1206, 619);
            this.sdgvScrapedContacts.TabIndex = 2;
            this.sdgvScrapedContacts.Text = "superGridControl2";
            this.sdgvScrapedContacts.CellClick += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellClickEventArgs>(this.sdgvScrapedContacts_CellClick);
            this.sdgvScrapedContacts.CellMouseEnter += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellEventArgs>(this.sdgvScrapedContacts_CellMouseEnter);
            this.sdgvScrapedContacts.DataBindingComplete += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs>(this.sdgvScrapedContacts_DataBindingComplete);
            this.sdgvScrapedContacts.PreRenderRow += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridPreRenderRowEventArgs>(this.sdgvScrapedContacts_PreRenderRow);
            // 
            // sdgvDeletedContacts
            // 
            this.sdgvDeletedContacts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvDeletedContacts.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvDeletedContacts.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvDeletedContacts.Location = new System.Drawing.Point(0, 0);
            this.sdgvDeletedContacts.Name = "sdgvDeletedContacts";
            this.sdgvDeletedContacts.PrimaryGrid.Caption.BackgroundImage = global::GCC.Properties.Resources.delete_icon;
            this.sdgvDeletedContacts.PrimaryGrid.Caption.BackgroundImageLayout = DevComponents.DotNetBar.SuperGrid.GridBackgroundImageLayout.TopLeft;
            this.sdgvDeletedContacts.PrimaryGrid.Caption.RowHeight = 35;
            this.sdgvDeletedContacts.PrimaryGrid.Caption.Text = "Marked for delete";
            this.sdgvDeletedContacts.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvDeletedContacts.PrimaryGrid.ColumnDragBehavior = DevComponents.DotNetBar.SuperGrid.ColumnDragBehavior.None;
            this.sdgvDeletedContacts.PrimaryGrid.ColumnHeaderClickBehavior = DevComponents.DotNetBar.SuperGrid.ColumnHeaderClickBehavior.None;
            backColorBlend1.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192))))),
        System.Drawing.Color.White};
            background1.BackColorBlend = backColorBlend1;
            background1.BackFillType = DevComponents.DotNetBar.SuperGrid.Style.BackFillType.HorizontalCenter;
            this.sdgvDeletedContacts.PrimaryGrid.DefaultVisualStyles.CaptionStyles.Default.Background = background1;
            this.sdgvDeletedContacts.PrimaryGrid.Header.Text = "";
            this.sdgvDeletedContacts.PrimaryGrid.MultiSelect = false;
            this.sdgvDeletedContacts.PrimaryGrid.RowDragBehavior = DevComponents.DotNetBar.SuperGrid.RowDragBehavior.None;
            this.sdgvDeletedContacts.PrimaryGrid.ShowCellInfo = false;
            this.sdgvDeletedContacts.PrimaryGrid.ShowRowDirtyMarker = false;
            this.sdgvDeletedContacts.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvDeletedContacts.Size = new System.Drawing.Size(150, 46);
            this.sdgvDeletedContacts.TabIndex = 2;
            // 
            // frmScrapedContacts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 668);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelInfo);
            this.EnableGlass = false;
            this.Name = "frmScrapedContacts";
            this.Text = "Researched Contacts";
            this.Load += new System.EventHandler(this.frmScrapedContacts_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateLoad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelInfo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvDeletedContacts;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvScrapedContacts;
        private DevComponents.DotNetBar.ButtonX btnLoad;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dateLoad;
        private DevComponents.DotNetBar.ButtonX btnDeleteContacts;
    }
}