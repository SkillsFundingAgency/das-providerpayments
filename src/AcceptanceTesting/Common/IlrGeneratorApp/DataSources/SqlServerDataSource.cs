using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace IlrGeneratorApp.DataSources
{
    public abstract class SqlServerDataSource
    {
        private string _connectionString;

        protected void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected T[] Query<T>(string command, object param = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    return connection.Query<T>(command, param).ToArray();
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
