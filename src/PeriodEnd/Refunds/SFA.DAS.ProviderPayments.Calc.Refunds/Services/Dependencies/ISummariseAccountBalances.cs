using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies
{
    public interface ISummariseAccountBalances
    {
        void IncrementAccountLevyBalance(IEnumerable<PaymentEntity> refunds);
        List<AccountLevyCredit> AsList();
    }
}