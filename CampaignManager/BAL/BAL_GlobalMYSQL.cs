using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using DAL;

namespace BAL
{
    public class BAL_Global:DAL_GlobalMySQL
    {
        public DataTable BAL_FetchTableMySQL(string sTableName, string sCondition)
        {
            
            DataTable dt_BAL = new DataTable();
            dt_BAL = DAL_FetchTableMySQL(sTableName, sCondition);
            return dt_BAL;
        }
        public bool BAL_SaveToTableMySQL(DataTable dt, string sTableName, string sExecMode, bool ShowError)
        {
            return DAL_SaveToTableMySQL(dt, sTableName, sExecMode, ShowError);
        }

        public DataTable BAL_ExecuteQueryMySQL(string sSQLText)
        {
            DataTable dt_BAL = new DataTable();
            dt_BAL =DAL_ExecuteQueryMySQL(sSQLText);
            return dt_BAL;
        }

        public DateTime BAL_GetDateTime()
        {
            return  DAL_GetDateTime();            
        }

        public DataSet BAL_ExecuteQueryMySQLSet(string sSQLText)
        {
            DataSet ds_BAL = new DataSet();
            ds_BAL = DAL_ExecuteQueryMySQLSet(sSQLText);
            return ds_BAL;
        }
        
        public void BAL_ExecuteNonReturnQueryMySQL(string sSQLText)
        {
            DAL_ExecuteNonReturnQueryMySQL(sSQLText);
        }

        public void BAL_ExecuteNonReturnQueryMySQL_ExclusiveCon(string sSQLText)
        {
            DAL_ExecuteNonReturnQueryMySQL_ExclusiveCon(sSQLText);
        }

        public void BAL_DeleteFromTableMySQL(string sTableName, string sCondition)
        {
            DAL_DeleteFromTableMySQL(sTableName, sCondition);
        }

        public void BAL_Procedure(string sProcedure, string sProjectID, string sAgentName)
        {
            DAL_Procedure(sProcedure, sProjectID, sAgentName);
        }

        public string BAL_InsertAndGetIdentityMySQL(string sSQLText)
        { 
            return DAL_InsertAndGetIdentityMySQL(sSQLText);
        }
        
       
    }
}
    