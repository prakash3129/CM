namespace GCC
{
    partial class frmUserMaster
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
            this.dgvUser = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.mnuGridEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.userPermessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.managerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.agentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userAccessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webRearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.teleResearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).BeginInit();
            this.mnuGridEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvUser
            // 
            this.dgvUser.AllowUserToAddRows = false;
            this.dgvUser.AllowUserToDeleteRows = false;
            this.dgvUser.AllowUserToOrderColumns = true;
            this.dgvUser.AllowUserToResizeRows = false;
            this.dgvUser.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUser.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUser.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUser.ContextMenuStrip = this.mnuGridEdit;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUser.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUser.EnableHeadersVisualStyles = false;
            this.dgvUser.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.dgvUser.Location = new System.Drawing.Point(0, 0);
            this.dgvUser.Name = "dgvUser";
            this.dgvUser.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUser.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvUser.RowHeadersVisible = false;
            this.dgvUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUser.Size = new System.Drawing.Size(368, 357);
            this.dgvUser.TabIndex = 0;
            this.dgvUser.DataSourceChanged += new System.EventHandler(this.dgvUser_DataSourceChanged);
            // 
            // mnuGridEdit
            // 
            this.mnuGridEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userPermessionToolStripMenuItem,
            this.userAccessToolStripMenuItem,
            this.addUserToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.mnuGridEdit.Name = "mnuGridEdit";
            this.mnuGridEdit.Size = new System.Drawing.Size(144, 92);
            // 
            // userPermessionToolStripMenuItem
            // 
            this.userPermessionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.managerToolStripMenuItem,
            this.agentToolStripMenuItem,
            this.qCToolStripMenuItem});
            this.userPermessionToolStripMenuItem.Image = global::GCC.Properties.Resources.Users_icon;
            this.userPermessionToolStripMenuItem.Name = "userPermessionToolStripMenuItem";
            this.userPermessionToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.userPermessionToolStripMenuItem.Text = "User Type";
            // 
            // managerToolStripMenuItem
            // 
            this.managerToolStripMenuItem.Image = global::GCC.Properties.Resources.Manager_icon;
            this.managerToolStripMenuItem.Name = "managerToolStripMenuItem";
            this.managerToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.managerToolStripMenuItem.Text = "Manager";
            this.managerToolStripMenuItem.Click += new System.EventHandler(this.managerToolStripMenuItem_Click);
            // 
            // agentToolStripMenuItem
            // 
            this.agentToolStripMenuItem.Image = global::GCC.Properties.Resources.receptionist_icon;
            this.agentToolStripMenuItem.Name = "agentToolStripMenuItem";
            this.agentToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.agentToolStripMenuItem.Text = "Agent";
            this.agentToolStripMenuItem.Click += new System.EventHandler(this.agentToolStripMenuItem_Click);
            // 
            // qCToolStripMenuItem
            // 
            this.qCToolStripMenuItem.Image = global::GCC.Properties.Resources.profile_check_icon;
            this.qCToolStripMenuItem.Name = "qCToolStripMenuItem";
            this.qCToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.qCToolStripMenuItem.Text = "QC";
            this.qCToolStripMenuItem.Click += new System.EventHandler(this.qCToolStripMenuItem_Click);
            // 
            // userAccessToolStripMenuItem
            // 
            this.userAccessToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.webRearchToolStripMenuItem,
            this.teleResearchToolStripMenuItem});
            this.userAccessToolStripMenuItem.Image = global::GCC.Properties.Resources.key_icon;
            this.userAccessToolStripMenuItem.Name = "userAccessToolStripMenuItem";
            this.userAccessToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.userAccessToolStripMenuItem.Text = "User Access";
            // 
            // webRearchToolStripMenuItem
            // 
            this.webRearchToolStripMenuItem.Image = global::GCC.Properties.Resources.Earth_icon;
            this.webRearchToolStripMenuItem.Name = "webRearchToolStripMenuItem";
            this.webRearchToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.webRearchToolStripMenuItem.Text = "Web Research";
            this.webRearchToolStripMenuItem.Click += new System.EventHandler(this.webRearchToolStripMenuItem_Click);
            // 
            // teleResearchToolStripMenuItem
            // 
            this.teleResearchToolStripMenuItem.Image = global::GCC.Properties.Resources.Devices_audio_headset_icon;
            this.teleResearchToolStripMenuItem.Name = "teleResearchToolStripMenuItem";
            this.teleResearchToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.teleResearchToolStripMenuItem.Text = "Tele Research";
            this.teleResearchToolStripMenuItem.Click += new System.EventHandler(this.teleResearchToolStripMenuItem_Click);
            // 
            // addUserToolStripMenuItem
            // 
            this.addUserToolStripMenuItem.Image = global::GCC.Properties.Resources.Actions_list_add_user_icon;
            this.addUserToolStripMenuItem.Name = "addUserToolStripMenuItem";
            this.addUserToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.addUserToolStripMenuItem.Text = "Add User";
            this.addUserToolStripMenuItem.Click += new System.EventHandler(this.addUserToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Image = global::GCC.Properties.Resources.Actions_list_remove_user_icon;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.removeToolStripMenuItem.Text = "Remove User";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(401, 120);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmUserMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(368, 357);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvUser);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(380, 390);
            this.Name = "frmUserMaster";
            this.Text = "User Control";
            this.Load += new System.EventHandler(this.frmUserMaster_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).EndInit();
            this.mnuGridEdit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgvUser;
        private System.Windows.Forms.ContextMenuStrip mnuGridEdit;
        private System.Windows.Forms.ToolStripMenuItem userPermessionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem managerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem agentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userAccessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem webRearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem teleResearchToolStripMenuItem;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolStripMenuItem addUserToolStripMenuItem;
    }
}