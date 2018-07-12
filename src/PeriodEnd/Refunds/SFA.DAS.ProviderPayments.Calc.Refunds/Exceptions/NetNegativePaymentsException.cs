using System;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Exceptions
{
    internal class NetNegativePaymentsException : Exception
    {
        public NetNegativePaymentsException(string message) : base(message)
        {}
    }
}
