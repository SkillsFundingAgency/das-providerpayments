using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.CollectionEarnings.DataLock.Domain.Extensions;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Services
{
    public interface IValidateDatalocks
    {
        DatalockValidationResult ValidateDatalockForProvider(
            ProviderCommitments providerCommitments, 
            List<RawEarning> providerEarnings, 
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
            List<RawEarning> providerEarnings,
            ImmutableHashSet<long> accountsWithNonPayableFlagSet)
        {
            var earningsByLearner = new ProviderEarnings(providerEarnings);
            var processedLearners = new HashSet<long>();
            
            var result = new DatalockValidationResult(accountsWithNonPayableFlagSet);

            foreach (var uln in providerCommitments.AllUlns())
            {
                processedLearners.Add(uln);

                var learnerCommitments = providerCommitments.CommitmentsForLearner(uln);
                var earnings = earningsByLearner.EarningsForLearner(uln);

                foreach (var earning in earnings)
                {
                    if (earning.HasNonIncentiveEarnings())
                    {
                        var onProgCensusDate = CalculateOnProgCensusDate(earning);
                        var commitments = learnerCommitments.ActiveCommitmentsForDate(onProgCensusDate).ToList();
                        var matchResult = _datalockMatcher.Match(commitments, earning, onProgCensusDate);
                        ValidateInitialResult(earning, matchResult.ErrorCodes, TransactionTypesFlag.AllLearning,
                            commitments, result, accountsWithNonPayableFlagSet, learnerCommitments.Commitments, onProgCensusDate);
                    }

                    if (earning.HasFirstIncentive())
                    {
                        // Checked for null above
                        // ReSharper disable once PossibleInvalidOperationException
                        var commitmentsForFirstIncentive = learnerCommitments.ActiveCommitmentsForDate(earning.FirstIncentiveCensusDate.Value).ToList();
                        var matchResult = _datalockMatcher.Match(commitmentsForFirstIncentive, earning, earning.FirstIncentiveCensusDate.Value);
                        ValidateInitialResult(earning, matchResult.ErrorCodes,
                            TransactionTypesFlag.FirstEmployerProviderIncentives, commitmentsForFirstIncentive, result,
                            accountsWithNonPayableFlagSet, learnerCommitments.Commitments, earning.FirstIncentiveCensusDate.Value);
                    }

                    if (earning.HasSecondIncentive())
                    {
                        // Checked for null above
                        // ReSharper disable once PossibleInvalidOperationException
                        var commitmentsForSecondIncentive = learnerCommitments.ActiveCommitmentsForDate(earning.SecondIncentiveCensusDate.Value).ToList();
                        var matchResult = _datalockMatcher.Match(commitmentsForSecondIncentive, earning, earning.SecondIncentiveCensusDate.Value);
                        ValidateInitialResult(earning, matchResult.ErrorCodes,
                            TransactionTypesFlag.SecondEmployerProviderIncentives, commitmentsForSecondIncentive,
                            result, accountsWithNonPayableFlagSet, learnerCommitments.Commitments, earning.SecondIncentiveCensusDate.Value);
                    }
                }
            }

            return result;
        }

        public void ValidateInitialResult(RawEarning earning, List<string> errors, TransactionTypesFlag paymentType,
            List<Commitment> commitments, DatalockValidationResult result, ImmutableHashSet<long> accountsWithNonPayableFlagSet,
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

            result.AddDistinctRecords(earning, errors, paymentType, commitments.First());
        }

        private void CheckForEarlierStartDate(RawEarning earning, List<Commitment> commitments, DateTime censusDate, TransactionTypesFlag paymentType, DatalockValidationResult result)
        {
            var matchResult = _datalockMatcher.Match(commitments, earning, censusDate);
            if (!matchResult.ErrorCodes.Any() && matchResult.Commitments.Any(x => x.Ukprn == earning.Ukprn))
            {
                if (commitments.Any(x => x.PausedOnDate.HasValue || x.PaymentStatus == 2))
                {
                    result.AddDistinctRecords(earning, new List<string> { DataLockErrorCodes.EmployerPaused }, paymentType,
                        matchResult.Commitments.First());
                }
            }
            else 
            {
                result.AddDistinctRecords(earning, matchResult.ErrorCodes, paymentType, matchResult.Commitments.First());
            }
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

    public class DatalockValidationResult
    {
        public List<DatalockValidationError> ValidationErrors { get; } = new List<DatalockValidationError>();
        public List<DatalockValidationErrorByPeriod> ValidationErrorsByPeriod { get; } = new List<DatalockValidationErrorByPeriod>();
        public List<PriceEpisodePeriodMatchEntity> PriceEpisodePeriodMatches { get; } = new List<PriceEpisodePeriodMatchEntity>();
        public List<PriceEpisodeMatchEntity> PriceEpisodeMatches { get; } = new List<PriceEpisodeMatchEntity>();
        public List<DatalockOutputEntity> DatalockOutputEntities { get; } = new List<DatalockOutputEntity>();
        private readonly ImmutableHashSet<long> _accountsWithNonPayableFlagSet;

        public DatalockValidationResult(ImmutableHashSet<long> accountsWithNonPayableFlagSet)
        {
            _accountsWithNonPayableFlagSet = accountsWithNonPayableFlagSet;
        }
        
        public void AddDistinctRecords(RawEarning earning, List<string> errors, TransactionTypesFlag paymentType, CommitmentEntity commitment)
        {
            var payable = false;
            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    if (ValidationErrors.FirstOrDefault(x =>
                            x.LearnRefNumber == earning.LearnRefNumber &&
                            x.AimSeqNumber == earning.AimSeqNumber &&
                            x.PriceEpisodeIdentifier == earning.PriceEpisodeIdentifier &&
                            x.RuleId == error &&
                            x.Ukprn == earning.Ukprn) == null &&
                        PriceEpisodePeriodMatches.FirstOrDefault(x => 
                            x.AimSeqNumber == earning.AimSeqNumber &&
                            x.LearnRefNumber == earning.LearnRefNumber &&
                            x.PriceEpisodeIdentifier == earning.PriceEpisodeIdentifier &&
                            x.Ukprn == earning.Ukprn && 
                            x.CommitmentId == commitment.CommitmentId &&
                            x.Period == earning.Period &&
                            x.TransactionTypesFlag == paymentType &&
                            x.Payable &&
                            x.VersionId == commitment.VersionId) == null)
                    {
                        ValidationErrors.Add(new DatalockValidationError
                        {
                            LearnRefNumber = earning.LearnRefNumber,
                            AimSeqNumber = earning.AimSeqNumber,
                            PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier,
                            RuleId = error,
                            Ukprn = earning.Ukprn,
                        });
                    }

                    if (ValidationErrorsByPeriod.FirstOrDefault(x =>
                            x.LearnRefNumber == earning.LearnRefNumber &&
                            x.AimSeqNumber == earning.AimSeqNumber &&
                            x.PriceEpisodeIdentifier == earning.PriceEpisodeIdentifier &&
                            x.RuleId == error &&
                            x.Period == earning.Period &&
                            x.Ukprn == earning.Ukprn) == null)
                    {
                        ValidationErrorsByPeriod.Add(new DatalockValidationErrorByPeriod()
                        {
                            LearnRefNumber = earning.LearnRefNumber,
                            AimSeqNumber = earning.AimSeqNumber,
                            PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier,
                            RuleId = error,
                            Ukprn = earning.Ukprn,
                            Period = earning.Period,
                        });
                    }
                }
            }
            else
            {
                payable = true;
            }

            PriceEpisodePeriodMatches.Add(new PriceEpisodePeriodMatchEntity
            {
                AimSeqNumber = earning.AimSeqNumber,
                CommitmentId = commitment.CommitmentId,
                LearnRefNumber = earning.LearnRefNumber,
                PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier,
                Period = earning.Period,
                TransactionTypesFlag = paymentType,
                Payable = payable,
                Ukprn = earning.Ukprn,
                VersionId = commitment.VersionId,
            });

            if (payable)
            {
                var validationErrorsToRemove = ValidationErrors.Where(x =>
                        x.Ukprn == earning.Ukprn &&
                        x.LearnRefNumber == earning.LearnRefNumber &&
                        x.PriceEpisodeIdentifier == earning.PriceEpisodeIdentifier)
                    .ToList();

                foreach (var validationError in validationErrorsToRemove)
                {
                    ValidationErrors.Remove(validationError);
                }
            }

            if (PriceEpisodeMatches.FirstOrDefault(x =>
                    x.IsSuccess == payable &&
                    x.CommitmentId == commitment.CommitmentId &&
                    x.LearnRefNumber == earning.LearnRefNumber &&
                    x.PriceEpisodeIdentifier == earning.PriceEpisodeIdentifier &&
                    x.Ukprn == earning.Ukprn) == null)
            {
                PriceEpisodeMatches.Add(new PriceEpisodeMatchEntity
                {
                    CommitmentId = commitment.CommitmentId,
                    IsSuccess = payable,
                    LearnRefNumber = earning.LearnRefNumber,
                    PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier,
                    Ukprn = earning.Ukprn,
                });
            }
        }
    }
}
