using NLog;
using SFA.DAS.ProviderPayments.Calculator.Domain.DependencyResolution;
using StructureMap;
using System;

namespace SFA.DAS.ProviderPayments.Calculator.Infrastructure.DependencyResolution
{
    public class TaskDependencyResolver : IDependencyResolver
    {
        private IContainer _container;

        public void Init(Type taskType)
        {
            _container = new Container(c =>
            {
                c.For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));
            }
            );
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }
    }
}
