﻿using System;
using SFA.DAS.ProviderPayments.Calc.Common.Application;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments
{
    public class RequiredPayment
    {
        public long? CommitmentId { get; set; }
        public string CommitmentVersionId { get; set; }
        public string LearnerRefNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal AmountDue { get; set; }
        public string AccountId { get; set; }
        public string AccountVersionId { get; set; }
        public long Uln { get; set; }
        public DateTime IlrSubmissionDateTime { get; set; }

        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }

        public int ApprenticeshipContractType { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
    }
}