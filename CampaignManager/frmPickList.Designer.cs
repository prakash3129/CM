namespace GCC
{
    partial class frmPickList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvPicklistCategory = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.dgvPickListValues = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.SplitPiclList = new System.Windows.Forms.SplitContainer();
            this.btnAddCategory = new DevComponents.DotNetBar.ButtonX();
            this.btnUpdate = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPicklistCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPickListValues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SplitPiclList)).BeginInit();
            this.SplitPiclList.Panel1.SuspendLayout();
            this.SplitPiclList.Panel2.SuspendLayout();
            this.SplitPiclList.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPicklistCategory
            // 
            this.dgvPicklistCategory.AllowUserToAddRows = false;
            this.dgvPicklistCategory.AllowUserToDeleteRows = false;
            this.dgvPicklistCategory.AllowUserToResizeRows = false;
            this.dgvPicklistCategory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPicklistCategory.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPicklistCategory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPicklistCategory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPicklistCategory.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPicklistCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPicklistCategory.EnableHeadersVisualStyles = false;
            this.dgvPicklistCategory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.dgvPicklistCategory.Location = new System.Drawing.Point(0, 0);
            this.dgvPicklistCategory.Name = "dgvPicklistCategory";
            this.dgvPicklistCategory.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPicklistCategory.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvPicklistCategory.RowHeadersVisible = false;
            this.dgvPicklistCategory.Size = new System.Drawing.Size(242, 558);
            this.dgvPicklistCategory.TabIndex = 0;
            this.dgvPicklistCategory.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPicklistCategory_RowEnter);
            // 
            // dgvPickListValues
            // 
            this.dgvPickListValues.AllowUserToResizeRows = false;
            this.dgvPickListValues.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPickListValues.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPickListValues.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPickListValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPickListValues.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPickListValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPickListValues.EnableHeadersVisualStyles = false;
            this.dgvPickListValues.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.dgvPickListValues.Location = new System.Drawing.Point(0, 0);
            this.dgvPickListValues.Name = "dgvPickListValues";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPickListValues.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvPickListValues.RowHeadersVisible = false;
            this.dgvPickListValues.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPickListValues.Size = new System.Drawing.Size(670, 558);
            this.dgvPickListValues.TabIndex = 1;
            this.dgvPickListValues.DataSourceChanged += new System.EventHandler(this.dgvPickListValues_DataSourceChanged);
            this.dgvPickListValues.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvPickListValues_RowsAdded);
            // 
            // SplitPiclList
            // 
            this.SplitPiclList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SplitPiclList.Location = new System.Drawing.Point(4, 2);
            this.SplitPiclList.Name = "SplitPiclList";
            // 
            // SplitPiclList.Panel1
            // 
            this.SplitPiclList.Panel1.Controls.Add(this.dgvPicklistCategory);
            // 
            // SplitPiclList.Panel2
            // 
            this.SplitPiclList.Panel2.Controls.Add(this.dgvPickListValues);
            this.SplitPiclList.Size = new System.Drawing.Size(916, 558);
            this.SplitPiclList.SplitterDistance = 242;
            this.SplitPiclList.TabIndex = 2;
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCategory.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddCategory.Location = new System.Drawing.Point(641, 569);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(89, 23);
            this.btnAddCategory.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAddCategory.TabIndex = 3;
            this.btnAddCategory.Text = "&Add Category";
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnUpdate.Location = new System.Drawing.Point(736, 569);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(89, 23);
            this.btnUpdate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "&Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(831, 569);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmPickList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(924, 598);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAddCategory);
            this.Controls.Add(this.SplitPiclList);
            this.EnableGlass = false;
            this.Name = "frmPickList";
            this.Text = "Selection List";
            this.Load += new System.EventHandler(this.frmPickList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPicklistCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPickListValues)).EndInit();
            this.SplitPiclList.Panel1.ResumeLayout(false);
            this.SplitPiclList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitPiclList)).EndInit();
            this.SplitPiclList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgvPicklistCategory;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvPickListValues;
        private System.Windows.Forms.SplitContainer SplitPiclList;
        private DevComponents.DotNetBar.ButtonX btnAddCategory;
        private DevComponents.DotNetBar.ButtonX btnUpdate;
        private DevComponents.DotNetBar.ButtonX btnCancel;
    }
}