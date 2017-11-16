using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts
{
    public class Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Commitment[] Commitments { get; set; }

        public IEnumerable<PaymentDue> Refunds { get; set; }
        public IEnumerable<PaymentDue> Payments { get; set; }
    }
}
