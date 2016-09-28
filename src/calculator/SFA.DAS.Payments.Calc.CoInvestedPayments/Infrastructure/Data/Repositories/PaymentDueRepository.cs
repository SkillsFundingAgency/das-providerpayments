using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Repositories
{
    public class PaymentDueRepository : DcfsRepository, IPaymentDueRepository
    {
        private const string DuePaymentsSource = "CoInvestedPayments.vw_RequiredPayments";

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
        private const string SelectDuePaymentsByUkprn = SelectDuePayments + " WHERE Ukprn = @Ukprn";

        public PaymentDueRepository(string connectionString)
            : base(connectionString)
        {
        }

        public PaymentDueEntity[] GetPaymentsDueByUkprn(long ukprn)
        {
            return Query<PaymentDueEntity>(SelectDuePaymentsByUkprn, new { ukprn });
        }
    }
}