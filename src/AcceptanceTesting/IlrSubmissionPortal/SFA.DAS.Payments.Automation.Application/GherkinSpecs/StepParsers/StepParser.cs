using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gherkin.Ast;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers
{
    public abstract class StepParser
    {
        protected StepParser(params StepParserAbility[] abilities)
        {
            Abilities = abilities;
        }

        public StepParserAbility[] Abilities { get; }

        public virtual bool CanParse(string keyword, string description)
        {
            return Abilities != null && Abilities.Any(x => x.Keyword.Equals(keyword, StringComparison.CurrentCultureIgnoreCase)
                                                        && Regex.IsMatch(description, x.DescriptionPattern, RegexOptions.IgnoreCase));
        }

        public abstract void Parse(Step step, Specification specification);
    }
}
