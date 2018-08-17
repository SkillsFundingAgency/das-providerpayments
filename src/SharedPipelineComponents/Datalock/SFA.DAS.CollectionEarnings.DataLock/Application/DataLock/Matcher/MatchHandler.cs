using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
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

        public MatchResult Match(IReadOnlyList<CommitmentEntity> commitments, RawEarning earning)
        {
            return Match(commitments, earning, new MatchResult() );
        }

        public abstract MatchResult Match(IReadOnlyList<CommitmentEntity> commitments, RawEarning earning, MatchResult matchResult);

        protected MatchResult ExecuteNextHandler(IReadOnlyList<CommitmentEntity> commitments, RawEarning priceEpisode, MatchResult matchResult)
        {
            return NextMatchHandler == null || (StopOnError && matchResult.ErrorCodes.Count > 0)
                ? matchResult
                : NextMatchHandler.Match(commitments, priceEpisode, matchResult);
        }
    }
}