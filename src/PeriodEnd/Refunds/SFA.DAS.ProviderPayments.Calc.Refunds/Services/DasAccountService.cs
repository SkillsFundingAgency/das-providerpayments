using System.Collections.Generic;
using NLog;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services
{
    public class DasAccountService : IDasAccountService
    {
        private readonly IDasAccountRepository _dasAccountRepository;
        private readonly ILogger _logger;

        public DasAccountService(IDasAccountRepository dasAccountRepository, ILogger logger)
        {
            _dasAccountRepository = dasAccountRepository;
            _logger = logger;
        }

        public void UpdateAccountLevyBalances(IEnumerable<AccountLevyCredit> items)
        {
            _logger.Info("Start - Updating Levy Balances");
            foreach (var credit in items)
            {
                _dasAccountRepository.UpdateBalance(credit.AccountId, credit.LevyCredit);
            }
            _logger.Info("Finish - Updating Levy Balances");
        }
    }
}
