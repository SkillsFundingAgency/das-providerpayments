using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
   public interface IMatcher
    {
        MatchResult Match(List<Commitment.Commitment> commitments, PriceEpisode.PriceEpisode priceEpisode, List<DasAccount.DasAccount> dasAccounts);

    }
}
