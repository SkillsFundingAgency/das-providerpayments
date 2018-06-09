using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public interface ILearnerProcessor
    {
        LearnerProcessResults Process(LearnerProcessParameters parameters);
    }
}