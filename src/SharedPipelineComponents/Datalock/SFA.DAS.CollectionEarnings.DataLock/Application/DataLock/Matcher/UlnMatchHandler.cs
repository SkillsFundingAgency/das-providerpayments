using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class UlnMatchHandler : MatchHandler
    {
        public UlnMatchHandler(MatchHandler nextMatchHandler) :
                base(nextMatchHandler)
        {}
      
        public override MatchResult Match(IReadOnlyList<Commitment> commitments, RawEarning earning,
            DateTime censusDate,
            MatchResult matchResult)
        {
            var commitmentsToMatch = commitments.Where(c => c.Uln == earning.Uln).ToList();

            if (!commitmentsToMatch.Any())
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingUln);
            }

            matchResult.Commitments = commitmentsToMatch.ToArray();
            return ExecuteNextHandler(commitmentsToMatch, earning, censusDate, matchResult);
        }
    }
}
