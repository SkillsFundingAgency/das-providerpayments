﻿using System;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class FundingDue : RequiredPaymentEntity, IFundingDue, IHoldCommitmentInformation
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

        public int Period { get; set; }
    }

    public interface IFundingDue
    {
        string LearnRefNumber { get; set; }
        long Ukprn { get; set; }
        int AimSeqNumber { get; set; }
        DateTime LearningStartDate { get; set; }
        int Period { get; set; }
        long Uln { get; set; }
        int ProgrammeType { get; set; }
        int FrameworkCode { get; set; }
        int PathwayCode { get; set; }
        int StandardCode { get; set; }
        decimal SfaContributionPercentage { get; set; }
        string FundingLineType { get; set; }
        string LearnAimRef { get; set; }
        int ApprenticeshipContractType { get; set; }
    }
}