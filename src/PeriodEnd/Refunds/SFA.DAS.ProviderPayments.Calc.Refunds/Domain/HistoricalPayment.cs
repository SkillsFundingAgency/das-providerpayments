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
        }

        public HistoricalPayment(
            PaymentEntity payment,
            RequiredPaymentEntity refund)
            : base(payment)
        {
            AccountId = refund.AccountId;
            ApprenticeshipContractType = refund.ApprenticeshipContractType;
        }

        public long AccountId { get; set; }

        public int ApprenticeshipContractType { get; set; }
    }
}
