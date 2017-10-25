﻿using System;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.TestContext
{
    public class GlobalTestContextSetupException : Exception
    {
        public GlobalTestContextSetupException(Exception innerException)
            : base("Error setting up global test context: " + innerException?.Message, innerException)
        {
        }
    }
}
