
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings
{
    public class PeriodEarning
    {
        public long? CommitmentId { get; set; }
        public string CommitmentVersionId { get; set; }
        public string AccountId { get; set; }
        public string AccountVersionId { get; set; }
        public long Ukprn { get; set; }
        public long Uln { get; set; }
        public string LearnerReferenceNumber { get; set; }
        public int AimSequenceNumber { get; set; }

        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }

        public int CollectionPeriodNumber { get; set; }
        public string CollectionAcademicYear { get; set; }

        public int CalendarMonth { get; set; }
        public int CalendarYear { get; set; }

        public decimal EarnedValue { get; set; }
        public TransactionType Type { get; set; }

        public int ApprenticeshipContractType { get; set; }
        public string PriceEpisodeIdentifier { get; set; }

        public decimal SfaContributionPercentage { get; set; }
        public string FundingLineType { get; set; }
    }
}
