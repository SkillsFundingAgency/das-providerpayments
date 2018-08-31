using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class StandardMatchHandler : MatchHandler
    {
        public StandardMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {}

        public override bool StopOnError { get { return false; } }

        public override MatchResult Match(IReadOnlyList<Commitment> commitments, RawEarning earning,
            DateTime censusDate,
            MatchResult matchResult)
        {
            matchResult.Commitments = commitments.ToArray();

            var hasStandardCode = earning.StandardCode > 0 ||
                                  commitments.Any(x => x.StandardCode.HasValue && x.StandardCode > 0);

            if (hasStandardCode)
            {
                var commitmentsToMatch = commitments.Where(c => c.StandardCode.HasValue &&
                                                                c.StandardCode.Value == earning.StandardCode).ToList();

                if (!commitmentsToMatch.Any())
                {
                    matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingStandard);
                }
                else
                {
                    matchResult.Commitments = commitmentsToMatch.ToArray();
                }
            }
            
            return ExecuteNextHandler(commitments, earning, censusDate, matchResult);
        }
    }
}