using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class PaymentsDueProcessorV2
    {
        private readonly IProviderRepository _providerRepository;
        private IProviderLearnersBuilder _providerLearnersBuilder;

        public PaymentsDueProcessorV2(
            IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public virtual void Process()
        {
            var providers = _providerRepository.GetAllProviders();

            foreach (var provider in providers)
            {
                ProcessProvider(provider);
            }
        }

        private void ProcessProvider(ProviderEntity provider)
        {
            var providerLearners = _providerLearnersBuilder.Build(provider.Ukprn);

            foreach (var learner in providerLearners)
            {
                // check for datalocks and if present filter out non-payable earnings, including why
                // payable earnings:
                // group by course, sfa contrib, etc (todo: what is etc)
                // get past payments grouped by same
                // compare totals for matching groups:
                // +ve: create payment
                // -ve: create refund
                // if refund then refund the most recent period or amount until 0 left to refund

                // eaernings for txn1 minus past pmts for txn1 = amt due for txn1... etc per learner per aim (and other things)
            }
        }
    }
}