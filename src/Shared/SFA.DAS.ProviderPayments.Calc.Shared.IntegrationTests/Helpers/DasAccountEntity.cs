using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers
{
    public class DasAccountEntity
    {
        public long AccountId { get; set; }
        [StringLength(50)]
        public string AccountHashId { get; set; }
        [StringLength(125)]
        public string AccountName { get; set; }
        public decimal? Balance { get; set; }
        [StringLength(50)]
        public string VersionId { get; set; }
        public bool IsLevyPayer { get; set; }
        public decimal? TransferAllowance { get; set; }
    }
}