using System;
using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
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
        public HashSet<DatalockOutput> DataLocks { get; } = new HashSet<DatalockOutput>();
        public List<RequiredPaymentEntity> HistoricalPayments { get; } = new List<RequiredPaymentEntity>();
        public List<Commitment> Commitments { get; } = new List<Commitment>();
        public List<DatalockValidationError> DatalockValidationErrors { get; set; } = new List<DatalockValidationError>();
        public DateTime FirstDayOfAcademicYear { get; set; }
    }
}