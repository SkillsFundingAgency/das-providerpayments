using System.Collections.Generic;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public interface IIShouldBeInTheDatalockComponent
    {
        IReadOnlyList<PriceEpisode> PriceEpisodes { get; }
        void ValidatePriceEpisodes();
    }
}