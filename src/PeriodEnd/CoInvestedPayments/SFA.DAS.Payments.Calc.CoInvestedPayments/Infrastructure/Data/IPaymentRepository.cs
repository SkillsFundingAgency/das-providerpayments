using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data
{
    public interface IPaymentRepository
    {
        void AddPayment(PaymentEntity payment);
        void AddPayments(PaymentEntity[] payments);
        PaymentHistoryEntity[] GetCoInvestedPaymentsHistory(int deliveryMonth, 
                                                            int deliveryYear, 
                                                            int transactionType,
                                                            int aimSequenceNumber,
                                                            long ukprn,
                                                            long uln,
                                                            int? FrameworkCode,
                                                            int? PathwayCode,
                                                            int? ProgrammeType,
                                                            long? StandardCode);

    }
}
