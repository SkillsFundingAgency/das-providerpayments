using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions.PaymentsAndEarningsRules
{
    public class ProviderPaidBySfaForUlnRule : PaymentsRuleBase
    {
        public override void AssertBreakdown(EarningsAndPaymentsBreakdown breakdown, ActualRuleResult ruleResult, EmployerAccountContext employerAccountContext)
        {
            foreach (var period in breakdown.ProviderPaidBySfaForUln)
            {
                var prevPeriod = new UlnPeriodValue
                {
                    Uln = period.Uln,
                    PeriodName = period.PeriodName.ToPeriodDateTime().AddMonths(-1).ToPeriodName(),
                    Value = period.Value
                };

                var allPayments = GetPaymentsForBreakdown(breakdown, ruleResult.LearnerResults)
                    .Where(p => p.FundingSource != FundingSource.CoInvestedEmployer && p.Uln == period.Uln && p.Amount > 0)
                    .ToArray();

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