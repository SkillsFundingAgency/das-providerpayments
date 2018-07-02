using System;
using JetBrains.Annotations;
using NLog;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.Refunds
{
    public class RefundsProcessor
    {
        private readonly IProviderRepository _providerRepository;
        private readonly ILogger _logger;

        [UsedImplicitly]
        public RefundsProcessor(IProviderRepository providerRepository, ILogger logger)
        {
            _providerRepository = providerRepository;
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
                    //_providerProcessor.Process(provider);
                }
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
