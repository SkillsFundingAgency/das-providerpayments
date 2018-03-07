using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using NUnit.Framework;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests
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
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                connection.Open();
                try
                {                    
                    connection.Execute($@"
                        if not exists(select 1 from sys.servers where name = '{GlobalTestContext.Instance.LinkedServerName}')
	                        EXEC master.dbo.sp_addlinkedserver @server = N'{GlobalTestContext.Instance.LinkedServerName}', @srvproduct = '', @provider = N'SQLNCLI', @datasrc = @@SERVERNAME;");

                    RunSqlScript(@"DasAccounts.Transient.ddl.sql", connection);
                    RunSqlScript(@"DasCommitments.Deds.ddl.sql", connection);


                    RunSqlScript(@"PeriodEnd.Transient.PaymentsDue.DDL.tables.sql", connection);
                    RunSqlScript(@"PeriodEnd.Transient.LevyPayments.ddl.tables.sql", connection);
                    RunSqlScript(@"PeriodEnd.Transient.ManualAdjustments.ddl.tables.sql", connection);
                    
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Providers.ddl.tables.sql", connection);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Providers.ddl.tables.sql", connection);
                    RunSqlScript(@"PeriodEnd.Transient.PaymentsHistory.ddl.tables.sql", connection);
                    RunSqlScript(@"Summarisation.Deds.DDL.sql", connection);
                    RunSqlScript(@"Summarisation.Deds.DML.sql", connection);


                    // Component scripts
                    RunSqlScript(@"PeriodEnd.Transient.Reference.CollectionPeriods.ddl.tables.sql", connection);

                    RunSqlScript(@"PeriodEnd.Transient.CoInvestedPayments.DDL.tables.sql", connection);
                    RunSqlScript(@"PeriodEnd.Transient.CoInvestedPayments.DDL.views.sql", connection);
                    RunSqlScript(@"PeriodEnd.Transient.CoInvestedPayments.DDL.sprocs.sql", connection);

                }
                finally
                {
                    connection.Close();
                }
            }


            using (var connection = new SqlConnection(GlobalTestContext.Instance.DedsConnectionString))
            {
                connection.Open();
                try
                {
                    // Pre-req scripts
                    RunSqlScript(@"Ilr.Deds.LearningProvider.DDL.sql", connection);
                    RunSqlScript(@"Summarisation.Deds.DDL.sql", connection);
                    RunSqlScript(@"Summarisation.Deds.DML.sql", connection);
                    RunSqlScript(@"PeriodEnd.Deds.PaymentsDue.DDL.tables.sql", connection);
                    RunSqlScript(@"DasAccounts.Transient.ddl.sql", connection);
                    RunSqlScript(@"DasCommitments.Deds.ddl.sql", connection);

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
                      .Replace("${ILR_Summarisation.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${ILR_Deds.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_PeriodEnd.FQ}", GlobalTestContext.Instance.BracketedDatabaseName);

        }
    }
}
