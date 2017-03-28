using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Repositories
{
    public class DcfsPaymentRepository : DcfsRepository, IPaymentRepository
    {
        private const string PaymentsDestination = "ProviderAdjustments.Payments";

        public DcfsPaymentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddProviderAdjustments(PaymentEntity[] payments)
        {
            ExecuteBatch(payments, PaymentsDestination);
        }
    }
}