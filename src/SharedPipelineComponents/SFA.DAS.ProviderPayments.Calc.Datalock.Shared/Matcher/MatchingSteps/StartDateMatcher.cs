using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher.MatchingSteps
{
    public class StartDateMatcher : MatchHandler
    {
        public StartDateMatcher(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {}
       
        public override  MatchResult Match(List<Commitment> commitments, PriceEpisode priceEpisode, List<DasAccount> dasAccounts, MatchResult matchResult)
        {
            var commitmentsToMatch = commitments.Where(c => priceEpisode.StartDate >= c.StartDate
                                                            && priceEpisode.StartDate < c.EndDate
                                                            && priceEpisode.StartDate >= c.EffectiveFrom 
                                                            && (c.EffectiveTo == null || priceEpisode.StartDate <= c.EffectiveTo)).ToList();

            if (!commitmentsToMatch.Any())
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.EarlierStartDate);
                matchResult.Commitments = commitments;
            }
            else
            {
                matchResult.Commitments = commitmentsToMatch;
            }

            return ExecuteNextHandler(commitmentsToMatch, priceEpisode,dasAccounts,matchResult);
        }
    }
}