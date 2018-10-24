using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Services.Extensions
{
    public static class PriceEpisodePeriodMatchExtensions
    {
        public static bool MatchesEarning(this PriceEpisodePeriodMatchEntity source, RawEarning rhs)
        {
            return source.AimSeqNumber == rhs.AimSeqNumber &&
                   source.LearnRefNumber == rhs.LearnRefNumber &&
                   source.PriceEpisodeIdentifier == rhs.PriceEpisodeIdentifier &&
                   source.Ukprn == rhs.Ukprn &&
                   source.Period == rhs.Period;
        }

        public static bool MatchesCommitment(this PriceEpisodePeriodMatchEntity source, CommitmentEntity rhs)
        {
            return source.CommitmentId == rhs.CommitmentId &&
                   source.VersionId == rhs.VersionId;
        }

        public static bool DoesNotContainEarningForCommitmentAndPaymentType(this List<PriceEpisodePeriodMatchEntity> source, 
            RawEarning earning, 
            CommitmentEntity commitment,
            CensusDateType censusDateType)
        {
            if (source.FirstOrDefault(x => x.MatchesEarning(earning) && 
                                           x.MatchesCommitment(commitment) &&
                                           x.Payable &&
                                           x.TransactionTypesFlag == censusDateType) == null)
            {
                return true;
            }

            return false;
        }
    }
}

