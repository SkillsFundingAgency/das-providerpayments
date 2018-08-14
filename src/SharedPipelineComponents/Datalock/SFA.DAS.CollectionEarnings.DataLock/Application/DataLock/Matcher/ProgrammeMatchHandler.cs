using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class ProgrammeMatchHandler : MatchHandler
    {
        public ProgrammeMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {}

        public override bool StopOnError { get { return false; } }
        
        public override MatchResult Match(IReadOnlyList<CommitmentEntity> commitments, RawEarning priceEpisode, IReadOnlyList<DasAccount.DasAccount> dasAccounts, MatchResult matchResult)
        {
            matchResult.Commitments = commitments.ToArray();

            if (priceEpisode.StandardCode == 0)
            {
                var commitmentsToMatch = commitments.Where(c => c.ProgrammeType.HasValue &&
                                                                priceEpisode.ProgrammeType != 0 &&
                                                                c.ProgrammeType.Value == priceEpisode.ProgrammeType)
                    .ToList();

                if (!commitmentsToMatch.Any())
                {
                    matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingProgramme);
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