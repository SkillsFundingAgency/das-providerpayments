namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities
{
    public class PriceEpisodeMatchEntity
    {
        public long Ukprn { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public string LearnRefNumber { get; set; }
        public long AimSeqNumber { get; set; }
        public long CommitmentId { get; set; }
        public bool IsSuccess { get; set; }

    }
}