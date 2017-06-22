namespace GCC
{
    partial class frmHelp
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
            this.btnClose = new System.Windows.Forms.Button();
            this.rtxtHelp = new DevComponents.DotNetBar.Controls.RichTextBoxEx();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(491, 136);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // rtxtHelp
            // 
            // 
            // 
            // 
            this.rtxtHelp.BackgroundStyle.Class = "RichTextBoxBorder";
            this.rtxtHelp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.rtxtHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.rtxtHelp.Location = new System.Drawing.Point(0, 0);
            this.rtxtHelp.Name = "rtxtHelp";
            this.rtxtHelp.ReadOnly = true;
            this.rtxtHelp.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs24\\par\r\n}\r\n";
            this.rtxtHelp.Size = new System.Drawing.Size(498, 396);
            this.rtxtHelp.TabIndex = 0;
            // 
            // frmHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(498, 396);
            this.Controls.Add(this.rtxtHelp);
            this.Controls.Add(this.btnClose);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.Name = "frmHelp";
            this.Text = "Help";
            this.Load += new System.EventHandler(this.frmHelp_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.RichTextBoxEx rtxtHelp;
        private System.Windows.Forms.Button btnClose;
    }
}