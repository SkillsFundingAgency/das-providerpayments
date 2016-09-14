using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data.Dcfs
{
    public class DcfsRequiredPaymentRepository : DcfsRepository, IRequiredPaymentRepository
    {
        private const string PaymentsDestination = "PaymentSchedule.RequiredPayments";

        public DcfsRequiredPaymentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddRequiredPayments(RequiredPaymentEntity[] payments)
        {
            ExecuteBatch(payments, PaymentsDestination);
        }
    }
}