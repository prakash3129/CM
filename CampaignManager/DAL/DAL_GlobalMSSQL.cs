using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using DevComponents.DotNetBar;
using GCC;

namespace DAL
{
    public abstract class DAL_GlobalMSSQL
    {
        //SqlConnection connection = new SqlConnection(GlobalVariables.sMSSQL);
        SqlCommand cmdtest = new SqlCommand("SELECT 1 AS Test", GV.conMSSQL);
        protected DataTable DAL_FetchTable(string sTableName, string sCondition)
        {
            string sSQL = string.Empty;
            try
            {
                
                
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                MSSQLConOpen();
                if (sCondition.Length > 0)
                    sSQL = "SELECT * FROM " + sTableName + " WHERE "+sCondition;
                else
                    sSQL = "SELECT * FROM " + sTableName + " WHERE 1=0";
                da.SelectCommand = new SqlCommand(sSQL, GV.conMSSQL);
                da.SelectCommand.CommandTimeout = 600;
                da.Fill(dt);
                GV.conMSSQL.Close();
                return dt;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true, sSQL);
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down."+Environment.NewLine+"Check your LAN or contact System Administrator.","Campaign Manager",MessageBoxButtons.OK,MessageBoxIcon.Error);
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
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                MSSQLConOpen();
                da.SelectCommand = new SqlCommand(sSQLText, GV.conMSSQL);
                da.SelectCommand.CommandTimeout = 600;
                da.Fill(dt);
                GV.conMSSQL.Close();
                return dt;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true, sSQLText);
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                SqlCommand cmd = new SqlCommand(sSQLText, GV.conMSSQL);
                cmd.CommandTimeout = 600;
                MSSQLConOpen();
                iIdentity = cmd.ExecuteScalar().ToString();
                GV.conMSSQL.Close();
                return iIdentity;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true, sSQLText);
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return string.Empty;
            }
        }

        public void DAL_SaveToTable(DataTable dt, string sTableName, string sExecMode, bool ShowError)
        {
            string sSql = "";
            try
            {
                sSql = "SELECT * FROM " + sTableName + " Where 1=0";
                //SqlConnection connection = new SqlConnection(GlobalVariables.sMSSQL);
                SqlDataAdapter da = new SqlDataAdapter(sSql, GV.conMSSQL);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                MSSQLConOpen();

                cb.ConflictOption = ConflictOption.OverwriteChanges;
                if (sExecMode == "New")
                    da.UpdateCommand = cb.GetInsertCommand();
                else if (sExecMode == "Delete")
                    da.UpdateCommand = cb.GetDeleteCommand();
                else if (sExecMode == "Update")
                    da.UpdateCommand = cb.GetUpdateCommand();
                da.UpdateCommand.CommandTimeout = 600;
                da.Update(dt);
                GV.conMSSQL.Close();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, ShowError);
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        

        public void DAL_DeleteFromTable(string sTableName, string sCondition)
        {
            string sSql = "";
            try
            {
                sSql = "DELETE FROM " + sTableName + " Where "+sCondition+" ";
                //SqlConnection connection = new SqlConnection(GlobalVariables.sMSSQL);
                SqlCommand cmd = new SqlCommand(sSql, GV.conMSSQL);
                cmd.CommandTimeout = 600;
                MSSQLConOpen();
                cmd.ExecuteNonQuery();
                GV.conMSSQL.Close();
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true, sSql);
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public bool MSSQLConOpen()//Bring back the connection if connection pool dropped
        {
            if (GV.conMSSQL.State == ConnectionState.Open)
            {
                try
                {
                    cmdtest.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        GV.conMSSQL.Open();
                        cmdtest.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex1)
                    {
                        GV.conMSSQL.Open();
                        cmdtest.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            else
            {
                try
                {
                    GV.conMSSQL.Open();
                    cmdtest.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        GV.conMSSQL.Open();
                        cmdtest.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex1)
                    {
                        GV.conMSSQL.Open();
                        cmdtest.ExecuteNonQuery();
                        return true;
                    }
                }
            }
        }
    }
}
