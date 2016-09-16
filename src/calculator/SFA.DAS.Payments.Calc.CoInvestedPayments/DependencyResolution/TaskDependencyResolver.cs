using System;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using StructureMap;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.DependencyResolution
{
    public class TaskDependencyResolver : IDependencyResolver
    {
        private IContainer _container;

        public void Init(Type taskType, ContextWrapper contextWrapper)
        {
            _container = new Container(c =>
                {
                    c.AddRegistry(new CoInvestedPaymentsRegistry(taskType));
                }
            );
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }
    }
}
