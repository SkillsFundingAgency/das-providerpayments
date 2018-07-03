using System.Collections.Generic;
using System.Linq;
using NLog;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services
{
    public class ProviderProcessor : IProviderProcessor
    {
        private readonly ILogger _logger;
        private readonly ILearnerBuilder _learnersBuilder;
        private readonly ILearnerProcessor _learnerProcessor;
        private readonly ISummariseAccountBalances _summariseAccountBalances;
        private readonly IRefundPaymentRepository _refundPaymentRepository;

        public ProviderProcessor(
            ILogger logger,
            ILearnerBuilder learnersBuilder,
            ILearnerProcessor learnerProcessor,
            ISummariseAccountBalances summariseAccountBalances,
            IRefundPaymentRepository refundPaymentRepository)
        {
            _logger = logger;
            _learnersBuilder = learnersBuilder;
            _learnerProcessor = learnerProcessor;
            _summariseAccountBalances = summariseAccountBalances;
            _refundPaymentRepository = refundPaymentRepository;
        }

        public IEnumerable<AccountLevyCredit> Process(ProviderEntity provider)
        {
            _logger.Info($"Processing refunds started for Provider UKPRN: [{provider.Ukprn}].");

            _summariseAccountBalances.Initialise();

            var learners = _learnersBuilder.CreateLearnersForThisProvider(provider.Ukprn);

            var allRefunds = new List<RefundPaymentEntity>();

            foreach (var learner in learners)
            {
                var refunds = _learnerProcessor.Process(learner).ToList();
                allRefunds.AddRange(refunds);
                _summariseAccountBalances.IncrementAccountLevyBalance(refunds);
            }

            _refundPaymentRepository.AddMany(allRefunds);
            _logger.Info($"Processing refunds finished for Provider UKPRN: [{provider.Ukprn}].");

            return _summariseAccountBalances.AsList();
        }
    }
}