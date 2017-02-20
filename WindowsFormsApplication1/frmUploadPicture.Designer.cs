namespace GCC
{
    partial class frmUploadPicture
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
            this.splitDP = new System.Windows.Forms.SplitContainer();
            this.pictureDP = new System.Windows.Forms.PictureBox();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnRotate = new DevComponents.DotNetBar.ButtonX();
            this.btnReset = new DevComponents.DotNetBar.ButtonX();
            this.Tooltip = new DevComponents.DotNetBar.SuperTooltip();
            this.lblPreview = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.splitDP)).BeginInit();
            this.splitDP.Panel1.SuspendLayout();
            this.splitDP.Panel2.SuspendLayout();
            this.splitDP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // splitDP
            // 
            this.splitDP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitDP.IsSplitterFixed = true;
            this.splitDP.Location = new System.Drawing.Point(0, 0);
            this.splitDP.Name = "splitDP";
            // 
            // splitDP.Panel1
            // 
            this.splitDP.Panel1.Controls.Add(this.pictureDP);
            // 
            // splitDP.Panel2
            // 
            this.splitDP.Panel2.Controls.Add(this.lblPreview);
            this.splitDP.Panel2.Controls.Add(this.pictureBoxPreview);
            this.splitDP.Panel2.Controls.Add(this.btnCancel);
            this.splitDP.Panel2.Controls.Add(this.btnSave);
            this.splitDP.Panel2.Controls.Add(this.btnRotate);
            this.splitDP.Panel2.Controls.Add(this.btnReset);
            this.splitDP.Size = new System.Drawing.Size(687, 467);
            this.splitDP.SplitterDistance = 528;
            this.splitDP.TabIndex = 0;
            // 
            // pictureDP
            // 
            this.pictureDP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureDP.Location = new System.Drawing.Point(0, 0);
            this.pictureDP.Name = "pictureDP";
            this.pictureDP.Size = new System.Drawing.Size(528, 467);
            this.pictureDP.TabIndex = 0;
            this.pictureDP.TabStop = false;
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPreview.Location = new System.Drawing.Point(12, 308);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(131, 147);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPreview.TabIndex = 4;
            this.pictureBoxPreview.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::GCC.Properties.Resources.Button_Delete_icon;
            this.btnCancel.Location = new System.Drawing.Point(82, 100);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(45, 45);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.Tooltip.SetSuperTooltip(this.btnCancel, new DevComponents.DotNetBar.SuperTooltipInfo("Close", "", "Cancel Image Edit", null, null, DevComponents.DotNetBar.eTooltipColor.Teal));
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Image = global::GCC.Properties.Resources.Actions_stock_save_as_icon__1_;
            this.btnSave.Location = new System.Drawing.Point(12, 100);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(45, 45);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.Tooltip.SetSuperTooltip(this.btnSave, new DevComponents.DotNetBar.SuperTooltipInfo("Save", "", "Saves your Image", null, null, DevComponents.DotNetBar.eTooltipColor.Teal));
            this.btnSave.TabIndex = 2;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRotate
            // 
            this.btnRotate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRotate.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRotate.Image = global::GCC.Properties.Resources.shape_rotate_clockwise_icon__1_;
            this.btnRotate.Location = new System.Drawing.Point(82, 18);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(45, 44);
            this.btnRotate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.Tooltip.SetSuperTooltip(this.btnRotate, new DevComponents.DotNetBar.SuperTooltipInfo("Rotate", "", "Rotates the Image in Clockwise Direction.", null, null, DevComponents.DotNetBar.eTooltipColor.Teal));
            this.btnRotate.TabIndex = 1;
            this.btnRotate.Click += new System.EventHandler(this.btnRotate_Click);
            // 
            // btnReset
            // 
            this.btnReset.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReset.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReset.Image = global::GCC.Properties.Resources.Actions_view_refresh_icon;
            this.btnReset.Location = new System.Drawing.Point(12, 17);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(45, 45);
            this.btnReset.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.Tooltip.SetSuperTooltip(this.btnReset, new DevComponents.DotNetBar.SuperTooltipInfo("Reload", "", "Resets Image to initial state.", null, null, DevComponents.DotNetBar.eTooltipColor.Teal));
            this.btnReset.TabIndex = 0;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // Tooltip
            // 
            this.Tooltip.DefaultTooltipSettings = new DevComponents.DotNetBar.SuperTooltipInfo("", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray);
            this.Tooltip.HoverDelayMultiplier = 0;
            this.Tooltip.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            // 
            // lblPreview
            // 
            this.lblPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            // 
            // 
            // 
            this.lblPreview.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPreview.Location = new System.Drawing.Point(13, 280);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(75, 23);
            this.lblPreview.TabIndex = 5;
            this.lblPreview.Text = "Preview";
            // 
            // frmUploadPicture
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(687, 467);
            this.Controls.Add(this.splitDP);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmUploadPicture";
            this.Text = "Upload Picture";
            this.Load += new System.EventHandler(this.frmUploadPicture_Load);
            this.splitDP.Panel1.ResumeLayout(false);
            this.splitDP.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitDP)).EndInit();
            this.splitDP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureDP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitDP;
        private System.Windows.Forms.PictureBox pictureDP;
        private DevComponents.DotNetBar.ButtonX btnReset;
        private DevComponents.DotNetBar.ButtonX btnRotate;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.SuperTooltip Tooltip;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private DevComponents.DotNetBar.LabelX lblPreview;
    }
}