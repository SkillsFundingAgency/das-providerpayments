using System;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities
{
    public class AdjustmentEntity
    {
        public long Ukprn { get; set; }
        public Guid SubmissionId { get; set; }
        public int SubmissionCollectionPeriod { get; set; }
        public int PaymentType { get; set; }
        public string PaymentTypeName { get; set; }
        public decimal Amount { get; set; }
    }
}