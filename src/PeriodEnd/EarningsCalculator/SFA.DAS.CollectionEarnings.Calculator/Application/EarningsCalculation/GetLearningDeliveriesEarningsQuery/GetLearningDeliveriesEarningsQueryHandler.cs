using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess;
using SFA.DAS.CollectionEarnings.Calculator.Tools.Extensions;
using SFA.DAS.CollectionEarnings.Calculator.Tools.Providers;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery
{
    public class GetLearningDeliveriesEarningsQueryHandler : IRequestHandler<GetLearningDeliveriesEarningsQueryRequest, GetLearningDeliveriesEarningsQueryResponse>
    {
        private const decimal CompletionPaymentRatio = 0.2m;
        private const decimal InLearningPaymentRatio = 1.00m - CompletionPaymentRatio;

        private readonly IDateTimeProvider _dateTimeProvider;

        private List<ApprenticeshipPriceEpisode.ApprenticeshipPriceEpisode> _apprenticeshipPriceEpisodes;
        private List<ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues> _apprenticeshipPriceEpisodePeriodisedValueses;
        private List<ApprenticeshipPriceEpisodePeriod.ApprenticeshipPriceEpisodePeriod> _apprenticeshipPriceEpisodePeriodEarnings;

        public GetLearningDeliveriesEarningsQueryHandler(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public GetLearningDeliveriesEarningsQueryResponse Handle(GetLearningDeliveriesEarningsQueryRequest message)
        {
            _apprenticeshipPriceEpisodes = new List<ApprenticeshipPriceEpisode.ApprenticeshipPriceEpisode>();
            _apprenticeshipPriceEpisodePeriodisedValueses = new List<ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues>();
            _apprenticeshipPriceEpisodePeriodEarnings = new List<ApprenticeshipPriceEpisodePeriod.ApprenticeshipPriceEpisodePeriod>();

            try
            {
                var learningDeliveries = message.LearningDeliveries.ToList();

                foreach (var learningDelivery in learningDeliveries)
                {
                    if (learningDelivery.PriceEpisodes == null)
                    {
                        continue;
                    }



                    foreach (var priceEpisode in learningDelivery.PriceEpisodes)
                    {
                        var completionAmount = CalculateCompletionPayment(priceEpisode);
                        var monthlyAmount = CalculateMonthlyInstallment(learningDelivery, priceEpisode, message.LearningDeliveries);

                        var apprenticeshipPriceEpisode = GetApprenticeshipPriceEpisode(learningDelivery, priceEpisode, monthlyAmount, completionAmount);

                        _apprenticeshipPriceEpisodes.Add(apprenticeshipPriceEpisode);

                        BuildPriceEpisodePeriodEarningsAndPeriodisedValues(learningDelivery, apprenticeshipPriceEpisode, monthlyAmount, completionAmount);
                    }
                }

                return new GetLearningDeliveriesEarningsQueryResponse
                {
                    IsValid = true,

                    PriceEpisodes = _apprenticeshipPriceEpisodes.ToArray(),
                    PriceEpisodesPeriodisedValues = _apprenticeshipPriceEpisodePeriodisedValueses.ToArray(),
                    PriceEpisodesPeriodsEarnings = _apprenticeshipPriceEpisodePeriodEarnings.ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetLearningDeliveriesEarningsQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }

        private int CalculateNumberOfPeriods(DateTime startDate, DateTime endDate)
        {
            var result = 0;
            
            var censusDate = startDate.LastDayOfMonth();

            while (censusDate <= endDate)
            {
                censusDate = censusDate.AddMonths(1).LastDayOfMonth();
                result++;
            }

            return result <= 0
                ? 1
                : result;
        }

        private decimal CalculateCompletionPayment(PriceEpisode priceEpisode)
        {
            return decimal.Round(priceEpisode.NegotiatedPrice * CompletionPaymentRatio, 5);
        }

        private decimal CalculateMonthlyInstallment(LearningDelivery learningDelivery, PriceEpisode priceEpisode, LearningDelivery[] learningDeliveries)
        {
            decimal prevEarnedAmount = 0m;

            //accumulate all the previous earnings for same learner for same programme
            foreach (var x in learningDeliveries)
            {
                if (x.LearnerReferenceNumber == learningDelivery.LearnerReferenceNumber &&
                                                    x.FrameworkCode == learningDelivery.FrameworkCode &&
                                                    x.StandardCode == learningDelivery.StandardCode &&
                                                    x.PathwayCode == learningDelivery.PathwayCode &&
                                                    x.ProgrammeType == learningDelivery.ProgrammeType &&
                                                    x.Uln == learningDelivery.Uln &&
                                                    x.Ukprn == learningDelivery.Ukprn &&
                                                    x.CompletionStatus == 6 &&
                                                    learningDelivery.AimSequenceNumber > x.AimSequenceNumber
                                                )
                    foreach (var pe in x.PriceEpisodes)
                    {

                        prevEarnedAmount += _apprenticeshipPriceEpisodePeriodEarnings.Where(y => y.PriceEpisodeId == pe.Id).Sum(z=> z.PriceEpisodeOnProgPayment);

                    }
            }


          
            var totalAvailableAmount = (priceEpisode.NegotiatedPrice * InLearningPaymentRatio) - prevEarnedAmount;

            if (priceEpisode.StartDate == learningDelivery.LearningStartDate)
            {
                return decimal.Round(totalAvailableAmount / CalculateNumberOfPeriods(learningDelivery.LearningStartDate, learningDelivery.LearningPlannedEndDate), 5);
            }

            return decimal.Round(totalAvailableAmount / CalculateNumberOfPeriods(priceEpisode.StartDate, learningDelivery.LearningPlannedEndDate), 5);
        }


        private DateTime CalculateFirstCensusDateForThePriceEpisode(ApprenticeshipPriceEpisode.ApprenticeshipPriceEpisode priceEpisode)
        {
            var firstCensusDateAfterYearOfCollectionStart = _dateTimeProvider.YearOfCollectionStart.LastDayOfMonth();
            var firstCensusDateAfterLearningStart = priceEpisode.StartDate.LastDayOfMonth();

            return firstCensusDateAfterYearOfCollectionStart < firstCensusDateAfterLearningStart
                ? firstCensusDateAfterLearningStart
                : firstCensusDateAfterYearOfCollectionStart;
        }

        private int CalculateFirstPeriodForThePriceEpisode(ApprenticeshipPriceEpisode.ApprenticeshipPriceEpisode priceEpisode)
        {
            var firstDayAfterYearOfCollectionStart = _dateTimeProvider.YearOfCollectionStart.FirstDayOfMonth();

            var period = priceEpisode.StartDate.Month - firstDayAfterYearOfCollectionStart.Month
                         + 12 * (priceEpisode.StartDate.Year - firstDayAfterYearOfCollectionStart.Year)
                         + 1;

            return period <= 0
                ? 1
                : period;
        }

        private ApprenticeshipPriceEpisode.ApprenticeshipPriceEpisode GetApprenticeshipPriceEpisode(LearningDelivery learningDelivery, PriceEpisode priceEpisode, decimal monthlyAmount, decimal completionAmount)
        {
            return new ApprenticeshipPriceEpisode.ApprenticeshipPriceEpisode
            {
                Id = priceEpisode.Id,
                LearnerReferenceNumber = learningDelivery.LearnerReferenceNumber,
                AimSequenceNumber = learningDelivery.AimSequenceNumber,
                StartDate = priceEpisode.StartDate,
                PlannedEndDate = priceEpisode.EndDate.HasValue ? priceEpisode.EndDate : learningDelivery.LearningPlannedEndDate,
                ActualEndDate = priceEpisode.EndDate.HasValue ? priceEpisode.EndDate : learningDelivery.LearningActualEndDate,
                NegotiatedPrice = priceEpisode.NegotiatedPrice,
                Tnp1 = priceEpisode.Tnp1,
                Tnp2 = priceEpisode.Tnp2,
                Tnp3 = priceEpisode.Tnp3,
                Tnp4 = priceEpisode.Tnp4,
                MonthlyAmount = monthlyAmount,
                CompletionAmount = completionAmount,
                Completed = priceEpisode.EndDate.HasValue || learningDelivery.LearningActualEndDate.HasValue ? true : (bool?)null,
                LastEpisode = priceEpisode.LastEpisode
            };
        }

        private void BuildPriceEpisodePeriodEarningsAndPeriodisedValues(LearningDelivery learningDelivery, ApprenticeshipPriceEpisode.ApprenticeshipPriceEpisode priceEpisode, decimal monthlyAmount, decimal completionAmount)
        {
            var periodEarnings = new List<ApprenticeshipPriceEpisodePeriod.ApprenticeshipPriceEpisodePeriod>();

            var onProgrammeEarning = new ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues
            {
                PriceEpisodeId = priceEpisode.Id,
                LearnerReferenceNumber = learningDelivery.LearnerReferenceNumber,
                AimSequenceNumber = learningDelivery.AimSequenceNumber,
                PriceEpisodeStartDate = priceEpisode.StartDate,
                AttributeName = AttributeNames.PriceEpisodeOnProgPayment
            };

            var completionEarning = new ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues
            {
                PriceEpisodeId = priceEpisode.Id,
                LearnerReferenceNumber = learningDelivery.LearnerReferenceNumber,
                AimSequenceNumber = learningDelivery.AimSequenceNumber,
                PriceEpisodeStartDate = priceEpisode.StartDate,
                AttributeName = AttributeNames.PriceEpisodeCompletionPayment
            };

            var balancingEarning = new ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues
            {
                PriceEpisodeId = priceEpisode.Id,
                LearnerReferenceNumber = learningDelivery.LearnerReferenceNumber,
                AimSequenceNumber = learningDelivery.AimSequenceNumber,
                PriceEpisodeStartDate = priceEpisode.StartDate,
                AttributeName = AttributeNames.PriceEpisodeBalancePayment
            };

            var shouldAddCompletionPayment = learningDelivery.LearningActualEndDate.HasValue
                                             && priceEpisode.LastEpisode
                                             && learningDelivery.CompletionStatus == 2;
            var shouldAddBalancingPayment = shouldAddCompletionPayment
                                            && learningDelivery.LearningActualEndDate.Value < learningDelivery.LearningPlannedEndDate
                                            && learningDelivery.CompletionStatus == 2;

            var censusDate = CalculateFirstCensusDateForThePriceEpisode(priceEpisode);
            var period = CalculateFirstPeriodForThePriceEpisode(priceEpisode);

            var plannedEndDate = priceEpisode.PlannedEndDate ?? learningDelivery.LearningPlannedEndDate;
            var learningEndCensusDate = priceEpisode.ActualEndDate?.LastDayOfMonth() ?? plannedEndDate.LastDayOfMonth();

            var onProgEarningsEndDate = priceEpisode.ActualEndDate.HasValue && priceEpisode.ActualEndDate.Value < plannedEndDate ?
                                        priceEpisode.ActualEndDate.Value :
                                        plannedEndDate;
           
            while (censusDate <= learningEndCensusDate && period <= 12)
            {
                var periodEarning = new ApprenticeshipPriceEpisodePeriod.ApprenticeshipPriceEpisodePeriod
                {
                    PriceEpisodeId = priceEpisode.Id,
                    LearnerReferenceNumber = priceEpisode.LearnerReferenceNumber,
                    AimSequenceNumber = priceEpisode.AimSequenceNumber,
                    PriceEpisodeStartDate = priceEpisode.StartDate,
                    Period = period
                };

                if ( censusDate <= onProgEarningsEndDate)
                {
                    onProgrammeEarning.SetPeriodValue(period, monthlyAmount);

                    periodEarning.PriceEpisodeOnProgPayment = monthlyAmount;
                }

                if (shouldAddCompletionPayment && censusDate == learningEndCensusDate)
                {
                    completionEarning.SetPeriodValue(period, completionAmount);

                    periodEarning.PriceEpisodeCompletionPayment = completionAmount;
                }

                if (shouldAddBalancingPayment && censusDate == learningEndCensusDate)
                {
                    var balancingPayment = CalculateBalancingPaymentAmount(monthlyAmount, completionAmount, priceEpisode);

                    balancingEarning.SetPeriodValue(period, balancingPayment);

                    periodEarning.PriceEpisodeBalancePayment = balancingPayment;
                }

                periodEarnings.Add(periodEarning);

                censusDate = censusDate.AddMonths(1).LastDayOfMonth();
                period++;
            }

            FillMissingPeriodEarnings(periodEarnings, priceEpisode);

            _apprenticeshipPriceEpisodePeriodisedValueses.Add(onProgrammeEarning);
            _apprenticeshipPriceEpisodePeriodisedValueses.Add(completionEarning);
            _apprenticeshipPriceEpisodePeriodisedValueses.Add(balancingEarning);

            _apprenticeshipPriceEpisodePeriodEarnings.AddRange(periodEarnings);
        }

        private decimal CalculateBalancingPaymentAmount(decimal monthlyInstallment, decimal completionPayment, ApprenticeshipPriceEpisode.ApprenticeshipPriceEpisode priceEpisode)
        {
            if (!priceEpisode.ActualEndDate.HasValue)
            {
                return 0.00m;
            }

            if (!priceEpisode.PlannedEndDate.HasValue)
            {
                return 0.00m;
            }

            var numberOfPeriods = CalculateNumberOfPeriods(priceEpisode.StartDate, priceEpisode.PlannedEndDate.Value);
            var numberOfPeriodsToBalance = CalculateNumberOfPeriodsToBalance(priceEpisode.PlannedEndDate.Value, priceEpisode.ActualEndDate.Value);

            var amountEarnedSoFar = monthlyInstallment * (numberOfPeriods - numberOfPeriodsToBalance);

            return decimal.Round(priceEpisode.NegotiatedPrice - completionPayment - amountEarnedSoFar, 5);
        }

        private int CalculateNumberOfPeriodsToBalance(DateTime plannedEndDate, DateTime actualEndDate)
        {
            var result = 0;


            var censusDate = actualEndDate==actualEndDate.LastDayOfMonth()
                ? actualEndDate.AddMonths(1).LastDayOfMonth() 
                : actualEndDate.LastDayOfMonth();

            while (censusDate <= plannedEndDate)
            {
                censusDate = censusDate.AddMonths(1).LastDayOfMonth();
                result++;
            }

            return result;
        }


        private void FillMissingPeriodEarnings(List<ApprenticeshipPriceEpisodePeriod.ApprenticeshipPriceEpisodePeriod> periodEarnings, ApprenticeshipPriceEpisode.ApprenticeshipPriceEpisode priceEpisode)
        {
            if (periodEarnings.Count == 12)
            {
                return;
            }

            for (var period = 1; period <= 12; period++)
            {
                if (periodEarnings.Count(pe => pe.Period == period) == 1)
                {
                    continue;
                }

                _apprenticeshipPriceEpisodePeriodEarnings.Add(
                    new ApprenticeshipPriceEpisodePeriod.ApprenticeshipPriceEpisodePeriod
                    {
                        PriceEpisodeId = priceEpisode.Id,
                        LearnerReferenceNumber = priceEpisode.LearnerReferenceNumber,
                        AimSequenceNumber = priceEpisode.AimSequenceNumber,
                        PriceEpisodeStartDate = priceEpisode.StartDate,
                        Period = period
                    });
            }
        }
    }
}