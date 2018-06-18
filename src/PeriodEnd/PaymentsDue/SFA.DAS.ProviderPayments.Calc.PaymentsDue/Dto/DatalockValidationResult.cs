using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto
{
    public class DatalockValidationResult
    {
        public DatalockValidationResult(
            List<FundingDue> earnings,
            List<NonPayableEarningEntity> nonPayableEarnings,
            List<int> periodsToIgnore = null)
        {
            Earnings = earnings;
            NonPayableEarnings = nonPayableEarnings;
            if (periodsToIgnore == null)
            {
                PeriodsToIgnore = new List<int>();
            }
            else
            {
                PeriodsToIgnore = periodsToIgnore;
            }
        }
        public List<FundingDue> Earnings { get; set; }
        public List<NonPayableEarningEntity> NonPayableEarnings { get; set; }
        public List<int> PeriodsToIgnore { get; set; }
    }
}
