using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
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

        void Format(DataRow[] drrFormat, DataRow drCompanies, DataColumn dcCompanyColumns, DataTable dtMessage, bool IsCompany)
        {
            if (drrFormat.Length > 0)
            {
                string sValue = drCompanies[dcCompanyColumns.ColumnName].ToString().Trim();
                foreach (DataRow drFormat in drrFormat)
                {
                    if (drFormat["SEARCH_TYPE"].ToString() == "CONTAINS" && sValue.Contains(drFormat["FROM_VALUE"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (drFormat["OPERATION_TYPE"].ToString() == "REPLACE")
                        {
                            //if (sValue.Contains(drFormat["FROM_VALUE"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                            sValue = sValue.Replace(drFormat["FROM_VALUE"].ToString(), drFormat["TO_VALUE"].ToString(), StringComparison.CurrentCultureIgnoreCase);
                        }
                        else if (drFormat["OPERATION_TYPE"].ToString() == "BLOCK")
                            dtMessage.Rows[0][1] += dcCompanyColumns.ColumnName + " : <font color = 'DarkCyan'>" + drFormat["MESSAGE"] + "</font>" + Environment.NewLine;
                    }
                    else if (drFormat["SEARCH_TYPE"].ToString() == "WORD")
                    {
                        if (drFormat["OPERATION_TYPE"].ToString() == "REPLACE")
                        {
                            if (sValue.Contains(" " + drFormat["FROM_VALUE"] + " ", StringComparison.CurrentCultureIgnoreCase))
                                sValue = sValue.Replace(" " + drFormat["FROM_VALUE"] + " ", " " + drFormat["TO_VALUE"] + " ", StringComparison.CurrentCultureIgnoreCase);
                            else if (sValue.ToLower().StartsWith(drFormat["FROM_VALUE"].ToString().ToLower() + " "))
                                sValue = sValue.Replace(drFormat["FROM_VALUE"] + " ", drFormat["TO_VALUE"] + " ", StringComparison.CurrentCultureIgnoreCase);
                            else if (sValue.ToLower().EndsWith(" " + drFormat["FROM_VALUE"].ToString().ToLower()))
                                sValue = sValue.Replace(" " + drFormat["FROM_VALUE"], " " + drFormat["TO_VALUE"], StringComparison.CurrentCultureIgnoreCase);
                            else if (sValue.ToLower() == drFormat["FROM_VALUE"].ToString().ToLower())
                                sValue = sValue.Replace(drFormat["FROM_VALUE"].ToString(), drFormat["TO_VALUE"].ToString(), StringComparison.CurrentCultureIgnoreCase);
                        }
                        else if (drFormat["OPERATION_TYPE"].ToString() == "BLOCK" && (drFormat["FROM_VALUE"].ToString().ToLower() == sValue.ToLower() || sValue.Contains(" " + drFormat["FROM_VALUE"] + " ", StringComparison.CurrentCultureIgnoreCase) || sValue.ToLower().StartsWith(drFormat["FROM_VALUE"].ToString().ToLower() + " ") || sValue.ToLower().EndsWith(" " + drFormat["FROM_VALUE"].ToString().ToLower())))
                            dtMessage.Rows[0][1] += dcCompanyColumns.ColumnName + " : <font color = 'DarkCyan'>" + drFormat["MESSAGE"] + "</font>" + Environment.NewLine;
                    }
                    else if (drFormat["SEARCH_TYPE"].ToString() == "ACCENTS" && sValue != RemoveDiacritics(sValue))
                        dtMessage.Rows[0][1] += dcCompanyColumns.ColumnName + " : <font color = 'DarkCyan'>" + drFormat["MESSAGE"] + "</font>" + Environment.NewLine;
                    else if (drFormat["SEARCH_TYPE"].ToString() == "POSTCODE")
                    {
                        if ((IsCompany && drFormat["TO_VALUE"].ToString().ToUpper() == drCompanies[drFormat["CONTACT_FIELDS"].ToString()].ToString().ToUpper()) || (!IsCompany && drFormat["TO_VALUE"].ToString().ToUpper() == drCompanies[drFormat["COMPANY_FIELDS"].ToString()].ToString().ToUpper()))
                        {
                            string sPost = PostCode(drFormat["FROM_VALUE"].ToString(), sValue, dtMessage);
                            sValue = sPost;
                        }
                    }
                }
                drCompanies[dcCompanyColumns.ColumnName] = sValue.Trim();
            }
        }

        string PostCode(string sFormat, string sIn, DataTable dtMessage)
        {
            Regex rAlphaNumeric = new Regex(@"[^a-zA-Z0-9]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            if (rAlphaNumeric.Replace(sFormat, string.Empty).Length == rAlphaNumeric.Replace(sIn, string.Empty).Length)
            {
                string sInput = rAlphaNumeric.Replace(sIn, string.Empty);
                string sOutPut = string.Empty;
                int j = 0;
                for (int i = 0; i<sFormat.Length; i++)
                {                    
                    if (sFormat[i] == '0')
                    {
                        if (Regex.IsMatch(sInput[j].ToString(), @"^[0-9]+$"))
                            sOutPut += sInput[j];
                        else
                        {
                            dtMessage.Rows[0][1] += "Postcode : <font color = 'DarkCyan'>Invalid Postcode format</font>" + Environment.NewLine;
                            return sIn;
                        }
                        j++;
                    }
                    else if (sFormat[i] == 'A')
                    {
                        if (Regex.IsMatch(sInput[j].ToString(), @"^[a-zA-Z]+$"))
                            sOutPut += sInput[j];
                        else
                        {
                            dtMessage.Rows[0][1] += "Postcode : <font color = 'DarkCyan'>Invalid Postcode format</font>" + Environment.NewLine;
                            return sIn;
                        }
                        j++;
                    }
                    else if (sFormat[i] == 'X')
                    {
                        if (Regex.IsMatch(sInput[j].ToString(), @"^[a-zA-Z0-9]+$"))
                            sOutPut += sInput[j];
                        else
                        {
                            dtMessage.Rows[0][1] +=
                                "Postcode : <font color = 'DarkCyan'>Invalid Postcode format</font>" +
                                Environment.NewLine;
                            return sIn;
                        }
                        j++;
                    }
                    else
                        sOutPut += sFormat[i];                    
                }
                return sOutPut;
            }
            else
            {
                dtMessage.Rows[0][1] += "Postcode : <font color = 'DarkCyan'>Invalid Postcode format</font>" + Environment.NewLine;
                return sIn;
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
                //DataTable dtPickList = Table(lstDatable, "PickList");
                //string sAccessType = dtProjectInfo.Select("Key = 'AccessTo'")[0]["Value"].ToString();
                //string sUserType = dtProjectInfo.Select("Key = 'UserType'")[0]["Value"].ToString();                
                //string sProjectID = dtProjectInfo.Select("Key = 'ProjectID'")[0]["Value"].ToString().ToUpper();
                //string sTREmailCheckContactStatus = dtProjectInfo.Select("Key = 'TRContactstatusTobeValidated' AND Type='String'")[0]["Value"].ToString();
                //string sWREmailCheckContactStatus = dtProjectInfo.Select("Key = 'WRContactstatusTobeValidated' AND Type='String'")[0]["Value"].ToString();
                string sFreezedCIDs = dtProjectInfo.Select("Key = 'FreezedContactIDs'")[0]["Value"].ToString();
                string sConstring = dtProjectInfo.Select("Key = 'MYSQLConString'")[0]["Value"].ToString();

                DataTable dtFormatTable = ExecuteQueryMySQL("SELECT * FROM informa_lookup;", sConstring);
                
                
                //string sMaster_ID = dtMasterCompanies.Rows[0]["MASTER_ID"].ToString();                
                //foreach (DataRow drCompanies in dtMasterCompanies.Rows)
                //{
                //    if (sMaster_ID.Length > 0)
                //        sMaster_ID += "," + drCompanies["Master_ID"].ToString();
                //    else
                //        sMaster_ID = drCompanies["Master_ID"].ToString();
                //}
                               
                foreach (DataRow drCompanies in dtMasterCompanies.Rows)
                {
                    foreach (DataColumn dcCompanyColumns in drCompanies.Table.Columns)
                    {
                        DataRow[] drrFormat = dtFormatTable.Select("COMPANY_FIELDS LIKE '%|" + dcCompanyColumns.ColumnName + "|%' ", "OPERATION_PRIORITY ASC");
                        Format(drrFormat, drCompanies, dcCompanyColumns, dtMessage, true);
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

                DataTable dtMasterContacts = Table(lstDatable, "MasterContact");
                foreach (DataRow drContacts in dtMasterContacts.Rows)
                {
                    foreach (DataColumn dcContactColumns in drContacts.Table.Columns)
                    {
                        if (drContacts["CONTACT_ID_P"].ToString().Length == 0 || (drContacts["CONTACT_ID_P"].ToString().Length > 0 && !lstFreezedContactIDs.Contains(Convert.ToInt32(drContacts["CONTACT_ID_P"]))))
                        {
                            DataRow[] drrFormat =
                                dtFormatTable.Select("CONTACT_FIELDS LIKE '%|" + dcContactColumns.ColumnName + "|%' ", "OPERATION_PRIORITY ASC");
                            Format(drrFormat, drContacts, dcContactColumns, dtMessage, false);
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

        static string RemoveDiacritics(string text)
        {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                              UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
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


    public static class StringExtensions
    {
        public static string Replace(this string originalString, string oldValue, string newValue, StringComparison comparisonType)
        {
            int startIndex = 0;
            while (true)
            {
                startIndex = originalString.IndexOf(oldValue, startIndex, comparisonType);
                if (startIndex == -1)
                    break;

                originalString = originalString.Substring(0, startIndex) + newValue + originalString.Substring(startIndex + oldValue.Length);

                startIndex += newValue.Length;
            }

            return originalString;
        }

        public static bool Contains(this string source, string cont, StringComparison compare)
        {
            return source.IndexOf(cont, compare) >= 0;
        }

    }
}
