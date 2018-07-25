using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies
{
    public interface IDasAccountService
    {
        void UpdateAccountLevyBalances(IEnumerable<AccountLevyCredit> items);
    }
}