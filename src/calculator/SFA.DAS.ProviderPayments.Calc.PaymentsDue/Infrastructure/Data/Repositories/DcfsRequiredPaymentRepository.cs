using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class DcfsRequiredPaymentRepository : DcfsRepository, IRequiredPaymentRepository
    {
        private const string PaymentsDestination = "PaymentsDue.RequiredPayments";

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