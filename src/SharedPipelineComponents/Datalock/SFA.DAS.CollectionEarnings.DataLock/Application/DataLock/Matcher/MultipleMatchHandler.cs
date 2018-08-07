using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class MultipleMatchHandler : MatchHandler
    {
        public MultipleMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {}

        public override MatchResult Match(List<CommitmentEntity> commitments, RawEarning priceEpisode, List<DasAccount.DasAccount> dasAccounts, MatchResult matchResult)
        {
            var distinctCommitmentIds = commitments
                .Where(x=> x.PaymentStatus != (int)Payments.DCFS.Domain.PaymentStatus.Cancelled 
                        && x.PaymentStatus != (int)Payments.DCFS.Domain.PaymentStatus.Deleted
                        && x.PaymentStatus != (int)Payments.DCFS.Domain.PaymentStatus.Completed)
                .Select(c => new {Id = c.CommitmentId})
                .Distinct()
                .ToArray();

            if (distinctCommitmentIds.Length > 1)
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.MultipleMatches);
            }
           
            matchResult.Commitments = commitments.ToArray();

            return ExecuteNextHandler(commitments, priceEpisode,dasAccounts, matchResult);
        }
    }
}