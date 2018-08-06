using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher.MatchingSteps
{
    public class UlnMatchHandler : MatchHandler
    {
        public UlnMatchHandler(MatchHandler nextMatchHandler) :
                base(nextMatchHandler)
        {}
      
        public override MatchResult Match(List<Commitment> commitments, PriceEpisode priceEpisode, List<DasAccount> dasAccounts, MatchResult matchResult)
        {
            var commitmentsToMatch = commitments.Where(c => priceEpisode.Uln.HasValue && c.Uln == priceEpisode.Uln.Value).ToList();

            if (!commitmentsToMatch.Any())
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingUln);
            }

            matchResult.Commitments = commitmentsToMatch;
            return ExecuteNextHandler(commitmentsToMatch, priceEpisode,dasAccounts, matchResult);
        }
    }
}
