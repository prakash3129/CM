using System;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    class EmailBox : DevComponents.DotNetBar.Controls.TextBoxX
    {
        private DevComponents.DotNetBar.ButtonX btn;
        DevComponents.DotNetBar.Controls.CircularProgress cp;
        string sLink;
        string sCondition;
        WebBrowser web;
        bool bCompleted = false;
        public EmailBox(string sBtnText, int iSize, string sCheckCondition,string sURL)
        {
            cp = new DevComponents.DotNetBar.Controls.CircularProgress();
            btn = new ButtonX();
            sLink = sURL;
            sCondition = sCheckCondition;
            //btn.UseVisualStyleBackColor = true;
            btn.Anchor = (AnchorStyles.Right | AnchorStyles.Top);
            btn.Text = sBtnText;
            btn.Width = iSize;
            btn.Click += new EventHandler(btn_Click);
            cp.Width = 15;
            cp.Height = 15;
            cp.BackColor = Color.Transparent;
            this.ForeColor = Color.Red;
            btn.Location = new Point(this.Left - 7, this.Top);
            btn.Visible = true;
            btn.Parent = this.Parent;
            cp.ProgressBarType = DevComponents.DotNetBar.eCircularProgressType.Dot;
        }
        private void btn_Click(object sender, EventArgs e)
        {
            if (this.BackColor != Color.FromArgb(0xE2, 0xE6, 0xE8))//Do not run if syntax is invalid
            {
                cp.IsRunning = true;// Start Loading circular
                web = new WebBrowser();
                web.Navigate(sLink + this.Text);
                web.ScriptErrorsSuppressed = true;

                while (web.Document == null || web.Document.Body == null || web.Document.Body.InnerText == null)
                {
                    Application.DoEvents();
                }

                //textBox2.Text = "1.Inner Text: " + web.Document.Body.InnerHtml;
                if (web.Document.Body.InnerHtml.Contains(sCondition))
                    this.ForeColor = Color.Green;
                else
                    this.ForeColor = Color.Red;
                //web.Dispose(); This triggers the External Browser I do not know why.. Got to figure it out..
                cp.IsRunning = false;// Stop Loading circular
            }
        }



        protected override void OnParentChanged(EventArgs e)
        {
            // Keeps label on the same parent as the text box
            base.OnParentChanged(e);
            btn.Parent = this.Parent;   // NOTE: no dispose necessary
            cp.Parent = this.Parent;
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.ForeColor = Color.Black;
        }

        private void moveLabel()
        {
            // Keep label right-aligned to the left of the text box
            btn.Location = new Point(this.Right, this.Top);
            cp.Location = new Point(btn.Right, btn.Top+3);
        }
        private void label_Resize(object sender, EventArgs e)
        {
            moveLabel();
        }
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            moveLabel();
        }

        //public string Description
        //{
        //    get { return btn.Text; }
        //    set { btn.Text = value; }
        //}

        //public override Font Font
        //{
        //    get { return base.Font; }
        //    set { base.Font = btn.Font = value; }
        //}

    }
}
