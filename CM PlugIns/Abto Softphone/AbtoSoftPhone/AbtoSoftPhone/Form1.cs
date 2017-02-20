using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using SIPVoipSDK;


namespace AbtoSoftPhone
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string ss = Environment.Is64BitOperatingSystem.ToString();

        


        CAbtoPhoneClass AbtoPhone = new CAbtoPhoneClass();
        private void Form1_Load(object sender, EventArgs e)
        {
            
            try
            {
                ConfigurePhone();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        protected bool ConfigurePhone()
        {
            
            try
            {
                //Assign event handlers


                //this.AbtoPhone.OnInitialized += new _IAbtoPhoneEvents_OnInitializedEventHandler(this.AbtoPhone_OnInitialized);
                //this.AbtoPhone.OnLineSwiched += new _IAbtoPhoneEvents_OnLineSwichedEventHandler(this.AbtoPhone_OnLineSwiched);
                //this.AbtoPhone.OnEstablishedCall += new _IAbtoPhoneEvents_OnEstablishedCallEventHandler(this.AbtoPhone_OnEstablishedCall);
                //this.AbtoPhone.OnIncomingCall += new _IAbtoPhoneEvents_OnIncomingCallEventHandler(this.AbtoPhone_OnIncomingCall);
                //this.AbtoPhone.OnClearedCall += new _IAbtoPhoneEvents_OnClearedCallEventHandler(this.AbtoPhone_OnClearedCall);
                //this.AbtoPhone.OnVolumeUpdated += new _IAbtoPhoneEvents_OnVolumeUpdatedEventHandler(this.AbtoPhone_OnVolumeUpdated);
                //this.AbtoPhone.OnRegistered += new _IAbtoPhoneEvents_OnRegisteredEventHandler(this.AbtoPhone_OnRegistered);
                //this.AbtoPhone.OnUnRegistered += new _IAbtoPhoneEvents_OnUnRegisteredEventHandler(this.AbtoPhone_OnUnRegistered);
                //this.AbtoPhone.OnPlayFinished += new _IAbtoPhoneEvents_OnPlayFinishedEventHandler(this.AbtoPhone_OnPlayFinished);
                //this.AbtoPhone.OnEstablishedConnection += new _IAbtoPhoneEvents_OnEstablishedConnectionEventHandler(this.AbtoPhone_OnEstablishedConnection);
                //this.AbtoPhone.OnClearedConnection += new _IAbtoPhoneEvents_OnClearedConnectionEventHandler(this.AbtoPhone_OnClearedConnection);
                //this.AbtoPhone.OnToneReceived += new _IAbtoPhoneEvents_OnToneReceivedEventHandler(this.AbtoPhone_OnToneReceived);
                //this.AbtoPhone.OnTextMessageReceived += new _IAbtoPhoneEvents_OnTextMessageReceivedEventHandler(this.AbtoPhone_OnTextMessageReceived);
                //this.AbtoPhone.OnTextMessageSentStatus += new _IAbtoPhoneEvents_OnTextMessageSentStatusEventHandler(AbtoPhone_OnTextMessageSentStatus);
                //this.AbtoPhone.OnPhoneNotify += new _IAbtoPhoneEvents_OnPhoneNotifyEventHandler(this.AbtoPhone_OnPhoneNotify);
                //this.AbtoPhone.OnRemoteAlerting += new _IAbtoPhoneEvents_OnRemoteAlertingEventHandler(this.AbtoPhone_OnRemoteAlerting);
                //this.AbtoPhone.OnSubscribeStatus += new _IAbtoPhoneEvents_OnSubscribeStatusEventHandler(AbtoPhone_OnSubscribeStatus);
                //this.AbtoPhone.OnSubscriptionNotify += new _IAbtoPhoneEvents_OnSubscriptionNotifyEventHandler(AbtoPhone_OnSubscriptionNotify);




                //this.AbtoPhone.OnDetectedAnswerTime+= new _IAbtoPhoneEvents_OnDetectedAnswerTimeEventHandler(this.AbtoPhone_OnDetectedAnswerTime);
                //this.AbtoPhone.OnRecordFinished +=new _IAbtoPhoneEvents_OnRecordFinishedEventHandler(AbtoPhone_OnRecordFinished);
                //this.AbtoPhone.OnReceivedRequestInfo += new _IAbtoPhoneEvents_OnReceivedRequestInfoEventHandler(AbtoPhone_OnReceivedRequestInfo);
                //AbtoPhone.OnToneDetected += new _IAbtoPhoneEvents_OnToneDetectedEventHandler(AbtoPhone_OnToneDetected);
                //this.AbtoPhone.OnPlayFinished2 += new _IAbtoPhoneEvents_OnPlayFinished2EventHandler(AbtoPhone_OnPlayFinished2);


                //Get config
                CConfig phoneCfg = AbtoPhone.Config;

                //Load config values from file
                phoneCfg.Load("phoneCfg.ini");

                //Manual set needed values
                //phoneCfg.ListenPort = 5060;
                //phoneCfg.StunServer = "";			

                phoneCfg.RegDomain = "172.27.138.185";
                phoneCfg.RegUser = "1007";
                phoneCfg.RegPass = "1007";
                phoneCfg.RegAuthId = "1007";
                //phoneCfg.RegExpire();		

                //Specify Licensy key
                phoneCfg.LicenseUserId = "Trial_version_for_Thanga_Prakash-90A4-576A-6BAF3881-61F2-0E9E-8739-4577AA869540";
                phoneCfg.LicenseKey = "jvJYD5dyHkA6X0QcB22UXRYYVbpzsaN7e/2sH0vxxAFR1jBTNfXt4qzNJHlDQH+k6CNsbFoaJhKlX4PSa5+W6g==";

                //Log level
                //phoneCfg.LogLevel= (LogLevelType)11;// LogLevelType.eLogDebug;//eLogError//(LogLevelType)11;
                //phoneCfg.LogPath = "C:\\temp\\logs";

                //Set AdditionalDnsServer as google dns
                //phoneCfg.AdditionalDnsServer = "8.8.8.8";

                //Specify network interface
                //phoneCfg.ActiveNetworkInterface = "...";
                //phoneCfg.TonesTypesToDetect = (int)ToneType.eToneMF + (int)ToneType.eToneDtmf;
                //phoneCfg.SignallingTransport = (int)TransportType.eTransportTLSv1;		
                //phoneCfg.TlsVerifyServerEnabled = false;
                //phoneCfg.EncryptedCallEnabled = 1;


                //phoneCfg.RegExpire =0;
                //phoneCfg.UserAgent = AbtoPhone.RetrieveVersion();
                string version = AbtoPhone.RetrieveVersion();

                //Set video windows
                //phoneCfg.RemoteVideoWindow = pictureReceivedVideo.Handle.ToInt32();
                //phoneCfg.LocalVideoWindow = pictureLocalVideo.Handle.ToInt32();
                //phoneCfg.RemoteVideoWindow_Add(pictureReceivedVideo.Handle.ToInt32());

                //phoneCfg->put_AudioQosDscpVal(40);
                //phoneCfg->put_VideoQosDscpVal(50);

                //... other initializations	

                //Apply modified config
                AbtoPhone.ApplyConfig();

                try
                {
                    AbtoPhone.Initialize();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return true;
        }

        private void txtDial_TextChanged(object sender, EventArgs e)
        {
            txtDial.Text = txtDial.Text.Replace(" ", "");
        }

        private void btnDial_Click(object sender, EventArgs e)
        {
            try
            {
                string address = txtDial.Text;
                if (address.Length == 0) return;
                int connId = AbtoPhone.StartCall2(address);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (txtSendNumber.Text.Length > 0)
            {
                bool bDtmfSent = false;
                while (!bDtmfSent)
                {
                    try
                    {
                        //AbtoPhone.SendToneEx(Convert.ToInt32(txtSendNumber.Text), 200, 1, 1, 0);                        
                        AbtoPhone.SendTone(txtSendNumber.Text);
                        bDtmfSent = true;
                    }
                    catch (Exception)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
                txtSendNumber.Text = string.Empty;
            }
        }

        private void btnHangup_Click(object sender, EventArgs e)
        {
            try
            {
                AbtoPhone.HangUpLastCall();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
