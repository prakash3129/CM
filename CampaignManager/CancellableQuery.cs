using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using System.Data.SqlClient;

namespace GCC
{
    public class CancellableQuery : IDisposable
    {
        // private variable used to check if the CancelQuery Method has been called
        private bool Cancelled;

        // private variable to used to store whether or not the query is currently runnnig.
        private bool isRunning;        

        //  The BackgroundWorker object that will be used to execute or read from the SQL Datasource 
        private BackgroundWorker Worker;
        
        // Event fired when the query has completed, i.e. all the data has been read from the Datasource
        public event QueryCompletedEventDelegate QueryCompleted;

        // Event fired when the query has failed for some reason
        public event QueryErrorEventDelegate OnQueryError;

        // Event Fired when the Query has been cancelled through the CancelQuery Method
        public event EventHandler QueryCancelled;

        // Event fired when a chunk of data has been read.  This is defined by the QueryChunkSize and ReturnDataInChunks Properties.
        public event QueryChunkCompletedEventDelegate QueryChunkCompleted;

        // Event fired whent he Columns returned by the SQL query have been received.
        public event QueryGetColumnsEventDelegate GetColumns;      

        /// <summary>
        /// Gets or sets the Connection string used for this Query.
        /// This currently only supports SQL Server Connection strings.
        /// </summary>
        /// 

        public CancellableQuery()
        {
            Worker = new BackgroundWorker() { WorkerSupportsCancellation = true };            
        }

        public string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the SQL that will be used to retrieve the data from the server
        /// </summary>
        
        public string CompanyTable
        {
            get;
            set;
        }

        public string ContactTable
        {
            get;
            set;
        }

        public List<string> QueryList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the chunk or number of rows that will be returned back to the client at any one time.
        /// This is only affective if the ReturnDataInChunks property is set to true.
        /// </summary>
        public int QueryChunkSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether or not the CancellableQuery will return its data in chunks or as one complete DataTable Object
        /// Set the QueryChunkSize Property and subscribe to the QueryChunkCompleted event for this to work.
        /// </summary>
        public bool ReturnDataInChunks
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a boolean value indicating if the Query is currently running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public DataTable dtQuery
        {
            get;
            set;
        }
        
        public bool ClearObjectsOnChunkCompleted
        {
            get;
            set;
        }

        int TableID = -1;
        string sContact_Company_IDs = string.Empty;
        string sContact_Company_Contact_IDs = string.Empty;
        string sCompany_Contact_IDs = string.Empty;

        string SQL = string.Empty;
        
        public void StartQueryExecution()
        {
                        
            isRunning = true;           
            //MySfgqlDataReader reader = null;
            //MyfgSqlConnection Connection = null;
                  
            //MygfSqlCommand Command = null;            
            List<object[]> Results = new List<object[]>();                      


            foreach (DataRow drQuery in dtQuery.Rows)
            {
                if (drQuery["Status"].ToString() == "0")//Untouched Query
                {
                    TableID = Convert.ToInt32(drQuery["TableID"]);

                    if (TableID == 0)                                            
                        SQL = drQuery["Query"].ToString();
                    else if (TableID == 1)
                    {
                        if (sContact_Company_IDs.Trim().Length > 0)
                            SQL = drQuery["Query"].ToString() + " WHERE MASTER_ID IN (" + sContact_Company_IDs + ")";                                                    
                        else
                        {
                            SQL = string.Empty;
                            drQuery["Status"] = "2";//Query Completed
                            continue;
                        }
                    }
                    else if (TableID == 2)
                        if (sContact_Company_IDs.Trim().Length > 0)
                            SQL = drQuery["Query"].ToString() + " WHERE MASTER_ID IN (" + sContact_Company_IDs + ")";
                        else
                        {
                            SQL = string.Empty;
                            drQuery["Status"] = "2";//Query Completed
                            continue;
                        }
                    else if (TableID == 3)
                        SQL = drQuery["Query"].ToString();
                    else if (TableID == 4)
                        if (sContact_Company_IDs.Trim().Length > 0)
                            SQL = drQuery["Query"].ToString() + " WHERE MASTER_ID IN (" + sContact_Company_IDs + ")";
                        else
                        {
                            SQL = string.Empty;
                            drQuery["Status"] = "2";//Query Completed
                            continue;
                        }
                    

                    drQuery["Status"] = "1";//Running Query
                    break;
                }
            }

            //sContact_Company_IDs = string.Empty;            
            
            try
            {
                if (TableID == 0 || TableID == 3)
                {
                    Worker.DoWork += (s, e) =>
                    {
                        try
                        {

                            //if (reader != null)
                            //{                               
                            //    reader.Close();
                            //    reader.Dispose();
                            //    reader = null;
                            //}

                            //if (Command != null)
                            //{
                                
                            //    Command.Dispose();
                            //    Command = null;
                            //}
                            

                            //if (Connection != null)
                            //{
                            //    Connection.Close();
                            //    Connection.Dispose();
                            //    Connection = null;
                            //}                            

                            if (SQL.Length > 0)
                            {
                                using (SqlConnection Connection = new SqlConnection(GV.sMSSQL1))
                                {
                                    Connection.Open();
                                    SqlCommand Command = new SqlCommand(SQL);
                                    Command.Connection = Connection;
                                    //Command.Connection = GV.conMYfSQLReader;
                                    SqlDataReader reader = Command.ExecuteReader();
                                    int CurrentRowCount = 0;
                                    while (reader.Read())
                                    {
                                        if (Cancelled)
                                        {
                                            if (QueryCancelled != null)
                                                QueryCancelled(this, EventArgs.Empty);


                                            reader.Close();
                                            reader.Dispose();
                                            Connection.Close();
                                            isRunning = false;
                                            break;
                                        }
                                        Object[] values = new Object[reader.FieldCount];
                                        reader.GetValues(values);
                                        Results.Add(values);

                                        if (values.GetValue(1).ToString().Length > 0)
                                        {
                                            switch (values.GetValue(0).ToString())
                                            {
                                                case "0":
                                                    if (sContact_Company_IDs.Length > 0)
                                                        sContact_Company_IDs += "," + values.GetValue(1).ToString();
                                                    else
                                                        sContact_Company_IDs = values.GetValue(1).ToString();
                                                    break;

                                                case "3":
                                                    if (sContact_Company_IDs.Length > 0)
                                                        sContact_Company_IDs += "," + values.GetValue(1).ToString();
                                                    else
                                                        sContact_Company_IDs = values.GetValue(1).ToString();
                                                    break;
                                            }
                                        }

                                        CurrentRowCount++;
                                        if (ReturnDataInChunks && CurrentRowCount == QueryChunkSize)
                                        {
                                            if (QueryChunkCompleted != null)
                                                QueryChunkCompleted(this, new QueryChunkCompletedEventArgs(Results));
                                            CurrentRowCount = 0;
                                            if (ClearObjectsOnChunkCompleted)
                                                Results.Clear();
                                            else Results = new List<object[]>();
                                        }
                                    }


                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // An exception has occoured somewhere, so raise the OnQueryError event if it has been subscribed to.
                            isRunning = false;
                            if (OnQueryError != null)
                                OnQueryError(this, new QueryErrorDelegate(ex));

                            // Set the isRunning varaible to false;

                        }
                    };


                    Worker.RunWorkerCompleted += (s, e) =>
                    {                        
                        
                        bool IsQueriesPending = false;
                        foreach (DataRow drQuery in dtQuery.Rows)
                        {
                            if (drQuery["TableID"].ToString() == TableID.ToString())
                            {
                                drQuery["Status"] = "2";
                                continue;
                            }

                            if (drQuery["Status"].ToString() == "0")//Untouched Query
                            {
                                IsQueriesPending = true;
                                break;
                            }
                        }
                        isRunning = IsQueriesPending;

                        if (QueryCompleted != null)
                            QueryCompleted(this, new QueryCompletedEventArgs(Results, TableID));

                        Results = new List<object[]>();

                        if(isRunning)
                            StartQueryExecution();
                    };
                }
                
                Worker.RunWorkerAsync();                
            }
            catch (Exception ex)
            {                
                if (OnQueryError != null)
                    OnQueryError(this, new QueryErrorDelegate(ex));
             
                isRunning = false;                
            }
            //finally
            //{
            //    // Do all the clean up required

            //    //if (Connection != null)
            //    //{
            //    //    Connection.Close();
            //    //    Connection.Dispose();
            //    //}

            //    //if (reader != null)
            //    //{
            //    //    reader.Close();
            //    //    reader.Dispose();
            //    //}
            //}
        }
        
        public void CancelQuery()
        {
            Cancelled = true;
            isRunning = false;
        }

        #region IDisposable Members

        public void Dispose()
        {

            if (Worker != null)
                Worker.Dispose();
        }

        #endregion

      
    }
}
