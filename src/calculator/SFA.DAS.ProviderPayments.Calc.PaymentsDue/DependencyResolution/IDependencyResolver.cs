using System;
using SFA.DAS.ProviderPayments.Calc.Common.Context;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.DependencyResolution
{
    public interface IDependencyResolver
    {
        void Init(Type taskType, ContextWrapper contextWrapper);

        T GetInstance<T>();
    }
}
