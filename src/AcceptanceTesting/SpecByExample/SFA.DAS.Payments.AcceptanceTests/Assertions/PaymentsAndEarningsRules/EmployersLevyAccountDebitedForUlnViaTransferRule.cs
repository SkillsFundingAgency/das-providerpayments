using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions.PaymentsAndEarningsRules
{
    public class EmployersLevyAccountDebitedForUlnViaTransferRule : PaymentsRuleBase
    {
        public override void AssertBreakdown(EarningsAndPaymentsBreakdown breakdown, ActualRuleResult ruleResult, EmployerAccountContext employerAccountContext)
        {
            foreach (var period in breakdown.EmployersLevyAccountDebitedForUlnViaTransfer)
            {
                var prevPeriod = new EmployerAccountUlnPeriodValue
                {
                    EmployerAccountId = period.EmployerAccountId,
                    Uln = period.Uln,
                    PeriodName = period.PeriodName.ToPeriodDateTime().AddMonths(-1).ToPeriodName(),
                    Value = period.Value
                };

                var allPayments = GetPaymentsForBreakdown(breakdown, ruleResult.LearnerResults)
                    .Where(p => p.FundingSource == FundingSource.Transfer && p.Uln == period.Uln && p.Amount > 0)
                    .ToArray();

                AssertResultsForPeriod(prevPeriod, allPayments);
            }
        }

        protected override string FormatAssertionFailureMessage(PeriodValue period, decimal actualPaymentInPeriod)
        {
            var employerUlnPeriod = (EmployerAccountUlnPeriodValue) period;

            return $"Expected Employer {employerUlnPeriod.EmployerAccountId} levy budget to be debited {employerUlnPeriod.Value} via transfer in {period} but was actually debited {actualPaymentInPeriod}";
        }
    }
}