using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data
{
    public interface IAccountRepository
    {
        AccountEntity GetNextAccountRequiringProcessing();
        AccountEntity GetAccountById(string id);

        void SpendLevy(string accountId, decimal amount);
        void MarkAccountAsProcessed(string accountId);
    }
}
