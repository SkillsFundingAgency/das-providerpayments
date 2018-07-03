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
        private readonly IDasAccountRepository _dasAccountRepository;
        private readonly ILogger _logger;

        [UsedImplicitly]
        public RefundsProcessor(IProviderRepository providerRepository, IProviderProcessor providerProcessor, ISummariseAccountBalances summariseAccountBalances, 
            IDasAccountRepository dasAccountRepository, ILogger logger)
        {
            _providerRepository = providerRepository;
            _providerProcessor = providerProcessor;
            _summariseAccountBalances = summariseAccountBalances;
            _dasAccountRepository = dasAccountRepository;
            _logger = logger;
        }

        public virtual void Process()
        {
            _logger.Info("Started Refunds Processor.");

            try
            {
                var providers = _providerRepository.GetAllProviders();
                _summariseAccountBalances.Initialise();

                foreach (var provider in providers)
                {
                    var levyBalances = _providerProcessor.Process(provider);
                    _summariseAccountBalances.IncrementAccountLevyBalance(levyBalances);
                }

                _dasAccountRepository.Update(); // Need to work out how batch updates work

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
