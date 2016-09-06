using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data
{
    public interface ICommitmentRepository
    {
        CommitmentEntity[] GetCommitmentsForAccount(string accountId);
    }
}
