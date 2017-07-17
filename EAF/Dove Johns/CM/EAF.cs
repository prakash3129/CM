using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;


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
        //MSSQLConString_TimeSheet                            : user id=user1;password=M3r1t1n6i#;data source=172.27.137.181;initial catalog=MVC : String
        //MSSQLConString_CM                            : user id=user1;password=M3r1t1n6i#;data source=172.27.137.181;initial catalog=MVC : String
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

        public string ExecuteQueryMySQL(string sSQLText, string sConstring, string sProjectID)
        {
            DataTable dtMessage = new DataTable("Message");
            string sReplay = string.Empty;
            try
            {
                dtMessage.Columns.Add("Type");
                dtMessage.Columns.Add("Message");
                dtMessage.Rows.Add("ER", string.Empty);
                dtMessage.Rows.Add("EX", string.Empty);
                using (SqlConnection conMYSQL = new SqlConnection(sConstring))
                {                    
                    sSQLText += " SELECT @@IDENTITY";
                    using (SqlCommand cmd = new SqlCommand(sSQLText, conMYSQL))
                    {
                        cmd.CommandTimeout = 600;
                        conMYSQL.Open();
                        string sMasterID = cmd.ExecuteScalar().ToString();
                        if (sMasterID.Length > 0)
                        {
                            using (
                                SqlCommand cmd1 =
                                    new SqlCommand(
                                        "UPDATE " + sProjectID + "_mastercompanies SET Group_ID = MASTER_ID WHERE MASTER_ID = '" +
                                        sMasterID + "'", conMYSQL))
                            {
                                cmd1.ExecuteNonQuery();
                            }                            
                        }
                    }
                }

                sReplay = "Location Added";
            }
            catch (Exception ex)
            {
                dtMessage.Rows.Add("EX", ex.Message);
                sReplay = "Error";
            }

            return sReplay;
            //try
            //{
            //    dtMessage.Columns.Add("Type");
            //    dtMessage.Columns.Add("Message");
            //    dtMessage.Rows.Add("ER", string.Empty);
            //    dtMessage.Rows.Add("EX", string.Empty);
            //    MySqlConnection connection = new MySqlConnection(sConstring);
            //    //MySqlConnection connection = new MySqlConnection(GlobalVariables.sMySQL);
            //    MySqlDataAdapter da = new MySqlDataAdapter();
            //    DataTable dt = new DataTable();
            //    if (connection.State == ConnectionState.Closed)
            //        connection.Open();

            //    da.SelectCommand = new MySqlCommand(sSQLText, connection);
            //    da.Fill(dt);
            //    //GlobalVariables.conMYSQL.Close();
            //    return dt;
            //}
            //catch (Exception ex)
            //{
            //    dtMessage.Rows.Add("EX", "MySQL Connection Error");
            //    return null;
            //}
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
                string sUserType = dtProjectInfo.Select("Key = 'UserType'")[0]["Value"].ToString();
                string sEmpName = dtProjectInfo.Select("Key = 'EmployeeName'")[0]["Value"].ToString();
                if (dtMasterCompanies.Rows[0]["Add_Location"].ToString().ToUpper() == "YES" && sUserType.ToUpper() == "AGENT")
                {
                    DataTable dtPickList = Table(lstDatable, "PickList");
                    string sAccessType = dtProjectInfo.Select("Key = 'AccessTo'")[0]["Value"].ToString();
                    string sProjectID = dtProjectInfo.Select("Key = 'ProjectID'")[0]["Value"].ToString().ToUpper();
                    string sConstring = dtProjectInfo.Select("Key = 'MSSQLConString_CM'")[0]["Value"].ToString();

                    DataRow[] drrColumnsToinsert = dtPickList.Select("PicklistCategory = 'CopyColumn'");

                    if (drrColumnsToinsert.Length > 0)
                    {
                        string sInsertColumns = string.Empty;
                        string sInsertValues = string.Empty;
                        foreach (DataRow drColumnsToinsert in drrColumnsToinsert)
                        {
                            string sColumnName = drColumnsToinsert["PicklistValue"].ToString();
                            if (dtMasterCompanies.Columns.Contains(sColumnName))
                            {
                                if (sInsertColumns.Length > 0)
                                {
                                    sInsertColumns += "," + sColumnName;

                                    sInsertValues += "," + InsertValue(dtMasterCompanies.Columns[sColumnName].DataType.Name.ToUpper(),dtMasterCompanies.Rows[0][sColumnName].ToString());
                                }
                                else
                                {
                                    sInsertColumns = sColumnName;
                                    sInsertValues = InsertValue(
                                        dtMasterCompanies.Columns[sColumnName].DataType.Name.ToUpper(),
                                        dtMasterCompanies.Rows[0][sColumnName].ToString());
                                }
                            }
                        }                        

                        if (sInsertColumns.Length > 0)
                        {
                            string sQuery = "INSERT INTO " + sProjectID + "_mastercompanies(" + sInsertColumns + " , COMPANY_ID,CREATED_BY , CREATED_DATE) VALUES (" + sInsertValues + ",'" + dtMasterCompanies.Rows[0]["MASTER_ID"] + "','" + sEmpName + "',GETDATE());";

                           string replay =  ExecuteQueryMySQL(sQuery,sConstring,sProjectID);
                           dtMasterCompanies.Rows[0]["Add_Location"] = replay;
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

        public string InsertValue(string sType, string sValue)
        {
            if (sType == "STRING")
                sValue = "'" + sValue.Replace("'","''") + "'";
            else if (sType == "INT16" || sType == "INT32" || sType == "INT64" || sType == "DOUBLE" || sType == "DECIMAL" || sType == "UINT16" || sType == "UINT32" || sType == "UINT64")
            {
                if (sValue.Length == 0)
                    sValue = "NULL";
            }
            else if (sType == "DATETIME")
            {
                if (sValue.Length > 0)
                    sValue = "'" + sValue + "'";
                else
                    sValue = "NULL";
            }
            else
                sValue = "'" + sValue.Replace("'", "''") + "'";
            return sValue;
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
