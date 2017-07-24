using System.Collections.Generic;
using CS.Common.External.Interfaces;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.Common.Context;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.IntegrationTests.TestComponents
{
    public class TestExternalTaskContext : IExternalContext
    {
        public TestExternalTaskContext()
        {
            Properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, GlobalTestContext.Instance.TransientConnectionString },
                { ContextPropertyKeys.LogLevel, "DEBUG" },
                { PaymentsContextPropertyKeys.YearOfCollection, "1617" }
            };
        }
        public IDictionary<string, string> Properties { get; set; }
    }
}
