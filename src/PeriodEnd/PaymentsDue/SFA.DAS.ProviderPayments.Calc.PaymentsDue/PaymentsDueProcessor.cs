using System;
using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.ReferenceData;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class PaymentsDueProcessor
    {
        private readonly ILogger _logger;
        private readonly IProviderRepository _providerRepository;
        private readonly IProviderProcessor _providerProcessor;
        private readonly ICopyIlrReferenceData _referenceData;
        private readonly ICollectionPeriodRepository _collectionPeriodRepository;

        public PaymentsDueProcessor(
            ILogger logger,
            IProviderRepository providerRepository, 
            IProviderProcessor providerProcessor,
            ICopyIlrReferenceData referenceData,
            ICollectionPeriodRepository collectionPeriodRepository)
        {
            _logger = logger;
            _providerRepository = providerRepository;
            _providerProcessor = providerProcessor;
            _referenceData = referenceData;
            _collectionPeriodRepository = collectionPeriodRepository;
        }

        public virtual void Process()
        {
            _logger.Info("Started copying reference data");

            _logger.Info("Copying earnings information");
            _referenceData.CopyEarningsInformation();

            _logger.Info("Copying earnings");
            var currentPeriod = _collectionPeriodRepository.GetCurrentCollectionPeriod();
            _referenceData.CopyRawEarnings(currentPeriod.Id);

            _logger.Info("Finished copying reference data");

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