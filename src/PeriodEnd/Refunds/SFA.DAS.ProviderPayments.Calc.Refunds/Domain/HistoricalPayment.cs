using System.ComponentModel.DataAnnotations;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Domain
{
    public class HistoricalPayment : PaymentEntity
    {
        public HistoricalPayment(HistoricalPaymentEntity historicalPaymentEntity)
        {
            DeliveryMonth = historicalPaymentEntity.DeliveryMonth;
            DeliveryYear = historicalPaymentEntity.DeliveryYear;
            Amount = historicalPaymentEntity.Amount ?? 0m;
            RequiredPaymentId = historicalPaymentEntity.RequiredPaymentId;
            CollectionPeriodMonth = historicalPaymentEntity.CollectionPeriodMonth;
            CollectionPeriodYear = historicalPaymentEntity.CollectionPeriodYear;
            CollectionPeriodName = historicalPaymentEntity.CollectionPeriodName;
            FundingSource = (FundingSource)historicalPaymentEntity.FundingSource;
            TransactionType = (TransactionType)historicalPaymentEntity.TransactionType;
            AccountId = historicalPaymentEntity.AccountId ?? 0;
            ApprenticeshipContractType = historicalPaymentEntity.ApprenticeshipContractType ?? 0;
            FundingLineType = historicalPaymentEntity.FundingLineType;
        }

        public HistoricalPayment(
            PaymentEntity payment,
            RequiredPaymentEntity refund)
            : base(payment)
        {
            AccountId = refund.AccountId;
            ApprenticeshipContractType = refund.ApprenticeshipContractType;
            FundingLineType = refund.FundingLineType;
        }

        [Range(1, 1000)]
        public long AccountId { get; set; }

        [Range(1, 2)]
        public int ApprenticeshipContractType { get; set; }

        [StringLength(100)]
        public string FundingLineType { get; set; }
    }
}
