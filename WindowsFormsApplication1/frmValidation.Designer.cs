namespace GCC
{
    partial class frmValidation
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
            DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn1 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmValidation));
            this.sdgvValidation = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.tagListControl1 = new FerretLib.WinForms.Controls.TagListControl();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.txtWhenCondition = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblField = new DevComponents.DotNetBar.LabelX();
            this.txtWhenField = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sdgvValidation
            // 
            this.sdgvValidation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvValidation.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvValidation.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvValidation.Location = new System.Drawing.Point(0, 0);
            this.sdgvValidation.Name = "sdgvValidation";
            gridColumn1.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridLabelXEditControl);
            gridColumn1.Name = "columnMessage";
            gridColumn1.Width = 400;
            this.sdgvValidation.PrimaryGrid.Columns.Add(gridColumn1);
            this.sdgvValidation.PrimaryGrid.DefaultRowHeight = 100;
            this.sdgvValidation.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvValidation.Size = new System.Drawing.Size(491, 815);
            this.sdgvValidation.TabIndex = 0;
            this.sdgvValidation.RowActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowActivatedEventArgs>(this.sdgvValidation_RowActivated);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.sdgvValidation);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.labelX6);
            this.splitContainer1.Panel2.Controls.Add(this.tagListControl1);
            this.splitContainer1.Panel2.Controls.Add(this.labelX5);
            this.splitContainer1.Panel2.Controls.Add(this.txtWhenCondition);
            this.splitContainer1.Panel2.Controls.Add(this.lblField);
            this.splitContainer1.Panel2.Controls.Add(this.txtWhenField);
            this.splitContainer1.Panel2.Controls.Add(this.buttonX1);
            this.splitContainer1.Panel2.Controls.Add(this.labelX3);
            this.splitContainer1.Panel2.Controls.Add(this.labelX4);
            this.splitContainer1.Panel2.Controls.Add(this.labelX1);
            this.splitContainer1.Panel2.Controls.Add(this.labelX2);
            this.splitContainer1.Size = new System.Drawing.Size(1474, 815);
            this.splitContainer1.SplitterDistance = 491;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 1;
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Location = new System.Drawing.Point(22, 154);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(31, 15);
            this.labelX6.TabIndex = 13;
            this.labelX6.Text = "Value";
            // 
            // tagListControl1
            // 
            this.tagListControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagListControl1.Location = new System.Drawing.Point(77, 151);
            this.tagListControl1.Name = "tagListControl1";
            this.tagListControl1.Size = new System.Drawing.Size(834, 347);
            this.tagListControl1.TabIndex = 12;
            this.tagListControl1.Tags = ((System.Collections.Generic.List<string>)(resources.GetObject("tagListControl1.Tags")));
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Location = new System.Drawing.Point(22, 114);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(49, 15);
            this.labelX5.TabIndex = 11;
            this.labelX5.Text = "Condition";
            // 
            // txtWhenCondition
            // 
            // 
            // 
            // 
            this.txtWhenCondition.Border.Class = "TextBoxBorder";
            this.txtWhenCondition.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtWhenCondition.ButtonCustom.Visible = true;
            this.txtWhenCondition.Location = new System.Drawing.Point(77, 111);
            this.txtWhenCondition.Name = "txtWhenCondition";
            this.txtWhenCondition.PreventEnterBeep = true;
            this.txtWhenCondition.Size = new System.Drawing.Size(201, 20);
            this.txtWhenCondition.TabIndex = 10;
            this.txtWhenCondition.ButtonCustomClick += new System.EventHandler(this.txtWhenCondition_ButtonCustomClick);
            // 
            // lblField
            // 
            this.lblField.AutoSize = true;
            // 
            // 
            // 
            this.lblField.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblField.Location = new System.Drawing.Point(22, 72);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(26, 15);
            this.lblField.TabIndex = 9;
            this.lblField.Text = "Field";
            // 
            // txtWhenField
            // 
            // 
            // 
            // 
            this.txtWhenField.Border.Class = "TextBoxBorder";
            this.txtWhenField.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtWhenField.ButtonCustom.Visible = true;
            this.txtWhenField.Location = new System.Drawing.Point(77, 71);
            this.txtWhenField.Name = "txtWhenField";
            this.txtWhenField.PreventEnterBeep = true;
            this.txtWhenField.Size = new System.Drawing.Size(201, 20);
            this.txtWhenField.TabIndex = 8;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(518, 85);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 7;
            this.buttonX1.Text = "buttonX1";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // labelX3
            // 
            this.labelX3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarCaptionText;
            this.labelX3.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelX3.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelX3.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelX3.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(22, 50);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(889, 3);
            this.labelX3.TabIndex = 5;
            this.labelX3.Text = ".";
            // 
            // labelX4
            // 
            this.labelX4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarCaptionText;
            this.labelX4.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelX4.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelX4.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelX4.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(22, 542);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(889, 3);
            this.labelX4.TabIndex = 4;
            this.labelX4.Text = ".";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.labelX1.Location = new System.Drawing.Point(22, 515);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(47, 21);
            this.labelX1.TabIndex = 3;
            this.labelX1.Text = "Action";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.labelX2.Location = new System.Drawing.Point(22, 24);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(44, 21);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "When";
            // 
            // frmValidation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1474, 815);
            this.Controls.Add(this.splitContainer1);
            this.EnableGlass = false;
            this.Name = "frmValidation";
            this.Text = "Rule Page";
            this.Load += new System.EventHandler(this.frmValidation_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvValidation;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtWhenField;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.TextBoxX txtWhenCondition;
        private DevComponents.DotNetBar.LabelX lblField;
        private FerretLib.WinForms.Controls.TagListControl tagListControl1;
        private DevComponents.DotNetBar.LabelX labelX6;
    }
}