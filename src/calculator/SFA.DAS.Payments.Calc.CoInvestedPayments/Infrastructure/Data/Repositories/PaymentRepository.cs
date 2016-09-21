using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Repositories
{
    public class PaymentRepository : DcfsRepository, IPaymentRepository
    {
        private const string PaymentsDestination = "CoInvestedPayments.AddPayment";
        private const string AddPaymentCommand = PaymentsDestination + " @CommitmentId, @LearnRefNumber, @AimSeqNumber, @Ukprn, @DeliveryMonth, @DeliveryYear, @CollectionPeriodMonth, @CollectionPeriodYear, @FundingSource, @TransactionType, @Amount";

        public PaymentRepository(string connectionString) 
            : base(connectionString)
        {
        }
        public void AddPayments(PaymentEntity[] payments)
        {
            ExecuteBatch(payments, AddPaymentCommand);
        }

        public void AddPayment(PaymentEntity payment)
        {
            Execute(AddPaymentCommand, new
            {
                CommitmentId = payment.CommitmentId,
                LearnRefNumber = payment.LearnerRefNumber,
                AimSeqNumber = payment.AimSequenceNumber,
                Ukprn = payment.Ukprn,
                DeliveryMonth = payment.DeliveryMonth,
                DeliveryYear = payment.DeliveryYear,
                CollectionPeriodMonth = payment.CollectionPeriodMonth,
                CollectionPeriodYear = payment.CollectionPeriodYear,
                FundingSource = payment.FundingSource,
                TransactionType = payment.TransactionType,
                Amount = payment.Amount
            });
        }
    }
}
