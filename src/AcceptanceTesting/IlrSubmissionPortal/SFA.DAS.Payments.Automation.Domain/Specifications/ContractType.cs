using System.ComponentModel;

namespace SFA.DAS.Payments.Automation.Domain.Specifications
{
    public enum ContractType
    {
        [Description("DAS")]
        ContractWithEmployer = 1,

        [Description("Non-DAS")]
        ContractWithSfa = 2
    }
}
