using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MySql.Data.MySqlClient;

namespace EAF
{
    public class EAF
    {
        //As of [2014 - 07 - 15] by Prakash
        
        //**************************************************************************************Value Reference***********************************************************************************************************************************************************************************************************
        //****************************************************************************************************************************************************************************************************************************************************************************************************************
        //ProjectName                               : CRU_Copper                : String
        //ProjectID                                 : CRUCRU005                 : String
        //EmployeeName                              : THANGAPRAKASH             : String
        //EmployeeNo                                : 600950                    : String
        //CompanyTable                              : CRUCRU005_Mastercompanies : String
        //ContactTable                              : CRUCRU005_MASTERCONTACTS  : String
        //UserType                                  : Agent                     : String
        //AccessTo                                  : TR                        : String
        //AllowTelephoneFormating                   : Y                         : String
        //AllowGeneralEmail                         : N                         : String
        //AllowDuplicateEmail                       : N                         : String
        //AllowDuplicateJobTitle                    : Y                         : String
        //AllowPublicDomainEmails                   : N                         : String
        //AllowDuplicateEmailAcrossProject          : N                         : String
        //AllowNewCompanyTR                         : N                         : String
        //AllowNewCompanyWR                         : Y                         : String
        //EmailCheckBinaryPath                      : C:\Users\THANGA~1\AppData\Local\Temp\Campaign Manager\Email_check.exe : String
        //SendKeyBinaryPath                         : C:\Users\THANGA~1\AppData\Local\Temp\Campaign Manager\SendKeys.exe : String
        //SpellCheckJobTitle                        : Y                         : String
        //AllowSwitchBoardNumberinContacts          : Y                         : String
        //FreezeWRCompletedRecords                  : Y                         : String
        //FreezeTRCompletedRecords                  : Y                         : String
        //FreezeWRCompanyCompletes                  : Y                         : String
        //FreezeTRCompanyCompletes                  : Y                         : String
        //MaxValidatedContactCount                  : 0                         : Int
        //MinValidatedContactCountComplets          : 0                         : Int
        //MinValidatedContactCountPartialComplets   : 0                         : Int
        //MinValidatedContactCountPartialComplets   : 0                         : Int
        //ShowOnGridMasterCompanies                 : ADDRESS_1~ADDRESS_2~ALTERNATE_TELEPHONE_NUMBERS~CITY~COMPANY_NAME~COUNTRY~COUNTY~GENERAL_EMAIL~MASTER_ID~POST_CODE~SWITCHBOARD~TR_AGENTNAME~TR_DATECALLED~TR_PRIMARY_DISPOSAL~TR_SECONDARY_DISPOSAL~WR_AGENTNAME~WR_DATE_OF_PROCESS~WR_PRIMARY_DISPOSAL~WR_SECONDARY_DISPOSAL : List
        //ShowOnGridMasterContacts                  : CONTACT_EMAIL~CONTACT_TELEPHONE~FIRST_NAME~JOB_TITLE~LAST_NAME~OTHERS_JOBTITLE~TITLE~TR_CONTACT_STATUS~WR_CONTACT_STATUS : List
        //ShowOnCriteriaMasterCompanies             : COMPANY_NAME~COUNTRY~MAIN_INDUSTRY~MASTER_ID~TR_AGENTNAME~TR_DATECALLED~TR_PRIMARY_DISPOSAL~WR_AGENTNAME~WR_DATE_OF_PROCESS : List
        //ShowOnCriteriaMasterContacts              : EMAIL_VERIFIED~REJECTION~REVIEW_TAG~TR_CONTACT_STATUS~TR_UPDATED_DATE~WR_CONTACT_STATUS~WR_UPDATED_DATE : List
        //ContactStatusToBeFreezed                  : VERIFIED AND COMPLETE~UPDATE AND COMPLETE~REPLACEMENT AND COMPLETE~NEW AND COMPLETE : List
        //DisposalsToBeFreezed                      : COMPLETE SURVEY           : List
        //TRContactStatusToBeValidated              : VERIFIED AND COMPLETE~UPDATE AND COMPLETE~REPLACEMENT AND COMPLETE~NEW AND COMPLETE : List
        //WRContactStatusToBeValidated              : VERIFIED AND COMPLETE~UPDATE AND COMPLETE~REPLACEMENT AND COMPLETE~NEW AND COMPLETE~WEBRESEARCH : List
        //TR_DeleteStatus                           : CONTACT LEFT~CONTACT UNKNOWN : List
        //WR_DeleteStatus                           : CONTACT LEFT~CONTACT UNKNOWN : List
        //DisposalsToBeValidated                    : COMPLETE SURVEY           : List
        //NewRecordContactStatus                    : NEW AND COMPLETE~NEW AND INCOMPLETE : List
        //UnchangedRecordContactStatus              : VERIFIED AND COMPLETE~PARTIAL VERIFIED : List
        //ChangedRecordContactStatus                : UPDATE AND COMPLETE       : List
        //ReplacementRecordContactStatus            :                           : List
        //ReplacementOptionRecordContactStatus      : CONTACT LEFT              : List
        //NeutralContactStatus                      :                           : List
        //EmailCheckContactStatus                   : 'VERIFIED AND COMPLETE','UPDATE AND COMPLETE','REPLACEMENT AND COMPLETE','NEW AND COMPLETE','WEBRESEARCH' : String
        //ReplacementOptionContactStatus            : 'CONTACT LEFT'            : String
        //TRContactstatusTobeValidated              : 'VERIFIED AND COMPLETE','UPDATE AND COMPLETE','REPLACEMENT AND COMPLETE','NEW AND COMPLETE' : String
        //WRContactstatusTobeValidated              : 'VERIFIED AND COMPLETE','UPDATE AND COMPLETE','REPLACEMENT AND COMPLETE','NEW AND COMPLETE','WEBRESEARCH' : String
        //SortableContactColumn                     : TR_UPDATED_DATE~WR_UPDATED_DATE~FIRST_NAME~CONTACT_EMAIL~TR_CONTACT_STATUS : List
        //MSSQLConString                            : user id=user1;password=M3r1t1n6i#;data source=172.27.137.181;initial catalog=MVC : String
        //MYSQLConString                            : user id=user1;password=M3r1t1n6i#;data source=172.27.137.181;initial catalog=MVC : String
        //ContactRowIndex                           : 2                         : Int
        //FieldName                                 : Job_Title                 : String
        //FreezedContactIDs                         :                           : List
        //BouncedContactIDs                         :                           : List
        //****************************************************************************************************************************************************************************************************************************************************************************************************************
        //****************************************************************************************************************************************************************************************************************************************************************************************************************


        //As of [2014 - 07 - 15] by Prakash
        //******************************************************************************************************Table Reference*******************************************************************************************************************************************************************************************
        //****************************************************************************************************************************************************************************************************************************************************************************************************************
        //MasterCompanies
        //MasterContact
        //FieldMaster
        //PickList
        //Validations
        //Country
        //ProjectInfo
        //****************************************************************************************************************************************************************************************************************************************************************************************************************
        //****************************************************************************************************************************************************************************************************************************************************************************************************************

        
        Exception objEX;
       
        //EAF()
        //{
        //    dtMessage.Columns.Add("Type");
        //    dtMessage.Columns.Add("Message");
        //    dtMessage.Rows.Add("ER", string.Empty);
        //    dtMessage.Rows.Add("EX", string.Empty);
        //}

        string sReturnMessage = string.Empty;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }   

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //if (dtMessage != null)
                //{
                //    dtMessage.Dispose();
                //    dtMessage = null;
                //}

            }
        }

        ~EAF()
        {
            Dispose(false);
        }


        public DataTable GetList(List<DataTable> lstDatable)
        {
            //********************************************************************************************************
            //The return Datable must contain the Column 'PicklistValue'. This column will be displayed on selection.
            //If 'PicklistValue' Column Not Available then rename the Search column to 'PicklistValue'
            //********************************************************************************************************

            DataTable dtMessage = new DataTable("Message");
            dtMessage.Columns.Add("Type");
            dtMessage.Columns.Add("Message");
            dtMessage.Rows.Add("ER", string.Empty);
            dtMessage.Rows.Add("EX", string.Empty);

            //DataValidator.DoWork += new DoWorkEventHandler(Validator_DoWork);
            //DataValidator.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Validator_RunWorkerCompleted);
            //btnSortMenu.Click += new EventHandler(sortToolStripMenuItem_Click);

            


            try
            {

                return null;

                
                //dtContact.Rows[Convert.ToInt32(Table(lstDatable, "ProjectInfo").Select("Key = 'ContactRowIndex'")[0][1])]["Last_Name"].ToString();

                //if (dtPickList.Select("PicklistCategory = '" + sCategory + "'").Length > 0)
                //    return dtPickList.Select("PicklistCategory = '" + sCategory + "'").CopyToDataTable();
                //else
                //    return null;
            }
            catch (Exception ex)
            {
                objEX = ex;
                dtMessage.Rows[1][1] = ex.Message;
                return dtMessage;
            }
        }

        public DataTable ExecuteQueryMySQL(string sSQLText, string sConstring)
        {
            DataTable dtMessage = new DataTable("Message");
            try
            {
                dtMessage.Columns.Add("Type");
                dtMessage.Columns.Add("Message");
                dtMessage.Rows.Add("ER", string.Empty);
                dtMessage.Rows.Add("EX", string.Empty);
                MySqlConnection connection = new MySqlConnection(sConstring);
                //MySqlConnection connection = new MySqlConnection(GlobalVariables.sMySQL);
                MySqlDataAdapter da = new MySqlDataAdapter();
                DataTable dt = new DataTable();
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                da.SelectCommand = new MySqlCommand(sSQLText, connection);
                da.Fill(dt);
                //GlobalVariables.conMYSQL.Close();
                return dt;
            }
            catch (Exception ex)
            {
                dtMessage.Rows.Add("EX", "MySQL Connection Error");
                return null;
            }
        }

        public DataTable Validation(List<DataTable> lstDatable)
        {
            
            DataTable dtMessage = new DataTable("Message");
            dtMessage.Columns.Add("Type");
            dtMessage.Columns.Add("Message");
            dtMessage.Rows.Add("ER", string.Empty);
            dtMessage.Rows.Add("EX", string.Empty);
            

            try
            {
                DataTable dtProjectInfo = Table(lstDatable, "ProjectInfo");
                DataTable dtMasterCompanies = Table(lstDatable, "MasterCompanies");
                DataTable dtPickList = Table(lstDatable, "PickList");
                string sAccessType = dtProjectInfo.Select("Key = 'AccessTo'")[0]["Value"].ToString();
                string sUserType = dtProjectInfo.Select("Key = 'UserType'")[0]["Value"].ToString();

                if (sUserType == "QC")
                    return dtMessage;


                string sProjectID = dtProjectInfo.Select("Key = 'ProjectID'")[0]["Value"].ToString().ToUpper();
                string sTREmailCheckContactStatus = dtProjectInfo.Select("Key = 'TRContactstatusTobeValidated' AND Type='String'")[0]["Value"].ToString();
                string sWREmailCheckContactStatus = dtProjectInfo.Select("Key = 'WRContactstatusTobeValidated' AND Type='String'")[0]["Value"].ToString();
                string sFreezedCIDs = dtProjectInfo.Select("Key = 'FreezedContactIDs'")[0]["Value"].ToString();
                string sConstring = dtProjectInfo.Select("Key = 'MYSQLConString'")[0]["Value"].ToString();



              

                
                //string sMaster_ID = dtMasterCompanies.Rows[0]["Master_ID"].ToString();
                string sMaster_ID = string.Empty;

                foreach(DataRow drCompanies in dtMasterCompanies.Rows)
                {
                    if (sMaster_ID.Length > 0)
                        sMaster_ID += "," + drCompanies["Master_ID"].ToString();
                    else
                        sMaster_ID = drCompanies["Master_ID"].ToString();
                }

                string sTR_ContStatus = string.Empty;
                string sWR_ContStatus = string.Empty;

                DataRow[] drrTR = dtPickList.Select("PicklistCategory = 'TR_EAF_STATUS'");
                DataRow[] drrWR = dtPickList.Select("PicklistCategory = 'WR_EAF_STATUS'");

                if (drrTR.Length > 0)
                {
                    foreach (DataRow dr in drrTR)
                    {
                        if (sTR_ContStatus.Length > 0)
                            sTR_ContStatus += ",'" + dr["PicklistValue"].ToString() + "'";
                        else
                            sTR_ContStatus = "'" + dr["PicklistValue"].ToString() + "'";
                    }
                }

                if (drrWR.Length > 0)
                {
                    foreach (DataRow dr in drrWR)
                    {
                        if (sWR_ContStatus.Length > 0)
                            sWR_ContStatus += ",'" + dr["PicklistValue"].ToString() + "'";
                        else
                            sWR_ContStatus = "'" + dr["PicklistValue"].ToString() + "'";
                    }
                }

                //List<string> lstFreezedContactIDString = new List<string>();
                //List<int> lstFreezedContactIDs = new List<int>();
               
                //if (sFreezedCIDs.Length > 0)
                //{
                //    lstFreezedContactIDString = sFreezedCIDs.Split('~').ToList();
                //    foreach (string s in lstFreezedContactIDString)
                //        lstFreezedContactIDs.Add(Convert.ToInt32(s));                    
                //}

                //Source_Email
                DataTable dtMasterContacts = Table(lstDatable, "MasterContact");
                {
                    //Check email duplicate with entire contacts of project
                    DataRow[] drrContactsToValidate = null;

                    if (sTR_ContStatus.Length > 1 && sWR_ContStatus.Length > 1)
                        drrContactsToValidate = dtMasterContacts.Select("TR_CONTACT_STATUS IN (" + sTR_ContStatus + ") OR WR_CONTACT_STATUS IN (" + sWR_ContStatus + ")");
                    else if(sTR_ContStatus.Length > 1)
                        drrContactsToValidate = dtMasterContacts.Select("TR_CONTACT_STATUS IN (" + sTR_ContStatus + ")");
                    else                    
                        drrContactsToValidate = dtMasterContacts.Select("WR_CONTACT_STATUS IN (" + sWR_ContStatus + ")");

                    List<string> lstDomains = new List<string>();

                    if (drrContactsToValidate.Length > 0)
                    {
                        foreach (DataRow dr in drrContactsToValidate)
                        {
                            if (dr["CONTACT_ID_P"].ToString().Length == 0 && dr["CONTACT_EMAIL"].ToString().Length > 0 && dr["CONTACT_EMAIL"].ToString().Contains("@"))
                            {
                                //if (dr["CONTACT_ID_P"].ToString().Length > 0 && lstFreezedContactIDs.Contains(Convert.ToInt32(dr["CONTACT_ID_P"])))
                                //    continue;//Do not check freezed records

                                lstDomains.Add(dr["CONTACT_EMAIL"].ToString().Trim().Replace("'", "''").Split('@').ToList()[1].ToLower());

                                //if (sEmails.Length > 0)
                                //    sEmails += ",'" + dr["CONTACT_EMAIL"].ToString().Trim().Replace("'", "''").Split('@').ToList()[1] + "'";
                                //else                                    
                                //    sEmails = "'" + dr["CONTACT_EMAIL"].ToString().Trim().Replace("'", "''").Split('@').ToList()[1] + "'";

                                //if (dr["CONTACT_EMAIL"].ToString().ToUpper().Trim() == dr["Source_Email"].ToString().ToUpper().Trim())
                                //    dtMessage.Rows[0][1] += "<font color = 'DarkCyan'> " + dr["CONTACT_EMAIL"].ToString() + "</font>: Same source Email found." + Environment.NewLine;
                            }
                        }

                        //DataRow[] drrMasterCotactsNewCheck = dtMasterContacts.Select("(TR_CONTACT_STATUS IN (" + sTREmailCheckContactStatus + ") OR WR_CONTACT_STATUS IN (" + sWREmailCheckContactStatus + "))");
                        //bool IsTrue = false;
                        //if (drrMasterCotactsNewCheck.Length > 0)
                        //{
                        //    foreach (DataRow dr in drrMasterCotactsNewCheck)
                        //    {
                        //        if (dr["CONTACT_ID_P"].ToString().Length == 0)
                        //        {
                        //            IsTrue = true;
                        //            break;
                        //        }
                        //    }
                        //}

                        if (lstDomains.Count > 0)
                        {                           
                            lstDomains = lstDomains.Distinct().ToList();                            
                            string sDomainList = string.Empty;
                            foreach (string sDomain in lstDomains)
                            {
                                if (sDomainList.Length > 0)
                                    sDomainList += ",'" + sDomain + "'";
                                else
                                    sDomainList = "'" + sDomain + "'";
                            }

                            //DataRow[] drrEmailDomainCount = dtPickList.Select("PicklistCategory = 'DomainCap'");

                            DataRow[] drrEmailDomainCap = dtPickList.Select("PicklistCategory = 'Industry_Size' AND PicklistValue = '" + dtMasterCompanies.Rows[0]["Emp_Count"] + "'");

                            if (drrEmailDomainCap.Length > 0 && drrEmailDomainCap[0]["remarks"].ToString().Length > 0)
                            {




                                int iCap = Convert.ToInt32(drrEmailDomainCap[0]["remarks"]);
                                
                                //if (sProjectID == "INBINB008")
                                //{
                                //    if (dtMasterCompanies.Rows[0]["Emp_Count"].ToString() == "1000-2499" || dtMasterCompanies.Rows[0]["Emp_Count"].ToString() == "2500-4999" || dtMasterCompanies.Rows[0]["Emp_Count"].ToString() == "5000-10000" || dtMasterCompanies.Rows[0]["Emp_Count"].ToString() == "10000-15000")
                                //        iCap = 60;
                                //}

                                DataTable dtDomainContacts = null;

                                if (sTR_ContStatus.Length > 1 && sWR_ContStatus.Length > 1)
                                    dtDomainContacts = ExecuteQueryMySQL("select Contact_Email from " + sProjectID + "_mastercontacts where domain IN (" + sDomainList + ") AND (TR_CONTACT_STATUS IN (" + sTR_ContStatus + ") OR WR_CONTACT_STATUS IN (" + sWR_ContStatus + ")) AND MASTER_ID NOT IN (" + sMaster_ID + ");", sConstring);
                                else if (sTR_ContStatus.Length > 1)
                                    dtDomainContacts = ExecuteQueryMySQL("select Contact_Email from " + sProjectID + "_mastercontacts where domain IN (" + sDomainList + ") AND TR_CONTACT_STATUS IN (" + sTR_ContStatus + ") AND MASTER_ID NOT IN (" + sMaster_ID + ");", sConstring);
                                else
                                    dtDomainContacts = ExecuteQueryMySQL("select Contact_Email from " + sProjectID + "_mastercontacts where domain IN (" + sDomainList + ") AND WR_CONTACT_STATUS IN (" + sWR_ContStatus + ") AND MASTER_ID NOT IN (" + sMaster_ID + ");", sConstring);

                                //DataTable dtDomainContacts = ExecuteQueryMySQL("select Contact_Email from " + sProjectID + "_mastercontacts where domain IN (" + sDomainList + ") AND TR_CONTACT_STATUS IN (" + sTR_ContStatus + ") AND MASTER_ID NOT IN (" + sMaster_ID + ");", sConstring);
                                foreach (string sDomains in lstDomains)
                                {
                                    int iInnerCount = 0;

                                    if (sTR_ContStatus.Length > 1 && sWR_ContStatus.Length > 1)
                                        iInnerCount = dtMasterContacts.Select("CONTACT_EMAIL LIKE '%@" + sDomains + "' AND (TR_CONTACT_STATUS IN (" + sTR_ContStatus + ") OR WR_CONTACT_STATUS IN (" + sWR_ContStatus + "))").Length;
                                    else if (sTR_ContStatus.Length > 1)
                                        iInnerCount = dtMasterContacts.Select("CONTACT_EMAIL LIKE '%@" + sDomains + "' AND TR_CONTACT_STATUS IN (" + sTR_ContStatus + ")").Length;
                                    else
                                        iInnerCount = dtMasterContacts.Select("CONTACT_EMAIL LIKE '%@" + sDomains + "' AND WR_CONTACT_STATUS IN (" + sWR_ContStatus + ")").Length;
                                            
                                    int iOuterCount = dtDomainContacts.Select("CONTACT_EMAIL LIKE '%@" + sDomains + "'").Length;

                                    if ((iInnerCount + iOuterCount) > iCap)
                                    {
                                        dtMessage.Rows[0][1] += "<font color = 'DarkCyan'>Domain count exceeded from <font color = 'Tomato'>" + iCap + "</font> to <font color = 'Tomato'>" + (iInnerCount + iOuterCount) + "</font> </font>. Domain : <font color = 'Tomato'>" + sDomains + "</font>" + Environment.NewLine;
                                    }
                                }
                            }

                            //DataRow[] drrEmailDimain = dtPickList.Select("PicklistCategory = 'BlackList' AND PicklistValue IN (" + sEmails + ")");

                            //if (drrEmailDimain.Length > 0)
                            //{
                            //    DataTable dtEmailDomain = drrEmailDimain.CopyToDataTable().DefaultView.ToTable(true, "PicklistValue"); ;
                            //    foreach (DataRow dr in dtEmailDomain.Rows)
                            //        dtMessage.Rows[0][1] += "<font color = 'DarkCyan'>Blacklisted Domain found : </font> <font color = 'Tomato'>" + dr["PicklistValue"] + "</font>" + Environment.NewLine;
                            //}
                        }
                    }
                }


                List<string> lstFreezedContactIDString = new List<string>();
                List<int> lstFreezedContactIDs = new List<int>();

                if (sFreezedCIDs.Length > 0)
                {
                    lstFreezedContactIDString = sFreezedCIDs.Split('~').ToList();
                    foreach (string s in lstFreezedContactIDString)
                        lstFreezedContactIDs.Add(Convert.ToInt32(s));
                }

                DataRow[] drrContactsToValidateNew = dtMasterContacts.Select("(TR_CONTACT_STATUS IN (" + sTREmailCheckContactStatus + ") OR WR_CONTACT_STATUS IN (" + sWREmailCheckContactStatus + "))");

                string sEmails = string.Empty;
                if (drrContactsToValidateNew.Length > 0)
                {
                    foreach (DataRow dr in drrContactsToValidateNew)
                    {
                        if (dr["CONTACT_EMAIL"].ToString().Length > 0 && dr["CONTACT_EMAIL"].ToString().Contains("@"))
                        {
                            if (dr["CONTACT_ID_P"].ToString().Length > 0 && lstFreezedContactIDs.Contains(Convert.ToInt32(dr["CONTACT_ID_P"])))
                                continue;//Do not check freezed records

                            if (sEmails.Length > 0)
                                sEmails += ",'" + dr["CONTACT_EMAIL"].ToString().Trim().Replace("'", "''").Split('@').ToList()[1] + "'";
                            else
                                sEmails = "'" + dr["CONTACT_EMAIL"].ToString().Trim().Replace("'", "''").Split('@').ToList()[1] + "'";

                            //if (dr["CONTACT_EMAIL"].ToString().ToUpper().Trim() == dr["Source_Email"].ToString().ToUpper().Trim())
                            //    dtMessage.Rows[0][1] += "<font color = 'DarkCyan'> " + dr["CONTACT_EMAIL"].ToString() + "</font>: Same source Email found." + Environment.NewLine;
                        }
                    }

                    if (sEmails.Length > 0)
                    {
                        DataRow[] drrEmailDimain = dtPickList.Select("PicklistCategory = 'BlackList' AND PicklistValue IN (" + sEmails + ")");
                        if (drrEmailDimain.Length > 0)
                        {
                            DataTable dtEmailDomain = drrEmailDimain.CopyToDataTable().DefaultView.ToTable(true, "PicklistValue"); ;
                            foreach (DataRow dr in dtEmailDomain.Rows)
                                dtMessage.Rows[0][1] += "<font color = 'DarkCyan'>Blacklisted Domain found : </font> <font color = 'Tomato'>" + dr["PicklistValue"] + "</font>" + Environment.NewLine;
                        }
                    }
                }


                DataRow[] drrDepartmentCheck= dtMasterContacts.Select("(TR_CONTACT_STATUS IN (" + sTREmailCheckContactStatus + ") OR WR_CONTACT_STATUS IN (" + sWREmailCheckContactStatus + ")) AND DEPARTMENT IN ('FINANCE','SALES')");
                if(drrDepartmentCheck.Length > 60)
                {
                    dtMessage.Rows[0][1] += "<font color = 'DarkCyan'>Deparments CAP exeeded.</font> <font color = 'Tomato'>" + Environment.NewLine;
                }




                ////Employee Size

                DataRow[] drrRestrictedEMP = dtPickList.Select("PicklistCategory = 'RestricketedEmpsize'");
                List<string> lstRestrictedEMPSize = new List<string>();

                if (drrRestrictedEMP.Length > 0)
                {
                    foreach (DataRow dr in drrRestrictedEMP)
                    {
                        lstRestrictedEMPSize.Add(dr["PicklistValue"].ToString());
                    }


                    foreach (DataRow drCom in dtMasterCompanies.Rows)
                    {
                        if (lstRestrictedEMPSize.Contains(drCom["Emp_Count"].ToString(),
                            StringComparer.OrdinalIgnoreCase))
                        {
                            DataRow[] drrMasterCotacts = dtMasterContacts.Select("(TR_CONTACT_STATUS IN (" + sTREmailCheckContactStatus + ") OR WR_CONTACT_STATUS IN (" + sWREmailCheckContactStatus + ")) AND MASTER_ID = " + drCom["MASTER_ID"].ToString());
                            if (drrMasterCotacts.Length > 0)
                            {
                                bool IsTrue = false;
                                foreach (DataRow dr in drrMasterCotacts)
                                {
                                    if (dr["CONTACT_ID_P"].ToString().Length == 0)
                                    {
                                        IsTrue = true;
                                        break;
                                    }
                                }
                                if (IsTrue)
                                    dtMessage.Rows[0][1] += "<font color = 'DarkCyan'>New contact not allowed. Employee size exceeded.</font>" + Environment.NewLine;
                            }
                        }                       
                    }
                }

                return dtMessage;


                //if (sProjectID == "INBINB008")
                //{
                //    foreach (DataRow drCom in dtMasterCompanies.Rows)
                //    {
                //        if (drCom["Emp_Count"].ToString() == "1-49")
                //        {
                //            DataRow[] drrMasterCotacts = dtMasterContacts.Select("(TR_CONTACT_STATUS IN (" + sTREmailCheckContactStatus + ") OR WR_CONTACT_STATUS IN (" + sWREmailCheckContactStatus + ")) AND MASTER_ID = " + drCom["MASTER_ID"].ToString());
                //            if (drrMasterCotacts.Length > 0)
                //            {
                //                bool IsTrue = false;
                //                foreach (DataRow dr in drrMasterCotacts)
                //                {
                //                    if (dr["CONTACT_ID_P"].ToString().Length == 0)
                //                    {
                //                        IsTrue = true;
                //                        break;
                //                    }
                //                }
                //                if (IsTrue)
                //                    dtMessage.Rows[0][1] += "<font color = 'DarkCyan'>New contact not allowed for Emp Size less then 50</font>" + Environment.NewLine;
                //            }
                //        }
                //    }
                //}
                //else if (sProjectID == "INBINB015" || sProjectID == "INBINB016")
                //{
                //    foreach (DataRow drCom in dtMasterCompanies.Rows)
                //    {
                //        if (drCom["Emp_Count"].ToString() == "1-49" || drCom["Emp_Count"].ToString() == "50-99")
                //        {
                //            DataRow[] drrMasterCotacts = dtMasterContacts.Select("(TR_CONTACT_STATUS IN (" + sTREmailCheckContactStatus + ") OR WR_CONTACT_STATUS IN (" + sWREmailCheckContactStatus + ")) AND MASTER_ID = " + drCom["MASTER_ID"].ToString());
                //            if (drrMasterCotacts.Length > 0)
                //            {
                //                bool IsTrue = false;
                //                foreach (DataRow dr in drrMasterCotacts)
                //                {
                //                    if (dr["CONTACT_ID_P"].ToString().Length == 0)
                //                    {
                //                        IsTrue = true;
                //                        break;
                //                    }
                //                }
                //                if (IsTrue)
                //                    dtMessage.Rows[0][1] += "<font color = 'DarkCyan'>New contact not allowed for Emp Size less then 100</font>" + Environment.NewLine;
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    foreach (DataRow drCom in dtMasterCompanies.Rows)
                //    {
                //        if (drCom["Emp_Count"].ToString() == "1-49" || drCom["Emp_Count"].ToString() == "50-99" || drCom["Emp_Count"].ToString() == "100-249")
                //        {
                //            DataRow[] drrMasterCotacts = dtMasterContacts.Select("(TR_CONTACT_STATUS IN (" + sTREmailCheckContactStatus + ") OR WR_CONTACT_STATUS IN (" + sWREmailCheckContactStatus + ")) AND MASTER_ID = " + drCom["MASTER_ID"].ToString());
                //            if (drrMasterCotacts.Length > 0)
                //            {
                //                bool IsTrue = false;
                //                foreach (DataRow dr in drrMasterCotacts)
                //                {
                //                    if (dr["CONTACT_ID_P"].ToString().Length == 0)
                //                    {
                //                        IsTrue = true;
                //                        break;
                //                    }
                //                }
                //                if (IsTrue)
                //                    dtMessage.Rows[0][1] += "<font color = 'DarkCyan'>New contact not allowed for Emp Size less then 250</font>" + Environment.NewLine;
                //            }
                //        }
                //    }
                //}
                
            }
            catch (Exception ex)
            {
                objEX = ex;
                dtMessage.Rows[1][1] = ex.Message;
                return dtMessage;
            }
        }

        public DataTable Table(List<DataTable> lstTable, string sTableName)
        {
            foreach (DataTable dt in lstTable)
            {
                if (dt.TableName.ToUpper() == sTableName.ToUpper())
                    return dt;
            }
            return null;
        }

        public Exception GetException()
        {
            return objEX;
        }
    }
}
