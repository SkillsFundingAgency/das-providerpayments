using System.Collections.Generic;
using System.Data;
using Dapper;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class CommitmentRepository : DcfsRepository, ICommitmentRepository
    {
        public CommitmentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IEnumerable<CommitmentEntity> GetProviderCommitments(long ukprn)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ukprn", ukprn, DbType.Int64);

            return QueryByProc<CommitmentEntity>("DataLock.GetCommitmentsForProvider",
                parameters,
                DataLockTask.CommandTimeout);
        }
    }
}