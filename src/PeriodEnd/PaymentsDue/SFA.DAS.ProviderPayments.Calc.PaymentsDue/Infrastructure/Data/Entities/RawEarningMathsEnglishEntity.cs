using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class RawEarningMathsEnglishEntity : IFundingDue
    {
        [StringLength(12)]
        public string LearnRefNumber { get; set; }
        public long Ukprn { get; set; }
        public int AimSeqNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime LearningStartDate { get; set; }
        public int Period { get; set; }
        public long Uln { get; set; }
        public int ProgrammeType { get; set; }
        public int FrameworkCode { get; set; }
        public int PathwayCode { get; set; }
        public int StandardCode { get; set; }
        public decimal SfaContributionPercentage { get; set; }
        [StringLength(100)]
        public string FundingLineType { get; set; }
        [StringLength(8)]
        public string LearnAimRef { get; set; }
        public int ApprenticeshipContractType { get; set; }
        public decimal TransactionType13 { get; set; }
        public decimal TransactionType14 { get; set; }
        public decimal TransactionType15 { get; set; }
    }
}