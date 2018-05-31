using System.Collections.Generic;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public interface IProviderLearnersBuilder
    {
        Dictionary<string, Learner> Build(long ukprn);
    }
}