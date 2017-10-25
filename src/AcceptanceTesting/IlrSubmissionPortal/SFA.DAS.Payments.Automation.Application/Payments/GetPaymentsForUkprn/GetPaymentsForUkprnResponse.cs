using SFA.DAS.Payments.Automation.Infrastructure.PaymentResults;
using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.Application.Payments.GetPaymentsForUkprn
{
    public class GetPaymentsForUkprnResponse : ApplicationResponse
    {
        public List<PaymentResult> Payments { get; set; }
    }
}