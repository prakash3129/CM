namespace GCC
{
    partial class frmExceptionMonitor
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
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn2 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.splitL1 = new System.Windows.Forms.SplitContainer();
            this.superGridErrorMain = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.splitL2 = new System.Windows.Forms.SplitContainer();
            this.superGridErrorSub = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitL1)).BeginInit();
            this.splitL1.Panel1.SuspendLayout();
            this.splitL1.Panel2.SuspendLayout();
            this.splitL1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitL2)).BeginInit();
            this.splitL2.Panel1.SuspendLayout();
            this.splitL2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitL1
            // 
            this.splitL1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitL1.Location = new System.Drawing.Point(0, 0);
            this.splitL1.Name = "splitL1";
            // 
            // splitL1.Panel1
            // 
            this.splitL1.Panel1.Controls.Add(this.superGridErrorMain);
            // 
            // splitL1.Panel2
            // 
            this.splitL1.Panel2.Controls.Add(this.splitL2);
            this.splitL1.Size = new System.Drawing.Size(1515, 809);
            this.splitL1.SplitterDistance = 476;
            this.splitL1.TabIndex = 0;
            // 
            // superGridErrorMain
            // 
            this.superGridErrorMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superGridErrorMain.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.superGridErrorMain.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.superGridErrorMain.Location = new System.Drawing.Point(0, 0);
            this.superGridErrorMain.Name = "superGridErrorMain";
            this.superGridErrorMain.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.superGridErrorMain.PrimaryGrid.ColumnHeaderClickBehavior = DevComponents.DotNetBar.SuperGrid.ColumnHeaderClickBehavior.None;
            gridColumn2.AllowEdit = false;
            gridColumn2.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            gridColumn2.DataPropertyName = "Message";
            gridColumn2.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn2.Name = "Messsage";
            gridColumn2.ReadOnly = true;
            this.superGridErrorMain.PrimaryGrid.Columns.Add(gridColumn2);
            this.superGridErrorMain.PrimaryGrid.ShowRowHeaders = false;
            this.superGridErrorMain.Size = new System.Drawing.Size(476, 809);
            this.superGridErrorMain.TabIndex = 0;
            this.superGridErrorMain.Text = "superGridControl1";
            this.superGridErrorMain.DataBindingComplete += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs>(this.superGridErrorMain_DataBindingComplete);
            this.superGridErrorMain.RowActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowActivatedEventArgs>(this.superGridErrorMain_RowActivated);
            // 
            // splitL2
            // 
            this.splitL2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitL2.Location = new System.Drawing.Point(0, 0);
            this.splitL2.Name = "splitL2";
            // 
            // splitL2.Panel1
            // 
            this.splitL2.Panel1.Controls.Add(this.superGridErrorSub);
            this.splitL2.Size = new System.Drawing.Size(1035, 809);
            this.splitL2.SplitterDistance = 308;
            this.splitL2.TabIndex = 0;
            // 
            // superGridErrorSub
            // 
            this.superGridErrorSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superGridErrorSub.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.superGridErrorSub.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.superGridErrorSub.Location = new System.Drawing.Point(0, 0);
            this.superGridErrorSub.Name = "superGridErrorSub";
            this.superGridErrorSub.Size = new System.Drawing.Size(308, 809);
            this.superGridErrorSub.TabIndex = 1;
            this.superGridErrorSub.Text = "superGridControl1";
            // 
            // frmExceptionMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1515, 809);
            this.Controls.Add(this.splitL1);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.Name = "frmExceptionMonitor";
            this.Text = "Exception Monitor";
            this.Load += new System.EventHandler(this.ExceptionMonitor_Load);
            this.splitL1.Panel1.ResumeLayout(false);
            this.splitL1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitL1)).EndInit();
            this.splitL1.ResumeLayout(false);
            this.splitL2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitL2)).EndInit();
            this.splitL2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitL1;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl superGridErrorMain;
        private System.Windows.Forms.SplitContainer splitL2;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl superGridErrorSub;
    }
}