using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.CollectionEarnings.DataLock.Domain.Extensions;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Common.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

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
                    if (earning.HasNonIncentiveEarnings())
                    {
                        ProcessEarning(accountsWithNonPayableFlagSet, earning, 
                            learnerCommitments, resultBuilder, TransactionTypesFlag.AllLearning);
                    }

                    if (earning.HasCompletionPayment())
                    {
                        ProcessEarning(accountsWithNonPayableFlagSet, earning,
                            learnerCommitments, resultBuilder, TransactionTypesFlag.Completion);
                    }

                    if (earning.HasFirstIncentive())
                    {
                        ProcessEarning(accountsWithNonPayableFlagSet, earning,
                            learnerCommitments, resultBuilder, TransactionTypesFlag.FirstEmployerProviderIncentives);
                    }

                    if (earning.HasSecondIncentive())
                    {
                        ProcessEarning(accountsWithNonPayableFlagSet, earning,
                            learnerCommitments, resultBuilder, TransactionTypesFlag.SecondEmployerProviderIncentives);
                    }
                }
            }

            var result = resultBuilder.Build();
            return result;
        }

        private void ProcessEarning(ImmutableHashSet<long> accountsWithNonPayableFlagSet, RawEarning earning,
            LearnerCommitments learnerCommitments, DatalockValidationResultBuilder result, TransactionTypesFlag earningType)
        {
            var censusDate = CalculateCensusDate(earning, earningType);
            var commitments = learnerCommitments.ActiveCommitmentsForDate(censusDate).ToList();
            var matchResult = _datalockMatcher.Match(commitments, earning, censusDate);
            ValidateInitialResult(earning, matchResult.ErrorCodes, earningType,
                commitments, result, accountsWithNonPayableFlagSet, learnerCommitments.Commitments, censusDate);
        }

        private DateTime CalculateCensusDate(RawEarning earning, TransactionTypesFlag earningType)
        {
            var date = new DateTime(1900, 01, 01);
            switch (earningType)
            {
                case TransactionTypesFlag.AllLearning:
                    date = CalculateOnProgCensusDate(earning);
                    break;
                case TransactionTypesFlag.FirstEmployerProviderIncentives:
                    date = earning.FirstIncentiveCensusDate ?? date;
                    break;
                case TransactionTypesFlag.SecondEmployerProviderIncentives:
                    date = earning.SecondIncentiveCensusDate ?? date;
                    break;
                case TransactionTypesFlag.Completion:
                    date = earning.EndDate ?? date;
                    break;
            }

            return date;
        }

        public void ValidateInitialResult(RawEarning earning, List<string> errors, TransactionTypesFlag paymentType,
            List<Commitment> commitments, DatalockValidationResultBuilder result, ImmutableHashSet<long> accountsWithNonPayableFlagSet,
            List<Commitment> allCommitments, DateTime censusDate)
        {
            if (commitments.Any(x => accountsWithNonPayableFlagSet.Contains(x.AccountId)))
            {
                errors.Add(DataLockErrorCodes.NotLevyPayer);
            }

            if (commitments.Count > 1 && paymentType == TransactionTypesFlag.AllLearning)
            {
                errors.Add(DataLockErrorCodes.MultipleMatches);
            }
            else if (commitments.Count == 0)
            {
                CheckForEarlierStartDate(earning, allCommitments, censusDate, paymentType, result);
                return;
            }

            result.Add(earning, errors, paymentType, commitments.First());
        }

        private void CheckForEarlierStartDate(RawEarning earning, 
            List<Commitment> commitments, 
            DateTime censusDate, 
            TransactionTypesFlag paymentType,
            DatalockValidationResultBuilder result)
        {
            var matchResult = _datalockMatcher.Match(commitments, earning, censusDate);
            result.Add(earning, matchResult.ErrorCodes, paymentType, matchResult.Commitments.LastOrDefault());
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
