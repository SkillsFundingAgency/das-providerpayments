using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public abstract class MatchHandler : IMatcher
    {
        public virtual bool StopOnError
        {
            get
            {
                return true;
            }
        }

        protected MatchHandler NextMatchHandler;

        protected MatchHandler(MatchHandler nextMatchHandler)
        {
            NextMatchHandler = nextMatchHandler;
        }

        public MatchResult Match(List<CommitmentEntity> commitments, PriceEpisode.PriceEpisode priceEpisode, List<DasAccount.DasAccount> dasAccounts)
        {
            return Match(commitments, priceEpisode,dasAccounts, new MatchResult() );
        }

        public abstract MatchResult Match(List<CommitmentEntity> commitments, PriceEpisode.PriceEpisode priceEpisode, List<DasAccount.DasAccount> dasAccounts, MatchResult matchResult);

        protected MatchResult ExecuteNextHandler(List<CommitmentEntity> commitments, PriceEpisode.PriceEpisode priceEpisode, List<DasAccount.DasAccount> dasAccounts, MatchResult matchResult)
        {
           
            return NextMatchHandler == null || (StopOnError && matchResult.ErrorCodes.Count > 0)
                ? matchResult
                : NextMatchHandler.Match(commitments, priceEpisode,dasAccounts, matchResult);
        }

    }
}