using MediatR;

namespace ExampleDCFSTask.Application.GetSomeDataQuery
{
    public class GetSomeDataQueryHandler : IRequestHandler<GetSomeDataQueryRequest, GetSomeDataQueryResponse>
    {
        public GetSomeDataQueryResponse Handle(GetSomeDataQueryRequest request)
        {
            return new GetSomeDataQueryResponse
            {
                IsValid = true,
                Items = new[]
                {
                    new Domain.SomeObject {Id = 1}
                }
            };
        }
    }
}
