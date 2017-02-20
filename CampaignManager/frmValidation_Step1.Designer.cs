namespace GCC
{
    partial class frmValidation_Step1
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
            this.metroTilePanel1 = new DevComponents.DotNetBar.Metro.MetroTilePanel();
            this.itemContainerTRValidations = new DevComponents.DotNetBar.ItemContainer();
            this.itemContainerWRValidations = new DevComponents.DotNetBar.ItemContainer();
            this.SuspendLayout();
            // 
            // metroTilePanel1
            // 
            // 
            // 
            // 
            this.metroTilePanel1.BackgroundStyle.Class = "MetroTilePanel";
            this.metroTilePanel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTilePanel1.ContainerControlProcessDialogKey = true;
            this.metroTilePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTilePanel1.DragDropSupport = true;
            this.metroTilePanel1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.itemContainerTRValidations,
            this.itemContainerWRValidations});
            this.metroTilePanel1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.metroTilePanel1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.metroTilePanel1.Location = new System.Drawing.Point(0, 0);
            this.metroTilePanel1.Name = "metroTilePanel1";
            this.metroTilePanel1.Size = new System.Drawing.Size(938, 779);
            this.metroTilePanel1.TabIndex = 0;
            this.metroTilePanel1.Text = "metroTilePanel1";
            this.metroTilePanel1.TouchEnabled = false;
            // 
            // itemContainerTRValidations
            // 
            // 
            // 
            // 
            this.itemContainerTRValidations.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainerTRValidations.MultiLine = true;
            this.itemContainerTRValidations.Name = "itemContainerTRValidations";
            // 
            // 
            // 
            this.itemContainerTRValidations.TitleStyle.Class = "MetroTileGroupTitle";
            this.itemContainerTRValidations.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainerTRValidations.TitleText = "TR Validations";
            // 
            // itemContainerWRValidations
            // 
            // 
            // 
            // 
            this.itemContainerWRValidations.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainerWRValidations.MultiLine = true;
            this.itemContainerWRValidations.Name = "itemContainerWRValidations";
            // 
            // 
            // 
            this.itemContainerWRValidations.TitleStyle.Class = "MetroTileGroupTitle";
            this.itemContainerWRValidations.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainerWRValidations.TitleText = "WR Validations";
            // 
            // frmValidation_Step1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 779);
            this.Controls.Add(this.metroTilePanel1);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.Name = "frmValidation_Step1";
            this.Text = "Validation_Table";
            this.Load += new System.EventHandler(this.Validation_Table_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Metro.MetroTilePanel metroTilePanel1;
        private DevComponents.DotNetBar.ItemContainer itemContainerWRValidations;
        private DevComponents.DotNetBar.ItemContainer itemContainerTRValidations;
    }
}