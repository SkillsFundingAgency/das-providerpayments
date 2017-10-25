using System;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers.StepTableParsing
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnHeaderAttribute : Attribute
    {
        public ColumnHeaderAttribute(string header)
        {
            Header = header;
        }
        public string Header { get; }
    }
}
