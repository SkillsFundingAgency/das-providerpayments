using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher.MatchingSteps
{
    public class ProgrammeMatchHandler : MatchHandler
    {
        public ProgrammeMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {}

        public override bool StopOnError
        {
            get
            {
                return false;
            }
        }

        public override MatchResult Match(List<Commitment> commitments, PriceEpisode priceEpisode, List<DasAccount> dasAccounts, MatchResult matchResult)
        {
            matchResult.Commitments = commitments;

            if (!priceEpisode.StandardCode.HasValue)
            {
                var commitmentsToMatch = commitments.Where(c => c.ProgrammeType.HasValue &&
                                                                priceEpisode.ProgrammeType.HasValue &&
                                                                c.ProgrammeType.Value == priceEpisode.ProgrammeType.Value)
                    .ToList();

                if (!commitmentsToMatch.Any())
                {
                    matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingProgramme);
                }
                else
                {
                    matchResult.Commitments = commitmentsToMatch;
                }
            }

            return ExecuteNextHandler(commitments, priceEpisode,dasAccounts, matchResult);
        }
    }
}