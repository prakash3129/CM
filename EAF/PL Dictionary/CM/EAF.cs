using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

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

            //DataTable dtMessage = new DataTable("Message");
            //dtMessage.Columns.Add("Type");
            //dtMessage.Columns.Add("Message");
            //dtMessage.Rows.Add("ER", string.Empty);
            //dtMessage.Rows.Add("EX", string.Empty);

            //try
            //{
            //    DataTable dtJobTitle = new DataTable();
            //    dtJobTitle.Columns.Add("Picklistvalue");
            //    dtJobTitle.Columns.Add("Count");
            //    dtJobTitle.Rows.Add("Managing Director", "2");
            //    dtJobTitle.Rows.Add("Chief Executive Officer", "1");
            //    dtJobTitle.Rows.Add("Partner", "3");
            //    dtJobTitle.Rows.Add("Owner", "1");
            //    dtJobTitle.Rows.Add("Creative", "2");
            //    dtJobTitle.Rows.Add("Business", "1");
            //    dtJobTitle.Rows.Add("Commercial", "3");
            //    dtJobTitle.Rows.Add("Sales", "5");
            //    dtJobTitle.Rows.Add("Marketing", "1");
            //    dtJobTitle.Rows.Add("Senior Management", "1");


            //    string sAccess = Table(lstDatable, "ProjectInfo").Select("key = 'AccessTo'")[0][1].ToString();
            //    string TRContactstatusTobeValidated = Table(lstDatable, "ProjectInfo").Select("key = 'TRContactstatusTobeValidated' AND Type='String'")[0][1].ToString();
            //    DataTable dtContact = Table(lstDatable, "MasterContact");
            //    DataRow[] drrCount = dtContact.Select(sAccess + "_CONTACT_STATUS IN (" + TRContactstatusTobeValidated + ")");

            //    if (drrCount.Length > 0)
            //    {
            //        string sJobList = string.Empty;
            //        DataTable dtContactCount = drrCount.CopyToDataTable();
            //        foreach (DataRow dr in dtJobTitle.Rows)
            //        {
            //            int x = dtContactCount.Select("JobtitleCatrgory = '" + dr[0].ToString() + "'").Length;
            //            int y = Convert.ToInt32(dr[1].ToString());

            //            if (x >= y)
            //            {
            //                if (sJobList.Length == 0)
            //                    sJobList = "'" + dr[0].ToString() + "'";
            //                else
            //                    sJobList += ",'" + dr[0].ToString() + "'";
            //            }
            //        }

            //        if (sJobList.Length > 0)
            //        {
            //            DataRow[] drrReturn = dtJobTitle.Select("Picklistvalue Not in (" + sJobList + ")");
            //            if (drrReturn.Length > 0)
            //                return drrReturn.CopyToDataTable();
            //            else
            //                return dtJobTitle;
            //        }
            //        else
            //            return dtJobTitle;

            //    }
            //    else
            //        return dtJobTitle;


                
                //dtContact.Rows[Convert.ToInt32(Table(lstDatable, "ProjectInfo").Select("Key = 'ContactRowIndex'")[0][1])]["Last_Name"].ToString();

                //if (dtPickList.Select("PicklistCategory = '" + sCategory + "'").Length > 0)
                //    return dtPickList.Select("PicklistCategory = '" + sCategory + "'").CopyToDataTable();
                //else
                //    return null;




            //}
            //catch (Exception ex)
            //{
            //    objEX = ex;
            //    dtMessage.Rows[1][1] = ex.Message;
            //    return dtMessage;
            //}

           
            return null;
            
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

            //    DataTable dtCompany = Table(lstDatable, "MasterCompanies");
            //    DataTable dtContact = Table(lstDatable, "MasterContact");
            //    int iContactCount = 0;

            //    switch (dtCompany.Rows[0]["EMpBAnd"].ToString())
            //    {
            //        case "500-1000":
            //            iContactCount = 3;
            //            break;
            //        case "1000-5000":
            //            iContactCount = 4;
            //            break;
            //        case ">5000":
            //            iContactCount = 5;
            //            break;
            //        case "1-500":
            //            iContactCount = 2;
            //            break;
            //    }

            //    if (iContactCount > 0)
            //    {
            //        string sAccess = Table(lstDatable, "ProjectInfo").Select("key = 'AccessTo'")[0][1].ToString();
            //        string TRContactstatusTobeValidated = Table(lstDatable, "ProjectInfo").Select("key = 'TRContactstatusTobeValidated' AND Type='String'")[0][1].ToString();
            //        DataRow[] drrCount = dtContact.Select(sAccess + "_CONTACT_STATUS IN (" + TRContactstatusTobeValidated + ")");

            //        if (iContactCount != drrCount.Length)
            //        {
            //            dtMessage.Rows[0][1] = "Contact Count must be " + iContactCount + " if EmpBand is " + dtCompany.Rows[0]["EMpBAnd"].ToString();
            //            return dtMessage;
            //        }
            //    }

                string sPath = Table(lstDatable, "ProjectInfo").Select("key = 'SendKeyBinaryPath'")[0][1].ToString();
                sPath = sPath.Replace("SendKeys.exe", string.Empty);
                if (System.IO.File.Exists(sPath + "dic.dic.user"))
                {
                    string sDic = System.IO.File.ReadAllText(sPath + "dic.dic.user");
                    if (!sDic.Contains("sourcing"))
                        System.IO.File.WriteAllText(sPath + "dic.dic.user", sDic+Environment.NewLine+"+sourcing");                        
                }
                else
                    System.IO.File.WriteAllText(sPath + "dic.dic.user", "+sourcing");

                dtMessage.Rows[0][1] = string.Empty;
                return dtMessage;
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
