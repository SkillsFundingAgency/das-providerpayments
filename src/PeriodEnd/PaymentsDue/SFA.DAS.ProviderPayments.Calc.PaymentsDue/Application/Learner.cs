using System.Collections.Generic;
using System.Linq;
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

        public List<RawEarningEntity> PayableEarnings { get; set; }
        public List<RawEarningEntity> NonPayableEarnings { get; set; }

        public void ValidateDatalocks()
        {

        }
    }
}