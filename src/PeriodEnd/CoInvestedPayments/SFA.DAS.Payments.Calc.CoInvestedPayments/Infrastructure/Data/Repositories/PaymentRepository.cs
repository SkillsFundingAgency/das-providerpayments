using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Repositories
{
    public class PaymentRepository : DcfsRepository, IPaymentRepository
    {
        private const string PaymentsDestination = "CoInvestedPayments.Payments";
        private const string PaymentsAddStoredProcedure = "CoInvestedPayments.AddPayment";
        private const string AddPaymentCommand = PaymentsAddStoredProcedure + " @RequiredPaymentId, @DeliveryMonth, @DeliveryYear, @CollectionPeriodMonth, @CollectionPeriodYear, @FundingSource, @TransactionType, @Amount, @CollectionPeriodName";

        public PaymentRepository(string connectionString) 
            : base(connectionString)
        {
        }
        public void AddPayments(PaymentEntity[] payments)
        {
            ExecuteBatch(payments, PaymentsDestination);
        }

        public void AddPayment(PaymentEntity payment)
        {
            Execute(AddPaymentCommand, new
            {
                RequiredPaymentId = payment.RequiredPaymentId,
                DeliveryMonth = payment.DeliveryMonth,
                DeliveryYear = payment.DeliveryYear,
                CollectionPeriodMonth = payment.CollectionPeriodMonth,
                CollectionPeriodYear = payment.CollectionPeriodYear,
                FundingSource = payment.FundingSource,
                TransactionType = payment.TransactionType,
                Amount = payment.Amount,
                CollectionPeriodName = payment.CollectionPeriodName
            });
        }
    }
}
