namespace GCC
{
    partial class frmScrapper
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
            DevComponents.DotNetBar.SuperGrid.Style.Background background1 = new DevComponents.DotNetBar.SuperGrid.Style.Background();
            DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend backColorBlend1 = new DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend();
            DevComponents.DotNetBar.SuperGrid.Style.Background background2 = new DevComponents.DotNetBar.SuperGrid.Style.Background();
            DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend backColorBlend2 = new DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend();
            DevComponents.DotNetBar.SuperGrid.Style.Background background3 = new DevComponents.DotNetBar.SuperGrid.Style.Background();
            DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend backColorBlend3 = new DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend();
            DevComponents.DotNetBar.SuperGrid.Style.Background background4 = new DevComponents.DotNetBar.SuperGrid.Style.Background();
            DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend backColorBlend4 = new DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn1 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.Style.Background background5 = new DevComponents.DotNetBar.SuperGrid.Style.Background();
            DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend backColorBlend5 = new DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn2 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.Style.Background background6 = new DevComponents.DotNetBar.SuperGrid.Style.Background();
            DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend backColorBlend6 = new DevComponents.DotNetBar.SuperGrid.Style.BackColorBlend();
            this.panelInfo = new DevComponents.DotNetBar.PanelEx();
            this.btnImportInfo = new DevComponents.DotNetBar.ButtonX();
            this.panelProcess = new System.Windows.Forms.Panel();
            this.txtLinkedInUserName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnProcess = new DevComponents.DotNetBar.ButtonX();
            this.txtLinkedInPassword = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.picProcess = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblCount = new DevComponents.DotNetBar.LabelX();
            this.lblCurrentCompany = new DevComponents.DotNetBar.LabelX();
            this.btnClear = new DevComponents.DotNetBar.ButtonX();
            this.btnImport = new DevComponents.DotNetBar.ButtonX();
            this.panelLegends = new System.Windows.Forms.Panel();
            this.pictureBoxPending = new System.Windows.Forms.PictureBox();
            this.pictureBoxAccept = new System.Windows.Forms.PictureBox();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.pictureBoxReject = new System.Windows.Forms.PictureBox();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.panelArchiveLegend = new System.Windows.Forms.Panel();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.splitHorizontal = new System.Windows.Forms.SplitContainer();
            this.splitPendingNDuplicate = new System.Windows.Forms.SplitContainer();
            this.sdgvPending = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.sdgvDuplicate = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.splitReadyNNotFit = new System.Windows.Forms.SplitContainer();
            this.sdgvReady = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.sdgvInvalid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.bProcess = new System.ComponentModel.BackgroundWorker();
            this.timerStatusReader = new System.Windows.Forms.Timer(this.components);
            this.ExpanelArchive = new DevComponents.DotNetBar.ExpandablePanel();
            this.contextMenuArchive = new DevComponents.DotNetBar.ContextMenuBar();
            this.buttonSelect = new DevComponents.DotNetBar.ButtonItem();
            this.btnMarked = new DevComponents.DotNetBar.ButtonItem();
            this.btnTopSelect = new DevComponents.DotNetBar.ButtonItem();
            this.btnClearSelection = new DevComponents.DotNetBar.ButtonItem();
            this.sdgvArchive = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.txtArchiveSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.ExpanelQueue = new DevComponents.DotNetBar.ExpandablePanel();
            this.txtQueueSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.sdgvQueue = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.panelInfo.SuspendLayout();
            this.panelProcess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picProcess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelLegends.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPending)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAccept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReject)).BeginInit();
            this.panelArchiveLegend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitHorizontal)).BeginInit();
            this.splitHorizontal.Panel1.SuspendLayout();
            this.splitHorizontal.Panel2.SuspendLayout();
            this.splitHorizontal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPendingNDuplicate)).BeginInit();
            this.splitPendingNDuplicate.Panel1.SuspendLayout();
            this.splitPendingNDuplicate.Panel2.SuspendLayout();
            this.splitPendingNDuplicate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitReadyNNotFit)).BeginInit();
            this.splitReadyNNotFit.Panel1.SuspendLayout();
            this.splitReadyNNotFit.Panel2.SuspendLayout();
            this.splitReadyNNotFit.SuspendLayout();
            this.ExpanelArchive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuArchive)).BeginInit();
            this.ExpanelQueue.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelInfo
            // 
            this.panelInfo.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelInfo.Controls.Add(this.btnImportInfo);
            this.panelInfo.Controls.Add(this.panelProcess);
            this.panelInfo.Controls.Add(this.lblCount);
            this.panelInfo.Controls.Add(this.lblCurrentCompany);
            this.panelInfo.Controls.Add(this.btnClear);
            this.panelInfo.Controls.Add(this.btnImport);
            this.panelInfo.Controls.Add(this.panelLegends);
            this.panelInfo.Controls.Add(this.panelArchiveLegend);
            this.panelInfo.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInfo.Location = new System.Drawing.Point(0, 0);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(1229, 88);
            this.panelInfo.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelInfo.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelInfo.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelInfo.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelInfo.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelInfo.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelInfo.Style.GradientAngle = 90;
            this.panelInfo.TabIndex = 0;
            // 
            // btnImportInfo
            // 
            this.btnImportInfo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnImportInfo.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnImportInfo.Image = global::GCC.Properties.Resources.info_icon;
            this.btnImportInfo.Location = new System.Drawing.Point(104, 8);
            this.btnImportInfo.Name = "btnImportInfo";
            this.btnImportInfo.Size = new System.Drawing.Size(22, 22);
            this.btnImportInfo.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnImportInfo.TabIndex = 21;
            this.btnImportInfo.Visible = false;
            // 
            // panelProcess
            // 
            this.panelProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelProcess.Controls.Add(this.txtLinkedInUserName);
            this.panelProcess.Controls.Add(this.btnProcess);
            this.panelProcess.Controls.Add(this.txtLinkedInPassword);
            this.panelProcess.Controls.Add(this.picProcess);
            this.panelProcess.Controls.Add(this.pictureBox1);
            this.panelProcess.Location = new System.Drawing.Point(851, 7);
            this.panelProcess.Name = "panelProcess";
            this.panelProcess.Size = new System.Drawing.Size(372, 75);
            this.panelProcess.TabIndex = 20;
            // 
            // txtLinkedInUserName
            // 
            this.txtLinkedInUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtLinkedInUserName.Border.Class = "TextBoxBorder";
            this.txtLinkedInUserName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtLinkedInUserName.Location = new System.Drawing.Point(59, 14);
            this.txtLinkedInUserName.Name = "txtLinkedInUserName";
            this.txtLinkedInUserName.PreventEnterBeep = true;
            this.txtLinkedInUserName.Size = new System.Drawing.Size(183, 20);
            this.txtLinkedInUserName.TabIndex = 2;
            this.txtLinkedInUserName.WatermarkBehavior = DevComponents.DotNetBar.eWatermarkBehavior.HideNonEmpty;
            this.txtLinkedInUserName.WatermarkText = "Premium User Name";
            // 
            // btnProcess
            // 
            this.btnProcess.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProcess.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnProcess.Location = new System.Drawing.Point(290, 12);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 49);
            this.btnProcess.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnProcess.TabIndex = 1;
            this.btnProcess.Text = "&Start";
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // txtLinkedInPassword
            // 
            this.txtLinkedInPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtLinkedInPassword.Border.Class = "TextBoxBorder";
            this.txtLinkedInPassword.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtLinkedInPassword.Location = new System.Drawing.Point(59, 40);
            this.txtLinkedInPassword.Name = "txtLinkedInPassword";
            this.txtLinkedInPassword.PreventEnterBeep = true;
            this.txtLinkedInPassword.Size = new System.Drawing.Size(183, 20);
            this.txtLinkedInPassword.TabIndex = 3;
            this.txtLinkedInPassword.UseSystemPasswordChar = true;
            this.txtLinkedInPassword.WatermarkBehavior = DevComponents.DotNetBar.eWatermarkBehavior.HideNonEmpty;
            this.txtLinkedInPassword.WatermarkText = "Password";
            // 
            // picProcess
            // 
            this.picProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picProcess.Image = global::GCC.Properties.Resources.ezgif_com_crop;
            this.picProcess.Location = new System.Drawing.Point(246, 19);
            this.picProcess.Name = "picProcess";
            this.picProcess.Size = new System.Drawing.Size(39, 35);
            this.picProcess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picProcess.TabIndex = 7;
            this.picProcess.TabStop = false;
            this.picProcess.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::GCC.Properties.Resources.linkedin_icon;
            this.pictureBox1.Location = new System.Drawing.Point(13, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 47);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            // 
            // 
            // 
            this.lblCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblCount.Location = new System.Drawing.Point(433, 35);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(56, 15);
            this.lblCount.TabIndex = 18;
            this.lblCount.Text = "Completed";
            // 
            // lblCurrentCompany
            // 
            this.lblCurrentCompany.AutoSize = true;
            // 
            // 
            // 
            this.lblCurrentCompany.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblCurrentCompany.Location = new System.Drawing.Point(433, 12);
            this.lblCurrentCompany.Name = "lblCurrentCompany";
            this.lblCurrentCompany.Size = new System.Drawing.Size(89, 15);
            this.lblCurrentCompany.TabIndex = 17;
            this.lblCurrentCompany.Text = "Current Company";
            // 
            // btnClear
            // 
            this.btnClear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClear.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClear.Location = new System.Drawing.Point(8, 41);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(91, 23);
            this.btnClear.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear All";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnImport
            // 
            this.btnImport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnImport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnImport.Location = new System.Drawing.Point(8, 7);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(91, 23);
            this.btnImport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "Import";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // panelLegends
            // 
            this.panelLegends.Controls.Add(this.pictureBoxPending);
            this.panelLegends.Controls.Add(this.pictureBoxAccept);
            this.panelLegends.Controls.Add(this.labelX3);
            this.panelLegends.Controls.Add(this.pictureBoxReject);
            this.panelLegends.Controls.Add(this.labelX2);
            this.panelLegends.Controls.Add(this.labelX1);
            this.panelLegends.Location = new System.Drawing.Point(127, 7);
            this.panelLegends.Name = "panelLegends";
            this.panelLegends.Size = new System.Drawing.Size(282, 71);
            this.panelLegends.TabIndex = 19;
            // 
            // pictureBoxPending
            // 
            this.pictureBoxPending.Image = global::GCC.Properties.Resources.Yellow;
            this.pictureBoxPending.Location = new System.Drawing.Point(10, 1);
            this.pictureBoxPending.Name = "pictureBoxPending";
            this.pictureBoxPending.Size = new System.Drawing.Size(19, 18);
            this.pictureBoxPending.TabIndex = 9;
            this.pictureBoxPending.TabStop = false;
            // 
            // pictureBoxAccept
            // 
            this.pictureBoxAccept.Image = global::GCC.Properties.Resources.Green1;
            this.pictureBoxAccept.Location = new System.Drawing.Point(10, 25);
            this.pictureBoxAccept.Name = "pictureBoxAccept";
            this.pictureBoxAccept.Size = new System.Drawing.Size(19, 18);
            this.pictureBoxAccept.TabIndex = 10;
            this.pictureBoxAccept.TabStop = false;
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(35, 1);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(189, 15);
            this.labelX3.TabIndex = 16;
            this.labelX3.Text = "Move to pending bin (Citation needed)";
            // 
            // pictureBoxReject
            // 
            this.pictureBoxReject.Image = global::GCC.Properties.Resources.Red1;
            this.pictureBoxReject.Location = new System.Drawing.Point(10, 49);
            this.pictureBoxReject.Name = "pictureBoxReject";
            this.pictureBoxReject.Size = new System.Drawing.Size(19, 18);
            this.pictureBoxReject.TabIndex = 12;
            this.pictureBoxReject.TabStop = false;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(35, 49);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(125, 15);
            this.labelX2.TabIndex = 15;
            this.labelX2.Text = "Manual reject bin (Not fit)";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(35, 25);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(228, 15);
            this.labelX1.TabIndex = 14;
            this.labelX1.Text = "Add company to data mining bin (Ready to go)";
            // 
            // panelArchiveLegend
            // 
            this.panelArchiveLegend.Controls.Add(this.labelX5);
            this.panelArchiveLegend.Controls.Add(this.labelX4);
            this.panelArchiveLegend.Controls.Add(this.pictureBox2);
            this.panelArchiveLegend.Location = new System.Drawing.Point(131, 19);
            this.panelArchiveLegend.Name = "panelArchiveLegend";
            this.panelArchiveLegend.Size = new System.Drawing.Size(200, 41);
            this.panelArchiveLegend.TabIndex = 25;
            this.panelArchiveLegend.Visible = false;
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Location = new System.Drawing.Point(13, 13);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(42, 15);
            this.labelX5.TabIndex = 24;
            this.labelX5.Text = "Click on";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(79, 13);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(91, 15);
            this.labelX4.TabIndex = 23;
            this.labelX4.Text = "to toggle selection";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::GCC.Properties.Resources.Gray;
            this.pictureBox2.Location = new System.Drawing.Point(57, 13);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(19, 18);
            this.pictureBox2.TabIndex = 22;
            this.pictureBox2.TabStop = false;
            // 
            // splitHorizontal
            // 
            this.splitHorizontal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitHorizontal.Location = new System.Drawing.Point(30, 88);
            this.splitHorizontal.Name = "splitHorizontal";
            this.splitHorizontal.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitHorizontal.Panel1
            // 
            this.splitHorizontal.Panel1.Controls.Add(this.splitPendingNDuplicate);
            // 
            // splitHorizontal.Panel2
            // 
            this.splitHorizontal.Panel2.Controls.Add(this.splitReadyNNotFit);
            this.splitHorizontal.Size = new System.Drawing.Size(1169, 645);
            this.splitHorizontal.SplitterDistance = 330;
            this.splitHorizontal.SplitterWidth = 8;
            this.splitHorizontal.TabIndex = 1;
            // 
            // splitPendingNDuplicate
            // 
            this.splitPendingNDuplicate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPendingNDuplicate.Location = new System.Drawing.Point(0, 0);
            this.splitPendingNDuplicate.Name = "splitPendingNDuplicate";
            // 
            // splitPendingNDuplicate.Panel1
            // 
            this.splitPendingNDuplicate.Panel1.Controls.Add(this.sdgvPending);
            // 
            // splitPendingNDuplicate.Panel2
            // 
            this.splitPendingNDuplicate.Panel2.Controls.Add(this.sdgvDuplicate);
            this.splitPendingNDuplicate.Size = new System.Drawing.Size(1169, 330);
            this.splitPendingNDuplicate.SplitterDistance = 531;
            this.splitPendingNDuplicate.TabIndex = 0;
            // 
            // sdgvPending
            // 
            this.sdgvPending.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvPending.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvPending.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvPending.Location = new System.Drawing.Point(0, 0);
            this.sdgvPending.Name = "sdgvPending";
            this.sdgvPending.PrimaryGrid.Caption.BackgroundImage = global::GCC.Properties.Resources.Status_dialog_warning_icon;
            this.sdgvPending.PrimaryGrid.Caption.BackgroundImageLayout = DevComponents.DotNetBar.SuperGrid.GridBackgroundImageLayout.TopLeft;
            this.sdgvPending.PrimaryGrid.Caption.RowHeight = 35;
            this.sdgvPending.PrimaryGrid.Caption.Text = "Citation Needed";
            this.sdgvPending.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvPending.PrimaryGrid.ColumnDragBehavior = DevComponents.DotNetBar.SuperGrid.ColumnDragBehavior.None;
            this.sdgvPending.PrimaryGrid.ColumnHeaderClickBehavior = DevComponents.DotNetBar.SuperGrid.ColumnHeaderClickBehavior.None;
            backColorBlend1.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192))))),
        System.Drawing.Color.White};
            background1.BackColorBlend = backColorBlend1;
            background1.BackFillType = DevComponents.DotNetBar.SuperGrid.Style.BackFillType.HorizontalCenter;
            this.sdgvPending.PrimaryGrid.DefaultVisualStyles.CaptionStyles.Default.Background = background1;
            this.sdgvPending.PrimaryGrid.Header.Text = "";
            this.sdgvPending.PrimaryGrid.MultiSelect = false;
            this.sdgvPending.PrimaryGrid.RowDragBehavior = DevComponents.DotNetBar.SuperGrid.RowDragBehavior.None;
            this.sdgvPending.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.sdgvPending.PrimaryGrid.ShowCellInfo = false;
            this.sdgvPending.PrimaryGrid.ShowRowDirtyMarker = false;
            this.sdgvPending.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvPending.Size = new System.Drawing.Size(531, 330);
            this.sdgvPending.TabIndex = 0;
            this.sdgvPending.RowActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowActivatedEventArgs>(this.sdgvPending_RowActivated);
            // 
            // sdgvDuplicate
            // 
            this.sdgvDuplicate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvDuplicate.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvDuplicate.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvDuplicate.Location = new System.Drawing.Point(0, 0);
            this.sdgvDuplicate.Name = "sdgvDuplicate";
            this.sdgvDuplicate.PrimaryGrid.Caption.BackgroundImage = global::GCC.Properties.Resources.duplicate32x32;
            this.sdgvDuplicate.PrimaryGrid.Caption.BackgroundImageLayout = DevComponents.DotNetBar.SuperGrid.GridBackgroundImageLayout.TopLeft;
            this.sdgvDuplicate.PrimaryGrid.Caption.RowHeight = 35;
            this.sdgvDuplicate.PrimaryGrid.Caption.Text = "Possible Duplicates";
            this.sdgvDuplicate.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvDuplicate.PrimaryGrid.ColumnDragBehavior = DevComponents.DotNetBar.SuperGrid.ColumnDragBehavior.None;
            this.sdgvDuplicate.PrimaryGrid.ColumnHeaderClickBehavior = DevComponents.DotNetBar.SuperGrid.ColumnHeaderClickBehavior.None;
            backColorBlend2.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.SteelBlue,
        System.Drawing.Color.White};
            background2.BackColorBlend = backColorBlend2;
            background2.BackFillType = DevComponents.DotNetBar.SuperGrid.Style.BackFillType.HorizontalCenter;
            this.sdgvDuplicate.PrimaryGrid.DefaultVisualStyles.CaptionStyles.Default.Background = background2;
            this.sdgvDuplicate.PrimaryGrid.Header.BackgroundImageLayout = DevComponents.DotNetBar.SuperGrid.GridBackgroundImageLayout.TopLeft;
            this.sdgvDuplicate.PrimaryGrid.Header.Text = "";
            this.sdgvDuplicate.PrimaryGrid.MultiSelect = false;
            this.sdgvDuplicate.PrimaryGrid.RowDragBehavior = DevComponents.DotNetBar.SuperGrid.RowDragBehavior.None;
            this.sdgvDuplicate.PrimaryGrid.ShowCellInfo = false;
            this.sdgvDuplicate.PrimaryGrid.ShowRowDirtyMarker = false;
            this.sdgvDuplicate.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvDuplicate.Size = new System.Drawing.Size(634, 330);
            this.sdgvDuplicate.TabIndex = 1;
            // 
            // splitReadyNNotFit
            // 
            this.splitReadyNNotFit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitReadyNNotFit.Location = new System.Drawing.Point(0, 0);
            this.splitReadyNNotFit.Name = "splitReadyNNotFit";
            // 
            // splitReadyNNotFit.Panel1
            // 
            this.splitReadyNNotFit.Panel1.Controls.Add(this.sdgvReady);
            // 
            // splitReadyNNotFit.Panel2
            // 
            this.splitReadyNNotFit.Panel2.Controls.Add(this.sdgvInvalid);
            this.splitReadyNNotFit.Size = new System.Drawing.Size(1169, 307);
            this.splitReadyNNotFit.SplitterDistance = 531;
            this.splitReadyNNotFit.TabIndex = 0;
            // 
            // sdgvReady
            // 
            this.sdgvReady.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvReady.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvReady.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvReady.Location = new System.Drawing.Point(0, 0);
            this.sdgvReady.Name = "sdgvReady";
            this.sdgvReady.PrimaryGrid.Caption.BackgroundImage = global::GCC.Properties.Resources.thumbsupbutton32x32ss__1_;
            this.sdgvReady.PrimaryGrid.Caption.BackgroundImageLayout = DevComponents.DotNetBar.SuperGrid.GridBackgroundImageLayout.TopLeft;
            this.sdgvReady.PrimaryGrid.Caption.RowHeight = 35;
            this.sdgvReady.PrimaryGrid.Caption.Text = "Ready to go";
            this.sdgvReady.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvReady.PrimaryGrid.ColumnDragBehavior = DevComponents.DotNetBar.SuperGrid.ColumnDragBehavior.None;
            this.sdgvReady.PrimaryGrid.ColumnHeaderClickBehavior = DevComponents.DotNetBar.SuperGrid.ColumnHeaderClickBehavior.None;
            backColorBlend3.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192))))),
        System.Drawing.Color.White};
            background3.BackColorBlend = backColorBlend3;
            background3.BackFillType = DevComponents.DotNetBar.SuperGrid.Style.BackFillType.HorizontalCenter;
            this.sdgvReady.PrimaryGrid.DefaultVisualStyles.CaptionStyles.Default.Background = background3;
            this.sdgvReady.PrimaryGrid.Header.Text = "";
            this.sdgvReady.PrimaryGrid.MultiSelect = false;
            this.sdgvReady.PrimaryGrid.RowDragBehavior = DevComponents.DotNetBar.SuperGrid.RowDragBehavior.None;
            this.sdgvReady.PrimaryGrid.ShowCellInfo = false;
            this.sdgvReady.PrimaryGrid.ShowRowDirtyMarker = false;
            this.sdgvReady.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvReady.Size = new System.Drawing.Size(531, 307);
            this.sdgvReady.TabIndex = 1;
            // 
            // sdgvInvalid
            // 
            this.sdgvInvalid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvInvalid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvInvalid.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvInvalid.Location = new System.Drawing.Point(0, 0);
            this.sdgvInvalid.Name = "sdgvInvalid";
            this.sdgvInvalid.PrimaryGrid.Caption.BackgroundImage = global::GCC.Properties.Resources.delete_icon;
            this.sdgvInvalid.PrimaryGrid.Caption.BackgroundImageLayout = DevComponents.DotNetBar.SuperGrid.GridBackgroundImageLayout.TopLeft;
            this.sdgvInvalid.PrimaryGrid.Caption.RowHeight = 35;
            this.sdgvInvalid.PrimaryGrid.Caption.Text = "Not fit";
            this.sdgvInvalid.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvInvalid.PrimaryGrid.ColumnDragBehavior = DevComponents.DotNetBar.SuperGrid.ColumnDragBehavior.None;
            this.sdgvInvalid.PrimaryGrid.ColumnHeaderClickBehavior = DevComponents.DotNetBar.SuperGrid.ColumnHeaderClickBehavior.None;
            backColorBlend4.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192))))),
        System.Drawing.Color.White};
            background4.BackColorBlend = backColorBlend4;
            background4.BackFillType = DevComponents.DotNetBar.SuperGrid.Style.BackFillType.HorizontalCenter;
            this.sdgvInvalid.PrimaryGrid.DefaultVisualStyles.CaptionStyles.Default.Background = background4;
            this.sdgvInvalid.PrimaryGrid.Header.Text = "";
            this.sdgvInvalid.PrimaryGrid.MultiSelect = false;
            this.sdgvInvalid.PrimaryGrid.RowDragBehavior = DevComponents.DotNetBar.SuperGrid.RowDragBehavior.None;
            this.sdgvInvalid.PrimaryGrid.ShowCellInfo = false;
            this.sdgvInvalid.PrimaryGrid.ShowRowDirtyMarker = false;
            this.sdgvInvalid.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvInvalid.Size = new System.Drawing.Size(634, 307);
            this.sdgvInvalid.TabIndex = 1;
            // 
            // openFile
            // 
            this.openFile.Filter = "CSV File|*.csv";
            // 
            // bProcess
            // 
            this.bProcess.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bProcess_DoWork);
            this.bProcess.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bProcess_RunWorkerCompleted);
            // 
            // timerStatusReader
            // 
            this.timerStatusReader.Interval = 5000;
            this.timerStatusReader.Tick += new System.EventHandler(this.timerStatusReader_Tick);
            // 
            // ExpanelArchive
            // 
            this.ExpanelArchive.CanvasColor = System.Drawing.SystemColors.Control;
            this.ExpanelArchive.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.RightToLeft;
            this.ExpanelArchive.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ExpanelArchive.Controls.Add(this.contextMenuArchive);
            this.ExpanelArchive.Controls.Add(this.txtArchiveSearch);
            this.ExpanelArchive.Controls.Add(this.sdgvArchive);
            this.ExpanelArchive.DisabledBackColor = System.Drawing.Color.Empty;
            this.ExpanelArchive.Dock = System.Windows.Forms.DockStyle.Left;
            this.ExpanelArchive.Expanded = false;
            this.ExpanelArchive.ExpandedBounds = new System.Drawing.Rectangle(0, 88, 598, 645);
            this.ExpanelArchive.ExpandOnTitleClick = true;
            this.ExpanelArchive.HideControlsWhenCollapsed = true;
            this.ExpanelArchive.Location = new System.Drawing.Point(0, 88);
            this.ExpanelArchive.Name = "ExpanelArchive";
            this.ExpanelArchive.Size = new System.Drawing.Size(30, 645);
            this.ExpanelArchive.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.ExpanelArchive.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.ExpanelArchive.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.ExpanelArchive.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.ExpanelArchive.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.ExpanelArchive.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.ExpanelArchive.Style.GradientAngle = 90;
            this.ExpanelArchive.TabIndex = 2;
            this.ExpanelArchive.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this.ExpanelArchive.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.ExpanelArchive.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.ExpanelArchive.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.ExpanelArchive.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ExpanelArchive.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.ExpanelArchive.TitleStyle.GradientAngle = 90;
            this.ExpanelArchive.TitleText = "Archive";
            this.ExpanelArchive.ExpandedChanging += new DevComponents.DotNetBar.ExpandChangeEventHandler(this.ExpanelArchive_ExpandedChanging);
            this.ExpanelArchive.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this.ExpanelArchive_ExpandedChanged);
            // 
            // contextMenuArchive
            // 
            this.contextMenuArchive.AntiAlias = true;
            this.contextMenuArchive.DockSide = DevComponents.DotNetBar.eDockSide.Left;
            this.contextMenuArchive.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuArchive.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonSelect});
            this.contextMenuArchive.Location = new System.Drawing.Point(12, 93);
            this.contextMenuArchive.Name = "contextMenuArchive";
            this.contextMenuArchive.Size = new System.Drawing.Size(75, 25);
            this.contextMenuArchive.Stretch = true;
            this.contextMenuArchive.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuArchive.TabIndex = 4;
            this.contextMenuArchive.TabStop = false;
            // 
            // buttonSelect
            // 
            this.buttonSelect.AutoExpandOnClick = true;
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnMarked,
            this.btnTopSelect,
            this.btnClearSelection});
            this.buttonSelect.Text = "Select";
            // 
            // btnMarked
            // 
            this.btnMarked.Name = "btnMarked";
            this.btnMarked.Text = "Mark Selected";
            this.btnMarked.Click += new System.EventHandler(this.btnMarked_Click);
            // 
            // btnTopSelect
            // 
            this.btnTopSelect.Name = "btnTopSelect";
            this.btnTopSelect.Text = "Select Top 30";
            this.btnTopSelect.Click += new System.EventHandler(this.btnTopSelect_Click);
            // 
            // btnClearSelection
            // 
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.Text = "Clear All Selection";
            this.btnClearSelection.Click += new System.EventHandler(this.btnClearSelection_Click);
            // 
            // sdgvArchive
            // 
            this.contextMenuArchive.SetContextMenuEx(this.sdgvArchive, this.buttonSelect);
            this.sdgvArchive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvArchive.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvArchive.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvArchive.Location = new System.Drawing.Point(0, 26);
            this.sdgvArchive.Name = "sdgvArchive";
            this.sdgvArchive.PrimaryGrid.Caption.BackgroundImage = global::GCC.Properties.Resources.archive_icon;
            this.sdgvArchive.PrimaryGrid.Caption.BackgroundImageLayout = DevComponents.DotNetBar.SuperGrid.GridBackgroundImageLayout.TopLeft;
            this.sdgvArchive.PrimaryGrid.Caption.RowHeight = 35;
            this.sdgvArchive.PrimaryGrid.Caption.Text = "Archived Companies";
            this.sdgvArchive.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvArchive.PrimaryGrid.ColumnDragBehavior = DevComponents.DotNetBar.SuperGrid.ColumnDragBehavior.None;
            this.sdgvArchive.PrimaryGrid.ColumnHeaderClickBehavior = DevComponents.DotNetBar.SuperGrid.ColumnHeaderClickBehavior.None;
            gridColumn1.DataPropertyName = "COMPANY_ID";
            gridColumn1.Name = "colCompanyID";
            gridColumn1.Visible = false;
            this.sdgvArchive.PrimaryGrid.Columns.Add(gridColumn1);
            backColorBlend5.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))),
        System.Drawing.Color.White};
            background5.BackColorBlend = backColorBlend5;
            background5.BackFillType = DevComponents.DotNetBar.SuperGrid.Style.BackFillType.HorizontalCenter;
            this.sdgvArchive.PrimaryGrid.DefaultVisualStyles.CaptionStyles.Default.Background = background5;
            this.sdgvArchive.PrimaryGrid.Header.Text = "";
            this.sdgvArchive.PrimaryGrid.MouseEditMode = DevComponents.DotNetBar.SuperGrid.MouseEditMode.None;
            this.sdgvArchive.PrimaryGrid.RowDragBehavior = DevComponents.DotNetBar.SuperGrid.RowDragBehavior.None;
            this.sdgvArchive.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.sdgvArchive.PrimaryGrid.ShowCellInfo = false;
            this.sdgvArchive.PrimaryGrid.ShowRowDirtyMarker = false;
            this.sdgvArchive.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvArchive.Size = new System.Drawing.Size(30, 619);
            this.sdgvArchive.TabIndex = 1;
            this.sdgvArchive.DataBindingComplete += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs>(this.sdgvArchive_DataBindingComplete);
            // 
            // txtArchiveSearch
            // 
            // 
            // 
            // 
            this.txtArchiveSearch.Border.Class = "TextBoxBorder";
            this.txtArchiveSearch.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtArchiveSearch.Location = new System.Drawing.Point(42, 33);
            this.txtArchiveSearch.Name = "txtArchiveSearch";
            this.txtArchiveSearch.PreventEnterBeep = true;
            this.txtArchiveSearch.Size = new System.Drawing.Size(129, 20);
            this.txtArchiveSearch.TabIndex = 2;
            this.txtArchiveSearch.WatermarkBehavior = DevComponents.DotNetBar.eWatermarkBehavior.HideNonEmpty;
            this.txtArchiveSearch.WatermarkText = "Search";
            this.txtArchiveSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // ExpanelQueue
            // 
            this.ExpanelQueue.CanvasColor = System.Drawing.SystemColors.Control;
            this.ExpanelQueue.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.LeftToRight;
            this.ExpanelQueue.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ExpanelQueue.Controls.Add(this.txtQueueSearch);
            this.ExpanelQueue.Controls.Add(this.sdgvQueue);
            this.ExpanelQueue.DisabledBackColor = System.Drawing.Color.Empty;
            this.ExpanelQueue.Dock = System.Windows.Forms.DockStyle.Right;
            this.ExpanelQueue.Expanded = false;
            this.ExpanelQueue.ExpandedBounds = new System.Drawing.Rectangle(500, 88, 729, 645);
            this.ExpanelQueue.ExpandOnTitleClick = true;
            this.ExpanelQueue.HideControlsWhenCollapsed = true;
            this.ExpanelQueue.Location = new System.Drawing.Point(1199, 88);
            this.ExpanelQueue.Name = "ExpanelQueue";
            this.ExpanelQueue.Size = new System.Drawing.Size(30, 645);
            this.ExpanelQueue.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.ExpanelQueue.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.ExpanelQueue.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.ExpanelQueue.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.ExpanelQueue.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.ExpanelQueue.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.ExpanelQueue.Style.GradientAngle = 90;
            this.ExpanelQueue.TabIndex = 3;
            this.ExpanelQueue.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this.ExpanelQueue.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.ExpanelQueue.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.ExpanelQueue.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.ExpanelQueue.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ExpanelQueue.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.ExpanelQueue.TitleStyle.GradientAngle = 90;
            this.ExpanelQueue.TitleText = "Queue";
            this.ExpanelQueue.ExpandedChanging += new DevComponents.DotNetBar.ExpandChangeEventHandler(this.ExpanelQueue_ExpandedChanging);
            this.ExpanelQueue.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this.ExpanelQueue_ExpandedChanged);
            // 
            // txtQueueSearch
            // 
            // 
            // 
            // 
            this.txtQueueSearch.Border.Class = "TextBoxBorder";
            this.txtQueueSearch.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtQueueSearch.Location = new System.Drawing.Point(42, 33);
            this.txtQueueSearch.Name = "txtQueueSearch";
            this.txtQueueSearch.PreventEnterBeep = true;
            this.txtQueueSearch.Size = new System.Drawing.Size(129, 20);
            this.txtQueueSearch.TabIndex = 2;
            this.txtQueueSearch.WatermarkBehavior = DevComponents.DotNetBar.eWatermarkBehavior.HideNonEmpty;
            this.txtQueueSearch.WatermarkText = "Search";
            this.txtQueueSearch.TextChanged += new System.EventHandler(this.txtQueueSearch_TextChanged);
            // 
            // sdgvQueue
            // 
            this.sdgvQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvQueue.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvQueue.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvQueue.Location = new System.Drawing.Point(0, 26);
            this.sdgvQueue.Name = "sdgvQueue";
            this.sdgvQueue.PrimaryGrid.Caption.BackgroundImage = global::GCC.Properties.Resources.sand_glass_icon;
            this.sdgvQueue.PrimaryGrid.Caption.BackgroundImageLayout = DevComponents.DotNetBar.SuperGrid.GridBackgroundImageLayout.TopLeft;
            this.sdgvQueue.PrimaryGrid.Caption.RowHeight = 35;
            this.sdgvQueue.PrimaryGrid.Caption.Text = "Companies in queue";
            this.sdgvQueue.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvQueue.PrimaryGrid.ColumnDragBehavior = DevComponents.DotNetBar.SuperGrid.ColumnDragBehavior.None;
            this.sdgvQueue.PrimaryGrid.ColumnHeaderClickBehavior = DevComponents.DotNetBar.SuperGrid.ColumnHeaderClickBehavior.None;
            gridColumn2.DataPropertyName = "COMPANY_ID";
            gridColumn2.Name = "colCompanyID";
            gridColumn2.Visible = false;
            this.sdgvQueue.PrimaryGrid.Columns.Add(gridColumn2);
            backColorBlend6.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128))))),
        System.Drawing.Color.White};
            background6.BackColorBlend = backColorBlend6;
            background6.BackFillType = DevComponents.DotNetBar.SuperGrid.Style.BackFillType.HorizontalCenter;
            this.sdgvQueue.PrimaryGrid.DefaultVisualStyles.CaptionStyles.Default.Background = background6;
            this.sdgvQueue.PrimaryGrid.Header.Text = "";
            this.sdgvQueue.PrimaryGrid.MultiSelect = false;
            this.sdgvQueue.PrimaryGrid.RowDragBehavior = DevComponents.DotNetBar.SuperGrid.RowDragBehavior.None;
            this.sdgvQueue.PrimaryGrid.ShowCellInfo = false;
            this.sdgvQueue.PrimaryGrid.ShowRowDirtyMarker = false;
            this.sdgvQueue.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvQueue.Size = new System.Drawing.Size(30, 619);
            this.sdgvQueue.TabIndex = 1;
            // 
            // frmScrapper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1229, 733);
            this.Controls.Add(this.splitHorizontal);
            this.Controls.Add(this.ExpanelQueue);
            this.Controls.Add(this.ExpanelArchive);
            this.Controls.Add(this.panelInfo);
            this.EnableGlass = false;
            this.Name = "frmScrapper";
            this.Text = "Data Miner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCompanyImport_FormClosing);
            this.Load += new System.EventHandler(this.frmCompanyImport_Load);
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.panelProcess.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picProcess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelLegends.ResumeLayout(false);
            this.panelLegends.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPending)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAccept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReject)).EndInit();
            this.panelArchiveLegend.ResumeLayout(false);
            this.panelArchiveLegend.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.splitHorizontal.Panel1.ResumeLayout(false);
            this.splitHorizontal.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitHorizontal)).EndInit();
            this.splitHorizontal.ResumeLayout(false);
            this.splitPendingNDuplicate.Panel1.ResumeLayout(false);
            this.splitPendingNDuplicate.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPendingNDuplicate)).EndInit();
            this.splitPendingNDuplicate.ResumeLayout(false);
            this.splitReadyNNotFit.Panel1.ResumeLayout(false);
            this.splitReadyNNotFit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitReadyNNotFit)).EndInit();
            this.splitReadyNNotFit.ResumeLayout(false);
            this.ExpanelArchive.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuArchive)).EndInit();
            this.ExpanelQueue.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelInfo;
        private System.Windows.Forms.SplitContainer splitHorizontal;
        private System.Windows.Forms.SplitContainer splitReadyNNotFit;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvPending;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvReady;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvInvalid;
        private System.Windows.Forms.SplitContainer splitPendingNDuplicate;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvDuplicate;
        private System.Windows.Forms.OpenFileDialog openFile;
        private DevComponents.DotNetBar.ButtonX btnProcess;
        private System.ComponentModel.BackgroundWorker bProcess;
        private DevComponents.DotNetBar.Controls.TextBoxX txtLinkedInPassword;
        private DevComponents.DotNetBar.Controls.TextBoxX txtLinkedInUserName;
        private System.Windows.Forms.Timer timerStatusReader;
        private DevComponents.DotNetBar.ButtonX btnImport;
        private System.Windows.Forms.PictureBox picProcess;
        private DevComponents.DotNetBar.ExpandablePanel ExpanelArchive;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvArchive;
        private DevComponents.DotNetBar.ButtonX btnClear;
        private DevComponents.DotNetBar.Controls.TextBoxX txtArchiveSearch;
        private DevComponents.DotNetBar.ExpandablePanel ExpanelQueue;
        private DevComponents.DotNetBar.Controls.TextBoxX txtQueueSearch;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvQueue;
        private System.Windows.Forms.PictureBox pictureBoxPending;
        private System.Windows.Forms.PictureBox pictureBoxReject;
        private System.Windows.Forms.PictureBox pictureBoxAccept;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX lblCurrentCompany;
        private DevComponents.DotNetBar.LabelX lblCount;
        private System.Windows.Forms.Panel panelLegends;
        private System.Windows.Forms.Panel panelProcess;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevComponents.DotNetBar.ButtonX btnImportInfo;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuArchive;
        private DevComponents.DotNetBar.ButtonItem buttonSelect;
        private DevComponents.DotNetBar.ButtonItem btnMarked;
        private DevComponents.DotNetBar.ButtonItem btnTopSelect;
        private DevComponents.DotNetBar.ButtonItem btnClearSelection;
        private System.Windows.Forms.PictureBox pictureBox2;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX5;
        private System.Windows.Forms.Panel panelArchiveLegend;
    }
}