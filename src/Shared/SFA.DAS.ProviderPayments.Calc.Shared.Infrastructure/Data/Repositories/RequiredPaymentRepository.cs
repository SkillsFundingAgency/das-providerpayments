using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories
{
    public class RequiredPaymentRepository : DcfsRepository, IRequiredPaymentRepository
    {
        public RequiredPaymentRepository(string connectionString)
            : base(connectionString)
        {
        }
        public IEnumerable<RequiredPaymentEntity> GetRefundsForProvider(long ukprn)
        {
            const string sql = @"
                SELECT * 
                FROM PaymentsDue.RequiredPayments 
                WHERE Ukprn = @ukprn 
                AND AmountDue < 0 
                ";

            return Query<RequiredPaymentEntity>(sql, new { ukprn });
        }
    }
}