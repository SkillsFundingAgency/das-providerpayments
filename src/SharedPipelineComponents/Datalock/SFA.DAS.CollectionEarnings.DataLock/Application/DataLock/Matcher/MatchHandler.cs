using System.Collections.Generic;

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

        public MatchResult Match(List<Commitment.Commitment> commitments, PriceEpisode.PriceEpisode priceEpisode, List<DasAccount.DasAccount> dasAccounts)
        {
            return Match(commitments, priceEpisode,dasAccounts, new MatchResult() );
        }

        public abstract MatchResult Match(List<Commitment.Commitment> commitments, PriceEpisode.PriceEpisode priceEpisode, List<DasAccount.DasAccount> dasAccounts, MatchResult matchResult);

        protected MatchResult ExecuteNextHandler(List<Commitment.Commitment> commitments, PriceEpisode.PriceEpisode priceEpisode, List<DasAccount.DasAccount> dasAccounts, MatchResult matchResult)
        {
           
            return NextMatchHandler == null || (StopOnError && matchResult.ErrorCodes.Count > 0)
                ? matchResult
                : NextMatchHandler.Match(commitments, priceEpisode,dasAccounts, matchResult);
        }

    }
}