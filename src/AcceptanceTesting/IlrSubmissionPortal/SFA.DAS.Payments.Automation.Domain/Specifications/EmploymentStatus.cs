using System.ComponentModel;

namespace SFA.DAS.Payments.Automation.Domain.Specifications
{
    public enum EmploymentStatus
    {
        [Description("in paid employment")]
        InPaidEmployment = 10,

        [Description("not in paid employment")]
        NotInPaidEmployment = 11
    }
}
