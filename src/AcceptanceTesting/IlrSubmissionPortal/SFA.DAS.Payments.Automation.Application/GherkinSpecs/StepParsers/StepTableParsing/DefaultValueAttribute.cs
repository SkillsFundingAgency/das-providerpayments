using System;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers.StepTableParsing
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultValueAttribute : Attribute
    {
        public DefaultValueAttribute(object value)
        {
            Value = value;
        }
        public object Value { get; }
    }
}
