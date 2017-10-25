using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.payments.Automation.Assertions
{
    public class AssertionResult
    {
        public long Ukprn { get; set; }
        public string Message { get; set; }
        public string ScenarioName { get; set; }
    }
}
