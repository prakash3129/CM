using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using Speak = Microsoft.CognitiveServices.SpeechRecognition;
using System.Configuration;
using System.Diagnostics;


namespace MicrosoftSpeech
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private const string IsolatedStorageSubscriptionKeyFileName = "Subscription.txt";
        private const string DefaultSubscriptionKeyPromptMessage = "Paste your subscription key here to start";
        private string subscriptionKey;
		private Speak.DataRecognitionClient dataClient;
        private Speak.MicrophoneRecognitionClient micClient;

        public bool IsMicrophoneClientShortPhrase { get; set; }        
        public bool IsMicrophoneClientDictation { get; set; }        
        public bool IsMicrophoneClientWithIntent { get; set; }        
        public bool IsDataClientShortPhrase { get; set; }        
        public bool IsDataClientWithIntent { get; set; }        
        public bool IsDataClientDictation { get; set; }
        private string LuisAppId
        {
            get { return ConfigurationManager.AppSettings["luisAppID"]; }
        }
        private string LuisSubscriptionID
        {
            get { return ConfigurationManager.AppSettings["luisSubscriptionID"]; }
        }
        private bool UseMicrophone
        {
            get
            {
                return this.IsMicrophoneClientWithIntent ||
                    this.IsMicrophoneClientDictation ||
                    this.IsMicrophoneClientShortPhrase;
            }
        }


        private void Initialize()
        {
            this.IsMicrophoneClientShortPhrase = true;
            this.IsMicrophoneClientWithIntent = false;
            this.IsMicrophoneClientDictation = false;
            this.IsDataClientShortPhrase = false;
            this.IsDataClientWithIntent = false;
            this.IsDataClientDictation = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbLanguage.SelectedIndex = 0;
        }

        private void btnSpeech_Click(object sender, EventArgs e)
        {
            btnSpeech.Enabled = false;            
            txtSpeechRaw.Text = ("\n--- Start speech recognition using microphone with short mode in "+ cmbLanguage.SelectedText.ToString() + " language ----\n\n");
            this.micClient = Speak.SpeechRecognitionServiceFactory.CreateMicrophoneClient(Speak.SpeechRecognitionMode.ShortPhrase, cmbLanguage.SelectedText.ToString(), "301ac603fc9c45f692e358fcb0158001");

            // Event handlers for speech recognition results
            this.micClient.OnMicrophoneStatus += this.OnMicrophoneStatus;
            this.micClient.OnPartialResponseReceived += this.OnPartialResponseReceivedHandler;
            //if (this.Mode == SpeechRecognitionMode.ShortPhrase)
            //{
                this.micClient.OnResponseReceived += this.OnMicShortPhraseResponseReceivedHandler;
            //}
            //else if (this.Mode == SpeechRecognitionMode.LongDictation)
            //{
            //    this.micClient.OnResponseReceived += this.OnMicDictationResponseReceivedHandler;
            //}

            this.micClient.OnConversationError += this.OnConversationErrorHandler;
            this.micClient.StartMicAndRecognition();
        }
        private void OnMicrophoneStatus(object sender, Speak.MicrophoneEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate () {
               // WriteLine("--- Microphone status change received by OnMicrophoneStatus() ---");
              //  WriteLine("********* Microphone status: {0} *********", e.Recording);
                if (e.Recording)
                {
                    WriteLine("Please start speaking.");
                }

                WriteLine();
            });

        }
        private void OnPartialResponseReceivedHandler(object sender, Speak.PartialSpeechResponseEventArgs e)
        {
            //this.WriteLine("--- Partial result received by OnPartialResponseReceivedHandler() ---");
            this.WriteLine("{0}", e.PartialResult);
            this.WriteLine();
        }

        private void OnMicShortPhraseResponseReceivedHandler(object sender, Speak.SpeechResponseEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate () {
                //this.WriteLine("--- OnMicShortPhraseResponseReceivedHandler ---");
                micClient.EndMicAndRecognition();
                btnSpeech.Enabled = true;
            });            
        }

        private void OnConversationErrorHandler(object sender, Speak.SpeechErrorEventArgs e)
        {
            
            this.BeginInvoke((MethodInvoker)delegate () {
                btnSpeech.Enabled = true;
            });

            this.WriteLine("--- Error received by OnConversationErrorHandler() ---");
            this.WriteLine("Error code: {0}", e.SpeechErrorCode.ToString());
            this.WriteLine("Error text: {0}", e.SpeechErrorText);
            this.WriteLine();
        }

        private void WriteLine(string format, params object[] args)
        {
            var formattedStr = string.Format(format, args);
            Trace.WriteLine(formattedStr);
            this.BeginInvoke((MethodInvoker)delegate () {
                txtSpeechRaw.AppendText(formattedStr + Environment.NewLine);
                if (formattedStr.Trim().Length > 0)
                    txtSpeech.Text = (formattedStr);

            });          
        }
        private void WriteLine()
        {
            this.WriteLine(string.Empty);
        }




    }
}
