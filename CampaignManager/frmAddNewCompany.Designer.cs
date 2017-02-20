namespace GCC
{
    partial class frmAddNewCompany
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
            this.dgvCompanyList = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.txtCompanyName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblCompanyName = new DevComponents.DotNetBar.LabelX();
            this.btnCreate = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.cmbSearchin = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblSearchIn = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompanyList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCompanyList
            // 
            this.dgvCompanyList.AllowUserToAddRows = false;
            this.dgvCompanyList.AllowUserToDeleteRows = false;
            this.dgvCompanyList.AllowUserToOrderColumns = true;
            this.dgvCompanyList.AllowUserToResizeRows = false;
            this.dgvCompanyList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCompanyList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.dgvCompanyList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCompanyList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCompanyList.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCompanyList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvCompanyList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvCompanyList.Location = new System.Drawing.Point(0, 50);
            this.dgvCompanyList.Name = "dgvCompanyList";
            this.dgvCompanyList.ReadOnly = true;
            this.dgvCompanyList.RowHeadersVisible = false;
            this.dgvCompanyList.Size = new System.Drawing.Size(886, 232);
            this.dgvCompanyList.TabIndex = 4;
            this.dgvCompanyList.TabStop = false;
            this.dgvCompanyList.DataSourceChanged += new System.EventHandler(this.dgvCompanyList_DataSourceChanged);
            // 
            // txtCompanyName
            // 
            // 
            // 
            // 
            this.txtCompanyName.Border.Class = "TextBoxBorder";
            this.txtCompanyName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCompanyName.Location = new System.Drawing.Point(93, 6);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.PreventEnterBeep = true;
            this.txtCompanyName.Size = new System.Drawing.Size(397, 20);
            this.txtCompanyName.TabIndex = 0;
            this.txtCompanyName.TextChanged += new System.EventHandler(this.txtCompanyName_TextChanged);
            // 
            // lblCompanyName
            // 
            // 
            // 
            // 
            this.lblCompanyName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblCompanyName.Location = new System.Drawing.Point(12, 3);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(75, 23);
            this.lblCompanyName.TabIndex = 2;
            this.lblCompanyName.Text = "New Company";
            // 
            // btnCreate
            // 
            this.btnCreate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCreate.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCreate.Location = new System.Drawing.Point(723, 3);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 36);
            this.btnCreate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCreate.TabIndex = 2;
            this.btnCreate.Text = "Search";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(804, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 36);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            // 
            // cmbSearchin
            // 
            this.cmbSearchin.DisplayMember = "Text";
            this.cmbSearchin.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSearchin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchin.FormattingEnabled = true;
            this.cmbSearchin.ItemHeight = 14;
            this.cmbSearchin.Location = new System.Drawing.Point(565, 6);
            this.cmbSearchin.Name = "cmbSearchin";
            this.cmbSearchin.Size = new System.Drawing.Size(148, 20);
            this.cmbSearchin.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbSearchin.TabIndex = 1;
            this.cmbSearchin.SelectedIndexChanged += new System.EventHandler(this.cmbSearchin_SelectedIndexChanged);
            // 
            // lblSearchIn
            // 
            // 
            // 
            // 
            this.lblSearchIn.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblSearchIn.Location = new System.Drawing.Point(511, 3);
            this.lblSearchIn.Name = "lblSearchIn";
            this.lblSearchIn.Size = new System.Drawing.Size(50, 23);
            this.lblSearchIn.TabIndex = 5;
            this.lblSearchIn.Text = "Search in";
            // 
            // frmAddNewCompany
            // 
            this.AcceptButton = this.btnCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(886, 282);
            this.Controls.Add(this.lblSearchIn);
            this.Controls.Add(this.cmbSearchin);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.lblCompanyName);
            this.Controls.Add(this.txtCompanyName);
            this.Controls.Add(this.dgvCompanyList);
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmAddNewCompany";
            this.Text = "New Company";
            this.Load += new System.EventHandler(this.frmAddNewCompany_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompanyList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgvCompanyList;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCompanyName;
        private DevComponents.DotNetBar.LabelX lblCompanyName;
        private DevComponents.DotNetBar.ButtonX btnCreate;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSearchin;
        private DevComponents.DotNetBar.LabelX lblSearchIn;
    }
}