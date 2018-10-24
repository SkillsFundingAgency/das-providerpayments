using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class PathwayMatchHandler : MatchHandler
    {
        public PathwayMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {}

        public override bool StopOnError { get { return false; } }

        public override MatchResult Match(IReadOnlyList<Commitment> commitments, RawEarning earning,
            DateTime censusDate,
            MatchResult matchResult)
        {
            matchResult.Commitments = commitments.ToArray();

            var hasPathwayCode = earning.PathwayCode > 0 ||
                                 commitments.Any(x => x.PathwayCode.HasValue && x.PathwayCode > 0);

            if (hasPathwayCode)
            {
                var commitmentsToMatch = commitments.Where(c => c.PathwayCode.HasValue &&
                                                                earning.PathwayCode > 0 &&
                                                                c.PathwayCode.Value == earning.PathwayCode).ToList();

                if (!commitmentsToMatch.Any())
                {
                    matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingPathway);
                }
            }
            
            return ExecuteNextHandler(commitments, earning, censusDate, matchResult);
        }
    }
}