using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface ILearnerProcessor
    {
        PaymentsDueResult Process(LearnerData parameters, long ukprn);
    }
}