using System;
using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public abstract class MatchHandler : IMatcher
    {
        public virtual bool StopOnError { get { return true; } }

        protected MatchHandler NextMatchHandler;

        protected MatchHandler(MatchHandler nextMatchHandler)
        {
            NextMatchHandler = nextMatchHandler;
        }

        public MatchResult Match(IReadOnlyList<Commitment> commitments, RawEarning earning, DateTime censusDate)
        {
            return Match(commitments, earning, censusDate, new MatchResult() );
        }

        public abstract MatchResult Match(IReadOnlyList<Commitment> commitments, RawEarning earning,
            DateTime censusDate, MatchResult matchResult);

        protected MatchResult ExecuteNextHandler(IReadOnlyList<Commitment> commitments, RawEarning priceEpisode,
            DateTime censusDate, MatchResult matchResult)
        {
            return NextMatchHandler == null || (StopOnError && matchResult.ErrorCodes.Count > 0)
                ? matchResult
                : NextMatchHandler.Match(commitments, priceEpisode, censusDate, matchResult);
        }
    }
}