using System.Data;
using System.Data.SqlClient;

namespace TestProject.Utils
{
    public class DataBaseHelper
    {
        public readonly string ConnectionString = "Data Source=YURA;Initial Catalog=Test;" +
                                                  "Integrated Security=True;Connect Timeout=30;Encrypt=False;" +
                                                  "TrustServerCertificate=False;ApplicationIntent=ReadWrite;" +
                                                  "MultiSubnetFailover=False";

        public void ExecuteSPNonQuery(string command)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(command, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
