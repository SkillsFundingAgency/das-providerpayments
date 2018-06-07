using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto
{
    public class LearnerProcessParameters
    {
        public LearnerProcessParameters(string learnerRefNumber, long? uln)
        {
            LearnRefNumber = learnerRefNumber;
            Uln = uln;
        }
        public string LearnRefNumber { get; }
        public long? Uln { get; }

        public List<RawEarning> RawEarnings { get; } = new List<RawEarning>();
        public List<RawEarningForMathsOrEnglish> RawEarningsMathsEnglish { get; } = new List<RawEarningForMathsOrEnglish>();
        public List<DataLockPriceEpisodePeriodMatchEntity> DataLocks { get; } = new List<DataLockPriceEpisodePeriodMatchEntity>();
        public List<RequiredPaymentsHistoryEntity> HistoricalPayments { get; } = new List<RequiredPaymentsHistoryEntity>();
        public List<Commitment> Commitments { get; } = new List<Commitment>();
    }
}