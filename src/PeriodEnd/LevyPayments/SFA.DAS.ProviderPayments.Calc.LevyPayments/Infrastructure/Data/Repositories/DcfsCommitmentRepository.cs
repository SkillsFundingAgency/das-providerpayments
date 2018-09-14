using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Repositories
{
    public class DcfsCommitmentRepository : DcfsRepository, ICommitmentRepository
    {
        private const string CommitmentsSource = "Reference.DasCommitments";
        private const string CommitmentsColumns = "CommitmentId [Id], VersionId";
        private const string SelectCommitments = "SELECT " + CommitmentsColumns + " FROM " + CommitmentsSource;
        private const string SelectCommitmentsForAccount = SelectCommitments + " WHERE AccountId = @AccountId ORDER BY Priority ASC";

        public DcfsCommitmentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public CommitmentEntity[] GetCommitmentsForAccount(long accountId)
        {
            return Query<CommitmentEntity>(SelectCommitmentsForAccount, new { accountId });
        }
    }
}
