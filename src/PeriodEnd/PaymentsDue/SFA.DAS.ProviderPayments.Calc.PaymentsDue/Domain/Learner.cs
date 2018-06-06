using System.Collections.Generic;
using System.Linq;
using FastMember;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class Learner
    {
        private List<CollectionPeriodEntity> CollectionPeriods { get; set; }

        public Learner(List<CollectionPeriodEntity> collectionPeriods)
        {
            CollectionPeriods = collectionPeriods;
        }

        // Input
        public List<RawEarningEntity> RawEarnings { get; set; } = new List<RawEarningEntity>();
        public List<RawEarningMathsEnglishEntity> RawEarningsMathsEnglish { get; set; } = new List<RawEarningMathsEnglishEntity>();
        public List<DataLockPriceEpisodePeriodMatchEntity> DataLocks { get; set; } = new List<DataLockPriceEpisodePeriodMatchEntity>();
        public List<RequiredPaymentsHistoryEntity> HistoricalPayments { get; set; } = new List<RequiredPaymentsHistoryEntity>();
        public List<RequiredPaymentEntity> RequiredPayments { get; set; } = new List<RequiredPaymentEntity>();
        public List<Commitment> Commitments { get; set; } = new List<Commitment>();

        // Output
        public List<FundingDue> PayableEarnings { get; set; }
        public List<FundingDue> FundingDue { get; } = new List<FundingDue>();
        public List<NonPayableEarningEntity> NonPayableEarnings { get; set; } = new List<NonPayableEarningEntity>();


        // Internal
        private IEnumerable<RawEarningEntity> Act1RawEarnings => RawEarnings.Where(x => x.ApprenticeshipContractType == 1);
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

        private class MatchSetForPayments
        {
            public int StandardCode { get; set; }
            public int FrameworkCode { get; set; }
            public int ProgrammeType { get; set; }
            public int PathwayCode { get; set; }
            public int ApprenticeshipContractType { get; set; }
            public int TransactionType { get; set; }
            public decimal SfaContributionPercentage { get; set; }
            public string LearnAimRef { get; set; }
            public string FundingLineType { get; set; }
            public int DeliveryYear { get; set; }
            public int DeliveryMonth { get; set; }
            public long? AccountId { get; set; }
            public long? CommitmentId { get; set; }
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
        private static readonly int[] RawEarningsTransactionTypes = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 15 };
        private static readonly int[] RawMathsAndEnglishTransactionTypes = { 13, 14, 15 };

        private void AddFundingDue(RawEarningEntity rawEarnings, IHoldCommitmentInformation commitmentInformation = null)
        {
            foreach (var transactionType in RawEarningsTransactionTypes)
            {
                var fundingDue = CreateBasicFundingDue(rawEarnings);
                fundingDue.TransactionType = transactionType;
                // Doing this to prevent a huge switch statement
                fundingDue.AmountDue = (decimal)FundingDueAccessor[rawEarnings, $"TransactionType{transactionType:D2}"];
                if (commitmentInformation != null)
                {
                    AddCommitmentInformation(fundingDue, commitmentInformation);
                }
                PayableEarnings.Add(fundingDue);
            }
        }
        
        private void AddFundingDue(RawEarningMathsEnglishEntity mathsAndEnglish, IHoldCommitmentInformation commitmentInformation = null)
        {
            foreach (var transactionType in RawMathsAndEnglishTransactionTypes)
            {
                var fundingDue = CreateBasicFundingDue(mathsAndEnglish);
                fundingDue.TransactionType = transactionType;
                // Doing this to prevent a huge switch statement
                fundingDue.AmountDue = (decimal) FundingDueAccessor[mathsAndEnglish, $"TransactionType{transactionType:D2}"];
                if (commitmentInformation != null)
                {
                    AddCommitmentInformation(fundingDue, commitmentInformation);
                }
                PayableEarnings.Add(fundingDue);
            }
        }

        private void AddNonpayableFundingDue(RawEarningEntity rawEarnings, string reason, IHoldCommitmentInformation commitmentInformation = null)
        {
            foreach (var transactionType in RawEarningsTransactionTypes)
            {
                var nonPayableEarning = CreateBasicNonPayableEarningEntity(rawEarnings);
                nonPayableEarning.TransactionType = transactionType;
                // Doing this to prevent a huge switch statement
                nonPayableEarning.AmountDue = (decimal)FundingDueAccessor[rawEarnings, $"TransactionType{transactionType:D2}"];
                if (commitmentInformation != null)
                {
                    AddCommitmentInformation(nonPayableEarning, commitmentInformation);
                }

                nonPayableEarning.Reason = reason;
                NonPayableEarnings.Add(nonPayableEarning);
            }
        }

        private void AddNonpayableFundingDue(RawEarningMathsEnglishEntity rawEarnings, string reason, IHoldCommitmentInformation commitmentInformation = null)
        {
            foreach (var transactionType in RawMathsAndEnglishTransactionTypes)
            {
                var nonPayableEarning = CreateBasicNonPayableEarningEntity(rawEarnings);
                nonPayableEarning.TransactionType = transactionType;
                // Doing this to prevent a huge switch statement
                nonPayableEarning.AmountDue = (decimal)FundingDueAccessor[rawEarnings, $"TransactionType{transactionType:D2}"];
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

        
        private FundingDue CreateBasicFundingDue(IFundingDue fundingDue)
        {
            return new FundingDue
            {
                AimSeqNumber = fundingDue.AimSeqNumber,
                ApprenticeshipContractType = fundingDue.ApprenticeshipContractType,
                FrameworkCode = fundingDue.FrameworkCode,
                PathwayCode = fundingDue.PathwayCode,
                FundingLineType = fundingDue.FundingLineType,
                LearnAimRef = fundingDue.LearnAimRef,
                LearnRefNumber = fundingDue.LearnRefNumber,
                LearningStartDate = fundingDue.LearningStartDate,
                Period = fundingDue.Period,
                ProgrammeType = fundingDue.ProgrammeType,
                StandardCode = fundingDue.StandardCode,
                SfaContributionPercentage = fundingDue.SfaContributionPercentage,
                Ukprn = fundingDue.Ukprn,
                Uln = fundingDue.Uln,
                DeliveryMonth = CalculateDeliveryMonth(fundingDue.Period),
                DeliveryYear = CalculateDeliveryYear(fundingDue.Period),
            };
        }

        private NonPayableEarningEntity CreateBasicNonPayableEarningEntity(IFundingDue fundingDue)
        {
            return new NonPayableEarningEntity
            {
                AimSeqNumber = fundingDue.AimSeqNumber,
                ApprenticeshipContractType = fundingDue.ApprenticeshipContractType,
                FrameworkCode = fundingDue.FrameworkCode,
                PathwayCode = fundingDue.PathwayCode,
                FundingLineType = fundingDue.FundingLineType,
                LearnAimRef = fundingDue.LearnAimRef,
                LearnRefNumber = fundingDue.LearnRefNumber,
                LearningStartDate = fundingDue.LearningStartDate,
                Period = fundingDue.Period,
                ProgrammeType = fundingDue.ProgrammeType,
                StandardCode = fundingDue.StandardCode,
                SfaContributionPercentage = fundingDue.SfaContributionPercentage,
                Ukprn = fundingDue.Ukprn,
                Uln = fundingDue.Uln,
                DeliveryMonth = CalculateDeliveryMonth(fundingDue.Period),
                DeliveryYear = CalculateDeliveryYear(fundingDue.Period),
            };
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