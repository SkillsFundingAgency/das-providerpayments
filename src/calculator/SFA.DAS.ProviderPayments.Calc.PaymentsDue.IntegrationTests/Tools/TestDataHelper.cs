using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools
{
    internal static class TestDataHelper
    {
        private static readonly Random Random = new Random();

        internal static long AddProvider(long ukprn)
        {
            Execute("INSERT INTO Valid.LearningProvider" +
                    "(UKPRN) " +
                    "VALUES " +
                    "(@ukprn)",
                new {ukprn}, false);

            return ukprn;
        }

        internal static void AddCommitment(string id,
                                           long ukprn,
                                           long uln = 0L,
                                           DateTime startDate = default(DateTime),
                                           DateTime endDate = default(DateTime),
                                           decimal agreedCost = 15000m,
                                           long? standardCode = null,
                                           int? programmeType = null,
                                           int? frameworkCode = null,
                                           int? pathwayCode = null)
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
                    "(CommitmentId,AccountId,Uln,Ukprn,StartDate,EndDate,AgreedCost,StandardCode,ProgrammeType,FrameworkCode,PathwayCode) " +
                    "VALUES " +
                    "(@id, 'Ac-001', @uln, @ukprn, @startDate, @endDate, @agreedCost, @standardCode, @programmeType, @frameworkCode, @pathwayCode)",
                    new { id, uln, ukprn, startDate, endDate, agreedCost, standardCode, programmeType, frameworkCode, pathwayCode }, false);
        }

        internal static void AddEarningForCommitment(string commitmentId,
                                                     string learnerRefNumber = null,
                                                     int aimSequenceNumber = 1,
                                                     string niNumber = "XX12345X",
                                                     int numberOfPeriods = 12,
                                                     int currentPeriod = 1,
                                                     DateTime? startDate = null,
                                                     DateTime? endDate = null,
                                                     DateTime? actualEndDate = null)
        {
            if (string.IsNullOrEmpty(learnerRefNumber))
            {
                learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            }

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
                  + "CASE WHEN @numberOfPeriods >= 1 THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END + CASE WHEN @numberOfPeriods = 1 THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN @numberOfPeriods >= 2 THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END + CASE WHEN @numberOfPeriods = 2 THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN @numberOfPeriods >= 3 THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END + CASE WHEN @numberOfPeriods = 3 THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN @numberOfPeriods >= 4 THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END + CASE WHEN @numberOfPeriods = 4 THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN @numberOfPeriods >= 5 THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END + CASE WHEN @numberOfPeriods = 5 THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN @numberOfPeriods >= 6 THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END + CASE WHEN @numberOfPeriods = 6 THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN @numberOfPeriods >= 7 THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END + CASE WHEN @numberOfPeriods = 7 THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN @numberOfPeriods >= 8 THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END + CASE WHEN @numberOfPeriods = 8 THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN @numberOfPeriods >= 9 THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END + CASE WHEN @numberOfPeriods = 9 THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN @numberOfPeriods >= 10 THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END + CASE WHEN @numberOfPeriods = 10 THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN @numberOfPeriods >= 11 THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END + CASE WHEN @numberOfPeriods = 11 THEN AgreedCost * 0.2 ELSE 0 END, "
                  + "CASE WHEN @numberOfPeriods >= 12 THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END + CASE WHEN @numberOfPeriods = 12 THEN AgreedCost * 0.2 ELSE 0 END "
                  + "FROM dbo.DasCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                  new { commitmentId, learnerRefNumber, aimSequenceNumber, currentPeriod, numberOfPeriods }, false);
        }

        internal static void AddPaymentForCommitment(string commitmentId, int month, int year, int transactionType, decimal amount)
        {
            Execute("INSERT INTO PaymentsDue.RequiredPayments "
                  + "SELECT "
                  + "NEWID(), "
                  + "CommitmentId, "
                  + "'', "
                  + "0, "
                  + "Ukprn, "
                  + "@month, "
                  + "@year, "
                  + "@month, "
                  + "@year, "
                  + "@transactionType, "
                  + "@amount "
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
                            AND o.name NOT IN ('Collection_Period_Mapping')
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
                            AND o.name NOT IN ('Collection_Period_Mapping')
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')

                    EXEC sys.sp_executesql @SQL                
                ", null, false);
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
    }
}