using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public class ProviderLearnersBuilder : IProviderLearnersBuilder
    {
        private readonly IRawEarningsRepository _rawEarningsRepository;
        private readonly IRawEarningsMathsEnglishRepository _rawEarningsMathsEnglishRepository;
        private readonly IRequiredPaymentsHistoryRepository _historicalPaymentsRepository;
        private readonly IDataLockPriceEpisodePeriodMatchesRepository _dataLockRepository;
        private readonly ICollectionPeriodRepository _collectionPeriodRepository;

        public ProviderLearnersBuilder(
            IRawEarningsRepository rawEarningsRepository, 
            IRawEarningsMathsEnglishRepository rawEarningsMathsEnglishRepository, 
            IRequiredPaymentsHistoryRepository historicalPaymentsRepository, 
            IDataLockPriceEpisodePeriodMatchesRepository dataLockRepository, 
            ICollectionPeriodRepository collectionPeriodRepository)
        {
            _rawEarningsRepository = rawEarningsRepository;
            _rawEarningsMathsEnglishRepository = rawEarningsMathsEnglishRepository;
            _historicalPaymentsRepository = historicalPaymentsRepository;
            _dataLockRepository = dataLockRepository;
            _collectionPeriodRepository = collectionPeriodRepository;
        }

        public Dictionary<string, Learner> Build(long ukprn)
        {
            var learners = new Dictionary<string, Learner>();

            foreach (var rawEarning in _rawEarningsRepository.GetAllForProvider(ukprn))
            {
                if (!learners.ContainsKey(rawEarning.LearnRefNumber))
                {
                    learners.Add(rawEarning.LearnRefNumber, new Learner(_collectionPeriodRepository.GetAllCollectionPeriods()));
                }

                learners[rawEarning.LearnRefNumber].RawEarnings.Add(rawEarning);
            }

            foreach (var rawEarningMathsEnglish in _rawEarningsMathsEnglishRepository.GetAllForProvider(ukprn))
            {
                if (!learners.ContainsKey(rawEarningMathsEnglish.LearnRefNumber))
                {
                    learners.Add(rawEarningMathsEnglish.LearnRefNumber, new Learner(_collectionPeriodRepository.GetAllCollectionPeriods()));
                }

                learners[rawEarningMathsEnglish.LearnRefNumber].RawEarningsMathsEnglish.Add(rawEarningMathsEnglish);
            }

            foreach (var historicalPayment in _historicalPaymentsRepository.GetAllForProvider(ukprn))
            {
                if (!learners.ContainsKey(historicalPayment.LearnRefNumber))
                {
                    learners.Add(historicalPayment.LearnRefNumber, new Learner(_collectionPeriodRepository.GetAllCollectionPeriods()));
                }

                learners[historicalPayment.LearnRefNumber].HistoricalPayments.Add(historicalPayment);
            }

            foreach (var dataLock in _dataLockRepository.GetAllForProvider(ukprn))
            {
                if (!learners.ContainsKey(dataLock.LearnRefNumber))
                {
                    learners.Add(dataLock.LearnRefNumber, new Learner(_collectionPeriodRepository.GetAllCollectionPeriods()));
                }

                learners[dataLock.LearnRefNumber].DataLocks.Add(dataLock);
            }

            return learners;
        }
    }
}