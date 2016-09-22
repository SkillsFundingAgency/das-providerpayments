﻿using System.Collections.Generic;
using CS.Common.External.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Common.Context;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools
{
    public class IntegrationTaskContext : IExternalContext
    {
        public IntegrationTaskContext()
        {
            Properties = new Dictionary<string, string>
            {
                {ContextPropertyKeys.TransientDatabaseConnectionString, GlobalTestContext.Instance.ConnectionString},
                {ContextPropertyKeys.LogLevel, "DEBUG"}
            };
        }

        public IDictionary<string, string> Properties { get; set; }
    }
}
