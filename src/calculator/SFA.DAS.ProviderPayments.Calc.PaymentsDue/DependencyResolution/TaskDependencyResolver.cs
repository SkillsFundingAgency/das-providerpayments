using System;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using StructureMap;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.DependencyResolution
{
    public class TaskDependencyResolver : IDependencyResolver
    {
        private IContainer _container;

        public void Init(Type taskType, ContextWrapper contextWrapper)
        {
            _container = new Container(c =>
                {
                    c.Policies.Add(new DcfsConnectionStringPolicy(contextWrapper));
                    c.AddRegistry(new PaymentsDueRegistry(taskType, contextWrapper));
                }
            );
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }
    }
}
