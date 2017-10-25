using MediatR;

namespace SFA.DAS.Payments.Reference.Commitments.Application.SetLastSeenEventIdCommand
{
    public class SetLastSeenEventIdCommandRequest : IRequest
    {
        public long LastSeenEventId { get; set; }
    }
}
