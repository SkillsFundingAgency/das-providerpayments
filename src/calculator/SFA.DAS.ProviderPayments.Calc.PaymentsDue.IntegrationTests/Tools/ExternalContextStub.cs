using System.Collections.Generic;
using CS.Common.External.Interfaces;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools
{
    internal class ExternalContextStub : IExternalContext
    {
        public IDictionary<string, string> Properties { get; set; }
    }
}