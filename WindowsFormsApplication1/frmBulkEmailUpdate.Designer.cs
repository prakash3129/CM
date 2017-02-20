namespace GCC
{
    partial class frmBulkEmailUpdate
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitBulkEmailUpdate = new System.Windows.Forms.SplitContainer();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnUpdateEmail = new DevComponents.DotNetBar.ButtonX();
            this.chkUpdateOnlyBlank = new System.Windows.Forms.CheckBox();
            this.lblInvalid = new DevComponents.DotNetBar.LabelX();
            this.cmbEmailFormat = new GCC.MultiColumnComboBox();
            this.lblEmailOut = new DevComponents.DotNetBar.LabelX();
            this.cmbDomainlist = new System.Windows.Forms.ComboBox();
            this.lblDomain = new DevComponents.DotNetBar.LabelX();
            this.lblEmailFormat = new DevComponents.DotNetBar.LabelX();
            this.txtMiddleName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtMiddleNameFirstLetter = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtLastNameFirstLetter = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtEmail = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtFirstNameFirstLetter = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtLastName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtFirstName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblMiddleName = new DevComponents.DotNetBar.LabelX();
            this.lblEmail = new DevComponents.DotNetBar.LabelX();
            this.lblLastname = new DevComponents.DotNetBar.LabelX();
            this.lblFirstName = new DevComponents.DotNetBar.LabelX();
            this.dgvBulkEmailUpdate = new DevComponents.DotNetBar.Controls.DataGridViewX();
            ((System.ComponentModel.ISupportInitialize)(this.splitBulkEmailUpdate)).BeginInit();
            this.splitBulkEmailUpdate.Panel1.SuspendLayout();
            this.splitBulkEmailUpdate.Panel2.SuspendLayout();
            this.splitBulkEmailUpdate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBulkEmailUpdate)).BeginInit();
            this.SuspendLayout();
            // 
            // splitBulkEmailUpdate
            // 
            this.splitBulkEmailUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBulkEmailUpdate.IsSplitterFixed = true;
            this.splitBulkEmailUpdate.Location = new System.Drawing.Point(0, 0);
            this.splitBulkEmailUpdate.Name = "splitBulkEmailUpdate";
            this.splitBulkEmailUpdate.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitBulkEmailUpdate.Panel1
            // 
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.btnCancel);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.btnUpdateEmail);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.chkUpdateOnlyBlank);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.lblInvalid);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.cmbEmailFormat);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.lblEmailOut);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.cmbDomainlist);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.lblDomain);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.lblEmailFormat);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.txtMiddleName);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.txtMiddleNameFirstLetter);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.txtLastNameFirstLetter);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.txtEmail);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.txtFirstNameFirstLetter);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.txtLastName);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.txtFirstName);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.lblMiddleName);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.lblEmail);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.lblLastname);
            this.splitBulkEmailUpdate.Panel1.Controls.Add(this.lblFirstName);
            // 
            // splitBulkEmailUpdate.Panel2
            // 
            this.splitBulkEmailUpdate.Panel2.Controls.Add(this.dgvBulkEmailUpdate);
            this.splitBulkEmailUpdate.Size = new System.Drawing.Size(702, 325);
            this.splitBulkEmailUpdate.SplitterDistance = 106;
            this.splitBulkEmailUpdate.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(615, 75);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdateEmail
            // 
            this.btnUpdateEmail.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUpdateEmail.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnUpdateEmail.Location = new System.Drawing.Point(615, 46);
            this.btnUpdateEmail.Name = "btnUpdateEmail";
            this.btnUpdateEmail.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateEmail.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnUpdateEmail.TabIndex = 23;
            this.btnUpdateEmail.Text = "&Update";
            this.btnUpdateEmail.Click += new System.EventHandler(this.btnUpdateEmail_Click);
            // 
            // chkUpdateOnlyBlank
            // 
            this.chkUpdateOnlyBlank.AutoSize = true;
            this.chkUpdateOnlyBlank.Location = new System.Drawing.Point(564, 6);
            this.chkUpdateOnlyBlank.Name = "chkUpdateOnlyBlank";
            this.chkUpdateOnlyBlank.Size = new System.Drawing.Size(113, 17);
            this.chkUpdateOnlyBlank.TabIndex = 3;
            this.chkUpdateOnlyBlank.Text = "U&pdate only Blank";
            this.chkUpdateOnlyBlank.UseVisualStyleBackColor = true;
            this.chkUpdateOnlyBlank.Visible = false;
            this.chkUpdateOnlyBlank.CheckedChanged += new System.EventHandler(this.chkUpdateOnlyBlank_CheckedChanged);
            // 
            // lblInvalid
            // 
            // 
            // 
            // 
            this.lblInvalid.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblInvalid.ForeColor = System.Drawing.Color.Red;
            this.lblInvalid.Location = new System.Drawing.Point(316, 80);
            this.lblInvalid.Name = "lblInvalid";
            this.lblInvalid.Size = new System.Drawing.Size(108, 14);
            this.lblInvalid.TabIndex = 21;
            this.lblInvalid.Text = "Invalid Email Format";
            this.lblInvalid.Visible = false;
            // 
            // cmbEmailFormat
            // 
            this.cmbEmailFormat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbEmailFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmailFormat.DropDownWidth = 200;
            this.cmbEmailFormat.FormattingEnabled = true;
            this.cmbEmailFormat.Location = new System.Drawing.Point(403, 6);
            this.cmbEmailFormat.Name = "cmbEmailFormat";
            this.cmbEmailFormat.Size = new System.Drawing.Size(154, 21);
            this.cmbEmailFormat.TabIndex = 1;
            this.cmbEmailFormat.SelectedIndexChanged += new System.EventHandler(this.cmbEmailFormat_SelectedIndexChanged);
            // 
            // lblEmailOut
            // 
            this.lblEmailOut.AutoSize = true;
            // 
            // 
            // 
            this.lblEmailOut.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblEmailOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblEmailOut.Location = new System.Drawing.Point(316, 56);
            this.lblEmailOut.Name = "lblEmailOut";
            this.lblEmailOut.PaddingTop = 3;
            this.lblEmailOut.Size = new System.Drawing.Size(198, 22);
            this.lblEmailOut.TabIndex = 16;
            this.lblEmailOut.Text = "thangaprakash.manivannan@";
            this.lblEmailOut.TextChanged += new System.EventHandler(this.lblEmailOut_TextChanged);
            // 
            // cmbDomainlist
            // 
            this.cmbDomainlist.FormattingEnabled = true;
            this.cmbDomainlist.Location = new System.Drawing.Point(403, 33);
            this.cmbDomainlist.Name = "cmbDomainlist";
            this.cmbDomainlist.Size = new System.Drawing.Size(154, 21);
            this.cmbDomainlist.TabIndex = 2;
            this.cmbDomainlist.SelectedIndexChanged += new System.EventHandler(this.cmbEmailFormat_SelectedIndexChanged);
            this.cmbDomainlist.TextChanged += new System.EventHandler(this.cmbEmailFormat_SelectedIndexChanged);
            // 
            // lblDomain
            // 
            // 
            // 
            // 
            this.lblDomain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblDomain.Location = new System.Drawing.Point(316, 36);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(40, 14);
            this.lblDomain.TabIndex = 18;
            this.lblDomain.Text = "Domain";
            // 
            // lblEmailFormat
            // 
            // 
            // 
            // 
            this.lblEmailFormat.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblEmailFormat.Location = new System.Drawing.Point(316, 10);
            this.lblEmailFormat.Name = "lblEmailFormat";
            this.lblEmailFormat.Size = new System.Drawing.Size(86, 14);
            this.lblEmailFormat.TabIndex = 15;
            this.lblEmailFormat.Text = "Detected Format";
            // 
            // txtMiddleName
            // 
            this.txtMiddleName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtMiddleName.Border.Class = "TextBoxBorder";
            this.txtMiddleName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtMiddleName.DisabledBackColor = System.Drawing.Color.White;
            this.txtMiddleName.Location = new System.Drawing.Point(115, 55);
            this.txtMiddleName.Name = "txtMiddleName";
            this.txtMiddleName.PreventEnterBeep = true;
            this.txtMiddleName.ReadOnly = true;
            this.txtMiddleName.Size = new System.Drawing.Size(184, 20);
            this.txtMiddleName.TabIndex = 14;
            this.txtMiddleName.TabStop = false;
            // 
            // txtMiddleNameFirstLetter
            // 
            this.txtMiddleNameFirstLetter.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtMiddleNameFirstLetter.Border.Class = "TextBoxBorder";
            this.txtMiddleNameFirstLetter.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtMiddleNameFirstLetter.DisabledBackColor = System.Drawing.Color.White;
            this.txtMiddleNameFirstLetter.Location = new System.Drawing.Point(83, 55);
            this.txtMiddleNameFirstLetter.Name = "txtMiddleNameFirstLetter";
            this.txtMiddleNameFirstLetter.PreventEnterBeep = true;
            this.txtMiddleNameFirstLetter.ReadOnly = true;
            this.txtMiddleNameFirstLetter.Size = new System.Drawing.Size(28, 20);
            this.txtMiddleNameFirstLetter.TabIndex = 13;
            this.txtMiddleNameFirstLetter.TabStop = false;
            // 
            // txtLastNameFirstLetter
            // 
            this.txtLastNameFirstLetter.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtLastNameFirstLetter.Border.Class = "TextBoxBorder";
            this.txtLastNameFirstLetter.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtLastNameFirstLetter.DisabledBackColor = System.Drawing.Color.White;
            this.txtLastNameFirstLetter.Location = new System.Drawing.Point(83, 31);
            this.txtLastNameFirstLetter.Name = "txtLastNameFirstLetter";
            this.txtLastNameFirstLetter.PreventEnterBeep = true;
            this.txtLastNameFirstLetter.ReadOnly = true;
            this.txtLastNameFirstLetter.Size = new System.Drawing.Size(28, 20);
            this.txtLastNameFirstLetter.TabIndex = 12;
            this.txtLastNameFirstLetter.TabStop = false;
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtEmail.Border.Class = "TextBoxBorder";
            this.txtEmail.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtEmail.DisabledBackColor = System.Drawing.Color.White;
            this.txtEmail.Location = new System.Drawing.Point(83, 79);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PreventEnterBeep = true;
            this.txtEmail.ReadOnly = true;
            this.txtEmail.Size = new System.Drawing.Size(216, 20);
            this.txtEmail.TabIndex = 11;
            this.txtEmail.TabStop = false;
            // 
            // txtFirstNameFirstLetter
            // 
            this.txtFirstNameFirstLetter.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtFirstNameFirstLetter.Border.Class = "TextBoxBorder";
            this.txtFirstNameFirstLetter.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtFirstNameFirstLetter.DisabledBackColor = System.Drawing.Color.White;
            this.txtFirstNameFirstLetter.Location = new System.Drawing.Point(83, 8);
            this.txtFirstNameFirstLetter.Name = "txtFirstNameFirstLetter";
            this.txtFirstNameFirstLetter.PreventEnterBeep = true;
            this.txtFirstNameFirstLetter.ReadOnly = true;
            this.txtFirstNameFirstLetter.Size = new System.Drawing.Size(28, 20);
            this.txtFirstNameFirstLetter.TabIndex = 10;
            this.txtFirstNameFirstLetter.TabStop = false;
            // 
            // txtLastName
            // 
            this.txtLastName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtLastName.Border.Class = "TextBoxBorder";
            this.txtLastName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtLastName.DisabledBackColor = System.Drawing.Color.White;
            this.txtLastName.Location = new System.Drawing.Point(115, 31);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.PreventEnterBeep = true;
            this.txtLastName.ReadOnly = true;
            this.txtLastName.Size = new System.Drawing.Size(184, 20);
            this.txtLastName.TabIndex = 9;
            this.txtLastName.TabStop = false;
            // 
            // txtFirstName
            // 
            this.txtFirstName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtFirstName.Border.Class = "TextBoxBorder";
            this.txtFirstName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtFirstName.DisabledBackColor = System.Drawing.Color.White;
            this.txtFirstName.Location = new System.Drawing.Point(115, 8);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.PreventEnterBeep = true;
            this.txtFirstName.ReadOnly = true;
            this.txtFirstName.Size = new System.Drawing.Size(184, 20);
            this.txtFirstName.TabIndex = 8;
            this.txtFirstName.TabStop = false;
            // 
            // lblMiddleName
            // 
            // 
            // 
            // 
            this.lblMiddleName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMiddleName.Location = new System.Drawing.Point(6, 57);
            this.lblMiddleName.Name = "lblMiddleName";
            this.lblMiddleName.Size = new System.Drawing.Size(69, 14);
            this.lblMiddleName.TabIndex = 7;
            this.lblMiddleName.Text = "Middle Name";
            // 
            // lblEmail
            // 
            // 
            // 
            // 
            this.lblEmail.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblEmail.Location = new System.Drawing.Point(6, 80);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(36, 14);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "Email";
            // 
            // lblLastname
            // 
            // 
            // 
            // 
            this.lblLastname.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblLastname.Location = new System.Drawing.Point(6, 33);
            this.lblLastname.Name = "lblLastname";
            this.lblLastname.Size = new System.Drawing.Size(56, 14);
            this.lblLastname.TabIndex = 1;
            this.lblLastname.Text = "Last Name";
            // 
            // lblFirstName
            // 
            // 
            // 
            // 
            this.lblFirstName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFirstName.Location = new System.Drawing.Point(6, 10);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(56, 14);
            this.lblFirstName.TabIndex = 0;
            this.lblFirstName.Text = "First Name";
            // 
            // dgvBulkEmailUpdate
            // 
            this.dgvBulkEmailUpdate.AllowUserToAddRows = false;
            this.dgvBulkEmailUpdate.AllowUserToDeleteRows = false;
            this.dgvBulkEmailUpdate.AllowUserToOrderColumns = true;
            this.dgvBulkEmailUpdate.AllowUserToResizeRows = false;
            this.dgvBulkEmailUpdate.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBulkEmailUpdate.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvBulkEmailUpdate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBulkEmailUpdate.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvBulkEmailUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBulkEmailUpdate.EnableHeadersVisualStyles = false;
            this.dgvBulkEmailUpdate.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.dgvBulkEmailUpdate.Location = new System.Drawing.Point(0, 0);
            this.dgvBulkEmailUpdate.Name = "dgvBulkEmailUpdate";
            this.dgvBulkEmailUpdate.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBulkEmailUpdate.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvBulkEmailUpdate.RowHeadersVisible = false;
            this.dgvBulkEmailUpdate.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBulkEmailUpdate.Size = new System.Drawing.Size(702, 215);
            this.dgvBulkEmailUpdate.TabIndex = 0;
            this.dgvBulkEmailUpdate.DataSourceChanged += new System.EventHandler(this.dgvBulkEmailUpdate_DataSourceChanged);
            this.dgvBulkEmailUpdate.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBulkEmailUpdate_CellContentClick);
            this.dgvBulkEmailUpdate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvBulkEmailUpdate_KeyPress);
            // 
            // frmBulkEmailUpdate
            // 
            this.AcceptButton = this.btnUpdateEmail;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(702, 325);
            this.Controls.Add(this.splitBulkEmailUpdate);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmBulkEmailUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bulk Email Update";
            this.Load += new System.EventHandler(this.frmBulkEmailUpdate_Load);
            this.splitBulkEmailUpdate.Panel1.ResumeLayout(false);
            this.splitBulkEmailUpdate.Panel1.PerformLayout();
            this.splitBulkEmailUpdate.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBulkEmailUpdate)).EndInit();
            this.splitBulkEmailUpdate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBulkEmailUpdate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitBulkEmailUpdate;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvBulkEmailUpdate;
        private DevComponents.DotNetBar.LabelX lblEmail;
        private DevComponents.DotNetBar.LabelX lblLastname;
        private DevComponents.DotNetBar.LabelX lblFirstName;
        private DevComponents.DotNetBar.LabelX lblMiddleName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMiddleName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMiddleNameFirstLetter;
        private DevComponents.DotNetBar.Controls.TextBoxX txtLastNameFirstLetter;
        private DevComponents.DotNetBar.Controls.TextBoxX txtEmail;
        private DevComponents.DotNetBar.Controls.TextBoxX txtFirstNameFirstLetter;
        private DevComponents.DotNetBar.Controls.TextBoxX txtLastName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtFirstName;
        private DevComponents.DotNetBar.LabelX lblEmailFormat;
        private DevComponents.DotNetBar.LabelX lblEmailOut;
        private System.Windows.Forms.ComboBox cmbDomainlist;
        private DevComponents.DotNetBar.LabelX lblDomain;
        private MultiColumnComboBox cmbEmailFormat;
        private DevComponents.DotNetBar.LabelX lblInvalid;
        private System.Windows.Forms.CheckBox chkUpdateOnlyBlank;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnUpdateEmail;
        
    }
}