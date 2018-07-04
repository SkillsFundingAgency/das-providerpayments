using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependiencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services
{
    class LearnerRefundProcessor : IProcessLearnerRefunds
    {

        private PaymentEntity CreatePayment(
            RequiredPaymentEntity refund, 
            decimal amount, 
            int deliveryYear,
            int deliveryMonth, 
            FundingSource fundingSource)
        {
            var payment = new PaymentEntity
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
