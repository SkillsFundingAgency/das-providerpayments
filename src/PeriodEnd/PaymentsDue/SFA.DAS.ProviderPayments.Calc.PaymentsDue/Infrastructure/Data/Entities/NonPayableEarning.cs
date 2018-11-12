using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class NonPayableEarning : FundingDue
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

        public NonPayableEarning(RequiredPayment requiredPayment)
        {
            Uln = requiredPayment.Uln;
            LearnRefNumber = requiredPayment.LearnRefNumber;
            AimSeqNumber = requiredPayment.AimSeqNumber;
            Ukprn = requiredPayment.Ukprn;
            StandardCode = requiredPayment.StandardCode;
            ProgrammeType = requiredPayment.ProgrammeType;
            FrameworkCode = requiredPayment.FrameworkCode;
            PathwayCode = requiredPayment.PathwayCode;
            ApprenticeshipContractType = requiredPayment.ApprenticeshipContractType;
            PriceEpisodeIdentifier = requiredPayment.PriceEpisodeIdentifier;
            SfaContributionPercentage = requiredPayment.SfaContributionPercentage;
            FundingLineType = requiredPayment.FundingLineType;
            LearnAimRef = requiredPayment.LearnAimRef;
            LearningStartDate = requiredPayment.LearningStartDate;
            DeliveryMonth = requiredPayment.DeliveryMonth;
            DeliveryYear = requiredPayment.DeliveryYear;
        }


        [StringLength(1000)]
        public string PaymentFailureMessage { get; set; }

        public PaymentFailureType PaymentFailureReason { get; set; }
    }
}