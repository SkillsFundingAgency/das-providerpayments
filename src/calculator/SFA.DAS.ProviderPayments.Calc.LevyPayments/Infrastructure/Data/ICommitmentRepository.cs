using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data
{
    public interface ICommitmentRepository
    {
        CommitmentEntity[] GetCommitmentsForAccount(string accountId);
    }
}
