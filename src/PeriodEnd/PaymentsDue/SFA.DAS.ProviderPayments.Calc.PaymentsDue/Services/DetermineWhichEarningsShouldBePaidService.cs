using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FastMember;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    // ReSharper disable once InconsistentNaming
    public class DetermineWhichEarningsShouldBePaidService : IDetermineWhichEarningsShouldBePaid
    {
        // ASSUMPTIONS from Looking at the live data.
        //  Datalocks are 'keyed' by UKPRN, LearnRefNumber, PriceEpisodeIdentifier and CommitmentId
        //  Where there are multiple commitmentids, one is payable and the rest are not
        //  Per unique key above, payable are either all true or all false

        // BUSINESS RULES:
        //  If a learner has any passed datalocks, then pay all M/E
        //  If a learner is ACT2 only, pay everything
        //  If a learner has a 'bad' datalock for a period, ignore that period (includes the above)

        private readonly ICollectionPeriodRepository _collectionPeriodRepository;

        private static readonly List<int> OnProgTransactionTypes = new List<int> { 1, 2, 3 };

        public DetermineWhichEarningsShouldBePaidService(ICollectionPeriodRepository collectionPeriodRepository)
        {
            _collectionPeriodRepository = collectionPeriodRepository;
        }

        /// <summary>
        /// Put each raw earning into a payable or non-payable pot
        /// </summary>
        public EarningValidationResult DeterminePayableEarnings(
            List<DatalockOutput> successfulDatalocks,
            List<RawEarning> earnings,
            List<RawEarningForMathsOrEnglish> mathsAndEnglishEarnings)
        {
            var academicYearDetail =  GetFirstDayOfAcademicYears();

            var rawEarnings = GetEarningsForCurrentAcademicYear(earnings, academicYearDetail);
            var datalockOutput = GetSuccessfulDatalocksForCurrentAcademicYear(successfulDatalocks, academicYearDetail);

            var result = ValidateEarnings(rawEarnings, datalockOutput);
            result += MatchMathsAndEnglishToOnProg(result, mathsAndEnglishEarnings, rawEarnings, datalockOutput);

            return result;
        }

        private List<DatalockOutput> GetSuccessfulDatalocksForCurrentAcademicYear(List<DatalockOutput> successfulDatalocks, AcademicYearDetail academicYearDetail)
        {
            return successfulDatalocks.Where(x => PriceEpisodeFallsWithinAcademicYear(x.PriceEpisodeIdentifier, academicYearDetail)).ToList();
        }

        private List<RawEarning> GetEarningsForCurrentAcademicYear(List<RawEarning> earnings, AcademicYearDetail academicYearDetail)
        {
            return earnings.Where(x => PriceEpisodeFallsWithinAcademicYear(x.PriceEpisodeIdentifier, academicYearDetail)).ToList();
        }

        private class AcademicYearDetail
        {
            internal AcademicYearDetail(DateTime firstDayOfThisAcademicYear, DateTime firstDayOfNextAcademicYear)
            {
                FirstDayOfThisAcademicYear = firstDayOfThisAcademicYear;
                FirstDayOfNextAcademicYear = firstDayOfNextAcademicYear;
            }

            internal DateTime FirstDayOfThisAcademicYear { get; }
            internal DateTime FirstDayOfNextAcademicYear { get; }
        }

        private AcademicYearDetail GetFirstDayOfAcademicYears()
        {
            var currentCollectionPeriodAcademicYear = _collectionPeriodRepository.GetCurrentCollectionPeriod()?.AcademicYear ?? "1718";
            var startingYear = int.Parse(currentCollectionPeriodAcademicYear.Substring(2)) + 2000; // will fail in 2100...
            var firstDayOfNextAcademicYear = new DateTime(startingYear, 8, 1);
            return new AcademicYearDetail(firstDayOfNextAcademicYear.AddYears(-1), firstDayOfNextAcademicYear);
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

        private bool PriceEpisodeFallsWithinAcademicYear(string priceEpisodeIdentifier, AcademicYearDetail academicYearDetail)
        {
            var priceEpisodeStartDate = DateFromPriceEpisodeIdentifier(priceEpisodeIdentifier);
            return priceEpisodeStartDate >= academicYearDetail.FirstDayOfThisAcademicYear &&
                   priceEpisodeStartDate < academicYearDetail.FirstDayOfNextAcademicYear;
        }

        private EarningValidationResult ValidateEarnings(List<RawEarning> rawEarnings, List<DatalockOutput> datalockOutput)
        {
            var result = new EarningValidationResult();

            if (rawEarnings.All(x => x.ApprenticeshipContractType == ApprenticeshipContractType.NonLevy))
            {
                result.AddPayableEarnings(GetFundingDueForNonZeroTransactionTypes(rawEarnings));
                return result;
            }

            // Look at the earnings now. We are expecting there to be at most one successful datalock per 
            //  period
            // If there are earnings and 0 successful datalocks, then ignore the period

            var earningsByPeriod = rawEarnings.ToLookup(x => x.Period);
            foreach (var periodGroup in earningsByPeriod.OrderBy(x => x.Key))
            {
                var earningsForPeriod = periodGroup
                    .Where(x => x.HasNonZeroTransactions())
                    .ToList();

                if (earningsForPeriod.All(x => x.ApprenticeshipContractType == ApprenticeshipContractType.NonLevy))
                {
                    result.AddPayableEarnings(GetFundingDueForNonZeroTransactionTypes(earningsForPeriod));
                    continue;
                }

                var periodEarningsForPriceEpisodeGroups = earningsForPeriod.ToLookup(x => x.PriceEpisodeIdentifier);
                foreach (var periodEarningsForPriceEpisode in periodEarningsForPriceEpisodeGroups)
                {
                    var priceEpisode = periodEarningsForPriceEpisode.Key;

                    var datalocks = datalockOutput
                        .Where(x => x.Period == periodGroup.Key &&
                                    x.PriceEpisodeIdentifier == priceEpisode)
                        .ToList();

                    if (datalocks.Count == 0)
                    {
                        result.AddNonPayableEarnings(GetNonPayableEarningsForNonZeroTransactionTypes(periodEarningsForPriceEpisode,
                            $"Could not find a matching datalock for price episode: {priceEpisode} in period: {periodGroup.Key}",
                            PaymentFailureType.CouldNotFindSuccessfulDatalock));
                        result.PeriodsToIgnore.Add(periodGroup.Key);
                        continue;
                    }

                    // There is more than one datalock, so go through all the transactiontypeflags
                    //  and pay each in turn
                    for (var transactionTypesFlag = 1; transactionTypesFlag < 4; transactionTypesFlag++)
                    {
                        var datalocksForFlag = datalocks
                            .Where(x => x.TransactionTypesFlag == transactionTypesFlag)
                            .ToList();
                        if (datalocksForFlag.Count > 1)
                        {
                            result.AddNonPayableEarnings(GetNonPayableEarningsForNonZeroTransactionTypes(periodEarningsForPriceEpisode,
                                $"Multiple matching datalocks for price episode: {priceEpisode} in period: {periodGroup.Key}",
                                PaymentFailureType.MultipleMatchingSuccessfulDatalocks,
                                datalocksForFlag.First()));
                            result.PeriodsToIgnore.Add(periodGroup.Key);
                            continue;
                        }

                        if (datalocksForFlag.Count == 1)
                        {
                            // We have 1 datalock and a commitment
                            result.AddPayableEarnings(GetFundingDueForNonZeroTransactionTypes(
                                periodEarningsForPriceEpisode, 
                                datalocksForFlag.Single(), 
                                transactionTypesFlag));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Matches maths and english earnings to on-prog earnings
        ///     If there are on-prog earnings, if they are payable then pay
        ///     the maths and english earnings that match them, otherwise not
        /// </summary>
        private EarningValidationResult MatchMathsAndEnglishToOnProg(
            EarningValidationResult resultSoFar,
            List<RawEarningForMathsOrEnglish> RawEarningsForMathsOrEnglish,
            List<RawEarning> rawEarnings,
            List<DatalockOutput> datalockOutput)
        {
            var result = new EarningValidationResult();
            
            // 450 learners with no on-prog - 200 of which are from one provider
            //  not sure what to do...


            // Special case for learner that has completed on-prog aim in one academic year 
            //  and continues maths/english aims onto next year
            // 1. If there are raw earnings for period 1 *only* and those would have 
            //      passed datalock, then pay maths/english earnings
            if (resultSoFar.NonPayableEarnings.Count == 0 && resultSoFar.PayableEarnings.Count == 0)
            {
                if (RawEarningsForMathsOrEnglish.All(x => x.ApprenticeshipContractType == ApprenticeshipContractType.NonLevy))
                {
                    foreach (var rawEarningForMathsOrEnglish in RawEarningsForMathsOrEnglish)
                    {
                        var matchingEarning = rawEarnings.FirstOrDefault(x =>
                            x.HasMatchingCourseInformationWith(rawEarningForMathsOrEnglish));
                        if (matchingEarning != null)
                        {
                            rawEarningForMathsOrEnglish.PriceEpisodeIdentifier = matchingEarning.PriceEpisodeIdentifier;
                            result.AddPayableEarnings(GetFundingDueForNonZeroTransactionTypes(
                                new List<RawEarningForMathsOrEnglish> { rawEarningForMathsOrEnglish }));
                        }
                        else
                        {
                            result.AddNonPayableEarnings(GetNonPayableEarningsForNonZeroTransactionTypes(
                                new List<RawEarningForMathsOrEnglish> { rawEarningForMathsOrEnglish },
                                "No on-prog earning found for maths/english earning",
                                PaymentFailureType.CouldNotFindMatchingOnprog));
                        }
                    }

                    return result;
                }

                foreach (var rawEarning in rawEarnings)
                {
                    // Do we have a datalock??
                    var datalock = datalockOutput
                        .FirstOrDefault(x =>
                        x.PriceEpisodeIdentifier == rawEarning.PriceEpisodeIdentifier);
                    if (datalock != null)
                    {
                        var matchingMathsAndEnglish = RawEarningsForMathsOrEnglish
                            .Where(x => x.HasMatchingCourseInformationWith(rawEarning))
                            .ToList();
                        matchingMathsAndEnglish.ForEach(x => x.PriceEpisodeIdentifier = rawEarning.PriceEpisodeIdentifier);
                        result.AddPayableEarnings(GetFundingDueForNonZeroTransactionTypes(matchingMathsAndEnglish, datalock));
                    }
                }

                return result;
            }

            // Find a matching payment with the same course information
            foreach (var mathsOrEnglishEarning in RawEarningsForMathsOrEnglish)
            {
                var matchingOnProg = resultSoFar.PayableEarnings.FirstOrDefault(x =>
                    x.HasMatchingCourseInformationWith(mathsOrEnglishEarning) &&
                    !resultSoFar.PeriodsToIgnore.Contains(x.Period));

                if (matchingOnProg != null)
                {
                    mathsOrEnglishEarning.PriceEpisodeIdentifier = matchingOnProg.PriceEpisodeIdentifier;
                    result.AddPayableEarnings(GetFundingDueForNonZeroTransactionTypes(new List<RawEarning> { mathsOrEnglishEarning }, matchingOnProg));
                }
                else
                {
                    result.AddNonPayableEarnings(GetNonPayableEarningsForNonZeroTransactionTypes(new List<RawEarning> { mathsOrEnglishEarning },
                        "No matching payable earning found for maths/english earning",
                        PaymentFailureType.CouldNotFindMatchingOnprog));
                }
            }

            return result;
        }

        private List<FundingDue> GetFundingDueForNonZeroTransactionTypes(
            IEnumerable<RawEarning> earnings,
            IHoldCommitmentInformation commitment = null,
            int datalockType = -1)
        {
            var payableEarnings = new List<FundingDue>();
            foreach (var rawEarning in earnings)
            {
                payableEarnings.AddRange(GetPayableEarnings(rawEarning, commitment, datalockType));
            }

            return payableEarnings;
        }

        private List<NonPayableEarning> GetNonPayableEarningsForNonZeroTransactionTypes(
            IEnumerable<RawEarning> earnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitment = null)
        {
            var nonPayableEarnings = new List<NonPayableEarning>();
            foreach (var rawEarning in earnings)
            {
                nonPayableEarnings.AddRange(GetNonPayableEarnings(rawEarning, reason, paymentFailureReason, commitment));
            }

            return nonPayableEarnings;
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

        private List<FundingDue> GetPayableEarnings(
            RawEarning rawEarnings,
            IHoldCommitmentInformation commitmentInformation = null,
            int datalockType = -1)
        {
            var payableEarnings = new List<FundingDue>();
            for (var transactionType = 1; transactionType <= 15; transactionType++)
            {
                if (datalockType != -1 && IgnoreTransactionType(datalockType, transactionType))
                {
                    continue;
                }

                var propertyName = $"TransactionType{transactionType:D2}";
                var amountDue = (decimal)FundingDueAccessor[rawEarnings, propertyName];
                if (amountDue == 0)
                {
                    continue;
                }
                var fundingDue = new FundingDue(rawEarnings);
                fundingDue.TransactionType = transactionType;

                if (!OnProgTransactionTypes.Contains(transactionType))
                {
                    fundingDue.SfaContributionPercentage = 1;
                }

                // Doing this to prevent a huge switch statement
                fundingDue.AmountDue = amountDue;
                commitmentInformation?.CopyCommitmentInformationTo(fundingDue);
                payableEarnings.Add(fundingDue);
            }

            return payableEarnings;
        }

        private List<NonPayableEarning> GetNonPayableEarnings(RawEarning rawEarnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitmentInformation = null)
        {
            var nonPayableEarnings = new List<NonPayableEarning>();
            for (var transactionType = 1; transactionType <= 15; transactionType++)
            {
                var amountDue = (decimal)FundingDueAccessor[rawEarnings, $"TransactionType{transactionType:D2}"];
                if (amountDue == 0)
                {
                    continue;
                }

                var nonPayableEarning = new NonPayableEarning(rawEarnings);
                nonPayableEarning.TransactionType = transactionType;

                // Doing this to prevent a huge switch statement
                nonPayableEarning.AmountDue = amountDue;
                commitmentInformation?.CopyCommitmentInformationTo(nonPayableEarning);

                nonPayableEarning.PaymentFailureMessage = reason;
                nonPayableEarning.PaymentFailureReason = paymentFailureReason;
                nonPayableEarnings.Add(nonPayableEarning);
            }

            return nonPayableEarnings;
        }
    }
}
