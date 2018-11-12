using System.Collections.Generic;
using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class ProviderPaymentsDueProcessor : IProviderPaymentsDueProcessor
    {
        private readonly ILogger _logger;
        private readonly ICorrelateLearnerData _learnerDataCorrelator;
        private readonly ICollectionPeriodRepository _collectionPeriodRepository;
        private readonly IProcessPaymentsDue _processPaymentsDue;
        private readonly INonPayableEarningRepository _nonPayableEarningRepository;
        private readonly IRequiredPaymentRepository _requiredPaymentRepository;

        public ProviderPaymentsDueProcessor(
            ILogger logger,
            ICorrelateLearnerData learnerDataCorrelator,
            ICollectionPeriodRepository collectionPeriodRepository,
            IProcessPaymentsDue processPaymentsDue,
            INonPayableEarningRepository nonPayableEarningRepository,
            IRequiredPaymentRepository requiredPaymentRepository)
        {
            _logger = logger;
            _learnerDataCorrelator = learnerDataCorrelator;
            _collectionPeriodRepository = collectionPeriodRepository;
            _processPaymentsDue = processPaymentsDue;
            _nonPayableEarningRepository = nonPayableEarningRepository;
            _requiredPaymentRepository = requiredPaymentRepository;
        }

        public void Process(ProviderEntity provider)
        {
            _logger.Info($"Processing started for Provider UKPRN: [{provider.Ukprn}].");

            var correlatedLearnerData = _learnerDataCorrelator.CreateLearnerDataForProvider(provider.Ukprn);
            var currentCollectionPeriod = _collectionPeriodRepository.GetCurrentCollectionPeriod();
            
            var allNonPayablesForProvider = new List<NonPayableEarning>();
            var allPayablesForProvider = new List<RequiredPayment>();

            foreach (var learnerData in correlatedLearnerData)
            {
                var learnerEarnings = _processPaymentsDue
                    .GetPayableAndNonPayableEarnings(learnerData, provider.Ukprn);

                allNonPayablesForProvider.AddRange(learnerEarnings.NonPayableEarnings);
                allPayablesForProvider.AddRange(learnerEarnings.PayableEarnings);
            }

            allNonPayablesForProvider.ForEach(nonPayable =>
            {
                AddCollectionDataToRequiredPayment(provider, nonPayable, currentCollectionPeriod);
            });

            allPayablesForProvider.ForEach(payable =>
            {
                AddCollectionDataToRequiredPayment(provider, payable, currentCollectionPeriod);
            });

            StoreEarnings(allNonPayablesForProvider, allPayablesForProvider);

            _logger.Info($"There are [{allNonPayablesForProvider.Count}] non-payable earnings for Provider UKPRN: [{provider.Ukprn}].");
            _logger.Info($"There are [{allPayablesForProvider.Count}] payable earnings for Provider UKPRN: [{provider.Ukprn}].");
            _logger.Info($"Processing finished for Provider UKPRN: [{provider.Ukprn}].");
        }

        private void StoreEarnings(List<NonPayableEarning> allNonPayablesForProvider, List<RequiredPayment> allPayablesForProvider)
        {
            _nonPayableEarningRepository.AddMany(allNonPayablesForProvider);
            _requiredPaymentRepository.AddMany(allPayablesForProvider);
        }

        private static void AddCollectionDataToRequiredPayment(
            ProviderEntity provider, 
            RequiredPayment requiredPayment,
            CollectionPeriodEntity currentCollectionPeriod)
        {
            requiredPayment.IlrSubmissionDateTime = provider.IlrSubmissionDateTime;
            requiredPayment.CollectionPeriodName = currentCollectionPeriod.CollectionPeriodName;
            requiredPayment.CollectionPeriodMonth = currentCollectionPeriod.Month;
            requiredPayment.CollectionPeriodYear = currentCollectionPeriod.Year;
        }
    }
}