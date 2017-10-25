using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Azure;

namespace ProviderPayments.TestStack.Infrastructure.Data
{
    public abstract class SqlServerRepository
    {
        private readonly string _connectionName;

        protected SqlServerRepository(string connectionName)
        {
            _connectionName = connectionName;
        }

        protected async Task<IEnumerable<T>> Query<T>(string commandText, object parameters = null)
        {
            var connectionString = CloudConfigurationManager.GetSetting(_connectionName);
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                try
                {
                    return connection.Query<T>(commandText, parameters);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected async Task Execute(string commandText, object parameters = null)
        {
            var connectionString = CloudConfigurationManager.GetSetting(_connectionName);
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                try
                {
                    await connection.ExecuteAsync(commandText, parameters);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
