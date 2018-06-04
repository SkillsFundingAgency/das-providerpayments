using System.Collections.Generic;
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
    }
}