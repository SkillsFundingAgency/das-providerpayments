using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gherkin.Ast;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers
{
    public class SpecificLevyBalanceStepParser : StepParser
    {
        private const string EmployerSpecificPattern = @"the (employer [0-9]{1,2}) has a levy balance of";
        private const string DefaultEmployerPattern = @"the employer's levy balance is";

        public SpecificLevyBalanceStepParser()
            : base(new StepParserAbility("given", EmployerSpecificPattern),
                new StepParserAbility("given", DefaultEmployerPattern))
        {
        }
        public override void Parse(Step step, Specification specification)
        {
            var specificEmployerMatch = Regex.Match(step.Text, EmployerSpecificPattern, RegexOptions.IgnoreCase);
            var employerKey = specificEmployerMatch.Success ? specificEmployerMatch.Groups[1].Value : Defaults.EmployerKey;

            var balances = specification.Arrangement.LevyBalances.SingleOrDefault(x => x.EmployerKey.Equals(employerKey, StringComparison.CurrentCultureIgnoreCase));
            if (balances == null)
            {
                balances = new EmployerAccountLevyBalances
                {
                    EmployerKey = employerKey
                };
                specification.Arrangement.LevyBalances.Add(balances);
            }

            var table = step.Argument as DataTable;
            ParseTableIntoEmployerAccountLevyBalances(balances, table);
        }

        private void ParseTableIntoEmployerAccountLevyBalances(EmployerAccountLevyBalances account, DataTable table)
        {
            var rows = table.Rows.ToArray();
            var headers = rows[0].Cells.Select(x => x.Value).ToArray();
            for (var i = 1; i < rows.Length; i++)
            {
                var cols = rows[1].Cells.Select(x => x.Value).ToArray();
                for (var j = 0; j < headers.Length; j++)
                {
                    var period = headers[j].Trim();
                    if (period == "...")
                    {
                        continue;
                    }

                    decimal balance;
                    if (!decimal.TryParse(cols[j], out balance))
                    {
                        throw new ArgumentOutOfRangeException($"{cols[j]} is no a valid balance");
                    }

                    if (account.BalancesPerPeriod.ContainsKey(period))
                    {
                        account.BalancesPerPeriod[period] = balance;
                    }
                    else
                    {
                        account.BalancesPerPeriod.Add(period, balance);
                    }
                }
            }
        }
    }
}