using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts
{
    public class Commitment
    {
        public long Id { get; set; }

        public IEnumerable<PaymentDue> Refunds { get; set; }

        public IEnumerable<PaymentDue> Payments { get; set; }
    }
}
