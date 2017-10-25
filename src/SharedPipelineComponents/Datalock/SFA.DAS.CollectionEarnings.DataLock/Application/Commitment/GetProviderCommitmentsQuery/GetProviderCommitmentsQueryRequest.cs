using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetProviderCommitmentsQuery
{
    public class GetProviderCommitmentsQueryRequest : IRequest<GetProviderCommitmentsQueryResponse>
    {
        public long Ukprn { get; set; }
    }
}
