﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using FastMember;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class Learner : ILearner
    {
        public Learner(
            IEnumerable<RawEarning> rawEarnings,
            IEnumerable<RawEarningForMathsOrEnglish> mathsAndEnglishEarnings,
            IEnumerable<PriceEpisode> priceEpisodes, 
            IEnumerable<RequiredPaymentEntity> pastPayments)
        {
            RawEarnings = rawEarnings.ToList();
            RawEarningsMathsEnglish = mathsAndEnglishEarnings.ToList();
            PriceEpisodes = priceEpisodes.ToList();
            PastPayments = pastPayments.ToList();
        }

        // Input
        private List<RawEarning> RawEarnings { get; }
        private IReadOnlyList<RawEarningForMathsOrEnglish> RawEarningsMathsEnglish { get; }
        public IReadOnlyList<PriceEpisode> PriceEpisodes { get; }
        public IReadOnlyList<RequiredPaymentEntity> PastPayments { get; }

        // Output
        public List<RequiredPaymentEntity> RequiredPayments { get; } = new List<RequiredPaymentEntity>();
        public List<NonPayableEarningEntity> NonPayableEarnings { get; } = new List<NonPayableEarningEntity>();
        public List<RequiredPaymentEntity> MustRefundThesePayments { get; } = new List<RequiredPaymentEntity>();
     
        // Internal
        private List<FundingDue> PayableEarnings { get; } = new List<FundingDue>();
        private IEnumerable<RawEarning> Act1RawEarnings => RawEarnings.Where(x => x.ApprenticeshipContractType == 1);
        private bool _ignoreLearner;                // Used for special case ACT2 -> ACT1 with datalock

        private void MatchMathsAndEnglishToOnProg()
        {
            var payableEarnings = new List<RawEarning>();
            var nonPayableEarnings = new List<RawEarning>();

            // Find a matching raw earning with the same course information
            foreach (var mathsOrEnglishEarning in RawEarningsMathsEnglish)
            {
                var matchingOnProg = RawEarnings.FirstOrDefault(x =>
                    x.FrameworkCode == mathsOrEnglishEarning.FrameworkCode &&
                    x.StandardCode == mathsOrEnglishEarning.StandardCode &&
                    x.PathwayCode == mathsOrEnglishEarning.PathwayCode &&
                    x.ProgrammeType == mathsOrEnglishEarning.ProgrammeType &&
                    x.ApprenticeshipContractType == mathsOrEnglishEarning.ApprenticeshipContractType);

                if (matchingOnProg != null)
                {
                    mathsOrEnglishEarning.PriceEpisodeIdentifier = matchingOnProg.PriceEpisodeIdentifier;
                    payableEarnings.Add(mathsOrEnglishEarning);
                }
                else
                {
                    nonPayableEarnings.Add(mathsOrEnglishEarning);
                }
            }

            RawEarnings.AddRange(payableEarnings);
            MarkAsNonPayable(nonPayableEarnings, "Maths or english aim with no matching on-prog aim");
        }

        private void ValidateEarningsAgainstDatalocks()
        {
            // The reason for this method is to put each raw earning into a payable or non-payable pot

            // BUSINESS RULES:
            //  If a learner has any passed datalocks, then pay all M/E
            //  If a learner is ACT2 only, pay everything
            //  If a learner moves from ACT2 to ACT1 and has a 'bad' datalock, then ignore them
            //  If a learner has a 'bad' datalock, ignore them for payments and refunds (includes the above)

            var act1 = Act1RawEarnings.ToList();
            // if there are *no* ACT1 earnings, then everything is ACT2 and payable
            if (act1.Count == 0)
            {
                MarkAsPayable(RawEarnings);

                return;
            }

            // THERE ARE ACT1 EARNINGS AT THIS POINT

            // Process each price episode seperately
            var earningsByPriceEpisode = RawEarnings.ToLookup(x => x.PriceEpisodeIdentifier).Distinct();

            foreach (var earningsForEpisode in earningsByPriceEpisode)
            {
                string reason;
                var availablePriceEpisodes = PriceEpisodes
                    .Where(x => x.PriceEpisodeIdentifier == earningsForEpisode.Key)
                    .ToList();
                if (availablePriceEpisodes.Count > 1)
                {
                    // Check for overlapping periods
                    var episodesThatArePayable = availablePriceEpisodes
                        .Where(x => x.MustRefundPriceEpisode == false).ToList();
                    if (episodesThatArePayable.Count == 0)
                    {
                        MarkAsNonPayable(earningsForEpisode, $"Could not find a datalock price episode for earning with price episode id: {earningsForEpisode.Key}");
                        continue;
                    }
                    var payablePeriodsSeen = new HashSet<int>();
                    foreach (var period in episodesThatArePayable.SelectMany(x => x.PayablePeriods))
                    {
                        if (payablePeriodsSeen.Contains(period))
                        {
                            MarkAsNonPayable(earningsForEpisode, "Multiple overlapping datalock price episodes found for earnings");
                            continue;
                        }

                        payablePeriodsSeen.Add(period);
                    }
                    // Split the earnings by price episode
                    foreach (var period in payablePeriodsSeen)
                    {
                        var earningsForPeriod = RawEarnings.Where(x =>
                            x.PriceEpisodeIdentifier == earningsForEpisode.Key &&
                            x.Period == period);
                        var episodeForPeriod = availablePriceEpisodes.Where(x => x.PayablePeriods.Contains(period)).ToList();
                        if (episodeForPeriod.Count > 1)
                        {
                            MarkAsNonPayable(earningsForPeriod, "Multiple overlapping commitments found for earnings");
                        }
                        else
                        {
                            var priceEpisode = episodeForPeriod.FirstOrDefault();
                            var allEarnings = CalculatePayableEarnings(earningsForEpisode.Key, priceEpisode, out reason, period);

                            MarkAsPayable(allEarnings.PayableEarnings, priceEpisode);
                            MarkAsNonPayable(allEarnings.NonPayableEarnings, reason, priceEpisode);
                            MarkAsMustRefund(allEarnings.MustRefundEarnings, reason, priceEpisode);
                        }
                    }
                }
                else
                {
                    var priceEpisode = availablePriceEpisodes.FirstOrDefault();
                    if (priceEpisode?.MustRefundPriceEpisode == true) 
                    {
                        MarkAsMustRefund(earningsForEpisode, "Not paying and also refunding as this if from a future academic year", priceEpisode);
                    }
                    else
                    {
                        var allEarnings = CalculatePayableEarnings(earningsForEpisode.Key, priceEpisode, out reason);

                        MarkAsPayable(allEarnings.PayableEarnings, priceEpisode);
                        MarkAsNonPayable(allEarnings.NonPayableEarnings, reason, priceEpisode);
                        MarkAsMustRefund(allEarnings.MustRefundEarnings, reason, priceEpisode);
                    }
                }
            }
        }

        class ShouldPayPriceEpisodeResult
        {
            public List<RawEarning> PayableEarnings { get; set; } = new List<RawEarning>();
            public List<RawEarning> NonPayableEarnings { get; set; } = new List<RawEarning>();
            public List<RawEarning> MustRefundEarnings { get; set; } = new List<RawEarning>();
        }

        private ShouldPayPriceEpisodeResult CalculatePayableEarnings(string priceEpisodeIdentifier, PriceEpisode priceEpisode, out string reason, int period = -1)
        {
            reason = string.Empty;
            List<RawEarning> earnings;
            if (period == -1)
            {
                earnings = RawEarnings.Where(x => x.PriceEpisodeIdentifier == priceEpisodeIdentifier).ToList();
            }
            else
            {
                earnings = RawEarnings
                    .Where(x => x.PriceEpisodeIdentifier == priceEpisodeIdentifier
                                && x.Period == period)
                    .ToList();
            }

            // If only act2 earnings
            if (earnings.All(x => x.ApprenticeshipContractType == 2))
            {
                // Pay the price episode
                return new ShouldPayPriceEpisodeResult
                {
                    PayableEarnings = earnings,
                };
            }
            
            // If the datalock price episode is set to 'Must Refund' add them to the must
            //  refund list
            if (priceEpisode?.MustRefundPriceEpisode == true)
            {
                reason = "Not paying and also refunding as this if from a future academic year";
                return new ShouldPayPriceEpisodeResult
                {
                    MustRefundEarnings = earnings,
                };
            }

            // If no 'bad' datalock, then pay for the price episode
            // TODO: May need to rename this to make it simpler to follow
            var earningsPeriodsThatAreNotPayableInThePriceEpisode = earnings.Select(x => x.Period)
                .Except(priceEpisode?.PayablePeriods ?? new List<int>())
                .ToList();

            if (!earningsPeriodsThatAreNotPayableInThePriceEpisode.Any())
            {
                return new ShouldPayPriceEpisodeResult
                {
                    PayableEarnings = earnings,
                };
            }

            // Check to see if we should be ignoring the learner
            var pastAct2Payments = PastPayments.Any(x => x.PriceEpisodeIdentifier == priceEpisodeIdentifier &&
                                                           x.ApprenticeshipContractType == 2);
            if (pastAct2Payments)
            {
                // Past payments for an ACT1 episode that were ACT2
                //  and a 'bad' datalock
                _ignoreLearner = true;
                reason = "ACT2 -> ACT1 learner with a failing datalock";
                return new ShouldPayPriceEpisodeResult
                {
                    NonPayableEarnings = earnings,
                };
            }

            if (priceEpisode == null)
            {
                reason = $"No datalock present for price episode {priceEpisodeIdentifier}";
            }
            else
            {
                reason = $"Datalock failure for price episode {priceEpisodeIdentifier}";
            }

            return new ShouldPayPriceEpisodeResult
            {
                PayableEarnings =
                    earnings.Where(x => !earningsPeriodsThatAreNotPayableInThePriceEpisode.Contains(x.Period))
                        .ToList(),
                NonPayableEarnings =
                    earnings.Where(x => earningsPeriodsThatAreNotPayableInThePriceEpisode.Contains(x.Period))
                        .ToList(),
            };
        }

        public LearnerProcessResults CalculatePaymentsDue()
        {
            MatchMathsAndEnglishToOnProg();
            ValidateEarningsAgainstDatalocks();

            if (_ignoreLearner)
            {
                return new LearnerProcessResults
                {
                    NonPayableEarnings = NonPayableEarnings,
                    PayableEarnings = new List<RequiredPaymentEntity>(),
                };
            }

            var processedGroups = new HashSet<MatchSetForPayments>();

            var groupedEarnings = PayableEarnings.GroupBy(x => new MatchSetForPayments
                (
                    x.StandardCode,
                    x.FrameworkCode,
                    x.ProgrammeType,
                    x.PathwayCode,
                    x.ApprenticeshipContractType,
                    x.TransactionType,
                    x.SfaContributionPercentage,
                    x.LearnAimRef,
                    x.FundingLineType,
                    x.DeliveryYear,
                    x.DeliveryMonth,
                    x.AccountId)
            ).ToDictionary(x => x.Key, x => x.ToList());

            var groupedPastPayments = PastPayments.GroupBy(x => new MatchSetForPayments
                (
                    x.StandardCode,
                    x.FrameworkCode,
                    x.ProgrammeType,
                    x.PathwayCode,
                    x.ApprenticeshipContractType,
                    x.TransactionType,
                    x.SfaContributionPercentage,
                    x.LearnAimRef,
                    x.FundingLineType,
                    x.DeliveryYear,
                    x.DeliveryMonth,
                    x.AccountId)
            ).ToDictionary(x => x.Key, x => x.ToList());

            var groupedIgnoredPayments = NonPayableEarnings.GroupBy(x => new MatchSetForPayments
                (
                    x.StandardCode,
                    x.FrameworkCode,
                    x.ProgrammeType,
                    x.PathwayCode,
                    x.ApprenticeshipContractType,
                    x.TransactionType,
                    x.SfaContributionPercentage,
                    x.LearnAimRef,
                    x.FundingLineType,
                    x.DeliveryYear,
                    x.DeliveryMonth,
                    x.AccountId)
            ).ToDictionary(x => x.Key, x => x.ToList());

            var groupedMustRefund = MustRefundThesePayments.GroupBy(x => new MatchSetForPayments
            (
                x.StandardCode,
                x.FrameworkCode,
                x.ProgrammeType,
                x.PathwayCode,
                x.ApprenticeshipContractType,
                x.TransactionType,
                x.SfaContributionPercentage,
                x.LearnAimRef,
                x.FundingLineType,
                x.DeliveryYear,
                x.DeliveryMonth,
                x.AccountId)
            ).ToDictionary(x => x.Key, x => x.ToList());

            // Payments for earnings
            foreach (var key in groupedEarnings.Keys)
            {
                processedGroups.Add(key);
                var earnings = groupedEarnings[key];
                var pastPayments = new List<RequiredPaymentEntity>();
                
                if (groupedPastPayments.ContainsKey(key))
                {
                    pastPayments = groupedPastPayments[key];
                }

                var payment = earnings.Sum(x => x.AmountDue) -
                              pastPayments.Sum(x => x.AmountDue);

                if (payment != 0)
                {
                    AddRequiredPayment(earnings.First(), payment);
                }
            }

            // Refunds for past payments that don't have corresponding earnings
            foreach (var key in groupedPastPayments.Keys)
            {
                if (processedGroups.Contains(key))
                {
                    // Already processed
                    continue;
                }

                var mustRefund = groupedMustRefund.ContainsKey(key);

                if (groupedIgnoredPayments.ContainsKey(key) && !mustRefund)
                {
                    continue;
                }

                var pastPayments = groupedPastPayments[key];

                var payment = - pastPayments.Sum(x => x.AmountDue);

                if (payment != 0)
                {
                    AddRequiredPayment(pastPayments.First(), payment);
                }
            }

            return new LearnerProcessResults
            {
                NonPayableEarnings = NonPayableEarnings,
                PayableEarnings = RequiredPayments,
            };
        }

        private void AddRequiredPayment(RequiredPaymentEntity requiredPayment, decimal amount)
        {
            var payment = new RequiredPaymentEntity(requiredPayment);
            payment.AmountDue = amount;
            RequiredPayments.Add(payment);
        }

        private void MarkAsPayable(IEnumerable<RawEarning> earnings, IHoldCommitmentInformation commitment = null)
        {
            foreach (var rawEarning in earnings)
            {
                if (rawEarning.ApprenticeshipContractType == 1 &&
                    rawEarning.SfaContributionPercentage < 1)
                {
                    rawEarning.UseLevyBalance = true;
                }

                AddFundingDue(rawEarning, commitment);
            }
        }

        private void MarkAsNonPayable(IEnumerable<RawEarning> earnings, string reason, IHoldCommitmentInformation commitment = null)
        {
            foreach (var rawEarning in earnings)
            {
                AddNonpayableFundingDue(rawEarning, reason, commitment);
            }
        }

        private void MarkAsMustRefund(IEnumerable<RawEarning> earnings, string reason, IHoldCommitmentInformation commitment = null)
        {
            foreach (var rawEarning in earnings)
            {
                AddMustRefundEarnings(rawEarning, reason, commitment);
            }
        }

        private static readonly TypeAccessor FundingDueAccessor = TypeAccessor.Create(typeof(RawEarning));
        private void AddFundingDue(RawEarning rawEarnings, IHoldCommitmentInformation commitmentInformation = null)
        {
            for (var i = 1; i <= 15; i++)
            {
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
                if (commitmentInformation != null)
                {
                    AddCommitmentInformation(fundingDue, commitmentInformation);
                }
                PayableEarnings.Add(fundingDue);
            }
        }

        private void AddNonpayableFundingDue(RawEarning rawEarnings, string reason, IHoldCommitmentInformation commitmentInformation = null)
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
                if (commitmentInformation != null)
                {
                    AddCommitmentInformation(nonPayableEarning, commitmentInformation);
                }

                nonPayableEarning.Reason = reason;
                NonPayableEarnings.Add(nonPayableEarning);
            }
        }

        private void AddMustRefundEarnings(RawEarning rawEarnings, string reason, IHoldCommitmentInformation commitmentInformation = null)
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
                
                if (commitmentInformation != null)
                {
                    AddCommitmentInformation(nonPayableEarning, commitmentInformation);
                }

                nonPayableEarning.Reason = reason;
                NonPayableEarnings.Add(nonPayableEarning);
                MustRefundThesePayments.Add(nonPayableEarning);
            }
        }

        private void AddCommitmentInformation(IHoldCommitmentInformation input, IHoldCommitmentInformation commitmentInformation)
        {
            input.AccountId = commitmentInformation.AccountId;
            input.AccountVersionId = commitmentInformation.AccountVersionId;
            input.CommitmentId = commitmentInformation.CommitmentId;
            input.CommitmentVersionId = commitmentInformation.CommitmentVersionId;
        }
    }
}