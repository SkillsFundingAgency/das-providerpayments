using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class PaymentsDueProcessorV2
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IProviderProcessor _providerProcessor;

        public PaymentsDueProcessorV2(
            IProviderRepository providerRepository, 
            IProviderProcessor providerProcessor)
        {
            _providerRepository = providerRepository;
            _providerProcessor = providerProcessor;
        }

        public virtual void Process()
        {
            var providers = _providerRepository.GetAllProviders();

            foreach (var provider in providers)
            {
                _providerProcessor.Process(provider);
            }
        }
    }
}