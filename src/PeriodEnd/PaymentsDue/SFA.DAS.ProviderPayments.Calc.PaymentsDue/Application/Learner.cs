using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public class Learner
    {
        public List<RawEarningEntity> RawEarnings { get; set; } = new List<RawEarningEntity>();
        public List<RawEarningMathsEnglishEntity> RawEarningsMathsEnglish { get; set; } = new List<RawEarningMathsEnglishEntity>();
        public List<DataLockPriceEpisodePeriodMatchEntity> DataLocks { get; set; } = new List<DataLockPriceEpisodePeriodMatchEntity>();
        public List<RequiredPaymentsHistoryEntity> HistoricalPayments { get; set; } = new List<RequiredPaymentsHistoryEntity>();
        public List<RequiredPaymentEntity> RequiredPayments { get; set; } = new List<RequiredPaymentEntity>();

        public List<string> DatalockErrors { get; private set; } = new List<string>();

        private IEnumerable<RawEarningEntity> Act1RawEarnings => RawEarnings.Where(x => x.Act == 1);
        private IEnumerable<RawEarningEntity> Act2RawEarnings => RawEarnings.Where(x => x.Act == 2);

        public List<FundingDueEntry> PayableEarnings { get; set; }
        public List<NonPayableFundingDue> NonPayableEarnings { get; set; }

        public void ValidateDatalocks()
        {
            var act1 = Act1RawEarnings.ToList();
            // if there are *no* ACT1 earnings, then everything is ACT2 and payable
            if (act1.Count == 0)
            {
                PayableEarnings.AddRange(RawEarnings.SelectMany(x => new FundingDue(x).FundingDueLines));
                PayableEarnings.AddRange(RawEarningsMathsEnglish.SelectMany(x => new FundingDue(x).FundingDueLines));
                return;
            }

            var act2 = Act2RawEarnings.ToList();
        }

        public void CalculateFundingDue()
        {
            
        }
    }
}