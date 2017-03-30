// BrowserSpeak
//
// by Mark Gladding
// Copyright 2009 Tumbywood Software
// http://www.text2go.com
//
// You are free to reuse this code in any commercial or non-commercial work.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using HttpServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrowserSpeak
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

        private delegate void ShowHttpRequestDelgate(string request);
        ShowHttpRequestDelgate mShowHttpRequestDelegate;

        private delegate void UpdateTextBoxDelgate(string newContents);
        UpdateTextBoxDelgate mUpdateTextBoxDelegate;

        private HttpCommandDispatcher mCmdDispatcher = new HttpCommandDispatcher(Properties.Resources.dummy);

        private void StartHttpCommandDispatcher()
        {
            mCmdDispatcher.AddResourceLocator(new ImageLocator(Properties.Resources.ResourceManager));            
            mCmdDispatcher.AddCommand(new BufferTextCommand());            
            mCmdDispatcher.RequestReceived += new HttpCommandDispatcher.RequestReceivedHandler(OnHttpRequestReceived);
            mCmdDispatcher.Start("http://localhost:60024/");
        }        
              
        private void OnButtonClearRequestsClick(object sender, EventArgs e)
        {
            mTextBoxRequests.Text = string.Empty;



            string stringData = "Prakash";
            string message = "{\"text\": \"" + stringData + "\"}";
            OpenStandardStreamOut(message);

            //while (OpenStandardStreamIn() != null || OpenStandardStreamIn() != "")
            //{
            //    OpenStandardStreamOut("Received to Native App: " + OpenStandardStreamIn());

            //    OpenStandardStreamOut("Recieved: " + OpenStandardStreamIn());

            //}
        }

        private void OnHttpRequestReceived(object source, RequestEventArgs e)
        {            
            BeginInvoke(mShowHttpRequestDelegate, e.Request);
        }

        private void OnTextBufferChanged(object sender, EventArgs e)
        {            
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


        










        private void MainForm_Load(object sender, EventArgs e)
        {
            

            //while (OpenStandardStreamIn() != null || OpenStandardStreamIn() != "")
            //{
            //    OpenStandardStreamOut("Received to Native App: " + OpenStandardStreamIn());

            //    OpenStandardStreamOut("Recieved: " + OpenStandardStreamIn());
            //}

        }

        private static string OpenStandardStreamIn()
        {
            //// We need to read first 4 bytes for length information
            Stream stdin = Console.OpenStandardInput();
            int length = 0;
            byte[] bytes = new byte[4];
            stdin.Read(bytes, 0, 4);
            length = System.BitConverter.ToInt32(bytes, 0);

            string input = "";
            for (int i = 0; i < length;i++ )
            {
            input += (char)stdin.ReadByte();
            }

            return input;
        }

        private static void OpenStandardStreamOut(string stringData)
        {
            //// We need to send the 4 btyes of length information
            int DataLength = stringData.Length;
            Stream stdout = Console.OpenStandardOutput();
            stdout.WriteByte((byte)((DataLength >> 0) & 0xFF));
            stdout.WriteByte((byte)((DataLength >> 8) & 0xFF));
            stdout.WriteByte((byte)((DataLength >> 16) & 0xFF));
            stdout.WriteByte((byte)((DataLength >> 24) & 0xFF));
            //Available total length : 4,294,967,295 ( FF FF FF FF )

            Console.Write(stringData);

           // MessageBox.Show("Sent");
        }
}
    
}
