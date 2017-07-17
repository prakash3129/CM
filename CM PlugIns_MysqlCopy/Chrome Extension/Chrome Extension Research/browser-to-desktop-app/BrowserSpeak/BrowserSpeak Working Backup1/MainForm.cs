

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
            TextBuffer.Instance.TextBufferChanged += new EventHandler(OnTextBufferChanged);
            StartHttpCommandDispatcher();
        }

        private void StartHttpCommandDispatcher()
        {
            //mCmdDispatcher.AddResourceLocator(new ImageLocator(Properties.Resources.ResourceManager));            
            mCmdDispatcher.AddCommand(new BufferTextCommand());            
            mCmdDispatcher.RequestReceived += new HttpCommandDispatcher.RequestReceivedHandler(OnHttpRequestReceived);
            mCmdDispatcher.Start("http://localhost:60024/");
        }
        
        private void OnButtonClearRequestsClick(object sender, EventArgs e)
        {
            mTextBoxRequests.Text = string.Empty;
        }

        private void OnHttpRequestReceived(object source, RequestEventArgs e)
        {
            // A HttpRequest is processed by a thread from the thread pool,
            // not the GUI thread. Therefore we need to switch back to the
            // GUI thread before modifying any GUI controls.
            BeginInvoke(mShowHttpRequestDelegate, e.Request);
        }

        private void OnTextBufferChanged(object sender, EventArgs e)
        {
            // The TextBuffer is modified by a thread from the thread pool,
            // not the GUI thread. Therefore we need to switch back to the
            // GUI thread before modifying any GUI controls.
            BeginInvoke(mUpdateTextBoxDelegate, TextBuffer.Instance.Text);
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

        private HttpCommandDispatcher mCmdDispatcher = new HttpCommandDispatcher();        
    }
}
