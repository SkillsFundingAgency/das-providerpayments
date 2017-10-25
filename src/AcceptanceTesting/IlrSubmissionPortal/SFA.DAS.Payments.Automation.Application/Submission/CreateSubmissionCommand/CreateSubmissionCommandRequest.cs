using System.Collections.Generic;
using MediatR;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.CreateSubmissionCommand
{
    public class CreateSubmissionCommandRequest : IRequest<CreateSubmissionCommandResponse>
    {
        public long Ukprn { get; set; }
        public string AcademicYear { get; set; }
        public Dictionary<string,string> LearnerScenarios { get; set; }

        public List<LearnerRecord> LearnerRecords { get; set; } = new List<LearnerRecord>();
        public List<Commitment> Commitments { get; set; } = new List<Commitment>();
        public List<EmployerAccountLevyBalances> Accounts { get; set; } = new List<EmployerAccountLevyBalances>();
        public List<ContractTypeRecord> ContractTypes { get; set; } = new List<ContractTypeRecord>();
        public List<LearningSupportRecord> LearningSupports { get; set; } = new List<LearningSupportRecord>();
        public List<LearnerEmploymentStatus> EmploymentStatuses { get; set; } = new List<LearnerEmploymentStatus>();

    }


}
