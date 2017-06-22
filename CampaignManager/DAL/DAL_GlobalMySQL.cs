using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using DevComponents.DotNetBar;
using MySql.Data.MySqlClient;
using GCC;


namespace DAL
{
    public abstract class DAL_GlobalMySQL 
    {
        protected DataTable DAL_FetchTableMySQL(string sTableName, string sCondition)
        {
            string sSQL = string.Empty;
            try
            {
                using (MySqlConnection conMYSQL = new MySqlConnection(GV.sMySQL))
                {
                    
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    DataTable dt = new DataTable();
                    conMYSQL.Open();
                   // MySqlConOpen();


                    if (sCondition.Length > 0)
                        sSQL = "SELECT * FROM " + sTableName + " WHERE " + sCondition;
                    else
                        sSQL = "SELECT * FROM " + sTableName + " WHERE 1=0";
                    da.SelectCommand = new MySqlCommand(sSQL, conMYSQL);
                    da.SelectCommand.CommandTimeout = 600;
                    da.Fill(dt);
                    //GV.conMYSQL.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true, sSQL);
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
        }

        protected DateTime DAL_GetDateTime()
        {
            string sSQL = string.Empty;
            try
            {
                using (MySqlConnection conMYSQL = new MySqlConnection(GV.sMySQL))
                {

                    MySqlDataAdapter da = new MySqlDataAdapter();
                    DataTable dt = new DataTable();
                    conMYSQL.Open();
                    // MySqlConOpen();                    
                    sSQL = "SELECT NOW();";
                    da.SelectCommand = new MySqlCommand(sSQL, conMYSQL);
                    da.SelectCommand.CommandTimeout = 600;
                    da.Fill(dt);
                    return Convert.ToDateTime(dt.Rows[0][0]);                    
                }
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

        protected DataTable DAL_ExecuteQueryMySQL(string sSQLText)
        {
            try
            {
                using (MySqlConnection conMYSQL = new MySqlConnection(GV.sMySQL))
                {
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    DataTable dt = new DataTable();
                    //MySqlConOpen();
                    conMYSQL.Open();
                    da.SelectCommand = new MySqlCommand(sSQLText, conMYSQL);
                    da.SelectCommand.CommandTimeout = 600;
                    da.Fill(dt);
                    //GV.conMYSQL.Close();
                    return dt;
                }
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


        protected DataSet DAL_ExecuteQueryMySQLSet(string sSQLText)
        {
            try
            {
                using (MySqlConnection conMYSQL = new MySqlConnection(GV.sMySQL))
                {
                    DataSet ds = new DataSet();
                   // MySqlConOpen();
                    conMYSQL.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(sSQLText, conMYSQL);
                    da.SelectCommand.CommandTimeout = 600;
                    da.Fill(ds);
                    //GV.conMYSQL.Close();
                    return ds;
                }

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

        protected void DAL_ExecuteNonReturnQueryMySQL(string sSQLText)
        {
            try
            {
                using (MySqlConnection conMYSQL = new MySqlConnection(GV.sMySQL))
                {
                   // MySqlConOpen();
                    conMYSQL.Open();
                    MySqlCommand cmd = new MySqlCommand(sSQLText, conMYSQL);
                    cmd.CommandTimeout = 600;
                    cmd.ExecuteNonQuery();
                 //   GV.conMYSQL.Close();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true, sSQLText);
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        protected void DAL_ExecuteNonReturnQueryMySQL_ExclusiveCon(string sSQLText)
        {
            //MySqlConnection myNewCon = null;
            try
            {
                using (MySqlConnection conMYSQL = new MySqlConnection(GV.sMySQL))
                {
                    //MySqlConnection myNewCon = new MySqlConnection(GV.sMySQL);
                    conMYSQL.Open();
                    MySqlCommand cmd = new MySqlCommand(sSQLText, conMYSQL);
                    cmd.CommandTimeout = 600;
                    cmd.ExecuteNonQuery();
                }
                
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false, sSQLText);//Supress error for Sessions
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

        public void DAL_Procedure(string sProcedure, string sProjectID, string sAgentName)
        {
            try
            {
                using (MySqlConnection conMYSQL = new MySqlConnection(GV.sMySQL))
                {
                    conMYSQL.Open();
                    MySqlCommand cmd = new MySqlCommand(sProcedure, conMYSQL);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("CREATED_BY", sAgentName));
                    cmd.Parameters.Add(new MySqlParameter("PROJECT_ID", sProjectID));                
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, false);//Supress error for Sessions
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //protected MySqlConnection DAL_GetCon()
        //{
        //    try
        //    {
        //        // MySqlConnection connection = new MySqlConnection(GlobalVariables.sMySQL);
        //        MySqlConOpen();
        //        return GV.conMYSQL;
               
        //    }
        //    catch (Exception ex)
        //    {
        //        GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
        //        if (GM.IsNetWorkDown())
        //            MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return null;
        //        //else
        //        //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    }
        //}

        protected string DAL_InsertAndGetIdentityMySQL(string sSQLText)
        {
            try
            {
                using (MySqlConnection conMYSQL = new MySqlConnection(GV.sMySQL))
                {
                    string iIdentity;
                    sSQLText += " SELECT LAST_INSERT_ID();";
                    //MySqlConnection connection = new MySqlConnection(GlobalVariables.sMySQL);
                    MySqlCommand cmd = new MySqlCommand(sSQLText, conMYSQL);
                    cmd.CommandTimeout = 600;
                   // MySqlConOpen();
                    conMYSQL.Open();
                    iIdentity = cmd.ExecuteScalar().ToString();
                    //GV.conMYSQL.Close();
                    return iIdentity;
                }
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

        public bool DAL_SaveToTableMySQL(DataTable dt, string sTableName, string sExecMode, bool ShowError)
        {
            string sSql = "";
            try
            {
                using (MySqlConnection conMYSQL = new MySqlConnection(GV.sMySQL))
                {
                    sSql = "SELECT * FROM " + sTableName + " Where 1=0";
                    // MySqlConnection connection = new MySqlConnection(GlobalVariables.sMySQL);
                    MySqlDataAdapter da = new MySqlDataAdapter(sSql, conMYSQL);
                    MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
                    //MySqlConOpen();
                    conMYSQL.Open();
                    cb.ConflictOption = ConflictOption.OverwriteChanges;
                    if (sExecMode == "New")
                        da.UpdateCommand = cb.GetInsertCommand();
                    else if (sExecMode == "Delete")
                        da.UpdateCommand = cb.GetDeleteCommand();
                    else if (sExecMode == "Update")
                        da.UpdateCommand = cb.GetUpdateCommand();

                    da.UpdateCommand.CommandTimeout = 600;
                    da.Update(dt);
                    
                    //GV.conMYSQL.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, ShowError);
                if (GM.IsNetWorkDown())
                    MessageBoxEx.Show("Network Down." + Environment.NewLine + "Check your LAN or contact System Administrator.", "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //else
                //    //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        

        public void DAL_DeleteFromTableMySQL(string sTableName, string sCondition)
        {
            string sSql = "";
            try
            {
                using (MySqlConnection conMYSQL = new MySqlConnection(GV.sMySQL))
                {
                    sSql = "DELETE FROM " + sTableName + " Where " + sCondition + " ";
                    // MySqlConnection connection = new MySqlConnection(GlobalVariables.sMySQL);

                    MySqlCommand cmd = new MySqlCommand(sSql, conMYSQL);
                    cmd.CommandTimeout = 600;
                    //MySqlConOpen();
                    conMYSQL.Open();
                    cmd.ExecuteNonQuery();
                    //GV.conMYSQL.Close();
                }
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

        //public bool MySqlConOpen()//Bring back the connection if connection pool dropped
        //{
        //    MySqlCommand cmdTimeOut = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999;", GV.conMYSQL); // Setting tiimeout on mysqlServer
        //    if (GV.conMYSQL.State == ConnectionState.Open)
        //    {
        //        try
        //        {                    
        //            GV.conMYSQL.Close();
        //            GV.conMYSQL.Open();
        //            cmdtest.ExecuteNonQuery();
        //            cmdTimeOut.ExecuteNonQuery();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            try
        //            {
        //                GV.conMYSQL.Open();
        //                cmdtest.ExecuteNonQuery();
        //                cmdTimeOut.ExecuteNonQuery();
        //                return true;
        //            }
        //            catch (Exception ex1)
        //            {
        //                GV.conMYSQL.Open();
        //                cmdtest.ExecuteNonQuery();
        //                cmdTimeOut.ExecuteNonQuery();
        //                return true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            GV.conMYSQL.Open();
        //            cmdtest.ExecuteNonQuery();
        //            cmdTimeOut.ExecuteNonQuery();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            try
        //            {
        //                GV.conMYSQL.Open();
        //                cmdtest.ExecuteNonQuery();
        //                cmdTimeOut.ExecuteNonQuery();
        //                return true;
        //            }
        //            catch (Exception ex1)
        //            {
        //                GV.conMYSQL.Open();
        //                cmdtest.ExecuteNonQuery();
        //                cmdTimeOut.ExecuteNonQuery();
        //                return true;
        //            }
        //        }             
        //    }
        //}
    }
}
