using System;

namespace ProviderPayments.TestStack.Domain
{
    public class Commitment
    {
        public string Id { get; set; }
        public Learner Learner { get; set; }
        public Provider Provider { get; set; }
        public Account Account { get; set; }
        public Course Course { get; set; }
        public decimal Cost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }

        public PaymentStatus Status { get; set; }
        public long Version { get; set; }
        public DateTime EffectiveFrom { get; set; }
    }
}
