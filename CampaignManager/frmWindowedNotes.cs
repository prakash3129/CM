using System;
using System.Drawing;
using System.Windows.Forms;

namespace GCC
{
    public partial class frmWindowedNotes : DevComponents.DotNetBar.Office2007Form
    {
        public frmWindowedNotes()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window
        }
        private string _sNoteText;
        private int _iNoteTextSize;

        public string sNoteText
        {
            get { return _sNoteText; }
            set { _sNoteText = value; }
        }

        private Form _frmParantForm;

        public Form frmParantForm
        {
            get { return _frmParantForm; }
            set { _frmParantForm = value; }
        }

        public int iNoteTextSize
        {
            get { return _iNoteTextSize; }
            set { _iNoteTextSize = value; }
        }

        private void frmPanel_Load(object sender, EventArgs e)
        {
            
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ResizeRedraw = false;
            this.Location = new Point(0, 0);
            this.ShowInTaskbar = false;
            txtNotes.Rtf = sNoteText;
            txtNotes.SelectionFont = new Font("Tahoma", iNoteTextSize);
            txtNotes.Focus();
        }

        private void sliderFontSize_ValueChanged(object sender, EventArgs e)
        {
            //txtNotes.Font = new Font(txtNotes.Font.FontFamily, sliderFontSize.Value);
            //iNoteTextSize = sliderFontSize.Value;
            txtNotes.SelectAll();
            txtNotes.SelectionFont = new Font("Tahoma", sliderFontSize.Value);
        }

        private void frmPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            sNoteText = txtNotes.Rtf;
            FrmContactsUpdate.IsNoteFormOpened = false;
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            /////Key press listener for overall window/////

            //if (keyData == (Keys.Control | Keys.Shift | Keys.M))
            //{
                
            //    if (this.TopMost)
            //    {
            //        this.TopMost = false;
            //        this.WindowState = FormWindowState.Minimized;
            //        frmContactsUpdate parent = (frmContactsUpdate)this.Owner;
            //        parent.dgvContacts.Focus();
            //    }

            //}

            if (keyData == (Keys.Control | Keys.Shift | Keys.N))
            {
                btnWindowed.PerformClick();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void btnWindowed_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
