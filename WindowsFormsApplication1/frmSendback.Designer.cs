namespace GCC
{
    partial class frmSendback
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
            this.CalViewSendBack = new DevComponents.DotNetBar.Schedule.CalendarView();
            this.barSendbackProperties = new DevComponents.DotNetBar.Bar();
            this.btnDayViewBack = new DevComponents.DotNetBar.ButtonItem();
            this.labelItem4 = new DevComponents.DotNetBar.LabelItem();
            this.labelBlue = new DevComponents.DotNetBar.LabelItem();
            this.checkBoxProcessed = new DevComponents.DotNetBar.CheckBoxItem();
            this.labelItem3 = new DevComponents.DotNetBar.LabelItem();
            this.labelYellow = new DevComponents.DotNetBar.LabelItem();
            this.checkBoxReProcessed = new DevComponents.DotNetBar.CheckBoxItem();
            this.labelItem2 = new DevComponents.DotNetBar.LabelItem();
            this.lblRed = new DevComponents.DotNetBar.LabelItem();
            this.checkBoxSendBack = new DevComponents.DotNetBar.CheckBoxItem();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.lblGreen = new DevComponents.DotNetBar.LabelItem();
            this.checkBoxOK = new DevComponents.DotNetBar.CheckBoxItem();
            this.backgroundLoader = new System.ComponentModel.BackgroundWorker();
            this.ProgressLoading = new DevComponents.DotNetBar.CircularProgressItem();
            ((System.ComponentModel.ISupportInitialize)(this.barSendbackProperties)).BeginInit();
            this.SuspendLayout();
            // 
            // CalViewSendBack
            // 
            this.CalViewSendBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.CalViewSendBack.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.CalViewSendBack.ContainerControlProcessDialogKey = true;
            this.CalViewSendBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CalViewSendBack.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.CalViewSendBack.Location = new System.Drawing.Point(0, 33);
            this.CalViewSendBack.MultiUserTabHeight = 19;
            this.CalViewSendBack.Name = "CalViewSendBack";
            this.CalViewSendBack.SelectedView = DevComponents.DotNetBar.Schedule.eCalendarView.Month;
            this.CalViewSendBack.Size = new System.Drawing.Size(840, 533);
            this.CalViewSendBack.TabIndex = 0;
            this.CalViewSendBack.Text = "calendarView1";
            this.CalViewSendBack.TimeIndicator.BorderColor = System.Drawing.Color.Empty;
            this.CalViewSendBack.TimeIndicator.Tag = null;
            this.CalViewSendBack.TimeSlotDuration = 30;
            this.CalViewSendBack.SelectedViewChanged += new System.EventHandler<DevComponents.DotNetBar.Schedule.SelectedViewEventArgs>(this.CalViewSendBack_SelectedViewChanged);
            this.CalViewSendBack.ItemDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CalViewSendBack_ItemDoubleClick);
            // 
            // barSendbackProperties
            // 
            this.barSendbackProperties.AccessibleDescription = "DotNetBar Bar (barSendbackProperties)";
            this.barSendbackProperties.AccessibleName = "DotNetBar Bar";
            this.barSendbackProperties.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.barSendbackProperties.AntiAlias = true;
            this.barSendbackProperties.Dock = System.Windows.Forms.DockStyle.Top;
            this.barSendbackProperties.DockedBorderStyle = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.barSendbackProperties.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.barSendbackProperties.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnDayViewBack,
            this.labelItem4,
            this.labelBlue,
            this.checkBoxProcessed,
            this.labelItem3,
            this.lblRed,
            this.checkBoxSendBack,
            this.labelItem2,
            this.labelYellow,
            this.checkBoxReProcessed,
            this.labelItem1,
            this.lblGreen,
            this.checkBoxOK,
            this.ProgressLoading});
            this.barSendbackProperties.Location = new System.Drawing.Point(0, 0);
            this.barSendbackProperties.MinimumSize = new System.Drawing.Size(0, 25);
            this.barSendbackProperties.Name = "barSendbackProperties";
            this.barSendbackProperties.Size = new System.Drawing.Size(840, 33);
            this.barSendbackProperties.Stretch = true;
            this.barSendbackProperties.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.barSendbackProperties.TabIndex = 1;
            this.barSendbackProperties.TabStop = false;
            // 
            // btnDayViewBack
            // 
            this.btnDayViewBack.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnDayViewBack.Image = global::GCC.Properties.Resources.back_icon;
            this.btnDayViewBack.Name = "btnDayViewBack";
            this.btnDayViewBack.Text = "Back";
            this.btnDayViewBack.Visible = false;
            this.btnDayViewBack.Click += new System.EventHandler(this.btnDayViewBack_Click);
            // 
            // labelItem4
            // 
            this.labelItem4.ForeColor = System.Drawing.Color.Transparent;
            this.labelItem4.Name = "labelItem4";
            this.labelItem4.Text = "TestSpace";
            // 
            // labelBlue
            // 
            this.labelBlue.Image = global::GCC.Properties.Resources.Blue;
            this.labelBlue.ImageTextSpacing = 0;
            this.labelBlue.Name = "labelBlue";
            // 
            // checkBoxProcessed
            // 
            this.checkBoxProcessed.Checked = true;
            this.checkBoxProcessed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxProcessed.Name = "checkBoxProcessed";
            this.checkBoxProcessed.Text = "Processed";
            this.checkBoxProcessed.Click += new System.EventHandler(this.checkBoxProcessed_Click);
            // 
            // labelItem3
            // 
            this.labelItem3.ForeColor = System.Drawing.Color.Transparent;
            this.labelItem3.Name = "labelItem3";
            this.labelItem3.Text = "TestSpace";
            // 
            // labelYellow
            // 
            this.labelYellow.Image = global::GCC.Properties.Resources.Yello;
            this.labelYellow.ImageTextSpacing = 0;
            this.labelYellow.Name = "labelYellow";
            // 
            // checkBoxReProcessed
            // 
            this.checkBoxReProcessed.Checked = true;
            this.checkBoxReProcessed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxReProcessed.Name = "checkBoxReProcessed";
            this.checkBoxReProcessed.Text = "Reprocessed";
            this.checkBoxReProcessed.Click += new System.EventHandler(this.checkBoxProcessed_Click);
            // 
            // labelItem2
            // 
            this.labelItem2.ForeColor = System.Drawing.Color.Transparent;
            this.labelItem2.Name = "labelItem2";
            this.labelItem2.Text = "TestSpace";
            // 
            // lblRed
            // 
            this.lblRed.Image = global::GCC.Properties.Resources.Red;
            this.lblRed.ImageTextSpacing = 0;
            this.lblRed.Name = "lblRed";
            // 
            // checkBoxSendBack
            // 
            this.checkBoxSendBack.Name = "checkBoxSendBack";
            this.checkBoxSendBack.Text = "Send Back";
            this.checkBoxSendBack.Click += new System.EventHandler(this.checkBoxProcessed_Click);
            // 
            // labelItem1
            // 
            this.labelItem1.ForeColor = System.Drawing.Color.Transparent;
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Text = "TestSpace";
            // 
            // lblGreen
            // 
            this.lblGreen.Image = global::GCC.Properties.Resources.Green;
            this.lblGreen.ImageTextSpacing = 0;
            this.lblGreen.Name = "lblGreen";
            // 
            // checkBoxOK
            // 
            this.checkBoxOK.Name = "checkBoxOK";
            this.checkBoxOK.Text = "OK";
            this.checkBoxOK.Click += new System.EventHandler(this.checkBoxProcessed_Click);
            // 
            // backgroundLoader
            // 
            this.backgroundLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundLoader_DoWork);
            this.backgroundLoader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundLoader_RunWorkerCompleted);
            // 
            // ProgressLoading
            // 
            this.ProgressLoading.AnimationSpeed = 20;
            this.ProgressLoading.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.ProgressLoading.Name = "ProgressLoading";
            this.ProgressLoading.ProgressBarType = DevComponents.DotNetBar.eCircularProgressType.Donut;
            // 
            // frmSendback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 566);
            this.Controls.Add(this.CalViewSendBack);
            this.Controls.Add(this.barSendbackProperties);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.Name = "frmSendback";
            this.Text = "Send Back";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSendback_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barSendbackProperties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Schedule.CalendarView CalViewSendBack;
        private DevComponents.DotNetBar.Bar barSendbackProperties;
        private DevComponents.DotNetBar.ButtonItem btnDayViewBack;
        private DevComponents.DotNetBar.LabelItem labelBlue;
        private DevComponents.DotNetBar.CheckBoxItem checkBoxProcessed;
        private DevComponents.DotNetBar.LabelItem labelYellow;
        private DevComponents.DotNetBar.CheckBoxItem checkBoxReProcessed;
        private DevComponents.DotNetBar.LabelItem lblRed;
        private DevComponents.DotNetBar.CheckBoxItem checkBoxSendBack;
        private DevComponents.DotNetBar.LabelItem lblGreen;
        private DevComponents.DotNetBar.CheckBoxItem checkBoxOK;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.LabelItem labelItem4;
        private DevComponents.DotNetBar.LabelItem labelItem3;
        private DevComponents.DotNetBar.LabelItem labelItem2;
        private System.ComponentModel.BackgroundWorker backgroundLoader;
        private DevComponents.DotNetBar.CircularProgressItem ProgressLoading;

    }
}