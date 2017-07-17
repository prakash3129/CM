

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Speech.Synthesis;


using GCC;

namespace GCC
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            

            mShowHttpRequestDelegate = ShowHttpRequest;
            mUpdateTextBoxDelegate = UpdateTextBoxContents;
            mCmdDispatcher.MessageRecived += new ChromeConnect.MessageRecivedHandler(OnTextBufferChanged);
            mCmdDispatcher.ExceptionRaised += new ChromeConnect.ExceptionRaisedHandler(OnTextBufferChanged);
            mCmdDispatcher.Start("http://localhost:60024/");
            StartHttpCommandDispatcher();
        }

        private void StartHttpCommandDispatcher()
        {
            //mCmdDispatcher.AddResourceLocator(new ImageLocator(Properties.Resources.ResourceManager));
            
        }
        
        private void OnButtonClearRequestsClick(object sender, EventArgs e)
        {
            mTextBoxRequests.Text = string.Empty;
        }

        private void OnTextBufferChanged(object sender, ExceptionRaisedEventArgs e)
        {
            MessageBox.Show(e.Ex.Message);
        }
        
        private void OnTextBufferChanged(object sender, MessageRecivedEventArgs e)
        {
            // The TextBuffer is modified by a thread from the thread pool,
            // not the GUI thread. Therefore we need to switch back to the
            // GUI thread before modifying any GUI controls.

            
            BeginInvoke(mUpdateTextBoxDelegate, e.Text);
        }

        private void UpdateTextBoxContents(string newContents)
        {
            mTextBoxSpeak.Text = newContents;
            ScrollTextBoxToEnd(mTextBoxSpeak);
        }

        private void ScrollTextBoxToEnd(TextBox textBox)
        {
            textBox.Select(textBox.Text.Length, 0);
            textBox.ScrollToCaret();
        }

        private void ShowHttpRequest(string request)
        {
            mTextBoxRequests.Text += request + "\r\n";
            ScrollTextBoxToEnd(mTextBoxRequests);
        }

        private delegate void ShowHttpRequestDelgate(string request);
        ShowHttpRequestDelgate mShowHttpRequestDelegate;

        private delegate void UpdateTextBoxDelgate(string newContents);
        UpdateTextBoxDelgate mUpdateTextBoxDelegate;

        private ChromeConnect mCmdDispatcher = new ChromeConnect();        
    }
}
