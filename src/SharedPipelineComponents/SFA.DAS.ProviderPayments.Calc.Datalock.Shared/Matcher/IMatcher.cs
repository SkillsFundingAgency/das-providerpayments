using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher
{
   public interface IMatcher
    {
        MatchResult Match(List<Commitment> commitments, PriceEpisode priceEpisode, List<DasAccount> dasAccounts);
    }
}
