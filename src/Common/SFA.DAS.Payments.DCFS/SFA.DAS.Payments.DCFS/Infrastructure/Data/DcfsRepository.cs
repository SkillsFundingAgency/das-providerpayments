using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FastMember;

namespace SFA.DAS.Payments.DCFS.Infrastructure.Data
{
    public abstract class DcfsRepository
    {
        private readonly string _connectionString;

        protected DcfsRepository(string connectionString)
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
        protected T QuerySingle<T>(string command, object param = null)
        {
            return Query<T>(command, param).SingleOrDefault();
        }
        protected void Execute(string command, object param = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    connection.Execute(command, param);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected void ExecuteBatch<T>(T[] batch, string destination)
        {
            var columns = typeof(T).GetProperties().Select(p => p.Name).ToArray();

            using (var bcp = new SqlBulkCopy(_connectionString))
            {
                foreach (var column in columns)
                {
                    bcp.ColumnMappings.Add(column, column);
                }

                bcp.BulkCopyTimeout = 0;
                bcp.DestinationTableName = destination;
                bcp.BatchSize = 1000;

                using (var reader = ObjectReader.Create(batch, columns))
                {
                    bcp.WriteToServer(reader);
                }
            }

        }
    }
}
