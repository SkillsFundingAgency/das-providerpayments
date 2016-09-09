using System;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Context;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.DependencyResolution
{
    public interface IDependencyResolver
    {
        void Init(Type taskType, ContextWrapper contextWrapper);

        T GetInstance<T>();
    }
}
