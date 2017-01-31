﻿using System;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.PaymentsDue
{
    public class PaymentDue
    {
        public Guid Id { get; set; }
        public long Ukprn { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal AmountDue { get; set; }
        public decimal SfaContributionPercentage { get; set; }
    }
}