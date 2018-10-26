using System;
using System.Threading.Tasks;
using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class PaymentsDueProcessor
    {
        private readonly ILogger _logger;
        private readonly IProviderRepository _providerRepository;
        private readonly IProviderPaymentsDueProcessor _providerProcessor;

        public PaymentsDueProcessor(
            ILogger logger,
            IProviderRepository providerRepository, 
            IProviderPaymentsDueProcessor providerProcessor)
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

                Parallel.ForEach(providers, new ParallelOptions { MaxDegreeOfParallelism = 25 }, 
                    provider =>
                {
                    _providerProcessor.Process(provider);
                });
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