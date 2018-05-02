using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions.PaymentsAndEarningsRules
{
    public class ProviderPaidBySfaRule : PaymentsRuleBase
    {
        public override void AssertBreakdown(EarningsAndPaymentsBreakdown breakdown, RuleResult ruleResult, EmployerAccountContext employerAccountContext)
        {
            var allPayments = GetPaymentsForBreakdown(breakdown, ruleResult.LearnerResults)
                .Where(p => p.FundingSource != FundingSource.CoInvestedEmployer && p.Amount>0)
                .ToArray();
            foreach (var period in breakdown.ProviderPaidBySfa)
            {
                var prevPeriod = new PeriodValue
                {
                    PeriodName = period.PeriodName.ToPeriodDateTime().AddMonths(-1).ToPeriodName(),
                    Value = period.Value
                };

                AssertResultsForPeriod(prevPeriod, allPayments);
            }
        }

        protected override string FormatAssertionFailureMessage(PeriodValue period, decimal actualPaymentInPeriod)
        {
            var specPeriod = period.PeriodName.ToPeriodDateTime().AddMonths(1).ToPeriodName();

            return $"Expected provider to be paid {period.Value} by SFA in {specPeriod} but actually paid {actualPaymentInPeriod}";
        }
    }
}