namespace GCC
{
    partial class frmCallLog
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
            DevComponents.Instrumentation.KnobColorTable knobColorTable1 = new DevComponents.Instrumentation.KnobColorTable();
            DevComponents.Instrumentation.Primitives.LinearGradientColorTable linearGradientColorTable1 = new DevComponents.Instrumentation.Primitives.LinearGradientColorTable();
            DevComponents.Instrumentation.KnobColorTable knobColorTable2 = new DevComponents.Instrumentation.KnobColorTable();
            DevComponents.Instrumentation.Primitives.LinearGradientColorTable linearGradientColorTable2 = new DevComponents.Instrumentation.Primitives.LinearGradientColorTable();
            this.lblInformation = new DevComponents.DotNetBar.LabelX();
            this.sdgvCallLog = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.btnClose = new System.Windows.Forms.Button();
            this.splitContainerAudioCallLog = new System.Windows.Forms.SplitContainer();
            this.btnSaveConfiguration = new DevComponents.DotNetBar.ButtonX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.knobPhoneVolume = new DevComponents.Instrumentation.KnobControl();
            this.knobMicVolume = new DevComponents.Instrumentation.KnobControl();
            this.chkAutoGainControl = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkEchoCancelation = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkNoiseCancellation = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAudioCallLog)).BeginInit();
            this.splitContainerAudioCallLog.Panel1.SuspendLayout();
            this.splitContainerAudioCallLog.Panel2.SuspendLayout();
            this.splitContainerAudioCallLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInformation
            // 
            // 
            // 
            // 
            this.lblInformation.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblInformation.BackgroundStyle.BorderBottomWidth = 1;
            this.lblInformation.BackgroundStyle.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.lblInformation.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblInformation.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblInformation.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lblInformation.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInformation.Location = new System.Drawing.Point(0, 0);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(501, 23);
            this.lblInformation.TabIndex = 0;
            this.lblInformation.Text = "labelX1";
            // 
            // sdgvCallLog
            // 
            this.sdgvCallLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdgvCallLog.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sdgvCallLog.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sdgvCallLog.Location = new System.Drawing.Point(0, 0);
            this.sdgvCallLog.Name = "sdgvCallLog";
            this.sdgvCallLog.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sdgvCallLog.PrimaryGrid.ReadOnly = true;
            this.sdgvCallLog.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.sdgvCallLog.PrimaryGrid.ShowRowHeaders = false;
            this.sdgvCallLog.Size = new System.Drawing.Size(501, 349);
            this.sdgvCallLog.TabIndex = 1;
            this.sdgvCallLog.DataBindingComplete += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs>(this.sdgvCallLog_DataBindingComplete);
            this.sdgvCallLog.RowDoubleClick += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowDoubleClickEventArgs>(this.sdgvCallLog_RowDoubleClick);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(3, 26);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // splitContainerAudioCallLog
            // 
            this.splitContainerAudioCallLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerAudioCallLog.IsSplitterFixed = true;
            this.splitContainerAudioCallLog.Location = new System.Drawing.Point(0, 0);
            this.splitContainerAudioCallLog.Name = "splitContainerAudioCallLog";
            // 
            // splitContainerAudioCallLog.Panel1
            // 
            this.splitContainerAudioCallLog.Panel1.Controls.Add(this.btnSaveConfiguration);
            this.splitContainerAudioCallLog.Panel1.Controls.Add(this.labelX3);
            this.splitContainerAudioCallLog.Panel1.Controls.Add(this.labelX2);
            this.splitContainerAudioCallLog.Panel1.Controls.Add(this.knobPhoneVolume);
            this.splitContainerAudioCallLog.Panel1.Controls.Add(this.knobMicVolume);
            this.splitContainerAudioCallLog.Panel1.Controls.Add(this.chkAutoGainControl);
            this.splitContainerAudioCallLog.Panel1.Controls.Add(this.chkEchoCancelation);
            this.splitContainerAudioCallLog.Panel1.Controls.Add(this.chkNoiseCancellation);
            this.splitContainerAudioCallLog.Panel1.Controls.Add(this.labelX1);
            // 
            // splitContainerAudioCallLog.Panel2
            // 
            this.splitContainerAudioCallLog.Panel2.Controls.Add(this.lblInformation);
            this.splitContainerAudioCallLog.Panel2.Controls.Add(this.sdgvCallLog);
            this.splitContainerAudioCallLog.Size = new System.Drawing.Size(634, 349);
            this.splitContainerAudioCallLog.SplitterDistance = 126;
            this.splitContainerAudioCallLog.SplitterWidth = 7;
            this.splitContainerAudioCallLog.TabIndex = 3;
            // 
            // btnSaveConfiguration
            // 
            this.btnSaveConfiguration.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveConfiguration.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveConfiguration.Location = new System.Drawing.Point(8, 316);
            this.btnSaveConfiguration.Name = "btnSaveConfiguration";
            this.btnSaveConfiguration.Size = new System.Drawing.Size(111, 23);
            this.btnSaveConfiguration.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSaveConfiguration.TabIndex = 9;
            this.btnSaveConfiguration.Text = "Save Configuration";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(27, 274);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 15);
            this.labelX3.TabIndex = 8;
            this.labelX3.Text = "Phone Volume";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(34, 170);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 15);
            this.labelX2.TabIndex = 7;
            this.labelX2.Text = "Mic Volume";
            // 
            // knobPhoneVolume
            // 
            this.knobPhoneVolume.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.knobPhoneVolume.FocusCuesEnabled = false;
            linearGradientColorTable1.End = System.Drawing.SystemColors.ActiveCaption;
            linearGradientColorTable1.Start = System.Drawing.SystemColors.ActiveCaption;
            knobColorTable1.KnobFaceColor = linearGradientColorTable1;
            knobColorTable1.KnobIndicatorPointerBorderWidth = 1;
            knobColorTable1.MajorTickColor = System.Drawing.Color.Black;
            knobColorTable1.MinorTickColor = System.Drawing.Color.Transparent;
            knobColorTable1.ZoneIndicatorColor = System.Drawing.Color.White;
            this.knobPhoneVolume.KnobColor = knobColorTable1;
            this.knobPhoneVolume.Location = new System.Drawing.Point(22, 193);
            this.knobPhoneVolume.MajorTickAmount = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.knobPhoneVolume.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.knobPhoneVolume.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.knobPhoneVolume.Name = "knobPhoneVolume";
            this.knobPhoneVolume.ShowTickLabels = false;
            this.knobPhoneVolume.Size = new System.Drawing.Size(80, 80);
            this.knobPhoneVolume.TabIndex = 6;
            this.knobPhoneVolume.TabStop = false;
            this.knobPhoneVolume.Text = "knobControl2";
            this.knobPhoneVolume.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.knobPhoneVolume.ValueChanged += new System.EventHandler<DevComponents.Instrumentation.ValueChangedEventArgs>(this.knobPhoneVolume_ValueChanged);
            // 
            // knobMicVolume
            // 
            this.knobMicVolume.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.knobMicVolume.FocusCuesEnabled = false;
            linearGradientColorTable2.End = System.Drawing.SystemColors.ActiveCaption;
            linearGradientColorTable2.Start = System.Drawing.SystemColors.ActiveCaption;
            knobColorTable2.KnobFaceColor = linearGradientColorTable2;
            knobColorTable2.KnobIndicatorPointerBorderWidth = 1;
            knobColorTable2.MajorTickColor = System.Drawing.Color.Black;
            knobColorTable2.MinorTickColor = System.Drawing.Color.Transparent;
            knobColorTable2.ZoneIndicatorColor = System.Drawing.Color.White;
            this.knobMicVolume.KnobColor = knobColorTable2;
            this.knobMicVolume.Location = new System.Drawing.Point(24, 88);
            this.knobMicVolume.MajorTickAmount = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.knobMicVolume.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.knobMicVolume.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.knobMicVolume.Name = "knobMicVolume";
            this.knobMicVolume.ShowTickLabels = false;
            this.knobMicVolume.Size = new System.Drawing.Size(80, 80);
            this.knobMicVolume.TabIndex = 5;
            this.knobMicVolume.TabStop = false;
            this.knobMicVolume.Text = "knobControl1";
            this.knobMicVolume.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.knobMicVolume.ValueChanged += new System.EventHandler<DevComponents.Instrumentation.ValueChangedEventArgs>(this.knobMicVolume_ValueChanged);
            // 
            // chkAutoGainControl
            // 
            // 
            // 
            // 
            this.chkAutoGainControl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkAutoGainControl.FocusCuesEnabled = false;
            this.chkAutoGainControl.Location = new System.Drawing.Point(10, 58);
            this.chkAutoGainControl.Name = "chkAutoGainControl";
            this.chkAutoGainControl.Size = new System.Drawing.Size(111, 23);
            this.chkAutoGainControl.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkAutoGainControl.TabIndex = 4;
            this.chkAutoGainControl.Text = "Auto Gain Control";
            // 
            // chkEchoCancelation
            // 
            // 
            // 
            // 
            this.chkEchoCancelation.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEchoCancelation.FocusCuesEnabled = false;
            this.chkEchoCancelation.Location = new System.Drawing.Point(10, 40);
            this.chkEchoCancelation.Name = "chkEchoCancelation";
            this.chkEchoCancelation.Size = new System.Drawing.Size(100, 23);
            this.chkEchoCancelation.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkEchoCancelation.TabIndex = 3;
            this.chkEchoCancelation.Text = "Echo Cancellation";
            // 
            // chkNoiseCancellation
            // 
            // 
            // 
            // 
            this.chkNoiseCancellation.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkNoiseCancellation.FocusCuesEnabled = false;
            this.chkNoiseCancellation.Location = new System.Drawing.Point(10, 21);
            this.chkNoiseCancellation.Name = "chkNoiseCancellation";
            this.chkNoiseCancellation.Size = new System.Drawing.Size(100, 23);
            this.chkNoiseCancellation.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkNoiseCancellation.TabIndex = 2;
            this.chkNoiseCancellation.Text = "Noise Reduction";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelX1.BackgroundStyle.BorderBottomWidth = 1;
            this.labelX1.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelX1.BackgroundStyle.BorderLeftWidth = 1;
            this.labelX1.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelX1.BackgroundStyle.BorderRightWidth = 1;
            this.labelX1.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelX1.BackgroundStyle.BorderTopWidth = 1;
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelX1.Location = new System.Drawing.Point(0, 0);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(126, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "  Audio Configuration";
            // 
            // frmCallLog
            // 
            this.AcceptButton = this.btnSaveConfiguration;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(634, 349);
            this.Controls.Add(this.splitContainerAudioCallLog);
            this.Controls.Add(this.btnClose);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmCallLog";
            this.Text = "Audio Configuration | Call Log";
            this.Load += new System.EventHandler(this.frmCallLog_Load);
            this.splitContainerAudioCallLog.Panel1.ResumeLayout(false);
            this.splitContainerAudioCallLog.Panel1.PerformLayout();
            this.splitContainerAudioCallLog.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAudioCallLog)).EndInit();
            this.splitContainerAudioCallLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX lblInformation;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sdgvCallLog;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SplitContainer splitContainerAudioCallLog;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkAutoGainControl;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEchoCancelation;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkNoiseCancellation;
        private DevComponents.Instrumentation.KnobControl knobMicVolume;
        private DevComponents.Instrumentation.KnobControl knobPhoneVolume;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX btnSaveConfiguration;
    }
}