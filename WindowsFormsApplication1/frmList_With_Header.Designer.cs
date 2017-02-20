namespace GCC
{
    partial class frmList_With_Header
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
            this.txtSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtSelectedValue = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.sdgvSearch = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            // 
            // 
            // 
            this.txtSearch.Border.Class = "TextBoxBorder";
            this.txtSearch.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PreventEnterBeep = true;
            this.txtSearch.Size = new System.Drawing.Size(340, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // txtSelectedValue
            // 
            // 
            // 
            // 
            this.txtSelectedValue.Border.Class = "TextBoxBorder";
            this.txtSelectedValue.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSelectedValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSelectedValue.Location = new System.Drawing.Point(0, 20);
            this.txtSelectedValue.Name = "txtSelectedValue";
            this.txtSelectedValue.PreventEnterBeep = true;
            this.txtSelectedValue.Size = new System.Drawing.Size(340, 20);
            this.txtSelectedValue.TabIndex = 3;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(208, 91);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(208, 120);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // sdgvSearch
            // 
            this.sdgvSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvSearch.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvSearch.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvSearch.Location = new System.Drawing.Point(0, 40);
            this.sdgvSearch.Name = "sdgvSearch";
            this.sdgvSearch.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvSearch.PrimaryGrid.ColumnHeader.Visible = false;
            this.sdgvSearch.PrimaryGrid.MouseEditMode = DevComponents.DotNetBar.SuperGrid.MouseEditMode.None;
            this.sdgvSearch.PrimaryGrid.MultiSelect = false;
            this.sdgvSearch.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.sdgvSearch.PrimaryGrid.ShowColumnHeader = false;
            this.sdgvSearch.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvSearch.Size = new System.Drawing.Size(340, 404);
            this.sdgvSearch.TabIndex = 1;
            this.sdgvSearch.Text = "superGridControl1";
            this.sdgvSearch.RowActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowActivatedEventArgs>(this.sdgvSearch_RowActivated);
            this.sdgvSearch.RowDoubleClick += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowDoubleClickEventArgs>(this.sdgvSearch_RowDoubleClick);
            this.sdgvSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sdgvSearch_KeyDown);
            // 
            // frmList_With_Header
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(340, 444);
            this.Controls.Add(this.sdgvSearch);
            this.Controls.Add(this.txtSelectedValue);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtSearch);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmList_With_Header";
            this.Load += new System.EventHandler(this.frmList_With_Header_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtSearch;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSelectedValue;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvSearch;
        
    }
}