using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data
{
    public interface IPaymentRepository
    {
        void AddPayment(PaymentEntity payment);
        PaymentHistoryEntity[] GetLevyPaymentsHistory(int deliveryMonth, int deliveryYear, int transactionType, long commitmentId);
    }
}
