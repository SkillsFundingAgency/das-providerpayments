using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class LearnerProcessParameters
    {
        public string LearnRefNumber { get; set; }
        public List<RawEarning> RawEarnings { get; set; } = new List<RawEarning>();
        public List<RawEarningForMathsOrEnglish> RawEarningsMathsEnglish { get; set; } = new List<RawEarningForMathsOrEnglish>();
        public List<DataLockPriceEpisodePeriodMatchEntity> DataLocks { get; set; } = new List<DataLockPriceEpisodePeriodMatchEntity>();
        public List<RequiredPaymentsHistoryEntity> HistoricalPayments { get; set; } = new List<RequiredPaymentsHistoryEntity>();
        public List<RequiredPaymentEntity> RequiredPayments { get; set; } = new List<RequiredPaymentEntity>();
        public List<Commitment> Commitments { get; set; } = new List<Commitment>();
        public List<CollectionPeriodEntity> CollectionPeriods { get; set; } = new List<CollectionPeriodEntity>();
    }
}