namespace GCC
{
    partial class frmScreen
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
            RemoteViewing.Vnc.VncClient vncClient1 = new RemoteViewing.Vnc.VncClient();
            this.vncControl = new RemoteViewing.Windows.Forms.VncControl();
            this.SuspendLayout();
            // 
            // vncControl
            // 
            this.vncControl.AllowClipboardSharingFromServer = false;
            this.vncControl.AllowClipboardSharingToServer = false;
            this.vncControl.AllowInput = true;
            this.vncControl.AllowRemoteCursor = true;
            this.vncControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vncControl.BackColor = System.Drawing.Color.Black;
            vncClient1.MaxUpdateRate = 15D;
            vncClient1.UserData = null;
            this.vncControl.Client = vncClient1;
            this.vncControl.Location = new System.Drawing.Point(12, 12);
            this.vncControl.Name = "vncControl";
            this.vncControl.Size = new System.Drawing.Size(793, 517);
            this.vncControl.TabIndex = 0;
            // 
            // frmScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 541);
            this.Controls.Add(this.vncControl);
            this.DoubleBuffered = true;
            this.Name = "frmScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Screen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmScreen_FormClosing);
            this.Load += new System.EventHandler(this.frmScreen_Load);
            this.Shown += new System.EventHandler(this.frmScreen_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private RemoteViewing.Windows.Forms.VncControl vncControl;
    }
}