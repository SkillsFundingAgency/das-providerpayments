using System.Collections.Generic;
using CS.Common.External.Interfaces;

namespace RunOpaEarningsCalculatorTask
{
    public class EarningsCalculatorContext : IExternalContext
    {
        public IDictionary<string, string> Properties { get; set; }
    }
}