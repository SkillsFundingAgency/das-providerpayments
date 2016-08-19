using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;

namespace SFA.DAS.ProviderPayments.Infrastructure.Data.InMemory
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        private const int PageSize = 10;

        private readonly List<AccountEntity> _accounts;

        public InMemoryAccountRepository()
        {
            _accounts = new List<AccountEntity>
            {
                new AccountEntity { Id = "DasAccount1" },
                new AccountEntity { Id = "DasAccount2" },
                new AccountEntity { Id = "DasAccount3" },
                new AccountEntity { Id = "DasAccount4" },
                new AccountEntity { Id = "DasAccount5" },
                new AccountEntity { Id = "DasAccount6" },
                new AccountEntity { Id = "DasAccount7" },
                new AccountEntity { Id = "DasAccount8" },
                new AccountEntity { Id = "DasAccount9" },
                new AccountEntity { Id = "DasAccount10" },
                new AccountEntity { Id = "DasAccount11" },
                new AccountEntity { Id = "DasAccount12" },
            };
        }

        public Task<PageOfEntities<AccountEntity>> GetPageOfAccountsAffectedInPeriodAsync(string periodCode, int pageNumber)
        {
            var skip = (pageNumber - 1) * PageSize;
            var items = _accounts.Skip(skip).Take(PageSize).ToArray();
            if (items.Length == 0)
            {
                return Task.FromResult<PageOfEntities<AccountEntity>>(null);
            }

            var page = new PageOfEntities<AccountEntity>
            {
                Items = items,
                TotalNumberOfItems = _accounts.Count,
                TotalNumberOfPages = (int)Math.Ceiling(_accounts.Count / (float)PageSize)
            };
            return Task.FromResult(page);
        }
    }
}
