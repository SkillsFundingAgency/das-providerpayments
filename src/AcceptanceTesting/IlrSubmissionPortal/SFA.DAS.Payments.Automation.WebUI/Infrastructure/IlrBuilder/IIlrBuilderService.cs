using SFA.DAS.Payments.Automation.WebUI.ViewModels;
using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.WebUI.Infrastructure
{
    public interface IIlrBuilderService
    {
        IlrBuilderResponse BuildIlrWithRefenceData(IlrBuilderRequest request);

    }
}
