using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Net.Mail;
using System.Management;
using Microsoft.Win32;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Google.Apis.CloudSpeechAPI.v1beta1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;


namespace GCC
{
    class GM // Global Methods
    {
        public static DataTable dtFilteredCompanyList = new DataTable();
        public static int iRotationCount = 0;
        List<string> lstTwoLetterWords = new List<string>();

        GM()
        {
            lstTwoLetterWords.Add("an");
            lstTwoLetterWords.Add("of");
            lstTwoLetterWords.Add("be");
            lstTwoLetterWords.Add("is");
            lstTwoLetterWords.Add("so");
            lstTwoLetterWords.Add("in");
            lstTwoLetterWords.Add("as");
        }

        static public CloudSpeechAPIService CreateAuthorizedClient()
        {
            try
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
                    ApplicationName = "Campaign Manager",
                });
            }
            catch(Exception ex)
            {
               GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                return null;
            }
        }

        public static bool InitilizeGSpeech()
        {
            try
            {
                GV.GSpeechCloudAPI = CreateAuthorizedClient();
                return GV.GSpeechCloudAPI != null;
                
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                return false;
            }
        }


        public static string ProperCaseLeaveCapital(string sText)
        {
            //Case correction for text:Proper Case
            List<string> lstString = new List<string>();
            lstString = sText.Split(' ').ToList();
            string sReturn = string.Empty;
            CultureInfo properCase = System.Threading.Thread.CurrentThread.CurrentCulture;
            TextInfo currentInfo = properCase.TextInfo;
            foreach (string s in lstString)
            {
                //if (s.Length > 2)
                {
                    //sReturn += " " + currentInfo.ToTitleCase(s);
                    if (!Regex.Match(s, "[A-Z]+").Success)
                        sReturn += " " + currentInfo.ToTitleCase(currentInfo.ToLower(s));
                    else
                        sReturn += " " + s;
                }
                //else
                //    sReturn += " " + s.ToUpper();
            }
            return sReturn.Trim();
        }

        public static void StringAppend(string sFunName)
        {
            GV.sPerformance += sFunName + ":\t" + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
        }

        public static string Decrypt_Password(string sData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(sData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        public static bool IsFormExist(string sFormName)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == sFormName)
                {
                    f.Focus();
                    return true;
                }
            }
            return false;
        }

        public static Form GetForm(string sFormName)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == sFormName)
                {
                    return f;
                }
            }
            return null;
        }


        public static Image GetImageFromByte(byte[] bStream)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bStream);
            return Image.FromStream(ms);
        }

        public static string ChromeResponse(string sReply = "")
        {
            Form frmCompanyList = GetForm("frmCompanyList");
            string sChromeMenu = "Fields:{}";
            if (frmCompanyList != null && ((frmCompanyList)frmCompanyList).sChromeColumnSettings.Length > 0)
                sChromeMenu = ((frmCompanyList)frmCompanyList).sChromeColumnSettings;
            return "Response:{" + sReply + "}" + sChromeMenu + "ProjectName:{" + GV.sProjectName + "}CompanyID:{" + GV.sCurrentCompanyID + "}CompanyName:{" + GV.sCurrentCompanyName + "}Agent:{" + GV.sEmployeeName + "}";
        }


        public static DateTime GetDateTime()
        {
            DateTime UtcDateTime = DateTime.UtcNow;
            TimeZoneInfo GMTTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(UtcDateTime, GMTTimeZone);
        }

        public static DateTime GetDateTime(bool IsDBTime)
        {
            if (IsDBTime)
                return GV.MYSQL.BAL_GetDateTime();
            else
                return GetDateTime();
        }

        public static void Moniter()//This method will be replaced by [Moniter(bool IsLoggedIn, string DummyVariable)] over a period of time. 
        {
            string sQuery = @"MERGE INTO EMoniter AS TARGET USING(SELECT '" + GV.sEmployeeNo + "' as EMPLOYE_ID, '" + GV.sEmployeeActualName.Replace("'", "''") + "' as EMPLOYE_NAME, GETDATE() as LAST_UPDATED_ON, '" + GV.sProjectName + "' AS PROJECT_NAME) AS SOURCE ";
            sQuery += " on(Source.EMPLOYE_ID = Target.EMPLOYE_ID) WHEN MATCHED THEN UPDATE SET EMPLOYE_NAME = SOURCE.EMPLOYE_NAME, LAST_UPDATED_ON = SOURCE.LAST_UPDATED_ON, PROJECT_NAME = SOURCE.PROJECT_NAME ";
            sQuery += " WHEN NOT MATCHED BY TARGET THEN INSERT(EMPLOYE_NAME, EMPLOYE_ID, LAST_UPDATED_ON, PROJECT_NAME) VALUES(SOURCE.EMPLOYE_NAME, SOURCE.EMPLOYE_ID, SOURCE.LAST_UPDATED_ON, SOURCE.PROJECT_NAME);";
            GV.MSSQL.BAL_ExecuteQuery(sQuery);
        }

        public static void Moniter(bool IsLoggedIn, string DummyVariable)
        {
            if (IsLoggedIn)
                GV.MSSQL.BAL_ExecuteQuery("UPDATE EMoniter SET PROJECT_NAME = '" + GV.sProjectName + "', CALL_STATUS = '', LAST_UPDATED_ON = GETDATE() WHERE EMPLOYE_ID='" + GV.sEmployeeNo + "'");
            else
                GV.MSSQL.BAL_ExecuteQuery("UPDATE EMoniter SET PROJECT_NAME = '', CALL_STATUS = '', LAST_UPDATED_ON = GETDATE() WHERE EMPLOYE_ID='" + GV.sEmployeeNo + "'");
        }

        public static void Moniter(string sCallStatus)
        {
            if (GV.sAccessTo == "TR")
                GV.MSSQL.BAL_ExecuteQuery("UPDATE EMoniter SET CALL_STATUS = '" + sCallStatus + "', LAST_UPDATED_ON = GETDATE() WHERE EMPLOYE_ID='" + GV.sEmployeeNo + "'");
        }

        public static void Moniter(bool IsCompanySaved)
        {
            if (GV.sAccessTo == "TR")
                GV.MSSQL.BAL_ExecuteQuery("UPDATE EMoniter SET CALL_STATUS = '', COMPANY_LAST_SAVED = GETDATE() , LAST_UPDATED_ON = GETDATE() WHERE EMPLOYE_ID='" + GV.sEmployeeNo + "'");
            else
                GV.MSSQL.BAL_ExecuteQuery("UPDATE EMoniter SET COMPANY_LAST_SAVED = GETDATE() , LAST_UPDATED_ON = GETDATE() WHERE EMPLOYE_ID='" + GV.sEmployeeNo + "'");
        }


        public static string ListToQueryString(List<string> lst, string sDateType)
        {
            string sOut = string.Empty;
            foreach (string s in lst)
            {
                if (sDateType == "String")
                {
                    if (sOut.Length == 0)
                        sOut = "'" + s + "'";
                    else
                        sOut += ",'" + s + "'";
                }
                else
                {
                    if (sOut.Length == 0)
                        sOut = s;
                    else
                        sOut += "," + s;
                }
            }
            return sOut.Trim();
        }

        public static string ListToQueryString(List<int> lst, string sDateType)
        {
            string sOut = string.Empty;
            foreach (int i in lst)
            {
                if (sDateType == "String")
                {
                    if (sOut.Length == 0)
                        sOut = "'" + i + "'";
                    else
                        sOut += ",'" + i + "'";
                }
                else
                {
                    if (sOut.Length == 0)
                        sOut = i.ToString();
                    else
                        sOut += "," + i;
                }
            }
            return sOut.Trim();
        }

        public static string ColumnToQString(string sColumnName, DataTable dt, string sDatatype)
        {
            string sReturn = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                if (sDatatype == "Int")
                {
                    if (sReturn.Length > 0)
                        sReturn += "," + dr[sColumnName].ToString();
                    else
                        sReturn = dr[sColumnName].ToString();
                }
                else
                {
                    if (sReturn.Length > 0)
                        sReturn += ",'" + dr[sColumnName].ToString() + "'";
                    else
                        sReturn = "'" + dr[sColumnName].ToString() + "'";
                }
            }

            return sReturn.Trim();
        }




        public static string ProperCase_ProjectSpecific(string sText)
        {
            List<string> lstStringCaps = new List<string> { "TR", "WR" };

            //Case correction for text:Proper Case
            List<string> lstString = new List<string>();
            lstString = sText.Split(' ').ToList();
            string sReturn = string.Empty;
            CultureInfo properCase = System.Threading.Thread.CurrentThread.CurrentCulture;
            TextInfo currentInfo = properCase.TextInfo;
            //return currentInfo.ToTitleCase(currentInfo.ToLower(sText));
            foreach (string s in lstString)
            {
                if (lstStringCaps.Contains(s, StringComparer.OrdinalIgnoreCase))
                    sReturn += " " + s.ToUpper();
                else
                {
                    if (s.Length > 2)
                    {
                        sReturn += " " + currentInfo.ToTitleCase(currentInfo.ToLower(s));
                    }
                    else if (lstString.Contains(s, StringComparer.OrdinalIgnoreCase))
                        sReturn += " " + s.ToLower();
                    else
                        sReturn += " " + s.ToUpper();
                }
            }
            return sReturn.Trim();
        }

        public static string ProperCase(string sText)
        {
            //Case correction for text:Proper Case
            List<string> lstString = new List<string>();
            lstString = sText.Split(' ').ToList();
            string sReturn = string.Empty;
            CultureInfo properCase = System.Threading.Thread.CurrentThread.CurrentCulture;
            TextInfo currentInfo = properCase.TextInfo;
            foreach (string s in lstString)
                sReturn += " " + currentInfo.ToTitleCase(currentInfo.ToLower(s));
            return sReturn.Trim();
        }


        public static bool Email_Check(string sEmail)
        {
            try
            {
                //Email Format Check
                //Regex regex = new Regex(@"^(\w[\w\.\'\-]+)@([\w\-\.]+)((\.(\w){2,})+)$");
                Regex regex = new Regex(@"^(\w[\w\'\-]*(?:\.?[\w\'\-]+){0,5})@(\w[\w\-\.]+)((\.(\w){2,})+)$");
                //Regex regex = new Regex(@"^(\w[\w\'\-]*(?:\.?[\w\'\-]+)*)@(\w[\w\-\.]+)((\.(\w){2,})+)$");
                //Regex regex = new Regex(@"^(\w[\w\'\-]*\.?[\w\'\-]+\.?[\w\'\-]*\.?[\w\'\-]*)@([\w\-\.]+)((\.(\w){2,})+)$");
                Match match = regex.Match(sEmail.Trim());
                if (match.Success)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static DataTable HRIMSWeekID(List<string> lstAgentName, DateTime dtProcessDate)
        {
            string sQuery = string.Empty;
            lstAgentName = lstAgentName.Distinct().ToList();
            string sAgents = GM.ListToQueryString(lstAgentName, "String");
            string sDate = "convert(VARCHAR, Convert(date, '" + dtProcessDate.ToString("yyyy-MM-dd") + "'),112)";

            if (GV.sAccessTo == "TR")// Between Friday to Thursday
            {
                sQuery = @"SELECT RecordID, AgentName FROM Timesheet..MIS_QC WHERE AgentName IN (" + sAgents + ") AND convert(VARCHAR, CalledWeek, 112) BETWEEN ";
                sQuery += "convert(VARCHAR, dateadd(dd, (7 - datepart (dw, " + sDate + ") + 5) % 7," + sDate + ") - 6, 112) AND convert(VARCHAR, dateadd(dd, (7 - datepart (dw, " + sDate + ") + 5) % 7, " + sDate + "), 112);";
            }
            else// Between Monday to Sunday
            {
                sQuery = @"SELECT RecordID, AgentName FROM Timesheet..MIS_QCData WHERE AgentName IN (" + sAgents + ") AND convert(VARCHAR, CalledWeek, 112) BETWEEN ";
                sQuery += "convert(VARCHAR, dateadd(dd, (7 - datepart (dw, " + sDate + ")) % 7, " + sDate + ") - 5, 112) AND convert(VARCHAR, dateadd(dd, (7 - datepart (dw, " + sDate + ")) % 7, " + sDate + ") + 1, 112);";
            }

            DataTable dtMISRecordID = GV.MSSQL.BAL_ExecuteQuery(sQuery);
            return dtMISRecordID;
        }


        public static Byte[] imgToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            byte[] imgArray = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return imgArray;
        }

        public static string NumberSuffix(string sNumber)
        {
            if (sNumber.Trim().Length > 0)
            {
                if (Convert.ToInt32(sNumber) > 10 && Convert.ToInt32(sNumber) < 21)
                    return sNumber + "th";
                else
                {
                    string sLastNumber = sNumber.ToString().Last().ToString();
                    switch (sLastNumber)
                    {
                        case "1":
                            return sNumber + "st";
                        case "2":
                            return sNumber + "nd";
                        case "3":
                            return sNumber + "rd";
                        default:
                            return sNumber + "th";
                    }
                }
            }
            else
                return string.Empty;
        }


        public static string LoadRTF(DataTable dtContact, DataTable dtCompany, int iIndex)
        {
            string sRTF = string.Empty;
            if (GV.sCallScriptPath.Length > 0)
            {
                RichTextBox txtrtf = new RichTextBox();
                txtrtf.LoadFile(GV.sCallScriptPath);
                sRTF = txtrtf.Rtf;
                string sFirstName = string.Empty;
                string sLastName = string.Empty;
                string sJobTitle = string.Empty;
                string sEmail = string.Empty;
                int i = 0;
                while (true)
                {
                    try
                    {
                        sFirstName = dtContact.Rows[i]["FIRST_NAME"].ToString();
                        sLastName = dtContact.Rows[i]["LAST_NAME"].ToString();
                        sJobTitle = dtContact.Rows[i]["JOB_TITLE"].ToString();
                        sEmail = dtContact.Rows[i]["CONTACT_EMAIL"].ToString();
                    }
                    catch (Exception ex)
                    {
                        sFirstName = string.Empty;
                        sLastName = string.Empty;
                        sJobTitle = string.Empty;
                        sEmail = string.Empty;
                    }
                    if (sFirstName.Length > 0 || sLastName.Length > 0)
                        sRTF = sRTF.Replace("<Name" + i + ">", sFirstName + " " + sLastName);
                    else
                        sRTF = sRTF.Replace("Name: <Name" + i + ">", string.Empty);

                    if (sJobTitle.Length > 0)
                        sRTF = sRTF.Replace("<Jobtitle" + i + ">", sJobTitle);
                    else
                        sRTF = sRTF.Replace("Job Title: <Jobtitle" + i + ">", string.Empty);

                    if (sEmail.Length > 0)
                        sRTF = sRTF.Replace("<Email" + i + ">", sEmail);
                    else
                        sRTF = sRTF.Replace("Email: <Email" + i + ">", string.Empty);
                    //sContact += "Name: " + dr["TITLE"]+" "+ dr["FIRST_NAME"] + " " + dr["LAST_NAME"] + Environment.NewLine;
                    //sContact += "Job Title: " + dr["JOB_TITLE"] + Environment.NewLine;
                    //sContact += "Email: " + dr["CONTACT_EMAIL"] + Environment.NewLine + Environment.NewLine;
                    i++;
                    if (i == 2)
                        break;
                }
                sRTF = sRTF.Replace("<AgentName>", GV.sEmployeeActualName);
                sRTF = sRTF.Replace("<CompanyName>", dtCompany.Rows[iIndex]["COMPANY_NAME"].ToString());
                sRTF = sRTF.Replace("<Address1>", dtCompany.Rows[iIndex]["ADDRESS_1"].ToString());
                sRTF = sRTF.Replace("<Address2>", dtCompany.Rows[iIndex]["ADDRESS_2"].ToString());
                sRTF = sRTF.Replace("<Address3>", dtCompany.Rows[iIndex]["ADDRESS_3"].ToString());
                sRTF = sRTF.Replace("<Address4>", dtCompany.Rows[iIndex]["ADDRESS_4"].ToString());
                sRTF = sRTF.Replace("<City>", dtCompany.Rows[iIndex]["CITY"].ToString());
                sRTF = sRTF.Replace("<County>", dtCompany.Rows[iIndex]["COUNTY"].ToString());
                sRTF = sRTF.Replace("<PostCode>", dtCompany.Rows[iIndex]["POST_CODE"].ToString());
                sRTF = sRTF.Replace("<Country>", dtCompany.Rows[iIndex]["COUNTRY"].ToString());
            }
            return sRTF;
        }


        public static void OpenHelp()
        {
            try
            {
                bool IsFormOpen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "frmHelp")
                    {
                        IsFormOpen = true;
                        f.Focus();
                        break;
                    }
                }
                if (!IsFormOpen)
                {
                    frmHelp objfrmHelp = new frmHelp();
                    objfrmHelp.ShowDialog();
                }
            }
            catch (Exception ex)
            {
            }
        }


        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }


        public static bool IsNetWorkDown()
        {
            foreach (System.Net.NetworkInformation.NetworkInterface ni in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Ethernet)
                {
                    foreach (System.Net.NetworkInformation.UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && ip.Address.ToString().StartsWith("172.27."))
                        {
                            if (ni.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                                return false;
                            else
                                return true;
                        }
                    }
                }
            }
            return true;
        }


        public static string IP()
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


        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        public static void HangUp()
        {
            try
            {
                #region Old
                //Process[] pName = Process.GetProcessesByName("IDialer");
                //if (pName.Length > 0)
                //Process.Start(GV.sSendKeyBinaryPath, "1 5 \"Interaction Client\" \"%H");
                //else
                //{
                //    FormCollection fCollection = Application.OpenForms;
                //    foreach (Form f in fCollection)
                //    {
                //        if (f.Name == "FrmContactsUpdate")
                //        {
                //            ToastNotification.Show(f, "Interaction Client is not Active", eToastPosition.TopRight);
                //            break;
                //        }
                //    }
                //} 
                #endregion

                if (GV.sDialerType == "iSystem")
                {
                    //wbDial.Navigate(New Uri("https://172.27.141.1/index.php?menu=agent_console&rawmode=yes&action=hangup"), False)
                    System.IO.StreamWriter objWriter = new System.IO.StreamWriter("C:\\temp\\dial.txt");
                    objWriter.Write("0");
                    objWriter.Close();
                }
                else if (GV.sDialerType == "X-Lite")
                {
                    Process[] xLiteProcess = Process.GetProcessesByName("X-Lite");
                    //Process[] CMProcess = Process.GetProcessesByName("Campaign Manager");
                    if (xLiteProcess.Length > 0)
                    {
                        if (File.Exists(@"C:\Program Files\CounterPath\X-Lite\x-lite.exe"))
                        {
                            Process.Start(@"C:\Program Files\CounterPath\X-Lite\x-lite.exe");//Activate the dialer if minimized
                            const int swRestore = 9;
                            // get the window handle
                            IntPtr hWnd = xLiteProcess[0].MainWindowHandle;
                            // if iconic, we need to restore the window
                            if (IsIconic(hWnd))
                            {
                                ShowWindowAsync(hWnd, swRestore);
                            }
                            // bring it to the foreground
                            SetForegroundWindow(hWnd);
                            // x-Lite requires Ctrl+h to hangup a call.
                            Process.Start(GV.sSendKeyBinaryPath, "1 5 \"X-Lite\" \"^h\"");
                            //ShowWindowAsync(CMProcess[0].MainWindowHandle, swRestore);                                                                
                        }
                    }                    
                }
                else if (GV.sDialerType == "Vortex")
                {
                    GV.VorteX.Hangup();
                }

                GM.Moniter("Post Call");
            }
            catch (Exception ex)
            {
                // Ignore error.
            }
        }


        public static bool Dial(string sTelephoneNumber, string sMasterID)
        {
            //return true;

            #region Old
            // Remove all the special characters and keep only the numbers.

            //try
            //{
            //    // Display phone number being dialled in the status bar.
            //    Process[] pName = Process.GetProcessesByName("IDialer");
            //    if (pName.Length > 0)
            //        Process.Start(GV.sSendKeyBinaryPath, "1 5 \"Interaction Client\" \"%n" + sTelephoneNumber + "%m\"");
            //    else
            //    {
            //        FormCollection fCollection = Application.OpenForms;
            //        foreach (Form f in fCollection)
            //        {
            //            if (f.Name == "FrmContactsUpdate")
            //            {
            //                ToastNotification.Show(f, "Please Login Interaction Client", eToastPosition.TopRight);
            //                break;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //// Ignore error.
            //}

            // Do not continue if the length of the telephone number if less then 7 digits.  Its probably an invalid number.
            //if (TelNo.Length < 7)
            //{
            //    lblMessage.Text = "Invalid number";
            //    return;
            //} 
            #endregion

            try
            {
                if (GV.sDialerType == "iSystem")
                {
                    if (!System.IO.Directory.Exists("C:\\temp"))
                        System.IO.Directory.CreateDirectory("C:\\temp");
                    System.IO.StreamWriter objWriter = new System.IO.StreamWriter("C:\\temp\\dial.txt");
                    objWriter.Write(sTelephoneNumber);
                    objWriter.Close();
                    return true;
                }
                else if (GV.sDialerType == "X-Lite")
                {
                    Process[] xLiteProcess = Process.GetProcessesByName("X-Lite");
                    if (xLiteProcess.Length > 0)
                    {
                        Process.Start(xLiteProcess[0].MainModule.FileName, "-dial=sip:" + sTelephoneNumber);
                        //Process.Start("C:\\Program Files\\CounterPath\\X-Lite\\X-LITE.EXE", "-dial=sip:" + sTelephoneNumber);
                        return true;
                    }
                    else
                        return false;
                }
                else if (GV.sDialerType == "Vortex")
                {
                    string sState = GV.VorteX.Call(sTelephoneNumber, GV.sProjectID, "GCC", GV.sEmployeeNo, false, Convert.ToInt32(sMasterID));
                    return (sState.Length == 0);
                }

                return false;
            }
            catch (Exception ex)
            {
                // ignore any error of release mode
                return false;
            }
        }



        public static string MailSettings(string sField)
        {
            try
            {
                if (GV.dtMailConfig != null && GV.dtMailConfig.Rows.Count > 0)
                { }
                else
                    GV.dtMailConfig = GV.MYSQL.BAL_FetchTableMySQL("c_picklists", "PicklistCategory = 'MailConfig'");//Load only for first Time

                if (GV.dtMailConfig.Select("PicklistField = '" + sField + "'").Length > 0)
                    return GV.dtMailConfig.Select("PicklistField = '" + sField + "'")[0]["PicklistValue"].ToString();
                else
                    return string.Empty;
            }
            catch (Exception ex)
            { return string.Empty; }

            //foreach (DataRow drMail in dtMailConfig.Rows)
            //{
            //    switch (drMail["PicklistField"].ToString().ToUpper())
            //    {
            //        case "USERNAME":
            //            GV.lstMailSettings[0] = drMail["PicklistValue"].ToString().Trim();
            //            break;
            //        case "PASSWORD":
            //            GV.lstMailSettings[1] = drMail["PicklistValue"].ToString().Trim();
            //            break;
            //        case "SUBJECT":
            //            GV.lstMailSettings[2] = drMail["PicklistValue"].ToString().Trim();
            //            break;
            //        case "FROM":
            //            GV.lstMailSettings[3] = drMail["PicklistValue"].ToString().Trim();
            //            GV.lstMailSettings[4] = drMail["remarks"].ToString().Trim();//Sender Display Name
            //            break;
            //        case "TO":
            //            GV.lstMailSettings[5] = drMail["PicklistValue"].ToString().Trim();
            //            break;
            //        case "CC":
            //            GV.lstMailSettings[6] = drMail["PicklistValue"].ToString().Trim();
            //            break;
            //        case "OUTGOINGHOST":
            //            GV.lstMailSettings[7] = drMail["PicklistValue"].ToString().Trim();
            //            break;
            //        case "ATTACHSCREENSHOT":
            //            GV.lstMailSettings[8] = drMail["PicklistValue"].ToString().Trim();
            //            break;
            //        case "MAILTIMEOUT":
            //            GV.lstMailSettings[9] = drMail["PicklistValue"].ToString().Trim();
            //            break;
            //        case "PORT":
            //            GV.lstMailSettings[10] = drMail["PicklistValue"].ToString().Trim();
            //            break;
            //    }
            //}
        }

        public static bool Web_Check(string sWeb)
        {
            try
            {

                // return (Uri.IsWellFormedUriString(sWeb, UriKind.RelativeOrAbsolute));
                ////Website Format Check
                Regex regex = new Regex(@"www\d*\.[\w\-]+\.[a-zA-Z]{2,3}(?:\.?[a-zA-Z]{0,2})(?:[\w\.?=%&\-@/$,+;#]*)");
                Match match = regex.Match(sWeb);
                if (match.Success)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        public static string IsTimeLoggerHasProject()
        {
            if (GV.HasAdminPermission)
                return string.Empty;

            string sTimeLoggerMessage = string.Empty;
            if (GV.sUserType == "Agent")
            {

                Process[] runningProcesses = Process.GetProcesses();
                var currentSessionID = Process.GetCurrentProcess().SessionId;
                Process[] pName = (from c in runningProcesses where c.ProcessName == "TimeSheetOnline" && c.SessionId == currentSessionID select c).ToArray();

                //Process[] pName = Process.GetProcessesByName("TimeSheetOnline");
                if (pName.Length == 0)
                    return "Please Login your Time Logger to get Records";
                else
                {
                    RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software\\VB and VBA Program Settings\\Merit\\Time Logger", true); //Get Current Registry Data//
                    if (reg == null)
                        return "Please Login your Time Logger to get Records";
                    else
                    {
                        if (reg.GetValue("EmployeeNo").ToString().Trim().Length == 0)
                            return "Please Login your Time Logger to get Records";

                        if (GV.sEmployeeNo.ToUpper() != reg.GetValue("EmployeeNo").ToString().ToUpper())
                            return "User mismatch detected in Time Logger. Please Reload Campaign Manager";
                        else if (GV.sProjectName.ToUpper() != reg.GetValue("ProjectName").ToString().ToUpper())
                            return "Invalid project detected in Time Logger.";// +Environment.NewLine + "Switch to correct project in Time Logger or Reload Campaign Manager";
                    }
                }
                return string.Empty;
            }
            else//Do not check for Admins, Managers and QC
                return string.Empty;
        }

        public static string RemoveQuote(string sText)
        {
            if (sText.Length > 0)
                return sText.Replace("'", "''").Trim();
            else
                return string.Empty;
        }

        public static string RemoveEndBackSlash(string sText)
        {
            if (sText.Length > 0)
            {
                if (sText.EndsWith("\\"))
                {
                    while (sText.EndsWith("\\"))
                        sText = sText.Remove(sText.Length - 1);

                    return sText;
                }
                return sText;
            }
            return string.Empty;
        }

        public static string HandleBackSlash(string sText)
        {
            if (sText.Length > 0)
            {
                if (sText.EndsWith(@"\"))
                    return sText + @"\";
                return sText;
            }
            return string.Empty;
        }

        //public static void Log(string sAction, string sDescription1, string sDescription2, string sDescription3, string sDescription4, string sDescription5, string sDescription6)
        public static void Log(string sAction, string sTable_Name = "", string sField = "", string sReferenceID = "", string sOldValue = "", string sNewValue = "", string sDescription1 = "", string sDescription2 = "", string sDescription3 = "", string sDescription4 = "", string sDescription5 = "", string sDescription6 = "")
        {
            GV.MYSQL.BAL_ExecuteNonReturnQueryMySQL("INSERT INTO c_log (WHO, `WHEN`, SYSTEMNAME, USERTYPE, RESEARCHTYPE, REFERENCE_ID, ACTION, TABLE_NAME, FIELD, OLD_VALUE, NEW_VALUE, DESCRIPTION1, DESCRIPTION2, DESCRIPTION3, DESCRIPTION4, DESCRIPTION5, DESCRIPTION6, PROJECTID, SOFTWARE_VERSION, SESSIONID) VALUES ('" + GV.sEmployeeName + "', NOW(), '" + Environment.MachineName + "', '" + GV.sUserType + "', '" + GV.sAccessTo + "', '" + sReferenceID + "', '" + sAction + "', '" + sTable_Name + "', '" + sField + "', '" + sOldValue + "', '" + sNewValue + "', '" + sDescription1 + "', '" + sDescription2 + "', '" + sDescription3 + "', '" + sDescription4 + "', '" + sDescription5 + "', '" + sDescription6 + "', '" + GV.sProjectID + "', '" + GV.sSoftwareVersion + "' , '" + GV.sSessionID + "')");

        }



        public static void Logging(DataTable dtNewTable, DataTable dtOldTable, string sTableName, string sIDColumn)
        {
            try
            {
                if (dtNewTable != null && dtOldTable != null)
                {

                    DataTable dtC_Log = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM c_log WHERE 1=0;");
                    foreach (DataRow drOldTable in dtOldTable.Rows)
                    {
                        string sID = drOldTable[sIDColumn].ToString();
                        DataRow[] drrNew = dtNewTable.Select(sIDColumn + " = " + sID);
                        if (drrNew.Length > 0)
                        {
                            foreach (DataColumn dcColumn in drrNew[0].Table.Columns)
                            {
                                string sColumnName = dcColumn.ColumnName;
                                if (drOldTable[sColumnName].ToString() != drrNew[0][sColumnName].ToString())
                                {
                                    //GM.Log("DataChanged", sTableName, sColumnName, drOldTable[sIDColumn].ToString(), drOldTable[sColumnName].ToString(), drrNew[0][sColumnName].ToString());
                                    DataRow drNewC_Log = dtC_Log.NewRow();
                                    drNewC_Log["WHO"] = GV.sEmployeeName;
                                    drNewC_Log["WHEN"] = GM.GetDateTime();
                                    drNewC_Log["SYSTEMNAME"] = Environment.MachineName;
                                    drNewC_Log["USERTYPE"] = GV.sUserType;
                                    drNewC_Log["RESEARCHTYPE"] = GV.sAccessTo;
                                    drNewC_Log["REFERENCE_ID"] = drOldTable[sIDColumn].ToString();
                                    drNewC_Log["ACTION"] = "DataChanged";
                                    drNewC_Log["TABLE_NAME"] = sTableName;
                                    drNewC_Log["FIELD"] = sColumnName;
                                    drNewC_Log["OLD_VALUE"] = drOldTable[sColumnName].ToString();
                                    drNewC_Log["NEW_VALUE"] = drrNew[0][sColumnName].ToString();
                                    drNewC_Log["PROJECTID"] = GV.sProjectID;
                                    drNewC_Log["SOFTWARE_VERSION"] = GV.sSoftwareVersion;
                                    drNewC_Log["SESSIONID"] = GV.sSessionID;
                                    dtC_Log.Rows.Add(drNewC_Log);
                                }
                            }
                        }
                        else
                        {
                            string sRemovedData = string.Empty;
                            DataRow drNewC_Log = dtC_Log.NewRow();
                            foreach (DataColumn dcOld in drOldTable.Table.Columns)
                                sRemovedData += "[" + dcOld.ColumnName + " : " + drOldTable[dcOld.ColumnName].ToString() + "]";

                            drNewC_Log["WHO"] = GV.sEmployeeName;
                            drNewC_Log["WHEN"] = GM.GetDateTime();
                            drNewC_Log["SYSTEMNAME"] = Environment.MachineName;
                            drNewC_Log["USERTYPE"] = GV.sUserType;
                            drNewC_Log["RESEARCHTYPE"] = GV.sAccessTo;
                            drNewC_Log["REFERENCE_ID"] = drOldTable[sIDColumn].ToString();
                            drNewC_Log["ACTION"] = "DataRemoved";
                            drNewC_Log["TABLE_NAME"] = sTableName;
                            drNewC_Log["OLD_VALUE"] = sRemovedData;
                            drNewC_Log["PROJECTID"] = GV.sProjectID;
                            drNewC_Log["SOFTWARE_VERSION"] = GV.sSoftwareVersion;
                            drNewC_Log["SESSIONID"] = GV.sSessionID;
                            dtC_Log.Rows.Add(drNewC_Log);

                            //GM.Log("DataRemoved", sTableName, string.Empty, drOldTable[sIDColumn].ToString(), sRemovedData);
                        }
                    }

                    if (dtC_Log.GetChanges(DataRowState.Added) != null && dtC_Log.GetChanges(DataRowState.Added).Rows.Count > 0)
                        GV.MYSQL.BAL_SaveToTableMySQL(dtC_Log, "c_log", "New", false);
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);
            }
        }


        public static bool FileInUse(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                    return !(fs.CanWrite);
                return false;
            }
            catch (IOException ex)
            {
                return true;
            }
        }

        public static void WriteLog(string sPath, string Content, bool Append)
        {
            using (StreamWriter sWrite = new StreamWriter(sPath, Append))
            {
                sWrite.WriteLine(Content);
                sWrite.Close();
            }
        }

        public static void Error_Log(MethodBase mBase, Exception ex, bool IsHandeled, bool IsErrorAllowed, string sAdditionalInfo = "")
        {


            //System.OperatingSystem osInfo = System.Environment.OSVersion;
            //string sArchitecture = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            //switch (osInfo.Version.Major)
            //{
            //    case 3:
            //        Console.WriteLine("Windows NT 3.51");
            //        break;
            //    case 4:
            //        Console.WriteLine("Windows NT 4.0");
            //        break;
            //    case 5:
            //        if (osInfo.Version.Minor == 0)
            //            Console.WriteLine("Windows 2000");
            //        else
            //            Console.WriteLine("Windows XP");
            //        break;
            //    case 6:
            //        if (osInfo.Version.Minor == 0)
            //            Console.WriteLine("Windows Vista");
            //        else if (osInfo.Version.Minor == 1)
            //            Console.WriteLine("Windows 7");
            //        else if (osInfo.Version.Minor == 2)
            //            Console.WriteLine("Windows 8");
            //        break;
            //}

            //if (ex.Message.Contains("Invoke or BeginInvoke"))
            //    return;

            string sExceptionImageID = "Ex" + GV.IP.Replace(".", string.Empty).Reverse() + GM.GetDateTime().ToString("yyMMddHHmmssff");
            string sShotPath = @"\\172.27.137.182\Campaign Manager\Exceptions\Shots\\" + sExceptionImageID + ".jpeg";

            if (GM.MailSettings("AttachScreenShot").ToUpper() == "YES")
            {
                try
                {
                    using (Bitmap bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb))
                    {
                        using (Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot))
                        {

                            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                            bmpScreenshot.Save(sShotPath, ImageFormat.Jpeg);
                        }
                    }
                }
                catch (Exception exImage) { sExceptionImageID = string.Empty; sShotPath = string.Empty; }
            }
            else
                sExceptionImageID = string.Empty;

            string sRowNumber = string.Empty;
            string sColNumber = string.Empty;
            string sMethod = string.Empty;
            //StackTrace trace = new StackTrace(ex, true);
            //StackFrame frame = trace.GetFrame(trace.FrameCount - 1);

            StackTrace trace = new StackTrace(ex, true);
            StackFrame sfFrame = null;
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
            sWriteError += "Error Date : " + GM.GetDateTime() + Environment.NewLine + Environment.NewLine;
            sWriteError += "ProjectName : " + GV.sProjectName + Environment.NewLine;
            sWriteError += "ProjectID : " + GV.sProjectID + Environment.NewLine;
            sWriteError += "ImageID : " + sExceptionImageID + Environment.NewLine;
            sWriteError += "Message : " + ex.Message + Environment.NewLine + Environment.NewLine;
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

            GV.sErrorCaptureName = GV.sCurrentCaptureName;

            try
            {
                if (GV.sEmployeeName.Length > 0)
                {
                    WriteLog(@"\\172.27.137.182\Campaign Manager\Exceptions\" + GV.sEmployeeName + ".txt", sWriteError, true);
                    //StreamWriter sWrite = new StreamWriter(@"\\172.27.137.182\Campaign Manager\Exceptions\" + GV.sEmployeeName + ".txt", true);
                    //sWrite.WriteLine(sWriteError);
                    //sWrite.Close();
                }
                else
                {
                    WriteLog(@"\\172.27.137.182\Campaign Manager\Exceptions\Unknown.txt", sWriteError, true);
                    //StreamWriter sWrite = new StreamWriter(@"\\172.27.137.182\Campaign Manager\Exceptions\Unknown.txt", true);
                    //sWrite.WriteLine(sWriteError);
                    //sWrite.Close();
                }
            }
            catch (Exception ex11) { }


            string sExceptionHandeled = string.Empty;
            if (IsHandeled)
                sExceptionHandeled = "Yes";
            else
                sExceptionHandeled = "No";
            //BAL.BAL_GlobalMySQL objBAL_Global = new BAL.BAL_GlobalMySQL();
            DataTable dtError = null;
            string sOS = JCS.OSVersionInfo.VersionString + " " + JCS.OSVersionInfo.Edition + " " + JCS.OSVersionInfo.OSBits;

            //Microsoft.VisualBasic.Devices.ComputerInfo cComputerInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();                    

            string sTotalPhysicalMemory = string.Empty;
            string sTotalAvailMemory = string.Empty;
            try
            {

                using (ManagementObjectSearcher mPhysicalMemory = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory"))
                {
                    UInt64 Capacity = 0;
                    foreach (ManagementObject mPhySicalMem in mPhysicalMemory.Get())
                        Capacity += Convert.ToUInt64(mPhySicalMem.Properties["Capacity"].Value);

                    sTotalPhysicalMemory = (Capacity / 1048576).ToString() + "MB";
                }

                using (ManagementObjectSearcher mAvailMemory = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                {
                    foreach (ManagementObject mAvailMem in mAvailMemory.Get())
                        sTotalAvailMemory = Convert.ToInt64(mAvailMem["FreePhysicalMemory"]) / 1024 + "MB";
                }

                string sMem = sTotalAvailMemory + " / " + sTotalPhysicalMemory;
                List<Process> processlist = Process.GetProcesses().OrderByDescending(x => x.WorkingSet64).ToList();
                string sProcessesTable = "<table border = 1>";
                string sProcesses = string.Empty;
                foreach (Process theprocess in processlist)
                {
                    sProcesses += theprocess.ProcessName + "\t" + theprocess.WorkingSet64 / (1048576) + Environment.NewLine;
                    sProcessesTable += "<tr><td>" + theprocess.ProcessName + "</td><td>" + theprocess.WorkingSet64 / (1048576) + " MB</td></tr>";
                }
                sProcessesTable += "</table>";
                if (GM.MailSettings("SendEmail").ToUpper() == "YES")
                {
                    dtError = GV.MYSQL.BAL_FetchTableMySQL("error_log", "ProjectID = '" + GV.sProjectID + "' AND ErrorTrace ='" + Regex.Replace(ex.StackTrace.Trim(), @"\\", @"\\") + "' AND CONVERT(Error_Date,date) = CONVERT(NOW(),date)");
                    if (dtError == null || dtError.Rows.Count == 0)//Send Email
                    {
                        using (MailMessage mMessage = new MailMessage())
                        {
                            using (SmtpClient cClient = new SmtpClient())
                            {
                                if (File.Exists(sShotPath))
                                    mMessage.Attachments.Add(new Attachment(sShotPath));
                                //mMessage = new MailMessage("thangaprakash.manivannan@meritgroup.co.uk", "thangaprakash.manivannan@meritgroup.co.uk");
                                mMessage.From = new MailAddress(GM.MailSettings("From"), GM.MailSettings("DisplayName"));

                                List<string> lstTo = GM.MailSettings("To").Split(';').ToList();
                                foreach (string sTo in lstTo)
                                {
                                    if (sTo.Length > 0 && GM.Email_Check(sTo))
                                        mMessage.To.Add(new MailAddress(sTo));
                                }

                                List<string> lstCC = GM.MailSettings("CC").Split(';').ToList();
                                foreach (string sCC in lstCC)
                                {
                                    if (sCC.Length > 0 && GM.Email_Check(sCC))
                                        mMessage.CC.Add(new MailAddress(sCC));
                                }


                                if (GM.MailSettings("Port").Length > 0 &&
                                    Microsoft.VisualBasic.Information.IsNumeric(GM.MailSettings("Port")))
                                    cClient.Port = Convert.ToInt32(GM.MailSettings("Port"));
                                else
                                    cClient.Port = 2525;

                                cClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                                cClient.UseDefaultCredentials = false;
                                //cClient.EnableSsl = true;
                                cClient.Host = GM.MailSettings("OutgoingHost");
                                cClient.Credentials = new System.Net.NetworkCredential(GM.MailSettings("UserName"), GM.MailSettings("Password"));
                                cClient.Timeout = 10000;
                                mMessage.Subject = GM.MailSettings("Subject");
                                mMessage.Priority = MailPriority.High;
                                mMessage.IsBodyHtml = true;

                                mMessage.Body = "<h3>CM Exception Notifier</h3><br>";
                                mMessage.Body += "<h4>Exception Details</h4>";
                                mMessage.Body += "<table border = 1><tr><td>Message</td><td>" + ex.Message + "</td></tr>";
                                mMessage.Body += "<tr><td>Target</td><td>" + ex.TargetSite.ToString().Trim() + "</td></tr>";
                                mMessage.Body += "<tr><td>Stack Trace</td><td>" + ex.StackTrace.Trim() + "</td></tr>";

                                if (sAdditionalInfo.Length > 0)
                                    mMessage.Body += "<tr><td>Info</td><td>" + sAdditionalInfo + "</td></tr>";

                                if (sRowNumber.Length > 0)
                                    mMessage.Body += "<tr><td>Row</td><td>" + sRowNumber + ":" + sColNumber + "</td></tr>";

                                if (IsHandeled)
                                    mMessage.Body += "<tr><td>Handled</td><td>Yes</td></tr></table><br/><br/>";
                                else
                                    mMessage.Body +=
                                        "<font color = 'red'><tr><td>Handled</td><td>No</td></tr></font></table><br/><br/>";
                                mMessage.Body += "<h4>Project Details</h4>";


                                mMessage.Body += "<table border = 1><tr><td>Agent</td><td>" + GV.sEmployeeName + "</td></tr>";
                                mMessage.Body += "<tr><td>Project Name</td><td>" + GV.sProjectName + "</td></tr>";
                                mMessage.Body += "<tr><td>User Permission</td><td>" + GV.sUserType + "</td></tr>";
                                mMessage.Body += "<tr><td>Research Type</td><td>" + GV.sAccessTo + "</td></tr></table><br/><br/>";


                                mMessage.Body += "<h4>System Details</h4>";
                                mMessage.Body += "<table border = 1><tr><td>Operating System</td><td>" + sOS + "</td></tr>";
                                mMessage.Body += "<tr><td>Machine Name</td><td>" + Environment.MachineName + "</td></tr>";
                                mMessage.Body += "<tr><td>Login</td><td>" + Environment.UserName + "</td></tr>";
                                mMessage.Body += "<tr><td>Version</td><td>" + GV.sSoftwareVersion + "</td></tr>";
                                mMessage.Body += "<tr><td>Total Physical Memory</td><td>" + sTotalPhysicalMemory + "</td></tr>";
                                mMessage.Body += "<tr><td>Total Available Memory</td><td>" + sTotalAvailMemory + "</td></tr></table><br/><br/>";
                                mMessage.Body += "<h4>Process List</h4>";
                                mMessage.Body += sProcessesTable;
                                cClient.Send(mMessage);
                            }
                        }
                    }
                }

                if (GM.MailSettings("LogToDB").ToUpper() == "YES")
                {
                    if (dtError == null)
                        dtError = GV.MYSQL.BAL_FetchTableMySQL("error_log", "1=0");

                    DataRow drNewError = dtError.NewRow();
                    drNewError["ProjectID"] = GV.sProjectID;
                    drNewError["SessionID"] = GV.sSessionID;
                    drNewError["CompanySessionID"] = GV.sCompanySessionID;
                    drNewError["ImageID"] = sExceptionImageID;
                    drNewError["CaptureID"] = GV.sErrorCaptureName;
                    drNewError["UserType"] = GV.sUserType;
                    drNewError["UserAccess"] = GV.sAccessTo;
                    drNewError["Error_Date"] = GM.GetDateTime();
                    drNewError["Agent"] = GV.sEmployeeName;
                    drNewError["Target_Site"] = ex.TargetSite.ToString().Trim();
                    drNewError["ErrorTrace"] = ex.StackTrace.Trim();
                    drNewError["StackTrace"] = Environment.StackTrace.Trim();
                    drNewError["OS"] = sOS;
                    drNewError["NTLogin"] = Environment.UserName;
                    drNewError["Machine"] = Environment.MachineName;
                    drNewError["Additional_Info"] = sAdditionalInfo;
                    drNewError["Line_Number"] = sRowNumber + ":" + sColNumber;
                    drNewError["Message"] = ex.Message.Trim();
                    drNewError["Processes"] = sProcesses;
                    drNewError["Memory"] = sMem;
                    drNewError["Handled"] = sExceptionHandeled;
                    drNewError["SoftwareVersion"] = GV.sSoftwareVersion;
                    dtError.Rows.Add(drNewError);
                    GV.MYSQL.BAL_SaveToTableMySQL(dtError.GetChanges(DataRowState.Added), "error_log", "New", false);
                }
            }
            catch (Exception ex1)
            {
                // MessageBoxEx.Show("Error Log");
            }
            finally
            {

                //System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Exception)).Serialize(Console.Out, new Exception());
                //System.IO.MemoryStream s = new System.IO.MemoryStream();
                //writer.Serialize(s, ex);  //Exception class not serilaizable
                if (dtError != null)
                {
                    dtError.Dispose();
                    dtError = null;
                }
            }

            if (IsErrorAllowed)
            {
                MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //ErrorHandle(ex.Message.ToLower());
            }
        }

        public static string RemoveNonXMLChars(string inString)
        {
            if (inString == null) return null;
            StringBuilder newString = new StringBuilder();
            char ch;
            for (int i = 0; i < inString.Length; i++)
            {
                ch = inString[i];
                // remove any characters outside the valid UTF-8 range as well as all control characters
                // except tabs and new lines
                //if ((ch < 0x00FD && ch > 0x001F) || ch == '\t' || ch == '\n' || ch == '\r')
                //if using .NET version prior to 4, use above logic
                if (XmlConvert.IsXmlChar(ch)) //this method is new in .NET 4
                    newString.Append(ch);
            }
            return newString.ToString();
        }

        public static DataTable IsPendingRecordsExist()
        {
            DataTable dt = new DataTable();
            if (GV.sUserType == "Admin" || GV.Override_UserAccess)
                dt = GV.MYSQL.BAL_FetchTableMySQL(GV.sCompanyTable, GV.sAccessTo + "_AGENTNAME = 'Current_" + GV.sEmployeeName + "'");
            else if (GV.Override_UserAccess)
                dt = GV.MYSQL.BAL_FetchTableMySQL(GV.sCompanyTable, GV.sAccessTo + "_AGENTNAME = 'Current_" + GV.sEmployeeName + "' AND FLAG IN ('TR','WR')");
            else
                dt = GV.MYSQL.BAL_FetchTableMySQL(GV.sCompanyTable, GV.sAccessTo + "_AGENTNAME = 'Current_" + GV.sEmployeeName + "' AND FLAG = '" + GV.sAccessTo + "'");
            return dt;
        }



        public static void OpenContactUpdate(string sID, bool IsNewCompany, bool IsOpenbyID, Form frmCurrent, frmCompanyList Companylist)
        {
            try
            {
                #region If contactUpdate already opened ?

                GV.PerformanceWatch = Stopwatch.StartNew();
                GV.sPerformance = "";

                StringAppend("Fetch Record");
                bool IsFormOpen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "FrmContactsUpdate")
                    {
                        IsFormOpen = true;
                        f.Focus();
                        if (DialogResult.Yes == MessageBoxEx.Show("Changes made in this screen will be lost." + Environment.NewLine + "Do you want to continue ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                        {
                            f.Close();
                            if (f.IsDisposed)
                                IsFormOpen = false;
                            else
                                return;
                        }
                        else
                            return;//
                        break;
                    }
                }
                #endregion

                if (!IsFormOpen)
                {
                    if (Companylist == null)
                    {
                        foreach (Form f in Application.OpenForms)
                        {
                            if (f.Name == "frmCompanyList")
                            {
                                Companylist = (frmCompanyList)f;
                                break;
                            }
                        }
                    }

                    if (GV.sUserType == "Agent")
                    {
                        #region Agent Open
                        DataTable dtRecordCheck = new DataTable();
                        if (IsOpenbyID)
                        {
                            #region Pending Check
                            DataTable dtPendingCheck = GM.IsPendingRecordsExist();
                            if (dtPendingCheck.Rows.Count > 0)
                            {
                                if (dtPendingCheck.Rows[0]["Master_ID"].ToString() != sID)
                                    MessageBoxEx.Show("Some contact(s) not saved properly. Opening them now.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                                FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(null, frmCurrent.MdiParent, "ListOpen", IsNewCompany, Companylist);
                                objfrmContactsUpdate.Show();
                                return;
                            }
                            #endregion
                            dtRecordCheck = GV.MYSQL.BAL_FetchTableMySQL(GV.sCompanyTable, "MASTER_ID = " + sID);
                        }
                        else
                        {
                            if (GV.Override_UserAccess)
                                dtRecordCheck = GV.MYSQL.BAL_FetchTableMySQL(GV.sCompanyTable, GV.sAccessTo + "_AGENTNAME = 'Current_" + GV.sEmployeeName + "' AND FLAG IN ('TR','WR')");
                            else
                                dtRecordCheck = GV.MYSQL.BAL_FetchTableMySQL(GV.sCompanyTable, GV.sAccessTo + "_AGENTNAME = 'Current_" + GV.sEmployeeName + "' AND FLAG = '" + GV.sAccessTo + "'");
                        }

                        if (dtRecordCheck.Rows.Count > 0)
                        {
                            if (dtRecordCheck.Rows[0]["FLAG"].ToString().Length > 0)
                            {
                                if (!GV.Override_UserAccess)
                                {
                                    if (dtRecordCheck.Rows[0]["FLAG"].ToString().ToUpper() != GV.sAccessTo)
                                    {
                                        ToastNotification.Show(frmCurrent.MdiParent, "This record is in " + GV.sOppositAccess + " bin.", eToastPosition.TopRight);
                                        return;
                                    }
                                }

                                if (dtRecordCheck.Rows[0]["Scrape_Status"].ToString().ToUpper() == "1")
                                {
                                    ToastNotification.Show(frmCurrent.MdiParent, "This company can't be opened.<br/>Company queued for scrapping.", eToastPosition.TopRight);
                                    return;
                                }
                            }
                            else
                            {
                                ToastNotification.Show(frmCurrent.MdiParent, "This record is not in your bin.", eToastPosition.TopRight);
                                return;
                            }

                            if (dtRecordCheck.Rows[0]["TR_AGENTNAME"].ToString().ToUpper().StartsWith("CURRENT_") || dtRecordCheck.Rows[0]["WR_AGENTNAME"].ToString().ToUpper().StartsWith("CURRENT_"))
                            {
                                if (GV.Override_UserAccess)
                                {
                                    if (dtRecordCheck.Rows[0]["TR_AGENTNAME"].ToString().ToUpper() == "CURRENT_" + GV.sEmployeeName.ToUpper() || dtRecordCheck.Rows[0]["WR_AGENTNAME"].ToString().ToUpper() == "CURRENT_" + GV.sEmployeeName.ToUpper())
                                    {
                                        FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(null, frmCurrent.MdiParent, "ListOpen", IsNewCompany, Companylist);
                                        objfrmContactsUpdate.Show();
                                        return;
                                    }
                                    else
                                    {
                                        string sAgentName = string.Empty;
                                        if (dtRecordCheck.Rows[0]["TR_AGENTNAME"].ToString().ToUpper().Contains("CURRENT_"))
                                            sAgentName = GM.ProperCase_ProjectSpecific(dtRecordCheck.Rows[0]["TR_AGENTNAME"].ToString().ToUpper().Replace("CURRENT_", string.Empty));
                                        else
                                            sAgentName = GM.ProperCase_ProjectSpecific(dtRecordCheck.Rows[0]["WR_AGENTNAME"].ToString().ToUpper().Replace("CURRENT_", string.Empty));

                                        MessageBoxEx.Show("This record is already in use by <font size = '10' color='OrangeRed'><b>" + sAgentName + "</b></font>", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (dtRecordCheck.Rows[0][GV.sAccessTo + "_AGENTNAME"].ToString().ToUpper() == "CURRENT_" + GV.sEmployeeName.ToUpper())
                                    {
                                        FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(null, frmCurrent.MdiParent, "ListOpen", IsNewCompany, Companylist);
                                        objfrmContactsUpdate.Show();
                                        return;
                                    }
                                    else
                                    {
                                        string sAgentName = GM.ProperCase_ProjectSpecific(dtRecordCheck.Rows[0][GV.sAccessTo + "_AGENTNAME"].ToString().Replace("Current_", string.Empty));
                                        MessageBoxEx.Show("This record is already in use by <font size = '10' color='OrangeRed'><b>" + sAgentName + "</b></font>", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                GV.MYSQL.BAL_ExecuteQueryMySQL("UPDATE " + GV.sCompanyTable + " SET " + GV.sAccessTo + "_AGENTNAME = 'Current_" + GV.sEmployeeName + "' WHERE GROUP_ID = " + dtRecordCheck.Rows[0]["GROUP_ID"] + ";");
                                FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(null, frmCurrent.MdiParent, "ListOpen", IsNewCompany, Companylist);
                                objfrmContactsUpdate.Show();
                            }
                        }
                        else
                            ToastNotification.Show(frmCurrent.MdiParent, "ID does not exist.", eToastPosition.TopRight);
                        #endregion
                    }
                    else
                    {
                        #region Non Agent Open
                        DataTable dtAdminCheck = GV.MYSQL.BAL_FetchTableMySQL(GV.sCompanyTable, "MASTER_ID = " + sID + ";");
                        if (dtAdminCheck.Rows.Count > 0)
                        {
                            if (GV.sUserType != "Admin")
                            {
                                if (dtAdminCheck.Rows[0]["Flag"].ToString().Length > 0)
                                {                                    
                                    if (GV.sUserType == "QC" && !GV.Override_QCAccess)
                                    {
                                        if (dtAdminCheck.Rows[0]["Flag"].ToString().ToUpper() != GV.sAccessTo)
                                        {
                                            ToastNotification.Show(frmCurrent.MdiParent, "This record is in " + GV.sOppositAccess + " bin.", eToastPosition.TopRight);
                                            return;
                                        }
                                    }
                                                                        
                                    if (GV.sUserType == "Manager" && !GV.Override_ManagerAccess)
                                    {
                                        if (dtAdminCheck.Rows[0]["Flag"].ToString().ToUpper() != GV.sAccessTo)
                                        {
                                            ToastNotification.Show(frmCurrent.MdiParent, "This record is in " + GV.sOppositAccess + " bin.", eToastPosition.TopRight);
                                            return;
                                        }
                                    }                                    

                                    if (dtAdminCheck.Rows[0]["Scrape_Status"].ToString().ToUpper() == "1")
                                    {
                                        ToastNotification.Show(frmCurrent.MdiParent, "This company can't be opened.<br/>Company queued for scrapping.", eToastPosition.TopRight);
                                        return;
                                    }
                                }
                                else
                                {
                                    ToastNotification.Show(frmCurrent.MdiParent, "This record is not in your bin.", eToastPosition.TopRight);
                                    return;
                                }
                            }

                            if (dtAdminCheck.Rows[0]["TR_AGENTNAME"].ToString().ToUpper().StartsWith("CURRENT_") || dtAdminCheck.Rows[0]["WR_AGENTNAME"].ToString().ToUpper().StartsWith("CURRENT_"))
                            {
                                string sAgentName = string.Empty;
                                if (dtAdminCheck.Rows[0]["TR_AGENTNAME"].ToString().ToUpper().Contains("CURRENT_"))
                                    sAgentName = GM.ProperCase_ProjectSpecific(dtAdminCheck.Rows[0]["TR_AGENTNAME"].ToString().Replace("Current_", string.Empty));
                                else
                                    sAgentName = GM.ProperCase_ProjectSpecific(dtAdminCheck.Rows[0]["WR_AGENTNAME"].ToString().Replace("Current_", string.Empty));
                                MessageBoxEx.Show("This record is already in use by <font size = '10' color='OrangeRed'><b>" + sAgentName + "</b></font>", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }

                            GV.sPerformance += "Got record. Opening Contact Update : " + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                            FrmContactsUpdate objfrmContactsUpdate = new FrmContactsUpdate(sID, frmCurrent.MdiParent, "ListOpen", false, Companylist);
                            objfrmContactsUpdate.Show();
                        }
                        else
                            ToastNotification.Show(frmCurrent.MdiParent, "ID does not exist.", eToastPosition.TopRight);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #region TimeAgo
        //public static string TimeAgo(string sDate)
        //{
        //    DateTime d = Convert.ToDateTime(sDate);
        //    // 1.
        //    // Get time span elapsed since the date.
        //    TimeSpan s = DateTime.Now.Subtract(d);

        //    // 2.
        //    // Get total number of days elapsed.
        //    int dayDiff = (int)s.TotalDays;

        //    // 3.
        //    // Get total number of seconds elapsed.
        //    int secDiff = (int)s.TotalSeconds;

        //    // 4.
        //    // Don't allow out of range values.
        //    if (dayDiff < 0 || dayDiff >= 31)
        //    {
        //       // return null;
        //    }

        //    // 5.
        //    // Handle same-day times.
        //    if (dayDiff == 0)
        //    {
        //        // A.
        //        // Less than one minute ago.
        //        if (secDiff < 60)
        //        {
        //            return "just now";
        //        }
        //        // B.
        //        // Less than 2 minutes ago.
        //        if (secDiff < 120)
        //        {
        //            return "1 minute ago";
        //        }
        //        // C.
        //        // Less than one hour ago.
        //        if (secDiff < 3600)
        //        {
        //            return string.Format("{0} minutes ago",
        //            Math.Floor((double)secDiff / 60));
        //        }
        //        // D.
        //        // Less than 2 hours ago.
        //        if (secDiff < 7200)
        //        {
        //            return "1 hour ago";
        //        }
        //        // E.
        //        // Less than one day ago.
        //        if (secDiff < 86400)
        //        {
        //            return string.Format("{0} hours ago", Math.Floor((double)secDiff / 3600));
        //        }
        //    }
        //    // 6.
        //    // Handle previous days.
        //    if (dayDiff == 1)
        //    {
        //        return "yesterday";
        //    }

        //    if (dayDiff < 7)
        //    {
        //        return string.Format("{0} days ago", dayDiff);
        //    }

        //    if (dayDiff < 31)
        //    {
        //        return string.Format("{0} weeks ago", Math.Ceiling((double)dayDiff / 7));
        //    }

        //    return string.Empty;
        //} 
        #endregion

        public static string TimeAgo(DateTime dt)
        {
            try
            {
                //DateTime dt = Convert.ToDateTime(sDate);
                TimeSpan span = GM.GetDateTime() - dt;
                if (span.Days >= 365)
                {
                    int years = (int)Math.Round((span.Days / 365.00));

                    //if (span.Days % 365 != 0)
                    //    years += 1;

                    return String.Format("{0} {1} ago", years, years == 1 ? "year" : "years");
                }

                if (span.Days > 30)
                {
                    int months = (int)Math.Round((span.Days / 30.00));

                    //if (span.Days % 31 != 0)
                    //    months += 1;

                    return String.Format("{0} {1} ago", months, months == 1 ? "month" : "months");
                }

                if (span.Days > 7)
                {
                    int weeks = (int)Math.Round((span.Days / 7.00));

                    //if (span.Days % 7 != 0)
                    //    weeks += 1;

                    return String.Format("{0} {1} ago", weeks, weeks == 1 ? "week" : "weeks");
                }

                if (span.Days > 0)
                    return String.Format("{0} {1} ago", span.Days, span.Days == 1 ? "day" : "days");

                if (span.Hours > 0)
                    return String.Format("{0} {1} ago", span.Hours, span.Hours == 1 ? "hour" : "hours");

                if (span.Minutes > 0)
                    return String.Format("{0} {1} ago", span.Minutes, span.Minutes == 1 ? "minute" : "minutes");

                if (span.Seconds >= 0)
                    return String.Format("{0} {1} ago", span.Seconds, span.Seconds == 1 ? "second" : "seconds");

                //if (span.Seconds > 0)
                //    return "just now";

                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }



        public static void ErrorHandle(string sError)
        {
            if (GV.dtErrorMap != null && GV.dtErrorMap.Rows.Count > 0)
            {
                foreach (DataRow drErr in GV.dtErrorMap.Rows)
                {
                    if (sError.Contains(drErr["PicklistField"].ToString().ToLower()))
                    {
                        MessageBoxIcon Mc;
                        if (drErr["remarks"].ToString() == "1")
                            Mc = MessageBoxIcon.Information;
                        else if (drErr["remarks"].ToString() == "2")
                            Mc = MessageBoxIcon.Exclamation;
                        else
                            Mc = MessageBoxIcon.Error;

                        MessageBoxEx.Show(drErr["PicklistValue"].ToString(), "Campaign Manager", MessageBoxButtons.OK, Mc);
                        return;
                    }
                }
            }
            MessageBoxEx.Show("Unknown error occured.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        public static DataTable ImportExcel(string fileName)
        {
            DataTable dtReturnTable = new DataTable();
            try
            {
                using (FileStream fsReader = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sReader = new StreamReader(fsReader))
                    {
                        using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(sReader.BaseStream, false))
                        {
                            WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                            IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                            string relationshipId = sheets.First().Id.Value;
                            WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                            Worksheet workSheet = worksheetPart.Worksheet;
                            SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                            IEnumerable<Row> rows = sheetData.Descendants<Row>();
                            foreach (Cell cell in rows.ElementAt(0))
                            {
                                dtReturnTable.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                            }

                            foreach (Row row in rows)
                            {
                                DataRow dataRow = dtReturnTable.NewRow();
                                for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                                {
                                    dataRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                                }
                                dtReturnTable.Rows.Add(dataRow);
                            }
                        }
                        dtReturnTable.Rows.RemoveAt(0);
                        sReader.Close();
                        fsReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true);
            }
            return dtReturnTable;
        }

        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }
    }
}
