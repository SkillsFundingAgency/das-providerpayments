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
        
        private IRequiredPaymentsHistoryRepository _paymentsHistoryRepository;
        private readonly IPayableEarningsCalculator _payableEarningsCalculator;
        private readonly IProviderRepository _providerRepository;
        private IRequiredPaymentRepository _requiredPaymentRepository;

        public PaymentsDueProcessorV2(IProviderRepository providerRepository, IPayableEarningsCalculator payableEarningsCalculator)
        {
            _providerRepository = providerRepository;
            _payableEarningsCalculator = payableEarningsCalculator;
        }

        public virtual void Process()
        {
            var providers = _providerRepository.GetAllProviders();

            // get all data here.
            //var providerContext = new { rawEarnings=new {}, me=0, prev=0, dl=0 }; // context builder
            //var distinctLearners = new List<Learner>(); // get distinct learners by ukprn and learn ref number.

            foreach (var provider in providers)
            {
                var payableEarnings = _payableEarningsCalculator.Calculate(provider.Ukprn)
                    .Select(earning => new Tuple<long, string>(earning.Uln, earning.PriceEpisodeIdentifier));
                /*var historicPayments = _paymentsHistoryRepository.GetAllForProvider(provider.Ukprn)
                    .Select(earning => new Tuple<long, string>(earning.Uln, earning.PriceEpisodeIdentifier));

                var intersection = new HashSet<Tuple<long, string>>(payableEarnings);//todo: some common type between rawearning and historical payment (just the keys required)
                intersection.IntersectWith(historicPayments);

                var payments = payableEarnings.Except(intersection);
                var refunds = historicPayments.Except(intersection);

                //todo: save payments and refunds somewhere.
                _requiredPaymentRepository.AddRequiredPayments(new RequiredPaymentEntity[]{});*/
            }
        }
    }

    public class Learner
    {
        public List<RawEarningEntity> RawEarningEntities { get; set; }
        public List<RawEarningMathsEnglishEntity> RawEarningMathsEnglishEntities { get; set; }
        public List<DataLockPriceEpisodePeriodMatchEntity> DataLockPriceEpisodePeriodMatchEntities { get; set; }

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