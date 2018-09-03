using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class SortProviderDataIntoLearnerDataService : ICorrelateLearnerData
    {
        private readonly IRawEarningsRepository _rawEarningsRepository;
        private readonly IRawEarningsMathsEnglishRepository _rawEarningsMathsEnglishRepository;
        private readonly IRequiredPaymentsHistoryRepository _historicalPaymentsRepository;
        private readonly IDatalockRepository _dataLockRepository;
        private readonly ICommitmentRepository _commitmentsRepository;
        private readonly ICollectionPeriodRepository _collectionPeriodRepository;

        public SortProviderDataIntoLearnerDataService(
            IRawEarningsRepository rawEarningsRepository,
            IRawEarningsMathsEnglishRepository rawEarningsMathsEnglishRepository,
            IRequiredPaymentsHistoryRepository historicalPaymentsRepository,
            IDatalockRepository dataLockRepository,
            ICommitmentRepository commitmentsRepository,
            ICollectionPeriodRepository collectionPeriodRepository)
        {
            _rawEarningsRepository = rawEarningsRepository;
            _rawEarningsMathsEnglishRepository = rawEarningsMathsEnglishRepository;
            _historicalPaymentsRepository = historicalPaymentsRepository;
            _dataLockRepository = dataLockRepository;
            _commitmentsRepository = commitmentsRepository;
            _collectionPeriodRepository = collectionPeriodRepository;
        }

        public List<LearnerData> Correlate(long ukprn)
        {
            var learnerResults = new SortLearnerResults();

            var allCollectionPeriods = _collectionPeriodRepository.GetAllCollectionPeriods();
            var periodToMonthMapper = allCollectionPeriods.ToDictionary(x => x.Id, x => x.Month);
            var periodToYearMapper = allCollectionPeriods.ToDictionary(x => x.Id, x => x.Year);
            
            foreach (var rawEarning in _rawEarningsRepository.GetAllForProvider(ukprn))
            {
                rawEarning.DeliveryMonth = periodToMonthMapper[rawEarning.Period];
                rawEarning.DeliveryYear = periodToYearMapper[rawEarning.Period];

                learnerResults.GetLearnerProcessParametersInstanceForLearner(rawEarning.LearnRefNumber, rawEarning.Uln).RawEarnings.Add(rawEarning);
            }

            foreach (var rawEarningMathsEnglish in _rawEarningsMathsEnglishRepository.GetAllForProvider(ukprn))
            {
                rawEarningMathsEnglish.DeliveryMonth = periodToMonthMapper[rawEarningMathsEnglish.Period];
                rawEarningMathsEnglish.DeliveryYear = periodToYearMapper[rawEarningMathsEnglish.Period];

                learnerResults.GetLearnerProcessParametersInstanceForLearner(rawEarningMathsEnglish.LearnRefNumber, rawEarningMathsEnglish.Uln).RawEarningsMathsEnglish.Add(rawEarningMathsEnglish);
            }

            foreach (var historicalPayment in _historicalPaymentsRepository.GetAllForProvider(ukprn))
            {
                learnerResults.GetLearnerProcessParametersInstanceForLearner(historicalPayment.LearnRefNumber, historicalPayment.Uln).HistoricalRequiredPayments.Add(historicalPayment);
            }

            foreach (var dataLock in _dataLockRepository.GetDatalockOutputForProvider(ukprn))
            {
                learnerResults.GetLearnerProcessParametersInstanceForLearner(dataLock.LearnRefNumber).DataLocks.Add(dataLock);
            }

            foreach (var datalockValidationError in _dataLockRepository.GetValidationErrorsForProvider(ukprn))
            {
                learnerResults.GetLearnerProcessParametersInstanceForLearner(datalockValidationError.LearnRefNumber).DatalockValidationErrors.Add(datalockValidationError);
            }

            foreach (var commitment in _commitmentsRepository.GetProviderCommitments(ukprn))
            {
                learnerResults.GetLearnerProcessParametersInstanceForLearner(commitment.Uln)?.Commitments.Add(commitment);
            }

            return learnerResults.LearnerProcessParameters.Values.ToList();
        }

    }

    class SortLearnerResults
    {
        public Dictionary<string, LearnerData> LearnerProcessParameters { get; }
        internal Dictionary<long, string> UlnToLearnerRefNumber { get; }

        public SortLearnerResults()
        {
            LearnerProcessParameters = new Dictionary<string, LearnerData>();
            UlnToLearnerRefNumber = new Dictionary<long, string>();
        }
    }

    static class SortLearnerResultsExtensions
    {
        internal static LearnerData GetLearnerProcessParametersInstanceForLearner(this SortLearnerResults sortLearnerResults, string learnerRefNumber, long? uln = null)
        {
            LearnerData instance;
            if (!sortLearnerResults.LearnerProcessParameters.ContainsKey(learnerRefNumber))
            {
                instance = new LearnerData(learnerRefNumber, uln);
                sortLearnerResults.LearnerProcessParameters.Add(learnerRefNumber, instance);
            }
            else
            {
                instance = sortLearnerResults.LearnerProcessParameters[learnerRefNumber];
            }

            // Add Uln to mapping dictionaries if a Uln exists
            if (uln != null)
            {
                sortLearnerResults.AddToMappingUlnToLearnerRefNumber(uln.Value, learnerRefNumber);
            }

            return instance;
        }
        internal static LearnerData GetLearnerProcessParametersInstanceForLearner(this SortLearnerResults sortLearnerResults, long uln)
        {
            var learnerRefNumber = sortLearnerResults.LookupLearnerRefNumber(uln);
            // TODO What do we do if no match is found? At the moment we return a null result
            if (learnerRefNumber == null)
            {
                return null;
            }
            return sortLearnerResults.GetLearnerProcessParametersInstanceForLearner(learnerRefNumber, uln);
        }

        internal static void AddToMappingUlnToLearnerRefNumber(this SortLearnerResults sortLearnerResults, long uln, string learnerRefNumber)
        {
            // TODO Discuss if a mapping already exists (and it's different from our expectation),what do we do?
            if (!sortLearnerResults.UlnToLearnerRefNumber.ContainsKey(uln))
            {
                sortLearnerResults.UlnToLearnerRefNumber.Add(uln, learnerRefNumber);
            }
        }
        internal static string LookupLearnerRefNumber(this SortLearnerResults sortLearnerResults, long uln)
        {
            if (sortLearnerResults.UlnToLearnerRefNumber.ContainsKey(uln))
            {
                return sortLearnerResults.UlnToLearnerRefNumber[uln];
            }
            return null;
        }

    }
}