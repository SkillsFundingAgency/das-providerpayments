using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Repositories
{
    public class DcfsPaymentRepository : DcfsRepository, IPaymentRepository
    {
        private const string AddPaymentCommand = "LevyPayments.AddPayment @CommitmentId, @LearnRefNumber, @AimSeqNumber, @Ukprn, @DeliveryMonth, @DeliveryYear, @CollectionPeriodMonth, @CollectionPeriodYear, @Source, @TransactionType, @Amount";

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
