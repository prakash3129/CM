namespace GCC
{
    partial class frmQC
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
            this.splitQC = new System.Windows.Forms.SplitContainer();
            this.panelQCInfo = new DevComponents.DotNetBar.PanelEx();
            this.lblProcessType = new DevComponents.DotNetBar.LabelX();
            this.lblSamplePercent = new DevComponents.DotNetBar.LabelX();
            this.lblProcessTable = new DevComponents.DotNetBar.LabelX();
            this.lblProcessDate = new DevComponents.DotNetBar.LabelX();
            this.dgvQCList = new DevComponents.DotNetBar.Controls.DataGridViewX();
            ((System.ComponentModel.ISupportInitialize)(this.splitQC)).BeginInit();
            this.splitQC.Panel2.SuspendLayout();
            this.splitQC.SuspendLayout();
            this.panelQCInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQCList)).BeginInit();
            this.SuspendLayout();
            // 
            // splitQC
            // 
            this.splitQC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitQC.Location = new System.Drawing.Point(0, 0);
            this.splitQC.Name = "splitQC";
            // 
            // splitQC.Panel2
            // 
            this.splitQC.Panel2.Controls.Add(this.panelQCInfo);
            this.splitQC.Panel2.Controls.Add(this.dgvQCList);
            this.splitQC.Size = new System.Drawing.Size(994, 435);
            this.splitQC.SplitterDistance = 186;
            this.splitQC.TabIndex = 0;
            // 
            // panelQCInfo
            // 
            this.panelQCInfo.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelQCInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelQCInfo.Controls.Add(this.lblProcessType);
            this.panelQCInfo.Controls.Add(this.lblSamplePercent);
            this.panelQCInfo.Controls.Add(this.lblProcessTable);
            this.panelQCInfo.Controls.Add(this.lblProcessDate);
            this.panelQCInfo.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelQCInfo.Location = new System.Drawing.Point(18, 35);
            this.panelQCInfo.Name = "panelQCInfo";
            this.panelQCInfo.Size = new System.Drawing.Size(738, 27);
            this.panelQCInfo.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelQCInfo.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelQCInfo.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelQCInfo.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelQCInfo.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelQCInfo.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelQCInfo.Style.GradientAngle = 90;
            this.panelQCInfo.TabIndex = 0;
            // 
            // lblProcessType
            // 
            this.lblProcessType.AutoSize = true;
            // 
            // 
            // 
            this.lblProcessType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblProcessType.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblProcessType.Location = new System.Drawing.Point(117, 0);
            this.lblProcessType.Name = "lblProcessType";
            this.lblProcessType.PaddingTop = 5;
            this.lblProcessType.Size = new System.Drawing.Size(39, 20);
            this.lblProcessType.TabIndex = 3;
            this.lblProcessType.Text = "labelX1";
            // 
            // lblSamplePercent
            // 
            this.lblSamplePercent.AutoSize = true;
            // 
            // 
            // 
            this.lblSamplePercent.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblSamplePercent.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSamplePercent.Location = new System.Drawing.Point(78, 0);
            this.lblSamplePercent.Name = "lblSamplePercent";
            this.lblSamplePercent.PaddingTop = 5;
            this.lblSamplePercent.Size = new System.Drawing.Size(39, 20);
            this.lblSamplePercent.TabIndex = 2;
            this.lblSamplePercent.Text = "labelX2";
            // 
            // lblProcessTable
            // 
            this.lblProcessTable.AutoSize = true;
            // 
            // 
            // 
            this.lblProcessTable.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblProcessTable.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblProcessTable.Location = new System.Drawing.Point(39, 0);
            this.lblProcessTable.Name = "lblProcessTable";
            this.lblProcessTable.PaddingTop = 5;
            this.lblProcessTable.Size = new System.Drawing.Size(39, 20);
            this.lblProcessTable.TabIndex = 1;
            this.lblProcessTable.Text = "labelX1";
            // 
            // lblProcessDate
            // 
            this.lblProcessDate.AutoSize = true;
            // 
            // 
            // 
            this.lblProcessDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblProcessDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblProcessDate.Location = new System.Drawing.Point(0, 0);
            this.lblProcessDate.Name = "lblProcessDate";
            this.lblProcessDate.PaddingTop = 5;
            this.lblProcessDate.Size = new System.Drawing.Size(39, 20);
            this.lblProcessDate.TabIndex = 0;
            this.lblProcessDate.Text = "labelX1";
            // 
            // dgvQCList
            // 
            this.dgvQCList.AllowUserToAddRows = false;
            this.dgvQCList.AllowUserToDeleteRows = false;
            this.dgvQCList.AllowUserToOrderColumns = true;
            this.dgvQCList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvQCList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvQCList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvQCList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvQCList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvQCList.EnableHeadersVisualStyles = false;
            this.dgvQCList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.dgvQCList.Location = new System.Drawing.Point(0, 0);
            this.dgvQCList.MultiSelect = false;
            this.dgvQCList.Name = "dgvQCList";
            this.dgvQCList.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvQCList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvQCList.RowHeadersVisible = false;
            this.dgvQCList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvQCList.Size = new System.Drawing.Size(804, 435);
            this.dgvQCList.TabIndex = 1;
            this.dgvQCList.BackgroundColorChanged += new System.EventHandler(this.dgvQCList_BackgroundColorChanged);
            this.dgvQCList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvQCList_CellDoubleClick);
            // 
            // frmQC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 435);
            this.Controls.Add(this.splitQC);
            this.DoubleBuffered = true;
            this.Name = "frmQC";
            this.Text = "QC";
            this.Load += new System.EventHandler(this.frmQC_Load);
            this.splitQC.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitQC)).EndInit();
            this.splitQC.ResumeLayout(false);
            this.panelQCInfo.ResumeLayout(false);
            this.panelQCInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQCList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitQC;
        public DevComponents.DotNetBar.Controls.DataGridViewX dgvQCList;
        private DevComponents.DotNetBar.PanelEx panelQCInfo;
        private DevComponents.DotNetBar.LabelX lblSamplePercent;
        private DevComponents.DotNetBar.LabelX lblProcessTable;
        private DevComponents.DotNetBar.LabelX lblProcessDate;
        private DevComponents.DotNetBar.LabelX lblProcessType;
    }
}