using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class CommitmentRepository : DcfsRepository, ICommitmentRepository
    {
        public CommitmentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IList<Commitment> GetProviderCommitments(long ukprn)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ukprn", ukprn, DbType.Int64);

            return QueryByProc<Commitment>("DataLock.GetCommitmentsForProvider",
                parameters,
                600).ToList();
        }
    }

    public interface ICommitmentRepository
    {
        IList<Commitment> GetProviderCommitments(long ukprn);
    }
}