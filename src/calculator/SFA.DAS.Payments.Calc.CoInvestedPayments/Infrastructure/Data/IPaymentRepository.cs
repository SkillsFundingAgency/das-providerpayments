using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data
{
    public interface IPaymentRepository
    {
        void AddPayment(PaymentEntity payment);
    }
}
