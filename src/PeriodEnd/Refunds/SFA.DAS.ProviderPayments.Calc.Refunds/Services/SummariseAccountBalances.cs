using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NLog;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services
{
    public class SummariseAccountBalances : ISummariseAccountBalances
    {
        private ConcurrentDictionary<long, decimal> _accountsDictionary = new ConcurrentDictionary<long, decimal>();
        public void IncrementAccountLevyBalance(IEnumerable<PaymentEntity> refunds)
        {

        }

        public List<AccountLevyCredit> AsList()
        {
            return _accountsDictionary.Select(x=>new AccountLevyCredit {AccountId = x.Key, LevyCredit = x.Value}).ToList();
        }
    }
}