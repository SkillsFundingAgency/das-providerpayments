using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.TestContext;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests
{
    public static class SqlExtensions
    {
        public static void RunDbSetupSqlScriptFile(this SqlConnection connection, string fileName, string databaseName, string linkedServerName)
        {
            var path = Path.Combine(GlobalTestContext.Current.AssemblyDirectory, "DbSetupScripts", fileName);
            RunSqlScriptFile(connection, path, databaseName, linkedServerName);
        }
        public static void RunSqlScriptFile(this SqlConnection connection, string path, string databaseName, string linkedServerName)
        {
            var sqlScript = File.ReadAllText(path);
            RunSqlScript(connection, sqlScript, databaseName, linkedServerName);
        }
        public static void RunSqlScript(this SqlConnection connection, string sqlScript, string databaseName, string linkedServerName)
        {
            var detokenizedSqlScript = ReplaceSqlTokens(sqlScript, databaseName, linkedServerName);
            var commands = detokenizedSqlScript.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var command in commands)
            {
                connection.Execute(command);
            }
        }

        private static string ReplaceSqlTokens(string sql, string databaseName, string linkedServerName)
        {
            return sql.Replace("${ILR_Deds.FQ}", databaseName)
                      .Replace("${ILR_Summarisation.FQ}", databaseName)
                      .Replace("${DAS_Commitments.FQ}", databaseName)
                      .Replace("${DAS_PeriodEnd.FQ}", databaseName)
                      .Replace("${DAS_ProviderEvents.FQ}", databaseName)
                      .Replace("${DAS_ProviderEvents.servername}", linkedServerName)
                      .Replace("${DAS_ProviderEvents.databasename}", databaseName)
                      .Replace("${DataLock_Deds.FQ}", databaseName)
                      .Replace("${YearOfCollection}", "1617");
        }
    }
}
