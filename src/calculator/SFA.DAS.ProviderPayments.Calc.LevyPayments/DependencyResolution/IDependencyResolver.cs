using System;
using SFA.DAS.ProviderPayments.Calc.Common.Context;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.DependencyResolution
{
    public interface IDependencyResolver
    {
        void Init(Type taskType, ContextWrapper contextWrapper);

        T GetInstance<T>();
    }
}
