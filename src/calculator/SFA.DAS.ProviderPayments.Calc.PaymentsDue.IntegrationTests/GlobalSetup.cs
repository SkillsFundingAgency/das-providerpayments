using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using NUnit.Framework;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests
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
            using (var transientConnection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            using (var dedsConnection = new SqlConnection(GlobalTestContext.Instance.DedsConnectionString))
            {
                transientConnection.Open();
                dedsConnection.Open();

                try
                {
                    // Pre-req scripts
                    RunSqlScript(@"Ilr.Deds.DDL.sql", dedsConnection);
                    RunSqlScript(@"Ilr.Deds.Earnings.DDL.sql", dedsConnection);
                    RunSqlScript(@"DasCommitments.Deds.ddl.sql", dedsConnection);
                    RunSqlScript(@"DasAccounts.Deds.DDL.sql", dedsConnection);
                    RunSqlScript(@"Summarisation.Deds.DDL.sql", dedsConnection);
                    RunSqlScript(@"Summarisation.Deds.DML.sql", dedsConnection);
                    RunSqlScript(@"DataLock.Transient.DDL.sql", transientConnection);
                    RunSqlScript(@"SeedReferenceData.sql", dedsConnection);

                    // Component scripts
                    RunSqlScript(@"PeriodEnd.Deds.PaymentsDue.DDL.tables.sql", dedsConnection);
                    RunSqlScript(@"PeriodEnd.Transient.PaymentsDue.Reference.DDL.tables.sql", transientConnection);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.CollectionPeriods.ddl.tables.sql", transientConnection);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Providers.ddl.tables.sql", transientConnection);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Accounts.ddl.tables.sql", transientConnection);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Commitments.ddl.tables.sql", transientConnection);
                    RunSqlScript(@"PeriodEnd.Transient.PaymentsDue.DDL.tables.sql", transientConnection);
                    RunSqlScript(@"PeriodEnd.Transient.PaymentsDue.DDL.views.sql", transientConnection);
                }
                finally
                {
                    transientConnection.Close();
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
            return sql.Replace("${ILR_Deds.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${ILR_Summarisation.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_Commitments.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_PeriodEnd.FQ}", GlobalTestContext.Instance.BracketedDatabaseName);
        }
    }
}