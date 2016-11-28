using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools
{
    internal static class TestDataHelper
    {
        private static readonly string[] PeriodEndCopyReferenceDataScripts =
        {
            "01 PeriodEnd.Populate.Reference.CollectionPeriods.dml.sql",
            "02 PeriodEnd.Populate.Reference.Providers.dml.sql",
            "03 PeriodEnd.Populate.Reference.Commitments.dml.sql",
            "04 PeriodEnd.Populate.Reference.Accounts.dml.sql",
            "05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql",
            "06 PeriodEnd.PaymentsDue.Populate.Reference.RequiredPaymentsHistory.dml.sql"
        };

        private static readonly Random Random = new Random();

        internal static long AddProvider(long ukprn)
        {
            Execute("INSERT INTO Valid.LearningProvider" +
                    "(UKPRN) " +
                    "VALUES " +
                    "(@ukprn)",
                new { ukprn }, false);

            Execute("INSERT INTO dbo.FileDetails (UKPRN,SubmittedTime) VALUES (@ukprn, @submissionDate)",
                new { ukprn, submissionDate = DateTime.Today }, false);

            return ukprn;
        }

        internal static void AddCommitment(long id,
                                           long ukprn,
                                           string learnerRefNumber,
                                           int aimSequenceNumber = 1,
                                           long uln = 0L,
                                           DateTime startDate = default(DateTime),
                                           DateTime endDate = default(DateTime),
                                           decimal agreedCost = 15000m,
                                           long? standardCode = null,
                                           int? programmeType = null,
                                           int? frameworkCode = null,
                                           int? pathwayCode = null,
                                           bool passedDataLock = true)
        {
            var minStartDate = new DateTime(2016, 8, 1);

            if (uln == 0)
            {
                uln = Random.Next(1, int.MaxValue);
            }

            if (!standardCode.HasValue && !programmeType.HasValue)
            {
                standardCode = 123456;
            }

            if (startDate < minStartDate)
            {
                startDate = minStartDate;
            }

            if (endDate < startDate)
            {
                endDate = startDate.AddYears(1);
            }

            Execute("INSERT INTO dbo.DasCommitments " +
                    "(CommitmentId,AccountId,Uln,Ukprn,StartDate,EndDate,AgreedCost,StandardCode,ProgrammeType,FrameworkCode,PathwayCode,PaymentStatus,PaymentStatusDescription,Payable,Priority,VersionId) " +
                    "VALUES " +
                    "(@id, '123', @uln, @ukprn, @startDate, @endDate, @agreedCost, @standardCode, @programmeType, @frameworkCode, @pathwayCode, 1, 'Active', 1, 1, '1')",
                    new { id, uln, ukprn, startDate, endDate, agreedCost, standardCode, programmeType, frameworkCode, pathwayCode }, false);

            if (passedDataLock)
            {
                Execute("INSERT INTO DataLock.DasLearnerCommitment "
                      + "(Ukprn,LearnRefNumber,AimSeqNumber,CommitmentId) "
                      + "VALUES "
                      + "(@ukprn,@learnerRefNumber,@aimSequenceNumber,@id)",
                      new { id, ukprn, learnerRefNumber, aimSequenceNumber });
            }
        }

        internal static void AddEarningForCommitment(long commitmentId,
                                                     string learnerRefNumber,
                                                     int aimSequenceNumber = 1,
                                                     string niNumber = "XX12345X",
                                                     int numberOfPeriods = 12,
                                                     int currentPeriod = 1,
                                                     DateTime? startDate = null,
                                                     DateTime? endDate = null,
                                                     DateTime? actualEndDate = null,
                                                     bool earlyFinisher = false)
        {
            Execute("INSERT INTO Rulebase.AE_LearningDelivery "
                  + "SELECT "
                  + "@learnerRefNumber, "
                  + "@aimSequenceNumber, "
                  + "Ukprn, "
                  + "Uln, "
                  + "@niNumber, "
                  + "StandardCode, "
                  + "ProgrammeType, "
                  + "FrameworkCode, "
                  + "PathwayCode, "
                  + "AgreedCost, "
                  + "COALESCE(@startDate, StartDate), "
                  + "NULL, "
                  + "COALESCE(@endDate, EndDate), "
                  + "@actualEndDate, "
                  + "(AgreedCost * 0.8) / @numberOfPeriods, "
                  + "(AgreedCost * 0.8) / @numberOfPeriods, "
                  + "AgreedCost * 0.2, "
                  + "AgreedCost * 0.2 "
                  + "FROM dbo.DasCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                new { commitmentId, learnerRefNumber, aimSequenceNumber, niNumber, numberOfPeriods, currentPeriod, startDate, endDate, actualEndDate }, false);

            Execute("INSERT INTO Rulebase.AE_LearningDelivery_PeriodisedValues "
                  + "SELECT "
                  + "Ukprn, "
                  + "@learnerRefNumber, "
                  + "@aimSequenceNumber, "
                  + "'ProgrammeAimOnProgPayment', "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= 1) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= 1) THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= 2) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= 2) THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= 3) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= 3) THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= 4) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= 4) THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= 5) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= 5) THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= 6) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= 6) THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= 7) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= 7) THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= 8) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= 8) THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= 9) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= 9) THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= 10) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= 10) THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= 11) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= 11) THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= 12) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= 12) THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END "
                  + "FROM dbo.DasCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                  new { commitmentId, learnerRefNumber, aimSequenceNumber, currentPeriod, numberOfPeriods, earlyFinisher }, false);

            Execute("INSERT INTO Rulebase.AE_LearningDelivery_PeriodisedValues "
                  + "SELECT "
                  + "Ukprn, "
                  + "@learnerRefNumber, "
                  + "@aimSequenceNumber, "
                  + "'ProgrammeAimCompletionPayment', "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = 1) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = 1) THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = 2) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = 2) THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = 3) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = 3) THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = 4) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = 4) THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = 5) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = 5) THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = 6) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = 6) THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = 7) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = 7) THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = 8) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = 8) THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = 9) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = 9) THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = 10) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = 10) THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = 11) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = 11) THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = 12) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = 12) THEN AgreedCost * 0.2 ELSE 0 END "
                  + "FROM dbo.DasCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                  new { commitmentId, learnerRefNumber, aimSequenceNumber, currentPeriod, numberOfPeriods, earlyFinisher }, false);

            Execute("INSERT INTO Rulebase.AE_LearningDelivery_PeriodisedValues "
                  + "SELECT "
                  + "Ukprn, "
                  + "@learnerRefNumber, "
                  + "@aimSequenceNumber, "
                  + "'ProgrammeAimBalPayment', "
                  + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = 1 THEN ((AgreedCost * 0.8) / @numberOfPeriods) * (@numberOfPeriods - 1) ELSE 0 END, "
                  + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = 2 THEN ((AgreedCost * 0.8) / @numberOfPeriods) * (@numberOfPeriods - 2) ELSE 0 END, "
                  + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = 3 THEN ((AgreedCost * 0.8) / @numberOfPeriods) * (@numberOfPeriods - 3) ELSE 0 END, "
                  + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = 4 THEN ((AgreedCost * 0.8) / @numberOfPeriods) * (@numberOfPeriods - 4) ELSE 0 END, "
                  + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = 5 THEN ((AgreedCost * 0.8) / @numberOfPeriods) * (@numberOfPeriods - 5) ELSE 0 END, "
                  + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = 6 THEN ((AgreedCost * 0.8) / @numberOfPeriods) * (@numberOfPeriods - 6) ELSE 0 END, "
                  + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = 7 THEN ((AgreedCost * 0.8) / @numberOfPeriods) * (@numberOfPeriods - 7) ELSE 0 END, "
                  + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = 8 THEN ((AgreedCost * 0.8) / @numberOfPeriods) * (@numberOfPeriods - 8) ELSE 0 END, "
                  + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = 9 THEN ((AgreedCost * 0.8) / @numberOfPeriods) * (@numberOfPeriods - 9) ELSE 0 END, "
                  + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = 10 THEN ((AgreedCost * 0.8) / @numberOfPeriods) * (@numberOfPeriods - 10) ELSE 0 END, "
                  + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = 11 THEN ((AgreedCost * 0.8) / @numberOfPeriods) * (@numberOfPeriods - 11) ELSE 0 END, "
                  + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = 12 THEN ((AgreedCost * 0.8) / @numberOfPeriods) * (@numberOfPeriods - 12) ELSE 0 END "
                  + "FROM dbo.DasCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                  new { commitmentId, learnerRefNumber, aimSequenceNumber, currentPeriod, numberOfPeriods, earlyFinisher }, false);

            Execute("INSERT INTO Valid.Learner "
                    + "(UKPRN,LearnRefNumber,ULN,Ethnicity,Sex,LLDDHealthProb) "
                    + "SELECT Ukprn, @learnerRefNumber,Uln,0,0,0 FROM dbo.DasCommitments "
                    + "WHERE CommitmentId = @commitmentId",
                new {commitmentId, learnerRefNumber}, false);
        }

        internal static void AddPaymentForCommitment(long commitmentId, int month, int year, int transactionType, decimal amount)
        {
            Execute("INSERT INTO PaymentsDue.RequiredPayments "
                  + "SELECT "
                  + "NEWID(), " // Id
                  + "CommitmentId, " // CommitmentId
                  + "VersionId, " // CommitmentVersionId
                  + "AccountId, " // AccountId
                  + "'NA', " // AccountVersionId
                  + "Uln, " // Uln
                  + "1, " // LearnRefNumber
                  + "1, " // AimSeqNumber
                  + "Ukprn, " // Ukprn
                  + "GETDATE(), " // IlrSubmissionDateTime
                  + "StandardCode, " // StandardCode
                  + "ProgrammeType, " // ProgrammeType
                  + "FrameworkCode, " // FrameworkCode
                  + "PathwayCode, " // PathwayCode
                  + "@month, " // DeliveryMonth
                  + "@year, " // DeliveryYear
                  + "'R01', " // CollectionPeriodName
                  + "@month, " // CollectionPeriodMonth
                  + "@year, " // CollectionPeriodYear
                  + "@transactionType, " // TransactionType
                  + "@amount " // AmountDue
                  + "FROM dbo.DasCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                  new { month, year, transactionType, amount, commitmentId }, false);
        }

        internal static void SetOpenCollection(int periodNumber)
        {
            Execute("UPDATE Collection_Period_Mapping "
                    + "SET Collection_Open = 0", null, false);

            Execute("UPDATE Collection_Period_Mapping "
                    + "SET Collection_Open = 1 "
                    + $"WHERE Collection_Period = 'R{periodNumber:00}'", null, false);
        }


        internal static RequiredPaymentEntity[] GetRequiredPaymentsForProvider(long ukprn)
        {
            return Query<RequiredPaymentEntity>("SELECT * FROM PaymentsDue.RequiredPayments WHERE Ukprn = @Ukprn ORDER BY DeliveryYear, DeliveryMonth", new { ukprn });
        }

        public static void Clean()
        {
            Execute(@"
                    DECLARE @SQL NVARCHAR(MAX) = ''

                    SELECT @SQL = (
                        SELECT 'TRUNCATE TABLE [' + s.name + '].[' + o.name + ']' + CHAR(13)
                        FROM sys.objects o WITH (NOWAIT)
                        JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
                        WHERE o.[type] = 'U'
                            AND s.name IN ('dbo', 'Valid', 'Rulebase', 'PaymentsDue')
                            AND o.name NOT IN ('Collection_Period_Mapping', 'DasAccounts')
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')

                    EXEC sys.sp_executesql @SQL                
                ");

            Execute(@"
                    DECLARE @SQL NVARCHAR(MAX) = ''

                    SELECT @SQL = (
                        SELECT 'TRUNCATE TABLE [' + s.name + '].[' + o.name + ']' + CHAR(13)
                        FROM sys.objects o WITH (NOWAIT)
                        JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
                        WHERE o.[type] = 'U'
                            AND s.name IN ('dbo', 'Valid', 'Rulebase', 'PaymentsDue')
                            AND o.name NOT IN ('Collection_Period_Mapping', 'DasAccounts')
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')

                    EXEC sys.sp_executesql @SQL                
                ", null, false);
        }

        internal static void CopyReferenceData()
        {
            foreach (var script in PeriodEndCopyReferenceDataScripts)
            {
                var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Tools\Sql\Copy Reference Data\{script}");

                var commands = ReplaceSqlTokens(sql).Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var command in commands)
                {
                    Execute(command);
                }
            }
        }

        private static void Execute(string command, object param = null, bool inTransient = true)
        {
            var connectionString = inTransient
                ? GlobalTestContext.Instance.TransientConnectionString
                : GlobalTestContext.Instance.DedsConnectionString;
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

        private static T[] Query<T>(string command, object param = null)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
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

        private static string ReplaceSqlTokens(string sql)
        {
            return sql.Replace("${ILR_Deds.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${ILR_Summarisation.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_Commitments.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_Accounts.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_PeriodEnd.FQ}", GlobalTestContext.Instance.BracketedDatabaseName);
        }
    }
}