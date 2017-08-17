using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.IntegrationTests.TestComponents.Entities;
using System;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.IntegrationTests.TestComponents
{
    internal static class TestDataHelper
    {
        internal static void Clean()
        {
            CleanDeds();
            CleanTransient();
        }
        internal static void CleanDeds()
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.DedsConnectionString))
            {
                connection.Execute("TRUNCATE TABLE Adjustments.ManualAdjustments");
            }
        }
        internal static void CleanTransient()
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                connection.Execute("TRUNCATE TABLE Adjustments.ManualAdjustments");
                connection.Execute("TRUNCATE TABLE Adjustments.TaskLog");

                connection.Execute("TRUNCATE TABLE CoInvestedPayments.Payments");
                connection.Execute("TRUNCATE TABLE CoInvestedPayments.TaskLog");

                connection.Execute("TRUNCATE TABLE LevyPayments.AccountProcessStatus");
                connection.Execute("TRUNCATE TABLE LevyPayments.Payments");
                connection.Execute("TRUNCATE TABLE LevyPayments.TaskLog");

                connection.Execute("TRUNCATE TABLE PaymentsDue.RequiredPayments");
                connection.Execute("TRUNCATE TABLE PaymentsDue.TaskLog");

                connection.Execute("TRUNCATE TABLE Reference.ApprenticeshipDeliveryEarnings");
                connection.Execute("TRUNCATE TABLE Reference.ApprenticeshipEarnings");
                connection.Execute("TRUNCATE TABLE Reference.CoInvestedPaymentsHistory");
                connection.Execute("TRUNCATE TABLE Reference.CollectionPeriods");
                connection.Execute("TRUNCATE TABLE Reference.DasAccounts");
                connection.Execute("TRUNCATE TABLE Reference.LevyPaymentsHistory");
                connection.Execute("TRUNCATE TABLE Reference.RequiredPaymentsHistory");
            }
        }


        internal static void CopyDataToTransient()
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                connection.RunSqlScript("DML\\PeriodEnd.Adjustments.Populate.ManualAdjustments.sql");
            }
        }


        internal static void WriteOpenCollectionPeriod(string name, int month, int year)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                connection.Execute("DELETE FROM Reference.CollectionPeriods");

                connection.Execute("INSERT INTO Reference.CollectionPeriods (Id, [Name], CalendarMonth, CalendarYear, [Open]) " +
                                   "VALUES (1, @name, @month, @year, 1)",
                    new { name, month, year });
            }
        }


        internal static void WriteEmployerAccount(string accountId, decimal balance)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                connection.Execute("INSERT INTO Reference.DasAccounts " +
                                   "(AccountId, AccountHashId, AccountName, Balance, VersionId, IsLevyPayer) " +
                                   "VALUES " +
                                   "(@accountId, @accountId, 'Account ' + cast(@accountId as varchar(10)), @balance, '20170719', 1)",
                                   new { accountId, balance });
            }
        }
        internal static decimal GetEmployerAccountBalance(string accountId)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                return connection.Query<decimal>("SELECT acc.Balance - ISNULL(stat.LevySpent,0) " +
                                                 "FROM Reference.DasAccounts acc " +
                                                 "LEFT JOIN LevyPayments.AccountProcessStatus stat ON acc.AccountId = stat.AccountId " +
                                                 "WHERE acc.AccountId = @accountId",
                                                 new { accountId }).SingleOrDefault();
            }
        }


        internal static void WriteAdjustment(ManualAdjustmentEntity adjustment)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.DedsConnectionString))
            {
                connection.Execute("INSERT INTO Adjustments.ManualAdjustments (RequiredPaymentIdToReverse, ReasonForReversal, RequestorName, DateUploaded, RequiredPaymentIdForReversal) " +
                                   "VALUES (@RequiredPaymentIdToReverse, @ReasonForReversal, @RequestorName, @DateUploaded, @RequiredPaymentIdForReversal)",
                    adjustment);
            }
        }

        internal static ManualAdjustmentEntity GetManualAdjustment(Guid requiredPaymentIdToReverse)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                return connection.QuerySingle<ManualAdjustmentEntity>("Select * from Adjustments.ManualAdjustments Where RequiredPaymentIdToReverse = @requiredPaymentIdToReverse" ,
                    new { requiredPaymentIdToReverse });
            }
        }


        internal static void WriteRequiredPayment(RequiredPaymentEntity requiredPayment)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                connection.Execute("INSERT INTO Reference.RequiredPaymentsHistory (Id,CommitmentId,CommitmentVersionId,AccountId,AccountVersionId,Uln,LearnRefNumber,AimSeqNumber,Ukprn," +
                                   "PriceEpisodeIdentifier,StandardCode,ProgrammeType,FrameworkCode,PathwayCode,DeliveryMonth," +
                                   "DeliveryYear,CollectionPeriodName,CollectionPeriodMonth,CollectionPeriodYear,TransactionType,AmountDue," +
                                   "IlrSubmissionDateTime,ApprenticeshipContractType,SfaContributionPercentage,FundingLineType,UseLevyBalance,LearnAimRef,LearningStartDate) " +
                                   "VALUES (@Id,@CommitmentId,@CommitmentVersionId,@AccountId,@AccountVersionId,@Uln,@LearnRefNumber,@AimSeqNumber,@Ukprn," +
                                   "@PriceEpisodeIdentifier,@StandardCode,@ProgrammeType,@FrameworkCode,@PathwayCode,@DeliveryMonth," +
                                   "@DeliveryYear,@CollectionPeriodName,@CollectionPeriodMonth,@CollectionPeriodYear,@TransactionType,@AmountDue," +
                                   "@IlrSubmissionDateTime,@ApprenticeshipContractType,@SfaContributionPercentage,@FundingLineType,@UseLevyBalance,@LearnAimRef,@LearningStartDate)",
                    requiredPayment);
            }
        }
        internal static RequiredPaymentEntity[] GetRequiredPayments()
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                return connection.Query<RequiredPaymentEntity>("SELECT * FROM PaymentsDue.RequiredPayments").ToArray();
            }
        }
        internal static RequiredPaymentEntity[] GetHistoryRequiredPayments()
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                return connection.Query<RequiredPaymentEntity>("SELECT * FROM Reference.RequiredPaymentsHistory").ToArray();
            }
        }


        internal static void WritePayment(PaymentEntity payment)
        {
            var destinationTable = payment.FundingSource == 1 ? "Reference.LevyPaymentsHistory" : "Reference.CoInvestedPaymentsHistory";
            var commitmentIdCol = payment.FundingSource == 1 ? ", CommitmentId" : "";
            var commitmentIdVal = payment.FundingSource == 1 ? ", @CommitmentId" : "";
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                connection.Execute($"INSERT INTO {destinationTable} (RequiredPaymentId, DeliveryMonth, DeliveryYear, " +
                                   $"FundingSource, TransactionType, Amount{commitmentIdCol})" +
                                    "VALUES (@RequiredPaymentId, @DeliveryMonth, @DeliveryYear, " +
                                   $"@FundingSource, @TransactionType, @Amount{commitmentIdVal})",
                    payment);
            }
        }
        internal static PaymentEntity[] GetPayments()
        {
            const string columns = "PaymentId, RequiredPaymentId, DeliveryMonth, DeliveryYear, " +
                                   "CollectionPeriodName, CollectionPeriodMonth, CollectionPeriodYear, FundingSource, TransactionType, Amount";
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                return connection.Query<PaymentEntity>($"SELECT {columns} FROM LevyPayments.Payments " +
                                                       $"UNION ALL " +
                                                       $"SELECT {columns} FROM CoInvestedPayments.Payments ").ToArray();
            }
        }

        internal static PaymentEntity[] GetHistoricalPayments()
        {
            const string columns = " RequiredPaymentId, DeliveryMonth, DeliveryYear, " +
                                   " FundingSource, TransactionType, Amount";
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                return connection.Query<PaymentEntity>($"SELECT {columns},CommitmentId FROM Reference.LevyPaymentsHistory " +
                                                       $"UNION ALL " +
                                                       $"SELECT {columns}, NULL as CommitmentId FROM Reference.CoInvestedPaymentsHistory ").ToArray();
            }
        }

    }
}
