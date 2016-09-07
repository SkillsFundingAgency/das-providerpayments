using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Dcfs
{
    public class DcfsPaymentRepository : DcfsRepository, IPaymentRepository
    {
        private const string AddPaymentCommand = "LevyPayments.AddPayment @CommitmentId, @LearnRefNumber, @AimSeqNumber, @Ukprn, @Source, @Amount";

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
                Amount = payment.Amount
            });
        }
    }
}
