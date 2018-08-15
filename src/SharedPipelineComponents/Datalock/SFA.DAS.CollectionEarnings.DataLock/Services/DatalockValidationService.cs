﻿using System;
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
                        var matchResult = _datalockMatcher.Match(commitments, earning);
                        if (!result.AddResult(earning, matchResult.ErrorCodes, TransactionTypesFlag.AllLearning,
                            commitments))
                        {
                            CheckForEarlierStartDate(earning, learnerCommitments.AllCommitments, TransactionTypesFlag.AllLearning, result);
                        };
                    }

                    if (earning.HasFirstIncentive())
                    {
                        var commitmentsForFirstIncentive = learnerCommitments.ActiveCommitmentsForDate(earning.FirstIncentiveCensusDate.Value).ToList();
                        var matchResult = _datalockMatcher.Match(commitmentsForFirstIncentive, earning);
                        if (!result.AddResult(earning, matchResult.ErrorCodes,
                            TransactionTypesFlag.FirstEmployerProviderIncentives, commitmentsForFirstIncentive))
                        {
                            CheckForEarlierStartDate(earning, learnerCommitments.AllCommitments, TransactionTypesFlag.FirstEmployerProviderIncentives, result);
                        }
                    }

                    if (earning.HasSecondIncentive())
                    {
                        var commitmentsForSecondIncentive = learnerCommitments.ActiveCommitmentsForDate(earning.SecondIncentiveCensusDate.Value).ToList();
                        var matchResult = _datalockMatcher.Match(commitmentsForSecondIncentive, earning);
                        if(!result.AddResult(earning, matchResult.ErrorCodes, TransactionTypesFlag.SecondEmployerProviderIncentives, commitmentsForSecondIncentive))
                        {
                            CheckForEarlierStartDate(earning, learnerCommitments.AllCommitments, TransactionTypesFlag.SecondEmployerProviderIncentives, result);
                        };
                    }
                }
            }

            return result;
        }

        private void CheckForEarlierStartDate(RawEarning earning, List<CommitmentEntity> commitments, TransactionTypesFlag paymentType, DatalockValidationResult result)
        {
            var matchResult = _datalockMatcher.Match(commitments, earning);
            if (matchResult.ErrorCodes.Any() && matchResult.Commitments.Any())
            {
                result.AddResult(earning, new List<string> {DataLockErrorCodes.EarlierStartDate}, paymentType,
                    matchResult.Commitments.First());
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
            if (month >= 8)
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
        
        public bool AddResult(RawEarning earning, List<string> errors, TransactionTypesFlag paymentType,
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
                return false;
            }

            AddResult(earning, errors, paymentType, commitments.First());
            return true;
        }

        public void AddResult(RawEarning earning, List<string> errors, TransactionTypesFlag paymentType, CommitmentEntity commitment)
        {
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

            if (PriceEpisodeMatches.FirstOrDefault(x =>
                    x.AimSeqNumber == earning.AimSeqNumber &&
                    x.CommitmentId == commitment.CommitmentId &&
                    x.LearnRefNumber == earning.LearnRefNumber &&
                    x.PriceEpisodeIdentifier == earning.PriceEpisodeIdentifier &&
                    x.Ukprn == earning.Ukprn) == null)
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
            }
        }
    }
}
