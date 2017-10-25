using MediatR;

namespace SFA.DAS.Payments.Reference.Commitments.Application.GetNextBatchOfCommitmentEventsQuery
{
    public class GetNextBatchOfCommitmentEventsQueryRequest : IRequest<GetNextBatchOfCommitmentEventsQueryResponse>
    {
        public long LastSeenEventId { get; set; }
    }
}
