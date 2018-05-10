using System;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Assertions.PaymentsAndEarningsRules;
using SFA.DAS.Payments.AcceptanceTests.Contexts;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions
{
    public static class PaymentsAndEarningsAssertions
    {
        private static readonly EarningsAndPaymentsRuleBase[] Rules =
        {
            new ProviderEarnedTotalRule(),
            new ProviderPaidBySfaRule(),
            new PaymentDueFromEmployersRule(),
            new EmployersLevyAccountDebitedRule(),
            new EmployersLevyAccountDebitedViaTransferRule(),
            new SfaLevyBudgetRule(),
            new SfaLevyCoFundBudgetRule(),
            new SfaNonLevyCoFundBudgetRule(),
            new SfaLevyAdditionalPaymentsRule(),
            new SfaNonLevyAdditionalPaymentsRule(),
            new RefundTakenBySfaRule(),
            new EmployersLevyAccountCreditedRule(),
            new RefundDueToEmployerRule()
        };

        public static void AssertPaymentsAndEarningsResults(EarningsAndPaymentsContext earningsAndPaymentsContext, PeriodContext periodContext, EmployerAccountContext employerAccountContext)
        {
            if (TestEnvironment.ValidateSpecsOnly)
            {
                return;
            }

            ValidateOverallEarningsAndPayments(earningsAndPaymentsContext, periodContext, employerAccountContext);
            ValidateLearnerSpecificEarningsAndPayments(earningsAndPaymentsContext, periodContext, employerAccountContext);
        }

        private static void ValidateOverallEarningsAndPayments(EarningsAndPaymentsContext earningsAndPaymentsContext, PeriodContext periodContext, EmployerAccountContext employerAccountContext)
        {
            foreach (var breakdown in earningsAndPaymentsContext.OverallEarningsAndPayments)
            {
                foreach (var rule in Rules)
                {
                    rule.AssertBreakdown(breakdown, new ActualRuleResult{ LearnerResults = periodContext.PeriodResults, TransferResults = periodContext.TransferResults }, employerAccountContext);
                }
            }
        }
        private static void ValidateLearnerSpecificEarningsAndPayments(EarningsAndPaymentsContext earningsAndPaymentsContext, PeriodContext periodContext, EmployerAccountContext employerAccountContext)
        {
            foreach (var breakdown in earningsAndPaymentsContext.LearnerOverallEarningsAndPayments)
            {
                var learnerResults = periodContext.PeriodResults.Where(r => r.LearnerReferenceNumber == breakdown.LearnerReferenceNumber).ToList();
                try
                {
                    foreach (var rule in Rules)
                    {
                        rule.AssertBreakdown(breakdown, new ActualRuleResult{ LearnerResults = learnerResults, TransferResults = periodContext.TransferResults }, employerAccountContext);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex.Message} (learner {breakdown.LearnerReferenceNumber})", ex);
                }
            }
        }
    }
}
