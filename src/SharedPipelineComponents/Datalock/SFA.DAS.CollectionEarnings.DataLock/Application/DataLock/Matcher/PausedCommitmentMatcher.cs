using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    class PausedCommitmentMatcher : MatchHandler
    {
        public PausedCommitmentMatcher(MatchHandler nextMatchHandler) : base(nextMatchHandler)
        {}

        public override bool StopOnError { get { return false; } }
        
        public override MatchResult Match(IReadOnlyList<Commitment> commitments, RawEarning earning,
            DateTime censusDate, MatchResult matchResult)
        {
            if (commitments.Any(x => x.PaymentStatus == (int)PaymentStatus.Paused))
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.EmployerPaused);
            }
            
            return ExecuteNextHandler(commitments, earning, censusDate, matchResult);
        }
    }
}
