using System.ComponentModel;

namespace SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels
{
    public enum ContractType
    {
        [Description("DAS")]
        ContractWithSfa = 1,

        [Description("Non-DAS")]
        ContractWithEmployer = 2
    }
}