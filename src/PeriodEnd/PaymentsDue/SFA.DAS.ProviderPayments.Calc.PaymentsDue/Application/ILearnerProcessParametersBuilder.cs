using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public interface ILearnerProcessParametersBuilder
    {
        HashSet<LearnerProcessParameters> Build(long ukprn);
    }
}