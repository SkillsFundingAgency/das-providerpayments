using System.Data;
using Dapper;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Repositories
{
    public class PaymentDueRepository : DcfsRepository, IPaymentDueRepository
    {
        public PaymentDueRepository(string connectionString)
            : base(connectionString)
        {
        }

        public PaymentDueEntity[] GetPaymentsDueByUkprn(long ukprn)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ukprn", ukprn, DbType.Int64);
            return QueryByProc<PaymentDueEntity>("CoInvestedPayments.GetRequiredPaymentsByUkPrn", parameters);
        }
    }
}