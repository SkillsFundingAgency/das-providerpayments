using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.payments.Automation.Assertions;

namespace SFA.DAS.Payments.Automation.WebUI.Infrastructure.Assertions
{
    public interface IAssertionsService
    {
        Task<Dictionary<string, List<AssertionResult>>> AssertPaymentResults(IlrBuilderRequest request);
    }
}