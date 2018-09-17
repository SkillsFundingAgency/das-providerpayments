using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface ILearnerPaymentsDueProcessor
    {
        PaymentsDueResult GetPayableAndNonPayableEarnings(LearnerData parameters, long ukprn);
    }
}