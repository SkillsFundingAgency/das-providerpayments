using System;
using SFA.DAS.ProviderPayments.Calculator.Common.Context;

namespace SFA.DAS.ProviderPayments.Calculator.PaymentSchedule.DependencyResolution
{
    public interface IDependencyResolver
    {
        void Init(Type taskType, ContextWrapper contextWrapper);

        T GetInstance<T>();
    }
}
