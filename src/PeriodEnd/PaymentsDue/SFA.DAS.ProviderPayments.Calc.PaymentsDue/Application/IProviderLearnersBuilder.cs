using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public interface IProviderLearnersBuilder
    {
        Dictionary<string, Learner> Build(long ukprn);
    }
}