﻿using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data
{
    public interface IDasAccountRepository
    {
        void AdjustBalance(long accountId, decimal balance);
        IEnumerable<DasAccounEntity> GetDasAccounts();
    }
}