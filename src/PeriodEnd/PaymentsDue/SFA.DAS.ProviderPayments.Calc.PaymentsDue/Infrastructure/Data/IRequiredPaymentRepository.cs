using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data
{
    public interface IRequiredPaymentRepository
    {
        void AddMany(List<RequiredPayment> payments);
    }
}