using System;
using System.Collections.Generic;
using CS.Common.External.Interfaces;

namespace ProviderPayments.TestStack.Engine.ExecutionProxy
{
    [Serializable]
    public class TestStackContext : IExternalContext
    {
        public IDictionary<string, string> Properties { get; set; }
    }
}