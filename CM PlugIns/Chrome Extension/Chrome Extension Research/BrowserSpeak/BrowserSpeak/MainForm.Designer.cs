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
            this.mButtonClearRequests = new System.Windows.Forms.Button();
            this.mLabelRequestsTitle = new System.Windows.Forms.Label();
            this.mTextBoxRequests = new System.Windows.Forms.TextBox();
            this.mToolTip = new System.Windows.Forms.ToolTip(this.components);
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
            // mButtonClearRequests
            // 
            this.mButtonClearRequests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonClearRequests.Location = new System.Drawing.Point(579, -1);
            this.mButtonClearRequests.Name = "mButtonClearRequests";
            this.mButtonClearRequests.Size = new System.Drawing.Size(43, 25);
            this.mButtonClearRequests.TabIndex = 2;
            this.mToolTip.SetToolTip(this.mButtonClearRequests, "Clear Requests");
            this.mButtonClearRequests.UseVisualStyleBackColor = true;
            this.mButtonClearRequests.Click += new System.EventHandler(this.OnButtonClearRequestsClick);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 315);
            this.Controls.Add(this.mSplitContainerMain);
            this.MinimumSize = new System.Drawing.Size(526, 349);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
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
        private System.Windows.Forms.Button mButtonClearRequests;
        private System.Windows.Forms.ToolTip mToolTip;
    }
}

