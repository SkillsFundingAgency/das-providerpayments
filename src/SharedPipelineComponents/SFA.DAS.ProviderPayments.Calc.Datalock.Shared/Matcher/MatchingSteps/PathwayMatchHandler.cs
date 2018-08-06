using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher.MatchingSteps
{
    public class PathwayMatchHandler : MatchHandler
    {
        public PathwayMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {

        }

        public override bool StopOnError
        {
            get
            {
                return false;
            }
        }

        public override MatchResult Match(List<Commitment> commitments, PriceEpisode priceEpisode, List<DasAccount> dasAccounts,  MatchResult matchResult)
        {
            matchResult.Commitments = commitments;

            if (!priceEpisode.StandardCode.HasValue)
            {
                var commitmentsToMatch = commitments.Where(c => c.PathwayCode.HasValue &&
                                                                priceEpisode.PathwayCode.HasValue &&
                                                                c.PathwayCode.Value == priceEpisode.PathwayCode.Value).ToList();

                if (!commitmentsToMatch.Any())
                {
                    matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingPathway);
                }
                else
                {
                    matchResult.Commitments = commitmentsToMatch;
                }
            }

            return ExecuteNextHandler(commitments, priceEpisode,dasAccounts,matchResult);
        }
    }
}