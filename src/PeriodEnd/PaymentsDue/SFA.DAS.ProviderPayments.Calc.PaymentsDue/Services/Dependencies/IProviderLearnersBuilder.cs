using System.Collections.Generic;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface IProviderLearnersBuilder
    {
        Dictionary<string, PaymentsDueCalculationService> Build(long ukprn);
    }
}