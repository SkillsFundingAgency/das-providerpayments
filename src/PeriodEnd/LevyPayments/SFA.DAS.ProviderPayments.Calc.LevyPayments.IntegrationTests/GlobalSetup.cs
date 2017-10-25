using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using NUnit.Framework;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests
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
                    RunSqlScript(@"Ilr.Deds.LearningProvider.DDL.sql", connection);

                    RunSqlScript(@"DasCommitments.Deds.ddl.sql", connection);
                    RunSqlScript(@"DasAccounts.Deds.ddl.sql", connection);
                    RunSqlScript(@"Summarisation.Deds.DDL.sql", connection);
                    RunSqlScript(@"Summarisation.Deds.DML.sql", connection);
                    RunSqlScript(@"Summarisation.Transient.PaymentsDue.DDL.tables.sql", connection);

                    // Component scripts
                    RunSqlScript(@"PeriodEnd.Transient.Reference.CollectionPeriods.ddl.tables.sql", connection);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Commitments.ddl.tables.sql", connection);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Accounts.ddl.tables.sql", connection);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Providers.ddl.tables.sql", connection);
                    RunSqlScript(@"PeriodEnd.Transient.ManualAdjustments.ddl.tables.sql", connection);

                    RunSqlScript(@"PeriodEnd.Transient.LevyPayments.DDL.tables.sql", connection);
                    RunSqlScript(@"PeriodEnd.Transient.LevyPayments.DDL.views.sql", connection);
                    RunSqlScript(@"PeriodEnd.Transient.LevyPayments.DDL.sprocs.sql", connection);

                    RunSqlScript(@"PeriodEnd.Transient.PaymentsHistory.ddl.tables.sql", connection);
                   
                    

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
            return sql.Replace("${DAS_Accounts.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_Commitments.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${ILR_Summarisation.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_PeriodEnd.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                       .Replace("$${ILR_Deds.FQ}", GlobalTestContext.Instance.BracketedDatabaseName);


        }
    }
}
