using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities
{
    public class AccountLegalEntityEntity
    {
        public long Id { get; set; }
        [StringLength(6)]
        public string PublicHashedId { get; set; }
        public long AccountId { get; set; }
        public long LegalEntityId { get; set; }
    }
}