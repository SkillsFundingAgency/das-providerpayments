using System;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests
{
    public class GlobalTestContextSetupException : Exception
    {
        public GlobalTestContextSetupException(Exception innerException)
            : base("Error setting up global test context: " + innerException?.Message, innerException)
        {
        }
    }
}
