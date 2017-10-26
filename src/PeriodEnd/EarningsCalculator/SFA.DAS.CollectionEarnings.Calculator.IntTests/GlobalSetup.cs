using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using NUnit.Framework;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests
{
    [SetUpFixture]
    public class GlobalSetup
    {
        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            SetupDatabase();
        }

        private void SetupDatabase()
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.ConnectionString))
            {
                connection.Open();
                try
                {
                    // Pre-req scripts
                    RunSqlScript(@"Ilr.Transient.DDL.sql", connection);

                    // Component scripts
                    RunSqlScript(@"Ilr.Transient.Earnings.DDL.Tables.sql", connection);
                    RunSqlScript(@"Ilr.Transient.Earnings.DDL.Views.sql", connection);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private void RunSqlScript(string fileName, SqlConnection connection)
        {
            var path = Path.Combine(GlobalTestContext.Instance.AssemblyDirectory, "DbSetupScripts", fileName);
            var sql = ReplaceSqlTokens(File.ReadAllText(path));
            var commands = sql.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var command in commands)
            {
                connection.Execute(command);
            }
        }
        private string ReplaceSqlTokens(string sql)
        {
            return sql.Replace("${ILR_Current.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${ILR_Previous.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_Accounts.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_Commitments.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${ILR_Summarisation.FQ}", GlobalTestContext.Instance.BracketedDatabaseName);
        }
    }
}
