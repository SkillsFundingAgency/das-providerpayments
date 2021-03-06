﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Interfaces.Payments;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities
{
    public class RawEarning : IHoldCourseInformation, IIdentifyCommitments
    {
        public long Ukprn { get; set; }

        [StringLength(12)]
        public string LearnRefNumber { get; set; }

        public long Uln { get; set; }

        [Range(0, 20)]
        public int AimSeqNumber { get; set; }

        [StringLength(8)]
        public string LearnAimRef { get; set; }

        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EpisodeStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EpisodeEffectiveTnpStartDate { get; set; }

        [Range(1, 14)]
        public int Period { get; set; }

        [Range(0, 1000)]
        public int ProgrammeType { get; set; }

        [Range(0, 1000)]
        public int FrameworkCode { get; set; }

        [Range(0, 1000)]
        public int PathwayCode { get; set; }

        [Range(0, 1000)]
        public int StandardCode { get; set; }

        [Range(0.9, 1.0)]
        public decimal SfaContributionPercentage { get; set; }

        [StringLength(100)]
        public string FundingLineType { get; set; }

        [DataType(DataType.Date)]
        public DateTime LearningStartDate { get; set; }

        public ApprenticeshipContractType ApprenticeshipContractType { get; set; }

        public decimal TransactionType01 { get; set; }
        public decimal TransactionType02 { get; set; }
        public decimal TransactionType03 { get; set; }
        public decimal TransactionType04 { get; set; }
        public decimal TransactionType05 { get; set; }
        public decimal TransactionType06 { get; set; }
        public decimal TransactionType07 { get; set; }
        public decimal TransactionType08 { get; set; }
        public decimal TransactionType09 { get; set; }
        public decimal TransactionType10 { get; set; }
        public decimal TransactionType11 { get; set; }
        public decimal TransactionType12 { get; set; }
        public decimal TransactionType13 { get; set; }
        public decimal TransactionType14 { get; set; }
        public decimal TransactionType15 { get; set; }
        public decimal TransactionType16 { get; set; }

        public decimal CumulativePmrs { get; set; }
        public int ExemptionCodeForCompletionHoldback { get; set; }

        [NotMapped]
        public int DeliveryMonth { get; set; }

        [NotMapped]
        public int DeliveryYear { get; set; }

        public bool UseLevyBalance => ApprenticeshipContractType == ApprenticeshipContractType.Levy && SfaContributionPercentage < 1;

        [DataType(DataType.Date)]
        public DateTime? FirstIncentiveCensusDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SecondIncentiveCensusDate { get; set; }

        public decimal AgreedPrice { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LearnerAdditionalPaymentsDate { get; set; }
    }
}