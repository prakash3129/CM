namespace GCC
{
    partial class frmNotifier
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
            this.circularProgressValidating = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.lblValidationMessage = new DevComponents.DotNetBar.LabelX();
            this.msgRefresh = new System.Windows.Forms.Timer(this.components);
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.pBoxPoint2 = new System.Windows.Forms.PictureBox();
            this.pBoxPoint3 = new System.Windows.Forms.PictureBox();
            this.pBoxPoint4 = new System.Windows.Forms.PictureBox();
            this.pBoxPoint5 = new System.Windows.Forms.PictureBox();
            this.pBoxPoint6 = new System.Windows.Forms.PictureBox();
            this.pBoxPoint1 = new System.Windows.Forms.PictureBox();
            this.pBoxPoint0 = new System.Windows.Forms.PictureBox();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint0)).BeginInit();
            this.SuspendLayout();
            // 
            // circularProgressValidating
            // 
            // 
            // 
            // 
            this.circularProgressValidating.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.circularProgressValidating.FocusCuesEnabled = false;
            this.circularProgressValidating.Location = new System.Drawing.Point(109, 178);
            this.circularProgressValidating.Name = "circularProgressValidating";
            this.circularProgressValidating.ProgressBarType = DevComponents.DotNetBar.eCircularProgressType.Donut;
            this.circularProgressValidating.Size = new System.Drawing.Size(19, 24);
            this.circularProgressValidating.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.circularProgressValidating.TabIndex = 0;
            // 
            // lblValidationMessage
            // 
            this.lblValidationMessage.AutoSize = true;
            // 
            // 
            // 
            this.lblValidationMessage.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblValidationMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.lblValidationMessage.Location = new System.Drawing.Point(12, 210);
            this.lblValidationMessage.Name = "lblValidationMessage";
            this.lblValidationMessage.Size = new System.Drawing.Size(46, 14);
            this.lblValidationMessage.TabIndex = 1;
            this.lblValidationMessage.Text = "Loading...";
            // 
            // msgRefresh
            // 
            this.msgRefresh.Enabled = true;
            this.msgRefresh.Interval = 300;
            this.msgRefresh.Tick += new System.EventHandler(this.msgRefresh_Tick);
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelX1.Location = new System.Drawing.Point(38, 71);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(175, 16);
            this.labelX1.TabIndex = 4;
            this.labelX1.Text = "Checking Switchboard Duplicate";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelX2.Location = new System.Drawing.Point(38, 44);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(181, 16);
            this.labelX2.TabIndex = 5;
            this.labelX2.Text = "Checking Company / Contact info";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelX3.Location = new System.Drawing.Point(38, 98);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(139, 16);
            this.labelX3.TabIndex = 6;
            this.labelX3.Text = "Checking Email Duplicate";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelX4.Location = new System.Drawing.Point(38, 17);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(134, 16);
            this.labelX4.TabIndex = 7;
            this.labelX4.Text = "Performaing auto update";
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelX5.Location = new System.Drawing.Point(38, 179);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(68, 16);
            this.labelX5.TabIndex = 8;
            this.labelX5.Text = "Saving Data";
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelX6.Location = new System.Drawing.Point(38, 152);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(189, 16);
            this.labelX6.TabIndex = 9;
            this.labelX6.Text = "Logging company and contact data";
            // 
            // labelX7
            // 
            this.labelX7.AutoSize = true;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelX7.Location = new System.Drawing.Point(38, 125);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(133, 16);
            this.labelX7.TabIndex = 10;
            this.labelX7.Text = "Tagging Date and Name";
            // 
            // pBoxPoint2
            // 
            this.pBoxPoint2.Image = global::GCC.Properties.Resources.loading_blue2;
            this.pBoxPoint2.Location = new System.Drawing.Point(12, 71);
            this.pBoxPoint2.Name = "pBoxPoint2";
            this.pBoxPoint2.Size = new System.Drawing.Size(20, 20);
            this.pBoxPoint2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBoxPoint2.TabIndex = 16;
            this.pBoxPoint2.TabStop = false;
            // 
            // pBoxPoint3
            // 
            this.pBoxPoint3.Image = global::GCC.Properties.Resources.loading_blue2;
            this.pBoxPoint3.Location = new System.Drawing.Point(12, 98);
            this.pBoxPoint3.Name = "pBoxPoint3";
            this.pBoxPoint3.Size = new System.Drawing.Size(20, 20);
            this.pBoxPoint3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBoxPoint3.TabIndex = 15;
            this.pBoxPoint3.TabStop = false;
            // 
            // pBoxPoint4
            // 
            this.pBoxPoint4.Image = global::GCC.Properties.Resources.loading_blue2;
            this.pBoxPoint4.Location = new System.Drawing.Point(12, 125);
            this.pBoxPoint4.Name = "pBoxPoint4";
            this.pBoxPoint4.Size = new System.Drawing.Size(20, 20);
            this.pBoxPoint4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBoxPoint4.TabIndex = 14;
            this.pBoxPoint4.TabStop = false;
            // 
            // pBoxPoint5
            // 
            this.pBoxPoint5.Image = global::GCC.Properties.Resources.loading_blue2;
            this.pBoxPoint5.Location = new System.Drawing.Point(12, 152);
            this.pBoxPoint5.Name = "pBoxPoint5";
            this.pBoxPoint5.Size = new System.Drawing.Size(20, 20);
            this.pBoxPoint5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBoxPoint5.TabIndex = 13;
            this.pBoxPoint5.TabStop = false;
            // 
            // pBoxPoint6
            // 
            this.pBoxPoint6.Image = global::GCC.Properties.Resources.loading_blue2;
            this.pBoxPoint6.Location = new System.Drawing.Point(12, 179);
            this.pBoxPoint6.Name = "pBoxPoint6";
            this.pBoxPoint6.Size = new System.Drawing.Size(20, 20);
            this.pBoxPoint6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBoxPoint6.TabIndex = 12;
            this.pBoxPoint6.TabStop = false;
            // 
            // pBoxPoint1
            // 
            this.pBoxPoint1.Image = global::GCC.Properties.Resources.loading_blue2;
            this.pBoxPoint1.Location = new System.Drawing.Point(12, 44);
            this.pBoxPoint1.Name = "pBoxPoint1";
            this.pBoxPoint1.Size = new System.Drawing.Size(20, 20);
            this.pBoxPoint1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBoxPoint1.TabIndex = 11;
            this.pBoxPoint1.TabStop = false;
            // 
            // pBoxPoint0
            // 
            this.pBoxPoint0.Image = global::GCC.Properties.Resources.loading_blue2;
            this.pBoxPoint0.Location = new System.Drawing.Point(12, 17);
            this.pBoxPoint0.Name = "pBoxPoint0";
            this.pBoxPoint0.Size = new System.Drawing.Size(20, 20);
            this.pBoxPoint0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBoxPoint0.TabIndex = 3;
            this.pBoxPoint0.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Image = global::GCC.Properties.Resources.Actions_application_exit_icon__1_;
            this.btnCancel.Location = new System.Drawing.Point(281, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(30, 30);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 2;
            // 
            // frmNotifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 234);
            this.Controls.Add(this.pBoxPoint2);
            this.Controls.Add(this.pBoxPoint3);
            this.Controls.Add(this.pBoxPoint4);
            this.Controls.Add(this.pBoxPoint5);
            this.Controls.Add(this.pBoxPoint6);
            this.Controls.Add(this.pBoxPoint1);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.pBoxPoint0);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblValidationMessage);
            this.Controls.Add(this.circularProgressValidating);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmNotifier";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPoint0)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.CircularProgress circularProgressValidating;
        public DevComponents.DotNetBar.ButtonX btnCancel;
        public DevComponents.DotNetBar.LabelX lblValidationMessage;
        public System.Windows.Forms.Timer msgRefresh;
        private System.Windows.Forms.PictureBox pBoxPoint0;
        public DevComponents.DotNetBar.LabelX labelX1;
        public DevComponents.DotNetBar.LabelX labelX2;
        public DevComponents.DotNetBar.LabelX labelX3;
        public DevComponents.DotNetBar.LabelX labelX4;
        public DevComponents.DotNetBar.LabelX labelX5;
        public DevComponents.DotNetBar.LabelX labelX6;
        public DevComponents.DotNetBar.LabelX labelX7;
        private System.Windows.Forms.PictureBox pBoxPoint1;
        private System.Windows.Forms.PictureBox pBoxPoint6;
        private System.Windows.Forms.PictureBox pBoxPoint5;
        private System.Windows.Forms.PictureBox pBoxPoint4;
        private System.Windows.Forms.PictureBox pBoxPoint3;
        private System.Windows.Forms.PictureBox pBoxPoint2;
    }
}