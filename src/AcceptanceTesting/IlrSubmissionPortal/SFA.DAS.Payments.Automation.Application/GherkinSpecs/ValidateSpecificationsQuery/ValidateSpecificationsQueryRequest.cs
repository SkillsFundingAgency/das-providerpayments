using MediatR;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.ValidateSpecificationsQuery
{
    public class ValidateSpecificationsQueryRequest : IRequest<ValidateSpecificationsQueryResponse>
    {
        public Specification[] Specifications { get; set; }
    }
}
