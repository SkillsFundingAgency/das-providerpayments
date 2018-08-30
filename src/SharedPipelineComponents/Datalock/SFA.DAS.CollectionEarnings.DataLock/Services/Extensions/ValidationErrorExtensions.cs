using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Interfaces.Payments;

namespace SFA.DAS.CollectionEarnings.DataLock.Services.Extensions
{
    public static class ValidationErrorExtensions
    {
        public static bool IsEqualTo(this IIdentifyCommitments source, IIdentifyCommitments rhs)
        {
            return source.LearnRefNumber == rhs.LearnRefNumber &&
                   source.AimSeqNumber == rhs.AimSeqNumber &&
                   source.PriceEpisodeIdentifier == rhs.PriceEpisodeIdentifier &&
                   source.Ukprn == rhs.Ukprn;
        }

        public static bool DoesNotContainMatch(this List<DatalockValidationError> source, IIdentifyCommitments earning, string error)
        {
            if (source.FirstOrDefault(x => x.IsEqualTo(earning) && x.RuleId == error) == null)
            {
                return true;
            }

            return false;
        }

        public static bool DoesNotContainMatch(this List<DatalockValidationErrorByPeriod> source, 
            RawEarning earning, 
            string error)
        {
            if (source.FirstOrDefault(x => x.IsEqualTo(earning) && 
                                           x.RuleId == error &&
                                           x.Period == earning.Period) == null)
            {
                return true;
            }

            return false;
        }
    }
}
