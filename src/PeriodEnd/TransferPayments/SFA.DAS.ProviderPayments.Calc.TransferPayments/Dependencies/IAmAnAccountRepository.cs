using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dependencies
{
    public interface IAmAnAccountRepository
    {
        IEnumerable<Account> AllAccounts();
    }
}
