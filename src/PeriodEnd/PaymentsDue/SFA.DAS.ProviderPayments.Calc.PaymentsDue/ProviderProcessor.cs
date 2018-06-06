using System.Collections.Generic;
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
        private readonly ILearnerProcessParametersBuilder _parametersBuilder;
        private readonly ILearnerProcessor _learnerProcessor;
        private readonly INonPayableEarningRepository _nonPayableEarningRepository;
        private readonly IRequiredPaymentRepository _requiredPaymentRepository;

        public ProviderProcessor(
            ILogger logger,
            ILearnerProcessParametersBuilder parametersBuilder,
            ILearnerProcessor learnerProcessor,
            INonPayableEarningRepository nonPayableEarningRepository,
            IRequiredPaymentRepository requiredPaymentRepository)
        {
            _logger = logger;
            _parametersBuilder = parametersBuilder;
            _learnerProcessor = learnerProcessor;
            _nonPayableEarningRepository = nonPayableEarningRepository;
            _requiredPaymentRepository = requiredPaymentRepository;
            
        }

        public void Process(ProviderEntity provider)
        {
            _logger.Info($"Processing started for Provider UKPRN: [{provider.Ukprn}].");

            var learnersParams = _parametersBuilder.Build(provider.Ukprn);

            var allNonPayablesForProvider = new List<NonPayableEarningEntity>();
            var allPayablesForProvider = new List<RequiredPaymentEntity>();

            foreach (var parameters in learnersParams)
            {
                _logger.Info($"Processing started for Learner LearnRefNumber: [{parameters.LearnRefNumber}].");//todo:move to learner processor

                var learnerResult = _learnerProcessor.Process(parameters);

                _logger.Info($"There are [{learnerResult.NonPayableEarnings.Count}] non-payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}].");
                allNonPayablesForProvider.AddRange(learnerResult.NonPayableEarnings);

                _logger.Info($"There are [{learnerResult.PayableEarnings.Count}] payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}].");
                allPayablesForProvider.AddRange(learnerResult.PayableEarnings);

                _logger.Info($"Processing finished for Learner LearnRefNumber: [{parameters.LearnRefNumber}].");
            }

            _logger.Info($"There are [{allNonPayablesForProvider.Count}] non-payable earnings for Learner LearnRefNumber: [{provider.Ukprn}].");
            _nonPayableEarningRepository.AddMany(allNonPayablesForProvider);

            _logger.Info($"There are [{allPayablesForProvider.Count}] payable earnings for Learner LearnRefNumber: [{provider.Ukprn}].");
            _requiredPaymentRepository.AddRequiredPayments(allPayablesForProvider.ToArray());

            _logger.Info($"Processing finished for Provider UKPRN: [{provider.Ukprn}].");
        }
    }
}