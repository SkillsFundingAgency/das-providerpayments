using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Repositories
{
    public class PaymentDueRepository : DcfsRepository, IPaymentDueRepository
    {
        private const string DuePaymentsSource = "CoInvestedPayments.vw_RequiredPayments";

        private const string DuePaymentsColumns = "Id," +
                                                    "Ukprn," +
                                                    "DeliveryMonth," +
                                                    "DeliveryYear," +
                                                    "TransactionType," +
                                                    "AmountDue," +
                                                    "SfaContributionPercentage," +
                                                    "AimSequenceNumber," + 
                                                    "FrameworkCode," +
                                                    "PathwayCode," +
                                                    "ProgrammeType," +
                                                    "StandardCode," +
                                                    "Uln" ;
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