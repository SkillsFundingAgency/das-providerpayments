using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
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
        {}

        public override MatchResult Match(List<CommitmentEntity> commitments, RawEarning priceEpisode, List<DasAccount.DasAccount> dasAccounts, MatchResult matchResult)
        {
            matchResult.Commitments = commitments.ToArray();

            if (priceEpisode.StandardCode == 0)
            {
                var commitmentsToMatch = commitments.Where(c => c.FrameworkCode.HasValue &&
                                                                priceEpisode.FrameworkCode != 00 &&
                                                                c.FrameworkCode.Value == priceEpisode.FrameworkCode)
                    .ToList();

                if (!commitmentsToMatch.Any())
                {
                    matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingFramework);
                }
                else
                {
                    matchResult.Commitments = commitmentsToMatch.ToArray();
                }
            }

            return ExecuteNextHandler(commitments, priceEpisode,dasAccounts, matchResult);
        }
    }
}