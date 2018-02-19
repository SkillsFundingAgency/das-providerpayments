using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using SFA.DAS.Provider.Events.Submission.IntegrationTests.TestContext;

namespace SFA.DAS.Provider.Events.Submission.IntegrationTests
{
    public static class SqlExtensions
    {
        public static void RunDbSetupSqlScriptFile(this SqlConnection connection, string fileName, string databaseName, string dedsServerName)
        {
            try
            {
                var path = Path.Combine(GlobalTestContext.Current.AssemblyDirectory, "DbSetupScripts", fileName);
                RunSqlScriptFile(connection, path, databaseName, dedsServerName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error running script file {fileName} - {ex.Message}");
            }
        }
        public static void RunSqlScriptFile(this SqlConnection connection, string path, string databaseName, string dedsServerName)
        {
            var sqlScript = File.ReadAllText(path);
            RunSqlScript(connection, sqlScript, databaseName, dedsServerName);
        }
        public static void RunSqlScript(this SqlConnection connection, string sqlScript, string databaseName, string dedsServerName)
        {
            var detokenizedSqlScript = ReplaceSqlTokens(sqlScript, databaseName, dedsServerName);
            var commands = detokenizedSqlScript.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var command in commands)
            {
                try
                {
                    connection.Execute(command);
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex.Message}\n(command: {command})");
                }
            }
        }

        private static string ReplaceSqlTokens(string sql, string databaseName, string dedsServerName)
        {
            return sql.Replace("${ILR_Deds.FQ}", databaseName)
                      .Replace("${ILR_Summarisation.FQ}", databaseName)
                      .Replace("${DAS_Commitments.FQ}", databaseName)
                      .Replace("${DAS_PeriodEnd.FQ}", databaseName)
                      .Replace("${DAS_ProviderEvents.FQ}", databaseName)
                      .Replace("${DAS_ProviderEvents.servername}", dedsServerName)
                      .Replace("${DAS_ProviderEvents.databasename}", databaseName)
                      .Replace("${YearOfCollection}", "1617");
        }
    }
}
