using System.Collections.Generic;
using System.Linq;
using FastMember;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class Learner
    {
        public Learner(List<CollectionPeriodEntity> collectionPeriods)
        {
            CollectionPeriods = collectionPeriods;
        }

        // Input
        public List<RawEarning> RawEarnings { get; set; } = new List<RawEarning>();
        public List<RawEarningForMathsOrEnglish> RawEarningsMathsEnglish { get; set; } = new List<RawEarningForMathsOrEnglish>();
        public List<DataLockPriceEpisodePeriodMatchEntity> DataLocks { get; set; } = new List<DataLockPriceEpisodePeriodMatchEntity>();
        public List<RequiredPaymentsHistoryEntity> HistoricalPayments { get; set; } = new List<RequiredPaymentsHistoryEntity>();
        public List<Commitment> Commitments { get; set; } = new List<Commitment>();

        // Output
        public List<FundingDue> PayableEarnings { get; set; }
        public List<FundingDue> FundingDue { get; } = new List<FundingDue>();
        public List<RequiredPaymentEntity> RequiredPayments { get; set; } = new List<RequiredPaymentEntity>();

        public List<NonPayableEarningEntity> NonPayableEarnings { get; set; } = new List<NonPayableEarningEntity>();


        // Internal
        private IEnumerable<RawEarning> Act1RawEarnings => RawEarnings.Where(x => x.ApprenticeshipContractType == 1);
        private List<CollectionPeriodEntity> CollectionPeriods { get; }


        public bool IgnoreForPayments { get; set; }

        public void ValidateEarnings()
        {
            var act1 = Act1RawEarnings.ToList();
            // if there are *no* ACT1 earnings, then everything is ACT2 and payable
            if (act1.Count == 0)
            {
                foreach (var rawEarningEntity in RawEarnings)
                {
                    AddFundingDue(rawEarningEntity);
                }

                foreach (var rawEarningMathsEnglishEntity in RawEarningsMathsEnglish)
                {
                    AddFundingDue(rawEarningMathsEnglishEntity);
                }

                return;
            }

            // If there are only datalock failures then ignore this learner
            if (DataLocks.All(x => x.Payable == false))
            {
                IgnoreForPayments = true;
                MarkAllEarningsAsNonPayable();
                return;
            }
            
            // Process each price episode seperately
            var priceEpisodes = RawEarnings.Select(x => x.PriceEpisodeIdentifier).Distinct();

            foreach (var priceEpisode in priceEpisodes)
            {
                ProcessPriceEpisode(priceEpisode);
            }
        }

        private void ProcessPriceEpisode(string priceEpisodeIdentifier)
        {
            var earnings = RawEarnings.Where(x => x.PriceEpisodeIdentifier == priceEpisodeIdentifier);
            var pastPayments = HistoricalPayments.Where(x => x.PriceEpisodeIdentifier == priceEpisodeIdentifier);
            var datalocks = DataLocks.Where(x => x.PriceEpisodeIdentifier == priceEpisodeIdentifier);

            // If there are ACT2 past payments then it means that they have swapped
            // and need to not have a datalock for them
            if (HistoricalPayments.Any(x => x.ApprenticeshipContractType == 2))
            {

            }

            // Mark all earnings as payable
            var datalock = DataLocks.FirstOrDefault();
            Commitment commitment = null;
            if (datalock != null)
            {
                commitment = Commitments.FirstOrDefault(x => x.CommitmentId == datalock.CommitmentId);
            }

            foreach (var rawEarningEntity in RawEarnings)
            {
                AddFundingDue(rawEarningEntity, commitment);
            }

            foreach (var rawEarningMathsEnglishEntity in RawEarningsMathsEnglish)
            {
                AddFundingDue(rawEarningMathsEnglishEntity, commitment);
            }
        }

        public void CalculateFundingDue()
        {
            var processedGroups = new HashSet<MatchSetForPayments>();

            var groupedEarnings = PayableEarnings.GroupBy(x => new MatchSetForPayments
            {
                StandardCode = x.StandardCode,
                FrameworkCode = x.FrameworkCode,
                ProgrammeType = x.ProgrammeType,
                PathwayCode = x.PathwayCode,
                ApprenticeshipContractType = x.ApprenticeshipContractType,
                TransactionType = x.TransactionType,
                SfaContributionPercentage = x.SfaContributionPercentage,
                LearnAimRef = x.LearnAimRef,
                FundingLineType = x.FundingLineType,
                DeliveryYear = x.DeliveryYear,
                DeliveryMonth = x.DeliveryMonth,
                AccountId = x.AccountId,
                CommitmentId = x.CommitmentId,
            }).ToDictionary(x => x.Key, x => x.ToList());

            var groupedPastPayments = HistoricalPayments.GroupBy(x => new MatchSetForPayments
            {
                StandardCode = x.StandardCode,
                FrameworkCode = x.FrameworkCode,
                ProgrammeType = x.ProgrammeType,
                PathwayCode = x.PathwayCode,
                ApprenticeshipContractType = x.ApprenticeshipContractType,
                TransactionType = x.TransactionType,
                SfaContributionPercentage = x.SfaContributionPercentage,
                LearnAimRef = x.LearnAimRef,
                FundingLineType = x.FundingLineType,
                DeliveryYear = x.DeliveryYear,
                DeliveryMonth = x.DeliveryMonth,
                AccountId = x.AccountId,
                CommitmentId = x.CommitmentId,
            }).ToDictionary(x => x.Key, x => x.ToList()); ;

            // Payments for earnings
            foreach (var key in groupedEarnings.Keys)
            {
                processedGroups.Add(key);
                var earnings = groupedEarnings[key];
                var pastPayments = new List<RequiredPaymentsHistoryEntity>();
                if (groupedPastPayments.ContainsKey(key))
                {
                    pastPayments = groupedPastPayments[key];
                }

                var payment = earnings.Sum(x => x.AmountDue) - pastPayments.Sum(x => x.AmountDue);
                if (payment != 0)
                {
                    AddPayment(earnings.First(), payment);
                }
            }

            // Refunds for past payments that don't have corresponding earnings
            foreach (var key in groupedPastPayments.Keys)
            {
                if (processedGroups.Contains(key))
                {
                    continue;
                }

                var payments = groupedPastPayments[key];
                var payment = -(payments.Sum(x => x.AmountDue));
                if (payment != 0)
                {
                    AddPayment(new FundingDue(payments.First()), payment);
                }
            }
        }

        private void AddPayment(FundingDue fundingDue, decimal amount)
        {
            fundingDue.AmountDue = amount;
            FundingDue.Add(fundingDue);
        }

        private void MarkAllEarningsAsNonPayable()
        {
            // Get the commitment information
            var datalock = DataLocks.FirstOrDefault();

            string reason;
            Commitment commitment = null;

            if (datalock == null)
            {
                reason = "Could not find a matching datalock for ACT 1 learner";
            }
            else
            {
                reason = "Datalock failed for ACT 1 learner";
                commitment = Commitments.First(x => x.CommitmentId == datalock.CommitmentId);
            }
            
            foreach (var rawEarningEntity in RawEarnings)
            {
                AddNonpayableFundingDue(rawEarningEntity, reason, commitment);
            }

            foreach (var rawEarningMathsEnglishEntity in RawEarningsMathsEnglish)
            {
                AddNonpayableFundingDue(rawEarningMathsEnglishEntity, reason, commitment);
            }
        }

        private static readonly TypeAccessor FundingDueAccessor = TypeAccessor.Create(typeof(FundingDue));
        
        private void AddFundingDue(RawEarning rawEarnings, IHoldCommitmentInformation commitmentInformation = null)
        {
            for (var i = 1; i <= 15; i++)
            {
                var amountDue = (decimal)FundingDueAccessor[rawEarnings, $"TransactionType{i:D2}"];
                if (amountDue == 0)
                {
                    continue;
                }
                var fundingDue = new FundingDue(rawEarnings);
                fundingDue.TransactionType = i;
                fundingDue.DeliveryMonth = CalculateDeliveryMonth(rawEarnings.Period);
                fundingDue.DeliveryYear = CalculateDeliveryYear(rawEarnings.Period);
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
                nonPayableEarning.DeliveryMonth = CalculateDeliveryMonth(rawEarnings.Period);
                nonPayableEarning.DeliveryYear = CalculateDeliveryYear(rawEarnings.Period);

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
        
        private int CalculateDeliveryMonth(int period)
        {
            return CollectionPeriods.First(x => x.Id == period).Month;
        }

        private int CalculateDeliveryYear(int period)
        {
            return CollectionPeriods.First(x => x.Id == period).Year;
        }
    }
}