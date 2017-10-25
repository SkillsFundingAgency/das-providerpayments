using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gherkin.Ast;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers
{
    public class IndefinateLevyBalanceStepParser : StepParser
    {
        private const string EmployerSpecificPattern = @"the (employer [0-9]{1,2}) has a levy balance > agreed price for all months";
        private const string DefaultEmployerPattern = @"levy balance > agreed price for all months";

        public IndefinateLevyBalanceStepParser()
            : base(new StepParserAbility("given", EmployerSpecificPattern),
                   new StepParserAbility("given", DefaultEmployerPattern))
        {
        }
        public override void Parse(Step step, Specification specification)
        {
            var specificEmployerMatch = Regex.Match(step.Text, EmployerSpecificPattern, RegexOptions.IgnoreCase);
            var employerKey = specificEmployerMatch.Success ? specificEmployerMatch.Groups[1].Value : Defaults.EmployerKey;

            var balances = specification.Arrangement.LevyBalances.SingleOrDefault(x => x.EmployerKey.Equals(employerKey, StringComparison.CurrentCultureIgnoreCase));
            if (balances != null)
            {
                balances.BalanceForAllPeriods = long.MaxValue;
                return;
            }

            specification.Arrangement.LevyBalances.Add(new EmployerAccountLevyBalances
            {
                EmployerKey = employerKey,
                BalanceForAllPeriods = long.MaxValue
            });
        }
    }
}
