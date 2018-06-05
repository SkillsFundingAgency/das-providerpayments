using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class NonPayableEarningEntity : IFundingDue, IHoldCommitmentInformation
    {
        public long? CommitmentId { get; set; }
        [StringLength(25)]
        public string CommitmentVersionId { get; set; }
        public long? AccountId { get; set; }
        [StringLength(50)]
        public string AccountVersionId { get; set; }
        [NotMapped]
        public int Period { get; set; }
        public long Uln { get; set; }
        [StringLength(12)]
        public string LearnRefNumber { get; set; }
        public int AimSeqNumber { get; set; }
        public long Ukprn { get; set; }
        public DateTime IlrSubmissionDateTime { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public int TransactionType { get; set; }
        public decimal AmountDue { get; set; }

        public int StandardCode { get; set; }
        public int ProgrammeType { get; set; }
        public int FrameworkCode { get; set; }
        public int PathwayCode { get; set; }

        public int ApprenticeshipContractType { get; set; }
        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; set; }

        public decimal SfaContributionPercentage { get; set; }
        [StringLength(100)]
        public string FundingLineType { get; set; }
        public bool UseLevyBalance { get; set; }
        [StringLength(8)]
        public string LearnAimRef { get; set; }
        public DateTime LearningStartDate { get; set; }
        [StringLength(1000)]
        public string Reason { get; set; }
    }
}