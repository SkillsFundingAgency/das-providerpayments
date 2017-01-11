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
    }
}