using System;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ExecutionManagers;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions.PaymentsAndEarningsRules
{
    public class EmployersLevyAccountDebitedViaTransferRule : EarningsAndPaymentsRuleBase
    {
        public override void AssertBreakdown(EarningsAndPaymentsBreakdown breakdown, ActualRuleResult ruleResult, EmployerAccountContext employerAccountContext)
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