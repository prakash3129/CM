namespace AbtoSoftPhone
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
            this.btnHangup = new System.Windows.Forms.Button();
            this.btnDial = new System.Windows.Forms.Button();
            this.txtDial = new System.Windows.Forms.TextBox();
            this.txtSendNumber = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnHangup
            // 
            this.btnHangup.Location = new System.Drawing.Point(269, 39);
            this.btnHangup.Name = "btnHangup";
            this.btnHangup.Size = new System.Drawing.Size(75, 23);
            this.btnHangup.TabIndex = 5;
            this.btnHangup.Text = "Hang Up";
            this.btnHangup.UseVisualStyleBackColor = true;
            this.btnHangup.Click += new System.EventHandler(this.btnHangup_Click);
            // 
            // btnDial
            // 
            this.btnDial.Location = new System.Drawing.Point(269, 9);
            this.btnDial.Name = "btnDial";
            this.btnDial.Size = new System.Drawing.Size(75, 23);
            this.btnDial.TabIndex = 4;
            this.btnDial.Text = "Dial";
            this.btnDial.UseVisualStyleBackColor = true;
            this.btnDial.Click += new System.EventHandler(this.btnDial_Click);
            // 
            // txtDial
            // 
            this.txtDial.Location = new System.Drawing.Point(12, 12);
            this.txtDial.Name = "txtDial";
            this.txtDial.Size = new System.Drawing.Size(251, 20);
            this.txtDial.TabIndex = 3;
            this.txtDial.Text = "8447836191191";
            this.txtDial.TextChanged += new System.EventHandler(this.txtDial_TextChanged);
            // 
            // txtSendNumber
            // 
            this.txtSendNumber.Location = new System.Drawing.Point(12, 42);
            this.txtSendNumber.Name = "txtSendNumber";
            this.txtSendNumber.Size = new System.Drawing.Size(63, 20);
            this.txtSendNumber.TabIndex = 6;
            this.txtSendNumber.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 74);
            this.Controls.Add(this.txtSendNumber);
            this.Controls.Add(this.btnHangup);
            this.Controls.Add(this.btnDial);
            this.Controls.Add(this.txtDial);
            this.Name = "Form1";
            this.Text = "Softphone";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnHangup;
        private System.Windows.Forms.Button btnDial;
        private System.Windows.Forms.TextBox txtDial;
        private System.Windows.Forms.TextBox txtSendNumber;
    }
}

