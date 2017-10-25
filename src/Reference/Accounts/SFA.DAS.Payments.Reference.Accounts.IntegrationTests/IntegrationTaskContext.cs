using System.Collections.Generic;
using CS.Common.External.Interfaces;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.Reference.Accounts.Context;

namespace SFA.DAS.Payments.Reference.Accounts.IntegrationTests
{
    public class IntegrationTaskContext : IExternalContext
    {
        public IntegrationTaskContext()
        {
            Properties = new Dictionary<string, string>
            {
                {ContextPropertyKeys.TransientDatabaseConnectionString, GlobalTestContext.Instance.TransientConnectionString},
                {ContextPropertyKeys.LogLevel, "DEBUG"},
                {KnownContextKeys.AccountsApiBaseUrl, "http://test" },
                { KnownContextKeys.AccountsApiClientId, "the-client" },
                { KnownContextKeys.AccountsApiClientSecret, "super-secret" },
                { KnownContextKeys.AccountsApiIdentifierUri, "http://unit.tests" },
                { KnownContextKeys.AccountsApiTenant, "http://ad.test" }
            };
        }

        public IDictionary<string, string> Properties { get; set; }
    }
}
