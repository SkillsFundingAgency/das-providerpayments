using System;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class EarningRelatedToPaymentDueEntity
    {
        public Guid RequiredPaymentId { get; set; }
        public decimal MonthlyInstallmentAmount { get; set; }
        public decimal CompletionAmount { get; set; }
        public int NumberOfInstallments { get; set; }
    }
}
