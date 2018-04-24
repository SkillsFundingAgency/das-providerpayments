using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dependencies;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Repositories
{
    class AccountRepository : DcfsRepository, IAmAnAccountRepository
    {
        public AccountRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<Account> AllAccounts()
        {
            var entities = AllAccountEntities();
            var accounts = entities.Select(x => new Account(x));
            return accounts;
        }

        IEnumerable<DasAccount> AllAccountEntities()
        {
            var command = "SELECT AccountId, " +
                          "Balance, " +
                          "IsLevyPayer, " +
                          "TransferBalance " +
                          "FROM" +
                          " DasAccounts";
            return Query<DasAccount>(command);
        }
    }
}
