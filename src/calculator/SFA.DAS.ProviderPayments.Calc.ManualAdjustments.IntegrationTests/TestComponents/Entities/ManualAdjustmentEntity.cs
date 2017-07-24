using System;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.IntegrationTests.TestComponents.Entities
{
    internal class ManualAdjustmentEntity
    {
        public Guid RequiredPaymentIdToReverse { get; set; }
        public string ReasonForReversal { get; set; }
        public string RequestorName { get; set; }
        public DateTime DateUploaded { get; set; }
        public Guid? RequiredPaymentIdForReversal { get; set; }
    }
}
