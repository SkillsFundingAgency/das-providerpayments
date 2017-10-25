using System;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs
{
    public class ParserException : Exception
    {
        public ParserException(Exception innerException) : base(innerException.Message, innerException)
        {

        }
    }
}
