using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Configuration;
using System.IO;

namespace TrustedBankDBFlush
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection conn = null;
            try
            {
                Console.WriteLine("Started DB Flushing...");
                conn = new SqlConnection(ConfigurationManager.AppSettings.Get("DBSRC"));

                Server server = new Server(new ServerConnection(conn));
                
                Console.WriteLine("Connected to DB...");
                var records = server.ConnectionContext.ExecuteNonQuery(ConfigurationManager.AppSettings.Get("Query"));

                var recordCount = server.ConnectionContext.ExecuteNonQuery(ConfigurationManager.AppSettings.Get("Query"));
                Console.WriteLine("Query executed " + recordCount.ToString() + " records ");
                conn.Close();
            }
            catch (Exception) { }
            finally
            {
                if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }
        }
    }
}
