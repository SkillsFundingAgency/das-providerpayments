using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions;

namespace SFA.DAS.CollectionEarnings.DataLock.Services
{
    public interface IValidateDatalocks
    {
        DatalockValidationResult ValidateDatalockForProvider(
            ProviderCommitments providerCommitments, 
            IEnumerable<RawEarning> providerEarnings, 
            ImmutableHashSet<long> accountsWithNonPayableFlagSet);
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
            IEnumerable<RawEarning> providerEarnings,
            ImmutableHashSet<long> accountsWithNonPayableFlagSet)
        {
            var earnings = new ProviderEarnings(providerEarnings);
            var processedLearners = new HashSet<long>();
            
            var resultBuilder = new DatalockValidationResultBuilder();

            var learners = earnings.AllUlns();

            foreach (var uln in learners)
            {
                processedLearners.Add(uln);

                var learnerCommitments = providerCommitments.CommitmentsForLearner(uln);
                var learnerEarnings = earnings.EarningsForLearner(uln);

                foreach (var earning in learnerEarnings)
                {
                    foreach (CensusDateType censusDateType in Enum.GetValues(typeof(CensusDateType)))
                    {
                        if (censusDateType == CensusDateType.All)
                        {
                            continue;
                        }

                        if (earning.HasValidTransactionsForCensusDateType(censusDateType))
                        {
                            ProcessEarning(accountsWithNonPayableFlagSet, earning,
                                learnerCommitments, resultBuilder, censusDateType);
                        }
                    }
                }
            }

            var result = resultBuilder.Build();
            return result;
        }

        private void ProcessEarning(ImmutableHashSet<long> accountsWithNonPayableFlagSet, RawEarning earning,
            LearnerCommitments learnerCommitments, DatalockValidationResultBuilder result, CensusDateType earningType)
        {
            var censusDate = CalculateCensusDate(earning, earningType);
            var commitments = learnerCommitments.ActiveCommitmentsForDate(censusDate).ToList();
            var matchResult = _datalockMatcher.Match(commitments, earning, censusDate);
            ValidateInitialResult(earning, matchResult.ErrorCodes, earningType,
                commitments, result, accountsWithNonPayableFlagSet, learnerCommitments.Commitments, censusDate);
        }

        private DateTime CalculateCensusDate(RawEarning earning, CensusDateType censusDateType)
        {
            var date = new DateTime(1900, 01, 01);
            switch (censusDateType)
            {
                case CensusDateType.OnProgLearning:
                    date = CalculateOnProgCensusDate(earning);
                    break;
                case CensusDateType.First16To18Incentive:
                    date = earning.FirstIncentiveCensusDate ?? date;
                    break;
                case CensusDateType.Second16To18Incentive:
                    date = earning.SecondIncentiveCensusDate ?? date;
                    break;
                case CensusDateType.CompletionPayments:
                    date = earning.EndDate ?? date;
                    break;
                case CensusDateType.LearnerIncentive:
                    break;
            }

            return date;
        }

        public void ValidateInitialResult(RawEarning earning, List<string> errors, CensusDateType censusDateType,
            List<Commitment> commitments, DatalockValidationResultBuilder result, ImmutableHashSet<long> accountsWithNonPayableFlagSet,
            List<Commitment> allCommitments, DateTime censusDate)
        {
            if (commitments.Any(x => accountsWithNonPayableFlagSet.Contains(x.AccountId)))
            {
                errors.Add(DataLockErrorCodes.NotLevyPayer);
            }

            if (commitments.Count > 1 && censusDateType == CensusDateType.OnProgLearning)
            {
                errors.Add(DataLockErrorCodes.MultipleMatches);
            }
            else if (commitments.Count == 0)
            {
                CheckForEarlierStartDate(earning, allCommitments, censusDate, censusDateType, result);
                return;
            }

            result.Add(earning, errors, censusDateType, commitments.First());
        }

        private void CheckForEarlierStartDate(RawEarning earning, 
            List<Commitment> commitments, 
            DateTime censusDate,
            CensusDateType censusDateType,
            DatalockValidationResultBuilder result)
        {
            var matchResult = _datalockMatcher.Match(commitments, earning, censusDate);
            result.Add(earning, matchResult.ErrorCodes, censusDateType, matchResult.Commitments.LastOrDefault());
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
            var month = MonthFromPeriod(period);
            var startOfAcademicYear = StartOfAcademicYearFromEpisodeStartDate(episodeStartDate);
            if (month < 8)
            {
                return startOfAcademicYear.Year + 1;
            }
            return startOfAcademicYear.Year;
        }

        private DateTime StartOfAcademicYearFromEpisodeStartDate(DateTime episodeStartDate)
        {
            var month = episodeStartDate.Month;
            if (month < 8)
            {
                return new DateTime(episodeStartDate.Year - 1, 8, 1);
            }
            return new DateTime(episodeStartDate.Year, 8, 1);
        }
    }
}
