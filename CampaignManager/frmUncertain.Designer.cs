namespace GCC
{
    partial class frmUncertain
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
            this.xGridUncertain = new DevExpress.XtraGrid.GridControl();
            this.lView = new DevExpress.XtraGrid.Views.Layout.LayoutView();
            this.colUncertain_Field = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_layoutViewColumn1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.colUncertainRecordCount = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_layoutViewColumn2 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.colUncertainAgentCount = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_layoutViewColumn3 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.colUncertainRaisedAgents = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.rMemoEditAgent_Names = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
            this.layoutViewField_layoutViewColumn5 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.colCheckStatus = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.rRadioGroup = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
            this.layoutViewField_layoutViewColumn1_1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.layoutViewCard1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewCard();
            this.Group1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.item1 = new DevExpress.XtraLayout.SimpleSeparator();
            this.item2 = new DevExpress.XtraLayout.SimpleSeparator();
            this.item3 = new DevExpress.XtraLayout.SimpleSeparator();
            this.item4 = new DevExpress.XtraLayout.SimpleSeparator();
            this.rCheckStatus = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rCheckStatus_Reject = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panelInfo = new DevComponents.DotNetBar.PanelEx();
            this.btnCaptchaCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnCaptchaGo = new DevComponents.DotNetBar.ButtonX();
            this.txtCaptchaText = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblCaptcha = new GCC.CustomLabel();
            this.lblSelectedField = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnLoadGrid = new DevComponents.DotNetBar.ButtonX();
            this.lblField = new DevComponents.DotNetBar.LabelX();
            this.cmbFieldNames = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnAccpetChecked = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.xGridUncertain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMemoEditAgent_Names)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rRadioGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn1_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Group1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.item1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.item2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.item3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.item4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rCheckStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rCheckStatus_Reject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gView)).BeginInit();
            this.panelInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // xGridUncertain
            // 
            this.xGridUncertain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xGridUncertain.Location = new System.Drawing.Point(0, 47);
            this.xGridUncertain.LookAndFeel.SkinName = "Office 2010 Blue";
            this.xGridUncertain.LookAndFeel.UseDefaultLookAndFeel = false;
            this.xGridUncertain.MainView = this.lView;
            this.xGridUncertain.Name = "xGridUncertain";
            this.xGridUncertain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rCheckStatus,
            this.rMemoEditAgent_Names,
            this.rRadioGroup,
            this.rCheckStatus_Reject});
            this.xGridUncertain.Size = new System.Drawing.Size(1405, 764);
            this.xGridUncertain.TabIndex = 0;
            this.xGridUncertain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.lView,
            this.gView});
            // 
            // lView
            // 
            this.lView.CardHorzInterval = 0;
            this.lView.CardMinSize = new System.Drawing.Size(264, 141);
            this.lView.CardVertInterval = 0;
            this.lView.Columns.AddRange(new DevExpress.XtraGrid.Columns.LayoutViewColumn[] {
            this.colUncertain_Field,
            this.colUncertainRecordCount,
            this.colUncertainAgentCount,
            this.colUncertainRaisedAgents,
            this.colCheckStatus});
            this.lView.GridControl = this.xGridUncertain;
            this.lView.Name = "lView";
            this.lView.OptionsItemText.TextToControlDistance = 1;
            this.lView.OptionsView.AllowHotTrackFields = false;
            this.lView.OptionsView.ShowCardCaption = false;
            this.lView.OptionsView.ShowCardExpandButton = false;
            this.lView.OptionsView.ShowFieldHints = false;
            this.lView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.lView.OptionsView.ShowHeaderPanel = false;
            this.lView.OptionsView.ViewMode = DevExpress.XtraGrid.Views.Layout.LayoutViewMode.MultiRow;
            this.lView.TemplateCard = this.layoutViewCard1;
            // 
            // colUncertain_Field
            // 
            this.colUncertain_Field.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.colUncertain_Field.AppearanceCell.Options.HighPriority = true;
            this.colUncertain_Field.AppearanceCell.Options.UseFont = true;
            this.colUncertain_Field.AppearanceCell.Options.UseTextOptions = true;
            this.colUncertain_Field.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colUncertain_Field.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.colUncertain_Field.FieldName = "FieldValue";
            this.colUncertain_Field.LayoutViewField = this.layoutViewField_layoutViewColumn1;
            this.colUncertain_Field.Name = "colUncertain_Field";
            this.colUncertain_Field.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colUncertain_Field.OptionsColumn.ShowCaption = false;
            this.colUncertain_Field.OptionsField.SortFilterButtonShowMode = DevExpress.XtraGrid.Views.Layout.SortFilterButtonShowMode.Nowhere;
            this.colUncertain_Field.OptionsFilter.AllowAutoFilter = false;
            this.colUncertain_Field.OptionsFilter.AllowFilter = false;
            // 
            // layoutViewField_layoutViewColumn1
            // 
            this.layoutViewField_layoutViewColumn1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutViewField_layoutViewColumn1.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.layoutViewField_layoutViewColumn1.EditorPreferredWidth = 229;
            this.layoutViewField_layoutViewColumn1.Location = new System.Drawing.Point(0, 0);
            this.layoutViewField_layoutViewColumn1.Name = "layoutViewField_layoutViewColumn1";
            this.layoutViewField_layoutViewColumn1.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutViewField_layoutViewColumn1.Size = new System.Drawing.Size(241, 28);
            this.layoutViewField_layoutViewColumn1.Spacing = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutViewField_layoutViewColumn1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutViewField_layoutViewColumn1.TextToControlDistance = 0;
            this.layoutViewField_layoutViewColumn1.TextVisible = false;
            // 
            // colUncertainRecordCount
            // 
            this.colUncertainRecordCount.Caption = "# of Records";
            this.colUncertainRecordCount.FieldName = "Record Count";
            this.colUncertainRecordCount.LayoutViewField = this.layoutViewField_layoutViewColumn2;
            this.colUncertainRecordCount.Name = "colUncertainRecordCount";
            this.colUncertainRecordCount.OptionsColumn.AllowEdit = false;
            this.colUncertainRecordCount.OptionsColumn.AllowFocus = false;
            this.colUncertainRecordCount.OptionsColumn.AllowMove = false;
            this.colUncertainRecordCount.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colUncertainRecordCount.OptionsColumn.ReadOnly = true;
            this.colUncertainRecordCount.OptionsField.SortFilterButtonShowMode = DevExpress.XtraGrid.Views.Layout.SortFilterButtonShowMode.Nowhere;
            this.colUncertainRecordCount.OptionsFilter.AllowAutoFilter = false;
            this.colUncertainRecordCount.OptionsFilter.AllowFilter = false;
            // 
            // layoutViewField_layoutViewColumn2
            // 
            this.layoutViewField_layoutViewColumn2.EditorPreferredWidth = 33;
            this.layoutViewField_layoutViewColumn2.Location = new System.Drawing.Point(0, 0);
            this.layoutViewField_layoutViewColumn2.Name = "layoutViewField_layoutViewColumn2";
            this.layoutViewField_layoutViewColumn2.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutViewField_layoutViewColumn2.Size = new System.Drawing.Size(113, 28);
            this.layoutViewField_layoutViewColumn2.Spacing = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutViewField_layoutViewColumn2.TextSize = new System.Drawing.Size(67, 13);
            // 
            // colUncertainAgentCount
            // 
            this.colUncertainAgentCount.Caption = "# of Agents";
            this.colUncertainAgentCount.FieldName = "Agent Count";
            this.colUncertainAgentCount.LayoutViewField = this.layoutViewField_layoutViewColumn3;
            this.colUncertainAgentCount.Name = "colUncertainAgentCount";
            this.colUncertainAgentCount.OptionsColumn.AllowEdit = false;
            this.colUncertainAgentCount.OptionsColumn.AllowFocus = false;
            this.colUncertainAgentCount.OptionsColumn.AllowMove = false;
            this.colUncertainAgentCount.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colUncertainAgentCount.OptionsColumn.ReadOnly = true;
            this.colUncertainAgentCount.OptionsField.SortFilterButtonShowMode = DevExpress.XtraGrid.Views.Layout.SortFilterButtonShowMode.Nowhere;
            this.colUncertainAgentCount.OptionsFilter.AllowAutoFilter = false;
            this.colUncertainAgentCount.OptionsFilter.AllowFilter = false;
            // 
            // layoutViewField_layoutViewColumn3
            // 
            this.layoutViewField_layoutViewColumn3.EditorPreferredWidth = 32;
            this.layoutViewField_layoutViewColumn3.Location = new System.Drawing.Point(115, 0);
            this.layoutViewField_layoutViewColumn3.Name = "layoutViewField_layoutViewColumn3";
            this.layoutViewField_layoutViewColumn3.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutViewField_layoutViewColumn3.Size = new System.Drawing.Size(112, 28);
            this.layoutViewField_layoutViewColumn3.Spacing = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutViewField_layoutViewColumn3.TextSize = new System.Drawing.Size(67, 13);
            // 
            // colUncertainRaisedAgents
            // 
            this.colUncertainRaisedAgents.Caption = "Agent List";
            this.colUncertainRaisedAgents.ColumnEdit = this.rMemoEditAgent_Names;
            this.colUncertainRaisedAgents.FieldName = "AgentName";
            this.colUncertainRaisedAgents.LayoutViewField = this.layoutViewField_layoutViewColumn5;
            this.colUncertainRaisedAgents.Name = "colUncertainRaisedAgents";
            this.colUncertainRaisedAgents.OptionsColumn.AllowMove = false;
            this.colUncertainRaisedAgents.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colUncertainRaisedAgents.OptionsColumn.ReadOnly = true;
            this.colUncertainRaisedAgents.OptionsField.SortFilterButtonShowMode = DevExpress.XtraGrid.Views.Layout.SortFilterButtonShowMode.Nowhere;
            this.colUncertainRaisedAgents.OptionsFilter.AllowAutoFilter = false;
            this.colUncertainRaisedAgents.OptionsFilter.AllowFilter = false;
            // 
            // rMemoEditAgent_Names
            // 
            this.rMemoEditAgent_Names.AutoHeight = false;
            this.rMemoEditAgent_Names.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rMemoEditAgent_Names.Name = "rMemoEditAgent_Names";
            this.rMemoEditAgent_Names.ReadOnly = true;
            this.rMemoEditAgent_Names.ShowIcon = false;
            // 
            // layoutViewField_layoutViewColumn5
            // 
            this.layoutViewField_layoutViewColumn5.EditorPreferredWidth = 153;
            this.layoutViewField_layoutViewColumn5.Location = new System.Drawing.Point(0, 30);
            this.layoutViewField_layoutViewColumn5.Name = "layoutViewField_layoutViewColumn5";
            this.layoutViewField_layoutViewColumn5.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutViewField_layoutViewColumn5.Size = new System.Drawing.Size(227, 28);
            this.layoutViewField_layoutViewColumn5.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 3, 3);
            this.layoutViewField_layoutViewColumn5.TextSize = new System.Drawing.Size(67, 13);
            // 
            // colCheckStatus
            // 
            this.colCheckStatus.Caption = "Status";
            this.colCheckStatus.ColumnEdit = this.rRadioGroup;
            this.colCheckStatus.FieldName = "Status";
            this.colCheckStatus.LayoutViewField = this.layoutViewField_layoutViewColumn1_1;
            this.colCheckStatus.Name = "colCheckStatus";
            this.colCheckStatus.OptionsColumn.AllowMove = false;
            this.colCheckStatus.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colCheckStatus.OptionsField.SortFilterButtonShowMode = DevExpress.XtraGrid.Views.Layout.SortFilterButtonShowMode.Nowhere;
            this.colCheckStatus.OptionsFilter.AllowAutoFilter = false;
            this.colCheckStatus.OptionsFilter.AllowFilter = false;
            // 
            // rRadioGroup
            // 
            this.rRadioGroup.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(true, "Accept"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(false, "Reject")});
            this.rRadioGroup.Name = "rRadioGroup";
            // 
            // layoutViewField_layoutViewColumn1_1
            // 
            this.layoutViewField_layoutViewColumn1_1.EditorPreferredWidth = 155;
            this.layoutViewField_layoutViewColumn1_1.Location = new System.Drawing.Point(0, 60);
            this.layoutViewField_layoutViewColumn1_1.Name = "layoutViewField_layoutViewColumn1_1";
            this.layoutViewField_layoutViewColumn1_1.Size = new System.Drawing.Size(227, 26);
            this.layoutViewField_layoutViewColumn1_1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 3, 3);
            this.layoutViewField_layoutViewColumn1_1.TextSize = new System.Drawing.Size(67, 13);
            // 
            // layoutViewCard1
            // 
            this.layoutViewCard1.CustomizationFormText = "TemplateCard";
            this.layoutViewCard1.ExpandButtonLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.layoutViewCard1.GroupBordersVisible = false;
            this.layoutViewCard1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutViewField_layoutViewColumn1,
            this.Group1,
            this.item4});
            this.layoutViewCard1.Name = "layoutViewCard1";
            this.layoutViewCard1.OptionsItemText.TextToControlDistance = 1;
            this.layoutViewCard1.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutViewCard1.Text = "TemplateCard";
            // 
            // Group1
            // 
            this.Group1.CustomizationFormText = "Group1";
            this.Group1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutViewField_layoutViewColumn2,
            this.layoutViewField_layoutViewColumn5,
            this.layoutViewField_layoutViewColumn3,
            this.item1,
            this.item2,
            this.layoutViewField_layoutViewColumn1_1,
            this.item3});
            this.Group1.Location = new System.Drawing.Point(0, 28);
            this.Group1.Name = "Group1";
            this.Group1.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.Group1.Size = new System.Drawing.Size(241, 119);
            this.Group1.Spacing = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.Group1.Text = "Info";
            // 
            // item1
            // 
            this.item1.AllowHotTrack = false;
            this.item1.CustomizationFormText = "item1";
            this.item1.Location = new System.Drawing.Point(113, 0);
            this.item1.Name = "item1";
            this.item1.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.item1.Size = new System.Drawing.Size(2, 28);
            this.item1.Spacing = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.item1.Text = "item1";
            // 
            // item2
            // 
            this.item2.AllowHotTrack = false;
            this.item2.CustomizationFormText = "item2";
            this.item2.Location = new System.Drawing.Point(0, 28);
            this.item2.Name = "item2";
            this.item2.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.item2.Size = new System.Drawing.Size(227, 2);
            this.item2.Spacing = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.item2.Text = "item2";
            // 
            // item3
            // 
            this.item3.AllowHotTrack = false;
            this.item3.CustomizationFormText = "item3";
            this.item3.Location = new System.Drawing.Point(0, 58);
            this.item3.Name = "item3";
            this.item3.Size = new System.Drawing.Size(227, 2);
            this.item3.Text = "item3";
            // 
            // item4
            // 
            this.item4.AllowHotTrack = false;
            this.item4.CustomizationFormText = "item4";
            this.item4.Location = new System.Drawing.Point(0, 147);
            this.item4.Name = "item4";
            this.item4.Size = new System.Drawing.Size(241, 2);
            this.item4.Text = "item4";
            // 
            // rCheckStatus
            // 
            this.rCheckStatus.AutoHeight = false;
            this.rCheckStatus.Name = "rCheckStatus";
            // 
            // rCheckStatus_Reject
            // 
            this.rCheckStatus_Reject.AutoHeight = false;
            this.rCheckStatus_Reject.Name = "rCheckStatus_Reject";
            // 
            // gView
            // 
            this.gView.GridControl = this.xGridUncertain;
            this.gView.Name = "gView";
            // 
            // panelInfo
            // 
            this.panelInfo.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelInfo.Controls.Add(this.btnCaptchaCancel);
            this.panelInfo.Controls.Add(this.btnCaptchaGo);
            this.panelInfo.Controls.Add(this.txtCaptchaText);
            this.panelInfo.Controls.Add(this.lblCaptcha);
            this.panelInfo.Controls.Add(this.lblSelectedField);
            this.panelInfo.Controls.Add(this.labelX1);
            this.panelInfo.Controls.Add(this.btnLoadGrid);
            this.panelInfo.Controls.Add(this.lblField);
            this.panelInfo.Controls.Add(this.cmbFieldNames);
            this.panelInfo.Controls.Add(this.btnAccpetChecked);
            this.panelInfo.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInfo.Location = new System.Drawing.Point(0, 0);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(1405, 47);
            this.panelInfo.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelInfo.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelInfo.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelInfo.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelInfo.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelInfo.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelInfo.Style.GradientAngle = 90;
            this.panelInfo.TabIndex = 1;
            // 
            // btnCaptchaCancel
            // 
            this.btnCaptchaCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCaptchaCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCaptchaCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCaptchaCancel.Location = new System.Drawing.Point(1347, 11);
            this.btnCaptchaCancel.Name = "btnCaptchaCancel";
            this.btnCaptchaCancel.Size = new System.Drawing.Size(44, 23);
            this.btnCaptchaCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCaptchaCancel.TabIndex = 10;
            this.btnCaptchaCancel.Text = "&Cancel";
            this.btnCaptchaCancel.Visible = false;
            this.btnCaptchaCancel.Click += new System.EventHandler(this.btnCaptchaCancel_Click);
            // 
            // btnCaptchaGo
            // 
            this.btnCaptchaGo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCaptchaGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCaptchaGo.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCaptchaGo.Location = new System.Drawing.Point(1295, 11);
            this.btnCaptchaGo.Name = "btnCaptchaGo";
            this.btnCaptchaGo.Size = new System.Drawing.Size(47, 23);
            this.btnCaptchaGo.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCaptchaGo.TabIndex = 9;
            this.btnCaptchaGo.Text = "&Go";
            this.btnCaptchaGo.Visible = false;
            this.btnCaptchaGo.Click += new System.EventHandler(this.btnCaptchaGo_Click);
            // 
            // txtCaptchaText
            // 
            this.txtCaptchaText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtCaptchaText.Border.Class = "TextBoxBorder";
            this.txtCaptchaText.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCaptchaText.Location = new System.Drawing.Point(1233, 13);
            this.txtCaptchaText.MaxLength = 5;
            this.txtCaptchaText.Name = "txtCaptchaText";
            this.txtCaptchaText.PreventEnterBeep = true;
            this.txtCaptchaText.Size = new System.Drawing.Size(56, 20);
            this.txtCaptchaText.TabIndex = 8;
            this.txtCaptchaText.Visible = false;
            this.txtCaptchaText.WatermarkFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCaptchaText.WatermarkText = "Enter Captcha";
            this.txtCaptchaText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCaptchaText_KeyPress);
            // 
            // lblCaptcha
            // 
            this.lblCaptcha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCaptcha.AutoSize = true;
            this.lblCaptcha.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaptcha.Location = new System.Drawing.Point(1101, 5);
            this.lblCaptcha.Name = "lblCaptcha";
            this.lblCaptcha.OutlineForeColor = System.Drawing.Color.Black;
            this.lblCaptcha.OutlineWidth = 1F;
            this.lblCaptcha.Size = new System.Drawing.Size(60, 24);
            this.lblCaptcha.TabIndex = 7;
            this.lblCaptcha.Text = "12345";
            this.lblCaptcha.Visible = false;
            // 
            // lblSelectedField
            // 
            this.lblSelectedField.AutoSize = true;
            // 
            // 
            // 
            this.lblSelectedField.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblSelectedField.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblSelectedField.Location = new System.Drawing.Point(445, 8);
            this.lblSelectedField.Name = "lblSelectedField";
            this.lblSelectedField.Size = new System.Drawing.Size(126, 26);
            this.lblSelectedField.TabIndex = 6;
            this.lblSelectedField.Text = "Selected Field";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(366, 14);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(84, 15);
            this.labelX1.TabIndex = 5;
            this.labelX1.Text = "Selected Field : ";
            // 
            // btnLoadGrid
            // 
            this.btnLoadGrid.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoadGrid.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLoadGrid.Location = new System.Drawing.Point(265, 11);
            this.btnLoadGrid.Name = "btnLoadGrid";
            this.btnLoadGrid.Size = new System.Drawing.Size(75, 23);
            this.btnLoadGrid.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLoadGrid.TabIndex = 4;
            this.btnLoadGrid.Text = "Load";
            this.btnLoadGrid.Click += new System.EventHandler(this.btnLoadGrid_Click);
            // 
            // lblField
            // 
            this.lblField.AutoSize = true;
            // 
            // 
            // 
            this.lblField.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblField.Location = new System.Drawing.Point(3, 14);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(60, 15);
            this.lblField.TabIndex = 3;
            this.lblField.Text = "Select Field";
            // 
            // cmbFieldNames
            // 
            this.cmbFieldNames.DisplayMember = "Text";
            this.cmbFieldNames.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFieldNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFieldNames.FocusCuesEnabled = false;
            this.cmbFieldNames.FormattingEnabled = true;
            this.cmbFieldNames.ItemHeight = 14;
            this.cmbFieldNames.Location = new System.Drawing.Point(69, 12);
            this.cmbFieldNames.Name = "cmbFieldNames";
            this.cmbFieldNames.Size = new System.Drawing.Size(181, 20);
            this.cmbFieldNames.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbFieldNames.TabIndex = 2;
            // 
            // btnAccpetChecked
            // 
            this.btnAccpetChecked.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAccpetChecked.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccpetChecked.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAccpetChecked.Location = new System.Drawing.Point(1331, 13);
            this.btnAccpetChecked.Name = "btnAccpetChecked";
            this.btnAccpetChecked.Size = new System.Drawing.Size(62, 23);
            this.btnAccpetChecked.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAccpetChecked.TabIndex = 0;
            this.btnAccpetChecked.Text = "&Apply";
            this.btnAccpetChecked.Click += new System.EventHandler(this.btnAccpetChecked_Click);
            // 
            // frmUncertain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1405, 811);
            this.Controls.Add(this.xGridUncertain);
            this.Controls.Add(this.panelInfo);
            this.Name = "frmUncertain";
            this.Text = "Uncertain";
            this.Load += new System.EventHandler(this.frm_Uncertain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xGridUncertain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMemoEditAgent_Names)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rRadioGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn1_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Group1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.item1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.item2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.item3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.item4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rCheckStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rCheckStatus_Reject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gView)).EndInit();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl xGridUncertain;
        private DevExpress.XtraGrid.Views.Layout.LayoutView lView;
        private DevExpress.XtraGrid.Views.Grid.GridView gView;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colUncertain_Field;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colUncertainRecordCount;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colUncertainAgentCount;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colUncertainRaisedAgents;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit rMemoEditAgent_Names;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rCheckStatus;
        private DevComponents.DotNetBar.PanelEx panelInfo;
        private DevComponents.DotNetBar.ButtonX btnAccpetChecked;
        private DevComponents.DotNetBar.LabelX lblField;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbFieldNames;
        private DevComponents.DotNetBar.ButtonX btnLoadGrid;
        private DevComponents.DotNetBar.LabelX lblSelectedField;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colCheckStatus;
        private CustomLabel lblCaptcha;
        private DevComponents.DotNetBar.ButtonX btnCaptchaCancel;
        private DevComponents.DotNetBar.ButtonX btnCaptchaGo;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCaptchaText;
        private DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup rRadioGroup;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rCheckStatus_Reject;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_layoutViewColumn1;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_layoutViewColumn2;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_layoutViewColumn3;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_layoutViewColumn5;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_layoutViewColumn1_1;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewCard layoutViewCard1;
        private DevExpress.XtraLayout.LayoutControlGroup Group1;
        private DevExpress.XtraLayout.SimpleSeparator item1;
        private DevExpress.XtraLayout.SimpleSeparator item2;
        private DevExpress.XtraLayout.SimpleSeparator item3;
        private DevExpress.XtraLayout.SimpleSeparator item4;        

    }
}