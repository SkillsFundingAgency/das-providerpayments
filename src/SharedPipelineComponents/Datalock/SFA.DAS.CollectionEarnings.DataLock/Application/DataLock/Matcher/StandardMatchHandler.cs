using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class StandardMatchHandler : MatchHandler
    {
        public StandardMatchHandler(MatchHandler nextMatchHandler):
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

        public override  MatchResult Match(List<CommitmentEntity> commitments, PriceEpisode.PriceEpisode priceEpisode, List<DasAccount.DasAccount> dasAccounts, MatchResult matchResult)
        {
            matchResult.Commitments = commitments.ToArray();
            if (priceEpisode.StandardCode.HasValue)
            {
                var commitmentsToMatch = commitments.Where(c => c.StandardCode.HasValue &&
                                                                c.StandardCode.Value == priceEpisode.StandardCode.Value).ToList();

                if (!commitmentsToMatch.Any())
                {
                   matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingStandard);
                }
                else
                {
                    matchResult.Commitments = commitmentsToMatch.ToArray();
                }
            }

            return ExecuteNextHandler(commitments, priceEpisode,dasAccounts,matchResult);
        }
    }
}