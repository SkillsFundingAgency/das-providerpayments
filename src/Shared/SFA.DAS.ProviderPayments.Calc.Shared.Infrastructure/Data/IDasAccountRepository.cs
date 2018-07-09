using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data
{
    public interface IDasAccountRepository
    {
        void AddMany(List<DasAccountEntity> dasAccounts);
        void UpdateBalance(long accountId, decimal balance);
    }
}