namespace GCC
{
    partial class frmCallScript
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
            this.rtxtCallScript = new DevComponents.DotNetBar.Controls.RichTextBoxEx();
            this.splitScriptContainer = new System.Windows.Forms.SplitContainer();
            this.btnDialCallScript = new DevComponents.DotNetBar.ButtonX();
            this.txtDialCallScript = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnResizeWindow = new DevComponents.DotNetBar.ButtonX();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitScriptContainer)).BeginInit();
            this.splitScriptContainer.Panel1.SuspendLayout();
            this.splitScriptContainer.Panel2.SuspendLayout();
            this.splitScriptContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtxtCallScript
            // 
            this.rtxtCallScript.BackColorRichTextBox = System.Drawing.Color.White;
            // 
            // 
            // 
            this.rtxtCallScript.BackgroundStyle.Class = "RichTextBoxBorder";
            this.rtxtCallScript.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.rtxtCallScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtCallScript.Location = new System.Drawing.Point(0, 0);
            this.rtxtCallScript.Name = "rtxtCallScript";
            this.rtxtCallScript.ReadOnly = true;
            this.rtxtCallScript.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
            this.rtxtCallScript.Size = new System.Drawing.Size(566, 521);
            this.rtxtCallScript.TabIndex = 0;
            // 
            // splitScriptContainer
            // 
            this.splitScriptContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitScriptContainer.Location = new System.Drawing.Point(0, 0);
            this.splitScriptContainer.Name = "splitScriptContainer";
            this.splitScriptContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitScriptContainer.Panel1
            // 
            this.splitScriptContainer.Panel1.Controls.Add(this.btnDialCallScript);
            this.splitScriptContainer.Panel1.Controls.Add(this.txtDialCallScript);
            this.splitScriptContainer.Panel1.Controls.Add(this.btnResizeWindow);
            // 
            // splitScriptContainer.Panel2
            // 
            this.splitScriptContainer.Panel2.Controls.Add(this.rtxtCallScript);
            this.splitScriptContainer.Panel2.Controls.Add(this.btnClose);
            this.splitScriptContainer.Size = new System.Drawing.Size(566, 561);
            this.splitScriptContainer.SplitterDistance = 36;
            this.splitScriptContainer.TabIndex = 1;
            // 
            // btnDialCallScript
            // 
            this.btnDialCallScript.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDialCallScript.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDialCallScript.Image = global::GCC.Properties.Resources.Phone_iconbig1;
            this.btnDialCallScript.Location = new System.Drawing.Point(145, 4);
            this.btnDialCallScript.Name = "btnDialCallScript";
            this.btnDialCallScript.Size = new System.Drawing.Size(29, 29);
            this.btnDialCallScript.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnDialCallScript.TabIndex = 2;
            // 
            // txtDialCallScript
            // 
            // 
            // 
            // 
            this.txtDialCallScript.Border.Class = "TextBoxBorder";
            this.txtDialCallScript.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtDialCallScript.Location = new System.Drawing.Point(3, 9);
            this.txtDialCallScript.Name = "txtDialCallScript";
            this.txtDialCallScript.PreventEnterBeep = true;
            this.txtDialCallScript.Size = new System.Drawing.Size(136, 20);
            this.txtDialCallScript.TabIndex = 1;
            // 
            // btnResizeWindow
            // 
            this.btnResizeWindow.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnResizeWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResizeWindow.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnResizeWindow.Location = new System.Drawing.Point(488, 3);
            this.btnResizeWindow.Name = "btnResizeWindow";
            this.btnResizeWindow.Size = new System.Drawing.Size(75, 23);
            this.btnResizeWindow.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnResizeWindow.TabIndex = 0;
            this.btnResizeWindow.Text = "Side by Side";
            this.btnResizeWindow.Click += new System.EventHandler(this.btnResizeWindow_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(425, 120);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmCallScript
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(566, 561);
            this.Controls.Add(this.splitScriptContainer);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.Name = "frmCallScript";
            this.Text = "Call Script";
            this.Load += new System.EventHandler(this.frmCallScript_Load);
            this.splitScriptContainer.Panel1.ResumeLayout(false);
            this.splitScriptContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitScriptContainer)).EndInit();
            this.splitScriptContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.RichTextBoxEx rtxtCallScript;
        private System.Windows.Forms.SplitContainer splitScriptContainer;
        private DevComponents.DotNetBar.ButtonX btnResizeWindow;
        private System.Windows.Forms.Button btnClose;
        public DevComponents.DotNetBar.Controls.TextBoxX txtDialCallScript;
        public DevComponents.DotNetBar.ButtonX btnDialCallScript;

    }
}