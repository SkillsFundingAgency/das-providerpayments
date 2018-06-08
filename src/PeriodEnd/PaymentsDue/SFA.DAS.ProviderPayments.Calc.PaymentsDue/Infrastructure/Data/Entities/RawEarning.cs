using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class RawEarning : IFundingDue
    {
        public long Ukprn { get; set; }
        [StringLength(12)]
        public string LearnRefNumber { get; set; }
        public long Uln { get; set; }
        [Range(0, 20)]
        public int AimSeqNumber { get; set; }
        [StringLength(8)]
        public string LearnAimRef { get; set; }
        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EpisodeStartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EpisodeEffectiveTnpStartDate { get; set; }
        [Range(1, 14)]
        public int Period { get; set; }
        [Range(0, 1000)]
        public int ProgrammeType { get; set; }
        [Range(0, 1000)]
        public int FrameworkCode { get; set; }
        [Range(0, 1000)]
        public int PathwayCode { get; set; }
        [Range(0, 1000)]
        public int StandardCode { get; set; }
        [Range(0.9, 1.0)]
        public decimal SfaContributionPercentage { get; set; }
        [StringLength(100)]
        public string FundingLineType { get; set; }
        [DataType(DataType.Date)]
        public DateTime LearningStartDate { get; set; }
        [Range(1, 2)]
        public int ApprenticeshipContractType { get; set; }

        public decimal TransactionType01 { get; set; }
        public decimal TransactionType02 { get; set; }
        public decimal TransactionType03 { get; set; }
        public decimal TransactionType04 { get; set; }
        public decimal TransactionType05 { get; set; }
        public decimal TransactionType06 { get; set; }
        public decimal TransactionType07 { get; set; }
        public decimal TransactionType08 { get; set; }
        public decimal TransactionType09 { get; set; }
        public decimal TransactionType10 { get; set; }
        public decimal TransactionType11 { get; set; }
        public decimal TransactionType12 { get; set; }
        public decimal TransactionType13 { get; set; }
        public decimal TransactionType14 { get; set; }
        public decimal TransactionType15 { get; set; }
    }
}