using System;
using System.Linq;
using Gherkin.Ast;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers
{
    public class NoLevyBalanceStepParser : StepParser
    {
        public NoLevyBalanceStepParser()
            : base(new StepParserAbility("given", @"levy balance = 0 for all months"))
        {
        }
        public override void Parse(Step step, Specification specification)
        {
            var employerKey = Defaults.EmployerKey;

            var balances = specification.Arrangement.LevyBalances.SingleOrDefault(x => x.EmployerKey.Equals(employerKey, StringComparison.CurrentCultureIgnoreCase));
            if (balances != null)
            {
                balances.BalanceForAllPeriods = 0;
                return;
            }

            specification.Arrangement.LevyBalances.Add(new EmployerAccountLevyBalances
            {
                EmployerKey = employerKey,
                BalanceForAllPeriods = 0
            });
        }
    }
}