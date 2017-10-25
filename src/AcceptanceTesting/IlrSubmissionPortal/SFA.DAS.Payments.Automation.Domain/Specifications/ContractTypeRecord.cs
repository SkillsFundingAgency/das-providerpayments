using System;

namespace SFA.DAS.Payments.Automation.Domain.Specifications
{
    public class ContractTypeRecord
    {
        public ContractType ContractType { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
