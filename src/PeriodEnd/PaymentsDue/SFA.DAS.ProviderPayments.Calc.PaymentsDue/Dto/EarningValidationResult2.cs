using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto
{
    public class EarningValidationResult
    {
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
