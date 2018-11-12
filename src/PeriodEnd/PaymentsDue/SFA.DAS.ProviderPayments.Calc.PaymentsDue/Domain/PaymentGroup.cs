using System;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    /// <summary>
    /// Value object for determining whether two payments should be
    ///     treated the same (ie aggregated) 
    ///     or differently (ie refunded and repaid)
    /// </summary>
    public sealed class PaymentGroup : BaseEquatableGroup<PaymentGroup>
    {
        public PaymentGroup(int standardCode,
            int frameworkCode,
            int programmeType,
            int pathwayCode,
            ApprenticeshipContractType apprenticeshipContractType,
            int transactionType,
            decimal sfaContributionPercentage,
            string learnAimRef,
            string fundingLineType,
            int deliveryYear,
            int deliveryMonth,
            long? accountId)
        {
            StandardCode = standardCode;
            FrameworkCode = frameworkCode;
            ProgrammeType = programmeType;
            PathwayCode = pathwayCode;
            ApprenticeshipContractType = apprenticeshipContractType;
            TransactionType = transactionType;
            SfaContributionPercentage = sfaContributionPercentage;
            LearnAimRef = learnAimRef;
            FundingLineType = fundingLineType;
            DeliveryMonth = deliveryMonth;
            DeliveryYear = deliveryYear;
            AccountId = accountId ?? -1;
        }

        public PaymentGroup(RequiredPayment copy)
        {
            StandardCode = copy.StandardCode;
            FrameworkCode = copy.FrameworkCode;
            ProgrammeType = copy.ProgrammeType;
            PathwayCode = copy.PathwayCode;
            ApprenticeshipContractType = copy.ApprenticeshipContractType;
            TransactionType = copy.TransactionType;
            SfaContributionPercentage = copy.SfaContributionPercentage;
            LearnAimRef = copy.LearnAimRef;
            FundingLineType = copy.FundingLineType;
            DeliveryMonth = copy.DeliveryMonth;
            DeliveryYear = copy.DeliveryYear;
            AccountId = copy.AccountId;
        }

        public int StandardCode { get; }
        public int FrameworkCode { get; }
        public int ProgrammeType { get; }
        public int PathwayCode { get; }
        public ApprenticeshipContractType ApprenticeshipContractType { get; }
        public int TransactionType { get; }
        public decimal SfaContributionPercentage { get; }
        public string LearnAimRef { get; }
        public string FundingLineType { get; }
        public int DeliveryYear { get; }
        public int DeliveryMonth { get; }
        public long AccountId { get; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = 31 * hash + StandardCode.GetHashCode();
                hash = 31 * hash + FrameworkCode.GetHashCode();
                hash = 31 * hash + ProgrammeType.GetHashCode();
                hash = 31 * hash + PathwayCode.GetHashCode();
                hash = 31 * hash + ApprenticeshipContractType.GetHashCode();
                hash = 31 * hash + TransactionType.GetHashCode();
                hash = 31 * hash + SfaContributionPercentage.GetHashCode();
                hash = 31 * hash + LearnAimRef.GetHashCode();
                hash = 31 * hash + FundingLineType.GetHashCode();
                hash = 31 * hash + DeliveryMonth.GetHashCode();
                hash = 31 * hash + DeliveryYear.GetHashCode();
                hash = 31 * hash + AccountId.GetHashCode();

                return hash;
            }
        }

       
        public override bool Equals(PaymentGroup test)
        {
            if (base.Equals(test))
                return true;

            if (test == null)
                return false;

            return StandardCode == test.StandardCode &&
                   FrameworkCode == test.FrameworkCode &&
                   ProgrammeType == test.ProgrammeType &&
                   PathwayCode == test.PathwayCode &&
                   ApprenticeshipContractType == test.ApprenticeshipContractType &&
                   TransactionType == test.TransactionType &&
                   SfaContributionPercentage == test.SfaContributionPercentage &&
                   LearnAimRef == test.LearnAimRef &&
                   FundingLineType == test.FundingLineType &&
                   DeliveryMonth == test.DeliveryMonth &&
                   DeliveryYear == test.DeliveryYear &&
                   AccountId == test.AccountId;
        }
    }
}