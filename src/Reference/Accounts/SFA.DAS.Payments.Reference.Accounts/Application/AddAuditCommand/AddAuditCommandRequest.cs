using System;
using MediatR;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.Application.AddAuditCommand
{
    public class AddAuditCommandRequest : IRequest
    {
        public DateTime CorrelationDate { get; set; }
        public long AccountsRead { get; set; }
        public bool CompletedSuccessfully { get; set; }
        public AuditType AuditType { get; set; }

        public AuditEntity ToAuditEntity()
        {
            return new AuditEntity
            {
                ReadDateTime = CorrelationDate,
                AccountsRead = AccountsRead,
                CompletedSuccessfully = CompletedSuccessfully,
                AuditType = (short)AuditType
            };
        }
    }
}
