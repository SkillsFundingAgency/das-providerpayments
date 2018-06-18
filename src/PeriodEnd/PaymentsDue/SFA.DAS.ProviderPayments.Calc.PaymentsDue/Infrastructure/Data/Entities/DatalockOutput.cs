using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class DatalockOutput 
    {
        public long Ukprn { get; set; }
        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; set; }
        [StringLength(12)]
        public string LearnRefNumber { get; set; }
        public int AimSeqNumber { get; set; }
        public long CommitmentId { get; set; }
        [StringLength(25)]
        public string VersionId { get; set; }
        [Range(1, 12)]
        public int Period { get; set; }
        public bool Payable { get; set; }
        [Range(1, 15)]
        public int TransactionType { get; set; }
        [Range(1,3)]
        public int TransactionTypesFlag { get; set; }
    }
}