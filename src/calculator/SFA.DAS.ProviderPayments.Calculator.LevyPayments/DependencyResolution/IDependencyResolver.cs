using System;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Context;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.DependencyResolution
{
    public interface IDependencyResolver
    {
        void Init(Type taskType, ContextWrapper contextWrapper);

        T GetInstance<T>();
    }
}
