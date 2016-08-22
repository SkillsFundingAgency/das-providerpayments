using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;

namespace SFA.DAS.ProviderPayments.Domain.Data
{
    public interface IAccountRepository
    {
        Task<AccountEntity> GetAccountAsync(string accountId);
        Task<PageOfEntities<AccountEntity>> GetPageOfAccountsAffectedInPeriodAsync(string periodCode, int pageNumber);
    }
}
