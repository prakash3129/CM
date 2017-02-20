namespace GCC
{
    partial class frmTeleAllocation
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
            this.dgvFilterAllocation = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.contextStripDGV = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnushowFilters = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuaddNewAgents = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteSelectedAllocation = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterAllocation)).BeginInit();
            this.contextStripDGV.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvFilterAllocation
            // 
            this.dgvFilterAllocation.AllowUserToAddRows = false;
            this.dgvFilterAllocation.AllowUserToDeleteRows = false;
            this.dgvFilterAllocation.AllowUserToOrderColumns = true;
            this.dgvFilterAllocation.AllowUserToResizeRows = false;
            this.dgvFilterAllocation.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFilterAllocation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFilterAllocation.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFilterAllocation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilterAllocation.ContextMenuStrip = this.contextStripDGV;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterAllocation.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvFilterAllocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFilterAllocation.EnableHeadersVisualStyles = false;
            this.dgvFilterAllocation.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.dgvFilterAllocation.Location = new System.Drawing.Point(0, 0);
            this.dgvFilterAllocation.Name = "dgvFilterAllocation";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFilterAllocation.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvFilterAllocation.RowHeadersVisible = false;
            this.dgvFilterAllocation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFilterAllocation.Size = new System.Drawing.Size(463, 305);
            this.dgvFilterAllocation.TabIndex = 19;
            this.dgvFilterAllocation.DataSourceChanged += new System.EventHandler(this.dgvFilterAllocation_DataSourceChanged);
            this.dgvFilterAllocation.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFilterAllocation_CellClick);
            this.dgvFilterAllocation.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvUserFilterAllocation_DataError);
            this.dgvFilterAllocation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvUserFilterAllocation_KeyDown);
            // 
            // contextStripDGV
            // 
            this.contextStripDGV.BackColor = System.Drawing.SystemColors.Control;
            this.contextStripDGV.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnushowFilters,
            this.mnuaddNewAgents,
            this.mnuDeleteSelectedAllocation});
            this.contextStripDGV.Name = "contextStripDGV";
            this.contextStripDGV.Size = new System.Drawing.Size(199, 70);
            // 
            // mnushowFilters
            // 
            this.mnushowFilters.Image = global::GCC.Properties.Resources.Filesystems_folder_open_icon;
            this.mnushowFilters.Name = "mnushowFilters";
            this.mnushowFilters.Size = new System.Drawing.Size(198, 22);
            this.mnushowFilters.Text = "Show Filters";
            this.mnushowFilters.Click += new System.EventHandler(this.mnushowFilters_Click);
            // 
            // mnuaddNewAgents
            // 
            this.mnuaddNewAgents.Image = global::GCC.Properties.Resources.Actions_list_add_user_icon;
            this.mnuaddNewAgents.Name = "mnuaddNewAgents";
            this.mnuaddNewAgents.Size = new System.Drawing.Size(198, 22);
            this.mnuaddNewAgents.Text = "Add New Agents";
            this.mnuaddNewAgents.Click += new System.EventHandler(this.mnuaddNewAgents_Click);
            // 
            // mnuDeleteSelectedAllocation
            // 
            this.mnuDeleteSelectedAllocation.Image = global::GCC.Properties.Resources.Actions_list_remove_user_icon;
            this.mnuDeleteSelectedAllocation.Name = "mnuDeleteSelectedAllocation";
            this.mnuDeleteSelectedAllocation.Size = new System.Drawing.Size(198, 22);
            this.mnuDeleteSelectedAllocation.Text = "Remove selected Agent";
            this.mnuDeleteSelectedAllocation.Click += new System.EventHandler(this.mnuDeleteSelectedAllocation_Click);
            // 
            // frmTeleAllocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 305);
            this.Controls.Add(this.dgvFilterAllocation);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(479, 339);
            this.Name = "frmTeleAllocation";
            this.Text = "Tele Allocation";
            this.Load += new System.EventHandler(this.frmTeleAllocation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterAllocation)).EndInit();
            this.contextStripDGV.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgvFilterAllocation;
        private System.Windows.Forms.ContextMenuStrip contextStripDGV;
        private System.Windows.Forms.ToolStripMenuItem mnushowFilters;
        private System.Windows.Forms.ToolStripMenuItem mnuaddNewAgents;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteSelectedAllocation;
    }
}