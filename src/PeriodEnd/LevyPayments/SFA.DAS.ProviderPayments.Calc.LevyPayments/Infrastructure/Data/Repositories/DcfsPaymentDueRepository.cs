using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Repositories
{
    public class DcfsPaymentDueRepository : DcfsRepository, IPaymentDueRepository
    {
        private const string DuePaymentsSource = "LevyPayments.vw_PaymentsDue";

        private const string DuePaymentsColumns = "Id," +
                                                  "CommitmentId," +
                                                  "LearnRefNumber," +
                                                  "AimSeqNumber," +
                                                  "Ukprn," +
                                                  "DeliveryMonth," +
                                                  "DeliveryYear," +
                                                  "TransactionType," +
                                                  "AmountDue";
        private const string SelectDuePayments = "SELECT " + DuePaymentsColumns + " FROM " + DuePaymentsSource;
        private const string OrderByCaluseDuePaymentsForCommitment = " ORDER BY DeliveryYear, DeliveryMonth ASC";
        private const string WhereClausePaymentsForCommitment = " WHERE CommitmentId = @CommitmentId  " + 
                                                                " AND ((@refundPayments = 1 AND AmountDue <0) OR (@refundPayments = 0 AND AmountDue > 0) )";

        private const string SelectDuePaymentsForCommitment = SelectDuePayments + WhereClausePaymentsForCommitment + OrderByCaluseDuePaymentsForCommitment;

        public DcfsPaymentDueRepository(string connectionString)
            : base(connectionString)
        {
        }

        public PaymentDueEntity[] GetPaymentsDueForCommitment(long commitmentId,bool refundPayments)
        {
            return Query<PaymentDueEntity>(SelectDuePaymentsForCommitment, new {commitmentId, refundPayments });
        }
    }
}