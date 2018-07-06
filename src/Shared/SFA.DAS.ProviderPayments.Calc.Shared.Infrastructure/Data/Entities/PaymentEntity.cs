using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities
{
    public class PaymentEntity
    {
        public PaymentEntity()
        {}

        public PaymentEntity(PaymentEntity payment)
        {
            DeliveryMonth = payment.DeliveryMonth;
            DeliveryYear = payment.DeliveryYear;
            Amount = payment.Amount;
            RequiredPaymentId = payment.RequiredPaymentId;
            CollectionPeriodMonth = payment.CollectionPeriodMonth;
            CollectionPeriodYear = payment.CollectionPeriodYear;
            CollectionPeriodName = payment.CollectionPeriodName;
            FundingSource = payment.FundingSource;
            TransactionType = payment.TransactionType;
        }

        [Range(2017, 2020)]
        public int DeliveryYear { get; set; }

        [Range(1, 12)]
        public int DeliveryMonth { get; set; }

        [Range(0, 1999)]
        public decimal Amount { get; set; }

        public Guid RequiredPaymentId { get; set; }

        [StringLength(8)]
        public string CollectionPeriodName { get; set; }

        [Range(1, 12)]
        public int CollectionPeriodMonth { get; set; }

        [Range(2017, 2020)]
        public int CollectionPeriodYear { get; set; }

        public FundingSource FundingSource { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}