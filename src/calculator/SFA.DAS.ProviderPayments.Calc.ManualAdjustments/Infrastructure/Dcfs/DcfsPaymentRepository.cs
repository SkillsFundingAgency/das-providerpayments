using System;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Dcfs
{
    public class DcfsPaymentRepository : DcfsRepository, IPaymentRepository
    {
        private const string HistoryLevySource = "Reference.LevyPaymentsHistory";
        private const string HistoryCoInvestedSource = "Reference.CoInvestedPaymentsHistory";
        private const string CurrentLevySource = "LevyPayments.Payments";
        private const string CurrentCoInvestedSource = "CoInvestedPayments.Payments";

        public DcfsPaymentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public PaymentEntity[] GetPaymentsForRequiredPayment(string requiredPaymentId)
        {
            return Query<PaymentEntity>($"SELECT RequiredPaymentId,DeliveryMonth,DeliveryYear,TransactionType,FundingSource,Amount FROM {HistoryLevySource} WHERE RequiredPaymentId = @requiredPaymentId " +
                                        $"UNION ALL " +
                                        $"SELECT RequiredPaymentId,DeliveryMonth,DeliveryYear,TransactionType,FundingSource,Amount FROM {HistoryCoInvestedSource} WHERE RequiredPaymentId = @requiredPaymentId ",
                new { requiredPaymentId });
        }

        public void CreatePayment(PaymentEntity payment)
        {
            var destinationTable = payment.FundingSource == 1 ? CurrentLevySource : CurrentCoInvestedSource;

            Execute($"INSERT INTO {destinationTable} (PaymentId,RequiredPaymentId,DeliveryMonth,DeliveryYear," +
                     "CollectionPeriodName,CollectionPeriodMonth,CollectionPeriodYear,FundingSource,TransactionType,Amount) " +
                     "VALUES (@PaymentId,@RequiredPaymentId,@DeliveryMonth,@DeliveryYear," +
                     "@CollectionPeriodName,@CollectionPeriodMonth,@CollectionPeriodYear,@FundingSource,@TransactionType,@Amount)", payment);
        }
    }
}
