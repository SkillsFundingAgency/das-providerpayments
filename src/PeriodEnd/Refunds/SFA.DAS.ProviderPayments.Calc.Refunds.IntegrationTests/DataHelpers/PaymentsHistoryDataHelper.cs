using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.IntegrationTests.DataHelpers
{
    internal static class PaymentsHistoryDataHelper
    {
        internal static void CreateHistoricalPayment(HistoricalPaymentEntity payment)
        {
            const string sql = @"
            INSERT INTO Reference.PaymentsHistory (
                PaymentId
                ,RequiredPaymentId
                ,DeliveryMonth
                ,DeliveryYear
                ,CollectionPeriodName
                ,CollectionPeriodMonth
                ,CollectionPeriodYear
                ,FundingSource
                ,TransactionType
                ,Amount
                ,ApprenticeshipContractType
                ,Ukprn
                ,AccountId
                ,LearnRefNumber
            ) VALUES (
                @PaymentId
                ,@RequiredPaymentId
                ,@DeliveryMonth
                ,@DeliveryYear
                ,@CollectionPeriodName
                ,@CollectionPeriodMonth
                ,@CollectionPeriodYear
                ,@FundingSource
                ,@TransactionType
                ,@Amount
                ,@ApprenticeshipContractType
                ,@Ukprn
                ,@AccountId
                ,@LearnRefNumber
            );";

            TestDataHelper.Execute(sql, payment);
        }

        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE Reference.PaymentsHistory";
            TestDataHelper.Execute(sql);
        }
    }
}