using System;
using SFA.DAS.Payments.DCFS.Context;
using StructureMap;

namespace ExampleDCFSTask.Infrastructure.DependencyResolution
{
    public class TaskDependencyResolver : IDependencyResolver
    {
        private IContainer _container;

        public void Init(Type taskType, ContextWrapper contextWrapper)
        {
            _container = new Container(c =>
                {
                    // Add any policies you want here

                    c.AddRegistry(new ExampleRegistry(taskType));
                });
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }
    }
}
