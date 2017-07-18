﻿using System;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities
{
    public class RequiredPaymentEntity
    {
        public string Id { get; set; }
        public long CommitmentId { get; set; }
        public string CommitmentVersionId { get; set; }
        public string AccountId { get; set; }
        public string AccountVersionId { get; set; }
        public long Uln { get; set; }
        public string LearnRefNumber { get; set; }
        public int AimSeqNumber { get; set; }
        public long Ukprn { get; set; }
        public DateTime IlrSubmissionDateTime { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public long StandardCode { get; set; }
        public int ProgrammeType { get; set; }
        public int FrameworkCode { get; set; }
        public int PathwayCode { get; set; }
        public int ApprenticeshipContractType { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public string CollectionPeriodName { get; set; }
        public int CollectionPeriodMonth { get; set; }
        public int CollectionPeriodYear { get; set; }
        public int TransactionType { get; set; }
        public decimal AmountDue { get; set; }
        public decimal SfaContributionPercentage { get; set; }
        public string FundingLineType { get; set; }
        public bool UseLevyBalance { get; set; }
    }
}
