using System.Collections.Generic;
using CS.Common.External.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Common.Context;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools
{
    internal class ExternalContextStub : IExternalContext
    {
        public ExternalContextStub()
        {
            Properties = new Dictionary<string, string>
            {
                {ContextPropertyKeys.TransientDatabaseConnectionString, GlobalTestContext.Instance.TransientConnectionString},
                {ContextPropertyKeys.LogLevel, "DEBUG"},
                {ContextPropertyKeys.YearOfCollection, "1718"}
            };
        }
        public IDictionary<string, string> Properties { get; set; }
    }
}