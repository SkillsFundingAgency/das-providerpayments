using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto
{
    public class PaymentsDueResult
    {
        public PaymentsDueResult(
            List<RequiredPaymentEntity> payableEarnings, 
            List<NonPayableEarningEntity> nonPayableEarnings)
        {
            PayableEarnings = payableEarnings;
            NonPayableEarnings = nonPayableEarnings;
        }

        public IReadOnlyList<RequiredPaymentEntity> PayableEarnings { get;  }
        public IReadOnlyList<NonPayableEarningEntity> NonPayableEarnings { get; }
    }
}