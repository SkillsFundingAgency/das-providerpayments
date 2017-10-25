using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure
{
    public interface IPaymentRepository
    {
        PaymentEntity[] GetPaymentsForRequiredPayment(string requiredPaymentId);

        void CreatePayment(PaymentEntity payment, RequiredPaymentEntity requiredPayment);
    }
}