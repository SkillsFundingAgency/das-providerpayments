using System;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisode
{
    public class PriceEpisode
    {
        public long Ukprn { get; set; }
        public string LearnerReferenceNumber { get; set; }
        public long? Uln { get; set; }
        public string NiNumber { get; set; }
        public long? AimSequenceNumber { get; set; }
        public long? StandardCode { get; set; }
        public long? ProgrammeType { get; set; }
        public long? FrameworkCode { get; set; }
        public long? PathwayCode { get; set; }
        public long? NegotiatedPrice { get; set; }
        public DateTime StartDate { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public DateTime EndDate { get; set; }
         public DateTime? FirstAdditionalPaymentThresholdDate { get; set; }
        public DateTime? SecondAdditionalPaymentThresholdDate { get; set; }
    }
}