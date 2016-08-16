using System;

namespace SFA.DAS.ProviderPayments.Calculator.Domain.DependencyResolution
{
    public interface IDependencyResolver
    {
        void Init(Type taskType);

        T GetInstance<T>();
    }
}
