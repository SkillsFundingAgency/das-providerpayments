using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using NUnit.Framework;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.IntegrationTests
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
                    RunSqlScript(@"Ilr.Deds.Earnings.DDL.sql", connection);
                    RunSqlScript(@"Ilr.Transient.DataLock.DDL.sql", connection);
                    RunSqlScript(@"DasAccounts.Deds.ddl.sql", connection);

                    // Component scripts
                    RunSqlScript(@"Summarisation.Transient.LevyPayments.DDL.tables.sql", connection);
                    RunSqlScript(@"Summarisation.Transient.LevyPayments.DDL.views.sql", connection);
                    RunSqlScript(@"Summarisation.Transient.LevyPayments.DDL.sprocs.sql", connection);
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
            return sql.Replace("${ILR_Current.FQ}", GlobalTestContext.Instance.DatabaseName)
                      .Replace("${ILR_Previous.FQ}", GlobalTestContext.Instance.DatabaseName)
                      .Replace("${DAS_Accounts.FQ}", GlobalTestContext.Instance.DatabaseName)
                      .Replace("${DAS_Commitments.FQ}", GlobalTestContext.Instance.DatabaseName);
        }
    }
}
