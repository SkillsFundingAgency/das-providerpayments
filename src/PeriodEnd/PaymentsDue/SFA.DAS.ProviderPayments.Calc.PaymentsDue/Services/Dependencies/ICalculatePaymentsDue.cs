using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface ICalculatePaymentsDue
    {
        List<RequiredPayment> Calculate(
            List<FundingDue> earnings,
            HashSet<int> periodsToIgnore,
            List<RequiredPayment> pastPayments);
    }
}