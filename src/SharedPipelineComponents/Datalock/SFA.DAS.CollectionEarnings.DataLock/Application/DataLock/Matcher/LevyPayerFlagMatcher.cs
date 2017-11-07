using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class LevyPayerFlagMatcher : MatchHandler
    {
        public LevyPayerFlagMatcher(MatchHandler nextMatchHandler) :
                base(nextMatchHandler)
        {

        }

        public override MatchResult Match(List<Commitment.Commitment> commitments, PriceEpisode.PriceEpisode priceEpisode, List<DasAccount.DasAccount> dasAccounts, MatchResult matchResult)
        {
            var commitment = commitments.FirstOrDefault();
            var accountsMatch = dasAccounts.Where(a => commitments.Any(c => c.AccountId == a.AccountId && a.IsLevyPayer == true));

            if (!accountsMatch.Any())
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.NotLevyPayer);
            }


            return ExecuteNextHandler(commitments, priceEpisode, dasAccounts, matchResult);
        }
    }
}