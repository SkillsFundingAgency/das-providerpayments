using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.ProviderPayments.Calc.Common.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class FrameworkMatchHandler : MatchHandler
    {
        public override bool StopOnError { get { return false; } }

        public FrameworkMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {}

        public override MatchResult Match(IReadOnlyList<Commitment> commitments, RawEarning earning,
            DateTime censusDate, MatchResult matchResult)
        {
            matchResult.Commitments = commitments.ToArray();

            var hasFrameworkCode = earning.FrameworkCode > 0 || 
                                   commitments.Any(x => x.FrameworkCode.HasValue && x.FrameworkCode > 0);

            if (hasFrameworkCode)
            {
                var commitmentsToMatch = commitments.Where(c => c.FrameworkCode.HasValue &&
                                                                earning.FrameworkCode != 0 &&
                                                                c.FrameworkCode.Value == earning.FrameworkCode)
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
            else
            {
                matchResult.Commitments = commitments.ToArray();
            }
            
            return ExecuteNextHandler(commitments, earning, censusDate, matchResult);
        }
    }
}