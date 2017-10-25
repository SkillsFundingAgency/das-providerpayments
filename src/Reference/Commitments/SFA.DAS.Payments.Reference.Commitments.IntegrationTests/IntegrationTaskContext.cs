using System.Collections.Generic;
using CS.Common.External.Interfaces;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.Payments.Reference.Commitments.IntegrationTests
{
    public class IntegrationTaskContext : IExternalContext
    {
        public IntegrationTaskContext()
        {
            Properties = new Dictionary<string, string>
            {
                {ContextPropertyKeys.TransientDatabaseConnectionString, GlobalTestContext.Instance.TransientConnectionString},
                {ContextPropertyKeys.LogLevel, "DEBUG"},
                {ImportCommitmentsContextKeys.BaseUrl, "http://test" },
                {ImportCommitmentsContextKeys.ClientToken, "token" }
            };
        }

        public IDictionary<string, string> Properties { get; set; }
    }
}
