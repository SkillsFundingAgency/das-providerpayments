using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class StandardMatchHandler : MatchHandler
    {
        public StandardMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {

        }
        public override bool StopOnError { get { return false; } }

        public override  MatchResult Match(IReadOnlyList<CommitmentEntity> commitments, RawEarning priceEpisode, MatchResult matchResult)
        {
            matchResult.Commitments = commitments.ToArray();

            if (priceEpisode.StandardCode > 0)
            {
                var commitmentsToMatch = commitments.Where(c => c.StandardCode.HasValue &&
                                                                c.StandardCode.Value == priceEpisode.StandardCode).ToList();

                if (!commitmentsToMatch.Any())
                {
                   matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingStandard);
                }
                else
                {
                    matchResult.Commitments = commitmentsToMatch.ToArray();
                }
            }

            return ExecuteNextHandler(commitments, priceEpisode,matchResult);
        }
    }
}