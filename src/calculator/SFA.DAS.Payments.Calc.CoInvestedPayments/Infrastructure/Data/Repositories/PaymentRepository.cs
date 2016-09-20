using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Repositories
{
    public class PaymentRepository : DcfsRepository, IPaymentRepository
    {
        private const string AddPaymentCommand = "LevyPayments.AddPayment @CommitmentId, @LearnRefNumber, @AimSeqNumber, @Ukprn, @DeliveryMonth, @DeliveryYear, @CollectionPeriodMonth, @CollectionPeriodYear, @Source, @TransactionType, @Amount";

        public PaymentRepository(string connectionString) 
            : base(connectionString)
        {
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
                Source = payment.Source,
                TransactionType = payment.TransactionType,
                Amount = payment.Amount
            });
        }
    }
}
