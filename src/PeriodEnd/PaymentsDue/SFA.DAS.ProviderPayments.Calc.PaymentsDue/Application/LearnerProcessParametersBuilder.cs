using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public class LearnerProcessParametersBuilder : ILearnerProcessParametersBuilder
    {
        private readonly IRawEarningsRepository _rawEarningsRepository;
        private readonly IRawEarningsMathsEnglishRepository _rawEarningsMathsEnglishRepository;
        private readonly IRequiredPaymentsHistoryRepository _historicalPaymentsRepository;
        private readonly IDatalockOutputRepository _dataLockRepository;
        private readonly ICommitmentRepository _commitmentsRepository;
        private readonly ICollectionPeriodRepository _collectionPeriodRepository;
        
        private Dictionary<string, LearnerProcessParameters> _learnerProcessParameters;
        private Dictionary<long, string> _ulnToLearnerRefNumber;
        private int _yearAcademicYearStarted;

        public LearnerProcessParametersBuilder(
            IRawEarningsRepository rawEarningsRepository, 
            IRawEarningsMathsEnglishRepository rawEarningsMathsEnglishRepository, 
            IRequiredPaymentsHistoryRepository historicalPaymentsRepository, 
            IDatalockOutputRepository dataLockRepository,
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

        public List<LearnerProcessParameters> Build(long ukprn)
        {
            ResetLearnerResultsList();
            SetYearAcademicYearStarted();

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

            foreach (var dataLock in _dataLockRepository.GetAllForProvider(ukprn))
            {
                GetLearnerProcessParametersInstanceForLearner(dataLock.LearnRefNumber).DataLocks.Add(dataLock);
            }

            foreach (var commitment in _commitmentsRepository.GetProviderCommitments(ukprn))
            {
                GetLearnerProcessParametersInstanceForLearner(commitment.Uln)?.Commitments.Add(commitment);
            }

            return _learnerProcessParameters.Values.ToList();
        }

        private void SetYearAcademicYearStarted()
        {
            var currentCollectionPeriodAcademicYear = _collectionPeriodRepository.GetCurrentCollectionPeriod()?.AcademicYear??"1718";
            var startingYear = int.Parse(currentCollectionPeriodAcademicYear.Substring(0, 2)) + 2000; // will fail in 2100...
            _yearAcademicYearStarted = startingYear;
        }

        private void ResetLearnerResultsList()
        {
            _learnerProcessParameters = new Dictionary<string, LearnerProcessParameters>();
            _ulnToLearnerRefNumber = new Dictionary<long, string>();
        }

        private LearnerProcessParameters GetLearnerProcessParametersInstanceForLearner(long uln)
        {
            var learnerRefNumber = LookupLearnerRefNumber(uln);
            // TODO What do we do if no match is found? At the moment we return a null result
            if (learnerRefNumber == null)
            {
                return null;
            }
            return GetLearnerProcessParametersInstanceForLearner(learnerRefNumber, uln);
        }
        
        private LearnerProcessParameters GetLearnerProcessParametersInstanceForLearner(string learnerRefNumber, long? uln = null)
        {
            LearnerProcessParameters instance; 
            if (!_learnerProcessParameters.ContainsKey(learnerRefNumber))
            {
                instance = new LearnerProcessParameters(learnerRefNumber, uln)
                {
                    YearAcademicYearStarted = _yearAcademicYearStarted,
                };
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