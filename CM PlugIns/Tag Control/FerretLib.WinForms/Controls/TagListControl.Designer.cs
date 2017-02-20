namespace FerretLib.WinForms.Controls
{
    partial class TagListControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtTag = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tagPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // txtTag
            // 
            // 
            // 
            // 
            this.txtTag.Border.Class = "TextBoxBorder";
            this.txtTag.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTag.ButtonCustom.Visible = true;
            this.txtTag.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtTag.Location = new System.Drawing.Point(0, 0);
            this.txtTag.Name = "txtTag";
            this.txtTag.PreventEnterBeep = true;
            this.txtTag.Size = new System.Drawing.Size(516, 20);
            this.txtTag.TabIndex = 5;
            this.txtTag.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtTag_KeyUp);
            // 
            // tagPanel
            // 
            this.tagPanel.AutoScroll = true;
            this.tagPanel.BackColor = System.Drawing.Color.Transparent;
            this.tagPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagPanel.Location = new System.Drawing.Point(0, 20);
            this.tagPanel.Name = "tagPanel";
            this.tagPanel.Size = new System.Drawing.Size(516, 292);
            this.tagPanel.TabIndex = 6;
            // 
            // TagListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tagPanel);
            this.Controls.Add(this.txtTag);
            this.Name = "TagListControl";
            this.Size = new System.Drawing.Size(516, 312);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtTag;
        private System.Windows.Forms.FlowLayoutPanel tagPanel;

    }
}
