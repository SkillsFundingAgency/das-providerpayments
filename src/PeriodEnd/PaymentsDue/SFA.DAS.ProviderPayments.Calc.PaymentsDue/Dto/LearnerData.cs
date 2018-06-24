using System;
using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto
{
    public class LearnerData
    {
        public LearnerData(string learnerRefNumber, long? uln)
        {
            LearnRefNumber = learnerRefNumber;
            Uln = uln;
        }
        public string LearnRefNumber { get; }
        public long? Uln { get; }

        public List<RawEarning> RawEarnings { get; } = new List<RawEarning>();
        public List<RawEarningForMathsOrEnglish> RawEarningsMathsEnglish { get; } = new List<RawEarningForMathsOrEnglish>();
        public List<DatalockOutputEntity> DataLocks { get; } = new List<DatalockOutputEntity>();
        public List<RequiredPaymentEntity> HistoricalPayments { get; } = new List<RequiredPaymentEntity>();
        public List<Commitment> Commitments { get; } = new List<Commitment>();
        public List<DatalockValidationError> DatalockValidationErrors { get; set; } = new List<DatalockValidationError>();
        public DateTime FirstDayOfAcademicYear { get; set; }
    }
}