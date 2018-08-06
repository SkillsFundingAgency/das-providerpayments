using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher.MatchingSteps
{
    public class LevyPayerFlagMatcher : MatchHandler
    {
        public LevyPayerFlagMatcher(MatchHandler nextMatchHandler) :
                base(nextMatchHandler)
        {

        }

        public override MatchResult Match(List<Commitment> commitments, PriceEpisode priceEpisode, List<DasAccount> dasAccounts, MatchResult matchResult)
        {
            var accountsMatch = dasAccounts.Where(a => commitments.Any(c => c.AccountId == a.AccountId && a.IsLevyPayer == true));

            if (!accountsMatch.Any())
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.NotLevyPayer);
            }


            return ExecuteNextHandler(commitments, priceEpisode, dasAccounts, matchResult);
        }
    }
}