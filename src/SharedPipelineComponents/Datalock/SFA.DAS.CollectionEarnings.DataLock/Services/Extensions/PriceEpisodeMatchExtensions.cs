using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Services.Extensions
{
    public static class PriceEpisodeMatchExtensions
    {
        public static bool MatchesEarning(this PriceEpisodeMatchEntity source, RawEarning rhs)
        {
            return source.LearnRefNumber == rhs.LearnRefNumber &&
                   source.PriceEpisodeIdentifier == rhs.PriceEpisodeIdentifier &&
                   source.Ukprn == rhs.Ukprn;
        }

        public static bool MatchesCommitment(this PriceEpisodeMatchEntity source, CommitmentEntity rhs)
        {
            return source.CommitmentId == rhs.CommitmentId;
        }

        public static bool DoesNotContainMatch(this List<PriceEpisodeMatchEntity> source,
            RawEarning earning,
            CommitmentEntity commitment,
            bool payable)
        {
            if (source.FirstOrDefault(x => x.MatchesEarning(earning) &&
                                           x.MatchesCommitment(commitment) &&
                                           x.IsSuccess == payable) == null)
            {
                return true;
            }

            return false;
        }
    }
}