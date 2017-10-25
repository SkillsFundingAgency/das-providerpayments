using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFA.DAS.Payments.Automation.WebUI.ViewModels
{
    public class NewUlnViewModel
    {
        public NewUlnViewModel()
        {
            Ulns = new List<long>();
        }
        public List<long> Ulns { get; set; }
    }
}