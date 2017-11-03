using System;
using System.Text.RegularExpressions;
using Gherkin.Ast;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers.StepTableParsing;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers
{
    public class EmploymentStatusStepParser : StepParser
    {
        public EmploymentStatusStepParser()
            : base(new StepParserAbility("when", @"the employment status in the ILR is:"))
        {
        }
        public override void Parse(Step step, Specification specification)
        {
            var table = step.Argument as DataTable;
            var employmentStatuses = TableParser.ParseTable<LearnerEmploymentStatusEntity>("Employment statuses", table);
            foreach (var status in employmentStatuses)
            {
                if (string.IsNullOrEmpty(status.SmallEmployer))
                {
                    continue;
                }

                var match = Regex.Match(status.SmallEmployer, @"^([A-Z]{3})([0-9]{1})$");
                if (!match.Success)
                {
                    throw new Exception($"{status.SmallEmployer} is not a valid value for small employer");
                }

                status.MonitoringType = (EmploymentStatusMonitoringType)Enum.Parse(typeof(EmploymentStatusMonitoringType), match.Groups[1].Value);
                status.MonitoringCode = int.Parse(match.Groups[2].Value);
            }
            specification.Arrangement.EmploymentStatuses.AddRange(employmentStatuses);
        }

        private class LearnerEmploymentStatusEntity : LearnerEmploymentStatus
        {
            [ColumnHeader("Employer")]
            [Optional]
            public override string EmployerKey { get; set; }

            [Optional]
            public string SmallEmployer { get; set; }

            [Optional]
            public override EmploymentStatusMonitoringType MonitoringType { get; set; }

            [Optional]
            public override int MonitoringCode { get; set; }
        }
    }
}
