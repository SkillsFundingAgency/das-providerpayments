using System;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ExecutionManagers;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions.PaymentsAndEarningsRules
{
    public class EmployersLevyAccountDebitedRule : PaymentsRuleBase
    {

        public override void AssertBreakdown(EarningsAndPaymentsBreakdown breakdown, RuleResult ruleResult, EmployerAccountContext employerAccountContext)
        {
            var payments = GetPaymentsForBreakdown(breakdown, ruleResult.LearnerResults)
                .Where(p => p.FundingSource == FundingSource.Levy && p.ContractType == ContractType.ContractWithSfa && p.Amount >= 0)
                .ToArray();
            foreach (var period in breakdown.EmployersLevyAccountDebited)
            {
                var employerPayments = payments.Where(p => p.EmployerAccountId == period.EmployerAccountId).ToArray();
                var prevPeriod = new EmployerAccountPeriodValue
                {
                    EmployerAccountId = period.EmployerAccountId,
                    PeriodName = period.PeriodName.ToPeriodDateTime().AddMonths(-1).ToPeriodName(),
                    Value = period.Value
                };

                AssertResultsForPeriod(prevPeriod, employerPayments);
            }
        }

        protected override string FormatAssertionFailureMessage(PeriodValue period, decimal actualPaymentInPeriod)
        {
            var employerPeriod = (EmployerAccountPeriodValue)period;
            var specPeriod = period.PeriodName.ToPeriodDateTime().AddMonths(1).ToPeriodName();

            return $"Expected Employer {employerPeriod.EmployerAccountId} levy budget to be debited {period.Value} in {specPeriod} but was actually debited {actualPaymentInPeriod}";
        }
    }

    public class EmployersLevyAccountDebitedViaTransferRule : EarningsAndPaymentsRuleBase
    {
        public override void AssertBreakdown(EarningsAndPaymentsBreakdown breakdown, RuleResult ruleResult, EmployerAccountContext employerAccountContext)
        {
            foreach (var period in breakdown.EmployersLevyAccountDebitedViaTransfer)
            {
                var prevPeriod = new EmployerAccountPeriodValue
                {
                    EmployerAccountId = period.EmployerAccountId,
                    PeriodName = period.PeriodName.ToPeriodDateTime().AddMonths(-1).ToPeriodName(),
                    Value = period.Value
                };

                var debitedInPeriod = ruleResult.TransferResults
                    .Where(x => PeriodNameHelper.GetStringDateFromLongPeriod(x.CollectionPeriodName) == prevPeriod.PeriodName && x.SendingAccountId == prevPeriod.EmployerAccountId)
                    .Sum(x => x.Amount);

                if (debitedInPeriod != prevPeriod.Value)
                {
                    throw new Exception($"Expected Employer {prevPeriod.EmployerAccountId} levy budget to be debited {period.Value} via transfer in {period} but was actually debited {debitedInPeriod}");
                }
            }
        }
    }
}