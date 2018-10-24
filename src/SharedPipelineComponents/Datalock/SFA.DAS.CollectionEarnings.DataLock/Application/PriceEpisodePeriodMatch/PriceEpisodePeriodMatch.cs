using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch
{
    public class PriceEpisodePeriodMatch
    {
        public long Ukprn { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public string LearnerReferenceNumber { get; set; }
        public long AimSequenceNumber { get; set; }
        public long CommitmentId { get; set; }
        public string CommitmentVersionId { get; set; }
        public int Period { get; set; }
        public bool Payable { get; set; }
        public TransactionType TransactionType { get; set; }

        public CensusDateType TransactionTypesFlag { get; set; }

    }
}