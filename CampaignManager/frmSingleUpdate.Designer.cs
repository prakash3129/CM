namespace GCC
{
    partial class frmSingleUpdate
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
            this.panelControls = new DevComponents.DotNetBar.PanelEx();
            this.sdgvContactGrid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.SuspendLayout();
            // 
            // panelControls
            // 
            this.panelControls.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelControls.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelControls.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(1201, 87);
            this.panelControls.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelControls.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelControls.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelControls.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelControls.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelControls.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelControls.Style.GradientAngle = 90;
            this.panelControls.TabIndex = 0;
            // 
            // sdgvContactGrid
            // 
            this.sdgvContactGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvContactGrid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvContactGrid.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvContactGrid.Location = new System.Drawing.Point(0, 87);
            this.sdgvContactGrid.Name = "sdgvContactGrid";
            this.sdgvContactGrid.Size = new System.Drawing.Size(1201, 419);
            this.sdgvContactGrid.TabIndex = 1;
            this.sdgvContactGrid.Text = "superGridControl1";
            // 
            // frmSingleUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1201, 506);
            this.Controls.Add(this.sdgvContactGrid);
            this.Controls.Add(this.panelControls);
            this.Name = "frmSingleUpdate";
            this.Text = "Single Update";
            this.Load += new System.EventHandler(this.frmSingleUpdate_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelControls;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvContactGrid;
    }
}