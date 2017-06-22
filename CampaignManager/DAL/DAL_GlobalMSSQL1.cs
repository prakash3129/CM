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
    public abstract class DAL_GlobalMSSQL1
    {        
        SqlCommand cmdtest = new SqlCommand("SELECT 1 AS Test", GV.conMSSQL1);
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
                da.SelectCommand = new SqlCommand(sSQL, GV.conMSSQL1);
                da.SelectCommand.CommandTimeout = 600;
                da.Fill(dt);
                GV.conMSSQL1.Close();
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

        protected void DAL_ExecuteNonReturnQuery(string sSQLText)
        {
            try
            {
                //using (MySqhjlConnection conMhYSQL = new MyhSqlConnection(GV.sMySQjhL))
                //{
                // MyjhSqlConOpen();
                //conMYSjhQL.Open();
                MSSQLConOpen();
                    SqlCommand cmd = new SqlCommand(sSQLText, GV.conMSSQL1);
                    cmd.CommandTimeout = 600;
                    cmd.ExecuteNonQuery();
                GV.conMSSQL1.Close();
                //   GV.conMYjhSQL.Close();
                // }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true, sSQLText);
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                da.SelectCommand = new SqlCommand(sSQLText, GV.conMSSQL1);
                da.SelectCommand.CommandTimeout = 600;
                da.Fill(dt);
                GV.conMSSQL1.Close();
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


        public void DAL_Procedure(string sProcedure, string sProjectID, string sAgentName)
        {
            try
            {

                MSSQLConOpen();
                    SqlCommand cmd = new SqlCommand(sProcedure, GV.conMSSQL1);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("CREATED_BY", sAgentName));
                    cmd.Parameters.Add(new SqlParameter("PROJECT_ID", sProjectID));
                    cmd.ExecuteNonQuery();
                GV.conMSSQL1.Close();

            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);//Supress error for Sessions
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        protected DataSet DAL_ExecuteQuerySet(string sSQLText)
        {
            try
            {
                
                    DataSet ds = new DataSet();
                    MSSQLConOpen();                    
                    SqlDataAdapter da = new SqlDataAdapter(sSQLText, GV.conMSSQL1);
                    da.SelectCommand.CommandTimeout = 600;
                    da.Fill(ds);
                GV.conMSSQL1.Close();
                //GV.conMYhjSQL.Close();
                return ds;
                

            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true, sSQLText);
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
                SqlCommand cmd = new SqlCommand(sSQLText, GV.conMSSQL1);
                cmd.CommandTimeout = 600;
                MSSQLConOpen();
                iIdentity = cmd.ExecuteScalar().ToString();
                GV.conMSSQL1.Close();
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

        protected void DAL_ExecuteNonReturnQuery_ExclusiveCon(string sSQLText)
        {
            //MyShjqlConnection myNewCon = null;
            try
            {
                using (SqlConnection conMSSQL = new SqlConnection(GV.sMSSQL1))
                {
                    //MyShjqlConnection myNewCon = new MyShjqlConnection(GV.sMySjhQL);
                    conMSSQL.Open();
                    SqlCommand cmd = new SqlCommand(sSQLText, conMSSQL);
                    cmd.CommandTimeout = 600;
                    cmd.ExecuteNonQuery();
                    conMSSQL.Close();
                }

            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false, sSQLText);//Supress error for Sessions
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            //finally
            //{
            //    myNewCon.Close();
            //    myNewCon.Dispose();
            //    myNewCon = null;
            //}
        }
        protected DateTime DAL_GetDateTime()
        {
            string sSQL = string.Empty;
            try
            {
               
                    SqlDataAdapter da = new SqlDataAdapter();
                    DataTable dt = new DataTable();
                // conMjYSQL.Open();
                MSSQLConOpen();
                sSQL = "SELECT GETDATE();";
                    da.SelectCommand = new SqlCommand(sSQL, GV.conMSSQL1);
                    da.SelectCommand.CommandTimeout = 600;
                    da.Fill(dt);
                GV.conMSSQL1.Close();
                return Convert.ToDateTime(dt.Rows[0][0]);
                
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, true, sSQL);
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return GM.GetDateTime();
            }
        }

        public bool DAL_SaveToTable(DataTable dt, string sTableName, string sExecMode, bool ShowError)
        {
            string sSql = "";
            try
            {
                sSql = "SELECT * FROM " + sTableName + " Where 1=0";
                //SqlConnection connection = new SqlConnection(GlobalVariables.sMSSQL);
                SqlDataAdapter da = new SqlDataAdapter(sSql, GV.conMSSQL1);
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
                GV.conMSSQL1.Close();

                return true;
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, ShowError);
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
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
                SqlCommand cmd = new SqlCommand(sSql, GV.conMSSQL1);
                cmd.CommandTimeout = 600;
                MSSQLConOpen();
                cmd.ExecuteNonQuery();
                GV.conMSSQL1.Close();
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
            if (GV.conMSSQL1.State == ConnectionState.Open)
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
                        GV.conMSSQL1.Open();
                        cmdtest.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex1)
                    {
                        GV.conMSSQL1.Open();
                        cmdtest.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            else
            {
                try
                {
                    GV.conMSSQL1.Open();
                    cmdtest.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        GV.conMSSQL1.Open();
                        cmdtest.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex1)
                    {
                        GV.conMSSQL1.Open();
                        cmdtest.ExecuteNonQuery();
                        return true;
                    }
                }
            }
        }
    }
}
