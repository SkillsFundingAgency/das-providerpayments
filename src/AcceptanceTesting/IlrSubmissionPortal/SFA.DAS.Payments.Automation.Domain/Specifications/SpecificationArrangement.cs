using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.Domain.Specifications
{
    public class SpecificationArrangement
    {
        public List<EmployerAccountLevyBalances> LevyBalances { get; set; } = new List<EmployerAccountLevyBalances>();
        public List<Commitment> Commitments { get; set; } = new List<Commitment>();

        public List<LearnerRecord> LearnerRecords { get; set; } = new List<LearnerRecord>();
        public List<ContractTypeRecord> ContractTypes { get; set; } = new List<ContractTypeRecord>();
        public List<LearnerEmploymentStatus> EmploymentStatuses { get; set; } = new List<LearnerEmploymentStatus>();
        public List<LearningSupportRecord> LearningSupports { get; set; } = new List<LearningSupportRecord>();

    }
}