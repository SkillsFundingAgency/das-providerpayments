using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data
{
    public interface IEarningRepository
    {
        EarningEntity GetEarningForCommitment(string commitmentId);
    }
}
