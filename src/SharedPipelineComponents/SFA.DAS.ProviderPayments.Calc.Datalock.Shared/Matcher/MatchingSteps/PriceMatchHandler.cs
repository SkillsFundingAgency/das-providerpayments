using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher.MatchingSteps
{
    public class PriceMatchHandler : MatchHandler
    {
        public PriceMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {}

        public override bool StopOnError
        {
            get
            {
                return false;
            }
        }

        public override MatchResult Match(List<Commitment> commitments, PriceEpisode priceEpisode, List<DasAccount> dasAccounts, MatchResult matchResult)
        {
            matchResult.Commitments = commitments;

            var commitmentsToMatch = commitments.Where(c => priceEpisode.NegotiatedPrice.HasValue &&
                                                            (long) c.NegotiatedPrice == priceEpisode.NegotiatedPrice.Value).ToList();

            if (!commitmentsToMatch.Any())
            {
               matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingPrice);
            }
        
            else
            {
                matchResult.Commitments = commitmentsToMatch;
            }

            return ExecuteNextHandler(commitments, priceEpisode,dasAccounts,matchResult);
        }
    }
}