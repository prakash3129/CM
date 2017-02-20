namespace GCC
{
    partial class frmWindowedNotes
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
            this.txtNotes = new DevComponents.DotNetBar.Controls.RichTextBoxEx();
            this.sliderFontSize = new DevComponents.DotNetBar.Controls.Slider();
            this.btnWindowed = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // txtNotes
            // 
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtNotes.BackgroundStyle.Class = "RichTextBoxBorder";
            this.txtNotes.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtNotes.Location = new System.Drawing.Point(1, -2);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
            this.txtNotes.Size = new System.Drawing.Size(384, 332);
            this.txtNotes.TabIndex = 0;
            // 
            // sliderFontSize
            // 
            this.sliderFontSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.sliderFontSize.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.sliderFontSize.FocusCuesEnabled = false;
            this.sliderFontSize.Location = new System.Drawing.Point(391, 27);
            this.sliderFontSize.Maximum = 40;
            this.sliderFontSize.Minimum = 7;
            this.sliderFontSize.Name = "sliderFontSize";
            this.sliderFontSize.Size = new System.Drawing.Size(40, 293);
            this.sliderFontSize.SliderOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.sliderFontSize.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.sliderFontSize.TabIndex = 1;
            this.sliderFontSize.Value = 0;
            this.sliderFontSize.ValueChanged += new System.EventHandler(this.sliderFontSize_ValueChanged);
            // 
            // btnWindowed
            // 
            this.btnWindowed.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnWindowed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWindowed.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnWindowed.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnWindowed.Image = global::GCC.Properties.Resources.load2_download_icon;
            this.btnWindowed.Location = new System.Drawing.Point(391, 12);
            this.btnWindowed.Name = "btnWindowed";
            this.btnWindowed.Size = new System.Drawing.Size(31, 47);
            this.btnWindowed.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnWindowed.TabIndex = 546;
            this.btnWindowed.Click += new System.EventHandler(this.btnWindowed_Click);
            // 
            // frmWindowedNotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnWindowed;
            this.ClientSize = new System.Drawing.Size(439, 330);
            this.Controls.Add(this.btnWindowed);
            this.Controls.Add(this.sliderFontSize);
            this.Controls.Add(this.txtNotes);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(455, 364);
            this.Name = "frmWindowedNotes";
            this.ShowIcon = false;
            this.Tag = "";
            this.Text = "Agent Notes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPanel_FormClosing);
            this.Load += new System.EventHandler(this.frmPanel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.Slider sliderFontSize;
        public DevComponents.DotNetBar.ButtonX btnWindowed;
        public DevComponents.DotNetBar.Controls.RichTextBoxEx txtNotes;


    }
}