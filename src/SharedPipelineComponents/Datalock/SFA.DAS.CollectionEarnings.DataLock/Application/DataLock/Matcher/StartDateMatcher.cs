using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class StartDateMatcher : MatchHandler
    {
        public StartDateMatcher(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {}
       
        public override  MatchResult Match(List<CommitmentEntity> commitments, RawEarning priceEpisode, List<DasAccount.DasAccount> dasAccounts, MatchResult matchResult)
        {
            var commitmentsToMatch = commitments.Where(c => priceEpisode.EpisodeStartDate >= c.StartDate
                                                            && priceEpisode.EpisodeStartDate < c.EndDate
                                                            && priceEpisode.EpisodeStartDate >= c.EffectiveFrom 
                                                            && (c.EffectiveTo == null || priceEpisode.EpisodeStartDate <= c.EffectiveTo)).ToList();

            if (!commitmentsToMatch.Any())
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.EarlierStartDate);
                matchResult.Commitments = commitments.ToArray();
            }
            else
            {
                matchResult.Commitments = commitmentsToMatch.ToArray();
            }

            return ExecuteNextHandler(commitmentsToMatch, priceEpisode,dasAccounts,matchResult);
        }
    }
}