using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Reflection;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using RestSharp;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.IO;

namespace VortexDial
{

    using ConnectionInfo = KeyValuePair<int, string>;
    using ConnectionsTbl = Dictionary<int, string>;

    public class LineInfo
    {

        public LineInfo(int id)
        {
            m_id = id;
            m_conn = new ConnectionsTbl();
            m_bCalling = false;
            m_bCallEstablished = false;
           // m_bCallHeld = false;
           // m_bCallPlayStarted = false;
            m_usrInputStr = "";
        }

        public ConnectionsTbl m_conn;
        public int m_id;
        public bool m_bCalling;
        public bool m_bCallEstablished;
      //  public bool m_bCallHeld;
      //  public bool m_bCallPlayStarted;
        public string m_usrInputStr;
    }

    class ConnListBoxItem
    {
        public int handle;
        public string connection;

        public ConnListBoxItem(int _handle, string _connection)
        {
            handle = _handle;
            connection = _connection;
        }

        public override string ToString()
        {
            return this.connection;
        }
    }

    public class Vortex
    {
        private bool _bIsConnected;

        [Description("Indicates if the dialer server is connected.")]
        public bool IsConnected
        {
            get { return _bIsConnected; }
            //set { _bIsConnected = value; }
        }


        private string _sAuthKey;
        [Description("Enter the 4-digit authentication key heard from hard phone.")]
        public string Authkey
        {
            get { return _sAuthKey; }
            set { _sAuthKey = value; }
        }

        private int _iAuthTimeout;
        [Description("Maximum time to wait for authentication code from user input.(in seconds)")]
        [DefaultValue(15)]
        public int AuthenticationTimeout
        {
            get { return _iAuthTimeout; }
            set { _iAuthTimeout = value; }
        }

        private bool _IsHardPhoneAutheticated;
        [Description("Hard phone authentication Status")]
        [DefaultValue(false)]
        public bool HardPhoneAutheticated
        {
            get { return _IsHardPhoneAutheticated; }
            //set { _IsHardPhoneAutheticated = value; }
        }


        private bool _supportVox;
        [Description("Compatible with Vox Reader")]
        [DefaultValue(true)]
        public bool SupportVox
        {
            get { return _supportVox; }
            set { _supportVox = value; }
        }

        private int _iMaxConcurrentCalls;
        [Description("Maximum Concurrent Calls in a session")]
        [DefaultValue(true)]
        public int MaxConcurrentCalls
        {
            get { return _iMaxConcurrentCalls; }
        }

        private bool _ConcurrentDial;
        [Description("Enable/Diable Concurrent Dialing")]
        [DefaultValue(false)]
        public bool ConcurrentDial
        {
            get { return _ConcurrentDial; }
            set { _ConcurrentDial = value; }
        }

        private bool _SupportIDialer;
        
        [Description("Compatible with iDialer"), DefaultValue(true)]
        public bool SupportiDialer { get { return _SupportIDialer; } set { _SupportIDialer = value; } }


        public event DialerEventHandler DialerEventRecieved = null;
        public event PhoneEventHandler PhoneEventRecieved = null;
        BackgroundWorker bWorker = new BackgroundWorker();
        BackgroundWorker bAuth = new BackgroundWorker();        



        string sDialerIP = "172.27.138.185";
        int iDialerPort = 1006;
        string sCodecs = "G.711 mu-law|G.711 a-law|RFC4733 DTMF tones|H.263-1998|G.729A";
        string sSoftLicUser = "Trial6cb9-90F2-577A-6BAF3881-61F2-0E9E-8739-4577AA869540";
        string sSoftLicKey = "XBdftFtO2xrtUMCqeearoOyqwda+xbPl3E6oPeGX7+L8gU5LBd/0f4VuKfFqfSzMONQmztJVJzw5SmfWtdlFMA==";
        static string sSystemLogPath = @"\\172.27.137.182\Campaign Manager\Vortex";
        

        string sExtension = string.Empty;
        string sExtID = string.Empty;
        Socket socket;
        string sConString = string.Empty;
        //bool IsHardAuthenticated = false;
        System.Windows.Forms.Timer tAuthTimeout = new System.Windows.Forms.Timer();
        int iAuthTimer = 0;
        DataTable dtLog;
        static string IP = GetIP();
        string sPreAnswerRecordingPath = string.Empty;
        string sAnsweredRecordingPath = string.Empty;
        string sRecordingFormat = string.Empty;
        
        bool IsSoftPhoneConnected = false;
        bool HardPhone = false;

        private string _SoftPhoneState;
        public string SoftPhoneState
        {
            get { return _SoftPhoneState = string.Empty; }
            //set { _bIsConnected = value; }
        }      

        VortexPhone SPhone;
        System.Collections.ArrayList m_lineConnections = new System.Collections.ArrayList();
        private int m_curLineId = 1;


        //public static void Main()
        //{
        //    foreach (string sResource in (System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames()))
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            (System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(sResource)).CopyTo(ms);
        //            //  ms.ToArray();

        //            string sFileName = sResource.Replace("Vortex.", string.Empty).Replace("SoftPhone.", string.Empty);

        //            //if (sFileName.Contains("Newtonsoft") || sFileName.Contains("RestSharp"))
        //            //    WriteFile(sFileName, ms.ToArray(), string.Empty);
        //            //else
        //            //    WriteFile(sFileName, ms.ToArray(), "SoftPhone");


        //        }
        //    }
        //}  
        
        StringBuilder sSystemLog = new StringBuilder();
        static string sLogName = "Vortex.txt";
        public Vortex()
        {
            
            if (IP.Length > 0)
                sLogName = IP.Replace("'", "") + ".txt";

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //bAuth.WorkerSupportsCancellation = true;
            bWorker.WorkerReportsProgress = true;
            _bIsConnected = false;
            _IsHardPhoneAutheticated = false;
            tAuthTimeout.Interval = 1000;

            int LineCount = 2;

            for (int i = 1; i <= LineCount; ++i)
                m_lineConnections.Add(new LineInfo(i));


            dtLog = new DataTable();
            dtLog.Columns.Add("Telephone");
            dtLog.Columns.Add("UniqueID");
            dtLog.Columns.Add("CallState");
            dtLog.Columns.Add("ProjectID");
            dtLog.Columns.Add("TeamName");
            dtLog.Columns.Add("ReferenceID");
            dtLog.Columns.Add("EmployeeID");
            dtLog.Columns.Add("CallType");
            dtLog.Columns.Add("AnswerTime");
            dtLog.Columns.Add("CallerID");
            dtLog.Columns.Add("RecordName");

            sConString = Connection.Connection.Getstring("VORTEX");

            WriteFile("avcodec-53.dll", VortexDial.Properties.Resources.avcodec_53, "SoftPhone");
            WriteFile("avutil-51.dll", VortexDial.Properties.Resources.avutil_51, "SoftPhone");
            WriteFile("codec_amr.dll", VortexDial.Properties.Resources.codec_amr, "SoftPhone");
            WriteFile("codec_g726.dll", VortexDial.Properties.Resources.codec_g726, "SoftPhone");
            WriteFile("codec_g729.dll", VortexDial.Properties.Resources.codec_g729, "SoftPhone");
            WriteFile("codec_gsm.dll", VortexDial.Properties.Resources.codec_gsm, "SoftPhone");
            WriteFile("codec_ilbc.dll", VortexDial.Properties.Resources.codec_ilbc, "SoftPhone");
            WriteFile("codec_pcmapcmu.dll", VortexDial.Properties.Resources.codec_pcmapcmu, "SoftPhone");
            WriteFile("codec_speex.dll", VortexDial.Properties.Resources.codec_speex, "SoftPhone");
            WriteFile("codec_tones.dll", VortexDial.Properties.Resources.codec_tones, "SoftPhone");
            WriteFile("Interop.SIPVoipSDK.dll", VortexDial.Properties.Resources.Interop_SIPVoipSDK, "SoftPhone");
            WriteFile("Interop.SpeechLib.dll", VortexDial.Properties.Resources.Interop_SpeechLib, "SoftPhone");
            WriteFile("lame.exe", VortexDial.Properties.Resources.lame, "SoftPhone");
            WriteFile("lame_enc.dll", VortexDial.Properties.Resources.lame_enc, "SoftPhone");
            WriteFile("libspandsp.dll", VortexDial.Properties.Resources.libspandsp, "SoftPhone");
            WriteFile("Microsoft.VC80.CRT.manifest", VortexDial.Properties.Resources.Microsoft_VC80_CRT, "SoftPhone");
            WriteFile("mozart.wav", VortexDial.Properties.Resources.mozart, "SoftPhone");

            WriteFile("msvcp80.dll", VortexDial.Properties.Resources.msvcp80, "SoftPhone");
            WriteFile("msvcr80.dll", VortexDial.Properties.Resources.msvcr80, "SoftPhone");
            WriteFile("SIPVoipSDK.dll", VortexDial.Properties.Resources.SIPVoipSDK, "SoftPhone");
            //WriteFile("SIPVoipSDK64.dll", VortexDial.Properties.Resources.SIPVoipSDK64, "SoftPhone");
            WriteFile("swscale-2.dll", VortexDial.Properties.Resources.swscale_2, "SoftPhone");
            WriteFile("telephone-ring.wav", VortexDial.Properties.Resources.telephone_ring, "SoftPhone");
            
            //foreach(string sResource in  (System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames()))
            //{
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        (System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(sResource)).CopyTo(ms);
            //        //  ms.ToArray();

            //        string sFileName = sResource.Replace("Vortex.", string.Empty).Replace("SoftPhone.", string.Empty);

            //        if (sFileName.Contains("Newtonsoft") || sFileName.Contains("RestSharp"))
            //            WriteFile(sFileName, ms.ToArray(), string.Empty);
            //        else
            //            WriteFile(sFileName, ms.ToArray(), "SoftPhone");


            //    }
            //}
        }

        public static DateTime GetDateTime()
        {
            DateTime UtcDateTime = DateTime.UtcNow;
            TimeZoneInfo GMTTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(UtcDateTime, GMTTimeZone);
        }

        public static void Error_Log(MethodBase mBase, Exception ex, bool IsHardPhone = false, string sAdditionalInfo = "")
        {                        
            string sRowNumber = string.Empty;
            string sColNumber = string.Empty;
            string sMethod = string.Empty;
            
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
            System.Diagnostics.StackFrame sfFrame = null;
            for (int i = 0; i <= trace.FrameCount; i++)
            {
                sfFrame = trace.GetFrame(i);
                if (sfFrame != null && sfFrame.GetMethod().Name == mBase.Name)
                    break;
            }
            if (sfFrame != null)
            {
                sRowNumber = sfFrame.GetFileLineNumber().ToString();
                sColNumber = sfFrame.GetFileColumnNumber().ToString();
                if (mBase == null)
                    sMethod = "(MethodBase is Null) " + sfFrame.GetMethod().DeclaringType.FullName + "." + sfFrame.GetMethod().Name;
                else
                    sMethod = mBase.DeclaringType.Name + "." + mBase.ToString();
            }
            else if (mBase != null)
                sMethod = "(StackFrame is Null)" + mBase.DeclaringType.Name + "." + mBase.ToString();



            string sWriteError = Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            sWriteError += "Application : " + AppDomain.CurrentDomain.FriendlyName + Environment.NewLine + Environment.NewLine;
            sWriteError += "Error Date : " + GetDateTime() + Environment.NewLine + Environment.NewLine;
            sWriteError += "Message : " + ex.Message + Environment.NewLine + Environment.NewLine;
            sWriteError += "Softphone : " + !IsHardPhone + Environment.NewLine + Environment.NewLine;
            sWriteError += "Target Site : " + ex.TargetSite.ToString().Trim() + Environment.NewLine + Environment.NewLine;
            sWriteError += "Stack Trace : " + ex.StackTrace.Trim() + Environment.NewLine + Environment.NewLine;

            if (sAdditionalInfo.Length > 0)
            {
                sAdditionalInfo = "Query::" + sAdditionalInfo + "|||" + "Method::" + sMethod;
                sWriteError += "Additional Info : " + sAdditionalInfo;
            }
            else
                sAdditionalInfo = "Method::" + sMethod;

            if (sRowNumber.Length > 0)
                sWriteError += "Row : " + sRowNumber + ":" + sColNumber;

           
            try
            {                                                                
                if(sSystemLogPath.Length > 0)
                    WriteLog(sSystemLogPath + "\\" + sLogName, sWriteError, true);
                else
                    WriteLog(@"\\172.27.137.182\Campaign Manager\Vortex\" + sLogName, sWriteError, true);
                
            }
            catch (Exception ex11)
            {
                WriteLog(AppDomain.CurrentDomain.BaseDirectory + "\\" + sLogName, sWriteError, true);
            }
        }

        private string WriteFile(string sFileName, Byte[] ByteFile, string sDir)
        {
            string sPath = AppDomain.CurrentDomain.BaseDirectory + @"\" + sDir;
            try
            {
                if (!Directory.Exists(sPath))
                    Directory.CreateDirectory(sPath);
                if (!(File.Exists(sPath + "\\" + sFileName)))
                    File.WriteAllBytes(sPath + "\\" + sFileName, ByteFile);
            }
            catch (Exception ex)
            { }
            return sPath + "\\" + sFileName;
        }

        public static void WriteLog(string sPath, string Content, bool Append)
        {
            try
            {
                using (StreamWriter sWrite = new StreamWriter(sPath, Append))
                {
                    sWrite.WriteLine(Content);
                    sWrite.Close();
                }
            }
            catch(Exception ex)
            {
                using (StreamWriter sWrite = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Vortex.txt", Append)){sWrite.WriteLine(Content);sWrite.Close();}                
            }
        }

        public static string GetIP()
        {
            try
            {
                foreach (System.Net.NetworkInformation.NetworkInterface ni in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Ethernet)
                    {
                        foreach (System.Net.NetworkInformation.UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && ip.Address.ToString().StartsWith("172.27."))
                                return ip.Address.ToString();
                        }
                    }
                }
                //return "01.01.01.01";
                return Environment.MachineName;
            }
            catch(Exception ex)
            { return Environment.MachineName; }
        }

        public string Connect(string ClientIP, string ApplicationName, bool HardPhone, bool ConcurrentDialing)
        {
            try
            {
                _bIsConnected = false;
                ConcurrentDial = ConcurrentDialing;
                if (sConString.Trim().Length == 0)
                    return "DB Connection failed.";

                if (ClientIP.Trim().Length == 0)
                    return "Invalid Client IP.";

                using (DataTable dtExt = ExecuteTable("UPDATE TOP (1) Extensions SET LastConnected = GETDATE(), LastUsedApplication = '" + ApplicationName.Replace("'", "''").Trim() + "' WHERE IP='"+ ClientIP.Replace("'", "''").Trim() + "';SELECT ID,Extension FROM Call_Timesheet..Extensions WHERE IP = '" + ClientIP.Replace("'", "''").Trim() + "' AND Active = 'Y';", sConString))
                {
                    if (dtExt != null && dtExt.Rows.Count > 0 && dtExt.Rows[0][1].ToString().Length > 0)
                    {
                        sExtension = dtExt.Rows[0][1].ToString();
                        sExtID = dtExt.Rows[0][0].ToString();
                    }
                    else
                    {
                        sExtension = string.Empty;
                        sExtID = string.Empty;

                        if (sExtension.Trim().Length == 0)
                            return "Extension not assigned to this machine (or) Invalid Client IP.";
                    }
                }

                using (DataTable dtConfig = ExecuteTable("SELECT * FROM Call_Timesheet..Vortex_Config;", sConString))
                {                    
                    if (dtConfig.Select("Name = 'DialerServerIP'").Length > 0 && dtConfig.Select("Name = 'DialerServerIP'")[0]["Value"].ToString().Trim().Length > 0)
                        sDialerIP = dtConfig.Select("Name = 'DialerServerIP'")[0]["Value"].ToString().Trim();

                    if (dtConfig.Select("Name = 'DialerServerPort'").Length > 0 && dtConfig.Select("Name = 'DialerServerPort'")[0]["Value"].ToString().All(char.IsDigit))
                        iDialerPort = Convert.ToInt32(dtConfig.Select("Name = 'DialerServerPort'")[0]["Value"].ToString().Trim());

                    if (dtConfig.Select("Name = 'SP_Active_Codec'").Length > 0 && dtConfig.Select("Name = 'SP_Active_Codec'")[0]["Value"].ToString().Trim().Length > 0)
                        sCodecs = dtConfig.Select("Name = 'SP_Active_Codec'")[0]["Value"].ToString().Trim();
                    
                    if (dtConfig.Select("Name = 'SoftphoneLicUser'").Length > 0 && dtConfig.Select("Name = 'SoftphoneLicUser'")[0]["Value"].ToString().Trim().Length > 0)
                        sSoftLicUser = dtConfig.Select("Name = 'SoftphoneLicUser'")[0]["Value"].ToString().Trim();
                    
                    if (dtConfig.Select("Name = 'SoftphoneLicKey'").Length > 0 && dtConfig.Select("Name = 'SoftphoneLicKey'")[0]["Value"].ToString().Trim().Length > 0)
                        sSoftLicKey = dtConfig.Select("Name = 'SoftphoneLicKey'")[0]["Value"].ToString().Trim();

                    if (dtConfig.Select("Name = 'LogPath'").Length > 0 && dtConfig.Select("Name = 'LogPath'")[0]["Value"].ToString().Trim().Length > 0)
                        sSystemLogPath = dtConfig.Select("Name = 'LogPath'")[0]["Value"].ToString().Trim();

                    if (dtConfig.Select("Name = 'PreAnswerRecordingPath'").Length > 0 && dtConfig.Select("Name = 'PreAnswerRecordingPath'")[0]["Value"].ToString().Trim().Length > 0)
                        sPreAnswerRecordingPath = dtConfig.Select("Name = 'PreAnswerRecordingPath'")[0]["Value"].ToString().Trim();

                    if (dtConfig.Select("Name = 'CallAnsweredRecordingPath'").Length > 0 && dtConfig.Select("Name = 'CallAnsweredRecordingPath'")[0]["Value"].ToString().Trim().Length > 0)
                        sAnsweredRecordingPath = dtConfig.Select("Name = 'CallAnsweredRecordingPath'")[0]["Value"].ToString().Trim();

                    if (dtConfig.Select("Name = 'RecordingFormat'").Length > 0 && dtConfig.Select("Name = 'RecordingFormat'")[0]["Value"].ToString().Trim().Length > 0)
                        sRecordingFormat = dtConfig.Select("Name = 'RecordingFormat'")[0]["Value"].ToString().Trim().ToUpper();

                    if (ConcurrentDial)
                    {
                        if (dtConfig.Select("Name = 'ConcurrentDial'").Length > 0 && dtConfig.Select("Name = 'ConcurrentDial'")[0]["Value"].ToString().Trim().Length > 0)
                            _iMaxConcurrentCalls = Convert.ToInt32(dtConfig.Select("Name = 'ConcurrentDial'")[0]["Value"].ToString().Trim());
                    }
                    else
                        _iMaxConcurrentCalls = 1;
                }
            }
            catch (Exception ex)
            {
                Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, HardPhone, "Get Extension");
                return "Error fetching extension.";
            }

            //if (sExtension.Trim().Length == 0)
            //    return "Extension not assigned to this machine (or) Invalid Client IP.";            
            
            try
            {
                if (!socket.Connected)
                {
                    socket.Connect(sDialerIP, iDialerPort);
                    _bIsConnected = true;
                }

                if (!HardPhone)
                {
                    if (SoftPhoneState.Length == 0)
                    {
                        SPhone = new VortexPhone();
                        SPhone.OnInitialized += new _IVortexPhoneEvents_OnInitializedEventHandler(this.VortexPhone_OnInitialized);
                        SPhone.OnLineSwiched += new _IVortexPhoneEvents_OnLineSwichedEventHandler(this.VortexPhone_OnLineSwiched);
                        SPhone.OnEstablishedCall += new _IVortexPhoneEvents_OnEstablishedCallEventHandler(this.VortexPhone_OnEstablishedCall);
                        SPhone.OnIncomingCall += new _IVortexPhoneEvents_OnIncomingCallEventHandler(this.VortexPhone_OnIncomingCall);
                        SPhone.OnClearedCall += new _IVortexPhoneEvents_OnClearedCallEventHandler(this.VortexPhone_OnClearedCall);
                        //SPhone.OnVolumeUpdated += new _IVortexPhoneEvents_OnVolumeUpdatedEventHandler(this.VortexPhone_OnVolumeUpdated);
                        SPhone.OnRegistered += new _IVortexPhoneEvents_OnRegisteredEventHandler(this.VortexPhone_OnRegistered);
                        SPhone.OnUnRegistered += new _IVortexPhoneEvents_OnUnRegisteredEventHandler(this.VortexPhone_OnUnRegistered);
                        //SPhone.OnPlayFinished += new _IVortexPhoneEvents_OnPlayFinishedEventHandler(this.VortexPhone_OnPlayFinished);
                        SPhone.OnEstablishedConnection += new _IVortexPhoneEvents_OnEstablishedConnectionEventHandler(this.VortexPhone_OnEstablishedConnection);
                        SPhone.OnClearedConnection += new _IVortexPhoneEvents_OnClearedConnectionEventHandler(this.VortexPhone_OnClearedConnection);
                        SPhone.OnToneReceived += new _IVortexPhoneEvents_OnToneReceivedEventHandler(this.VortexPhone_OnToneReceived);
                        //SPhone.OnTextMessageReceived += new _IVortexPhoneEvents_OnTextMessageReceivedEventHandler(this.VortexPhone_OnTextMessageReceived);
                        //SPhone.OnTextMessageSentStatus += new _IVortexPhoneEvents_OnTextMessageSentStatusEventHandler(VortexPhone_OnTextMessageSentStatus);
                        SPhone.OnPhoneNotify += new _IVortexPhoneEvents_OnPhoneNotifyEventHandler(this.VortexPhone_OnPhoneNotify);
                        SPhone.OnRemoteAlerting += new _IVortexPhoneEvents_OnRemoteAlertingEventHandler(this.VortexPhone_OnRemoteAlerting);

                        

                        //this.AbtoPhone.OnToneDetected += new _IAbtoPhoneEvents_OnToneDetectedEventHandler(AbtoPhone_OnToneDetected);
                        CConfig phoneCfg = SPhone.Config;


                        

                        phoneCfg.RegDomain = sDialerIP;
                        phoneCfg.RegUser = sExtension;
                        phoneCfg.RegPass = sExtension;
                        phoneCfg.RegAuthId = sExtension;
                        phoneCfg.LogLevel = (CConfig.LogLevelType)11;
                        phoneCfg.EchoCancelationEnabled = 1;
                        phoneCfg.NoiseReductionEnabled = 0;
                        phoneCfg.AutoGainControlEnabled = 0;
                        phoneCfg.DialToneEnabled = 0;
                        string sVersion = SPhone.RetrieveVersion();
                        string sPath = SPhone.SDKPath();

                        if(sRecordingFormat == "MP3")
                            phoneCfg.MP3RecordingEnabled = 1;

                        phoneCfg.SetCodecOrder(sCodecs, 0);

                        //phoneCfg.RegExpire();		

                        //Specify Licensy key
                        //phoneCfg.LicenseUserId = "Trial_version_for_Thanga_Prakash-90A4-576A-6BAF3881-61F2-0E9E-8739-4577AA869540";
                        //phoneCfg.LicenseKey = "jvJYD5dyHkA6X0QcB22UXRYYVbpzsaN7e/2sH0vxxAFR1jBTNfXt4qzNJHlDQH+k6CNsbFoaJhKlX4PSa5+W6g==";

                        phoneCfg.LicenseUserId = sSoftLicUser;
                        phoneCfg.LicenseKey = sSoftLicKey;

                     //   System.Windows.Forms.MessageBox.Show(sSoftLicUser);
                        //string version = SPhone.RetrieveVersion();                    
                        //Apply modified config
                        SPhone.ApplyConfig();                        

                        try
                        {
                            SPhone.Initialize();
                            _SoftPhoneState = "Connecting";
                        }
                        catch (Exception ex)
                        {
                            IsSoftPhoneConnected = false;
                            Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, HardPhone, "Initialize Softphone");
                            PhoneEvents("InitializeError", 1, ex.Message);
                            return ex.Message;
                        }
                    }
                }
                //this.OnDialerEventRecieved("Dialer Connection sucess");
                bWorker.DoWork += bWorker_DoWork;
                bWorker.RunWorkerCompleted += bWorker_RunWorkerCompleted;
                bWorker.ProgressChanged += bWorker_Progress;
                bWorker.RunWorkerAsync();
                return string.Empty;

            }
            catch (Exception ex)
            {
                Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, HardPhone);
                PhoneEvents("InitializeError",1, ex.Message);
                _bIsConnected = false;
                return "Dialer is not reachable";
            }            
        }

        #region Softphone Events

        #region SoftPhone Methods
        public LineInfo GetCurrentLine()
        {
            return (LineInfo)m_lineConnections[m_curLineId - 1];
        }

        public LineInfo GetLine(int lineId)
        {
            return (LineInfo)m_lineConnections[lineId - 1];
        }

        public List<LineInfo> GetActiveLines()
        {
            List<LineInfo> lstLineInfo = new List<LineInfo>();
            foreach(LineInfo lInfo in m_lineConnections)
            {
                if (lInfo.m_bCalling || lInfo.m_bCallEstablished)
                    lstLineInfo.Add(lInfo);
            }
            return lstLineInfo;

        }

        public List<LineInfo> GetAllLines()
        {
            List<LineInfo> lstLineInfo = new List<LineInfo>();
            foreach (LineInfo lInfo in m_lineConnections)
                lstLineInfo.Add(lInfo);
            return lstLineInfo;
        }



        public string SendTone(Char Tone)
        {
            if (Tone.ToString().All(char.IsDigit))
            { //Do Nothing
            }
            else if(Tone.ToString() == "*" || Tone.ToString() == "#")
            { }
            else
                return "Invalid Input";

            if (HardPhone)            
                return "Send Tone not possible on Hard phones";
            else
            {
                if(IsSoftPhoneConnected)
                {
                    if(GetCurrentLine().m_bCallEstablished)
                    {
                        SPhone.SendTone(Tone.ToString());
                    }
                    else
                        return "Not in call";
                }
                else
                    return "Softphone not connected";
            }
            return string.Empty;
        }

        public void SetCurrentLine(int LineID)
        {
            SPhone.SetCurrentLine(LineID);            
        }

       

        public void MuteMic(bool Mute)
        {
            if (SPhone != null)
            {
                if (Mute)
                    SPhone.RecordMuted = 1;
                else
                    SPhone.RecordMuted = 0;
            }
        }

        public void MuteSpeaker(bool Mute)
        {
            if (SPhone != null)
            {
                if (Mute)
                    SPhone.PlaybackMuted = 1;
                else
                    SPhone.PlaybackMuted = 0;
            }
        }

        public void SetSpeakerVolume(int Volume)
        {
            if (SPhone != null)
            {
                if (Volume > 100)
                    SPhone.PlaybackVolume = 100;
                else if (Volume < 0)
                    SPhone.PlaybackVolume = 0;
                else
                    SPhone.PlaybackVolume = Volume;
            }
        }

        public void SetMicVolume(int Volume)
        {
            if (SPhone != null)
            {
                if (Volume > 100)
                    SPhone.RecordVolume = 100;
                else if (Volume < 0)
                    SPhone.RecordVolume = 0;
                else
                    SPhone.RecordVolume = Volume;
            }
        }

        public string GetAudioConfig()
        {
            if (SPhone != null)
            {
                string sReturn = string.Empty;

                if (SPhone.Config.NoiseReductionEnabled == 1)
                    sReturn = "NR:1";
                else
                    sReturn = "NR:0";

                if (SPhone.Config.EchoCancelationEnabled == 1)
                    sReturn += "|EC:1";
                else
                    sReturn += "|EC:0";

                if (SPhone.Config.AutoGainControlEnabled == 1)
                    sReturn += "|AC:1";
                else
                    sReturn += "|AC:0";
                sReturn += "|MVol:" + SPhone.RecordVolume;
                sReturn += "|SVol:" + SPhone.PlaybackVolume;

                return sReturn;
            }
            return string.Empty;
        }

        public bool IsLineinUse(int LineID)
        {
            return SPhone.IsLineOccupied(LineID) != 0;            
        }

        public void AudioConfig(bool EchoCancellation, bool NoiseReduction, bool AutoGainControl)
        {
            
        }

        #endregion


        //private void VortexPhone_OnLineSwiched(int lineId)
        //{
        //    //Display line as pressed button
        //    HighlightCurLine(m_curLineId, lineId);

        //    //Remember
        //    m_curLineId = lineId;

        //    //Display conections of cur line
        //    LineInfo lnInfo = GetCurrentLine();
        //    DisplayConnectionsAll(lnInfo);

        //    //Show/Hide call controls
        //    ChageControlsState(lnInfo);
        //}


        //private void VortexPhone_OnVolumeUpdated(int IsMicrophone, int level)
        //{
        //    if (IsMicrophone == 0)
        //        spkVolumeBar.Value = level;
        //    else
        //        micVolumeBar.Value = level;
        //}

        //private void VortexPhone_OnPlayFinished(string Msg)
        //{
        //    string playStr = "Play Finished on Line: ";

        //    int idx = Msg.IndexOf(playStr);
        //    if (idx == 0)
        //    {
        //        string lineStr = Msg.Substring(playStr.Length);
        //        stopStartPlaying(true, int.Parse(lineStr));
        //    }

        //    displayNotifyMsg(Msg);
        //}

        //private void VortexPhone_OnTextMessageReceived(string from, string message)
        //{
        //    displayNotifyMsg("'" + message + "' received from: " + from);
        //}

        //private void VortexPhone_OnTextMessageSentStatus(string address, string reason, int bSuccess)
        //{
        //    if (bSuccess != 0) displayNotifyMsg("Message sent succesfully to " + address + " Reason: " + reason);
        //    else displayNotifyMsg("Message sent failure to " + address + " Reason: " + reason);
        //}

        private void VortexPhone_OnLineSwiched(int lineId)
        {           
            //Remember
            m_curLineId = lineId;
            PhoneEvents("LineSwitched", lineId);
        }

        private void VortexPhone_OnInitialized(string Msg)
        {
            if (Msg.Contains("expired") || Msg.Contains("Invalid"))
            {
                IsSoftPhoneConnected = false;
                PhoneEvents("InitializeError",1, Msg);
            }
            else
                PhoneEvents("Initialized",1, Msg);
            _SoftPhoneState = string.Empty;
        }


        private void VortexPhone_OnRegistered(string Msg)
        {
            if (Msg.Contains("failure"))
            {
                IsSoftPhoneConnected = false;
                PhoneEvents("RegistrationFailed", 1, Msg);                
            }
            else
            {
                IsSoftPhoneConnected = true;
                PhoneEvents("Registration", 1, Msg);







                SPhone.RecordVolume = 100;
                SPhone.PlaybackVolume = 100;








            }
            _SoftPhoneState = string.Empty;
        }


        private void VortexPhone_OnEstablishedConnection(string addrFrom, string addrTo, int connectionId, int lineId)
        {

            LineInfo lnInfo = GetLine(lineId);
            string addr = lnInfo.m_bCalling ? addrTo : addrFrom;
            lnInfo.m_conn.Add(connectionId, addr);            
            PhoneEvents("CallStarted", lineId, "");
        }

        private void VortexPhone_OnEstablishedCall(string adress, int lineId)
        {
            //Update line state

            LineInfo lnInfo = GetLine(lineId);
            lnInfo.m_usrInputStr = "";
            lnInfo.m_bCallEstablished = true;
            lnInfo.m_bCalling = false;

            //Update controls (only when it's cur line event)
           // if (lineId == m_curLineId)
            {
                PhoneEvents("CallAnswered", lineId, "");
            }
        }

        private void VortexPhone_OnIncomingCall(string adress, int lineId)
        {
            SPhone.AnswerCallLine(lineId);
            SPhone.RejectCallLine(lineId);
        }


        private void VortexPhone_OnToneReceived(int tone, int connectionId, int lineId)
        {            
            LineInfo lnInfo = GetLine(lineId);
            StringBuilder sb = new StringBuilder();
            sb.Append(lnInfo.m_usrInputStr);
            sb.Append((char)tone);
            lnInfo.m_usrInputStr = sb.ToString();            
        }

        private void VortexPhone_OnRemoteAlerting(int connectionId, int responseCode, string reasonMsg)
        {
            string str = responseCode.ToString() + "|" + reasonMsg;
            PhoneEvents("OnCall", m_curLineId, str);
        }

        private void VortexPhone_OnPhoneNotify(string message)
        {           
            //"Redirect Success. Connection: x";
            //"Redirect Failure. Connection: x Status y";
            Match match = Regex.Match(message, @"Redirect.*Connection: \d+");
            if (match.Success)
            {
                string connIdStr = Regex.Match(match.Value, @"\d+").Value;
                SPhone.HangUp(int.Parse(connIdStr));
            }
        }


        private void VortexPhone_OnClearedCall(string Msg, int status, int lineId)
        {
            //Update line state            
            LineInfo lnInfo = GetLine(lineId);
            lnInfo.m_usrInputStr = "";
            lnInfo.m_bCallEstablished = false;
            lnInfo.m_bCalling = false;

            //Update controls (only when it's cur line event)
            //if (lineId == m_curLineId)
            {
                PhoneEvents("CallHangup", lineId, "");
            }
        }

        private void VortexPhone_OnClearedConnection(int connectionId, int lineId)
        {
            LineInfo lnInfo = GetLine(lineId);
            lnInfo.m_conn.Remove(connectionId);
        }


        private void VortexPhone_OnUnRegistered(string Msg)
        {
            IsSoftPhoneConnected = false;
            PhoneEvents("UnRegister", 1 , Msg);
        }

        void PhoneEvents(string EventType, int LineID, string sValue1 = "", string sValue2 = "", string sValue3 = "", string sValue4 = "")
        {            
            switch (EventType)
            {
                case "Initialized":
                case "InitializeError":
                case "Registration":
                case "RegistrationFailed":
                case "OnCall":
                case "UnRegister":
                    OnPhoneEventRecieved(EventType + "|" + sValue1, LineID);
                    break;
                case "CallStarted":
                case "CallAnswered":
                case "CallHangup":
                case "LineSwitched":
                    OnPhoneEventRecieved(EventType, LineID);
                    break;
            }
        }
        #endregion


        public void AthunticateHardPhone()
        {
            try
            {
                if (!IsConnected)
                    throw new Exception("Dialer not connected.");

                if (tAuthTimeout.Enabled)
                    tAuthTimeout.Stop();
                if (bAuth.IsBusy)
                {
                    bAuth.CancelAsync();
                    bAuth.Dispose();
                    bAuth = null;
                }
                bAuth = new BackgroundWorker();
                bAuth.WorkerSupportsCancellation = true;
                bAuth.DoWork += bAuth_DoWork;
                bAuth.RunWorkerCompleted += bAuth_RunWorkerCompleted;
                //bAuth.ProgressChanged += bWorker_Progress;
                _sAuthKey = string.Empty;
                iAuthTimer = 0;
                tAuthTimeout.Start();
                bAuth.RunWorkerAsync();
            }
            catch(Exception ex)
            {
                Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, HardPhone);
            }
        }

        private void AuthTimer_Tick(object sender, EventArgs e)
        {
            iAuthTimer++;
        }

        bool HardPhoneAuth()
        {
            try
            {
                if (sExtension.Length > 0)
                {
                    var rClient = new RestClient("http://172.27.138.185/call_initiate.php?ext=" + sExtension);
                    var rRequest = new RestRequest(Method.POST);
                    rRequest.JsonSerializer.ContentType = "application/x-www-form-urlencoded";
                    IRestResponse rResponse = rClient.Execute(rRequest);
                    //var content = rResponse.Content; // raw content as string
                    if (rResponse.ResponseStatus == ResponseStatus.Completed)
                    {
                        _sAuthKey = string.Empty;
                        while (Authkey.Length == 0)
                        {
                            if (iAuthTimer > AuthenticationTimeout)
                            {
                                return false;
                            }
                        }

                        var rClientAuth = new RestClient("http://172.27.138.185/check.php?ext=" + sExtension + "&voicemail=" + Authkey);
                        var rRequestAuth = new RestRequest(Method.POST);
                        rRequestAuth.JsonSerializer.ContentType = "application/x-www-form-urlencoded";
                        IRestResponse rResponseAuth = rClientAuth.Execute(rRequestAuth);
                        return rResponseAuth.Content.ToString().ToLower().Contains("susscessfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, HardPhone);
            }

            return false;
        }

       
       public string Hangup(int LineID)
        {
            if (IsConnected)
            {
                if (HardPhone)
                {
                    var rHangupclient = new RestClient("http://172.27.138.185/aster-dialer-api/index.php/ami/hangupami");
                    var rHanguprequest = new RestRequest(Method.POST);
                    rHanguprequest.AddHeader("Accept", "application/json");
                    rHanguprequest.Parameters.Clear();
                    string sHangUp = "{ \"hangupcall\": { \"extension\":\"" + sExtension + "\" } }";
                    rHanguprequest.AddParameter("application/json", sHangUp, ParameterType.RequestBody); // adds to POST or URL querystring based on Method
                    IRestResponse response = rHangupclient.Execute(rHanguprequest);
                    var content = response.Content; // raw content as string
                    return string.Empty;
                }
                else
                {
                    try
                    {

                        SPhone.HangUpCallLine(LineID);

                        //LineInfo lnInfo = GetCurrentLine();
                        //if (lnInfo.m_bCallEstablished || lnInfo.m_bCalling)
                        //{
                        //    SPhone.HangUpCallLine(lnInfo.m_id);
                        //}
                        //else
                        //    SPhone.HangUpLastCall();
                    }
                    catch(Exception ex)
                    {
                        if (sSystemLogPath.Length > 0)
                            WriteLog(sSystemLogPath + "\\" + sLogName, ex.Message, true);
                        else
                            WriteLog(@"\\172.27.137.182\Campaign Manager\Vortex\" + sLogName, ex.Message, true);
                    }
                    return string.Empty;
                }
            }
            else
                return "Dialer not connected.";
        }

        public void HangUpAll()
        {
            List<LineInfo> lstlInfo = GetActiveLines();
            foreach(LineInfo ln in lstlInfo)
            {
                if(ln.m_bCalling || ln.m_bCallEstablished)
                {
                    SPhone.HangUpCallLine(ln.m_id);
                }
            }
        }

        private void bAuth_DoWork(object sender, DoWorkEventArgs e)
        {
            _IsHardPhoneAutheticated = HardPhoneAuth();
        }

        public string Call(string TelephoneNumber, string ProjectID, string Division, string EmployeeID, bool PrefixOverride, int ReferenceID, int LineID)
        {
            try
            {
                if (!IsConnected)
                    return "Dialer not connected.";

                string sCallType = string.Empty;
                if (HardPhone)
                {
                    sCallType = "Hard";
                    if (_IsHardPhoneAutheticated)
                    {
                        dtLog.Rows.Add(TelephoneNumber, "", "", ProjectID, Division, ReferenceID, EmployeeID, sCallType, "");

                        if (SupportiDialer)
                            ExecuteQuery("INSERT INTO Call_Timesheet..AspectDialerLogger (AgentName, LoginID, StationID, TelephoneNumber, RecordingID, Duration, DateTimeStamp, ProjectName, CampaignID, Company_ID) VALUES ('" + EmployeeID + "','" + EmployeeID + "','" + sExtension + "','" + TelephoneNumber + "','0','00:00:00',GETDATE(),'" + ProjectID + "','" + Division + "','" + ReferenceID + "');", sConString);

                        string sCallState = PlaceCall(TelephoneNumber, EmployeeID, ProjectID, ReferenceID.ToString());
                        return sCallState;
                    }
                    else
                        return "Hard phone Authentication failed.";
                }
                else
                {
                    sCallType = "Soft";
                    if (IsSoftPhoneConnected)
                    {
                        dtLog.Rows.Add(TelephoneNumber, "", "", ProjectID, Division, ReferenceID, EmployeeID, sCallType, "");
                        if (SupportiDialer)
                            ExecuteQuery("INSERT INTO Call_Timesheet..AspectDialerLogger (AgentName, LoginID, StationID, TelephoneNumber, RecordingID, Duration, DateTimeStamp, ProjectName, CampaignID, Company_ID) VALUES ('" + EmployeeID + "','" + EmployeeID + "','" + sExtension + "','" + TelephoneNumber + "','0','00:00:00',GETDATE(),'" + ProjectID + "','" + Division + "','" + ReferenceID + "');", sConString);

                        //SPhone.HangUpLastCall();     
                        
                        SPhone.StartCall(TelephoneNumber);
                        
                        //SPhone.StartCallExLine(0, TelephoneNumber, "");

                        

                        GetCurrentLine().m_bCalling = true;

                        return m_curLineId.ToString();
                    }
                    else
                        return "Softphone connection failed.";                
                }
            }
            catch(Exception ex)
            {
                Error_Log(MethodBase.GetCurrentMethod(), ex, HardPhone);
                return ex.Message;
            }
        }

        string PlaceCall(string sTel, string sEmpID, string sProj, string sRefID)
        {
            var client = new RestClient("http://172.27.138.185/aster-dialer-api/index.php/ami/manualami");
            var request = new RestRequest(Method.POST);
            request.JsonSerializer.ContentType = "application/x-www-form-urlencoded";
            request.AddParameter("data", sExtension + "#" + sTel + "#data1=" + sRefID + "#data2=" + sEmpID + "#data3=" + sProj);
            IRestResponse response = client.Execute(request);
            //var content = response.Content; // raw content as string
            if (response.Content.ToString().Contains("Call success"))
                return string.Empty;
            else
                return "Call Failed";
        }

        static void ExecuteQuery(string sQuery, string sConStr)
        {
            using (SqlConnection con = new SqlConnection(sConStr))
            {
                try
                {
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    using (SqlCommand cmd = new SqlCommand(sQuery, con))
                    {
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                    }
                }
                catch (Exception ex)
                {                    
                    using (SqlConnection con1 = new SqlConnection(sConStr))
                    {
                        try
                        {
                            if (con1.State != System.Data.ConnectionState.Open)
                                con1.Open();
                            using (SqlCommand cmd = new SqlCommand(sQuery, con1))
                            {
                                cmd.ExecuteNonQuery();
                                con.Close();
                                con.Dispose();
                            }
                        }
                        catch (Exception ex1)
                        {
                            Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex1, false, sQuery);
                        }
                    }
                }
            }
        }

        static string InsertAndGetIdentity(string sQuery, string sConStr)
        {
            using (SqlConnection con = new SqlConnection(sConStr))
            {
                try
                {
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    string iIdentity;
                    sQuery += " SELECT @@IDENTITY";
                    //SqlConnection connection = new SqlConnection(GlobalVariables.sMSSQL);
                    SqlCommand cmd = new SqlCommand(sQuery, con);
                    cmd.CommandTimeout = 600;
                    iIdentity = cmd.ExecuteScalar().ToString();
                    return iIdentity;
                }
                catch (Exception ex)
                {
                    using (SqlConnection con1 = new SqlConnection(sConStr))
                    {
                        try
                        {
                            if (con.State != System.Data.ConnectionState.Open)
                                con.Open();
                            string iIdentity;
                            sQuery += " SELECT @@IDENTITY";
                            //SqlConnection connection = new SqlConnection(GlobalVariables.sMSSQL);
                            SqlCommand cmd = new SqlCommand(sQuery, con);
                            cmd.CommandTimeout = 600;
                            iIdentity = cmd.ExecuteScalar().ToString();
                            return iIdentity;
                        }
                        catch (Exception ex1)
                        {
                            Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex1, false, sQuery);
                            return string.Empty;
                        }
                    }
                }
            }
        }

        static DataTable ExecuteTable(string sQuery, string sConStr)
        {
            using (SqlConnection con = new SqlConnection(sConStr))
            {
                try
                {
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    using (SqlCommand cmd = new SqlCommand(sQuery, con))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dtReturn = new DataTable())
                            {
                                da.Fill(dtReturn);
                                con.Close();
                                con.Dispose();
                                return dtReturn;                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    using (SqlConnection con1 = new SqlConnection(sConStr))
                    {
                        try
                        {
                            if (con1.State != System.Data.ConnectionState.Open)
                                con1.Open();
                            using (SqlCommand cmd = new SqlCommand(sQuery, con1))
                            {
                                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                                {
                                    using (DataTable dtReturn = new DataTable())
                                    {
                                        da.Fill(dtReturn);
                                        con1.Close();
                                        con1.Dispose();
                                        return dtReturn;
                                    }
                                }
                            }
                        }
                        catch (Exception ex1)
                        {
                            Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex1, false, sQuery);
                        }
                    }

                    return null;
                }
            }
        }

        private void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Console.WriteLine("Connected");
                StringBuilder stringBuilder = new StringBuilder();
                while (true)
                {
                    byte[] buffer = new byte[20000];
                    stringBuilder.Clear();
                    int num = socket.Receive(buffer);
                    for (int index = 0; index < num; ++index)
                        stringBuilder.Append(Convert.ToChar(buffer[index]));

                    if (stringBuilder.Length > 0)
                    {
                        StreamWriter sWrite = new StreamWriter(@"Log.txt", true);
                        sWrite.WriteLine(stringBuilder.ToString());
                        sWrite.Close();            
                        
                                    
                        if (stringBuilder.ToString().StartsWith("{"))
                        {                            
                           List<string> lstJSon =  stringBuilder.ToString().Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries).Select(x => "{" + x + "}").ToList();
                            foreach (string sJString in lstJSon)
                            {
                                dynamic jSon = JsonConvert.DeserializeObject(sJString);
                                if (jSon != null)
                                {
                                    if (jSon.extension == sExtension)
                                    {
                                        if (jSon.evt == "INITIATED")
                                        {
                                            //DataRow[] drrLog = dtLog.Select("Telephone = '8919790783282'");
                                            DataRow[] drrLog = dtLog.Select("Telephone = '" + jSon.phonenumber + "'");
                                            if (drrLog.Length > 0)
                                            {
                                                drrLog[0]["UniqueID"] = jSon.uniqueid;
                                                drrLog[0]["CallState"] = "INITIATED";                                                
                                                drrLog[0]["RecordName"] = InsertAndGetIdentity("INSERT INTO Call_Timesheet..Call_Log (CallID, Extension, TelephoneNumber, InitiateTime, TeamName, ProjectID, ReferenceID, EmployeeID, CallType, CallStatus) VALUES('" + drrLog[0]["UniqueID"] + "', '" + sExtension + "', '" + drrLog[0]["Telephone"] + "', '" + jSon.calltime + "', '" + drrLog[0]["TeamName"] + "', '" + drrLog[0]["ProjectID"] + "', '" + drrLog[0]["ReferenceID"] + "', '" + drrLog[0]["EmployeeID"] + "', '" + drrLog[0]["CallType"] + "','INITIATED'); ", sConString);
                                                //bWorker.ReportProgress(0, "Call Initiated:" + drrLog[0]["Telephone"] + "||ID:" + jSon.uniqueid + "||Ext:" + sExtension + "||CallTime:" + jSon.calltime);
                                                bWorker.ReportProgress(0, stringBuilder.ToString());

                                                if (drrLog[0]["RecordName"].ToString().Length > 0 && sPreAnswerRecordingPath.Length > 0)
                                                    SPhone.StartRecording(sPreAnswerRecordingPath + "\\" + drrLog[0]["RecordName"]);
                                            }
                                        }
                                        else if (jSon.evt == "ANSWERED")
                                        {
                                            DataRow[] drrLog = dtLog.Select("UniqueID = '" + jSon.uniqueid + "'");
                                            if (drrLog.Length > 0)
                                            {
                                                drrLog[0]["AnswerTime"] = jSon.answer_time;
                                                drrLog[0]["CallerID"] = jSon.callerid;

                                                if (drrLog[0]["RecordName"].ToString().Length > 0 && sPreAnswerRecordingPath.Length > 0)
                                                    SPhone.StopRecording();

                                                if (drrLog[0]["RecordName"].ToString().Length > 0 && sAnsweredRecordingPath.Length > 0)
                                                    SPhone.StartRecording(sAnsweredRecordingPath + "\\" + drrLog[0]["RecordName"]);

                                                //bWorker.ReportProgress(0, "Call Answered:" + drrLog[0]["Telephone"] + "||ID:" + jSon.uniqueid + "||Ext:" + sExtension + "||AnswerTime:" + jSon.answer_time);
                                                bWorker.ReportProgress(0, stringBuilder.ToString());
                                            }
                                        }
                                        else if (jSon.evt == "HANGUP")
                                        {
                                            DataRow[] drrLog = dtLog.Select("UniqueID = '" + jSon.uniqueid + "'");
                                            if (drrLog.Length > 0)
                                            {
                                                SPhone.StopRecording();
                                                int Speak = Convert.ToInt32(TimeSpan.Parse(jSon.talktime.ToString()).TotalSeconds);
                                                int Total = Convert.ToInt32(TimeSpan.Parse(jSon.duration.ToString()).TotalSeconds);
                                                if(drrLog[0]["AnswerTime"].ToString().Length > 0)                                                
                                                    ExecuteQuery("UPDATE Call_Timesheet..Call_Log SET AnswerTime ='" + drrLog[0]["AnswerTime"] + "', CallerID = '"+ drrLog[0]["CallerID"] + "', HangUpTime = '" + jSon.endtime + "',SpeakDuration = '" + Speak + "', TotalCallDuration = '" + Total + "', PreAnswerDuration = '" + (Total - Speak) + "', CallStatus = '" + jSon.status + "' WHERE CallID ='" + jSon.uniqueid + "';", sConString);
                                                else
                                                    ExecuteQuery("UPDATE Call_Timesheet..Call_Log SET CallerID = '" + drrLog[0]["CallerID"] + "', HangUpTime = '" + jSon.endtime + "',SpeakDuration = '" + Speak + "', TotalCallDuration = '" + Total + "', PreAnswerDuration = '" + (Total - Speak) + "', CallStatus = '" + jSon.status + "' WHERE CallID ='" + jSon.uniqueid + "';", sConString);

                                                if (SupportVox)
                                                    ExecuteQuery("INSERT INTO Call_Timesheet..DialerFile_Imports (Agent_Ext, UserName, Phone_No, Call_Date, Call_Duration, TrackNo, WaveFile, File_name, Import_Date, ServerRegion) VALUES ('" + sExtension + "', '" + drrLog[0]["EmployeeID"] + "', '" + drrLog[0]["Telephone"] + "', GETDATE(), '" + Total + "', '" + jSon.uniqueid + "', '" + jSon.uniqueid + ".mp3', '" + jSon.uniqueid + "', GETDATE(), 'Vortex');", sConString);

                                                if (SupportiDialer)
                                                    ExecuteQuery("INSERT INTO Call_Timesheet..AspectDialerLogger (AgentName, LoginID, StationID, TelephoneNumber, RecordingID, Duration, DateTimeStamp, ProjectName, CampaignID, Company_ID) VALUES ('" + drrLog[0]["EmployeeID"] + "','" + drrLog[0]["EmployeeID"] + "','" + sExtension + "','" + drrLog[0]["Telephone"] + "','1','" + jSon.duration.ToString() + "',GETDATE(),'" + drrLog[0]["ProjectID"] + "','" + drrLog[0]["TeamName"] + "','" + drrLog[0]["ReferenceID"] + "');", sConString);
                                                
                                                //bWorker.ReportProgress(0, "Call HangUp:" + drrLog[0]["Telephone"] + "||ID:" + jSon.uniqueid + "||CallEndTime:" + jSon.endtime + "||TalkTime:" + jSon.talktime + "||TotalDuration:" + jSon.duration + "||CallStatus:" + jSon.status);
                                                bWorker.ReportProgress(0, stringBuilder.ToString());
                                            }
                                        }
                                    }
                                    //bWorker.ReportProgress(0, stringBuilder.ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        _bIsConnected = false;
                        throw new Exception("Dialer connection lost.");
                    }
                }

                _bIsConnected = false;
            }
            catch (Exception ex)
            {
                _bIsConnected = false;
                throw (ex);
            }
        }
        private void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            socket.Disconnect(false);
        }

        private void bAuth_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_IsHardPhoneAutheticated)
                this.OnDialerEventRecieved("Hardphone Authentication sucess");
            else
                this.OnDialerEventRecieved("Hardphone Authentication failed");
        }


        private void bWorker_Progress(object sender, ProgressChangedEventArgs e)
        {
            this.OnDialerEventRecieved(e.UserState.ToString());
        }

        protected void OnDialerEventRecieved(string message)
        {            
            if (this.DialerEventRecieved == null)
                return;
            this.DialerEventRecieved((object)this, new DialerEventArgs()
            {
                Data = message
            });
        }


        protected void OnPhoneEventRecieved(string message, int iLineID)
        {
            if (this.PhoneEventRecieved == null)
                return;
            this.PhoneEventRecieved((object)this, new PhoneEventArgs()
            {
                Data = message,
                LindID = iLineID
            });
        }


        public delegate void DialerEventHandler(object sender, DialerEventArgs e);
        public class DialerEventArgs : EventArgs
        {
            public string Data { get; set; }
        }


        public delegate void PhoneEventHandler(object sender, PhoneEventArgs e);
        public class PhoneEventArgs : EventArgs
        {
            public string Data { get; set; }
            public int LindID { get; set; }
        }
    }
}
