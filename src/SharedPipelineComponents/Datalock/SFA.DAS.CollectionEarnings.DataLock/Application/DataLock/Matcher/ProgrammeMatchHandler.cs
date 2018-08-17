﻿using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class ProgrammeMatchHandler : MatchHandler
    {
        public ProgrammeMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {}

        public override bool StopOnError { get { return false; } }
        
        public override MatchResult Match(IReadOnlyList<CommitmentEntity> commitments, RawEarning earning, MatchResult matchResult)
        {
            matchResult.Commitments = commitments.ToArray();

            var hasProgrammeType = earning.ProgrammeType > 0 ||
                                   commitments.Any(x => x.ProgrammeType.HasValue && x.ProgrammeType > 0);

            if (hasProgrammeType)
            {
                var commitmentsToMatch = commitments.Where(c => c.ProgrammeType.HasValue &&
                                                                earning.ProgrammeType != 0 &&
                                                                c.ProgrammeType.Value == earning.ProgrammeType)
                    .ToList();

                if (!commitmentsToMatch.Any())
                {
                    matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingProgramme);
                }
                else
                {
                    matchResult.Commitments = commitmentsToMatch.ToArray();
                }
            }

            return ExecuteNextHandler(commitments, earning, matchResult);
        }
    }
}