using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDateTime DATE_CONV(string DateString)
    {
        // Put your code here
        try
        {
            return Convert.ToDateTime(DateString);
        }
        catch(Exception ex)
        {
            return SqlDateTime.Null;
        }        
    }
}
