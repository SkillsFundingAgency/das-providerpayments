using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities
{
    public class RequiredPaymentEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string CollectionPeriodName { get; set; }

        [Range(1, 12)]
        public int CollectionPeriodMonth { get; set; }

        [Range(2017, 2020)]
        public int CollectionPeriodYear { get; set; }

        public TransactionType TransactionType { get; set; }

        [Range(-1999, 0)]
        public decimal AmountDue { get; set; }

        [Range(1, 12)]
        public int DeliveryMonth { get; set; }

        [Range(2017, 2020)]
        public int DeliveryYear { get; set; }

        [Range(1, 10000)]
        public long AccountId { get; set; }

        [Range(1, 2)]
        public int ApprenticeshipContractType { get; set; }
    }
}