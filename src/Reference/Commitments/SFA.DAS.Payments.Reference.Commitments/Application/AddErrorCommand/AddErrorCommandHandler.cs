using MediatR;
using NLog;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data;

namespace SFA.DAS.Payments.Reference.Commitments.Application.AddErrorCommand
{
    public class AddErrorCommandHandler : IRequestHandler<AddErrorCommandRequest, Unit>
    {
        private readonly IProcessErrorRepository _processErrorRepository;
        private readonly ILogger _logger;

        public AddErrorCommandHandler(IProcessErrorRepository processErrorRepository, ILogger logger)
        {
            _processErrorRepository = processErrorRepository;
            _logger = logger;
        }

        public Unit Handle(AddErrorCommandRequest message)
        {
            _processErrorRepository.WriteError(message.Error.ToString());

            return Unit.Value;
        }
    }
}