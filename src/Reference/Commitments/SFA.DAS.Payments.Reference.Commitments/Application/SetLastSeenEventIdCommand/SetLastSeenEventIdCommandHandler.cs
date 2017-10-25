using System;
using MediatR;
using NLog;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data;

namespace SFA.DAS.Payments.Reference.Commitments.Application.SetLastSeenEventIdCommand
{
    public class SetLastSeenEventIdCommandHandler : IRequestHandler<SetLastSeenEventIdCommandRequest, Unit>
    {
        private readonly IEventStreamPointerRepository _eventStreamPointerRepository;
        private readonly ILogger _logger;

        public SetLastSeenEventIdCommandHandler(IEventStreamPointerRepository eventStreamPointerRepository, ILogger logger)
        {
            _eventStreamPointerRepository = eventStreamPointerRepository;
            _logger = logger;
        }

        public Unit Handle(SetLastSeenEventIdCommandRequest message)
        {
            _logger.Info($"Setting last seen event id to {message.LastSeenEventId}");

            try
            {
                _eventStreamPointerRepository.SetLastEventId(message.LastSeenEventId, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failed to set last seen event id to {message.LastSeenEventId}.");
                throw new PersistenceException(ex.Message, ex);
            }
            return Unit.Value;
        }
    }
}