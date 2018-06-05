using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class RequiredPaymentsHistoryEntity
    {
        public Guid Id { get; set; }
        public long? CommitmentId { get; set; }
        [StringLength(25)]
        public string CommitmentVersionId { get; set; }
        public long? AccountId { get; set; }
        [StringLength(50)]
        public string AccountVersionId { get; set; }
        [StringLength(12)]
        public string LearnRefNumber { get; set; }
        public long Uln { get; set; }
        public int AimSeqNumber { get; set; }
        public long Ukprn { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        [StringLength(8)]
        public string CollectionPeriodName { get; set; }
        public int CollectionPeriodMonth { get; set; }
        public int CollectionPeriodYear { get; set; }
        public int TransactionType { get; set; }
        public decimal AmountDue { get; set; }
        public int StandardCode { get; set; }
        public int ProgrammeType { get; set; }
        public int FrameworkCode { get; set; }
        public int PathwayCode { get; set; }
        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; set; }
        [StringLength(8)]
        public string LearnAimRef { get; set; }
        public DateTime LearningStartDate { get; set; }
        public DateTime IlrSubmissionDateTime { get; set; }
        public int ApprenticeshipContractType { get; set; }
        public decimal SfaContributionPercentage { get; set; }
        public string FundingLineType { get; set; }
        public bool UseLevyBalance { get; set; }
    }
}