using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using NUnit.Framework;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.IntegrationTests
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
                    RunSqlScript(@"EAS.Deds.ddl.tables.sql", dedsConnection);
                    RunSqlScript(@"EAS.Deds.PaymentTypes.dml.sql", dedsConnection);
                    RunSqlScript(@"Summarisation.Deds.dddl.tables.sql", dedsConnection);
                    RunSqlScript(@"Summarisation.Deds.CollectionPeriods.dml.sql", dedsConnection);

                    // Component scripts
                    RunSqlScript(@"PeriodEnd.Deds.ProviderAdjustments.ddl.tables.sql", dedsConnection);
                    RunSqlScript(@"PeriodEnd.Transient.ProviderAdjustments.Reference.ddl.tables.sql", transientConnection);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.CollectionPeriods.ddl.tables.sql", transientConnection);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Commitments.ddl.tables.sql", transientConnection);
                    RunSqlScript(@"PeriodEnd.Transient.ProviderAdjustments.ddl.tables.sql", transientConnection);
                    RunSqlScript(@"PeriodEnd.Transient.ProviderAdjustments.ddl.views.sql", transientConnection);
                }
                finally
                {
                    transientConnection.Close();
                    dedsConnection.Close();
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
            return sql.Replace("${EAS_Deds.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${ILR_Summarisation.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_PeriodEnd.FQ}", GlobalTestContext.Instance.BracketedDatabaseName);
        }
    }
}