using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Data;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Repositories
{
    class AccountRepository :DcfsRepository
    {
        public AccountRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<DasAccount> AllAccounts()
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
