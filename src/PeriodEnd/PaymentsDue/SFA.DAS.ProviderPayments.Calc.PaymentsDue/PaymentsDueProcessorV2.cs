using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class PaymentsDueProcessorV2
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IPayableEarningsCalculator _payableEarningsCalculator;
        private readonly IRequiredPaymentsHistoryRepository _paymentsHistoryRepository;
        private IRequiredPaymentRepository _requiredPaymentRepository;
        private IProviderLearnersBuilder _providerLearnersBuilder;

        public PaymentsDueProcessorV2(IProviderRepository providerRepository, IPayableEarningsCalculator payableEarningsCalculator, IRequiredPaymentsHistoryRepository paymentsHistoryRepository)
        {
            _providerRepository = providerRepository;
            _payableEarningsCalculator = payableEarningsCalculator;
            _paymentsHistoryRepository = paymentsHistoryRepository;
        }

        public virtual void Process()
        {
            var providers = _providerRepository.GetAllProviders();

            // get all data here.
            //var providerContext = new { rawEarnings=new {}, me=0, prev=0, dl=0 }; // context builder
            //var distinctLearners = new List<Learner>(); // get distinct learners by ukprn and learn ref number.

            foreach (var provider in providers)
            {
                var providerLearners = _providerLearnersBuilder.Build(provider.Ukprn);

                foreach (var learner in providerLearners)
                {
                    
                }


                var comparablePayableEarnings = _payableEarningsCalculator.Calculate(provider.Ukprn)
                    .Select(earning => new ComparableEarning() { Ukprn = earning.Ukprn, PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier, LearnRefNumber = earning.LearnRefNumber});
                var comparableHistoricalEarnings = new List<RequiredPaymentsHistoryEntity>()
                    .Select(earning => new ComparableEarning() { Ukprn = earning.Ukprn, PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier });

                /*var intersection = new HashSet<Tuple<long, string>>(payableEarnings);//todo: some common type between rawearning and historical payment (just the keys required)
                intersection.IntersectWith(historicPayments);

                var payments = payableEarnings.Except(intersection);
                var refunds = historicPayments.Except(intersection);

                //todo: save payments and refunds somewhere.
                _requiredPaymentRepository.AddRequiredPayments(new RequiredPaymentEntity[]{});*/

                // eaernings for txn1 minus past pmts for txn1 = amt due for txn1... etc per learner per aim (and other things)
            }
        }
    }

/*    public class PayableEarningsCalculator : IPayableEarningsCalculator
    {
        private IRawEarningsRepository _rawEarningsRepository;

        public List<PayableEarning> Calculate(long ukprn)
        {
            throw new System.NotImplementedException();
            var rawEarningsForProvider = _rawEarningsRepository.GetAllForProvider(ukprn);

            var act1RawEarnings = rawEarningsForProvider.Where(entity => entity.Act == 1);
            // todo: exclude data lock failures
            // todo: link to maths and english
            // gives payable earnings.
        }
    }*/
}