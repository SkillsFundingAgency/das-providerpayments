using System.Collections.Generic;
using NLog;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services
{
    public class ProviderProcessor : IProviderProcessor
    {
        private readonly ILogger _logger;
        private readonly ILearnerBuilder _learnersBuilder;

        public ProviderProcessor(
            ILogger logger,
            ILearnerBuilder learnersBuilder)
        {
            _logger = logger;
            _learnersBuilder = learnersBuilder;
        }

        public List<AccountLevyCredit> Process(ProviderEntity provider)
        {
            _logger.Info($"Processing refunds started for Provider UKPRN: [{provider.Ukprn}].");

            var learners = _learnersBuilder.CreateLearnersForThisProvider(provider.Ukprn);

            //var currentCollectionPeriod = _collectionPeriodRepository.GetCurrentCollectionPeriod();
            
            //var allNonPayablesForProvider = new List<NonPayableEarningEntity>();
            //var allPayablesForProvider = new List<RequiredPaymentEntity>();

            //foreach (var parameters in learners)
            //{
            //    var learnerResult = _learnerProcessor.Process(parameters, provider.Ukprn);

            //    allNonPayablesForProvider.AddRange(learnerResult.NonPayableEarnings);
            //    allPayablesForProvider.AddRange(learnerResult.PayableEarnings);
            //}

            //allNonPayablesForProvider.ForEach(nonPayable =>
            //{
            //    nonPayable.IlrSubmissionDateTime = provider.IlrSubmissionDateTime;
            //    nonPayable.CollectionPeriodName = currentCollectionPeriod.CollectionPeriodName;
            //    nonPayable.CollectionPeriodMonth = currentCollectionPeriod.Month;
            //    nonPayable.CollectionPeriodYear = currentCollectionPeriod.Year;
            //});

            //allPayablesForProvider.ForEach(payable =>
            //{
            //    payable.IlrSubmissionDateTime = provider.IlrSubmissionDateTime;
            //    payable.CollectionPeriodName = currentCollectionPeriod.CollectionPeriodName;
            //    payable.CollectionPeriodMonth = currentCollectionPeriod.Month;
            //    payable.CollectionPeriodYear = currentCollectionPeriod.Year;
            //});

            //_nonPayableEarningRepository.AddMany(allNonPayablesForProvider);
            //_requiredPaymentRepository.AddRequiredPayments(allPayablesForProvider.ToArray());

            _logger.Info($"Processing refunds finished for Provider UKPRN: [{provider.Ukprn}].");

            return null;
        }
    }
}