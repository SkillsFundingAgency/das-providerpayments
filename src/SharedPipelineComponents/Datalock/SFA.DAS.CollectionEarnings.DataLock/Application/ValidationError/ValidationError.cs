namespace SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError
{
    public class ValidationError
    {
        public long Ukprn { get; set; }
        public string LearnerReferenceNumber { get; set; }
        public long? AimSequenceNumber { get; set; }
        public string RuleId { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
    }
}