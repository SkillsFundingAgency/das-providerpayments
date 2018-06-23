using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class FundingDue : RequiredPaymentEntity, ICanStoreCommitmentInformation, IHoldCommitmentInformation
    {
        public FundingDue()
        {
        }

        public FundingDue(RawEarning rawEarning)
        {
            Uln = rawEarning.Uln;
            LearnRefNumber = rawEarning.LearnRefNumber;
            AimSeqNumber = rawEarning.AimSeqNumber;
            Ukprn = rawEarning.Ukprn;
            StandardCode = rawEarning.StandardCode;
            ProgrammeType = rawEarning.ProgrammeType;
            FrameworkCode = rawEarning.FrameworkCode;
            PathwayCode = rawEarning.PathwayCode;
            ApprenticeshipContractType = rawEarning.ApprenticeshipContractType;
            PriceEpisodeIdentifier = rawEarning.PriceEpisodeIdentifier;
            SfaContributionPercentage = rawEarning.SfaContributionPercentage;
            FundingLineType = rawEarning.FundingLineType;
            LearnAimRef = rawEarning.LearnAimRef;
            LearningStartDate = rawEarning.LearningStartDate;
            DeliveryMonth = rawEarning.DeliveryMonth;
            DeliveryYear = rawEarning.DeliveryYear;
            UseLevyBalance = rawEarning.UseLevyBalance;
        }

        public FundingDue(RequiredPaymentEntity requiredPaymentsHistoryEntity)
        {
            CommitmentId = requiredPaymentsHistoryEntity.CommitmentId;
            CommitmentVersionId = requiredPaymentsHistoryEntity.CommitmentVersionId;
            AccountId = requiredPaymentsHistoryEntity.AccountId;
            AccountVersionId = requiredPaymentsHistoryEntity.AccountVersionId;
            Uln = requiredPaymentsHistoryEntity.Uln;
            LearnRefNumber = requiredPaymentsHistoryEntity.LearnRefNumber;
            AimSeqNumber = requiredPaymentsHistoryEntity.AimSeqNumber;
            Ukprn = requiredPaymentsHistoryEntity.Ukprn;
            IlrSubmissionDateTime = requiredPaymentsHistoryEntity.IlrSubmissionDateTime;
            DeliveryMonth = requiredPaymentsHistoryEntity.DeliveryMonth;
            DeliveryYear = requiredPaymentsHistoryEntity.DeliveryYear;
            TransactionType = requiredPaymentsHistoryEntity.TransactionType;
            AmountDue = requiredPaymentsHistoryEntity.AmountDue;
            StandardCode = requiredPaymentsHistoryEntity.StandardCode;
            ProgrammeType = requiredPaymentsHistoryEntity.ProgrammeType;
            FrameworkCode = requiredPaymentsHistoryEntity.FrameworkCode;
            PathwayCode = requiredPaymentsHistoryEntity.PathwayCode;
            ApprenticeshipContractType = requiredPaymentsHistoryEntity.ApprenticeshipContractType;
            PriceEpisodeIdentifier = requiredPaymentsHistoryEntity.PriceEpisodeIdentifier;
            SfaContributionPercentage = requiredPaymentsHistoryEntity.SfaContributionPercentage;
            FundingLineType = requiredPaymentsHistoryEntity.FundingLineType;
            UseLevyBalance = requiredPaymentsHistoryEntity.UseLevyBalance;
            LearnAimRef = requiredPaymentsHistoryEntity.LearnAimRef;
            LearningStartDate = requiredPaymentsHistoryEntity.LearningStartDate;
        }
    }
}
