using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Automation.Infrastructure.Entities
{
    public class UsedUlnEntity
    {
        public long Uln { get; set; }
        public long Ukprn { get; set; }
        public string ScenarioName { get; set; }
        public string LearnRefNumber { get; set; }

    }
}
