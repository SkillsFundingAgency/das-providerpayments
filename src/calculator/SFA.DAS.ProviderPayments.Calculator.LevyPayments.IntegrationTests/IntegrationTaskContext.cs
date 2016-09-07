using System.Collections.Generic;
using CS.Common.External.Interfaces;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Context;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.IntegrationTests
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
