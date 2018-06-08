using System.Collections.Generic;
using System.Linq;
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
        private readonly ICollectionPeriodRepository _colrepo;

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
                var learnerResult = _learnerProcessor.Process(parameters);

                allNonPayablesForProvider.AddRange(learnerResult.NonPayableEarnings);
                allPayablesForProvider.AddRange(learnerResult.PayableEarnings);
            }

            allNonPayablesForProvider.ForEach(nonPayable =>
            {
                nonPayable.IlrSubmissionDateTime = provider.IlrSubmissionDateTime;
            });

            /*foreach (var requiredPaymentEntity in allPayablesForProvider)
            {
                requiredPaymentEntity.IlrSubmissionDateTime = provider.IlrSubmissionDateTime;
                requiredPaymentEntity.CollectionPeriodMonth = _colrepo.GetAllCollectionPeriods().Where(entity => entity.Open).First().Month
            }*/

            _nonPayableEarningRepository.AddMany(allNonPayablesForProvider);
            _requiredPaymentRepository.AddRequiredPayments(allPayablesForProvider.ToArray());

            _logger.Info($"There are [{allNonPayablesForProvider.Count}] non-payable earnings for Learner LearnRefNumber: [{provider.Ukprn}].");
            _logger.Info($"There are [{allPayablesForProvider.Count}] payable earnings for Learner LearnRefNumber: [{provider.Ukprn}].");
            _logger.Info($"Processing finished for Provider UKPRN: [{provider.Ukprn}].");
        }
    }
}