using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class ProviderProcessor : IProviderProcessor
    {
        private readonly IProviderLearnersBuilder _providerLearnersBuilder;
        private readonly IRequiredPaymentRepository _requiredPaymentRepository;

        public ProviderProcessor(IProviderLearnersBuilder providerLearnersBuilder,
            IRequiredPaymentRepository requiredPaymentRepository)
        {
            _providerLearnersBuilder = providerLearnersBuilder;
            _requiredPaymentRepository = requiredPaymentRepository;
        }

        public void Process(ProviderEntity provider)
        {
            var providerLearners = _providerLearnersBuilder.Build(provider.Ukprn);

            foreach (var learner in providerLearners)
            {
                _requiredPaymentRepository.AddRequiredPayments(learner.Value.RequiredPayments.ToArray());
            }
        }
    }
}