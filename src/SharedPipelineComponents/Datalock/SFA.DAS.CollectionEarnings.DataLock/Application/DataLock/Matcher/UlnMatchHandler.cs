using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class UlnMatchHandler : MatchHandler
    {
        public UlnMatchHandler(MatchHandler nextMatchHandler) :
                base(nextMatchHandler)
        {}
      
        public override MatchResult Match(IReadOnlyList<CommitmentEntity> commitments, RawEarning priceEpisode, MatchResult matchResult)
        {
            var commitmentsToMatch = commitments.Where(c => c.Uln == priceEpisode.Uln).ToList();

            if (!commitmentsToMatch.Any())
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingUln);
            }

            matchResult.Commitments = commitmentsToMatch.ToArray();
            return ExecuteNextHandler(commitmentsToMatch, priceEpisode, matchResult);
        }
    }
}
