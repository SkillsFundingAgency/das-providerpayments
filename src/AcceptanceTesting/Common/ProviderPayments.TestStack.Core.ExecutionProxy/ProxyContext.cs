using System;
using System.Collections.Generic;
using CS.Common.External.Interfaces;

namespace ProviderPayments.TestStack.Core.ExecutionProxy
{
    [Serializable]
    public class ProxyContext : IExternalContext
    {
        public IDictionary<string, string> Properties { get; set; }
    }
}