using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using System.Collections.Generic;

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
        public List<HistoricalPayment> HistoricalPayments { get; } = new List<HistoricalPayment>();
    }
}