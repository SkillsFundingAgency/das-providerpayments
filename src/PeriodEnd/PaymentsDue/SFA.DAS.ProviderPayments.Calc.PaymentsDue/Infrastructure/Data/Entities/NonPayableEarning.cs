using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class NonPayableEarning : RequiredPayment, ICanStoreCommitmentInformation
    {
        public NonPayableEarning()
        {}

        public NonPayableEarning(RawEarning rawEarning)
        {
            Uln = rawEarning.Uln;
            LearnRefNumber = rawEarning.LearnRefNumber;
            AimSeqNumber = rawEarning.AimSeqNumber;
            Ukprn = rawEarning.Ukprn;
            StandardCode = rawEarning.StandardCode;
            ProgrammeType = rawEarning.ProgrammeType;
            FrameworkCode = rawEarning.FrameworkCode;
            PathwayCode = rawEarning.PathwayCode;
            ApprenticeshipContractType = rawEarning.ApprenticeshipContractType;
            PriceEpisodeIdentifier = rawEarning.PriceEpisodeIdentifier;
            SfaContributionPercentage = rawEarning.SfaContributionPercentage;
            FundingLineType = rawEarning.FundingLineType;
            LearnAimRef = rawEarning.LearnAimRef;
            LearningStartDate = rawEarning.LearningStartDate;
            DeliveryMonth = rawEarning.DeliveryMonth;
            DeliveryYear = rawEarning.DeliveryYear;
        }

        [StringLength(1000)]
        public string PaymentFailureMessage { get; set; }

        public PaymentFailureType PaymentFailureReason { get; set; }
    }
}