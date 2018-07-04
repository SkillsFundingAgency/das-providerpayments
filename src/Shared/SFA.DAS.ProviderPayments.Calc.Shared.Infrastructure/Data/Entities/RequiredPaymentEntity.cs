using System;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities
{
    public class RequiredPaymentEntity
    {
        public Guid Id { get; set; }
        public string CollectionPeriodName { get; set; }
        public int CollectionPeriodMonth { get; set; }
        public int CollectionPeriodYear { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal AmountDue { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
    }
}