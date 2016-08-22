using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;

namespace SFA.DAS.ProviderPayments.Infrastructure.Data.InMemory
{
    public class InMemoryPeriodEndRepository : IPeriodEndRepository
    {
        private const int PageSize = 10;

        private readonly List<PeriodEndEntity> _periodEnds;

        public InMemoryPeriodEndRepository()
        {
            _periodEnds = new List<PeriodEndEntity>
            {
                new PeriodEndEntity { PeriodCode = "201704" },
                new PeriodEndEntity { PeriodCode = "201705" },
                new PeriodEndEntity { PeriodCode = "201706" },
                new PeriodEndEntity { PeriodCode = "201707" },
                new PeriodEndEntity { PeriodCode = "201708" },
                new PeriodEndEntity { PeriodCode = "201709" },
                new PeriodEndEntity { PeriodCode = "201710" },
                new PeriodEndEntity { PeriodCode = "201711" },
                new PeriodEndEntity { PeriodCode = "201712" },
                new PeriodEndEntity { PeriodCode = "201801" },
                new PeriodEndEntity { PeriodCode = "201802" },
                new PeriodEndEntity { PeriodCode = "201803" }
            };
        }

        public Task<PeriodEndEntity> GetPeriodEndAsync(string periodCode)
        {
            var period = _periodEnds.SingleOrDefault(p => p.PeriodCode == periodCode);

            return Task.FromResult(period);
        }
        public Task<PageOfEntities<PeriodEndEntity>> GetPageAsync(int pageNumber)
        {
            var skip = (pageNumber - 1) * PageSize;
            var items = _periodEnds.Skip(skip).Take(PageSize).ToArray();
            if (items.Length == 0)
            {
                return Task.FromResult<PageOfEntities<PeriodEndEntity>>(null);
            }

            var page = new PageOfEntities<PeriodEndEntity>
            {
                Items = items,
                TotalNumberOfItems = _periodEnds.Count,
                TotalNumberOfPages = (int)Math.Ceiling(_periodEnds.Count / (float)PageSize)
            };

            return Task.FromResult(page);
        }
    }
}
