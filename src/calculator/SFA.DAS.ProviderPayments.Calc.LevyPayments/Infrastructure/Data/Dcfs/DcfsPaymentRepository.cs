using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Dcfs
{
    public class DcfsPaymentRepository : DcfsRepository, IPaymentRepository
    {
        private const string AddPaymentCommand = "LevyPayments.AddPayment @CommitmentId, @LearnRefNumber, @AimSeqNumber, @Ukprn, @Source, @TransactionType, @Amount";

        public DcfsPaymentRepository(string connectionString) 
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
                Source = payment.Source,
                TransactionType = payment.TransactionType,
                Amount = payment.Amount
            });
        }
    }
}
