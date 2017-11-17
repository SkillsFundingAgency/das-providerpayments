using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using NUnit.Framework;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests
{
    [SetUpFixture]
    public class GlobalSetup
    {
        [OneTimeSetUp]
        public void BeforeAllTests()
        {
           SetupSubmissionDatabases();
            SetupPeriodEndDatabase();
        }

        private void SetupSubmissionDatabases()
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.SubmissionConnectionString))
            {
                connection.Open();

                try
                {
                    // Pre-req scripts
                    RunSqlScript(@"Ilr.Transient.DDL.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Ilr.Transient.Earnings.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Summarisation.Deds.DDL.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"DasCommitments.deds.DDL.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);

                    // Component scripts
                    RunSqlScript(@"Ilr.Transient.Reference.Commitments.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Ilr.Transient.Reference.CollectionPeriods.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Ilr.Transient.Reference.Accounts.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);

                    RunSqlScript(@"Ilr.Transient.DataLock.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Ilr.Transient.DataLock.DDL.Views.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Ilr.Transient.DataLock.DDL.Procs.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);

                    RunSqlScript(@"Ilr.Transient.Source.DataLock.DDL.Views.SchemaBinding.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            using (var connection = new SqlConnection(GlobalTestContext.Instance.SubmissionDedsConnectionString))
            {
                connection.Open();

                try
                {
                    // Component scripts
                    RunSqlScript(@"Ilr.Deds.DataLock.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                    RunSqlScript(@"Ilr.Deds.DataLock.DDL.sprocs.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"1_Ilr.Deds.DataLock.Tables.Change_Column_Types.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                    RunSqlScript(@"2_Ilr.Deds.DataLock.Tables.Change_version_id_type.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                    RunSqlScript(@"3_Ilr.Deds.DataLock.Tables.Index.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void SetupPeriodEndDatabase()
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.PeriodEndConnectionString))
            {
                connection.Open();

                try
                {
                    // Pre-req scripts
                    RunSqlScript(@"Ilr.Deds.DDL.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"Summarisation.Deds.DDL.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"DasCommitments.deds.DDL.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"Ilr.Deds.Earnings.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);

                    // Component scripts
                    RunSqlScript(@"PeriodEnd.Transient.DataLock.Reference.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);

                    RunSqlScript(@"PeriodEnd.Transient.Reference.CollectionPeriods.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Commitments.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Providers.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Accounts.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);

                    RunSqlScript(@"PeriodEnd.Deds.DataLock.DDL.sprocs.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"PeriodEnd.Transient.DataLock.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"PeriodEnd.Transient.DataLock.DDL.Views.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"PeriodEnd.Transient.DataLock.DDL.Procs.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);

                    RunSqlScript(@"PeriodEnd.Transient.Source.DataLock.DDL.Views.SchemaBinding.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                }
                finally
                {
                    connection.Close();
                }
            }

            using (var connection = new SqlConnection(GlobalTestContext.Instance.PeriodEndDedsConnectionString))
            {
                connection.Open();

                try
                {
                    // Component scripts
                    RunSqlScript(@"PeriodEnd.Deds.DataLock.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName);
                    RunSqlScript(@"PeriodEnd.Deds.DataLock.DDL.sprocs.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void RunSqlScript(string fileName, SqlConnection connection, string databaseName)
        {
            var path = Path.Combine(GlobalTestContext.Instance.AssemblyDirectory, "DbSetupScripts", fileName);
            var sql = ReplaceSqlTokens(File.ReadAllText(path), databaseName);
            var commands = sql.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var command in commands)
            {
                connection.Execute(command);
            }
        }

        private string ReplaceSqlTokens(string sql, string databaseName)
        {
            return sql.Replace("${ILR_Deds.FQ}", databaseName)
                      .Replace("${ILR_Summarisation.FQ}", databaseName)
                      .Replace("${DAS_Commitments.FQ}", databaseName)
                      .Replace("${DAS_PeriodEnd.FQ}", databaseName)
                      .Replace("${DAS_Accounts.FQ}", databaseName)
                      .Replace("${YearOfCollection}", "1617");
        }
    }
}