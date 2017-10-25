using MediatR;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.Submission.TransformExpecationsCommand
{
    public class TransformExpecationsCommandRequest : IRequest<TransformExpecationsCommandResponse>
    {
        public SpecificationExpectations Expectations { get; set; }

        public int? ShiftToMonth { get; set; }
        public int? ShiftToYear { get; set; }
    }
}
