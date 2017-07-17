using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Data.SqlClient;
using System.Threading;

namespace GCC
{
    class ESpeechServer
    {
        int receiverPort = 20000;

        public ESpeechServer()
        {
            StartServer();
        }

        void StartServer()
        {
            while (true)
            {
                try
                {
                    Random Rand = new Random();
                    int iPort = Rand.Next(10000, 60000);//Max 65535
                    UdpClient UDPReceiver = new UdpClient(iPort);
                    UDPReceiver.BeginReceive(DataReceived, UDPReceiver);
                    GV.ESPort = iPort.ToString();
                    if(GV.IsWindowsXP)
                        ExecuteQuery("UPDATE c_machines set ESPort = '" + iPort + "', ESState = -1 WHERE MachineID='" + GV.sMachineID + "';");
                    else
                        ExecuteQuery("UPDATE c_machines set ESPort = '" + iPort + "', ESState = 0 WHERE MachineID='" + GV.sMachineID + "';");

                    break;
                }
                catch(Exception ex)
                {
                    //Thread.Sleep(10000);
                }   
            }
        }

        private static void DataReceived(IAsyncResult ar)
        {
            string sInfo = string.Empty;
            string sSendHost = string.Empty;
            string sSendPort = string.Empty;
            bool iSQuery = false;
            try
            {
                UdpClient c = (UdpClient)ar.AsyncState;
                IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);
                string sReceived = ASCIIEncoding.ASCII.GetString(receivedBytes);
                List<string> lstRecived = sReceived.Split(new string[] { "[:|:]" }, StringSplitOptions.None).ToList();
                string sAudioID = string.Empty;
                string sName = string.Empty;
                string sPhonetics = string.Empty;
                if (lstRecived.Count > 1)
                {
                    if (lstRecived[0] == "ESQuery")
                    {
                        iSQuery = true;
                        sSendPort = lstRecived[5];
                        sSendHost = lstRecived[1];
                        sName = lstRecived[2];
                        string sAPI = "http://espeech.com/namesayer_servertest?name=" + lstRecived[2] + "&user=meritapi&tts=1";
                        bool IsExisting = lstRecived[4].ToString() == "True";
                        using (WebClient wClient = new WebClient())
                        {
                            string sResponse = wClient.DownloadString(sAPI);
                            if (sResponse.Contains(":ttsspeech") && sResponse.Contains(".wav"))
                            {
                                sPhonetics = sResponse.Replace("<html>", string.Empty).Replace("</html>", string.Empty).Replace("\n", string.Empty).Split(new string[] { ":ttsspeech" }, StringSplitOptions.None)[0];
                                if (IsExisting)
                                {
                                    sAudioID = lstRecived[3];
                                    GV.MSSQL1.BAL_ExecuteQuery("UPDATE RM..NameSayer SET HitCount = HitCount + 1, LastHitDate = GETDATE() WHERE AudioID = '" + sAudioID + "'");
                                }
                                else
                                {
                                    sAudioID = GV.MSSQL1.BAL_InsertAndGetIdentity("INSERT INTO RM..NameSayer (Name, Phonetics, HitCount, LastHitDate) VALUES ('" + GM.HandleBackSlash(sName.Replace("'", "''")) + "', '" + sPhonetics.Replace("'", "''") + "', 1, GETDATE());");
                                }
                                if (sAudioID.Length > 0)
                                    wClient.DownloadFile("https://www.espeech.com/" + sResponse.Substring(sResponse.IndexOf("ttsspeech")).Replace("</html>", string.Empty).Replace("\n", string.Empty), @"\\CH1031SF02\Campaign Manager\NameSayer\" + sAudioID + ".wav");
                            }
                        }
                        string sDataToReturn = "ESResult[:|:]" + sAudioID + "[:|:]" + sPhonetics + "[:|:]" + sName;
                        using (System.Net.Sockets.UdpClient sender1 = new System.Net.Sockets.UdpClient(7000))
                        {
                            sender1.Send(Encoding.ASCII.GetBytes(sDataToReturn), sDataToReturn.Length, lstRecived[1], Convert.ToInt32(lstRecived[5]));
                        }
                    }

                    #region OldGSpeech
                    //else if (lstRecived[0] == "GSQuery")
                    //{
                    //    iSQuery = true;
                    //    sSendPort = lstRecived[3];
                    //    sSendHost = lstRecived[1];
                    //    string sAudioPath = lstRecived[2];

                    //    var request = new Google.Apis.CloudSpeechAPI.v1beta1.Data.SyncRecognizeRequest()
                    //    {
                    //        Config = new Google.Apis.CloudSpeechAPI.v1beta1.Data.RecognitionConfig()
                    //        {
                    //            Encoding = "LINEAR16",
                    //            SampleRate = 16000,
                    //            LanguageCode = "en-IN"
                    //        },
                    //        Audio = new Google.Apis.CloudSpeechAPI.v1beta1.Data.RecognitionAudio()
                    //        {
                    //            Content = Convert.ToBase64String(System.IO.File.ReadAllBytes(sAudioPath))
                    //        }
                    //    };
                    //    //var response = string.Empty; //GV.GSpeechCloudAPI.Speech.Syncrecognize(request).Execute();
                    //    string sOut = string.Empty;
                    //    //foreach (var result in response.Results)
                    //    //{
                    //    //    foreach (var alternative in result.Alternatives)
                    //    //        sOut += alternative.Transcript;
                    //    //}

                    //    string sDataToReturn;
                    //    if (sOut.Length > 0)
                    //        sDataToReturn = "GSResult[:|:]" + sOut + "[:|:]Sucess[:|:]" + sAudioPath;                                                    
                    //    else
                    //        sDataToReturn = "GSResult[:|:][:|:]Error[:|:]" + sAudioPath;

                    //    using (System.Net.Sockets.UdpClient sender1 = new System.Net.Sockets.UdpClient(7000))
                    //    {
                    //        sender1.Send(Encoding.ASCII.GetBytes(sDataToReturn), sDataToReturn.Length, lstRecived[1], Convert.ToInt32(lstRecived[3]));
                    //    }                                               
                    //} 
                    #endregion

                    else if (lstRecived[0] == "ESResult")
                    {
                        foreach (System.Windows.Forms.Form f in System.Windows.Forms.Application.OpenForms)
                        {
                            if (f.Name == "FrmContactsUpdate")
                            {
                                if (((FrmContactsUpdate)f).sCurrentESName == lstRecived[3])
                                {
                                    ((FrmContactsUpdate)f).sESAudioID = lstRecived[1];
                                    ((FrmContactsUpdate)f).sESPhonetics = lstRecived[2];
                                }
                                break;
                            }
                        }
                    }
                    //else if (lstRecived[0] == "GSResult")
                    //{
                    //    foreach (System.Windows.Forms.Form f in System.Windows.Forms.Application.OpenForms)
                    //    {
                    //        if (f.Name == "FrmContactsUpdate")
                    //        {
                    //            if (((FrmContactsUpdate)f).sCurrentAudioPath == lstRecived[3])
                    //            {
                    //                ((FrmContactsUpdate)f).sCommentText = lstRecived[1];
                    //                ((FrmContactsUpdate)f).sCommentError = lstRecived[2];
                    //                try
                    //                {
                    //                    GV.MSSQL1.BAL_ExecuteNonReturnQuery("INSERT INTO c_gspeech_log (SessionID,CompanySessionID,ProjectID,AudioFileName,RecognitionText,[Who],[When]) VALUES('" + GV.sSessionID + "','" + GV.sCompanySessionID + "','" + GV.sProjectID + "','" + System.IO.Path.GetFileName(lstRecived[3]) + "','" + lstRecived[1].Replace("'", "''") + "','" + GV.sEmployeeName.Replace("'", "''") + "',GETDATE());");
                    //                }
                    //                catch (Exception ex)
                    //                {}

                    //            }
                    //            break;
                    //        }
                    //    }
                    //}
                    else if (lstRecived[0] == "Error")
                    {
                        foreach (System.Windows.Forms.Form f in System.Windows.Forms.Application.OpenForms)
                        {
                            if (f.Name == "FrmContactsUpdate")
                            {
                                ((FrmContactsUpdate)f).sESAudioID = "Error";
                                ((FrmContactsUpdate)f).sESPhonetics = "Error";
                                break;
                            }
                        }
                    }
                }                
                //  MessageBox.Show(receivedIpEndPoint + ": " + receivedText + Environment.NewLine);
                c.BeginReceive(DataReceived, ar.AsyncState);
            }
            catch(Exception ex)
            {
                if(iSQuery)
                {
                    using (System.Net.Sockets.UdpClient sender1 = new System.Net.Sockets.UdpClient(7000))
                    {
                        sender1.Send(Encoding.ASCII.GetBytes("Error[:|:]"), 10, sSendHost, Convert.ToInt32(sSendPort));
                    }
                }
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);
                
            }
        }


        static void ExecuteQuery(string sQuery)
        {
            using (SqlConnection con = new SqlConnection(GV.sMSSQL1))
            {
                try
                {
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    using (SqlCommand cmd = new SqlCommand(sQuery, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false, sQuery);
                }
            }
        }


    }

    

}
