using System.Collections.Generic;
using CS.Common.External.Interfaces;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.Common
{
    internal class ExternalContextStub : IExternalContext
    {
        public IDictionary<string, string> Properties { get; set; }
    }
}
