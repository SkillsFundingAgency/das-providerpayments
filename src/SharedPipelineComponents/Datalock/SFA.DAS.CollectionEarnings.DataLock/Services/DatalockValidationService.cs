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

                var allCommitments = providerCommitments.CommitmentsForLearner(uln);
                var earnings = earningsByLearner.EarningsForLearner(uln);

                foreach (var earning in earnings)
                {
                    if (earning.HasNonIncentiveEarnings())
                    {
                        var onProgCensusDate = CalculateOnProgCensusDate(earning);
                        var commitments = allCommitments.ActiveCommitmentsForDate(onProgCensusDate).ToList();
                        var matchResult = _datalockMatcher.Match(commitments, earning);
                        result.AddResult(earning, matchResult.ErrorCodes, TransactionTypesFlag.AllLearning, commitments);
                    }

                    if (earning.HasFirstIncentive())
                    {
                        var commitmentsForFirstIncentive = allCommitments.ActiveCommitmentsForDate(earning.FirstIncentiveCensusDate.Value).ToList();
                        var matchResult = _datalockMatcher.Match(commitmentsForFirstIncentive, earning);
                        result.AddResult(earning, matchResult.ErrorCodes, TransactionTypesFlag.FirstEmployerProviderIncentives, commitmentsForFirstIncentive);
                    }

                    if (earning.HasSecondIncentive())
                    {
                        var commitmentsForSecondIncentive = allCommitments.ActiveCommitmentsForDate(earning.SecondIncentiveCensusDate.Value).ToList();
                        var matchResult = _datalockMatcher.Match(commitmentsForSecondIncentive, earning);
                        result.AddResult(earning, matchResult.ErrorCodes, TransactionTypesFlag.SecondEmployerProviderIncentives, commitmentsForSecondIncentive);
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

    public class DatalockValidationResult
    {
        public List<DatalockValidationError> ValidationErrors { get; } = new List<DatalockValidationError>();
        public List<PriceEpisodePeriodMatchEntity> PriceEpisodePeriodMatches { get; } = new List<PriceEpisodePeriodMatchEntity>();
        public List<PriceEpisodeMatchEntity> PriceEpisodeMatches { get; } = new List<PriceEpisodeMatchEntity>();
        public List<DatalockOutputEntity> DatalockOutputEntities { get; } = new List<DatalockOutputEntity>();
        private readonly ImmutableHashSet<long> _accountsWithNonPayableFlagSet;

        public DatalockValidationResult(ImmutableHashSet<long> accountsWithNonPayableFlagSet)
        {
            _accountsWithNonPayableFlagSet = accountsWithNonPayableFlagSet;
        }
        
        public void AddResult(RawEarning earning, List<string> errors, TransactionTypesFlag paymentType,
            List<CommitmentEntity> commitments)
        {
            if (commitments.Any(x => _accountsWithNonPayableFlagSet.Contains(x.AccountId)))
            {
                errors.Add(DataLockErrorCodes.NotLevyPayer);
            }

            if (commitments.Count > 1 && paymentType == TransactionTypesFlag.AllLearning)
            {
                errors.Add(DataLockErrorCodes.MultipleMatches);
            }
            else if (commitments.Count == 0)
            {
                return;
            }

            AddResult(earning, errors, paymentType, commitments.First());
        }

        public void AddResult(RawEarning earning, List<string> errors, TransactionTypesFlag paymentType, CommitmentEntity commitment)
        {
            PriceEpisodeMatches.Add(new PriceEpisodeMatchEntity
            {
                AimSeqNumber = earning.AimSeqNumber,
                CommitmentId = commitment.CommitmentId,
                IsSuccess = true,
                LearnRefNumber = earning.LearnRefNumber,
                PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier,
                Ukprn = earning.Ukprn,
            });

            var payable = false;
            if (errors.Count > 0)
            {
                foreach (var error in errors)
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
        }
    }
}
