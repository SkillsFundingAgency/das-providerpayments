using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Entities
{
    public class HistoricalPaymentEntity
    {
        public Guid PaymentId { get; set; }

        public Guid RequiredPaymentId { get; set; }

        [Range(1, 12)]
        public int DeliveryMonth { get; set; }

        [Range(1, 12)]
        public int DeliveryYear { get; set; }

        [StringLength(8)]
        public string CollectionPeriodName { get; set; }

        public int CollectionPeriodMonth { get; set; }

        public int CollectionPeriodYear { get; set; }

        public int FundingSource { get; set; }

        public int TransactionType { get; set; }

        public decimal? Amount { get; set; }

        public int? ApprenticeshipContractType { get; set; }

        public long? Ukprn { get; set; }

        public long? AccountId { get; set; }

        [StringLength(12)]
        public string LearnRefNumber { get; set; }

        [StringLength(100)]
        public string FundingLineType { get; set; }
    }
}