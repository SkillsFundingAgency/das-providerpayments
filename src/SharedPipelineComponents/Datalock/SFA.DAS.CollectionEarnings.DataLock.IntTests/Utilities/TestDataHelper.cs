using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Utilities
{
    public class TestDataHelper
    {
        private static readonly string[] PeriodEndCopyReferenceDataScripts =
        {
            "01 PeriodEnd.Populate.Reference.CollectionPeriods.dml.sql",
            "02 PeriodEnd.Populate.Reference.Providers.dml.sql",
            "03 PeriodEnd.Populate.Reference.Commitments.dml.sql",
            "05 PeriodEnd.DataLock.Populate.Reference.DataLockPriceEpisode.dml.sql",
            "06 PeriodEnd.Populate.Reference.Accounts.dml.sql",
            "07 PeriodEnd.DataLock.Populate.Reference.ApprenticeshipPriceEpisode_Period.dml.sql"


        };

        private static readonly string[] SubmissionCopyReferenceDataScripts =
        {
            "01 Ilr.Populate.Reference.CollectionPeriods.dml.sql",
            "02 Ilr.DataLock.Populate.Reference.DasCommitments.dml.sql",
            "03 ilr.Populate.Reference.Accounts.dml.sql"

        };


        internal static void Clean()
        {
            Clean(GlobalTestContext.Instance.SubmissionConnectionString);
            Clean(GlobalTestContext.Instance.SubmissionDedsConnectionString);
            Clean(GlobalTestContext.Instance.PeriodEndConnectionString);
            Clean(GlobalTestContext.Instance.PeriodEndDedsConnectionString);
        }

        private static void Clean(string connectionString)
        {
            Execute(connectionString,
                @"DECLARE @SQL NVARCHAR(MAX) = ''

                    SELECT @SQL = (
                        SELECT 'TRUNCATE TABLE [' + s.name + '].[' + o.name + ']' + CHAR(13)
                        FROM sys.tables o WITH (NOWAIT)
                        JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
                        WHERE o.[type] = 'U'
                            AND s.name IN ('dbo', 'Input', 'Valid', 'Invalid', 'Reference', 'DataLock', 'Rulebase')
                            AND o.object_id NOT IN (
							SELECT t.object_id
							FROM sys.tables t
							JOIN sys.schemas s1 ON t.[schema_id] = s1.[schema_id]
							WHERE
								(s1.name = 'dbo' AND t.name = 'DasCommitments')
								OR (s1.name = 'Reference' AND t.name = 'DasCommitments')
                                OR (s1.name = 'Reference' AND t.name = 'DataLockPriceEpisode')
						)
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')

                    EXEC sys.sp_executesql @SQL                

                   IF EXISTS (
							SELECT 1
							FROM sys.tables o WITH (NOWAIT)
							JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
							WHERE s.name = 'dbo'
							AND o.name = 'DasCommitments')
					BEGIN
						DELETE FROM dbo.DasCommitments
					END

                    IF EXISTS (
							SELECT 1
							FROM sys.tables o WITH (NOWAIT)
							JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
							WHERE s.name = 'Reference'
							AND o.name = 'DasCommitments')
					BEGIN
						DELETE FROM Reference.DasCommitments
					END

                    IF EXISTS (
							SELECT 1
							FROM sys.tables o WITH (NOWAIT)
							JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
							WHERE s.name = 'Reference'
							AND o.name = 'DataLockPriceEpisode')
					BEGIN
						DELETE FROM Reference.DataLockPriceEpisode
					END

                    
                ");
        }

        internal static void AddCommitment(CommitmentEntity commitment)
        {
            AddCommitment(GlobalTestContext.Instance.SubmissionConnectionString, commitment);
        }

        internal static void PeriodEndAddCommitment(CommitmentEntity commitment)
        {
            AddCommitment(GlobalTestContext.Instance.PeriodEndConnectionString, commitment);
        }

        private static void AddCommitment(string connectionString, CommitmentEntity commitment)
        {
            Execute(connectionString,
                @"INSERT INTO [dbo].[DasCommitments] (
                        CommitmentId, 
                        VersionId,
                        Uln, 
                        Ukprn, 
                        AccountId, 
                        StartDate, 
                        EndDate, 
                        AgreedCost, 
                        StandardCode, 
                        ProgrammeType, 
                        FrameworkCode, 
                        PathwayCode,
                        Priority,
                        PaymentStatus,
                        PaymentStatusDescription,
                        EffectiveFromDate,
                        EffectiveToDate
                    ) VALUES (
                        @CommitmentId, 
                        @VersionId, 
                        @Uln, 
                        @Ukprn, 
                        @AccountId, 
                        @StartDate, 
                        @EndDate, 
                        @AgreedCost, 
                        @StandardCode, 
                        @ProgrammeType, 
                        @FrameworkCode, 
                        @PathwayCode,
                        @Priority,
                        @PaymentStatus,
                        @PaymentStatusDescription,
                        @EffectiveFromDate,
                        @EffectiveToDate
                    )",
                new
                {
                    CommitmentId = commitment.CommitmentId,
                    VersionId = commitment.VersionId,
                    Uln = commitment.Uln,
                    Ukprn = commitment.Ukprn,
                    AccountId = commitment.AccountId,
                    StartDate = commitment.StartDate,
                    EndDate = commitment.EndDate,
                    AgreedCost = commitment.AgreedCost,
                    StandardCode = commitment.StandardCode,
                    ProgrammeType = commitment.StandardCode.HasValue ? (int?)null : commitment.ProgrammeType,
                    FrameworkCode = commitment.StandardCode.HasValue ? (int?)null : commitment.FrameworkCode,
                    PathwayCode = commitment.StandardCode.HasValue ? (int?)null : commitment.PathwayCode,
                    PaymentStatus = commitment.PaymentStatus,
                    PaymentStatusDescription = commitment.PaymentStatusDescription,
                    EffectiveFromDate = commitment.EffectiveFrom,
                    EffectiveToDate = commitment.EffectiveTo,
                    Priority  = commitment.Priority
                });
        }


        internal static void AddDasAccount(DasAccount account)
        {
            AddDasAccount(GlobalTestContext.Instance.SubmissionConnectionString, account);
        }

        internal static void PeriodEndAddDasAccount(DasAccount account)
        {
            AddDasAccount(GlobalTestContext.Instance.PeriodEndConnectionString, account);
        }

        private static void AddDasAccount(string connectionString, DasAccount account)
        {
            Execute(connectionString,
                @"INSERT INTO [dbo].[DasAccounts] 
                    ([AccountId],[AccountHashId],[AccountName],[Balance],[VersionId],[IsLevyPayer]) 
                    VALUES (
                        @AccountId, 
                        '',
                        '',
                        1,
                        10000,
                        @IsLevyPayer
                    )",
                new
                {
                    AccountId = account.AccountId,
                    IsLevyPayer = account.IsLevyPayer
                });
        }

        internal static void AddProvider(long ukprn)
        {
            AddProvider(ukprn, GlobalTestContext.Instance.SubmissionDedsConnectionString);
            Execute(GlobalTestContext.Instance.SubmissionConnectionString,
                "INSERT INTO [Input].[LearningProvider] (LearningProvider_Id, UKPRN) VALUES (@id, @ukprn)",
                new
                {
                    id = ukprn,
                    ukprn = ukprn
                });
        }

        internal static void AddValidProvider(long ukprn, bool inDeds = false)
        {
            AddProvider(ukprn, GlobalTestContext.Instance.SubmissionDedsConnectionString);
            AddProvider(ukprn, GlobalTestContext.Instance.SubmissionConnectionString);
        }

        internal static void AddValidLearner(long uln, bool inDeds = false)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.SubmissionDedsConnectionString
                : GlobalTestContext.Instance.SubmissionConnectionString;

            var learnRefNumber = uln.ToString();
            Execute(connectionString,
                "INSERT INTO [Valid].[Learner] (ULN,LearnRefNumber,Ethnicity,Sex,LLDDHealthProb) VALUES (@uln,@learnRefNumber,1,'M',1)",
                new
                {
                   
                    uln = uln,
                    learnRefNumber=learnRefNumber

                });
        }

        internal static void PeriodEndAddValidLearner(long ukprn, long uln)
        {
           
            var learnRefNumber = uln.ToString();
            Execute(GlobalTestContext.Instance.PeriodEndConnectionString,
                "INSERT INTO [Valid].[Learner] (UKPRN,ULN,LearnRefNumber,Ethnicity,Sex,LLDDHealthProb) VALUES (@ukprn,@uln,@learnRefNumber,1,'M',1)",
                new
                {
                    ukprn = ukprn,
                    uln = uln,
                    learnRefNumber = learnRefNumber

                });
        }


        internal static void PeriodEndAddProvider(long ukprn)
        {
            AddProvider(ukprn, GlobalTestContext.Instance.PeriodEndDedsConnectionString);
            AddProvider(ukprn, GlobalTestContext.Instance.PeriodEndConnectionString);

            Execute(GlobalTestContext.Instance.PeriodEndConnectionString,
                "INSERT INTO dbo.FileDetails (UKPRN, SubmittedTime) VALUES (@ukprn, @submittedTime)",
                new
                {
                    ukprn = ukprn,
                    submittedTime = DateTime.Today
                });
        }

        internal static void AddCollectionPeriod()
        {
            AddCollectionPeriod(GlobalTestContext.Instance.SubmissionConnectionString);
        }

        internal static void PeriodEndAddCollectionPeriod()
        {
            AddCollectionPeriod(GlobalTestContext.Instance.PeriodEndConnectionString);
        }

        private static void AddCollectionPeriod(string connectionString)
        {
            Execute(connectionString,
                " INSERT INTO [dbo].[Collection_Period_Mapping] ([Collection_Year],[Period_ID], [Return_Code],[Collection_Period_Name],[Collection_ReturnCode], [Calendar_Month], [Calendar_Year], [Collection_Open], [ActualsSchemaPeriod]) " +
                " VALUES (1617,1, 'R01','1617-R01','', 8, 2016, 1, 201608)");
        }

        private static void AddProvider(long ukprn, string connectionString)
        {
            Execute(connectionString,
                   "INSERT INTO [Valid].[LearningProvider] (UKPRN) VALUES (@ukprn)",
                   new
                   {
                       ukprn = ukprn
                   });
        }

        internal static PriceEpisodeMatchEntity[] GetPriceEpisodeMatches(bool inDeds = false)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.SubmissionDedsConnectionString
                : GlobalTestContext.Instance.SubmissionConnectionString;

            return GetPriceEpisodeMatches(connectionString);
        }

        internal static PriceEpisodeMatchEntity[] PeriodEndGetPriceEpisodeMatches(bool inDeds = false)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.PeriodEndDedsConnectionString
                : GlobalTestContext.Instance.PeriodEndConnectionString;

            return GetPriceEpisodeMatches(connectionString);
        }

        private static PriceEpisodeMatchEntity[] GetPriceEpisodeMatches(string connectionString)
        {
            return Query<PriceEpisodeMatchEntity>(connectionString, "SELECT * FROM [DataLock].[PriceEpisodeMatch]");
        }

        internal static PriceEpisodeMatchEntity[] GetPriceEpisodePeriodMatches(bool inDeds = false)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.SubmissionDedsConnectionString
                : GlobalTestContext.Instance.SubmissionConnectionString;

            return GetPriceEpisodePeriodMatches(connectionString);
        }

        internal static PriceEpisodeMatchEntity[] PeriodEndGetPriceEpisodePeriodMatches(bool inDeds = false)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.PeriodEndDedsConnectionString
                : GlobalTestContext.Instance.PeriodEndConnectionString;

            return GetPriceEpisodePeriodMatches(connectionString);
        }

        internal static PriceEpisodePeriodMatchEntity[] GetPriceEpisodePeriodMatchForPeriod(bool inDeds = false,int period = 1)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.SubmissionDedsConnectionString
                : GlobalTestContext.Instance.SubmissionConnectionString;


            return GetPriceEpisodePeriodMatchForPeriod(connectionString,period);
        }
        internal static PriceEpisodePeriodMatchEntity[] GetPriceEpisodePeriodMatchForPeriodEnd(bool inDeds = false, int period = 1)
        {
            var connectionString = inDeds
                 ? GlobalTestContext.Instance.PeriodEndDedsConnectionString
                 : GlobalTestContext.Instance.PeriodEndConnectionString;


            return GetPriceEpisodePeriodMatchForPeriod(connectionString, period);
        }

        internal static CommitmentEntity[] GetProviderCommitmentsForIlrSubmission(long ukprn, bool inDeds = false)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.SubmissionDedsConnectionString
                : GlobalTestContext.Instance.SubmissionConnectionString;

            return GetProviderCommitments(connectionString, ukprn);
        }

        private static PriceEpisodeMatchEntity[] GetPriceEpisodePeriodMatches(string connectionString)
        {
            return Query<PriceEpisodeMatchEntity>(connectionString, "SELECT * FROM [DataLock].[PriceEpisodePeriodMatch]");
        }

        private static CommitmentEntity[] GetProviderCommitments(string connectionString, long ukprn)
        {
            return Query<CommitmentEntity>(connectionString, "SELECT * FROM DataLock.vw_Commitments WHERE ProviderUkprn = @ukprn", new { ukprn });
        }

        private static PriceEpisodePeriodMatchEntity[] GetPriceEpisodePeriodMatchForPeriod(string connectionString,int period)
        {
            return Query<PriceEpisodePeriodMatchEntity>(connectionString, "SELECT * FROM [DataLock].[PriceEpisodePeriodMatch] WHERE Period=@period",new { period});
        }

        internal static ValidationErrorEntity[] GetValidationErrors(bool inDeds = false)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.SubmissionDedsConnectionString
                : GlobalTestContext.Instance.SubmissionConnectionString;

            return GetValidationErrors(connectionString);
        }

        internal static ValidationErrorEntity[] PeriodEndGetValidationErrors(bool inDeds = false)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.PeriodEndDedsConnectionString
                : GlobalTestContext.Instance.PeriodEndConnectionString;

            return GetValidationErrors(connectionString);
        }

        private static ValidationErrorEntity[] GetValidationErrors(string connectionString)
        {
            return Query<ValidationErrorEntity>(connectionString, "SELECT * FROM [DataLock].[ValidationError]");
        }

        internal static void ExecuteScript(string script, bool inDeds = false)
        {
            var databaseName = inDeds
                ? GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName
                : GlobalTestContext.Instance.BracketedSubmissionDatabaseName;

            var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Utilities\Sql\{script}");

            ExecuteScript(
                GlobalTestContext.Instance.SubmissionConnectionString,
                databaseName,
                sql);
        }

        internal static void PeriodEndExecuteScript(string script, bool inDeds = false)
        {
            var databaseName = inDeds
                ? GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName
                : GlobalTestContext.Instance.BracketedPeriodEndDatabaseName;

            var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Utilities\Sql\{script}");

            ExecuteScript(
                GlobalTestContext.Instance.PeriodEndConnectionString,
                databaseName,
                sql);
        }

        internal static void CopyReferenceData()
        {
            foreach (var script in SubmissionCopyReferenceDataScripts)
            {
                var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Utilities\Sql\Copy Reference Data\{script}");

                ExecuteScript(
                    GlobalTestContext.Instance.SubmissionConnectionString,
                    GlobalTestContext.Instance.BracketedSubmissionDatabaseName,
                    sql);
            }
        }

        internal static void PeriodEndCopyReferenceData()
        {
            foreach (var script in PeriodEndCopyReferenceDataScripts)
            {
                var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Utilities\Sql\Copy Reference Data\{script}");

                ExecuteScript(
                    GlobalTestContext.Instance.PeriodEndConnectionString,
                    GlobalTestContext.Instance.BracketedPeriodEndDatabaseName,
                    sql);
            }
        }

        private static void ExecuteScript(string connectionString, string databaseName, string sql)
        {
            var commands = ReplaceSqlTokens(sql, databaseName).Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var command in commands)
            {
                Execute(connectionString, command);
            }
        }

        private static void Execute(string connectionString, string command, object param = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    connection.Execute(command, param);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static T[] Query<T>(string connectionString, string command, object param = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    return connection.Query<T>(command, param)?.ToArray();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static string ReplaceSqlTokens(string sql, string databaseName)
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
