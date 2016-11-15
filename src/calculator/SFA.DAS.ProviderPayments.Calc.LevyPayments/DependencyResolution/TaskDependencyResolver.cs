using System;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using StructureMap;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.DependencyResolution
{
    public class TaskDependencyResolver : IDependencyResolver
    {
        private IContainer _container;

        public void Init(Type taskType, ContextWrapper contextWrapper)
        {
            _container = new Container(c =>
                {
                    c.Policies.Add(new DcfsConnectionStringPolicy(contextWrapper));
                    c.Policies.Add(new YearOfCollectionPolicy(contextWrapper));
                    c.AddRegistry(new CalcRegistry(taskType));
                }
            );
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }
    }
}
