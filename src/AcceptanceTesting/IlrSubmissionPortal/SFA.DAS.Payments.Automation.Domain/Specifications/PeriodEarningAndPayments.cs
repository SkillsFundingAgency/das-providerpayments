using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.Domain.Specifications
{
    public class PeriodEarningAndPayments
    {
        public virtual string Period { get; set; }

        public virtual decimal ProviderEarnedTotal { get; set; }
        public virtual decimal ProviderEarnedFromSfa { get; set; }
        public virtual decimal ProviderEarnedFromEmployer { get; set; }
        public virtual decimal ProviderPaidBySfa { get; set; }
        public virtual decimal PaymentDueFromEmployer { get; set; }
        public virtual decimal LevyAccountDebited { get; set; }
        public virtual decimal SfaLevyEmployerBudget { get; set; }
        public virtual decimal SfaLevyCoFundingBudget { get; set; }
        public virtual decimal SfaNonLevyCoFundingBudget { get; set; }
        public virtual decimal SfaLevyAdditionalPaymentsBudget { get; set; }
        public virtual decimal SfaNonLevyAdditionalPaymentsBudget { get; set; }
        public virtual decimal RefundTakenBySfa { get; set; }
        public virtual decimal RefundDueToEmployer { get; set; }
        public virtual decimal LevyAccountCredited { get; set; }

        public virtual List<EmployerPeriodValue> EmployerLevyAccountsDebited { get; set; } = new List<EmployerPeriodValue>();
        public virtual List<EmployerPeriodValue> PaymentDueFromEmployers { get; set; } = new List<EmployerPeriodValue>();
        public virtual List<EmployerPeriodValue> ProviderEarnedFromEmployers { get; set; } = new List<EmployerPeriodValue>();
    }

    public class EmployerPeriodValue
    {
        public string EmployerKey { get; set; }
        public decimal Value { get; set; }
    }
}