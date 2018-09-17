using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Domain.Extensions
{
    public static class ProviderPaymentsGroupExtensions
    {
        public static PaymentEntity MakePayment(this ProviderPaymentsGroup source, decimal amount)
        {
            var payment = new PaymentEntity
            {
                Amount = amount,
                PaymentType = source.PaymentType,
                Ukprn = source.Ukprn,
                PaymentTypeName = source.PaymentTypeName,
                SubmissionCollectionPeriod = source.Period,
                SubmissionId = source.SubmissionId,
            };
            return payment;
        }
    }
}
