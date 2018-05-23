using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public interface IRequiredPaymentsHistoryRepository
    {
        List<RequiredPaymentsHistoryEntity> GetAllForProvider(long ukprn);
    }

    public class RequiredPaymentsHistoryRepository : DcfsRepository, IRequiredPaymentsHistoryRepository
    {
        public RequiredPaymentsHistoryRepository(string connectionString) : base(connectionString)
        {
        }

        public List<RequiredPaymentsHistoryEntity> GetAllForProvider(long ukprn)
        {
            const string sql = @"
            SELECT *
            FROM Reference.RequiredPaymentsHistory
            WHERE Ukprn = @ukprn";

            var result = Query<RequiredPaymentsHistoryEntity>(sql, new { ukprn })
                .ToList();

            return result;
        }
    }
}