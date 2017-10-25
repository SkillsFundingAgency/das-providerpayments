using SFA.DAS.payments.Automation.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFA.DAS.Payments.Automation.WebUI.ViewModels
{
    public class AssertionResults
    {
        public Dictionary<string,List<AssertionResult>> Results { get; set; }
    }
}