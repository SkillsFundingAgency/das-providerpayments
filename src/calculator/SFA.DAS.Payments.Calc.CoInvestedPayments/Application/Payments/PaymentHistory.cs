using SFA.DAS.Payments.DCFS.Domain;
using System;


namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments
{
    public class PaymentHistory
    {
        public Guid RequiredPaymentId { get; set; }
        public FundingSource FundingSource { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }

        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }
        public long Uln { get; set; }
        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }
    }
}
