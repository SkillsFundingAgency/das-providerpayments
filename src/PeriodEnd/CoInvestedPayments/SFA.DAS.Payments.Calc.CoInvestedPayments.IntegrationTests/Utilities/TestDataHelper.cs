using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Utilities
{
    internal static class TestDataHelper
    {
        private static readonly string[] PeriodEndCopyReferenceDataScripts =
        {
            "01 PeriodEnd.Populate.Reference.CollectionPeriods.dml.sql",
            "02 PeriodEnd.Populate.Reference.Providers.dml.sql"
        };

        private static readonly Random Random = new Random();

        internal static void AddAccount(long id, string name = null, decimal balance = 999999999,bool isDeds = false)
        {
            if (name == null)
            {
                name = id.ToString();
            }

            Execute("INSERT INTO dbo.DasAccounts (AccountId, AccountName, Balance) VALUES (@id, @name, @balance)",
                new { id, name, balance },isDeds);
        }


        internal static void AddPaymentHistoryForCommitment(string requiredPaymentId,
                                                            FundingSource fundingSource,
                                                            decimal amount,
                                                            int deliveryMonth,
                                                            int deliveryYear,
                                                            TransactionType transactionType, 
                                                            bool isDeds)
        {

            Execute("INSERT INTO Payments.Payments "
                  +"Values ( "
                  + "NEWID(), "
                  + "@requiredPaymentId, "
                  + "@deliveryMonth, "
                  + "@deliveryYear, "
                  + "'2017-R01', "
                  + "1, "
                  + "2017, "
                  + "@fundingSource, "
                  + "@transactionType, "
                  + "@amount) ",
                new { requiredPaymentId, deliveryMonth, deliveryYear, transactionType, fundingSource, amount }, isDeds);
        }

        internal static void AddCommitment(long id, 
                                           long accountId, 
                                           long uln = 0L, 
                                           long ukprn = 0L, 
                                           DateTime startDate = default(DateTime), 
                                           DateTime endDate = default(DateTime),
                                           decimal agreedCost = 15000m,
                                           long? standardCode = null,
                                           int? programmeType = null,
                                           int? frameworkCode = null,
                                           int? pathwayCode = null,
                                           bool isDeds = false)
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
                    "(CommitmentId,AccountId,Uln,Ukprn,StartDate,EndDate,AgreedCost,StandardCode,ProgrammeType,FrameworkCode,PathwayCode,PaymentStatus,PaymentStatusDescription,Payable,Priority,VersionId) " +
                    "VALUES " +
                    "(@id, @accountId, @uln, @ukprn, @startDate, @endDate, @agreedCost, @standardCode, @programmeType, @frameworkCode, @pathwayCode, 1, 'Active', 1, 1, '1')",
                    new { id, accountId, uln, ukprn, startDate, endDate, agreedCost, standardCode, programmeType, frameworkCode, pathwayCode },isDeds);
        }

        internal static void AddPaymentDueForProvider(
            long commitmentId,
            long ukprn,
            string learnerRefNumber = null,
            int aimSequenceNumber = 1,
            TransactionType transactionType = TransactionType.Learning,
            decimal amountDue = 1000.00m,
            decimal sfaContributionPercentage = 0.9m,
            string requiredPaymentId = null,
            bool isDeds = false)
        {
            if (string.IsNullOrEmpty(learnerRefNumber))
            {
                learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            }
            if (string.IsNullOrEmpty(requiredPaymentId))
            {
                requiredPaymentId = Guid.NewGuid().ToString();
            }

            Execute("INSERT INTO PaymentsDue.RequiredPayments (Id, CommitmentId, AccountId, Uln, LearnRefNumber, AimSeqNumber, Ukprn, "
                  + "DeliveryMonth, DeliveryYear, TransactionType, AmountDue, SfaContributionPercentage, CollectionPeriodName, CollectionPeriodMonth, CollectionPeriodYear)"
                  + "SELECT "
                  + "@requiredPaymentId, "
                  + "CommitmentId, "
                  + "AccountId, "
                  + "Uln, "
                  + "@learnerRefNumber, "
                  + "@aimSequenceNumber, "
                  + "@ukprn, "
                  + "(SELECT Period FROM CoInvestedPayments.vw_CollectionPeriods WHERE Collection_Open = 1), "
                  + "(SELECT Calendar_Year FROM CoInvestedPayments.vw_CollectionPeriods WHERE Collection_Open = 1), "
                  + "@transactionType, "
                  + "@amountDue, "
                  + "@sfaContributionPercentage, "
                  + "'PN', 1, 2017 "
                  + "FROM dbo.DasCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                new { commitmentId,requiredPaymentId, learnerRefNumber, aimSequenceNumber, ukprn, transactionType, amountDue, sfaContributionPercentage },isDeds);
        }

        internal static void AddRequiredPaymentForReversal(string requiredPaymentId)
        {
                Execute("Insert Into [Adjustments].[ManualAdjustments]" +
                                "([RequiredPaymentIdToReverse]" +
                                ",[ReasonForReversal]" +
                                ",[RequestorName]" +
                                ",[DateUploaded]" +
                                ",[RequiredPaymentIdForReversal])" +
                            "Values( NEWID()," +
                                    "'Test scenario', " +
                                    "'Test', " +
                                    "getDate()," +
                                    "@requiredPaymentId)" ,
                  new
                  {
                     requiredPaymentId
                  });
            
        }

        internal static void AddPaymentDueForProvider2(
            long commitmentId,
            long ukprn,
            int deliveryMonth,
            int deliveryYear,
            int aimSequenceNumber = 1,
            string learnerRefNumber = null,
            TransactionType transactionType = TransactionType.Learning,
            decimal amountDue = 1000.00m,
            decimal sfaContributionPercentage = 0.9m,
             string requiredPaymentId = null,
            bool isDeds = false)
        {
            if (string.IsNullOrEmpty(learnerRefNumber))
            {
                learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            }

            if (string.IsNullOrEmpty(requiredPaymentId))
            {
                requiredPaymentId = Guid.NewGuid().ToString();
            }
            Execute("INSERT INTO PaymentsDue.RequiredPayments (Id, CommitmentId, AccountId, Uln, LearnRefNumber, AimSeqNumber, Ukprn, "
                  + "DeliveryMonth, DeliveryYear, TransactionType, AmountDue, SfaContributionPercentage, CollectionPeriodName, CollectionPeriodMonth, CollectionPeriodYear)"
                  + "SELECT "
                  + "@requiredPaymentId, "
                  + "CommitmentId, "
                  + "AccountId, "
                  + "Uln, "
                  + "@learnerRefNumber, "
                  + "@aimSequenceNumber, "
                  + "@ukprn, "
                  + "@deliveryMonth, "
                  + "@deliveryYear, "
                  + "@transactionType, "
                  + "@amountDue, "
                  + "@sfaContributionPercentage, "
                  + "'PN', 1, 2017"
                  + "FROM dbo.DasCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                new { commitmentId, requiredPaymentId, deliveryMonth, deliveryYear, learnerRefNumber, aimSequenceNumber, ukprn, transactionType, amountDue, sfaContributionPercentage },isDeds);
        }

        internal static void AddPaymentDueForNonDas(
            long ukprn,
            long uln,
            string learnerRefNumber = null,
            int aimSequenceNumber = 1,
            TransactionType transactionType = TransactionType.Learning,
            decimal amountDue = 1000.00m,
            decimal sfaContributionPercentage = 0.9m,
              string requiredPaymentId = null)
        {
            if (string.IsNullOrEmpty(learnerRefNumber))
            {
                learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            }

            if (string.IsNullOrEmpty(requiredPaymentId))
            {
                requiredPaymentId = Guid.NewGuid().ToString();
            }

            Execute("INSERT INTO PaymentsDue.RequiredPayments (Id, Uln, LearnRefNumber, AimSeqNumber, Ukprn, "
                  + "DeliveryMonth, DeliveryYear, TransactionType, AmountDue, SfaContributionPercentage, ApprenticeshipContractType, CollectionPeriodName, CollectionPeriodYear, CollectionPeriodMonth) "
                  + "VALUES ("
                  + "@requiredPaymentId, "
                  + "@uln, "
                  + "@learnerRefNumber, "
                  + "@aimSequenceNumber, "
                  + "@ukprn, "
                  + "(SELECT Period FROM CoInvestedPayments.vw_CollectionPeriods WHERE Collection_Open = 1), "
                  + "(SELECT Calendar_Year FROM CoInvestedPayments.vw_CollectionPeriods WHERE Collection_Open = 1), "
                  + "@transactionType, "
                  + "@amountDue, "
                  + "@sfaContributionPercentage, "
                  + "2, 'PN', 2017, 1)",
                new { requiredPaymentId,uln, learnerRefNumber, aimSequenceNumber, ukprn, transactionType, amountDue, sfaContributionPercentage });
        }

        internal static long AddProvider(long ukprn)
        {
            Execute("INSERT INTO Valid.LearningProvider" +
                    "(UKPRN) " +
                    "VALUES " +
                    "(@ukprn)",
                new { ukprn },true);

            Execute("INSERT INTO dbo.FileDetails (UKPRN,SubmittedTime) VALUES (@ukprn, @submissionDate)",
                new { ukprn, submissionDate = DateTime.Today },true);

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
                            AND s.name IN ('dbo', 'PaymentsDue', 'CoInvestedPayments')
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
                            AND s.name IN ('dbo', 'PaymentsDue', 'CoInvestedPayments')
                            AND o.name NOT IN ('Collection_Period_Mapping')
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')

                    EXEC sys.sp_executesql @SQL                
                ",null,true);
        }

        internal static PaymentEntity[] GetPaymentsForCommitment(long commitmentId)
        {
            return Query<PaymentEntity>("SELECT * FROM CoInvestedPayments.Payments WHERE RequiredPaymentId IN (SELECT Id FROM PaymentsDue.RequiredPayments WHERE CommitmentId = @commitmentId)", new { commitmentId });
        }

        internal static PaymentEntity[] GetPaymentsForUln(long uln,long ukprn)
        {
            return Query<PaymentEntity>("SELECT * FROM CoInvestedPayments.Payments WHERE RequiredPaymentId IN (SELECT Id FROM PaymentsDue.RequiredPayments WHERE Uln = @uln AND UKPRN = @ukprn)", new { uln,ukprn });
        }

        internal static int GetPaymentsCount()
        {
            return Count("CoInvestedPayments.Payments");
        }
        
        internal static void CopyReferenceData()
        {
            foreach (var script in PeriodEndCopyReferenceDataScripts)
            {
                var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Utilities\Sql\Copy Reference Data\{script}");

                var commands = ReplaceSqlTokens(sql).Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var command in commands)
                {
                    Execute(command);
                }
            }
        }

        private static int Count(string tablename)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                connection.Open();
                try
                {
                    return connection.ExecuteScalar<int>($"SELECT count(*) FROM {tablename}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private static void Execute(string command, object param = null, bool isDeds = false)
        {
            var connString = isDeds ? GlobalTestContext.Instance.DedsConnectionString : GlobalTestContext.Instance.TransientConnectionString;
            using (var connection = new SqlConnection(connString))
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
            return sql.Replace("${ILR_Summarisation.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                .Replace("${ILR_Deds.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                .Replace("${DAS_PeriodEnd.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                .Replace("${DAS_PeriodEnd.servername}", GlobalTestContext.Instance.LinkedServerName)
                .Replace("${DAS_PeriodEnd.databasename}", GlobalTestContext.Instance.BracketedDatabaseName)
                .Replace("${YearOfCollection}", "1617");
        }
    }
}
