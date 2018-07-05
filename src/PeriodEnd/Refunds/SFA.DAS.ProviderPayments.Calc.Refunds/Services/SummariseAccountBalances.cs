using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services
{
    public class SummariseAccountBalances : ISummariseAccountBalances
    {
        private ConcurrentDictionary<long, decimal> _accountsDictionary = new ConcurrentDictionary<long, decimal>();
        public void IncrementAccountLevyBalance(IEnumerable<Refund> refunds)
        {
            var groupsByAccountId = refunds.GroupBy(x => x.AccountId);
            foreach (var account in groupsByAccountId)
            {
                IncrementOrAddValue(account.Key, account.Sum(x => x.Amount) * -1);
            }
        }

        public List<AccountLevyCredit> AsList()
        {
            return _accountsDictionary.Select(x=>new AccountLevyCredit {AccountId = x.Key, LevyCredit = x.Value}).ToList();
        }

        private void IncrementOrAddValue(long accountId, decimal amountToCredit)
        {
            bool updateFails;
            do
            {
                decimal currentValueToCredit;
                if (_accountsDictionary.TryGetValue(accountId, out currentValueToCredit))
                {
                    updateFails = !_accountsDictionary.TryUpdate(accountId, currentValueToCredit + amountToCredit, currentValueToCredit);
                }
                else
                {
                    updateFails = !_accountsDictionary.TryAdd(accountId, amountToCredit);
                }

            } while (updateFails);
        }
    }
}