using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface ICalculatePaymentsDue
    {
        List<RequiredPayment> Calculate(
            List<FundingDue> earnings,
            List<int> periodsToIgnore,
            List<RequiredPayment> pastPayments);
    }
}