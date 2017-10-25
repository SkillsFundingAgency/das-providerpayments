using MediatR;
using NLog;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data;

namespace SFA.DAS.Payments.Reference.Commitments.Application.GetLastSeenEventIdQuery
{
    public class GetLastSeenEventIdQueryHandler : IRequestHandler<GetLastSeenEventIdQueryRequest, GetLastSeenEventIdQueryResponse>
    {
        private readonly IEventStreamPointerRepository _eventStreamPointerRepository;
        private readonly ILogger _logger;

        public GetLastSeenEventIdQueryHandler(IEventStreamPointerRepository eventStreamPointerRepository, ILogger logger)
        {
            _eventStreamPointerRepository = eventStreamPointerRepository;
            _logger = logger;
        }

        public GetLastSeenEventIdQueryResponse Handle(GetLastSeenEventIdQueryRequest message)
        {
            return new GetLastSeenEventIdQueryResponse
            {
                EventId = _eventStreamPointerRepository.GetLastEventId()
            };
        }
    }
}