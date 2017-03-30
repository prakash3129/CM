namespace BrowserSpeak
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.mSplitContainerMain = new System.Windows.Forms.SplitContainer();
            this.mTextBoxSpeak = new System.Windows.Forms.TextBox();
            this.mLabelRequestsTitle = new System.Windows.Forms.Label();
            this.mTextBoxRequests = new System.Windows.Forms.TextBox();
            this.mToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mButtonAbout = new System.Windows.Forms.Button();
            this.mButtonPauseResume = new System.Windows.Forms.Button();
            this.mButtonStop = new System.Windows.Forms.Button();
            this.mButtonSpeak = new System.Windows.Forms.Button();
            this.mButtonClearRequests = new System.Windows.Forms.Button();
            this.mSplitContainerMain.Panel1.SuspendLayout();
            this.mSplitContainerMain.Panel2.SuspendLayout();
            this.mSplitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // mSplitContainerMain
            // 
            this.mSplitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mSplitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.mSplitContainerMain.Name = "mSplitContainerMain";
            this.mSplitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mSplitContainerMain.Panel1
            // 
            this.mSplitContainerMain.Panel1.Controls.Add(this.mButtonAbout);
            this.mSplitContainerMain.Panel1.Controls.Add(this.mButtonPauseResume);
            this.mSplitContainerMain.Panel1.Controls.Add(this.mButtonStop);
            this.mSplitContainerMain.Panel1.Controls.Add(this.mButtonSpeak);
            this.mSplitContainerMain.Panel1.Controls.Add(this.mTextBoxSpeak);
            // 
            // mSplitContainerMain.Panel2
            // 
            this.mSplitContainerMain.Panel2.Controls.Add(this.mButtonClearRequests);
            this.mSplitContainerMain.Panel2.Controls.Add(this.mLabelRequestsTitle);
            this.mSplitContainerMain.Panel2.Controls.Add(this.mTextBoxRequests);
            this.mSplitContainerMain.Size = new System.Drawing.Size(627, 315);
            this.mSplitContainerMain.SplitterDistance = 189;
            this.mSplitContainerMain.TabIndex = 0;
            // 
            // mTextBoxSpeak
            // 
            this.mTextBoxSpeak.AcceptsReturn = true;
            this.mTextBoxSpeak.AcceptsTab = true;
            this.mTextBoxSpeak.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mTextBoxSpeak.Location = new System.Drawing.Point(0, 69);
            this.mTextBoxSpeak.Multiline = true;
            this.mTextBoxSpeak.Name = "mTextBoxSpeak";
            this.mTextBoxSpeak.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.mTextBoxSpeak.Size = new System.Drawing.Size(627, 117);
            this.mTextBoxSpeak.TabIndex = 0;
            // 
            // mLabelRequestsTitle
            // 
            this.mLabelRequestsTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mLabelRequestsTitle.Location = new System.Drawing.Point(-3, 6);
            this.mLabelRequestsTitle.Name = "mLabelRequestsTitle";
            this.mLabelRequestsTitle.Size = new System.Drawing.Size(630, 13);
            this.mLabelRequestsTitle.TabIndex = 1;
            this.mLabelRequestsTitle.Text = "Requests";
            this.mLabelRequestsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mTextBoxRequests
            // 
            this.mTextBoxRequests.AcceptsReturn = true;
            this.mTextBoxRequests.AcceptsTab = true;
            this.mTextBoxRequests.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mTextBoxRequests.Location = new System.Drawing.Point(0, 28);
            this.mTextBoxRequests.Multiline = true;
            this.mTextBoxRequests.Name = "mTextBoxRequests";
            this.mTextBoxRequests.ReadOnly = true;
            this.mTextBoxRequests.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.mTextBoxRequests.Size = new System.Drawing.Size(627, 94);
            this.mTextBoxRequests.TabIndex = 0;
            // 
            // mButtonAbout
            // 
            this.mButtonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonAbout.Image = global::BrowserSpeak.Properties.Resources.house;
            this.mButtonAbout.Location = new System.Drawing.Point(511, 23);
            this.mButtonAbout.Name = "mButtonAbout";
            this.mButtonAbout.Size = new System.Drawing.Size(111, 34);
            this.mButtonAbout.TabIndex = 4;
            this.mButtonAbout.Text = "About...";
            this.mButtonAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.mButtonAbout.UseVisualStyleBackColor = true;
            this.mButtonAbout.Click += new System.EventHandler(this.OnButtonAboutClick);
            // 
            // mButtonPauseResume
            // 
            this.mButtonPauseResume.Image = global::BrowserSpeak.Properties.Resources.pause_icon;
            this.mButtonPauseResume.Location = new System.Drawing.Point(144, 23);
            this.mButtonPauseResume.Name = "mButtonPauseResume";
            this.mButtonPauseResume.Size = new System.Drawing.Size(111, 34);
            this.mButtonPauseResume.TabIndex = 3;
            this.mButtonPauseResume.Text = "Pause";
            this.mButtonPauseResume.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.mButtonPauseResume.UseVisualStyleBackColor = true;
            this.mButtonPauseResume.Click += new System.EventHandler(this.OnButtonPauseResumeClick);
            // 
            // mButtonStop
            // 
            this.mButtonStop.Image = global::BrowserSpeak.Properties.Resources.cross;
            this.mButtonStop.Location = new System.Drawing.Point(272, 23);
            this.mButtonStop.Name = "mButtonStop";
            this.mButtonStop.Size = new System.Drawing.Size(111, 34);
            this.mButtonStop.TabIndex = 2;
            this.mButtonStop.Text = "Stop";
            this.mButtonStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.mButtonStop.UseVisualStyleBackColor = true;
            this.mButtonStop.Click += new System.EventHandler(this.OnButtonStopClick);
            // 
            // mButtonSpeak
            // 
            this.mButtonSpeak.Image = global::BrowserSpeak.Properties.Resources.resultset_next;
            this.mButtonSpeak.Location = new System.Drawing.Point(12, 23);
            this.mButtonSpeak.Name = "mButtonSpeak";
            this.mButtonSpeak.Size = new System.Drawing.Size(111, 34);
            this.mButtonSpeak.TabIndex = 1;
            this.mButtonSpeak.Text = "Speak";
            this.mButtonSpeak.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.mButtonSpeak.UseVisualStyleBackColor = true;
            this.mButtonSpeak.Click += new System.EventHandler(this.OnButtonSpeakClick);
            // 
            // mButtonClearRequests
            // 
            this.mButtonClearRequests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonClearRequests.Image = global::BrowserSpeak.Properties.Resources.bomb;
            this.mButtonClearRequests.Location = new System.Drawing.Point(579, -1);
            this.mButtonClearRequests.Name = "mButtonClearRequests";
            this.mButtonClearRequests.Size = new System.Drawing.Size(43, 25);
            this.mButtonClearRequests.TabIndex = 2;
            this.mToolTip.SetToolTip(this.mButtonClearRequests, "Clear Requests");
            this.mButtonClearRequests.UseVisualStyleBackColor = true;
            this.mButtonClearRequests.Click += new System.EventHandler(this.OnButtonClearRequestsClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 315);
            this.Controls.Add(this.mSplitContainerMain);
            this.MinimumSize = new System.Drawing.Size(526, 349);
            this.Name = "MainForm";
            this.Text = "Browser Speak";
            this.mSplitContainerMain.Panel1.ResumeLayout(false);
            this.mSplitContainerMain.Panel1.PerformLayout();
            this.mSplitContainerMain.Panel2.ResumeLayout(false);
            this.mSplitContainerMain.Panel2.PerformLayout();
            this.mSplitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mSplitContainerMain;
        private System.Windows.Forms.TextBox mTextBoxRequests;
        private System.Windows.Forms.Label mLabelRequestsTitle;
        private System.Windows.Forms.TextBox mTextBoxSpeak;
        private System.Windows.Forms.Button mButtonSpeak;
        private System.Windows.Forms.Button mButtonPauseResume;
        private System.Windows.Forms.Button mButtonStop;
        private System.Windows.Forms.Button mButtonClearRequests;
        private System.Windows.Forms.ToolTip mToolTip;
        private System.Windows.Forms.Button mButtonAbout;
    }
}

