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
            ///////////////////////////////////////////////////////////////////////////////////
            ////////            SUBMISSION TRANSIENT
            ///////////////////////////////////////////////////////////////////////////////////
            using (var connection = new SqlConnection(GlobalTestContext.Instance.SubmissionConnectionString))
            {
                connection.Open();

                try
                {
                    connection.Execute($@"
                        if not exists(select 1 from sys.servers where name = '{GlobalTestContext.Instance.LinkedServerName}')
	                        EXEC master.dbo.sp_addlinkedserver @server = N'{GlobalTestContext.Instance.LinkedServerName}', @srvproduct = '', @provider = N'SQLNCLI', @datasrc = @@SERVERNAME;");
                    
                    // Pre-req scripts
                    RunSqlScript(@"Transient/IlrSubmission/Ilr.Transient.DDL.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Transient/IlrSubmission/Ilr.Transient.Earnings.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Transient/IlrSubmission/Ilr.Transient.Staging.ddl.views.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);

                    // Component scripts
                    RunSqlScript(@"Transient/IlrSubmission/Ilr.Transient.Reference.Commitments.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Transient/IlrSubmission/Ilr.Transient.Reference.CollectionPeriods.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Transient/IlrSubmission/Ilr.Transient.Reference.Accounts.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);

                    RunSqlScript(@"Transient/IlrSubmission/Ilr.Transient.DataLock.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Transient/IlrSubmission/Ilr.Transient.DataLock.DDL.Views.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Transient/IlrSubmission/Ilr.Transient.DataLock.DDL.Procs.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);

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

            ///////////////////////////////////////////////////////////////////////////////////
            ////////            SUBMISSION DEDS
            ///////////////////////////////////////////////////////////////////////////////////
            using (var connection = new SqlConnection(GlobalTestContext.Instance.SubmissionDedsConnectionString))
            {
                connection.Open();

                try
                {
                    RunSqlScript(@"Deds/Summarisation.Deds.DDL.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                    RunSqlScript(@"Deds/0013_Ilr.Deds.Datalock.Tables.add_validation_error_by_period.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);

                    RunSqlScript(@"Deds/ddl.deds.commitments.tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                    RunSqlScript(@"Deds/001_ddl.deds.commitments.tables.change_versionId.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                    RunSqlScript(@"Deds/0013_ddl.deds.dbo.commitments.add_transferfields.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                    RunSqlScript(@"Deds/0014_ddl.deds.dbo.commitments.add_stop_pause_fields.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);

                    RunSqlScript(@"Deds/ddl.deds.accounts.tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                    RunSqlScript(@"Deds/001_DEDS.dbo.DasAccounts_Add_TransferAllowance.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);

                    // Component scripts
                    RunSqlScript(@"Deds/Ilr.Deds.DataLock.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                    RunSqlScript(@"Deds/Ilr.Deds.DataLock.DDL.sprocs.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                    RunSqlScript(@"Deds/1_Ilr.Deds.DataLock.Tables.Change_Column_Types.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                    RunSqlScript(@"Deds/2_Ilr.Deds.DataLock.Tables.Change_version_id_type.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                    RunSqlScript(@"Deds/3_Ilr.Deds.DataLock.Tables.Index.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void SetupPeriodEndDatabase()
        {
            ///////////////////////////////////////////////////////////////////////////////////
            ////////            PERIOD END TRANSIENT
            ///////////////////////////////////////////////////////////////////////////////////
            using (var connection = new SqlConnection(GlobalTestContext.Instance.PeriodEndConnectionString))
            {
                connection.Open();

                try
                {
                    // Pre-req scripts
                    RunSqlScript(@"Transient/Common/PeriodEnd.Transient.Staging.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);

                    // Component scripts
                    RunSqlScript(@"Transient/PeriodEnd/PeriodEnd.Transient.DataLock.Reference.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);

                    RunSqlScript(@"Transient/PeriodEnd/PeriodEnd.Transient.Reference.CollectionPeriods.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"Transient/PeriodEnd/PeriodEnd.Transient.Reference.Commitments.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"Transient/PeriodEnd/PeriodEnd.Transient.Reference.Providers.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"Transient/PeriodEnd/PeriodEnd.Transient.Reference.Accounts.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);

                    RunSqlScript(@"Transient/PeriodEnd/PeriodEnd.Transient.DataLock.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"Transient/PeriodEnd/PeriodEnd.Transient.DataLock.DDL.Views.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"Transient/PeriodEnd/PeriodEnd.Transient.DataLock.DDL.Procs.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                }
                finally
                {
                    connection.Close();
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////
            ////////            PERIOD END DEDS
            ///////////////////////////////////////////////////////////////////////////////////
            using (var connection = new SqlConnection(GlobalTestContext.Instance.PeriodEndDedsConnectionString))
            {
                connection.Open();

                try
                {
                    RunSqlScript(@"Deds/Ilr.Deds.DDL.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName);
                    RunSqlScript(@"Deds/Summarisation.Deds.DDL.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName);
                    RunSqlScript(@"Deds/0013_PeriodEnd.Deds.Datalock.Tables.add_validation_error_by_period.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName);

                    RunSqlScript(@"Deds/ddl.deds.commitments.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName);

                    RunSqlScript(@"Deds/001_ddl.deds.commitments.tables.change_versionId.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName);
                    RunSqlScript(@"Deds/0013_ddl.deds.dbo.commitments.add_transferfields.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName);
                    RunSqlScript(@"Deds/0014_ddl.deds.dbo.commitments.add_stop_pause_fields.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName);
                    
                    RunSqlScript(@"Deds/Ilr.Deds.Earnings.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName);

                    RunSqlScript(@"Deds/ddl.deds.accounts.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName);
                    RunSqlScript(@"Deds/001_DEDS.dbo.DasAccounts_Add_TransferAllowance.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName);


                    // Component scripts
                    RunSqlScript(@"Deds/PeriodEnd.Deds.DataLock.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName);
                    RunSqlScript(@"Deds/PeriodEnd.Deds.DataLock.DDL.sprocs.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName);
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