using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;

namespace SFA.DAS.ProviderPayments.Infrastructure.Data.InMemory
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        public Task<PageOfEntities<AccountEntity>> GetPageOfAccountsAffectedInPeriodAsync(string periodCode, int pageNumber)
        {
            return Task.FromResult(new PageOfEntities<AccountEntity>
            {
                TotalNumberOfItems = 1,
                TotalNumberOfPages = 1,
                Items = new[]
                {
                    new AccountEntity
                    {
                        Id = "DasAccount1"
                    }
                }
            });
        }
    }
}
