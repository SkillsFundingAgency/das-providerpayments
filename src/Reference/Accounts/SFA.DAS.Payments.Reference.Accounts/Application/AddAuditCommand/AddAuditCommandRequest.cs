using System;
using MediatR;

namespace SFA.DAS.Payments.Reference.Accounts.Application.AddAuditCommand
{
    public class AddAuditCommandRequest : IRequest
    {
        public DateTime CorrelationDate { get; set; }
        public long AccountRead { get; set; }
        public bool CompletedSuccessfully { get; set; }
    }
}
