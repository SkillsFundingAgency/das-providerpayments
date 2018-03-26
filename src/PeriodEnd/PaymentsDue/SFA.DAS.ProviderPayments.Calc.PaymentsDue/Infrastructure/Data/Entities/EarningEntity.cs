using System;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class EarningEntity
    {
        public long? CommitmentId { get; set; }
        public string CommitmentVersionId { get; set; }
        public string AccountId { get; set; }
        public string AccountVersionId { get; set; }
        public string LearnerRefNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }
        public long Uln { get; set; }

        public int Period { get; set; }

        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }

        public int ApprenticeshipContractType { get; set; }
        public string ApprenticeshipContractTypeCode { get; set; }
        public DateTime? ApprenticeshipContractTypeStartDate { get; set; }
        public DateTime? ApprenticeshipContractTypeEndDate { get; set; }
        public string PriceEpisodeIdentifier { get; set; }

        public string PriceEpisodeFundLineType { get; set; }
        public decimal PriceEpisodeSfaContribPct { get; set; }
        public int? PriceEpisodeLevyNonPayInd { get; set; }

        public DateTime PriceEpisodeEndDate { get; set; }

        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
        public bool Payable { get; set; }
        public bool IsSuccess { get; set; }

        public string LearnAimRef { get; set; }
        public DateTime LearningStartDate { get; set; }

        public bool IsSmallEmployer { get; set; }
        
    }
}