using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class UkprnMatchHandler : MatchHandler

    {
        public UkprnMatchHandler(MatchHandler nextMatchHandler) :
                base(nextMatchHandler)
        {}
      
        public override MatchResult Match(IReadOnlyList<CommitmentEntity> commitments, RawEarning priceEpisode, MatchResult matchResult)
        {
            var commitmentsToMatch = commitments.Where(c => c.Ukprn == priceEpisode.Ukprn).ToList();

            if (!commitmentsToMatch.Any())
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingUkprn);
                matchResult.Commitments = commitments.ToArray();
            }
            else
            {
                matchResult.Commitments = commitmentsToMatch.ToArray();
            }

            return ExecuteNextHandler(commitmentsToMatch, priceEpisode, matchResult);
        }
    }
}