﻿using System;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments
{
    public class Payment
    {
        public Guid RequiredPaymentId { get; set; }

        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public string CollectionPeriodName { get; set; }
        public int CollectionPeriodMonth { get; set; }
        public int CollectionPeriodYear { get; set; }

        public FundingSource FundingSource { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}
