using System;

namespace SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels.ProviderAdjustments
{
    public class EasSubmissionValues
    {
        public Guid SubmissionId { get; set; }
        public int CollectionPeriod { get; set; }
        public int PaymentId { get; set; }
        public decimal PaymentValue { get; set; }
    }
}
