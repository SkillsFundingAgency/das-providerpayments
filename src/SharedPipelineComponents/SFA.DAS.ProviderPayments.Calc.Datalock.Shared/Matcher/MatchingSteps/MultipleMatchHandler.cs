using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher.MatchingSteps
{
    public class MultipleMatchHandler : MatchHandler
    {
        public MultipleMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {

        }

        public override MatchResult Match(List<Commitment> commitments, PriceEpisode priceEpisode, List<DasAccount> dasAccounts, MatchResult matchResult)
        {
            var distinctCommitmentIds = commitments
                .Where(x=> x.PaymentStatus != (int)PaymentStatus.Cancelled 
                        && x.PaymentStatus != (int)PaymentStatus.Deleted
                        && x.PaymentStatus != (int)PaymentStatus.Completed)
                .Select(c => new {Id = c.CommitmentId})
                .Distinct()
                .ToArray();

            if (distinctCommitmentIds.Length > 1)
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.MultipleMatches);
            }
           
            matchResult.Commitments = commitments;

            return ExecuteNextHandler(commitments, priceEpisode,dasAccounts, matchResult);
        }
    }
}