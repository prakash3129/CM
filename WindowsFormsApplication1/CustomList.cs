using System;
using System.Drawing;
using System.Windows.Forms;

namespace GCC
{
    class CustomList : TextBox
    {
        private Button btn;
        public CustomList(string sBtnText, int iSize, EventHandler e,string sFieldReference)
        {
            btn = new Button();
            btn.UseVisualStyleBackColor = true;
            btn.Anchor = (AnchorStyles.Right | AnchorStyles.Top);
            btn.Text = "...";
            btn.Tag = "Button"+sFieldReference;
            btn.Width = 25;
            btn.Click += new EventHandler(e);
            this.ReadOnly = true;
            this.BackColor = Color.White;
        }
        protected override void OnParentChanged(EventArgs e)
        {
            // Keeps label on the same parent as the text box
            base.OnParentChanged(e);
            btn.Parent = this.Parent;   // NOTE: no dispose necessary
        }

        private void moveLabel()
        {
            // Keep label right-aligned to the left of the text box
            btn.Location = new Point(this.Right + 10, this.Top);
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

        public string Description
        {
            get { return btn.Text; }
            set { btn.Text = value; }
        }

        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = btn.Font = value; }
        }

    }
}
