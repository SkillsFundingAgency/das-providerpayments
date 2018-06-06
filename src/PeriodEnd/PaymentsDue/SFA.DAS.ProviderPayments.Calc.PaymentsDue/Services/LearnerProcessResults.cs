using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class LearnerProcessResults
    {
        public List<RequiredPaymentEntity> PayableEarnings { get; set; } = new List<RequiredPaymentEntity>();
        //public List<FundingDue> FundingDue { get; } = new List<FundingDue>();
        public List<NonPayableEarningEntity> NonPayableEarnings { get; set; } = new List<NonPayableEarningEntity>();
    }
}