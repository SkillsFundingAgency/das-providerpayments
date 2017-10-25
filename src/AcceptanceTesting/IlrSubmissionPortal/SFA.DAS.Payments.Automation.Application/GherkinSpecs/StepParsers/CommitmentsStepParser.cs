using Gherkin.Ast;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers.StepTableParsing;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers
{
    public class CommitmentsStepParser : StepParser
    {
        public CommitmentsStepParser()
            : base(new StepParserAbility("given", @"the following commitments exist( on [0-9]{1,2}\/[0-9]{1,2}\/[0-9]{4}){0,1}"))
        {
        }
        public override void Parse(Step step, Specification specification)
        {
            var table = step.Argument as DataTable;
            var commitments = TableParser.ParseValueTable<CommitmentEntity>("Commitments", table);
            foreach (var commitment in commitments)
            {
                if (!commitment.StandardCode.HasValue && !commitment.FrameworkCode.HasValue)
                {
                    commitment.StandardCode = Defaults.StandardCode;
                }
                if (!commitment.EffectiveFrom.HasValue)
                {
                    commitment.EffectiveFrom = commitment.StartDate;
                }
            }
            specification.Arrangement.Commitments.AddRange(commitments);
        }

        private class CommitmentEntity : Commitment
        {
            [ColumnHeader("Employer")]
            [DefaultValue(Defaults.EmployerKey)]
            public override string EmployerKey { get; set; }

            [ColumnHeader("ULN")]
            [DefaultValue(Defaults.LearnerKey)]
            public override string LearnerKey { get; set; }

            [DefaultValue(1)]
            public override int Priority { get; set; }

            [ColumnHeader("Provider")]
            [DefaultValue(Defaults.ProviderKey)]
            public override string ProviderKey { get; set; }

            [ColumnHeader("CommitmentId")]
            [DefaultValue(Defaults.CommitmentId)]
            public override long CommitmentId { get; set; }

            [ColumnHeader("Status")]
            [DefaultValue(Defaults.Status)]
            public override CommitmentPaymentStatus Status { get; set; }

            [ColumnHeader("VersionId")]
            [DefaultValue(Defaults.VersionId)]
            public override int VersionId { get; set; }
        }
    }
}
