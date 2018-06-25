using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class SortProviderDataIntoLearnerData : ISortProviderDataIntoLearnerData
    {
        private readonly IRawEarningsRepository _rawEarningsRepository;
        private readonly IRawEarningsMathsEnglishRepository _rawEarningsMathsEnglishRepository;
        private readonly IRequiredPaymentsHistoryRepository _historicalPaymentsRepository;
        private readonly IDatalockRepository _dataLockRepository;
        private readonly ICommitmentRepository _commitmentsRepository;
        private readonly ICollectionPeriodRepository _collectionPeriodRepository;

        private Dictionary<string, LearnerData> _learnerProcessParameters;
        private Dictionary<long, string> _ulnToLearnerRefNumber;
       
        public SortProviderDataIntoLearnerData(
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

        public List<LearnerData> Sort(long ukprn)
        {
            ResetLearnerResultsList();
            
            var allCollectionPeriods = _collectionPeriodRepository.GetAllCollectionPeriods();
            var periodToMonthMapper = allCollectionPeriods.ToDictionary(x => x.Id, x => x.Month);
            var periodToYearMapper = allCollectionPeriods.ToDictionary(x => x.Id, x => x.Year);
            
            foreach (var rawEarning in _rawEarningsRepository.GetAllForProvider(ukprn))
            {
                rawEarning.DeliveryMonth = periodToMonthMapper[rawEarning.Period];
                rawEarning.DeliveryYear = periodToYearMapper[rawEarning.Period];

                GetLearnerProcessParametersInstanceForLearner(rawEarning.LearnRefNumber, rawEarning.Uln).RawEarnings.Add(rawEarning);
            }

            foreach (var rawEarningMathsEnglish in _rawEarningsMathsEnglishRepository.GetAllForProvider(ukprn))
            {
                rawEarningMathsEnglish.DeliveryMonth = periodToMonthMapper[rawEarningMathsEnglish.Period];
                rawEarningMathsEnglish.DeliveryYear = periodToYearMapper[rawEarningMathsEnglish.Period];

                GetLearnerProcessParametersInstanceForLearner(rawEarningMathsEnglish.LearnRefNumber, rawEarningMathsEnglish.Uln).RawEarningsMathsEnglish.Add(rawEarningMathsEnglish);
            }

            foreach (var historicalPayment in _historicalPaymentsRepository.GetAllForProvider(ukprn))
            {
                GetLearnerProcessParametersInstanceForLearner(historicalPayment.LearnRefNumber, historicalPayment.Uln).HistoricalPayments.Add(historicalPayment);
            }

            foreach (var dataLock in _dataLockRepository.GetDatalockOutputForProvider(ukprn))
            {
                GetLearnerProcessParametersInstanceForLearner(dataLock.LearnRefNumber).DataLocks.Add(dataLock);
            }

            foreach (var datalockValidationError in _dataLockRepository.GetValidationErrorsForProvider(ukprn))
            {
                GetLearnerProcessParametersInstanceForLearner(datalockValidationError.LearnRefNumber).DatalockValidationErrors.Add(datalockValidationError);
            }

            foreach (var commitment in _commitmentsRepository.GetProviderCommitments(ukprn))
            {
                GetLearnerProcessParametersInstanceForLearner(commitment.Uln)?.Commitments.Add(commitment);
            }

            return _learnerProcessParameters.Values.ToList();
        }

        private void ResetLearnerResultsList()
        {
            _learnerProcessParameters = new Dictionary<string, LearnerData>();
            _ulnToLearnerRefNumber = new Dictionary<long, string>();
        }

        private LearnerData GetLearnerProcessParametersInstanceForLearner(long uln)
        {
            var learnerRefNumber = LookupLearnerRefNumber(uln);
            // TODO What do we do if no match is found? At the moment we return a null result
            if (learnerRefNumber == null)
            {
                return null;
            }
            return GetLearnerProcessParametersInstanceForLearner(learnerRefNumber, uln);
        }

        private LearnerData GetLearnerProcessParametersInstanceForLearner(string learnerRefNumber, long? uln = null)
        {
            LearnerData instance;
            if (!_learnerProcessParameters.ContainsKey(learnerRefNumber))
            {
                instance = new LearnerData(learnerRefNumber, uln);
                _learnerProcessParameters.Add(learnerRefNumber, instance);
            }
            else
            {
                instance = _learnerProcessParameters[learnerRefNumber];
            }

            // Add Uln to mapping dictionaries if a Uln exists
            if (uln != null)
            {
                AddToMappingUlnToLearnerRefNumber(uln.Value, learnerRefNumber);
            }

            return instance;
        }

        private void AddToMappingUlnToLearnerRefNumber(long uln, string learnerRefNumber)
        {
            // TODO Discuss if a mapping already exists (and it's different from our expectation),what do we do?
            if (!_ulnToLearnerRefNumber.ContainsKey(uln))
            {
                _ulnToLearnerRefNumber.Add(uln, learnerRefNumber);
            }
        }

        private string LookupLearnerRefNumber(long uln)
        {
            if (_ulnToLearnerRefNumber.ContainsKey(uln))
            {
                return _ulnToLearnerRefNumber[uln];
            }

            return null;
        }
    }
}