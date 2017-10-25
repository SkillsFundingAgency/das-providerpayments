using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.Automation.Domain.Specifications;
using SFA.DAS.Payments.Automation.Infrastructure.PaymentResults;

namespace SFA.DAS.payments.Automation.Assertions
{
    public static class PaymentAssertions 
    {
        public static List<AssertionResult> AssertPayments(List<ProviderEarningsAndPayments> expectations, List<PaymentResult> actuals,string scenarioName)
        {
            var results = new List<AssertionResult>();

            foreach (var earnings in expectations)
            {
                foreach (var expected in earnings.EarningAndPaymentsByPeriod)
                {
                    var expectedPeriod = ToPeriodName(ToPeriodDateTime(expected.Period).AddMonths(-1));
                  
                    var providerpaidBySfa = actuals.Where(x => x.DeliveryPeriod == expectedPeriod && x.FundingSource != FundingSource.CoInvestedEmployer).Sum(y => y.Amount);
                    var levyAccountDebited = actuals.Where(x => x.DeliveryPeriod == expectedPeriod && x.FundingSource == FundingSource.Levy).Sum(y => y.Amount);
                    var paymentsDueFromEmployers = actuals.Where(x => x.DeliveryPeriod == expectedPeriod && x.FundingSource == FundingSource.CoInvestedEmployer).Sum(y => y.Amount);


                    var providerearnedTotal = actuals.Where(x => x.CalculationPeriod == expected.Period).Sum(y => y.Amount);
                    var sfaLevyEmployerBudget = actuals.Where(x => x.DeliveryPeriod == expected.Period && x.FundingSource == FundingSource.Levy).Sum(y => y.Amount);
                    var sfaLevyCoFundingBudget = actuals.Where(x => x.DeliveryPeriod == expected.Period && x.FundingSource == FundingSource.CoInvestedSfa && x.ContractType == Payments.Automation.Infrastructure.PaymentResults.ContractType.ContractWithEmployer).Sum(y => y.Amount);
                    var sfaNonLevyCoFundingBudget = actuals.Where(x => x.DeliveryPeriod == expected.Period && x.FundingSource == FundingSource.CoInvestedSfa && x.ContractType == Payments.Automation.Infrastructure.PaymentResults.ContractType.ContractWithSfa).Sum(y => y.Amount);

                    var sfaLevyAdditionalPaymentsBudget = actuals.Where(x => x.DeliveryPeriod == expected.Period && x.FundingSource == FundingSource.FullyFundedSfa && x.ContractType == Payments.Automation.Infrastructure.PaymentResults.ContractType.ContractWithEmployer).Sum(y => y.Amount);
                    var sfaNonLevyAdditionalPaymentsBudget = actuals.Where(x => x.DeliveryPeriod == expected.Period && x.FundingSource == FundingSource.FullyFundedSfa && x.ContractType == Payments.Automation.Infrastructure.PaymentResults.ContractType.ContractWithSfa).Sum(y => y.Amount);
                    
                    //provider earned total
                    if (providerearnedTotal != expected.ProviderEarnedTotal)
                    {
                        AddResult(scenarioName, results, $"Provider earned Total expected {expected.ProviderEarnedTotal} for period {expected.Period} but was {providerearnedTotal}");
                    }

                    //provider paid by sfa
                    if (providerpaidBySfa != expected.ProviderPaidBySfa)
                    {
                        AddResult(scenarioName,results, $"Provider Paid By Sfa expected {expected.ProviderPaidBySfa} for period {expected.Period} but was {providerpaidBySfa}");
                    }

                    //levy account debited
                    if (levyAccountDebited != expected.LevyAccountDebited)
                    {
                        AddResult(scenarioName,results, $"Levy Account Debited expected {expected.LevyAccountDebited} for period {expected.Period} but was {levyAccountDebited}");
                    }

                    //levy employer budget
                    if (sfaLevyEmployerBudget != expected.SfaLevyEmployerBudget)
                    {
                        AddResult(scenarioName,results, $"Sfa levy Employer Budget expected {expected.SfaLevyEmployerBudget} for period {expected.Period} but was {sfaLevyEmployerBudget}");
                    }

                    //levy co fund  budget
                    if (sfaLevyCoFundingBudget != expected.SfaLevyCoFundingBudget)
                    {
                        AddResult(scenarioName, results, $"Sfa levy co-funding Budget expected {expected.SfaLevyCoFundingBudget} for period {expected.Period} but was {sfaLevyCoFundingBudget}");
                    }

                    //non levy co fund  budget
                    if (sfaNonLevyCoFundingBudget != expected.SfaNonLevyCoFundingBudget)
                    {
                        AddResult(scenarioName,results, $"Sfa Non levy co-funding Budget expected {expected.SfaNonLevyCoFundingBudget} for period {expected.Period} but was {sfaNonLevyCoFundingBudget}");
                    }

                    //levy additional funding budget
                    if (sfaLevyAdditionalPaymentsBudget != expected.SfaLevyAdditionalPaymentsBudget)
                    {
                        AddResult(scenarioName, results, $"Sfa levy additional payments budget expected {expected.SfaLevyAdditionalPaymentsBudget} for period {expected.Period} but was {sfaLevyAdditionalPaymentsBudget}");
                    }

                    //non levy additional funding budget
                    if (sfaNonLevyAdditionalPaymentsBudget != expected.SfaNonLevyAdditionalPaymentsBudget)
                    {
                        AddResult(scenarioName, results, $"Sfa non levy additional payments budget expected {expected.SfaNonLevyAdditionalPaymentsBudget} for period {expected.Period} but was {sfaNonLevyAdditionalPaymentsBudget}");
                    }
                    
                    //payment due from employers
                    var totalExpectedFromEmployers = expected.PaymentDueFromEmployers?.Sum(x => x.Value) ?? 0;
                    if (paymentsDueFromEmployers != totalExpectedFromEmployers)
                    {
                        AddResult(scenarioName, results, $"Payments Due From Employers expected {totalExpectedFromEmployers} for period {expected.Period} but was {paymentsDueFromEmployers}");
                    }
                }
            }

            return results;
        }

        private static DateTime ToPeriodDateTime(string name)
        {
            return new DateTime(int.Parse(name.Substring(3, 2)) + 2000, int.Parse(name.Substring(0, 2)), 1);
        }

        internal static string ToPeriodName(DateTime date)
        {
            return $"{date.Month:00}/{date.Year - 2000:00}";
        }

        private static void AddResult(string scenarioName, List<AssertionResult> items, string message)
        {
            items.Add(new AssertionResult
            {
                ScenarioName=scenarioName,
                Message = message,
            });
        }
    }
}
