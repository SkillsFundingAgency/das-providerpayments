using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto
{
    public class EarningValidationResult
    {
        public EarningValidationResult()
        {}

        public EarningValidationResult(
            List<FundingDue> earnings,
            List<NonPayableEarning> nonPayableEarnings,
            List<int> periodsToIgnore = null)
        {
            PayableEarnings = new List<FundingDue>(earnings);
            NonPayableEarnings = new List<NonPayableEarning>(nonPayableEarnings);
            if (periodsToIgnore == null)
            {
                PeriodsToIgnore = new HashSet<int>();
            }
            else
            {
                PeriodsToIgnore = new HashSet<int>(periodsToIgnore);
            }
        }
        public List<FundingDue> PayableEarnings { get; set; } = new List<FundingDue>();
        public List<NonPayableEarning> NonPayableEarnings { get; set; } = new List<NonPayableEarning>();
        public HashSet<int> PeriodsToIgnore { get; set; } = new HashSet<int>();

        public static EarningValidationResult operator +(EarningValidationResult left, EarningValidationResult right)
        {
            left.AddPayableEarnings(right.PayableEarnings);
            left.AddNonPayableEarnings(right.NonPayableEarnings);
            left.AddPeriodsToIgnore(right.PeriodsToIgnore);
            return left;
        }

        public void AddPayableEarnings(IEnumerable<FundingDue> payableEarnings)
        {
            PayableEarnings.AddRange(payableEarnings);
        }

        public void AddNonPayableEarnings(IEnumerable<NonPayableEarning> nonPayableEarnings)
        {
            NonPayableEarnings.AddRange(nonPayableEarnings);
        }

        public void AddPeriodsToIgnore(HashSet<int> periodsToIgnore)
        {
            PeriodsToIgnore.UnionWith(periodsToIgnore);
        }
    }
}
