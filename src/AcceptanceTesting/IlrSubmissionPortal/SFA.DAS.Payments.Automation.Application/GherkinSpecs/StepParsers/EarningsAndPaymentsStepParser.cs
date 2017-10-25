using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gherkin.Ast;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers.StepTableParsing;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers
{
    public class EarningsAndPaymentsStepParser : StepParser
    {
        private static string NamedProviderPattern = @"the (provider [a-z]{1}) earnings and payments break down as follows";

        public EarningsAndPaymentsStepParser()
            : base(new StepParserAbility("then", @"the provider earnings and payments break down as follows"),
                   new StepParserAbility("then", NamedProviderPattern))
        {
        }
        public override void Parse(Step step, Specification specification)
        {
            var table = step.Argument as DataTable;
            var earningAndPayments = TableParser.ParsePeriodTable<PeriodEarningAndPaymentsEntity>("Earnings and Payments", table);
            foreach (var x in earningAndPayments)
            {
                if (x.Employer1LevyAccountDebited > 0)
                {
                    x.EmployerLevyAccountsDebited.Add(new EmployerPeriodValue { EmployerKey = "employer 1", Value = x.Employer1LevyAccountDebited });
                }
                if (x.Employer2LevyAccountDebited > 0)
                {
                    x.EmployerLevyAccountsDebited.Add(new EmployerPeriodValue { EmployerKey = "employer 2", Value = x.Employer2LevyAccountDebited });
                }

                if (x.PaymentDueFromEmployer1 > 0)
                {
                    x.PaymentDueFromEmployers.Add(new EmployerPeriodValue { EmployerKey = "employer 1", Value = x.PaymentDueFromEmployer1 });
                }
                if (x.PaymentDueFromEmployer2 > 0)
                {
                    x.PaymentDueFromEmployers.Add(new EmployerPeriodValue { EmployerKey = "employer 2", Value = x.PaymentDueFromEmployer2 });
                }

                if (x.ProviderEarnedFromEmployer1 > 0)
                {
                    x.ProviderEarnedFromEmployers.Add(new EmployerPeriodValue { EmployerKey = "employer 1", Value = x.ProviderEarnedFromEmployer1 });
                }
                if (x.ProviderEarnedFromEmployer2 > 0)
                {
                    x.ProviderEarnedFromEmployers.Add(new EmployerPeriodValue { EmployerKey = "employer 2", Value = x.ProviderEarnedFromEmployer2 });
                }
            }

            var match = Regex.Match(step.Text, NamedProviderPattern);
            var providerKey = match.Success ? match.Groups[1].Value : Defaults.ProviderKey;

            var provider = specification.Expectations.EarningsAndPayments.SingleOrDefault(x => x.ProviderKey == providerKey);
            if (provider == null)
            {
                provider = new ProviderEarningsAndPayments { ProviderKey = providerKey };
                specification.Expectations.EarningsAndPayments.Add(provider);
            }
            provider.EarningAndPaymentsByPeriod.AddRange(earningAndPayments);
        }

        private class PeriodEarningAndPaymentsEntity : PeriodEarningAndPayments
        {
            [Optional]
            public override string Period { get; set; }

            [Optional]
            public override decimal ProviderEarnedTotal { get; set; }

            [Optional]
            public override decimal ProviderEarnedFromSfa { get; set; }

            [Optional]
            public override decimal ProviderEarnedFromEmployer { get; set; }

            [Optional]
            public override decimal ProviderPaidBySfa { get; set; }

            [Optional]
            public override decimal PaymentDueFromEmployer { get; set; }

            [Optional]
            public override decimal LevyAccountDebited { get; set; }

            [Optional]
            public override decimal SfaLevyEmployerBudget { get; set; }

            [Optional]
            public override decimal SfaLevyCoFundingBudget { get; set; }

            [Optional]
            public override decimal SfaNonLevyCoFundingBudget { get; set; }

            [Optional]
            public override decimal SfaLevyAdditionalPaymentsBudget { get; set; }

            [Optional]
            public override decimal SfaNonLevyAdditionalPaymentsBudget { get; set; }

            [Optional]
            public override decimal RefundTakenBySfa { get; set; }

            [Optional]
            public override decimal RefundDueToEmployer { get; set; }

            [Optional]
            public override decimal LevyAccountCredited { get; set; }


            //Psudo properties
            [Optional]
            public override List<EmployerPeriodValue> EmployerLevyAccountsDebited { get; set; } = new List<EmployerPeriodValue>();

            [Optional]
            public decimal Employer1LevyAccountDebited { get; set; }

            [Optional]
            public decimal Employer2LevyAccountDebited { get; set; }


            [Optional]
            public override List<EmployerPeriodValue> PaymentDueFromEmployers { get; set; }

            [Optional]
            public decimal PaymentDueFromEmployer1 { get; set; }

            [Optional]
            public decimal PaymentDueFromEmployer2 { get; set; }


            [Optional]
            public override List<EmployerPeriodValue> ProviderEarnedFromEmployers { get; set; }

            [Optional]
            public decimal ProviderEarnedFromEmployer1 { get; set; }

            [Optional]
            public decimal ProviderEarnedFromEmployer2 { get; set; }
        }
    }
}
