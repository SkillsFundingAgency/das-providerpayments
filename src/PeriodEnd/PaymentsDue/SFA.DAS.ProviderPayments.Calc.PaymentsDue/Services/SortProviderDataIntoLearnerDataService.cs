using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;
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
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICollectionPeriodRepository _collectionPeriodRepository;
        
        public SortProviderDataIntoLearnerDataService(
            IRawEarningsRepository rawEarningsRepository,
            IRawEarningsMathsEnglishRepository rawEarningsMathsEnglishRepository,
            IRequiredPaymentsHistoryRepository historicalPaymentsRepository,
            IDatalockRepository dataLockRepository,
            ICommitmentRepository commitmentsRepository,
            ICollectionPeriodRepository collectionPeriodRepository, 
            IPaymentRepository paymentRepository)
        {
            _rawEarningsRepository = rawEarningsRepository;
            _rawEarningsMathsEnglishRepository = rawEarningsMathsEnglishRepository;
            _historicalPaymentsRepository = historicalPaymentsRepository;
            _dataLockRepository = dataLockRepository;
            _commitmentsRepository = commitmentsRepository;
            _paymentRepository = paymentRepository;
            _collectionPeriodRepository = collectionPeriodRepository;
        }

        public List<LearnerData> CreateLearnerDataForProvider(long ukprn)
        {
            var learnerResults = new SortLearnerResults();

            var allCollectionPeriods = _collectionPeriodRepository.GetAllCollectionPeriods();
            var periodToMonthMapper = allCollectionPeriods.ToDictionary(x => x.Id, x => x.Month);
            var periodToYearMapper = allCollectionPeriods.ToDictionary(x => x.Id, x => x.Year);

            foreach (var rawEarning in _rawEarningsRepository.GetAllForProvider(ukprn))
            {
                rawEarning.DeliveryMonth = periodToMonthMapper[rawEarning.Period];
                rawEarning.DeliveryYear = periodToYearMapper[rawEarning.Period];

                learnerResults
                    .GetLearnerDataForLearner(rawEarning.LearnRefNumber, rawEarning.Uln)
                    .RawEarnings
                    .Add(rawEarning);
            }

            foreach (var rawEarningMathsEnglish in _rawEarningsMathsEnglishRepository.GetAllForProvider(ukprn))
            {
                rawEarningMathsEnglish.DeliveryMonth = periodToMonthMapper[rawEarningMathsEnglish.Period];
                rawEarningMathsEnglish.DeliveryYear = periodToYearMapper[rawEarningMathsEnglish.Period];

                learnerResults
                    .GetLearnerDataForLearner(rawEarningMathsEnglish.LearnRefNumber, rawEarningMathsEnglish.Uln)
                    .RawEarningsMathsEnglish
                    .Add(rawEarningMathsEnglish);
            }

            foreach (var paymentEntity in _paymentRepository.GetRoundedDownEmployerPaymentsForProvider(ukprn))
            {
                learnerResults
                    .GetLearnerDataForLearner(paymentEntity.LearnRefNumber)
                    .HistoricalEmployerPayments
                    .Add(paymentEntity);
            }

            foreach (var historicalPayment in _historicalPaymentsRepository.GetAllForProvider(ukprn))
            {
                learnerResults
                    .GetLearnerDataForLearner(historicalPayment.LearnRefNumber, historicalPayment.Uln)
                    .HistoricalRequiredPayments
                    .Add(historicalPayment);
            }

            foreach (var dataLock in _dataLockRepository.GetDatalockOutputForProvider(ukprn))
            {
                learnerResults
                    .GetLearnerDataForLearner(dataLock.LearnRefNumber)
                    .DataLocks
                    .Add(dataLock);
            }

            foreach (var datalockValidationError in _dataLockRepository.GetValidationErrorsForProvider(ukprn))
            {
                learnerResults
                    .GetLearnerDataForLearner(datalockValidationError.LearnRefNumber)
                    .DatalockValidationErrors
                    .Add(datalockValidationError);
            }

            foreach (var commitment in _commitmentsRepository.GetProviderCommitments(ukprn))
            {
                learnerResults
                    .GetLearnerDataForLearner(commitment.Uln)?
                    .Commitments
                    .Add(commitment);
            }

            return learnerResults.AllLearnerData();
        }
    }

    class SortLearnerResults
    {
        private Dictionary<string, LearnerData> LearnerData { get; }
        private Dictionary<long, string> UlnToLearnRefNumberMapping { get; }

        public SortLearnerResults()
        {
            LearnerData = new Dictionary<string, LearnerData>();
            UlnToLearnRefNumberMapping = new Dictionary<long, string>();
        }

        public List<LearnerData> AllLearnerData()
        {
            return LearnerData.Values.ToList();
        }

        public LearnerData GetLearnerDataForLearner(string learnRefNumber, long? uln = null)
        {
            LearnerData instance;
            if (!LearnerData.ContainsKey(learnRefNumber))
            {
                instance = new LearnerData(learnRefNumber, uln);
                LearnerData.Add(learnRefNumber, instance);
            }
            else
            {
                instance = LearnerData[learnRefNumber];
            }

            // Add Uln to mapping dictionaries if a Uln exists
            if (uln != null)
            {
                AddUlnToLearnRefNumberMapping(uln.Value, learnRefNumber);
            }

            return instance;
        }

        private void AddUlnToLearnRefNumberMapping(long uln, string learnRefNumber)
        {
            // TODO Discuss if a mapping already exists (and it's different from our expectation),what do we do?
            if (!UlnToLearnRefNumberMapping.ContainsKey(uln))
            {
                UlnToLearnRefNumberMapping.Add(uln, learnRefNumber);
            }
        }

        public LearnerData GetLearnerDataForLearner(long uln)
        {
            var learnerRefNumber = LookupLearnRefNumber(uln);
            // TODO What do we do if no match is found? At the moment we return a null result
            if (learnerRefNumber == null)
            {
                return null;
            }
            return GetLearnerDataForLearner(learnerRefNumber, uln);
        }

        private string LookupLearnRefNumber(long uln)
        {
            if (UlnToLearnRefNumberMapping.ContainsKey(uln))
            {
                return UlnToLearnRefNumberMapping[uln];
            }
            return null;
        }
    }
}