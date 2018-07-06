using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NLog;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services
{
    public class SummariseAccountBalances : ISummariseAccountBalances
    {
        private readonly ILogger _logger;
        private readonly ConcurrentDictionary<long, decimal> _accountLevyBalanceAdjustmentDictionary;

        public SummariseAccountBalances(ILogger logger)
        {
            _logger = logger;
            _accountLevyBalanceAdjustmentDictionary = new ConcurrentDictionary<long, decimal>();
        }

        public void IncrementAccountLevyBalance(IEnumerable<Refund> refunds)
        {
            var groupsByAccountId = refunds.Where(x => x.FundingSource == FundingSource.Levy).GroupBy(x => x.AccountId);
            foreach (var account in groupsByAccountId)
            {
                IncrementOrAddValue(account.Key, account.Sum(x => x.Amount) * -1);
            }
        }

        public List<AccountLevyCredit> AsList()
        {
            return _accountLevyBalanceAdjustmentDictionary.Select(x=>new AccountLevyCredit {AccountId = x.Key, LevyCredit = x.Value}).ToList();
        }

        private void IncrementOrAddValue(long accountId, decimal amountToCredit)
        {
            bool updateFails;
            int i = 0;
            do
            {
                i++;
                decimal currentValueToCredit;
                if (_accountLevyBalanceAdjustmentDictionary.TryGetValue(accountId, out currentValueToCredit))
                {
                    updateFails = !_accountLevyBalanceAdjustmentDictionary.TryUpdate(accountId, currentValueToCredit + amountToCredit, currentValueToCredit);
                }
                else
                {
                    updateFails = !_accountLevyBalanceAdjustmentDictionary.TryAdd(accountId, amountToCredit);
                }
            } while (updateFails || i >= 50);

            if (i >= 50)
            {
                var message = "Refunds.SummariseAccountBalances class has failed to add or update account values";
                _logger.Error(message);
                throw new Exception(message);
            }

        }
    }
}