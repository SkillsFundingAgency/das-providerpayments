using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Repositories;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services
{
    public class LearnerBuilder : ILearnerBuilder
    {
        private readonly IRequiredPaymentRepository _requiredPaymentRepository;
        private readonly IHistoricalPaymentsRepository _historicalPaymentsRepository;

        private Dictionary<string, LearnerData> _learnerProcessParameters;
       
        public LearnerBuilder(
            IRequiredPaymentRepository requiredPaymentRepository,
            IHistoricalPaymentsRepository historicalPaymentsRepository)
        {
            _requiredPaymentRepository = requiredPaymentRepository;
            _historicalPaymentsRepository = historicalPaymentsRepository;
        }

        public List<LearnerData> CreateLearnersForProvider(long ukprn)
        {
            ResetLearnerResultsList();
            
            foreach (var requiredRefund in _requiredPaymentRepository.GetRefundsForProvider(ukprn))
            {
                GetLearnerProcessParametersInstanceForLearner(requiredRefund.LearnRefNumber).RequiredRefunds.Add(requiredRefund);
            }

            foreach (var historicalPayment in _historicalPaymentsRepository.GetAllForProvider(ukprn))
            {
                GetLearnerProcessParametersInstanceForLearner(historicalPayment.LearnRefNumber).HistoricalPayments.Add(new HistoricalPayment(historicalPayment));
            }

            return _learnerProcessParameters.Values.ToList();
        }

        private void ResetLearnerResultsList()
        {
            _learnerProcessParameters = new Dictionary<string, LearnerData>();
        }

        private LearnerData GetLearnerProcessParametersInstanceForLearner(string learnerRefNumber)
        {
            LearnerData instance;
            if (!_learnerProcessParameters.ContainsKey(learnerRefNumber))
            {
                instance = new LearnerData(learnerRefNumber);
                _learnerProcessParameters.Add(learnerRefNumber, instance);
            }
            else
            {
                instance = _learnerProcessParameters[learnerRefNumber];
            }

            return instance;
        }
    }
}