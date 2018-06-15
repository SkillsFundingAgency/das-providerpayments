using System.Collections.Generic;
using System.Linq;
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
            PriceEpisodeDatalockResults = priceEpisodes.ToList();
            PastPayments = pastPayments.ToList();
        }

        // Input
        private List<RawEarning> RawEarnings { get; }
        private IReadOnlyList<RawEarningForMathsOrEnglish> RawEarningsMathsEnglish { get; }
        public IReadOnlyList<PriceEpisode> PriceEpisodeDatalockResults { get; }
        public IReadOnlyList<RequiredPaymentEntity> PastPayments { get; }

        // Output
        public List<RequiredPaymentEntity> RequiredPayments { get; } = new List<RequiredPaymentEntity>();
        public List<NonPayableEarningEntity> NonPayableEarnings { get; } = new List<NonPayableEarningEntity>();
        public List<RequiredPaymentEntity> MustRefundThesePayments { get; } = new List<RequiredPaymentEntity>();
     
        // Internal
        private List<FundingDue> PayableEarnings { get; } = new List<FundingDue>();
        private IEnumerable<RawEarning> Act1RawEarnings => RawEarnings.Where(x => x.ApprenticeshipContractType == 1);
        
        private void MatchMathsAndEnglishToOnProg()
        {
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

                    var priceEpisode = PriceEpisodeDatalockResults.FirstOrDefault(x =>
                        x.PriceEpisodeIdentifier == matchingOnProg.PriceEpisodeIdentifier);

                    if (priceEpisode != null)
                    {
                        if (priceEpisode.SuccesfulDatalock)
                        {
                            MarkAsPayable(new List<RawEarning>{ mathsOrEnglishEarning }, priceEpisode);
                            continue;
                        }
                        if (priceEpisode.MustRefundPriceEpisode)
                        {
                            MarkAsMustRefund(new List<RawEarning> { mathsOrEnglishEarning },
                                $"Payment for future academic year - refunding - with price episode id: {matchingOnProg.PriceEpisodeIdentifier}",
                                priceEpisode);
                            continue;
                        }
                    }
                    MarkAsNonPayable(new List<RawEarning>{ mathsOrEnglishEarning }, "Not paying maths/english. No price episode or price episode is datalocked");
                }
            }
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
                MarkAsPayable(RawEarningsMathsEnglish);

                return;
            }

            // THERE ARE ACT1 EARNINGS AT THIS POINT
            
            // Process each price episode seperately
            var earningsByPriceEpisode = RawEarnings.ToLookup(x => x.PriceEpisodeIdentifier).Distinct();
            
            foreach (var earningsForEpisode in earningsByPriceEpisode)
            {
                foreach (var rawEarning in earningsForEpisode)
                {
                    var priceEpisode = PriceEpisodeDatalockResults.FirstOrDefault(x =>
                        x.PriceEpisodeIdentifier == rawEarning.PriceEpisodeIdentifier &&
                        !x.PeriodsToIgnore.Contains(rawEarning.Period));
                    if (priceEpisode != null && priceEpisode.SuccesfulDatalock)
                    {
                        MarkAsPayable(new List<RawEarning>{rawEarning}, priceEpisode);
                    }
                    else if (priceEpisode != null && priceEpisode.MustRefundPriceEpisode)
                    {
                        MarkAsMustRefund(new List<RawEarning>{rawEarning}, "Future academic year payment clawback", priceEpisode);
                    }
                    else if (priceEpisode == null)
                    {
                        MarkAsPayable(new List<RawEarning> { rawEarning });
                    } 
                    else
                    {
                        MarkAsNonPayable(new List<RawEarning> { rawEarning }, "Datalock failure for earning", priceEpisode);
                    }

                }
            }
        }

        public LearnerProcessResults CalculatePaymentsDue()
        {
            MatchMathsAndEnglishToOnProg();
            ValidateEarningsAgainstDatalocks();

            var periodsToIgnore = PriceEpisodeDatalockResults
                .Where(x => !x.SuccesfulDatalock)
                .SelectMany(x => x.PeriodsToIgnore)
                .ToList();
            var periodsToProcess = Enumerable.Range(1, 12)
                .Except(periodsToIgnore)
                .ToList();


            if (periodsToProcess.Count == 0)
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
                var mustRefund = new List<RequiredPaymentEntity>();
                
                if (groupedPastPayments.ContainsKey(key))
                {
                    pastPayments = groupedPastPayments[key];
                }

                if (groupedMustRefund.ContainsKey(key))
                {
                    mustRefund = groupedMustRefund[key];
                }

                var payment = earnings.Sum(x => x.AmountDue) -
                              pastPayments.Sum(x => x.AmountDue) -
                              mustRefund.Sum(x => x.AmountDue);

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