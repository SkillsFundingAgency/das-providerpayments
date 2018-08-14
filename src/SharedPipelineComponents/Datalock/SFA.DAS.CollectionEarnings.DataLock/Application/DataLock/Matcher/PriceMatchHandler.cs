using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class PriceMatchHandler : MatchHandler
    {
        public PriceMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {}

        public override bool StopOnError { get { return false; } }

        public override MatchResult Match(IReadOnlyList<CommitmentEntity> commitments, RawEarning priceEpisode, MatchResult matchResult)
        {
            matchResult.Commitments = commitments.ToArray();
            // TODO!
            var commitmentsToMatch = commitments.Where(c => c.AgreedCost == priceEpisode.AgreedPrice).ToList();

            if (!commitmentsToMatch.Any())
            {
               matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingPrice);
            }
        
            else
            {
                matchResult.Commitments = commitmentsToMatch.ToArray();
            }

            return ExecuteNextHandler(commitments, priceEpisode,matchResult);
        }
    }
}