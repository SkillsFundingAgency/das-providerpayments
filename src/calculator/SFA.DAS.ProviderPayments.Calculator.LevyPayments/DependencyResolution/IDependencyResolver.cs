using System;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.DependencyResolution
{
    public interface IDependencyResolver
    {
        void Init(Type taskType);

        T GetInstance<T>();
    }
}
