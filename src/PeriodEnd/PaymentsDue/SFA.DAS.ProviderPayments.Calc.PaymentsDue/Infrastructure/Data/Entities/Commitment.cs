using System;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class Commitment : IHoldCommitmentInformation
    {
        public long? CommitmentId { get; set; }
        public string CommitmentVersionId { get; set; }
        public long Uln { get; set; }
        public long Ukprn { get; set; }
        public long? AccountId { get; set; }
        public string AccountVersionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StandardCode { get; set; }
        public int ProgrammeType { get; set; }
        public int FrameworkCode { get; set; }
        public int PathwayCode { get; set; }
        public int PaymentStatus { get; set; }
        public int Priority { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string LegalEntityName { get; set; }
        public long TransferSendingEmployerAccountId { get; set; }
        public DateTime? TransferApprovalDate { get; set; }
        public bool IsLevyPayer { get; set; }
        public decimal AgreedCost { get; set; }
    }
}
