using System;
using System.Runtime.InteropServices;

namespace SIPVoipSDK
{
	[Flags]
	public enum ToneType : int
	{
		eToneDtmf = 1, eToneBaudot = 2, eToneSIT = 4, eToneMF = 8, eToneEnergy = 16
	}

	#region Event Handler Types
	public delegate void _IAbtoPhoneEvents_OnInitializedEventHandler([MarshalAs(UnmanagedType.BStr)]string Msg);
	public delegate void _IAbtoPhoneEvents_OnLineSwichedEventHandler(int lineId);
	public delegate void _IAbtoPhoneEvents_OnEstablishedCallEventHandler([MarshalAs(UnmanagedType.BStr)]string adress, int lineId);
	public delegate void _IAbtoPhoneEvents_OnIncomingCallEventHandler([MarshalAs(UnmanagedType.BStr)]string adress, int lineId);
	public delegate void _IAbtoPhoneEvents_OnClearedCallEventHandler([MarshalAs(UnmanagedType.BStr)]string Msg, int status, int lineId);
	public delegate void _IAbtoPhoneEvents_OnVolumeUpdatedEventHandler(int IsMicrophone, int level);
	public delegate void _IAbtoPhoneEvents_OnRegisteredEventHandler([MarshalAs(UnmanagedType.BStr)]string Msg);
	public delegate void _IAbtoPhoneEvents_OnUnRegisteredEventHandler([MarshalAs(UnmanagedType.BStr)]string Msg);
	public delegate void _IAbtoPhoneEvents_OnPlayFinishedEventHandler([MarshalAs(UnmanagedType.BStr)]string Msg);
	public delegate void _IAbtoPhoneEvents_OnEstablishedConnectionEventHandler([MarshalAs(UnmanagedType.BStr)]string addrFrom, [MarshalAs(UnmanagedType.BStr)]string addrTo, int connectionId, int lineId);
	public delegate void _IAbtoPhoneEvents_OnClearedConnectionEventHandler(int connectionId, int lineId);
	public delegate void _IAbtoPhoneEvents_OnToneReceivedEventHandler(int tone, int connectionId, int lineId);
	public delegate void _IAbtoPhoneEvents_OnTextMessageReceivedEventHandler([MarshalAs(UnmanagedType.BStr)]string from, [MarshalAs(UnmanagedType.BStr)]string message);
	public delegate void _IAbtoPhoneEvents_OnPhoneNotifyEventHandler([MarshalAs(UnmanagedType.BStr)]string message);
	public delegate void _IAbtoPhoneEvents_OnRemoteAlertingEventHandler(int connectionId, int responseCode, [MarshalAs(UnmanagedType.BStr)]string reasonMsg);
	public delegate void _IAbtoPhoneEvents_OnHoldCallEventHandler(int LineId, int isHeld);
	public delegate void _IAbtoPhoneEvents_OnTextMessageSentStatusEventHandler([MarshalAs(UnmanagedType.BStr)]string address, [MarshalAs(UnmanagedType.BStr)]string reason, int bSuccess);
	public delegate void _IAbtoPhoneEvents_OnRecordFinishedEventHandler([MarshalAs(UnmanagedType.BStr)]string message);
	public delegate void _IAbtoPhoneEvents_OnSubscribeStatusEventHandler(int subscriptionId, int statusCode, [MarshalAs(UnmanagedType.BStr)]string statusMsg);
	public delegate void _IAbtoPhoneEvents_OnUnSubscribeStatusEventHandler(int subscriptionId, int statusCode, [MarshalAs(UnmanagedType.BStr)]string statusMsg);
	public delegate void _IAbtoPhoneEvents_OnSubscriptionRequestEventHandler([MarshalAs(UnmanagedType.BStr)]string fromUri, [MarshalAs(UnmanagedType.BStr)]string eventStr);
	public delegate void _IAbtoPhoneEvents_OnSubscriptionNotifyEventHandler(int subscriptionId, [MarshalAs(UnmanagedType.BStr)]string StateStr, [MarshalAs(UnmanagedType.BStr)]string NotifyStr);
	public delegate void _IAbtoPhoneEvents_OnDetectedAnswerTimeEventHandler(int TimeSpanMs, int ConnectionId);
	public delegate void _IAbtoPhoneEvents_OnBaudotToneReceivedEventHandler(int Tone, int ConnectionId, int LineId);
	public delegate void _IAbtoPhoneEvents_OnSubscriptionTerminatedEventHandler([MarshalAs(UnmanagedType.BStr)]string fromUri);
	public delegate void _IAbtoPhoneEvents_OnToneDetectedEventHandler(ToneType type, [MarshalAs(UnmanagedType.BStr)]string ToneStr, int ConnectionId, int LineId);
	public delegate void _IAbtoPhoneEvents_OnReceivedSipNotifyMsgEventHandler([MarshalAs(UnmanagedType.BStr)]string SipNotifyMsgStr);
	public delegate void _IAbtoPhoneEvents_OnPlayFinished2EventHandler([MarshalAs(UnmanagedType.BStr)]string Msg, int LineId);
	public delegate void _IAbtoPhoneEvents_OnRemoteAlerting2EventHandler(int ConnectionId, int LineId, int responseCode, [MarshalAs(UnmanagedType.BStr)]string reasonMsg);
	public delegate void _IAbtoPhoneEvents_OnReceivedRequestInfoEventHandler(int ConnectionId, int LineId, [MarshalAs(UnmanagedType.BStr)]string contentType, [MarshalAs(UnmanagedType.BStr)]string body);
	public delegate void _IAbtoPhoneEvents_OnBaudotFinishedEventHandler([MarshalAs(UnmanagedType.BStr)]string Msg, int LineId);


		
	#endregion // Event Handler Types




	public class CConfig
	{
		#region Constructors
		IntPtr m_cfgPtr;

        bool Is64Bit = false;

		public CConfig(IntPtr cfgPtr)
		{
			m_cfgPtr = cfgPtr;
            Is64Bit =  System.Environment.Is64BitOperatingSystem;
		}

        #endregion // Constructors
        #region Imports       

#if Is64Bit
        public const string ImportedDllName =  @"SoftPhone\SIPVoipSDK64.dll";
#else
        public const string ImportedDllName = @"SoftPhone\SIPVoIPSDK.dll";
#endif

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_StunServer(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string StunAddress);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_StunServer(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string StunAddress);

		public string StunServer
		{
			get { string s = ""; cfg_get_StunServer(m_cfgPtr, ref s); return s; }
			set { cfg_put_StunServer(m_cfgPtr, value); }
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_ListenPort(IntPtr cfgPtr, int Port);
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_ListenPort(IntPtr cfgPtr, ref int Port);
		public int ListenPort
		{
			get { int s = 0; cfg_get_ListenPort(m_cfgPtr, ref s); return s; }
			set { cfg_put_ListenPort(m_cfgPtr, value); }
		}
	

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_RtpStartPort(IntPtr cfgPtr, int Port);					
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_RtpStartPort(IntPtr cfgPtr, ref int Port);

		public int RtpStartPort
		{
			get { int s = 0; cfg_get_RtpStartPort(m_cfgPtr, ref s); return s; }
			set { cfg_put_RtpStartPort(m_cfgPtr, value); }
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_ActivePlaybackDevice(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string DeviceName);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_ActivePlaybackDevice(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string DeviceName);
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_PlaybackDeviceCount(IntPtr cfgPtr, ref int pCount);
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_PlaybackDevice(IntPtr cfgPtr, int Index, [MarshalAs(UnmanagedType.BStr)]ref string  pVal);

		public string ActivePlaybackDevice
		{
			get { string s = ""; cfg_get_ActivePlaybackDevice(m_cfgPtr, ref s); return s; }
			set { cfg_put_ActivePlaybackDevice(m_cfgPtr, value); }
		}

		public int PlaybackDeviceCount
		{
			get { int s = 0; cfg_get_PlaybackDeviceCount(m_cfgPtr, ref s); return s; }
		}

		public string get_PlaybackDevice(int Index)
		{
			string s = ""; cfg_get_PlaybackDevice(m_cfgPtr, Index, ref s); return s;
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_ActiveRecordDevice(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string DeviceName);			
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_ActiveRecordDevice(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  DeviceName);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_RecordDeviceCount(IntPtr cfgPtr, ref int pCount);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_RecordDevice(IntPtr cfgPtr, int Index, [MarshalAs(UnmanagedType.BStr)]ref string  pVal);

		public string ActiveRecordDevice
		{
			get { string s = ""; cfg_get_ActiveRecordDevice(m_cfgPtr, ref s); return s; }
			set { cfg_put_ActiveRecordDevice(m_cfgPtr, value); }
		}

		public int RecordDeviceCount
		{
			get { int s = 0; cfg_get_RecordDeviceCount(m_cfgPtr, ref s); return s; }
		}

		public string get_RecordDevice(int Index)
		{
			string s = ""; cfg_get_RecordDevice(m_cfgPtr, Index, ref s); return s;
		}


		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_ActiveNetworkInterface(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string val);			
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_ActiveNetworkInterface(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  pVal);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_NetworkInterfaceCount(IntPtr cfgPtr, ref int pCount);			
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_NetworkInterface(IntPtr cfgPtr, int Index, [MarshalAs(UnmanagedType.BStr)]ref string  pVal);

		public string ActiveNetworkInterface
		{
			get { string s = ""; cfg_get_ActiveNetworkInterface(m_cfgPtr, ref s); return s; }
			set { cfg_put_ActiveNetworkInterface(m_cfgPtr, value); }
		}

		public int NetworkInterfaceCount
		{
			get { int s = 0; cfg_get_NetworkInterfaceCount(m_cfgPtr, ref s); return s; }
		}

		public string get_NetworkInterface(int Index)
		{
			string s = ""; cfg_get_NetworkInterface(m_cfgPtr, Index, ref s); return s;
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_ActiveVideoDevice(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string DeviceName);			
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_ActiveVideoDevice(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  DeviceName);			
        public string ActiveVideoDevice
		{
			get { string s = ""; cfg_get_ActiveVideoDevice(m_cfgPtr, ref s); return s; }
			set { cfg_put_ActiveVideoDevice(m_cfgPtr, value); }
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_VideoDeviceCount(IntPtr cfgPtr, ref int pCount);				
        public int VideoDeviceCount
		{
			get { int s = 0; cfg_get_VideoDeviceCount(m_cfgPtr, ref s); return s; }
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_VideoDevice(IntPtr cfgPtr, int Index, [MarshalAs(UnmanagedType.BStr)]ref string  pVal);
		public string get_VideoDevice(int Index)
		{
			string s = ""; cfg_get_VideoDevice(m_cfgPtr, Index, ref s); return s;
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_MinJitterDelay(IntPtr cfgPtr, int Delay);					
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_MinJitterDelay(IntPtr cfgPtr, ref int Delay);					
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_MaxJitterDelay(IntPtr cfgPtr, int Delay);					
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_MaxJitterDelay(IntPtr cfgPtr, ref int Delay);					

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_SoundBufferDepth(IntPtr cfgPtr, int Depth);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_SoundBufferDepth(IntPtr cfgPtr, ref int Depth);				

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_SamplesPerSecond(IntPtr cfgPtr, int Smpls);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_SamplesPerSecond(IntPtr cfgPtr, ref int Smpls);
		public int SamplesPerSecon
		{
			get { int s = 0; cfg_get_SamplesPerSecond(m_cfgPtr, ref s); return s; }
			set { cfg_put_SamplesPerSecond(m_cfgPtr, value); }
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_DialToneEnabled(IntPtr cfgPtr, int Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_DialToneEnabled(IntPtr cfgPtr, ref int Enabled);
		public int DialToneEnabled
		{
			get { int s = 0; cfg_get_DialToneEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_DialToneEnabled(m_cfgPtr, value); }
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_RingToneEnabled(IntPtr cfgPtr, int Enabled);
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_RingToneEnabled(IntPtr cfgPtr, ref int Enabled);
		public int RingToneEnabled
		{
			get { int s = 0; cfg_get_RingToneEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_RingToneEnabled(m_cfgPtr, value); }
		}

    

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_put_SendRingingMsgEnabled(IntPtr cfgPtr, int Enabled);
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_get_SendRingingMsgEnabled(IntPtr cfgPtr, ref int Enabled);
        public int SendRingingMsgEnabled
		{
            get { int s = 0; cfg_get_SendRingingMsgEnabled(m_cfgPtr, ref s); return s; }
            set { cfg_put_SendRingingMsgEnabled(m_cfgPtr, value); }
		}

                
        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_put_RingToneFile(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string file);
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_get_RingToneFile(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string file);
        public string RingToneFile
		{
            get { string s=""; cfg_get_RingToneFile(m_cfgPtr, ref s); return s; }
            set { cfg_put_RingToneFile(m_cfgPtr, value); }
		}


		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_VolumeUpdateSubscribed(IntPtr cfgPtr, int Enabled);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_VolumeUpdateSubscribed(IntPtr cfgPtr, ref int Enabled);
		public int VolumeUpdateSubscribed
		{
			get { int s = 0; cfg_get_VolumeUpdateSubscribed(m_cfgPtr, ref s); return s; }
			set { cfg_put_VolumeUpdateSubscribed(m_cfgPtr, value); }
		}	

		[Flags]
		public enum LogLevelType : int
		{
			eLogNone = -1, eLogCritical = 2, eLogError = 3, eLogWarning = 4, eLogInfo = 6, eLogDebug = 7
		}
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_LogLevel(IntPtr cfgPtr, LogLevelType level);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_LogLevel(IntPtr cfgPtr, ref LogLevelType level);
		public LogLevelType LogLevel
		{
			get { LogLevelType s = 0; cfg_get_LogLevel(m_cfgPtr, ref s); return s; }
			set { cfg_put_LogLevel(m_cfgPtr, value); }
		}


		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_EchoCancelationEnabled(IntPtr cfgPtr, int Enabled);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_EchoCancelationEnabled(IntPtr cfgPtr, ref int Enabled);
		public int EchoCancelationEnabled
		{
			get { int s = 0; cfg_get_EchoCancelationEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_EchoCancelationEnabled(m_cfgPtr, value); }
		}	

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_SilenceDetectionEnabled(IntPtr cfgPtr, int Enabled);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_SilenceDetectionEnabled(IntPtr cfgPtr, ref int Enabled);
		public int SilenceDetectionEnabled
		{
			get { int s = 0; cfg_get_SilenceDetectionEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_SilenceDetectionEnabled(m_cfgPtr, value); }
		}


		[DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_NoiseReductionEnabled(IntPtr cfgPtr, int Enabled);
		[DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_NoiseReductionEnabled(IntPtr cfgPtr, ref int Enabled);
		public int NoiseReductionEnabled
		{
			get { int s = 0; cfg_get_NoiseReductionEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_NoiseReductionEnabled(m_cfgPtr, value); }
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_AutoGainControlEnabled(IntPtr cfgPtr, int Enabled);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_AutoGainControlEnabled(IntPtr cfgPtr, ref int Enabled);
		public int AutoGainControlEnabled
		{
			get { int s = 0; cfg_get_AutoGainControlEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_AutoGainControlEnabled(m_cfgPtr, value); }
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_UserAgent(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string AgentName);					
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_UserAgent(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  AgentName);

		public string UserAgent
		{
			get { string s = ""; cfg_get_UserAgent(m_cfgPtr, ref s); return s; }
			set { cfg_put_UserAgent(m_cfgPtr, value); }
		}
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_CallerId(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string CallerId);						
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_CallerId(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  CallerId);

		public string CallerId
		{
			get { string s = ""; cfg_get_CallerId(m_cfgPtr, ref s); return s; }
			set { cfg_put_CallerId(m_cfgPtr, value); }
		}

        

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_put_RegRealm(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string Domain);						
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_get_RegRealm(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string Domain);
        public string RegRealm
		{
            get { string s = ""; cfg_get_RegRealm(m_cfgPtr, ref s); return s; }
            set { cfg_put_RegRealm(m_cfgPtr, value); }
		}	


		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_RegDomain(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string Domain);						
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_RegDomain(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  Domain);

		public string RegDomain
		{
			get { string s = ""; cfg_get_RegDomain(m_cfgPtr, ref s); return s; }
			set { cfg_put_RegDomain(m_cfgPtr, value); }
		}	
	
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_RegUser(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string User);							
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_RegUser(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  User);

		public string RegUser
		{
			get { string s = ""; cfg_get_RegUser(m_cfgPtr, ref s); return s; }
			set { cfg_put_RegUser(m_cfgPtr, value); }
		}
		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_RegPass(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string Pass);							
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_RegPass(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  Pass);

		public string RegPass
		{
			get { string s = ""; cfg_get_RegPass(m_cfgPtr, ref s); return s; }
			set { cfg_put_RegPass(m_cfgPtr, value); }
		}
			
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_RegAuthId(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string AuthId);						
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_RegAuthId(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  AuthId);

		public string RegAuthId
		{
			get { string s = ""; cfg_get_RegAuthId(m_cfgPtr, ref s); return s; }
			set { cfg_put_RegAuthId(m_cfgPtr, value); }
		}
			
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_RegExpire(IntPtr cfgPtr, int Expire);						
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_RegExpire(IntPtr cfgPtr, ref int Expire);

		public int RegExpire
		{
			get { int s = 0; cfg_get_RegExpire(m_cfgPtr, ref s); return s; }
			set { cfg_put_RegExpire(m_cfgPtr, value); }
		}	


		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_ProxyDomain(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string Domain);					
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_ProxyDomain(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  Domain);

		public string ProxyDomain
		{
			get { string s = ""; cfg_get_ProxyDomain(m_cfgPtr, ref s); return s; }
			set { cfg_put_ProxyDomain(m_cfgPtr, value); }
		}
		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_ProxyUser(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string User);						
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_ProxyUser(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  User);

		public string ProxyUser
		{
			get { string s = ""; cfg_get_ProxyUser(m_cfgPtr, ref s); return s; }
			set { cfg_put_ProxyUser(m_cfgPtr, value); }
		}
				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_ProxyPass(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string Pass);						
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_ProxyPass(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  Pass);
		public string ProxyPass
		{
			get { string s = ""; cfg_get_ProxyPass(m_cfgPtr, ref s); return s; }
			set { cfg_put_ProxyPass(m_cfgPtr, value); }
		}	



		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_LicenseUserId(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string UserId);					
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_LicenseUserId(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  UserId);
		public string LicenseUserId
		{
			get { string s = ""; cfg_get_LicenseUserId(m_cfgPtr, ref s); return s; }
			set { cfg_put_LicenseUserId(m_cfgPtr, value); }
		}	
			
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_LicenseKey(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string Key);						
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_LicenseKey(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  Key);
		public string LicenseKey
		{
			get { string s = ""; cfg_get_LicenseKey(m_cfgPtr, ref s); return s; }
			set { cfg_put_LicenseKey(m_cfgPtr, value); }
		}	
		
	

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_AdditionalDnsServer(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string Addr);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_AdditionalDnsServer(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  Addr);
		public string AdditionalDnsServer
		{
			get { string s = ""; cfg_get_AdditionalDnsServer(m_cfgPtr, ref s); return s; }
			set { cfg_put_AdditionalDnsServer(m_cfgPtr, value); }
		}			
																
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_AdditionalSDPContent(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string Addr);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_AdditionalSDPContent(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string  Addr);
		public string AdditionalSDPContent
		{
			get { string s = ""; cfg_get_AdditionalSDPContent(m_cfgPtr, ref s); return s; }
			set { cfg_put_AdditionalSDPContent(m_cfgPtr, value); }
		}	
																
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_AutoAnswerEnabled(IntPtr cfgPtr, int Enabled);
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_AutoAnswerEnabled(IntPtr cfgPtr, ref int Enabled);
		public int AutoAnswerEnabled
		{
			get { int s = 0; cfg_get_AutoAnswerEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_AutoAnswerEnabled(m_cfgPtr, value); }
		}	
																
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_MP3RecordingEnabled(IntPtr cfgPtr, int Enabled);			
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_MP3RecordingEnabled(IntPtr cfgPtr, ref int Enabled);
		public int MP3RecordingEnabled
		{
			get { int s = 0; cfg_get_MP3RecordingEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_MP3RecordingEnabled(m_cfgPtr, value); }
		}


		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_EncryptedCallEnabled(IntPtr cfgPtr, int Enabled);			
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_EncryptedCallEnabled(IntPtr cfgPtr, ref int Enabled);
		public int EncryptedCallEnabled
		{
			get { int s = 0; cfg_get_EncryptedCallEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_EncryptedCallEnabled(m_cfgPtr, value); }
		}	


		
																
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_LocalAudioEnabled(IntPtr cfgPtr, int Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_LocalAudioEnabled(IntPtr cfgPtr, ref int Enabled);
		public int LocalAudioEnabled
		{
			get { int s = 0; cfg_get_LocalAudioEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_LocalAudioEnabled(m_cfgPtr, value); }
		}		

		
        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_put_LocalTonesEnabled(IntPtr cfgPtr, int Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_get_LocalTonesEnabled(IntPtr cfgPtr, ref int Enabled);
        public int LocalTonesEnabled
		{
            get { int s = 0; cfg_get_LocalTonesEnabled(m_cfgPtr, ref s); return s; }
            set { cfg_put_LocalTonesEnabled(m_cfgPtr, value); }
		}			


        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_put_CallInviteTimeout(IntPtr cfgPtr, int seconds);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_get_CallInviteTimeout(IntPtr cfgPtr, ref int seconds);
        public int CallInviteTimeout
		{
            get { int s = 0; cfg_get_CallInviteTimeout(m_cfgPtr, ref s); return s; }
            set { cfg_put_CallInviteTimeout(m_cfgPtr, value); }
		}		
        
										
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_VideoCallEnabled(IntPtr cfgPtr, int Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_VideoCallEnabled(IntPtr cfgPtr, ref int Enabled);
		public int VideoCallEnabled
		{
			get { int s = 0; cfg_get_VideoCallEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_VideoCallEnabled(m_cfgPtr, value); }
		}
																
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_LocalVideoWindow(IntPtr cfgPtr, IntPtr hVideoWnd);			
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_LocalVideoWindow(IntPtr cfgPtr, ref IntPtr hVideoWnd);
		public IntPtr LocalVideoWindow
		{
			get { IntPtr s = IntPtr.Zero; cfg_get_LocalVideoWindow(m_cfgPtr, ref s); return s; }
			set { cfg_put_LocalVideoWindow(m_cfgPtr, value); }
		}	
																
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_RemoteVideoWindow(IntPtr cfgPtr, IntPtr hVideoWnd);			
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_RemoteVideoWindow(IntPtr cfgPtr, ref IntPtr hVideoWnd);
		public IntPtr RemoteVideoWindow
		{
			get { IntPtr s = IntPtr.Zero; cfg_get_RemoteVideoWindow(m_cfgPtr, ref s); return s; }
			set { cfg_put_RemoteVideoWindow(m_cfgPtr, value); }
		}	

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_CodecCount(IntPtr cfgPtr, ref int pCount);
		public int CodecCount
		{
			get { int s = 0; cfg_get_CodecCount(m_cfgPtr, ref s); return s; }
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_CodecName(IntPtr cfgPtr, int Index, [MarshalAs(UnmanagedType.BStr)]ref string  Name);
		public string get_CodecName(int Index)
		{
			string s = ""; cfg_get_CodecName(m_cfgPtr, Index, ref s); return s;
		}
	
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_CodecSelected(IntPtr cfgPtr, int Index, ref int Selected);
		public int get_CodecSelected(int Index)
		{
			int s = 0; cfg_get_CodecSelected(m_cfgPtr, Index, ref s); return s;
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_SetCodecOrder(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string CodecksListAsStr, int sc);
		public void SetCodecOrder(string CodecksListAsStr, int sc)
		{
			cfg_SetCodecOrder(m_cfgPtr, CodecksListAsStr, sc);
		}







        [DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_put_KeepAliveTimeRTP(IntPtr cfgPtr, int TimeSeconds);
        [DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_get_KeepAliveTimeRTP(IntPtr cfgPtr, ref int TimeSeconds);
        public int KeepAliveTimeRTP
        {
            get { int s = 0; cfg_get_KeepAliveTimeRTP(m_cfgPtr, ref s); return s; }
            set { cfg_put_KeepAliveTimeRTP(m_cfgPtr, value); }
        }


        [DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_put_KeepAliveTimeSIP(IntPtr cfgPtr, int TimeSeconds);
        [DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_get_KeepAliveTimeSIP(IntPtr cfgPtr, ref int TimeSeconds);
        public int KeepAliveTimeSIP
        {
            get { int s = 0; cfg_get_KeepAliveTimeSIP(m_cfgPtr, ref s); return s; }
            set { cfg_put_KeepAliveTimeSIP(m_cfgPtr, value); }
        }


		[DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_put_VideoFrameWidth(IntPtr cfgPtr, int width);
        [DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_get_VideoFrameWidth(IntPtr cfgPtr, ref int width);
        public int VideoFrameWidth
        {
            get { int s = 0; cfg_get_VideoFrameWidth(m_cfgPtr, ref s); return s; }
            set { cfg_put_VideoFrameWidth(m_cfgPtr, value); }
        }


        [DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_put_VideoFrameHeight(IntPtr cfgPtr, int height);
        [DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void cfg_get_VideoFrameHeight(IntPtr cfgPtr, ref int height);
        public int VideoFrameHeight
        {
            get { int s = 0; cfg_get_VideoFrameHeight(m_cfgPtr, ref s); return s; }
            set { cfg_put_VideoFrameHeight(m_cfgPtr, value); }
        }



		[DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_ComfortNoiseOnMutedMicEnabled(IntPtr cfgPtr, int Enabled);
		[DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_ComfortNoiseOnMutedMicEnabled(IntPtr cfgPtr, ref int Enabled);
		public int ComfortNoiseOnMutedMicEnabled
		{
			get { int s = 0; cfg_get_ComfortNoiseOnMutedMicEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_ComfortNoiseOnMutedMicEnabled(m_cfgPtr, value); }
		}


		[DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_LogPath(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string path);
		[DllImport(ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_LogPath(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string path);
		public string LogPath
		{
			get { string s=""; cfg_get_LogPath(m_cfgPtr, ref s); return s; }
			set { cfg_put_LogPath(m_cfgPtr, value); }
		}
        


		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_Load(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string fileName, ref int Result);
		public int Load(string fileName)
		{
			int Result = 0;
			cfg_Load(m_cfgPtr, fileName, ref Result);
			return Result;
		}
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_Store(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string fileName);
		public void Store(string fileName)
		{
			cfg_Store(m_cfgPtr, fileName);
		}
		#endregion // Imports


        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_MixerFilePlayerEnabled(IntPtr cfgPtr, int Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_MixerFilePlayerEnabled(IntPtr cfgPtr, ref int Enabled);
		public int MixerFilePlayerEnabled
		{
			get { int s = 0; cfg_get_MixerFilePlayerEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_MixerFilePlayerEnabled(m_cfgPtr, value); }
		}
        
        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_ICEEnabled(IntPtr cfgPtr, int Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_ICEEnabled(IntPtr cfgPtr, ref int Enabled);
		public int ICEEnabled
		{
			get { int s = 0; cfg_get_ICEEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_ICEEnabled(m_cfgPtr, value); }
		}

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_DtmfAsSipInfoEnabled(IntPtr cfgPtr, int Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_DtmfAsSipInfoEnabled(IntPtr cfgPtr, ref int Enabled);
		public int DtmfAsSipInfoEnabled
		{
			get { int s = 0; cfg_get_DtmfAsSipInfoEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_DtmfAsSipInfoEnabled(m_cfgPtr, value); }
		}

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_SDPInRingingMsgEnabled(IntPtr cfgPtr, int Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_SDPInRingingMsgEnabled(IntPtr cfgPtr, ref int Enabled);
		public int SDPInRingingMsgEnabled
		{
			get { int s = 0; cfg_get_SDPInRingingMsgEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_SDPInRingingMsgEnabled(m_cfgPtr, value); }
		}

        [Flags]
		public enum SignallingTransportType : int
		{
			eTransportUDP = 1, eTransportTCP = 2, eTransportTLSv1 = 4, eTransportSSLv23 = 8
		}
        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_SignallingTransport(IntPtr cfgPtr, SignallingTransportType transport);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_SignallingTransport(IntPtr cfgPtr, ref SignallingTransportType transport);
		public SignallingTransportType SignallingTransport
		{
			get { SignallingTransportType s = 0; cfg_get_SignallingTransport(m_cfgPtr, ref s); return s; }
			set { cfg_put_SignallingTransport(m_cfgPtr, value); }
		}


        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_LoopbackNetworkInterfaceEnabled(IntPtr cfgPtr, int Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_LoopbackNetworkInterfaceEnabled(IntPtr cfgPtr, ref int Enabled);
		public int LoopbackNetworkInterfaceEnabled
		{
			get { int s = 0; cfg_get_LoopbackNetworkInterfaceEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_LoopbackNetworkInterfaceEnabled(m_cfgPtr, value); }
		}
	



        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_ExSipAccount_Count(IntPtr cfgPtr, ref int pVal);
		public int ExSipAccount_Count()
		{
			int val = 0; cfg_ExSipAccount_Count(m_cfgPtr, ref val); return val;
		}


        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_ExSipAccount_Get(IntPtr cfgPtr, int ExAccountIdx, [MarshalAs(UnmanagedType.BStr)]ref string Domain, [MarshalAs(UnmanagedType.BStr)]ref string User, 
                                                        [MarshalAs(UnmanagedType.BStr)]ref string Pass, [MarshalAs(UnmanagedType.BStr)]ref string AuthId, [MarshalAs(UnmanagedType.BStr)]ref string DisplName, ref int Expire, ref int bEnable);
		public void ExSipAccount_Get(int ExAccountIdx, ref string Domain, ref string User, 
                                      ref string Pass, ref string AuthId, ref string DisplName, ref int Expire, ref int bEnable)
		{
			cfg_ExSipAccount_Get(m_cfgPtr, ExAccountIdx, ref Domain, ref User, ref Pass, ref AuthId, ref DisplName, ref Expire, ref bEnable);
		}


         [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_ExSipAccount_Set(IntPtr cfgPtr, int ExAccountIdx, [MarshalAs(UnmanagedType.BStr)]string Domain, [MarshalAs(UnmanagedType.BStr)]string User, 
                                                        [MarshalAs(UnmanagedType.BStr)]string Pass, [MarshalAs(UnmanagedType.BStr)]string AuthId, [MarshalAs(UnmanagedType.BStr)]string DisplName, int Expire, int bEnable);
		public void ExSipAccount_Set(int ExAccountIdx, string Domain, string User, 
                                    string Pass, string AuthId, string DisplName, int Expire, int bEnable)
		{
			cfg_ExSipAccount_Set(m_cfgPtr, ExAccountIdx, Domain, User, Pass, AuthId, DisplName, Expire, bEnable);
		}


        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_ExSipAccount_Add(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string Domain, [MarshalAs(UnmanagedType.BStr)]string User, 
                                                        [MarshalAs(UnmanagedType.BStr)]string Pass, [MarshalAs(UnmanagedType.BStr)]string AuthId, [MarshalAs(UnmanagedType.BStr)]string DisplName, 
                                                        int Expire, int bEnable, int bDefault);
		public void ExSipAccount_Add(string Domain, string User, 
                                     string Pass, string AuthId, string DisplName, 
                                     int Expire, int bEnable, int bDefault)
		{
			cfg_ExSipAccount_Add(m_cfgPtr, Domain, User, Pass, AuthId, DisplName, Expire, bEnable, bDefault);
		}


        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_ExSipAccount_ResetAll(IntPtr cfgPtr);
		public void ExSipAccount_ResetAll()
		{
			cfg_ExSipAccount_ResetAll(m_cfgPtr);
		}
	
        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_ExSipAccount_GetDefaultIdx(IntPtr cfgPtr, ref int ExAccountIdx);
		public int ExSipAccount_GetDefaultIdx()
		{
            int val=0; cfg_ExSipAccount_GetDefaultIdx(m_cfgPtr, ref val); return val;			
		}
	
        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_ExSipAccount_SetDefaultIdx(IntPtr cfgPtr, int ExAccountIdx);
		public void ExSipAccount_GetDefaultIdx(int ExAccountIdx)
		{
            cfg_ExSipAccount_SetDefaultIdx(m_cfgPtr, ExAccountIdx);
		}
	
        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_ExSipAccountStr(IntPtr cfgPtr, int ExAccountIdx, [MarshalAs(UnmanagedType.BStr)]ref string AccountAsStr);
		public string get_ExSipAccountStr(int ExAccountIdx)
		{
            string val=""; cfg_get_ExSipAccountStr(m_cfgPtr, ExAccountIdx, ref val); return val;
		}
	
	
        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_FixedTransportInterfaceEnabled(IntPtr cfgPtr, int Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_FixedTransportInterfaceEnabled(IntPtr cfgPtr, ref int Enabled);
		public int FixedTransportInterfaceEnabled
		{
			get { int s = 0; cfg_get_FixedTransportInterfaceEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_FixedTransportInterfaceEnabled(m_cfgPtr, value); }
		}


        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_LoadFromStr(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string str, ref int result);
		public int LoadFromStr(string str)
		{
            int val=0; cfg_LoadFromStr(m_cfgPtr, str, ref val); return val;		
		}

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_StoreAsStr(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string str);
        public string StoreAsStr()
		{
            string val=""; cfg_StoreAsStr(m_cfgPtr, ref val); return val;
		}

       
        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_ProvisionalMode(IntPtr cfgPtr, int mode);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_ProvisionalMode(IntPtr cfgPtr, ref int mode);
		public int ProvisionalMode
		{
			get { int s = 0; cfg_get_ProvisionalMode(m_cfgPtr, ref s); return s; }
			set { cfg_put_ProvisionalMode(m_cfgPtr, value); }
		}	

     
        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_TonesTypesToDetect(IntPtr cfgPtr, int types);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_TonesTypesToDetect(IntPtr cfgPtr, ref int types);
		public int TonesTypesToDetect
		{
			get { int s = 0; cfg_get_TonesTypesToDetect(m_cfgPtr, ref s); return s; }
			set { cfg_put_TonesTypesToDetect(m_cfgPtr, value); }
		}

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_AudioQosDscpVal(IntPtr cfgPtr, int val);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_AudioQosDscpVal(IntPtr cfgPtr, ref int val);
		public int AudioQosDscpVal
		{
			get { int s = 0; cfg_get_AudioQosDscpVal(m_cfgPtr, ref s); return s; }
			set { cfg_put_AudioQosDscpVal(m_cfgPtr, value); }
		}	

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_VideoQosDscpVal(IntPtr cfgPtr, int val);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_VideoQosDscpVal(IntPtr cfgPtr, ref int val);
		public int VideoQosDscpVal
		{
			get { int s = 0; cfg_get_VideoQosDscpVal(m_cfgPtr, ref s); return s; }
			set { cfg_put_VideoQosDscpVal(m_cfgPtr, value); }
		}	


        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_AudioSSRCVal(IntPtr cfgPtr, int val);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_AudioSSRCVal(IntPtr cfgPtr, ref int val);
		public int AudioSSRCVal
		{
			get { int s = 0; cfg_get_AudioSSRCVal(m_cfgPtr, ref s); return s; }
			set { cfg_put_AudioSSRCVal(m_cfgPtr, value); }
		}	

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_VideoSSRCVal(IntPtr cfgPtr, int val);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_VideoSSRCVal(IntPtr cfgPtr, ref int val);
		public int VideoSSRCVal
		{
			get { int s = 0; cfg_get_VideoSSRCVal(m_cfgPtr, ref s); return s; }
			set { cfg_put_VideoSSRCVal(m_cfgPtr, value); }
		}	


        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_TonesBaudRate(IntPtr cfgPtr, double rate);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_TonesBaudRate(IntPtr cfgPtr, ref double rate);
		public double TonesBaudRate
		{
			get { double s = 0; cfg_get_TonesBaudRate(m_cfgPtr, ref s); return s; }
			set { cfg_put_TonesBaudRate(m_cfgPtr, value); }
		}	

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_OverrideRtpDest(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string dest);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_OverrideRtpDest(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string dest);
		public string OverrideRtpDest
		{
			get { string s = ""; cfg_get_OverrideRtpDest(m_cfgPtr, ref s); return s; }
			set { cfg_put_OverrideRtpDest(m_cfgPtr, value); }
		}	


        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_RemoteVideoWindow_Get(IntPtr cfgPtr, int wndIdx, ref IntPtr hVideoWnd);
		public void RemoteVideoWindow_Get(int wndIdx, ref IntPtr hVideoWnd)
		{
            cfg_RemoteVideoWindow_Get(m_cfgPtr, wndIdx, ref hVideoWnd);
		}

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_RemoteVideoWindow_Set(IntPtr cfgPtr, int wndIdx, IntPtr hVideoWnd);
		public void RemoteVideoWindow_Set(int wndIdx, IntPtr hVideoWnd)
		{
            cfg_RemoteVideoWindow_Set(m_cfgPtr, wndIdx, hVideoWnd);
		}

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_RemoteVideoWindow_Add(IntPtr cfgPtr, IntPtr hVideoWnd);
		public void RemoteVideoWindow_Add(IntPtr hVideoWnd)
		{
            cfg_RemoteVideoWindow_Add(m_cfgPtr, hVideoWnd);
		}

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_RemoteVideoWindow_ResetAll(IntPtr cfgPtr);
		public void RemoteVideoWindow_ResetAll()
		{
            cfg_RemoteVideoWindow_ResetAll(m_cfgPtr);
		}

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_RemoteVideoWindow_Count(IntPtr cfgPtr, ref int count);
		public void RemoteVideoWindow_Count(ref int count)
		{
            cfg_RemoteVideoWindow_Count(m_cfgPtr, ref count);
		}

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_AECDelayMs(IntPtr cfgPtr, int ms);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_AECDelayMs(IntPtr cfgPtr, ref int ms);
		public int AECDelayMs
		{
			get { int s = 0; cfg_get_AECDelayMs(m_cfgPtr, ref s); return s; }
			set { cfg_put_AECDelayMs(m_cfgPtr, value); }
		}	

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_DmpCreateEnabled(IntPtr cfgPtr, int Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_DmpCreateEnabled(IntPtr cfgPtr, ref int Enabled);
		public int DmpCreateEnabled
		{
			get { int s = 0; cfg_get_DmpCreateEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_DmpCreateEnabled(m_cfgPtr, value); }
		}

	    [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_VideoAutoSendEnabled(IntPtr cfgPtr, int Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_VideoAutoSendEnabled(IntPtr cfgPtr, ref int Enabled);
		public int VideoAutoSendEnabled
		{
			get { int s = 0; cfg_get_VideoAutoSendEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_VideoAutoSendEnabled(m_cfgPtr, value); }
		}

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_ProxyForceForAllRequests(IntPtr cfgPtr, int Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_ProxyForceForAllRequests(IntPtr cfgPtr, ref int Enabled);
		public int ProxyForceForAllRequests
		{
			get { int s = 0; cfg_get_ProxyForceForAllRequests(m_cfgPtr, ref s); return s; }
			set { cfg_put_ProxyForceForAllRequests(m_cfgPtr, value); }
		}

        [DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_IntenalVolumeImplEnabled(IntPtr cfgPtr, bool Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_IntenalVolumeImplEnabled(IntPtr cfgPtr, ref bool Enabled);
		public bool IntenalVolumeImplEnabled
		{
			get { bool s = false; cfg_get_IntenalVolumeImplEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_IntenalVolumeImplEnabled(m_cfgPtr, value); }
		}


		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_AppendExtToRecFileNameEnabled(IntPtr cfgPtr, bool Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_AppendExtToRecFileNameEnabled(IntPtr cfgPtr, ref bool Enabled);
		public bool AppendExtToRecFileNameEnabled
		{
			get { bool s = false; cfg_get_AppendExtToRecFileNameEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_AppendExtToRecFileNameEnabled(m_cfgPtr, value); }
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_OverridePlayingFileEnabled(IntPtr cfgPtr, bool Enabled);				
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_OverridePlayingFileEnabled(IntPtr cfgPtr, ref bool Enabled);
		public bool OverridePlayingFileEnabled
		{
			get { bool s = false; cfg_get_OverridePlayingFileEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_OverridePlayingFileEnabled(m_cfgPtr, value); }
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_SubscriptionExpire(IntPtr cfgPtr, int seconds);		
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_SubscriptionExpire(IntPtr cfgPtr, ref int seconds);
		public int SubscriptionExpire
		{
			get { int s = 0; cfg_get_SubscriptionExpire(m_cfgPtr, ref s); return s; }
			set { cfg_put_SubscriptionExpire(m_cfgPtr, value); }
		}	


		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_TlsPrivKeyPass(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string pass);							
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_TlsPrivKeyPass(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string pass);
		public string TlsPrivKeyPass
		{
			get { string s = ""; cfg_get_TlsPrivKeyPass(m_cfgPtr, ref s); return s; }
			set { cfg_put_TlsPrivKeyPass(m_cfgPtr, value); }
		}

		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_TlsCertFilename(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string fileName);							
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_TlsCertFilename(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string fileName);
		public string TlsCertFilename
		{
			get { string s = ""; cfg_get_TlsCertFilename(m_cfgPtr, ref s); return s; }
			set { cfg_put_TlsCertFilename(m_cfgPtr, value); }
		}


		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_TlsPrivKeyFilename(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string fileName);							
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_TlsPrivKeyFilename(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref string fileName);
		public string TlsPrivKeyFilename
		{
			get { string s = ""; cfg_get_TlsPrivKeyFilename(m_cfgPtr, ref s); return s; }
			set { cfg_put_TlsPrivKeyFilename(m_cfgPtr, value); }
		}


		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_TlsVerifyServerEnabled(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]bool Enabled);							
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_TlsVerifyServerEnabled(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref bool Enabled);
		public bool TlsVerifyServerEnabled
		{
			get { bool s=false; cfg_get_TlsVerifyServerEnabled(m_cfgPtr, ref s); return s; }
			set { cfg_put_TlsVerifyServerEnabled(m_cfgPtr, value); }
		}


		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_put_OutgoingTcpPort(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]int Port);							
		[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void cfg_get_OutgoingTcpPort(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]ref int Port);
		public int OutgoingTcpPort
		{
			get { int s=0; cfg_get_OutgoingTcpPort(m_cfgPtr, ref s); return s; }
			set { cfg_put_OutgoingTcpPort(m_cfgPtr, value); }
		}

	}//CConfig



	public class CSubscriptions
	{		
		IntPtr m_subscrPtr;

		public CSubscriptions(IntPtr subscrPtr)
		{
			m_subscrPtr = subscrPtr;
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void subscr_SubscribeBLF(IntPtr subscrPtr, [MarshalAs(UnmanagedType.BStr)]string sipUri, ref int subscriptionId);
		public int SubscribeBLF(string sipUri)
		{
			int subscriptionId=0;
			subscr_SubscribeBLF(m_subscrPtr, sipUri, ref subscriptionId);
			return subscriptionId;
		}
		
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void subscr_SubscribePresence(IntPtr subscrPtr, [MarshalAs(UnmanagedType.BStr)]string sipUri, ref int subscriptionId);
		public int SubscribePresence(string sipUri)
		{
			int subscriptionId=0;
			subscr_SubscribePresence(m_subscrPtr, sipUri, ref subscriptionId);
			return subscriptionId;
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void subscr_SubscribeMessageSummary(IntPtr cfgPtr, ref int subscriptionId);
		public int SubscribeMessageSummary()
		{
			int subscriptionId=0;
			subscr_SubscribeMessageSummary(m_subscrPtr, ref subscriptionId);
			return subscriptionId;
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void subscr_SubscribeCustomEvent(IntPtr cfgPtr, [MarshalAs(UnmanagedType.BStr)]string eventName,
																			 [MarshalAs(UnmanagedType.BStr)]string mimeSubType,
																			[MarshalAs(UnmanagedType.BStr)]string sipUri, ref int subscriptionId);
		public int SubscribeCustomEvent(string eventName, string mimeSubType, string sipUri)
		{
			int subscriptionId=0;
			subscr_SubscribeCustomEvent(m_subscrPtr, eventName, mimeSubType, sipUri, ref subscriptionId);
			return subscriptionId;
		}
				
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void subscr_UnSubscribeBLF(IntPtr subscrPtr, int subscriptionId);
		public void UnSubscribeBLF(int subscriptionId)
		{		
			subscr_UnSubscribeBLF(m_subscrPtr, subscriptionId);		
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void subscr_UnSubscribePresence(IntPtr subscrPtr, int subscriptionId);
		public void UnSubscribePresence(int subscriptionId)
		{		
			subscr_UnSubscribePresence(m_subscrPtr, subscriptionId);		
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void subscr_UnSubscribeMessageSummary(IntPtr subscrPtr, int subscriptionId);
		public void UnSubscribeMessageSummary(int subscriptionId)
		{		
			subscr_UnSubscribeMessageSummary(m_subscrPtr, subscriptionId);		
		}
		
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void subscr_UnSubscribeCustomEvent(IntPtr subscrPtr, int subscriptionId);
		public void UnSubscribeCustomEvent(int subscriptionId)
		{		
			subscr_UnSubscribeCustomEvent(m_subscrPtr, subscriptionId);		
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void subscr_NotifyPresence(IntPtr subscrPtr, int open, [MarshalAs(UnmanagedType.BStr)]string status);
		public void NotifyPresence(int open, string status)
		{			
			subscr_NotifyPresence(m_subscrPtr, open, status);			
		}

	};//CSubscriptions



	///////////////////////////////////////////////////////////////////////////////////

	public class CAbtoPhoneClass : IDisposable
	{
		private IntPtr m_instPtr;

		#region Constructors

		public CAbtoPhoneClass()
		{
			m_instPtr = ABTOPhone_Create();

			m_pFuncInitializedEventT			= new OnInitializedEventT			(FuncInitializedEventT			);
			m_pFuncLineSwichedEventT			= new OnLineSwichedEventT			(FuncLineSwichedEventT			); 
			m_pFuncEstablishedConnectionEventT	= new OnEstablishedConnectionEventT	(FuncEstablishedConnectionEventT); 
			m_pFuncClearedConnectionEventT		= new OnClearedConnectionEventT		(FuncClearedConnectionEventT	); 
			m_pFuncIncomingCallEventT			= new OnIncomingCallEventT			(FuncIncomingCallEventT			);
			m_pFuncEstablishedCallEventT		= new OnEstablishedCallEventT		(FuncEstablishedCallEventT		);
			m_pFuncClearedCallEventT			= new OnClearedCallEventT			(FuncClearedCallEventT			);
			m_pFuncVolumeUpdatedEventT			= new OnVolumeUpdatedEventT			(FuncVolumeUpdatedEventT		);
			m_pFuncRegisteredEventT				= new OnRegisteredEventT			(FuncRegisteredEventT			);
			m_pFuncUnRegisteredEventT			= new OnUnRegisteredEventT			(FuncUnRegisteredEventT			);
			m_pFuncPlayFinishedEventT			= new OnPlayFinishedEventT			(FuncPlayFinishedEventT			);
			m_pFuncToneReceivedEventT			= new OnToneReceivedEventT			(FuncToneReceivedEventT			);
			m_pFuncTextMessageReceivedEventT	= new OnTextMessageReceivedEventT	(FuncTextMessageReceivedEventT	);
			m_pFuncPhoneNotifyEventT			= new OnPhoneNotifyEventT			(FuncPhoneNotifyEventT			);
			m_pFuncRemoteAlertingEventT			= new OnRemoteAlertingEventT		(FuncRemoteAlertingEventT		);
			m_pFuncHoldCallEventT				= new OnHoldCallEventT				(FuncHoldCallEventT				);
			m_pFuncTextMessageSentStatusEventT	= new OnTextMessageSentStatusEventT	(FuncTextMessageSentStatusEventT);
			m_pFuncRecordFinishedEventT			= new OnRecordFinishedEventT		(FuncRecordFinishedEventT		);

			m_pFuncSubscribeStatusEventT		= new OnSubscribeStatusEventT		(FuncSubscribeStatusEventT		);
			m_pFuncUnSubscribeStatusEventT		= new OnUnSubscribeStatusEventT		(FuncUnSubscribeStatusEventT	);
			m_pFuncSubscriptionRequestEventT	= new OnSubscriptionRequestEventT	(FuncSubscriptionRequestEventT	);
			m_pFuncSubscriptionNotifyEventT		= new OnSubscriptionNotifyEventT	(FuncSubscriptionNotifyEventT	);
			m_pFuncDetectedAnswerTimeEventT		= new OnDetectedAnswerTimeEventT	(FuncDetectedAnswerTimeEventT	);
			m_pFuncBaudotToneReceivedEventT		= new OnBaudotToneReceivedEventT	(FuncBaudotToneReceivedEventT	);
			m_pFuncSubscriptionTerminatedEventT	= new OnSubscriptionTerminatedEventT(FuncSubscriptionTerminatedEventT);
			m_pFuncToneDetectedEventT			= new OnToneDetectedEventT			(FuncToneDetectedEventT			);
			m_pFuncReceivedSipNotifyMsgEventT	= new OnReceivedSipNotifyMsgEventT	(FuncReceivedSipNotifyMsgEventT);
			m_pFuncPlayFinished2EventT			= new OnPlayFinished2EventT			(FuncPlayFinished2EventT		);
			m_pFuncRemoteAlerting2EventT		= new OnRemoteAlerting2EventT		(FuncRemoteAlerting2EventT		);
			m_pFuncReceivedRequestInfoEventT	= new OnReceivedRequestInfoEventT	(FuncReceivedRequestInfoEventT	);
			m_pFuncBaudotFinishedEventT			= new OnBaudotFinishedEventT		(FuncBaudotFinishedEventT		);

			event_Set_OnInitialized(m_instPtr, m_pFuncInitializedEventT);
			event_Set_OnLineSwiched(m_instPtr, m_pFuncLineSwichedEventT);
			event_Set_OnEstablishedConnection(m_instPtr, m_pFuncEstablishedConnectionEventT);
			event_Set_OnClearedConnection(m_instPtr, m_pFuncClearedConnectionEventT);
			event_Set_OnIncomingCall(m_instPtr, m_pFuncIncomingCallEventT);
			event_Set_OnEstablishedCall(m_instPtr, m_pFuncEstablishedCallEventT);
			event_Set_OnClearedCall(m_instPtr, m_pFuncClearedCallEventT);
			event_Set_OnVolumeUpdated(m_instPtr, m_pFuncVolumeUpdatedEventT);
			event_Set_OnRegistered(m_instPtr, m_pFuncRegisteredEventT);
			event_Set_OnUnRegistered(m_instPtr, m_pFuncUnRegisteredEventT);
			event_Set_OnPlayFinished(m_instPtr, m_pFuncPlayFinishedEventT);
			event_Set_OnToneReceived(m_instPtr, m_pFuncToneReceivedEventT);
			event_Set_OnTextMessageReceived(m_instPtr, m_pFuncTextMessageReceivedEventT);
			event_Set_OnPhoneNotify(m_instPtr, m_pFuncPhoneNotifyEventT);
			event_Set_OnRemoteAlerting(m_instPtr, m_pFuncRemoteAlertingEventT);
			event_Set_OnHoldCall(m_instPtr, m_pFuncHoldCallEventT);
			event_Set_OnTextMessageSentStatus(m_instPtr, m_pFuncTextMessageSentStatusEventT);
			event_Set_OnRecordFinished(m_instPtr, m_pFuncRecordFinishedEventT);

			event_Set_OnSubscribeStatus(m_instPtr, m_pFuncSubscribeStatusEventT);
			event_Set_OnUnSubscribeStatus(m_instPtr, m_pFuncUnSubscribeStatusEventT);
			event_Set_OnSubscriptionRequest(m_instPtr, m_pFuncSubscriptionRequestEventT);
			event_Set_OnSubscriptionNotify(m_instPtr, m_pFuncSubscriptionNotifyEventT);
			event_Set_OnDetectedAnswerTime(m_instPtr, m_pFuncDetectedAnswerTimeEventT);
			event_Set_OnBaudotToneReceived(m_instPtr, m_pFuncBaudotToneReceivedEventT);
			event_Set_OnSubscriptionTerminated(m_instPtr, m_pFuncSubscriptionTerminatedEventT);
			event_Set_OnToneDetected(m_instPtr, m_pFuncToneDetectedEventT);
			event_Set_OnReceivedSipNotifyMsg(m_instPtr, m_pFuncReceivedSipNotifyMsgEventT);
			event_Set_OnPlayFinished2(m_instPtr, m_pFuncPlayFinished2EventT);
			event_Set_OnRemoteAlerting2(m_instPtr, m_pFuncRemoteAlerting2EventT);
			event_Set_OnReceivedRequestInfo(m_instPtr, m_pFuncReceivedRequestInfoEventT);
			event_Set_OnBaudotFinished(m_instPtr, m_pFuncBaudotFinishedEventT);

			
			IntPtr pValCfg = IntPtr.Zero;
			phone_get_Config(m_instPtr, ref pValCfg);
			mConfig = new CConfig(pValCfg);

			IntPtr pValSub = IntPtr.Zero;
			phone_get_Subscriptions(m_instPtr, ref pValSub);
			mSubscriptions = new CSubscriptions(pValSub);

		}

		#endregion // Constructors

		#region Destructors

		~CAbtoPhoneClass()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				// Free other state (managed objects).
			}

			// Free your own state (unmanaged objects).
			event_Set_OnInitialized(m_instPtr, null);
			event_Set_OnLineSwiched(m_instPtr, null);
			event_Set_OnEstablishedConnection(m_instPtr, null);
			event_Set_OnClearedConnection(m_instPtr, null);
			event_Set_OnIncomingCall(m_instPtr, null);
			event_Set_OnEstablishedCall(m_instPtr, null);
			event_Set_OnClearedCall(m_instPtr, null);
			event_Set_OnVolumeUpdated(m_instPtr, null);
			event_Set_OnRegistered(m_instPtr, null);
			event_Set_OnUnRegistered(m_instPtr, null);
			event_Set_OnPlayFinished(m_instPtr, null);
			event_Set_OnToneReceived(m_instPtr, null);
			event_Set_OnTextMessageReceived(m_instPtr, null);
			event_Set_OnPhoneNotify(m_instPtr, null);
			event_Set_OnRemoteAlerting(m_instPtr, null);
			event_Set_OnHoldCall(m_instPtr, null);
			event_Set_OnTextMessageSentStatus(m_instPtr, null);
			event_Set_OnRecordFinished(m_instPtr, null);

			event_Set_OnSubscribeStatus			(m_instPtr, null);
			event_Set_OnUnSubscribeStatus		(m_instPtr, null);
			event_Set_OnSubscriptionRequest		(m_instPtr, null);
			event_Set_OnSubscriptionNotify		(m_instPtr, null);
			event_Set_OnDetectedAnswerTime		(m_instPtr, null);
			event_Set_OnBaudotToneReceived		(m_instPtr, null);
			event_Set_OnSubscriptionTerminated	(m_instPtr, null);
			event_Set_OnToneDetected			(m_instPtr, null);
			event_Set_OnReceivedSipNotifyMsg	(m_instPtr, null);
			event_Set_OnPlayFinished2			(m_instPtr, null);
			event_Set_OnRemoteAlerting2			(m_instPtr, null);
			event_Set_OnReceivedRequestInfo		(m_instPtr, null);
			event_Set_OnBaudotFinished			(m_instPtr, null);

			ABTOPhone_Free(m_instPtr);
			m_instPtr = IntPtr.Zero;
		}

		#endregion // Destructors

	

		#region Events
			
		public event _IAbtoPhoneEvents_OnInitializedEventHandler			OnInitialized;
		public event _IAbtoPhoneEvents_OnLineSwichedEventHandler			OnLineSwiched;
		public event _IAbtoPhoneEvents_OnEstablishedCallEventHandler		OnEstablishedCall;
		public event _IAbtoPhoneEvents_OnIncomingCallEventHandler			OnIncomingCall;
		public event _IAbtoPhoneEvents_OnClearedCallEventHandler			OnClearedCall;
		public event _IAbtoPhoneEvents_OnVolumeUpdatedEventHandler			OnVolumeUpdated;
		public event _IAbtoPhoneEvents_OnRegisteredEventHandler				OnRegistered;
		public event _IAbtoPhoneEvents_OnUnRegisteredEventHandler			OnUnRegistered;
		public event _IAbtoPhoneEvents_OnPlayFinishedEventHandler			OnPlayFinished;
		public event _IAbtoPhoneEvents_OnEstablishedConnectionEventHandler	OnEstablishedConnection;
		public event _IAbtoPhoneEvents_OnClearedConnectionEventHandler		OnClearedConnection;
		public event _IAbtoPhoneEvents_OnToneReceivedEventHandler			OnToneReceived;
		public event _IAbtoPhoneEvents_OnTextMessageReceivedEventHandler	OnTextMessageReceived;
		public event _IAbtoPhoneEvents_OnPhoneNotifyEventHandler			OnPhoneNotify;
		public event _IAbtoPhoneEvents_OnRemoteAlertingEventHandler			OnRemoteAlerting;
		public event _IAbtoPhoneEvents_OnHoldCallEventHandler				OnHoldCall;
		public event _IAbtoPhoneEvents_OnTextMessageSentStatusEventHandler	OnTextMessageSentStatus;
		public event _IAbtoPhoneEvents_OnRecordFinishedEventHandler			OnRecordFinished;

		public event _IAbtoPhoneEvents_OnSubscribeStatusEventHandler		OnSubscribeStatus;
		public event _IAbtoPhoneEvents_OnUnSubscribeStatusEventHandler		OnUnSubscribeStatus;
		public event _IAbtoPhoneEvents_OnSubscriptionRequestEventHandler	OnSubscriptionRequest;
		public event _IAbtoPhoneEvents_OnSubscriptionNotifyEventHandler		OnSubscriptionNotify;
		public event _IAbtoPhoneEvents_OnDetectedAnswerTimeEventHandler		OnDetectedAnswerTime;
		public event _IAbtoPhoneEvents_OnBaudotToneReceivedEventHandler		OnBaudotToneReceived;
		public event _IAbtoPhoneEvents_OnSubscriptionTerminatedEventHandler	OnSubscriptionTerminated;
		public event _IAbtoPhoneEvents_OnToneDetectedEventHandler			OnToneDetected;
		public event _IAbtoPhoneEvents_OnReceivedSipNotifyMsgEventHandler	OnReceivedSipNotifyMsg;
		public event _IAbtoPhoneEvents_OnPlayFinished2EventHandler			OnPlayFinished2;
		public event _IAbtoPhoneEvents_OnRemoteAlerting2EventHandler		OnRemoteAlerting2;
		public event _IAbtoPhoneEvents_OnReceivedRequestInfoEventHandler	OnReceivedRequestInfo;
		public event _IAbtoPhoneEvents_OnBaudotFinishedEventHandler			OnBaudotFinished;
	
		#endregion // Events

		#region Imports


        [DllImport(CConfig.ImportedDllName, CallingConvention = CallingConvention.StdCall)]
		private static extern IntPtr ABTOPhone_Create();

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall)]
		private static extern void ABTOPhone_Free(IntPtr phoneInstPtr);


		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall)]
		private static extern int phone_Initialize(IntPtr instPtr);
		public int Initialize()
		{
			return phone_Initialize(m_instPtr);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall)]
		private static extern int phone_InitializeEx(IntPtr instPtr, int sendEventsFromSameThrd);
		public int InitializeEx(bool sendEventsFromSameThrd)
		{
			return phone_InitializeEx(m_instPtr, sendEventsFromSameThrd ? 1 : 0);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall)]
		private static extern void phone_SetCurrentLine(IntPtr instPtr, int LineId);
		public void SetCurrentLine(int LineId)
		{
			phone_SetCurrentLine(m_instPtr, LineId);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_StartCall(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string Destination);
		public void StartCall(string Destination)
		{
			phone_StartCall(m_instPtr, Destination);
		}


        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern int phone_StartCall2(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string Destination);
		public int StartCall2(string Destination)
		{
			return phone_StartCall2(m_instPtr, Destination);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern int phone_StartCallEx(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string Destination, [MarshalAs(UnmanagedType.BStr)]string FromDisplName);
        public int StartCallEx(string Destination, string FromDisplName)
		{
			return phone_StartCallEx(m_instPtr, Destination, FromDisplName);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern int phone_StartCallExLine(IntPtr instPtr, int lineId, [MarshalAs(UnmanagedType.BStr)]string Destination, [MarshalAs(UnmanagedType.BStr)]string FromDisplName);
        public int StartCallExLine(int lineId, string Destination, string FromDisplName)
		{
			return phone_StartCallExLine(m_instPtr, lineId, Destination, FromDisplName);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_HangUp(IntPtr instPtr, int ConnectionId);
		public void HangUp(int ConnectionId)
		{
			phone_HangUp(m_instPtr, ConnectionId);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_HangUpLastCall(IntPtr instPtr);
		public void HangUpLastCall()
		{
			phone_HangUpLastCall(m_instPtr);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_AnswerCall(IntPtr instPtr);
		public void AnswerCall()
		{
			phone_AnswerCall(m_instPtr);
		}	
		
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_RejectCall(IntPtr instPtr);
		public void RejectCall()
		{
			phone_RejectCall(m_instPtr);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_TransferCall(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string Destination);
		public void TransferCall(string Destination)
		{
			phone_TransferCall(m_instPtr, Destination);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_TransferConnection(IntPtr instPtr, int ConnectionId, [MarshalAs(UnmanagedType.BStr)]string Dest);
		public void TransferConnection(int ConnectionId, string Dest)
		{
			phone_TransferConnection(m_instPtr, ConnectionId, Dest);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_AttendedTransferCall(IntPtr instPtr, int lineId);
		public void AttendedTransferCall(int lineId)
		{
			phone_AttendedTransferCall(m_instPtr, lineId);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_HoldRetrieveCurrentCall(IntPtr instPtr);
		public void HoldRetrieveCurrentCall()
		{
			phone_HoldRetrieveCurrentCall(m_instPtr);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_JoinToCurrentCall(IntPtr instPtr, int lineId);
		public void JoinToCurrentCall(int lineId)
		{
			phone_JoinToCurrentCall(m_instPtr, lineId);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_PlayFile(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string FilePath, ref int Succeeded);
		public int PlayFile(string FilePath)
		{
			int Succeeded=0;
			phone_PlayFile(m_instPtr, FilePath, ref Succeeded);
			return Succeeded;
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_PlayFileLine(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string FilePath, int lineId, ref int Succeeded);
		public int PlayFileLine(string FilePath, int lineId)
		{
			int Succeeded = 0;
			phone_PlayFileLine(m_instPtr, FilePath, lineId, ref Succeeded);
			return Succeeded;
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_StopPlayback(IntPtr instPtr);
		public void StopPlayback()
		{
			phone_StopPlayback(m_instPtr);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_StartRecording(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string FilePath);
		public void StartRecording(string FilePath)
		{
			phone_StartRecording(m_instPtr, FilePath);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_StopRecording(IntPtr instPtr);
		public void StopRecording()
		{
			phone_StopRecording(m_instPtr);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_SendTextMessage(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string Addr, [MarshalAs(UnmanagedType.BStr)]string Msg, int bSendU);
		public void SendTextMessage(string Addr, string Msg, int bSendU)
		{
			phone_SendTextMessage(m_instPtr, Addr, Msg, bSendU);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_SendTone(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string Tone);
		public void SendTone(string Tone)
		{
			phone_SendTone(m_instPtr, Tone);
		}

	
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_RetrieveExternalAddress(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]ref string pVal);
		public string RetrieveExternalAddress()
		{
			string s="";
			phone_RetrieveExternalAddress(m_instPtr, ref s);
			return s;
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_RetrieveVersion(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]ref string pVal);
		public string RetrieveVersion()
		{
			string s="";
			phone_RetrieveVersion(m_instPtr, ref s);
			return s;
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_SDKPath(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]ref string pVal);
		public string SDKPath()
		{
			string s="";
			phone_SDKPath(m_instPtr, ref s);
			return s;
		}


        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_GetSIPHeaderValue(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string Header, [MarshalAs(UnmanagedType.BStr)]ref string pVal);
        public string GetSIPHeaderValue(string Header)
		{
			string s="";
            phone_GetSIPHeaderValue(m_instPtr, Header, ref s);
			return s;
		}


        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_HasSIPBodyVideoMedia(IntPtr instPtr, int lineId, ref int pHasVideoMediaVal);
        public int HasSIPBodyVideoMedia(int lineId)
		{
			int s=0;
			phone_HasSIPBodyVideoMedia(m_instPtr, lineId, ref s);
			return s;
		}


		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_ApplyConfig(IntPtr instPtr);
		public void ApplyConfig()
		{
			phone_ApplyConfig(m_instPtr);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_CancelConfig(IntPtr instPtr);
		public void CancelConfig()
		{
			phone_CancelConfig(m_instPtr);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_get_Config(IntPtr instPtr, ref IntPtr pVal);
		public CConfig Config
		{
			get
			{
				IntPtr pVal = IntPtr.Zero;
				phone_get_Config(m_instPtr, ref pVal);
				//return new CConfig(pVal);
				return mConfig;
			}
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_get_Subscriptions(IntPtr instPtr, ref IntPtr pVal);
		public CSubscriptions Subscriptions
		{
			get{	return mSubscriptions;	}
		}



		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_put_PlaybackVolume(IntPtr instPtr, int Level);
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_get_PlaybackVolume(IntPtr instPtr, ref int Level);
		public int PlaybackVolume
		{
			get { int s = 0; phone_get_PlaybackVolume(m_instPtr, ref s); return s; }
			set { phone_put_PlaybackVolume(m_instPtr, value); }
		}


		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_put_PlaybackMuted(IntPtr instPtr, int Muted);
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_get_PlaybackMuted(IntPtr instPtr, ref int Muted);
		public int PlaybackMuted
		{
			get { int s = 0; phone_get_PlaybackMuted(m_instPtr, ref s); return s; }
			set { phone_put_PlaybackMuted(m_instPtr, value); }
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_put_RecordVolume(IntPtr instPtr, int Level);
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_get_RecordVolume(IntPtr instPtr, ref int Level);
		public int RecordVolume
		{	
			get { int s = 0; phone_get_RecordVolume(m_instPtr, ref s); return s; }
			set { phone_put_RecordVolume(m_instPtr, value); }
		}			

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_put_RecordMuted(IntPtr instPtr, int Muted);
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_get_RecordMuted(IntPtr instPtr, ref int Muted);
		public int RecordMuted
		{
			get { int s = 0; phone_get_RecordMuted(m_instPtr, ref s); return s; }
			set { phone_put_RecordMuted(m_instPtr, value); }
		}	

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_SetConnectionContribution(IntPtr instPtr, int ConnId, int inputGain, int outputGain);
		public void SetConnectionContribution(int ConnId, int inputGain, int outputGain)
		{
			phone_SetConnectionContribution(m_instPtr, ConnId, inputGain, outputGain);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_SetConnectionContributionRelated(IntPtr instPtr, int ConnIdInput, int ConnIdOutput, int inputGain, int outputGain);
		public void SetConnectionContributionRelated(int ConnIdInput, int ConnIdOutput, int inputGain, int outputGain)
		{
			phone_SetConnectionContributionRelated(m_instPtr, ConnIdInput, ConnIdOutput, inputGain, outputGain);
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_SetConnectionContributionRelatedLocal(IntPtr instPtr, int ConnIdInput, int inputGain, int outputGain);
		public void SetConnectionContributionRelatedLocal(int ConnIdInput, int inputGain, int outputGain)
		{
			phone_SetConnectionContributionRelatedLocal(m_instPtr, ConnIdInput, inputGain, outputGain);
		}
	

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_IsLineOccupied(IntPtr instPtr, int lineId, ref int Occupied);
		public int IsLineOccupied(int lineId)
		{
			int Occupied=0; phone_IsLineOccupied(m_instPtr, lineId, ref Occupied); return Occupied;
		}
        

        //added 2015/08/13
        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_SetSIPHeaderValue(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string Header, [MarshalAs(UnmanagedType.BStr)]string HeaderVal);
		public void SetSIPHeaderValue(string Header, string HeaderVal)
		{	
			phone_SetSIPHeaderValue(m_instPtr, Header, HeaderVal);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_GetSIPHeaderValueLine(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string Header, int lineId, [MarshalAs(UnmanagedType.BStr)]ref string headerVal);
		public string GetSIPHeaderValueLine(string Header, int lineId)
		{
			string val=""; phone_GetSIPHeaderValueLine(m_instPtr, Header, lineId, ref val); return val;
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_StartRecordingConnection(IntPtr instPtr, int ConnectionId, [MarshalAs(UnmanagedType.BStr)]string FilePath);
		public void StartRecordingConnection(int ConnectionId, string FilePath)
		{
			phone_StartRecordingConnection(m_instPtr, ConnectionId, FilePath);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_StopRecordingConnection(IntPtr instPtr, int ConnectionId);
		public void StopRecordingConnection(int ConnectionId)
		{
			phone_StopRecordingConnection(m_instPtr, ConnectionId);
		}


        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_GetSIPBodyMultiPartMimeText(IntPtr instPtr, int PartIdx, [MarshalAs(UnmanagedType.BStr)]ref string TextVal);
		public string GetSIPBodyMultiPartMimeText(int PartIdx)
		{
			string val=""; phone_GetSIPBodyMultiPartMimeText(m_instPtr, PartIdx, ref val); return val;
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_HoldRetrieveCall(IntPtr instPtr, int lineId);
		public void HoldRetrieveCall(int lineId)
		{
			phone_HoldRetrieveCall(m_instPtr, lineId);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_SendBaudotTone(IntPtr instPtr, int BaudotTone);
		public void SendBaudotTone(int BaudotTone)
		{
			phone_SendBaudotTone(m_instPtr, BaudotTone);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_DnsSrvLookup(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string sipSrvName, ref int receivedRecordsCount);
		public int SendBaudotTone(string sipSrvName)
		{
			int val=0; phone_DnsSrvLookup(m_instPtr, sipSrvName, ref val); return val;
		}

         [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_DnsSrvGetDetails(IntPtr instPtr, int receivedRecordIndex, [MarshalAs(UnmanagedType.BStr)]ref string hostName, [MarshalAs(UnmanagedType.BStr)]ref string srcIP, ref int port, ref int priority, ref int weight);
		public void DnsSrvGetDetails(int receivedRecordIndex, [MarshalAs(UnmanagedType.BStr)]ref string hostName, [MarshalAs(UnmanagedType.BStr)]ref string srcIP, ref int port, ref int priority, ref int weight)
		{
			phone_DnsSrvGetDetails(m_instPtr, receivedRecordIndex, ref hostName, ref srcIP, ref port, ref priority, ref weight);
		}

	
        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_SetPlayingFileContribution(IntPtr instPtr, int gain);
		public void SetPlayingFileContribution(int gain)
		{
			phone_SetPlayingFileContribution(m_instPtr, gain);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void  phone_StartVoiceEnergyDetectionPeriod(IntPtr instPtr, int timeOutMs, int minEnergyValue, int maxEnergyValue, int connectionId);
		public void StartVoiceEnergyDetectionPeriod(int timeOutMs, int minEnergyValue, int maxEnergyValue, int connectionId)
		{
			phone_StartVoiceEnergyDetectionPeriod(m_instPtr, timeOutMs, minEnergyValue, maxEnergyValue, connectionId);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void   phone_StopPlaybackLine(IntPtr instPtr, int lineId);
		public void StopPlaybackLine(int lineId)
		{
			phone_StopPlaybackLine(m_instPtr, lineId);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void   phone_StartSendingNoise(IntPtr instPtr, int Tone1AmpPermille, int Tone2AmpPermille, int IntervalMs, int FrequencyHz);
		public void StartSendingNoise(int Tone1AmpPermille, int Tone2AmpPermille, int IntervalMs, int FrequencyHz)
		{
			phone_StartSendingNoise(m_instPtr, Tone1AmpPermille, Tone2AmpPermille, IntervalMs, FrequencyHz);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void   phone_MoveConnectionToLine(IntPtr instPtr, int lineId, int ConnectionId);
		public void MoveConnectionToLine(int lineId, int ConnectionId)
		{
			phone_MoveConnectionToLine(m_instPtr, lineId, ConnectionId);
		}
	
        //         [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        // 		private static extern void   phone_StartRecordingConnectionBuffer(int ConnectionId, intint bufferPtr, int size);
        // 		public void phone_StartRecordingConnectionBuffer(int ConnectionId, intint bufferPtr, int size);
        // 		{
        // 			phone_StartRecordingConnectionBuffer(int ConnectionId, intint bufferPtr, int size);
        // 		}
        // 	    phone_StartRecordingConnectionBuffer(int ConnectionId, intint bufferPtr, int size);

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void   phone_BindToAudioDevices(IntPtr instPtr);
		public void BindToAudioDevices()
		{
			phone_BindToAudioDevices(m_instPtr);
		}

	    [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_AttendedTransferCallLine(IntPtr instPtr, int fromLineId, int toLineId);
		public void AttendedTransferCallLine(int fromLineId, int toLineId)
		{
			phone_AttendedTransferCallLine(m_instPtr, fromLineId, toLineId);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_SendBaudotString(IntPtr instPtr, string BaudotString);
		public void SendBaudotString(string BaudotString)
		{
			phone_SendBaudotString(m_instPtr, BaudotString);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void phone_RestartAudioSubsystemEx(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string PrimaryPlaybackDvc, [MarshalAs(UnmanagedType.BStr)]string SecPlaybackDvc, [MarshalAs(UnmanagedType.BStr)]string PrimaryRecordDvc, [MarshalAs(UnmanagedType.BStr)]string SecRecordDvc);
		public void RestartAudioSubsystemEx(string PrimaryPlaybackDvc, string SecPlaybackDvc, string PrimaryRecordDvc, string SecRecordDvc)
		{
			phone_RestartAudioSubsystemEx(m_instPtr, PrimaryPlaybackDvc, SecPlaybackDvc, PrimaryRecordDvc, SecRecordDvc);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_GetSIPHeaderValueLineArr(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string Header, int lineId, ref object values);
		public object GetSIPHeaderValueLineArr(string Header, int lineId)
		{
			object val=null; phone_GetSIPHeaderValueLineArr(m_instPtr, Header, lineId, ref val); return val;
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_MuteLocalVideo(IntPtr instPtr, int bMute);
		public void MuteLocalVideo(int bMute)
		{
			phone_MuteLocalVideo(m_instPtr, bMute);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_UpdateCall(IntPtr instPtr, int ConnectionId, [MarshalAs(UnmanagedType.BStr)]string FromDisplName, bool bSendInvite);
		public void UpdateCall(int ConnectionId, string FromDisplName, bool bSendInvite)
		{
			phone_UpdateCall(m_instPtr, ConnectionId, FromDisplName, bSendInvite);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_SendRequestInfo(IntPtr instPtr, int ConnectionId, [MarshalAs(UnmanagedType.BStr)]string contentType, [MarshalAs(UnmanagedType.BStr)]string msg);	
		public void SendRequestInfo(int ConnectionId, string contentType, string msg)
		{
			phone_SendRequestInfo(m_instPtr, ConnectionId, contentType, msg);	
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_SendRequestMWINotify(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string destination, [MarshalAs(UnmanagedType.BStr)]string body);
		public void SendRequestMWINotify(string destination, string body)
		{
			phone_SendRequestMWINotify(m_instPtr, destination, body);	
		}
				
        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_MakeDumpFile(IntPtr instPtr);
		public void MakeDumpFile()
		{
			phone_MakeDumpFile(m_instPtr);
		}
	
        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_AnswerCallLine(IntPtr instPtr, int lineId);
		public void AnswerCallLine(int lineId)
		{
			phone_AnswerCallLine(m_instPtr, lineId);
		}
	
        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_RejectCallLine(IntPtr instPtr, int lineId);
		public void RejectCallLine(int lineId)
		{
			phone_RejectCallLine(m_instPtr, lineId);
		}
	
        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_GetSIPMessageLine(IntPtr instPtr, int lineId, [MarshalAs(UnmanagedType.BStr)]ref string msg);
		public string GetSIPMessageLine(int lineId)
		{
			string val=""; phone_GetSIPMessageLine(m_instPtr, lineId, ref val); return val;
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_AssignVideoWindow(IntPtr instPtr, int ConnectionId, int hVideoWindow);
		public void AssignVideoWindow(int ConnectionId, int hVideoWindow)
		{
			phone_AssignVideoWindow(m_instPtr, ConnectionId, hVideoWindow);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_StartCall3(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string Destination, [MarshalAs(UnmanagedType.BStr)]string FromUserName, ref int ConnectionId);
		public int StartCall3(string Destination,string FromUserName)
		{
			int val=0; phone_StartCall3(m_instPtr, Destination, FromUserName, ref val); return val;
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void  phone_SendBaudotTone2(IntPtr instPtr, int BaudotTone, int gain);
		public void SendBaudotTone2(int BaudotTone, int gain)
		{
			 phone_SendBaudotTone2(m_instPtr, BaudotTone, gain);
		}

        [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_SendBaudotString2(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string BaudotString, int gain);
        public void SendBaudotString2(string BaudotString, int gain)
		{
			 phone_SendBaudotString2(m_instPtr, BaudotString, gain);
		}


		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_StartCall4(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string Destination, int transp, ref int ConnId);
        public int StartCall4(string Destination, int transp)
		{
			int ConnId=0;
			phone_StartCall4(m_instPtr, Destination, transp, ref ConnId);
			return ConnId;
		}

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_HangUpCallLine(IntPtr instPtr, int lineId);
		public void HangUpCallLine(int lineId)
		{
			phone_HangUpCallLine(m_instPtr, lineId);
		}


		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_CalcAudioFileDuration(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string FilePath, ref int durationMS);
		public int CalcAudioFileDuration(string FilePath)
		{
			int durationMS=0;
			phone_CalcAudioFileDuration(m_instPtr, FilePath, ref durationMS);
			return durationMS;			
		}


		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_StartScreenSharing(IntPtr instPtr, int bStart);
		public void StartScreenSharing()
		{			
			phone_StartScreenSharing(m_instPtr, 0);		
		}
			
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_StopScreenSharing(IntPtr instPtr);
		public void StopScreenSharing()
		{			
			phone_StopScreenSharing(m_instPtr);
		}			
		
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_StartPlayVideoFile(IntPtr instPtr, [MarshalAs(UnmanagedType.BStr)]string FilePath);
		public void StartPlayVideoFile(string FilePath)
		{			
			phone_StartPlayVideoFile(m_instPtr, FilePath);
		}
			
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void phone_StopPlayVideoFile(IntPtr instPtr);
		public void StopPlayVideoFile()
		{			
			phone_StopPlayVideoFile(m_instPtr);
		}

		
		
		//[DllImport(ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		//private unsafe static extern void phone_PlayBuffer(byte* buffer, int size, int rate);

		//[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		//private unsafe static extern void phone_StartRecordingBuffer(byte* buffer, int size);		

		#endregion // Imports

		#region Event Generation Imports/Internal delegates

		delegate void OnInitializedEventT([MarshalAs(UnmanagedType.BStr)]string Msg);
		delegate void OnLineSwichedEventT(int LineId);
		delegate void OnEstablishedConnectionEventT([MarshalAs(UnmanagedType.BStr)]string AddrFrom, 
													[MarshalAs(UnmanagedType.BStr)]string AddrTo, 
													int ConnectionId, int LineId);
		delegate void OnClearedConnectionEventT(int ConnectionId, int LineId);
		delegate void OnIncomingCallEventT([MarshalAs(UnmanagedType.BStr)]string AddrFrom, int LineId);
		delegate void OnEstablishedCallEventT([MarshalAs(UnmanagedType.BStr)]string Msg, int LineId);
		delegate void OnClearedCallEventT([MarshalAs(UnmanagedType.BStr)]string Msg, int Status, int LineId);
		delegate void OnVolumeUpdatedEventT(int IsMicrophone, int Level);
		delegate void OnRegisteredEventT([MarshalAs(UnmanagedType.BStr)]string Msg);
		delegate void OnUnRegisteredEventT([MarshalAs(UnmanagedType.BStr)]string Msg);
		delegate void OnPlayFinishedEventT([MarshalAs(UnmanagedType.BStr)]string Msg);
		delegate void OnToneReceivedEventT(int Tone, int ConnectionId, int LineId);
		delegate void OnTextMessageReceivedEventT([MarshalAs(UnmanagedType.BStr)]string address, 
												  [MarshalAs(UnmanagedType.BStr)]string message);	
		delegate void OnPhoneNotifyEventT([MarshalAs(UnmanagedType.BStr)]string Msg);
		delegate void OnRemoteAlertingEventT(int ConnectionId, int responseCode, 
											[MarshalAs(UnmanagedType.BStr)]string reasonMsg);
		delegate void OnHoldCallEventT(int LineId, int isHeld);
		delegate void OnTextMessageSentStatusEventT([MarshalAs(UnmanagedType.BStr)]string address, 
													[MarshalAs(UnmanagedType.BStr)]string reason, int bSuccess);
		delegate void OnRecordFinishedEventT([MarshalAs(UnmanagedType.BStr)]string Msg);

        //added 2015/08/13
        delegate void OnSubscribeStatusEventT(int subscriptionId, int statusCode, [MarshalAs(UnmanagedType.BStr)]string statusMsg);
        delegate void OnUnSubscribeStatusEventT(int subscriptionId, int statusCode, [MarshalAs(UnmanagedType.BStr)]string statusMsg);
        delegate void OnSubscriptionRequestEventT([MarshalAs(UnmanagedType.BStr)]string fromUri, [MarshalAs(UnmanagedType.BStr)]string eventStr);
        delegate void OnSubscriptionNotifyEventT(int subscriptionId, [MarshalAs(UnmanagedType.BStr)]string StateStr, 
                                                    [MarshalAs(UnmanagedType.BStr)]string NotifyStr);
        delegate void OnDetectedAnswerTimeEventT(int TimeSpanMs, int ConnectionId);
        delegate void OnBaudotToneReceivedEventT(int Tone, int ConnectionId, int LineId);
        delegate void OnSubscriptionTerminatedEventT([MarshalAs(UnmanagedType.BStr)]string fromUri);
        delegate void OnToneDetectedEventT(ToneType type, [MarshalAs(UnmanagedType.BStr)]string ToneStr, int ConnectionId, int LineId);
        delegate void OnReceivedSipNotifyMsgEventT([MarshalAs(UnmanagedType.BStr)]string SipNotifyMsgStr);
        delegate void OnPlayFinished2EventT([MarshalAs(UnmanagedType.BStr)]string Msg, int LineId);
        delegate void OnRemoteAlerting2EventT(int ConnectionId, int LineId, int responseCode, [MarshalAs(UnmanagedType.BStr)]string reasonMsg);
        delegate void OnReceivedRequestInfoEventT(int ConnectionId, int LineId, 
                                                [MarshalAs(UnmanagedType.BStr)]string contentType, 
                                                [MarshalAs(UnmanagedType.BStr)]string body);
        delegate void OnBaudotFinishedEventT([MarshalAs(UnmanagedType.BStr)]string Msg, int LineId);


		CConfig mConfig;
		CSubscriptions mSubscriptions;

		OnInitializedEventT				m_pFuncInitializedEventT			;
		OnLineSwichedEventT				m_pFuncLineSwichedEventT			;
		OnEstablishedConnectionEventT	m_pFuncEstablishedConnectionEventT	;
		OnClearedConnectionEventT		m_pFuncClearedConnectionEventT		;
		OnIncomingCallEventT			m_pFuncIncomingCallEventT			;
		OnEstablishedCallEventT			m_pFuncEstablishedCallEventT		;
		OnClearedCallEventT				m_pFuncClearedCallEventT			; 
		OnVolumeUpdatedEventT			m_pFuncVolumeUpdatedEventT			;
		OnRegisteredEventT				m_pFuncRegisteredEventT				;
		OnUnRegisteredEventT			m_pFuncUnRegisteredEventT			;
		OnPlayFinishedEventT			m_pFuncPlayFinishedEventT			;
		OnToneReceivedEventT			m_pFuncToneReceivedEventT			;
		OnTextMessageReceivedEventT		m_pFuncTextMessageReceivedEventT	;
		OnPhoneNotifyEventT				m_pFuncPhoneNotifyEventT			;
		OnRemoteAlertingEventT			m_pFuncRemoteAlertingEventT			;
		OnHoldCallEventT				m_pFuncHoldCallEventT				;
		OnTextMessageSentStatusEventT	m_pFuncTextMessageSentStatusEventT	;
		OnRecordFinishedEventT			m_pFuncRecordFinishedEventT			;


		OnSubscribeStatusEventT         m_pFuncSubscribeStatusEventT          ;
		OnUnSubscribeStatusEventT       m_pFuncUnSubscribeStatusEventT        ;
		OnSubscriptionRequestEventT     m_pFuncSubscriptionRequestEventT      ;
		OnSubscriptionNotifyEventT     	m_pFuncSubscriptionNotifyEventT       ;
		OnDetectedAnswerTimeEventT      m_pFuncDetectedAnswerTimeEventT       ;
		OnBaudotToneReceivedEventT      m_pFuncBaudotToneReceivedEventT       ;
		OnSubscriptionTerminatedEventT  m_pFuncSubscriptionTerminatedEventT   ;
		OnToneDetectedEventT            m_pFuncToneDetectedEventT             ;
		OnReceivedSipNotifyMsgEventT    m_pFuncReceivedSipNotifyMsgEventT     ;
		OnPlayFinished2EventT           m_pFuncPlayFinished2EventT            ;
		OnRemoteAlerting2EventT         m_pFuncRemoteAlerting2EventT          ;
		OnReceivedRequestInfoEventT     m_pFuncReceivedRequestInfoEventT      ;
		OnBaudotFinishedEventT          m_pFuncBaudotFinishedEventT           ;

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnInitialized(IntPtr instPtr, OnInitializedEventT e);
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnLineSwiched(IntPtr instPtr, OnLineSwichedEventT e);
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnEstablishedConnection(IntPtr instPtr, OnEstablishedConnectionEventT e);	
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnClearedConnection(IntPtr instPtr, OnClearedConnectionEventT e);			
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnIncomingCall(IntPtr instPtr, OnIncomingCallEventT e);					
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnEstablishedCall(IntPtr instPtr, OnEstablishedCallEventT e);				
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnClearedCall(IntPtr instPtr, OnClearedCallEventT e);
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnVolumeUpdated(IntPtr instPtr, OnVolumeUpdatedEventT e);					
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnRegistered(IntPtr instPtr, OnRegisteredEventT e);						
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnUnRegistered(IntPtr instPtr, OnUnRegisteredEventT e);					
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnPlayFinished(IntPtr instPtr, OnPlayFinishedEventT e);					
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnToneReceived(IntPtr instPtr, OnToneReceivedEventT e);					
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnTextMessageReceived(IntPtr instPtr, OnTextMessageReceivedEventT e);		
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnPhoneNotify(IntPtr instPtr, OnPhoneNotifyEventT e);						
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnRemoteAlerting(IntPtr instPtr, OnRemoteAlertingEventT e);				
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnHoldCall(IntPtr instPtr, OnHoldCallEventT e);							
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnTextMessageSentStatus(IntPtr instPtr, OnTextMessageSentStatusEventT e);	
		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnRecordFinished(IntPtr instPtr, OnRecordFinishedEventT e);

		[DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnSubscribeStatus(IntPtr instPtr, OnSubscribeStatusEventT e);
	    [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnUnSubscribeStatus(IntPtr instPtr, OnUnSubscribeStatusEventT e);
	    [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnSubscriptionRequest(IntPtr instPtr, OnSubscriptionRequestEventT e);	
	    [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnSubscriptionNotify(IntPtr instPtr, OnSubscriptionNotifyEventT e);
	    [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnDetectedAnswerTime(IntPtr instPtr, OnDetectedAnswerTimeEventT e);
	    [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnBaudotToneReceived(IntPtr instPtr, OnBaudotToneReceivedEventT e);
	    [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnSubscriptionTerminated(IntPtr instPtr, OnSubscriptionTerminatedEventT e);
	    [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnToneDetected(IntPtr instPtr, OnToneDetectedEventT e);
	    [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnReceivedSipNotifyMsg(IntPtr instPtr, OnReceivedSipNotifyMsgEventT e);
	    [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnPlayFinished2(IntPtr instPtr, OnPlayFinished2EventT e);
	    [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnRemoteAlerting2(IntPtr instPtr, OnRemoteAlerting2EventT e);
	    [DllImport(CConfig.ImportedDllName, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern void event_Set_OnReceivedRequestInfo(IntPtr instPtr, OnReceivedRequestInfoEventT e);
        [DllImport(CConfig.ImportedDllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void event_Set_OnBaudotFinished(IntPtr instPtr, OnBaudotFinishedEventT e);

				
		void FuncInitializedEventT([MarshalAs(UnmanagedType.BStr)]string Msg)
		{
			if(OnInitialized != null)	OnInitialized(Msg);
		}

		void FuncLineSwichedEventT(int lineId)
		{
			if(OnLineSwiched != null)	OnLineSwiched(lineId);
		}

		void FuncEstablishedCallEventT([MarshalAs(UnmanagedType.BStr)]string adress, int lineId)
		{
			if(OnEstablishedCall != null)	OnEstablishedCall(adress, lineId);
		}

		void FuncIncomingCallEventT([MarshalAs(UnmanagedType.BStr)]string adress, int lineId)
		{
			if(OnIncomingCall != null)	OnIncomingCall(adress, lineId);
		}

		void FuncClearedCallEventT([MarshalAs(UnmanagedType.BStr)]string Msg, int status, int lineId)
		{
			if(OnClearedCall != null)	OnClearedCall(Msg, status, lineId);
		}

		void FuncVolumeUpdatedEventT(int IsMicrophone, int level)
		{
			if(OnVolumeUpdated != null)	OnVolumeUpdated(IsMicrophone, level);
		}

		void FuncRegisteredEventT([MarshalAs(UnmanagedType.BStr)]string Msg)
		{
			if(OnRegistered != null)	OnRegistered(Msg);
		}

		void FuncUnRegisteredEventT([MarshalAs(UnmanagedType.BStr)]string Msg)
		{
			if(OnUnRegistered != null)	OnUnRegistered(Msg);
		}

		void FuncPlayFinishedEventT([MarshalAs(UnmanagedType.BStr)]string Msg)
		{
			if(OnPlayFinished != null)	OnPlayFinished(Msg);
		}
		
		void FuncEstablishedConnectionEventT([MarshalAs(UnmanagedType.BStr)]string addrFrom, [MarshalAs(UnmanagedType.BStr)]string addrTo, int connectionId, int lineId)
		{
			if(OnEstablishedConnection != null)	OnEstablishedConnection(addrFrom, addrTo, connectionId, lineId);
		}

		void FuncClearedConnectionEventT(int connectionId, int lineId)
		{
			if(OnClearedConnection != null)	OnClearedConnection(connectionId, lineId);
		}
		
		void FuncToneReceivedEventT(int tone, int connectionId, int lineId)
		{
			if(OnToneReceived != null)	OnToneReceived(tone, connectionId, lineId);
		}

		void FuncTextMessageReceivedEventT([MarshalAs(UnmanagedType.BStr)]string from, [MarshalAs(UnmanagedType.BStr)]string message)
		{
			if(OnTextMessageReceived != null)	OnTextMessageReceived(from, message);
		}

		void FuncPhoneNotifyEventT([MarshalAs(UnmanagedType.BStr)]string message)
		{
			if(OnPhoneNotify != null)	OnPhoneNotify(message);
		}

		void FuncRemoteAlertingEventT(int connectionId, int responseCode, [MarshalAs(UnmanagedType.BStr)]string reasonMsg)
		{
			if(OnRemoteAlerting != null)	OnRemoteAlerting(connectionId, responseCode, reasonMsg);
		}

		void FuncHoldCallEventT(int LineId, int isHeld)
		{
			if(OnHoldCall != null) OnHoldCall(LineId, isHeld);
		}

		void FuncTextMessageSentStatusEventT([MarshalAs(UnmanagedType.BStr)]string address, [MarshalAs(UnmanagedType.BStr)]string reason, int bSuccess)
		{
			if(OnTextMessageSentStatus != null) OnTextMessageSentStatus(address, reason, bSuccess);
		}

		void FuncRecordFinishedEventT([MarshalAs(UnmanagedType.BStr)]string message)
		{
			if(OnRecordFinished != null) OnRecordFinished(message);
		}


		void FuncSubscribeStatusEventT(int subscriptionId, int statusCode, [MarshalAs(UnmanagedType.BStr)]string statusMsg)
		{
			if (OnSubscribeStatus != null) OnSubscribeStatus(subscriptionId, statusCode, statusMsg);
		}

		void FuncUnSubscribeStatusEventT(int subscriptionId, int statusCode, [MarshalAs(UnmanagedType.BStr)]string statusMsg)
		{
			if (OnUnSubscribeStatus != null) OnUnSubscribeStatus(subscriptionId, statusCode, statusMsg);
		}

		void FuncSubscriptionRequestEventT([MarshalAs(UnmanagedType.BStr)]string fromUri, [MarshalAs(UnmanagedType.BStr)]string eventStr)
		{
			if(OnSubscriptionRequest != null) OnSubscriptionRequest(fromUri, eventStr);
		}

		void FuncSubscriptionNotifyEventT(int subscriptionId, [MarshalAs(UnmanagedType.BStr)]string StateStr, [MarshalAs(UnmanagedType.BStr)]string NotifyStr)
		{
			if (OnSubscriptionNotify != null) OnSubscriptionNotify(subscriptionId, StateStr, NotifyStr);
		}

		void FuncDetectedAnswerTimeEventT(int TimeSpanMs, int ConnectionId)
		{
			if(OnDetectedAnswerTime != null ) OnDetectedAnswerTime(TimeSpanMs, ConnectionId);
		}
		void FuncBaudotToneReceivedEventT(int Tone, int ConnectionId, int LineId)
		{
			if (OnBaudotToneReceived != null) OnBaudotToneReceived(Tone, ConnectionId, LineId);
		}

		void FuncSubscriptionTerminatedEventT([MarshalAs(UnmanagedType.BStr)]string fromUri)
		{
			if(OnSubscriptionTerminated != null) OnSubscriptionTerminated(fromUri);
		}

		void FuncToneDetectedEventT(ToneType type, [MarshalAs(UnmanagedType.BStr)]string ToneStr, int ConnectionId, int LineId)
		{
			if(OnToneDetected != null) OnToneDetected(type, ToneStr, ConnectionId, LineId);
		}

		void FuncReceivedSipNotifyMsgEventT([MarshalAs(UnmanagedType.BStr)]string SipNotifyMsgStr)
		{
			if(OnReceivedSipNotifyMsg != null) OnReceivedSipNotifyMsg(SipNotifyMsgStr);
		}

		void FuncPlayFinished2EventT([MarshalAs(UnmanagedType.BStr)]string Msg, int LineId)
		{
			if(OnPlayFinished2 != null) OnPlayFinished2(Msg, LineId);
		}

		void FuncRemoteAlerting2EventT(int ConnectionId, int LineId, int responseCode, [MarshalAs(UnmanagedType.BStr)]string reasonMsg)
		{
			if (OnRemoteAlerting2 != null) OnRemoteAlerting2(ConnectionId, LineId, responseCode, reasonMsg);
		}

		void FuncReceivedRequestInfoEventT(int ConnectionId, int LineId, [MarshalAs(UnmanagedType.BStr)]string contentType, [MarshalAs(UnmanagedType.BStr)]string body)
		{
			if(OnReceivedRequestInfo != null ) OnReceivedRequestInfo(ConnectionId, LineId, contentType, body);
		}

		void FuncBaudotFinishedEventT([MarshalAs(UnmanagedType.BStr)]string Msg, int LineId)
		{
			if(OnBaudotFinished != null) OnBaudotFinished(Msg, LineId);
		}


		#endregion // Event Generation Imports


	}//CAbtoPhoneClass

}
