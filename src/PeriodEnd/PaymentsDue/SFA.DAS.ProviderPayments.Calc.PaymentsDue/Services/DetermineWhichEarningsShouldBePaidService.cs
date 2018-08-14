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

        private DateTime _firstDayOfThisAcademicYear;
        private DateTime _firstDayOfNextAcademicYear;
        private readonly ICollectionPeriodRepository _collectionPeriodRepository;

        private static readonly List<int> OnProgTransactionTypes = new List<int> { 1, 2, 3 };

        public DetermineWhichEarningsShouldBePaidService(ICollectionPeriodRepository collectionPeriodRepository)
        {
            _collectionPeriodRepository = collectionPeriodRepository;
        }

        // INPUT
        List<RawEarning> RawEarnings { get; set; }
        List<RawEarningForMathsOrEnglish> RawEarningsMathsOrEnglish { get; set; }
        List<DatalockOutput> DatalockOutput { get; set; }
        
        // OUTPUT
        private List<FundingDue> PayableEarnings { get; set; }
        private HashSet<int> PeriodsToIgnore { get; set; }
        private List<NonPayableEarning> NonPayableEarnings { get; set; }


        public EarningValidationResult DeterminePayableEarnings(
            List<DatalockOutput> datalockOutput,
            List<RawEarning> earnings,
            List<RawEarningForMathsOrEnglish> mathsAndEnglishEarnings,
            CompletionPaymentEvidence completionPaymentEvidence)
        {
            PayableEarnings = new List<FundingDue>();
            PeriodsToIgnore = new HashSet<int>();
            NonPayableEarnings = new List<NonPayableEarning>();

            SetFirstDayOfAcademicYears();

            // Exclude any earnings or datalocks that fall outside this academic year
            RawEarnings = earnings.Where(x => PriceEpisodeFallsWithinAcademicYear(x.PriceEpisodeIdentifier)).ToList();
            DatalockOutput = datalockOutput.Where(x => PriceEpisodeFallsWithinAcademicYear(x.PriceEpisodeIdentifier)).ToList();

            RawEarningsMathsOrEnglish = mathsAndEnglishEarnings;

            // The reason for this method is to put each raw earning into a payable or non-payable pot

            ValidateEarnings(completionPaymentEvidence);
            MatchMathsAndEnglishToOnProg();

            return new EarningValidationResult(PayableEarnings, NonPayableEarnings, PeriodsToIgnore.ToList());
        }

        private void SetFirstDayOfAcademicYears()
        {
            var currentCollectionPeriodAcademicYear = _collectionPeriodRepository.GetCurrentCollectionPeriod()?.AcademicYear ?? "1718";
            var startingYear = int.Parse(currentCollectionPeriodAcademicYear.Substring(2)) + 2000; // will fail in 2100...
            _firstDayOfNextAcademicYear = new DateTime(startingYear, 8, 1);
            _firstDayOfThisAcademicYear = _firstDayOfNextAcademicYear.AddYears(-1);
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

        private bool PriceEpisodeFallsWithinAcademicYear(string priceEpisodeIdentifier)
        {
            var priceEpisodeStartDate = DateFromPriceEpisodeIdentifier(priceEpisodeIdentifier);
            return priceEpisodeStartDate >= _firstDayOfThisAcademicYear &&
                   priceEpisodeStartDate < _firstDayOfNextAcademicYear;
        }

        private void ValidateEarnings(CompletionPaymentEvidence completionPaymentEvidence)
        {
            if (RawEarnings.All(x => x.ApprenticeshipContractType == 2))
            {
                MarkNonZeroTransactionTypesAsPayable(RawEarnings);
                return;
            }

            // Look at the earnings now. We are expecting there to be at most one successful datalock per 
            //  period
            // If there are earnings and 0 successful datalocks, then ignore the period

            var earningsByPeriod = RawEarnings.ToLookup(x => x.Period);
            foreach (var periodGroup in earningsByPeriod.OrderBy(x => x.Key))
            {
                var earningsForPeriod = periodGroup
                    .Where(x => x.HasNonZeroTransactions())
                    .ToList();

                if (earningsForPeriod.All(x => x.ApprenticeshipContractType == 2))
                {
                    MarkNonZeroTransactionTypesAsPayable(earningsForPeriod);
                    continue;
                }

                var periodEarningsForPriceEpisodeGroups = earningsForPeriod.ToLookup(x => x.PriceEpisodeIdentifier);
                foreach (var periodEarningsForPriceEpisode in periodEarningsForPriceEpisodeGroups)
                {
                    var priceEpisode = periodEarningsForPriceEpisode.Key;

                    var datalocks = DatalockOutput
                        .Where(x => x.Period == periodGroup.Key &&
                                    x.PriceEpisodeIdentifier == priceEpisode)
                        .ToList();

                    if (datalocks.Count == 0)
                    {
                        MarkNonZeroTransactionTypesAsNonPayable(periodEarningsForPriceEpisode,
                            $"Could not find a matching datalock for price episode: {priceEpisode} in period: {periodGroup.Key}",
                            PaymentFailureType.CouldNotFindSuccessfulDatalock);
                        PeriodsToIgnore.Add(periodGroup.Key);
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
                            MarkNonZeroTransactionTypesAsNonPayable(periodEarningsForPriceEpisode,
                                $"Multiple matching datalocks for price episode: {priceEpisode} in period: {periodGroup.Key}",
                                PaymentFailureType.MultipleMatchingSuccessfulDatalocks,
                                datalocksForFlag.First());
                            PeriodsToIgnore.Add(periodGroup.Key);
                            continue;
                        }

                        if (datalocksForFlag.Count == 1)
                        {
                            // We have 1 datalock and a commitment
                            MarkNonZeroTransactionTypesAsPayable(
                                periodEarningsForPriceEpisode,
                                datalocksForFlag.Single(),
                                completionPaymentEvidence,
                                transactionTypesFlag);
                        }
                    }
                }
            }
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
                    foreach (var rawEarningForMathsOrEnglish in RawEarningsMathsOrEnglish)
                    {
                        var matchingEarning = RawEarnings.FirstOrDefault(x =>
                            x.HasMatchingCourseInformationWith(rawEarningForMathsOrEnglish));
                        if (matchingEarning != null)
                        {
                            rawEarningForMathsOrEnglish.PriceEpisodeIdentifier = matchingEarning.PriceEpisodeIdentifier;
                            MarkNonZeroTransactionTypesAsPayable(
                                new List<RawEarningForMathsOrEnglish> { rawEarningForMathsOrEnglish });
                        }
                        else
                        {
                            MarkNonZeroTransactionTypesAsNonPayable(
                                new List<RawEarningForMathsOrEnglish> { rawEarningForMathsOrEnglish },
                                "No on-prog earning found for maths/english earning",
                                PaymentFailureType.CouldNotFindMatchingOnprog);
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
                            .Where(x => x.HasMatchingCourseInformationWith(rawEarning))
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
                    x.HasMatchingCourseInformationWith(mathsOrEnglishEarning) &&
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
            CompletionPaymentEvidence completionPaymentEvidence = null,
            int datalockType = -1)
        {
            string reasonToHoldBack = "";
            bool holdBack = false;
            if(completionPaymentEvidence != null)
            {
                holdBack = HoldBackCompletionPayment(completionPaymentEvidence, out reasonToHoldBack);
            }

            foreach (var rawEarning in earnings)
            {
                if (rawEarning.ApprenticeshipContractType == 1 &&
                    rawEarning.SfaContributionPercentage < 1)
                {
                    rawEarning.UseLevyBalance = true;
                }

                if (rawEarning.TransactionType02 > 0 && holdBack)
                {
                    MarkCompletionPaymentsAsHeldBackAndNonPayable(rawEarning, reasonToHoldBack, commitment);
                    rawEarning.TransactionType02 = 0;
                }

                AddFundingDue(rawEarning, commitment, datalockType);
            }
        }

        private void MarkCompletionPaymentsAsHeldBackAndNonPayable(
            RawEarning rawEarning,
            string reason,
            IHoldCommitmentInformation commitment = null)
        {
            var nonPayableEarning = new NonPayableEarning(rawEarning);
            nonPayableEarning.TransactionType = (int)TransactionType.Completion;

            // Doing this to prevent a huge switch statement
            nonPayableEarning.AmountDue = rawEarning.TransactionType02;
            commitment?.CopyCommitmentInformationTo(nonPayableEarning);

            nonPayableEarning.PaymentFailureMessage = reason;
            nonPayableEarning.PaymentFailureReason = PaymentFailureType.HeldBackCompletionPayment;
            NonPayableEarnings.Add(nonPayableEarning);
        }

        private bool HoldBackCompletionPayment(CompletionPaymentEvidence completionPaymentEvidence, out string reason)
        {
            if (completionPaymentEvidence.State == CompletionPaymentEvidenceState.ErrorOnIlr)
            {
                reason = "Error on PMR records in ILR";
                return true;
            }

            if (Decimal.Round(completionPaymentEvidence.IlrEvidenceEmployerPayment) <
                Decimal.Round(completionPaymentEvidence.TotalHistoricEmployerPayment))
            {
                reason = "Historic Evidence does not show enough employer payments were made";
                return true;
            }

            reason = "";
            return false;
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
            foreach (var transactionTypeValue in Enum.GetValues(typeof(TransactionType)))
            {
                var transactionType = (int)transactionTypeValue;
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
                PayableEarnings.Add(fundingDue);
            }
        }

        private void AddNonpayableFundingDue(RawEarning rawEarnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitmentInformation = null)
        {
            foreach (var transactionTypeValue in Enum.GetValues(typeof(TransactionType)))
            {
                var transactionType = (int) transactionTypeValue;
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
                NonPayableEarnings.Add(nonPayableEarning);
            }
        }
    }
}
