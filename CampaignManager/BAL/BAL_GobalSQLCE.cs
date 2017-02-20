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
    public class BAL_GobalSQLCE : DAL_GlobalSQLCE
    {
        public DataTable BAL_FetchTable(string sTableName, string sCondition)
        {

            DataTable dt_BAL = new DataTable();
            dt_BAL = DAL_FetchTable(sTableName, sCondition);
            return dt_BAL;
        }
        public void BAL_SaveToTable(DataTable dt, string sTableName, string sExecMode)
        {
            DAL_SaveToTable(dt, sTableName, sExecMode);
        }

        public DataTable BAL_ExecuteQuery(string sSQLText)
        {
            DataTable dt_BAL = new DataTable();
            dt_BAL = DAL_ExecuteQuery(sSQLText);
            return dt_BAL;
        }

        public void BAL_DeleteFromTable(string sTableName, string sCondition)
        {
            DAL_DeleteFromTable(sTableName, sCondition);
        }

        public string BAL_InsertAndGetIdentity(string sSQLText)
        {
            return DAL_InsertAndGetIdentity(sSQLText);
        }

        public void BAL_ExecuteQueryNonReturn(string sQuery)
        {
            DAL_ExecuteQueryNonReturn(sQuery);
        }
    }
}
