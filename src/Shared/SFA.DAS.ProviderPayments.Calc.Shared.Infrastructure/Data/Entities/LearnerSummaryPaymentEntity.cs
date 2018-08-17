using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities
{
    public class LearnerSummaryPaymentEntity
    {
        public LearnerSummaryPaymentEntity()
        { }

        public LearnerSummaryPaymentEntity(LearnerSummaryPaymentEntity payment)
        {
            LearnRefNumber = payment.LearnRefNumber;
            Amount = payment.Amount;
        }

        [StringLength(12)]
        public string LearnRefNumber { get; set; }

        public decimal Amount { get; set; }

    }
}