using System.Collections.Generic;
using CS.Common.External.Interfaces;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Tools
{
    internal class ExternalContextStub : IExternalContext
    {
        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>()
        {
            {"foo", "bar" }
        };
    }
}