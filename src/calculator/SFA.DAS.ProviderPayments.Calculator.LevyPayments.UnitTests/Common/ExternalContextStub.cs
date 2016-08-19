using System.Collections.Generic;
using CS.Common.External.Interfaces;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Common
{
    internal class ExternalContextStub : IExternalContext
    {
        public IDictionary<string, string> Properties { get; set; }
    }
}
