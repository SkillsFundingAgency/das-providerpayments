﻿using System;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Exceptions
{
    public class LevyPaymentsInvalidContextException : Exception
    {
        public LevyPaymentsInvalidContextException(string message)
            : base(message)
        {
        }

        public LevyPaymentsInvalidContextException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
