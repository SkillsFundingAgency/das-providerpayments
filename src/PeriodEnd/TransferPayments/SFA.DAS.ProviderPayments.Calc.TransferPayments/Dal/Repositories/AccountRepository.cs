﻿using System.Collections.Concurrent;
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
        private static ConcurrentDictionary<long, Account> _accounts;

        public AccountRepository(string connectionString) : base(connectionString)
        {
            _accounts = new ConcurrentDictionary<long, Account>(AllAccounts()
                .Select(x => new KeyValuePair<long, Account>(x.Id, x)));
        }

        public IEnumerable<Account> AllAccounts()
        {
            var entities = AllAccountEntities();
            var accounts = entities.Select(x => new Account(x));
            return accounts;
        }

        public Account Account(long accountId)
        {
            Account account;
            if (!_accounts.TryGetValue(accountId, out account))
            {
                return null;
            }

            return account;
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
