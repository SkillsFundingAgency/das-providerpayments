using System.Collections.Generic;
using System.Linq;
using FastMember;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public class Learner
    {
        public List<RawEarningEntity> RawEarnings { get; set; } = new List<RawEarningEntity>();
        public List<RawEarningMathsEnglishEntity> RawEarningsMathsEnglish { get; set; } = new List<RawEarningMathsEnglishEntity>();
        public List<DataLockPriceEpisodePeriodMatchEntity> DataLocks { get; set; } = new List<DataLockPriceEpisodePeriodMatchEntity>();
        public List<RequiredPaymentsHistoryEntity> HistoricalPayments { get; set; } = new List<RequiredPaymentsHistoryEntity>();
        public List<RequiredPaymentEntity> RequiredPayments { get; set; } = new List<RequiredPaymentEntity>();

        public List<string> DatalockErrors { get; private set; } = new List<string>();

        public List<Commitment> Commitments { get; set; } = new List<Commitment>();

        private IEnumerable<RawEarningEntity> Act1RawEarnings => RawEarnings.Where(x => x.ApprenticeshipContractType == 1);
        private IEnumerable<RawEarningEntity> Act2RawEarnings => RawEarnings.Where(x => x.ApprenticeshipContractType == 2);

        public List<FundingDue> PayableEarnings { get; set; }
        public List<NonPayableEarningEntity> NonPayableEarnings { get; set; } = new List<NonPayableEarningEntity>();

        public bool IgnoreForPayments { get; set; }

        public void ValidateDatalocks()
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
            
            // If there is a datalock then ignore this learner
            foreach (var rawEarningEntity in act1.Where(x => x.TransactionType01 != 0))
            {
                // Find a matching datalock
                var datalock = DataLocks.FirstOrDefault(x =>
                    x.PriceEpisodeIdentifier == rawEarningEntity.PriceEpisodeIdentifier &&
                    x.Period == rawEarningEntity.Period);

                if (datalock == null)
                {
                    IgnoreForPayments = true;
                    AddNonpayableFundingDue(rawEarningEntity, "No datalock found for ACT1 earning");
                    continue;
                }

                var commitment = Commitments.First(x => x.CommitmentId == datalock.CommitmentId);

                if (datalock.Payable == false)
                {
                    IgnoreForPayments = true;
                    AddNonpayableFundingDue(rawEarningEntity, "No datalock found for ACT1 earning", commitment);
                    continue;
                }
                AddFundingDue(rawEarningEntity, commitment);
            }
        }

        public void CalculateFundingDue()
        {
            
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
        
        private void AddFundingDue(RawEarningMathsEnglishEntity mathsAndEnglish)
        {
            foreach (var transactionType in RawMathsAndEnglishTransactionTypes)
            {
                var fundingDue = CreateBasicFundingDue(mathsAndEnglish);
                fundingDue.TransactionType = transactionType;
                // Doing this to prevent a huge switch statement
                fundingDue.AmountDue = (decimal) FundingDueAccessor[mathsAndEnglish, $"TransactionType{transactionType:D2}"];
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
            };
        }
    }
}