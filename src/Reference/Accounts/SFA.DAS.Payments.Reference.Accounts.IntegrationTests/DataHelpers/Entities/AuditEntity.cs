using System;

namespace SFA.DAS.Payments.Reference.Accounts.IntegrationTests.DataHelpers.Entities
{
    internal class AuditEntity
    {
        public DateTime ReadDateTime { get; set; }
        public long AccountsRead { get; set; }
        public bool CompletedSuccessfully { get; set; }
    }
}
