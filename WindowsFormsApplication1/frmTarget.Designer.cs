namespace GCC
{
    partial class frmTarget
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel1 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel2 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel3 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTarget));
            this.chartBarTarget = new DevExpress.XtraCharts.ChartControl();
            this.panelAgent = new DevComponents.DotNetBar.PanelEx();
            this.lblEmpName = new System.Windows.Forms.Label();
            this.lblUpload = new DevComponents.DotNetBar.LabelX();
            this.btnTarget = new DevComponents.DotNetBar.ButtonX();
            this.txtTarget = new DevComponents.Editors.IntegerInput();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.pictureDP = new System.Windows.Forms.PictureBox();
            this.lblTargetMissed = new DevComponents.DotNetBar.LabelX();
            this.lblTargetAchived = new DevComponents.DotNetBar.LabelX();
            this.lblMonthlyPoints = new DevComponents.DotNetBar.LabelX();
            this.lblPanelHeader = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.chartBarTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel3)).BeginInit();
            this.panelAgent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDP)).BeginInit();
            this.SuspendLayout();
            // 
            // chartBarTarget
            // 
            this.chartBarTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartBarTarget.BackColor = System.Drawing.Color.Transparent;
            xyDiagram1.AxisX.Range.ScrollingRange.SideMarginsEnabled = true;
            xyDiagram1.AxisX.Range.SideMarginsEnabled = true;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.Range.ScrollingRange.SideMarginsEnabled = true;
            xyDiagram1.AxisY.Range.SideMarginsEnabled = true;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.chartBarTarget.Diagram = xyDiagram1;
            this.chartBarTarget.Location = new System.Drawing.Point(31, 43);
            this.chartBarTarget.Name = "chartBarTarget";
            sideBySideBarSeriesLabel1.LineVisible = true;
            series1.Label = sideBySideBarSeriesLabel1;
            series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series1.Name = "Target";
            pointSeriesLabel1.LineVisible = true;
            series2.Label = pointSeriesLabel1;
            series2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series2.Name = "Team Avg";
            series2.View = lineSeriesView1;
            sideBySideBarSeriesLabel2.LineVisible = true;
            series3.Label = sideBySideBarSeriesLabel2;
            series3.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series3.Name = "Achieved";
            this.chartBarTarget.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2,
        series3};
            sideBySideBarSeriesLabel3.LineVisible = true;
            this.chartBarTarget.SeriesTemplate.Label = sideBySideBarSeriesLabel3;
            this.chartBarTarget.Size = new System.Drawing.Size(731, 478);
            this.chartBarTarget.TabIndex = 0;
            // 
            // panelAgent
            // 
            this.panelAgent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAgent.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelAgent.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelAgent.Controls.Add(this.lblEmpName);
            this.panelAgent.Controls.Add(this.lblUpload);
            this.panelAgent.Controls.Add(this.btnTarget);
            this.panelAgent.Controls.Add(this.txtTarget);
            this.panelAgent.Controls.Add(this.labelX1);
            this.panelAgent.Controls.Add(this.pictureDP);
            this.panelAgent.Controls.Add(this.lblTargetMissed);
            this.panelAgent.Controls.Add(this.lblTargetAchived);
            this.panelAgent.Controls.Add(this.lblMonthlyPoints);
            this.panelAgent.Controls.Add(this.lblPanelHeader);
            this.panelAgent.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelAgent.Location = new System.Drawing.Point(768, 43);
            this.panelAgent.Name = "panelAgent";
            this.panelAgent.Size = new System.Drawing.Size(287, 478);
            this.panelAgent.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelAgent.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelAgent.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelAgent.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelAgent.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelAgent.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelAgent.Style.GradientAngle = 90;
            this.panelAgent.TabIndex = 1;
            // 
            // lblEmpName
            // 
            this.lblEmpName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblEmpName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblEmpName.Location = new System.Drawing.Point(0, 455);
            this.lblEmpName.Name = "lblEmpName";
            this.lblEmpName.Size = new System.Drawing.Size(287, 23);
            this.lblEmpName.TabIndex = 17;
            this.lblEmpName.Text = "Employee Name";
            this.lblEmpName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUpload
            // 
            this.lblUpload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUpload.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.lblUpload.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpload.Location = new System.Drawing.Point(32, 406);
            this.lblUpload.Name = "lblUpload";
            this.lblUpload.Size = new System.Drawing.Size(223, 33);
            this.lblUpload.TabIndex = 16;
            this.lblUpload.Text = "Upload Image";
            this.lblUpload.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // btnTarget
            // 
            this.btnTarget.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTarget.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.btnTarget.Location = new System.Drawing.Point(193, 162);
            this.btnTarget.Name = "btnTarget";
            this.btnTarget.Size = new System.Drawing.Size(62, 23);
            this.btnTarget.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnTarget.TabIndex = 15;
            this.btnTarget.Text = "&Set Target";
            this.btnTarget.Click += new System.EventHandler(this.btnTarget_Click);
            // 
            // txtTarget
            // 
            // 
            // 
            // 
            this.txtTarget.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtTarget.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTarget.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtTarget.Location = new System.Drawing.Point(114, 162);
            this.txtTarget.MinValue = 1;
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.ShowUpDown = true;
            this.txtTarget.Size = new System.Drawing.Size(62, 20);
            this.txtTarget.TabIndex = 14;
            this.txtTarget.Value = 1;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.labelX1.Location = new System.Drawing.Point(12, 162);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(91, 18);
            this.labelX1.TabIndex = 13;
            this.labelX1.Text = "Today\'s Target";
            // 
            // pictureDP
            // 
            this.pictureDP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureDP.Image = ((System.Drawing.Image)(resources.GetObject("pictureDP.Image")));
            this.pictureDP.Location = new System.Drawing.Point(32, 247);
            this.pictureDP.Name = "pictureDP";
            this.pictureDP.Size = new System.Drawing.Size(223, 192);
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
            this.lblTargetMissed.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblTargetMissed.Location = new System.Drawing.Point(13, 124);
            this.lblTargetMissed.Name = "lblTargetMissed";
            this.lblTargetMissed.Size = new System.Drawing.Size(17, 18);
            this.lblTargetMissed.TabIndex = 10;
            this.lblTargetMissed.Text = "03";
            // 
            // lblTargetAchived
            // 
            this.lblTargetAchived.AutoSize = true;
            // 
            // 
            // 
            this.lblTargetAchived.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTargetAchived.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblTargetAchived.Location = new System.Drawing.Point(13, 89);
            this.lblTargetAchived.Name = "lblTargetAchived";
            this.lblTargetAchived.Size = new System.Drawing.Size(17, 18);
            this.lblTargetAchived.TabIndex = 9;
            this.lblTargetAchived.Text = "15";
            // 
            // lblMonthlyPoints
            // 
            this.lblMonthlyPoints.AutoSize = true;
            // 
            // 
            // 
            this.lblMonthlyPoints.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMonthlyPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblMonthlyPoints.Location = new System.Drawing.Point(13, 54);
            this.lblMonthlyPoints.Name = "lblMonthlyPoints";
            this.lblMonthlyPoints.Size = new System.Drawing.Size(17, 18);
            this.lblMonthlyPoints.TabIndex = 6;
            this.lblMonthlyPoints.Text = "26";
            // 
            // lblPanelHeader
            // 
            this.lblPanelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPanelHeader.AutoSize = true;
            // 
            // 
            // 
            this.lblPanelHeader.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPanelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblPanelHeader.Location = new System.Drawing.Point(63, 3);
            this.lblPanelHeader.Name = "lblPanelHeader";
            this.lblPanelHeader.Size = new System.Drawing.Size(112, 19);
            this.lblPanelHeader.TabIndex = 0;
            this.lblPanelHeader.Text = "Overall Statistics";
            // 
            // frmTarget
            // 
            this.AcceptButton = this.btnTarget;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 548);
            this.Controls.Add(this.panelAgent);
            this.Controls.Add(this.chartBarTarget);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.Name = "frmTarget";
            this.Text = "Target";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmTarget_Load);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartBarTarget)).EndInit();
            this.panelAgent.ResumeLayout(false);
            this.panelAgent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDP)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartBarTarget;
        private DevComponents.DotNetBar.PanelEx panelAgent;
        private DevComponents.DotNetBar.LabelX lblPanelHeader;
        private DevComponents.DotNetBar.LabelX lblMonthlyPoints;
        private DevComponents.DotNetBar.LabelX lblTargetMissed;
        private DevComponents.DotNetBar.LabelX lblTargetAchived;
        private System.Windows.Forms.PictureBox pictureDP;
        private DevComponents.Editors.IntegerInput txtTarget;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnTarget;
        private DevComponents.DotNetBar.LabelX lblUpload;
        private System.Windows.Forms.Label lblEmpName;
    }
}