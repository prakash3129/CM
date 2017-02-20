using System;
using System.Collections.Generic;

namespace GCC
{  
    public delegate void QueryCompletedEventDelegate(object sender, QueryCompletedEventArgs args);
    public delegate void QueryErrorEventDelegate(object sender, QueryErrorDelegate args);
    public delegate void QueryChunkCompletedEventDelegate(object sender, QueryChunkCompletedEventArgs args);
    public delegate void QueryGetColumnsEventDelegate(object sender, GetColumnsEventArgs args);

    public class QueryUpdateRowCountEventArgs
    {
        public int RowCount;

        public QueryUpdateRowCountEventArgs(int rowCount)
        {
            RowCount = rowCount;
        }
    }

    public class QueryErrorDelegate : EventArgs
    {
        public Exception ex;

        public QueryErrorDelegate(Exception e)
        {
            ex = e;
        }
    }

    public class QueryCompletedEventArgs : EventArgs
    {
        public QueryCompletedEventArgs(List<object[]> data, int tableID)
        {
            Results = data;
            TableID = tableID;
        }

       
        public List<object[]> Results
        {
            get;
            set;
        }

        public int TableID
        {
            get;
            set;
        }
    }


    public class QueryChunkCompletedEventArgs : EventArgs
    {
        public QueryChunkCompletedEventArgs(List<object[]> data)
        {
            Results = data;           
        }

        public List<object[]> Results
        {
            get;
            set;
        }       
    }

    public class GetColumnsEventArgs
    {
        public GetColumnsEventArgs(QueryColumnCollection columns, int tableID)
        {
            Columns = columns;
            TableID = tableID;
        }

        public QueryColumnCollection Columns
        {
            get;
            set;
        }

        public int TableID
        {
            get;
            set;
        }
    }
}
