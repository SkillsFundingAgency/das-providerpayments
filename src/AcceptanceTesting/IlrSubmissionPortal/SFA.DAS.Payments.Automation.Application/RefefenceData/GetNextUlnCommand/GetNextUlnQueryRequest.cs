

using MediatR;

namespace SFA.DAS.Payments.Automation.Application.RefefenceData.GetNextUlnCommand
{
    public class GetNextUlnQueryRequest : IRequest<GetNextUlnQueryApplicationScalarResponse>
    {
        public string LearnerKey { get; set; }
        public string Scenarioname { get; set; }
        public long Ukprn { get; set; }


    }
}
