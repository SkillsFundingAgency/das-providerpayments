﻿using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Interfaces.Payments;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    /// <summary>
    /// Used as an intermediary step in calculating payments due
    /// </summary>
    public class FundingDue : RequiredPayment, 
        ICanStoreCommitmentInformation, 
        IHoldCommitmentInformation,
        IHoldCourseInformation
    {
        public FundingDue()
        {
        }

        public FundingDue(RawEarning rawEarning)
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
            UseLevyBalance = rawEarning.UseLevyBalance;
        }
    }
}
