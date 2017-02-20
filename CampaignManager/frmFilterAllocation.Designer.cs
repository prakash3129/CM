namespace GCC
{
    partial class frmFilterAllocation
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblName = new System.Windows.Forms.Label();
            this.txtFilterName = new System.Windows.Forms.TextBox();
            this.lblFilterDesc = new System.Windows.Forms.Label();
            this.mnuConditionGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addConditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeConditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAllocationGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activateFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deactivateFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previewFilterDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtFilterDesc = new System.Windows.Forms.TextBox();
            this.splitGridAndControls = new System.Windows.Forms.SplitContainer();
            this.dgvAllocationFilter = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.splitConditions = new System.Windows.Forms.SplitContainer();
            this.lblRechurn = new System.Windows.Forms.Label();
            this.lblFilterFor = new System.Windows.Forms.Label();
            this.swtchNewRecords = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.lblTimeZone = new System.Windows.Forms.Label();
            this.swtchTimezone = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.label1 = new System.Windows.Forms.Label();
            this.swtchRandom = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.lblRandomize = new System.Windows.Forms.Label();
            this.lblActive = new System.Windows.Forms.Label();
            this.swtchActive = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.txtQuery = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblDateTo = new System.Windows.Forms.Label();
            this.dateTo = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.dateFrom = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.txtTableName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtFieldName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnReloadCount = new DevComponents.DotNetBar.ButtonX();
            this.btnClose = new DevComponents.DotNetBar.ButtonX();
            this.btnCloseHidden = new DevComponents.DotNetBar.ButtonX();
            this.lblConditionInfo = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblField = new System.Windows.Forms.Label();
            this.btnDiscard = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.lblCondition = new System.Windows.Forms.Label();
            this.dgvConditions = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.cmbCondition = new System.Windows.Forms.ComboBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.txtValue = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblValue = new System.Windows.Forms.Label();
            this.mnuConditionGrid.SuspendLayout();
            this.mnuAllocationGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGridAndControls)).BeginInit();
            this.splitGridAndControls.Panel1.SuspendLayout();
            this.splitGridAndControls.Panel2.SuspendLayout();
            this.splitGridAndControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllocationFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitConditions)).BeginInit();
            this.splitConditions.Panel1.SuspendLayout();
            this.splitConditions.Panel2.SuspendLayout();
            this.splitConditions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConditions)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblName.Location = new System.Drawing.Point(16, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // txtFilterName
            // 
            this.txtFilterName.Location = new System.Drawing.Point(19, 26);
            this.txtFilterName.Name = "txtFilterName";
            this.txtFilterName.Size = new System.Drawing.Size(223, 20);
            this.txtFilterName.TabIndex = 3;
            this.txtFilterName.TextChanged += new System.EventHandler(this.txtFilterName_TextChanged);
            // 
            // lblFilterDesc
            // 
            this.lblFilterDesc.AutoSize = true;
            this.lblFilterDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblFilterDesc.Location = new System.Drawing.Point(16, 59);
            this.lblFilterDesc.Name = "lblFilterDesc";
            this.lblFilterDesc.Size = new System.Drawing.Size(60, 13);
            this.lblFilterDesc.TabIndex = 10;
            this.lblFilterDesc.Text = "Description";
            // 
            // mnuConditionGrid
            // 
            this.mnuConditionGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addConditionToolStripMenuItem,
            this.removeConditionToolStripMenuItem});
            this.mnuConditionGrid.Name = "mnuConditionGrid";
            this.mnuConditionGrid.Size = new System.Drawing.Size(174, 48);
            // 
            // addConditionToolStripMenuItem
            // 
            this.addConditionToolStripMenuItem.Image = global::GCC.Properties.Resources.math_add_icon;
            this.addConditionToolStripMenuItem.Name = "addConditionToolStripMenuItem";
            this.addConditionToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.addConditionToolStripMenuItem.Text = "Add Condition";
            this.addConditionToolStripMenuItem.Click += new System.EventHandler(this.addConditionToolStripMenuItem_Click);
            // 
            // removeConditionToolStripMenuItem
            // 
            this.removeConditionToolStripMenuItem.Image = global::GCC.Properties.Resources.erase_icon;
            this.removeConditionToolStripMenuItem.Name = "removeConditionToolStripMenuItem";
            this.removeConditionToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.removeConditionToolStripMenuItem.Text = "Remove Condition";
            this.removeConditionToolStripMenuItem.Click += new System.EventHandler(this.removeConditionToolStripMenuItem_Click);
            // 
            // mnuAllocationGrid
            // 
            this.mnuAllocationGrid.BackColor = System.Drawing.SystemColors.Control;
            this.mnuAllocationGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFilterToolStripMenuItem,
            this.activateFilterToolStripMenuItem,
            this.deactivateFilterToolStripMenuItem,
            this.previewFilterDataToolStripMenuItem,
            this.editFilterToolStripMenuItem});
            this.mnuAllocationGrid.Name = "mnuAllocationGrid";
            this.mnuAllocationGrid.Size = new System.Drawing.Size(159, 114);
            // 
            // newFilterToolStripMenuItem
            // 
            this.newFilterToolStripMenuItem.Image = global::GCC.Properties.Resources.math_add_icon;
            this.newFilterToolStripMenuItem.Name = "newFilterToolStripMenuItem";
            this.newFilterToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.newFilterToolStripMenuItem.Text = "New Filter";
            this.newFilterToolStripMenuItem.Click += new System.EventHandler(this.newFilterToolStripMenuItem_Click);
            // 
            // activateFilterToolStripMenuItem
            // 
            this.activateFilterToolStripMenuItem.Image = global::GCC.Properties.Resources.Ok_icon;
            this.activateFilterToolStripMenuItem.Name = "activateFilterToolStripMenuItem";
            this.activateFilterToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.activateFilterToolStripMenuItem.Text = "Activate Filter";
            this.activateFilterToolStripMenuItem.Click += new System.EventHandler(this.activateFilterToolStripMenuItem_Click);
            // 
            // deactivateFilterToolStripMenuItem
            // 
            this.deactivateFilterToolStripMenuItem.Image = global::GCC.Properties.Resources.Actions_dialog_cancel_icon;
            this.deactivateFilterToolStripMenuItem.Name = "deactivateFilterToolStripMenuItem";
            this.deactivateFilterToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.deactivateFilterToolStripMenuItem.Text = "Deactivate Filter";
            this.deactivateFilterToolStripMenuItem.Click += new System.EventHandler(this.deactivateFilterToolStripMenuItem_Click);
            // 
            // previewFilterDataToolStripMenuItem
            // 
            this.previewFilterDataToolStripMenuItem.Image = global::GCC.Properties.Resources.Actions_document_preview_icon;
            this.previewFilterDataToolStripMenuItem.Name = "previewFilterDataToolStripMenuItem";
            this.previewFilterDataToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.previewFilterDataToolStripMenuItem.Text = "Filter Details";
            this.previewFilterDataToolStripMenuItem.Click += new System.EventHandler(this.previewFilterDataToolStripMenuItem_Click);
            // 
            // editFilterToolStripMenuItem
            // 
            this.editFilterToolStripMenuItem.Image = global::GCC.Properties.Resources.pencil_icon;
            this.editFilterToolStripMenuItem.Name = "editFilterToolStripMenuItem";
            this.editFilterToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.editFilterToolStripMenuItem.Text = "Edit Filter";
            this.editFilterToolStripMenuItem.Click += new System.EventHandler(this.editFilterToolStripMenuItem_Click);
            // 
            // txtFilterDesc
            // 
            this.txtFilterDesc.Location = new System.Drawing.Point(19, 78);
            this.txtFilterDesc.Multiline = true;
            this.txtFilterDesc.Name = "txtFilterDesc";
            this.txtFilterDesc.Size = new System.Drawing.Size(223, 35);
            this.txtFilterDesc.TabIndex = 9;
            this.txtFilterDesc.TextChanged += new System.EventHandler(this.txtFilterDesc_TextChanged);
            // 
            // splitGridAndControls
            // 
            this.splitGridAndControls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitGridAndControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitGridAndControls.Location = new System.Drawing.Point(0, 0);
            this.splitGridAndControls.Name = "splitGridAndControls";
            this.splitGridAndControls.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitGridAndControls.Panel1
            // 
            this.splitGridAndControls.Panel1.Controls.Add(this.dgvAllocationFilter);
            // 
            // splitGridAndControls.Panel2
            // 
            this.splitGridAndControls.Panel2.Controls.Add(this.splitConditions);
            this.splitGridAndControls.Size = new System.Drawing.Size(796, 413);
            this.splitGridAndControls.SplitterDistance = 149;
            this.splitGridAndControls.TabIndex = 21;
            // 
            // dgvAllocationFilter
            // 
            this.dgvAllocationFilter.AllowUserToAddRows = false;
            this.dgvAllocationFilter.AllowUserToDeleteRows = false;
            this.dgvAllocationFilter.AllowUserToResizeRows = false;
            this.dgvAllocationFilter.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAllocationFilter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAllocationFilter.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAllocationFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllocationFilter.ContextMenuStrip = this.mnuAllocationGrid;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAllocationFilter.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAllocationFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAllocationFilter.EnableHeadersVisualStyles = false;
            this.dgvAllocationFilter.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvAllocationFilter.Location = new System.Drawing.Point(0, 0);
            this.dgvAllocationFilter.MultiSelect = false;
            this.dgvAllocationFilter.Name = "dgvAllocationFilter";
            this.dgvAllocationFilter.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAllocationFilter.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvAllocationFilter.RowHeadersVisible = false;
            this.dgvAllocationFilter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAllocationFilter.Size = new System.Drawing.Size(794, 147);
            this.dgvAllocationFilter.TabIndex = 4;
            this.dgvAllocationFilter.DataSourceChanged += new System.EventHandler(this.dgvAllocationFilter_DataSourceChanged);
            this.dgvAllocationFilter.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAllocationFilter_CellClick);
            this.dgvAllocationFilter.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFilterAllocation_RowEnter);
            // 
            // splitConditions
            // 
            this.splitConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitConditions.Location = new System.Drawing.Point(0, 0);
            this.splitConditions.Name = "splitConditions";
            // 
            // splitConditions.Panel1
            // 
            this.splitConditions.Panel1.Controls.Add(this.lblRechurn);
            this.splitConditions.Panel1.Controls.Add(this.lblFilterFor);
            this.splitConditions.Panel1.Controls.Add(this.swtchNewRecords);
            this.splitConditions.Panel1.Controls.Add(this.lblTimeZone);
            this.splitConditions.Panel1.Controls.Add(this.swtchTimezone);
            this.splitConditions.Panel1.Controls.Add(this.label1);
            this.splitConditions.Panel1.Controls.Add(this.swtchRandom);
            this.splitConditions.Panel1.Controls.Add(this.lblRandomize);
            this.splitConditions.Panel1.Controls.Add(this.lblName);
            this.splitConditions.Panel1.Controls.Add(this.lblFilterDesc);
            this.splitConditions.Panel1.Controls.Add(this.txtFilterName);
            this.splitConditions.Panel1.Controls.Add(this.txtFilterDesc);
            this.splitConditions.Panel1.Controls.Add(this.lblActive);
            this.splitConditions.Panel1.Controls.Add(this.swtchActive);
            // 
            // splitConditions.Panel2
            // 
            this.splitConditions.Panel2.Controls.Add(this.txtQuery);
            this.splitConditions.Panel2.Controls.Add(this.lblDateTo);
            this.splitConditions.Panel2.Controls.Add(this.dateTo);
            this.splitConditions.Panel2.Controls.Add(this.dateFrom);
            this.splitConditions.Panel2.Controls.Add(this.txtTableName);
            this.splitConditions.Panel2.Controls.Add(this.txtFieldName);
            this.splitConditions.Panel2.Controls.Add(this.btnReloadCount);
            this.splitConditions.Panel2.Controls.Add(this.btnClose);
            this.splitConditions.Panel2.Controls.Add(this.btnCloseHidden);
            this.splitConditions.Panel2.Controls.Add(this.lblConditionInfo);
            this.splitConditions.Panel2.Controls.Add(this.lblCount);
            this.splitConditions.Panel2.Controls.Add(this.lblField);
            this.splitConditions.Panel2.Controls.Add(this.btnDiscard);
            this.splitConditions.Panel2.Controls.Add(this.btnSave);
            this.splitConditions.Panel2.Controls.Add(this.lblCondition);
            this.splitConditions.Panel2.Controls.Add(this.dgvConditions);
            this.splitConditions.Panel2.Controls.Add(this.cmbCondition);
            this.splitConditions.Panel2.Controls.Add(this.lblFilter);
            this.splitConditions.Panel2.Controls.Add(this.txtValue);
            this.splitConditions.Panel2.Controls.Add(this.lblValue);
            this.splitConditions.Size = new System.Drawing.Size(794, 258);
            this.splitConditions.SplitterDistance = 245;
            this.splitConditions.TabIndex = 34;
            // 
            // lblRechurn
            // 
            this.lblRechurn.AutoSize = true;
            this.lblRechurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F);
            this.lblRechurn.Location = new System.Drawing.Point(130, 162);
            this.lblRechurn.Name = "lblRechurn";
            this.lblRechurn.Size = new System.Drawing.Size(46, 12);
            this.lblRechurn.TabIndex = 22;
            this.lblRechurn.Text = "(Rechurn)";
            this.lblRechurn.Visible = false;
            // 
            // lblFilterFor
            // 
            this.lblFilterFor.AutoSize = true;
            this.lblFilterFor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblFilterFor.Location = new System.Drawing.Point(127, 124);
            this.lblFilterFor.Name = "lblFilterFor";
            this.lblFilterFor.Size = new System.Drawing.Size(69, 13);
            this.lblFilterFor.TabIndex = 20;
            this.lblFilterFor.Text = "Record Type";
            // 
            // swtchNewRecords
            // 
            // 
            // 
            // 
            this.swtchNewRecords.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.swtchNewRecords.FocusCuesEnabled = false;
            this.swtchNewRecords.Location = new System.Drawing.Point(130, 140);
            this.swtchNewRecords.Name = "swtchNewRecords";
            this.swtchNewRecords.OffBackColor = System.Drawing.SystemColors.Control;
            this.swtchNewRecords.OffText = "Processed";
            this.swtchNewRecords.OnBackColor = System.Drawing.SystemColors.Control;
            this.swtchNewRecords.OnText = "New";
            this.swtchNewRecords.Size = new System.Drawing.Size(79, 22);
            this.swtchNewRecords.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.swtchNewRecords.SwitchWidth = 17;
            this.swtchNewRecords.TabIndex = 21;
            this.swtchNewRecords.Value = true;
            this.swtchNewRecords.ValueObject = "Y";
            this.swtchNewRecords.ValueChanged += new System.EventHandler(this.swtchNewRecords_ValueChanged);
            // 
            // lblTimeZone
            // 
            this.lblTimeZone.AutoSize = true;
            this.lblTimeZone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblTimeZone.Location = new System.Drawing.Point(19, 124);
            this.lblTimeZone.Name = "lblTimeZone";
            this.lblTimeZone.Size = new System.Drawing.Size(80, 13);
            this.lblTimeZone.TabIndex = 18;
            this.lblTimeZone.Text = "Use Time Zone";
            // 
            // swtchTimezone
            // 
            // 
            // 
            // 
            this.swtchTimezone.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.swtchTimezone.FocusCuesEnabled = false;
            this.swtchTimezone.Location = new System.Drawing.Point(22, 141);
            this.swtchTimezone.Name = "swtchTimezone";
            this.swtchTimezone.OffBackColor = System.Drawing.SystemColors.Control;
            this.swtchTimezone.OffText = "No";
            this.swtchTimezone.OnText = "Yes";
            this.swtchTimezone.Size = new System.Drawing.Size(77, 22);
            this.swtchTimezone.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.swtchTimezone.TabIndex = 19;
            this.swtchTimezone.Value = true;
            this.swtchTimezone.ValueObject = "Y";
            this.swtchTimezone.ValueChanged += new System.EventHandler(this.swtchTimezone_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F);
            this.label1.Location = new System.Drawing.Point(131, 229);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "Not recomended";
            // 
            // swtchRandom
            // 
            // 
            // 
            // 
            this.swtchRandom.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.swtchRandom.FocusCuesEnabled = false;
            this.swtchRandom.Location = new System.Drawing.Point(130, 207);
            this.swtchRandom.Name = "swtchRandom";
            this.swtchRandom.OffBackColor = System.Drawing.SystemColors.Control;
            this.swtchRandom.OffText = "No";
            this.swtchRandom.OnText = "Yes";
            this.swtchRandom.Size = new System.Drawing.Size(79, 22);
            this.swtchRandom.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.swtchRandom.TabIndex = 16;
            this.swtchRandom.ValueChanged += new System.EventHandler(this.swtchRandom_ValueChanged);
            // 
            // lblRandomize
            // 
            this.lblRandomize.AutoSize = true;
            this.lblRandomize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblRandomize.Location = new System.Drawing.Point(127, 190);
            this.lblRandomize.Name = "lblRandomize";
            this.lblRandomize.Size = new System.Drawing.Size(98, 13);
            this.lblRandomize.TabIndex = 15;
            this.lblRandomize.Text = "Randomize records";
            // 
            // lblActive
            // 
            this.lblActive.AutoSize = true;
            this.lblActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblActive.Location = new System.Drawing.Point(19, 190);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(37, 13);
            this.lblActive.TabIndex = 13;
            this.lblActive.Text = "Active";
            // 
            // swtchActive
            // 
            // 
            // 
            // 
            this.swtchActive.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.swtchActive.FocusCuesEnabled = false;
            this.swtchActive.Location = new System.Drawing.Point(22, 207);
            this.swtchActive.Name = "swtchActive";
            this.swtchActive.OffBackColor = System.Drawing.SystemColors.Control;
            this.swtchActive.OffText = "No";
            this.swtchActive.OnText = "Yes";
            this.swtchActive.Size = new System.Drawing.Size(77, 22);
            this.swtchActive.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.swtchActive.TabIndex = 14;
            this.swtchActive.ValueChanged += new System.EventHandler(this.swtchActive_ValueChanged);
            // 
            // txtQuery
            // 
            this.txtQuery.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtQuery.Border.Class = "TextBoxBorder";
            this.txtQuery.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtQuery.Location = new System.Drawing.Point(211, 222);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.PreventEnterBeep = true;
            this.txtQuery.ReadOnly = true;
            this.txtQuery.Size = new System.Drawing.Size(150, 20);
            this.txtQuery.TabIndex = 44;
            this.txtQuery.Visible = false;
            this.txtQuery.WatermarkText = "Query";
            // 
            // lblDateTo
            // 
            this.lblDateTo.AutoSize = true;
            this.lblDateTo.BackColor = System.Drawing.Color.Transparent;
            this.lblDateTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblDateTo.Location = new System.Drawing.Point(413, 185);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.Size = new System.Drawing.Size(16, 13);
            this.lblDateTo.TabIndex = 43;
            this.lblDateTo.Text = "to";
            this.lblDateTo.Visible = false;
            // 
            // dateTo
            // 
            this.dateTo.AllowEmptyState = false;
            // 
            // 
            // 
            this.dateTo.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dateTo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTo.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dateTo.ButtonDropDown.Visible = true;
            this.dateTo.DateTimeSelectorVisibility = DevComponents.Editors.DateTimeAdv.eDateTimeSelectorVisibility.DateSelector;
            this.dateTo.IsPopupCalendarOpen = false;
            this.dateTo.Location = new System.Drawing.Point(428, 181);
            this.dateTo.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.dateTo.MinDate = new System.DateTime(2013, 1, 1, 0, 0, 0, 0);
            // 
            // 
            // 
            this.dateTo.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTo.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTo.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this.dateTo.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dateTo.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dateTo.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTo.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dateTo.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dateTo.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dateTo.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dateTo.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTo.MonthCalendar.DisplayMonth = new System.DateTime(2015, 8, 1, 0, 0, 0, 0);
            this.dateTo.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday;
            this.dateTo.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dateTo.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTo.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dateTo.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTo.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dateTo.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTo.MonthCalendar.TodayButtonVisible = true;
            this.dateTo.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dateTo.Name = "dateTo";
            this.dateTo.Size = new System.Drawing.Size(102, 20);
            this.dateTo.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.dateTo.TabIndex = 42;
            this.dateTo.Value = new System.DateTime(2013, 1, 1, 0, 0, 0, 0);
            this.dateTo.Visible = false;
            this.dateTo.TextChanged += new System.EventHandler(this.dateFrom_TextChanged);
            // 
            // dateFrom
            // 
            this.dateFrom.AllowEmptyState = false;
            // 
            // 
            // 
            this.dateFrom.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dateFrom.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateFrom.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dateFrom.ButtonDropDown.Visible = true;
            this.dateFrom.DateTimeSelectorVisibility = DevComponents.Editors.DateTimeAdv.eDateTimeSelectorVisibility.DateSelector;
            this.dateFrom.IsPopupCalendarOpen = false;
            this.dateFrom.Location = new System.Drawing.Point(309, 181);
            this.dateFrom.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.dateFrom.MinDate = new System.DateTime(2013, 1, 1, 0, 0, 0, 0);
            // 
            // 
            // 
            this.dateFrom.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateFrom.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateFrom.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this.dateFrom.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dateFrom.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dateFrom.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dateFrom.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dateFrom.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dateFrom.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dateFrom.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dateFrom.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateFrom.MonthCalendar.DisplayMonth = new System.DateTime(2015, 8, 1, 0, 0, 0, 0);
            this.dateFrom.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday;
            this.dateFrom.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dateFrom.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateFrom.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dateFrom.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dateFrom.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dateFrom.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateFrom.MonthCalendar.TodayButtonVisible = true;
            this.dateFrom.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dateFrom.Name = "dateFrom";
            this.dateFrom.Size = new System.Drawing.Size(102, 20);
            this.dateFrom.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.dateFrom.TabIndex = 41;
            this.dateFrom.Value = new System.DateTime(2013, 1, 1, 0, 0, 0, 0);
            this.dateFrom.Visible = false;
            this.dateFrom.TextChanged += new System.EventHandler(this.dateFrom_TextChanged);
            // 
            // txtTableName
            // 
            this.txtTableName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtTableName.Border.Class = "TextBoxBorder";
            this.txtTableName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTableName.ButtonCustom.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtTableName.Location = new System.Drawing.Point(13, 183);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.ReadOnly = true;
            this.txtTableName.Size = new System.Drawing.Size(64, 20);
            this.txtTableName.TabIndex = 40;
            this.txtTableName.TextChanged += new System.EventHandler(this.txtTableName_TextChanged);
            // 
            // txtFieldName
            // 
            this.txtFieldName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtFieldName.Border.Class = "TextBoxBorder";
            this.txtFieldName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtFieldName.ButtonCustom.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtFieldName.ButtonCustom.Visible = true;
            this.txtFieldName.Location = new System.Drawing.Point(83, 183);
            this.txtFieldName.Name = "txtFieldName";
            this.txtFieldName.ReadOnly = true;
            this.txtFieldName.Size = new System.Drawing.Size(126, 20);
            this.txtFieldName.TabIndex = 39;
            this.txtFieldName.ButtonCustomClick += new System.EventHandler(this.txtFieldName_ButtonCustomClick);
            this.txtFieldName.TextChanged += new System.EventHandler(this.txtFieldName_TextChanged);
            // 
            // btnReloadCount
            // 
            this.btnReloadCount.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReloadCount.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReloadCount.FocusCuesEnabled = false;
            this.btnReloadCount.Image = global::GCC.Properties.Resources.arrowrefresh3New;
            this.btnReloadCount.Location = new System.Drawing.Point(13, 217);
            this.btnReloadCount.Name = "btnReloadCount";
            this.btnReloadCount.Size = new System.Drawing.Size(26, 28);
            this.btnReloadCount.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnReloadCount.TabIndex = 38;
            this.btnReloadCount.Click += new System.EventHandler(this.btnReloadCount_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClose.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(575, 219);
            this.btnClose.Name = "btnClose";
            this.btnClose.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnClose.Size = new System.Drawing.Size(60, 26);
            this.btnClose.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnClose.TabIndex = 37;
            this.btnClose.Text = "Close";
            // 
            // btnCloseHidden
            // 
            this.btnCloseHidden.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCloseHidden.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.btnCloseHidden.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCloseHidden.Location = new System.Drawing.Point(561, 219);
            this.btnCloseHidden.Name = "btnCloseHidden";
            this.btnCloseHidden.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnCloseHidden.Size = new System.Drawing.Size(76, 26);
            this.btnCloseHidden.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCloseHidden.TabIndex = 36;
            this.btnCloseHidden.Text = "Close";
            this.btnCloseHidden.Click += new System.EventHandler(this.btnCloseHidden_Click);
            // 
            // lblConditionInfo
            // 
            this.lblConditionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConditionInfo.AutoSize = true;
            this.lblConditionInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblConditionInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblConditionInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblConditionInfo.Location = new System.Drawing.Point(223, 6);
            this.lblConditionInfo.Name = "lblConditionInfo";
            this.lblConditionInfo.Size = new System.Drawing.Size(313, 13);
            this.lblConditionInfo.TabIndex = 35;
            this.lblConditionInfo.Text = "Right-Click to Add or Remove Conditions / Click to Edit Condition";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.BackColor = System.Drawing.Color.Transparent;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblCount.Location = new System.Drawing.Point(42, 224);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(35, 13);
            this.lblCount.TabIndex = 34;
            this.lblCount.Text = "Count";
            // 
            // lblField
            // 
            this.lblField.AutoSize = true;
            this.lblField.BackColor = System.Drawing.Color.Transparent;
            this.lblField.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblField.Location = new System.Drawing.Point(10, 163);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(60, 13);
            this.lblField.TabIndex = 23;
            this.lblField.Text = "Field Name";
            // 
            // btnDiscard
            // 
            this.btnDiscard.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDiscard.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.btnDiscard.Location = new System.Drawing.Point(455, 219);
            this.btnDiscard.Name = "btnDiscard";
            this.btnDiscard.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnDiscard.Size = new System.Drawing.Size(76, 26);
            this.btnDiscard.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnDiscard.TabIndex = 33;
            this.btnDiscard.Text = "Cancel";
            this.btnDiscard.Click += new System.EventHandler(this.btnDiscard_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.btnSave.Location = new System.Drawing.Point(367, 219);
            this.btnSave.Name = "btnSave";
            this.btnSave.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.btnSave.Size = new System.Drawing.Size(75, 26);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblCondition
            // 
            this.lblCondition.AutoSize = true;
            this.lblCondition.BackColor = System.Drawing.Color.Transparent;
            this.lblCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblCondition.Location = new System.Drawing.Point(212, 162);
            this.lblCondition.Name = "lblCondition";
            this.lblCondition.Size = new System.Drawing.Size(51, 13);
            this.lblCondition.TabIndex = 24;
            this.lblCondition.Text = "Condition";
            // 
            // dgvConditions
            // 
            this.dgvConditions.AllowUserToAddRows = false;
            this.dgvConditions.AllowUserToOrderColumns = true;
            this.dgvConditions.AllowUserToResizeColumns = false;
            this.dgvConditions.AllowUserToResizeRows = false;
            this.dgvConditions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvConditions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvConditions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvConditions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvConditions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConditions.ContextMenuStrip = this.mnuConditionGrid;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvConditions.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvConditions.EnableHeadersVisualStyles = false;
            this.dgvConditions.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvConditions.Location = new System.Drawing.Point(13, 26);
            this.dgvConditions.MultiSelect = false;
            this.dgvConditions.Name = "dgvConditions";
            this.dgvConditions.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvConditions.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvConditions.RowHeadersVisible = false;
            this.dgvConditions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConditions.Size = new System.Drawing.Size(523, 126);
            this.dgvConditions.TabIndex = 29;
            this.dgvConditions.DataSourceChanged += new System.EventHandler(this.dgvConditions_DataSourceChanged);
            this.dgvConditions.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConditions_CellClick);
            this.dgvConditions.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConditions_CellValueChanged);
            // 
            // cmbCondition
            // 
            this.cmbCondition.AllowDrop = true;
            this.cmbCondition.FormattingEnabled = true;
            this.cmbCondition.Location = new System.Drawing.Point(215, 181);
            this.cmbCondition.Name = "cmbCondition";
            this.cmbCondition.Size = new System.Drawing.Size(88, 21);
            this.cmbCondition.TabIndex = 27;
            this.cmbCondition.TextChanged += new System.EventHandler(this.cmbCondition_TextChanged);
            this.cmbCondition.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbCondition_KeyPress);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.BackColor = System.Drawing.Color.Transparent;
            this.lblFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblFilter.Location = new System.Drawing.Point(10, 10);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(29, 13);
            this.lblFilter.TabIndex = 30;
            this.lblFilter.Text = "Filter";
            // 
            // txtValue
            // 
            // 
            // 
            // 
            this.txtValue.Border.Class = "TextBoxBorder";
            this.txtValue.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtValue.ButtonCustom.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtValue.ButtonCustom.Visible = true;
            this.txtValue.Location = new System.Drawing.Point(309, 181);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(221, 20);
            this.txtValue.TabIndex = 28;
            this.txtValue.ButtonCustomClick += new System.EventHandler(this.cmbTreeValue_ButtonCustomClick_1);
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.BackColor = System.Drawing.Color.Transparent;
            this.lblValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblValue.Location = new System.Drawing.Point(306, 162);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(34, 13);
            this.lblValue.TabIndex = 25;
            this.lblValue.Text = "Value";
            // 
            // frmFilterAllocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCloseHidden;
            this.ClientSize = new System.Drawing.Size(796, 413);
            this.Controls.Add(this.splitGridAndControls);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmFilterAllocation";
            this.Text = "Filter Allocation";
            this.Load += new System.EventHandler(this.frmFilterAllocation_Load);
            this.mnuConditionGrid.ResumeLayout(false);
            this.mnuAllocationGrid.ResumeLayout(false);
            this.splitGridAndControls.Panel1.ResumeLayout(false);
            this.splitGridAndControls.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGridAndControls)).EndInit();
            this.splitGridAndControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllocationFilter)).EndInit();
            this.splitConditions.Panel1.ResumeLayout(false);
            this.splitConditions.Panel1.PerformLayout();
            this.splitConditions.Panel2.ResumeLayout(false);
            this.splitConditions.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitConditions)).EndInit();
            this.splitConditions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConditions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtFilterName;
        private System.Windows.Forms.Label lblFilterDesc;
        private System.Windows.Forms.TextBox txtFilterDesc;
        private System.Windows.Forms.SplitContainer splitGridAndControls;
        private System.Windows.Forms.ContextMenuStrip mnuConditionGrid;
        private System.Windows.Forms.ContextMenuStrip mnuAllocationGrid;
        private DevComponents.DotNetBar.Controls.SwitchButton swtchActive;
        private System.Windows.Forms.Label lblActive;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvAllocationFilter;
        private System.Windows.Forms.ToolStripMenuItem removeConditionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deactivateFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activateFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewFilterDataToolStripMenuItem;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvConditions;
        private System.Windows.Forms.Label lblFilter;
        private DevComponents.DotNetBar.Controls.TextBoxX txtValue;
        private System.Windows.Forms.Label lblField;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.ComboBox cmbCondition;
        private System.Windows.Forms.Label lblCondition;
        private DevComponents.DotNetBar.ButtonX btnDiscard;
        private System.Windows.Forms.SplitContainer splitConditions;
        private System.Windows.Forms.ToolStripMenuItem editFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addConditionToolStripMenuItem;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblConditionInfo;
        private DevComponents.DotNetBar.ButtonX btnCloseHidden;
        private DevComponents.DotNetBar.Controls.SwitchButton swtchRandom;
        private System.Windows.Forms.Label lblRandomize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTimeZone;
        private DevComponents.DotNetBar.Controls.SwitchButton swtchTimezone;
        private DevComponents.DotNetBar.ButtonX btnClose;
        private DevComponents.DotNetBar.ButtonX btnReloadCount;
        private DevComponents.DotNetBar.Controls.TextBoxX txtFieldName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTableName;
        private System.Windows.Forms.Label lblFilterFor;
        private DevComponents.DotNetBar.Controls.SwitchButton swtchNewRecords;
        private System.Windows.Forms.Label lblRechurn;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dateTo;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dateFrom;
        private System.Windows.Forms.Label lblDateTo;
        private DevComponents.DotNetBar.Controls.TextBoxX txtQuery;
    }
}