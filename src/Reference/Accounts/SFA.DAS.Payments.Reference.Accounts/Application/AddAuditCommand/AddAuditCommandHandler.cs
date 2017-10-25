using System;
using MediatR;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data;

namespace SFA.DAS.Payments.Reference.Accounts.Application.AddAuditCommand
{
    public class AddAuditCommandHandler : IRequestHandler<AddAuditCommandRequest, Unit>
    {
        private readonly IAuditRepository _auditRepository;

        public AddAuditCommandHandler(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public Unit Handle(AddAuditCommandRequest message)
        {
            _auditRepository.CreateAudit(message.CorrelationDate, message.AccountRead, message.CompletedSuccessfully);
            return Unit.Value;
        }
    }
}