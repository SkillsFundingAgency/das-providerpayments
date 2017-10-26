using System.Collections.Generic;
using CS.Common.External.Interfaces;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools
{
    public class ExternalContextStub : IExternalContext
    {
        public IDictionary<string, string> Properties { get; set; }
    }
}