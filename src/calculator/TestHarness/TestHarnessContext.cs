using System.Collections.Generic;
using CS.Common.External.Interfaces;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Context;

namespace TestHarness
{
    internal class TestHarnessContext : IExternalContext
    {
        public TestHarnessContext()
        {
            Properties = new Dictionary<string, string>
            {
                {ContextPropertyKeys.TransientDatabaseConnectionString, "server=.;database=ProvPayTestStack;trusted_connection=true;"},
                {ContextPropertyKeys.LogLevel, "Debug"}
            };
        }
        public IDictionary<string, string> Properties { get; set; }
    }
}
