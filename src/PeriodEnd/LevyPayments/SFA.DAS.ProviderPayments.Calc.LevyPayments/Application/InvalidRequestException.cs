using System;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application
{
    public class InvalidRequestException : Exception
    {
        public InvalidRequestException(string message) : base(message)
        {
        }
    }
}
