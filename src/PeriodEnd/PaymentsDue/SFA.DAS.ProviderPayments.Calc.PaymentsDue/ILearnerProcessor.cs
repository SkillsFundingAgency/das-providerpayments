using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public interface ILearnerProcessor
    {
        LearnerProcessResults Process(LearnerProcessParameters parameters);
    }
}