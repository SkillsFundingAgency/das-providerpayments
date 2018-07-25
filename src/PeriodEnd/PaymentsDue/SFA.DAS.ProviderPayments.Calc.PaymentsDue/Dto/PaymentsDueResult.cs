using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto
{
    public class PaymentsDueResult
    {
        public PaymentsDueResult(
            List<RequiredPayment> payableEarnings, 
            List<NonPayableEarning> nonPayableEarnings)
        {
            PayableEarnings = payableEarnings;
            NonPayableEarnings = nonPayableEarnings;
        }

        public IReadOnlyList<RequiredPayment> PayableEarnings { get;  }
        public IReadOnlyList<NonPayableEarning> NonPayableEarnings { get; }
    }
}