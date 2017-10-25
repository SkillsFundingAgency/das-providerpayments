using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data;

namespace ProviderPayments.TestStack.Infrastructure.Data
{
    public class SqlServerDatabaseCleaner : SqlServerRepository, IDatabaseCleaner
    {
        public SqlServerDatabaseCleaner() 
            : base("DedsConnectionString")
        {
        }

        public async Task Clean(string pattern)
        {
            var tableNames = await Query<string>("SELECT s.[name] + '.' + t.[name] [name] FROM sys.tables t INNER JOIN sys.schemas s ON t.[schema_id] = s.[schema_id]");
            foreach (var tableName in tableNames)
            {
                if (Regex.IsMatch(tableName, pattern))
                {
                    await Execute($"DELETE FROM {tableName}");
                }
            }
        }
    }
}
