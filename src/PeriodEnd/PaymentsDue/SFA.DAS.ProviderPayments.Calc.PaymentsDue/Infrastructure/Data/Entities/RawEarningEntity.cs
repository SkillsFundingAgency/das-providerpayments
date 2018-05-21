using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class RawEarningEntity
    {
        [StringLength(12)]
        public string LearnRefNumber { get; set; }
        public long Ukprn { get; set; }
        public int PriceEpisodeAimSeqNumber { get; set; }
        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; set; }
        [DataType(DataType.Date)]
        public DateTime EpisodeStartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EpisodeEffectiveTnpStartDate { get; set; }
        public int Period { get; set; }
        public long Uln { get; set; }
        public int ProgType { get; set; }
        public int FworkCode { get; set; }
        public int PwayCode { get; set; }
        public int StdCode { get; set; }
        public decimal PriceEpisodeSfaContribPct { get; set; }
        [StringLength(100)]
        public string PriceEpisodeFundLineType { get; set; }
        [StringLength(8)]
        public string LearnAimRef { get; set; }
        [DataType(DataType.Date)]
        public DateTime LearnStartDate { get; set; }
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
        public decimal TransactionType15 { get; set; }
        public short Act { get; set; }
    }
}