﻿using System;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.IntegrationTests
{
    public class GlobalTestContextSetupException : Exception
    {
        public GlobalTestContextSetupException(Exception innerException)
            : base("Error setting up global test context: " + innerException?.Message, innerException)
        {
        }
    }
}