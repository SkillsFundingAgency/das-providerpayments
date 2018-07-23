using System;
using JetBrains.Annotations;
using NLog;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.Refunds
{
    public class RefundsProcessor
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IProviderProcessor _providerProcessor;
        private readonly ISummariseAccountBalances _summariseAccountBalances;
        private readonly IDasAccountService _dasAccountService;
        private readonly ILogger _logger;

        [UsedImplicitly]
        public RefundsProcessor(IProviderRepository providerRepository, IProviderProcessor providerProcessor, ISummariseAccountBalances summariseAccountBalances, 
            IDasAccountService dasAccountService, ILogger logger)
        {
            _providerRepository = providerRepository;
            _providerProcessor = providerProcessor;
            _summariseAccountBalances = summariseAccountBalances;
            _dasAccountService = dasAccountService;
            _logger = logger;
        }

        public virtual void Process()
        {
            _logger.Info("Started Refunds Processor.");

            try
            {
                var providers = _providerRepository.GetAllProviders();

                foreach (var provider in providers)
                {
                    var refunds = _providerProcessor.Process(provider);
                    _summariseAccountBalances.IncrementAccountLevyBalance(refunds);
                }

                _dasAccountService.UpdateAccountLevyBalances(_summariseAccountBalances.AsList()); 

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }

            _logger.Info("Finished Refunds Processor.");
        }
    }
}
