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
    public class BAL_GlobalMSSQL_RM : DAL_GlobalMSSQL_RM
    {
        public DataTable BAL_FetchTable(string sTableName,string sCondition)
        {
            
            DataTable dt_BAL = new DataTable();
            dt_BAL = DAL_FetchTable(sTableName, sCondition);
            return dt_BAL;
        }
        public bool BAL_SaveToTable(DataTable dt, string sTableName, string sExecMode, bool ShowError)
        {
            return DAL_SaveToTable(dt, sTableName, sExecMode, ShowError);
        }

        public DataTable BAL_ExecuteQuery(string sSQLText)
        {
            DataTable dt_BAL = new DataTable();
            dt_BAL =DAL_ExecuteQuery(sSQLText);
            return dt_BAL;
        }

        public DateTime BAL_GetDateTime()
        {
            return DAL_GetDateTime();
        }

        public void BAL_ExecuteNonReturnQuery(string sSQLText)
        {
            DAL_ExecuteNonReturnQuery(sSQLText);
        }

        public DataSet BAL_ExecuteQuerySet(string sSQLText)
        {
            DataSet ds_BAL = new DataSet();
            ds_BAL = DAL_ExecuteQuerySet(sSQLText);
            return ds_BAL;
        }

        public void BAL_Procedure(string sProcedure, string sProjectID, string sAgentName)
        {
            DAL_Procedure(sProcedure, sProjectID, sAgentName);
        }

        public void BAL_DeleteFromTable(string sTableName, string sCondition)
        {
            DAL_DeleteFromTable(sTableName, sCondition);
        }

        public void BAL_ExecuteNonReturnQuery_ExclusiveCon(string sSQLText)
        {
            DAL_ExecuteNonReturnQuery_ExclusiveCon(sSQLText);
        }

        public string BAL_InsertAndGetIdentity(string sSQLText)
        { 
            return DAL_InsertAndGetIdentity(sSQLText);
        }
    }
}
    