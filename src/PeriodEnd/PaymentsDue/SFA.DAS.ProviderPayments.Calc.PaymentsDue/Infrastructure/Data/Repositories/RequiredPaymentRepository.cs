using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class RequiredPaymentRepository : DcfsRepository, IRequiredPaymentRepository
    {
        private const string PaymentsDestination = "PaymentsDue.RequiredPayments";

        public RequiredPaymentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddRequiredPayments(List<RequiredPayment> payments)
        {
            var paymentsEntities = payments.Select(x => new RequiredPaymentEntity(x));
            ExecuteBatch(paymentsEntities.ToArray(), PaymentsDestination);
        }
    }
}