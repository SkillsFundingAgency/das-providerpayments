using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.IntegrationTests
{
    internal static class SqlConnectionExtensions
    {
        internal static void RunSqlScript(this SqlConnection connection, string fileName)
        {
            var path = Path.Combine(GlobalTestContext.Instance.AssemblyDirectory, "DbSetupScripts", fileName);
            var sql = ReplaceSqlTokens(File.ReadAllText(path));
            var commands = sql.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var command in commands)
            {
                connection.Execute(command);
            }
        }
        internal static string ReplaceSqlTokens(string sql)
        {
            return sql.Replace("${DAS_PeriodEnd.FQ}", GlobalTestContext.Instance.BracketedDatabaseName);
        }
    }
}
