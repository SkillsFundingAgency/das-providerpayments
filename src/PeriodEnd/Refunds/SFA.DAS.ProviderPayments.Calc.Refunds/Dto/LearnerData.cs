using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Dto
{
    public class LearnerData
    {
        public LearnerData(string learnerRefNumber)
        {
            LearnRefNumber = learnerRefNumber;
        }
        public string LearnRefNumber { get; }
        public List<RequiredPaymentEntity> RequiredRefunds { get; } = new List<RequiredPaymentEntity>();
        public List<PaymentEntity> HistoricalPayments { get; } = new List<PaymentEntity>();
    }
}