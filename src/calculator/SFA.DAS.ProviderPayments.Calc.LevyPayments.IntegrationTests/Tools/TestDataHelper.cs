using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests.Tools
{
    internal static class TestDataHelper
    {
        private readonly static Random _random = new Random();

        internal static void AddAccount(long id, string name = null, decimal balance = 999999999)
        {
            if (name == null)
            {
                name = id.ToString();
            }

            Execute("INSERT INTO dbo.DasAccounts (AccountId, AccountName, Balance) VALUES (@id, @name, @balance)", new { id, name, balance });
        }

        internal static void AddCommitment(long id, 
                                           string accountId, 
                                           long uln = 0l, 
                                           long ukprn = 0l, 
                                           DateTime startDate = default(DateTime), 
                                           DateTime endDate = default(DateTime),
                                           decimal agreedCost = 15000m,
                                           long? standardCode = null,
                                           int? programmeType = null,
                                           int? frameworkCode = null,
                                           int? pathwayCode = null,
                                           int priority = 1)
        {
            var minStartDate = new DateTime(2017, 4, 1);

            if (uln == 0)
            {
                uln = _random.Next(1, int.MaxValue);
            }
            if (ukprn == 0)
            {
                ukprn = _random.Next(1, int.MaxValue);
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
                    "(CommitmentId,AccountId,Uln,Ukprn,StartDate,EndDate,AgreedCost,StandardCode,ProgrammeType,FrameworkCode,PathwayCode,Priority,VersionId) " +
                    "VALUES " +
                    "(@id, @accountId, @uln, @ukprn, @startDate, @endDate, @agreedCost, @standardCode, @programmeType, @frameworkCode, @pathwayCode, @priority,'1')",
                    new { id, accountId, uln, ukprn, startDate, endDate, agreedCost, standardCode, programmeType, frameworkCode, pathwayCode, priority });
        }

        internal static void AddPaymentDueForCommitment(long commitmentId, 
                                                        string learnerRefNumber = null, 
                                                        int aimSequenceNumber = 1,
                                                        Common.Application.TransactionType transactionType = Common.Application.TransactionType.Learning,
                                                        decimal amountDue = 1000.00m,
                                                        int deliveryMonth = 0,
                                                        int deliveryYear = 0)
        {
            if (string.IsNullOrEmpty(learnerRefNumber))
            {
                learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            }

            if (deliveryMonth == 0)
            {
                deliveryMonth = Query<int>("SELECT Period FROM LevyPayments.vw_CollectionPeriods WHERE Collection_Open = 1").Single();
            }

            if (deliveryYear == 0)
            {
                deliveryYear = Query<int>("SELECT Calendar_Year FROM LevyPayments.vw_CollectionPeriods WHERE Collection_Open = 1").Single();
            }

            Execute("INSERT INTO PaymentsDue.RequiredPayments "
                  + "SELECT "
                  + "NEWID(), "
                  + "CommitmentId, "
                  + "@learnerRefNumber, "
                  + "@aimSequenceNumber, "
                  + "Ukprn, "
                  + "@deliveryMonth, "
                  + "@deliveryYear, "
                  + "@transactionType, "
                  + "@amountDue "
                  + "FROM dbo.DasCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                new { commitmentId, learnerRefNumber, aimSequenceNumber, transactionType, amountDue, deliveryMonth, deliveryYear });
        }

        internal static void Clean()
        {
            Execute(@"
                    DECLARE @SQL NVARCHAR(MAX) = ''

                    SELECT @SQL = (
                        SELECT 'TRUNCATE TABLE [' + s.name + '].[' + o.name + ']' + CHAR(13)
                        FROM sys.objects o WITH (NOWAIT)
                        JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
                        WHERE o.[type] = 'U'
                            AND s.name IN ('dbo', 'PaymentsDue', 'LevyPayments')
                            AND o.name NOT IN ('Collection_Period_Mapping')
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')

                    EXEC sys.sp_executesql @SQL                
                ");
        }

        internal static PaymentEntity[] GetPaymentsForCommitment(long commitmentId)
        {
            return Query<PaymentEntity>("SELECT * FROM LevyPayments.Payments WHERE CommitmentId = @commitmentId", new { commitmentId });
        }


        private static void Execute(string command, object param = null)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.ConnectionString))
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
            using (var connection = new SqlConnection(GlobalTestContext.Instance.ConnectionString))
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
