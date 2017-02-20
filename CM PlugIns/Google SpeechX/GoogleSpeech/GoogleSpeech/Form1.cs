using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Google.Apis.CloudSpeechAPI.v1beta1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.IO;
using NAudio.Wave;
using Microsoft.Speech.Recognition;
using DirectShowLib;
using IronPython.Hosting;


namespace GoogleSpeech
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
        }

        static public CloudSpeechAPIService CreateAuthorizedClient()
        {
            
                GoogleCredential credential = GoogleCredential.GetApplicationDefaultAsync().Result;
                // Inject the Cloud Storage scope if required.
                if (credential.IsCreateScopedRequired)
                {
                    credential = credential.CreateScoped(new[]
                    {
                    CloudSpeechAPIService.Scope.CloudPlatform
                });
                }
                return new CloudSpeechAPIService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "DotNet Google Cloud Platform Speech Sample",
                });
            
        }
        string sJsonPath = string.Empty;
        private void Form1_Load(object sender, EventArgs e)
        {
            cmbLanguage.SelectedIndex = 0;
            try
            {


                //var input = Console.ReadLine();


                var py = Python.CreateEngine();
                try
                {
                    py.Execute("print('From Python: input')");

                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                       "Oops! We couldn't print your message because of an exception: " + ex.Message);
                }



                //DsDevice[] audioRenderers;
                ////audioRenderers = DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice);
                //audioRenderers = DsDevice.GetDevicesOfCat(FilterCategory.AudioRendererCategory);
                //foreach (DsDevice device in audioRenderers)
                //{
                //    MessageBox.Show("Speaker :" + device.Name);
                //}


                //DsDevice[] audioRendererss;
                //audioRendererss = DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice);
                ////audioRenderers = DsDevice.GetDevicesOfCat(FilterCategory.AudioRendererCategory);
                //foreach (DsDevice device in audioRendererss)
                //{
                //    MessageBox.Show("Mic :" + device.Name);
                //}

                //return;


                sJsonPath = WriteFile("PrakashCM-42efb8111c24.json", Properties.Resources.PrakashCM_42efb8111c24);
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", sJsonPath, EnvironmentVariableTarget.Process);

               // Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"D:\GCredena\PrakashCM-42efb8111c24.json", EnvironmentVariableTarget.User);
            }
            catch(Exception ex)
            {
                txtInfo.Text = "Load Error  : " + ex.Message;
            }
            // Transcribe();
        }

        void Transcribe()
        {
            try
            {
                System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
                s.Start();

                var service = CreateAuthorizedClient();
                
                string audio_file_path = AppDomain.CurrentDomain.BaseDirectory + @"\SpeechAud.wav";
                var request = new Google.Apis.CloudSpeechAPI.v1beta1.Data.SyncRecognizeRequest()
                {
                    Config = new Google.Apis.CloudSpeechAPI.v1beta1.Data.RecognitionConfig()
                    {
                        Encoding = "LINEAR16",
                        SampleRate = 16000,
                        LanguageCode = cmbLanguage.SelectedText.ToString()
                    },
                    Audio = new Google.Apis.CloudSpeechAPI.v1beta1.Data.RecognitionAudio()
                    {
                        Content = Convert.ToBase64String(File.ReadAllBytes(audio_file_path))
                    }
                };
                txtSpeech.Text = "";
                var response = service.Speech.Syncrecognize(request).Execute();
                foreach (var result in response.Results)
                {
                    foreach (var alternative in result.Alternatives)
                        txtSpeech.AppendText(alternative.Transcript);
                }
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");


                //RecognizerInfo recognizerInfo = null;
                //SpeechRecognitionEngine sre = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
                //foreach (RecognizerInfo ri in SpeechRecognitionEngine.InstalledRecognizers())
                //{
                //    if ((ri.Culture.TwoLetterISOLanguageName.Equals("en")) && (recognizerInfo == null))
                //    {
                //        recognizerInfo = ri;
                //        break;
                //    }
                //}

                //sre.SetInputToWaveFile(@"C:\Users\thangaprakashm\Downloads\dotnet-docs-samples-master\dotnet-docs-samples-master\speech\api\resources\naudio.wav");

                //Choices colors = new Choices();
                //colors.Add(new string[] { "red", "green", "blue" });
                //GrammarBuilder gb = new GrammarBuilder();
                //gb.Culture = recognizerInfo.Culture;
                //gb.Append(colors);

                //// Create the Grammar instance.
                //Grammar g = new Grammar(gb);
                //sre.LoadGrammar(g);
                //sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
                //sre.Recognize();

                txtInfo.Text = "Time Taken : " + s.ElapsedMilliseconds;
            }
            catch(Exception ex)
            {
                txtInfo.Text = "Google Error : " + ex.Message;
            }
        }

        private string WriteFile(string sFileName, Byte[] ByteFile)
        {
            string sPath = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                if (!Directory.Exists(sPath))
                    Directory.CreateDirectory(sPath);
                if (!(File.Exists(sPath + "\\" + sFileName)))
                    File.WriteAllBytes(sPath + "\\" + sFileName, ByteFile);
            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message, "GSpeech", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return sPath  + sFileName;
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            MessageBox.Show("Speech recognized: " + e.Result.Text);
        }

        public WaveIn waveSource;
        public WaveFileWriter waveFile = null;
        private void btnRecord_Click(object sender, EventArgs e)
        {
            try
            {
                txtInfo.Text = "";
                txtSpeech.Text = "";

                waveSource = new WaveIn();
                waveSource.WaveFormat = new WaveFormat(16000, 1);
                waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);
                waveFile = new WaveFileWriter(AppDomain.CurrentDomain.BaseDirectory + @"\SpeechAud.wav", waveSource.WaveFormat);
                waveSource.StartRecording();
            }
            catch(Exception ex)
            {
                txtInfo.Text = "Recording Error : " + ex.Message;
            }
        }

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }

        void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }

           // System.Threading.Thread.Sleep(3000);
            Transcribe();
            //StartBtn.Enabled = true;
        }

        private void Stop_Click(object sender, EventArgs e)
        {


            waveSource.StopRecording();
            
            //mp3SoundCapture.Stop();


        }
    }
}

