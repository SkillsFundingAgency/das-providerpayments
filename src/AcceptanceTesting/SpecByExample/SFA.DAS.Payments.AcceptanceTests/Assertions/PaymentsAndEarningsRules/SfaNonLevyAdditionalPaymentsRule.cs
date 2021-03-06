﻿using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using SFA.DAS.Payments.AcceptanceTests.ResultsDataModels;
using System;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions.PaymentsAndEarningsRules
{
    public class SfaNonLevyAdditionalPaymentsRule: PaymentsRuleBase
    {
        public override void AssertBreakdown(EarningsAndPaymentsBreakdown breakdown, ActualRuleResult ruleResult, EmployerAccountContext employerAccountContext)
        {
            var payments = GetPaymentsForBreakdown(breakdown, ruleResult.LearnerResults)
                .Where(p => p.FundingSource == FundingSource.FullyFundedSfa && p.ContractType == ContractType.ContractWithEmployer)
                .ToArray();
            foreach (var period in breakdown.SfaNonLevyAdditionalPayments)
            {
                AssertResultsForPeriod(period, payments);
            }
        }

        protected new void AssertResultsForPeriod(PeriodValue period, PaymentResult[] allPayments)
        {
            var paidInPeriod = allPayments.Where(p => p.DeliveryPeriod == period.PeriodName).Sum(p => p.Amount);

            if (paidInPeriod >= 0 && !AreValuesEqual(period.Value, paidInPeriod))
            {
                throw new Exception(FormatAssertionFailureMessage(period, paidInPeriod));
            }
        }

        protected override string FormatAssertionFailureMessage(PeriodValue period, decimal actualPaymentInPeriod)
        {
            return $"Expected SFA non Levy additional payment of {period.Value} in {period.PeriodName} but was actually {actualPaymentInPeriod}";
        }
    }
}
