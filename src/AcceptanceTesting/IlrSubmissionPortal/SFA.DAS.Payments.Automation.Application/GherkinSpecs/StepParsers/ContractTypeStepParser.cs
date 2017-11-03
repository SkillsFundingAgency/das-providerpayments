using Gherkin.Ast;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers.StepTableParsing;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers
{
    public class ContractTypeStepParser : StepParser
    {
        public ContractTypeStepParser()
            : base(new StepParserAbility("when", @"the Contract type in the ILR is:"))
        {
        }
        public override void Parse(Step step, Specification specification)
        {
            var table = step.Argument as DataTable;
            var contractTypes = TableParser.ParseTable<ContractTypeRecord>("Contract types", table);
            specification.Arrangement.ContractTypes.AddRange(contractTypes);
        }
    }
}
