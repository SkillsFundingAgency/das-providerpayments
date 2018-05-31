using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
                var historicalPayments = _paymentsHistoryRepository.GetAllForProvider(provider.Ukprn);

                var providerLearners = _providerLearnersBuilder.Build(provider.Ukprn);
                foreach (var historicalPayment in historicalPayments)
                {
                    if (!providerLearners.ContainsKey(historicalPayment.LearnRefNumber))
                    {
                        var learner = new Learner();
                        providerLearners.Add(historicalPayment.LearnRefNumber, learner);
                    }

                    providerLearners[historicalPayment.LearnRefNumber].RequiredPaymentsHistoryEntities.Add(historicalPayment);
                }

                foreach (var learner in providerLearners)
                {
                    
                }


                var comparablePayableEarnings = _payableEarningsCalculator.Calculate(provider.Ukprn)
                    .Select(earning => new ComparableEarning() { Ukprn = earning.Ukprn, PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier, LearnRefNumber = earning.LearnRefNumber});
                var comparableHistoricalEarnings = historicalPayments
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

    public interface IProviderLearnersBuilder
    {
        Dictionary<string, Learner> Build(long ukprn);
    }

    public class Learner
    {
        public List<RawEarningEntity> RawEarningEntities { get; set; }
        public List<RawEarningMathsEnglishEntity> RawEarningMathsEnglishEntities { get; set; }
        public List<DataLockPriceEpisodePeriodMatchEntity> DataLockPriceEpisodePeriodMatchEntities { get; set; }
        public List<RequiredPaymentsHistoryEntity> RequiredPaymentsHistoryEntities { get; set; }

        public void CalculateFundingDue()
        {
            // get price episodes for learner - i.e. get rawEarnings for learner. based on ukprn and learnrefnumber
        }
    }

    public interface IPayableEarningsCalculator// dependent on data lock and maths english
    {
        List<PayableEarning> Calculate(long ukprn); //act1 raw earnings, 
    }

    public class PayableEarning// todo: this list of fields is currently yolo
    {
        [StringLength(12)]
        public string LearnRefNumber { get; set; }
        public long Ukprn { get; set; }
        public int PriceEpisodeAimSeqNumber { get; set; }
        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EpisodeStartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EpisodeEffectiveTnpStartDate { get; set; }
        public int Period { get; set; }
        public long Uln { get; set; }
        public int? ProgType { get; set; }
        public int? FworkCode { get; set; }
        public int? PwayCode { get; set; }
        public int? StdCode { get; set; }
        public decimal? PriceEpisodeSfaContribPct { get; set; }
        [StringLength(100)]
        public string PriceEpisodeFundLineType { get; set; }
        [StringLength(8)]
        public string LearnAimRef { get; set; }
        [DataType(DataType.Date)]
        public DateTime LearnStartDate { get; set; }
    }

    public class ComparableEarning
    {
        public string LearnRefNumber { get; set; }
        public long Ukprn { get; set; }
        public int PriceEpisodeAimSeqNumber { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public long Uln { get; set; }
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