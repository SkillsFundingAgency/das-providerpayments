using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data
{
    public interface IAccountRepository
    {
        AccountEntity GetNextAccountRequiringProcessing();
        AccountEntity GetAccountById(long id);
        IEnumerable<AccountPaymentEntity> GetAccountAndPaymentInformationForProcessing();
        void UpdateLevyBalance(long accountId, decimal amount);
        void MarkAccountAsProcessed(long accountId);
    }
}
