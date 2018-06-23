﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FastMember;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    // ReSharper disable once InconsistentNaming
    public class IShouldBeInTheDatalockComponent : IIShouldBeInTheDataLockComponent
    {
        // ASSUMPTIONS from Looking at the live data.
        //  Datalocks are 'keyed' by UKPRN, LearnRefNumber, PriceEpisodeIdentifier and CommitmentId
        //  Where there are multiple commitmentids, one is payable and the rest are not
        //  Per unique key above, payable are either all true or all false

        // BUSINESS RULES:
        //  If a learner has any passed datalocks, then pay all M/E
        //  If a learner is ACT2 only, pay everything
        //  If a learner moves from ACT2 to ACT1 and has a 'bad' datalock, then ignore them
        //  If a learner has a 'bad' datalock, ignore them for payments and refunds (includes the above)


        // INPUT
        List<RawEarning> RawEarnings { get; set; }
        List<RawEarningForMathsOrEnglish> RawEarningsMathsOrEnglish { get; set; }
        List<DatalockOutput> DatalockOutput { get; set; }
        
        private DateTime _firstDayOfAcademicYear;

        // INTERNAL
        private List<FundingDue> PayableEarnings { get; } = new List<FundingDue>();
        
        // OUTPUT
        private HashSet<int> PeriodsToIgnore { get; } = new HashSet<int>();
        private List<NonPayableEarningEntity> NonPayableEarnings { get; } = new List<NonPayableEarningEntity>();


        public DatalockValidationResult ValidatePriceEpisodes(
            List<DatalockOutput> datalockOutput,
            List<RawEarning> earnings,
            List<RawEarningForMathsOrEnglish> mathsAndEnglishEarnings,
            DateTime firstDayOfAcademicYear)
        {
            RawEarnings = earnings;
            RawEarningsMathsOrEnglish = mathsAndEnglishEarnings;
            DatalockOutput = datalockOutput;
            _firstDayOfAcademicYear = firstDayOfAcademicYear;

            // The reason for this method is to put each raw earning into a payable or non-payable pot

            ValidateEarnings();
            MatchMathsAndEnglishToOnProg();

            return new DatalockValidationResult(PayableEarnings, NonPayableEarnings, PeriodsToIgnore.ToList());
        }

        private void ValidateEarnings()
        {
            if (RawEarnings.All(x => x.ApprenticeshipContractType == 2))
            {
                MarkNonZeroTransactionTypesAsPayable(RawEarnings);
                return;
            }

            // Look at the earnings now. We are expecting there to be at most one successful datalock per 
            //  period
            // If there are 0 successful datalocks, then ignore the period

            var earningsByPeriod = RawEarnings.ToLookup(x => x.Period);
            foreach (var period in earningsByPeriod.OrderBy(x => x.Key))
            {
                var earningsForPeriod = period.Where(x => x.TransactionType01 != 0 ||
                                                          x.TransactionType02 != 0 ||
                                                          x.TransactionType03 != 0 ||
                                                          x.TransactionType04 != 0 ||
                                                          x.TransactionType05 != 0 ||
                                                          x.TransactionType06 != 0 ||
                                                          x.TransactionType07 != 0 ||
                                                          x.TransactionType08 != 0 ||
                                                          x.TransactionType09 != 0 ||
                                                          x.TransactionType10 != 0 ||
                                                          x.TransactionType11 != 0 ||
                                                          x.TransactionType12 != 0 ||
                                                          x.TransactionType13 != 0 ||
                                                          x.TransactionType14 != 0 ||
                                                          x.TransactionType15 != 0)
                    .ToList();

                if (earningsForPeriod.All(x => x.ApprenticeshipContractType == 2))
                {
                    MarkNonZeroTransactionTypesAsPayable(earningsForPeriod);
                    continue;
                }

                var earningsForPeriodByPriceEpisode = earningsForPeriod.ToLookup(x => x.PriceEpisodeIdentifier);
                foreach (var periodEarningsForPriceEpisode in earningsForPeriodByPriceEpisode)
                {
                    var priceEpisode = periodEarningsForPriceEpisode.Key;

                    var datalocks = DatalockOutput
                        .Where(x => x.Period == period.Key &&
                                    x.PriceEpisodeIdentifier == priceEpisode)
                        .ToList();

                    if (datalocks.Count == 0)
                    {
                        MarkNonZeroTransactionTypesAsNonPayable(periodEarningsForPriceEpisode,
                            $"Could not find a matching datalock for price episode: {priceEpisode} in period: {period.Key}",
                            PaymentFailureType.CouldNotFindSuccessfulDatalock);
                        PeriodsToIgnore.Add(period.Key);
                        continue;
                    }

                    // There is more than one datalock, so go through all the transactiontypeflags
                    //  and pay each in turn
                    for (var i = 1; i < 4; i++)
                    {
                        var datalocksForFlag = datalocks.Where(x => x.TransactionTypesFlag == i).ToList();
                        if (datalocksForFlag.Count > 1)
                        {
                            MarkNonZeroTransactionTypesAsNonPayable(periodEarningsForPriceEpisode,
                                $"Multiple matching datalocks for price episode: {priceEpisode} in period: {period.Key}",
                                PaymentFailureType.MultipleMatchingSuccessfulDatalocks,
                                datalocksForFlag.First());
                            PeriodsToIgnore.Add(period.Key);
                            continue;
                        }

                        if (datalocksForFlag.Count == 1)
                        {
                            // We have 1 datalock and a commitment
                            MarkNonZeroTransactionTypesAsPayable(periodEarningsForPriceEpisode, datalocksForFlag.Single(), i);
                        }
                    }
                }
            }
        }

        private DateTime DateFromPriceEpisodeIdentifier(string priceEpisodeIdentifier)
        {
            var datePortion = priceEpisodeIdentifier.Substring(priceEpisodeIdentifier.Length - 10);
            DateTime date;
            if (DateTime.TryParseExact(datePortion, "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date;
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// Matches maths and english earnings to on-prog earnings
        ///     If there are on-prog earnings, if they are payable then pay
        ///     the maths and english earnings that match them, otherwise not
        /// </summary>
        private void MatchMathsAndEnglishToOnProg()
        {
            // 450 learners with no on-prog - 200 of which are from one provider
            //  not sure what to do...


            // Special case for learner that has completed on-prog aim in one academic year 
            //  and continues maths/english aims onto next year
            // 1. If there are raw earnings for period 1 *only* and those would have 
            //      passed datalock, then pay maths/english earnings
            if (NonPayableEarnings.Count == 0 && PayableEarnings.Count == 0)
            {
                if (RawEarningsMathsOrEnglish.All(x => x.ApprenticeshipContractType == 2))
                {
                    var firstDayOfNextAcademicYear = _firstDayOfAcademicYear.AddYears(1);
                    foreach (var rawEarningForMathsOrEnglish in RawEarningsMathsOrEnglish)
                    {
                        var matchingEarning = RawEarnings.FirstOrDefault(x =>
                            x.StandardCode == rawEarningForMathsOrEnglish.StandardCode &&
                            x.ProgrammeType == rawEarningForMathsOrEnglish.ProgrammeType &&
                            x.PathwayCode == rawEarningForMathsOrEnglish.PathwayCode &&
                            x.FrameworkCode == rawEarningForMathsOrEnglish.FrameworkCode &&
                            x.ApprenticeshipContractType == rawEarningForMathsOrEnglish.ApprenticeshipContractType &&
                            DateFromPriceEpisodeIdentifier(x.PriceEpisodeIdentifier) < firstDayOfNextAcademicYear &&
                            DateFromPriceEpisodeIdentifier(x.PriceEpisodeIdentifier) >= _firstDayOfAcademicYear
                            );
                        if (matchingEarning != null)
                        {
                            rawEarningForMathsOrEnglish.PriceEpisodeIdentifier = matchingEarning.PriceEpisodeIdentifier;
                            MarkNonZeroTransactionTypesAsPayable(
                                new List<RawEarningForMathsOrEnglish> { rawEarningForMathsOrEnglish });
                        }
                    }

                    return;
                }

                foreach (var rawEarning in RawEarnings)
                {
                    // Do we have a datalock??
                    var datalock = DatalockOutput
                        .FirstOrDefault(x =>
                        x.PriceEpisodeIdentifier == rawEarning.PriceEpisodeIdentifier);
                    if (datalock != null)
                    {
                        var matchingMathsAndEnglish = RawEarningsMathsOrEnglish
                            .Where(x => x.StandardCode == rawEarning.StandardCode &&
                                        x.ProgrammeType == rawEarning.ProgrammeType &&
                                        x.FrameworkCode == rawEarning.FrameworkCode &&
                                        x.PathwayCode == rawEarning.PathwayCode &&
                                        x.ApprenticeshipContractType == rawEarning.ApprenticeshipContractType)
                            .ToList();
                        matchingMathsAndEnglish.ForEach(x => x.PriceEpisodeIdentifier = rawEarning.PriceEpisodeIdentifier);
                        MarkNonZeroTransactionTypesAsPayable(matchingMathsAndEnglish, datalock);
                    }
                }

                return;
            }

            // Find a matching payment with the same course information
            foreach (var mathsOrEnglishEarning in RawEarningsMathsOrEnglish)
            {
                var matchingOnProg = PayableEarnings.FirstOrDefault(x =>
                    x.FrameworkCode == mathsOrEnglishEarning.FrameworkCode &&
                    x.StandardCode == mathsOrEnglishEarning.StandardCode &&
                    x.PathwayCode == mathsOrEnglishEarning.PathwayCode &&
                    x.ProgrammeType == mathsOrEnglishEarning.ProgrammeType &&
                    x.ApprenticeshipContractType == mathsOrEnglishEarning.ApprenticeshipContractType &&
                    !PeriodsToIgnore.Contains(x.Period));

                if (matchingOnProg != null)
                {
                    mathsOrEnglishEarning.PriceEpisodeIdentifier = matchingOnProg.PriceEpisodeIdentifier;
                    MarkNonZeroTransactionTypesAsPayable(new List<RawEarning> { mathsOrEnglishEarning }, matchingOnProg);
                }
                else
                {
                    MarkNonZeroTransactionTypesAsNonPayable(new List<RawEarning> { mathsOrEnglishEarning },
                        "No matching payable earning found for maths/english earning",
                        PaymentFailureType.CouldNotFindMatchingOnprog);
                }
            }
        }

        private void MarkNonZeroTransactionTypesAsPayable(
            IEnumerable<RawEarning> earnings,
            IHoldCommitmentInformation commitment = null,
            int datalockType = -1)
        {
            foreach (var rawEarning in earnings)
            {
                if (rawEarning.ApprenticeshipContractType == 1 &&
                    rawEarning.SfaContributionPercentage < 1)
                {
                    rawEarning.UseLevyBalance = true;
                }

                AddFundingDue(rawEarning, commitment, datalockType);
            }
        }

        private void MarkNonZeroTransactionTypesAsNonPayable(
            IEnumerable<RawEarning> earnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitment = null)
        {
            foreach (var rawEarning in earnings)
            {
                AddNonpayableFundingDue(rawEarning, reason, paymentFailureReason, commitment);
            }
        }

        private static bool IgnoreTransactionType(int datalockType, int transactionType)
        {
            if (datalockType == 2 && (transactionType != 4 && transactionType != 5))
            {
                return true;
            }

            if (datalockType == 3 && (transactionType != 6 && transactionType != 7))
            {
                return true;
            }

            if (datalockType == 1 && (transactionType == 4 ||
                                      transactionType == 5 ||
                                      transactionType == 6 ||
                                      transactionType == 7))
            {
                return true;
            }

            return false;
        }

        private static readonly TypeAccessor FundingDueAccessor = TypeAccessor.Create(typeof(RawEarning));
        private void AddFundingDue(
            RawEarning rawEarnings,
            IHoldCommitmentInformation commitmentInformation = null,
            int datalockType = -1)
        {
            for (var i = 1; i <= 15; i++)
            {
                if (datalockType != -1 && IgnoreTransactionType(datalockType, i))
                {
                    continue;
                }

                var propertyName = $"TransactionType{i:D2}";
                var amountDue = (decimal)FundingDueAccessor[rawEarnings, propertyName];
                if (amountDue == 0)
                {
                    continue;
                }
                var fundingDue = new FundingDue(rawEarnings);
                fundingDue.TransactionType = i;

                // Doing this to prevent a huge switch statement
                fundingDue.AmountDue = amountDue;
                commitmentInformation?.TransferCommitmentInformationTo(fundingDue);
                PayableEarnings.Add(fundingDue);
            }
        }

        private void AddNonpayableFundingDue(RawEarning rawEarnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitmentInformation = null)
        {
            for (var i = 1; i <= 15; i++)
            {
                var amountDue = (decimal)FundingDueAccessor[rawEarnings, $"TransactionType{i:D2}"];
                if (amountDue == 0)
                {
                    continue;
                }

                var nonPayableEarning = new NonPayableEarningEntity(rawEarnings);
                nonPayableEarning.TransactionType = i;

                // Doing this to prevent a huge switch statement
                nonPayableEarning.AmountDue = amountDue;
                commitmentInformation?.TransferCommitmentInformationTo(nonPayableEarning);

                nonPayableEarning.PaymentFailureMessage = reason;
                nonPayableEarning.PaymentFailureReason = paymentFailureReason;
                NonPayableEarnings.Add(nonPayableEarning);
            }
        }
    }
}
