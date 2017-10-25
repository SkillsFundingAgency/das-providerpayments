namespace SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodeMatch
{
    public class PriceEpisodeMatch
    {
        public long Ukprn { get; set; }
        public string LearnerReferenceNumber { get; set; }
        public long AimSequenceNumber { get; set; }
        public long CommitmentId { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public bool IsSuccess { get; set; }
    }
}