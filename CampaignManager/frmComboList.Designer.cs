namespace GCC
{
    partial class frmComboList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvList = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.mnuSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.checkSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtSelected = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lstSelected = new System.Windows.Forms.ListBox();
            this.splitCustomComboChild = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.mnuSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCustomComboChild)).BeginInit();
            this.splitCustomComboChild.Panel1.SuspendLayout();
            this.splitCustomComboChild.Panel2.SuspendLayout();
            this.splitCustomComboChild.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToOrderColumns = true;
            this.dgvList.AllowUserToResizeColumns = false;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.ColumnHeadersVisible = false;
            this.dgvList.ContextMenuStrip = this.mnuSelect;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.EnableHeadersVisualStyles = false;
            this.dgvList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.dgvList.Location = new System.Drawing.Point(0, 0);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(294, 215);
            this.dgvList.TabIndex = 1;
            this.dgvList.DataSourceChanged += new System.EventHandler(this.dgvList_DataSourceChanged);
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellContentClick);
            this.dgvList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellDoubleClick);
            this.dgvList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvList_KeyDown);
            // 
            // mnuSelect
            // 
            this.mnuSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkSelectedToolStripMenuItem});
            this.mnuSelect.Name = "mnuSelect";
            this.mnuSelect.Size = new System.Drawing.Size(155, 26);
            // 
            // checkSelectedToolStripMenuItem
            // 
            this.checkSelectedToolStripMenuItem.Name = "checkSelectedToolStripMenuItem";
            this.checkSelectedToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.checkSelectedToolStripMenuItem.Text = "Check Selected";
            this.checkSelectedToolStripMenuItem.Click += new System.EventHandler(this.checkSelectedToolStripMenuItem_Click);
            // 
            // txtSearch
            // 
            // 
            // 
            // 
            this.txtSearch.Border.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarFloatingBorder;
            this.txtSearch.Border.Class = "TextBoxBorder";
            this.txtSearch.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 3);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(294, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.WatermarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.WatermarkText = "Ctrl + ↑ to Search Here";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // txtSelected
            // 
            this.txtSelected.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtSelected.Border.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarFloatingBorder;
            this.txtSelected.Border.Class = "TextBoxBorder";
            this.txtSelected.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSelected.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSelected.Location = new System.Drawing.Point(0, 23);
            this.txtSelected.Margin = new System.Windows.Forms.Padding(0);
            this.txtSelected.Name = "txtSelected";
            this.txtSelected.Size = new System.Drawing.Size(294, 20);
            this.txtSelected.TabIndex = 1;
            this.txtSelected.WatermarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtSelected.WatermarkText = "(Ctrl + L to Clear and Ctrl + W to Clear last word)";
            // 
            // btnSubmit
            // 
            this.btnSubmit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSubmit.Location = new System.Drawing.Point(61, 323);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 4;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(106, 248);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lstSelected
            // 
            this.lstSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSelected.FormattingEnabled = true;
            this.lstSelected.Location = new System.Drawing.Point(0, 43);
            this.lstSelected.Margin = new System.Windows.Forms.Padding(0);
            this.lstSelected.Name = "lstSelected";
            this.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelected.Size = new System.Drawing.Size(294, 94);
            this.lstSelected.TabIndex = 2;
            this.lstSelected.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstSelected_KeyDown);
            // 
            // splitCustomComboChild
            // 
            this.splitCustomComboChild.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCustomComboChild.IsSplitterFixed = true;
            this.splitCustomComboChild.Location = new System.Drawing.Point(0, 0);
            this.splitCustomComboChild.Name = "splitCustomComboChild";
            this.splitCustomComboChild.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitCustomComboChild.Panel1
            // 
            this.splitCustomComboChild.Panel1.Controls.Add(this.lstSelected);
            this.splitCustomComboChild.Panel1.Controls.Add(this.txtSelected);
            this.splitCustomComboChild.Panel1.Controls.Add(this.txtSearch);
            this.splitCustomComboChild.Panel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            // 
            // splitCustomComboChild.Panel2
            // 
            this.splitCustomComboChild.Panel2.Controls.Add(this.dgvList);
            this.splitCustomComboChild.Size = new System.Drawing.Size(294, 356);
            this.splitCustomComboChild.SplitterDistance = 137;
            this.splitCustomComboChild.TabIndex = 8;
            this.splitCustomComboChild.TabStop = false;
            // 
            // frmComboList
            // 
            this.AcceptButton = this.btnSubmit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(294, 356);
            this.Controls.Add(this.splitCustomComboChild);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnClose);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmComboList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "List";
            this.Load += new System.EventHandler(this.frmComboList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.mnuSelect.ResumeLayout(false);
            this.splitCustomComboChild.Panel1.ResumeLayout(false);
            this.splitCustomComboChild.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCustomComboChild)).EndInit();
            this.splitCustomComboChild.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgvList;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSearch;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSelected;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip mnuSelect;
        private System.Windows.Forms.ToolStripMenuItem checkSelectedToolStripMenuItem;
        private System.Windows.Forms.ListBox lstSelected;
        private System.Windows.Forms.SplitContainer splitCustomComboChild;
    }
}