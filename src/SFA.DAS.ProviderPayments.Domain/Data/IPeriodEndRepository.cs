﻿using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;

namespace SFA.DAS.ProviderPayments.Domain.Data
{
    public interface IPeriodEndRepository
    {
        Task<PageOfEntities<PeriodEndEntity>> GetPageAsync(int pageNumber);
    }
}
