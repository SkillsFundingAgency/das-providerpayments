using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto
{
    public class EarningValidationResult
    {
        public EarningValidationResult(
            List<FundingDue> earnings,
            List<NonPayableEarning> nonPayableEarnings,
            List<int> periodsToIgnore = null)
        {
            PayableEarnings = new List<FundingDue>(earnings);
            NonPayableEarnings = new List<NonPayableEarning>(nonPayableEarnings);
            if (periodsToIgnore == null)
            {
                PeriodsToIgnore = new List<int>();
            }
            else
            {
                PeriodsToIgnore = new List<int>(periodsToIgnore);
            }
        }
        public List<FundingDue> PayableEarnings { get; set; }
        public List<NonPayableEarning> NonPayableEarnings { get; set; }
        public List<int> PeriodsToIgnore { get; set; }
    }
}
