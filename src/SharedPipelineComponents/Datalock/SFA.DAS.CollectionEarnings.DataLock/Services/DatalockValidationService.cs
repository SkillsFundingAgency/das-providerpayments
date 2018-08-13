using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Services
{
    public interface IValidateDatalocks
    {
        DatalockValidationResult ValidateDatalockForProvider(ProviderCommitments providerCommitments, List<RawEarning> providerEarnings, List<DasAccount> accounts);
    }

    public class DatalockValidationService : IValidateDatalocks
    {
        private readonly IMatcher _datalockMatcher;

        public DatalockValidationService(IMatcher datalockMatcher)
        {
            _datalockMatcher = datalockMatcher;
        }

        public DatalockValidationResult ValidateDatalockForProvider(
            ProviderCommitments providerCommitments, 
            List<RawEarning> providerEarnings, 
            List<DasAccount> accounts)
        {
            var earningsByLearner = new ProviderEarnings(providerEarnings);
            var processedLearners = new HashSet<long>();
            
            var result = new DatalockValidationResult();

            foreach (var uln in providerCommitments.AllUlns())
            {
                processedLearners.Add(uln);

                var allCommitments = providerCommitments.CommitmentsForLearner(uln);
                var earnings = earningsByLearner.EarningsForLearner(uln);

                foreach (var earning in earnings)
                {
                    if (earning.HasNonIncentiveEarnings())
                    {
                        var onProgCensusDate = CalculateOnProgCensusDate(earning);
                        var commitments = allCommitments.CommitmentsForDate(onProgCensusDate).ToList();
                        var datamatchResult = _datalockMatcher.Match(commitments, earning, accounts);
                        result += datamatchResult;
                    }

                    if (earning.HasFirstIncentive())
                    {
                        var commitmentsForFirstIncentive = allCommitments.CommitmentsForDate(earning.FirstIncentiveCensusDate.Value);
                        var datamatchResult = _datalockMatcher.Match(commitmentsForFirstIncentive, earning, accounts);
                        result += datamatchResult;
                    }

                    if (earning.HasSecondIncentive())
                    {
                        var commitmentsForSecondIncentive = allCommitments.CommitmentsForDate(earning.SecondIncentiveCensusDate.Value);
                        var datamatchResult = _datalockMatcher.Match(commitmentsForSecondIncentive, earning, accounts);
                        result += datamatchResult;
                    }
                }
            }

            return result;
        }

        private DateTime CalculateOnProgCensusDate(RawEarning earning)
        {
            var month = MonthFromPeriod(earning.Period);
            var year = YearFromPeriod(earning.Period, earning.EpisodeStartDate ?? new DateTime(9999, 01, 01));

            var lastDayOfMonth = DateTime.DaysInMonth(year, month);
            return new DateTime(year, month, lastDayOfMonth);
        }

        private int MonthFromPeriod(int period)
        {
            if (period < 6)
            {
                return period + 7;
            }

            return period - 5;
        }

        private int YearFromPeriod(int period, DateTime episodeStartDate)
        {
            if (episodeStartDate.Month >= 8)
            {
                return episodeStartDate.Year;
            }
            return episodeStartDate.Year + 1;
        }
    }

    public static class EarningExtensions
    {
        public static bool HasFirstIncentive(this RawEarning earning)
        {
            return (earning.TransactionType04 > 0 || earning.TransactionType05 > 0) && 
                   earning.FirstIncentiveCensusDate.HasValue;
        }

        public static bool HasSecondIncentive(this RawEarning earning)
        {
            return (earning.TransactionType06 > 0 || earning.TransactionType07 > 0) &&
                   earning.SecondIncentiveCensusDate.HasValue;
        }

        public static bool HasNonIncentiveEarnings(this RawEarning earning)
        {
            return (earning.TransactionType01 > 0 ||
                    earning.TransactionType02 > 0 ||
                    earning.TransactionType03 > 0 ||
                    earning.TransactionType08 > 0 ||
                    earning.TransactionType09 > 0 ||
                    earning.TransactionType10 > 0 ||
                    earning.TransactionType11 > 0 ||
                    earning.TransactionType12 > 0 ||
                    earning.TransactionType13 > 0 ||
                    earning.TransactionType14 > 0 ||
                    earning.TransactionType15 > 0
                );
        }
    }

    public class DatalockValidationResult
    {
        public IEnumerable<DatalockValidationError> ValidationErrors { get; set; }
        public IEnumerable<PriceEpisodePeriodMatchEntity> PriceEpisodePeriodMatches { get; set; }
        public IEnumerable<PriceEpisodeMatchEntity> PriceEpisodeMatches { get; set; }
        public IEnumerable<DatalockOutputEntity> DatalockOutputEntities { get; set; }

        public static DatalockValidationResult operator + (DatalockValidationResult lhs, MatchResult rhs)
        {
            return lhs;
        }
    }
}
