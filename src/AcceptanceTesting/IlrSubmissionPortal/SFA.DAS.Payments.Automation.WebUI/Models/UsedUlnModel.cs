using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFA.DAS.Payments.Automation.WebUI.Models
{
    public class UsedUlnModel
    {
        public long Uln { get; set; }
        public long Ukprn { get; set; }
        public string ScenarioName { get; set; }
        public string LearnRefNumber { get; set; }
    }
}