using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class NonPayableEarningEntity : RequiredPaymentEntity, IFundingDue, IHoldCommitmentInformation
    {
        public NonPayableEarningEntity()
        {}

        public NonPayableEarningEntity(RawEarning rawEarning)
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
        public string Reason { get; set; }
    }
}