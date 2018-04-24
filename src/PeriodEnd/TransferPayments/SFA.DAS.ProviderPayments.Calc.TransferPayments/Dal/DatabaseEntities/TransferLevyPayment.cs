using System;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities
{
    public class TransferLevyPayment
    {
        public TransferLevyPayment(RequiredTransferPayment requiredPayment, decimal amount)
        {
            PaymentId = Guid.NewGuid();
            RequiredPaymentId = requiredPayment.RequiredPaymentId;
            DeliveryMonth = requiredPayment.DeliveryMonth;
            DeliveryYear = requiredPayment.DeliveryYear;
            CollectionPeriodName = requiredPayment.CollectionPeriodName;
            CollectionPeriodMonth = requiredPayment.CollectionPeriodMonth;
            CollectionPeriodYear = requiredPayment.CollectionPeriodYear;
            TransactionType = requiredPayment.TransactionType;
            Amount = amount;
        }

        public Guid PaymentId { get; set; }
        public Guid RequiredPaymentId { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public string CollectionPeriodName { get; set; }
        public int CollectionPeriodMonth { get; set; }
        public int CollectionPeriodYear { get; set; }
        public FundingSource FundingSource { get; set; } = FundingSource.Transfer;
        public TransactionType TransactionType { get; set; } 
        public decimal Amount { get; set; }
    }
}
