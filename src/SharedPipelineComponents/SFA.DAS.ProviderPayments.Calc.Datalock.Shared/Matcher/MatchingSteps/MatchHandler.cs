using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher.MatchingSteps
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

        public MatchResult Match(List<Commitment> commitments, PriceEpisode priceEpisode, List<DasAccount> dasAccounts)
        {
            return Match(commitments, priceEpisode, dasAccounts, new MatchResult());
        }

        public abstract MatchResult Match(List<Commitment> commitments, PriceEpisode priceEpisode, List<DasAccount> dasAccounts, MatchResult matchResult);

        protected MatchResult ExecuteNextHandler(List<Commitment> commitments, PriceEpisode priceEpisode, List<DasAccount> dasAccounts, MatchResult matchResult)
        {

            return NextMatchHandler == null || (StopOnError && matchResult.ErrorCodes.Count > 0)
                ? matchResult
                : NextMatchHandler.Match(commitments, priceEpisode, dasAccounts, matchResult);
        }
    }
}