using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.IntegrationTests.TestComponents.Entities;

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

        internal static void WriteAdjustment(ManualAdjustmentEntity adjustment)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.DedsConnectionString))
            {
                connection.Execute("INSERT INTO Adjustments.ManualAdjustments (RequiredPaymentIdToReverse, ReasonForReversal, RequestorName, DateUploaded, RequiredPaymentIdForReversal) " +
                                   "VALUES (@RequiredPaymentIdToReverse, @ReasonForReversal, @RequestorName, @DateUploaded, @RequiredPaymentIdForReversal)",
                    adjustment);
            }
        }

        internal static void WriteRequiredPayment(RequiredPaymentEntity requiredPayment)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                connection.Execute("INSERT INTO Reference.RequiredPaymentsHistory (Id,CommitmentId,CommitmentVersionId,AccountId,AccountVersionId,Uln,LearnRefNumber,AimSeqNumber,Ukprn," +
                                   "PriceEpisodeIdentifier,StandardCode,ProgrammeType,FrameworkCode,PathwayCode,DeliveryMonth," +
                                   "DeliveryYear,CollectionPeriodName,CollectionPeriodMonth,CollectionPeriodYear,TransactionType,AmountDue) " +
                                   "VALUES (@Id,@CommitmentId,@CommitmentVersionId,@AccountId,@AccountVersionId,@Uln,@LearnRefNumber,@AimSeqNumber,@Ukprn," +
                                   "@PriceEpisodeIdentifier,@StandardCode,@ProgrammeType,@FrameworkCode,@PathwayCode,@DeliveryMonth," +
                                   "@DeliveryYear,@CollectionPeriodName,@CollectionPeriodMonth,@CollectionPeriodYear,@TransactionType,@AmountDue)",
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

    }
}
