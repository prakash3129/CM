using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using DevComponents.DotNetBar;
using GCC;

namespace DAL
{
    public abstract class DAL_GlobalSQLCE
    {
        //SqlConnection connection = new SqlConnection(GlobalVariables.sMSSQL);
        SqlCeCommand cmdtest = new SqlCeCommand("SELECT 1 AS Test", GV.conSQLCE);
        protected DataTable DAL_FetchTable(string sTableName, string sCondition)
        {
            try
            {
                string sSQL;

                SqlCeDataAdapter da = new SqlCeDataAdapter();
                DataTable dt = new DataTable();
                SQLCEConOpen();
                if (sCondition.Length > 0)
                    sSQL = "SELECT * FROM " + sTableName + " WHERE " + sCondition;
                else
                    sSQL = "SELECT * FROM " + sTableName + " WHERE 1=0";
                da.SelectCommand = new SqlCeCommand(sSQL, GV.conSQLCE);
                //da.SelectCommand.CommandTimeout = 600;
                da.Fill(dt);
                //GlobalVariables.conSQLCE.Close();
                return dt;
            }
            catch (Exception ex)
            {
                GM.Error_Log(ex, true, true);
                //if (GM.IsNetWorkDown())
                //    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
        }

        protected DataTable DAL_ExecuteQuery(string sSQLText)
        {
            try
            {
                //SqlConnection connection = new SqlConnection(GlobalVariables.sMSSQL);
                SqlCeDataAdapter da = new SqlCeDataAdapter();
                DataTable dt = new DataTable();
                SQLCEConOpen();
                da.SelectCommand = new SqlCeCommand(sSQLText, GV.conSQLCE);
                //da.SelectCommand.CommandTimeout = 600;
                da.Fill(dt);
                //GlobalVariables.conSQLCE.Close();
                return dt;
            }
            catch (Exception ex)
            {
                GM.Error_Log(ex, true, true);
                //if (GM.IsNetWorkDown())
                //    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
        }

        protected string DAL_InsertAndGetIdentity(string sSQLText)
        {
            try
            {
                string iIdentity;
                sSQLText += " SELECT @@IDENTITY";
                //SqlConnection connection = new SqlConnection(GlobalVariables.sMSSQL);
                SqlCeCommand cmd = new SqlCeCommand(sSQLText, GV.conSQLCE);
                //cmd.CommandTimeout = 600;
                SQLCEConOpen();
                iIdentity = cmd.ExecuteScalar().ToString();
                //GlobalVariables.conSQLCE.Close();
                return iIdentity;
            }
            catch (Exception ex)
            {
                GM.Error_Log(ex, true, true);
                //if (GM.IsNetWorkDown())
                //    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return string.Empty;
            }
        }

        public void DAL_SaveToTable(DataTable dt, string sTableName, string sExecMode)
        {
            string sSql = "";
            try
            {
                sSql = "SELECT * FROM " + sTableName + " Where 1=0";
                //SqlConnection connection = new SqlConnection(GlobalVariables.sMSSQL);
                SqlCeDataAdapter da = new SqlCeDataAdapter(sSql, GV.conSQLCE);
                SqlCeCommandBuilder cb = new SqlCeCommandBuilder(da);
                SQLCEConOpen();

                cb.ConflictOption = ConflictOption.OverwriteChanges;
                if (sExecMode == "New")
                    da.UpdateCommand = cb.GetInsertCommand();
                else if (sExecMode == "Delete")
                    da.UpdateCommand = cb.GetDeleteCommand();
                else if (sExecMode == "Update")
                    da.UpdateCommand = cb.GetUpdateCommand();
                //da.UpdateCommand.CommandTimeout = 600;
                da.Update(dt);
                //GlobalVariables.conSQLCE.Close();
            }
            catch (Exception ex)
            {
                GM.Error_Log(ex, true, true);
                //if (GM.IsNetWorkDown())
                //    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void DAL_ExecuteQueryNonReturn(string sSql)
        {
            
            try
            {
            //    sSql = "DELETE FROM " + sTableName + " Where " + sCondition + " ";
                //SqlConnection connection = new SqlConnection(GlobalVariables.sMSSQL);

                SqlCeCommand cmd = new SqlCeCommand(sSql, GV.conSQLCE);
                //cmd.CommandTimeout = 600;
                SQLCEConOpen();

                cmd.ExecuteNonQuery();
                //GlobalVariables.conSQLCE.Close();
            }
            catch (Exception ex)
            {
                GM.Error_Log(ex, true, true);
                //if (GM.IsNetWorkDown())
                //    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }



        public void DAL_DeleteFromTable(string sTableName, string sCondition)
        {
            string sSql = "";
            try
            {

                sSql = "DELETE FROM " + sTableName + " Where " + sCondition + " ";
                //SqlConnection connection = new SqlConnection(GlobalVariables.sMSSQL);

                SqlCeCommand cmd = new SqlCeCommand(sSql, GV.conSQLCE);
                //cmd.CommandTimeout = 600;
                SQLCEConOpen();

                cmd.ExecuteNonQuery();
                //GlobalVariables.conSQLCE.Close();
            }
            catch (Exception ex)
            {
                GM.Error_Log(ex, true, true);
                //if (GM.IsNetWorkDown())
                //    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public bool SQLCEConOpen()
        {
            if (GV.conSQLCE.State == ConnectionState.Open)
            {
                try
                {
                    cmdtest.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    cmdtest.ExecuteNonQuery();
                    return true;
                }
            }
            else
            {
                try
                {
                    GV.conSQLCE.Open();
                    cmdtest.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    GV.conSQLCE.Open();
                    cmdtest.ExecuteNonQuery();
                    return true;
                }
            }
        }
    }
}
