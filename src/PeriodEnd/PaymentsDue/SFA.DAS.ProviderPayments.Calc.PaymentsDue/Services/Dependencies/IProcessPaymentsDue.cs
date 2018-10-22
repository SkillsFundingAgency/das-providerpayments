using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface IProcessPaymentsDue
    {
        PaymentsDueResult GetPayableAndNonPayableEarnings(LearnerData parameters, long ukprn);
    }
}