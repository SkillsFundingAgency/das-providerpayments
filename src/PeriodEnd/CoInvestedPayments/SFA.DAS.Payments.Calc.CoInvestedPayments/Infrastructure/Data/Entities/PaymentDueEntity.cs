﻿using System;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities
{
    public class PaymentDueEntity
    {
        public Guid Id { get; set; }
        public long Ukprn { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public int TransactionType { get; set; }
        public decimal AmountDue { get; set; }
        public decimal SfaContributionPercentage { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Uln { get; set; }
        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }

        public string LearnRefNumber { get; set; }
    }
}