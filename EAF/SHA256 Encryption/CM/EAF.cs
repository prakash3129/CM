using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;

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
                SqlConnection connection = new SqlConnection(sConstring);
                //MySqlConnection connection = new MySqlConnection(GlobalVariables.sMySQL);
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                da.SelectCommand = new SqlCommand(sSQLText, connection);
                da.Fill(dt);
                //GlobalVariables.conMYSQL.Close();
                return dt;
            }
            catch (Exception ex)
            {
                dtMessage.Rows.Add("EX", "MSSQL Connection Error");
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
                string sAccessType = dtProjectInfo.Select("Key = 'AccessTo'")[0]["Value"].ToString();
                string sProjectID = dtProjectInfo.Select("Key = 'ProjectID'")[0]["Value"].ToString();
                string sEmailCheckContactStatus = dtProjectInfo.Select("Key = 'EmailCheckContactStatus'")[0]["Value"].ToString();
                string sFreezedCIDs = dtProjectInfo.Select("Key = 'FreezedContactIDs'")[0]["Value"].ToString();
                string sConstring = dtProjectInfo.Select("Key = 'MSSQLConString_CM'")[0]["Value"].ToString();
                string sMaster_ID = dtMasterCompanies.Rows[0]["Master_ID"].ToString();
                
                List<string> lstFreezedContactIDString = new List<string>();
                List<int> lstFreezedContactIDs = new List<int>();
               
                if (sFreezedCIDs.Length > 0)
                {
                    lstFreezedContactIDString = sFreezedCIDs.Split('~').ToList();
                    foreach (string s in lstFreezedContactIDString)
                        lstFreezedContactIDs.Add(Convert.ToInt32(s));                    
                }

                DataTable dtMasterContacts = Table(lstDatable, "MasterContact");
                {
                    //Check email duplicate with entire contacts of project
                    DataRow[] drrContactsToValidate = dtMasterContacts.Select("(TR_CONTACT_STATUS IN (" + sEmailCheckContactStatus + ") OR WR_CONTACT_STATUS IN (" + sEmailCheckContactStatus + "))");
                                         
                    if (drrContactsToValidate.Length > 0)
                    {

                        using (DataTable dtEmailSHA2 = new DataTable())
                        {
                            dtEmailSHA2.Columns.Add("EMAIL");
                            dtEmailSHA2.Columns.Add("SHA2");
                            foreach (DataRow dr in drrContactsToValidate)
                            {
                                if (dr["CONTACT_EMAIL"].ToString().Length > 0)
                                {
                                    if (dr["CONTACT_ID_P"].ToString().Length > 0 && lstFreezedContactIDs.Contains(Convert.ToInt32(dr["CONTACT_ID_P"])))
                                        continue;//Do not check freezed records
                                    if (dr["CONTACT_EMAIL"].ToString().Trim().Length > 0)
                                    {
                                        DataRow drNewSHA = dtEmailSHA2.NewRow();
                                        drNewSHA["EMAIL"] = dr["CONTACT_EMAIL"].ToString().Trim().Replace(" ", string.Empty);
                                        drNewSHA["SHA2"] = SHA256(dr["CONTACT_EMAIL"].ToString().Trim().Replace(" ", string.Empty).ToLower());
                                        dtEmailSHA2.Rows.Add(drNewSHA);
                                    }
                                }
                            }

                        if (dtEmailSHA2.Rows.Count > 0)
                        {                            
                                DataTable dtSHAResult;
                                string sSHA2 = string.Empty;
                                foreach(DataRow drEncryptList in dtEmailSHA2.Rows)
                                {
                                    if (sSHA2.Length > 0)
                                        sSHA2 += ",'" + drEncryptList["SHA2"] + "'";
                                    else
                                        sSHA2 = "'" + drEncryptList["SHA2"] + "'";
                                }

                                //dtMasterContactsEmail = ExecuteQueryMySQL("SELECT Contact.MASTER_ID, Contact.CONTACT_EMAIL FROM " + GV.sContactTable + " Contact WHERE Contact.CONTACT_EMAIL IN (" + sEmails + ") AND (TR_CONTACT_STATUS IN (" + sEmailCheckContactStatus + ") OR WR_CONTACT_STATUS IN (" + sEmailCheckContactStatus + "));");
                                dtSHAResult = ExecuteQueryMySQL("select SHA_256 from INBOX_EXCLUSION_SHA_LIST where SHA_256 IN (" + sSHA2 + ");", sConstring);

                                if (dtSHAResult == null)
                                    dtMessage.Rows[0][1] += "Email duplicate Check:<font color = 'Red'> Error connecting server.</font>";
                                else
                                {
                                    if (dtSHAResult.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dtSHAResult.Rows)
                                        {
                                            DataRow[] drrEmailSHA2 = dtEmailSHA2.Select("SHA2 = '" + dr["SHA_256"] + "'");
                                            if (drrEmailSHA2.Length > 0)
                                            {                                                
                                                dtMessage.Rows[0][1] += "<font color = 'DarkCyan'>Email : </font> <font color = 'Tomato'>" + drrEmailSHA2[0]["EMAIL"] + "</font> already exist. SHA256:<font color = 'Tomato'>" + drrEmailSHA2[0]["SHA2"] + "</font>" + Environment.NewLine;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return dtMessage;
            }
            catch (Exception ex)
            {
                objEX = ex;
                dtMessage.Rows[1][1] = ex.Message;
                return dtMessage;
            }
        }

        static string SHA256(string password)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(password), 0, Encoding.ASCII.GetByteCount(password));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
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
