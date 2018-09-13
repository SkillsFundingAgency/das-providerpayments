using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities
{
    public class PriceEpisodeMatchEntity
    {
        public long Ukprn { get; set; }
        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; set; }
        [StringLength(12)]
        public string LearnRefNumber { get; set; }
        public long AimSeqNumber { get; set; }
        public long CommitmentId { get; set; }
        public bool IsSuccess { get; set; }
    }
}