using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class LevyPayerFlagMatcher : MatchHandler
    {
        public LevyPayerFlagMatcher(MatchHandler nextMatchHandler) :
                base(nextMatchHandler)
        {}

        public override MatchResult Match(IReadOnlyList<CommitmentEntity> commitments, RawEarning priceEpisode, IReadOnlyList<DasAccount.DasAccount> dasAccounts, MatchResult matchResult)
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