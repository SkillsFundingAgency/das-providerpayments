using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher.MatchingSteps
{
    public class FrameworkMatchHandler : MatchHandler
    {
        public override bool StopOnError
        {
            get
            {
                return false;
            }
        }

        public FrameworkMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {
           
        }

        public override MatchResult Match(List<Commitment> commitments, PriceEpisode priceEpisode, List<DasAccount> dasAccounts, MatchResult matchResult)
        {
            matchResult.Commitments = commitments;

            if (!priceEpisode.StandardCode.HasValue)
            {
                var commitmentsToMatch = commitments.Where(c => c.FrameworkCode.HasValue &&
                                                                priceEpisode.FrameworkCode.HasValue &&
                                                                c.FrameworkCode.Value == priceEpisode.FrameworkCode.Value)
                    .ToList();

                if (!commitmentsToMatch.Any())
                {
                    matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingFramework);
                }
                else
                {
                    matchResult.Commitments = commitmentsToMatch;
                }

            }

            return ExecuteNextHandler(commitments, priceEpisode,dasAccounts, matchResult);
        }
    }
}