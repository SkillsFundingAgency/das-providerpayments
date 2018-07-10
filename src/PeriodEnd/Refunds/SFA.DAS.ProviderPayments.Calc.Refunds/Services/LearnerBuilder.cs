using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Refunds.Domain;
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

        public LearnerBuilder(
            IRequiredPaymentRepository requiredPaymentRepository,
            IHistoricalPaymentsRepository historicalPaymentsRepository)
        {
            _requiredPaymentRepository = requiredPaymentRepository;
            _historicalPaymentsRepository = historicalPaymentsRepository;
        }

        public List<LearnerData> CreateLearnersForProvider(long ukprn)
        {
            var learnersDictionary = new Dictionary<string, LearnerData>();
            
            foreach (var requiredRefund in _requiredPaymentRepository.GetRefundsForProvider(ukprn))
            {
                learnersDictionary.GetOrCreateLearnerInstance(requiredRefund.LearnRefNumber).RequiredRefunds.Add(requiredRefund);
            }

            foreach (var historicalPayment in _historicalPaymentsRepository.GetAllForProvider(ukprn))
            {
                learnersDictionary.GetOrCreateLearnerInstance(historicalPayment.LearnRefNumber).HistoricalPayments.Add(new HistoricalPayment(historicalPayment));
            }

            return learnersDictionary.Values.ToList();
        }

    }
    static class LearnerDictionaryExtensions
    {
        internal static LearnerData GetOrCreateLearnerInstance(this Dictionary<string, LearnerData> dictionary, string learnerRefNumber)
        {
            LearnerData instance;
            if (!dictionary.ContainsKey(learnerRefNumber))
            {
                instance = new LearnerData(learnerRefNumber);
                dictionary.Add(learnerRefNumber, instance);
            }
            else
            {
                instance = dictionary[learnerRefNumber];
            }

            return instance;
        }

    }

}