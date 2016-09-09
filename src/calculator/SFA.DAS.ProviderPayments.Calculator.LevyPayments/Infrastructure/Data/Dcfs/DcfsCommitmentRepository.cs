using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Dcfs
{
    public class DcfsCommitmentRepository : DcfsRepository, ICommitmentRepository
    {
        private const string CommitmentsSource = "LevyPayments.vw_AccountCommitments";
        private const string CommitmentsColumns = "CommitmentId [Id]";
        private const string SelectCommitments = "SELECT " + CommitmentsColumns + " FROM " + CommitmentsSource;
        private const string SelectCommitmentsForAccount = SelectCommitments + " WHERE AccountId = @AccountId";

        public DcfsCommitmentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public CommitmentEntity[] GetCommitmentsForAccount(string accountId)
        {
            return Query<CommitmentEntity>(SelectCommitmentsForAccount, new { accountId });
        }
    }
}
