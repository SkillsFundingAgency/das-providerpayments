﻿namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data
{
    public interface IDasAccountRepository
    {
        void UpdateBalance(long accountId, decimal balance);
    }
}