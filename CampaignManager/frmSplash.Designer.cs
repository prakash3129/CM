namespace GCC
{
    partial class frmSplash
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
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.pictureBoxCM = new System.Windows.Forms.PictureBox();
            this.pictureBoxLoading = new System.Windows.Forms.PictureBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.pictureBoxMeritLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMeritLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxIcon.Image = global::GCC.Properties.Resources.Businessman_icon__1_;
            this.pictureBoxIcon.Location = new System.Drawing.Point(-1, -11);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new System.Drawing.Size(204, 277);
            this.pictureBoxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxIcon.TabIndex = 0;
            this.pictureBoxIcon.TabStop = false;
            // 
            // pictureBoxCM
            // 
            this.pictureBoxCM.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxCM.Image = global::GCC.Properties.Resources._9__5_;
            this.pictureBoxCM.Location = new System.Drawing.Point(193, 199);
            this.pictureBoxCM.Name = "pictureBoxCM";
            this.pictureBoxCM.Size = new System.Drawing.Size(281, 50);
            this.pictureBoxCM.TabIndex = 1;
            this.pictureBoxCM.TabStop = false;
            // 
            // pictureBoxLoading
            // 
            this.pictureBoxLoading.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLoading.Image = global::GCC.Properties.Resources.GCNyjJY;
            this.pictureBoxLoading.Location = new System.Drawing.Point(372, 2);
            this.pictureBoxLoading.Name = "pictureBoxLoading";
            this.pictureBoxLoading.Size = new System.Drawing.Size(100, 100);
            this.pictureBoxLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLoading.TabIndex = 2;
            this.pictureBoxLoading.TabStop = false;
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(387, 248);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblVersion.Size = new System.Drawing.Size(75, 13);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "v 2.0.0 Build 1";
            // 
            // pictureBoxMeritLogo
            // 
            this.pictureBoxMeritLogo.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxMeritLogo.Image = global::GCC.Properties.Resources.Merrit_Logo_with_claim_final;
            this.pictureBoxMeritLogo.Location = new System.Drawing.Point(237, 109);
            this.pictureBoxMeritLogo.Name = "pictureBoxMeritLogo";
            this.pictureBoxMeritLogo.Size = new System.Drawing.Size(180, 69);
            this.pictureBoxMeritLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxMeritLogo.TabIndex = 4;
            this.pictureBoxMeritLogo.TabStop = false;
            // 
            // frmSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GCC.Properties.Resources.abstract_design___Copy1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(475, 274);
            this.Controls.Add(this.pictureBoxCM);
            this.Controls.Add(this.pictureBoxMeritLogo);
            this.Controls.Add(this.pictureBoxIcon);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.pictureBoxLoading);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSplash";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSplash_FormClosing);
            this.Load += new System.EventHandler(this.frmSplash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMeritLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private System.Windows.Forms.PictureBox pictureBoxCM;
        private System.Windows.Forms.PictureBox pictureBoxLoading;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.PictureBox pictureBoxMeritLogo;
    }
}