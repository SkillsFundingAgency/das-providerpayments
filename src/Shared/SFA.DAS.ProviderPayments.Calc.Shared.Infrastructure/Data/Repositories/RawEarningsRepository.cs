using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public interface IRawEarningsRepository
    {
        List<RawEarning> GetAllForProvider(long ukprn);
    }

    public class RawEarningsRepository : DcfsRepository, IRawEarningsRepository
    {
        public RawEarningsRepository(string connectionString) : base(connectionString)
        {
        }

        public List<RawEarning> GetAllForProvider(long ukprn)
        {
            const string sql = @"
            SELECT *
            FROM Staging.RawEarnings
            WHERE Ukprn = @ukprn
            ";

            var result = Query<RawEarning>(sql, new {ukprn})
                .ToList();

            return result;
        }
    }
}