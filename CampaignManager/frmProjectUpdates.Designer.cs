namespace GCC
{
    partial class frmProjectUpdates
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
            this.advTreeProjectUpdates = new DevComponents.AdvTree.AdvTree();
            this.columnHeader4 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader5 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader6 = new DevComponents.AdvTree.ColumnHeader();
            this.nodeProject = new DevComponents.AdvTree.Node();
            this.nodeProjectInfo = new DevComponents.AdvTree.Node();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.nodeProjectUpdate = new DevComponents.AdvTree.Node();
            this.nodeProjectNotification = new DevComponents.AdvTree.Node();
            this.nodeCampaignManager = new DevComponents.AdvTree.Node();
            this.nodeCMRelease = new DevComponents.AdvTree.Node();
            this.nodeCMBugFixes = new DevComponents.AdvTree.Node();
            this.nodeCMNotification = new DevComponents.AdvTree.Node();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.BoldStyle = new DevComponents.DotNetBar.ElementStyle();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.wBrowser = new System.Windows.Forms.WebBrowser();
            this.panelExInstructionHeader = new DevComponents.DotNetBar.PanelEx();
            this.btnAknowledge = new DevComponents.DotNetBar.ButtonX();
            this.lblFrom = new DevComponents.DotNetBar.LabelX();
            this.columnHeader1 = new DevComponents.AdvTree.ColumnHeader();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeProjectUpdates)).BeginInit();
            this.panelEx1.SuspendLayout();
            this.panelEx2.SuspendLayout();
            this.panelExInstructionHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // advTreeProjectUpdates
            // 
            this.advTreeProjectUpdates.AllowDrop = true;
            this.advTreeProjectUpdates.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeProjectUpdates.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeProjectUpdates.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTreeProjectUpdates.Columns.Add(this.columnHeader4);
            this.advTreeProjectUpdates.Columns.Add(this.columnHeader5);
            this.advTreeProjectUpdates.Columns.Add(this.columnHeader6);
            this.advTreeProjectUpdates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTreeProjectUpdates.ExpandButtonType = DevComponents.AdvTree.eExpandButtonType.Triangle;
            this.advTreeProjectUpdates.ExpandWidth = 14;
            this.advTreeProjectUpdates.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.advTreeProjectUpdates.Location = new System.Drawing.Point(0, 0);
            this.advTreeProjectUpdates.Name = "advTreeProjectUpdates";
            this.advTreeProjectUpdates.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.nodeProject,
            this.nodeCampaignManager});
            this.advTreeProjectUpdates.NodeStyle = this.elementStyle2;
            this.advTreeProjectUpdates.PathSeparator = ";";
            this.advTreeProjectUpdates.SelectionBoxStyle = DevComponents.AdvTree.eSelectionStyle.FullRowSelect;
            this.advTreeProjectUpdates.Size = new System.Drawing.Size(913, 210);
            this.advTreeProjectUpdates.Styles.Add(this.elementStyle2);
            this.advTreeProjectUpdates.Styles.Add(this.elementStyle1);
            this.advTreeProjectUpdates.Styles.Add(this.BoldStyle);
            this.advTreeProjectUpdates.TabIndex = 1;
            this.advTreeProjectUpdates.Text = "advTree2";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Name = "columnHeader4";
            this.columnHeader4.Text = "From";
            this.columnHeader4.Width.Absolute = 200;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Name = "columnHeader5";
            this.columnHeader5.Text = "Subject";
            this.columnHeader5.Width.Absolute = 512;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Name = "columnHeader6";
            this.columnHeader6.Text = "Received";
            this.columnHeader6.Width.Absolute = 150;
            // 
            // nodeProject
            // 
            this.nodeProject.Expanded = true;
            this.nodeProject.Name = "nodeProject";
            this.nodeProject.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.nodeProjectInfo,
            this.nodeProjectUpdate,
            this.nodeProjectNotification});
            this.nodeProject.Text = "Project";
            // 
            // nodeProjectInfo
            // 
            this.nodeProjectInfo.Expanded = true;
            this.nodeProjectInfo.Name = "nodeProjectInfo";
            this.nodeProjectInfo.NodesColumns.Add(this.columnHeader1);
            this.nodeProjectInfo.Style = this.elementStyle1;
            this.nodeProjectInfo.Text = "Information <font color=\"Red\"><b>(1)</b></font>";
            // 
            // elementStyle1
            // 
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            // 
            // nodeProjectUpdate
            // 
            this.nodeProjectUpdate.Expanded = true;
            this.nodeProjectUpdate.Name = "nodeProjectUpdate";
            this.nodeProjectUpdate.Text = "Updates";
            // 
            // nodeProjectNotification
            // 
            this.nodeProjectNotification.Expanded = true;
            this.nodeProjectNotification.Name = "nodeProjectNotification";
            this.nodeProjectNotification.Text = "Notification";
            // 
            // nodeCampaignManager
            // 
            this.nodeCampaignManager.Expanded = true;
            this.nodeCampaignManager.Name = "nodeCampaignManager";
            this.nodeCampaignManager.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.nodeCMRelease,
            this.nodeCMBugFixes,
            this.nodeCMNotification});
            this.nodeCampaignManager.Text = "Campaign Manager";
            // 
            // nodeCMRelease
            // 
            this.nodeCMRelease.Name = "nodeCMRelease";
            this.nodeCMRelease.Text = "Release";
            // 
            // nodeCMBugFixes
            // 
            this.nodeCMBugFixes.Name = "nodeCMBugFixes";
            this.nodeCMBugFixes.Text = "Bug Fixes";
            // 
            // nodeCMNotification
            // 
            this.nodeCMNotification.Name = "nodeCMNotification";
            this.nodeCMNotification.Text = "Notification";
            // 
            // elementStyle2
            // 
            this.elementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle2.Name = "elementStyle2";
            this.elementStyle2.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // BoldStyle
            // 
            this.BoldStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.BoldStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoldStyle.Name = "BoldStyle";
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.advTreeProjectUpdates);
            this.panelEx1.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(913, 210);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 2;
            this.panelEx1.Text = "panelEx1";
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.Location = new System.Drawing.Point(0, 210);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(913, 6);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 3;
            this.expandableSplitter1.TabStop = false;
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.wBrowser);
            this.panelEx2.Controls.Add(this.panelExInstructionHeader);
            this.panelEx2.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(0, 216);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(913, 341);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 4;
            this.panelEx2.Text = "panelEx2";
            // 
            // wBrowser
            // 
            this.wBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wBrowser.IsWebBrowserContextMenuEnabled = false;
            this.wBrowser.Location = new System.Drawing.Point(0, 38);
            this.wBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.wBrowser.Name = "wBrowser";
            this.wBrowser.Size = new System.Drawing.Size(913, 303);
            this.wBrowser.TabIndex = 0;
            this.wBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // panelExInstructionHeader
            // 
            this.panelExInstructionHeader.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelExInstructionHeader.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelExInstructionHeader.Controls.Add(this.btnAknowledge);
            this.panelExInstructionHeader.Controls.Add(this.lblFrom);
            this.panelExInstructionHeader.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelExInstructionHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExInstructionHeader.Location = new System.Drawing.Point(0, 0);
            this.panelExInstructionHeader.Name = "panelExInstructionHeader";
            this.panelExInstructionHeader.Size = new System.Drawing.Size(913, 38);
            this.panelExInstructionHeader.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExInstructionHeader.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelExInstructionHeader.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelExInstructionHeader.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExInstructionHeader.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelExInstructionHeader.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExInstructionHeader.Style.GradientAngle = 90;
            this.panelExInstructionHeader.TabIndex = 1;
            // 
            // btnAknowledge
            // 
            this.btnAknowledge.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAknowledge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAknowledge.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAknowledge.Location = new System.Drawing.Point(791, 5);
            this.btnAknowledge.Name = "btnAknowledge";
            this.btnAknowledge.Size = new System.Drawing.Size(118, 28);
            this.btnAknowledge.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAknowledge.TabIndex = 1;
            this.btnAknowledge.Text = "Acknowledge Receipt";
            this.btnAknowledge.Visible = false;
            this.btnAknowledge.Click += new System.EventHandler(this.btnAknowledge_Click);
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            // 
            // 
            // 
            this.lblFrom.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblFrom.Location = new System.Drawing.Point(9, 10);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(100, 18);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "Thanga Prakash";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Name = "columnHeader1";
            this.columnHeader1.Text = "Column";
            this.columnHeader1.Width.Absolute = 150;
            // 
            // frmProjectUpdates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 557);
            this.Controls.Add(this.panelEx2);
            this.Controls.Add(this.expandableSplitter1);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.Name = "frmProjectUpdates";
            this.Text = "Project Updates";
            this.Load += new System.EventHandler(this.frmProjectUpdates_Load);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeProjectUpdates)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.panelEx2.ResumeLayout(false);
            this.panelExInstructionHeader.ResumeLayout(false);
            this.panelExInstructionHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.AdvTree.AdvTree advTreeProjectUpdates;
        private DevComponents.AdvTree.ColumnHeader columnHeader4;
        private DevComponents.AdvTree.ColumnHeader columnHeader5;
        private DevComponents.AdvTree.ColumnHeader columnHeader6;
        private DevComponents.AdvTree.Node nodeProject;
        private DevComponents.AdvTree.Node nodeProjectInfo;
        private DevComponents.AdvTree.Node nodeProjectUpdate;
        private DevComponents.AdvTree.Node nodeProjectNotification;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        private DevComponents.AdvTree.Node nodeCampaignManager;
        private DevComponents.AdvTree.Node nodeCMRelease;
        private DevComponents.AdvTree.Node nodeCMBugFixes;
        private DevComponents.AdvTree.Node nodeCMNotification;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private System.Windows.Forms.WebBrowser wBrowser;
        private DevComponents.DotNetBar.ElementStyle BoldStyle;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.PanelEx panelExInstructionHeader;
        private DevComponents.DotNetBar.LabelX lblFrom;
        private DevComponents.DotNetBar.ButtonX btnAknowledge;
        private DevComponents.AdvTree.ColumnHeader columnHeader1;
    }
}