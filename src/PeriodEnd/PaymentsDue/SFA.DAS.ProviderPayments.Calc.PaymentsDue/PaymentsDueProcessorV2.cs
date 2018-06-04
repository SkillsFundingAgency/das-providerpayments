using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class PaymentsDueProcessorV2
    {
        private readonly ILogger _logger;
        private readonly IProviderRepository _providerRepository;
        private readonly IProviderProcessor _providerProcessor;

        public PaymentsDueProcessorV2(
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

            var providers = _providerRepository.GetAllProviders();

            foreach (var provider in providers)
            {
                _providerProcessor.Process(provider);
            }

            _logger.Info("Finished Payments Due Processor.");
        }
    }
}