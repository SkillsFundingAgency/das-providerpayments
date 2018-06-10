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
            var priceEpisodes = RawEarnings.ToLookup(x => x.PriceEpisodeIdentifier).Distinct();

            foreach (var earningsForEpisode in priceEpisodes)
            {
                string reason;
                var priceEpisode = PriceEpisodes.SingleOrDefault(x => x.PriceEpisodeIdentifier == earningsForEpisode.Key);

                if (ShouldPayPriceEpisode(earningsForEpisode.Key, priceEpisode, out reason))
                {
                    MarkAsPayable(earningsForEpisode, priceEpisode);
                }
                else
                {
                    MarkAsNonPayable(earningsForEpisode, reason, priceEpisode);
                }
            }
        }

        private bool ShouldPayPriceEpisode(string priceEpisodeIdentifier, PriceEpisode priceEpisode, out string reason)
        {
            reason = string.Empty;
            var earnings = RawEarnings.Where(x => x.PriceEpisodeIdentifier == priceEpisodeIdentifier).ToList();

            // If only act2 earnings
            if (earnings.All(x => x.ApprenticeshipContractType == 2))
            {
                // Pay the price episode
                return true;
            }

            // If no 'bad' datalock, then pay for the price episode
            if (priceEpisode?.Payable ?? false)
            {
                return true;
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
                return false;
            }

            if (priceEpisode == null)
            {
                reason = $"No datalock present for price episode {priceEpisodeIdentifier}";
            }
            else
            {
                reason = $"Datalock failure for price episode {priceEpisodeIdentifier}";
            }

            return false;
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
                    x.AccountId,
                    x.CommitmentId)
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
                x.AccountId,
                x.CommitmentId)).ToDictionary(x => x.Key, x => x.ToList());

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
                x.AccountId,
                x.CommitmentId)).ToDictionary(x => x.Key, x => x.ToList());

            // Payments for earnings
            foreach (var key in groupedEarnings.Keys)
            {
                processedGroups.Add(key);
                var earnings = groupedEarnings[key];
                var pastPayments = new List<RequiredPaymentEntity>();
                var ignoredEarnings = new List<NonPayableEarningEntity>();

                if (groupedPastPayments.ContainsKey(key))
                {
                    pastPayments = groupedPastPayments[key];
                }

                if (groupedIgnoredPayments.ContainsKey(key))
                {
                    ignoredEarnings = groupedIgnoredPayments[key];
                }

                var payment = 
                    earnings.Sum(x => x.AmountDue) - 
                    pastPayments.Sum(x => x.AmountDue) -
                    ignoredEarnings.Sum(x => x.AmountDue);

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

                var pastPayments = groupedPastPayments[key];

                var ignoredEarnings = new List<NonPayableEarningEntity>();
                if (groupedIgnoredPayments.ContainsKey(key))
                {
                    ignoredEarnings = groupedIgnoredPayments[key];
                }

                var payment = ignoredEarnings.Sum(x => x.AmountDue) 
                              - pastPayments.Sum(x => x.AmountDue);

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
                if (rawEarning.ApprenticeshipContractType == 1)
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

        private IHoldCommitmentInformation AddCommitmentInformation(IHoldCommitmentInformation input, IHoldCommitmentInformation commitmentInformation)
        {
            input.AccountId = commitmentInformation.AccountId;
            input.AccountVersionId = commitmentInformation.AccountVersionId;
            input.CommitmentId = commitmentInformation.CommitmentId;
            input.CommitmentVersionId = commitmentInformation.CommitmentVersionId;
            return input;
        }
    }
}