using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    class WithdrawnCommitmentMatcher : MatchHandler
    {
        public WithdrawnCommitmentMatcher(MatchHandler nextMatchHandler) : base(nextMatchHandler)
        {}

        public override bool StopOnError { get { return false; } }


        public override MatchResult Match(IReadOnlyList<Commitment> commitments, RawEarning earning,
            DateTime censusDate, MatchResult matchResult)
        {
            var withdrawnCommitments = commitments
                .Where(x => x.PaymentStatus == 3 || x.WithdrawnOnDate.HasValue)
                .ToList();
            var activeWithdrawnCommitments = withdrawnCommitments
                .Where(x => x.WithdrawnOnDate > censusDate)
                .ToList();

            if (withdrawnCommitments.Any() && !activeWithdrawnCommitments.Any())
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.EmployerStopped);
            }
            
            return ExecuteNextHandler(commitments, earning, censusDate, matchResult);
        }
    }
}
