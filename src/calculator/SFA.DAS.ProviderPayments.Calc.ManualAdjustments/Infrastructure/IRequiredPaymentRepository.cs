using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure
{
    public interface IRequiredPaymentRepository
    {
        RequiredPaymentEntity GetRequiredPayment(string requiredPaymentId);

        void CreateRequiredPayment(RequiredPaymentEntity requiredPayment);
    }
}