using System;

namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities
{
    public class AuditEntity
    {
        public DateTime ReadDateTime { get; set; }
        public long AccountsRead { get; set; }
        public short AuditType { get; set; }
        public bool CompletedSuccessfully { get; set; }
    }
}