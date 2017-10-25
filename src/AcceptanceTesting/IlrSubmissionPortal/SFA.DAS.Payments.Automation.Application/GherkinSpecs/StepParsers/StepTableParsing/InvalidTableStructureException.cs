using System;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers.StepTableParsing
{
    public class InvalidTableStructureException : Exception
    {
        public InvalidTableStructureException(string message)
            : base(message)
        {

        }
    }
}
