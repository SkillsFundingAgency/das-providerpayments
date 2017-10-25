using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities
{
    public class PriceEpisodePeriodMatchEntity
    {
        public long Ukprn { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public string LearnRefNumber { get; set; }
        public long AimSeqNumber { get; set; }
        public long CommitmentId { get; set; }
        public string VersionId { get; set; }
        public int Period { get; set; }
        public bool Payable { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}