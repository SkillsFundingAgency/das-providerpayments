using System.Collections.Generic;
using CS.Common.External.Interfaces;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.ProviderPayments.Calc.Transfers.IntegrationTests.Utilities
{
    internal class ExternalContextStub : IExternalContext
    {
        public ExternalContextStub()
        {
            Properties = new Dictionary<string, string>
            {
                {ContextPropertyKeys.TransientDatabaseConnectionString, GlobalTestContext.Instance.TransientConnectionString},
                {ContextPropertyKeys.LogLevel, "DEBUG"},
            };
        }
        public IDictionary<string, string> Properties { get; set; }
    }
}