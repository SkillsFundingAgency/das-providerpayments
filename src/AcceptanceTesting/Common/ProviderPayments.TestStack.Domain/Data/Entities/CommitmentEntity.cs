using System;

namespace ProviderPayments.TestStack.Domain.Data.Entities
{
    public class CommitmentEntity
    {
        public long Id { get; set; }
        public long Uln { get; set; }
        public long Ukprn { get; set; }
        public string AccountId { get; set; }
        public long? StandardCode { get; set; }
        public int? PathwayCode { get; set; }
        public int? FrameworkCode { get; set; }
        public int? ProgrammeType { get; set; }
        public decimal Cost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public long Version { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentStatusDescription { get; set; }
        public DateTime EffectiveFrom { get; set; }
    }
}
