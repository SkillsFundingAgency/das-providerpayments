using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.Tools.Extensions;
using SFA.DAS.CollectionEarnings.DataLock.Tools.Providers;
using SFA.DAS.Payments.DCFS.Extensions;
using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Domain;
using System.Diagnostics;
using SFA.DAS.CollectionEarnings.DataLock.Application.Earnings;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery
{
    public class RunDataLockValidationQueryHandler : IRequestHandler<RunDataLockValidationQueryRequest, RunDataLockValidationQueryResponse>
    {
        private readonly IMatcher _initialHandler;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RunDataLockValidationQueryHandler(IDateTimeProvider dateTimeProvider, IMatcher matcher)
        {

            _dateTimeProvider = dateTimeProvider;
            _initialHandler = matcher;
        }

        public RunDataLockValidationQueryResponse Handle(RunDataLockValidationQueryRequest message)
        {
            try
            {

                var validationErrors = new ConcurrentBag<DatalockValidationError>();
                var priceEpisodeMatches = new ConcurrentBag<PriceEpisodeMatch.PriceEpisodeMatch>();
                var priceEpisodePeriodMatches = new ConcurrentBag<PriceEpisodePeriodMatch.PriceEpisodePeriodMatch>();

                var priceEpisodes = message.PriceEpisodes.ToList();
                var partitioner = Partitioner.Create(0, priceEpisodes.Count);

                Parallel.ForEach(partitioner, range =>
                {
                    for (var x = range.Item1; x < range.Item2; x++)
                    {
                        var priceEpisode = priceEpisodes[x];

                        // Execute the matching chain
                        var matchResult = _initialHandler.Match(message.Commitments.ToList(), priceEpisode, message.DasAccounts.ToList());

                        if (matchResult.ErrorCodes.Count > 0)
                        {
                            matchResult.ErrorCodes.ForEach(
                                errorCode =>
                                        validationErrors.Add(new DatalockValidationError
                                        {
                                            Ukprn = priceEpisode.Ukprn,
                                            LearnRefNumber = priceEpisode.LearnRefNumber,
                                            AimSeqNumber = priceEpisode.AimSeqNumber,
                                            RuleId = errorCode,
                                            PriceEpisodeIdentifier = priceEpisode.PriceEpisodeIdentifier
                                        }));
                        }

                        if (matchResult.Commitments.Any())
                        {
                            var isSuccess = !matchResult.ErrorCodes.Any();
                            var distinctCommitmentIds = matchResult.Commitments.Select(c => c.CommitmentId).Distinct();

                            foreach (var commitmentId in distinctCommitmentIds)
                            {
                                priceEpisodeMatches.Add(new PriceEpisodeMatch.PriceEpisodeMatch
                                {
                                    Ukprn = priceEpisode.Ukprn,
                                    LearnerReferenceNumber = priceEpisode.LearnRefNumber,
                                    AimSequenceNumber = priceEpisode.AimSeqNumber,
                                    CommitmentId = commitmentId,
                                    PriceEpisodeIdentifier = priceEpisode.PriceEpisodeIdentifier,
                                    IsSuccess = isSuccess
                                });

                                var latestCommitmentVerion = matchResult.Commitments.Where(y => y.CommitmentId == commitmentId)
                                                            .OrderByDescending(c => c.VersionId.Contains("-") ? long.Parse(c.VersionId.Split('-')[1]) : long.Parse(c.VersionId))
                                                            .First();

                                var commitmentVersions = GetMatchingFutureVersionsForTheLatestMatchedCommitmentVersion(message.Commitments, latestCommitmentVerion);
                                var incentiveEarnings = GetIncentiveEarningsForPriceEpisode(message.IncentiveEarnings, priceEpisode);

                                var periodMatches = GetPriceEpisodePeriodMatches(priceEpisode, commitmentVersions, incentiveEarnings);

                                priceEpisodePeriodMatches.AddRange(periodMatches);
                            }
                        }
                    }
                });

                return new RunDataLockValidationQueryResponse
                {
                    IsValid = true,
                    ValidationErrors = validationErrors.ToArray(),
                    PriceEpisodeMatches = priceEpisodeMatches.ToArray(),
                    PriceEpisodePeriodMatches = priceEpisodePeriodMatches.ToArray()
                };
            }
            catch (Exception ex)
            {
                return new RunDataLockValidationQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }

        private CommitmentEntity[] GetMatchingFutureVersionsForTheLatestMatchedCommitmentVersion(IEnumerable<CommitmentEntity> commitments, CommitmentEntity matchedCommitment)
        {
            return commitments
                .Where(
                    c =>
                        c.CommitmentId == matchedCommitment.CommitmentId &&
                        c.Ukprn == matchedCommitment.Ukprn &&
                        c.Uln == matchedCommitment.Uln &&
                        c.StartDate == matchedCommitment.StartDate &&
                        c.AgreedCost == matchedCommitment.AgreedCost &&
                        c.StandardCode == matchedCommitment.StandardCode &&
                        c.ProgrammeType == matchedCommitment.ProgrammeType &&
                        c.FrameworkCode == matchedCommitment.FrameworkCode &&
                        c.PathwayCode == matchedCommitment.PathwayCode &&
                        (c.VersionId.Contains("-") ? long.Parse(c.VersionId.Split('-')[1]) : long.Parse(c.VersionId)) >=
                        (matchedCommitment.VersionId.Contains("-") ? long.Parse(matchedCommitment.VersionId.Split('-')[1]) : long.Parse(matchedCommitment.VersionId))
                )
                .ToArray();
        }

        private List<IncentiveEarnings> GetIncentiveEarningsForPriceEpisode(IEnumerable<IncentiveEarnings> incentiveEarnings, RawEarning priceEpisode)
        {
            var earnings = incentiveEarnings.Where(x => x.LearnRefNumber == priceEpisode.LearnRefNumber && x.PriceEpisodeIdentifier == priceEpisode.PriceEpisodeIdentifier).ToList();
            return earnings;
        }

        private PriceEpisodePeriodMatch.PriceEpisodePeriodMatch[] GetPriceEpisodePeriodMatches(
                                                    RawEarning priceEpisode, 
                                                    CommitmentEntity[] commitments,
                                                    IEnumerable<IncentiveEarnings> incentiveEarnings)
        {
            var periodMatches = new List<PriceEpisodePeriodMatch.PriceEpisodePeriodMatch>();

            //var period = CalculateFirstPeriodForThePriceEpisode(priceEpisode);
            //var censusDate = CalculateFirstCensusDateForThePriceEpisode(priceEpisode);

            // TODO: ? fix this??
            //while (censusDate <= priceEpisode.EndDate && period <= 12)
            //{
            //    periodMatches.AddRange(BuildPriceEpisodePeriodMatch(priceEpisode, commitments, period, censusDate,incentiveEarnings));
            //    censusDate = censusDate.AddMonths(1).LastDayOfMonth();
            //    period++;
            //}

            //if (period <= 12 && priceEpisode.EndDate != priceEpisode.EndDate.LastDayOfMonth())
            //{
            //    periodMatches.AddRange(BuildPriceEpisodePeriodMatch(priceEpisode, commitments, period, priceEpisode.EndDate, incentiveEarnings));
            //}

            return periodMatches.ToArray();
        }

      
        // TODO: Not sure these will be relevant, so not fixing them just yet

        //private DateTime CalculateFirstCensusDateForThePriceEpisode(RawEarning priceEpisode)
        //{
        //    var firstCensusDateAfterYearOfCollectionStart = _dateTimeProvider.YearOfCollectionStart.LastDayOfMonth();
        //    var firstCensusDateAfterLearningStart = priceEpisode.StartDate.LastDayOfMonth();

        //    return firstCensusDateAfterYearOfCollectionStart < firstCensusDateAfterLearningStart
        //        ? firstCensusDateAfterLearningStart
        //        : firstCensusDateAfterYearOfCollectionStart;
        //}

        //private int CalculateFirstPeriodForThePriceEpisode(RawEarning priceEpisode)
        //{
        //    var firstDayAfterYearOfCollectionStart = _dateTimeProvider.YearOfCollectionStart.FirstDayOfMonth();

        //    var period = priceEpisode.StartDate.Month - firstDayAfterYearOfCollectionStart.Month
        //                 + 12 * (priceEpisode.StartDate.Year - firstDayAfterYearOfCollectionStart.Year)
        //                 + 1;

        //    return period <= 0
        //        ? 1
        //        : period;
        //}

        private CommitmentEntity GetMatchingCommitment(DateTime date, CommitmentEntity[] commitments)
        {
            var matchingCommitment = commitments
                    .Where(
                        c =>
                            c.EffectiveFrom <= date
                            && (!c.EffectiveTo.HasValue || c.EffectiveTo >= date))
                    .OrderByDescending(c => c.VersionId.Contains("-") ? long.Parse(c.VersionId.Split('-')[1]) : long.Parse(c.VersionId))
                    .FirstOrDefault();

            if (matchingCommitment == null
                && commitments.Any(c => c.EffectiveFrom > date)
                && commitments.Any(c => c.EffectiveTo < date))
            {
                matchingCommitment = commitments
                    .Where(
                        c =>
                            c.EffectiveTo.HasValue && c.EffectiveTo < date)
                    .OrderByDescending(c => c.VersionId.Contains("-") ? long.Parse(c.VersionId.Split('-')[1]) : long.Parse(c.VersionId))
                    .First();
            }

            return matchingCommitment;
        }

        private PriceEpisodePeriodMatch.PriceEpisodePeriodMatch[] BuildPriceEpisodePeriodMatch(
                                                        RawEarning priceEpisode,
                                                        CommitmentEntity[] commitments, 
                                                        int period, 
                                                        DateTime periodDate,
                                                        IEnumerable<IncentiveEarnings> incentiveEarnings)
        {
            var priceEpisodePeriodMacthes = new List<PriceEpisodePeriodMatch.PriceEpisodePeriodMatch>();

            var matchingCommitment = GetMatchingCommitment(periodDate, commitments);
            if (matchingCommitment != null)
            {
                var allEarningsPayable = matchingCommitment.PaymentStatus == (int)PaymentStatus.Active || matchingCommitment.PaymentStatus == (int)PaymentStatus.Completed;
                priceEpisodePeriodMacthes.Add(GetPriceEpisodePeriodMatch(priceEpisode, matchingCommitment, period, TransactionTypesFlag.AllLearning, allEarningsPayable));
            }


            if (priceEpisode.FirstIncentiveCensusDate.HasValue && incentiveEarnings.Any(x=> x.Period == period && x.First16To18EmployerIncentive !=0))
            {
                matchingCommitment = GetMatchingCommitment(priceEpisode.FirstIncentiveCensusDate.Value, commitments);
                if (matchingCommitment != null)
                {
                    priceEpisodePeriodMacthes.Add(GetPriceEpisodePeriodMatchFor16To18Payments(priceEpisode, matchingCommitment, period, periodDate,TransactionTypesFlag.FirstEmployerProviderIncentives));
                }
            }

            if (priceEpisode.SecondIncentiveCensusDate.HasValue && incentiveEarnings.Any(x => x.Period == period && x.Second16To18EmployerIncentive != 0))
            {
                matchingCommitment = GetMatchingCommitment(priceEpisode.SecondIncentiveCensusDate.Value, commitments);
                if (matchingCommitment != null)
                {
                    priceEpisodePeriodMacthes.Add(GetPriceEpisodePeriodMatchFor16To18Payments(priceEpisode, matchingCommitment, period, periodDate,TransactionTypesFlag.SecondEmployerProviderIncentives));
                }
            }

            return priceEpisodePeriodMacthes.ToArray();

        }

      

        private PriceEpisodePeriodMatch.PriceEpisodePeriodMatch GetPriceEpisodePeriodMatchFor16To18Payments(
                                            RawEarning priceEpisode,
                                            CommitmentEntity commitment,
                                            int period, 
                                            DateTime periodDate,
                                            TransactionTypesFlag transactionTypesFlag)
        {

            var isPayable = commitment.PaymentStatus == (int)PaymentStatus.Active || commitment.PaymentStatus == (int)PaymentStatus.Completed;
            var threshholdDate = transactionTypesFlag == TransactionTypesFlag.FirstEmployerProviderIncentives ? priceEpisode.FirstIncentiveCensusDate : priceEpisode.SecondIncentiveCensusDate;

            var incentivePayable = IsIncentivePayable(threshholdDate, commitment, periodDate);
            return GetPriceEpisodePeriodMatch(priceEpisode, commitment, period, transactionTypesFlag, isPayable && incentivePayable);

        }

      


        private bool IsIncentivePayable(DateTime? threshholdDate, CommitmentEntity commitment, DateTime censusDate)
        {
            if (!threshholdDate.HasValue)
                return false;


            if (threshholdDate.Value >= commitment.EffectiveFrom
                && (!commitment.EffectiveTo.HasValue || threshholdDate < commitment.EffectiveTo))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private PriceEpisodePeriodMatch.PriceEpisodePeriodMatch GetPriceEpisodePeriodMatch(RawEarning priceEpisode, CommitmentEntity commitment, int period, TransactionTypesFlag transactionTypesFlag, bool payable)
        {
            return new PriceEpisodePeriodMatch.PriceEpisodePeriodMatch
            {
                Ukprn = priceEpisode.Ukprn,
                PriceEpisodeIdentifier = priceEpisode.PriceEpisodeIdentifier,
                LearnerReferenceNumber = priceEpisode.LearnRefNumber,
                AimSequenceNumber = priceEpisode.AimSeqNumber,
                CommitmentId = commitment.CommitmentId,
                CommitmentVersionId = commitment.VersionId,
                Period = period,
                Payable = payable,
                TransactionTypesFlag = transactionTypesFlag
                
            };
        }
    }
}