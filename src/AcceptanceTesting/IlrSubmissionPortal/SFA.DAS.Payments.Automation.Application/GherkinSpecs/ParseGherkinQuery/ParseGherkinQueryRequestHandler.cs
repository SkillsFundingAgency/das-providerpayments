using System;
using System.IO;
using System.Linq;
using System.Text;
using Gherkin.Ast;
using MediatR;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.ParseGherkinQuery
{
    public class ParseGherkinQueryRequestHandler : IRequestHandler<ParseGherkinQueryRequest, ParseGherkinQueryResponse>
    {
        private static readonly string[] PrimaryScenarioKeywords = { "Given", "When", "Then" };
        private static readonly StepParser[] StepParsers =
        {
            new CommitmentsStepParser(),
            new IndefinateLevyBalanceStepParser(),
            new NoLevyBalanceStepParser(),
            new SpecificLevyBalanceStepParser(),
            new SubmissionStepParser(),
            new ContractTypeStepParser(),
            new EmploymentStatusStepParser(),
            new EarningsAndPaymentsStepParser()
        };

        public ParseGherkinQueryResponse Handle(ParseGherkinQueryRequest message)
        {
            try
            {
                var doc = ParseGherkin(message.GherkinSpecs);
                var docSpecs = doc.Feature.Children.ToArray();
                var specifications = new Specification[docSpecs.Length];

                for (var i = 0; i < specifications.Length; i++)
                {
                    specifications[i] = ParseScenario(docSpecs[i]);
                }

                return new ParseGherkinQueryResponse
                {
                    Results = specifications
                };
            }
            catch (Exception ex)
            {
                return new ParseGherkinQueryResponse
                {
                    Error = new ParserException(ex)
                };
            }
        }

        private Gherkin.Ast.GherkinDocument ParseGherkin(string specs)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(specs)))
            using (var reader = new StreamReader(stream))
            {
                var parser = new Gherkin.Parser();
                return parser.Parse(reader);
            }
        }
        private Specification ParseScenario(ScenarioDefinition scenarioDefinition)
        {
            var specification = new Specification
            {
                Name = scenarioDefinition.Name
            };

            var currentStepKeyword = "";
            foreach (var step in scenarioDefinition.Steps)
            {
                if (PrimaryScenarioKeywords.Any(x => x.Equals(step.Keyword.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                {
                    currentStepKeyword = step.Keyword.Trim().ToLower();
                }

                ParseStep(currentStepKeyword, step, specification);
            }
            return specification;
        }
        private void ParseStep(string keyword, Step step, Specification specification)
        {
            foreach (var parser in StepParsers)
            {
                if (parser.CanParse(keyword, step.Text))
                {
                    parser.Parse(step, specification);
                    return;
                }
            }
        }
    }
}