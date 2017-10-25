using System;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Payments.DCFS.StructureMap.Infrastructure.DependencyResolution
{
    public abstract class StructureMapDependencyResolver : IDependencyResolver 
    {
        protected Container Container { get; private set; }

        public virtual void Init(Type taskType, ContextWrapper contextWrapper)
        {
            Container = new Container(c =>
            {
                AddPolicies(c, taskType, contextWrapper);
                AddRegistry(c, taskType, contextWrapper);
            });
        }
        public virtual T GetInstance<T>()
        {
            return Container.GetInstance<T>();
        }


        protected virtual void AddRegistry(ConfigurationExpression config, Type taskType, ContextWrapper contextWrapper)
        {
            config.AddRegistry(CreateRegistry(taskType, contextWrapper));
        }
        protected abstract Registry CreateRegistry(Type taskType, ContextWrapper contextWrapper);

        protected virtual void AddPolicies(ConfigurationExpression config, Type taskType, ContextWrapper contextWrapper)
        {
            config.Policies.Add(new DcfsConnectionStringPolicy(contextWrapper));
        }
    }
}
