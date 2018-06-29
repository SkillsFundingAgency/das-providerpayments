using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions.PaymentsAndEarningsRules
{
    public class EmployersLevyAccountDebitedForUlnRule : PaymentsRuleBase
    {

        public override void AssertBreakdown(EarningsAndPaymentsBreakdown breakdown, ActualRuleResult ruleResult, EmployerAccountContext employerAccountContext)
        {
            var payments = GetPaymentsForBreakdown(breakdown, ruleResult.LearnerResults)
                .Where(p => p.FundingSource == FundingSource.Levy && p.ContractType == ContractType.ContractWithSfa && p.Amount >= 0)
                .ToArray();

            foreach (var period in breakdown.EmployersLevyAccountDebitedForUln)
            {
                var employerUlnPayments = payments.Where(p => p.EmployerAccountId == period.EmployerAccountId && p.Uln == period.Uln).ToArray();
                var prevPeriod = new EmployerAccountUlnPeriodValue
                {
                    EmployerAccountId = period.EmployerAccountId,
                    Uln = period.Uln,
                    PeriodName = period.PeriodName.ToPeriodDateTime().AddMonths(-1).ToPeriodName(),
                    Value = period.Value
                };

                AssertResultsForPeriod(prevPeriod, employerUlnPayments);
            }
        }

        protected override string FormatAssertionFailureMessage(PeriodValue period, decimal actualPaymentInPeriod)
        {
            var employerUlnPeriod = (EmployerAccountUlnPeriodValue)period;
            var specPeriod = period.PeriodName.ToPeriodDateTime().AddMonths(1).ToPeriodName();

            return $"Expected Employer {employerUlnPeriod.EmployerAccountId} levy budget to be debited {employerUlnPeriod.Value} for ULN {employerUlnPeriod.Uln} in {specPeriod} but was actually debited {actualPaymentInPeriod}";
        }
    }
}