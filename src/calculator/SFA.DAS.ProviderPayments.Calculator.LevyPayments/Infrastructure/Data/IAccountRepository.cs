using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data
{
    public interface IAccountRepository
    {
        AccountEntity GetNextAccountRequiringProcessing();
        AccountEntity GetAccountById(string id);
        void SpendLevy(string accountId, decimal amount);
    }
}
