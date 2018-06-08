using System;
using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class PaymentsDueProcessor
    {
        private readonly ILogger _logger;
        private readonly IProviderRepository _providerRepository;
        private readonly IProviderProcessor _providerProcessor;

        public PaymentsDueProcessor(
            ILogger logger,
            IProviderRepository providerRepository, 
            IProviderProcessor providerProcessor)
        {
            _logger = logger;
            _providerRepository = providerRepository;
            _providerProcessor = providerProcessor;
        }

        public virtual void Process()
        {
            _logger.Info("Started Payments Due Processor.");

            try
            {
                var providers = _providerRepository.GetAllProviders();

                foreach (var provider in providers)
                {
                    _providerProcessor.Process(provider);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }

            _logger.Info("Finished Payments Due Processor.");
        }
    }
}