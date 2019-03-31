using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TestAssembly.Utils
{
    public class DataBaseHelper
    {
        public readonly string ConnectionString = "Data Source=YURA;Initial Catalog=Test;" +
                                                  "Integrated Security=True;Connect Timeout=120;Encrypt=False;" +
                                                  "TrustServerCertificate=False;ApplicationIntent=ReadWrite;" +
                                                  "MultiSubnetFailover=False";

        public void ExecuteNonQuery(string query)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<T> ExecuteReader<T>(string query, Func<SqlDataReader, T> converter) where T : new()
        {
            var result = new List<T>();

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            result.Add(converter(sqlDataReader));
                        }
                    }
                }
            }

            return result;
        }

        public IEnumerable<string> ExecuteReader(string query)
        {
            var result = new List<string>();

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            result.Add(sqlDataReader.GetString(0));
                        }
                    }
                }
            }

            return result;
        }
    }
}
