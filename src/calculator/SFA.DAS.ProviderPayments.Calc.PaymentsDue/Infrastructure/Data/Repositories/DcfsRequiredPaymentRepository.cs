using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class DcfsRequiredPaymentRepository : DcfsRepository, IRequiredPaymentRepository
    {
        private const string PaymentsDestination = "PaymentsDue.RequiredPayments";

        private const string PaymentHistorySource = "PaymentsDue.vw_PaymentHistory";
        private const string PaymentHistoryColumns = "CommitmentId, "
                                                   //+ "CommitmentVersionId, "
                                                   //+ "AccountId, "
                                                   //+ "AccountVersionId, "
                                                   //+ "Uln, "
                                                   + "LearnRefNumber, "
                                                   + "AimSeqNumber, "
                                                   + "Ukprn, "
                                                   + "DeliveryMonth, "
                                                   + "DeliveryYear, "
                                                   + "AmountDue, "
                                                   + "TransactionType";
        private const string SelectPaymentsForCommitment = "SELECT " + PaymentHistoryColumns + " FROM " + PaymentHistorySource
                                                         + " WHERE CommitmentId = @commitmentId AND Ukprn = @ukprn";


        public DcfsRequiredPaymentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddRequiredPayments(RequiredPaymentEntity[] payments)
        {
            ExecuteBatch(payments, PaymentsDestination);
        }

        public RequiredPaymentEntity[] GetPreviousPaymentsForCommitment(long ukprn, long commitmentId)
        {
            return Query<RequiredPaymentEntity>(SelectPaymentsForCommitment, new {ukprn, commitmentId});
        }
    }
}