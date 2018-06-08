using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public interface ILearner
    {
        LearnerProcessResults CalculatePaymentsDue();
    }
}