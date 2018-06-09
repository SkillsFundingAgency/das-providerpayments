using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public interface ILearnerProcessParametersBuilder
    {
        List<LearnerProcessParameters> Build(long ukprn);
    }
}