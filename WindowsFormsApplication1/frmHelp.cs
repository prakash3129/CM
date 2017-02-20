﻿using System;

namespace GCC
{
    public partial class frmHelp : DevComponents.DotNetBar.Office2007Form
    {
        public frmHelp()
        {
            InitializeComponent();
        }

        private void frmHelp_Load(object sender, EventArgs e)
        {
            string sHelp = string.Empty;
            sHelp += "Process Screen:" + Environment.NewLine+Environment.NewLine;
            sHelp += "  Contact Update Screen : Ctrl + U" + Environment.NewLine;
            sHelp += "  Add New Company : Ctrl + N" + Environment.NewLine;
            sHelp += "  Processed List Screen : Ctrl + P" + Environment.NewLine;
            sHelp += "  Reload : F5" + Environment.NewLine;
            sHelp += "  Filter : Ctrl + Shift + L (Toggle)" + Environment.NewLine;
            sHelp += "  Clear : Ctrl + Shift + L (Toggle)" + Environment.NewLine;
            sHelp += "  Enter ID : Ctrl + I" + Environment.NewLine;
            sHelp += "  Open Selected Company : Enter" + Environment.NewLine+Environment.NewLine;

            sHelp += "Contact Update:" + Environment.NewLine + Environment.NewLine;
            sHelp += "  Navigate Tabs : Alt + F6" + Environment.NewLine;
            sHelp += "  Switch between Panels : F6" + Environment.NewLine;
            sHelp += "  Open Custom ComboList : F2" + Environment.NewLine;
            sHelp += "  Open Agent Notes : Ctrl + Shift + N (Toggle)" + Environment.NewLine;
            sHelp += "  Add New Contact : Alt + A" + Environment.NewLine;
            sHelp += "  Expand Grid : Ctrl + E (Toggle)" + Environment.NewLine;
            sHelp += "  Save : Ctrl + S" + Environment.NewLine;
            sHelp += "  Close : Ctrl + Q" + Environment.NewLine + Environment.NewLine;
            sHelp += "  Campaign Manager Chrome URL:" + Environment.NewLine;
            sHelp += "https://chrome.google.com/webstore/detail/campaign-manager/edennfbknmkoomkfinpciofgdcamangh";
            rtxtHelp.Text = sHelp;
            

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
