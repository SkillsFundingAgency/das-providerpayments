using MediatR;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.ParseGherkinQuery
{
    public class ParseGherkinQueryRequest : IRequest<ParseGherkinQueryResponse>
    {
        public string GherkinSpecs { get; set; }
    }
}
