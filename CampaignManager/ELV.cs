using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MySql.Data.MySqlClient;
using System.Net;
using System.Threading;
using System.IO;

namespace GCC
{
    public class ELV
    {


        public ELV(DataTable dtEmailStatus)
        {
            try
            {
                
                
                //BackgroundWorker bELV = new BackgroundWorker();
                DateTime dProcess_StartTime = default(DateTime);
                using (MySqlConnection con1 = new MySqlConnection(GV.sMySQL))
                {
                    
                    while (true)
                    {                        
                        using (DataTable dtEmails = new DataTable())
                        {
                            
                            Thread.Sleep(GV.ibg_Interval);
                            
                            
                            
                            //string sQuery = @"UPDATE c_email_checks AS A INNER JOIN (SELECT EM.ID, EM.PROJECT_ID, EM.CONTACT_ID FROM c_email_checks EM 
                            //WHERE EMAIL_SOURCE = '0' AND EM.DESCRIPTION IS NULL AND (EM.APPENDED_DATE IS NULL OR DATE_ADD(EM.APPENDED_DATE, INTERVAL " + GV.ibg_CheckTimeoutPerBatch +
                            //        " SECOND) > NOW()) ORDER BY RAND() LIMIT " + GV.ibg_LoadCount + ") AS B ON (A.ID = B.ID) SET A.PROCESSED_SERVER = '" + GV.sSessionID +
                            //        "', A.APPENDED_DATE = NOW(); SELECT * FROM c_email_checks WHERE DESCRIPTION IS NULL AND PROCESSED_SERVER = '" + GV.sSessionID +
                            //        "' AND (APPENDED_DATE IS NULL OR DATE_ADD(APPENDED_DATE, INTERVAL " + GV.ibg_CheckTimeoutPerBatch +
                            //        " SECOND) > NOW());";
                            
                            string sProjectTables_Update = string.Empty;
                            try
                            {
                                using (MySqlCommand cmdEmails = new MySqlCommand("CALL MVC.ELV ('" + GV.sSessionID + "',"+ GV.ibg_BatchExpiry + ","+ GV.ibg_LoadCount + ")", con1))
                                //using (MySqlCommand cmdEmails = new MySqlCommand("ELV", con1))
                                {
                                    if (con1.State != ConnectionState.Open)
                                        con1.Open();
                                    //cmdEmails.CommandType = CommandType.StoredProcedure;
                                    //cmdEmails.Parameters.Add(new MySqlParameter("serverid", GV.sSessionID));
                                    //cmdEmails.Parameters.Add(new MySqlParameter("inter", GV.ibg_BatchExpiry));
                                    //cmdEmails.Parameters.Add(new MySqlParameter("caveats", GV.ibg_LoadCount));

                                    using (MySqlDataAdapter daEmails = new MySqlDataAdapter(cmdEmails))
                                    {
                                        
                                        daEmails.Fill(dtEmails);
                                        
                                    }
                                    con1.Close();
                                }

                                if (dtEmails != null && dtEmails.Rows.Count > 0)
                                {
                                    dProcess_StartTime = Convert.ToDateTime(dtEmails.Rows[0]["APPENDED_DATE"]);
                                    while (dtEmails.Select("LEN(ISNULL(DESCRIPTION,'')) = 0").Length > 0)
                                    {
                                        
                                        for (int i = 0; i < dtEmails.Rows.Count; i++)
                                        {
                                            if (dtEmails.Rows[i]["DESCRIPTION"].ToString().Trim().Length == 0)
                                            {
                                                
                                                Thread.Sleep(5000);
                                                
                                                try
                                                {
                                                    
                                                    WebRequest wrGETURL;
                                                    wrGETURL = WebRequest.Create(GV.sbg_API.Replace("|EmailIDString|", dtEmails.Rows[i]["Email"].ToString().Replace("\n", "")));
                                                    WebProxy myProxy = new WebProxy("myproxy", 80);
                                                    myProxy.BypassProxyOnLocal = true;
                                                    wrGETURL.Proxy = WebProxy.GetDefaultProxy();
                                                    wrGETURL.Timeout = GV.ibg_CheckTimeoutPerEmail;
                                                    using (Stream objStream = wrGETURL.GetResponse().GetResponseStream())
                                                    {
                                                        using (StreamReader objReader = new StreamReader(objStream))
                                                        {
                                                            string sLine = string.Empty;
                                                            string sLineTemp = string.Empty;
                                                            while (sLineTemp != null)
                                                            {
                                                                sLineTemp = objReader.ReadLine();
                                                                if (sLineTemp != null)
                                                                    sLine += sLineTemp;
                                                            }

                                                            if (!sLine.Contains("<html>"))//Incase ELV down
                                                            {
                                                                if (sLine.Contains("|"))
                                                                {
                                                                    dtEmails.Rows[i]["DETAIL"] = sLine.Split('|')[0].Trim();
                                                                    dtEmails.Rows[i]["DESCRIPTION"] = sLine.Split('|')[1].Trim();
                                                                }
                                                                else
                                                                {
                                                                    if (sLine.Trim().Length > 0)
                                                                        dtEmails.Rows[i]["DESCRIPTION"] = sLine.Trim();
                                                                    //else//Incase ELV doesn't respond any result
                                                                    //    dtEmails.Rows[i]["DESCRIPTION"] = "NEW DESCRIPTION[Invalid Result]";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                
                                                                Thread.Sleep(10000);
                                                                
                                                            }
                                                        }
                                                    }

                                                    
                                                }
                                                catch (Exception e)
                                                {
                                                    
                                                    Thread.Sleep(10000);//Hold for a while if the Web API has to be clear out                                                    
                                                    break;//Break the for loop and re-enter the parant while loop                                            
                                                }
                                            }
                                        }
                                    }

                                    
                                    foreach (DataRow drEmails in dtEmails.Rows)
                                    {
                                        string sDescription = drEmails["DESCRIPTION"].ToString();
                                        string sDetails = drEmails["DETAIL"].ToString();

                                        if (sDescription.Trim().Length > 0)
                                        {
                                            if (dtEmailStatus.Select("PicklistField = '" + sDescription.Replace("'", "''").Replace("\\", string.Empty) + "'").Length > 0)
                                            {
                                                DataRow drEmailBounce_Names = dtEmailStatus.Select("PicklistField = '" + sDescription.Replace("'", "''").Replace("\\", string.Empty) + "'")[0];
                                                string sBounceStatus = dtEmailStatus.Select("PicklistField = '" + sDescription.Replace("'", "''").Replace("\\", string.Empty) + "'")[0]["PicklistValue"].ToString();
                                                if (sBounceStatus == "HARD")
                                                    sBounceStatus = "BOUNCED";

                                                sProjectTables_Update += "UPDATE " + drEmails["PROJECT_ID"] + "_mastercontacts set EMAIL_VERIFIED = '" + sBounceStatus + "', BOUNCE_STATUS = '" + sDescription.Replace("'", "''").Replace("\\", string.Empty) +
                                                                "', BOUNCE_LOADED_DATE = NOW(), BOUNCE_LOADED_BY = 'CM' WHERE CONTACT_ID_P=" + drEmails["CONTACT_ID"] + ";";

                                                //drEmails["DESCRIPTION"] = sDescription;
                                                //drEmails["DETAIL"] = sDetails;
                                                //drEmails["PROCESSED_SERVER"] = GV.sSessionID;
                                            }
                                            else
                                            {
                                                drEmails["DESCRIPTION"] = "NEW DESCRIPTION[" + sDescription + "]";
                                                //drEmails["DETAIL"] = sDetails;
                                                //drEmails["PROCESSED_SERVER"] = GV.sSessionID;
                                                //Trigger Email
                                                //sTables_Update += "UPDATE " + drEmails["PROJECT_ID"] + "_mastercontacts set EMAIL_VERIFIED = 'Category not found', BOUNCE_STATUS = '" + drrEmail[0]["statusdescription"].ToString().Replace("'", "''") + "', BOUNCE_LOADED_DATE = NOW() WHERE CONTACT_ID_P=23;";
                                            }
                                        }
                                    }

                                    if (GM.GetDateTime(true) < dProcess_StartTime.AddSeconds(GV.ibg_BatchExpiry))
                                    {
                                        using (MySqlConnection con2 = new MySqlConnection(GV.sMySQL))
                                        {
                                            using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM c_email_checks Where 1=0", con2))
                                            {
                                                using (MySqlCommandBuilder cb = new MySqlCommandBuilder(da))
                                                {
                                                    if(con2.State != ConnectionState.Open)
                                                        con2.Open();

                                                    cb.ConflictOption = ConflictOption.OverwriteChanges;
                                                    da.UpdateCommand = cb.GetUpdateCommand();
                                                    da.UpdateCommand.CommandTimeout = 600;

                                                    GV.sPerformance += "Update Email Table Start:\t" + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                                                    da.Update(dtEmails);
                                                    GV.sPerformance += "Update Email Table End:\t" + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                                                }
                                            }

                                            if (sProjectTables_Update.Length > 0)
                                            {
                                                using (MySqlCommand cmdUpdateTables = new MySqlCommand(sProjectTables_Update, con2))
                                                {
                                                    if (con2.State != ConnectionState.Open)
                                                        con2.Open();
                                                    GV.sPerformance += "Update Project Table Start:\t" + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                                                    cmdUpdateTables.ExecuteNonQuery();
                                                    GV.sPerformance += "Update Project Table End:\t" + GV.PerformanceWatch.Elapsed.TotalSeconds + Environment.NewLine;
                                                }
                                            }
                                            con2.Close();
                                        }
                                    }
                                }

                            }
                            catch(Exception ex)
                            {
                                //if(!ex.Message.Contains("Deadlock"))
                                    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false, sProjectTables_Update);

                               // continue;
                            }



                        }//dtEmails

                    }
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);
            }
        }        
    }
}
