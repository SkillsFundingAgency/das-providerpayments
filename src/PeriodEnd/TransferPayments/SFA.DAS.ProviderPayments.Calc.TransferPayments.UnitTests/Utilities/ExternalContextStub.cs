using System.Collections.Generic;
using CS.Common.External.Interfaces;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.Utilities
{
    internal class ExternalContextStub : IExternalContext
    {
        public IDictionary<string, string> Properties { get; set; }
    }
}