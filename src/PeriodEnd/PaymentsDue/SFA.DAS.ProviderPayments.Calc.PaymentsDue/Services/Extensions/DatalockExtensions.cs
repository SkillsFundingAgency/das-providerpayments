using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions
{
    internal static class DatalockExtensions
    {
        internal static bool HasDatalockValidationError(this DatalockOutputEntity source,
            List<string> priceEpisodeIdsThatHaveDatalockValidationErrors)
        {
            return priceEpisodeIdsThatHaveDatalockValidationErrors.Contains(source.PriceEpisodeIdentifier);
        }
    }
}