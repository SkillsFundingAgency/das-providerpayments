using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories
{
    public interface ICommitmentRepository
    {
        IEnumerable<CommitmentEntity> GetProviderCommitments(long ukprn);
    }

    public class CommitmentRepository : DcfsRepository, ICommitmentRepository
    {
        public CommitmentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IEnumerable<CommitmentEntity> GetProviderCommitments(long ukprn)
        {
            const string sql = @"
                    SELECT * 
                    FROM Reference.DasCommitments
                    WHERE Ukprn = @ukprn
                ";
            
            return Query<CommitmentEntity>(sql, new {ukprn});
        }
    }
}