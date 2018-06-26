using System.Collections.Generic;
using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class ProviderProcessor : IProviderProcessor
    {
        private readonly ILogger _logger;
        private readonly ISortProviderDataIntoLearnerData _parametersBuilder;
        private readonly ICollectionPeriodRepository _collectionPeriodRepository;
        private readonly ILearnerProcessor _learnerProcessor;
        private readonly INonPayableEarningRepository _nonPayableEarningRepository;
        private readonly IRequiredPaymentRepository _requiredPaymentRepository;

        public ProviderProcessor(
            ILogger logger,
            ISortProviderDataIntoLearnerData parametersBuilder,
            ICollectionPeriodRepository collectionPeriodRepository,
            ILearnerProcessor learnerProcessor,
            INonPayableEarningRepository nonPayableEarningRepository,
            IRequiredPaymentRepository requiredPaymentRepository)
        {
            _logger = logger;
            _parametersBuilder = parametersBuilder;
            _collectionPeriodRepository = collectionPeriodRepository;
            _learnerProcessor = learnerProcessor;
            _nonPayableEarningRepository = nonPayableEarningRepository;
            _requiredPaymentRepository = requiredPaymentRepository;
        }

        public void Process(ProviderEntity provider)
        {
            _logger.Info($"Processing started for Provider UKPRN: [{provider.Ukprn}].");

            var learnersParams = _parametersBuilder.Sort(provider.Ukprn);
            var currentCollectionPeriod = _collectionPeriodRepository.GetCurrentCollectionPeriod();
            
            var allNonPayablesForProvider = new List<NonPayableEarningEntity>();
            var allPayablesForProvider = new List<RequiredPaymentEntity>();

            foreach (var parameters in learnersParams)
            {
                var learnerResult = _learnerProcessor.Process(parameters, provider.Ukprn);

                allNonPayablesForProvider.AddRange(learnerResult.NonPayableEarnings);
                allPayablesForProvider.AddRange(learnerResult.PayableEarnings);
            }

            allNonPayablesForProvider.ForEach(nonPayable =>
            {
                nonPayable.IlrSubmissionDateTime = provider.IlrSubmissionDateTime;
                nonPayable.CollectionPeriodName = currentCollectionPeriod.CollectionPeriodName;
                nonPayable.CollectionPeriodMonth = currentCollectionPeriod.Month;
                nonPayable.CollectionPeriodYear = currentCollectionPeriod.Year;
            });

            allPayablesForProvider.ForEach(payable =>
            {
                payable.IlrSubmissionDateTime = provider.IlrSubmissionDateTime;
                payable.CollectionPeriodName = currentCollectionPeriod.CollectionPeriodName;
                payable.CollectionPeriodMonth = currentCollectionPeriod.Month;
                payable.CollectionPeriodYear = currentCollectionPeriod.Year;
            });

            _nonPayableEarningRepository.AddMany(allNonPayablesForProvider);
            _requiredPaymentRepository.AddRequiredPayments(allPayablesForProvider.ToArray());

            _logger.Info($"There are [{allNonPayablesForProvider.Count}] non-payable earnings for Provider UKPRN: [{provider.Ukprn}].");
            _logger.Info($"There are [{allPayablesForProvider.Count}] payable earnings for Provider UKPRN: [{provider.Ukprn}].");
            _logger.Info($"Processing finished for Provider UKPRN: [{provider.Ukprn}].");
        }
    }
}