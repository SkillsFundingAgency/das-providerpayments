using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities
{
    public class RequiredPaymentEntity
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public long? CommitmentId { get; set; }

        [StringLength(25)]
        public string CommitmentVersionId { get; set; }

        public long? AccountId { get; set; }

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
        public int? StandardCode { get; set; }

        [Range(0, 1000)]
        public int? ProgrammeType { get; set; }

        [Range(0, 1000)]
        public int? FrameworkCode { get; set; }

        [Range(0, 1000)]
        public int? PathwayCode { get; set; }

        [Range(1, 2)]
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
    }
}