using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface ILearner
    {
        List<RequiredPaymentEntity> CalculatePaymentsDue();
    }
}