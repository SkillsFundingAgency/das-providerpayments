using System;
using SFA.DAS.Payments.DCFS.Context;

namespace ExampleDCFSTask.Infrastructure.DependencyResolution
{
    public interface IDependencyResolver
    {
        void Init(Type taskType, ContextWrapper contextWrapper);

        T GetInstance<T>();
    }
}
