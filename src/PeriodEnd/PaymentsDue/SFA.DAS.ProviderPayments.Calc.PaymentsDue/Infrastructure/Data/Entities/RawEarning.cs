using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class RawEarning 
    {
        public RawEarning()
        {
        }

        public RawEarning(RawEarning copy)
        {
            Ukprn = copy.Ukprn;
            LearnRefNumber = copy.LearnRefNumber;
            Uln = copy.Uln;
            AimSeqNumber = copy.AimSeqNumber;
            LearnAimRef = copy.LearnAimRef;
            PriceEpisodeIdentifier = copy.PriceEpisodeIdentifier;
            EpisodeStartDate = copy.EpisodeStartDate;
            EpisodeEffectiveTnpStartDate = copy.EpisodeEffectiveTnpStartDate;
            Period = copy.Period;
            ProgrammeType = copy.ProgrammeType;
            StandardCode = copy.StandardCode;
            FrameworkCode = copy.FrameworkCode;
            PathwayCode = copy.PathwayCode;
            SfaContributionPercentage = copy.SfaContributionPercentage;
            FundingLineType = copy.FundingLineType;
            LearningStartDate = copy.LearningStartDate;
            ApprenticeshipContractType = copy.ApprenticeshipContractType;
            DeliveryMonth = copy.DeliveryMonth;
            DeliveryYear = copy.DeliveryYear;
            UseLevyBalance = copy.UseLevyBalance;
            
            TransactionType01 = copy.TransactionType01;
            TransactionType02 = copy.TransactionType02;
            TransactionType03 = copy.TransactionType03;
            TransactionType04 = copy.TransactionType04;
            TransactionType05 = copy.TransactionType05;
            TransactionType06 = copy.TransactionType06;
            TransactionType07 = copy.TransactionType07;
            TransactionType08 = copy.TransactionType08;
            TransactionType09 = copy.TransactionType09;
            TransactionType10 = copy.TransactionType10;
            TransactionType11 = copy.TransactionType11;
            TransactionType12 = copy.TransactionType12;
            TransactionType13 = copy.TransactionType13;
            TransactionType14 = copy.TransactionType14;
            TransactionType15 = copy.TransactionType15;
        }

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

        [Range(1, 2)]
        public int ApprenticeshipContractType { get; set; }

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

        [NotMapped]
        public int DeliveryMonth { get; set; }

        [NotMapped]
        public int DeliveryYear { get; set; }

        public bool UseLevyBalance { get; set; }
    }
}