using System.Collections.Generic;
using System.Linq;
using NLog;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services
{
    public class ProviderRefundsProcessor : IProviderProcessor
    {
        private readonly ILogger _logger;
        private readonly ILearnerBuilder _learnersBuilder;
        private readonly ILearnerProcessor _learnerProcessor;
        private readonly IPaymentRepository _refundPaymentRepository;

        public ProviderRefundsProcessor(
            ILogger logger,
            ILearnerBuilder learnersBuilder,
            ILearnerProcessor learnerProcessor,
            IPaymentRepository refundPaymentRepository)
        {
            _logger = logger;
            _learnersBuilder = learnersBuilder;
            _learnerProcessor = learnerProcessor;
            _refundPaymentRepository = refundPaymentRepository;
        }

        public IEnumerable<PaymentEntity> Process(ProviderEntity provider)
        {
            _logger.Info($"Processing refunds started for Provider UKPRN: [{provider.Ukprn}].");

            var learners = _learnersBuilder.CreateLearnersForProvider(provider.Ukprn);

            var allRefunds = new List<PaymentEntity>();

            foreach (var learner in learners)
            {
                var refunds = _learnerProcessor.Process(learner).ToList();
                allRefunds.AddRange(refunds);
            }

            _refundPaymentRepository.AddMany(allRefunds);
            _logger.Info($"Processing refunds finished for Provider UKPRN: [{provider.Ukprn}].");

            return allRefunds;
        }
    }
}