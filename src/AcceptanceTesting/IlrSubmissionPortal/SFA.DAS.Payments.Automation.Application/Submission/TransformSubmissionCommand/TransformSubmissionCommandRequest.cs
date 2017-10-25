using MediatR;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.Submission.TransformSubmissionCommand
{
    public class TransformSubmissionCommandRequest : IRequest<TransformSubmissionCommandResponse>
    {
        public Specification Specification { get; set; }

        public int? ShiftToMonth { get; set; }
        public int? ShiftToYear { get; set; }
    }
}
