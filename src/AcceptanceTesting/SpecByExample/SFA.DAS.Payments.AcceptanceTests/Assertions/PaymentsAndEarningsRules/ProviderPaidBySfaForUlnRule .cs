using System;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using SFA.DAS.Payments.AcceptanceTests.ResultsDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions.PaymentsAndEarningsRules
{
    public class ProviderPaidBySfaForUlnRule : PaymentsRuleBase
    {
        public override void AssertBreakdown(EarningsAndPaymentsBreakdown breakdown, ActualRuleResult ruleResult, EmployerAccountContext employerAccountContext)
        {
            var allLearners = ruleResult.LearnerResults.ToArray();

            foreach (var period in breakdown.ProviderPaidBySfaForUln)
            {
                var prevPeriod = new UlnPeriodValue
                {
                    Uln = period.Uln,
                    PeriodName = period.PeriodName.ToPeriodDateTime().AddMonths(-1).ToPeriodName(),
                    Value = period.Value
                };
                AssertResultsForLearners(prevPeriod, allLearners);
            }
        }

        protected override string FormatAssertionFailureMessage(PeriodValue period, decimal actualPaymentInPeriod)
        {
            var specPeriod = period.PeriodName.ToPeriodDateTime().AddMonths(1).ToPeriodName();

            return $"Expected provider to be paid {period.Value} by SFA in {specPeriod} for Uln {((UlnPeriodValue)period).Uln} but actually paid {actualPaymentInPeriod}";
        }

        protected void AssertResultsForLearners(UlnPeriodValue period, LearnerResults[] allLearners)
        {
            var paidInPeriodForUln = allLearners.Single(x => x.Uln == period.Uln).Payments
                .Where(p => p.CalculationPeriod == period.PeriodName).Sum(p => p.Amount);

            if (!AreValuesEqual(period.Value, paidInPeriodForUln))
            {
                throw new Exception(FormatAssertionFailureMessage(period, paidInPeriodForUln));
            }
        }


    }
}