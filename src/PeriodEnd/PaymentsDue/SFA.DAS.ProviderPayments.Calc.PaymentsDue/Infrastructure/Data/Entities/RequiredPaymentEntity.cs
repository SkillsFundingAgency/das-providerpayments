using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class RequiredPaymentEntity
    {
        public RequiredPaymentEntity()
        {}

        public RequiredPaymentEntity(RequiredPaymentEntity copy)
        {
            CommitmentId = copy.CommitmentId;
            CommitmentVersionId = copy.CommitmentVersionId;
            AccountId = copy.AccountId;
            AccountVersionId = copy.AccountVersionId;
            Uln = copy.Uln;
            LearnAimRef = copy.LearnAimRef;
            LearnRefNumber = copy.LearnRefNumber;
            AimSeqNumber = copy.AimSeqNumber;
            Ukprn = copy.Ukprn;
            IlrSubmissionDateTime = copy.IlrSubmissionDateTime;
            DeliveryMonth = copy.DeliveryMonth;
            DeliveryYear = copy.DeliveryYear;
            TransactionType = copy.TransactionType;
            AmountDue = copy.AmountDue;
            CollectionPeriodYear = copy.CollectionPeriodYear;
            CollectionPeriodMonth = copy.CollectionPeriodMonth;
            CollectionPeriodName = copy.CollectionPeriodName;
            StandardCode = copy.StandardCode;
            ProgrammeType = copy.ProgrammeType;
            FrameworkCode = copy.FrameworkCode;
            PathwayCode = copy.PathwayCode;
            ApprenticeshipContractType = copy.ApprenticeshipContractType;
            PriceEpisodeIdentifier = copy.PriceEpisodeIdentifier;
            SfaContributionPercentage = copy.SfaContributionPercentage;
            FundingLineType = copy.FundingLineType;
            UseLevyBalance = copy.UseLevyBalance;
            LearningStartDate = copy.LearningStartDate;
            Period = copy.Period;
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public long CommitmentId { get; set; }

        [StringLength(25)]
        public string CommitmentVersionId { get; set; }

        public long AccountId { get; set; }

        [StringLength(50)]
        public string AccountVersionId { get; set; }

        public long Uln { get; set; }

        [StringLength(12)]
        public string LearnRefNumber { get; set; }

        [Range(0, 20)]
        public int AimSeqNumber { get; set; }

        public long Ukprn { get; set; }

        public DateTime IlrSubmissionDateTime { get; set; }

        [Range(1, 12)]
        public int DeliveryMonth { get; set; }

        [Range(2017, 2019)]
        public int DeliveryYear { get; set; }

        [Range(1, 15)]
        public int TransactionType { get; set; }

        public decimal AmountDue { get; set; }

        [Range(0, 1000)]
        public int StandardCode { get; set; }

        [Range(0, 1000)]
        public int ProgrammeType { get; set; }

        [Range(0, 1000)]
        public int FrameworkCode { get; set; }

        [Range(0, 1000)]
        public int PathwayCode { get; set; }

        [Range(1,2)]
        public int ApprenticeshipContractType { get; set; }

        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; set; }

        [Range(0.9, 1.0)]
        public decimal SfaContributionPercentage { get; set; }

        [StringLength(100)]
        public string FundingLineType { get; set; }

        public bool UseLevyBalance { get; set; }

        [StringLength(8)]
        public string LearnAimRef { get; set; }

        public DateTime LearningStartDate { get; set; }

        [StringLength(8)]
        public string CollectionPeriodName { get; set; }

        [Range(1, 12)]
        public int CollectionPeriodMonth { get; set; }

        [Range(2017, 2019)]
        public int CollectionPeriodYear { get; set; }

        [NotMapped]
        [Range(1, 12)]
        public int Period { get; set; }
    }
}