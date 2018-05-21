using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class RawEarningMathsEnglishEntity
    {
        [StringLength(12)]
        public string LearnRefNumber { get; set; }
        public long Ukprn { get; set; }
        public int AimSeqNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime LearnStartDate { get; set; }
        public int Period { get; set; }
        public long Uln { get; set; }
        public int ProgType { get; set; }
        public int FworkCode { get; set; }
        public int PwayCode { get; set; }
        public int StdCode { get; set; }
        public decimal LearnDelSfaContribPct { get; set; }
        [StringLength(100)]
        public string FundLineType { get; set; }
        [StringLength(8)]
        public string LearnAimRef { get; set; }
        public decimal TransactionType13 { get; set; }
        public decimal TransactionType14 { get; set; }
        public decimal TransactionType15 { get; set; }
        public short Act { get; set; }
    }
}