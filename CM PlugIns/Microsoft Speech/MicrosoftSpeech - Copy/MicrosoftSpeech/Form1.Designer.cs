namespace MicrosoftSpeech
{
    partial class Form1
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
            this.txtSpeechRaw = new System.Windows.Forms.TextBox();
            this.txtSpeech = new System.Windows.Forms.TextBox();
            this.btnSpeech = new System.Windows.Forms.Button();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtSpeechRaw
            // 
            this.txtSpeechRaw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpeechRaw.Location = new System.Drawing.Point(12, 12);
            this.txtSpeechRaw.Multiline = true;
            this.txtSpeechRaw.Name = "txtSpeechRaw";
            this.txtSpeechRaw.Size = new System.Drawing.Size(560, 540);
            this.txtSpeechRaw.TabIndex = 0;
            // 
            // txtSpeech
            // 
            this.txtSpeech.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSpeech.Location = new System.Drawing.Point(13, 575);
            this.txtSpeech.Multiline = true;
            this.txtSpeech.Name = "txtSpeech";
            this.txtSpeech.Size = new System.Drawing.Size(481, 110);
            this.txtSpeech.TabIndex = 1;
            // 
            // btnSpeech
            // 
            this.btnSpeech.Location = new System.Drawing.Point(500, 602);
            this.btnSpeech.Name = "btnSpeech";
            this.btnSpeech.Size = new System.Drawing.Size(72, 83);
            this.btnSpeech.TabIndex = 2;
            this.btnSpeech.Text = "Speak";
            this.btnSpeech.UseVisualStyleBackColor = true;
            this.btnSpeech.Click += new System.EventHandler(this.btnSpeech_Click);
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Items.AddRange(new object[] {
            "en-IN",
            "en-US",
            "en-UK"});
            this.cmbLanguage.Location = new System.Drawing.Point(500, 575);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(72, 21);
            this.cmbLanguage.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 697);
            this.Controls.Add(this.cmbLanguage);
            this.Controls.Add(this.btnSpeech);
            this.Controls.Add(this.txtSpeech);
            this.Controls.Add(this.txtSpeechRaw);
            this.Name = "Form1";
            this.Text = "Microsoft Speech";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSpeechRaw;
        private System.Windows.Forms.TextBox txtSpeech;
        private System.Windows.Forms.Button btnSpeech;
        private System.Windows.Forms.ComboBox cmbLanguage;
    }
}

