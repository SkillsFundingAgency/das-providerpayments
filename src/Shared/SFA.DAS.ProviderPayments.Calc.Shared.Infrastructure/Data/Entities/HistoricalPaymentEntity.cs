using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities
{
    public class HistoricalPaymentEntity : PaymentEntity
    {
        public HistoricalPaymentEntity()
        {}

        public HistoricalPaymentEntity(
            PaymentEntity payment,
            RequiredPaymentEntity refund)
            : base(payment)
        {
            AccountId = refund.AccountId;
            ApprenticeshipContractType = refund.ApprenticeshipContractType;
        }

        [Range(1, 10000)]
        public long AccountId { get; set; }

        [Range(1,2)]
        public int ApprenticeshipContractType { get; set; }

        [StringLength(100)]
        public string FundingLineType { get; set; }
    }
}
