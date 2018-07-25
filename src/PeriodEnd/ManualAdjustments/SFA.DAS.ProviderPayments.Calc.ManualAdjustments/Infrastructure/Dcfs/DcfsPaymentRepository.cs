using System;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Dcfs
{
    public class DcfsPaymentRepository : DcfsRepository, IPaymentRepository
    {
        private const string HistorySource = "Reference.PaymentsHistory";
        private const string CurrentLevySource = "LevyPayments.Payments";
        private const string CurrentCoInvestedSource = "CoInvestedPayments.Payments";

        public DcfsPaymentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public PaymentEntity[] GetPaymentsForRequiredPayment(string requiredPaymentId)
        {
            return Query<PaymentEntity>($"SELECT RequiredPaymentId,DeliveryMonth,DeliveryYear,TransactionType," +
                                        $"FundingSource,Amount " +
                                        $"FROM {HistorySource} " +
                                        $"WHERE RequiredPaymentId = @requiredPaymentId ",
                                        
                new { requiredPaymentId });
        }

        private void CreatePayment(PaymentEntity payment, string destinationTable)
        {
            
            Execute($"INSERT INTO {destinationTable} (PaymentId,RequiredPaymentId,DeliveryMonth,DeliveryYear," +
                     "CollectionPeriodName,CollectionPeriodMonth,CollectionPeriodYear,FundingSource,TransactionType,Amount) " +
                     "VALUES (@PaymentId,@RequiredPaymentId,@DeliveryMonth,@DeliveryYear," +
                     "@CollectionPeriodName,@CollectionPeriodMonth,@CollectionPeriodYear,@FundingSource,@TransactionType,@Amount)", payment);
        }

        private void CreateHistoryPayment(PaymentEntity payment,RequiredPaymentEntity requiredPayment)
        {

            Execute($"INSERT INTO {HistorySource} " +
                    $"(PaymentId, " +
                    $"RequiredPaymentId, " +
                    $"DeliveryMonth, " +
                    $"DeliveryYear, " +
                    $"CollectionPeriodName, " +
                    $"CollectionPeriodYear, " +
                    $"CollectionPeriodMonth, " +
                    $"FundingSource, " +
                    $"TransactionType, " +
                    $"Amount, " +
                    $"ApprenticeshipContractType, " +
                    $"Ukprn, " +
                    $"AccountId, " +
                    $"LearnRefNumber, " +
                    $"FundingLineType) " +
                     "VALUES ('" + 
                    Guid.NewGuid() + "', " +
                    "@RequiredPaymentId, " +
                    "@DeliveryMonth, " +
                    "@DeliveryYear, " +
                    "@CollectionPeriodName, " +
                    "@CollectionPeriodYear, " +
                    "@CollectionPeriodMonth, " +
                    "@FundingSource, " +
                    "@TransactionType, " +
                    "@Amount, " +
                    "@ApprenticeshipContractType, " +
                    "@Ukprn, " +
                    "@AccountId, " +
                    "@LearnRefNumber, " +
                    "@FundingLineType" +
                    ")", 
                     new {
                         payment.RequiredPaymentId,
                         payment.DeliveryMonth,
                         payment.DeliveryYear,
                         payment.FundingSource,
                         payment.TransactionType,
                         payment.Amount,
                         requiredPayment.Ukprn,
                         requiredPayment.CollectionPeriodMonth,
                         requiredPayment.CollectionPeriodYear,
                         requiredPayment.CollectionPeriodName,
                         requiredPayment.ApprenticeshipContractType, 
                         requiredPayment.AccountId, 
                         requiredPayment.LearnRefNumber, 
                         requiredPayment.FundingLineType,
                     } );
        }

        public void CreatePayment(PaymentEntity payment, RequiredPaymentEntity requiredPayment)
        {

            var currentDestinationTable = payment.FundingSource == 1 ? CurrentLevySource : CurrentCoInvestedSource;

            CreatePayment(payment, currentDestinationTable);

            CreateHistoryPayment(payment, requiredPayment);
        }
    }
}
