using System.ComponentModel.DataAnnotations;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Common.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities
{
    public class PriceEpisodePeriodMatchEntity
    {
        public long Ukprn { get; set; }
        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; set; }
        [StringLength(12)]
        public string LearnRefNumber { get; set; }
        public long AimSeqNumber { get; set; }
        public long CommitmentId { get; set; }
        [StringLength(25)]
        public string VersionId { get; set; }
        [Range(1, 12)]
        public int Period { get; set; }
        public bool Payable { get; set; }
        public TransactionType TransactionType { get; set; }
        public CensusDateType TransactionTypesFlag { get; set; }
    }
}