using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto
{
    public class EarningValidationResult
    {
        public EarningValidationResult(
            List<FundingDue> earnings,
            List<NonPayableEarningEntity> nonPayableEarnings,
            List<int> periodsToIgnore = null)
        {
            Earnings = new List<FundingDue>(earnings);
            NonPayableEarnings = new List<NonPayableEarningEntity>(nonPayableEarnings);
            if (periodsToIgnore == null)
            {
                PeriodsToIgnore = new List<int>();
            }
            else
            {
                PeriodsToIgnore = new List<int>(periodsToIgnore);
            }
        }
        public List<FundingDue> Earnings { get; set; }
        public List<NonPayableEarningEntity> NonPayableEarnings { get; set; }
        public List<int> PeriodsToIgnore { get; set; }
    }
}
