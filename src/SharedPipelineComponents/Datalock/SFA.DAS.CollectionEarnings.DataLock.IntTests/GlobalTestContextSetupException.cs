using System;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests
{
    public class GlobalTestContextSetupException : Exception
    {
        public GlobalTestContextSetupException(Exception innerException)
            : base("Error setting up global test context: " + innerException?.Message, innerException)
        {
        }
    }
}