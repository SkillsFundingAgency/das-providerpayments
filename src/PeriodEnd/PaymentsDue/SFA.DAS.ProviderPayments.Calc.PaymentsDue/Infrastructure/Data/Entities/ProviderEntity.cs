using System;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class ProviderEntity
    {
        public long Ukprn { get; set; }
        public DateTime IlrSubmissionDateTime { get; set; }
    }
}