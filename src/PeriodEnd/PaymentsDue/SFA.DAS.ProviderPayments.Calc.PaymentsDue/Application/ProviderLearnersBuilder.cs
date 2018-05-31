using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public class ProviderLearnersBuilder : IProviderLearnersBuilder
    {
        private readonly IRawEarningsRepository _rawEarningsRepository;
        private readonly IRawEarningsMathsEnglishRepository _rawEarningsMathsEnglishRepository;
        private readonly IRequiredPaymentsHistoryRepository _historicalPaymentsRepository;
        private readonly IDataLockPriceEpisodePeriodMatchesRepository _dataLockRepository;

        public ProviderLearnersBuilder(
            IRawEarningsRepository rawEarningsRepository, 
            IRawEarningsMathsEnglishRepository rawEarningsMathsEnglishRepository, 
            IRequiredPaymentsHistoryRepository historicalPaymentsRepository, 
            IDataLockPriceEpisodePeriodMatchesRepository dataLockRepository)
        {
            _rawEarningsRepository = rawEarningsRepository;
            _rawEarningsMathsEnglishRepository = rawEarningsMathsEnglishRepository;
            _historicalPaymentsRepository = historicalPaymentsRepository;
            _dataLockRepository = dataLockRepository;
        }

        public Dictionary<string, Learner> Build(long ukprn)
        {
            var learners = new Dictionary<string, Learner>();

            foreach (var rawEarning in _rawEarningsRepository.GetAllForProvider(ukprn))
            {
                if (!learners.ContainsKey(rawEarning.LearnRefNumber))
                {
                    learners.Add(rawEarning.LearnRefNumber, new Learner());
                }

                learners[rawEarning.LearnRefNumber].RawEarnings.Add(rawEarning);
            }

            foreach (var rawEarningMathsEnglish in _rawEarningsMathsEnglishRepository.GetAllForProvider(ukprn))
            {
                if (!learners.ContainsKey(rawEarningMathsEnglish.LearnRefNumber))
                {
                    learners.Add(rawEarningMathsEnglish.LearnRefNumber, new Learner());
                }

                learners[rawEarningMathsEnglish.LearnRefNumber].RawEarningsMathsEnglish.Add(rawEarningMathsEnglish);
            }

            foreach (var historicalPayment in _historicalPaymentsRepository.GetAllForProvider(ukprn))
            {
                if (!learners.ContainsKey(historicalPayment.LearnRefNumber))
                {
                    learners.Add(historicalPayment.LearnRefNumber, new Learner());
                }

                learners[historicalPayment.LearnRefNumber].HistoricalPayments.Add(historicalPayment);
            }

            foreach (var dataLock in _dataLockRepository.GetAllForProvider(ukprn))
            {
                if (!learners.ContainsKey(dataLock.LearnRefNumber))
                {
                    learners.Add(dataLock.LearnRefNumber, new Learner());
                }

                learners[dataLock.LearnRefNumber].DataLocks.Add(dataLock);
            }

            return learners;
        }
    }
}