using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class UlnMatchHandler : MatchHandler
    {
        public UlnMatchHandler(MatchHandler nextMatchHandler) :
                base(nextMatchHandler)
        {

        }
      
        public override MatchResult Match(List<CommitmentEntity> commitments, PriceEpisode.PriceEpisode priceEpisode, List<DasAccount.DasAccount> dasAccounts, MatchResult matchResult)
        {
            var commitmentsToMatch = commitments.Where(c => priceEpisode.Uln.HasValue && c.Uln == priceEpisode.Uln.Value).ToList();

            if (!commitmentsToMatch.Any())
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingUln);
            }

            matchResult.Commitments = commitmentsToMatch.ToArray();
            return ExecuteNextHandler(commitmentsToMatch, priceEpisode,dasAccounts, matchResult);
        }
    }
}
