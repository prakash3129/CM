namespace GCC
{
    partial class frmTeamPerformance
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
            DevComponents.DotNetBar.SuperGrid.Style.Background background1 = new DevComponents.DotNetBar.SuperGrid.Style.Background();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn2 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.Style.Background background2 = new DevComponents.DotNetBar.SuperGrid.Style.Background();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTeamPerformance));
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn3 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.Style.Background background3 = new DevComponents.DotNetBar.SuperGrid.Style.Background();
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn4 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            DevComponents.DotNetBar.SuperGrid.Style.Background background4 = new DevComponents.DotNetBar.SuperGrid.Style.Background();
            this.sdgvProjectPerformance = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.sdgvOverallPerformance = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.lblEmpID = new DevComponents.DotNetBar.LabelX();
            this.lblResearchType = new DevComponents.DotNetBar.LabelX();
            this.lblTenure = new DevComponents.DotNetBar.LabelX();
            this.lblAgentName = new DevComponents.DotNetBar.LabelX();
            this.lblUpload = new DevComponents.DotNetBar.LabelX();
            this.lblCurrentProject = new DevComponents.DotNetBar.LabelX();
            this.btnTarget = new DevComponents.DotNetBar.ButtonX();
            this.lblUserType = new DevComponents.DotNetBar.LabelX();
            this.txtTarget = new DevComponents.Editors.IntegerInput();
            this.lblTodaysTarget = new DevComponents.DotNetBar.LabelX();
            this.pictureDP = new System.Windows.Forms.PictureBox();
            this.lblTargetMissed = new DevComponents.DotNetBar.LabelX();
            this.lblTargetAchived = new DevComponents.DotNetBar.LabelX();
            this.lblMonthlyPoints = new DevComponents.DotNetBar.LabelX();
            this.splitContainerGrids = new System.Windows.Forms.SplitContainer();
            this.btnShowAllProject = new DevComponents.DotNetBar.ButtonX();
            this.btnShowAllMTD = new DevComponents.DotNetBar.ButtonX();
            this.splitContainerInformation = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.txtTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGrids)).BeginInit();
            this.splitContainerGrids.Panel1.SuspendLayout();
            this.splitContainerGrids.Panel2.SuspendLayout();
            this.splitContainerGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInformation)).BeginInit();
            this.splitContainerInformation.Panel1.SuspendLayout();
            this.splitContainerInformation.Panel2.SuspendLayout();
            this.splitContainerInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // sdgvProjectPerformance
            // 
            this.sdgvProjectPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvProjectPerformance.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvProjectPerformance.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvProjectPerformance.Location = new System.Drawing.Point(0, 0);
            this.sdgvProjectPerformance.Name = "sdgvProjectPerformance";
            this.sdgvProjectPerformance.PrimaryGrid.AllowRowResize = true;
            this.sdgvProjectPerformance.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvProjectPerformance.PrimaryGrid.ColumnHeader.Visible = false;
            gridColumn1.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.None;
            gridColumn1.CellHighlightMode = DevComponents.DotNetBar.SuperGrid.Style.CellHighlightMode.Content;
            gridColumn1.CellStyles.Default.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            background1.Color1 = System.Drawing.Color.White;
            background1.Color2 = System.Drawing.Color.LightSkyBlue;
            gridColumn1.CellStyles.MouseOver.Background = background1;
            gridColumn1.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridImageEditControl);
            gridColumn1.FillWeight = 25;
            gridColumn1.Name = "Photo";
            gridColumn1.ResizeMode = DevComponents.DotNetBar.SuperGrid.ColumnResizeMode.None;
            gridColumn1.Width = 115;
            gridColumn2.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.None;
            background2.Color1 = System.Drawing.Color.White;
            background2.Color2 = System.Drawing.Color.LightSkyBlue;
            gridColumn2.CellStyles.MouseOver.Background = background2;
            gridColumn2.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn2.EnableHeaderMarkup = true;
            gridColumn2.FillWeight = 70;
            gridColumn2.MinimumWidth = 100;
            gridColumn2.Name = "Information";
            gridColumn2.ResizeMode = DevComponents.DotNetBar.SuperGrid.ColumnResizeMode.MaintainTotalWidth;
            gridColumn2.Width = 150;
            this.sdgvProjectPerformance.PrimaryGrid.Columns.Add(gridColumn1);
            this.sdgvProjectPerformance.PrimaryGrid.Columns.Add(gridColumn2);
            this.sdgvProjectPerformance.PrimaryGrid.DefaultRowHeight = 70;
            this.sdgvProjectPerformance.PrimaryGrid.GridLines = DevComponents.DotNetBar.SuperGrid.GridLines.Horizontal;
            this.sdgvProjectPerformance.PrimaryGrid.Header.RowHeight = 70;
            this.sdgvProjectPerformance.PrimaryGrid.Header.Text = "<div align = \'Right\'><font size=\'15\'>Project Position</font><br/>You are <b>12</b" +
    ">th out of <b>14</b><b></b></div>";
            this.sdgvProjectPerformance.PrimaryGrid.ReadOnly = true;
            this.sdgvProjectPerformance.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.sdgvProjectPerformance.PrimaryGrid.ShowColumnHeader = false;
            this.sdgvProjectPerformance.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvProjectPerformance.Size = new System.Drawing.Size(944, 515);
            this.sdgvProjectPerformance.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1 (5).jpg");
            this.imageList1.Images.SetKeyName(1, "1 (6).jpg");
            this.imageList1.Images.SetKeyName(2, "1 (7).jpg");
            this.imageList1.Images.SetKeyName(3, "1 (8) - Copy.jpg");
            this.imageList1.Images.SetKeyName(4, "1 (8).jpg");
            this.imageList1.Images.SetKeyName(5, "3759007_orig.jpg");
            // 
            // sdgvOverallPerformance
            // 
            this.sdgvOverallPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvOverallPerformance.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvOverallPerformance.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvOverallPerformance.Location = new System.Drawing.Point(0, 0);
            this.sdgvOverallPerformance.Name = "sdgvOverallPerformance";
            this.sdgvOverallPerformance.PrimaryGrid.AllowRowResize = true;
            this.sdgvOverallPerformance.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvOverallPerformance.PrimaryGrid.ColumnHeader.Visible = false;
            gridColumn3.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.None;
            gridColumn3.CellHighlightMode = DevComponents.DotNetBar.SuperGrid.Style.CellHighlightMode.Content;
            gridColumn3.CellStyles.Default.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            background3.Color1 = System.Drawing.Color.White;
            background3.Color2 = System.Drawing.Color.LightSkyBlue;
            gridColumn3.CellStyles.MouseOver.Background = background3;
            gridColumn3.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridImageEditControl);
            gridColumn3.FillWeight = 25;
            gridColumn3.Name = "Photo";
            gridColumn3.ResizeMode = DevComponents.DotNetBar.SuperGrid.ColumnResizeMode.None;
            gridColumn3.ToolTip = "Image";
            gridColumn3.Width = 115;
            gridColumn4.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.None;
            background4.Color1 = System.Drawing.Color.White;
            background4.Color2 = System.Drawing.Color.LightSkyBlue;
            gridColumn4.CellStyles.MouseOver.Background = background4;
            gridColumn4.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn4.EnableHeaderMarkup = true;
            gridColumn4.FillWeight = 70;
            gridColumn4.MinimumWidth = 100;
            gridColumn4.Name = "Information";
            gridColumn4.ResizeMode = DevComponents.DotNetBar.SuperGrid.ColumnResizeMode.MaintainTotalWidth;
            gridColumn4.Width = 150;
            this.sdgvOverallPerformance.PrimaryGrid.Columns.Add(gridColumn3);
            this.sdgvOverallPerformance.PrimaryGrid.Columns.Add(gridColumn4);
            this.sdgvOverallPerformance.PrimaryGrid.DefaultRowHeight = 70;
            this.sdgvOverallPerformance.PrimaryGrid.GridLines = DevComponents.DotNetBar.SuperGrid.GridLines.Horizontal;
            this.sdgvOverallPerformance.PrimaryGrid.Header.RowHeight = 70;
            this.sdgvOverallPerformance.PrimaryGrid.Header.Text = "<div align = \'Right\'>Month to Date</div>";
            this.sdgvOverallPerformance.PrimaryGrid.ReadOnly = true;
            this.sdgvOverallPerformance.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.sdgvOverallPerformance.PrimaryGrid.ShowColumnHeader = false;
            this.sdgvOverallPerformance.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvOverallPerformance.Size = new System.Drawing.Size(869, 515);
            this.sdgvOverallPerformance.TabIndex = 7;
            this.sdgvOverallPerformance.Text = "superGridControl2";
            // 
            // lblEmpID
            // 
            this.lblEmpID.AutoSize = true;
            // 
            // 
            // 
            this.lblEmpID.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblEmpID.Font = new System.Drawing.Font("Arial", 11F);
            this.lblEmpID.Location = new System.Drawing.Point(213, 71);
            this.lblEmpID.Name = "lblEmpID";
            this.lblEmpID.Size = new System.Drawing.Size(51, 19);
            this.lblEmpID.TabIndex = 17;
            this.lblEmpID.Text = "600950";
            // 
            // lblResearchType
            // 
            this.lblResearchType.AutoSize = true;
            // 
            // 
            // 
            this.lblResearchType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblResearchType.Font = new System.Drawing.Font("Arial", 11F);
            this.lblResearchType.Location = new System.Drawing.Point(213, 143);
            this.lblResearchType.Name = "lblResearchType";
            this.lblResearchType.Size = new System.Drawing.Size(39, 19);
            this.lblResearchType.TabIndex = 15;
            this.lblResearchType.Text = "Voice";
            // 
            // lblTenure
            // 
            this.lblTenure.AutoSize = true;
            // 
            // 
            // 
            this.lblTenure.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTenure.Font = new System.Drawing.Font("Arial", 11F);
            this.lblTenure.Location = new System.Drawing.Point(213, 107);
            this.lblTenure.Name = "lblTenure";
            this.lblTenure.Size = new System.Drawing.Size(53, 19);
            this.lblTenure.TabIndex = 14;
            this.lblTenure.Text = "3 Years";
            // 
            // lblAgentName
            // 
            this.lblAgentName.AutoSize = true;
            // 
            // 
            // 
            this.lblAgentName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblAgentName.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAgentName.Location = new System.Drawing.Point(213, 7);
            this.lblAgentName.Name = "lblAgentName";
            this.lblAgentName.Size = new System.Drawing.Size(51, 35);
            this.lblAgentName.TabIndex = 13;
            this.lblAgentName.Text = "Tha";
            // 
            // lblUpload
            // 
            this.lblUpload.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.lblUpload.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpload.Location = new System.Drawing.Point(11, 144);
            this.lblUpload.Name = "lblUpload";
            this.lblUpload.Size = new System.Drawing.Size(138, 33);
            this.lblUpload.TabIndex = 16;
            this.lblUpload.Text = "Upload Image";
            this.lblUpload.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblCurrentProject
            // 
            this.lblCurrentProject.AutoSize = true;
            // 
            // 
            // 
            this.lblCurrentProject.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblCurrentProject.Font = new System.Drawing.Font("Arial", 11F);
            this.lblCurrentProject.Location = new System.Drawing.Point(731, 71);
            this.lblCurrentProject.Name = "lblCurrentProject";
            this.lblCurrentProject.Size = new System.Drawing.Size(79, 19);
            this.lblCurrentProject.TabIndex = 20;
            this.lblCurrentProject.Text = "Project Red";
            // 
            // btnTarget
            // 
            this.btnTarget.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTarget.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.btnTarget.Location = new System.Drawing.Point(1749, 139);
            this.btnTarget.Name = "btnTarget";
            this.btnTarget.Size = new System.Drawing.Size(62, 23);
            this.btnTarget.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnTarget.TabIndex = 15;
            this.btnTarget.Text = "&Set Target";
            this.btnTarget.Click += new System.EventHandler(this.btnTarget_Click);
            // 
            // lblUserType
            // 
            this.lblUserType.AutoSize = true;
            // 
            // 
            // 
            this.lblUserType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblUserType.Font = new System.Drawing.Font("Arial", 11F);
            this.lblUserType.Location = new System.Drawing.Point(731, 107);
            this.lblUserType.Name = "lblUserType";
            this.lblUserType.Size = new System.Drawing.Size(41, 19);
            this.lblUserType.TabIndex = 19;
            this.lblUserType.Text = "Agent";
            // 
            // txtTarget
            // 
            this.txtTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtTarget.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtTarget.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTarget.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtTarget.Location = new System.Drawing.Point(1670, 142);
            this.txtTarget.MinValue = 1;
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.ShowUpDown = true;
            this.txtTarget.Size = new System.Drawing.Size(62, 20);
            this.txtTarget.TabIndex = 14;
            this.txtTarget.Value = 1;
            // 
            // lblTodaysTarget
            // 
            this.lblTodaysTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTodaysTarget.AutoSize = true;
            // 
            // 
            // 
            this.lblTodaysTarget.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTodaysTarget.Font = new System.Drawing.Font("Arial", 11F);
            this.lblTodaysTarget.Location = new System.Drawing.Point(1561, 143);
            this.lblTodaysTarget.Name = "lblTodaysTarget";
            this.lblTodaysTarget.Size = new System.Drawing.Size(100, 19);
            this.lblTodaysTarget.TabIndex = 13;
            this.lblTodaysTarget.Text = "Today\'s Target";
            // 
            // pictureDP
            // 
            this.pictureDP.Image = ((System.Drawing.Image)(resources.GetObject("pictureDP.Image")));
            this.pictureDP.Location = new System.Drawing.Point(7, 7);
            this.pictureDP.Name = "pictureDP";
            this.pictureDP.Size = new System.Drawing.Size(142, 160);
            this.pictureDP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureDP.TabIndex = 11;
            this.pictureDP.TabStop = false;
            this.pictureDP.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureDP_MouseDown);
            this.pictureDP.MouseEnter += new System.EventHandler(this.pictureDP_MouseEnter);
            this.pictureDP.MouseLeave += new System.EventHandler(this.pictureDP_MouseLeave);
            // 
            // lblTargetMissed
            // 
            this.lblTargetMissed.AutoSize = true;
            // 
            // 
            // 
            this.lblTargetMissed.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTargetMissed.Font = new System.Drawing.Font("Arial", 11F);
            this.lblTargetMissed.Location = new System.Drawing.Point(429, 71);
            this.lblTargetMissed.Name = "lblTargetMissed";
            this.lblTargetMissed.Size = new System.Drawing.Size(95, 19);
            this.lblTargetMissed.TabIndex = 8;
            this.lblTargetMissed.Text = "Target Missed";
            // 
            // lblTargetAchived
            // 
            this.lblTargetAchived.AutoSize = true;
            // 
            // 
            // 
            this.lblTargetAchived.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTargetAchived.Font = new System.Drawing.Font("Arial", 11F);
            this.lblTargetAchived.Location = new System.Drawing.Point(429, 143);
            this.lblTargetAchived.Name = "lblTargetAchived";
            this.lblTargetAchived.Size = new System.Drawing.Size(109, 19);
            this.lblTargetAchived.TabIndex = 7;
            this.lblTargetAchived.Text = "Target Achieved";
            // 
            // lblMonthlyPoints
            // 
            this.lblMonthlyPoints.AutoSize = true;
            // 
            // 
            // 
            this.lblMonthlyPoints.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMonthlyPoints.Font = new System.Drawing.Font("Arial", 11F);
            this.lblMonthlyPoints.Location = new System.Drawing.Point(429, 107);
            this.lblMonthlyPoints.Name = "lblMonthlyPoints";
            this.lblMonthlyPoints.Size = new System.Drawing.Size(99, 19);
            this.lblMonthlyPoints.TabIndex = 2;
            this.lblMonthlyPoints.Text = "Monthly Points";
            // 
            // splitContainerGrids
            // 
            this.splitContainerGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerGrids.Location = new System.Drawing.Point(0, 0);
            this.splitContainerGrids.Name = "splitContainerGrids";
            // 
            // splitContainerGrids.Panel1
            // 
            this.splitContainerGrids.Panel1.Controls.Add(this.btnShowAllProject);
            this.splitContainerGrids.Panel1.Controls.Add(this.sdgvProjectPerformance);
            this.splitContainerGrids.Panel1MinSize = 200;
            // 
            // splitContainerGrids.Panel2
            // 
            this.splitContainerGrids.Panel2.Controls.Add(this.btnShowAllMTD);
            this.splitContainerGrids.Panel2.Controls.Add(this.sdgvOverallPerformance);
            this.splitContainerGrids.Panel2MinSize = 100;
            this.splitContainerGrids.Size = new System.Drawing.Size(1823, 515);
            this.splitContainerGrids.SplitterDistance = 944;
            this.splitContainerGrids.SplitterWidth = 10;
            this.splitContainerGrids.TabIndex = 9;
            this.splitContainerGrids.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerGrids_SplitterMoved);
            // 
            // btnShowAllProject
            // 
            this.btnShowAllProject.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnShowAllProject.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnShowAllProject.FocusCuesEnabled = false;
            this.btnShowAllProject.Location = new System.Drawing.Point(12, 23);
            this.btnShowAllProject.Name = "btnShowAllProject";
            this.btnShowAllProject.Size = new System.Drawing.Size(75, 23);
            this.btnShowAllProject.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnShowAllProject.TabIndex = 9;
            this.btnShowAllProject.Text = "Show All";
            this.btnShowAllProject.Click += new System.EventHandler(this.btnShowAllProject_Click);
            // 
            // btnShowAllMTD
            // 
            this.btnShowAllMTD.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnShowAllMTD.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnShowAllMTD.FocusCuesEnabled = false;
            this.btnShowAllMTD.Location = new System.Drawing.Point(17, 23);
            this.btnShowAllMTD.Name = "btnShowAllMTD";
            this.btnShowAllMTD.Size = new System.Drawing.Size(75, 23);
            this.btnShowAllMTD.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnShowAllMTD.TabIndex = 8;
            this.btnShowAllMTD.Text = "Show All";
            this.btnShowAllMTD.Click += new System.EventHandler(this.btnShowAllMTD_Click);
            // 
            // splitContainerInformation
            // 
            this.splitContainerInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerInformation.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerInformation.Location = new System.Drawing.Point(0, 0);
            this.splitContainerInformation.Name = "splitContainerInformation";
            this.splitContainerInformation.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerInformation.Panel1
            // 
            this.splitContainerInformation.Panel1.Controls.Add(this.splitContainerGrids);
            this.splitContainerInformation.Panel1MinSize = 200;
            // 
            // splitContainerInformation.Panel2
            // 
            this.splitContainerInformation.Panel2.AutoScroll = true;
            this.splitContainerInformation.Panel2.Controls.Add(this.lblUpload);
            this.splitContainerInformation.Panel2.Controls.Add(this.lblCurrentProject);
            this.splitContainerInformation.Panel2.Controls.Add(this.pictureDP);
            this.splitContainerInformation.Panel2.Controls.Add(this.lblAgentName);
            this.splitContainerInformation.Panel2.Controls.Add(this.btnTarget);
            this.splitContainerInformation.Panel2.Controls.Add(this.lblTenure);
            this.splitContainerInformation.Panel2.Controls.Add(this.lblMonthlyPoints);
            this.splitContainerInformation.Panel2.Controls.Add(this.lblTodaysTarget);
            this.splitContainerInformation.Panel2.Controls.Add(this.lblUserType);
            this.splitContainerInformation.Panel2.Controls.Add(this.lblResearchType);
            this.splitContainerInformation.Panel2.Controls.Add(this.lblTargetAchived);
            this.splitContainerInformation.Panel2.Controls.Add(this.lblEmpID);
            this.splitContainerInformation.Panel2.Controls.Add(this.txtTarget);
            this.splitContainerInformation.Panel2.Controls.Add(this.lblTargetMissed);
            this.splitContainerInformation.Size = new System.Drawing.Size(1823, 702);
            this.splitContainerInformation.SplitterDistance = 515;
            this.splitContainerInformation.SplitterWidth = 10;
            this.splitContainerInformation.TabIndex = 10;
            this.splitContainerInformation.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerInformation_SplitterMoved);
            // 
            // frmTeamPerformance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1823, 702);
            this.Controls.Add(this.splitContainerInformation);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.Name = "frmTeamPerformance";
            this.Text = "TeamPerformance";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.TeamPerformance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDP)).EndInit();
            this.splitContainerGrids.Panel1.ResumeLayout(false);
            this.splitContainerGrids.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGrids)).EndInit();
            this.splitContainerGrids.ResumeLayout(false);
            this.splitContainerInformation.Panel1.ResumeLayout(false);
            this.splitContainerInformation.Panel2.ResumeLayout(false);
            this.splitContainerInformation.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInformation)).EndInit();
            this.splitContainerInformation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvProjectPerformance;
        private System.Windows.Forms.ImageList imageList1;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvOverallPerformance;
        private DevComponents.DotNetBar.LabelX lblResearchType;
        private DevComponents.DotNetBar.LabelX lblTenure;
        private DevComponents.DotNetBar.LabelX lblAgentName;
        private DevComponents.DotNetBar.LabelX lblEmpID;
        private DevComponents.DotNetBar.LabelX lblUserType;
        private DevComponents.DotNetBar.LabelX lblCurrentProject;
        private DevComponents.DotNetBar.LabelX lblUpload;
        private DevComponents.DotNetBar.ButtonX btnTarget;
        private DevComponents.Editors.IntegerInput txtTarget;
        private DevComponents.DotNetBar.LabelX lblTodaysTarget;
        private System.Windows.Forms.PictureBox pictureDP;
        private DevComponents.DotNetBar.LabelX lblTargetMissed;
        private DevComponents.DotNetBar.LabelX lblTargetAchived;
        private DevComponents.DotNetBar.LabelX lblMonthlyPoints;
        private System.Windows.Forms.SplitContainer splitContainerGrids;
        private System.Windows.Forms.SplitContainer splitContainerInformation;
        private DevComponents.DotNetBar.ButtonX btnShowAllMTD;
        private DevComponents.DotNetBar.ButtonX btnShowAllProject;
    }
}