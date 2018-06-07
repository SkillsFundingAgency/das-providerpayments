using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public class LearnerProcessParametersBuilder : ILearnerProcessParametersBuilder
    {
        private readonly IRawEarningsRepository _rawEarningsRepository;
        private readonly IRawEarningsMathsEnglishRepository _rawEarningsMathsEnglishRepository;
        private readonly IRequiredPaymentsHistoryRepository _historicalPaymentsRepository;
        private readonly IDataLockPriceEpisodePeriodMatchesRepository _dataLockRepository;
        private readonly ICommitmentRepository _commitmentsRepository;
        private Dictionary<string, LearnerProcessParameters> _learnerProcessParameters;
        private Dictionary<long, string> _ulnToLearnerRefNumber;
        private Dictionary<string, long> _learnerRefNumberToUln;

        public LearnerProcessParametersBuilder(
            IRawEarningsRepository rawEarningsRepository, 
            IRawEarningsMathsEnglishRepository rawEarningsMathsEnglishRepository, 
            IRequiredPaymentsHistoryRepository historicalPaymentsRepository, 
            IDataLockPriceEpisodePeriodMatchesRepository dataLockRepository,
            ICommitmentRepository commitmentsRepository)
        {
            _rawEarningsRepository = rawEarningsRepository;
            _rawEarningsMathsEnglishRepository = rawEarningsMathsEnglishRepository;
            _historicalPaymentsRepository = historicalPaymentsRepository;
            _dataLockRepository = dataLockRepository;
            _commitmentsRepository = commitmentsRepository;
        }

        public List<LearnerProcessParameters> Build(long ukprn)
        {
            ResetLearnerResultsList();

            foreach (var rawEarning in _rawEarningsRepository.GetAllForProvider(ukprn))
            {
                GetLearnerProcessParametersInstanceForLearner(rawEarning.LearnRefNumber, rawEarning.Uln).RawEarnings.Add(rawEarning);
            }

            foreach (var rawEarningMathsEnglish in _rawEarningsMathsEnglishRepository.GetAllForProvider(ukprn))
            {
                GetLearnerProcessParametersInstanceForLearner(rawEarningMathsEnglish.LearnRefNumber, rawEarningMathsEnglish.Uln).RawEarningsMathsEnglish.Add(rawEarningMathsEnglish);
            }

            foreach (var historicalPayment in _historicalPaymentsRepository.GetAllForProvider(ukprn))
            {
                GetLearnerProcessParametersInstanceForLearner(historicalPayment.LearnRefNumber).HistoricalPayments.Add(historicalPayment);
            }

            foreach (var dataLock in _dataLockRepository.GetAllForProvider(ukprn))
            {
                GetLearnerProcessParametersInstanceForLearner(dataLock.LearnRefNumber).DataLocks.Add(dataLock);
            }

            foreach (var commitment in _commitmentsRepository.GetProviderCommitments(ukprn))
            {
                //GetLearnerProcessParametersInstanceForLearner(learners, _c.LearnRefNumber).DataLocks.Add(dataLock);
            }

            return _learnerProcessParameters.Values.ToList();
        }

        private void ResetLearnerResultsList()
        {
            _learnerProcessParameters = new Dictionary<string, LearnerProcessParameters>();
            _ulnToLearnerRefNumber = new Dictionary<long, string>();
            _learnerRefNumberToUln = new Dictionary<string, long>();
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
            if (uln == null)
            {
                uln = LookupUln(learnerRefNumber);
            }

            LearnerProcessParameters instance; 
            if (!_learnerProcessParameters.ContainsKey(learnerRefNumber))
            {
                instance = new LearnerProcessParameters(learnerRefNumber, uln);
                _learnerProcessParameters.Add(learnerRefNumber, instance);
            }
            else
            {
                instance = _learnerProcessParameters[learnerRefNumber];
            }

            if (uln != null)
            {
                AddToMappingUlnToLearnerRefNumber(uln.Value, learnerRefNumber);
            }

            AddToMappingLearnerRefNumberToUln(uln, learnerRefNumber);

            return instance;
        }

        private void AddToMappingLearnerRefNumberToUln(string learnerRefNumber, long uln)
        {
            if (!_learnerRefNumberToUln.ContainsKey(learnerRefNumber))
            {
                _learnerRefNumberToUln.Add(learnerRefNumber, uln);
            }
        }

        private void AddToMappingUlnToLearnerRefNumber(long uln, string learnerRefNumber)
        {
            // TODO Discuss if a mapping already exists (and it's different from our expectation),what do we do?
            if (!_ulnToLearnerRefNumber.ContainsKey(uln))
            {
                _ulnToLearnerRefNumber.Add(uln, learnerRefNumber);
            }
        }

        private string LookupLearnerRefNumber(long? uln)
        {
            throw new System.NotImplementedException();
        }

        private long? LookupUln(string learnerRefNumber)
        {
            throw new System.NotImplementedException();
        }
    }
}