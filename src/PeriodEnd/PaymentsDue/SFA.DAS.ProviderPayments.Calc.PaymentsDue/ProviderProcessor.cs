using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class ProviderProcessor : IProviderProcessor
    {
        private readonly ILogger _logger;
        private readonly IProviderLearnersBuilder _providerLearnersBuilder;
        private readonly INonPayableEarningRepository _nonPayableEarningRepository;
        private readonly IRequiredPaymentRepository _requiredPaymentRepository;

        public ProviderProcessor(
            ILogger logger,
            IProviderLearnersBuilder providerLearnersBuilder,
            INonPayableEarningRepository nonPayableEarningRepository,
            IRequiredPaymentRepository requiredPaymentRepository)
        {
            _logger = logger;
            _providerLearnersBuilder = providerLearnersBuilder;
            _nonPayableEarningRepository = nonPayableEarningRepository;
            _requiredPaymentRepository = requiredPaymentRepository;
        }

        public void Process(ProviderEntity provider)
        {
            _logger.Info($"Processing started for Provider UKPRN: [{provider.Ukprn}].");

            var providerLearners = _providerLearnersBuilder.Build(provider.Ukprn);

            foreach (var learner in providerLearners)
            {
                _logger.Info($"Processing started for Learner LearnRefNumber: [{learner.Key}].");

                // todo: learner data lock calcs
                // todo: learner aggregate payment calcs

                _logger.Info($"There are [{learner.Value.NonPayableEarnings.Count}] non-payable earnings for Learner LearnRefNumber: [{learner.Key}].");
                _nonPayableEarningRepository.AddMany(learner.Value.NonPayableEarnings);

                _logger.Info($"There are [{learner.Value.RequiredPayments.Count}] payable earnings for Learner LearnRefNumber: [{learner.Key}].");
                _requiredPaymentRepository.AddRequiredPayments(learner.Value.RequiredPayments.ToArray());

                _logger.Info($"Processing finished for Learner LearnRefNumber: [{learner.Key}].");
            }

            _logger.Info($"Processing finished for Provider UKPRN: [{provider.Ukprn}].");
        }
    }
}