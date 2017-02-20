namespace GCC
{
    partial class JSTest
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
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.TxtName = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Phone = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.swtchBranch = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNoEmp = new DevComponents.Editors.IntegerInput();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbOfficeType = new System.Windows.Forms.ComboBox();
            this.txtJS = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtNoEmp)).BeginInit();
            this.SuspendLayout();
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(93, 82);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(210, 20);
            this.txtEmail.TabIndex = 2;
            this.txtEmail.Text = "prakash.cyberdyne@gmail.com";
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(93, 16);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(210, 20);
            this.TxtName.TabIndex = 0;
            this.TxtName.Text = "Prakash";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(93, 48);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(210, 20);
            this.txtPhone.TabIndex = 1;
            this.txtPhone.Text = "9790783282";
            this.txtPhone.Enter += new System.EventHandler(this.txtPhone_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Email";
            // 
            // Phone
            // 
            this.Phone.AutoSize = true;
            this.Phone.Location = new System.Drawing.Point(31, 55);
            this.Phone.Name = "Phone";
            this.Phone.Size = new System.Drawing.Size(38, 13);
            this.Phone.TabIndex = 9;
            this.Phone.Text = "Phone";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Name";
            // 
            // swtchBranch
            // 
            // 
            // 
            // 
            this.swtchBranch.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.swtchBranch.Location = new System.Drawing.Point(93, 155);
            this.swtchBranch.Name = "swtchBranch";
            this.swtchBranch.OffText = "No";
            this.swtchBranch.OnText = "Yes";
            this.swtchBranch.Size = new System.Drawing.Size(66, 22);
            this.swtchBranch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.swtchBranch.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Branch";
            // 
            // txtNoEmp
            // 
            // 
            // 
            // 
            this.txtNoEmp.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtNoEmp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtNoEmp.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtNoEmp.Location = new System.Drawing.Point(93, 198);
            this.txtNoEmp.Name = "txtNoEmp";
            this.txtNoEmp.ShowUpDown = true;
            this.txtNoEmp.Size = new System.Drawing.Size(80, 20);
            this.txtNoEmp.TabIndex = 7;
            this.txtNoEmp.Value = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 198);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "No. Emp";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(31, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Office Type";
            // 
            // cmbOfficeType
            // 
            this.cmbOfficeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOfficeType.FormattingEnabled = true;
            this.cmbOfficeType.Items.AddRange(new object[] {
            "RQ",
            "HQ",
            "OT"});
            this.cmbOfficeType.Location = new System.Drawing.Point(93, 117);
            this.cmbOfficeType.Name = "cmbOfficeType";
            this.cmbOfficeType.Size = new System.Drawing.Size(210, 21);
            this.cmbOfficeType.TabIndex = 3;
            // 
            // txtJS
            // 
            this.txtJS.Location = new System.Drawing.Point(12, 258);
            this.txtJS.Name = "txtJS";
            this.txtJS.Size = new System.Drawing.Size(284, 20);
            this.txtJS.TabIndex = 23;
            this.txtJS.Text = "http://172.27.137.182:81/afddata.pce?Data=Address&Task=FastFind&Fields=List&MaxQu" +
    "antity=10&Lookup=lister";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 242);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Query String";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // JSTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 470);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtJS);
            this.Controls.Add(this.swtchBranch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNoEmp);
            this.Controls.Add(this.cmbOfficeType);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Phone);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.TxtName);
            this.Controls.Add(this.txtEmail);
            this.Name = "JSTest";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtNoEmp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox TxtName;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Phone;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.Controls.SwitchButton swtchBranch;
        private System.Windows.Forms.Label label7;
        private DevComponents.Editors.IntegerInput txtNoEmp;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbOfficeType;
        private System.Windows.Forms.TextBox txtJS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer1;
    }
}