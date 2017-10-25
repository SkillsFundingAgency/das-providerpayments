using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.Domain.Specifications
{
    public class EmployerAccountLevyBalances
    {
        public string EmployerKey { get; set; }
        public decimal? BalanceForAllPeriods { get; set; }
        public Dictionary<string, decimal> BalancesPerPeriod { get; set; } = new Dictionary<string, decimal>();
    }
}