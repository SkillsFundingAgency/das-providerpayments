using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    class StartDateMatcher : MatchHandler
    {
        public StartDateMatcher(MatchHandler nextMatchHandler) : base(nextMatchHandler)
        {}

        public override bool StopOnError { get { return false; } }

        public override MatchResult Match(IReadOnlyList<Commitment> commitments, RawEarning earning,
            DateTime censusDate,
            MatchResult matchResult)
        {
            var commitmentsToMatch = commitments.Where(c =>
            {
                if (c.IsVersioned)
                {
                    return c.EffectiveFrom <= earning.EpisodeStartDate;
                }
                return c.StartDate <= earning.EpisodeStartDate;
            }).ToList();

            if (!commitmentsToMatch.Any())
            {
                matchResult.ErrorCodes.Add(DataLockErrorCodes.EarlierStartDate);
                matchResult.Commitments = commitments.ToArray();
            }
            else
            {
                matchResult.Commitments = commitmentsToMatch.ToArray();
            }

            return ExecuteNextHandler(commitments, earning, censusDate, matchResult);
        }
    }
}
