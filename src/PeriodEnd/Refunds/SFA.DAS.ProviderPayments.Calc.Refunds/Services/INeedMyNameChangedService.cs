using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services
{
    class INeedMyNameChangedService
    {
        private PaymentEntity CreatePayment(RequiredPaymentEntity refund, decimal amount, int deliveryYear, int deliveryMonth, FundingSource fundingSource)
        {
            var payment = new Payment
            {
                DeliveryYear = deliveryYear,
                DeliveryMonth = deliveryMonth,
                Amount = amount,
                RequiredPaymentId = refund.Id,
                CollectionPeriodName = refund.CollectionPeriodName,
                CollectionPeriodMonth = refund.CollectionPeriodMonth,
                CollectionPeriodYear = refund.CollectionPeriodYear,
                FundingSource = fundingSource,
                TransactionType = refund.TransactionType,
            };

            return payment;
        }
    }
}
