using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface IProviderLearnersBuilder
    {
        Dictionary<string, PaymentsDueCalculationService> Build(long ukprn);
    }
}