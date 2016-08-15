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
                new PeriodEndEntity { PeriodCode = "201704" }
            };
        }

        public Task<PageOfEntities<PeriodEndEntity>> GetPageAsync(int pageNumber)
        {
            var skip = (pageNumber - 1) * PageSize;
            var items = _periodEnds.Skip(skip).Take(PageSize).ToArray();
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
