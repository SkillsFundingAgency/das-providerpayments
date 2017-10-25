using System;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution
{
    public interface IDependencyResolver
    {
        void Init(Type taskType, ContextWrapper contextWrapper);

        T GetInstance<T>();
    }
}