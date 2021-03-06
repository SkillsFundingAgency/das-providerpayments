﻿using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data
{
    public interface IPriceEpisodeMatchRepository
    {
        void AddPriceEpisodeMatches(PriceEpisodeMatchEntity[] priceEpisodeMatches);
    }
}