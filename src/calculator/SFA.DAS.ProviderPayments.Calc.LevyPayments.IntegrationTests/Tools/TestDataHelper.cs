using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests.Tools
{
    internal static class TestDataHelper
    {
        private static readonly string[] PeriodEndCopyReferenceDataScripts =
        {
            "01 PeriodEnd.Populate.Reference.CollectionPeriods.dml.sql",
            "02 PeriodEnd.Populate.Reference.Providers.dml.sql",
            "03 PeriodEnd.Populate.Reference.Commitments.dml.sql",
            "04 PeriodEnd.Populate.Reference.Accounts.dml.sql"



        };

        private static readonly Random Random = new Random();

        internal static void AddAccount(long id, string name = null, decimal balance = 999999999)
        {
            if (name == null)
            {
                name = id.ToString();
            }

            Execute("INSERT INTO dbo.DasAccounts (AccountId, AccountHashId, AccountName, Balance,VersionId,IsLevyPayer) VALUES (@id, @id, @name, @balance, '1',1)", new { id, name, balance });
        }

        internal static decimal[] GetAccountBalance(long id)
        {

            return Query<decimal>("SELECT BALANCE FROM dbo.DasAccounts WHERE AccountId = @id", new { id });
        }

        internal static void AddCommitment(long id,
                                           string accountId,
                                           long uln = 0L,
                                           long ukprn = 0L,
                                           DateTime startDate = default(DateTime),
                                           DateTime endDate = default(DateTime),
                                           decimal agreedCost = 15000m,
                                           long? standardCode = null,
                                           int? programmeType = null,
                                           int? frameworkCode = null,
                                           int? pathwayCode = null,
                                           int priority = 1,
                                           long versionId = 1)
        {
            var minStartDate = new DateTime(2017, 4, 1);

            if (uln == 0)
            {
                uln = Random.Next(1, int.MaxValue);
            }
            if (ukprn == 0)
            {
                ukprn = Random.Next(1, int.MaxValue);
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
                    "(CommitmentId,VersionId,AccountId,Uln,Ukprn,StartDate,EndDate,AgreedCost,StandardCode,ProgrammeType,FrameworkCode,PathwayCode,PaymentStatus,PaymentStatusDescription,Priority,EffectiveFromDate) " +
                    "VALUES " +
                    "(@id, @versionId, @accountId, @uln, @ukprn, @startDate, @endDate, @agreedCost, @standardCode, @programmeType, @frameworkCode, @pathwayCode,  1, 'Active', @priority, @startDate)",
                    new { id, versionId, accountId, uln, ukprn, startDate, endDate, agreedCost, standardCode, programmeType, frameworkCode, pathwayCode, priority });
        }

        internal static void AddPaymentDueForCommitment(long commitmentId,
                                                        string learnerRefNumber = null,
                                                        int aimSequenceNumber = 1,
                                                        TransactionType transactionType = TransactionType.Learning,
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
                  + "@amountDue, "
                + "  1 "
                  + "FROM dbo.DasCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                new { commitmentId, learnerRefNumber, aimSequenceNumber, transactionType, amountDue, deliveryMonth, deliveryYear });
        }


        internal static void AddPaymentHistoryForCommitment(long commitmentId)
        {

            Execute("INSERT INTO Payments.Payments "
                  + "SELECT "
                  + "NEWID(), "
                  + "Id, "
                  + "DeliveryMonth, "
                  + "DeliveryYear, "
                  + "'1617-R01', "
                  + "1, "
                  + "2017, "
                  + "1, "
                  + "TransactionType, "
                  + "AmountDue * -1"
                  + "FROM PaymentsDue.RequiredPayments "
                  + "WHERE CommitmentId = @commitmentId",
                new { commitmentId });
        }
        internal static void AddPaymentHistoryForCommitment(long commitmentId, int deliveryMonth, int deliveryYear, decimal amountDue, FundingSource source = FundingSource.Levy)
        {
            Execute("DELETE FROM Payments.Payments "
                  + "WHERE DeliveryMonth = @deliveryMonth "
                  + "AND DeliveryYear = @deliveryYear "
                  + "AND FundingSource = @source",
                new { deliveryMonth, deliveryYear, source = (int)source });

            Execute("INSERT INTO Payments.Payments "
                  + "SELECT "
                  + "NEWID(), "
                  + "Id, "
                  + "DeliveryMonth, "
                  + "DeliveryYear, "
                  + "'2017-R01', "
                  + "1, "
                  + "2017, "
                  + "@source, "
                  + "TransactionType, "
                  + "@amountDue "
                  + "FROM PaymentsDue.RequiredPayments "
                  + "WHERE CommitmentId = @commitmentId "
                  + "AND DeliveryMonth = @deliveryMonth "
                  + "AND DeliveryYear = @deliveryYear",
                new { commitmentId, deliveryMonth, deliveryYear, amountDue, source = (int)source });
        }

        internal static long AddProvider(long ukprn)
        {
            Execute("INSERT INTO Valid.LearningProvider" +
                    "(UKPRN) " +
                    "VALUES " +
                    "(@ukprn)",
                new { ukprn });

            Execute("INSERT INTO dbo.FileDetails (UKPRN,SubmittedTime) VALUES (@ukprn, @submissionDate)",
                new { ukprn, submissionDate = DateTime.Today });

            return ukprn;
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
            return Query<PaymentEntity>("SELECT * FROM LevyPayments.Payments WHERE RequiredPaymentId IN (SELECT Id FROM PaymentsDue.RequiredPayments WHERE CommitmentId = @commitmentId)", new { commitmentId });
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

        internal static void PopulatePaymentsHistory()
        {
            var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Tools\Sql\Copy Reference Data\05 PeriodEnd.Populate.Reference.PaymentsHistory.dml.sql");

            var commands = ReplaceSqlTokens(sql).Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var command in commands)
            {
                Execute(command);
            }

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
        private static string ReplaceSqlTokens(string sql)
        {
            return sql.Replace("${DAS_Accounts.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_Commitments.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${ILR_Summarisation.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                    .Replace("${DAS_PeriodEnd.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                    .Replace("${ILR_Deds.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                    .Replace("${YearOfCollection}", "1617");

        }
    }
}
