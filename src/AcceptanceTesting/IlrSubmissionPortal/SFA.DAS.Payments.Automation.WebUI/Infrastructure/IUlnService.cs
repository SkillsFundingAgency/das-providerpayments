using SFA.DAS.Payments.Automation.WebUI.Models;
using SFA.DAS.Payments.Automation.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Automation.WebUI.Infrastructure
{
    public interface IUlnService
    {
        List<UsedUlnModel> GetAllUsedUlns();
        List<long> GetNewUlns(int count);
    }
}
