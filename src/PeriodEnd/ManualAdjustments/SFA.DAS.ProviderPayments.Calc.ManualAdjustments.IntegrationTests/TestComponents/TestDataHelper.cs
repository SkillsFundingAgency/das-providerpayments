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
                connection.RunSqlScript("DML\\01 PeriodEnd.Adjustments.Populate.ManualAdjustments.sql");
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
                                   "(@accountId, @accountId, 'Account ' + cast(@accountId as bigint), @balance, '20170719', 1)",
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


        internal static void WritePayment(PaymentEntity payment,RequiredPaymentEntity requiredPayment)
        {
            
        
            if (payment.CommitmentId.HasValue && payment.CommitmentId.Value>0 )
            {
                using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
                {
                    connection.Execute($"INSERT INTO Reference.LevyPaymentsHistory (RequiredPaymentId, DeliveryMonth, DeliveryYear, " +
                                       $"FundingSource, TransactionType, Amount, CommitmentId)" +
                                        "VALUES (@RequiredPaymentId, @DeliveryMonth, @DeliveryYear, " +
                                       $"@FundingSource, @TransactionType, @Amount, @CommitmentId)",
                        payment);
                }
            }
            if (payment.FundingSource!= 1)
            {
                using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
                {

                    connection.Execute($"INSERT INTO Reference.CoInvestedPaymentsHistory (RequiredPaymentId,DeliveryMonth,DeliveryYear,FundingSource,TransactionType,Amount,ULN,Ukprn,AimSeqNumber,StandardCode,ProgrammeType,FrameworkCode,PathwayCode,CommitmentId) " +
                   "VALUES (@RequiredPaymentId,@DeliveryMonth,@DeliveryYear,@FundingSource,@TransactionType,@Amount,@ULN,@Ukprn,@AimSeqNumber,@StandardCode,@ProgrammeType,@FrameworkCode,@PathwayCode,@CommitmentId)",
                   new
                   {
                       RequiredPaymentId = payment.RequiredPaymentId,
                       DeliveryMonth = payment.DeliveryMonth,
                       DeliveryYear = payment.DeliveryYear,
                       FundingSource = payment.FundingSource,
                       TransactionType = payment.TransactionType,
                       Amount = payment.Amount,
                       ULN = requiredPayment.Uln,
                       Ukprn = requiredPayment.Ukprn,
                       AimSeqNumber = requiredPayment.AimSeqNumber,
                       StandardCode = requiredPayment.StandardCode,
                       ProgrammeType = requiredPayment.ProgrammeType,
                       FrameworkCode = requiredPayment.FrameworkCode,
                       PathwayCode = requiredPayment.PathwayCode,
                       CommitmentId = requiredPayment.CommitmentId
                       
                   });
                }
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
                                   " FundingSource, TransactionType, Amount,CommitmentId";
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                return connection.Query<PaymentEntity>($"SELECT {columns} FROM Reference.LevyPaymentsHistory " +
                                                       $"UNION " +
                                                       $"SELECT {columns} FROM Reference.CoInvestedPaymentsHistory ").ToArray();
            }
        }

        internal static CoInvestedPaymentHistoryEntity[] GetHistoricalCoInvestedPayments()
        {
            const string columns = " RequiredPaymentId,DeliveryMonth,DeliveryYear,FundingSource,TransactionType,Amount,ULN,Ukprn,AimSeqNumber,StandardCode,ProgrammeType,FrameworkCode,PathwayCode";
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                return connection.Query<CoInvestedPaymentHistoryEntity>($"SELECT {columns} FROM Reference.CoInvestedPaymentsHistory ").ToArray();
            }
        }

    }
}
