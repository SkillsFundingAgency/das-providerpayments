﻿using System;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application
{
    public abstract class Response
    {
        public bool IsValid { get; set; }
        public Exception Exception { get; set; }
    }
}