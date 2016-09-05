using System;
using StructureMap;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.DependencyResolution
{
    public class TaskDependencyResolver : IDependencyResolver
    {
        private IContainer _container;

        public void Init(Type taskType)
        {
            _container = new Container(c =>
                {
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
